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
    public class ArchivoManualAD
    {
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public ArchivoManualAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        protected DataSet ejecutarConsulta(ArchivoManual objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Archivo_Manual", conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pOperacion", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@pOperacion"].Value = objEntidad.pOperacion;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pContrato", SqlDbType.VarChar));
                if (objEntidad.pContrato > 0)
                {
                    adaptador.SelectCommand.Parameters["@pContrato"].Value = objEntidad.pContrato;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pContrato"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pNombre", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pNombre))
                {
                    adaptador.SelectCommand.Parameters["@pNombre"].Value = objEntidad.pNombre;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pNombre"].Value = String.Empty;
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pAutorizar", SqlDbType.VarChar));
                if (objEntidad.pAutorizar != null)
                {
                    if (objEntidad.pAutorizar.Value)
                        adaptador.SelectCommand.Parameters["@pAutorizar"].Value = "1";
                    else
                        adaptador.SelectCommand.Parameters["@pAutorizar"].Value = "0";
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pAutorizar"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pEstadoCliente", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pEstadoCliente))
                {
                    adaptador.SelectCommand.Parameters["@pEstadoCliente"].Value = objEntidad.pEstadoCliente;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pEstadoCliente"].Value = String.Empty;
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

        public List<ArchivoManual> consultar(ArchivoManual objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DataSet datos = ejecutarConsulta(objEntidad);

            List<ArchivoManual> lista = new List<ArchivoManual>();
            ArchivoManual objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new ArchivoManual();
                objEntidad2.pContrato = Convertidor.aEntero32(fila[ArchivoManualDEF.Contrato]);
                objEntidad2.pNombre = Convertidor.aCadena(fila[ArchivoManualDEF.Nombre]);
                objEntidad2.pNumeroCuenta = Convertidor.aCadena(fila[ArchivoManualDEF.NumeroCuenta]);
                objEntidad2.pTipoCuenta = Convertidor.aCadena(fila[ArchivoManualDEF.TipoCuenta]);
                objEntidad2.pValor = Convertidor.aCadena(fila[ArchivoManualDEF.Valor]);
                objEntidad2.pAutorizar = Convertidor.aBooleano(fila[ArchivoManualDEF.Autorizar]);
                objEntidad2.pEstadoCliente = Convertidor.aCadena(fila[ArchivoManualDEF.EstadoCliente]);
                objEntidad2.pBanco = Convertidor.aCadena(fila[ArchivoManualDEF.Banco]);
                lista.Add(objEntidad2);
            }

            return lista;
        }

        public int ejecutarNoConsulta(ArchivoManual objEntidad)
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

        public DataSet ConsultarArchivoManual(String Estado, String Banco, String Fecha, String BancoDebita)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Consultar_Archivo_Manual", conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@Estado", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@Estado"].Value = Estado;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@Banco", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@Banco"].Value = Banco;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@Fecha", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@Fecha"].Value = Fecha;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@BancoDebita", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@BancoDebita"].Value = BancoDebita;

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

        public DataSet ConsultaDebitoManual(ArchivoManual objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Consulta_Debito_Manual", conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@Fecha", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@Fecha"].Value = objEntidad.pFecha;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@Contrato", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@Contrato"].Value = objEntidad.pContrato;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@Banco", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@Banco"].Value = objEntidad.pBanco;

                datos = new DataSet();
                adaptador.Fill(datos, "TablaManual");
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

        private DataTable consultar(String query)
        {
            Querys objQuery = new Querys();
            DataTable datos = objQuery.consultarDatos(query).Tables["tabla"];
            Error = objQuery.Error;
            return datos;
        }
    }
}
