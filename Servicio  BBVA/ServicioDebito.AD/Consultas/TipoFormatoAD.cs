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
    public class TipoFormatoAD
    {
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public TipoFormatoAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        public DataSet ejecutarConsulta(TipoFormato objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexionSQL();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Tipo_Formato", conexion);
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pValor", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pValor))
                {
                    adaptador.SelectCommand.Parameters["@pValor"].Value = objEntidad.pValor;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pValor"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pValorNuevo", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pValorNuevo))
                {
                    adaptador.SelectCommand.Parameters["@pValorNuevo"].Value = objEntidad.pValorNuevo;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pValorNuevo"].Value = String.Empty;
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
