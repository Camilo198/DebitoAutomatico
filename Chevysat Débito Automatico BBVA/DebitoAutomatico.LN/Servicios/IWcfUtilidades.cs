using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using Renci.SshNet;

namespace DebitoAutomatico.LN
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IWcfUtilidades" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IWcfUtilidades
    {

        [OperationContract]
        string fechaJulianaToGregoriana(long fechaJuliana);

        [OperationContract]
        Int64 ConvertToJuliana(string Date);

        [OperationContract]
        int CalculaEdad(DateTime FechaInicio, DateTime FechaFin);

        [OperationContract]
        double CalculaRetefuente(double monto);

        [OperationContract]
        double CalculaIVA(double monto);

        [OperationContract]
        double CalculaICA(double monto);

        [OperationContract]
        string ValidaFechaRango(DateTime FecDesde, DateTime FecHasta);

        [OperationContract]
        string ValidaFecha(string Fecha);

        [OperationContract]
        String calculoDigito(String codigo);

        [OperationContract]
        string ConvierteValorLetras(string valor);

        [OperationContract]
        string DownloadFTP(string RemoteFile, string NombreArchivo, string LocalDirectory, string Login, string Password);

        [OperationContract]
        string UploadFTP(string FilePath, string RemotePath, string Login, string Password);

        [OperationContract]
        string EnvioMail(string _Adjunto, string _Asunto, string _Mensaje, string _Para, string _Desde, string _Copia);

        [OperationContract]
        IList<String> LeerFicheroFTP(string Ip, string NombreArchivo, string RutaFTP, string Login, string Password, string fechaPago = "", int codigoBanco = 0);
    }
}
