using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WS_Autenticador_BancoABC;

namespace Interfaz_Adm_Usr.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public WS_Autenticador_BancoABC.Usuarios Usuario { get; set; }

        public string Mensaje { get; set; }

        public IActionResult OnGet()
        {
            // Si ya está logeado, redirigir según su rol
            var tipoUsuario = HttpContext.Session.GetString("TipoUsuario");

            if (!string.IsNullOrEmpty(tipoUsuario))
            {
                if (tipoUsuario.Equals("1"))
                {
                    Response.Redirect("/Administrador/AdministracionClientes");
                }
                else if (tipoUsuario.Equals("2"))
                {
                    Response.Redirect("/Usuarios/ListaCuentasYTarjetas");
                }

            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                using (var clienteWS = new ServiceClient())
                {
                    // Autenticar Usuario // HTTPS ya cifra la comunicación por defecto
                    var respuesta = await clienteWS.AutenticarUsuarioAsync(Usuario);

                    if (respuesta.Resultado && respuesta.Datos != null)
                    {
                        // Guardar datos importantes en Session
                        HttpContext.Session.SetString("Identificacion", respuesta.Datos.Identificacion);
                        HttpContext.Session.SetString("TipoUsuario", respuesta.Datos.TipoUsuario);

                        // Redirección según el tipo de usuario
                        if (respuesta.Datos.TipoUsuario == "1")
                        {
                            return RedirectToPage("/Administrador/AdministracionClientes");
                        }
                        else if (respuesta.Datos.TipoUsuario == "2")
                        {
                            return RedirectToPage("/Usuarios/ListaCuentasYTarjetas");
                        }
                        else
                        {
                            Mensaje = "Rol de usuario no válido.";
                            return Page();
                        }
                    }
                    else
                    {
                        Mensaje = respuesta.Mensaje ?? "Error de autenticación.";
                        return Page();
                    }
                
                } // cierre de cliente 
            }
            catch (Exception ex)
            {
                Mensaje = "Error de conexión: " + ex.Message;
                return Page();
            }
        }

        public IActionResult OnGetLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Index");
        }

    }
}
