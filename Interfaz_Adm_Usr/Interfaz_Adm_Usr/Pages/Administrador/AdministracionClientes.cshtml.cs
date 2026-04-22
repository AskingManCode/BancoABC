using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WS_Autenticador_BancoABC;
using WS_Autorizador_BancoABC;

namespace Interfaz_Adm_Usr.Pages.Administrador
{
    public class AdministracionClientesModel : PageModel
    {
        [BindProperty]
        public Personas Cliente { get; set; }
        
        public string Modo { get; set; } = "Listado"; // Listado, Nuevo, Editar

        public List<Personas> ListaPersonas { get; set; }
        public string Mensaje { get; set; }

        public async Task<IActionResult> OnGetAsync(string modo, string identificacion, string filtro)
        {
            var tipoUsuario = HttpContext.Session.GetString("TipoUsuario");

            if (string.IsNullOrEmpty(tipoUsuario) || tipoUsuario != "1")
            {
                return RedirectToPage("/Index");
            }

            if (modo == "nuevo")
            {
                Modo = "Nuevo";
                Cliente = new Personas();
            }
            else if (modo == "editar" && !string.IsNullOrEmpty(identificacion))
            {
                Modo = "Editar";
                await ObtenerPersonaAsync(identificacion);
            }
            else
            {
                Modo = "Listado";
                await ListarPersonasAsync(filtro);
            }

            return Page();
        }

        private async Task ListarPersonasAsync(string filtro = null)
        {
            try
            {
                using (var clienteWS = new ServiceClient())
                {
                    var respuesta = await clienteWS.ListarPersonasAsync();

                    if (respuesta.Resultado && respuesta.Datos != null)
                    {
                        ListaPersonas = respuesta.Datos;
                        
                        // Filtros
                        if (!string.IsNullOrWhiteSpace(filtro))
                        {
                            filtro = filtro.ToLower();
                            ListaPersonas = ListaPersonas
                                .Where(p =>
                                    (p.Identificacion != null && p.Identificacion.ToLower().Contains(filtro)) ||
                                    (p.Nombre != null && p.Nombre.ToLower().Contains(filtro)) ||
                                    (p.PrimerApellido != null && p.PrimerApellido.ToLower().Contains(filtro)) ||
                                    (p.SegundoApellido != null && p.SegundoApellido.ToLower().Contains(filtro)))
                                .ToList();
                        }
                    }
                    else
                    {
                        Mensaje = respuesta.Mensaje;
                    }
                }
            }
            catch (Exception ex)
            {
                Mensaje = "Error de conexión: " + ex.Message;
            }
        }

        private async Task ObtenerPersonaAsync(string identificacion)
        {
            try
            {
                using (var clienteWS = new ServiceClient())
                {
                    var respuesta = await clienteWS.ListarPersonasAsync();

                    if (respuesta.Resultado && respuesta.Datos != null)
                    {
                        var persona = respuesta.Datos.FirstOrDefault(p => p.Identificacion == identificacion);

                        if (persona != null)
                        {
                            Cliente = persona;
                        }
                        else
                        {
                            Mensaje = "Cliente no encontrado.";
                        }
                    }
                    else
                    {
                        Mensaje = respuesta.Mensaje;
                    }
                }
            }
            catch (Exception ex)
            {
                Mensaje = "Error de conexión: " + ex.Message;
            }
        }

        public async Task<IActionResult> OnPostGuardarAsync(string modo)
        {
            var tipoUsuario = HttpContext.Session.GetString("TipoUsuario");

            if (string.IsNullOrEmpty(tipoUsuario) || tipoUsuario != "1")
            {
                return RedirectToPage("/Index");
            }

            Modo = modo;

            try
            {
                using (var clienteWS = new ServiceClient())
                {
                    if (Modo == "Nuevo")
                    {
                        var respuesta = await clienteWS.RegistrarPersonaAsync(Cliente);

                        if (respuesta.Resultado)
                        {
                            using (var autClient = new AutorizadorServiceClient())
                            {
                                // Aquí va el registro del cliente
                            }

                            return RedirectToPage(new { modo = (string)null, identificacion = (string)null });
                        }

                        Mensaje = respuesta.Mensaje;
                    }
                    else if (Modo == "Editar")
                    {
                        var respuesta = await clienteWS.ModificarPersonaAsync(Cliente);

                        if (respuesta.Resultado)
                        {
                            return RedirectToPage(new { modo = (string)null, identificacion = (string)null });
                        }
                         
                        Mensaje = respuesta.Mensaje;
                    }
                }
            }
            catch (Exception ex)
            {
                Mensaje = "Error: " + ex.Message;
            }

            // Si hay error, permanece en el formulario
            return Page();
        }

        public async Task<IActionResult> OnPostEliminarAsync(string identificacion)
        {
            var tipoUsuario = HttpContext.Session.GetString("TipoUsuario");
            if (string.IsNullOrEmpty(tipoUsuario) || tipoUsuario != "1")
            {
                return RedirectToPage("/Index");
            }

            try
            {
                using (var clienteWS = new ServiceClient())
                {
                    var respuesta = await clienteWS.EliminarPersonaAsync(identificacion);

                    if (respuesta.Resultado)
                    {
                        Mensaje = "Cliente eliminado correctamente.";
                    }
                    else
                    {
                        if (respuesta.Mensaje != null && respuesta.Mensaje.Contains("No es posible eliminar"))
                        {
                            Mensaje = "No es posible eliminar el usuario.";
                        }
                        else
                        {
                            Mensaje = respuesta.Mensaje ?? "No se pudo eliminar el cliente.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Mensaje = "Error: " + ex.Message;
            }

            Modo = "Listado";
            await ListarPersonasAsync();
            return Page();
        }
    }
}