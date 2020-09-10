using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DebitoAutomatico.AD;
using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;

namespace DebitoAutomatico.LN.Consultas
{
    public class LogsParametrosLN
    {
        public String Error { get; set; }

        public List<LogsParametros> consultar(LogsParametros objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            LogsParametrosAD objConsultor = new LogsParametrosAD();
            List<LogsParametros> lista = new List<LogsParametros>();
            lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public int insertar(LogsParametros objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            LogsParametrosAD objConsultor = new LogsParametrosAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }
    }
}
