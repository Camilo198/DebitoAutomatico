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
    public class TitularCuentaInconsistenteLN
    {
        public String Error { get; set; }

        public DataSet consultarTerceros(TitularCuentaInconsistente objEntidad)
        {
            DataSet TerceroIncon = new DataSet();
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            TitularCuentaInconsistenteAD objConsultor = new TitularCuentaInconsistenteAD();
            TerceroIncon = objConsultor.ejecutarConsulta(objEntidad);
            Error = objConsultor.Error;
            return TerceroIncon;
        }

        public int borrar(TitularCuentaInconsistente objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ELIMINAR;
            int cuenta = -1;
            TitularCuentaInconsistenteAD objConsultor = new TitularCuentaInconsistenteAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }
    }
}
