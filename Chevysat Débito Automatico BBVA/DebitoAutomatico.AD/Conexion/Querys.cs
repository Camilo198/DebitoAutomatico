using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DebitoAutomatico.AD.Conexion
{
    public class Querys
    {
        public String Error { get; set; }

        public Querys()
        {
            Error = String.Empty;
        }

        #region "Funcion: consultarDatos(String sql)"
        /// <summary>
        /// Consulta datos del comando sql enviado
        /// </summary>
        /// <param name="sql">Comando sql</param>
        /// <returns>Datos que se recuprearon de la consulta</returns>
        public DataSet consultarDatos(String sql)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter();
                adaptador.SelectCommand = new SqlCommand(sql, conexion);
                datos = new DataSet();
                adaptador.Fill(datos, "tabla");
                adaptador.Dispose();
            }
            catch (SqlException ex)
            {
                Error = ex.Message;
            }
            finally
            {
                if (conexion.State != ConnectionState.Closed)
                    conexion.Close();
            }
            return datos;
        }
        #endregion

        #region "Funcion: insertarActualizarDatos(String sql)"
        /// <summary>
        /// Ejecuta un comando sql, para insertar o actualizar datos
        /// </summary>
        /// <param name="sql">Comando SQL</param>
        /// <returns>Numero de filas afectadas</returns>
        public int insertarActualizarDatos(String sql)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlConnection conexion = null;
            SqlCommand comando;
            int filasAfectadas = -1;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                comando = new SqlCommand(sql, conexion);
                filasAfectadas = comando.ExecuteNonQuery();
                comando.Dispose();
            }
            catch (SqlException ex)
            {
                Error = ex.Message;
            }
            finally
            {
                if (conexion.State != ConnectionState.Closed)
                    conexion.Close();
            }

            return filasAfectadas;
        }
        #endregion
    }
}
