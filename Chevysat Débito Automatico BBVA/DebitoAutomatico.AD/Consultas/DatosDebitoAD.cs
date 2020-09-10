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
    public class DatosDebitoAD
    {
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public DatosDebitoAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        protected DataSet ejecutarConsulta(DatosDebito objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {

                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Datos_Debito", conexion);
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdFormatoDebito", SqlDbType.VarChar));
                if (objEntidad.pIdFormatoDebito > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdFormatoDebito"].Value = objEntidad.pIdFormatoDebito;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdFormatoDebito"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdFormatoCancelacion", SqlDbType.VarChar));
                if (objEntidad.pIdFormatoCancelacion > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdFormatoCancelacion"].Value = objEntidad.pIdFormatoCancelacion;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdFormatoCancelacion"].Value = String.Empty;
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pDireccionIp", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pDireccionIp))
                {
                    adaptador.SelectCommand.Parameters["@pDireccionIp"].Value = objEntidad.pDireccionIp;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pDireccionIp"].Value = String.Empty;
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pSuspendido", SqlDbType.VarChar));
                if (objEntidad.pSuspendido != null)
                {
                    if (objEntidad.pSuspendido.Value)
                        adaptador.SelectCommand.Parameters["@pSuspendido"].Value = "1";
                    else
                        adaptador.SelectCommand.Parameters["@pSuspendido"].Value = "0";
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pSuspendido"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pFechaInicioSus", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pFechaInicioSus))
                {
                    adaptador.SelectCommand.Parameters["@pFechaInicioSus"].Value = objEntidad.pFechaInicioSus;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pFechaInicioSus"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pFechaFinSus", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pFechaFinSus))
                {
                    adaptador.SelectCommand.Parameters["@pFechaFinSus"].Value = objEntidad.pFechaFinSus;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pFechaFinSus"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIntentos", SqlDbType.VarChar));
                if (objEntidad.pIntentos > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIntentos"].Value = objEntidad.pIntentos;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIntentos"].Value = 0;
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

        public List<DatosDebito> consultar(DatosDebito objEntidad)
        {
            DataSet datos = ejecutarConsulta(objEntidad);

            List<DatosDebito> lista = new List<DatosDebito>();
            DatosDebito objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new DatosDebito();
                objEntidad2.pFechaDebito = Convertidor.aEntero32(fila[DatosDebitoDEF.FechaDebito]);
                lista.Add(objEntidad2);
            }

            return lista;
        }

        public int ejecutarNoConsulta(DatosDebito objEntidad)
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


        public DataSet consultarEstados(String Estado)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Consultar_Autorizacion_Cuentas", conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@Estado", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@Estado"].Value = Estado;

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

        public DataSet ConsultarxProceso(String Estado, String Banco, String Fecha, String Procedimiento)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter(Procedimiento, conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@Estado", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@Estado"].Value = Estado;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@Banco", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@Banco"].Value = Banco;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@Fecha", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@Fecha"].Value = Fecha;

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

        public DataSet consultarContratosBusqueda(Int32 TipoCliente, String Contrato, String Identificacion)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Consultar_Contratos", conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pContrato", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@pContrato"].Value = Contrato;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdentificacion", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@pIdentificacion"].Value = Identificacion;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pTipoCliente", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@pTipoCliente"].Value = TipoCliente;

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

        public DataSet ConsultarValorDebito(String Estado, String Banco, String BancoDebita, String Fecha, String FechaAnoMes, int DiaHoyHabil, 
                                            bool Sus, bool Adj, bool Gan, bool CuoxD)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Consulta_Tabla_Debito", conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@Banco", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@Banco"].Value = Banco;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@Estado", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@Estado"].Value = Estado;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@Fecha", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@Fecha"].Value = Fecha;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@FechaAnoMes", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@FechaAnoMes"].Value = FechaAnoMes;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@DiaHoyHabil", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@DiaHoyHabil"].Value = DiaHoyHabil;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@BancoDebitar", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@BancoDebitar"].Value = BancoDebita;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@Sus", SqlDbType.VarChar));
                if (Sus == true)
                    adaptador.SelectCommand.Parameters["@Sus"].Value = "S";
                else
                    adaptador.SelectCommand.Parameters["@Sus"].Value = String.Empty;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@Adj", SqlDbType.VarChar));
                if (Adj == true)
                    adaptador.SelectCommand.Parameters["@Adj"].Value = "A";
                else
                    adaptador.SelectCommand.Parameters["@Adj"].Value = String.Empty;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@Gan", SqlDbType.VarChar));
                if (Gan == true)
                    adaptador.SelectCommand.Parameters["@Gan"].Value = "G";
                else
                    adaptador.SelectCommand.Parameters["@Gan"].Value = String.Empty;


                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@CuxDe", SqlDbType.VarChar));
                if (CuoxD == true)
                    adaptador.SelectCommand.Parameters["@CuxDe"].Value = "C";
                else
                    adaptador.SelectCommand.Parameters["@CuxDe"].Value = String.Empty;
                

                datos = new DataSet();
                adaptador.Fill(datos, "TablaDebito");
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
