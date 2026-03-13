using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WS.Entities;

// NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IService1" en el código y en el archivo de configuración a la vez.
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
