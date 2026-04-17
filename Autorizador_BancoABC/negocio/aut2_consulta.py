from typing import Dict, Any

from datos.repositorio_tarjetas import RepositorioTarjetas
from datos.repositorio_cajeros import RepositorioCajeros
from integraciones.cliente_core import ClienteCore
from bitacora.bitacora_worker import BitacoraWorker

from negocio.validaciones import iguales_por_texto_descifrado, fechas_iguales
from negocio.normalizacion import normalizar_tipo_transaccion
from seguridad.cifrado_aes import descifrar


def _campo_obligatorio(req: Dict[str, Any], key: str) -> bool:
    return key in req and str(req.get(key, "")).strip() != ""


def _formatear_monto(monto: float) -> str:
    return f"{monto:,.2f}"


def procesar_aut2_consulta(
    request: Dict[str, Any],
    repo_tarjetas: RepositorioTarjetas,
    repo_cajeros: RepositorioCajeros,
    cliente_core: ClienteCore,
    bitacora: BitacoraWorker,
    habilitar_core: bool = False
) -> Dict[str, Any]:
    
    print("*** DENTRO DE AUT2_CONSULTA ***")
    
    from seguridad.cifrado_aes import descifrar
    print("CVV cifrado recibido:", request.get("CodigoDeVerificacion"))
    print("CVV descifrado recibido:", descifrar(request.get("CodigoDeVerificacion")))
    print("Fecha cifrada recibida:", request.get("FechaDeVencimiento"))
    print("Fecha descifrada recibida:", descifrar(request.get("FechaDeVencimiento")))
    
    """
    AUT2: Consulta (sin monto).
    - Valida obligatorios
    - Valida cajero activo (NO cifrado)
    - Valida tarjeta activa + PIN/Venc/CVV (cifrados)
    - Crédito: devuelve saldo de adelanto (MySQL)
    - Débito: consulta saldo en Core (si habilitar_core=True)
    """

    def _bitacora(cajero: str, tarjeta: str, cliente: str, monto_txt: str) -> None:
        bitacora.encolar({
            "tipo": "Consulta",
            "cajero": cajero,
            "tarjeta": tarjeta,
            "cliente": cliente,
            "monto": monto_txt,
        })

    obligatorios = [
        "NumeroDeTarjeta",
        "FechaDeVencimiento",
        "CodigoDeVerificacion",
        "IdentificadorDelCajero",
        "TipoDeTransaccion",
    ]
    for k in obligatorios:
        if not _campo_obligatorio(request, k):
            _bitacora(str(request.get("IdentificadorDelCajero", "")), str(request.get("NumeroDeTarjeta", "")), "", "0.00")
            return {"status": "2"}
    print("Campos obligatorios OK")

    # Validar tipo de transacción (consulta)
    if normalizar_tipo_transaccion(str(request.get("TipoDeTransaccion", ""))) != "Consulta":
        _bitacora(str(request.get("IdentificadorDelCajero", "")), str(request.get("NumeroDeTarjeta", "")), "", "0.00")
        print("Tipo de transacción no es Consulta")
        return {"status": "2"}
    print("Tipo de transacción OK")

    cajero_id = str(request["IdentificadorDelCajero"]).strip()
    try:
        cajero = repo_cajeros.obtener_cajero_activo_por_identificador(cajero_id)
        print(f"Cajero {cajero_id} encontrado: {cajero is not None}")
    except Exception as e:
        print(f"ERROR al obtener cajero: {e}")
        cajero = None
        
    if not cajero:
        _bitacora(cajero_id, str(request.get("NumeroDeTarjeta", "")), "", "0.00")
        return {"status": "2"}
    
    numero_cif = str(request["NumeroDeTarjeta"]).strip()
    
    try:
        tarjeta = repo_tarjetas.obtener_detalle_tarjeta_por_numero(numero_cif)
        print(f"Tarjeta encontrada: {tarjeta is not None}")
    except Exception as e:
        print(f"ERROR al obtener tarjeta: {e}")
        tarjeta = None
    
    print(f"Tarjeta encontrada: {tarjeta is not None}")
    
    if not tarjeta:
        _bitacora(cajero_id, numero_cif, "", "0.00")
        return {"status": "2"}
    
    print("TARJETA_Estado:", tarjeta.get("TARJETA_Estado"))
    print("CUENTA_Estado:", tarjeta.get("CUENTA_Estado"))
    print("TARJETA_TIPO_TARJ_ID:", tarjeta.get("TARJETA_TIPO_TARJ_ID"))

    cliente_id = str(tarjeta.get("CUENTA_ID", "")).strip()

#Guardar mis cambios

    # Activa/inactiva
    if int(tarjeta.get("TARJETA_Estado", 0)) != 1 or int(tarjeta.get("CUENTA_Estado", 0)) != 1:
        _bitacora(cajero_id, numero_cif, cliente_id, "0.00")
        print("Tarjeta o cuenta inactiva")
        return {"status": "3"}

    # CVV (cifrado)
    if not iguales_por_texto_descifrado(
        str(request["CodigoDeVerificacion"]).strip(),
        str(tarjeta.get("TARJETA_NumVerificacion", "")).strip()
    ):
        _bitacora(cajero_id, numero_cif, cliente_id, "0.00")
        print("CVV incorrecto")
        return {"status": "2"}
    print("CVV OK")

    # Fecha (cifrada)
    # 1) Validar que la fecha ingresada coincide con la registrada (dato incorrecto -> "2")
    if not fechas_iguales(
        str(request["FechaDeVencimiento"]).strip(),
        str(tarjeta.get("TARJETA_FechaVencimiento", "")).strip()
    ):
        _bitacora(cajero_id, numero_cif, cliente_id, "0.00")
        print("Fecha incorrecta")
        return {"status": "2"}
    print("Fecha OK")

    # 2) Validar vencimiento contra hoy (tarjeta vencida -> "4")
    from negocio.validaciones import tarjeta_esta_vencida
    vencida = tarjeta_esta_vencida(str(tarjeta.get("TARJETA_FechaVencimiento", "")).strip())
    if vencida is None:
        _bitacora(cajero_id, numero_cif, cliente_id, "0.00")
        print("Error al verificar vencimiento")
        return {"status": "2"}
    if vencida:
        _bitacora(cajero_id, numero_cif, cliente_id, "0.00")
        print("Tarjeta vencida")
        return {"status": "4"}
    print("Tarjeta no vencida")

    tipo_id = int(tarjeta.get("TARJETA_TIPO_TARJ_ID", 0))

    # 2 = Crédito (MySQL)
    if tipo_id == 2:
        row = repo_tarjetas.obtener_consulta_credito(numero_cif)
        if not row:
            print("Error al obtener saldo crédito")
            _bitacora(cajero_id, numero_cif, cliente_id, "0.00")
            return {"status": "5"}

        try:
            saldo = float(row.get("SaldoDisponibleAdelantoEfectivo") or 0.0)
        except Exception:
            _bitacora(cajero_id, numero_cif, cliente_id, "0.00")
            print("Error al parsear saldo crédito")
            return {"status": "5"}

        _bitacora(cajero_id, numero_cif, cliente_id, f"{saldo:.2f}")
        print(f"Saldo crédito obtenido: {saldo}")
        return {"status": "OK", "Monto": _formatear_monto(saldo)}

    # 1 = Débito (Core Java por JSON)
    if tipo_id == 1:
        if not habilitar_core:
            _bitacora(cajero_id, numero_cif, cliente_id, "0.00")
            print("Core no habilitado")
            return {"status": "5"} 

        numero_cuenta = str(tarjeta.get("CUENTA_ID", "")).strip()
        if not numero_cuenta:
            _bitacora(cajero_id, numero_cif, cliente_id, "0.00")
            print("Número de cuenta vacío")
            return {"status": "5"}
        # El Core compara contra número ENMASCARADO (6 + ****** + 4)
        from seguridad.cifrado_aes import enmascarar_tarjeta_core
        numero_tarjeta_core = enmascarar_tarjeta_core(numero_cif)

        saldo = cliente_core.consultar_saldo_debito(
            numero_cuenta=numero_cuenta,
            numero_tarjeta=numero_tarjeta_core
        )
        if saldo is None:
            _bitacora(cajero_id, numero_cif, cliente_id, "0.00")
            print("Error al consultar core")
            return {"status": "5"} 

        _bitacora(cajero_id, numero_cif, cliente_id, f"{float(saldo):.2f}")
        
        print(f"Saldo débito obtenido: {saldo}")
        return {"status": "OK", "Monto": _formatear_monto(float(saldo))}

    print("Tipo de tarjeta desconocido")
    return {"status": "5"}
