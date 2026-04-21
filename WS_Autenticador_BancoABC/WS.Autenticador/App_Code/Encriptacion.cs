using System;
using System.Security.Cryptography;
using System.Text;

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

    public string Descifrar(string textoCifrado)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Key;
            aes.IV = IV;

            var decryptor = aes.CreateDecryptor();
            byte[] encryptedBytes = Convert.FromBase64String(textoCifrado);
            byte[] decrypted = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            return Encoding.UTF8.GetString(decrypted);
        }
    }
}