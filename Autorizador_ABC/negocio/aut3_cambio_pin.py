from typing import Dict, Any

from datos.repositorio_tarjetas import RepositorioTarjetas
from datos.repositorio_cajeros import RepositorioCajeros
from bitacora.bitacora_worker import BitacoraWorker
from negocio.validaciones import iguales_por_texto_descifrado, fechas_iguales
from negocio.normalizacion import normalizar_tipo_transaccion
from seguridad.cifrado_aes import descifrar


def _try_descifrar(valor: str) -> str:
    v = (valor or "").strip()
    if not v:
        return ""
    try:
        return descifrar(v).strip()
    except Exception:
        return v


def _campo_obligatorio(req: Dict[str, Any], key: str) -> bool:
    return key in req and str(req.get(key, "")).strip() != ""


def procesar_aut3_cambio_pin(
    request: Dict[str, Any],
    repo_tarjetas: RepositorioTarjetas,
    repo_cajeros: RepositorioCajeros,
    bitacora: BitacoraWorker
) -> Dict[str, Any]:
    """
    AUT3: Cambio de PIN
    - Valida obligatorios
    - Valida cajero activo
    - Valida tarjeta activa + CVV/Venc (cifrados) antes de ejecutar SP
    - Llama SP_AUT3_CAMBIOPIN(cajero, tarjeta, pin_actual, pin_nuevo, OUT)
    - Responde OK/ERROR (como C#)
    """

    obligatorios = [
        "NumeroDeTarjeta",
        "PinActual",
        "PinNuevo",
        "FechaDeVencimiento",
        "CodigoDeVerificacion",
        "IdentificadorDelCajero",
        "TipoDeTransaccion",
    ]
    def _bitacora(cajero: str, tarjeta: str, cliente: str) -> None:
        bitacora.encolar({
            "tipo": "Cambio de PIN",
            "cajero": cajero,
            "tarjeta": tarjeta,
            "cliente": cliente,
            "monto": "0.00"
        })

    for k in obligatorios:
        if not _campo_obligatorio(request, k):
            _bitacora(str(request.get("IdentificadorDelCajero", "")), str(request.get("NumeroDeTarjeta", "")), "")
            return {"status": "2"}

    if normalizar_tipo_transaccion(str(request.get("TipoDeTransaccion", ""))) != "CambioPIN":
        _bitacora(str(request.get("IdentificadorDelCajero", "")), str(request.get("NumeroDeTarjeta", "")), "")
        return {"status": "2"}

    cajero_id = str(request["IdentificadorDelCajero"]).strip()
    cajero = repo_cajeros.obtener_cajero_activo_por_identificador(cajero_id)
    if not cajero:
        _bitacora(cajero_id, str(request.get("NumeroDeTarjeta", "")), "")
        return {"status": "2"}

    numero_cif = str(request["NumeroDeTarjeta"]).strip()
    tarjeta = repo_tarjetas.obtener_detalle_tarjeta_por_numero(numero_cif)
    if not tarjeta:
        _bitacora(cajero_id, numero_cif, "")
        return {"status": "2"}

    cliente_id = str(tarjeta.get("CUENTA_ID", "")).strip()

    if int(tarjeta.get("TARJETA_Estado", 0)) != 1 or int(tarjeta.get("CUENTA_Estado", 1)) != 1:
        _bitacora(cajero_id, numero_cif, cliente_id)
        return {"status": "3"}

    # CVV (cifrado)
    if not iguales_por_texto_descifrado(
        str(request["CodigoDeVerificacion"]).strip(),
        str(tarjeta.get("TARJETA_NumVerificacion", "")).strip()
    ):
        _bitacora(cajero_id, numero_cif, cliente_id)
        return {"status": "2"}

    # Fecha (cifrada)
    if not fechas_iguales(
        str(request["FechaDeVencimiento"]).strip(),
        str(tarjeta.get("TARJETA_FechaVencimiento", "")).strip()
    ):
        _bitacora(cajero_id, numero_cif, cliente_id)
        return {"status": "4"}

    # Validaciones del PIN nuevo (HU 4.2)
    pin_actual_txt = _try_descifrar(str(request.get("PinActual", "")))
    pin_nuevo_txt = _try_descifrar(str(request.get("PinNuevo", "")))

    if (not pin_nuevo_txt.isdigit()) or len(pin_nuevo_txt) != 4:
        _bitacora(cajero_id, numero_cif, cliente_id)
        return {"status": "2"}

    if pin_actual_txt == pin_nuevo_txt:
        _bitacora(cajero_id, numero_cif, cliente_id)
        return {"status": "2"}

    # El SP se encarga de validar pin actual y aplicar el nuevo
    resp_sp = repo_tarjetas.ejecutar_sp_aut3_cambio_pin(
        cod_cajero=cajero_id,
        num_tarjeta=numero_cif,
        pin_actual=str(request["PinActual"]).strip(),
        pin_nuevo=str(request["PinNuevo"]).strip(),
    )

    if resp_sp == 0:
        _bitacora(cajero_id, numero_cif, cliente_id)
        return {"status": "OK"}

    _bitacora(cajero_id, numero_cif, cliente_id)
    return {"status": str(resp_sp) if str(resp_sp) in ("2", "5") else "5"}
