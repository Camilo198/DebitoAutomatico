using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioDebito.EN.Definicion
{
    [Serializable()]
    public class DatosDebitoDEF
    {
        public const String _NombreTabla = "tb_DEB_DATOS_DEBITO";
        public const String Id = "ID";
        public const String Contrato = "CONTRATO";
        public const String Digito = "DIGITO";
        public const String Estado = "ESTADO";
        public const String IdBanco = "ID_BANCO";
        public const String TipoCuenta = "TIPO_CUENTA";
        public const String NumeroCuenta = "NUMERO_CUENTA";
        public const String FormatoDebito = "ID_FORMATO_DEBITO";
        public const String FormatoCancelacion = "ID_FORMATO_CANCELACION";
        public const String IdTitularCuenta = "ID_TITULAR_CUENTA";
        public const String DireccionIP = "DIRECCION_IP";
        public const String Tercero = "TERCERO";
        public const String Suspendido = "SUSPENDIDO";
        public const String FechaInicioSus = "FECHA_INICIO_SUSPENSION";
        public const String FechaFinSus = "FECHA_FIN_SUSPENSION";
    }
}
