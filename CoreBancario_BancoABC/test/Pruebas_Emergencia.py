import socket
import json

HOST = "127.0.0.1"
PORT = 6000

def main():
    # Crear socket TCP
    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
        s.connect((HOST, PORT))
        print("Conectado al servidor")

        """
        # Mensaje JSON de prueba
        mensaje = {
            "TipoDeTransaccion": "Retiro",
            "NumeroCuenta": "1",
            "NumeroTarjeta": "453210******5678",
            "Monto": 532.00
        }

        json_envio = json.dumps(mensaje)
        print("Enviando:", json_envio + "\n")

        # Enviar (UTF-8)
        s.sendall((json_envio + "\n").encode("utf-8"))
        
        # Mensaje JSON de prueba
        mensaje = {
            "TipoDeTransaccion": "Consulta",
            "NumeroCuenta": "2",
            "NumeroTarjeta": "453210******4321"
        }

        json_envio = json.dumps(mensaje)
        print("Enviando:", json_envio)

        # Enviar (UTF-8)
        s.sendall((json_envio + "\n").encode("utf-8"))
       """
        
        # Mensaje JSON de prueba
        mensaje = {
            "TipoDeTransaccion": "Confirmacion",
            "NumeroCuenta": "5",
            "NumeroTarjeta": "453210******1122",
            "CodigoAutorizacion": "123456789",
            "Monto": 43250.75
        }

        json_envio = json.dumps(mensaje)
        print("Enviando:", json_envio)

        # Enviar (UTF-8)
        s.sendall((json_envio + "\n").encode("utf-8")) 
        
        # Recibir respuesta
        while True:
            data = s.recv(4096)
            if not data:
                break
            print("Respuesta del servidor:", data.decode("utf-8"))

if __name__ == "__main__":
    main()
