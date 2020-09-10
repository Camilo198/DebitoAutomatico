using ServicioDebito.AD.Consultas;
using ServicioDebito.EN;
using ServicioDebito.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioDebito.LN.Consulta
{
    public class TipoFormatoLN
    {
        public String Error { get; set; }

        public DataSet consultarTipoF(TipoFormato objEntidad)
        {
            DataSet TipoF = new DataSet();
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            TipoFormatoAD objConsultor = new TipoFormatoAD();
            TipoF = objConsultor.ejecutarConsulta(objEntidad);
            Error = objConsultor.Error;
            return TipoF;
        }
    }
}
