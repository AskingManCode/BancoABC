using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

// NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IService1" en el código y en el archivo de configuración a la vez.
[ServiceContract]
public interface IAutorizadorService
{
    [OperationContract]
    RespuestaConsulta ConsultarSaldo(
        string numeroTarjeta,
        string cvv,
        string fechaVencimiento,
        string identificadorCajero
    );

    [OperationContract]
    RespuestaSimple CambiarPIN(
        string numeroTarjeta,
        string pinActual,
        string pinNuevo,
        string fechaVencimiento,
        string cvv,
        string identificadorCajero
    );

    [OperationContract]
    RespuestaSimple AutorizarRetiro(
        string numeroTarjeta,
        string cvv,
        string pin,
        string fechaVencimiento,
        string identificadorCajero,
        string montoRetiro
    );

    [OperationContract]
    List<CuentaInfo> ObtenerCuentasPorCliente(
        string identificacionCliente
    );

    [OperationContract]
    List<TarjetaInfo> ObtenerTarjetasPorCliente(
        string identificacionCliente
    );

    [OperationContract]
    List<MovimientoCuenta> ObtenerMovimientosCuenta(
        string identificacionCliente, string numeroCuenta
    );

    [OperationContract]
    List<MovimientoCredito> ObtenerMovimientosTarjetaCredito(
        string identificacionCliente, string numeroTarjeta
    );
}
