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
    public class HistorialProcesoUsuarioLN
    {
        public String Error { get; set; }

        public List<HistorialProcesoUsuario> consultar(HistorialProcesoUsuario objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            HistorialProcesoUsuarioAD objConsultor = new HistorialProcesoUsuarioAD();
            List<HistorialProcesoUsuario> lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public int insertar(HistorialProcesoUsuario objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            HistorialProcesoUsuarioAD objConsultor = new HistorialProcesoUsuarioAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizar(HistorialProcesoUsuario objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            HistorialProcesoUsuarioAD objConsultor = new HistorialProcesoUsuarioAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public DataTable consultarEstadoDebitado(String BancoDebita, String Fecha)
        {
            return new HistorialProcesoUsuarioAD().consultarEstadoDebitado(BancoDebita, Fecha);
        }

        public DataTable consultarFechasTransaccionesDebitado(String BancoDebita)
        {
            return new HistorialProcesoUsuarioAD().consultarFechasTransaccionesDebitado(BancoDebita);
        }

        public DataTable consultarFechasTransaccionesDebitoProceso(String Estado)
        {
            return new HistorialProcesoUsuarioAD().consultarFechasTransaccionesDebitoProceso(Estado);
        }

        public DataTable consultarEstadoDebitoEnProceso(String Fecha, String Estado)
        {
            return new HistorialProcesoUsuarioAD().consultarEstadoDebitoEnProceso(Fecha, Estado);
        }

        public DataTable reversar(String EstadoD, String IDTC, String IDHPU, String Causal, String EstadoFinalD, String EstadoRespuesta)
        {
            return new HistorialProcesoUsuarioAD().reversar(EstadoD, IDTC, IDHPU, Causal, EstadoFinalD, EstadoRespuesta);
        }

        public DataTable consultarDebitoXFecha(String Fecha, String BancoDebita)
        {
            return new HistorialProcesoUsuarioAD().consultarDebitoXFecha(Fecha, BancoDebita);
        }

        public DataTable consultarPrenotaXFecha(String Fecha, String BancoDebita)
        {   
            return new HistorialProcesoUsuarioAD().consultarPrenotaXFecha(Fecha, BancoDebita);
        }
    }
}
