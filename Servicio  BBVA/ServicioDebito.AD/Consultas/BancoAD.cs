using log4net;
using ServicioDebito.AD.Conexion;
using ServicioDebito.EN.Definicion;
using ServicioDebito.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioDebito.AD.Consultas
{
    public class BancoAD
    {
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public BancoAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        public DataSet ejecutarConsulta(Banco objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexionSQL();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Banco", conexion);
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCodigo", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pCodigo))
                {
                    adaptador.SelectCommand.Parameters["@pCodigo"].Value = objEntidad.pCodigo;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCodigo"].Value = String.Empty;
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pNit", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pNit))
                {
                    adaptador.SelectCommand.Parameters["@pNit"].Value = objEntidad.pNit;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pNit"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pActivo", SqlDbType.VarChar));
                if (objEntidad.pActivo != null)
                {
                    if (objEntidad.pActivo.Value)
                        adaptador.SelectCommand.Parameters["@pActivo"].Value = "1";
                    else
                        adaptador.SelectCommand.Parameters["@pActivo"].Value = "0";
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pActivo"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pDebito", SqlDbType.VarChar));
                if (objEntidad.pDebito != null)
                {
                    if (objEntidad.pDebito.Value)
                        adaptador.SelectCommand.Parameters["@pDebito"].Value = "1";
                    else
                        adaptador.SelectCommand.Parameters["@pDebito"].Value = "0";
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pDebito"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCorreoControl", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pCorreoControl))
                {
                    adaptador.SelectCommand.Parameters["@pCorreoControl"].Value = objEntidad.pCorreoControl;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCorreoControl"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCorreoEnvio", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pCorreoEnvio))
                {
                    adaptador.SelectCommand.Parameters["@pCorreoEnvio"].Value = objEntidad.pCorreoEnvio;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCorreoEnvio"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pRemitente", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pRemitente))
                {
                    adaptador.SelectCommand.Parameters["@pRemitente"].Value = objEntidad.pRemitente;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pRemitente"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdRuta", SqlDbType.VarChar));
                if (objEntidad.pIdRuta > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdRuta"].Value = objEntidad.pIdRuta;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdRuta"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdPrenota", SqlDbType.VarChar));
                if (objEntidad.pIdPrenota > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdPrenota"].Value = objEntidad.pIdPrenota;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdPrenota"].Value = String.Empty;
                }


                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdPrenotaManual", SqlDbType.VarChar));
                if (objEntidad.pIdPrenotaManual > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdPrenotaManual"].Value = objEntidad.pIdPrenotaManual;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdPrenotaManual"].Value = String.Empty;
                }


                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdDebito", SqlDbType.VarChar));
                if (objEntidad.pIdDebito > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdDebito"].Value = objEntidad.pIdDebito;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdDebito"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdDebitoManual", SqlDbType.VarChar));
                if (objEntidad.pIdDebitoManual > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdDebitoManual"].Value = objEntidad.pIdDebitoManual;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdDebitoManual"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdPagos", SqlDbType.VarChar));
                if (objEntidad.pIdPagos > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdPagos"].Value = objEntidad.pIdPagos;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdPagos"].Value = String.Empty;
                }


                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdErrores", SqlDbType.VarChar));
                if (objEntidad.pIdErrores > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdErrores"].Value = objEntidad.pIdErrores;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdErrores"].Value = String.Empty;
                }


                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdRecibidos", SqlDbType.VarChar));
                if (objEntidad.pIdRecibidos > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdRecibidos"].Value = objEntidad.pIdRecibidos;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdRecibidos"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdHistorico", SqlDbType.VarChar));
                if (objEntidad.pIdHistorico > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdHistorico"].Value = objEntidad.pIdHistorico;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdHistorico"].Value = String.Empty;
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
    }
}
