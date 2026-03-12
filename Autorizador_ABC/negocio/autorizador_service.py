from typing import Any, Dict

from datos.conexion_mysql import ConexionMySQL
from datos.repositorio_tarjetas import RepositorioTarjetas
from datos.repositorio_cajeros import RepositorioCajeros
from datos.repositorio_transacciones import RepositorioTransacciones
from integraciones.cliente_core import ClienteCore
from bitacora.bitacora_worker import BitacoraWorker

from negocio.aut1_retiro import procesar_aut1_retiro
from negocio.aut2_consulta import procesar_aut2_consulta
from negocio.aut3_cambio_pin import procesar_aut3_cambio_pin
from negocio.aut5_confirmacion import procesar_aut5_confirmacion
from negocio.normalizacion import normalizar_tipo_transaccion


class AutorizadorService:
    def __init__(
        self,
        conexion_mysql: ConexionMySQL,
        repo_tarjetas: RepositorioTarjetas,
        repo_cajeros: RepositorioCajeros,
        repo_trans: RepositorioTransacciones,
        cliente_core: ClienteCore,
        bitacora: BitacoraWorker,
        habilitar_core: bool = False,
    ):
        self._conexion = conexion_mysql
        self._repo_tarjetas = repo_tarjetas
        self._repo_cajeros = repo_cajeros
        self._repo_trans = repo_trans
        self._cliente_core = cliente_core
        self._bitacora = bitacora
        self._habilitar_core = habilitar_core

    def manejar(self, request: Dict[str, Any]) -> Dict[str, Any]:

        tipo = normalizar_tipo_transaccion(str(request.get("TipoDeTransaccion", "")).strip())

        # AUT5: Confirmación
        codigo_aut = str(request.get("CodigoDeAutorizacion", "")).strip()
        if codigo_aut:
            return procesar_aut5_confirmacion(
                request=request,
                repo_tarjetas=self._repo_tarjetas,
                repo_cajeros=self._repo_cajeros,
                repo_trans=self._repo_trans,
                cliente_core=self._cliente_core,
                bitacora=self._bitacora,
                habilitar_core=self._habilitar_core
            )

        # AUT3: Cambio de PIN
        if ("PinActual" in request and "PinNuevo" in request) or tipo in ("Cambio de PIN", "CambioPIN"):
            return procesar_aut3_cambio_pin(
                request=request,
                repo_tarjetas=self._repo_tarjetas,
                repo_cajeros=self._repo_cajeros,
                bitacora=self._bitacora
            )

        # AUT2: Consulta
        if tipo == "Consulta":
            return procesar_aut2_consulta(
                request=request,
                repo_tarjetas=self._repo_tarjetas,
                repo_cajeros=self._repo_cajeros,
                cliente_core=self._cliente_core,
                bitacora=self._bitacora,
                habilitar_core=self._habilitar_core
            )

        # AUT1: Retiro
        monto_retiro = str(request.get("MontoRetiro", "")).strip()
        if tipo == "Retiro" and monto_retiro:
            return procesar_aut1_retiro(
                request=request,
                repo_tarjetas=self._repo_tarjetas,
                repo_cajeros=self._repo_cajeros,
                cliente_core=self._cliente_core,
                bitacora=self._bitacora,
                habilitar_core=self._habilitar_core
            )

        return {"status": "2"}
