using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.LN.Consultas
{
    public class CalendarioLN
    {
        public String Error { get; set; }

        public List<Calendario> consultarCalendario(Calendario objEntidad)
        {
            List<Calendario> lista = new CalendarioAD().consultar(objEntidad);
            Error = new CalendarioAD().Error;
            return lista;
        }
    }
}
