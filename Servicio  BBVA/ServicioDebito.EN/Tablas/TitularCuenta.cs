using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioDebito.EN.Tablas
{
    [Serializable()]
    public class TitularCuenta
    {
        public String pOperacion { get; set; }
        public int? pId { get; set; }
        public String pNombre { get; set; }
        public int? pTipoIdentificacion { get; set; }
        public String pNumeroIdentificacion { get; set; }
        public String pFechaIngreso { get; set; }
        public String pFechaFinalizacion { get; set; }
        public int? pMontoMaximo { get; set; }
        public int? pIntentos { get; set; }

        
    }
}
