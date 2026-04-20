using System;
using System.Linq;
using FluentValidation;
using WS.DataAccess;
using WS.Entities;

public class Service : IService
{
    ValidadorPersonas ValidadorPersona = new ValidadorPersonas();
    DataBaseMongoDB mongoDB = new DataBaseMongoDB();
    Bitacora Bitacora = new Bitacora();
    Encriptacion Encriptacion = new Encriptacion();

    public StandardResponse<Object> AutenticarUsuario(Usuarios usuario)
    { // Recibe Usuario, Contraseña, ya no recibe rol debido a que no puede saberlo desde interfaz

        var StandardResponse = new StandardResponse<Object>();
        Personas persona = new Personas {Usuario = usuario};
        try
        {
            if (persona.Usuario == null || 
                persona.Usuario.User == null ||
                persona.Usuario.Password == null)
            {
                StandardResponse.Resultado = false;
                StandardResponse.Mensaje = "Usuario y/o constraseña incorrectos.";
                StandardResponse.Datos = null;
                return StandardResponse;
            }

            // Validaciones
            var resultado = ValidadorPersona.Validate(persona, ruleSet: "ValidarAutenticacion");

            // Encriptación
            persona.Usuario.User = Encriptacion.Cifrar(persona.Usuario.User);
            persona.Usuario.Password = Encriptacion.Cifrar(persona.Usuario.Password);

            if (!resultado.IsValid)
            {
                StandardResponse.Resultado = false;
                StandardResponse.Mensaje = resultado.Errors.First().ErrorMessage;
                StandardResponse.Datos = null;
                return StandardResponse;
            }

            // Validar en Base de datos
            // Valida Usuario, Contraseña (encriptados)
            if (!mongoDB.VerificarUsuario(persona.Usuario.User, persona.Usuario.Password))
            {
                StandardResponse.Resultado = false;
                StandardResponse.Mensaje = "Usuario y/o constraseña incorrectos.";
                StandardResponse.Datos = null;
                return StandardResponse;
            }

            if (!mongoDB.UsuarioActivo(persona.Usuario.User))
            {
                StandardResponse.Resultado = false;
                StandardResponse.Mensaje = "Esta cuenta de usuario se encuentra desactivada.";
                StandardResponse.Datos = null;
                return StandardResponse;
            }

            // Si todo sale bien
            StandardResponse.Resultado = true;
            StandardResponse.Mensaje = "Acceso autorizado.";
            StandardResponse.Datos = new
            {
                Identificacion = mongoDB.ObtenerIdentificacion(persona.Usuario.User, persona.Usuario.Password),
                TipoUsuario = mongoDB.ObtenerTipoUsuario(persona.Usuario.User, persona.Usuario.Password)
            };
            return StandardResponse;

        }
        catch (Exception e)
        {
            StandardResponse.Resultado = false;
            StandardResponse.Mensaje = "Error no controlado: " + e.Message;
            StandardResponse.Datos = null;
            return StandardResponse;
        }
        finally
        {
            var SolicitudRecibida = new
            {
                Solicitud = "Autenticar Usuario",
                User = persona.Usuario.User,
                Password = persona.Usuario.Password,
            };

            Bitacora.RegistrarActividad(SolicitudRecibida, StandardResponse);
        }

    }

    public StandardResponse<bool> RegistrarPersona(Personas newUser)
    { // Recibe todos los datos menos los datos relacionados a una cuenta

        var StandardResponse = new StandardResponse<bool>();
        try
        {
            // Objeto vacío
            if (newUser == null ||
                newUser.Identificacion == null ||
                newUser.Nombre == null || 
                newUser.PrimerApellido == null ||
                newUser.SegundoApellido == null ||
                newUser.Correo == null) 
            {
                StandardResponse.Resultado = false;
                StandardResponse.Mensaje = "No se recibieron los datos necesarios para completar el registro.";
                StandardResponse.Datos = false;
                return StandardResponse;
            }

            // Validaciones
            var resultado = ValidadorPersona.Validate(newUser, ruleSet: "ValidarPersona");

            if (!resultado.IsValid)
            {
                StandardResponse.Resultado = false;
                StandardResponse.Mensaje = resultado.Errors.First().ErrorMessage;
                StandardResponse.Datos = false;
                return StandardResponse;
            }

            // Validación de duplicados
            // Valida si no existe otra identificacion igual
            if (mongoDB.ExisteIdentificacion(newUser.Identificacion))
            {
                StandardResponse.Resultado = false;
                StandardResponse.Mensaje = "Ya existe una persona registrada con los datos proporcionados.";
                StandardResponse.Datos = false;
                return StandardResponse;
            }

            // Guarda el persona en la base de datos
            if (mongoDB.GuardarPersona(newUser)) 
            {
                StandardResponse.Resultado = true;
                StandardResponse.Mensaje = "Persona registrada correctamente.";
                StandardResponse.Datos = true;
                return StandardResponse;
            }
            else
            {
                StandardResponse.Resultado = false;
                StandardResponse.Mensaje = "No se pudieron guardar los datos en el sistema.";
                StandardResponse.Datos = false;
                return StandardResponse;
            }
            
        }
        catch (Exception e)
        {
            StandardResponse.Resultado = false;
            StandardResponse.Mensaje = "Error no controlado: " + e.Message;
            StandardResponse.Datos = false;
            return StandardResponse;
        }
        finally
        {
            var SolicitudRecibida = new
            {
                Solicitud = "Registrar Persona",
                Identificacion = newUser.Identificacion,
                Nombre = newUser.Nombre,
                PrimerApellido = newUser.PrimerApellido,
                SegundoApellido = newUser.SegundoApellido,
                Correo = newUser.Correo
            };

            Bitacora.RegistrarActividad(SolicitudRecibida, StandardResponse);
        }
        
    }

    public StandardResponse<bool> CrearUsuario(Personas persona)
    {
        var StandardResponse = new StandardResponse<bool>();
        try
        {
            // Objeto vacío
            if (persona == null ||
                persona.Identificacion == null ||
                persona.Usuario == null ||
                persona.Usuario.User == null ||
                persona.Usuario.Password == null ||
                persona.Usuario.TipoUsuario == null)
            {
                StandardResponse.Resultado = false;
                StandardResponse.Mensaje = "No se recibieron los datos necesarios para completar el registro.";
                StandardResponse.Datos = false;
                return StandardResponse;
            }

            // Validaciones
            var resultado = ValidadorPersona.Validate(persona, ruleSet: "ValidarUsuario");

            // Encriptación
            persona.Usuario.User = Encriptacion.Cifrar(persona.Usuario.User);
            persona.Usuario.Password = Encriptacion.Cifrar(persona.Usuario.Password);

            if (!resultado.IsValid)
            {
                StandardResponse.Resultado = false;
                StandardResponse.Mensaje = resultado.Errors.First().ErrorMessage;
                StandardResponse.Datos = false;
                return StandardResponse;
            }

            // Valida que la persona exista por su cédula
            if (!mongoDB.ExisteIdentificacion(persona.Identificacion))
            {
                StandardResponse.Resultado = false;
                StandardResponse.Mensaje = "No existe una persona registrada con la identificación proporcionada.";
                StandardResponse.Datos = false;
                return StandardResponse;
            }

            // verifica que esta persona no tenga un usuario registrado
            if (mongoDB.PersonaTieneUsuario(persona.Identificacion))
            {
                StandardResponse.Resultado = false;
                StandardResponse.Mensaje = "Esta persona ya cuenta con un usuario registrado.";
                StandardResponse.Datos = false;
                return StandardResponse;
            }

            // Valida que el nombre de usuario no sea el mismo que el de otra persona
            if (mongoDB.ExisteUsuario(persona.Usuario.User))
            {
                StandardResponse.Resultado = false;
                StandardResponse.Mensaje = "El nombre de usuario ya está en uso. Por favor elija otro."; 
                StandardResponse.Datos = false;
                return StandardResponse;
            }

            // Guarda el usuario en la base de datos
            if (mongoDB.GuardarUsuario(persona))
            {
                StandardResponse.Resultado = true;
                StandardResponse.Mensaje = "Usuario registrado correctamente.";
                StandardResponse.Datos = true;
                return StandardResponse;
            }
            else
            {
                StandardResponse.Resultado = false;
                StandardResponse.Mensaje = "No se pudieron guardar los datos en el sistema.";
                StandardResponse.Datos = false;
                return StandardResponse;
            }

        }
        catch (Exception e)
        {
            StandardResponse.Resultado = false;
            StandardResponse.Mensaje = "Error no controlado: " + e.Message;
            StandardResponse.Datos = false;
            return StandardResponse;
        }
        finally
        {
            var SolicitudRecibida = new
            {
                Solicitud = "Crear Usuario",
                Identificacion = persona.Identificacion,
                Usuario = persona.Usuario
            };

            Bitacora.RegistrarActividad(SolicitudRecibida, StandardResponse);
        }
        
    }

    /* public StandardResponse<bool> ModificarUsuario(Personas usuario)
    { // Recibe Identificacion, nombre, primer apellido, segundo apellido, correo, usuario y contraseña

        var StandardResponse = new StandardResponse<bool>();
        try
        {
            if (usuario == null ||
                usuario.Identificacion == null ||
                usuario.Nombre == null ||
                usuario.PrimerApellido == null ||
                usuario.SegundoApellido == null ||
                usuario.Correo == null ||
                usuario.User == null ||
                usuario.Password == null)
            {
                StandardResponse.Resultado = false;
                StandardResponse.Mensaje = "No se recibieron todos los datos del usuario.";
                StandardResponse.Datos = false;
                return StandardResponse;
            }

            // Validaciones
            var resultado = ValidadorPersona.Validate(usuario, ruleSet: "ValidarModificacionPersona");

            usuario.User = Encriptacion.Cifrar(usuario.User); 
            usuario.Password = Encriptacion.Cifrar(usuario.Password); 

            if (!resultado.IsValid)
            {
                StandardResponse.Resultado = false;
                StandardResponse.Mensaje = resultado.Errors.First().ErrorMessage;
                StandardResponse.Datos = false;
                return StandardResponse;
            }

            // Validar que exista y que cumpla con los datos correctos
            // Identificación y Nombre de Usuario no deberían cambiarse porque son únicos por usuario
            if (!mongoDB.CompararIdentificacion(usuario.Identificacion) || !mongoDB.CompararUsuario(usuario.User)) // false, osea que no existe
            {
                StandardResponse.Resultado = false;
                StandardResponse.Mensaje = "No existe un usuario registrado con los datos proporcionados.";
                StandardResponse.Datos = false;
                return StandardResponse;
            }

            if (mongoDB.ModificarUsuario(usuario))
            {
                StandardResponse.Resultado = true;
                StandardResponse.Mensaje = "Usuario modificado correctamente.";
                StandardResponse.Datos = true;
                return StandardResponse;
            }
            else
            {
                StandardResponse.Resultado = false;
                StandardResponse.Mensaje = "No se detectaron cambios en la información del usuario o no se encontró el registro.";
                StandardResponse.Datos = false;
                return StandardResponse;
            }

        }
        catch (Exception e)
        {
            StandardResponse.Resultado = false;
            StandardResponse.Mensaje = "Error no controlado: " + e.Message;
            StandardResponse.Datos = false;
            return StandardResponse;
        }
        finally
        {
            var SolicitudRecibida = new
            {
                Solicitud = "Modificar Usuario",
                Identificacion = usuario.Identificacion,
                Nombre = usuario.Nombre,
                PrimerApellido = usuario.PrimerApellido,
                SegundoApellido = usuario.SegundoApellido,
                Correo = usuario.Correo,
                User = usuario.User,
                Password = usuario.Password
            };

            Bitacora.RegistrarActividad(SolicitudRecibida, StandardResponse);
        }
    } */

    /* public StandardResponse<bool> ModificarEstadoUsuario(Personas usuario)
    { // Recibe identificacion y estado

        var StandardResponse = new StandardResponse<bool>();
        try
        {
            if (usuario == null ||
                usuario.Identificacion == null)
            {
                StandardResponse.Resultado = false;
                StandardResponse.Mensaje = "No se recibieron todos los datos del usuario.";
                StandardResponse.Datos = false;
                return StandardResponse;
            }

            // Validaciones 
            var resultado = ValidadorPersona.Validate(usuario, ruleSet: "Identificacion");

            if (!resultado.IsValid)
            {
                StandardResponse.Resultado = false;
                StandardResponse.Mensaje = resultado.Errors.First().ErrorMessage;
                StandardResponse.Datos = false;
                return StandardResponse;
            }

            // Validar que tenga un usuario y que exista
            if (!mongoDB.CompararID(usuario.Identificacion)) // false, osea no existe
            {
                StandardResponse.Resultado = false;
                StandardResponse.Mensaje = "No existe un usuario registrado con los datos proporcionados.";
                StandardResponse.Datos = false;
                return StandardResponse;
            }

            // Modificar
            if (mongoDB.ModificarEstadoUsuario(usuario))
            {
                StandardResponse.Resultado = true;
                StandardResponse.Mensaje = usuario.Estado ? "Usuario activado correctamente." : "Usuario desactivado correctamente.";
                StandardResponse.Datos = true;
                return StandardResponse;
            }
            else
            {
                StandardResponse.Resultado = false;
                StandardResponse.Mensaje = "No se detectaron cambios en el estado del usuario o no se encontró el registro.";
                StandardResponse.Datos = false;
                return StandardResponse;
            }
            
        } 
        catch (Exception e)
        {
            StandardResponse.Resultado = false;
            StandardResponse.Mensaje = "Error no controlado: " + e.Message;
            StandardResponse.Datos = false;
            return StandardResponse;
        }
        finally
        {
            var SolicitudRecibida = new
            {
                Solicitud = "Modificar Estado Usuario",
                Identificacion = usuario.Identificacion,
                Estado = usuario.Estado
            };

            Bitacora.RegistrarActividad(SolicitudRecibida, StandardResponse);
        }

    } */

}
