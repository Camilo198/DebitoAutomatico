using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Tablas
{
    [Serializable()]
    public class Banco
    {
        public String pOperacion { get; set; }
        public int? pId { get; set; }
        public bool? pActivo { get; set; }
        public bool? pDebito { get; set; }
        public String pCodigo { get; set; }
        public String pNombre { get; set; }
        public String pNit { get; set; }
        public String pCorreoControl { get; set; }
        public String pCorreoEnvio { get; set; }
        public String pRemitente { get; set; }
        public int? pIdRuta { get; set; }
        public int? pIdPrenota { get; set; }
        public int? pIdPrenotaManual { get; set; }
        public int? pIdDebito { get; set; }
        public int? pIdDebitoManual { get; set; }
        public int? pIdPagos { get; set; }
        public int? pIdErrores { get; set; }
        public int? pIdRecibidos { get; set; }
        public int? pIdHistorico { get; set; }

    }
}
