using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioDebito.EN.Tablas
{
    [Serializable()]
    public class Usuario
    {
        public String pOperacion { get; set; }
        public Int32? pId { get; set; }
        public String pUsuario { get; set; }
        public String pPassword { get; set; }
        public Int32? pIdPerfil { get; set; }
        public bool? pHabilita { get; set; }
    }
}
