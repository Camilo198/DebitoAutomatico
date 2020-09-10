using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Tablas
{
    public class PagoParcial
    {
        public String pOperacion { get; set; }
        public int? pId { get; set; }
        public String pContrato { get; set; }
        public String pFecha { get; set; }
        public int? pValorSico { get; set; }
        public int? pValorCobrado { get; set; }
    }
}
