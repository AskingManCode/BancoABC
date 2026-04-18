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

[DataContract]
public class CuentaInfo
{
    [DataMember]
    public string NumeroCuenta { get; set; }
    [DataMember]
    public decimal Saldo { get; set; }
}

[DataContract]
public class TarjetaInfo
{
    [DataMember]
    public string NumeroTarjeta { get; set; }
    [DataMember]
    public string Tipo { get; set; }
    [DataMember]
    public string NumeroCuentaAsociada { get; set; }
    [DataMember]
    public bool EsCredito { get; set; }
}

[DataContract]
public class MovimientoCuenta
{
    [DataMember]
    public DateTime Fecha { get; set; }
    [DataMember]
    public string Descripcion { get; set; }
    [DataMember]
    public decimal Monto { get; set; }
}

[DataContract]
public class MovimientoCredito
{
    [DataMember]
    public DateTime Fecha { get; set; }
    [DataMember]
    public string CodigoAutorizacion { get; set; }
    [DataMember]
    public string Comercio { get; set; }
    [DataMember]
    public decimal Monto { get; set; }
}