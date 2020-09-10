using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Tablas
{
    [Serializable()]
    public class DatosDebitoInconsistente
    {
        public String pOperacion { get; set; }
        public int? pId { get; set; }
        public String pContrato { get; set; }
        public int? pDigito { get; set; }
        public int? pEstado { get; set; }
        public int? pIdBanco { get; set; }
        public int? pTipoCuenta { get; set; }
        public String pNumeroCuenta { get; set; }
        public bool? pTercero { get; set; }
        public int? pIdTitularCuenta { get; set; }
        public int? pFormato { get; set; }
        public String pDireccion_Ip { get; set; }
        public int? pTipoInconsistencia { get; set; }
        public String pObservaciones { get; set; }
        public int? pFechaDebito { get; set; }
    }
}
