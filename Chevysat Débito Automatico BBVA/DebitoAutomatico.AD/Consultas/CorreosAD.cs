using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using DebitoAutomatico.AD.Conexion;
using DebitoAutomatico.EN.Tablas;
using Excel.Log;

namespace DebitoAutomatico.AD.Consultas
{
    public class CorreosAD
    {
        public String Error { get; set; }
        public ILog Registrador { get; set; }
        protected DataSet ejecutarConsulta(Correos objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {

                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Correos", conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pId", SqlDbType.VarChar));
                if (objEntidad.Id > 0)
                {
                    adaptador.SelectCommand.Parameters["@pId"].Value = objEntidad.Id;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pId"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pOperacion", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.Operacion))
                {
                    adaptador.SelectCommand.Parameters["@pOperacion"].Value = objEntidad.Operacion;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pOperacion"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pObervaciones", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.Obervaciones))
                {
                    adaptador.SelectCommand.Parameters["@pObervaciones"].Value = objEntidad.Obervaciones;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pObervaciones"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pContrato", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.Contrato))
                {
                    adaptador.SelectCommand.Parameters["@pContrato"].Value = objEntidad.Contrato;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pContrato"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pNombreArchivo", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.NombreArchivo))
                {
                    adaptador.SelectCommand.Parameters["@pNombreArchivo"].Value = objEntidad.NombreArchivo;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pNombreArchivo"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pEnvio", SqlDbType.VarChar));
                if (objEntidad.Envio != null)
                {
                    if (objEntidad.Envio.Value)
                        adaptador.SelectCommand.Parameters["@pEnvio"].Value = "1";
                    else
                        adaptador.SelectCommand.Parameters["@pEnvio"].Value = "0";
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pEnvio"].Value = String.Empty;
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

        public int ejecutarNoConsulta(Correos objEntidad)
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
