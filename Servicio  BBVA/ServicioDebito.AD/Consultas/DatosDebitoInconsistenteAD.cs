using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ServicioDebito.AD.Conexion;
using ServicioDebito.EN;
using ServicioDebito.EN.Definicion;
using ServicioDebito.EN.Tablas;
using log4net;

namespace ServicioDebito.AD.Consultas
{

    public class DatosDebitoInconsistenteAD
    {
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public DatosDebitoInconsistenteAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        public DataSet ejecutarConsulta(DatosDebitoInconsistente objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexionSQL();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Datos_Debito_Inconsistente", conexion);
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pContrato", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pContrato))
                {
                    adaptador.SelectCommand.Parameters["@pContrato"].Value = objEntidad.pContrato;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pContrato"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pDigito", SqlDbType.VarChar));
                if (objEntidad.pDigito > 0)
                {
                    adaptador.SelectCommand.Parameters["@pDigito"].Value = objEntidad.pDigito;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pDigito"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pEstado", SqlDbType.VarChar));
                if (objEntidad.pEstado > 0)
                {
                    adaptador.SelectCommand.Parameters["@pEstado"].Value = objEntidad.pEstado;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pEstado"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdBanco", SqlDbType.VarChar));
                if (objEntidad.pIdBanco > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdBanco"].Value = objEntidad.pIdBanco;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdBanco"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pTipoCuenta", SqlDbType.VarChar));
                if (objEntidad.pTipoCuenta > 0)
                {
                    adaptador.SelectCommand.Parameters["@pTipoCuenta"].Value = objEntidad.pTipoCuenta;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pTipoCuenta"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pNumeroCuenta", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pNumeroCuenta))
                {
                    adaptador.SelectCommand.Parameters["@pNumeroCuenta"].Value = objEntidad.pNumeroCuenta;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pNumeroCuenta"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pTercero", SqlDbType.VarChar));
                if (objEntidad.pTercero != null)
                {
                    if (objEntidad.pTercero.Value)
                        adaptador.SelectCommand.Parameters["@pTercero"].Value = "1";
                    else
                        adaptador.SelectCommand.Parameters["@pTercero"].Value = "0";
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pTercero"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdTitularCuenta", SqlDbType.VarChar));
                if (objEntidad.pIdTitularCuenta > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdTitularCuenta"].Value = objEntidad.pIdTitularCuenta;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdTitularCuenta"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pFormato", SqlDbType.VarChar));
                if (objEntidad.pFormato > 0)
                {
                    adaptador.SelectCommand.Parameters["@pFormato"].Value = objEntidad.pFormato;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pFormato"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pDireccion_Ip", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pDireccion_Ip))
                {
                    adaptador.SelectCommand.Parameters["@pDireccion_Ip"].Value = objEntidad.pDireccion_Ip;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pDireccion_Ip"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pTipoInconsistencia", SqlDbType.VarChar));
                if (objEntidad.pTipoInconsistencia > 0)
                {
                    adaptador.SelectCommand.Parameters["@pTipoInconsistencia"].Value = objEntidad.pTipoInconsistencia;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pTipoInconsistencia"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pObservaciones", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pObservaciones))
                {
                    adaptador.SelectCommand.Parameters["@pObservaciones"].Value = objEntidad.pObservaciones;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pObservaciones"].Value = String.Empty;
                }


                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pFechaDebito", SqlDbType.VarChar));
                if (objEntidad.pFechaDebito > 0)
                {
                    adaptador.SelectCommand.Parameters["@pFechaDebito"].Value = objEntidad.pFechaDebito;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pFechaDebito"].Value = String.Empty;
                }

                datos = new DataSet();
                adaptador.Fill(datos, "ClienteInconsistente");
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

        public int ejecutarNoConsulta(DatosDebitoInconsistente objEntidad)
        {
            int cuenta = -1;
            DataSet datos = ejecutarConsulta(objEntidad);
            try
            {
                cuenta = Convertidor.aEntero32(datos.Tables["ClienteInconsistente"].Rows[0]["Cuenta"]);
            }
            catch (Exception ex)
            {
                Registrador.Warn(ex.Message);
            }
            return cuenta;
        }
    }
}

