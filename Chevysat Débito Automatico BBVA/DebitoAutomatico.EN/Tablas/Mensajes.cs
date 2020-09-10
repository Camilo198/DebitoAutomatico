using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Tablas
{
    [Serializable()]
    public class Mensajes
    {
        public String pOperacion { get; set; }
        public int? pId { get; set; }
        public String pTipoContrato { get; set; }
        public int? pEstadoDebito { get; set; }
        public int? pMotivo { get; set; }
        public String pAsunto { get; set; }
        public String pMensaje { get; set; }
    }
}
