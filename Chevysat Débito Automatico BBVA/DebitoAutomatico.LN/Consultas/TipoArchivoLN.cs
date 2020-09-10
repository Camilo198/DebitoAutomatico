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
    public class TipoArchivoLN
    {
        public String Error { get; set; }

        public List<TipoArchivo> consultar(TipoArchivo objArchivos)
        {
            objArchivos.pOperacion = TiposConsultas.CONSULTAR;
            TipoArchivoAD objConsultor = new TipoArchivoAD();
            List<TipoArchivo> lista = new List<TipoArchivo>();
            lista = objConsultor.consultar(objArchivos);
            Error = objConsultor.Error;
            return lista;
        }


        public List<TipoArchivo> consultarProceso(TipoArchivo objArchivos)
        {
            objArchivos.pOperacion = TiposConsultas.CONSULTAR_2;
            TipoArchivoAD objConsultor = new TipoArchivoAD();
            List<TipoArchivo> lista = new List<TipoArchivo>();
            lista = objConsultor.consultar(objArchivos);
            Error = objConsultor.Error;
            return lista;
        }

        public DataTable consultarEntidades()
        {
            TipoArchivoAD objTA = new TipoArchivoAD();
            DataTable tabla = objTA.consultarEntidades();
            Error = objTA.Error;
            return tabla;
        }

    }
}
