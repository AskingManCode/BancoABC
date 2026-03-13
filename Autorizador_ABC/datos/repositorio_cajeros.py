from typing import Optional, Dict, Any
from datos.conexion_mysql import ConexionMySQL


class RepositorioCajeros:
    def __init__(self, conexion: ConexionMySQL):
        self._conexion = conexion

    def obtener_cajero_activo_por_identificador(self, identificador: str) -> Optional[Dict[str, Any]]:
        """
        Valida cajero por CAJ_ID_Identificador y CAJ_Estado=1.
        El identificador NO va cifrado nunca. Ej: 'SIM-001'
        """
        sql = """
            SELECT CAJ_ID, CAJ_ID_Identificador, CAJ_Estado
            FROM CAJEROS_TB
            WHERE CAJ_ID_Identificador = %s AND CAJ_Estado = 1
            LIMIT 1;
        """
        with self._conexion.cursor_dict() as cur:
            cur.execute(sql, (identificador,))
            return cur.fetchone()
