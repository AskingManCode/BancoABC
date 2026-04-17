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
    StandardResponse<Usuarios> AutenticarUsuario(Usuarios newUser);

    [OperationContract]
    StandardResponse<bool> CrearNuevoUsuario(Usuarios newUser);

    [OperationContract]
    StandardResponse<bool> ModificarUsuario(Usuarios usuario);

    [OperationContract]
    StandardResponse<bool> ModificarEstadoUsuario(Usuarios usuario);
}
