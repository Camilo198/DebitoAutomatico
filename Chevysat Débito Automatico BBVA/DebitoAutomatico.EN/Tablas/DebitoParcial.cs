using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Tablas
{
    [Serializable]
    public class DebitoParcial
    {
        public String pOperacion { get; set; }
        public int? pId { get; set; }
        public int? pIdBanco { get; set; }
        public String pValorParcial { get; set; }
    }
}
