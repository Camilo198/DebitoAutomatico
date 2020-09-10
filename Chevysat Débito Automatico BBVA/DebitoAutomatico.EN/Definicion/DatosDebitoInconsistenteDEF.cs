using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Definicion
{
    [Serializable()]
    public class DatosDebitoInconsistenteDEF
    {
        public const String _NombreTabla = "tb_DEB_DATOS_DEBITO_INCONSISTENTE";
        public const String Id = "ID";
        public const String Contrato = "CONTRATO";
        public const String Digito = "DIGITO";
        public const String Estado = "ESTADO";
        public const String IdBanco = "ID_BANCO";
        public const String TipoCuenta = "TIPO_CUENTA";
        public const String NumeroCuenta = "NUMERO_CUENTA";
        public const String Tercero = "TERCERO";
        public const String IdTitularCuenta = "ID_TITULAR_CUENTA";
        public const String Formato = "FORMATO";
        public const String DireccionIp = "DIRECCION_IP";
        public const String TipoInconsistencia = "TIPO_INCONSISTENCIA";
        public const String Observaciones = "OBSERVACIONES";
        public const String FechaDebito = "FECHA_DEBITO";
    }
}
