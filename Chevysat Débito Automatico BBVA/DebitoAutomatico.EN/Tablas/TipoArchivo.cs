using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Tablas
{
    [Serializable()]
    public class TipoArchivo
    {
        public String pOperacion { get; set; }

        public String pId { get; set; }
        public String pNombre { get; set; }
    }
}
