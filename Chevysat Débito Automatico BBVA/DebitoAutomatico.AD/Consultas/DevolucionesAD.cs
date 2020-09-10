using DebitoAutomatico.AD.Conexion;
using DebitoAutomatico.EN.Definicion;
using DebitoAutomatico.EN.Tablas;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.AD.Consultas
{
    public class DevolucionesAD
    {
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public DevolucionesAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }
        protected DataSet ejecutarConsulta(Devoluciones objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Devoluciones", conexion);
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
                    adaptador.SelectCommand.Parameters["@pId"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pHistCliente", SqlDbType.VarChar));
                if (objEntidad.pHistCliente > 0)
                {
                    adaptador.SelectCommand.Parameters["@pHistCliente"].Value = objEntidad.pHistCliente;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pHistCliente"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdTipoCausal", SqlDbType.VarChar));
                if (objEntidad.pIdTipoCausal > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdTipoCausal"].Value = objEntidad.pIdTipoCausal;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdTipoCausal"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pFechaGiro", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pFechaGiro))
                {
                    adaptador.SelectCommand.Parameters["@pFechaGiro"].Value = objEntidad.pFechaGiro;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pFechaGiro"].Value = String.Empty;
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

        public List<Devoluciones> consultar(Devoluciones objEntidad)
        {
            DataSet datos = ejecutarConsulta(objEntidad);

            List<Devoluciones> lista = new List<Devoluciones>();
            Devoluciones objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new Devoluciones();
                objEntidad2.pId = Convertidor.aEntero32(fila[DevolucionesDEF.Id]);
                objEntidad2.pHistCliente = Convertidor.aEntero32(fila[DevolucionesDEF.IdHistCliente]);
                objEntidad2.pIdTipoCausal = Convertidor.aEntero32(fila[DevolucionesDEF.IdTipoCausal]);
                objEntidad2.pFechaGiro = Convertidor.aCadena(fila[DevolucionesDEF.FechaGiro]);
                lista.Add(objEntidad2);
            }

            return lista;
        }

        public int ejecutarNoConsulta(Devoluciones objEntidad)
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
    }
}
