using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace CapaNegocio
{
    public class ConexionTCP
    {
        private TcpClient client;
        private NetworkStream stream;
        private readonly string ip;
        private readonly int puerto;

        public ConexionTCP (string ip, int puerto)
        {
            this.ip = ip;
            this.puerto = puerto;
        }

        public bool Conectar()
        {
            try
            {
                if (client == null || !client.Connected)
                {
                    client = new TcpClient();
                    client.Connect(ip, puerto);
                    stream = client.GetStream();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EstaConectado()
        {
            return client != null && client.Connected;
        }

        public bool Enviar(string mensaje)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(mensaje);
                stream.Write(data, 0, data.Length);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string Recibir()
        {
            byte[] buffer = new byte[1024];
            int bytes = stream.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer, 0, bytes);
        }

        public void Cerrar()
        {
            stream?.Close();
            client?.Close();
            stream = null;
            client = null;
        }
    }
}
