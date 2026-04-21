using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WS.Entities;

[ServiceContract]
public interface IService
{
    [OperationContract]
    StandardResponse<RespuestaAutenticacion> AutenticarUsuario(Usuarios usuario);

    [OperationContract]
    StandardResponse<bool> RegistrarPersona(Personas persona);

    [OperationContract]
    StandardResponse<bool> ModificarPersona(Personas usuario);

    [OperationContract]
    StandardResponse<bool> EliminarPersona(string identificacion);

    [OperationContract]
    StandardResponse<List<Personas>> ListarPersonas();

    [OperationContract]
    StandardResponse<bool> CrearUsuario(string identificacion, Usuarios usuario);

    [OperationContract]
    StandardResponse<bool> ModificarEstadoUsuario(Usuarios usuario);

    [OperationContract]
    StandardResponse<List<Usuarios>> ListarUsuarios();
}
