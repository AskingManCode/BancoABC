using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WS.Entities
{
    [DataContract]
    public class RespuestaAutenticacion
    {
        [DataMember]
        public string Identificacion { get; set; }

        [DataMember]
        public string TipoUsuario { get; set; }
    }
}
