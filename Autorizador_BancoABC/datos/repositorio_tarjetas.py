from typing import Optional, Dict, Any
from datos.conexion_mysql import ConexionMySQL


class RepositorioTarjetas:
    def __init__(self, conexion: ConexionMySQL):
        self._conexion = conexion

    def obtener_detalle_tarjeta_por_numero(self, numero_tarjeta_cifrado: str) -> Optional[Dict[str, Any]]:
        sql = """
            SELECT 
                TARJ.TARJETA_ID,
                TARJ.TARJETA_Numero,
                TARJ.TARJETA_PIN,
                TARJ.TARJETA_NumVerificacion,
                TARJ.TARJETA_FechaVencimiento,
                TARJ.TARJETA_TIPO_TARJ_ID,
                TARJ.TARJETA_Estado,
                TT.TIPO_TARJ_Nombre,
                CU.CUENTA_ID,
                CU.CUENTA_MontoAdelantadoEfectivo,
                CU.CUENTA_Estado
            FROM TIPOS_TARJETAS_TB TT
            INNER JOIN TARJETAS_TB TARJ
                ON TT.TIPO_TARJ_ID = TARJ.TARJETA_TIPO_TARJ_ID
            INNER JOIN CUENTAS_TB CU
                ON TARJ.TARJETA_ID = CU.CUENTA_TARJETA_ID
            WHERE TARJ.TARJETA_Numero = %s
            LIMIT 1;
        """
        with self._conexion.cursor_dict() as cur:
            cur.execute(sql, (numero_tarjeta_cifrado,))
            return cur.fetchone()

    def obtener_consulta_credito(self, numero_tarjeta_cifrado: str) -> Optional[Dict[str, Any]]:
        sql = """
            SELECT 
                TAR.TARJETA_Numero AS NumeroDeTarjeta,
                TT.TIPO_TARJ_Nombre AS TipoDeTarjeta,
                CU.CUENTA_MontoAdelantadoEfectivo AS SaldoDisponibleAdelantoEfectivo
            FROM TIPOS_TARJETAS_TB TT
            INNER JOIN TARJETAS_TB TAR
                ON TT.TIPO_TARJ_ID = TAR.TARJETA_TIPO_TARJ_ID
            INNER JOIN CUENTAS_TB CU
                ON TAR.TARJETA_ID = CU.CUENTA_TARJETA_ID
            WHERE TT.TIPO_TARJ_Nombre = 'Credito'
              AND TAR.TARJETA_Estado = 1
              AND TAR.TARJETA_Numero = %s
            LIMIT 1;
        """
        with self._conexion.cursor_dict() as cur:
            cur.execute(sql, (numero_tarjeta_cifrado,))
            return cur.fetchone()

    def ejecutar_sp_aut3_cambio_pin(
        self,
        cod_cajero: str,
        num_tarjeta: str,
        pin_actual: str,
        pin_nuevo: str
    ) -> int:
        args = [cod_cajero, num_tarjeta, pin_actual, pin_nuevo, 0]
        with self._conexion.cursor() as cur:
            out = cur.callproc("SP_AUT3_CAMBIOPIN", args)
            self._conexion.commit()
            return int(out[-1])
        
    def obtener_tarjetas_por_identificacion(self, identificacion: str) -> list:
        """
        Devuelve todas las tarjetas (débito y crédito) de un cliente.
        Se pasa la identificación en texto plano (ya descifrada).
        """
        sql = """
            SELECT 
                t.TARJETA_Numero,
                tt.TIPO_TARJ_Nombre,
                c.CUENTA_ID
            FROM TARJETAS_TB t
            INNER JOIN CLIENTES_TB cl ON t.CLIENTE_ID = cl.CLIENTE_ID
            INNER JOIN TIPOS_TARJETAS_TB tt ON t.TARJETA_TIPO_TARJ_ID = tt.TIPO_TARJ_ID
            LEFT JOIN CUENTAS_TB c ON t.CUENTA_ID = c.CUENTA_ID
            WHERE cl.CLIENTE_Identificacion = %s
        """
        with self._conexion.cursor_dict() as cur:
            cur.execute(sql, (identificacion,))
            return cur.fetchall()  # lista de diccionarios

    def obtener_movimientos_credito(self, identificacion: str, numero_tarjeta: str) -> list:
        """
        Devuelve los movimientos (autorizaciones exitosas) de una tarjeta de crédito.
        Se pasa identificación y número de tarjeta en texto plano (descifrados).
        """
        sql = """
            SELECT 
                m.MOV_Fecha,
                m.MOV_CodigoAutorizacion,
                m.MOV_Comercio,
                m.MOV_Monto
            FROM MOVIMIENTOS_CREDITO_TB m
            INNER JOIN TARJETAS_TB t ON m.TARJETA_ID = t.TARJETA_ID
            INNER JOIN CLIENTES_TB cl ON t.CLIENTE_ID = cl.CLIENTE_ID
            WHERE cl.CLIENTE_Identificacion = %s 
            AND t.TARJETA_Numero = %s
            ORDER BY m.MOV_Fecha DESC
        """
        with self._conexion.cursor_dict() as cur:
            cur.execute(sql, (identificacion, numero_tarjeta))
            return cur.fetchall()
        


    def obtener_tarjetas_adm(self, identificacion: str) -> list:
        """
        Método exclusivo para ADM4.
        Usa la estructura REAL de la base.
        """
        sql = """
            SELECT 
                t.TARJETA_Numero,
                tt.TIPO_TARJ_Nombre,
                c.CUENTA_ID,
                t.TARJETA_Estado
            FROM PERSONAS_TB p
            INNER JOIN PERSONASXCUENTAS pc
                ON p.PERSONA_ID = pc.PERSONAXCUENTA_PERSONA_ID
            INNER JOIN CUENTAS_TB c
                ON pc.PERSONAXCUENTA_CUENTA_ID = c.CUENTA_ID
            LEFT JOIN TARJETAS_TB t
                ON c.CUENTA_TARJETA_ID = t.TARJETA_ID
            LEFT JOIN TIPOS_TARJETAS_TB tt
                ON t.TARJETA_TIPO_TARJ_ID = tt.TIPO_TARJ_ID
            WHERE p.PERSONA_Identificacion = %s
            ORDER BY t.TARJETA_ID DESC
        """
        with self._conexion.cursor_dict() as cur:
            cur.execute(sql, (identificacion,))
            return cur.fetchall()
    def crear_tarjeta(
        self,
        numero_tarjeta_cifrada: str,
        pin_cifrado: str,
        cvv_cifrado: str,
        fecha_cifrada: str,
        tipo_tarjeta_id: int
    ) -> int:
        sql = """
            INSERT INTO TARJETAS_TB
            (
                TARJETA_Numero,
                TARJETA_PIN,
                TARJETA_NumVerificacion,
                TARJETA_FechaVencimiento,
                TARJETA_TIPO_TARJ_ID,
                TARJETA_Estado
            )
            VALUES (%s, %s, %s, %s, %s, 1)
        """
        with self._conexion.cursor() as cur:
            cur.execute(sql, (
                numero_tarjeta_cifrada,
                pin_cifrado,
                cvv_cifrado,
                fecha_cifrada,
                tipo_tarjeta_id
            ))
            self._conexion.commit()
            return cur.lastrowid

    def asociar_tarjeta_a_cuenta(self, cuenta_id: int, tarjeta_id: int) -> None:
        sql = """
            UPDATE CUENTAS_TB
            SET CUENTA_TARJETA_ID = %s
            WHERE CUENTA_ID = %s
        """
        with self._conexion.cursor() as cur:
            cur.execute(sql, (tarjeta_id, cuenta_id))
            self._conexion.commit()

    def inactivar_tarjeta(self, numero_tarjeta_cifrada: str) -> bool:
        sql = """
            UPDATE TARJETAS_TB
            SET TARJETA_Estado = 0
            WHERE TARJETA_Numero = %s
        """
        with self._conexion.cursor() as cur:
            cur.execute(sql, (numero_tarjeta_cifrada,))
            self._conexion.commit()
            return cur.rowcount > 0