using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DebitoAutomatico.AD;
using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;


namespace DebitoAutomatico.LN.Consultas
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

        public List<Usuario> consultarUsuarioChevy(Usuario objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR_2;
            UsuarioAD objConsultor = new UsuarioAD();
            List<Usuario> lista = new List<Usuario>();
            lista = objConsultor.consultarChevy(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        public int insertar(Usuario objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            UsuarioAD objConsultor = new UsuarioAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizar(Usuario objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            UsuarioAD objConsultor = new UsuarioAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int eliminar(Usuario objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ELIMINAR;
            int cuenta = -1;
            UsuarioAD objConsultor = new UsuarioAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }
    }
}
