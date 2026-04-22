using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

[DataContract]
public class StandardResponse<T>
{
    [DataMember]
    public bool Resultado { get; set; }

    [DataMember]
    public string Mensaje { get; set; }

    [DataMember]
    public T Datos { get; set; }
}