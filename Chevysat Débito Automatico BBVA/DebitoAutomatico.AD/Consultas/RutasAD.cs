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
    public class RutasAD
    {
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public RutasAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        protected DataSet ejecutarConsulta(Rutas objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Rutas", conexion);
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pRuta", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pRuta))
                {
                    adaptador.SelectCommand.Parameters["@pRuta"].Value = objEntidad.pRuta;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pRuta"].Value = String.Empty;
                }

                //adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pRutaEntrada", SqlDbType.VarChar));
                //if (!String.IsNullOrEmpty(objEntidad.pRutaEntrada))
                //{
                //    adaptador.SelectCommand.Parameters["@pRutaEntrada"].Value = objEntidad.pRutaEntrada;
                //}
                //else
                //{
                //    adaptador.SelectCommand.Parameters["@pRutaEntrada"].Value = String.Empty;
                //}

                //adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pRutaSalida", SqlDbType.VarChar));
                //if (!String.IsNullOrEmpty(objEntidad.pRutaSalida))
                //{
                //    adaptador.SelectCommand.Parameters["@pRutaSalida"].Value = objEntidad.pRutaSalida;
                //}
                //else
                //{
                //    adaptador.SelectCommand.Parameters["@pRutaSalida"].Value = String.Empty;
                //}

                //adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pRutaSico", SqlDbType.VarChar));
                //if (!String.IsNullOrEmpty(objEntidad.pRutaSico))
                //{
                //    adaptador.SelectCommand.Parameters["@pRutaSico"].Value = objEntidad.pRutaSico;
                //}
                //else
                //{
                //    adaptador.SelectCommand.Parameters["@pRutaSico"].Value = String.Empty;
                //}

                //adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pUsaFtp", SqlDbType.VarChar));
                //if (!String.IsNullOrEmpty(objEntidad.pUsaFtp))
                //{
                //    adaptador.SelectCommand.Parameters["@pUsaFtp"].Value = objEntidad.pUsaFtp;
                //}
                //else
                //{
                //    adaptador.SelectCommand.Parameters["@pUsaFtp"].Value = String.Empty;
                //}

                //adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pPassFtp", SqlDbType.VarChar));
                //if (!String.IsNullOrEmpty(objEntidad.pPassFtp))
                //{
                //    adaptador.SelectCommand.Parameters["@pPassFtp"].Value = objEntidad.pPassFtp;
                //}
                //else
                //{
                //    adaptador.SelectCommand.Parameters["@pPassFtp"].Value = String.Empty;
                //}

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

        public List<Rutas> consultar(Rutas objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DataSet datos = ejecutarConsulta(objEntidad);

            List<Rutas> lista = new List<Rutas>();
            Rutas objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new Rutas();
                objEntidad2.pId = Convertidor.aEntero32(fila[RutasDEF.Id]);
                objEntidad2.pRuta = Convertidor.aCadena(fila[RutasDEF.Ruta]);
                //objEntidad2.pRutaEntrada = Convertidor.aCadena(fila[RutasDEF.RutaEntrada]);
                //objEntidad2.pRutaSalida = Convertidor.aCadena(fila[RutasDEF.RutaSalida]);
                //objEntidad2.pRutaSico = Convertidor.aCadena(fila[RutasDEF.RutaSico]);
                //objEntidad2.pUsaFtp = Convertidor.aCadena(fila[RutasDEF.UsaFtp]);
                //objEntidad2.pPassFtp = Convertidor.aCadena(fila[RutasDEF.PassFtp]);
                lista.Add(objEntidad2);
            }

            return lista;
        }

        public int ejecutarNoConsulta(Rutas objEntidad)
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
