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
    public class FechasLN
    {
        public String Error { get; set; }

        public DataSet consultarFechas(Fechas objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR_2;
            FechasAD objConsultor = new FechasAD();
            DataSet dsFechas = objConsultor.ejecutarConsulta(objEntidad);
            Error = objConsultor.Error;
            return dsFechas;
        }
    }
}
