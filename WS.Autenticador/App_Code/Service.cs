using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WS.Entities;
using WS.DataAccess;


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



            // Guarda al usuario en la base de datos
            if (/*DataBaseMongoDB.GuardarDatos()*/)
            {
                // Mensaje de éxito

            }
            else
            {
                // Mensaje de error
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

    #region "Validaciones"

    private StandardResponse<bool> Validaciones(Usuarios newUser)
    {
        
        return new StandardResponse<bool>;
    }

    #endregion "Validaciones"

}
