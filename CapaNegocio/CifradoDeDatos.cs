using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CapaNegocio;

namespace CapaNegocio
{
    public class CifradoDeDatos
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
                byte[] buffer = Convert.FromBase64String(textoCifrado);
                byte[] decrypted = decryptor.TransformFinalBlock(buffer, 0, buffer.Length);

                return Encoding.UTF8.GetString(decrypted);
            }
        }
    }
}
