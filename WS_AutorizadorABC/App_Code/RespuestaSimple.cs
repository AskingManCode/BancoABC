using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

[DataContract]
public class RespuestaSimple
{
    [DataMember]
    public bool Resultado { get; set; }

    [DataMember]
    public string Mensaje { get; set; }

}