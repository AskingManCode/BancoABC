using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

[DataContract]
public class RespuestaConsulta
{
    [DataMember]
    public bool Resultado { get; set; }

    [DataMember]
    public string Mensaje { get; set; }

    [DataMember]
    public string Saldo { get; set; }
}