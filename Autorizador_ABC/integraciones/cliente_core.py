# integraciones/cliente_core.py
import json
import socket
from typing import Optional, Dict, Any


class ClienteCore:

    def __init__(self, host: str, port: int, timeout_seg: float = 5.0):
        self._host = host
        self._port = port
        self._timeout = timeout_seg

    def _enviar_json(self, payload: Dict[str, Any], reintentos: int = 2) -> Optional[Dict[str, Any]]:
        """
        Envía un JSON al core y espera respuesta JSON.
        """
        mensaje = json.dumps(payload, ensure_ascii=False) + "\n"
        data_out = mensaje.encode("utf-8")

        for _ in range(max(1, int(reintentos) + 1)):
            try:
                with socket.create_connection((self._host, self._port), timeout=self._timeout) as s:
                    s.settimeout(self._timeout)
                    s.sendall(data_out)

                    buffer = b""
                    while True:
                        chunk = s.recv(4096)
                        if not chunk:
                            break
                        buffer += chunk
                        if b"\n" in buffer:
                            break

                texto = buffer.decode("utf-8", errors="ignore").strip()
                if not texto:
                    continue
                return json.loads(texto)

            except Exception:
                continue

        return None


    def consultar_saldo_debito(
        self,
        numero_cuenta: str,
        numero_tarjeta: str
    ) -> Optional[float]:
        payload = {
            "TipoDeTransaccion": "Consulta",
            "NumeroCuenta": str(numero_cuenta),
            "NumeroTarjeta": str(numero_tarjeta)
        }

        resp = self._enviar_json(payload)
        if not resp:
            return None

        if resp.get("status") != "OK":
            return None

        # Core puede devolver "Saldo" o "saldo".
        try:
            if "Saldo" in resp:
                return float(resp.get("Saldo"))
            if "saldo" in resp:
                return float(resp.get("saldo"))
            return None
        except Exception:
            return None

    def verificar_fondos_debito(
        self,
        numero_cuenta: str,
        numero_tarjeta: str,
        monto: float
    ) -> bool:
        payload = {
            "TipoDeTransaccion": "Retiro",
            "NumeroCuenta": str(numero_cuenta),
            "NumeroTarjeta": str(numero_tarjeta),
            "Monto": f"{float(monto):.2f}"
        }

        resp = self._enviar_json(payload)
        if not resp:
            return False

        return resp.get("status") == "OK"

    def confirmar_retiro_debito(
        self,
        numero_cuenta: str,
        numero_tarjeta: str,
        monto: float
        ,
        codigo_autorizacion: str = ""
    ) -> str:
        payload = {
            "TipoDeTransaccion": "Confirmacion",
            "NumeroCuenta": str(numero_cuenta),
            "NumeroTarjeta": str(numero_tarjeta),
            "Monto": f"{float(monto):.2f}"
        }

        if codigo_autorizacion:
            payload["CodigoAutorizacion"] = str(codigo_autorizacion).strip()

        resp = self._enviar_json(payload)
        if not resp:
            return "ERROR"

        # status esperado: OK / INSUF / ERROR
        st = str(resp.get("status", "ERROR")).strip().upper()
        if st == "OK":
            return "OK"
        if st == "INSUF":
            return "INSUF"
        if st == "INVALID":
            return "INVALID"
        return "ERROR"
