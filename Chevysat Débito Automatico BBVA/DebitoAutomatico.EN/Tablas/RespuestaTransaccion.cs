using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Tablas
{
    [Serializable()]
    public class RespuestaTransaccion
    {
        public String pOperacion { get; set; }
        public int? pId { get; set; }
        public int? pBanco { get; set; }
        public String pCodigo { get; set; }
        public String pRespuesta{ get; set; }
        public String pEstadoRespuesta { get; set; }
        public int? pEstadoPrenota { get; set; }
        public int? pEstadoDebito { get; set; }
        public int? pEnvioCorreo { get; set; }

    }
}
