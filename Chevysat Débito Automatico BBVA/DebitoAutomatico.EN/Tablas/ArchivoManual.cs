using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Tablas
{
    [Serializable()]
    public class ArchivoManual
    {
        public String pOperacion { get; set; }
        public int? pContrato { get; set; }
        public String pNombre { get; set; }
        public String pValor { get; set; }
        public bool? pAutorizar { get; set; }
        public String pEstadoCliente { get; set; }

        public String pBanco { get; set; }
        public String pFecha { get; set; }

        public String pNumeroCuenta { get; set; }
        public String pTipoCuenta { get; set; }
    }
    
}
