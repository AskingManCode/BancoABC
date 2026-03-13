import json
import os
import socket
import time

from datos.conexion_mysql import ConexionMySQL
from datos.repositorio_tarjetas import RepositorioTarjetas
from datos.repositorio_cajeros import RepositorioCajeros
from datos.repositorio_transacciones import RepositorioTransacciones

from integraciones.cliente_core import ClienteCore
from bitacora.bitacora_worker import BitacoraWorker
from negocio.autorizador_service import AutorizadorService
from servidor.servidor_autorizador import ServidorAutorizador


def cargar_config(ruta: str = "config.json") -> dict:
    if not os.path.exists(ruta):
        raise FileNotFoundError(f"No existe {ruta}")
    with open(ruta, "r", encoding="utf-8") as f:
        return json.load(f)


def main() -> None:
    cfg = cargar_config()
    #hola#

    # Bitácora
    ruta_bitacora = cfg["bitacora"]["ruta_archivo"]
    bitacora = BitacoraWorker(ruta_bitacora)
    bitacora.start()

    # Datos (MySQL)
    conexion = ConexionMySQL(cfg["mysql"])
    repo_tarjetas = RepositorioTarjetas(conexion)
    repo_cajeros = RepositorioCajeros(conexion)
    repo_trans = RepositorioTransacciones(conexion)

    # Core
    core_cfg = cfg["core"]
    cliente_core = ClienteCore(
        host=core_cfg["host"],
        port=int(core_cfg["port"]),
        timeout_seg=float(core_cfg.get("timeout_seg", 5.0)),
    )
    habilitar_core = bool(core_cfg.get("habilitar_core", False))

    # Si el Core es obligatorio, no levantamos el Autorizador hasta que esté arriba.
    if habilitar_core:
        host = core_cfg["host"]
        port = int(core_cfg["port"])
        intentos = int(core_cfg.get("reintentos_espera", 30))
        espera = float(core_cfg.get("espera_seg", 1.0))

        ok = False
        for _ in range(max(1, intentos)):
            try:
                with socket.create_connection((host, port), timeout=2.0):
                    ok = True
                    break
            except Exception:
                time.sleep(max(0.2, espera))

        if not ok:
            raise RuntimeError(
                f"Core no disponible en {host}:{port}. No se puede iniciar el Autorizador (habilitar_core=true)."
            )

    # Negocio
    service = AutorizadorService(
        conexion_mysql=conexion,
        repo_tarjetas=repo_tarjetas,
        repo_cajeros=repo_cajeros,
        repo_trans=repo_trans,
        cliente_core=cliente_core,
        bitacora=bitacora,
        habilitar_core=habilitar_core
    )

    # Servidor
    srv_cfg = cfg["servidor"]
    servidor = ServidorAutorizador(
        host=srv_cfg["host"],
        port=int(srv_cfg["port"]),
        handler=service.manejar
    )

    try:
        servidor.iniciar()
    except KeyboardInterrupt:
        print("\n[main] Cerrando...")
    finally:

        servidor.detener()
        bitacora.detener()

        try:
            conexion.cerrar()
        except Exception:
            pass


if __name__ == "__main__":
    main()
