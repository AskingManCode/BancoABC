using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

// NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código, en svc y en el archivo de configuración.
public class AutorizadorService : IAutorizadorService
{
    string ip = "127.0.0.1";
    int puerto = 5000;

    private string EnviarSocket(string mensaje)
    {
        using (TcpClient client = new TcpClient(ip, puerto))
        {
            NetworkStream stream = client.GetStream();

            byte[] datos = Encoding.UTF8.GetBytes(mensaje);
            stream.Write(datos, 0, datos.Length);

            byte[] buffer = new byte[1024];
            int bytes = stream.Read(buffer, 0, buffer.Length);

            return Encoding.UTF8.GetString(buffer, 0, bytes);
        }
    }

    public RespuestaConsulta ConsultarSaldo(
        string numeroTarjeta,
        string cvv,
        string fechaVencimiento,
        string identificadorCajero)
    {
        try
        {
            string trama = "CONSULTA|" + numeroTarjeta + "|" + cvv + "|" + fechaVencimiento + "|" + identificadorCajero;

            string respuesta = EnviarSocket(trama);

            return new RespuestaConsulta
            {
                Resultado = true,
                Mensaje = "Transacción exitosa",
                Saldo = respuesta
            };
        }
        catch
        {
            return new RespuestaConsulta
            {
                Resultado = false,
                Mensaje = "Error en el autorizador",
                Saldo = "0"
            };
        }
    }

    public RespuestaSimple CambiarPIN(
        string numeroTarjeta,
        string pinActual,
        string pinNuevo,
        string fechaVencimiento,
        string cvv,
        string identificadorCajero)
    {
        try
        {
            string trama = "PIN|" + numeroTarjeta + "|" + pinActual + "|" + pinNuevo + "|" + fechaVencimiento + "|" + cvv + "|" + identificadorCajero;

            string respuesta = EnviarSocket(trama);

            return new RespuestaSimple
            {
                Resultado = true,
                Mensaje = "Transacción exitosa"
            };
        }
        catch
        {
            return new RespuestaSimple
            {
                Resultado = false,
                Mensaje = "No autorizado"
            };
        }
    }
}