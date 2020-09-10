using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioDebito.EN.Definicion
{
    [Serializable()]
    public class ActualizaClienteDEF
    {
        public const String _NombreTabla = "tb_DEB_ACTUALIZA_CLIENTES";
        public const String Id = "ID";
        public const String Contrato = "CONTRATO";
        public const String TitularCuenta = "ID_TITULAR_CUENTA";
        public const String IdBanco = "ID_BANCO";
        public const String TipoCuenta = "TIPO_CUENTA";
        public const String NumeroCuenta = "NUMERO_CUENTA";
        public const String DireccionIp = "DIRECCION_IP";
        public const String FechaProceso = "FECHA_PROCESO";
        public const String Usuario = "USUARIO";
        
    }
}
