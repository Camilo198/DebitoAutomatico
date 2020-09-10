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
    public class EquivalenciasLN
    {
        public String Error { get; set; }

        public List<Equivalencias> consultar(Equivalencias objEntidad)
        {
            EquivalenciasAD objConsultor = new EquivalenciasAD();
            List<Equivalencias> lista = new List<Equivalencias>();
            lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public int borrar(Equivalencias objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ELIMINAR;
            int cuenta = -1;
            EquivalenciasAD objConsultor = new EquivalenciasAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int insertar(Equivalencias objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            EquivalenciasAD objConsultor = new EquivalenciasAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizar(Equivalencias objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            EquivalenciasAD objConsultor = new EquivalenciasAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public DataTable consultarEquivalencias(String TipoArchivo, String TipoLinea)
        {
            EquivalenciasAD objEAAD = new EquivalenciasAD();
            DataTable tabla = objEAAD.consultarEquivalencias(TipoArchivo, TipoLinea);
            Error = objEAAD.Error;
            return tabla;
        }

        public DataTable consultarEquivalenciasXTipoArchivo(String TipoArchivo)
        {
            EquivalenciasAD objEAAD = new EquivalenciasAD();
            DataTable tabla = objEAAD.consultarEquivalenciasXTipoArchivo(TipoArchivo);
            Error = objEAAD.Error;
            return tabla;
        } 
    }
}
