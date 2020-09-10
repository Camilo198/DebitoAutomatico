using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using DebitoAutomatico.AD;
using DebitoAutomatico.AD.Conexion;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Definicion;
using DebitoAutomatico.EN.Tablas;
using log4net;

namespace DebitoAutomatico.AD.Consultas
{
    public class ConfiguracionAD
    {
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public ConfiguracionAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        protected DataSet ejecutarConsulta(ConfiguracionEst objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Configuracion", conexion);
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pTipoLinea", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pTipoLinea))
                {
                    adaptador.SelectCommand.Parameters["@pTipoLinea"].Value = objEntidad.pTipoLinea;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pTipoLinea"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pTipoArchivo", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pTipoArchivo))
                {
                    adaptador.SelectCommand.Parameters["@pTipoArchivo"].Value = objEntidad.pTipoArchivo;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pTipoArchivo"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdBanco", SqlDbType.VarChar));
                if (objEntidad.pIdBanco > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdBanco"].Value = objEntidad.pIdBanco;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdBanco"].Value = "0";
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

        public List<ConfiguracionEst> consultar(ConfiguracionEst objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DataSet datos = ejecutarConsulta(objEntidad);

            List<ConfiguracionEst> lista = new List<ConfiguracionEst>();
            ConfiguracionEst objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new ConfiguracionEst();
                objEntidad2.pId = Convertidor.aEntero32(fila[ConfiguracionDEF.Id]);
                objEntidad2.pTipoLinea = Convertidor.aCadena(fila[ConfiguracionDEF.TipoLinea]);
                objEntidad2.pTipoArchivo = Convertidor.aCadena(fila[ConfiguracionDEF.TipoArchivo]);
                objEntidad2.pIdBanco = Convertidor.aEntero32(fila[ConfiguracionDEF.IdBanco]);

                lista.Add(objEntidad2);
            }

            return lista;
        }

        public int ejecutarNoConsulta(ConfiguracionEst objEntidad)
        {
            int cuenta = -1;
            DataSet datos = ejecutarConsulta(objEntidad);
            try
            {
                cuenta = Convertidor.aEntero32(datos.Tables["tabla"].Rows[0]["Cuenta"]);
            }
            catch (Exception ex)
            {
                Registrador.Error(ex.Message);
            }
            return cuenta;
        }

    }
}
