using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WS.Entities
{
    [DataContract]
    public class Usuarios
    {
        [DataMember]
        public string User { get; set; } // Cifrado

        [DataMember]
        public string Password { get; set; } // Cifrado

        [DataMember]
        public string TipoUsuario { get; set; } // 1 = Administrador / 2 = Cliente

        [DataMember]
        public bool Estado { get; set; } // 1 = Activo / 0 = Inactivo
    }
}
