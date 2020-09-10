using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.LN.Consultas
{
    public class ReanudacionLN
    {
        public String Error { get; set; }

        public DataSet consultar(Reanudacion objEntidad)
        {
            DataSet dsConsulta = new DataSet();
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            ReanudacionAD objConsultor = new ReanudacionAD();
            dsConsulta = objConsultor.ejecutarConsulta(objEntidad);
            Error = objConsultor.Error;
            return dsConsulta;
        }

        public List<Reanudacion> consultarMes(Reanudacion objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            ReanudacionAD objConsultor = new ReanudacionAD();
            List<Reanudacion> lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public int insertar(Reanudacion objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            ReanudacionAD objConsultor = new ReanudacionAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

    }
}
