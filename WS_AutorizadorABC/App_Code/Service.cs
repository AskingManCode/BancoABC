using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using CapaNegocio;

// NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código, en svc y en el archivo de configuración.
public class AutorizadorService : IAutorizadorService
{
    private readonly string ip = "127.0.0.1";
    private readonly int puerto = 5000;

    public RespuestaConsulta ConsultarSaldo(
        string numeroTarjetaCifrado,
        string cvvCifrado,
        string fechaVencimientoCifrado,
        string identificadorCajeroCifrado)
    {
        try
        {
            var trama = new
            {
                NumeroDeTarjeta = numeroTarjetaCifrado,
                Pin = "",
                FechaDeVencimiento = fechaVencimientoCifrado,
                CodigoDeVerificacion = cvvCifrado,
                IdentificadorDelCajero = identificadorCajeroCifrado,
                TipoDeTransaccion = "Consulta"
            };

            string json = JsonConvert.SerializeObject(trama);

            using (var tcp = new ConexionTCP(ip, puerto))
            {
                string respuestaJson = tcp.EnviarYRecibir(json);
                var respuesta = JsonConvert.DeserializeObject<Dictionary<string, object>>(respuestaJson);

                if (respuesta != null && respuesta.ContainsKey("status") && respuesta["status"].ToString() == "OK")
                {
                    string monto = respuesta.ContainsKey("Monto") ? respuesta["Monto"].ToString() : "0.00";
                    return new RespuestaConsulta
                    {
                        Resultado = true,
                        Mensaje = "Transacción exitosa",
                        Saldo = monto
                    };
                }
                else
                {
                    return new RespuestaConsulta
                    {
                        Resultado = false,
                        Mensaje = "No se ha autorizado la transacción",
                        Saldo = "0"
                    };
                }
            }
        }
        catch (Exception)
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
        string numeroTarjetaCifrado,
        string pinActualCifrado,
        string pinNuevoCifrado,
        string fechaVencimientoCifrado,
        string cvvCifrado,
        string identificadorCajeroCifrado)
    {
        try
        {
            var trama = new
            {
                NumeroDeTarjeta = numeroTarjetaCifrado,
                PinActual = pinActualCifrado,
                PinNuevo = pinNuevoCifrado,
                FechaDeVencimiento = fechaVencimientoCifrado,
                CodigoDeVerificacion = cvvCifrado,
                IdentificadorDelCajero = identificadorCajeroCifrado,
                TipoDeTransaccion = "CambioPIN"
            };

            string json = JsonConvert.SerializeObject(trama);

            using (var tcp = new ConexionTCP(ip, puerto))
            {
                string respuestaJson = tcp.EnviarYRecibir(json);
                var respuesta = JsonConvert.DeserializeObject<Dictionary<string, object>>(respuestaJson);

                if (respuesta != null && respuesta.ContainsKey("status") && respuesta["status"].ToString() == "OK")
                {
                    return new RespuestaSimple { Resultado = true, Mensaje = "Transacción exitosa" };
                }
                else
                {
                    return new RespuestaSimple { Resultado = false, Mensaje = "No se ha autorizado la transacción" };
                }
            }
        }
        catch
        {
            return new RespuestaSimple { Resultado = false, Mensaje = "Error en el autorizador" };
        }
    }
}