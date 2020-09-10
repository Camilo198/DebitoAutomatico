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
    public class ReanudacionAD
    {
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public ReanudacionAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        public DataSet ejecutarConsulta(Reanudacion objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;
            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Reanudaciones", conexion);
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
                    adaptador.SelectCommand.Parameters["@pId"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pMes", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pMes))
                {
                    adaptador.SelectCommand.Parameters["@pMes"].Value = objEntidad.pMes;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pMes"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pAño", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pAño))
                {
                    adaptador.SelectCommand.Parameters["@pAño"].Value = objEntidad.pAño;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pAño"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pUsuario", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pUsuario))
                {
                    adaptador.SelectCommand.Parameters["@pUsuario"].Value = objEntidad.pUsuario;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pUsuario"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pObservaciones", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pObservaciones))
                {
                    adaptador.SelectCommand.Parameters["@pObservaciones"].Value = objEntidad.pObservaciones;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pObservaciones"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pEstado", SqlDbType.VarChar));
                if (objEntidad.pEstado != null)
                {
                    if (objEntidad.pEstado.Value)
                        adaptador.SelectCommand.Parameters["@pEstado"].Value = "1";
                    else
                        adaptador.SelectCommand.Parameters["@pEstado"].Value = "0";
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pEstado"].Value = DBNull.Value;
                }
                datos = new DataSet();
                adaptador.Fill(datos, "tabla");
                adaptador.Dispose();
            }
            catch (SqlException ex)
            {
                Error = ex.Message;
                Registrador.Warn(ex.Message);
            }
            finally
            {
                if (conexion.State != ConnectionState.Closed)
                    conexion.Close();
            }
            return datos;
        }

        public List<Reanudacion> consultar(Reanudacion objEntidad)
        {
            DataSet datos = ejecutarConsulta(objEntidad);

            List<Reanudacion> lista = new List<Reanudacion>();
            Reanudacion objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new Reanudacion();
                objEntidad2.pId = Convertidor.aEntero32(fila[ReanudacionDEF.Id]);
                objEntidad2.pMes = Convertidor.aCadena(fila[ReanudacionDEF.Mes]);
                objEntidad2.pAño = Convertidor.aCadena(fila[ReanudacionDEF.Año]);
                objEntidad2.pUsuario = Convertidor.aCadena(fila[ReanudacionDEF.Usuario]);
                objEntidad2.pObservaciones = Convertidor.aCadena(fila[ReanudacionDEF.Observaciones]);
                objEntidad2.pEstado = Convertidor.aBooleano(fila[ReanudacionDEF.Estado]);

                lista.Add(objEntidad2);
            }

            return lista;
        }

        public int ejecutarNoConsulta(Reanudacion objEntidad)
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
    }
}
