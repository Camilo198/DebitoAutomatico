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
    public class FiduciasAD
    {
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public FiduciasAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        protected DataSet ejecutarConsulta(Fiducias objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Fiducias", conexion);
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pFiducia", SqlDbType.VarChar));
                if (objEntidad.pFiducia > 0)
                {
                    adaptador.SelectCommand.Parameters["@pFiducia"].Value = objEntidad.pFiducia;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pFiducia"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCuentaFiducia1", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pCuentaFiducia1))
                {
                    adaptador.SelectCommand.Parameters["@pCuentaFiducia1"].Value = objEntidad.pCuentaFiducia1;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCuentaFiducia1"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCuentaFiducia2", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pCuentaFiducia2))
                {
                    adaptador.SelectCommand.Parameters["@pCuentaFiducia2"].Value = objEntidad.pCuentaFiducia2;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCuentaFiducia2"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdTipoCuenta1", SqlDbType.VarChar));
                if (objEntidad.pIdTipoCuenta1 > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdTipoCuenta1"].Value = objEntidad.pIdTipoCuenta1;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdTipoCuenta1"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdTipoCuenta2", SqlDbType.VarChar));
                if (objEntidad.pIdTipoCuenta2 > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdTipoCuenta2"].Value = objEntidad.pIdTipoCuenta2;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdTipoCuenta2"].Value = String.Empty;
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

        public List<Fiducias> consultar(Fiducias objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DataSet datos = ejecutarConsulta(objEntidad);

            List<Fiducias> lista = new List<Fiducias>();
            Fiducias objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new Fiducias();
                objEntidad2.pId = Convertidor.aEntero32(fila[FiduciasDEF.Id]);
                objEntidad2.pFiducia = Convertidor.aEntero32(fila[FiduciasDEF.Fiducia]);
                objEntidad2.pCuentaFiducia1  = Convertidor.aCadena(fila[FiduciasDEF.CuentaFiducia1]);
                objEntidad2.pCuentaFiducia2  = Convertidor.aCadena(fila[FiduciasDEF.CuentaFiducia2]);
                objEntidad2.pIdTipoCuenta1 = Convertidor.aEntero32(fila[FiduciasDEF.IdTipoCuenta1]);
                objEntidad2.pIdTipoCuenta2 = Convertidor.aEntero32(fila[FiduciasDEF.IdTipoCuenta2]);
                lista.Add(objEntidad2);
            }

            return lista;
        }

        public int ejecutarNoConsulta(Fiducias objEntidad)
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
