using System;
using System.Collections.Generic;
using System.Text;

using DebitoAutomatico.AD;
using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;

namespace DebitoAutomatico.LN.Consultas
{
    public class ConfiguracionLN
    {
        public String Error { get; set; }

        public List<ConfiguracionEst> consultar(ConfiguracionEst objEntidad)
        {
            ConfiguracionAD objConsultor = new ConfiguracionAD();
            List<ConfiguracionEst> lista = new List<ConfiguracionEst>();
            lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public int borrarLineas(ConfiguracionEst objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ELIMINAR;
            int cuenta = -1;
            ConfiguracionAD objConsultor = new ConfiguracionAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int insertarLineas(ConfiguracionEst objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            ConfiguracionAD objConsultor = new ConfiguracionAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }
    }
}
