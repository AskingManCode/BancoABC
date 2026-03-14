using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

/// <summary>
/// Descripción breve de EncriptacionParaPruebasEntregable
/// Esta clase es temporal y se borrará para el tercer entregable
/// Su función es encriptar los datos en el WebService para cumplir
/// con los requerimientos del WebService de Autenticación de Usuarios
/// Se entiende que más adelante todo viene cifrado desde Interfaz de Usuario
/// </summary>
public class EncriptacionParaPruebasEntregable
{
    private static readonly byte[] Key =
            Encoding.UTF8.GetBytes("12345678901234567890123456789012");

    private static readonly byte[] IV =
        Encoding.UTF8.GetBytes("1234567890123456");

    public string Cifrar(string texto)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Key;
            aes.IV = IV;

            var encryptor = aes.CreateEncryptor();
            byte[] inputBytes = Encoding.UTF8.GetBytes(texto);
            byte[] encrypted = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);

            return Convert.ToBase64String(encrypted);
        }
    }
}