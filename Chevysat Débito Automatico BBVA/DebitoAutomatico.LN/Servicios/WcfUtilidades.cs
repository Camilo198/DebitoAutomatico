using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;
using System.Data.Sql;
using System.Net;
using System.Data;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using DebitoAutomatico.LN;
using DebitoAutomatico.LN.Consultas;

namespace DebitoAutomatico.LN
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "WcfUtilidades" en el código y en el archivo de configuración a la vez.
    public class WcfUtilidades : IWcfUtilidades
    {
        private wsenviocorreos.Service EnvioCorreo = new wsenviocorreos.Service();
        private DataRow rowC;
        DebitoAutomatico.AD.Servicios.WcfData wsc = new DebitoAutomatico.AD.Servicios.WcfData();

        /// <summary>
        /// Covierte fecha Juliana en gregoriana
        /// </summary>
        /// <param name="fechaJuliana">número con la fecha juliana</param>
        /// <returns>string con fecha gregoriana dd/MM/yyyy</returns>
        public string fechaJulianaToGregoriana(long fechaJuliana)
        {
            return DateTime.FromOADate(Convert.ToDouble(fechaJuliana + 10594)).ToString("dd/MM/yyyy");
        }


        public Int64 ConvertToJuliana(string Date)
        {
            return Convert.ToInt64(Convert.ToDateTime(Date).ToOADate() - 10594);
        }



        /// <summary>
        /// Devuelve la cantidad de meses entre dos fechas
        /// </summary>
        /// <param name="FechaInicial">Fecha de inicio</param>
        /// <param name="FechaFinal">fecha fin</param>
        /// <returns>Número de meses entre las dos fechas</returns>
        public int CalculaEdad(DateTime FechaInicial, DateTime FechaFinal)
        {
            int meses = 0;
            return meses;
        }

        /// <summary>
        /// Calcula el valor del impuesto Retención en la Fuente
        /// </summary>
        /// <param name="monto">valor sobre el que se calculará</param>
        /// <returns>Valor del impuesto</returns>
        public double CalculaRetefuente(double monto)
        {
            double ReteFuente = 0;
            return ReteFuente;
        }

        /// <summary>
        /// Calcula el valor del impuesto IVA
        /// </summary>
        /// <param name="monto">valor sobre el que se calculará</param>
        /// <returns>Valor del impuesto</returns>
        public double CalculaIVA(double monto)
        {
            double iva = 0;
            return iva;
        }

        /// <summary>
        /// Calcula el valor del impuesto ICA
        /// </summary>
        /// <param name="monto">valor sobre el que se calculará</param>
        /// <returns>Valor del impuesto</returns>
        public double CalculaICA(double monto)
        {
            double ica = 0;
            return ica;
        }



        /// <summary>
        /// Genera un PDF deacuerdo al informe solicitado y lo graba en la dirección predeterminada
        /// </summary>
        /// <param name="url">Dirección del servidor de reportes</param>
        /// <param name="path">ubicación del informe a procesar</param>
        /// <param name="archivo">dirección y nombre del archivo a generar</param>
        /// <param name="parameters">colección de parametros para la ejecución del informe</param>
        /// 



        /// <summary>
        /// Valida un rango de fechas
        /// </summary>
        /// <param name="FecDesde">fecha inicio</param>
        /// <param name="FecHasta">Fecha fin</param>
        /// <returns></returns>
        public string ValidaFechaRango(DateTime FecDesde, DateTime FecHasta)
        {
            if (FecDesde > FecHasta)
                return "R";
            else
            {
                if (FecHasta < DateTime.Now)
                    return "I";
                else
                    return "B";
            }

        }

        /// <summary>
        /// Valida si el texto ingresado correponde a una fecha válida y si es menor o superior a la actual
        /// </summary>
        /// <param name="Fecha"> cadena con la fecha a evaluar</param>
        /// <returns> retorna los valores "Inferior", Superior", "Igual" o "Errada"</returns>
        public string ValidaFecha(string Fecha)
        {
            try
            {

                //   string Fecha = System.Text.RegularExpressions.Regex.Replace(Fecha, "\\b(?<day>\\d{1,2})/(?<month>\\d{1,2})/(?<year>\\d{2,4})\\b", "${month}/${day}/${year}");
                if (Convert.ToDateTime(Fecha) < DateTime.Now.Date)
                {
                    return "Inferior";
                }
                else
                {
                    if (Convert.ToDateTime(Fecha) > DateTime.Now.Date)
                    {
                        return "Superior";
                    }
                    else
                        return "Igual";
                }
            }
            catch (Exception)
            {
                return "Errada";
            }
        }

        /// <summary>
        /// Calculo el dígitode verificación
        /// </summary>
        /// <param name="codigo">Valor al que se le calcula el dígito</param>
        /// <returns>Dígito calculado</returns>
        public String calculoDigito(String codigo)
        {
            var suma = 0;
            var evaluar = 0;
            var aprox = 0;
            var digito = 0;
            String cadena = codigo.Trim().PadLeft(12, '0');

            for (int i = 1; i < 13; i++)
            {
                if (i % 2 == 0)
                {

                    evaluar = Convert.ToInt32(cadena.Substring(i - 1, 1)) * 2;
                    if (evaluar.ToString().Trim().Length == 1)
                        suma = suma + evaluar;
                    else
                    {
                        if (evaluar.ToString().Trim().Length == 2)
                        {
                            suma = suma + Convert.ToInt32(evaluar.ToString().Trim().Substring(0, 1));
                            suma = suma + Convert.ToInt32(evaluar.ToString().Trim().Substring(1, 1));
                        }
                    }
                }
                else
                {
                    evaluar = Convert.ToInt32(cadena.Substring(i - 1, 1));
                    suma = suma + evaluar;
                }
            }

            aprox = (int)Math.Ceiling(suma / 10d) * 10;

            if (aprox >= suma)
                digito = aprox - suma;
            else
                digito = (aprox + 10) - suma;

            return digito.ToString().Trim();
        }
        /// <summary>
        /// Divide una cadena que contiene un cupo en Grupo, Afiliación y Nivel
        /// </summary>
        /// <param name="Cupo">cadena que contiene el cupo</param>
        /// <returns>Arreglo con el grupo, afiliacion, nivel y digito de verificación</returns>
        public string[,] SeparaCupo(string Cupo)
        {
            string[,] CupoS = new string[2, 4];
            if (Cupo.Length == 10)
            {
                CupoS[0, 0] = "Grupo";
                CupoS[0, 1] = "Afiliacion";
                CupoS[0, 2] = "Nivel";
                CupoS[0, 3] = "Digito";
                CupoS[1, 0] = Cupo.Substring(0, 5);
                CupoS[2, 1] = Cupo.Substring(5, 3);
                CupoS[0, 2] = Cupo.Substring(8, 2);
                CupoS[0, 3] = Cupo.Substring(10, (Cupo.Length - 10));
            }
            else
            {
                CupoS[0, 0] = "Grupo";
                CupoS[0, 1] = "Afiliacion";
                CupoS[0, 2] = "Nivel";
                CupoS[1, 0] = "Longitud del cupo errada";
                CupoS[2, 1] = "0";
                CupoS[0, 2] = "0";
            }
            return CupoS;
        }

        /// <summary>
        /// Convierte un valor en letras
        /// </summary>
        /// <param name="valor">cifra a convertir</param>
        /// <returns>cadena con el valor</returns>

        public string ConvierteValorLetras(string valor)
        {
            string letras = "***";
            int contador = 1;
            int inicio = 0;
            string string_ = valor.PadLeft(9);
            while (contador < 4)
            {
                string bloque = string_.Substring(inicio, 3);
                string cientos = bloque.Substring(0, 1);
                string dieces = bloque.Substring(1, 2);
                string unidad = bloque.Substring(2, 1);
                if (bloque.Trim() != "")
                {
                    if (Convert.ToInt32(bloque) > 99)
                    {
                        if (Convert.ToInt32(bloque) != 100)
                        {
                            cientos = cientos + "00";
                            switch (cientos)
                            {
                                case "100":
                                    letras = letras + "CIENTO ";
                                    break;
                                case "200":
                                    letras = letras + "DOSCIENTOS ";
                                    break;
                                case "300":
                                    letras = letras + "TRESCIENTOS ";
                                    break;
                                case "400":
                                    letras = letras + "CUATROCIENTOS ";
                                    break;
                                case "500":
                                    letras = letras + "QUINIENTOS ";
                                    break;
                                case "600":
                                    letras = letras + "SEISCIENTOS ";
                                    break;
                                case "700":
                                    letras = letras + "SETECIENTOS ";
                                    break;
                                case "800":
                                    letras = letras + "OCHOCIENTOS ";
                                    break;
                                case "900":
                                    letras = letras + "NOVECIENTOS ";
                                    break;
                            };

                        }
                        else
                            letras = letras + "CIEN ";

                    }

                    double x = Convert.ToDouble(dieces);

                    if (x > 0)
                    {
                        if ((Convert.ToInt32(x / 10.0) == x / 10.0) | (x > 9 & x <= 20)) ///primera
                        {
                            switch (dieces)
                            {
                                case "10":
                                    letras = letras + "DIEZ ";
                                    break;
                                case "11":
                                    letras = letras + "ONCE ";
                                    break;
                                case "12":
                                    letras = letras + "DOCE ";
                                    break;
                                case "13":
                                    letras = letras + "TRECE  ";
                                    break;
                                case "14":
                                    letras = letras + "CATORCE ";
                                    break;
                                case "15":
                                    letras = letras + "QUINCE ";
                                    break;
                                case "16":
                                    letras = letras + "DIECISEIS ";
                                    break;
                                case "17":
                                    letras = letras + "DIECISIETE ";
                                    break;
                                case "18":
                                    letras = letras + "DIECIOCHO ";
                                    break;
                                case "19":
                                    letras = letras + "DIECINUEVE ";
                                    break;
                                case "20":
                                    letras = letras + "VEINTE ";
                                    break;
                            };
                        }
                        if (x > 20 & (Convert.ToInt32(x / 10.0) != x / 10.0)) ///segunda
                        {
                            dieces = dieces.Substring(0, 1) + "0";
                            switch (dieces)
                            {
                                case "20":
                                    letras = letras + "VEINTI ";
                                    break;
                                case "30":
                                    letras = letras + "TREINTA Y ";
                                    break;
                                case "40":
                                    letras = letras + "CUARENTA Y ";
                                    break;
                                case "50":
                                    letras = letras + "CINCUENTA Y  ";
                                    break;
                                case "60":
                                    letras = letras + "SESENTA Y ";
                                    break;
                                case "70":
                                    letras = letras + "SETENTA Y ";
                                    break;
                                case "80":
                                    letras = letras + "OCHENTA Y ";
                                    break;
                                case "90":
                                    letras = letras + "NOVENTA Y ";
                                    break;
                            };
                            switch (unidad)
                            {
                                case "1":
                                    letras = letras + "UN ";
                                    break;
                                case "2":
                                    letras = letras + "DOS ";
                                    break;
                                case "3":
                                    letras = letras + "TRES ";
                                    break;
                                case "4":
                                    letras = letras + "CUATRO ";
                                    break;
                                case "5":
                                    letras = letras + "CINCO ";
                                    break;
                                case "6":
                                    letras = letras + "SEIS ";
                                    break;
                                case "7":
                                    letras = letras + "SIETE ";
                                    break;
                                case "8":
                                    letras = letras + "OCHO ";
                                    break;
                                case "9":
                                    letras = letras + "NUEVE ";
                                    break;
                            };
                        }


                        if (x < 10)
                        {
                            switch (unidad)
                            {
                                case "1":
                                    letras = letras + "UN ";
                                    break;
                                case "2":
                                    letras = letras + "DOS ";
                                    break;
                                case "3":
                                    letras = letras + "TRES ";
                                    break;
                                case "4":
                                    letras = letras + "CUATRO ";
                                    break;
                                case "5":
                                    letras = letras + "CINCO ";
                                    break;
                                case "6":
                                    letras = letras + "SEIS ";
                                    break;
                                case "7":
                                    letras = letras + "SIETE ";
                                    break;
                                case "8":
                                    letras = letras + "OCHO ";
                                    break;
                                case "9":
                                    letras = letras + "NUEVE ";
                                    break;
                            };
                        }

                    }
                    if (Convert.ToInt32(valor) > 999999 & contador == 1)
                    {
                        if (Convert.ToInt32(valor) / 1000000 > 1)
                            letras = letras + "MILLONES ";
                        else
                            letras = letras + "MILLON ";
                    }

                    if (Convert.ToInt32(valor) > 999 & contador == 2 & Convert.ToInt32(bloque) != 0)
                        letras = letras + "MIL ";
                }
                inicio = inicio + 3;
                contador = contador + 1;
            }

            return letras + " PESOS ***";
        }

        /// <summary>
        /// Descarga un archivo desde una ruta FTP
        /// </summary>
        /// <param name="LocalDirectory">Ruta destino</param>
        /// <param name="RemoteFile">Ruta origen</param>
        /// <param name="Login">Usuario</param>
        /// <param name="Password">Password</param>
        public string DownloadFTP(string RemoteFile, string NombreArchivo, string LocalDirectory, string Login, string Password)
        {
            try
            {
                FtpWebRequest ftp;
                string donde = RemoteFile + NombreArchivo;
                System.IO.FileStream outPutStream = new System.IO.FileStream(donde, FileMode.Create);
                ftp = ((FtpWebRequest)(FtpWebRequest.Create(new Uri((LocalDirectory + NombreArchivo.Trim())))));
                ftp.Credentials = new NetworkCredential(Login, Password);
                ftp.Method = WebRequestMethods.Ftp.DownloadFile;
                ftp.UseBinary = true;
                FtpWebResponse response;
                response = ((FtpWebResponse)(ftp.GetResponse()));
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while ((readCount > 0))
                {
                    outPutStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                ftpStream.Close();
                outPutStream.Close();
                response.Close();
                return "S";
            }
            catch (Exception exc)
            {
                return exc.Message;
            }
        }
        /// <summary>
        /// Carga un archivo en una ruta FTP
        /// </summary>
        /// <param name="FilePath">Ubcicación y nombre del archivo exacto</param>
        /// <param name="RemotePath">Ruta del destino</param>
        /// <param name="Login">Usuario sitio FTP</param>
        /// <param name="Password">Password usuario FTP</param>
        public string UploadFTP(string FilePath, string RemotePath, string Login, string Password)
        {
            try
            {
                using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    string url = Path.Combine(RemotePath, Path.GetFileName(FilePath));
                    FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(url);
                    ftp.Credentials = new NetworkCredential(Login, Password);
                    ftp.KeepAlive = false;
                    ftp.Method = WebRequestMethods.Ftp.UploadFile;
                    ftp.UseBinary = true;
                    ftp.ContentLength = fs.Length;
                    ftp.Proxy = null;
                    fs.Position = 0;
                    int buffLength = 2048;
                    byte[] buff = new byte[buffLength];
                    int contentLen;
                    using (Stream strm = ftp.GetRequestStream())
                    {
                        contentLen = fs.Read(buff, 0, buffLength);
                        while (contentLen != 0)
                        {
                            strm.Write(buff, 0, contentLen);
                            contentLen = fs.Read(buff, 0, buffLength);
                        }
                    }
                }
                return "OK";
            }
            catch (Exception Exc)
            {
                return Exc.Message;
            }
        }

        public string ValidaFechaNoSuperior(string Fecha)
        {
            try
            {
                if (Convert.ToDateTime(Fecha) > DateTime.Now.Date)
                    return "Superior";
                else
                    return "Igual";
            }
            catch (Exception)
            {
                return "Errada";
            }
        }

        public string EnvioMail(string _Adjunto, string _Asunto, string _Mensaje, string _Para, string _Desde, string _Copia)
        {
            try
            {
                DataSet DsCorreos = new DataSet();
                DsCorreos.Tables.Add("Reportes");
                DsCorreos.Tables["Reportes"].Columns.Add("strTo");
                DsCorreos.Tables["Reportes"].Columns.Add("strCc");
                DsCorreos.Tables["Reportes"].Columns.Add("strCo");
                DsCorreos.Tables["Reportes"].Columns.Add("strSubject");
                DsCorreos.Tables["Reportes"].Columns.Add("strMessaje");
                DsCorreos.Tables["Reportes"].Columns.Add("strPath");

                rowC = DsCorreos.Tables["Reportes"].NewRow();
                rowC["strTo"] = _Para;
                rowC["strCo"] = _Copia;
                rowC["strSubject"] = _Asunto;
                rowC["strMessaje"] = _Mensaje;
                rowC["strPath"] = _Adjunto.Trim();
                DsCorreos.Tables["Reportes"].Rows.Add(rowC);
                EnvioCorreo.EnvioCorreos(DsCorreos, _Desde, false);
                return "MENSAJE ENVIADO";
            }
            catch (Exception exc)
            {
                return exc.Message;
            }
        }

        public IList<String> LeerFicheroFTP(string Ip, string NombreArchivo, string RutaFTP, string Login, string Password, string fechaPago = "", int codigoBanco = 0)
        {
            IList<String> texto = null;
            try
            {
                SftpClient cliente = new SftpClient(Ip, Login, Password);
                cliente.Connect();
                texto = cliente.ReadAllLines(RutaFTP + NombreArchivo);

                cliente.Disconnect();
               
            }
            catch (Exception ex)
            {
                PagosRptLN pagosLN = new PagosRptLN();
                pagosLN.insertaLogErroresLN("DA: "+ex.Message.ToString() + ": " + NombreArchivo, fechaPago, codigoBanco);
                texto = new List<string>();
            }
            return texto;
        }
    }
}