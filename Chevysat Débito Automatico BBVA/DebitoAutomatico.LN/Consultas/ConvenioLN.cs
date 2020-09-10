using System;
using System.Collections.Generic;
using System.Text;

using DebitoAutomatico.AD;
using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;

namespace DebitoAutomatico.LN.Consultas
{
    public class ConvenioLN
    {
        public String Error { get; set; }

        public List<Convenio> consultar(Convenio objEntidad)
        {
            ConvenioAD objConsultor = new ConvenioAD();
            List<Convenio> lista = new List<Convenio>();
            lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public int borrar(Convenio objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ELIMINAR;
            int cuenta = -1;
            ConvenioAD objConsultor = new ConvenioAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int insertar(Convenio objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            ConvenioAD objConsultor = new ConvenioAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }
    }
}
