using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using DebitoAutomatico.AD.Conexion;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Definicion;
using DebitoAutomatico.EN.Tablas;
using log4net;

namespace DebitoAutomatico.AD.Consultas
{
    public class TipoArchivoAD
    {
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public TipoArchivoAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        protected DataSet ejecutarConsulta(TipoArchivo objArchivo)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;
            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Tipo_Archivo", conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pOperacion", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@pOperacion"].Value = objArchivo.pOperacion;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pId", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objArchivo.pId))
                {
                    adaptador.SelectCommand.Parameters["@pId"].Value = objArchivo.pId;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pId"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pNombre", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objArchivo.pNombre))
                {
                    adaptador.SelectCommand.Parameters["@pNombre"].Value = objArchivo.pNombre;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pNombre"].Value = DBNull.Value;
                }
                datos = new DataSet();
                adaptador.Fill(datos, "tabla");
                adaptador.Dispose();
            }
            catch (SqlException ex)
            {
                Error = ex.Message;
                Registrador.Warn(ex.Message);
            }
            finally
            {
                if (conexion.State != ConnectionState.Closed)
                    conexion.Close();
            }
            return datos;
        }

        public List<TipoArchivo> consultar(TipoArchivo objArchivo)
        {
            
            DataSet datos = ejecutarConsulta(objArchivo);

            List<TipoArchivo> lista = new List<TipoArchivo>();
            TipoArchivo objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new TipoArchivo();
                objEntidad2.pId = Convertidor.aCadena(fila[TipoArchivoDEF.Id]);
                objEntidad2.pNombre = Convertidor.aCadena(fila[TipoArchivoDEF.Nombre]);
                lista.Add(objEntidad2);
            }

            return lista;
        }

        public int ejecutarNoConsulta(TipoArchivo objArchivo)
        {
            int cuenta = -1;
            DataSet datos = ejecutarConsulta(objArchivo);
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

        private DataTable consultar(String query)
        {
            Querys objQuery = new Querys();
            DataTable datos = objQuery.consultarDatos(query).Tables["tabla"];
            Error = objQuery.Error;
            return datos;
        }

        public DataTable consultarEntidades()
        {
            String query = "SELECT TA.ID, TA.NOMBRE FROM tb_DEB_TIPO_ARCHIVO as TA";
            return consultar(query);
        }
    }
}
