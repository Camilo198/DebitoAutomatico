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
    public class CamposEquivalenciasLN
    {
        public String Error { get; set; }

        public List<CamposEquivalencias> consultar(CamposEquivalencias objEntidad)
        {
            CamposEquivalenciasAD objConsultor = new CamposEquivalenciasAD();
            List<CamposEquivalencias> lista = new List<CamposEquivalencias>();
            lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public int borrar(CamposEquivalencias objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ELIMINAR;
            int cuenta = -1;
            CamposEquivalenciasAD objConsultor = new CamposEquivalenciasAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int insertar(CamposEquivalencias objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            CamposEquivalenciasAD objConsultor = new CamposEquivalenciasAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizar(CamposEquivalencias objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            CamposEquivalenciasAD objConsultor = new CamposEquivalenciasAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public DataTable consultarCampoEquivalencias(String tipoArchivo, String codBanco)
        {
            CamposEquivalenciasAD objCE = new CamposEquivalenciasAD();
            DataTable tabla = objCE.consultarCampoEquivalencias(tipoArchivo, codBanco);
            Error = objCE.Error;
            return tabla;
        }
    }
}
