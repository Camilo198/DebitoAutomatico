using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Tablas
{
    [Serializable()]
    public class Calendario
    {
       public int? pCaleAno { get; set; }
       public int? pCaleDias { get; set; }
       public String pCaleMes { get; set; }
       public String pCaleTipo { get; set; }

    }
}
