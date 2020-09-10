using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Tablas
{
    [Serializable()]
    public class Perfil
    {
        public String pOperacion { get; set; }
        public Int32? pId { get; set; }
        public String pPerfil { get; set; }
    }
}
