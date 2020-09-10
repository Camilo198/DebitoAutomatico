using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioDebito.EN.Tablas
{
    [Serializable()]
    public class Cesion
    {
        //Cesi
        public String CesiNueCla { get; set; }
        public String CesiNueIde { get; set; }
        public String CesiNuePer { get; set; }

        public String CesiAntCla { get; set; }
        public String CesiAntIde { get; set; }

        public String CesiCons { get; set; }
        public String CesiTipDoc { get; set; }
        public String CesiNroDoc { get; set; }

        public String CalculaCons { get; set; }

        //Peaf
        public String PeafIdeClase { get; set; }
        public String PeafIdeNro { get; set; }
        public String PeafNombres { get; set; }
        public String PeafApellidos { get; set; }

        //Cias
        public String CiasIdeClase { get; set; }
        public String CiasIdeNro { get; set; }
        public String CiasNombre { get; set; }

        //Indi
        public String IndiNroCon { get; set; }

        //Filtro
        public String TipoIdDebito { get; set; }
        public String NumeroDocDebito { get; set; }
        public String Grupo { get; set; }
        public String Numero { get; set; }
        public String Nivel { get; set; }
    }
}
