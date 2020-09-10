using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace ServicioDebito.AD.Conexion
{
    internal class ConectorBD
    {
        private ConectorBD() { }
        private static ConectorBD conexion = null;

        public SqlConnection abrirConexionSQL()
        {
            SqlConnection objSqlConect = new SqlConnection();
            try
            {
                String cadenaConexion = ConfigurationManager.ConnectionStrings["SQLDebito"].ConnectionString;
                objSqlConect.ConnectionString = cadenaConexion;
                objSqlConect.Open();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return objSqlConect;
        }

        public SqlConnection abrirConexionSQLSicoDiario()
        {
            SqlConnection objSqlConect = new SqlConnection();
            try
            {
                String cadenaConexion = ConfigurationManager.ConnectionStrings["SQLSico"].ConnectionString;
                objSqlConect.ConnectionString = cadenaConexion;
                objSqlConect.Open();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return objSqlConect;
        }

        public OdbcConnection abrirConexionSICO()
        {
            OdbcConnection objOdbcConect = new OdbcConnection();
            try
            {
                String cadenaConexion = ConfigurationManager.ConnectionStrings["SICO"].ConnectionString;
                objOdbcConect.ConnectionString = cadenaConexion;
                objOdbcConect.Open();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return objOdbcConect;
        }

        /// <summary>
        /// Retorna la instacia de la conexión actual, o la crea en caso de que este nula
        /// </summary>
        /// <returns>Instancia de la conexion actual</returns>
        public static ConectorBD obtenerInstancia()
        {
            if (conexion == null)
                conexion = new ConectorBD();
            return conexion;
        }
    }
}
