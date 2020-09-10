using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Tablas
{
    [Serializable()]
    public class CamposEquivalencias
    {
        public String pOperacion { get; set; }

        public int? pId { get; set; }
        public int? pTablasEquivalencias { get; set; }
        public String pCodigo { get; set; }
        public String pDescripcion { get; set; }
        public String pValor { get; set; }
        public bool? pValorPorDefecto { get; set; }
    }
}
