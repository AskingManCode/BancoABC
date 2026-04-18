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

        public IActionResult OnPost()
        {
            var clienteWS = new ServiceClient();

            var respuesta = clienteWS.AutenticarUsuarioAsync(Usuario);

            if (respuesta.Resultado && respuesta.Datos != null)
            {
                // Guardar datos en Session
                HttpContext.Session.SetString("Identificacion", respuesta.Datos.Identificacion);
                HttpContext.Session.SetString("TipoUsuario", respuesta.Datos.TipoUsuario);
            }
        }

    }
}
