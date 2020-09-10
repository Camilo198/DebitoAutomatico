using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Tablas
{
    [Serializable()]
    public class Convenio
    {
        public String pOperacion { get; set; }
        public int? pIdProceso { get; set; }

        public int? pId { get; set; }
        public int? pIdBancoDebito { get; set; }
        public int? pIdBanco { get; set; }
        public String pIdPrenota { get; set; }
        public String pIdDebito { get; set; }
        
    }
}
