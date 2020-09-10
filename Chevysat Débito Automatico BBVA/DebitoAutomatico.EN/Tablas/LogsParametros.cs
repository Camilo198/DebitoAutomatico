using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Tablas
{
    [Serializable()]
    public class LogsParametros
    {
        public String pOperacion { get; set; }
        public String pFecha { get; set; }
        public String pMovimiento { get; set; }
        public String pUsuario { get; set; }
        public String pDetalle { get; set; }
    }
}
