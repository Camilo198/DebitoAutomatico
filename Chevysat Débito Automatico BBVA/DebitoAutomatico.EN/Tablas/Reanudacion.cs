using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Tablas
{
    public class Reanudacion
    {
        public String pOperacion { get; set; }
        public int? pId { get; set; }
        public String pMes { get; set; }
        public String pAño { get; set; }
        public String pUsuario { get; set; }
        public String pObservaciones { get; set; }
        public bool? pEstado { get; set; }
    }
}
