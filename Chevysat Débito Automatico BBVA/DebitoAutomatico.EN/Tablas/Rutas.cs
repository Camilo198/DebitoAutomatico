using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Tablas
{
    [Serializable()]
    public class Rutas
    {
        public String pOperacion { get; set; }
        public int? pId { get; set; }
        public String pRuta { get; set; }
        //public String pRutaEntrada { get; set; }
        //public String pRutaSalida { get; set; }
        //public String pRutaSico { get; set; }
        //public String pUsaFtp { get; set; }
        //public String pPassFtp { get; set; }
    }
}
