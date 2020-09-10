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
    public class HistorialProcesoUsuarioAD
    {
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public HistorialProcesoUsuarioAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        /// <summary>
        /// Esta funcion es la encargada de llenar los datos y ejecutar un procedimiento almacenado
        /// </summary>
        /// <param name="objEntidad">Entidad que contienen los datos a llenar en los parametros del procedimiento almacenado</param>
        /// <returns>Conjuntos de datos de respuesta de la ejecución del procedimiento almacenado</returns>
        protected DataSet ejecutarConsulta(HistorialProcesoUsuario objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_Deb_Historial_Proceso_Usuario", conexion);
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pNumeroIdentificacion", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pNumeroIdentificacion))
                {
                    adaptador.SelectCommand.Parameters["@pNumeroIdentificacion"].Value = objEntidad.pNumeroIdentificacion;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pNumeroIdentificacion"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pTipoCuenta", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pTipoCuenta))
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pNombreBanco", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pNombreBanco) && objEntidad.pNombreBanco != "[Seleccione]")
                {
                    adaptador.SelectCommand.Parameters["@pNombreBanco"].Value = objEntidad.pNombreBanco;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pNombreBanco"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pTipoTransferencia", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pTipoTransferencia))
                {
                    adaptador.SelectCommand.Parameters["@pTipoTransferencia"].Value = objEntidad.pTipoTransferencia;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pTipoTransferencia"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pFecha", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pFecha))
                {
                    adaptador.SelectCommand.Parameters["@pFecha"].Value = objEntidad.pFecha;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pFecha"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pFechaFin", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pFechaFin))
                {
                    adaptador.SelectCommand.Parameters["@pFechaFin"].Value = objEntidad.pFechaFin;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pFechaFin"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pValor", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pValor))
                {
                    adaptador.SelectCommand.Parameters["@pValor"].Value = objEntidad.pValor;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pValor"].Value = String.Empty;
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCausal", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pCausal))
                {
                    adaptador.SelectCommand.Parameters["@pCausal"].Value = objEntidad.pCausal;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCausal"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pFechaProceso", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pFechaProceso))
                {
                    adaptador.SelectCommand.Parameters["@pFechaProceso"].Value = objEntidad.pFechaProceso;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pFechaProceso"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pUsuario", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pUsuario))
                {
                    adaptador.SelectCommand.Parameters["@pUsuario"].Value = objEntidad.pUsuario;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pUsuario"].Value = String.Empty;
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pNombreBancoDebita", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pNombreBancoDebita) && objEntidad.pNombreBancoDebita != "[Seleccione]")
                {
                    adaptador.SelectCommand.Parameters["@pNombreBancoDebita"].Value = objEntidad.pNombreBancoDebita;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pNombreBancoDebita"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pNombreCliente", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pNombreCliente))
                {
                    adaptador.SelectCommand.Parameters["@pNombreCliente"].Value = objEntidad.pNombreCliente;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pNombreCliente"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pNombreArchivo", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pNombreArchivo))
                {
                    adaptador.SelectCommand.Parameters["@pNombreArchivo"].Value = objEntidad.pNombreArchivo;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pNombreArchivo"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pEstado", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pEstado))
                {
                    adaptador.SelectCommand.Parameters["@pEstado"].Value = objEntidad.pEstado;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pEstado"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pEstadoCliente", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pEstadoCliente) && objEntidad.pEstadoCliente != "0")
                {
                    adaptador.SelectCommand.Parameters["@pEstadoCliente"].Value = objEntidad.pEstadoCliente;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pEstadoCliente"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pValidacion", SqlDbType.VarChar));
                if (objEntidad.pValidacion != null)
                {
                    if (objEntidad.pValidacion.Value)
                        adaptador.SelectCommand.Parameters["@pValidacion"].Value = "1";
                    else
                        adaptador.SelectCommand.Parameters["@pValidacion"].Value = "0";
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pValidacion"].Value = DBNull.Value;
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

        /// <summary>
        /// Permite la consulta de los ajustes existentes en la base de datos
        /// </summary>
        /// <param name="objEntidad">Entidad que contienen los datos a llenar en los parametros del procedimiento almacenado</param>
        /// <returns>Lista de datos</returns>
        public List<HistorialProcesoUsuario> consultar(HistorialProcesoUsuario objEntidad)
        {
            //objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DataSet datos = ejecutarConsulta(objEntidad);

            List<HistorialProcesoUsuario> lista = new List<HistorialProcesoUsuario>();
            HistorialProcesoUsuario objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new HistorialProcesoUsuario();
                objEntidad2.pId = Convertidor.aEntero32(fila[HistorialProcesoUsuarioDEF.Id]);
                objEntidad2.pNumeroIdentificacion = Convertidor.aCadena(fila[HistorialProcesoUsuarioDEF.NumeroIdentificacion]);
                objEntidad2.pTipoCuenta = Convertidor.aCadena(fila[HistorialProcesoUsuarioDEF.TipoCuenta]).Substring(0, 1);
                objEntidad2.pNumeroCuenta = Convertidor.aCadena(fila[HistorialProcesoUsuarioDEF.NumeroCuenta]);
                objEntidad2.pNombreBanco = Convertidor.aCadena(fila[HistorialProcesoUsuarioDEF.NombreBanco]);
                objEntidad2.pTipoTransferencia = Convertidor.aCadena(fila[HistorialProcesoUsuarioDEF.TipoTransferencia]).Substring(0, 1);
                objEntidad2.pFecha = Convertidor.aCadena(fila[HistorialProcesoUsuarioDEF.Fecha]);
                objEntidad2.pValor = Convertidor.aCadena(fila[HistorialProcesoUsuarioDEF.Valor]);
                objEntidad2.pRespuesta = Convertidor.aCadena(fila[HistorialProcesoUsuarioDEF.Respuesta]);
                objEntidad2.pCausal = Convertidor.aCadena(fila[HistorialProcesoUsuarioDEF.Causal]);
                objEntidad2.pFechaProceso = Convertidor.aCadena(fila[HistorialProcesoUsuarioDEF.FechaProceso]);
                objEntidad2.pUsuario = Convertidor.aCadena(fila[HistorialProcesoUsuarioDEF.Usuario]);
                objEntidad2.pContrato = Convertidor.aCadena(fila[HistorialProcesoUsuarioDEF.Contrato]);
                objEntidad2.pNombreBancoDebita = Convertidor.aCadena(fila[HistorialProcesoUsuarioDEF.NombreBancoDebita]);
                objEntidad2.pNombreCliente = Convertidor.aCadena(fila[HistorialProcesoUsuarioDEF.NombreCliente]);
                objEntidad2.pNombreArchivo = Convertidor.aCadena(fila[HistorialProcesoUsuarioDEF.NombreArchivo]);
                objEntidad2.pEstado = Convertidor.aCadena(fila[HistorialProcesoUsuarioDEF.Estado]);
                objEntidad2.pIdTipoCausal = Convertidor.aCadena(fila[HistorialProcesoUsuarioDEF.IdTipoCausal]);
                objEntidad2.pFechaGiro = Convertidor.aCadena(fila[HistorialProcesoUsuarioDEF.FechaGiro]);
                objEntidad2.pIdCliente = Convertidor.aEntero64(fila[HistorialProcesoUsuarioDEF.IdCliente]);
                objEntidad2.pEstadoCliente = Convertidor.aCadena(fila[HistorialProcesoUsuarioDEF.EstadoCliente]);
                objEntidad2.pFechaDebito = Convertidor.aEntero32(fila[HistorialProcesoUsuarioDEF.FechaDebito]);
                lista.Add(objEntidad2);
            }

            return lista;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int ejecutarNoConsulta(HistorialProcesoUsuario objEntidad)
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

        public DataTable consultarEstadoDebitado(String BancoDebita, String Fecha)
        {
            String query = "SELECT dd.[ID_TITULAR_CUENTA] IDTC,HPU.[ID] IDHPU,HPU.[CONTRATO] as CONTRATO_FISICO,"
                          + "HPU.[NUMERO_IDENTIFICACION] as IDENTIFICACION,HPU.[NOMBRE_CLIENTE] as NOMBRE,"
                          + "HPU.[BANCO],HPU.[TIPO_CUENTA],HPU.[NUMERO_CUENTA],HPU.[VALOR],HPU.[ESTADO] "
                          + "FROM [tb_DEB_HISTORIAL_PROCESO_USUARIO] HPU "
                          + "inner join [tb_DEB_DATOS_DEBITO] as dd on HPU.[CONTRATO] = dd.[CONTRATO] "
                          + "where HPU.[BANCO_DEBITA] like '" + BancoDebita + "%' and HPU.[FECHA_PROCESO] = '" + Fecha + "' and HPU.[TIPO_TRANSFERENCIA] = 'DEBITO' "
                          + "and HPU.[ESTADO] = 'ACEPTADO'";
            return consultar(query);
        }

        public DataTable consultarFechasTransaccionesDebitado(String BancoDebita)
        {
            String query = "SELECT convert(varchar(23),HPU.[FECHA_PROCESO],121) as FECHA_PROCESO "
                          + "FROM [tb_DEB_HISTORIAL_PROCESO_USUARIO] HPU "
                          + "inner join [tb_DEB_DATOS_DEBITO] as dd on HPU.[CONTRATO] = dd.[CONTRATO] "
                          + "where HPU.[BANCO_DEBITA] like '" + BancoDebita + "%' and HPU.[TIPO_TRANSFERENCIA] = 'DEBITO' "
                          + "and HPU.[ESTADO] = 'ACEPTADO' group by FECHA_PROCESO order by FECHA_PROCESO";
            return consultar(query);
        }

        public DataTable consultarFechasTransaccionesDebitoProceso(String Estado)
        {
            String query = "select convert(varchar(23),[FECHA_ESTADO],121) as [FECHA_ESTADO]  FROM [tb_DEB_TITULAR_CUENTA]"
                          + "where [ESTADO] = '" + Estado + "' group by [FECHA_ESTADO]";
            return consultar(query);
        }

        public DataTable consultarEstadoDebitoEnProceso(String Fecha, String Estado)
        {
            String query = "select TC.ID IDTC,0 IDHPU,DD.[CONTRATO] CONTRATO_FISICO," +
                           "TC.[NUMERO_IDENTIFICACION] IDENTIFICACION,TC.[NOMBRE] NOMBRE,B.[NOMBRE] BANCO,TPC.VALOR [TIPO_CUENTA]," +
                           "DD.[NUMERO_CUENTA],0 VALOR FROM [tb_DEB_DATOS_DEBITO] DD " +
                           "inner join [tb_DEB_TITULAR_CUENTA] TC on TC.ID = DD.[ID_TITULAR_CUENTA] " +
                           "inner join [tb_DEB_BANCO] B on B.ID = DD.[ID_BANCO] " +
                           "inner join [tb_DEB_TIPO_CUENTA] TPC on TPC.ID = DD.[TIPO_CUENTA] " +
                           "where TC.ESTADO = '" + Estado + "' and TC.[FECHA_ESTADO] ='" + Fecha + "'";
            return consultar(query);
        }

        public DataTable reversar(String EstadoD, String IDTC, String IDHPU, String Causal, String EstadoFinalD, String EstadoRespuesta)
        {
            String query = "execute [dbo].[pa_DEB_Reversar_Contratos] '" + EstadoD + "','" + IDTC + "','" + IDHPU + "','" + Causal + "','" + EstadoFinalD + "','" + EstadoRespuesta + "'";
            return consultar(query);
        }

        public DataTable consultarDebitoXFecha(String Fecha, String BancoDebita)
        {
            String query = "SELECT HPU.CONTRATO,HPU.FECHA,HPU.RESPUESTA,HPU.VALOR "
                           + "FROM [tb_DEB_HISTORIAL_PROCESO_USUARIO] HPU "
                           + "where HPU.[BANCO_DEBITA] like '" + BancoDebita + "%' and HPU.[TIPO_TRANSFERENCIA] = 'DEBITO' "
                           + "and (CONVERT(VARCHAR,HPU.FECHA,103)='" + Fecha + "')";
            return consultar(query);
        }

        public DataTable consultarPrenotaXFecha(String Fecha, String BancoDebita)
        {
            String query = "SELECT HPU.CONTRATO,HPU.FECHA,HPU.RESPUESTA,HPU.VALOR "
                           + "FROM [tb_DEB_HISTORIAL_PROCESO_USUARIO] HPU "
                           + "where HPU.[BANCO_DEBITA] like '" + BancoDebita + "%' and HPU.[TIPO_TRANSFERENCIA] = 'PRENOTA' "
                             + "and (CONVERT(VARCHAR,HPU.FECHA,103)='" + Fecha + "')";
            return consultar(query);
        }

    }
}
