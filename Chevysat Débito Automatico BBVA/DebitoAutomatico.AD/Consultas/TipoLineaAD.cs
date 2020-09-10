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
    public class TipoLineaAD
    {
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public TipoLineaAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        protected DataSet ejecutarConsulta(TipoLinea objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Tipo_Linea", conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pOperacion", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@pOperacion"].Value = objEntidad.pOperacion;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pId", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pId))
                {
                    adaptador.SelectCommand.Parameters["@pId"].Value = objEntidad.pId;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pId"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pNombre", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pNombre))
                {
                    adaptador.SelectCommand.Parameters["@pNombre"].Value = objEntidad.pNombre;
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
                Registrador.Error(Error);
            }
            finally
            {
                if (conexion.State != ConnectionState.Closed)
                    conexion.Close();
            }

            return datos;
        }

        public List<TipoLinea> consultar(TipoLinea objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DataSet datos = ejecutarConsulta(objEntidad);
            List<TipoLinea> lista = new List<TipoLinea>();
            TipoLinea objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new TipoLinea();
                objEntidad2.pNombre = Convertidor.aCadena(fila[TipoLineaDEF.Nombre]);
                objEntidad2.pNombre = Convertidor.aCadena(fila[TipoLineaDEF.Id]);
                lista.Add(objEntidad2);
            }
            return lista;
        }

        public int ejecutarNoConsulta(TipoLinea objEntidad)
        {
            int cuenta = -1;
            DataSet datos = ejecutarConsulta(objEntidad);
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

        public DataTable consultarLineas(String TipoArchivo, int IdBanco)
        {
            String query = "SELECT c.ID, tl.NOMBRE, c.TIPO_ARCHIVO, c.TIPO_LINEA"
                + " FROM tb_DEB_TIPO_ARCHIVO ta, tb_DEB_CONFIGURACION c, tb_DEB_TIPO_LINEA tl"
                + " WHERE ta.ID = c.TIPO_ARCHIVO"
                + " AND c.TIPO_LINEA = tl.ID"
                + " AND c.ID_BANCO = " + IdBanco
                + " AND ta.ID = '" + TipoArchivo + "'"
                + " ORDER BY TIPO_LINEA";

            return consultar(query);
        }

        public DataTable consultarLineasDisponibles(String TipoArchivo , int IdBanco)
        {
            String query = "SELECT ID, NOMBRE"
                + " FROM tb_DEB_TIPO_LINEA"
            + " WHERE ID NOT IN (SELECT c.TIPO_LINEA"
            + " FROM tb_DEB_TIPO_ARCHIVO ta, tb_DEB_CONFIGURACION c, tb_DEB_TIPO_LINEA tl"
            + " WHERE ta.ID = c.TIPO_ARCHIVO"
            + " AND c.TIPO_LINEA = tl.ID"
            + " AND c.ID_BANCO = " + IdBanco
            + " AND ta.ID = '" + TipoArchivo + "')"
            + " ORDER BY ID";

            return consultar(query);
        }

    }
}
