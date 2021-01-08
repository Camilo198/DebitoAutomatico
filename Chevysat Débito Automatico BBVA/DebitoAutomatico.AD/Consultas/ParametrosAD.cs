using DebitoAutomatico.AD.Servicios;
using DebitoAutomatico.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebitoAutomatico.AD.Consultas
{
    public class ParametrosAD
    {
        WcfData wsc = new WcfData();
        public string InsertarParametrosAD(String procedimiento, ParametrosEN objEntidad)
        {
            try
            {
                string[,,] Param = new string[5, 3, 1];

                Param[0, 0, 0] = objEntidad.Tipo;
                Param[0, 1, 0] = "@tipo";
                Param[0, 2, 0] = "varchar(255)";

                Param[1, 0, 0] = objEntidad.NombreParametro;
                Param[1, 1, 0] = "@nombreParametro";
                Param[1, 2, 0] = "varchar(255)";

                Param[2, 0, 0] = objEntidad.ValorParametro;
                Param[2, 1, 0] = "@valorParametro";
                Param[2, 2, 0] = "varchar(255)";

                Param[3, 0, 0] = objEntidad.Descripcion;
                Param[3, 1, 0] = "@descripcion";
                Param[3, 2, 0] = "varchar(255)";

                Param[4, 0, 0] = "I";
                Param[4, 1, 0] = "@pOperacion";
                Param[4, 2, 0] = "varchar(3)";

                return wsc.Ejecutar(Param, procedimiento, "SQLBancos");
            }
            catch (Exception ex)
            {
                return "0" + ex.Message;
            }

        }
        public IList<ParametrosEN> ConsultarParametrosAD(String procedimiento, ParametrosEN objEntidad)
        {
            List<string[,]> lista = new List<string[,]>();
            List<ParametrosEN> listParametro = new List<ParametrosEN>();
            try
            {
                string[,,] Param = new string[5, 3, 1];

                Param[0, 0, 0] = objEntidad.Tipo;
                Param[0, 1, 0] = "@tipo";
                Param[0, 2, 0] = "varchar(255)";

                Param[1, 0, 0] = objEntidad.NombreParametro;
                Param[1, 1, 0] = "@nombreParametro";
                Param[1, 2, 0] = "varchar(255)";

                Param[2, 0, 0] = objEntidad.ValorParametro;
                Param[2, 1, 0] = "@valorParametro";
                Param[2, 2, 0] = "varchar(255)";

                Param[3, 0, 0] = objEntidad.Descripcion;
                Param[3, 1, 0] = "@descripcion";
                Param[3, 2, 0] = "varchar(255)";

                Param[4, 0, 0] = "C";
                Param[4, 1, 0] = "@pOperacion";
                Param[4, 2, 0] = "varchar(3)";

                lista = wsc.LlenarLista(Param, procedimiento, "SQLBancos", "SP", "Sql");
                string[,] Valida;

                if (lista.Count > 0)
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        ParametrosEN objParametros = new ParametrosEN();
                        Valida = lista[i];

                        objParametros.Tipo = Valida[0, 1].ToString();
                        objParametros.NombreParametro = Valida[1, 1].ToString();
                        objParametros.ValorParametro = Valida[2, 1].ToString();
                        objParametros.Descripcion = Valida[3, 1].ToString();
                        listParametro.Add(objParametros);
                    }
                }

                return listParametro;

            }
            catch (Exception ex)
            {
                return new List<ParametrosEN>();
            }

        }
        public string ActualizarParametrosAD(String procedimiento, ParametrosEN objEntidad)
        {
            try
            {
                string[,,] Param = new string[5, 3, 1];

                Param[0, 0, 0] = objEntidad.Tipo;
                Param[0, 1, 0] = "@tipo";
                Param[0, 2, 0] = "varchar(255)";

                Param[1, 0, 0] = objEntidad.NombreParametro;
                Param[1, 1, 0] = "@nombreParametro";
                Param[1, 2, 0] = "varchar(255)";

                Param[2, 0, 0] = objEntidad.ValorParametro;
                Param[2, 1, 0] = "@valorParametro";
                Param[2, 2, 0] = "varchar(255)";

                Param[3, 0, 0] = objEntidad.Descripcion;
                Param[3, 1, 0] = "@descripcion";
                Param[3, 2, 0] = "varchar(255)";

                Param[4, 0, 0] = "U";
                Param[4, 1, 0] = "@pOperacion";
                Param[4, 2, 0] = "varchar(3)";

                return wsc.Ejecutar(Param, procedimiento, "SQLBancos");
            }
            catch (Exception ex)
            {
                return "0" + ex.Message;
            }

        }
        public string EliminarParametrosAD(String procedimiento, ParametrosEN objEntidad)
        {
            try
            {
                string[,,] Param = new string[5, 3, 1];

                Param[0, 0, 0] = objEntidad.Tipo;
                Param[0, 1, 0] = "@tipo";
                Param[0, 2, 0] = "varchar(255)";

                Param[1, 0, 0] = objEntidad.NombreParametro;
                Param[1, 1, 0] = "@nombreParametro";
                Param[1, 2, 0] = "varchar(255)";

                Param[2, 0, 0] = objEntidad.ValorParametro;
                Param[2, 1, 0] = "@valorParametro";
                Param[2, 2, 0] = "varchar(255)";

                Param[3, 0, 0] = objEntidad.Descripcion;
                Param[3, 1, 0] = "@descripcion";
                Param[3, 2, 0] = "varchar(255)";

                Param[4, 0, 0] = "D";
                Param[4, 1, 0] = "@pOperacion";
                Param[4, 2, 0] = "varchar(3)";

                return wsc.Ejecutar(Param, procedimiento, "SQLBancos");
            }
            catch (Exception ex)
            {
                return "0" + ex.Message;
            }

        }
    }
}
