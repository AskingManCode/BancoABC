using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WS_Autenticador_BancoABC;

namespace Interfaz_Adm_Usr.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public WS_Autenticador_BancoABC.Usuarios Usuario { get; set; }

        public string MensajeError { get; set; }

        public void OnGet()
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
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var clienteWS = new ServiceClient();

                // Autenticar Usuario // HTTPS ya cifra la comunicación por defecto
                var respuesta = await clienteWS.AutenticarUsuarioAsync(Usuario); 

                if (respuesta.Resultado && respuesta.Datos != null)
                {
                    var datos = respuesta.Datos as Dictionary<string, string>;

                    // Guardar datos importantes a Session
                    HttpContext.Session.SetString("Identificacion", datos["Identificacion"]);
                    HttpContext.Session.SetString("TipoUsuario", datos["TipoUsuario"]);

                    // Redirigir según el tipo de usuario
                    if (datos["TipoUsuario"] == "1")
                    {
                        return RedirectToPage("/Administrador/AdministracionClientes");
                    }
                    else if (datos["TipoUsuario"] == "2")
                    {
                        return RedirectToPage("/Usuarios/ListaCuentasYTarjetas");
                    }
                    else
                    {
                        MensajeError = "Rol de usuario no válido.";
                        return Page();
                    }
                }
                else
                {
                    MensajeError = respuesta.Mensaje ?? "Error de autenticación.";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                MensajeError = "Error de conexión: " + ex.Message;
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
