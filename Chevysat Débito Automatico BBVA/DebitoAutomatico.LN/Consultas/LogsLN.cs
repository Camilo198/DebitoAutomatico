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
    public class LogsLN
    {
        public String Error { get; set; }

        public List<Logs> consultar(Logs objEntidad)
        {
            LogsAD objConsultor = new LogsAD();
            List<Logs> lista = new List<Logs>();
            lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public int insertar(Logs objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            LogsAD objConsultor = new LogsAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public DataTable consultarLogs(String TipoArchivo, String TipoProceso, String FechaInicial, String FechaFinal)
        {
            return new LogsAD().consultarLogs(TipoArchivo, TipoProceso, FechaInicial, FechaFinal);
        }
    }
}
