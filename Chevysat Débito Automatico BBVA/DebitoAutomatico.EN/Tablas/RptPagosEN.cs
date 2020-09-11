using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Tablas
{
    public class RptPagosEN
    {
        public int codigoBanco { get; set; }
        public string fechaPago { get; set; }
        public DateTime fechaProceso { get; set; }
        public int cantPagosArchivo { get; set; }
        public double valorMontoArchivo { get; set; }
        public int cantPagosReacudo { get; set; }
        public int cantPagosSicoCon { get; set; }
        public double valorMontoSicoCon { get; set; }
        public int cantPagosSicoInc { get; set; }
        public double valorMontoSicoInc { get; set; }
        public DateTime fechaModificacionArch { get; set; }
    }
}
