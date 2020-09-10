using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using DebitoAutomatico.AD;
using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;

namespace DebitoAutomatico.LN.Consultas
{
    public class TipoLineaLN
    {
        public DataTable consultarLineas(String TipoArchivo, int IdBanco)
        {
            TipoLineaAD objConsultor = new TipoLineaAD();
            return objConsultor.consultarLineas(TipoArchivo, IdBanco);
        }

        public DataTable consultarLineasDisponibles(String TipoArchivo, int IdBanco)
        {
            TipoLineaAD objConsultor = new TipoLineaAD();
            return objConsultor.consultarLineasDisponibles(TipoArchivo,IdBanco);
        }
    }
}
