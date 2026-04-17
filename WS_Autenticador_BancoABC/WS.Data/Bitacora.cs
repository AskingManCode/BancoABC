using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WS.Entities;
using Newtonsoft.Json;

namespace WS.DataAccess
{
    public class Bitacora
    {
        private string _rutaCarpeta;
        private readonly string _nombreArchivo = "Bitacora.json";

        public Bitacora()
        {
            string directorioBase = AppDomain.CurrentDomain.BaseDirectory;
            _rutaCarpeta = Path.Combine(directorioBase, "App_Data", "Logs");

            if (!Directory.Exists(Path.Combine(directorioBase, "App_Data")))
            {
                _rutaCarpeta = Path.Combine(Path.GetTempPath(), "WS_Bitacora_Logs");
            }

            CrearDirectorio();
        }

        private void CrearDirectorio()
        {
            try
            {
                if (!Directory.Exists(_rutaCarpeta))
                {
                    Directory.CreateDirectory(_rutaCarpeta);
                }
            }
            catch (Exception ex)
            {
                _rutaCarpeta = Path.Combine(Path.GetTempPath(), "WS_Bitacora_Fallback");
                if (!Directory.Exists(_rutaCarpeta))
                {
                    Directory.CreateDirectory(_rutaCarpeta);
                }
            }
        }

        public void RegistrarActividad(object solicitudRecibida, object resultadoSolicitud)
        {
            try
            {
                string rutaCompleta = Path.Combine(_rutaCarpeta, _nombreArchivo);

                var datosEntrada = new
                {
                    Fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    SolicitudRecibida = solicitudRecibida,
                    RespuestaEnviada = resultadoSolicitud
                };

                string registroJson = JsonConvert.SerializeObject(datosEntrada, Formatting.Indented);

                lock (this)
                {
                    File.AppendAllLines(rutaCompleta, new[] { registroJson });
                }
            }
            catch
            {
                // Silenciar errores de bitácora para no afectar el servicio
            }
        }
    }
}