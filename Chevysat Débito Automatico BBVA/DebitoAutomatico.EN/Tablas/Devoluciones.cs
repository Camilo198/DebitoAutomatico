using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Tablas
{
    [Serializable()]
    public class Devoluciones
    {
        public String pOperacion { get; set; }
        public int? pId { get; set; }
        public int? pHistCliente { get; set; }
        public int? pIdTipoCausal { get; set; }
        public String pFechaGiro { get; set; }
    }
}
