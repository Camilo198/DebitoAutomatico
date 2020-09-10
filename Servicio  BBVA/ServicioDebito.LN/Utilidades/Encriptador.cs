using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ServicioDebito.LN.Utilidades
{
    public class Encriptador
    {
        /// <summary>
        /// Encripta un texto plano
        /// </summary>
        /// <param name="texto">Texto a encriptar</param>
        /// <returns>Texto encriptado</returns>
        public String encriptar(String texto)
        {
            return encriptar(texto, "1qa2ws3eD@753RL04X", "mr#ts&jn", "MD5", 1, "@17HgR(78!jAQMN;", 256);
        }

        /// <summary>
        /// Método para encriptar un texto plano
        /// </summary>
        /// <returns>Texto Encriptado</returns>
        private String encriptar(String texto, String claveBase, String valorRelleno, String algoritmoHash, int iteraciones, String vectorInicial, int tamanioClave)
        {
            byte[] vectorInicialBytes = Encoding.ASCII.GetBytes(vectorInicial);
            byte[] valorRellenoBytes = Encoding.ASCII.GetBytes(valorRelleno);
            byte[] textoBytes = Encoding.UTF8.GetBytes(texto);

            PasswordDeriveBytes clave = new PasswordDeriveBytes(claveBase, valorRellenoBytes, algoritmoHash, iteraciones);
            byte[] claveBytes = clave.GetBytes(tamanioClave / 8);

            RijndaelManaged claveSimetrica = new RijndaelManaged()
            {
                Mode = CipherMode.CBC
            };

            ICryptoTransform encriptador = claveSimetrica.CreateEncryptor(claveBytes, vectorInicialBytes);
            MemoryStream objMemoryStream = new MemoryStream();
            CryptoStream objCryptoStream = new CryptoStream(objMemoryStream, encriptador, CryptoStreamMode.Write);

            objCryptoStream.Write(textoBytes, 0, textoBytes.Length);
            objCryptoStream.FlushFinalBlock();
            byte[] textoCifradoBytes = objMemoryStream.ToArray();

            objMemoryStream.Close();
            objCryptoStream.Close();
            String cifradoTexto = Convert.ToBase64String(textoCifradoBytes);
            return cifradoTexto;
        }

        /// <summary> 
        /// Método para desencriptar un texto encriptado.
        /// </summary>
        /// <param name="textoEncriptado">Texto a desencriptar</param>
        /// <returns>Texto desencriptado</returns>
        public String desencriptar(String textoEncriptado)
        {
            return desencriptar(textoEncriptado, "1qa2ws3eD@753RL04X", "mr#ts&jn", "MD5", 1, "@17HgR(78!jAQMN;", 256);
        }

        /// <summary> 
        /// Método para desencriptar un texto encriptado
        /// </summary>
        /// <returns>Texto desencriptado</returns> 
        private String desencriptar(String textoEncriptado, String claveBase, String valorRelleno, String algoritmoHash, int iteraciones, String vectorInicial, int tamanioClave)
        {
            byte[] vectorInicialBytes = Encoding.ASCII.GetBytes(vectorInicial);
            byte[] valorRellenoBytes = Encoding.ASCII.GetBytes(valorRelleno);
            byte[] textoCifradoBytes = Convert.FromBase64String(textoEncriptado);

            PasswordDeriveBytes clave = new PasswordDeriveBytes(claveBase, valorRellenoBytes, algoritmoHash, iteraciones);
            byte[] claveBytes = clave.GetBytes(tamanioClave / 8);

            RijndaelManaged claveSimetrica = new RijndaelManaged()
            {
                Mode = CipherMode.CBC
            };

            ICryptoTransform desencriptador = claveSimetrica.CreateDecryptor(claveBytes, vectorInicialBytes);
            MemoryStream objMemoryStream = new MemoryStream(textoCifradoBytes);
            CryptoStream objCryptoStream = new CryptoStream(objMemoryStream, desencriptador, CryptoStreamMode.Read);

            byte[] textoBytes = new byte[textoCifradoBytes.Length];
            int decryptedByteCount = objCryptoStream.Read(textoBytes, 0, textoBytes.Length);

            objMemoryStream.Close();
            objCryptoStream.Close();

            String textoOriginal = Encoding.UTF8.GetString(textoBytes, 0, decryptedByteCount);
            return textoOriginal;
        }
    }
}
