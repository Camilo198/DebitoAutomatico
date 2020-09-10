using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioDebito.EN.Definicion
{
    [Serializable()]
    public class TitularCuentaDEF
    {
        public const String _NombreTabla = "tb_DEB_TITULAR_CUENTA";
        public const String Id = "ID";
        public const String Nombre = "NOMBRE";
        public const String TipoIdentificacion = "TIPO_IDENTIFICACION";
        public const String NumeroIdentificacion = "NUMERO_IDENTIFICACION";
        public const String Celular = "CELULAR";
        public const String Correo = "CORREO";
        public const String EstadoLLave = "NOMBRE_ESTADO";
        public const String FechaIngreso = "FECHA_INGRESO";
        public const String FechaFinalizacion = "FECHA_FINALIZACION";
        public const String MontoMaximo = "MONTO_MAXIMO";
        public const String Intentos = "INTENTOS";
    }
}
