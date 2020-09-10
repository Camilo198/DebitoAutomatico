using System;
using System.Configuration;
using System.IO;
using System.Threading;

namespace DebitoAutomatico.LN.Utilidades
{
    public class Registrador
    {
        private String ClaseOrigen { get; set; }

        public Registrador(String tipo)
        {
            ClaseOrigen = tipo;
        }

        private void escribir(String mensaje)
        {
            StreamWriter log;
            String directorio = ConfigurationManager.AppSettings["RutaLog"],
                archivo = directorio + "Debito_Automatico_Log_" + DateTime.Now.ToString("yyyyMMdd") + ".log";

            if (!Directory.Exists(directorio))
            {
                Directory.CreateDirectory(directorio);
            }

            try
            {
                if (!File.Exists(archivo))
                {
                    log = new StreamWriter(archivo);
                }
                else
                {
                    log = File.AppendText(archivo);
                }
                log.WriteLine(mensaje);
                log.Close();
            }
            catch
            {
                Thread.Sleep(1000);
                escribir(mensaje);
            }
        }

        public void escribirInformacion(String mensaje, int numeroLinea)
        {
            escribir("[INFORMACION][" + ClaseOrigen + "][" + numeroLinea + "][" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + "]: " + mensaje);
        }

        public void escribirAdvertencia(String mensaje, int numeroLinea)
        {
            escribir("[ADVERTENCIA][" + ClaseOrigen + "][" + numeroLinea + "][" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + "]: " + mensaje);
        }

        public void escribirError(String mensaje, int numeroLinea)
        {
            escribir("[ERROR][" + ClaseOrigen + "][" + numeroLinea + "][" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + "]: " + mensaje);
        }
    }
}
