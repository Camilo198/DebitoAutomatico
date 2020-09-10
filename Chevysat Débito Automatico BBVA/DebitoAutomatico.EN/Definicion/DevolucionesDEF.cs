using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Definicion
{
    [Serializable]
    public class DevolucionesDEF
    {
        public const String _NombreTabla = "tb_DEB_DEVOLUCIONES";
        public const String Id = "ID";
        public const String IdHistCliente = "ID_HIST_CLIENTE";
        public const String IdTipoCausal = "ID_TIPO_CAUSAL";
        public const String FechaGiro = "FECHA_GIRO";
    }
}
