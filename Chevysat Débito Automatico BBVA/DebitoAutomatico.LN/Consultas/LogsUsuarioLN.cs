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
    public  class LogsUsuarioLN
    {
        public String Error { get; set; }

        public List<LogsUsuario> consultar(LogsUsuario objEntidad)
        {
            LogsUsuarioAD objConsultor = new LogsUsuarioAD();
            List<LogsUsuario> lista = new List<LogsUsuario>();
            lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public int insertar(LogsUsuario objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            LogsUsuarioAD objConsultor = new LogsUsuarioAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }
    }
}
