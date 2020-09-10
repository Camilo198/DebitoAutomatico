using System;
using System.Collections.Generic;
using System.Text;

using DebitoAutomatico.AD;
using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;

namespace DebitoAutomatico.LN.Consultas
{
    public class TablasEquivalenciasLN
    {
        public String Error { get; set; }

        public List<TablasEquivalencias> consultar(TablasEquivalencias objEntidad)
        {
            TablasEquivalenciasAD objConsultor = new TablasEquivalenciasAD();
            List<TablasEquivalencias> lista = new List<TablasEquivalencias>();
            lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public int borrar(TablasEquivalencias objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ELIMINAR;
            int cuenta = -1;
            TablasEquivalenciasAD objConsultor = new TablasEquivalenciasAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int insertar(TablasEquivalencias objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            TablasEquivalenciasAD objConsultor = new TablasEquivalenciasAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizar(TablasEquivalencias objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            TablasEquivalenciasAD objConsultor = new TablasEquivalenciasAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }
    }
}
