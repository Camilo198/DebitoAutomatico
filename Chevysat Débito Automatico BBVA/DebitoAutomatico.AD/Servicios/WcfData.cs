using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Configuration;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;


namespace DebitoAutomatico.AD.Servicios
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "WcfData" en el código y en el archivo de configuración a la vez.
    public class WcfData : IWcfData, IDisposable
    {
        private SqlParameter[] parametro;


        public SqlConnection ConectaSql(string key)
        {
            /// <summary>
            /// Realiza conexión a bases de datos SQL Server
            /// </summary>
            /// <param name="key">Clave que identifica el string de conexión en los connectionStrings</param>
            /// <returns>Retorna una conexión</returns>
            string strConnString = ConfigurationManager.ConnectionStrings[key].ConnectionString;
            SqlConnection Conn = new System.Data.SqlClient.SqlConnection();
            Conn.ConnectionString = strConnString;
            try
            {
                Conn.Open();
                return Conn;
            }
            catch (Exception ex)
            {
                string m = ex.Message;
                return Conn;
            }
        }

        /// <summary>
        /// Realiza una conexión tipo Odbc
        /// </summary>
        /// <param name="key">Clave con la que identifica el string de conexión</param>
        /// <returns>conexión abierta</returns>
        public OdbcConnection ConectaOdbc(string key)
        {

            string strConnString = ConfigurationManager.ConnectionStrings[key].ConnectionString;
            OdbcConnection Conn = new System.Data.Odbc.OdbcConnection();
            Conn.ConnectionString = strConnString;
            try
            {
                Conn.Open();
                return Conn;
            }
            catch (Exception ex)
            {
                string m = ex.Message;
                return Conn;
            }
        }

        /// <summary>
        /// Crea un sqlParameter con el número de posiciones indicadas
        /// </summary>
        /// <param name="tamaño">Número de posiciones con que se creará el arreglo</param>
        public void CreaParametro(int tamaño)
        {
            parametro = new SqlParameter[tamaño];
        }

        /// <summary>
        /// Adiciona un parámetro al arreglo creado
        /// </summary>
        /// <param name="param">Nombre del parámetro</param>
        /// <param name="valor">Valor del parámetro</param>
        /// <param name="posicion">Posición en el arreglo para el parámtro</param>
        public void AdicionaParametro(String param, object valor, int posicion)
        {
            parametro[posicion] = new SqlParameter(param, valor);

        }

        /// <summary>
        /// Adiciona un parametro a la colección creada mediante el método CreaParametro
        /// </summary>
        /// <param name="param">Nombre del parametro</param>
        /// <param name="valor">Valor a asignar al parámetro</param>
        /// <param name="posicion">posición del arreglo en el que va el parametro</param>
        public void AdicionaParametro2(String param, object valor, int posicion, string SqlTipo)
        {
            switch (SqlTipo)
            {
                case "int": parametro[posicion] = new SqlParameter(param, Convert.ToInt32(valor));
                    break;
                case "double": parametro[posicion] = new SqlParameter(param, Convert.ToDouble(valor));
                    break;
                case "datetime": parametro[posicion] = new SqlParameter(param, Convert.ToDateTime(valor));
                    break;
                case "date": parametro[posicion] = new SqlParameter(param, Convert.ToDateTime(valor));
                    break;
                case "bool": parametro[posicion] = new SqlParameter(param, Convert.ToBoolean(valor));
                    break;
                default: parametro[posicion] = new SqlParameter(param, valor);
                    break;

            }

        }


        /// <summary>
        /// Ejecuta un NonQuery creando el arreglo para los parámetros
        /// </summary>
        /// <param name="Valor">Tamaño del arreglo para los paramétros</param>
        /// <param name="procedimiento">Procedimiento almacenado que ejecuta</param>
        /// <param name="strConn">Clave que identifica la cadena de conexión en los connectionStrings</param>
        /// <returns></returns>
        public string Ejecutar(string[, ,] Valor, string procedimiento, string strConn)
        {
            try
            {
                int count = Valor.Length / Valor.Rank;
                CreaParametro(count);
                for (int i = 0; i < count; i++)
                {
                    AdicionaParametro2(Valor[i, 1, 0].ToString(), Valor[i, 0, 0].ToString(), i, Valor[i, 2, 0].ToString());
                }
                string pasa = Actualizar(procedimiento, parametro, ConectaSql(strConn));
                return pasa;
            }
            catch (Exception exc)
            {
                return "0 " + exc.Message;
            }

        }

        /// <summary>
        /// Realiza el proceso de actualizacion de la base de datos, (Insertar, Actualizar o Borrar)")
        /// </summary>
        /// <param name="procedimiento">Nombre del Procedimiento Almacenado que se ejecutara</param>
        /// <param name="parametros"> sqlParameter[] con los parametros que recibe el Procedimiento</param>
        /// <param name="conexion">Nombre de la conexion a la base de datos que utiliza</param>
        /// <returns>Retorna en número de registros afectados</returns>
        public string Actualizar(string procedimiento, SqlParameter[] parametros, SqlConnection ActConexion)
        {
            int rowCount = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = ActConexion;
            cmd.CommandText = procedimiento;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            for (int i = 0; i < parametros.Length; i++)
            {
                cmd.Parameters.Add(parametros[i]);
            }
            try
            {
                SqlParameter Parameter = cmd.Parameters.Add("@RowCount", SqlDbType.Int);
                Parameter.Direction = ParameterDirection.ReturnValue;
                cmd.ExecuteNonQuery();
                rowCount = (Int32)cmd.Parameters["@RowCount"].Value;
                return rowCount.ToString().Trim();
            }
            catch (Exception e)
            {

                return "0" + e.Message;
            }
            finally
            {
                cmd.Dispose();
                ActConexion.Close();
                ActConexion.Dispose();
            }

        }




        /// <summary>
        /// llena el arreglo de parametros
        /// </summary>
        /// <param name="Valor">Tamaño del arreglo</param>
        /// <returns></returns>
        public string LlenaParametro(string[, ,] Valor)
        {
            try
            {
                int count = Valor.Length / Valor.Rank;
                CreaParametro(count);
                for (int i = 0; i < count; i++)
                {
                    AdicionaParametro2(Valor[i, 1, 0].ToString(), Valor[i, 0, 0].ToString(), i, Valor[i, 2, 0].ToString());
                }
                return "OK";

            }
            catch (Exception )
            {
                return "NO";
            }

        }


        /// <summary>
        /// Realiza una consulta a base de datos mediante SqlClint o OdbcClient y con sentencia Sql estandar o procedimiento almacenado
        /// </summary>
        /// <param name="Parametro">Colección de parámetros que recibe el procedimiento almacenado</param>
        /// <param name="sentenciaSql">Sentencia SQL con la consulta o nombre del procedimiento almacenado</param>
        /// <param name="key">Clave que identifica el string de conexión</param>
        /// <param name="TipoCons">Indica si la consulta es mediante procedimiento almacenado o sentencia</param>
        /// <param name="TipConeccion">Indica si la conexión es odbc o sql</param>
        /// <returns>Retorna una lista de arreglos de tres posiciones donde almacena nombre de la columna, valor, y tipo de dato, con los resultados de la consulta</returns>
        public List<string[,]> LlenarLista(string[, ,] Parametro, string sentenciaSql, string key, string TipoCons, string TipConeccion)
        {
            List<string[,]> Lista = new List<string[,]>();
            string[,] _fila;
            try
            {
                string strConnString = ConfigurationManager.ConnectionStrings[key].ConnectionString;

                if (TipConeccion == "Sql")
                {
                    SqlConnection Con = ConectaSql(key);
                    try
                    {
                        SqlDataReader DataReader;
                        SqlCommand Command = new SqlCommand();
                        Command.Connection = Con;
                        if (TipoCons == "SP")
                        {
                            Command.CommandType = CommandType.StoredProcedure;
                            Command.CommandText = sentenciaSql;
                            LlenaParametro(Parametro);
                            for (int i = 0; i < parametro.Length; i++)
                            {
                                Command.Parameters.Add(parametro[i]);
                            }

                        }
                        else
                        {
                            Command.CommandType = CommandType.Text;
                            Command.CommandText = sentenciaSql;
                        }

                        DataReader = Command.ExecuteReader();

                        if (DataReader.HasRows)
                        {
                            int Columnas = DataReader.FieldCount;

                            string j = DataReader.GetName(0);
                            while (DataReader.Read())
                            {
                                _fila = new string[Columnas, 3];
                                for (int i = 0; i < Columnas; i++)
                                {
                                    _fila[i, 0] = DataReader.GetName(i);
                                    _fila[i, 1] = DataReader[i].ToString();
                                    _fila[i, 2] = DataReader[i].GetType().ToString();
                                }
                                Lista.Add(_fila);
                            }
                        }
                        return Lista;

                    }
                    catch (Exception r)
                    {
                        string m = r.Message;
                        return Lista;

                    }
                    finally
                    {
                        Con.Close();
                    }
                }
                else
                {
                    OdbcConnection Con = ConectaOdbc(key);
                    try
                    {

                        OdbcCommand Command = new OdbcCommand();
                        Command.CommandType = CommandType.Text;
                        Command.CommandText = sentenciaSql;
                        OdbcDataReader DataReader;
                        Command.Connection = Con;
                        DataReader = Command.ExecuteReader();
                        if (DataReader.HasRows)
                        {
                            int Columnas = DataReader.FieldCount;

                            string j = DataReader.GetName(0);
                            while (DataReader.Read())
                            {
                                _fila = new string[Columnas, 3];
                                for (int i = 0; i < Columnas; i++)
                                {
                                    _fila[i, 0] = DataReader.GetName(i);
                                    _fila[i, 1] = DataReader[i].ToString();
                                    _fila[i, 2] = DataReader[i].GetType().ToString();
                                }
                                Lista.Add(_fila);
                            }
                        }
                        return Lista;
                    }
                    catch (Exception r)
                    {
                        string m = r.Message;
                        return Lista;
                    }
                    finally
                    {
                        Con.Close();
                    }

                }
            }
            catch (Exception r)
            {
                string m = r.Message;
                return Lista;
            }
        }

        /// <summary>
        /// Realiza una consulta de tipo SQL mediante sentencia Sql estandar o procedimiento almacenado
        /// </summary>
        /// <param name="sentenciaSql">Sentencia sql que realiza la consulta o nombre del procedimiento almacenado</param>
        /// <param name="key">Nombre que identifica el string de conexión</param>
        /// <returns>Retorna un string con el resultado de la consulta</returns>
        public string ConsultaSqlDato(string procedimiento, string key)
        {
            string dato = string.Empty;
            string strConnString = ConfigurationManager.ConnectionStrings[key].ConnectionString;
            SqlConnection SqlCon = new System.Data.SqlClient.SqlConnection();
            SqlCon.ConnectionString = strConnString;
            SqlCon.Open();
            SqlDataAdapter DataAdapterSql = new SqlDataAdapter();
            SqlCommand Commandsql = new SqlCommand();
            Commandsql.Connection = SqlCon;
            Commandsql.CommandType = CommandType.StoredProcedure;
            Commandsql.CommandText = procedimiento;
            DataAdapterSql.SelectCommand = Commandsql;
            for (int i = 0; i < parametro.Length; i++)
            {
                Commandsql.Parameters.Add(parametro[i]);
            }
            try
            {
                SqlParameter parameter = Commandsql.Parameters.Add("@Dato", SqlDbType.VarChar, 10);
                parameter.Direction = ParameterDirection.Output;
                Commandsql.ExecuteNonQuery();
                dato = Commandsql.Parameters["@Dato"].Value.ToString();
            }
            catch (Exception r)
            {
                dato = "0" + r.Message;
            }
            finally
            {
                DataAdapterSql.Dispose();
                SqlCon.Close();

            }
            return dato;
        }


        /// <summary>
        /// Consulta un registro en la Base de datos mediante un Procedimiento Almacenado
        /// </summary>
        /// <param name="procedimiento">Nombre del Procedimiento Almacenado que se ejecutara</param>
        /// <param name="parametros"> sqlParameter[] con los parametros que recibe el Procedimiento</param>
        /// <param name="conexion">Nombre de la conexion a la base de datos que utiliza</param>



        public DataSet consultaRegistro(string procedimiento, SqlParameter[] parametros, SqlConnection ConsConexion, string tabla)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter DataAdapter = new SqlDataAdapter();
            DataSet dsBasico = new DataSet();
            cmd.Connection = ConsConexion;
            cmd.CommandText = procedimiento;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            for (int i = 0; i < parametros.Length; i++)
            {
                cmd.Parameters.Add(parametros[i]);
            }

            DataAdapter = new SqlDataAdapter();
            try
            {
                DataAdapter = new SqlDataAdapter(cmd);
                DataAdapter.Fill(dsBasico, tabla);
                int h = dsBasico.Tables[tabla].Rows.Count;
                return dsBasico;
            }
            catch (Exception e)
            {
                string se = e.Message;
                return dsBasico;
            }
            finally
            {
                DataAdapter.Dispose();
                ConsConexion.Close();
                ConsConexion.Dispose();
            }
        }

        /// <summary>
        /// Llena una tabla SQL con los resultados de una consulta ODBC (Fox o relativity)
        /// </summary>
        /// <param name="keyOdbc">Clave que identifica la cadena de conexión ODBC</param>
        /// <param name="sentenciaOdbc">Sentencia odbc que realiza la consulta</param>
        /// <param name="keySql">Clave que identifica la cadena de conexión SQL</param>
        /// <param name="TablaDestino">Tabla que se llenará con la consulta</param>
        /// <returns>Resultado del proceso </returns>
        public string sqlInsert(string keyOdbc, string sentenciaOdbc, string keySql, string TablaDestino)
        {
            string strConnString = ConfigurationManager.ConnectionStrings[keyOdbc].ConnectionString;
            OdbcConnection OdbcCon = new System.Data.Odbc.OdbcConnection();
            OdbcCon.ConnectionString = strConnString;
            OdbcCon.Open();
            OdbcCommand commandSourceData = new OdbcCommand(sentenciaOdbc, OdbcCon);
            OdbcDataReader reader = commandSourceData.ExecuteReader();

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConectaSql(keySql)))
            {
                bulkCopy.DestinationTableName = TablaDestino;

                try
                {

                    bulkCopy.WriteToServer(reader);
                    return "OK";
                }
                catch (Exception ex)
                {
                    return "NO" + ex.Message;
                }
                finally
                {
                    reader.Close();
                }
            }


        }

        // Flag: Has Dispose already been called?
        bool disposed = false;

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                //
                this.Dispose();
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }

        ~WcfData()
        {
            Dispose(false);
        }


        /// <summary>
        /// Realiza una consulta de tipo ODBC mediante sentencia Sql estandar.
        /// </summary>
        /// <param name="NomTabla">Nombre de la tabla que almacena el resultado de la consulta.</param>
        /// <param name="sentenciaSql">cadena con la sentencia sql que realiza la consulta</param>
        /// <param name="key">Nombre que identifica el string de conexión</param>
        /// <returns>Retorna un dataset con los resultados de la consulta</returns>
        public DataSet ConsultaOdbc(string NomTabla, string sentenciaSql, string key)
        {
            DataSet dsBasico = new DataSet();
            string strConnString = ConfigurationManager.ConnectionStrings[key].ConnectionString;
            OdbcConnection OdbcCon = new System.Data.Odbc.OdbcConnection();
            OdbcCon.ConnectionString = strConnString;
            OdbcCon.Open();

            OdbcDataAdapter DataAdapterOdbc = new OdbcDataAdapter();
            try
            {
                DataAdapterOdbc = new OdbcDataAdapter(sentenciaSql, OdbcCon);
                DataAdapterOdbc.Fill(dsBasico, NomTabla);
                return dsBasico;

            }
            catch (Exception r)
            {
                string m = r.Message;
                return dsBasico;
            }
            finally
            {
                DataAdapterOdbc.Dispose();
                OdbcCon.Close();

            }
        }
       
    }


}
