using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebitoAutomatico.EN.Tablas
{
    [Serializable()]
    public class ActualizaCliente
    {
        public String pOperacion { get; set; }
        public int? pId { get; set; }
        public String pContrato { get; set; }
        public int? pIdTitularCuenta { get; set; }
        public int? pIdBanco { get; set; }
        public int? pIdTipoCuenta { get; set; }
        public String pNumeroCuenta { get; set; }
        public String pDireccionIp { get; set; }
        public String pUsuario { get; set; }
        
    }
}
