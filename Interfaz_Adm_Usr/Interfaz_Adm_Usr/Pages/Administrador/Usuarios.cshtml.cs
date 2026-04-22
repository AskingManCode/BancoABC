using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WS_Autenticador_BancoABC;

namespace Interfaz_Adm_Usr.Pages.Administrador
{
    public class UsuariosModel : PageModel
    {
        [BindProperty]
        public NuevoUsuarioInput NuevoUsuario { get; set; } = new();

        public List<UsuarioFila> ListaUsuarios { get; set; } = new();
        public List<Personas> PersonasDisponibles { get; set; } = new();

        [TempData]
        public string? MensajeExito { get; set; }

        [TempData]
        public string? MensajeError { get; set; }

        public class NuevoUsuarioInput
        {
            [Required(ErrorMessage = "Debe seleccionar una identificación.")]
            public string Identificacion { get; set; } = string.Empty;

            [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
            public string User { get; set; } = string.Empty;

            [Required(ErrorMessage = "La contraseña es obligatoria.")]
            public string Password { get; set; } = string.Empty;

            [Required(ErrorMessage = "Debe seleccionar el tipo de usuario.")]
            public string TipoUsuario { get; set; } = string.Empty;
        }

        public class UsuarioFila
        {
            public string User { get; set; } = string.Empty;
            public string TipoUsuario { get; set; } = string.Empty;
            public bool Estado { get; set; }

            public string TipoUsuarioTexto =>
                TipoUsuario == "1" ? "Administrador" :
                TipoUsuario == "2" ? "Cliente" : TipoUsuario;

            public string EstadoTexto => Estado ? "Activo" : "Inactivo";
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!UsuarioEsAdministrador())
                return RedirectToPage("/Index");

            await CargarDatosAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostCrearAsync()
        {
            if (!UsuarioEsAdministrador())
                return RedirectToPage("/Index");

            if (!ModelState.IsValid)
            {
                await CargarDatosAsync();
                return Page();
            }

            try
            {
                var client = new ServiceClient();

                var usuarioWs = new WS_Autenticador_BancoABC.Usuarios
                {
                    User = NuevoUsuario.User,
                    Password = NuevoUsuario.Password,
                    TipoUsuario = NuevoUsuario.TipoUsuario,
                    Estado = true
                };

                var respuesta = await client.CrearUsuarioAsync(NuevoUsuario.Identificacion, usuarioWs);

                if (respuesta.Resultado)
                {
                    MensajeExito = "Usuario creado correctamente.";
                    return RedirectToPage();
                }

                MensajeError = string.IsNullOrWhiteSpace(respuesta.Mensaje)
                    ? "No fue posible crear el usuario."
                    : respuesta.Mensaje;

                await CargarDatosAsync();
                return Page();
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al crear el usuario: {ex.Message}";
                await CargarDatosAsync();
                return Page();
            }
        }

        public async Task<IActionResult> OnPostCambiarEstadoAsync(string user, string tipoUsuario, bool estadoActual)
        {
            if (!UsuarioEsAdministrador())
                return RedirectToPage("/Index");

            try
            {
                var client = new ServiceClient();

                var usuarioWs = new WS_Autenticador_BancoABC.Usuarios
                {
                    User = user,
                    TipoUsuario = tipoUsuario,
                    Estado = !estadoActual
                };

                var respuesta = await client.ModificarEstadoUsuarioAsync(usuarioWs);

                if (respuesta.Resultado)
                {
                    MensajeExito = estadoActual
                        ? "Usuario inactivado correctamente."
                        : "Usuario activado correctamente.";
                }
                else
                {
                    MensajeError = string.IsNullOrWhiteSpace(respuesta.Mensaje)
                        ? "No fue posible cambiar el estado del usuario."
                        : respuesta.Mensaje;
                }
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al cambiar el estado: {ex.Message}";
            }

            return RedirectToPage();
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                var client = new ServiceClient();

                var respuestaUsuarios = await client.ListarUsuariosAsync();
                var respuestaPersonas = await client.ListarPersonasAsync();

                PersonasDisponibles = (respuestaPersonas?.Datos ?? new List<Personas>())
                    .OrderBy(p => p.Identificacion)
                    .ToList();

                ListaUsuarios = (respuestaUsuarios?.Datos ?? new List<WS_Autenticador_BancoABC.Usuarios>())
                    .Select(u => new UsuarioFila
                    {
                        User = u.User ?? string.Empty,
                        TipoUsuario = u.TipoUsuario ?? string.Empty,
                        Estado = u.Estado
                    })
                    .OrderBy(x => x.User)
                    .ToList();
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al cargar los usuarios: {ex.Message}";
                ListaUsuarios = new List<UsuarioFila>();
                PersonasDisponibles = new List<Personas>();
            }
        }

        private bool UsuarioEsAdministrador()
        {
            string tipoUsuario = HttpContext.Session.GetString("TipoUsuario") ?? "";
            return tipoUsuario == "1";
        }
    }
}