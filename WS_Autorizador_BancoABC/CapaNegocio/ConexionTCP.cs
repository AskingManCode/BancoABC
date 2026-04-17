using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class ConexionTCP : IDisposable
    {
        private TcpClient client;
        private NetworkStream stream;
        private readonly string ip;
        private readonly int puerto;

        public ConexionTCP(string ip, int puerto)
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
            catch { return false; }
        }

        public bool EstaConectado() => client != null && client.Connected;

        public string EnviarYRecibir(string mensaje)
        {
            if (!EstaConectado() && !Conectar())
                throw new Exception("No se pudo conectar al autorizador");

            if (!mensaje.EndsWith("\n"))
                mensaje += "\n";

            byte[] data = Encoding.UTF8.GetBytes(mensaje);
            stream.Write(data, 0, data.Length);

            using (MemoryStream ms = new MemoryStream())
            {
                byte[] buffer = new byte[1];
                int bytesRead;
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, bytesRead);
                    if (buffer[0] == '\n')
                        break;
                }
                byte[] respuestaBytes = ms.ToArray();
                return Encoding.UTF8.GetString(respuestaBytes).TrimEnd('\n');
            }
        }

        public void Cerrar()
        {
            stream?.Close();
            client?.Close();
            stream = null;
            client = null;
        }

        public void Dispose() => Cerrar();
    }
}
