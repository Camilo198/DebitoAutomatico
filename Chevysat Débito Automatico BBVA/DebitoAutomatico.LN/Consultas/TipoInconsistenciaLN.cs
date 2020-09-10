using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;

namespace DebitoAutomatico.LN.Consultas
{
    public class TipoInconsistenciaLN
    {
        public String Error { get; set; }

        public List<TipoInconsistencia> consultarTipoIn(TipoInconsistencia objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            TipoInconsistenciaAD objConsultor = new TipoInconsistenciaAD();
            List<TipoInconsistencia> lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public List<TipoInconsistencia> consultarTipoInconsistenciaExiste(TipoInconsistencia objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR_2;
            TipoInconsistenciaAD objConsultor = new TipoInconsistenciaAD();
            List<TipoInconsistencia> lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public int insertar(TipoInconsistencia objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            TipoInconsistenciaAD objConsultor = new TipoInconsistenciaAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizar(TipoInconsistencia objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            TipoInconsistenciaAD objConsultor = new TipoInconsistenciaAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int eliminar(TipoInconsistencia objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ELIMINAR;
            int cuenta = -1;
            TipoInconsistenciaAD objConsultor = new TipoInconsistenciaAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

    }
}
