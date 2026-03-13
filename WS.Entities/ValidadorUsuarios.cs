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
            RuleFor(x => x.Password);
        }
    }
}
