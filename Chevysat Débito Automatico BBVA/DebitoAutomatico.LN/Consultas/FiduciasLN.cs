using System;
using System.Collections.Generic;
using System.Text;

using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;

namespace DebitoAutomatico.LN.Consultas
{
    public class FiduciasLN
    {
        public String Error { get; set; }

        public List<Fiducias> consultarFiducias(Fiducias objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            FiduciasAD objConsultor = new FiduciasAD();
            List<Fiducias> lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public int insertar(Fiducias objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            FiduciasAD objConsultor = new FiduciasAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizar(Fiducias objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            FiduciasAD objConsultor = new FiduciasAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int borrar(Fiducias objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ELIMINAR;
            int cuenta = -1;
            FiduciasAD objConsultor = new FiduciasAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }
    }
}
