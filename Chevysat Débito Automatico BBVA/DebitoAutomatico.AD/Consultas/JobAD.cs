using DebitoAutomatico.AD.Conexion;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.AD.Consultas
{
    public class JobAD
    {
        public String Error { get; set; }
        public ILog Registrador { get; set; }

        public DataSet EjecucionJob()
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_EjecucionJob", conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

                datos = new DataSet();
                adaptador.Fill(datos, "Job");
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

        public DataSet EjecucionJobConsulta()
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_EjecucionJobConsulta", conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

                datos = new DataSet();
                adaptador.Fill(datos, "EjecucionJob");
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


        public DataSet EjecucionJobConsultaTiempo()
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_EjecucionJobConsultaTiempo", conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

                datos = new DataSet();
                adaptador.Fill(datos, "ConsultaJob");
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
    }
}
