using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Newtonsoft.Json;
using CapaNegocio;

// NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código, en svc y en el archivo de configuración.
public class AutorizadorService : IAutorizadorService
{
    private readonly string ip = "127.0.0.1";
    private readonly int puerto = 5000;

    // ===== TUS MÉTODOS =====

    public RespuestaConsulta ConsultarSaldo(
        string numeroTarjeta,
        string cvv,
        string fechaVencimiento,
        string identificadorCajero)
    {
        try
        {
            // 1. Cifrar los datos sensibles
            CifradoDeDatos cifrador = new CifradoDeDatos();
            var trama = new
            {
                NumeroDeTarjeta = cifrador.Cifrar(numeroTarjeta),
                Pin = "", // Vacío para consulta
                FechaDeVencimiento = cifrador.Cifrar(fechaVencimiento),
                CodigoDeVerificacion = cifrador.Cifrar(cvv),
                IdentificadorDelCajero = identificadorCajero, // No se cifra
                TipoDeTransaccion = "Consulta"
            };

            // 2. Enviar al autorizador Python
            string json = JsonConvert.SerializeObject(trama);
            using (var tcp = new ConexionTCP(ip, puerto))
            {
                string respuestaJson = tcp.EnviarYRecibir(json);
                var respuesta = JsonConvert.DeserializeObject<Dictionary<string, object>>(respuestaJson);

                // 3. Procesar respuesta
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
        catch (Exception ex)
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
            // 1. Cifrar los datos sensibles
            CifradoDeDatos cifrador = new CifradoDeDatos();
            var trama = new
            {
                NumeroDeTarjeta = cifrador.Cifrar(numeroTarjeta),
                PinActual = cifrador.Cifrar(pinActual),
                PinNuevo = cifrador.Cifrar(pinNuevo),
                FechaDeVencimiento = cifrador.Cifrar(fechaVencimiento),
                CodigoDeVerificacion = cifrador.Cifrar(cvv),
                IdentificadorDelCajero = identificadorCajero, // No se cifra
                TipoDeTransaccion = "CambioPIN"
            };

            // 2. Enviar al autorizador Python
            string json = JsonConvert.SerializeObject(trama);
            using (var tcp = new ConexionTCP(ip, puerto))
            {
                string respuestaJson = tcp.EnviarYRecibir(json);
                var respuesta = JsonConvert.DeserializeObject<Dictionary<string, object>>(respuestaJson);

                // 3. Procesar respuesta
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

    public RespuestaSimple AutorizarRetiro(
    string numeroTarjeta,
    string cvv,
    string pin,
    string fechaVencimiento,
    string identificadorCajero,
    string montoRetiro)
    {
        try
        {
            decimal montoDecimal;
            if (!decimal.TryParse(montoRetiro, out montoDecimal) || montoDecimal <= 0)
            {
                return new RespuestaSimple
                {
                    Resultado = false,
                    Mensaje = "No se ha autorizado la transacción"
                };
            }

            if (montoDecimal > 50000 && string.IsNullOrWhiteSpace(pin))
            {
                return new RespuestaSimple
                {
                    Resultado = false,
                    Mensaje = "No se ha autorizado la transacción"
                };
            }

            CifradoDeDatos cifrador = new CifradoDeDatos();

            var trama = new
            {
                NumeroDeTarjeta = cifrador.Cifrar(numeroTarjeta),
                Pin = string.IsNullOrWhiteSpace(pin) ? "" : cifrador.Cifrar(pin),
                FechaDeVencimiento = cifrador.Cifrar(fechaVencimiento),
                CodigoDeVerificacion = cifrador.Cifrar(cvv),
                IdentificadorDelCajero = identificadorCajero,
                TipoDeTransaccion = "Retiro",
                MontoRetiro = montoRetiro
            };


            string json = JsonConvert.SerializeObject(trama);

            using (var tcp = new ConexionTCP(ip, puerto))
            {
                string respuestaJson = tcp.EnviarYRecibir(json);
                var respuesta = JsonConvert.DeserializeObject<Dictionary<string, object>>(respuestaJson);

                if (respuesta == null || !respuesta.ContainsKey("status"))
                {
                    return new RespuestaSimple
                    {
                        Resultado = false,
                        Mensaje = "No se ha autorizado la transacción"
                    };
                }

                if (respuesta["status"].ToString() == "OK")
                {
                    return new RespuestaSimple
                    {
                        Resultado = true,
                        Mensaje = "Transacción autorizada"
                    };
                }

                return new RespuestaSimple
                {
                    Resultado = false,
                    Mensaje = "No se ha autorizado la transacción"
                };
            }
        }
        catch
        {
            return new RespuestaSimple
            {
                Resultado = false,
                Mensaje = "Error en el autorizador"
            };
        }
    }

    public List<CuentaInfo> ObtenerCuentasPorCliente(string identificacionCliente)
    {
        try
        {
            CifradoDeDatos cifrador = new CifradoDeDatos();
            var trama = new
            {
                Identificacion = cifrador.Cifrar(identificacionCliente),
                TipoDeTransaccion = "ObtenerCuentas"
            };

            string json = JsonConvert.SerializeObject(trama);
            using (var tcp = new ConexionTCP(ip, puerto))
            {
                string respuestaJson = tcp.EnviarYRecibir(json);
                var respuesta = JsonConvert.DeserializeObject<Dictionary<string, object>>(respuestaJson);

                if (respuesta != null && respuesta.ContainsKey("status") && respuesta["status"].ToString() == "OK")
                {
                    // Se espera una propiedad "cuentas" que es un array de objetos
                    var cuentasJson = respuesta["cuentas"].ToString();
                    return JsonConvert.DeserializeObject<List<CuentaInfo>>(cuentasJson);
                }
                return new List<CuentaInfo>();
            }
        }
        catch
        {
            return new List<CuentaInfo>();
        }
    }

    public List<TarjetaInfo> ObtenerTarjetasPorCliente(string identificacionCliente)
    {
        try
        {
            CifradoDeDatos cifrador = new CifradoDeDatos();
            var trama = new
            {
                Identificacion = cifrador.Cifrar(identificacionCliente),
                TipoDeTransaccion = "ObtenerTarjetas"
            };

            string json = JsonConvert.SerializeObject(trama);
            using (var tcp = new ConexionTCP(ip, puerto))
            {
                string respuestaJson = tcp.EnviarYRecibir(json);
                var respuesta = JsonConvert.DeserializeObject<Dictionary<string, object>>(respuestaJson);

                if (respuesta != null && respuesta.ContainsKey("status") && respuesta["status"].ToString() == "OK")
                {
                    var tarjetasJson = respuesta["tarjetas"].ToString();
                    return JsonConvert.DeserializeObject<List<TarjetaInfo>>(tarjetasJson);
                }
                return new List<TarjetaInfo>();
            }
        }
        catch
        {
            return new List<TarjetaInfo>();
        }
    }

    public List<MovimientoCuenta> ObtenerMovimientosCuenta(string identificacionCliente, string numeroCuenta)
    {
        try
        {
            CifradoDeDatos cifrador = new CifradoDeDatos();
            var trama = new
            {
                Identificacion = cifrador.Cifrar(identificacionCliente),
                NumeroCuenta = cifrador.Cifrar(numeroCuenta),
                TipoDeTransaccion = "ObtenerMovimientosCuenta"
            };

            string json = JsonConvert.SerializeObject(trama);
            using (var tcp = new ConexionTCP(ip, puerto))
            {
                string respuestaJson = tcp.EnviarYRecibir(json);
                var respuesta = JsonConvert.DeserializeObject<Dictionary<string, object>>(respuestaJson);

                if (respuesta != null && respuesta.ContainsKey("status") && respuesta["status"].ToString() == "OK")
                {
                    var movimientosJson = respuesta["movimientos"].ToString();
                    return JsonConvert.DeserializeObject<List<MovimientoCuenta>>(movimientosJson);
                }
                return new List<MovimientoCuenta>();
            }
        }
        catch
        {
            return new List<MovimientoCuenta>();
        }
    }

    public List<MovimientoCredito> ObtenerMovimientosTarjetaCredito(string identificacionCliente, string numeroTarjeta)
    {
        try
        {
            CifradoDeDatos cifrador = new CifradoDeDatos();
            var trama = new
            {
                Identificacion = cifrador.Cifrar(identificacionCliente),
                NumeroTarjeta = cifrador.Cifrar(numeroTarjeta),
                TipoDeTransaccion = "ObtenerMovimientosCredito"
            };

            string json = JsonConvert.SerializeObject(trama);
            using (var tcp = new ConexionTCP(ip, puerto))
            {
                string respuestaJson = tcp.EnviarYRecibir(json);
                var respuesta = JsonConvert.DeserializeObject<Dictionary<string, object>>(respuestaJson);

                if (respuesta != null && respuesta.ContainsKey("status") && respuesta["status"].ToString() == "OK")
                {
                    var movimientosJson = respuesta["movimientos"].ToString();
                    return JsonConvert.DeserializeObject<List<MovimientoCredito>>(movimientosJson);
                }
                return new List<MovimientoCredito>();
            }
        }
        catch
        {
            return new List<MovimientoCredito>();
        }
    }
    public List<TarjetaInfo> ObtenerTarjetasADM(string identificacionCliente)
    {
        try
        {
            CifradoDeDatos cifrador = new CifradoDeDatos();
            var trama = new
            {
                Identificacion = cifrador.Cifrar(identificacionCliente),
                TipoDeTransaccion = "ObtenerTarjetasADM"
            };

            string json = JsonConvert.SerializeObject(trama);
            using (var tcp = new ConexionTCP(ip, puerto))
            {
                string respuestaJson = tcp.EnviarYRecibir(json);
                var respuesta = JsonConvert.DeserializeObject<Dictionary<string, object>>(respuestaJson);

                if (respuesta != null &&
                    respuesta.ContainsKey("status") &&
                    respuesta["status"].ToString() == "OK")
                {
                    var tarjetasJson = respuesta["tarjetas"].ToString();
                    return JsonConvert.DeserializeObject<List<TarjetaInfo>>(tarjetasJson);
                }

                return new List<TarjetaInfo>();
            }
        }
        catch
        {
            return new List<TarjetaInfo>();
        }
    }
    public RespuestaSimple CrearTarjeta(
    string identificacionCliente,
    string tipoTarjeta,
    string numeroCuenta,
    string numeroTarjeta,
    string pin,
    string cvv,
    string fechaVencimiento)
    {
        try
        {
            CifradoDeDatos cifrador = new CifradoDeDatos();

            var trama = new
            {
                Identificacion = cifrador.Cifrar(identificacionCliente),
                TipoTarjeta = tipoTarjeta,
                NumeroCuenta = string.IsNullOrWhiteSpace(numeroCuenta) ? "" : numeroCuenta,
                NumeroDeTarjeta = cifrador.Cifrar(numeroTarjeta),
                Pin = cifrador.Cifrar(pin),
                CodigoDeVerificacion = cifrador.Cifrar(cvv),
                FechaDeVencimiento = cifrador.Cifrar(fechaVencimiento),
                TipoDeTransaccion = "CrearTarjeta"
            };

            string json = JsonConvert.SerializeObject(trama);

            using (var tcp = new ConexionTCP(ip, puerto))
            {
                string respuestaJson = tcp.EnviarYRecibir(json);
                var respuesta = JsonConvert.DeserializeObject<Dictionary<string, object>>(respuestaJson);

                if (respuesta != null &&
                    respuesta.ContainsKey("status") &&
                    respuesta["status"].ToString() == "OK")
                {
                    return new RespuestaSimple
                    {
                        Resultado = true,
                        Mensaje = respuesta.ContainsKey("mensaje")
                            ? respuesta["mensaje"].ToString()
                            : "Tarjeta creada correctamente"
                    };
                }

                return new RespuestaSimple
                {
                    Resultado = false,
                    Mensaje = respuesta != null && respuesta.ContainsKey("mensaje")
                        ? respuesta["mensaje"].ToString()
                        : "No fue posible crear la tarjeta"
                };
            }
        }
        catch
        {
            return new RespuestaSimple
            {
                Resultado = false,
                Mensaje = "Error en el autorizador"
            };
        }
    }
    public RespuestaSimple InactivarTarjeta(string numeroTarjeta)
    {
        try
        {
            CifradoDeDatos cifrador = new CifradoDeDatos();

            var trama = new
            {
                NumeroDeTarjeta = cifrador.Cifrar(numeroTarjeta),
                TipoDeTransaccion = "InactivarTarjeta"
            };

            string json = JsonConvert.SerializeObject(trama);

            using (var tcp = new ConexionTCP(ip, puerto))
            {
                string respuestaJson = tcp.EnviarYRecibir(json);
                var respuesta = JsonConvert.DeserializeObject<Dictionary<string, object>>(respuestaJson);

                if (respuesta != null &&
                    respuesta.ContainsKey("status") &&
                    respuesta["status"].ToString() == "OK")
                {
                    return new RespuestaSimple
                    {
                        Resultado = true,
                        Mensaje = respuesta.ContainsKey("mensaje")
                            ? respuesta["mensaje"].ToString()
                            : "Tarjeta inactivada correctamente"
                    };
                }

                return new RespuestaSimple
                {
                    Resultado = false,
                    Mensaje = respuesta != null && respuesta.ContainsKey("mensaje")
                        ? respuesta["mensaje"].ToString()
                        : "No fue posible inactivar la tarjeta"
                };
            }
        }
        catch
        {
            return new RespuestaSimple
            {
                Resultado = false,
                Mensaje = "Error en el autorizador"
            };
        }
    }

    public StandardResponse<bool> GuardarPersona(Personas persona)
    {
        try
        {
            // Validar nulo
            if (persona == null)
            {
                return new StandardResponse<bool>
                {
                    Resultado = false,
                    Mensaje = "No se recibieron los datos necesarios.",
                    Datos = false
                };
            }

            // Validar campos vacíos (incluye strings de solo espacios)
            if (string.IsNullOrWhiteSpace(persona.Identificacion) ||
                string.IsNullOrWhiteSpace(persona.Nombre) ||
                string.IsNullOrWhiteSpace(persona.PrimerApellido) ||
                string.IsNullOrWhiteSpace(persona.SegundoApellido) ||
                string.IsNullOrWhiteSpace(persona.Correo))
            {
                return new StandardResponse<bool>
                {
                    Resultado = false,
                    Mensaje = "Todos los campos son obligatorios.",
                    Datos = false
                };
            }

            // Limpiar espacios
            persona.Identificacion = persona.Identificacion.Trim();
            persona.Nombre = persona.Nombre.Trim();
            persona.PrimerApellido = persona.PrimerApellido.Trim();
            persona.SegundoApellido = persona.SegundoApellido.Trim();
            persona.Correo = persona.Correo.Trim();

            // Validar longitudes (según tu tabla)
            if (persona.Identificacion.Length > 25)
            {
                return new StandardResponse<bool>
                {
                    Resultado = false,
                    Mensaje = "La identificación no puede exceder 25 caracteres.",
                    Datos = false
                };
            }

            if (persona.Nombre.Length > 150 ||
                persona.PrimerApellido.Length > 150 ||
                persona.SegundoApellido.Length > 150 ||
                persona.Correo.Length > 150)
            {
                return new StandardResponse<bool>
                {
                    Resultado = false,
                    Mensaje = "Los campos de texto no pueden exceder 150 caracteres.",
                    Datos = false
                };
            }

            // Enviar al servidor
            string json = JsonConvert.SerializeObject(persona);

            using (var tcp = new ConexionTCP(ip, puerto))
            {
                string respuestaJson = tcp.EnviarYRecibir(json);
                var respuesta = JsonConvert.DeserializeObject<Dictionary<string, object>>(respuestaJson);

                if (respuesta == null || !respuesta.ContainsKey("status"))
                {
                    return new StandardResponse<bool>
                    {
                        Resultado = false,
                        Mensaje = "Respuesta inválida del servidor.",
                        Datos = false
                    };
                }

                if (respuesta["status"].ToString() == "OK")
                {
                    return new StandardResponse<bool>
                    {
                        Resultado = true,
                        Mensaje = "Persona registrada correctamente.",
                        Datos = true
                    };
                }

                // Si el servidor envió un mensaje de error, lo mostramos
                string mensajeError = respuesta.ContainsKey("mensaje")
                    ? respuesta["mensaje"].ToString()
                    : "No se ha aceptado el registro de esta persona.";

                return new StandardResponse<bool>
                {
                    Resultado = false,
                    Mensaje = mensajeError,
                    Datos = false
                };
            }
        }
        catch (Exception e)
        {
            return new StandardResponse<bool>
            {
                Resultado = false,
                Mensaje = "Error no controlado: " + e.Message,
                Datos = false
            };
        }
    }

}