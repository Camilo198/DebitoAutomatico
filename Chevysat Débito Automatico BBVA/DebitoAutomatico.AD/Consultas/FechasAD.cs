using DebitoAutomatico.AD.Conexion;
using DebitoAutomatico.EN.Definicion;
using DebitoAutomatico.EN.Tablas;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.AD.Consultas
{
    public class FechasAD
    {
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public FechasAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        protected DataSet ejecutarConsulta(Fechas objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Fechas", conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pOperacion", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@pOperacion"].Value = objEntidad.pOperacion;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pId", SqlDbType.VarChar));
                if (objEntidad.pId > 0)
                {
                    adaptador.SelectCommand.Parameters["@pId"].Value = objEntidad.pId;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pId"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pDia", SqlDbType.VarChar));
                if (objEntidad.pDia > 0)
                {
                    adaptador.SelectCommand.Parameters["@pDia"].Value = objEntidad.pDia;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pDia"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pValor", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pValor))
                {
                    adaptador.SelectCommand.Parameters["@pValor"].Value = objEntidad.pValor;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pValor"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pHabilita", SqlDbType.VarChar));
                if (objEntidad.pHabilita != null)
                {
                    if (objEntidad.pHabilita.Value)
                        adaptador.SelectCommand.Parameters["@pHabilita"].Value = "1";
                    else
                        adaptador.SelectCommand.Parameters["@pHabilita"].Value = "0";
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pHabilita"].Value = String.Empty;
                }

                datos = new DataSet();
                adaptador.Fill(datos, "tabla");
                adaptador.Dispose();
            }
            catch (SqlException ex)
            {
                Error = ex.Message;
                Registrador.Error(Error);
            }
            finally
            {
                if (conexion.State != ConnectionState.Closed)
                    conexion.Close();
            }

            return datos;
        }

        public List<Fechas> consultar(Fechas objEntidad)
        {
            DataSet datos = ejecutarConsulta(objEntidad);

            List<Fechas> lista = new List<Fechas>();
            Fechas objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new Fechas();
                objEntidad2.pId = Convertidor.aEntero32(fila[FechasDEF.Id]);
                objEntidad2.pDia = Convertidor.aEntero32(fila[FechasDEF.Dia]);
                objEntidad2.pValor = Convertidor.aCadena(fila[FechasDEF.Valor]);
                objEntidad2.pHabilita = Convertidor.aBooleano(fila[FechasDEF.Habilita]);
                lista.Add(objEntidad2);
            }

            return lista;
        }

        public int ejecutarNoConsulta(Fechas objEntidad)
        {
            int cuenta = -1;
            DataSet datos = ejecutarConsulta(objEntidad);
            try
            {
                cuenta = Convertidor.aEntero32(datos.Tables["tabla"].Rows[0]["Cuenta"]);
            }
            catch (Exception ex)
            {
                Registrador.Warn(ex.Message);
            }
            return cuenta;
        }

        private DataTable consultar(String query)
        {
            Querys objQuery = new Querys();
            DataTable datos = objQuery.consultarDatos(query).Tables["tabla"];
            Error = objQuery.Error;
            return datos;
        }
    }
}
