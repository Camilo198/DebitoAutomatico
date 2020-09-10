using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;

namespace DebitoAutomatico.LN.Consultas
{
    public class TitularCuentaInconsistenteLN
    {
        public String Error { get; set; }

        public List<TitularCuentaInconsistente> consultarTerceros(TitularCuentaInconsistente objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            TitularCuentaInconsistenteAD objConsultor = new TitularCuentaInconsistenteAD();
            List<TitularCuentaInconsistente> lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public int insertar(TitularCuentaInconsistente objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            TitularCuentaInconsistenteAD objConsultor = new TitularCuentaInconsistenteAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizar(TitularCuentaInconsistente objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            TitularCuentaInconsistenteAD objConsultor = new TitularCuentaInconsistenteAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
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
