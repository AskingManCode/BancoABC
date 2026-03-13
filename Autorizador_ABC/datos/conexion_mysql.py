import threading
import mysql.connector
from mysql.connector import Error
from typing import Any, Dict, Optional, Iterator
from contextlib import contextmanager


class ConexionMySQL:
    def __init__(self, config: Dict[str, Any]):
        self._cfg = config
        self._local = threading.local()  # conn por hilo

    def _get_conn(self) -> Optional[mysql.connector.MySQLConnection]:
        return getattr(self._local, "conn", None)

    def _set_conn(self, conn: Optional[mysql.connector.MySQLConnection]) -> None:
        setattr(self._local, "conn", conn)

    def conectar(self) -> mysql.connector.MySQLConnection:
        vieja = self._get_conn()
        if vieja is not None:
            try:
                vieja.close()
            except Exception:
                pass

        try:
            conn = mysql.connector.connect(
                host=self._cfg["host"],
                port=self._cfg.get("port", 3306),
                user=self._cfg["user"],
                password=self._cfg["password"],
                database=self._cfg["database"],
                autocommit=False,
            )
            self._set_conn(conn)
            return conn
        except Error as e:
            raise RuntimeError(f"Error conectando a MySQL: {e}")

    def _asegurar_conexion(self) -> mysql.connector.MySQLConnection:
        conn = self._get_conn()
        if conn is None or not conn.is_connected():
            return self.conectar()
        return conn

    @contextmanager
    def cursor(self) -> Iterator[mysql.connector.cursor.MySQLCursor]:
        conn = self._asegurar_conexion()
        cur = conn.cursor(buffered=True)
        try:
            yield cur
        finally:
            try:
                cur.close()
            except Exception:
                pass

    @contextmanager
    def cursor_dict(self) -> Iterator[mysql.connector.cursor.MySQLCursorDict]:
        conn = self._asegurar_conexion()
        cur = conn.cursor(dictionary=True, buffered=True)
        try:
            yield cur
        finally:
            try:
                cur.close()
            except Exception:
                pass

    def commit(self) -> None:
        try:
            self._asegurar_conexion().commit()
        except Error as e:
            raise RuntimeError(f"Error haciendo commit en MySQL: {e}")

    def rollback(self) -> None:
        try:
            self._asegurar_conexion().rollback()
        except Error as e:
            raise RuntimeError(f"Error haciendo rollback en MySQL: {e}")

    def cerrar(self) -> None:
        conn = self._get_conn()
        if conn:
            try:
                conn.close()
            except Exception:
                pass
        self._set_conn(None)
