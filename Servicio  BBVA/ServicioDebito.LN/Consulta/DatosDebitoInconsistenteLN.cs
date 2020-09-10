using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ServicioDebito.AD.Consultas;
using ServicioDebito.EN;
using ServicioDebito.EN.Tablas;
using System.Data;

namespace ServicioDebito.LN.Consulta
{
    public class DatosDebitoInconsistenteLN
    {
        public String Error { get; set; }

        public DataSet consultarDatos(DatosDebitoInconsistente objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DataSet prueba = new DataSet();
            DatosDebitoInconsistenteAD objConsultor = new DatosDebitoInconsistenteAD();
            prueba = objConsultor.ejecutarConsulta(objEntidad);
            return prueba;
        }

        public int borrar(DatosDebitoInconsistente objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ELIMINAR;
            int cuenta = -1;
            DatosDebitoInconsistenteAD objConsultor = new DatosDebitoInconsistenteAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }
        
    }
}
