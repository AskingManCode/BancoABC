using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WS_Autenticador_BancoABC;
using WS_Autorizador_BancoABC;

namespace Interfaz_Adm_Usr.Pages.Administrador
{
    public class RegistrarTarjetasClientesModel : PageModel
    {
        [BindProperty]
        public NuevaTarjetaInput NuevaTarjeta { get; set; } = new();

        [BindProperty]
        public string FiltroIdentificacion { get; set; } = string.Empty;

        public List<Personas> PersonasDisponibles { get; set; } = new();
        public List<CuentaItem> CuentasDisponibles { get; set; } = new();
        public List<TarjetaFila> ListaTarjetas { get; set; } = new();

        [TempData]
        public string? MensajeExito { get; set; }

        [TempData]
        public string? MensajeError { get; set; }

        public class NuevaTarjetaInput
        {
            [Required(ErrorMessage = "Debe seleccionar una identificación.")]
            public string Identificacion { get; set; } = string.Empty;

            public string NombreCliente { get; set; } = string.Empty;

            [Required(ErrorMessage = "Debe seleccionar el tipo de tarjeta.")]
            public string TipoTarjeta { get; set; } = string.Empty;

            public string NumeroCuenta { get; set; } = string.Empty;
            public string NumeroTarjeta { get; set; } = string.Empty;
            public string Pin { get; set; } = string.Empty;
            public string Cvv { get; set; } = string.Empty;
            public string FechaVencimiento { get; set; } = string.Empty;
        }

        public class TarjetaFila
        {
            public string NumeroTarjeta { get; set; } = string.Empty;
            public string Identificacion { get; set; } = string.Empty;
            public string NombreCliente { get; set; } = string.Empty;
            public string TipoTarjeta { get; set; } = string.Empty;
            public string NumeroCuenta { get; set; } = string.Empty;
            public bool EstadoActiva { get; set; }
        }

        public class CuentaItem
        {
            public string Id { get; set; } = string.Empty;
            public string Texto { get; set; } = string.Empty;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!UsuarioEsAdministrador())
                return RedirectToPage("/Index");

            await CargarPersonasAsync();
            CargarCuentasQuemadas();
            return Page();
        }

        public async Task<IActionResult> OnPostSeleccionarPersonaAsync()
        {
            if (!UsuarioEsAdministrador())
                return RedirectToPage("/Index");

            await CargarPersonasAsync();
            CargarCuentasQuemadas();

            if (!string.IsNullOrWhiteSpace(NuevaTarjeta.Identificacion))
            {
                var persona = PersonasDisponibles.FirstOrDefault(p => p.Identificacion == NuevaTarjeta.Identificacion);
                if (persona != null)
                {
                    NuevaTarjeta.NombreCliente = $"{persona.Nombre} {persona.PrimerApellido} {persona.SegundoApellido}".Trim();
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostGenerarAsync()
        {
            if (!UsuarioEsAdministrador())
                return RedirectToPage("/Index");

            await CargarPersonasAsync();
            CargarCuentasQuemadas();

            if (string.IsNullOrWhiteSpace(NuevaTarjeta.Identificacion))
            {
                MensajeError = "Debe seleccionar una identificación.";
                return Page();
            }

            if (string.IsNullOrWhiteSpace(NuevaTarjeta.TipoTarjeta))
            {
                MensajeError = "Debe seleccionar el tipo de tarjeta.";
                return Page();
            }

            var persona = PersonasDisponibles.FirstOrDefault(p => p.Identificacion == NuevaTarjeta.Identificacion);
            if (persona != null)
            {
                NuevaTarjeta.NombreCliente = $"{persona.Nombre} {persona.PrimerApellido} {persona.SegundoApellido}".Trim();
            }

            string prefijo = NuevaTarjeta.TipoTarjeta.Equals("Debito", StringComparison.OrdinalIgnoreCase)
                ? "911111"
                : "911112";

            NuevaTarjeta.NumeroTarjeta = GenerarNumeroTarjeta(prefijo);
            NuevaTarjeta.Pin = GenerarPin();
            NuevaTarjeta.Cvv = GenerarCvv();
            NuevaTarjeta.FechaVencimiento = DateTime.Today.AddYears(4).ToString("yyyy-MM-dd");

            MensajeExito = "Datos de la tarjeta generados correctamente.";
            return Page();
        }

        public async Task<IActionResult> OnPostCrearAsync()
        {
            if (!UsuarioEsAdministrador())
                return RedirectToPage("/Index");

            await CargarPersonasAsync();
            CargarCuentasQuemadas();

            var persona = PersonasDisponibles.FirstOrDefault(p => p.Identificacion == NuevaTarjeta.Identificacion);
            if (persona != null)
            {
                NuevaTarjeta.NombreCliente = $"{persona.Nombre} {persona.PrimerApellido} {persona.SegundoApellido}".Trim();
            }

            if (!ModelState.IsValid)
                return Page();

            if (string.IsNullOrWhiteSpace(NuevaTarjeta.NumeroTarjeta) ||
                string.IsNullOrWhiteSpace(NuevaTarjeta.Pin) ||
                string.IsNullOrWhiteSpace(NuevaTarjeta.Cvv) ||
                string.IsNullOrWhiteSpace(NuevaTarjeta.FechaVencimiento))
            {
                MensajeError = "Primero debe generar los datos de la tarjeta.";
                return Page();
            }

            if (NuevaTarjeta.TipoTarjeta.Equals("Debito", StringComparison.OrdinalIgnoreCase) &&
                string.IsNullOrWhiteSpace(NuevaTarjeta.NumeroCuenta))
            {
                MensajeError = "Debe seleccionar una cuenta para la tarjeta de débito.";
                return Page();
            }

            try
            {
                var ws = new AutorizadorServiceClient();

                var respuesta = await ws.CrearTarjetaAsync(
                    NuevaTarjeta.Identificacion,
                    NuevaTarjeta.TipoTarjeta,
                    NuevaTarjeta.NumeroCuenta ?? "",
                    NuevaTarjeta.NumeroTarjeta,
                    NuevaTarjeta.Pin,
                    NuevaTarjeta.Cvv,
                    NuevaTarjeta.FechaVencimiento
                );

                if (respuesta.Resultado)
                {
                    MensajeExito = "Tarjeta creada correctamente.";
                    FiltroIdentificacion = NuevaTarjeta.Identificacion;
                    await CargarTarjetasAdmAsync(FiltroIdentificacion);
                    LimpiarFormulario();
                    return Page();
                }

                MensajeError = string.IsNullOrWhiteSpace(respuesta.Mensaje)
                    ? "No fue posible crear la tarjeta."
                    : respuesta.Mensaje;

                return Page();
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al crear la tarjeta: {ex.Message}";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostBuscarAsync()
        {
            if (!UsuarioEsAdministrador())
                return RedirectToPage("/Index");

            await CargarPersonasAsync();
            CargarCuentasQuemadas();

            if (string.IsNullOrWhiteSpace(FiltroIdentificacion))
            {
                MensajeError = "Debe indicar una identificación para buscar.";
                return Page();
            }

            await CargarTarjetasAdmAsync(FiltroIdentificacion.Trim());
            return Page();
        }

        public async Task<IActionResult> OnPostInactivarAsync(string numeroTarjeta, string identificacion)
        {
            if (!UsuarioEsAdministrador())
                return RedirectToPage("/Index");

            await CargarPersonasAsync();
            CargarCuentasQuemadas();

            try
            {
                var ws = new AutorizadorServiceClient();
                var respuesta = await ws.InactivarTarjetaAsync(numeroTarjeta);

                if (respuesta.Resultado)
                {
                    MensajeExito = "Tarjeta inactivada correctamente.";
                }
                else
                {
                    MensajeError = string.IsNullOrWhiteSpace(respuesta.Mensaje)
                        ? "No fue posible inactivar la tarjeta."
                        : respuesta.Mensaje;
                }

                FiltroIdentificacion = identificacion;
                if (!string.IsNullOrWhiteSpace(FiltroIdentificacion))
                {
                    await CargarTarjetasAdmAsync(FiltroIdentificacion);
                }

                return Page();
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al inactivar la tarjeta: {ex.Message}";
                return Page();
            }
        }

        private async Task CargarPersonasAsync()
        {
            try
            {
                var client = new ServiceClient();
                var resp = await client.ListarPersonasAsync();
                PersonasDisponibles = resp?.Datos?.OrderBy(p => p.Identificacion).ToList() ?? new List<Personas>();
            }
            catch
            {
                PersonasDisponibles = new List<Personas>();
            }
        }

        private void CargarCuentasQuemadas()
        {
            CuentasDisponibles = new List<CuentaItem>
            {
                new CuentaItem { Id = "1", Texto = "Cuenta 1" },
                new CuentaItem { Id = "2", Texto = "Cuenta 2" },
                new CuentaItem { Id = "3", Texto = "Cuenta 3" }
            };
        }

        private async Task CargarTarjetasAdmAsync(string identificacion)
        {
            try
            {
                var ws = new AutorizadorServiceClient();
                var tarjetas = await ws.ObtenerTarjetasADMAsync(identificacion);

                var persona = PersonasDisponibles.FirstOrDefault(p => p.Identificacion == identificacion);
                string nombreCompleto = persona == null
                    ? string.Empty
                    : $"{persona.Nombre} {persona.PrimerApellido} {persona.SegundoApellido}".Trim();

                ListaTarjetas = (tarjetas ?? new List<TarjetaInfo>())
                    .Where(t => !string.IsNullOrWhiteSpace(t.NumeroTarjeta))
                    .Select(t => new TarjetaFila
                    {
                        NumeroTarjeta = t.NumeroTarjeta ?? string.Empty,
                        Identificacion = identificacion,
                        NombreCliente = nombreCompleto,
                        TipoTarjeta = t.TipoTarjeta ?? string.Empty,
                        NumeroCuenta = t.NumeroCuenta ?? string.Empty,
                        EstadoActiva = t.Estado
                    })
                    .ToList();
            }
            catch
            {
                ListaTarjetas = new List<TarjetaFila>();
            }
        }

        private static string GenerarNumeroTarjeta(string prefijo)
        {
            var random = new Random();
            string restante = string.Concat(Enumerable.Range(0, 10).Select(_ => random.Next(0, 10).ToString()));
            return prefijo + restante;
        }

        private static string GenerarPin()
        {
            var random = new Random();
            return random.Next(1000, 10000).ToString();
        }

        private static string GenerarCvv()
        {
            var random = new Random();
            return random.Next(100, 1000).ToString();
        }

        private void LimpiarFormulario()
        {
            NuevaTarjeta = new NuevaTarjetaInput();
        }

        private bool UsuarioEsAdministrador()
        {
            string tipoUsuario = HttpContext.Session.GetString("TipoUsuario") ?? "";
            return tipoUsuario == "1";
        }
    }
}