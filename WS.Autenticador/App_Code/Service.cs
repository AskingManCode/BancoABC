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
    StandardResponse<bool> standardResponse;

    public StandardResponse<bool> CrearNuevoUsuario(
        string identificacion,
        string nombre,
        string primerApellido,
        string segundoApellido,
        string correo,
        string user,
        string password,
        string tipoUsuario,
        Boolean activo = true)
    {

        try
        {
            Usuarios newUser = ValidacionesGenerales(
                identificacion,
                nombre,
                primerApellido,
                segundoApellido,
                correo,
                user,
                password,
                tipoUsuario,
                activo
            );

            if (!standardResponse.Resultado)
            {
                return standardResponse;

            }
            else 
            { 
            
            
            }

            // Guarda al usuario en la base de datos
            /*if (DataBaseMongoDB.GuardarDatos(newUser))
            {
                // Mensaje de éxito

            }
            else
            {
                // Mensaje de error
            }*/

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

    #region 'Validaciones'

    public Usuarios ValidacionesGenerales(
        string identificacion,
        string nombre,
        string primerApellido,
        string segundoApellido,
        string correo,
        string user,
        string password,
        string tipoUsuario,
        Boolean activo = true)
    {



        return new Usuarios();
    }

    public string ValidarTexto(string texto, string nombreCampo) // validar identificacion
    {

        if (string.IsNullOrWhiteSpace(texto))
        {
            throw new ArgumentException("El campo '" + nombreCampo + "' se encuentra vacío.");
        }

        return texto.Trim();
    }

    public int ValidarNumero(string numero, string nombreCampo) 
    {

        return 1;
    }

    #endregion 'Validaciones'
}
