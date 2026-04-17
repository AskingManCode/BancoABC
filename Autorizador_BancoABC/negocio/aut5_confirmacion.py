from typing import Dict, Any
from decimal import Decimal, InvalidOperation

from datos.repositorio_tarjetas import RepositorioTarjetas
from datos.repositorio_cajeros import RepositorioCajeros
from datos.repositorio_transacciones import RepositorioTransacciones
from integraciones.cliente_core import ClienteCore
from bitacora.bitacora_worker import BitacoraWorker

from negocio.validaciones import iguales_por_texto_descifrado, fechas_iguales
from negocio.almacen_autorizaciones import almacen_autorizaciones
from negocio.normalizacion import normalizar_tipo_transaccion
from seguridad.cifrado_aes import descifrar, enmascarar_tarjeta_core


def _campo_obligatorio(req: Dict[str, Any], key: str) -> bool:
    return key in req and str(req.get(key, "")).strip() != ""


def _parse_monto(monto_txt: Any) -> Decimal:
    try:
        limpio = str(monto_txt).replace(",", "").strip()
        return Decimal(limpio)
    except (InvalidOperation, ValueError):
        return Decimal("-1")


def procesar_aut5_confirmacion(
    request: Dict[str, Any],
    repo_tarjetas: RepositorioTarjetas,
    repo_cajeros: RepositorioCajeros,
    repo_trans: RepositorioTransacciones,
    cliente_core: ClienteCore,
    bitacora: BitacoraWorker,
    habilitar_core: bool = False
) -> Dict[str, Any]:
    """AUT5: Confirmación
    - Respuesta SIEMPRE: status = OK o "1".."5".
    - En confirmación NO se envía PIN.
    - Debe validar el CodigoAutorizacion / CodigoDeAutorizacion generado en AUT1.
    """

    def _bitacora(cajero: str, tarjeta: str, cliente: str, monto_txt: str, autorizacion: str) -> None:
        bitacora.encolar({
            "tipo": "Confirmacion",
            "cajero": cajero,
            "tarjeta": tarjeta,
            "cliente": cliente,
            "monto": monto_txt,
            "autorizacion": autorizacion
        })

    # Obligatorios PantallaRetiros.cs (ojo: el código se valida aparte para aceptar 2 llaves)
    obligatorios = [
        "NumeroDeTarjeta",
        "FechaDeVencimiento",
        "CodigoDeVerificacion",
        "IdentificadorDelCajero",
        "TipoDeTransaccion",
        "MontoRetiro",
    ]
    for k in obligatorios:
        if not _campo_obligatorio(request, k):
            _bitacora(
                str(request.get("IdentificadorDelCajero", "")),
                str(request.get("NumeroDeTarjeta", "")),
                "",
                "0.00",
                str((request.get("CodigoAutorizacion") or request.get("CodigoDeAutorizacion") or ""))
            )
            return {"status": "2"}

    # Validación especial del código: acepta CodigoAutorizacion o CodigoDeAutorizacion
    if not _campo_obligatorio(request, "CodigoAutorizacion") and not _campo_obligatorio(request, "CodigoDeAutorizacion"):
        _bitacora(
            str(request.get("IdentificadorDelCajero", "")),
            str(request.get("NumeroDeTarjeta", "")),
            "",
            "0.00",
            str((request.get("CodigoAutorizacion") or request.get("CodigoDeAutorizacion") or ""))
        )
        return {"status": "2"}

    if normalizar_tipo_transaccion(str(request.get("TipoDeTransaccion", ""))) != "Confirmacion":
        _bitacora(
            str(request.get("IdentificadorDelCajero", "")),
            str(request.get("NumeroDeTarjeta", "")),
            "",
            "0.00",
            str((request.get("CodigoAutorizacion") or request.get("CodigoDeAutorizacion") or ""))
        )
        return {"status": "2"}

    cajero_id = str(request["IdentificadorDelCajero"]).strip()
    codigo_req = str((request.get("CodigoAutorizacion") or request.get("CodigoDeAutorizacion") or "")).strip()

    cajero = repo_cajeros.obtener_cajero_activo_por_identificador(cajero_id)
    if not cajero:
        _bitacora(cajero_id, str(request.get("NumeroDeTarjeta", "")), "", "0.00", codigo_req)
        return {"status": "2"}

    numero_cif = str(request["NumeroDeTarjeta"]).strip()
    tarjeta = repo_tarjetas.obtener_detalle_tarjeta_por_numero(numero_cif)
    if not tarjeta:
        _bitacora(cajero_id, numero_cif, "", "0.00", codigo_req)
        return {"status": "2"}

    cliente_id = str(tarjeta.get("CUENTA_ID", "")).strip()

    if int(tarjeta.get("TARJETA_Estado", 0)) != 1 or int(tarjeta.get("CUENTA_Estado", 1)) != 1:
        _bitacora(cajero_id, numero_cif, cliente_id, "0.00", codigo_req)
        return {"status": "3"}

    # CVV (cifrado en BD)
    if not iguales_por_texto_descifrado(
        str(request["CodigoDeVerificacion"]).strip(),
        str(tarjeta.get("TARJETA_NumVerificacion", "")).strip()
    ):
        _bitacora(cajero_id, numero_cif, cliente_id, "0.00", codigo_req)
        return {"status": "2"}

    # Fecha (cifrada en BD) -> descifrar antes de comparar para evitar fallos de formato/cifrado
    fecha_req = str(request["FechaDeVencimiento"]).strip()
    fecha_bd_cif = str(tarjeta.get("TARJETA_FechaVencimiento", "")).strip()
    try:
        fecha_bd = descifrar(fecha_bd_cif).strip()
    except Exception:
        fecha_bd = fecha_bd_cif

    if not fechas_iguales(fecha_req, fecha_bd):
        # Datos incorrectos (fecha no coincide con la registrada)
        _bitacora(cajero_id, numero_cif, cliente_id, "0.00", codigo_req)
        return {"status": "2"}

    # Validar vencimiento contra hoy (tarjeta vencida -> "4")
    from negocio.validaciones import tarjeta_esta_vencida
    vencida = tarjeta_esta_vencida(fecha_bd)
    if vencida is None:
        _bitacora(cajero_id, numero_cif, cliente_id, "0.00", codigo_req)
        return {"status": "2"}
    if vencida:
        _bitacora(cajero_id, numero_cif, cliente_id, "0.00", codigo_req)
        return {"status": "4"}

    monto = _parse_monto(request["MontoRetiro"])
    if monto <= 0:
        _bitacora(cajero_id, numero_cif, cliente_id, "0.00", codigo_req)
        return {"status": "2"}

    monto_txt = f"{monto:.2f}"
    codigo = codigo_req

    # Validar código AUT1 (HU 6.1)
    ok_codigo, _ = almacen_autorizaciones.validar_y_consumir(
        codigo=codigo,
        tarjeta_numero=numero_cif,
        cajero_id=cajero_id,
        monto=monto
    )
    if not ok_codigo:
        _bitacora(cajero_id, numero_cif, cliente_id, monto_txt, codigo)
        return {"status": "2"}

    tipo_id = int(tarjeta.get("TARJETA_TIPO_TARJ_ID", 0))

    # 2 = Crédito (MySQL) -> SP (adelanto efectivo)
    if tipo_id == 2:
        _bitacora(cajero_id, numero_cif, cliente_id, monto_txt, codigo)
        return {"status": "OK"}

    # 1 = Débito (Core)
    if tipo_id == 1:
        if not habilitar_core:
            _bitacora(cajero_id, numero_cif, cliente_id, monto_txt, codigo)
            return {"status": "5"}

        numero_cuenta = str(tarjeta.get("CUENTA_ID", "")).strip()
        if not numero_cuenta:
            _bitacora(cajero_id, numero_cif, cliente_id, monto_txt, codigo)
            return {"status": "5"}

        # El Core compara contra número ENMASCARADO (6 + ****** + 4)
        numero_tarjeta_core = enmascarar_tarjeta_core(numero_cif)

        resp_core = cliente_core.confirmar_retiro_debito(
            numero_cuenta=numero_cuenta,
            numero_tarjeta=numero_tarjeta_core,
            monto=float(monto),
            codigo_autorizacion=codigo,
        )

        _bitacora(cajero_id, numero_cif, cliente_id, monto_txt, codigo)

        if resp_core == "OK":
            return {"status": "OK"}
        if resp_core == "INSUF":
            return {"status": "1"}
        if resp_core == "INVALID":
            return {"status": "2"}
        return {"status": "5"}

    _bitacora(cajero_id, numero_cif, cliente_id, monto_txt, codigo)
    return {"status": "5"}
