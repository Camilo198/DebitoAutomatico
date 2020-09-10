using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using DebitoAutomatico.AD.Conexion;
using System.Data.SqlClient;
using DebitoAutomatico.EN.Tablas;
using log4net;

namespace DebitoAutomatico.AD.Consultas
{
    public class ClienteSicoAD
    {
        public String Error { get; set; }

        DataSet dsBasico = new DataSet();
        OdbcDataAdapter DataAdapterOdbc = new OdbcDataAdapter();
        ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
        OdbcConnection Sic = new System.Data.Odbc.OdbcConnection();
        OdbcCommand odbcCommand = new OdbcCommand();

        public ILog Registrador { get; set; }

        public ClienteSicoAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        public DataSet ejecutarConsulta(ClienteSico objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Consulta_Clientes", conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pContrato", SqlDbType.VarChar));
                if (objEntidad.pContrato > 0)
                {
                    adaptador.SelectCommand.Parameters["@pContrato"].Value = objEntidad.pContrato;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pContrato"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pNroDocumento", SqlDbType.VarChar));
                if (objEntidad.pNroDocumento > 0)
                {
                    adaptador.SelectCommand.Parameters["@pNroDocumento"].Value = objEntidad.pNroDocumento;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pNroDocumento"].Value = String.Empty;
                }

                datos = new DataSet();
                adaptador.Fill(datos, "ClienteSICO");
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

        public DataSet ejecutarConsultaLectura(ClienteSico objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Consulta_Cliente_Lectura", conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pContrato", SqlDbType.VarChar));
                if (objEntidad.pContrato > 0)
                {
                    adaptador.SelectCommand.Parameters["@pContrato"].Value = objEntidad.pContrato;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pContrato"].Value = String.Empty;
                }

                datos = new DataSet();
                adaptador.Fill(datos, "ClienteSICO");
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

        public DataSet ejecutarConsultaCorreo(ClienteSico objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Consulta_Cliente_Correo", conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pContrato", SqlDbType.VarChar));
                if (objEntidad.pContrato > 0)
                {
                    adaptador.SelectCommand.Parameters["@pContrato"].Value = objEntidad.pContrato;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pContrato"].Value = String.Empty;
                }
                datos = new DataSet();
                adaptador.Fill(datos, "ClienteSICO");
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

        public DataSet ejecutarConsultaDebito(ClienteSico objEntidad)
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pContrato", SqlDbType.VarChar));
                if (objEntidad.pContrato > 0)
                {
                    adaptador.SelectCommand.Parameters["@pContrato"].Value = objEntidad.pContrato;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pContrato"].Value = String.Empty;
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


        //CONSULTA DE LA NUEVA TABLA DE SICO CON EL VALOR PROYECTO DE DEBITO AUTOMATICO
        //public DataSet Sico_ConsultaTablaDebito(String FechaDebito, bool sus, bool adj, bool gana, bool cuoxdevol, Int32 vencimiento)
        //{

        //    Sic = objConexionDB.abrirConexionOdbc();
        //    Error = Sic.State.ToString();

        //    try
        //    {
        //        DataAdapterOdbc = new OdbcDataAdapter("SELECT DbatContrato,DbatCupo,DbatValorAPagar FROM DBAT where DbatEstado='S'", Sic);
        //        DataAdapterOdbc.Fill(dsBasico, "DBAT");
        //        if (dsBasico.Tables["DBAT"].Rows.Count > 0)
        //        {
        //            Int32 grupo, afiliacion, nivel = 0;
        //            String EstadoCliente = String.Empty;
        //            dsBasico.Tables["DBAT"].Columns.Add("TipoCliente");

        //            foreach (DataRow row in dsBasico.Tables["DBAT"].Rows)
        //            {
        //                if (row[1].ToString().Length < 8)
        //                {
        //                    row.Delete();
        //                    continue;
        //                }

        //                if (row[1].ToString().Length == 9)
        //                {
        //                    grupo = Convert.ToInt32(row[1].ToString().Substring(0, 4));
        //                    afiliacion = Convert.ToInt32(row[1].ToString().Substring(4, 3));
        //                    nivel = Convert.ToInt32(row[1].ToString().Substring(7, 2));
        //                }
        //                else
        //                {
        //                    grupo = Convert.ToInt32(row[1].ToString().Substring(0, 3));
        //                    afiliacion = Convert.ToInt32(row[1].ToString().Substring(3, 3));
        //                    nivel = Convert.ToInt32(row[1].ToString().Substring(6, 2));
        //                }

        //                odbcCommand = new OdbcCommand("SELECT AfilTipo FROM AFIL "
        //                                                           + "WHERE AfilGrupo=" + grupo.ToString() + " and"
        //                                                           + " AfilNroAf=" + afiliacion.ToString() + " and"
        //                                                           + " AfilNivAf=" + nivel.ToString(), Sic);

        //                EstadoCliente = odbcCommand.ExecuteScalar().ToString();
        //            }
        //        }
        //        dsBasico.AcceptChanges();
        //        return dsBasico;
        //    }
        //    catch (Exception r)
        //    {
        //        string m = r.Message;
        //        return dsBasico;
        //    }
        //    finally
        //    {
        //        DataAdapterOdbc.Dispose();
        //        Sic.Close();
        //    }

        //}

    }
}
