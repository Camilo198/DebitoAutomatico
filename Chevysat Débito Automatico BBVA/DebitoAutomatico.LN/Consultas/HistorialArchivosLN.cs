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
    public class HistorialArchivosLN
    {
        public String Error { get; set; }

        public List<HistorialArchivos> consultar(HistorialArchivos objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            HistorialArchivosAD objConsultor = new HistorialArchivosAD();
            List<HistorialArchivos> lista = new List<HistorialArchivos>();
            lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public List<HistorialArchivos> consultarConsecutivo(HistorialArchivos objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR_2;
            HistorialArchivosAD objConsultor = new HistorialArchivosAD();
            List<HistorialArchivos> lista = new List<HistorialArchivos>();
            lista = objConsultor.consultarCon(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public int insertar(HistorialArchivos objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            HistorialArchivosAD objConsultor = new HistorialArchivosAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizarPrenota(HistorialArchivos objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            HistorialArchivosAD objConsultor = new HistorialArchivosAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizarDebito(HistorialArchivos objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR_2;
            int cuenta = -1;
            HistorialArchivosAD objConsultor = new HistorialArchivosAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

       
        public DataSet consultarArchivos(HistorialArchivos objRT)
        {
            return new HistorialArchivosAD().consultarArchivosFecha(objRT);
        }

        public List<HistorialArchivos> consultarArchivosCorreo(HistorialArchivos objRT)
        {
            return new HistorialArchivosAD().consultarcorreo(objRT);
        }

        public DataTable consultarMaxLote()
        {
            return new HistorialArchivosAD().consultarMaxLote();
        }

        public DataTable consultarConsecutivoXBanco(String Banco, String TipoArchivoS, String Fecha)
        {
            return new HistorialArchivosAD().consultarConsecutivoXBanco(Banco, TipoArchivoS, Fecha);
        }

        public DataTable consultarLineasConsecutivo(String Banco, String TipoArchivoS, String Fecha, String Consecutivo)
        {
            return new HistorialArchivosAD().consultarLineasConsecutivo(Banco, TipoArchivoS, Fecha, Consecutivo);
        }
    }
}
