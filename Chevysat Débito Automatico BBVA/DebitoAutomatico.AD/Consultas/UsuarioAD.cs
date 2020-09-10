using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using DebitoAutomatico.AD.Conexion;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Definicion;
using DebitoAutomatico.EN.Tablas;
using log4net;
using System.Data;
using System.Data.SqlClient;

namespace DebitoAutomatico.AD.Consultas
{
    public class UsuarioAD
    {
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public UsuarioAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        protected DataSet ejecutarConsulta(Usuario objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Usuario", conexion);
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
                    adaptador.SelectCommand.Parameters["@pId"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pUsuario", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pUsuario))
                {
                    adaptador.SelectCommand.Parameters["@pUsuario"].Value = objEntidad.pUsuario;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pUsuario"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pPassword", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pPassword))
                {
                    adaptador.SelectCommand.Parameters["@pPassword"].Value = objEntidad.pPassword;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pPassword"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdPerfil", SqlDbType.VarChar));
                if (objEntidad.pIdPerfil > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdPerfil"].Value = objEntidad.pIdPerfil;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdPerfil"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pHabilita", SqlDbType.VarChar));
                if (objEntidad.pHabilita != null)
                {
                    if (objEntidad.pHabilita.Value)
                        adaptador.SelectCommand.Parameters["@pHabilita"].Value = "1";
                    else
                        adaptador.SelectCommand.Parameters["@pHabilita"].Value = "0";
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pHabilita"].Value = String.Empty;
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


        public List<Usuario> consultar(Usuario objEntidad)
        {
            DataSet datos = ejecutarConsulta(objEntidad);

            List<Usuario> lista = new List<Usuario>();
            Usuario objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new Usuario();
                objEntidad2.pId = Convertidor.aEntero32(fila[UsuarioDEF.Id]);
                objEntidad2.pUsuario = Convertidor.aCadena(fila[UsuarioDEF.Usuario]);
                objEntidad2.pPassword = Convertidor.aCadena(fila[UsuarioDEF.Password]);
                objEntidad2.pIdPerfil = Convertidor.aEntero32(fila[UsuarioDEF.IdPerfil]);
                objEntidad2.pHabilita = Convertidor.aBooleano(fila[UsuarioDEF.Habilita]);
                lista.Add(objEntidad2);
            }
            return lista;
        }

        public List<Usuario> consultarChevy(Usuario objEntidad)
        {
            DataSet datos = ejecutarConsulta(objEntidad);

            List<Usuario> lista = new List<Usuario>();
            Usuario objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new Usuario();
                objEntidad2.pUsuario = Convertidor.aCadena(fila[UsuarioDEF.Usuario]);
                lista.Add(objEntidad2);
            }
            return lista;
        }

        public int ejecutarNoConsulta(Usuario objEntidad)
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

    }
}
