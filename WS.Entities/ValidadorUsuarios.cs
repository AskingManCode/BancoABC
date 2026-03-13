using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using System.Linq;

namespace WS.Entities
{
    public class ValidadorUsuarios : AbstractValidator<Usuarios>
    {
        public ValidadorUsuarios() {

            RuleSet("CrearNuevoUsuario", () =>
            {

                // Identificacion
                RuleFor(x => x.Identificacion)
                    .NotEmpty().WithMessage("El número de identificación es obligatorio.")
                    .MinimumLength(9).WithMessage("La identificación debe contener almenos 9 numeros.")
                    .Matches(@"^[0-9]+$").WithMessage("La identificación solo debe contener números.");

                // Nombre
                RuleFor(x => x.Nombre)
                    .NotEmpty().WithMessage("El nombre es obligatorio.")
                    .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ' ]+$").WithMessage("El nombre no puede contener números ni caracteres especiales.");

                // Primer Apellido
                RuleFor(x => x.PrimerApellido)
                    .NotEmpty().WithMessage("El primer apellido es obligatorio.")
                    .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ' ]+$").WithMessage("El primer apellido no puede contener números ni caracteres especiales.");

                // Segundo Apellido
                RuleFor(x => x.SegundoApellido)
                    .NotEmpty().WithMessage("El segundo apellido es obligatorio.")
                    .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ' ]+$").WithMessage("El segundo apellido no puede contener números ni caracteres especiales.");

                // Correo 
                RuleFor(x => x.Correo)
                    .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
                    .EmailAddress().WithMessage("El formato del correo electrónico no es correcto.");

                // Usuario
                RuleFor(x => x.User)
                    .NotEmpty().WithMessage("El nombre de usuario es obligatorio");

                // Contraseña
                RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("La contraseña es obligatoria.")
                    .MinimumLength(14).WithMessage("La contraseña debe tener al menos 14 caracteres.")
                    .Matches(@"[A-Z]").WithMessage("La contraseña debe tener al menos una letra mayúscula.")
                    .Matches(@"[a-z]").WithMessage("La contraseña debe tener al menos una letra minúscula.")
                    .Matches(@"[0-9]").WithMessage("La contraseña debe tener al menos un número.")
                    .Matches(@"[^a-zA-Z0-9]").WithMessage("La contraseña debe tener al menos un carácter especial.");

                // Tipo Usuario
                RuleFor(x => x.TipoUsuario)
                    .Must(tipo => tipo == "1" || tipo == "2")
                    .WithMessage("El tipo de usuario debe ser 1 (Enpleado) o 2 (Cliente)");

                // Estado
                RuleFor(x => x.Estado)
                    .Equal(true).WithMessage("El estado debe ser 'Activo' para usuarios nuevos.");

            });
        }

        public object Validate(Usuarios newUser, Action<object> value)
        {
            throw new NotImplementedException();
        }
    }
}
