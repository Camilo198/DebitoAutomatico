using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Tablas
{
    [Serializable()]
    public class TipoDocumento
    {
        public String pOperacion { get; set; }
        public int? pId { get; set; }
        public String pCodigo { get; set; }
        public String pAbreviatura { get; set; }
    }
}
