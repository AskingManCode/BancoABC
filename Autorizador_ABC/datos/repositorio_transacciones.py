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
        """
        SP_AUT5_ADELANTO_EFECTIVO valida tipo = CONFIRMACION.
        """
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
        except Exception:
            try:
                self._conexion.rollback()
            except Exception:
                pass
            return 5
        
    def guardar_codigo_autorizacion(self, transac_id: int, codigo: str) -> None:
        pass

    def obtener_transaccion_por_codigo_y_tarjeta(self, codigo_autorizacion: str, tarjeta_id: int) -> Optional[Dict[str, Any]]:
        return None
