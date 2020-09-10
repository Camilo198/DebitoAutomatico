using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.LN.Consultas
{
    public class FechasLN
    {
        public String Error { get; set; }

        public List<Fechas> consultarFechas(Fechas objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            FechasAD objConsultor = new FechasAD();
            List<Fechas> lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public int insertar(Fechas objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            FechasAD objConsultor = new FechasAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizar(Fechas objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            FechasAD objConsultor = new FechasAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int eliminar(Fechas objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ELIMINAR;
            int cuenta = -1;
            FechasAD objConsultor = new FechasAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }
    }
}
