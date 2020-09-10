using log4net;
using ServicioDebito.AD.Conexion;
using ServicioDebito.EN;
using ServicioDebito.EN.Definicion;
using ServicioDebito.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioDebito.AD.Consultas
{
    public class ActualizaClienteAD
    {
         public String Error { get; set; }

        public ILog Registrador { get; set; }

        public ActualizaClienteAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        public DataSet ejecutarConsulta(ActualizaCliente objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexionSQL();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Actualiza_Clientes", conexion);
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pContrato", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pContrato))
                {
                    adaptador.SelectCommand.Parameters["@pContrato"].Value = objEntidad.pContrato;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pContrato"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdTitularCuenta", SqlDbType.VarChar));
                if (objEntidad.pIdTitularCuenta > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdTitularCuenta"].Value = objEntidad.pIdTitularCuenta;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdTitularCuenta"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdBanco", SqlDbType.VarChar));
                if (objEntidad.pIdBanco > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdBanco"].Value = objEntidad.pIdBanco;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdBanco"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdTipoCuenta", SqlDbType.VarChar));
                if (objEntidad.pIdTipoCuenta > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdTipoCuenta"].Value = objEntidad.pIdTipoCuenta;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdTipoCuenta"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pNumeroCuenta", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pNumeroCuenta))
                {
                    adaptador.SelectCommand.Parameters["@pNumeroCuenta"].Value = objEntidad.pNumeroCuenta;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pNumeroCuenta"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pDireccionIp", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pDireccionIp))
                {
                    adaptador.SelectCommand.Parameters["@pDireccionIp"].Value = objEntidad.pDireccionIp;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pDireccionIp"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pUsuario", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pUsuario))
                {
                    adaptador.SelectCommand.Parameters["@pUsuario"].Value = objEntidad.pUsuario;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pUsuario"].Value = String.Empty;
                }

                datos = new DataSet();
                adaptador.Fill(datos, "ActualizaCliente");
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

        public List<ActualizaCliente> consultar(ActualizaCliente objEntidad)
        {
            DataSet datos = ejecutarConsulta(objEntidad);

            List<ActualizaCliente> lista = new List<ActualizaCliente>();
            ActualizaCliente objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["ActualizaCliente"].Rows)
            {
                objEntidad2 = new ActualizaCliente();
                objEntidad2.pId = Convertidor.aEntero32(fila[ActualizaClienteDEF.Id]);
                objEntidad2.pContrato = Convertidor.aCadena(fila[ActualizaClienteDEF.Contrato]);
                objEntidad2.pIdTitularCuenta = Convertidor.aEntero32(fila[ActualizaClienteDEF.TitularCuenta]);
                objEntidad2.pIdBanco = Convertidor.aEntero32(fila[ActualizaClienteDEF.IdBanco]);
                objEntidad2.pIdTipoCuenta = Convertidor.aEntero32(fila[ActualizaClienteDEF.TipoCuenta]);
                objEntidad2.pNumeroCuenta = Convertidor.aCadena(fila[ActualizaClienteDEF.NumeroCuenta]);
                objEntidad2.pDireccionIp = Convertidor.aCadena(fila[ActualizaClienteDEF.DireccionIp]);
                objEntidad2.pUsuario = Convertidor.aCadena(fila[ActualizaClienteDEF.Usuario]);
                lista.Add(objEntidad2);
            }

            return lista;
        }

        public int ejecutarNoConsulta(ActualizaCliente objEntidad)
        {
            int cuenta = -1;
            DataSet datos = ejecutarConsulta(objEntidad);
            try
            {
                cuenta = Convertidor.aEntero32(datos.Tables["ActualizaCliente"].Rows[0]["Cuenta"]);
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
            DataTable datos = objQuery.consultarDatos(query).Tables["ActualizaCliente"];
            Error = objQuery.Error;
            return datos;
        }
    }
}
