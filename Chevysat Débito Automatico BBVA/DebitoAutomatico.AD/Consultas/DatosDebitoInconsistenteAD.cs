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
    public class DatosDebitoInconsistenteAD
    {
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public DatosDebitoInconsistenteAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        protected DataSet ejecutarConsulta(DatosDebitoInconsistente objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
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
                if (objEntidad.pTipoInconsistencia  > 0)
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
                    adaptador.SelectCommand.Parameters["@pFechaDebito"].Value = 0;
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

        public List<DatosDebitoInconsistente> consultar(DatosDebitoInconsistente objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DataSet datos = ejecutarConsulta(objEntidad);

            List<DatosDebitoInconsistente> lista = new List<DatosDebitoInconsistente>();
            DatosDebitoInconsistente objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {                
                objEntidad2 = new DatosDebitoInconsistente();
                objEntidad2.pId = Convertidor.aEntero32(fila[DatosDebitoInconsistenteDEF.Id]);
                objEntidad2.pContrato = Convertidor.aCadena(fila[DatosDebitoInconsistenteDEF.Contrato]);
                objEntidad2.pDigito = Convertidor.aEntero32(fila[DatosDebitoInconsistenteDEF.Digito]);
                objEntidad2.pEstado = Convertidor.aEntero32(fila[DatosDebitoInconsistenteDEF.Estado]);
                objEntidad2.pIdBanco = Convertidor.aEntero32(fila[DatosDebitoInconsistenteDEF.IdBanco]);
                objEntidad2.pTipoCuenta = Convertidor.aEntero32(fila[DatosDebitoInconsistenteDEF.TipoCuenta]);
                objEntidad2.pNumeroCuenta = Convertidor.aCadena(fila[DatosDebitoInconsistenteDEF.NumeroCuenta]);
                objEntidad2.pTercero = Convertidor.aBooleano(fila[DatosDebitoInconsistenteDEF.Tercero]);
                objEntidad2.pIdTitularCuenta = Convertidor.aEntero32(fila[DatosDebitoInconsistenteDEF.IdTitularCuenta]);
                objEntidad2.pFormato = Convertidor.aEntero32(fila[DatosDebitoInconsistenteDEF.Formato]);
                objEntidad2.pDireccion_Ip = Convertidor.aCadena(fila[DatosDebitoInconsistenteDEF.DireccionIp]);
                objEntidad2.pTipoInconsistencia = Convertidor.aEntero32(fila[DatosDebitoInconsistenteDEF.TipoInconsistencia]);
                objEntidad2.pObservaciones = Convertidor.aCadena(fila[DatosDebitoInconsistenteDEF.Observaciones]);
                objEntidad.pFechaDebito = Convertidor.aEntero32(fila[DatosDebitoInconsistenteDEF.FechaDebito]);
                lista.Add(objEntidad2);
            }

            return lista;
        }

        public int ejecutarNoConsulta(DatosDebitoInconsistente objEntidad)
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

        public DataTable consultarContratosBusqueda(String Contrato, String Identificacion)
        {
            String query = "SELECT DD.CONTRATO, TIC.NUMERO_IDENTIFICACION"
                + " FROM [tb_DEB_DATOS_DEBITO_INCONSISTENTE] AS DD"
                + " INNER JOIN tb_DEB_TITULAR_CUENTA_INCONSISTENTE AS TIC ON DD.ID_TITULAR_CUENTA = TIC.ID"
                + " WHERE (DD.CONTRATO = '" + Contrato + "'"
                + " OR TIC.NUMERO_IDENTIFICACION = '" + Identificacion + "')"
                + " ORDER BY DD.CONTRATO";
            return consultar(query);
        }

    }
}
