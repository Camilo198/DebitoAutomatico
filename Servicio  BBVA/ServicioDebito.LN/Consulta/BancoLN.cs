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
    public class BancoLN
    {
        public String Error { get; set; }

        public DataSet consultarCorreo(Banco objEntidad)
        {
            DataSet DsBanco = new DataSet();
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            BancoAD objConsultor = new BancoAD();
            DsBanco = objConsultor.ejecutarConsulta(objEntidad);
            Error = objConsultor.Error;
            return DsBanco;
        }

        public DataSet consultarBanco(Banco objEntidad)
        {
            DataSet DsBanco = new DataSet();
            objEntidad.pOperacion = TiposConsultas.CONSULTAR_2;
            BancoAD objConsultor = new BancoAD();
            DsBanco = objConsultor.ejecutarConsulta(objEntidad);
            Error = objConsultor.Error;
            return DsBanco;
        }
    }
}
