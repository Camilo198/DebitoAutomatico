using log4net;
using ServicioDebito.AD.Conexion;
using ServicioDebito.EN;
using ServicioDebito.EN.Definicion;
using ServicioDebito.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioDebito.AD.Consultas
{
    public class LogsUsuarioAD
    {
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public LogsUsuarioAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        protected DataSet ejecutarConsulta(LogsUsuario objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexionSQL();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Logs_Usuario", conexion);
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pContrato", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pContrato))
                {
                    adaptador.SelectCommand.Parameters["@pContrato"].Value = objEntidad.pContrato;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pContrato"].Value = DBNull.Value;
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

        public List<LogsUsuario> consultar(LogsUsuario objEntidad)
        {

            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DataSet datos = ejecutarConsulta(objEntidad);

            List<LogsUsuario> lista = new List<LogsUsuario>();
            LogsUsuario objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new LogsUsuario();
                objEntidad2.pFecha = Convertidor.aCadena(fila[LogsUsuarioDEF.Fecha]);
                objEntidad2.pMovimiento = Convertidor.aCadena(fila[LogsUsuarioDEF.Movimiento]);
                objEntidad2.pUsuario = Convertidor.aCadena(fila[LogsUsuarioDEF.Usuario]);
                objEntidad2.pDetalle = Convertidor.aCadena(fila[LogsUsuarioDEF.Detalle]);
                objEntidad2.pContrato = Convertidor.aCadena(fila[LogsUsuarioDEF.Contrato]);
                lista.Add(objEntidad2);
            }
            return lista;
        }

        public int ejecutarNoConsulta(LogsUsuario objEntidad)
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
