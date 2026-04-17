-----------------------------------
-- USE CORE_BANCOABC_DB;

/*
* mensaje = {
*    "TipoDeTransaccion": "Confirmacion",
*    "NumeroCuenta": "4",
*    "NumeroTarjeta": "541230******7788",
*    "CodigoAutorizacion": "123456789"
*    "Monto": 5000.00
* }
*/

CREATE PROCEDURE SP_CONFIRMACION_DE_RETIRO
    @TipoDeTransaccion VARCHAR(25),
    @NumeroCuenta INT,
    @NumeroTarjeta VARCHAR(50),
    @Monto DECIMAL(10,2),
    @CodAutorizacion VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRAN;

        IF @TipoDeTransaccion != 'Confirmacion'
        BEGIN
            ROLLBACK TRAN;
            THROW 50001, 'TIPO_TRANSAC_INVALIDO', 1;
        END;

        DECLARE @SaldoActual DECIMAL(10,2);

        SELECT @SaldoActual = CUENTA_MontoDisponible
        FROM CUENTAS_TB
        WHERE CUENTA_ID = @NumeroCuenta
          AND CUENTA_Tarjeta = @NumeroTarjeta
          AND CUENTA_Estado = 1;

        IF @SaldoActual IS NULL
        BEGIN
            ROLLBACK TRAN;
            THROW 50002, 'DATOS_INCORRECTOS', 1;
        END;

        IF @Monto > @SaldoActual
        BEGIN
            ROLLBACK TRAN;
            THROW 50003, 'INSUF', 1;
        END;

        UPDATE CUENTAS_TB
        SET CUENTA_MontoDisponible = CUENTA_MontoDisponible - @Monto
        WHERE CUENTA_ID = @NumeroCuenta
          AND CUENTA_Tarjeta = @NumeroTarjeta
          AND CUENTA_Estado = 1;

        INSERT INTO TRANSACCIONES_TB(
            TRAN_CUENTA_ID,
            TRAN_TIPO_TRAN_ID,
            TRAN_MontoRetiro,
            TRAN_CodAutorizacion
        ) VALUES (
            @NumeroCuenta,
            1,
            @Monto,
            @CodAutorizacion
        );

        COMMIT TRAN;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRAN;

        THROW;
    END CATCH
END;
