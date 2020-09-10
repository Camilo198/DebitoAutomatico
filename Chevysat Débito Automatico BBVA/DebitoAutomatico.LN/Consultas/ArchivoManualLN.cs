using System;
using System.Collections.Generic;
using System.Text;

using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;
using System.Data;

namespace DebitoAutomatico.LN.Consultas
{
    public class ArchivoManualLN
    {
        public String Error { get; set; }

        public List<ArchivoManual> consultar(ArchivoManual objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            ArchivoManualAD objConsultor = new ArchivoManualAD();
            List<ArchivoManual> lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public int insertar(ArchivoManual objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            ArchivoManualAD objConsultor = new ArchivoManualAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizar(ArchivoManual objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            ArchivoManualAD objConsultor = new ArchivoManualAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int borrar(ArchivoManual objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ELIMINAR;
            int cuenta = -1;
            ArchivoManualAD objConsultor = new ArchivoManualAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int borrarTodo(ArchivoManual objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ELIMINAR_TODO;
            int cuenta = -1;
            ArchivoManualAD objConsultor = new ArchivoManualAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public DataSet ConsultarArchivoManual(String Estado, String Banco, String Fecha, String BancoDebita)
        {
            return new ArchivoManualAD().ConsultarArchivoManual(Estado, Banco, Fecha, BancoDebita);
        }

        public DataSet ConsultarValorDebito(ArchivoManual objEntidad)
        {
            return new ArchivoManualAD().ConsultaDebitoManual(objEntidad);
        }
        
    }
}
