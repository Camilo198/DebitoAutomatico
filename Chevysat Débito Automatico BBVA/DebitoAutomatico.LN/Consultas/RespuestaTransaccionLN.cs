using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;

namespace DebitoAutomatico.LN.Consultas
{
    public class RespuestaTransaccionLN
    {
        public String Error { get; set; }

        public List<RespuestaTransaccion> consultarRespuestas(RespuestaTransaccion objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            RespuestaTransaccionAD objConsultor = new RespuestaTransaccionAD();
            List<RespuestaTransaccion> lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public int insertar(RespuestaTransaccion objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            RespuestaTransaccionAD objConsultor = new RespuestaTransaccionAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizar(RespuestaTransaccion objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            RespuestaTransaccionAD objConsultor = new RespuestaTransaccionAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int borrar(RespuestaTransaccion objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ELIMINAR;
            int cuenta = -1;
            RespuestaTransaccionAD objConsultor = new RespuestaTransaccionAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public DataSet consultar(RespuestaTransaccion objRT)
        {
            return new RespuestaTransaccionAD().consultarRespuestasDebito(objRT);
        }

    }
}
