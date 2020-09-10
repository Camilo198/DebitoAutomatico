using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Tablas
{
    [Serializable()]
    public class Fiducias
    {
        public String pOperacion { get; set; }
        public int? pId { get; set; }
        public int? pFiducia { get; set; }
        public String pCuentaFiducia1 { get; set; }
        public String pCuentaFiducia2 { get; set; }
        public int? pIdTipoCuenta1 { get; set; }
        public int? pIdTipoCuenta2 { get; set; }
       
    }
}
