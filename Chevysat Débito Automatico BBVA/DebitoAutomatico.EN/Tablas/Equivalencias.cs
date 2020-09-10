using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Tablas
{
    [Serializable()]
    public class Equivalencias
    {
        public String pOperacion { get; set; }

        public int? pId { get; set; }
        public int? pIdEstructuraArchivo { get; set; }
        public int? pIdTablasEquivalencias { get; set; }
        public int? pIdCamposEquivalencias { get; set; }
    }
}
