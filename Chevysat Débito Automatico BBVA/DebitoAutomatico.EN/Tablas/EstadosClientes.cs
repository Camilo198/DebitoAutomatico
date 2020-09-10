using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Tablas
{
    [Serializable()]
    public class EstadosClientes
    {
        public String pOperacion { get; set; }
        public int? pId { get; set; }
        public String pValor { get; set; }
        public String pValorNuevo { get; set; }
    }
}
