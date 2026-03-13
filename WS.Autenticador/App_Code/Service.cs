using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WS.DataAccess;
using WS.Entities;

/*string identificacion, -- PK
string nombre,
string primerApellido,
string segundoApellido,
string correo,
string user, -- PK
string password,
string tipoUsuario,
Boolean activo = true*/

public class Service : IService
{
    ValidadorUsuarios UserValidator = new ValidadorUsuarios();
    DataBaseMongoDB mongoDB = new DataBaseMongoDB();

    public StandardResponse<Usuarios> AutenticarUsuario(Usuarios usuario)
    { // Recibe Usuario, Contraseña y Rol

        try
        {
            if (usuario == null)
            {
                return new StandardResponse<Usuarios>
                {
                    Resultado = false,
                    Mensaje = "No se recibieron datos del usuario.",
                    Datos = null
                };
            }

            // Validaciones
            var resultado = UserValidator.Validate(usuario, ruleSet: "ValidarAutenticacion");

            if (!resultado.IsValid)
            {
                return new StandardResponse<Usuarios>
                {
                    Resultado = false,
                    Mensaje = resultado.Errors.First().ErrorMessage,
                    Datos = null
                };
            }

            // Validar en Base de datos
            // Valida Usuario, Contraseña (encriptados)
            if (!mongoDB.VerificarUsuario(usuario.User, usuario.Password))
            {
                return new StandardResponse<Usuarios>
                {
                    Resultado = false,
                    Mensaje = "Usuario y/o constraseña incorrectos.",
                    Datos = null
                };
            }

            // Si todo sale bien
            return new StandardResponse<Usuarios>
            {
                Resultado = true,
                Mensaje = "Acceso autorizado.",
                Datos = new Usuarios
                {
                    TipoUsuario = mongoDB.ObtenerRolUsuario(usuario.User)
                }
            };

        }
        catch (Exception e)
        {
            return new StandardResponse<Usuarios>
            {
                Resultado = false,
                Mensaje = "Error no controlado: " + e.Message,
                Datos = null
            };
        }

    }

    public StandardResponse<bool> CrearNuevoUsuario(Usuarios newUser)
    { // Recibe todos los datos

        try
        {
            // Objeto vacío
            if (newUser == null) 
            {
                return new StandardResponse<bool>
                {
                    Resultado = false,
                    Mensaje = "No se recibieron datos del usuario.",
                    Datos = false
                };
            }

            // Validaciones
            var resultado = UserValidator.Validate(newUser, ruleSet: "ValidarNuevoUsuario");

            if (!resultado.IsValid)
            {
                return new StandardResponse<bool>
                {
                    Resultado = false,
                    Mensaje = resultado.Errors.First().ErrorMessage,
                    Datos = false
                };
            
            }

            // Validación de duplicados
            // Valida si no existe otra identificacion o nombre de usuario igual
            if (mongoDB.CompararID(newUser.Identificacion) || mongoDB.CompararUsuario(newUser.User))
            {
                return new StandardResponse<bool>
                {
                    Resultado = false,
                    Mensaje = "Ya existe un usuario registrado con los datos proporcionados.",
                    Datos = false
                };
            }

            // Guarda el usuario en la base de datos
            if (mongoDB.GuardarDatos(newUser))
            {
                return new StandardResponse<bool>
                {
                    Resultado = true,
                    Mensaje = "Usuario creado correctamente.",
                    Datos = true
                };
            }
            else
            {
                return new StandardResponse<bool>
                {
                    Resultado = false,
                    Mensaje = "No se pudo guardar el usuario en el sistema.",
                    Datos = false
                };
            }
            
        }
        catch (Exception e)
        {
            return new StandardResponse<bool>
            {
                Resultado = false,
                Mensaje = "Error no controlado: " + e.Message,
                Datos = false
            };
        }
        
    }

    public StandardResponse<bool> ModificarUsuario(Usuarios usuario)
    { // Recibe Identificacion, nombre, primer apellido, segundo apellido, correo, usuario y contraseña

        try
        {
            if (usuario == null)
            {
                return new StandardResponse<bool>
                {
                    Resultado = false,
                    Mensaje = "No se recibieron datos del usuario.",
                    Datos = false
                };
            }

            // Validaciones
            var resultado = UserValidator.Validate(usuario, ruleSet: "ValidarModificacion");

            if (!resultado.IsValid)
            {
                return new StandardResponse<bool>
                {
                    Resultado = false,
                    Mensaje = resultado.Errors.First().ErrorMessage,
                    Datos = false
                };
            }

            if (!mongoDB.CompararID(usuario.Identificacion)) // false, osea no existe
            {
                return new StandardResponse<bool>
                {
                    Resultado = false,
                    Mensaje = "No existe un usuario registrado con los datos proporcionados.",
                    Datos = false
                };
            }

            if (mongoDB.ModificarUsuario(usuario))
            {
                return new StandardResponse<bool>
                {
                    Resultado = true,
                    Mensaje = "Usuario modificado correctamente.",
                    Datos = true
                };
            }
            else
            {
                return new StandardResponse<bool>
                {
                    Resultado = false,
                    Mensaje = "No se pudo modificar el usuario en el sistema.",
                    Datos = false
                };
            }

        }
        catch (Exception e)
        {
            return new StandardResponse<bool>
            {
                Resultado = false,
                Mensaje = "Error no controlado: " + e.Message,
                Datos = false
            };
        }

    }

    public StandardResponse<bool> ModificarEstadoUsuario(Usuarios usuario)
    { // Recibe identificacion y estado

        try
        {
            if (usuario == null)
            {
                return new StandardResponse<bool>
                {
                    Resultado = false,
                    Mensaje = "No se recibieron datos del usuario.",
                    Datos = false
                };
            }

            // Validaciones 
            var resultado = UserValidator.Validate(usuario, ruleSet: "Identificacion");

            if (!resultado.IsValid)
            {
                return new StandardResponse<bool>
                {
                    Resultado = false,
                    Mensaje = resultado.Errors.First().ErrorMessage,
                    Datos = false
                };
            }

            // Validar que tenga un usuario y que exista
            if (!mongoDB.CompararID(usuario.Identificacion)) // false, osea no existe
            {
                return new StandardResponse<bool>
                {
                    Resultado = false,
                    Mensaje = "No existe un usuario registrado con los datos proporcionados.",
                    Datos = false
                };
            }

            // Modificar
            if (mongoDB.ModificarEstadoUsuario(usuario.Identificacion, usuario.Estado))
            {
                return new StandardResponse<bool>
                {
                    Resultado = true,
                    Mensaje = usuario.Estado ? "Usuario activado correctamente." : "Usuario desactivado correctamente.",
                    Datos = true
                };
            }
            else
            {
                return new StandardResponse<bool>
                {
                    Resultado = false,
                    Mensaje = "No se pudo modificar el estado del usuario en el sistema.",
                    Datos = false
                };
            }
            
        } 
        catch (Exception e)
        {
            return new StandardResponse<bool>
            {
                Resultado = false,
                Mensaje = "Error no controlado: " + e.Message
            };
        }

    }

}
