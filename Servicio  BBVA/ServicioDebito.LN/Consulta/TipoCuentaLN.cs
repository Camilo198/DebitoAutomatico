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
    public class TipoCuentaLN
    {
        public String Error { get; set; }

        public DataSet consultarTipoC(TipoCuenta objEntidad)
        {
            DataSet dsTipoC = new DataSet();
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            TipoCuentaAD objConsultor = new TipoCuentaAD();
            dsTipoC = objConsultor.ejecutarConsulta(objEntidad);
            Error = objConsultor.Error;
            return dsTipoC;
        }
    }
}
