using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;


namespace DebitoAutomatico.LN.Consultas
{
    public class DatosDebitoInconsistenteLN
    {
        public String Error { get; set; }

        public List<DatosDebitoInconsistente> consultarDatos(DatosDebitoInconsistente objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DatosDebitoInconsistenteAD objConsultor = new DatosDebitoInconsistenteAD();
            List<DatosDebitoInconsistente> lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public int insertar(DatosDebitoInconsistente objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            DatosDebitoInconsistenteAD objConsultor = new DatosDebitoInconsistenteAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizar(DatosDebitoInconsistente objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            DatosDebitoInconsistenteAD objConsultor = new DatosDebitoInconsistenteAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
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
        
        public DataTable consultarContratosBusqueda(String Contrato, String Identificacion)
        {
            return new DatosDebitoInconsistenteAD().consultarContratosBusqueda(Contrato, Identificacion);
        }

    }
}
