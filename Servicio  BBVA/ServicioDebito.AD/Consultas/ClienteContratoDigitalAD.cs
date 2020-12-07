using log4net;
using ServicioDebito.AD.Conexion;
using ServicioDebito.EN.Tablas;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ServicioDebito.AD.Consultas
{
    public class ClienteContratoDigitalAD
    {
        public String Error { get; set; }
        public ILog Registrador { get; set; }
        public ClienteContratoDigitalAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }
        public DataSet ejecutarConsultaContratoDigital(ClienteContratoDigitalEN objEntidad, String Procedimiento)
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

                datos = new DataSet();
                adaptador.Fill(datos, "ClienteContratoDigital");
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
