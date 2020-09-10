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
    public class TipoCuentaAD
    {
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public TipoCuentaAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        protected DataSet ejecutarConsulta(TipoCuenta objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Tipo_Cuenta", conexion);
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pValor", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pValor))
                {
                    adaptador.SelectCommand.Parameters["@pValor"].Value = objEntidad.pValor;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pValor"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pValorNuevo", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pValorNuevo))
                {
                    adaptador.SelectCommand.Parameters["@pValorNuevo"].Value = objEntidad.pValorNuevo;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pValorNuevo"].Value = String.Empty;
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

        public List<TipoCuenta> consultar(TipoCuenta objEntidad)
        {
            DataSet datos = ejecutarConsulta(objEntidad);

            List<TipoCuenta> lista = new List<TipoCuenta>();
            TipoCuenta objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new TipoCuenta();
                objEntidad2.pId = Convertidor.aEntero32(fila[TipoCuentaDEF.Id]);
                objEntidad2.pValor = Convertidor.aCadena(fila[TipoCuentaDEF.Valor]);
                lista.Add(objEntidad2);
            }

            return lista;
        }

        public int ejecutarNoConsulta(TipoCuenta objEntidad)
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
