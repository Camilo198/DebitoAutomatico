using log4net;
using ServicioDebito.AD.Conexion;
using ServicioDebito.EN;
using ServicioDebito.EN.Definicion;
using ServicioDebito.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioDebito.AD.Consultas
{
    public class ClienteSICOAD
    {
        public String Error { get; set; }

        DataSet dsBasico = new DataSet();
        OdbcDataAdapter DataAdapterOdbc = new OdbcDataAdapter();
        ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
        OdbcConnection Sic = new System.Data.Odbc.OdbcConnection();
        OdbcCommand odbcCommand = new OdbcCommand();

        public ILog Registrador { get; set; }

        public ClienteSICOAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }



        public DataSet ejecutarConsultaSICO(ClienteSico objEntidad, String Procedimiento)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexionSQL();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter(Procedimiento, conexion);
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


        public DataSet ejecutarConsultaCesion(ClienteSico objEntidad, String Procedimiento)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexionSQLSicoDiario();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter(Procedimiento, conexion);
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

                //adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pTipoDoc", SqlDbType.VarChar));
                //if (!String.IsNullOrEmpty(objEntidad.pTipoDocumento))
                //{
                //    adaptador.SelectCommand.Parameters["@pTipoDoc"].Value = objEntidad.pTipoDocumento;
                //}
                //else
                //{
                //    adaptador.SelectCommand.Parameters["@pTipoDoc"].Value = String.Empty;
                //}

                //adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pNroDocumento", SqlDbType.VarChar));
                //if (objEntidad.pNroDocumento > 0)
                //{
                //    adaptador.SelectCommand.Parameters["@pNroDocumento"].Value = objEntidad.pNroDocumento;
                //}
                //else
                //{
                //    adaptador.SelectCommand.Parameters["@pNroDocumento"].Value = String.Empty;
                //}

                datos = new DataSet();
                adaptador.Fill(datos, "InfoCesion");
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
