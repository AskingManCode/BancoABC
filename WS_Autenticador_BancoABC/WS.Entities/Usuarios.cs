using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace WS.Entities
{
    [BsonIgnoreExtraElements]
    [DataContract]
    public class Usuarios
    {
        [DataMember]
        public string Identificacion { get; set; }

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public string PrimerApellido { get; set; }

        [DataMember]
        public string SegundoApellido { get; set; }

        [DataMember]
        public string Correo { get; set; }

        [DataMember]
        public string User { get; set; } // Cifrado

        [DataMember]
        public string Password { get; set; } // Cifrado

        [DataMember]
        public string TipoUsuario { get; set; } // 1 = Empleado / 2 = Cliente

        [DataMember]
        public bool Estado { get; set; } // 1 = Activo / 0 = Inactivo

        /*[DataMember]
        public List<Usuarios> ListaUsuarios { get; set; } */
    }

}
