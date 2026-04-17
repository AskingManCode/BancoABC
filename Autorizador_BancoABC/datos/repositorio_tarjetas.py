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