using DebitoAutomatico.AD.Conexion;
using DebitoAutomatico.EN;
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
    public class MensajesAD
    {
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public MensajesAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        protected DataSet ejecutarConsulta(Mensajes objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;
            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Mensajes", conexion);
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pTipoContrato", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pTipoContrato))
                {
                    adaptador.SelectCommand.Parameters["@pTipoContrato"].Value = objEntidad.pTipoContrato;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pTipoContrato"].Value = DBNull.Value;
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pMotivo", SqlDbType.VarChar));
                if (objEntidad.pMotivo > 0)
                {
                    adaptador.SelectCommand.Parameters["@pMotivo"].Value = objEntidad.pMotivo;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pMotivo"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pAsunto", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pAsunto))
                {
                    adaptador.SelectCommand.Parameters["@pAsunto"].Value = objEntidad.pAsunto;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pAsunto"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pMensaje", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pMensaje))
                {
                    adaptador.SelectCommand.Parameters["@pMensaje"].Value = objEntidad.pMensaje;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pMensaje"].Value = DBNull.Value;
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

        public List<Mensajes> consultar(Mensajes objEntidad)
        {
            DataSet datos = ejecutarConsulta(objEntidad);

            List<Mensajes> lista = new List<Mensajes>();
            Mensajes objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new Mensajes();
                objEntidad2.pId = Convertidor.aEntero32(fila[MensajesDEF.Id]);
                objEntidad2.pTipoContrato = Convertidor.aCadena(fila[MensajesDEF.TipoContrato]);
                objEntidad2.pEstadoDebito = Convertidor.aEntero32(fila[MensajesDEF.EstadoDebito]);
                objEntidad2.pMotivo = Convertidor.aEntero32(fila[MensajesDEF.Motivo]);
                objEntidad2.pAsunto = Convertidor.aCadena(fila[MensajesDEF.Asunto]);
                objEntidad2.pMensaje = Convertidor.aCadena(fila[MensajesDEF.Mensaje]);

                lista.Add(objEntidad2);
            }

            return lista;
        }

        public int ejecutarNoConsulta(Mensajes objEntidad)
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
