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
    StandardResponse<Object> AutenticarUsuario(Usuarios usuario);

    [OperationContract]
    StandardResponse<bool> RegistrarPersona(Personas newPerson);

    [OperationContract]
    StandardResponse<bool> CrearUsuario(Personas newUser);

    //[OperationContract]
    //StandardResponse<bool> ModificarUsuario(Personas usuario);

    //[OperationContract]
    //StandardResponse<bool> ModificarEstadoUsuario(Personas usuario);
}
