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

    public class PagoParcialAD
    {
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public PagoParcialAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        protected DataSet ejecutarConsulta(PagoParcial objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Pago_Parcial", conexion);
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pValorSico", SqlDbType.VarChar));
                if (objEntidad.pValorSico > 0)
                {
                    adaptador.SelectCommand.Parameters["@pValorSico"].Value = objEntidad.pValorSico;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pValorSico"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pValorCobrado", SqlDbType.VarChar));
                if (objEntidad.pValorCobrado > 0)
                {
                    adaptador.SelectCommand.Parameters["@pValorCobrado"].Value = objEntidad.pValorCobrado;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pValorCobrado"].Value = String.Empty;
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

        public List<PagoParcial> consultar(PagoParcial objEntidad)
        {
            DataSet datos = ejecutarConsulta(objEntidad);

            List<PagoParcial> lista = new List<PagoParcial>();
            PagoParcial objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new PagoParcial();
                objEntidad2.pId = Convertidor.aEntero32(fila[PagoParcialDEF.Id]);
                objEntidad2.pContrato = Convertidor.aCadena(fila[PagoParcialDEF.Contrato]);
                objEntidad2.pValorSico = Convertidor.aEntero32(fila[PagoParcialDEF.ValorSico]);
                objEntidad2.pValorCobrado = Convertidor.aEntero32(fila[PagoParcialDEF.ValorCobrado]);
                lista.Add(objEntidad2);
            }

            return lista;
        }

        public int ejecutarNoConsulta(PagoParcial objEntidad)
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
