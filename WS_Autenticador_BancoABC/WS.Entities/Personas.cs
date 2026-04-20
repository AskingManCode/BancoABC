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
    public class Personas
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
        public Usuarios Usuario { get; set; }
    }

}
