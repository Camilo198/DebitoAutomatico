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
    public class PerfilLN
    {
        public String Error { get; set; }

        public List<Perfil> consultar(Perfil objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            PerfilAD objConsultor = new PerfilAD();
            List<Perfil> lista = new List<Perfil>();
            lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }
    }
}
