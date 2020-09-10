using log4net;
using ServicioDebito.AD.Conexion;
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
    public class TitularCuentaInconsistenteAD
    {
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public TitularCuentaInconsistenteAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        public DataSet ejecutarConsulta(TitularCuentaInconsistente objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexionSQL();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Titular_Cuenta_Inconsistente", conexion);
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pNombre", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pNombre))
                {
                    adaptador.SelectCommand.Parameters["@pNombre"].Value = objEntidad.pNombre;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pNombre"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pTipoIdentificacion", SqlDbType.VarChar));
                if (objEntidad.pTipoIdentificacion > 0)
                {
                    adaptador.SelectCommand.Parameters["@pTipoIdentificacion"].Value = objEntidad.pTipoIdentificacion;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pTipoIdentificacion"].Value = String.Empty;
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

                datos = new DataSet();
                adaptador.Fill(datos, "Titular Inconsistente");
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


        public int ejecutarNoConsulta(TitularCuentaInconsistente objEntidad)
        {
            int cuenta = -1;
            DataSet datos = ejecutarConsulta(objEntidad);
            try
            {
                cuenta = Convertidor.aEntero32(datos.Tables["Titular Inconsistente"].Rows[0]["Cuenta"]);
            }
            catch (Exception ex)
            {
                Registrador.Warn(ex.Message);
            }
            return cuenta;
        }
    }

}
