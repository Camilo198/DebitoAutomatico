using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DebitoAutomatico.AD;
using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;
namespace DebitoAutomatico.LN.Consultas
{
    public  class DebitoParcialLN
    {
        public String Error { get; set; }

        public List<DebitoParcial> consultar(DebitoParcial objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DebitoParcialAD objConsultor = new DebitoParcialAD();
            List<DebitoParcial> lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public int insertar(DebitoParcial objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            DebitoParcialAD objConsultor = new DebitoParcialAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizar(DebitoParcial objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            DebitoParcialAD objConsultor = new DebitoParcialAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int borrar(DebitoParcial objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ELIMINAR;
            int cuenta = -1;
            DebitoParcialAD objConsultor = new DebitoParcialAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }
    }
}
