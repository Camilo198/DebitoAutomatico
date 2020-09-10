using System;
using System.Collections.Generic;
using System.Text;

using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;

namespace DebitoAutomatico.LN.Consultas
{
    public class RutasLN
    {
        public String Error { get; set; }

        public List<Rutas> consultarRuta(Rutas objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            RutasAD objConsultor = new RutasAD();
            List<Rutas> lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        //public int insertar(Rutas objEntidad)
        //{
        //    objEntidad.pOperacion = TiposConsultas.INSERTAR;
        //    int cuenta = -1;
        //    RutasAD objConsultor = new RutasAD();
        //    cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
        //    Error = objConsultor.Error;
        //    return cuenta;
        //}

        //public int actualizar(Rutas objEntidad)
        //{
        //    objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
        //    int cuenta = -1;
        //    RutasAD objConsultor = new RutasAD();
        //    cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
        //    Error = objConsultor.Error;
        //    return cuenta;
        //}

        //public int borrar(Rutas objEntidad)
        //{
        //    objEntidad.pOperacion = TiposConsultas.ELIMINAR;
        //    int cuenta = -1;
        //    RutasAD objConsultor = new RutasAD();
        //    cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
        //    Error = objConsultor.Error;
        //    return cuenta;
        //}

    }
}
