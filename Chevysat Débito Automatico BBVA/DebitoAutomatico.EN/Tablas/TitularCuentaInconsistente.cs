using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Tablas
{
    [Serializable()]
    public class TitularCuentaInconsistente
    {
        public String pOperacion { get; set; }
        public int? pId { get; set; }
        public String pNombre { get; set; }
        public int? pTipoIdentificacion { get; set; }
        public String pNumeroIdentificacion { get; set; }
        public String pFechaIngreso { get; set; }
    }
}
