using System;
using System.Collections.Generic;
using System.Text;

using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;


namespace DebitoAutomatico.LN.Consultas
{
    public class EstadosClientesLN
    {
        public String Error { get; set; }

        public List<EstadosClientes> consultarEstado(EstadosClientes objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            EstadosClientesAD objConsultor = new EstadosClientesAD();
            List<EstadosClientes> lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public List<EstadosClientes> consultarTipoEstado(EstadosClientes objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR_2;
            EstadosClientesAD objConsultor = new EstadosClientesAD();
            List<EstadosClientes> lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public List<EstadosClientes> consultarEstadoExiste(EstadosClientes objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR_3;
            EstadosClientesAD objConsultor = new EstadosClientesAD();
            List<EstadosClientes> lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public int insertar(EstadosClientes objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            EstadosClientesAD objConsultor = new EstadosClientesAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizar(EstadosClientes objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            EstadosClientesAD objConsultor = new EstadosClientesAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int eliminar(EstadosClientes objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ELIMINAR;
            int cuenta = -1;
            EstadosClientesAD objConsultor = new EstadosClientesAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }
    }
}
