from __future__ import annotations
from typing import Optional
from seguridad.cifrado_aes import descifrar


def _try_descifrar(valor: str) -> str:
    v = (valor or "").strip()
    if not v:
        return ""
    try:
        return descifrar(v).strip()
    except Exception:
        return v


def _limpiar_basico(s: str) -> str:
    # Quita acentos comunes y espacios para comparar
    t = (s or "").strip().upper()
    # Reemplazos de acentos (suficiente para este proyecto)
    t = (t.replace("Á", "A").replace("É", "E").replace("Í", "I").replace("Ó", "O").replace("Ú", "U")
           .replace("Ü", "U").replace("Ñ", "N"))
    # quita espacios y guiones
    t = t.replace(" ", "").replace("-", "")
    return t


def normalizar_tipo_transaccion(valor: str) -> str:
    """Devuelve un nombre canónico: Retiro, Consulta, CambioPIN, Confirmacion o ""."""
    v = _try_descifrar(valor)
    k = _limpiar_basico(v)

    if k == "RETIRO":
        return "Retiro"
    if k == "CONSULTA":
        return "Consulta"
    if k in ("CAMBIOPIN", "CAMBIODEPIN"):
        return "CambioPIN"
    if k == "CONFIRMACION":
        return "Confirmacion"
    return ""
