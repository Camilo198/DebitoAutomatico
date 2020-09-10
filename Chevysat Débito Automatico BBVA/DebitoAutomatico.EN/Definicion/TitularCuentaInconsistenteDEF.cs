using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Definicion
{
    [Serializable()]
    public class TitularCuentaInconsistenteDEF
    {
        public const String _NombreTabla = "tb_DEB_TITULAR_CUENTA_INCONSISTENTE";
        public const String Id = "ID";
        public const String Nombre = "NOMBRE";
        public const String TipoIdentificacion = "TIPO_IDENTIFICACION";
        public const String NumeroIdentificacion = "NUMERO_IDENTIFICACION";
        public const String FechaIngreso = "FECHA_INGRESO";
    }
}
