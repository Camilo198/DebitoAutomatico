using System;
using System.Collections.Generic;
using System.Text;

using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;

namespace DebitoAutomatico.LN.Consultas
{
    public class TipoDocumentoLN
    {
        public String Error { get; set; }

        public List<TipoDocumento> consultarTipoD(TipoDocumento objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            TipoDocumentoAD objConsultor = new TipoDocumentoAD();
            List<TipoDocumento> lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

    }
}
