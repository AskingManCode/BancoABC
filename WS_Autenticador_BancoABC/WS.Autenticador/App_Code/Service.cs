using System;
using System.Linq;
using FluentValidation;
using WS.DataAccess;
using WS.Entities;

/* Datos del Usuario
 * string identificacion, -- PK
 * string nombre,
 * string primerApellido,
 * string segundoApellido,
 * string correo,
 * string user, -- PK
 * string password,
 * string tipoUsuario,
 * Boolean activo = true 
*/

public class Service : IService
{
    ValidadorUsuarios UserValidator = new ValidadorUsuarios();
    DataBaseMongoDB mongoDB = new DataBaseMongoDB();
    Bitacora Bitacora = new Bitacora();

    Encriptacion EncriptacionParaEntregable = new Encriptacion();

    public StandardResponse<Usuarios> AutenticarUsuario(Usuarios usuario)
    { // Recibe Usuario, Contraseña, ya no recibe rol sino como va a saber que rol tiene

        var StandardResponse = new StandardResponse<Usuarios>();
        try
        {
            if (usuario == null || 
                usuario.Password == null)
            {
                StandardResponse.Resultado = false;
                StandardResponse.Mensaje = "No se recibieron todos los datos del usuario.";
                StandardResponse.Datos = null;
                return StandardResponse;
            }

            /* Validaciones
            var resultado = UserValidator.Validate(usuario, ruleSet: "ValidarAutenticacion");

            usuario.User = EncriptacionParaEntregable.Cifrar(usuario.User);
            usuario.Password = EncriptacionParaEntregable.Cifrar(usuario.Password);

            if (!resultado.IsValid)
            {
                StandardResponse.Resultado = false;
                StandardResponse.Mensaje = resultado.Errors.First().ErrorMessage;
                StandardResponse.Datos = null;
                return StandardResponse;
            }*/

            // Validar en Base de datos
            // Valida Usuario, Contraseña (encriptados)
            if (!mongoDB.VerificarUsuario(usuario.User, usuario.Password))
            {
                StandardResponse.Resultado = false;
                StandardResponse.Mensaje = "Usuario y/o constraseña incorrectos.";
                StandardResponse.Datos = null;
                return StandardResponse;
            }

            if (!mongoDB.VerificarUsuarioActivo(usuario.User))
            {
                StandardResponse.Resultado = false;
                StandardResponse.Mensaje = "Esta cuenta de usuario se encuentra desactivada.";
                StandardResponse.Datos = null;
                return StandardResponse;
            }

            /*if (!mongoDB.VerificarRolUsuario(usuario.User, usuario.TipoUsuario))
            {
                StandardResponse.Resultado = false;
                StandardResponse.Mensaje = "El usuario no tiene permisos para el rol indicado.";
                StandardResponse.Datos = null;
                return StandardResponse;
            }*/

            // Si todo sale bien
            StandardResponse.Resultado = true;
            StandardResponse.Mensaje = "Acceso autorizado.";
            StandardResponse.Datos = new Usuarios
            {
                Identificacion = mongoDB.ObtenerID(usuario.User, usuario.Password),
                TipoUsuario = mongoDB.ObtenerTipoUsuario(usuario.User, usuario.Password)
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
                User = usuario.User,
                Password = usuario.Password,
                TipoUsuario = usuario.TipoUsuario
            };

            Bitacora.RegistrarActividad(SolicitudRecibida, StandardResponse);
        }

    }

    public StandardResponse<bool> CrearNuevoUsuario(Usuarios newUser)
    { // Recibe todos los datos

        var StandardResponse = new StandardResponse<bool>();
        try
        {
            // Objeto vacío
            if (newUser == null ||
                newUser.Identificacion == null ||
                newUser.Nombre == null ||
                newUser.PrimerApellido == null ||
                newUser.SegundoApellido == null ||
                newUser.Correo == null ||
                newUser.User == null ||
                newUser.Password == null ||
                newUser.TipoUsuario == null) 
            {
                StandardResponse.Resultado = false;
                StandardResponse.Mensaje = "No se recibieron todos los datos del usuario.";
                StandardResponse.Datos = false;
                return StandardResponse;
            }

            // Validaciones
            var resultado = UserValidator.Validate(newUser, ruleSet: "ValidarNuevoUsuario");


            /////////////////////////////    TEMPORAL PARA PRUEBAS Y PRESENTACIÓN    ///////////////////////////////////////
            /**/ newUser.User = EncriptacionParaEntregable.Cifrar(newUser.User);                                         //
            /**/ newUser.Password = EncriptacionParaEntregable.Cifrar(newUser.Password);                                //
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////


            if (!resultado.IsValid)
            {
                StandardResponse.Resultado = false;
                StandardResponse.Mensaje = resultado.Errors.First().ErrorMessage;
                StandardResponse.Datos = false;
                return StandardResponse;
            }

            // Validación de duplicados
            // Valida si no existe otra identificacion o nombre de usuario igual
            if (mongoDB.CompararID(newUser.Identificacion) || mongoDB.CompararUsuario(newUser.User))
            {
                StandardResponse.Resultado = false;
                StandardResponse.Mensaje = "Ya existe un usuario registrado con los datos proporcionados.";
                StandardResponse.Datos = false;
                return StandardResponse;
            }

            // Guarda el usuario en la base de datos
            if (mongoDB.GuardarDatos(newUser))
            {
                StandardResponse.Resultado = true;
                StandardResponse.Mensaje = "Usuario creado correctamente.";
                StandardResponse.Datos = true;
                return StandardResponse;
            }
            else
            {
                StandardResponse.Resultado = false;
                StandardResponse.Mensaje = "No se pudo guardar el usuario en el sistema.";
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
                Solicitud = "Crear Nuevo Usuario",
                Identificacion = newUser.Identificacion,
                Nombre = newUser.Nombre,
                PrimerApellido = newUser.PrimerApellido,
                SegundoApellido = newUser.SegundoApellido,
                Correo = newUser.Correo,
                User = newUser.User,
                Password = newUser.Password,
                TipoUsuario = newUser.TipoUsuario,
                Estado = newUser.Estado
            };

            Bitacora.RegistrarActividad(SolicitudRecibida, StandardResponse);
        }
        
    }

    public StandardResponse<bool> ModificarUsuario(Usuarios usuario)
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
            var resultado = UserValidator.Validate(usuario, ruleSet: "ValidarModificacion");


            /////////////////////////////    TEMPORAL PARA PRUEBAS Y PRESENTACIÓN    ///////////////////////////////////////
            /**/ usuario.User = EncriptacionParaEntregable.Cifrar(usuario.User);                                         //
            /**/ usuario.Password = EncriptacionParaEntregable.Cifrar(usuario.Password);                                //
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////


            if (!resultado.IsValid)
            {
                StandardResponse.Resultado = false;
                StandardResponse.Mensaje = resultado.Errors.First().ErrorMessage;
                StandardResponse.Datos = false;
                return StandardResponse;
            }

            // Validar que exista y que cumpla con los datos correctos
            // Identificación y Nombre de Usuario no deberían cambiarse porque son únicos por usuario
            if (!mongoDB.CompararID(usuario.Identificacion) || !mongoDB.CompararUsuario(usuario.User)) // false, osea que no existe
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
    }

    public StandardResponse<bool> ModificarEstadoUsuario(Usuarios usuario)
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
            var resultado = UserValidator.Validate(usuario, ruleSet: "Identificacion");

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

    }

}
