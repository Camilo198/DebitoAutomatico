using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using DebitoAutomatico.EN;

namespace DebitoAutomatico.AD.Administracion
{
    public class ConsultorXML
    {
        public String RutaXML { get; set; }
        public String Error { get; set; }

        /// <summary>
        /// Lee un campo del archivo xml de configuraciones
        /// </summary>
        /// <param name="objCamposXML">Valores del campo que se necesita consultar, tabla y campo</param>
        /// <returns>Valor del campo solicitado</returns>
        public String leerDatosXML(CamposXML objCamposXML)
        {
            DataSet dsDatos = new DataSet();
            String strValor = String.Empty;

            try
            {
                dsDatos.ReadXml(RutaXML);
                strValor = dsDatos.Tables[objCamposXML.pTabla].Rows[0][objCamposXML.pCampo].ToString();
            }
            catch (System.Security.SecurityException ex)
            {
                Error = "ERROR: " + ex.Message;
            }
            return strValor;
        }

        /// <summary>
        /// Modifica una atributo del xml de configuraciones
        /// </summary>
        /// <param name="objCamposXML">Valores del campo que se necesita modificar, tabla, campo y valor</param>
        public void modificarXML(CamposXML objCamposXML)
        {
            try
            {
                DataSet dsDatos = new DataSet();
                dsDatos.ReadXml(RutaXML);
                dsDatos.Tables[objCamposXML.pTabla].Rows[0][objCamposXML.pCampo] = objCamposXML.pValor;
                dsDatos.WriteXml(RutaXML);
            }
            catch (System.Security.SecurityException ex)
            {
                Error = "ERROR: " + ex.Message;
            }
        }
        /// <summary>
        /// Trae el valor de la cadena de conexion a la base de datos.
        /// </summary>
        /// <returns>Valor de la cadena de conexion</returns>
        public String leerCadenaConexion()
        {
            DataSet dsDatos = new DataSet();
            String strValor = String.Empty;

            try
            {
                dsDatos.ReadXml(System.AppDomain.CurrentDomain.BaseDirectory + "\\Modulos\\XML\\Configuracion.xml");
                strValor = desencriptar(dsDatos.Tables["BD"].Rows[0]["A"].ToString());
            }
            catch (System.Security.SecurityException ex)
            {
                Error = "ERROR: " + ex.Message;
            }
            return strValor;
        }

        /// <summary>
        /// Trae el valor de la cadena de conexion a la base de datos.
        /// </summary>
        /// <returns>Valor de la cadena de conexion</returns>
        public String leerCadenaConexionOdbc()
        {
            DataSet dsDatos = new DataSet();
            String strValor = String.Empty;

            try
            {
                dsDatos.ReadXml(System.AppDomain.CurrentDomain.BaseDirectory + "\\Modulos\\XML\\Configuracion.xml");
                strValor = desencriptar(dsDatos.Tables["BD"].Rows[0]["E"].ToString());
            }
            catch (System.Security.SecurityException ex)
            {
                Error = "ERROR: " + ex.Message;
            }
            return strValor;
        }

        /// <summary> 
        /// Método para desencriptar un texto encriptado
        /// </summary>
        /// <returns>Texto desencriptado</returns> 
        protected String desencriptar(String textoEncriptado)
        {
            byte[] vectorInicialBytes = Encoding.ASCII.GetBytes("@17HgR(78!jAQMN;");
            byte[] valorRellenoBytes = Encoding.ASCII.GetBytes("mr#ts&jn");
            byte[] textoCifradoBytes = Convert.FromBase64String(textoEncriptado);

            PasswordDeriveBytes clave = new PasswordDeriveBytes("1qa2ws3eD@753RL04X", valorRellenoBytes, "MD5", 1);
            byte[] claveBytes = clave.GetBytes(256 / 8);

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
