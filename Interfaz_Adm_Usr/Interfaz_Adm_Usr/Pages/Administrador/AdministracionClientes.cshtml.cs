using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WS_Autenticador_BancoABC;

namespace Interfaz_Adm_Usr.Pages.Administrador
{
    public class AdministracionClientesModel : PageModel
    {
        [BindProperty]
        public WS_Autenticador_BancoABC.Personas personas { get; set; }

        public string Mensaje { get; set; }

        public IActionResult OnGet()
        {
            // Verifica si hay sesión
            var tipoUsuario = HttpContext.Session.GetString("TipoUsuario");

            if (string.IsNullOrEmpty(tipoUsuario))
            {
                // No hay login, devolver al login
                return RedirectToPage("/Index");
            }

            // Verificar que sea Administrador
            if (tipoUsuario != "1")
            {
                // No hay login, devolver al login
                return RedirectToPage("/Index");
            }

            return Page();
        }
    }
}
