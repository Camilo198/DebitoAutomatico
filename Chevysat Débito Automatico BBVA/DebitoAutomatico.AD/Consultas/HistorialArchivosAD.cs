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
    public class HistorialArchivosAD
    {
        public String Error { get; set; }
        
        public ILog Registrador { get; set; }

        public HistorialArchivosAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        /// <summary>
        /// Esta funcion es la encargada de llenar los datos y ejecutar un procedimiento almacenado
        /// </summary>
        /// <param name="objEntidad">Entidad que contienen los datos a llenar en los parametros del procedimiento almacenado</param>
        /// <returns>Conjuntos de datos de respuesta de la ejecución del procedimiento almacenado</returns>
        protected DataSet ejecutarConsulta(HistorialArchivos objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_Deb_Historial_Archivos", conexion);
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
                    adaptador.SelectCommand.Parameters["@pFecha"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCodigoBanco", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pCodigoBanco))
                {
                    adaptador.SelectCommand.Parameters["@pCodigoBanco"].Value = objEntidad.pCodigoBanco;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCodigoBanco"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pTipoArchivo", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pTipoArchivo))
                {
                    adaptador.SelectCommand.Parameters["@pTipoArchivo"].Value = objEntidad.pTipoArchivo;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pTipoArchivo"].Value = String.Empty;
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdCliente", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pIdCliente))
                {
                    adaptador.SelectCommand.Parameters["@pIdCliente"].Value = objEntidad.pIdCliente;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdCliente"].Value = String.Empty;
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pNombreArchivo", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pNombreArchivo))
                {
                    adaptador.SelectCommand.Parameters["@pNombreArchivo"].Value = objEntidad.pNombreArchivo;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pNombreArchivo"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pConsecutivo", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pConsecutivo))
                {
                    adaptador.SelectCommand.Parameters["@pConsecutivo"].Value = objEntidad.pConsecutivo;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pConsecutivo"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pManual", SqlDbType.VarChar));
                if (objEntidad.pManual != null)
                {
                    if (objEntidad.pManual.Value)
                        adaptador.SelectCommand.Parameters["@pManual"].Value = "1";
                    else
                        adaptador.SelectCommand.Parameters["@pManual"].Value = "0";
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pManual"].Value = String.Empty;
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


                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pUsuarioModifico", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pUsuarioModifico))
                {
                    adaptador.SelectCommand.Parameters["@pUsuarioModifico"].Value = objEntidad.pUsuarioModifico;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pUsuarioModifico"].Value = String.Empty;
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
        public List<HistorialArchivos> consultar(HistorialArchivos objEntidad)
        {
           
            DataSet datos = ejecutarConsulta(objEntidad);

            List<HistorialArchivos> lista = new List<HistorialArchivos>();
            HistorialArchivos objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new HistorialArchivos();
                objEntidad2.pFecha  = Convertidor.aCadena(fila[HistorialArchivosDEF.Fecha ]);
                objEntidad2.pCodigoBanco = Convertidor.aCadena(fila[HistorialArchivosDEF.CodigoBanco]);
                objEntidad2.pTipoArchivo = Convertidor.aCadena(fila[HistorialArchivosDEF.TipoArchivo]);
                objEntidad2.pContrato = Convertidor.aCadena(fila[HistorialArchivosDEF.Contrato]);
                objEntidad2.pIdCliente = Convertidor.aCadena(fila[HistorialArchivosDEF.IdCliente]);
                objEntidad2.pValor = Convertidor.aCadena(fila[HistorialArchivosDEF.Valor]);
                objEntidad2.pNombreArchivo = Convertidor.aCadena(fila[HistorialArchivosDEF.NombreArchivo]);
                objEntidad2.pConsecutivo = Convertidor.aCadena(fila[HistorialArchivosDEF.Consecutivo]);
                objEntidad2.pManual = Convertidor.aBooleano(fila[HistorialArchivosDEF.Manual]);
                objEntidad2.pEstado = Convertidor.aCadena(fila[HistorialArchivosDEF.Estado]);
                objEntidad2.pUsuarioModifico = Convertidor.aCadena(fila[HistorialArchivosDEF.UsuarioModifico]);
                
                
                lista.Add(objEntidad2);
            }

            return lista;
        }


        /// <summary>
        /// Permite la consulta de los ajustes existentes en la base de datos
        /// </summary>
        /// <param name="objEntidad">Entidad que contienen los datos a llenar en los parametros del procedimiento almacenado</param>
        /// <returns>Lista de datos para Correo</returns>
        public List<HistorialArchivos> consultarcorreo(HistorialArchivos objEntidad)
        {

            DataSet datos = ejecutarConsulta(objEntidad);

            List<HistorialArchivos> lista = new List<HistorialArchivos>();
            HistorialArchivos objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new HistorialArchivos();
                objEntidad2.pContrato = Convertidor.aCadena(fila[HistorialArchivosDEF.Contrato]);
                objEntidad2.pRespuesta = Convertidor.aCadena(fila[HistorialArchivosDEF.Respuesta]);
                objEntidad2.pCausal = Convertidor.aCadena(fila[HistorialArchivosDEF.Causal]);
                objEntidad2.pEstado = Convertidor.aCadena(fila[HistorialArchivosDEF.Estado]);
                objEntidad2.pTipo_transferencia = Convertidor.aCadena(fila[HistorialArchivosDEF.Tipo_Transferencia]);
                objEntidad2.pbanco = Convertidor.aCadena(fila[HistorialArchivosDEF.Banco]);
                objEntidad2.pValor = Convertidor.aCadena(fila[HistorialArchivosDEF.Valor]);
                objEntidad2.pFechaProceso = Convertidor.aCadena(fila[HistorialArchivosDEF.FechaProceso]);
                objEntidad2.pNombreCliente = Convertidor.aCadena(fila[HistorialArchivosDEF.Nombre_Cliente]);
                objEntidad2.pNombreArchivo = Convertidor.aCadena(fila[HistorialArchivosDEF.NombreArchivo]);
                objEntidad2.pMarca = true; 
                lista.Add(objEntidad2);
            }

            return lista;
        }


        public List<HistorialArchivos> consultarCon(HistorialArchivos objEntidad)
        {

            DataSet datos = ejecutarConsulta(objEntidad);

            List<HistorialArchivos> lista = new List<HistorialArchivos>();
            HistorialArchivos objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new HistorialArchivos();
                objEntidad2.pConsecutivo = Convertidor.aCadena(fila[HistorialArchivosDEF.Consecutivo]);
                lista.Add(objEntidad2);
            }
            return lista;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int ejecutarNoConsulta(HistorialArchivos objEntidad)
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

        public DataSet consultarArchivosFecha(HistorialArchivos objRT)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_Deb_Historial_Archivos_Por_Fecha", conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pFecha", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@pFecha"].Value = objRT.pFecha;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pTipoArchivo", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@pTipoArchivo"].Value = objRT.pTipoArchivo;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pManual", SqlDbType.VarChar));
                if (objRT.pManual != null)
                {
                    if (objRT.pManual.Value)
                        adaptador.SelectCommand.Parameters["@pManual"].Value = "1";
                    else
                        adaptador.SelectCommand.Parameters["@pManual"].Value = "0";
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pManual"].Value = String.Empty;
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

        //BUSQUEDA DE TODAS LAS FECHAS EN EL CUAL EXISTE UN ARCHIVO CON COINCIDENCIA A UN BANCO
        public DataTable consultarMaxLote()
        {
            String query = "SELECT MAX(CONSECUTIVO) AS CONSECUTIVO FROM dbo.tb_DEB_HISTORIAL_ARCHIVOS ";
            return consultar(query);
        }
        //BUSQUEDA DE TODOS LOS CONSECUTIVOS DEL ARCHIVO CON COINCIDENCIA A UN BANCO Y UNA FECHA
        public DataTable consultarConsecutivoXBanco(String Banco, String TipoArchivoS, String Fecha)
        {
            String query = "SELECT CONSECUTIVO FROM tb_DEB_HISTORIAL_ARCHIVOS"
                + " WHERE CODIGO_BANCO = '" + Banco + "' AND TIPO_ARCHIVO = '" + TipoArchivoS 
                + "' AND FECHA = '" + Fecha + "' GROUP BY CONSECUTIVO ORDER BY CONSECUTIVO";
            return consultar(query);
        }
        // RETORNA LAS LINEAS DE UN ARCHIVO CON COINCIDENCIA A UN BANCO, UNA FECHA Y CONSECUTIVO
        public DataTable consultarLineasConsecutivo(String Banco, String TipoArchivoS, String Fecha, String Consecutivo)
        {
            String query = "SELECT LINEAS_ARCHIVO FROM tb_DEB_HISTORIAL_ARCHIVOS"
                + " WHERE CODIGO_BANCO = '" + Banco + "' AND TIPO_ARCHIVO = '" + TipoArchivoS 
                + "' AND FECHA = '" + Fecha + "' AND CONSECUTIVO = '" + Consecutivo + "'";
            return consultar(query);
        }
    }
}
