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
    public class EstructuraArchivoAD
    {
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public EstructuraArchivoAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        protected DataSet ejecutarConsulta(EstructuraArchivo objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;
            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Estructura_Archivo", conexion);
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pConfiguracion", SqlDbType.VarChar));
                if (objEntidad.pConfiguracion > 0)
                {
                    adaptador.SelectCommand.Parameters["@pConfiguracion"].Value = objEntidad.pConfiguracion;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pConfiguracion"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pTipoDato", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pTipoDato))
                {
                    adaptador.SelectCommand.Parameters["@pTipoDato"].Value = objEntidad.pTipoDato;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pTipoDato"].Value = DBNull.Value;
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pColumna", SqlDbType.VarChar));
                if (objEntidad.pColumna >= 0)
                {
                    adaptador.SelectCommand.Parameters["@pColumna"].Value = objEntidad.pColumna;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pColumna"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pLongitud", SqlDbType.VarChar));
                if (objEntidad.pLongitud >= 0)
                {
                    adaptador.SelectCommand.Parameters["@pLongitud"].Value = objEntidad.pLongitud;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pLongitud"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCaracterRelleno", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pCaracterRelleno))
                {
                    adaptador.SelectCommand.Parameters["@pCaracterRelleno"].Value = objEntidad.pCaracterRelleno;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCaracterRelleno"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pAlineacion", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pAlineacion))
                {
                    adaptador.SelectCommand.Parameters["@pAlineacion"].Value = objEntidad.pAlineacion;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pAlineacion"].Value = DBNull.Value;
                }


                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCantidadDecimales", SqlDbType.VarChar));
                if (objEntidad.pCantidadDecimales >= 0)
                {
                    adaptador.SelectCommand.Parameters["@pCantidadDecimales"].Value = objEntidad.pCantidadDecimales;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCantidadDecimales"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pFormatoFecha", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pFormatoFecha))
                {
                    adaptador.SelectCommand.Parameters["@pFormatoFecha"].Value = objEntidad.pFormatoFecha;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pFormatoFecha"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pEsContador", SqlDbType.VarChar));
                if (objEntidad.pEsContador != null)
                {
                    if (objEntidad.pEsContador == true)
                        adaptador.SelectCommand.Parameters["@pEsContador"].Value = "1";
                    else
                        adaptador.SelectCommand.Parameters["@pEsContador"].Value = "0";
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pEsContador"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pSumaCampo", SqlDbType.VarChar));
                if (objEntidad.pSumaCampo >= 0)
                {
                    adaptador.SelectCommand.Parameters["@pSumaCampo"].Value = objEntidad.pSumaCampo;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pSumaCampo"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pRequiereCambio", SqlDbType.VarChar));
                if (objEntidad.pRequiereCambio != null)
                {
                    if (objEntidad.pRequiereCambio == true)
                        adaptador.SelectCommand.Parameters["@pRequiereCambio"].Value = "1";
                    else
                        adaptador.SelectCommand.Parameters["@pRequiereCambio"].Value = "0";
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pRequiereCambio"].Value = DBNull.Value;
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pValor", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pValor))
                {
                    adaptador.SelectCommand.Parameters["@pValor"].Value = objEntidad.pValor;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pValor"].Value = String.Empty;
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

        public List<EstructuraArchivo> consultar(EstructuraArchivo objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DataSet datos = ejecutarConsulta(objEntidad);

            List<EstructuraArchivo> lista = new List<EstructuraArchivo>();
            EstructuraArchivo objEntidad2 = null;

            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new EstructuraArchivo();

                objEntidad2.pId = Convertidor.aEntero32(fila[EstructuraArchivoDEF.Id]);
                objEntidad2.pConfiguracion = Convertidor.aEntero32(fila[EstructuraArchivoDEF.Configuracion]);
                objEntidad2.pTipoDato = Convertidor.aCadena(fila[EstructuraArchivoDEF.TipoDato]);
                objEntidad2.pNombre = Convertidor.aCadena(fila[EstructuraArchivoDEF.Nombre]);
                objEntidad2.pColumna = Convertidor.aEntero32(fila[EstructuraArchivoDEF.Columna]);
                objEntidad2.pLongitud = Convertidor.aEntero32(fila[EstructuraArchivoDEF.Longitud]);
                objEntidad2.pCaracterRelleno = Convertidor.aCadena(fila[EstructuraArchivoDEF.CaracterRelleno]);
                objEntidad2.pAlineacion = Convertidor.aCadena(fila[EstructuraArchivoDEF.Alineacion]);
                objEntidad2.pCantidadDecimales = Convertidor.aEntero32(fila[EstructuraArchivoDEF.CantidadDecimales]);
                objEntidad2.pFormatoFecha = Convertidor.aCadena(fila[EstructuraArchivoDEF.FormatoFecha]);
                objEntidad2.pEsContador = Convertidor.aBooleano(fila[EstructuraArchivoDEF.EsContador]);
                objEntidad2.pSumaCampo = Convertidor.aEntero32(fila[EstructuraArchivoDEF.SumaCampo]);
                objEntidad2.pRequiereCambio = Convertidor.aBooleano(fila[EstructuraArchivoDEF.RequiereCambio]);
                objEntidad2.pValorPorDefecto = Convertidor.aBooleano(fila[EstructuraArchivoDEF.ValorPorDefecto]);
                objEntidad2.pValor = Convertidor.aCadena(fila[EstructuraArchivoDEF.Valor]);
                lista.Add(objEntidad2);
            }
            return lista;
        }

        public int ejecutarNoConsulta(EstructuraArchivo objEntidad)
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

        public DataTable consultarEstructuraArchivo(String tipoLinea, String tipoProceso, Int32 IdBanco)
        {

            String query = "SELECT * "
               + " FROM tb_DEB_ESTRUCTURA_ARCHIVO AS EA"
               + " INNER JOIN tb_DEB_CONFIGURACION AS C ON EA.CONFIGURACION = C.ID"
               + " WHERE (C.TIPO_ARCHIVO = '" + tipoProceso + "')"
               + " AND (C.TIPO_LINEA = '" + tipoLinea + "')"
               + " AND (C.ID_BANCO = '" + IdBanco + "')"
               + " ORDER BY EA.COLUMNA";
            return consultar(query);
           
        }

        public DataTable consultarEstructuraArchivo(String tipoProceso, Int32 IdBanco)
        {

            String query = "SELECT * "
               + " FROM tb_DEB_ESTRUCTURA_ARCHIVO AS EA"
               + " INNER JOIN tb_DEB_CONFIGURACION AS C ON EA.CONFIGURACION = C.ID"
               + " WHERE (C.TIPO_ARCHIVO = '" + tipoProceso + "')"
               + " AND (C.ID_BANCO = '" + IdBanco + "')"
               + " ORDER BY EA.COLUMNA";
            return consultar(query);

        }

        public DataTable consultarSumaCampos(String tipoProceso, int IdBanco)
        {
            String query = "SELECT ea.ID, ea.SUMA_CAMPO, ea.NOMBRE"
                + " FROM tb_DEB_ESTRUCTURA_ARCHIVO AS ea"
                + " INNER JOIN tb_DEB_CONFIGURACION AS c ON c.ID = ea.CONFIGURACION"
                + " WHERE (c.TIPO_ARCHIVO = '" + tipoProceso + "' AND c.ID_BANCO = " + IdBanco + ")"
                + " ORDER BY ea.NOMBRE";
            return consultar(query);
        }

    }
}
