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
    public class TablasEquivalenciasAD
    {
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public TablasEquivalenciasAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        protected DataSet ejecutarConsulta(TablasEquivalencias objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;
            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Tablas_Equivalencias", conexion);
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pNombre", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pNombre))
                {
                    adaptador.SelectCommand.Parameters["@pNombre"].Value = objEntidad.pNombre;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pNombre"].Value = DBNull.Value;
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
                if (objEntidad.pIdBanco >= 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdBanco"].Value = objEntidad.pIdBanco;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdBanco"].Value = DBNull.Value;
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

        public List<TablasEquivalencias> consultar(TablasEquivalencias objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DataSet datos = ejecutarConsulta(objEntidad);

            List<TablasEquivalencias> lista = new List<TablasEquivalencias>();
            TablasEquivalencias objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new TablasEquivalencias();
                objEntidad2.pId = Convertidor.aEntero32(fila[TablasEquivalenciasDEF.Id]);
                objEntidad2.pNombre = Convertidor.aCadena(fila[TablasEquivalenciasDEF.Nombre]);
                objEntidad2.pTipoArchivo = Convertidor.aCadena(fila[TablasEquivalenciasDEF.TipoArchivo]);
                objEntidad2.pIdBanco = Convertidor.aEntero32(fila[TablasEquivalenciasDEF.IdBanco]);
                lista.Add(objEntidad2);
            }

            return lista;
        }

        public int ejecutarNoConsulta(TablasEquivalencias objEntidad)
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
    }
}
