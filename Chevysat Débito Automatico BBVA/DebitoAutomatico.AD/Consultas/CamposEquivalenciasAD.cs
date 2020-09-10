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
    public class CamposEquivalenciasAD
    {
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public CamposEquivalenciasAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        protected DataSet ejecutarConsulta(CamposEquivalencias objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Campos_Equivalencias", conexion);
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pTablasEquivalencias", SqlDbType.VarChar));
                if (objEntidad.pTablasEquivalencias > 0)
                {
                    adaptador.SelectCommand.Parameters["@pTablasEquivalencias"].Value = objEntidad.pTablasEquivalencias;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pTablasEquivalencias"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCodigo", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pCodigo))
                {
                    adaptador.SelectCommand.Parameters["@pCodigo"].Value = objEntidad.pCodigo;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCodigo"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pDescripcion", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pDescripcion))
                {
                    adaptador.SelectCommand.Parameters["@pDescripcion"].Value = objEntidad.pDescripcion;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pDescripcion"].Value = DBNull.Value;
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pValorPorDefecto", SqlDbType.VarChar));
                if (objEntidad.pValorPorDefecto != null)
                {
                    if (objEntidad.pValorPorDefecto == true)
                        adaptador.SelectCommand.Parameters["@pValorPorDefecto"].Value = "1";
                    else
                        adaptador.SelectCommand.Parameters["@pValorPorDefecto"].Value = "0";
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pValorPorDefecto"].Value = DBNull.Value;
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

        public List<CamposEquivalencias> consultar(CamposEquivalencias objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DataSet datos = ejecutarConsulta(objEntidad);

            List<CamposEquivalencias> lista = new List<CamposEquivalencias>();
            CamposEquivalencias objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new CamposEquivalencias();
                objEntidad2.pId = Convertidor.aEntero32(fila[CamposEquivalenciasDEF.Id]);
                objEntidad2.pTablasEquivalencias = Convertidor.aEntero32(fila[CamposEquivalenciasDEF.TablasEquivalencias]);
                objEntidad2.pCodigo = Convertidor.aCadena(fila[CamposEquivalenciasDEF.Codigo]);
                objEntidad2.pDescripcion = Convertidor.aCadena(fila[CamposEquivalenciasDEF.Descripcion]);
                objEntidad2.pValor = Convertidor.aCadena(fila[CamposEquivalenciasDEF.Valor]);
                objEntidad2.pValorPorDefecto = Convertidor.aBooleano(fila[CamposEquivalenciasDEF.ValorPorDefecto]);
                lista.Add(objEntidad2);
            }
            return lista;
        }

        public int ejecutarNoConsulta(CamposEquivalencias objEntidad)
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

        private DataTable consultar(String query)
        {
            Querys objQuery = new Querys();
            DataTable datos = objQuery.consultarDatos(query).Tables["tabla"];
            Error = objQuery.Error;
            return datos;
        }

        public DataTable consultarCampoEquivalencias(String tipoArchivo,String codBanco)
        {
            String query = "SELECT * FROM tb_DEB_CAMPOS_EQUIVALENCIAS AS CE "
                + " INNER JOIN tb_DEB_TABLAS_EQUIVALENCIAS AS TE ON TE.ID=CE.TABLAS_EQUIVALENCIAS"
                + " WHERE (TE.TIPO_ARCHIVO = '" + tipoArchivo + "' AND TE.ID_BANCO = '" + codBanco + "')"
                + " ORDER BY CE.TABLAS_EQUIVALENCIAS";
            return consultar(query);
        }
    }
}
