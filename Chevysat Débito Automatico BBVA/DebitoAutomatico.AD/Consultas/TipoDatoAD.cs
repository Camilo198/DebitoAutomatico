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
    public class TipoDatoAD
    {
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public TipoDatoAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        protected DataSet ejecutarConsulta(TipoDato objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;
            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Tipo_Dato", conexion);
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
                Registrador.Warn(ex.Message);
            }
            finally
            {
                if (conexion.State != ConnectionState.Closed)
                    conexion.Close();
            }
            return datos;
        }

        public List<TipoDato> consultar(TipoDato objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DataSet datos = ejecutarConsulta(objEntidad);

            List<TipoDato> lista = new List<TipoDato>();
            TipoDato objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new TipoDato();
                objEntidad2.pId = Convertidor.aCadena(fila[TipoDatoDEF.Id]);
                objEntidad2.pNombre = Convertidor.aCadena(fila[TipoDatoDEF.Nombre]);

                lista.Add(objEntidad2);
            }

            return lista;
        }

        public int ejecutarNoConsulta(TipoDato objEntidad)
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
