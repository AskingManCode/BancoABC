import json
import socket
import threading
from typing import Any, Dict, Callable, Optional


class ServidorAutorizador:
    """
    Servidor TCP que recibe JSON y responde JSON.
    - Soporta clientes que envían en varios chunks.
    - Procesa por "líneas" (cada request termina con \n).
      Si el cliente no manda \n, igual intenta parsear cuando el buffer forma un JSON válido.
    """

    def __init__(self, host: str, port: int, handler: Callable[[Dict[str, Any]], Dict[str, Any]]):
        self._host = host
        self._port = port
        self._handler = handler
        self._server_socket: Optional[socket.socket] = None
        self._detener = threading.Event()

    def iniciar(self) -> None:
        self._server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self._server_socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        self._server_socket.bind((self._host, self._port))
        self._server_socket.listen(50)
        print(f"[ServidorAutorizador] Escuchando en {self._host}:{self._port}")

        while not self._detener.is_set():
            try:
                client_sock, _addr = self._server_socket.accept()
            except OSError:
                break

            th = threading.Thread(target=self._atender_cliente, args=(client_sock,), daemon=True)
            th.start()

    def detener(self) -> None:
        self._detener.set()
        if self._server_socket:
            try:
                self._server_socket.close()
            except Exception:
                pass

    def _try_parse_json_objeto(self, data: bytes) -> Optional[Dict[str, Any]]:
        """
        Intenta parsear el buffer completo como JSON objeto (dict).
        Retorna dict si se pudo, si no retorna None.
        """
        try:
            txt = data.decode("utf-8", errors="ignore").strip()
            if not txt:
                return None
            obj = json.loads(txt)
            if isinstance(obj, dict):
                return obj
            return None
        except Exception:
            return None

    def _atender_cliente(self, client_sock: socket.socket) -> None:
        buffer = b""
        try:
            while True:
                chunk = client_sock.recv(4096)
                if not chunk:
                    break

                buffer += chunk

                # 1) Si el cliente manda "\n", procesamos por líneas
                if b"\n" in buffer:
                    lineas = buffer.split(b"\n")
                    buffer = lineas[-1] 

                    for linea in lineas[:-1]:
                        req = self._try_parse_json_objeto(linea)
                        if req is None:
                            continue

                        resp = self._procesar_req(req)
                        client_sock.sendall((json.dumps(resp, ensure_ascii=False) + "\n").encode("utf-8"))
                    continue

                # 2) Si no hay "\n", intentamos parsear el buffer completo (por si el cliente manda todo de una)
                req = self._try_parse_json_objeto(buffer)
                if req is None:
                    continue

                resp = self._procesar_req(req)
                client_sock.sendall((json.dumps(resp, ensure_ascii=False) + "\n").encode("utf-8"))
                buffer = b""
        finally:
            try:
                client_sock.close()
            except Exception:
                pass

    def _procesar_req(self, req: Dict[str, Any]) -> Dict[str, Any]:
        try:
            resp = self._handler(req)
            if not isinstance(resp, dict):
                return {"status": "5"}
            return resp
        except Exception:
            return {"status": "5"}
