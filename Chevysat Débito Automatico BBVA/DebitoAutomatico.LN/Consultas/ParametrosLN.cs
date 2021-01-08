using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebitoAutomatico.LN.Consulta
{
   
    public class ParametrosLN
    {
        private String SP_Ban_Parametros = "pa_Ban_Parametros";
        public String InsertarParametrosLN(ParametrosEN objEntidad)
        {
            return new ParametrosAD().InsertarParametrosAD(SP_Ban_Parametros, objEntidad);
        }
        public IList<ParametrosEN> ConsultarParametrosLN(ParametrosEN objEntidad)
        {
            return new ParametrosAD().ConsultarParametrosAD(SP_Ban_Parametros, objEntidad);
        }
        public String ActualizarParametrosLN(ParametrosEN objEntidad, String Operacion)
        {
            return new ParametrosAD().ActualizarParametrosAD(SP_Ban_Parametros, objEntidad);
        }
        public String EliminarParametrosLN(ParametrosEN objEntidad, String Operacion)
        {
            return new ParametrosAD().EliminarParametrosAD(SP_Ban_Parametros, objEntidad);
        }
    }
}
