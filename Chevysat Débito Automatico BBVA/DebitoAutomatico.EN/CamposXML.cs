using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN
{
    [Serializable()]
    public class CamposXML
    {
        public String pTabla { get; set; }
        public String pCampo { get; set; }
        public String pValor { get; set; }
    }
}
