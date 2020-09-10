using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Tablas
{
    [Serializable()]
    public class Chevyplan
    {
        public String pOperacion { get; set; }
        public int? pId { get; set; }       
        public String pEmpresa{ get; set; }
        public String pIdentificacion { get; set; }
    }
}
