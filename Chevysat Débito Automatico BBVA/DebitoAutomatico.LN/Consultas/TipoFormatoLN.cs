using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;

namespace DebitoAutomatico.LN.Consultas
{
    public class TipoFormatoLN
    {
        public String Error { get; set; }

        public List<TipoFormato> consultarTipoF(TipoFormato objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            TipoFormatoAD objConsultor = new TipoFormatoAD();
            List<TipoFormato> lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }


        public List<TipoFormato> consultarFormatoExiste(TipoFormato objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR_2;
            TipoFormatoAD objConsultor = new TipoFormatoAD();
            List<TipoFormato> lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public int insertar(TipoFormato objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            TipoFormatoAD objConsultor = new TipoFormatoAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizar(TipoFormato objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            TipoFormatoAD objConsultor = new TipoFormatoAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int eliminar(TipoFormato objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ELIMINAR;
            int cuenta = -1;
            TipoFormatoAD objConsultor = new TipoFormatoAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }
    }
}
