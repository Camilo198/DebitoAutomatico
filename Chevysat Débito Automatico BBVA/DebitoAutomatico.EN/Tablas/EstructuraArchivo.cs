using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Tablas
{
    [Serializable()]
    public class EstructuraArchivo
    {
        public String pOperacion { get; set; }

        public int? pId { get; set; }
        public int? pConfiguracion { get; set; }
        public String pTipoDato { get; set; }
        public String pNombre { get; set; }
        public int? pColumna { get; set; }
        public int? pLongitud { get; set; }
        public String pCaracterRelleno { get; set; }
        public String pAlineacion { get; set; }
        public int? pCantidadDecimales { get; set; }
        public String pFormatoFecha { get; set; }
        public bool? pEsContador { get; set; }
        public int? pSumaCampo { get; set; }
        public bool? pRequiereCambio { get; set; }
        public bool? pValorPorDefecto { get; set; }
        public String pValor { get; set; }
    }
}
