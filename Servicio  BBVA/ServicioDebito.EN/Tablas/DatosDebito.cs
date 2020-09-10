using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioDebito.EN.Tablas
{
    [Serializable()]
    public class DatosDebito
    {
        public String pOperacion { get; set; }
        public int? pId { get; set; }
        public String pContrato { get; set; }
        public int? pDigito { get; set; }
        public int? pEstado { get; set; }
        public int? pIdBanco { get; set; }
        public int? pTipoCuenta { get; set; }
        public String pNumeroCuenta { get; set; }
        public int? pIdFormatoDebito { get; set; }
        public int? pIdFormatoCancelacion { get; set; }
        public int? pIdTitularCuenta { get; set; }
        public String pDireccionIp { get; set; }
        public bool? pTercero { get; set; }
        public bool? pSuspendido { get; set; }
        public String pFechaInicioSus { get; set; }
        public String pFechaFinSus { get; set; }
        public int? pIntentos { get; set; }
        public int? pFechaDebito { get; set; }
        

        public String pNombreEstado { get; set; }
    }
}
