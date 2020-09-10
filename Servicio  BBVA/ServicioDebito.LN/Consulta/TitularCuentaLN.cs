using ServicioDebito.AD.Consultas;
using ServicioDebito.EN;
using ServicioDebito.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioDebito.LN.Consulta
{
    public class TitularCuentaLN
    {
        public String Error { get; set; }

        public DataSet consultarTerceros(TitularCuenta objEntidad)
        {
            DataSet Terceros = new DataSet();
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            TitularCuentaAD objConsultor = new TitularCuentaAD();
            Terceros = objConsultor.ejecutarConsulta(objEntidad);
            return Terceros;
        }

        public int insertar(TitularCuenta objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            TitularCuentaAD objConsultor = new TitularCuentaAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizar(TitularCuenta objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            TitularCuentaAD objConsultor = new TitularCuentaAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }
    }
}
