using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebitoAutomatico.LN.Consulta
{
    public class ActualizaClienteLN
    {
        public String Error { get; set; }

        public List<ActualizaCliente> consultarDatos(ActualizaCliente objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            ActualizaClienteAD objConsultor = new ActualizaClienteAD();
            List<ActualizaCliente> lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public int eliminar(ActualizaCliente objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ELIMINAR;
            int cuenta = -1;
            ActualizaClienteAD objConsultor = new ActualizaClienteAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }
    }
}
