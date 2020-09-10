using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;

namespace DebitoAutomatico.LN.Consultas
{
    public class CorreosLN
    {
        public String Error { get; set; }

        public int insertar(Correos objEntidad)
        {
            objEntidad.Operacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            CorreosAD objConsultor = new CorreosAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }
    }
}
