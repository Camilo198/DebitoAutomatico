using System;
using System.Data.SqlClient;
using System.Configuration;
using DebitoAutomatico.AD.Administracion;
using log4net;
using System.Data.Odbc;


namespace DebitoAutomatico.AD.Conexion
{
    public class ConectorBD
    {
        public String Error { get; set; }
        public bool SeEstablecioConexion { get; set; }

        private ConectorBD() { }
        private static ConectorBD conexion = null;

        /// <summary>
        /// Establece la conexion con la base de datos.
        /// </summary>
        /// <returns>Conexion con la base de datos, para realizar transacciones</returns>
        public SqlConnection abrirConexion()
        {
            ILog log = LogManager.GetLogger("DebitoAutomatico.AD.Conexion.ConectorBD");
            ConsultorXML objConsultorXML = new ConsultorXML();
            String cadenaConexion = objConsultorXML.leerCadenaConexion();
            SqlConnection objSqlConect = new SqlConnection();
            objSqlConect.ConnectionString = cadenaConexion;
            try
            {
                objSqlConect.Open();
                SeEstablecioConexion = true;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                log.Fatal(Error);
                SeEstablecioConexion = false;
            }
            return objSqlConect;
        }


        /// <summary>
        /// Establece la conexion con la base de datos.
        /// </summary>
        /// <returns>Conexion con la base de datos, para realizar transacciones</returns>
        public OdbcConnection abrirConexionOdbc()
        {
            ILog log = LogManager.GetLogger("DebitoAutomatico.AD.Conexion.ConectorBD");
            ConsultorXML objConsultorXML = new ConsultorXML();
            String cadenaConexion = objConsultorXML.leerCadenaConexionOdbc();
            OdbcConnection objOdbcConect = new OdbcConnection();
            objOdbcConect.ConnectionString = cadenaConexion;
            try
            {
                objOdbcConect.Open();
                SeEstablecioConexion = true;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                log.Fatal(Error);
                SeEstablecioConexion = false;
            }
            return objOdbcConect;
        }

        /// <summary>
        /// Retorna la instacia de la coneción actual, o la crea en caso de que este nula
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
