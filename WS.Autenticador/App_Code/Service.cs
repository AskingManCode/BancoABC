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


// NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código, en svc y en el archivo de configuración.
public class Service : IService
{
    public StandardResponse<bool> CrearNuevoUsuario(Usuarios newUser) 
    {
        /*string identificacion,
        string nombre,
        string primerApellido,
        string segundoApellido,
        string correo,
        string user,
        string password,
        string tipoUsuario,
        Boolean activo = true*/
        try
        {

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
            ValidadorUsuarios validador = new ValidadorUsuarios();
            var resultado = validador.Validate(newUser, ruleSet: "CrearNuevoUsuario");

            if (!resultado.IsValid) // true o false
            {
                return new StandardResponse<bool>
                {
                    Resultado = false,
                    Mensaje = resultado.Errors.First().ErrorMessage,
                    Datos = false
                };
            
            } 

            // Validación de duplicados
            // etc

            // Guarda al usuario en la base de datos
            if (DataBaseMongoDB.GuardarDatos(newUser))
            {
                return new StandardResponse<bool>
                {
                    Resultado = false,
                    Mensaje = "Usuario creado correctamente.",
                    Datos = false
                };
            }
            else
            {
                return new StandardResponse<bool>
                {
                    Resultado = false,
                    Mensaje = "Usuario creado correctamente.",
                    Datos = false
                };
            }

        }
        catch (Exception e)
        {
            // Mensaje de error
            return new StandardResponse<bool>
            {
                Resultado = false,
                Mensaje = "Error no controlado: " + e.Message,
                Datos = false
            };
        }
        
    }

    public void AutenticarUsuario(
        string user,
        string password,
        string tipoUsuario)
    {



    }

    public void ModificarUsuario(
        string identificacion,
        string nombre,
        string primerApellido,
        string segundoApellido,
        string correo,
        string user,
        string password)
    {

        // this.AutenticarUsuario(); // Revisar

    }

    public void ModificarEstadoUsuario(
        string identificacion,
        Boolean activo)
    {



    }

}
