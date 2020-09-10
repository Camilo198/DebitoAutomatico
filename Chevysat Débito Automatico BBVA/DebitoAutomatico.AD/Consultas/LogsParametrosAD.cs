using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DebitoAutomatico.AD.Conexion;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Definicion;
using DebitoAutomatico.EN.Tablas;
using log4net;
using System.Data;
using System.Data.SqlClient;

namespace DebitoAutomatico.AD.Consultas
{
    public class LogsParametrosAD
    {
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public LogsParametrosAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        protected DataSet ejecutarConsulta(LogsParametros objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Logs_Parametros", conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pOperacion", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@pOperacion"].Value = objEntidad.pOperacion;
                                

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pFecha", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pFecha))
                {
                    adaptador.SelectCommand.Parameters["@pFecha"].Value = objEntidad.pFecha;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pFecha"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pMovimiento", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pMovimiento))
                {
                    adaptador.SelectCommand.Parameters["@pMovimiento"].Value = objEntidad.pMovimiento;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pMovimiento"].Value = DBNull.Value;
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pDetalle", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pDetalle))
                {
                    adaptador.SelectCommand.Parameters["@pDetalle"].Value = objEntidad.pDetalle;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pDetalle"].Value = DBNull.Value;
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

        public List<LogsParametros> consultar(LogsParametros objEntidad)
        {

            DataSet datos = ejecutarConsulta(objEntidad);

            List<LogsParametros> lista = new List<LogsParametros>();
            LogsParametros objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new LogsParametros();
                objEntidad2.pFecha = Convertidor.aCadena(fila[LogsUsuarioDEF.Fecha]);
                objEntidad2.pMovimiento = Convertidor.aCadena(fila[LogsUsuarioDEF.Movimiento]);
                objEntidad2.pUsuario = Convertidor.aCadena(fila[LogsUsuarioDEF.Usuario]);
                objEntidad2.pDetalle = Convertidor.aCadena(fila[LogsUsuarioDEF.Detalle]);
                lista.Add(objEntidad2);
            }
            return lista;
        }

        public int ejecutarNoConsulta(LogsParametros objEntidad)
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
