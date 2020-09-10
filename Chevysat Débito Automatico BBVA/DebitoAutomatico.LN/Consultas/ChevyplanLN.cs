using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;

namespace DebitoAutomatico.LN.Consultas
{
    public class ChevyplanLN
    {
        public String Error { get; set; }

        public List<Chevyplan> consultarChevyplan(Chevyplan objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            ChevyplanAD objConsultor = new ChevyplanAD();
            List<Chevyplan> lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public int insertar(Chevyplan objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            ChevyplanAD objConsultor = new ChevyplanAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizar(Chevyplan objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            ChevyplanAD objConsultor = new ChevyplanAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }
    }
}
