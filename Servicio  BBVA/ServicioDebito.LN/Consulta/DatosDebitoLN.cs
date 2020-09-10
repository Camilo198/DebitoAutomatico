
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicioDebito.AD.Consultas;
using ServicioDebito.EN.Tablas;
using ServicioDebito.EN;

namespace ServicioDebito.LN.Consulta
{
    public class DatosDebitoLN
    {
        public String Error { get; set; }

        public DataSet consultarDatos(DatosDebito objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DataSet prueba = new DataSet();
            DatosDebitoAD objConsultor = new DatosDebitoAD();
            prueba = objConsultor.ejecutarConsulta(objEntidad);
            return prueba;
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

        public int actualizarEstado(DatosDebito objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR_2;
            int cuenta = -1;
            DatosDebitoAD objConsultor = new DatosDebitoAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizarFecha(DatosDebito objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR_5;
            int cuenta = -1;
            DatosDebitoAD objConsultor = new DatosDebitoAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }
    }
}
