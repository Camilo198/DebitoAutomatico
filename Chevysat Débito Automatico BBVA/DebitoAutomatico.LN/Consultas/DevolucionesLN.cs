using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.LN.Consultas
{
    public class DevolucionesLN
    {
        public String Error { get; set; }

        public List<Devoluciones> consultarDatos(Devoluciones objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DevolucionesAD objConsultor = new DevolucionesAD();
            List<Devoluciones> lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        //Migracion
        public List<Devoluciones> consultarDatosMigracion(Devoluciones objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR_2;
            DevolucionesAD objConsultor = new DevolucionesAD();
            List<Devoluciones> lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public int insertar(Devoluciones objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            DevolucionesAD objConsultor = new DevolucionesAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }
    }
}
