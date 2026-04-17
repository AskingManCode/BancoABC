from __future__ import annotations
from dataclasses import dataclass
from decimal import Decimal, ROUND_HALF_UP
from threading import Lock
from time import time
from datetime import datetime
from typing import Dict, Optional, Tuple


def _q2(d: Decimal) -> Decimal:
    # Normaliza a 2 decimales para comparar montos de forma consistente
    return d.quantize(Decimal("0.01"), rounding=ROUND_HALF_UP)


@dataclass
class _RegistroAutorizacion:
    tarjeta_numero: str
    cajero_id: str
    monto: Decimal
    dia: str
    expira_en: float
    usado: bool = False


class AlmacenAutorizacionesMemoria:
    """Almacén en memoria para códigos de autorización (sin cambios en BD).

    - Sirve para enlazar AUT1 (genera código) con AUT5 (confirma con código).
    - Limitación: si el servidor Python se reinicia, se pierden los códigos.
    """
    def __init__(self, ttl_segundos: int = 86400):
        self._ttl = int(ttl_segundos)
        self._lock = Lock()
        self._items: Dict[str, _RegistroAutorizacion] = {}
        # Para cumplir HU: "código único por día" incluso si ya se consumió.
        self._usados_por_dia: Dict[str, set[str]] = {}

    def _hoy(self) -> str:
        return datetime.now().strftime("%Y%m%d")

    def existe_codigo_hoy(self, codigo: str) -> bool:
        c = (codigo or "").strip()
        if not c:
            return False
        hoy = self._hoy()
        with self._lock:
            if c in self._items:
                return True
            return c in self._usados_por_dia.get(hoy, set())

    def guardar(self, codigo: str, tarjeta_numero: str, cajero_id: str, monto: Decimal) -> None:
        c = (codigo or "").strip()
        if not c:
            return
        ahora = time()
        hoy = self._hoy()
        reg = _RegistroAutorizacion(
            tarjeta_numero=str(tarjeta_numero).strip(),
            cajero_id=str(cajero_id).strip(),
            monto=_q2(Decimal(str(monto))),
            dia=hoy,
            expira_en=ahora + self._ttl,
            usado=False,
        )
        with self._lock:
            # unicidad por día
            usados = self._usados_por_dia.setdefault(hoy, set())
            usados.add(c)
            self._items[c] = reg
            self._limpiar_expirados_locked(ahora)

    def validar_y_consumir(self, codigo: str, tarjeta_numero: str, cajero_id: str, monto: Decimal) -> Tuple[bool, str]:
        c = (codigo or "").strip()
        if not c:
            return False, "Código vacío"

        ahora = time()
        with self._lock:
            self._limpiar_expirados_locked(ahora)

            reg = self._items.get(c)
            if not reg:
                return False, "Código inválido"
            # Si por alguna razón el código es de otro día, se rechaza.
            if reg.dia != self._hoy():
                self._items.pop(c, None)
                return False, "Código expirado"
            if reg.usado:
                return False, "Código ya utilizado"
            if reg.expira_en <= ahora:
                # por si quedó, lo removemos
                self._items.pop(c, None)
                return False, "Código expirado"

            if reg.tarjeta_numero != str(tarjeta_numero).strip():
                return False, "Código no corresponde a la tarjeta"
            if reg.cajero_id != str(cajero_id).strip():
                return False, "Código no corresponde al cajero"

            try:
                m = _q2(Decimal(str(monto)))
            except Exception:
                return False, "Monto inválido"

            if reg.monto != m:
                return False, "Monto no coincide"

            # Consumir
            reg.usado = True
            # Opcional: remover para no crecer
            self._items.pop(c, None)
            return True, "OK"

    def _limpiar_expirados_locked(self, ahora: Optional[float] = None) -> None:
        t = time() if ahora is None else ahora
        expirados = [k for k, v in self._items.items() if v.expira_en <= t or v.usado]
        for k in expirados:
            self._items.pop(k, None)

        # Limpiar días viejos (dejamos solo hoy y ayer por seguridad)
        try:
            hoy = self._hoy()
            dias = sorted(self._usados_por_dia.keys())
            for d in dias:
                if d < hoy:
                    # dejamos ayer si existe
                    # (si querés dejar solo hoy, borrá esta condición)
                    if len(dias) > 1 and d == dias[-2]:
                        continue
                    self._usados_por_dia.pop(d, None)
        except Exception:
            pass


# Singleton por proceso (lo usan AUT1 y AUT5)
almacen_autorizaciones = AlmacenAutorizacionesMemoria(ttl_segundos=86400)
