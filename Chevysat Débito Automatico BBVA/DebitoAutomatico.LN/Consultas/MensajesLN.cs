using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.LN.Consultas
{
    public class MensajesLN
    {
        public String Error { get; set; }

        public List<Mensajes> consultar(Mensajes objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            MensajesAD objConsultor = new MensajesAD();
            List<Mensajes> lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public int insertar(Mensajes objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            MensajesAD objConsultor = new MensajesAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizar(Mensajes objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            MensajesAD objConsultor = new MensajesAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }
    }
}
