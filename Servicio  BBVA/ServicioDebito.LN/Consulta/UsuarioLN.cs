using ServicioDebito.AD.Consultas;
using ServicioDebito.EN;
using ServicioDebito.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioDebito.LN.Consulta
{
    public class UsuarioLN
    {
        public String Error { get; set; }

        public List<Usuario> consultar(Usuario objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            UsuarioAD objConsultor = new UsuarioAD();
            List<Usuario> lista = new List<Usuario>();
            lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }
    }
}
