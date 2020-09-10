using System;
using System.Collections.Generic;
using System.Text;

using DebitoAutomatico.AD;
using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;
using System.Data;

namespace DebitoAutomatico.LN.Consultas
{
    public class EstructuraArchivoLN
    {
        public String Error { get; set; }

        public List<EstructuraArchivo> consultar(EstructuraArchivo objEntidad)
        {
            EstructuraArchivoAD objConsultor = new EstructuraArchivoAD();
            List<EstructuraArchivo> lista = new List<EstructuraArchivo>();
            lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public int borrar(EstructuraArchivo objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ELIMINAR;
            int cuenta = -1;
            EstructuraArchivoAD objConsultor = new EstructuraArchivoAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int insertar(EstructuraArchivo objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            EstructuraArchivoAD objConsultor = new EstructuraArchivoAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizar(EstructuraArchivo objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            EstructuraArchivoAD objConsultor = new EstructuraArchivoAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }


        public DataTable consultarEstructuraArchivo(String tipoLinea, String tipoProceso, Int32 IdBanco)
        {
            EstructuraArchivoAD objEAAD = new EstructuraArchivoAD();
            DataTable tabla = objEAAD.consultarEstructuraArchivo(tipoLinea, tipoProceso, IdBanco);
            Error = objEAAD.Error;
            return tabla;
        }

        public DataTable consultarEstructuraArchivo(String tipoProceso, Int32 IdBanco)
        {
            EstructuraArchivoAD objEAAD = new EstructuraArchivoAD();
            DataTable tabla = objEAAD.consultarEstructuraArchivo(tipoProceso, IdBanco);
            Error = objEAAD.Error;
            return tabla;
        }

        public DataTable consultarSumaCampos(String tipoProceso, int IdBanco)
        {
            EstructuraArchivoAD objEAAD = new EstructuraArchivoAD();
            DataTable tabla = objEAAD.consultarSumaCampos(tipoProceso, IdBanco);
            Error = objEAAD.Error;
            return tabla;
        }
    }
}
