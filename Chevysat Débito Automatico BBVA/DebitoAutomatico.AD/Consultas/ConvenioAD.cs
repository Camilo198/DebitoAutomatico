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
    public class ConvenioAD
    {
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public ConvenioAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        protected DataSet ejecutarConsulta(Convenio objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Convenio", conexion);
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdBancoDebito", SqlDbType.VarChar));
                if (objEntidad.pIdBancoDebito > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdBancoDebito"].Value = objEntidad.pIdBancoDebito;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdBancoDebito"].Value = DBNull.Value;
                }               

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdBanco", SqlDbType.VarChar));
                if (objEntidad.pIdBanco > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdBanco"].Value = objEntidad.pIdBanco;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdBanco"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdProceso", SqlDbType.VarChar));
                if (objEntidad.pIdProceso > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdProceso"].Value = objEntidad.pIdProceso;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdProceso"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdPrenota", SqlDbType.VarChar));
                if (objEntidad.pIdPrenota != null)
                {
                    adaptador.SelectCommand.Parameters["@pIdPrenota"].Value = objEntidad.pIdPrenota;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdPrenota"].Value = DBNull.Value;
                }


                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdDebito", SqlDbType.VarChar));
                if (objEntidad.pIdDebito != null)
                {
                    adaptador.SelectCommand.Parameters["@pIdDebito"].Value = objEntidad.pIdDebito;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdDebito"].Value = DBNull.Value;
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

        public List<Convenio> consultar(Convenio objEntidad)
        {

            DataSet datos = ejecutarConsulta(objEntidad);

            List<Convenio> lista = new List<Convenio>();
            Convenio objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new Convenio();
                objEntidad2.pId = Convertidor.aEntero32(fila[ConvenioDEF.Id]);
                objEntidad2.pIdBancoDebito = Convertidor.aEntero32(fila[ConvenioDEF.IdBancoDebito]);
                objEntidad2.pIdBanco = Convertidor.aEntero32(fila[ConvenioDEF.IdBanco]);
                objEntidad2.pIdPrenota = Convertidor.aCadena(fila[ConvenioDEF.IdPrenota]);
                objEntidad2.pIdDebito = Convertidor.aCadena(fila[ConvenioDEF.IdDebito]);

                lista.Add(objEntidad2);
            }

            return lista;
        }

        public int ejecutarNoConsulta(Convenio objEntidad)
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
