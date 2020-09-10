using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioDebito.EN.Tablas
{
    [Serializable()]
    public class TipoCuenta
    {
        public String pOperacion { get; set; }
        public int? pId { get; set; }
        public String pValor { get; set; }
        public String pValorNuevo { get; set; }
    }
}
