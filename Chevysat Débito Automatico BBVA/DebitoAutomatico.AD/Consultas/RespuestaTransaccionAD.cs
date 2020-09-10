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
    public class RespuestaTransaccionAD
    {
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public RespuestaTransaccionAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        protected DataSet ejecutarConsulta(RespuestaTransaccion objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Respuesta_Transaccion", conexion);
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pBanco", SqlDbType.VarChar));
                if (objEntidad.pBanco > 0)
                {
                    adaptador.SelectCommand.Parameters["@pBanco"].Value = objEntidad.pBanco;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pBanco"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCodigo", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pCodigo))
                {
                    adaptador.SelectCommand.Parameters["@pCodigo"].Value = objEntidad.pCodigo;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCodigo"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pRespuesta", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pRespuesta))
                {
                    adaptador.SelectCommand.Parameters["@pRespuesta"].Value = objEntidad.pRespuesta;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pRespuesta"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pEstadoRespuesta", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pEstadoRespuesta))
                {
                    adaptador.SelectCommand.Parameters["@pEstadoRespuesta"].Value = objEntidad.pEstadoRespuesta;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pEstadoRespuesta"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pEstadoPrenota", SqlDbType.VarChar));
                if (objEntidad.pEstadoPrenota > 0)
                {
                    adaptador.SelectCommand.Parameters["@pEstadoPrenota"].Value = objEntidad.pEstadoPrenota;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pEstadoPrenota"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pEstadoDebito", SqlDbType.VarChar));
                if (objEntidad.pEstadoDebito > 0)
                {
                    adaptador.SelectCommand.Parameters["@pEstadoDebito"].Value = objEntidad.pEstadoDebito;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pEstadoDebito"].Value = String.Empty;
                }
                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pEnvioCorreo", SqlDbType.VarChar));
                if (objEntidad.pEnvioCorreo < 2)
                {
                    adaptador.SelectCommand.Parameters["@pEnvioCorreo"].Value = objEntidad.pEnvioCorreo;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pEnvioCorreo"].Value = String.Empty;
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

        public List<RespuestaTransaccion> consultar(RespuestaTransaccion objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DataSet datos = ejecutarConsulta(objEntidad);

            List<RespuestaTransaccion> lista = new List<RespuestaTransaccion>();
            RespuestaTransaccion objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new RespuestaTransaccion();
                objEntidad2.pId = Convertidor.aEntero32(fila[RespuestaTransaccionDEF.Id]);
                objEntidad2.pBanco = Convertidor.aEntero32(fila[RespuestaTransaccionDEF.Banco]);
                objEntidad2.pCodigo = Convertidor.aCadena(fila[RespuestaTransaccionDEF.Codigo]);
                objEntidad2.pRespuesta = Convertidor.aCadena(fila[RespuestaTransaccionDEF.Respuesta]);
                objEntidad2.pEstadoRespuesta = Convertidor.aCadena(fila[RespuestaTransaccionDEF.EstadoRespuesta]);
                objEntidad2.pEstadoPrenota = Convertidor.aEntero32(fila[RespuestaTransaccionDEF.EstadoPrenota]);
                objEntidad2.pEstadoDebito = Convertidor.aEntero32(fila[RespuestaTransaccionDEF.EstadoDebito]);
                objEntidad2.pEnvioCorreo = Convertidor.aEntero32(fila[RespuestaTransaccionDEF.EnvioCorreo]);
                lista.Add(objEntidad2);
            }

            return lista;
        }

        public int ejecutarNoConsulta(RespuestaTransaccion objEntidad)
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

        public DataSet consultarRespuestasDebito(RespuestaTransaccion objRT)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Consultar_Respuestas_Debito", conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pBanco", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@pBanco"].Value = objRT.pBanco;

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
    }
}
