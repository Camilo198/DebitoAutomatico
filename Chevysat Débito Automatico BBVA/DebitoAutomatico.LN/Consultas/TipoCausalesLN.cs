using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;

namespace DebitoAutomatico.LN.Consultas
{
    public class TipoCausalesLN
    {
        public String Error { get; set; }

        public List<TipoCausales> consultarTipoCausal(TipoCausales objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            TipoCausalesAD objConsultor = new TipoCausalesAD();
            List<TipoCausales> lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }


        public List<TipoCausales> consultarTipoCausalesExiste(TipoCausales objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR_2;
            TipoCausalesAD objConsultor = new TipoCausalesAD();
            List<TipoCausales> lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public int insertar(TipoCausales objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            TipoCausalesAD objConsultor = new TipoCausalesAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizar(TipoCausales objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            TipoCausalesAD objConsultor = new TipoCausalesAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int eliminar(TipoCausales objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ELIMINAR;
            int cuenta = -1;
            TipoCausalesAD objConsultor = new TipoCausalesAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }
    }
}
