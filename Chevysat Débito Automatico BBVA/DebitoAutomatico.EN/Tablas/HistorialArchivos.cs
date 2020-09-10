using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Tablas
{
    [Serializable()]
    public class HistorialArchivos
    {
        public String pOperacion { get; set; }
        public String pFecha { get; set; }
        public String pCodigoBanco { get; set; }
        public String pTipoArchivo { get; set; }
        public String pContrato { get; set; }
        public String pIdCliente { get; set; }
        public String pValor { get; set; }
        public String pNombreArchivo{ get; set; }
        public String pConsecutivo { get; set; }
        public bool? pManual { get; set; }
        public String pEstado { get; set; }
        public String pUsuarioModifico { get; set; }
        public String pRespuesta { get; set; }
        public String pCausal { get; set; }
        public String pTipo_transferencia { get; set; }
        public bool pMarca { get; set; }
        public String pbanco { get; set; }
        public string pNombreCliente { get; set; }
        public string pFechaProceso { get; set; }
    }
}
