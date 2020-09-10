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
    public class PerfilAD
    {
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public PerfilAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        protected DataSet ejecutarConsulta(Perfil objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Perfil", conexion);
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pPerfil", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pPerfil))
                {
                    adaptador.SelectCommand.Parameters["@pPerfil"].Value = objEntidad.pPerfil;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pPerfil"].Value = DBNull.Value;
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


        public List<Perfil> consultar(Perfil objEntidad)
        {
            DataSet datos = ejecutarConsulta(objEntidad);

            List<Perfil> lista = new List<Perfil>();
            Perfil objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new Perfil();
                objEntidad2.pId = Convertidor.aEntero32(fila[PerfilDEF.Id]);
                objEntidad2.pPerfil = Convertidor.aCadena(fila[PerfilDEF.Perfil]);
                lista.Add(objEntidad2);
            }
            return lista;
        }

    }
}
