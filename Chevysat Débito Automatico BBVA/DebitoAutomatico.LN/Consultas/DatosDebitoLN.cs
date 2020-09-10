using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;

namespace DebitoAutomatico.LN.Consultas
{
    public class DatosDebitoLN
    {
        public String Error { get; set; }

        public List<DatosDebito> consultar(DatosDebito objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR_2;
            DatosDebitoAD objConsultor = new DatosDebitoAD();
            List<DatosDebito> lista = new List<DatosDebito>();
            lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public List<DatosDebito> consultarFecha(DatosDebito objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DatosDebitoAD objConsultor = new DatosDebitoAD();
            List<DatosDebito> lista = new List<DatosDebito>();
            lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public int insertar(DatosDebito objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            DatosDebitoAD objConsultor = new DatosDebitoAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizar(DatosDebito objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            DatosDebitoAD objConsultor = new DatosDebitoAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizarCuentas(DatosDebito objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR_2;
            int cuenta = -1;
            DatosDebitoAD objConsultor = new DatosDebitoAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }


        public int actualizarEstado(DatosDebito objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR_3;
            int cuenta = -1;
            DatosDebitoAD objConsultor = new DatosDebitoAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizarIntentos(DatosDebito objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR_4;
            int cuenta = -1;
            DatosDebitoAD objConsultor = new DatosDebitoAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }


        public DataSet consultar(String Estado)
        {
            return new DatosDebitoAD().consultarEstados(Estado);
        }

        public DataSet ConsultarxProceso(String Estado, String Banco, String Fecha, String Procedimiento)
        {
            return new DatosDebitoAD().ConsultarxProceso(Estado, Banco, Fecha, Procedimiento);
        }

        public DataSet ConsultaDebito(String Estado, String Banco, String BancoDebita, String Fecha, String FechaAnoMes, int DiaHoyHabil, bool Sus, bool Adj, bool Gan, bool CuoxD)
        {
            return new DatosDebitoAD().ConsultarValorDebito(Estado, Banco, BancoDebita, Fecha, FechaAnoMes, DiaHoyHabil, Sus, Adj, Gan, CuoxD);
        }

        public DataSet consultarContratosBusqueda(Int32 TipoCliente, String Contrato, String Identificacion)
        {
            return new DatosDebitoAD().consultarContratosBusqueda(TipoCliente, Contrato, Identificacion);
        }
    }
}
