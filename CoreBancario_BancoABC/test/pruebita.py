import socket
import json
import threading

HOST = "127.0.0.1"
PORT = 6000

def cliente(id_cliente):
    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
        s.connect((HOST, PORT))
        print(f"Cliente {id_cliente} conectado")

        mensaje = {
            "TipoDeTransaccion": "Consulta",
            "NumeroCuenta": "7",
            "NumeroTarjeta": "541230******6677"
        }

        json_envio = json.dumps(mensaje)
        s.sendall((json_envio + "\n").encode("utf-8"))

        while True:
            data = s.recv(4096)
            if not data:
                break
            print(f"Cliente {id_cliente} recibe:", data.decode("utf-8"))

threads = []

for i in range(4):  # 4 clientes simultáneos
    t = threading.Thread(target=cliente, args=(i,))
    t.start()
    threads.append(t)

for t in threads:
    t.join()
