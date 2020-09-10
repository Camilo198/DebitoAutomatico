using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using DebitoAutomatico.AD.Conexion;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Definicion;
using DebitoAutomatico.EN.Tablas;
using log4net;

namespace DebitoAutomatico.AD.Consultas
{
    public class LogsAD
    {
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public LogsAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        protected DataSet ejecutarConsulta(Logs objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Logs", conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pOperacion", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@pOperacion"].Value = objEntidad.pOperacion;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdBanco", SqlDbType.VarChar));
                if (objEntidad.pIdBanco > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdBanco"].Value = objEntidad.pIdBanco;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdBanco"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pFecha", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pFecha))
                {
                    adaptador.SelectCommand.Parameters["@pFecha"].Value = objEntidad.pFecha;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pFecha"].Value = DBNull.Value;
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pTipoArchivo", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pTipoArchivo))
                {
                    adaptador.SelectCommand.Parameters["@pTipoArchivo"].Value = objEntidad.pTipoArchivo;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pTipoArchivo"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pTipoProceso", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pTipoProceso))
                {
                    adaptador.SelectCommand.Parameters["@pTipoProceso"].Value = objEntidad.pTipoProceso;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pTipoProceso"].Value = DBNull.Value;
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

        public List<Logs> consultar(Logs objEntidad)
        {

            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DataSet datos = ejecutarConsulta(objEntidad);

            List<Logs> lista = new List<Logs>();
            Logs objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new Logs();
                objEntidad2.pFecha = Convertidor.aCadena(fila[LogsDEF.Fecha]);
                objEntidad2.pUsuario = Convertidor.aCadena(fila[LogsDEF.Usuario]);
                objEntidad2.pDetalle = Convertidor.aCadena(fila[LogsDEF.Detalle]);
                objEntidad2.pTipoArchivo = Convertidor.aCadena(fila[LogsDEF.TipoArchivo]);
                objEntidad2.pTipoProceso = Convertidor.aCadena(fila[LogsDEF.TipoProceso]);
                objEntidad2.pIdBanco = Convertidor.aEntero32(fila[LogsDEF.IdBanco]);
                lista.Add(objEntidad2);
            }
            return lista;
        }

        public int ejecutarNoConsulta(Logs objEntidad)
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

        public DataTable consultarLogs(String TipoArchivo, String TipoProceso, String FechaInicial, String FechaFinal)
        {

            String query = " SELECT FECHA,USUARIO,DETALLE" +
                           " FROM [tb_DEB_LOGS]" +
                           " WHERE TIPO_ARCHIVO = '" + TipoArchivo + "'" +
                           " AND TIPO_PROCESO = '" + TipoProceso + "'" +
                           " AND (FECHA >= '" + FechaInicial + " 00:00:00' AND FECHA <= '" + FechaFinal + " 23:59:59') ORDER BY FECHA";
            return consultar(query);
        }
    }
}
