using System;
using System.Collections.Generic;
using System.Text;

using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;

namespace DebitoAutomatico.LN.Consultas
{
    public class TipoCuentaLN
    {
        public String Error { get; set; }

        public List<TipoCuenta> consultarTipoC(TipoCuenta objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            TipoCuentaAD objConsultor = new TipoCuentaAD();
            List<TipoCuenta> lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public List<TipoCuenta> consultarTipoCuentaExiste(TipoCuenta objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR_2;
            TipoCuentaAD objConsultor = new TipoCuentaAD();
            List<TipoCuenta> lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public int insertar(TipoCuenta objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            TipoCuentaAD objConsultor = new TipoCuentaAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizar(TipoCuenta objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            TipoCuentaAD objConsultor = new TipoCuentaAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int eliminar(TipoCuenta objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ELIMINAR;
            int cuenta = -1;
            TipoCuentaAD objConsultor = new TipoCuentaAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }
    }
}
