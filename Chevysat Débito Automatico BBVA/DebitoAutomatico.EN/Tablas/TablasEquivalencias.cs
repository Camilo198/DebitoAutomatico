using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Tablas
{
    [Serializable()]
    public class TablasEquivalencias
    {
        public String pOperacion { get; set; }

        public int? pId { get; set; }
        public String pNombre { get; set; }
        public String pTipoArchivo { get; set; }
        public int? pIdBanco { get; set; }
    }
}
