using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

using ServicioDebito.AD.Conexion;
using ServicioDebito.EN;
using ServicioDebito.EN.Definicion;
using ServicioDebito.EN.Tablas;

namespace ServicioDebito.AD.Consultas
{
    public class DatosDebitoAD
    {
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public DatosDebitoAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        public DataSet ejecutarConsulta(DatosDebito objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexionSQL();
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
                    adaptador.SelectCommand.Parameters["@pIntentos"].Value = String.Empty;
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
                adaptador.Fill(datos, "ClienteDebito");
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
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DataSet datos = ejecutarConsulta(objEntidad);

            List<DatosDebito> lista = new List<DatosDebito>();
            DatosDebito objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["ClienteDebito"].Rows)
            {
                objEntidad2 = new DatosDebito();
                objEntidad2.pId = Convertidor.aEntero32(fila[DatosDebitoDEF.Id]);
                objEntidad2.pContrato = Convertidor.aCadena(fila[DatosDebitoDEF.Contrato]);
                objEntidad2.pDigito = Convertidor.aEntero32(fila[DatosDebitoDEF.Digito]);
                objEntidad2.pIdBanco = Convertidor.aEntero32(fila[DatosDebitoDEF.IdBanco]);
                objEntidad2.pEstado = Convertidor.aEntero32(fila[DatosDebitoDEF.Estado]);
                objEntidad2.pTipoCuenta = Convertidor.aEntero32(fila[DatosDebitoDEF.TipoCuenta]);
                objEntidad2.pNumeroCuenta = Convertidor.aCadena(fila[DatosDebitoDEF.NumeroCuenta]);
                objEntidad2.pIdFormatoDebito = Convertidor.aEntero32(fila[DatosDebitoDEF.FormatoDebito]);
                objEntidad2.pIdFormatoCancelacion = Convertidor.aEntero32(fila[DatosDebitoDEF.FormatoCancelacion]);
                objEntidad2.pIdTitularCuenta = Convertidor.aEntero32(fila[DatosDebitoDEF.IdTitularCuenta]);
                objEntidad2.pDireccionIp = Convertidor.aCadena(fila[DatosDebitoDEF.DireccionIP]);
                objEntidad2.pTercero = Convertidor.aBooleano(fila[DatosDebitoDEF.Tercero]);
                objEntidad2.pSuspendido = Convertidor.aBooleano(fila[DatosDebitoDEF.Suspendido]);
                objEntidad2.pFechaInicioSus = Convertidor.aCadena(fila[DatosDebitoDEF.FechaInicioSus]);
                objEntidad2.pFechaFinSus = Convertidor.aCadena(fila[DatosDebitoDEF.FechaFinSus]);
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
                cuenta = Convertidor.aEntero32(datos.Tables["ClienteDebito"].Rows[0]["Cuenta"]);
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
            DataTable datos = objQuery.consultarDatos(query).Tables["ClienteDebito"];
            Error = objQuery.Error;
            return datos;
        }

    }
}
