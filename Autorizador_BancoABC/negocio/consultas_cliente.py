from typing import Dict, Any
from seguridad.cifrado_aes import descifrar

def _campo_obligatorio(req: Dict[str, Any], key: str) -> bool:
    return key in req and str(req.get(key, "")).strip() != ""

def procesar_obtener_cuentas(
    request: Dict[str, Any],
    cliente_core,
    **kwargs
) -> Dict[str, Any]:
    print("*** DENTRO DE OBTENER_CUENTAS ***")
    if not _campo_obligatorio(request, "Identificacion"):
        return {"status": "2"}
    try:
        identificacion = descifrar(request["Identificacion"])
    except:
        return {"status": "2"}
    if not identificacion:
        return {"status": "2"}
    cuentas = cliente_core.obtener_cuentas(identificacion)
    if cuentas is None:
        return {"status": "5"}
    return {"status": "OK", "cuentas": cuentas}

def procesar_obtener_tarjetas(
    request: Dict[str, Any],
    repo_tarjetas,
    **kwargs
) -> Dict[str, Any]:
    print("*** DENTRO DE OBTENER_TARJETAS ***")
    if not _campo_obligatorio(request, "Identificacion"):
        return {"status": "2"}
    try:
        identificacion = descifrar(request["Identificacion"])
    except:
        return {"status": "2"}
    if not identificacion:
        return {"status": "2"}
    tarjetas = repo_tarjetas.obtener_tarjetas_por_identificacion(identificacion)
    if tarjetas is None:
        return {"status": "5"}
    resultado = []
    for t in tarjetas:
        es_credito = (t.get("TIPO_TARJ_Nombre") == "Credito")
        resultado.append({
            "NumeroTarjeta": t.get("TARJETA_Numero"),
            "Tipo": t.get("TIPO_TARJ_Nombre"),
            "NumeroCuentaAsociada": t.get("CUENTA_ID") if not es_credito else None,
            "EsCredito": es_credito
        })
    return {"status": "OK", "tarjetas": resultado}

def procesar_obtener_movimientos_cuenta(
    request: Dict[str, Any],
    cliente_core,
    **kwargs
) -> Dict[str, Any]:
    print("*** DENTRO DE OBTENER_MOVIMIENTOS_CUENTA ***")
    if not _campo_obligatorio(request, "Identificacion") or not _campo_obligatorio(request, "NumeroCuenta"):
        return {"status": "2"}
    try:
        identificacion = descifrar(request["Identificacion"])
        numero_cuenta = descifrar(request["NumeroCuenta"])
    except:
        return {"status": "2"}
    if not identificacion or not numero_cuenta:
        return {"status": "2"}
    movimientos = cliente_core.obtener_movimientos_cuenta(identificacion, numero_cuenta)
    if movimientos is None:
        return {"status": "5"}
    return {"status": "OK", "movimientos": movimientos}

def procesar_obtener_movimientos_credito(
    request: Dict[str, Any],
    repo_tarjetas,
    **kwargs
) -> Dict[str, Any]:
    print("*** DENTRO DE OBTENER_MOVIMIENTOS_CREDITO ***")
    if not _campo_obligatorio(request, "Identificacion") or not _campo_obligatorio(request, "NumeroTarjeta"):
        return {"status": "2"}
    try:
        identificacion = descifrar(request["Identificacion"])
        numero_tarjeta = descifrar(request["NumeroTarjeta"])
    except:
        return {"status": "2"}
    if not identificacion or not numero_tarjeta:
        return {"status": "2"}
    movimientos = repo_tarjetas.obtener_movimientos_credito(identificacion, numero_tarjeta)
    if movimientos is None:
        return {"status": "5"}
    resultado = []
    for m in movimientos:
        resultado.append({
            "Fecha": m.get("MOV_Fecha").isoformat() if hasattr(m.get("MOV_Fecha"), 'isoformat') else str(m.get("MOV_Fecha")),
            "CodigoAutorizacion": m.get("MOV_CodigoAutorizacion"),
            "Comercio": m.get("MOV_Comercio"),
            "Monto": float(m.get("MOV_Monto"))
        })
    return {"status": "OK", "movimientos": resultado}