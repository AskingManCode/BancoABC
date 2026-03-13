import json
import os
import queue
import threading
from datetime import datetime
from typing import Any, Dict

from seguridad.cifrado_aes import enmascarar_tarjeta


class BitacoraWorker(threading.Thread):
    def __init__(self, ruta_archivo: str):
        super().__init__(daemon=True)
        self._ruta = ruta_archivo
        self._cola: "queue.Queue[Dict[str, Any]]" = queue.Queue()
        self._stop_event = threading.Event()

        carpeta = os.path.dirname(self._ruta)
        if carpeta:
            os.makedirs(carpeta, exist_ok=True)

    def _monto_2_dec(self, valor: Any) -> str:
        """Devuelve monto SIEMPRE como string con 2 decimales (HU AUT4)."""
        if valor is None:
            return "0.00"
        s = str(valor).strip().replace(",", "")
        if not s:
            return "0.00"
        try:
            return f"{float(s):.2f}"
        except Exception:
            return "0.00"

    def encolar(self, payload: Dict[str, Any]) -> None:
        tarjeta = payload.get("tarjeta", "")
        cajero = payload.get("cajero", "")
        cliente = payload.get("cliente", payload.get("cliente_identificacion", ""))
        tipo = payload.get("tipo", payload.get("tipo_transaccion", ""))
        monto = payload.get("monto", 0)

        autorizacion = payload.get("autorizacion", payload.get("Autorizacion", ""))

        self.encolar_evento(
            tarjeta_cifrada_o_clara=str(tarjeta),
            cajero_identificador=str(cajero),
            cliente_identificacion=str(cliente),
            tipo_transaccion=str(tipo),
            monto_str=self._monto_2_dec(monto),
            autorizacion=str(autorizacion).strip()
        )

    def encolar_evento(
        self,
        tarjeta_cifrada_o_clara: str,
        cajero_identificador: str,
        cliente_identificacion: str,
        tipo_transaccion: str,
        monto_str: str,
        autorizacion: str = ""
    ) -> None:
        # HU AUT4: el formato es
        # DD/MM/YYYY: {"tarjeta": "...", "cajero": "...", "cliente": "...", "tipo": "...", "Monto": "75000.00"}
        self._cola.put({
            "_fecha_prefijo": datetime.now().strftime("%d/%m/%Y"),
            "tarjeta": enmascarar_tarjeta(tarjeta_cifrada_o_clara),
            "cajero": cajero_identificador,
            "cliente": cliente_identificacion,
            "tipo": tipo_transaccion,
            "Monto": str(monto_str),
            "autorizacion": autorizacion
        })

    def detener(self) -> None:
        self._stop_event.set()
        self._cola.put({"_fin": True})

    def run(self) -> None:
        while not self._stop_event.is_set():
            item = self._cola.get()
            if item.get("_fin"):
                break
            try:
                fecha = item.pop("_fecha_prefijo", datetime.now().strftime("%d/%m/%Y"))
                with open(self._ruta, "a", encoding="utf-8") as f:
                    f.write(f"{fecha}: {json.dumps(item, ensure_ascii=False)}\n")
            except Exception:
                pass
