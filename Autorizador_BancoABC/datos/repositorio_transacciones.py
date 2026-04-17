from typing import Optional, Dict, Any
from decimal import Decimal
from datos.conexion_mysql import ConexionMySQL


class RepositorioTransacciones:
    def __init__(self, conexion: ConexionMySQL):
        self._conexion = conexion

    def ejecutar_sp_aut5_adelanto_efectivo(
        self,
        numero_tarjeta_cifrado: str,
        identificador_cajero: str,
        monto: Decimal
    ) -> int:
        try:
            with self._conexion.cursor() as cur:
                cur.execute("SET @RESPUESTA = -1;")
                cur.execute(
                    "CALL SP_AUT5_ADELANTO_EFECTIVO(%s, %s, %s, %s, @RESPUESTA);",
                    (numero_tarjeta_cifrado, identificador_cajero, "Confirmacion", str(monto))
                )
                cur.execute("SELECT @RESPUESTA AS RESP;")
                row = cur.fetchone()
                self._conexion.commit()
                return int(row[0]) if row and row[0] is not None else 5
        except Exception as ex:
            print("ERROR SP AUT5:", ex)
            try:
                self._conexion.rollback()
            except Exception:
                pass
            return 5

    def descontar_adelanto_credito_y_registrar_retiro(
        self,
        cuenta_id: int,
        tarjeta_id: int,
        identificador_cajero: str,
        monto: Decimal
    ) -> int:
        """
        Devuelve:
        0 = OK
        1 = fondos insuficientes
        2 = datos incorrectos
        5 = error
        """
        try:
            with self._conexion.cursor_dict() as cur:
                # Validar cajero
                cur.execute("""
                    SELECT CAJ_ID
                    FROM CAJEROS_TB
                    WHERE CAJ_ID_Identificador = %s
                      AND CAJ_Estado = 1
                    LIMIT 1
                """, (identificador_cajero,))
                row_caj = cur.fetchone()
                if not row_caj:
                    return 2

                cajero_id = int(row_caj["CAJ_ID"])

                # Validar saldo actual
                cur.execute("""
                    SELECT CUENTA_MontoAdelantadoEfectivo
                    FROM CUENTAS_TB
                    WHERE CUENTA_ID = %s
                      AND CUENTA_TARJETA_ID = %s
                      AND CUENTA_Estado = 1
                    LIMIT 1
                """, (cuenta_id, tarjeta_id))
                row_cta = cur.fetchone()
                if not row_cta:
                    return 2

                saldo = row_cta["CUENTA_MontoAdelantadoEfectivo"]
                if saldo is None:
                    return 1

                saldo_actual = Decimal(str(saldo))
                if monto > saldo_actual:
                    return 1

                # Descontar saldo
                cur.execute("""
                    UPDATE CUENTAS_TB
                    SET CUENTA_MontoAdelantadoEfectivo = CUENTA_MontoAdelantadoEfectivo - %s
                    WHERE CUENTA_ID = %s
                      AND CUENTA_TARJETA_ID = %s
                      AND CUENTA_Estado = 1
                """, (str(monto), cuenta_id, tarjeta_id))

                if cur.rowcount == 0:
                    self._conexion.rollback()
                    return 2

                # Insertar transacción
                cur.execute("""
                    INSERT INTO TRANSACCIONES_TB (
                        TRANSAC_CUENTA_ID,
                        TRANSAC_TIPO_TRANSAC_ID,
                        TRANSAC_Monto,
                        TRANSAC_ESTADO_ID
                    )
                    SELECT
                        %s,
                        TT.TIPO_TRANSAC_ID,
                        %s,
                        ET.EST_TRANSAC_ID
                    FROM TIPOS_TRANSACCIONES_TB TT
                    JOIN ESTADOS_TRANSACCIONES_TB ET
                      ON ET.EST_TRANSAC_Nombre = 'Completada'
                    WHERE TT.TIPO_TRANSAC_Nombre = 'Retiro'
                    LIMIT 1
                """, (cuenta_id, str(monto)))

                transac_id = cur.lastrowid
                if not transac_id:
                    self._conexion.rollback()
                    return 5

                # Relacionar con cajero
                cur.execute("""
                    INSERT INTO TRANSACCIONES_CAJEROS_TB (
                        TRANSAC_CAJ_CAJ_ID,
                        TRANSAC_CAJ_TRANSAC_ID,
                        TRANSAC_FechaHora
                    )
                    VALUES (%s, %s, NOW())
                """, (cajero_id, transac_id))

                self._conexion.commit()
                return 0

        except Exception as ex:
            print("ERROR descontar_adelanto_credito_y_registrar_retiro:", ex)
            try:
                self._conexion.rollback()
            except Exception:
                pass
            return 5

    def guardar_codigo_autorizacion(self, transac_id: int, codigo: str) -> None:
        pass

    def obtener_transaccion_por_codigo_y_tarjeta(
        self,
        codigo_autorizacion: str,
        tarjeta_id: int
    ) -> Optional[Dict[str, Any]]:
        return None