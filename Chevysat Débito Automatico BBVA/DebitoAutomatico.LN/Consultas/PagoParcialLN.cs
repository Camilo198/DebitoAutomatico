using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.LN.Consultas
{
    public class PagoParcialLN
    {
        public String Error { get; set; }

        public List<PagoParcial> consultar(PagoParcial objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            PagoParcialAD objConsultor = new PagoParcialAD();
            List<PagoParcial> lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public int insertar(PagoParcial objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            PagoParcialAD objConsultor = new PagoParcialAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizar(PagoParcial objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            PagoParcialAD objConsultor = new PagoParcialAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int borrar(PagoParcial objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ELIMINAR;
            int cuenta = -1;
            PagoParcialAD objConsultor = new PagoParcialAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

    }
}
