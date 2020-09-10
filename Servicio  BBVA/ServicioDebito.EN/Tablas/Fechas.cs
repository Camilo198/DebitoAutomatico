using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioDebito.EN.Tablas
{
    [Serializable()]
    public class Fechas
    {
        public String pOperacion { get; set; }
        public int? pId { get; set; }
        public int? pDia { get; set; }
        public String pValor { get; set; }
        public bool? pHabilita { get; set; }
    }
}
