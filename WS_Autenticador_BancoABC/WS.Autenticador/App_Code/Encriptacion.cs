using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

public class Encriptacion
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