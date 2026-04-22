from seguridad.cifrado_aes import descifrar


def procesar_obtener_tarjetas_adm(request, repo_tarjetas):
    try:
        identificacion_cifrada = str(request.get("Identificacion", "")).strip()

        if not identificacion_cifrada:
            return {"status": "2", "mensaje": "Identificación requerida"}

        identificacion = descifrar(identificacion_cifrada)
        tarjetas = repo_tarjetas.obtener_tarjetas_adm(identificacion)

        return {
            "status": "OK",
            "tarjetas": tarjetas
        }

    except Exception as ex:
        return {"status": "5", "mensaje": str(ex)}


def procesar_crear_tarjeta(request, repo_tarjetas):
    try:
        tipo_tarjeta = str(request.get("TipoTarjeta", "")).strip()
        numero_cuenta = str(request.get("NumeroCuenta", "")).strip()
        numero_tarjeta_cifrada = str(request.get("NumeroDeTarjeta", "")).strip()
        pin_cifrado = str(request.get("Pin", "")).strip()
        cvv_cifrado = str(request.get("CodigoDeVerificacion", "")).strip()
        fecha_cifrada = str(request.get("FechaDeVencimiento", "")).strip()

        if not tipo_tarjeta or not numero_tarjeta_cifrada or not pin_cifrado or not cvv_cifrado or not fecha_cifrada:
            return {"status": "2", "mensaje": "Datos incompletos"}

        tipo_normalizado = tipo_tarjeta.lower()
        if tipo_normalizado in ("debito", "débito"):
            tipo_id = 1
        elif tipo_normalizado in ("credito", "crédito"):
            tipo_id = 2
        else:
            return {"status": "2", "mensaje": "Tipo de tarjeta inválido"}

        tarjeta_id = repo_tarjetas.crear_tarjeta(
            numero_tarjeta_cifrada=numero_tarjeta_cifrada,
            pin_cifrado=pin_cifrado,
            cvv_cifrado=cvv_cifrado,
            fecha_cifrada=fecha_cifrada,
            tipo_tarjeta_id=tipo_id
        )

        # Solo si es débito y viene una cuenta
        if tipo_id == 1 and numero_cuenta:
            cuenta_id = int(numero_cuenta)
            repo_tarjetas.asociar_tarjeta_a_cuenta(cuenta_id, tarjeta_id)

        return {"status": "OK", "mensaje": "Tarjeta creada correctamente"}

    except Exception as ex:
        return {"status": "5", "mensaje": str(ex)}


def procesar_inactivar_tarjeta(request, repo_tarjetas):
    try:
        numero_tarjeta_cifrada = str(request.get("NumeroDeTarjeta", "")).strip()

        if not numero_tarjeta_cifrada:
            return {"status": "2", "mensaje": "Número de tarjeta requerido"}

        actualizado = repo_tarjetas.inactivar_tarjeta(numero_tarjeta_cifrada)

        if actualizado:
            return {"status": "OK", "mensaje": "Tarjeta inactivada correctamente"}

        return {"status": "2", "mensaje": "No se encontró la tarjeta"}

    except Exception as ex:
        return {"status": "5", "mensaje": str(ex)}
def procesar_obtener_tarjetas_adm(request, repo_tarjetas):
    try:
        identificacion = request.get("Identificacion", "").strip()

        if not identificacion:
            return {"status": "2", "mensaje": "Identificación requerida"}

        tarjetas = repo_tarjetas.obtener_tarjetas_adm(identificacion)

        return {
            "status": "OK",
            "tarjetas": tarjetas
        }

    except Exception as ex:
        return {"status": "5", "mensaje": str(ex)}
    
