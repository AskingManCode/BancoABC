from datetime import datetime
from typing import Optional, Tuple

from seguridad.cifrado_aes import descifrar


def _try_descifrar(valor: str) -> str:
    """Intenta descifrar un valor.

    Si falla (no cifrado / dato raro), devuelve el texto original limpio.
    """
    v = (valor or "").strip()
    if not v:
        return ""
    try:
        return descifrar(v).strip()
    except Exception:
        return v


def _parse_fecha_flexible(txt: str) -> Optional[datetime]:
    """Parsea fecha en varios formatos.

    OJO: incluye formatos de vencimiento (mes/año) para poder normalizar.
    """
    t = (txt or "").strip() 
    if not t: 
        return None 

    formatos = [
        # vencimiento común (mes/año)
        "%m/%y",
        "%m/%Y",
        "%m-%y",
        "%m-%Y",
        "%y/%m",
        "%Y/%m",
        "%Y-%m",
        # fechas completas
        "%Y-%m-%d",
        "%Y/%m/%d",
        "%d/%m/%Y",
        "%m/%d/%Y",
        "%d-%m-%Y",
        "%m-%d-%Y",
        "%Y-%m-%d %H:%M:%S",
        "%d/%m/%Y %H:%M:%S",
        "%m/%d/%Y %H:%M:%S",
    ]

    for f in formatos:
        try:
            return datetime.strptime(t, f)
        except Exception:
            continue

    return None


def _parse_vencimiento_flexible(txt: str) -> Optional[Tuple[int, int]]:
    """Parsea una fecha de vencimiento y devuelve (año, mes).

    Acepta: MM/YY, MM/YYYY, YYYY-MM, YYYY/MM y fechas completas (ignora el día).
    """
    t = (txt or "").strip()
    if not t:
        return None

    # 1) Intento con datetime (formatos comunes y fechas completas)
    dt = _parse_fecha_flexible(t)
    if dt:
        return (dt.year, dt.month)

    # 2) Fallback manual para formatos raros
    raw = t.replace(" ", "")

    # YYYYMM
    if raw.isdigit() and len(raw) == 6:
        y = int(raw[:4])
        m = int(raw[4:6])
        if 1 <= m <= 12:
            return (y, m)

    # MMYY
    if raw.isdigit() and len(raw) == 4:
        m = int(raw[:2])
        y2 = int(raw[2:4])
        y = 2000 + y2 if y2 < 70 else 1900 + y2
        if 1 <= m <= 12:
            return (y, m)

    # Separadores
    for sep in ("/", "-", "."):
        if sep in raw:
            parts = [p for p in raw.split(sep) if p]
            if len(parts) == 2:
                a, b = parts[0], parts[1]

                # YYYY-MM
                if len(a) == 4 and a.isdigit() and b.isdigit():
                    y = int(a)
                    m = int(b)
                    if 1 <= m <= 12:
                        return (y, m)

                # MM-YYYY o MM-YY
                if a.isdigit() and b.isdigit() and len(a) <= 2 and len(b) in (2, 4):
                    m = int(a)
                    if len(b) == 2:
                        y2 = int(b)
                        y = 2000 + y2 if y2 < 70 else 1900 + y2
                    else:
                        y = int(b)
                    if 1 <= m <= 12:
                        return (y, m)

    return None


def fechas_iguales(req_cifrado: str, bd_cifrado: str) -> bool: 
    """Compara FECHA DE VENCIMIENTO
    - descifra ambos valores
    - normaliza a (año, mes)
    - compara SOLO mes/año (no día)
    """
    req_txt = _try_descifrar(req_cifrado)
    bd_txt = _try_descifrar(bd_cifrado)

    req_vm = _parse_vencimiento_flexible(req_txt)
    bd_vm = _parse_vencimiento_flexible(bd_txt)

    if req_vm and bd_vm:
        return req_vm[0] == bd_vm[0] and req_vm[1] == bd_vm[1]

    # fallback seguro
    return req_txt.strip() == bd_txt.strip()




def obtener_vencimiento_anio_mes(valor_cifrado_o_claro: str) -> Optional[Tuple[int, int]]:
    """Obtiene (año, mes) de una fecha de vencimiento.

    - Intenta descifrar si viene cifrada.
    - Acepta formatos comunes: MM/YY, MM/YYYY, YYYY-MM, YYYY/MM, etc.
    - Devuelve None si no se puede interpretar.
    """
    txt = _try_descifrar(valor_cifrado_o_claro)
    return _parse_vencimiento_flexible(txt)


def tarjeta_esta_vencida(valor_cifrado_o_claro: str, fecha_referencia: Optional[datetime] = None) -> Optional[bool]:
    """Valida si la tarjeta está vencida contra la fecha actual.

    Devuelve:
    - True  -> vencida
    - False -> vigente (mes/año >= hoy)
    - None  -> no se pudo interpretar el vencimiento
    """
    vm = obtener_vencimiento_anio_mes(valor_cifrado_o_claro)
    if not vm:
        return None

    hoy = fecha_referencia or datetime.now()
    return (vm[0], vm[1]) < (hoy.year, hoy.month)

def iguales_por_texto_descifrado(req_cifrado: str, bd_cifrado: str) -> bool:
    """Para PIN / CVV:

    - descifra ambos
    - compara por texto limpio
    - si no se puede descifrar, compara el string original
    """
    a = _try_descifrar(req_cifrado)
    b = _try_descifrar(bd_cifrado)
    return a.strip() == b.strip()


def normalizar_tipo_transaccion(tipo: str) -> str:
    """Normaliza el campo TipoDeTransaccion del request."""
    if not tipo:
        return ""
    return str(tipo).strip().upper()
