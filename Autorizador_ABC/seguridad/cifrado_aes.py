import base64
from Crypto.Cipher import AES
from Crypto.Util.Padding import pad, unpad

_KEY = b"12345678901234567890123456789012"  # 32 bytes (AES-256)
_IV  = b"1234567890123456"                  # 16 bytes (CBC)


def cifrar(texto: str) -> str:
    """
    Cifra texto plano con AES-CBC y devuelve Base64.
    """
    if texto is None:
        return ""

    data = str(texto).encode("utf-8")
    cipher = AES.new(_KEY, AES.MODE_CBC, _IV)
    ct = cipher.encrypt(pad(data, AES.block_size))
    return base64.b64encode(ct).decode("utf-8")


def descifrar(texto_b64: str) -> str:
    """
    Descifra un texto Base64 cifrado con AES-CBC.
    Lanza excepción si el texto no es válido.
    """
    if not texto_b64:
        return ""

    raw = base64.b64decode(texto_b64)
    cipher = AES.new(_KEY, AES.MODE_CBC, _IV)
    pt = unpad(cipher.decrypt(raw), AES.block_size)
    return pt.decode("utf-8")


def enmascarar_tarjeta(tarjeta_cifrada_o_clara: str) -> str:
    """
    Enmascara número de tarjeta:
    - Intenta descifrar
    - Si falla, asume texto claro
    - Muestra solo algunos dígito
    """
    if not tarjeta_cifrada_o_clara:
        return "**** **** **** ****"

    try:
        tarjeta = descifrar(tarjeta_cifrada_o_clara)
    except Exception:
        tarjeta = str(tarjeta_cifrada_o_clara)

    # solo dígitos
    dig = "".join(ch for ch in tarjeta if ch.isdigit())

    if len(dig) < 8:
        return "**** **** **** ****"

    # Formato: 1234 56** **** 7890
    p4 = dig[:4]
    m2 = dig[4:6]
    u4 = dig[-4:]

    return f"{p4} {m2}** **** {u4}"
def enmascarar_tarjeta_core(tarjeta_cifrada_o_clara: str) -> str:
    """
    Formato que espera el Core
    6 primeros dígitos + '******' + 4 últimos dígitos
    """
    if not tarjeta_cifrada_o_clara:
        return ""

    try:
        tarjeta = descifrar(tarjeta_cifrada_o_clara)
    except Exception:
        tarjeta = str(tarjeta_cifrada_o_clara)

    dig = "".join(ch for ch in tarjeta if ch.isdigit())

    # Mínimo: 6 + 4
    if len(dig) < 10:
        return dig

    return f"{dig[:6]}******{dig[-4:]}"
