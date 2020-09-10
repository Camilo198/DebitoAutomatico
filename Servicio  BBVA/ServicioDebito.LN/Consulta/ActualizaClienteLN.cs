using ServicioDebito.AD.Consultas;
using ServicioDebito.EN;
using ServicioDebito.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioDebito.LN.Consulta
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

        public int insertar(ActualizaCliente objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            ActualizaClienteAD objConsultor = new ActualizaClienteAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizar(ActualizaCliente objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            ActualizaClienteAD objConsultor = new ActualizaClienteAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }
    }
}
