from typing import Dict, Any
from decimal import Decimal, InvalidOperation
import secrets

from datos.repositorio_tarjetas import RepositorioTarjetas
from datos.repositorio_cajeros import RepositorioCajeros
from integraciones.cliente_core import ClienteCore
from bitacora.bitacora_worker import BitacoraWorker

from negocio.validaciones import iguales_por_texto_descifrado, fechas_iguales
from negocio.almacen_autorizaciones import almacen_autorizaciones
from negocio.normalizacion import normalizar_tipo_transaccion
from seguridad.cifrado_aes import descifrar


def _campo_obligatorio(req: Dict[str, Any], key: str) -> bool:
    return key in req and str(req.get(key, "")).strip() != ""


def _parse_monto(monto_txt: str) -> Decimal:
    try:
        limpio = str(monto_txt).replace(",", "").strip()
        return Decimal(limpio)
    except (InvalidOperation, ValueError):
        return Decimal("-1")


def _generar_codigo_autorizacion_8() -> str:
    return "".join(str(secrets.randbelow(10)) for _ in range(8))


def procesar_aut1_retiro(
    request: Dict[str, Any],
    repo_tarjetas: RepositorioTarjetas,
    repo_cajeros: RepositorioCajeros,
    cliente_core: ClienteCore,
    bitacora: BitacoraWorker,
    habilitar_core: bool = False
) -> Dict[str, Any]:
    """
    AUT1: Retiro
    - IMPORTANTE: C# siempre intenta leer "Autorizacion", así que SIEMPRE lo devolvemos.
    - Con MySQL, la operación real de crédito la ejecuta SP_AUT5 en confirmación.
    - Débito: usa Core (si habilitar_core=True) para validar fondos y autorizar.
    """

    def _bitacora(tipo: str, cajero: str, tarjeta: str, cliente: str, monto_txt: str, autorizacion: str = "") -> None:
        bitacora.encolar({
            "tipo": tipo,
            "cajero": cajero,
            "tarjeta": tarjeta,
            "cliente": cliente,
            "monto": monto_txt,
            "autorizacion": autorizacion
        })

    def resp(status: str, codigo: str = "") -> Dict[str, Any]:
        # El simulador solo usa Autorizacion cuando status == OK.
        if status == "OK":
            return {"status": status, "Autorizacion": codigo}
        return {"status": status}

    obligatorios = [
        "NumeroDeTarjeta",
        "Pin",
        "FechaDeVencimiento",
        "CodigoDeVerificacion",
        "IdentificadorDelCajero",
        "TipoDeTransaccion",
        "MontoRetiro"
    ]
    for k in obligatorios:
        if not _campo_obligatorio(request, k):
            _bitacora("Retiro", str(request.get("IdentificadorDelCajero", "")), str(request.get("NumeroDeTarjeta", "")), "", "0.00", "")
            return resp("2")

    if normalizar_tipo_transaccion(str(request.get("TipoDeTransaccion", ""))) != "Retiro":
        _bitacora("Retiro", str(request.get("IdentificadorDelCajero", "")), str(request.get("NumeroDeTarjeta", "")), "", "0.00", "")
        return resp("2")

    cajero_id = str(request["IdentificadorDelCajero"]).strip()
    cajero = repo_cajeros.obtener_cajero_activo_por_identificador(cajero_id)
    if not cajero:
        _bitacora("Retiro", cajero_id, str(request.get("NumeroDeTarjeta", "")), "", "0.00", "")
        return resp("2")

    numero_cif = str(request["NumeroDeTarjeta"]).strip()
    tarjeta = repo_tarjetas.obtener_detalle_tarjeta_por_numero(numero_cif) 
    if not tarjeta:
        _bitacora("Retiro", cajero_id, numero_cif, "", "0.00", "")
        return resp("2")

    cliente_id = str(tarjeta.get("CUENTA_ID", "")).strip()

    if int(tarjeta.get("TARJETA_Estado", 0)) != 1 or int(tarjeta.get("CUENTA_Estado", 0)) != 1:
        _bitacora("Retiro", cajero_id, numero_cif, cliente_id, "0.00", "")
        return resp("3")

    # PIN (cifrado)
    if not iguales_por_texto_descifrado(
        str(request["Pin"]).strip(),
        str(tarjeta.get("TARJETA_PIN", "")).strip()
    ):
        _bitacora("Retiro", cajero_id, numero_cif, cliente_id, "0.00", "")
        return resp("2")

    # CVV (cifrado)
    if not iguales_por_texto_descifrado(
        str(request["CodigoDeVerificacion"]).strip(),
        str(tarjeta.get("TARJETA_NumVerificacion", "")).strip()
    ):
        _bitacora("Retiro", cajero_id, numero_cif, cliente_id, "0.00", "")
        return resp("2")

    # Fecha (cifrada)
    # 1) Validar que la fecha ingresada coincide con la registrada (dato incorrecto -> "2")
    if not fechas_iguales(
        str(request["FechaDeVencimiento"]).strip(),
        str(tarjeta.get("TARJETA_FechaVencimiento", "")).strip()
    ):
        _bitacora("Retiro", cajero_id, numero_cif, cliente_id, "0.00", "")
        return resp("2")

    # 2) Validar vencimiento contra hoy (tarjeta vencida -> "4")
    from negocio.validaciones import tarjeta_esta_vencida
    vencida = tarjeta_esta_vencida(str(tarjeta.get("TARJETA_FechaVencimiento", "")).strip())
    if vencida is None:
        _bitacora("Retiro", cajero_id, numero_cif, cliente_id, "0.00", "")
        return resp("2")
    if vencida:
        _bitacora("Retiro", cajero_id, numero_cif, cliente_id, "0.00", "")
        return resp("4")

    monto = _parse_monto(request["MontoRetiro"])
    if monto <= 0:
        _bitacora("Retiro", cajero_id, numero_cif, cliente_id, "0.00", "")
        return resp("2")

    monto_txt = f"{monto:.2f}"

    tipo_id = int(tarjeta.get("TARJETA_TIPO_TARJ_ID", 0))

    # 1 = Débito -> Core
    if tipo_id == 1:
        if not habilitar_core:
            _bitacora("Retiro", cajero_id, numero_cif, cliente_id, monto_txt, "")
            return resp("5")  

        numero_cuenta = str(tarjeta.get("CUENTA_ID", "")).strip()
        if not numero_cuenta:
            _bitacora("Retiro", cajero_id, numero_cif, cliente_id, monto_txt, "")
            return resp("5")
        # El Core compara contra número ENMASCARADO (6 + ****** + 4)
        from seguridad.cifrado_aes import enmascarar_tarjeta_core
        numero_tarjeta_core = enmascarar_tarjeta_core(numero_cif)

        ok = cliente_core.verificar_fondos_debito(
            numero_cuenta=numero_cuenta,
            numero_tarjeta=numero_tarjeta_core,
            monto=float(monto)
        )
        if not ok:
            _bitacora("Retiro", cajero_id, numero_cif, cliente_id, monto_txt, "")
            return resp("1")

        # Código único por día
        codigo = _generar_codigo_autorizacion_8()
        while almacen_autorizaciones.existe_codigo_hoy(codigo):
            codigo = _generar_codigo_autorizacion_8()
        # Guardamos el código en memoria para validarlo en AUT5
        almacen_autorizaciones.guardar(codigo=codigo, tarjeta_numero=numero_cif, cajero_id=cajero_id, monto=monto)

        _bitacora("Retiro", cajero_id, numero_cif, cliente_id, monto_txt, codigo)

        return resp("OK", codigo)

    # 2 = Crédito -> valida saldo de adelanto en MySQL y genera código
    if tipo_id == 2:
        adelanto = tarjeta.get("CUENTA_MontoAdelantadoEfectivo", None)
        if adelanto is None:
            _bitacora("Retiro", cajero_id, numero_cif, cliente_id, monto_txt, "")
            return resp("1")

        try:
            adelanto_dec = Decimal(str(adelanto))
        except Exception:
            _bitacora("Retiro", cajero_id, numero_cif, cliente_id, monto_txt, "")
            return resp("5")

        if monto > adelanto_dec:
            _bitacora("Retiro", cajero_id, numero_cif, cliente_id, monto_txt, "")
            return resp("1")

        codigo = _generar_codigo_autorizacion_8()
        while almacen_autorizaciones.existe_codigo_hoy(codigo):
            codigo = _generar_codigo_autorizacion_8()
        # Guardamos el código en memoria para validarlo en AUT5
        almacen_autorizaciones.guardar(codigo=codigo, tarjeta_numero=numero_cif, cajero_id=cajero_id, monto=monto)

        _bitacora("Retiro", cajero_id, numero_cif, cliente_id, monto_txt, codigo)

        return resp("OK", codigo)

    return resp("5")
