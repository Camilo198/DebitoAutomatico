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
    public class BancoAD
    {
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public BancoAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        protected DataSet ejecutarConsulta(Banco objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Banco", conexion);
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCodigo", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pCodigo))
                {
                    adaptador.SelectCommand.Parameters["@pCodigo"].Value = objEntidad.pCodigo;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCodigo"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pNombre", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pNombre))
                {
                    adaptador.SelectCommand.Parameters["@pNombre"].Value = objEntidad.pNombre;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pNombre"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pNit", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pNit))
                {
                    adaptador.SelectCommand.Parameters["@pNit"].Value = objEntidad.pNit;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pNit"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pActivo", SqlDbType.VarChar));
                if (objEntidad.pActivo != null)
                {
                    if (objEntidad.pActivo.Value)
                        adaptador.SelectCommand.Parameters["@pActivo"].Value = "1";
                    else
                        adaptador.SelectCommand.Parameters["@pActivo"].Value = "0";
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pActivo"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pDebito", SqlDbType.VarChar));
                if (objEntidad.pDebito != null)
                {
                    if (objEntidad.pDebito.Value)
                        adaptador.SelectCommand.Parameters["@pDebito"].Value = "1";
                    else
                        adaptador.SelectCommand.Parameters["@pDebito"].Value = "0";
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pDebito"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCorreoControl", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pCorreoControl))
                {
                    adaptador.SelectCommand.Parameters["@pCorreoControl"].Value = objEntidad.pCorreoControl;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCorreoControl"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCorreoEnvio", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pCorreoEnvio))
                {
                    adaptador.SelectCommand.Parameters["@pCorreoEnvio"].Value = objEntidad.pCorreoEnvio;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCorreoEnvio"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pRemitente", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pRemitente))
                {
                    adaptador.SelectCommand.Parameters["@pRemitente"].Value = objEntidad.pRemitente;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pRemitente"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdRuta", SqlDbType.VarChar));
                if (objEntidad.pIdRuta > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdRuta"].Value = objEntidad.pIdRuta;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdRuta"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdPrenota", SqlDbType.VarChar));
                if (objEntidad.pIdPrenota > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdPrenota"].Value = objEntidad.pIdPrenota;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdPrenota"].Value = String.Empty;
                }


                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdPrenotaManual", SqlDbType.VarChar));
                if (objEntidad.pIdPrenotaManual > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdPrenotaManual"].Value = objEntidad.pIdPrenotaManual;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdPrenotaManual"].Value = String.Empty;
                }


                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdDebito", SqlDbType.VarChar));
                if (objEntidad.pIdDebito > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdDebito"].Value = objEntidad.pIdDebito;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdDebito"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdDebitoManual", SqlDbType.VarChar));
                if (objEntidad.pIdDebitoManual > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdDebitoManual"].Value = objEntidad.pIdDebitoManual;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdDebitoManual"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdPagos", SqlDbType.VarChar));
                if (objEntidad.pIdPagos > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdPagos"].Value = objEntidad.pIdPagos;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdPagos"].Value = String.Empty;
                }


                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdErrores", SqlDbType.VarChar));
                if (objEntidad.pIdErrores > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdErrores"].Value = objEntidad.pIdErrores;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdErrores"].Value = String.Empty;
                }


                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdRecibidos", SqlDbType.VarChar));
                if (objEntidad.pIdRecibidos > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdRecibidos"].Value = objEntidad.pIdRecibidos;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdRecibidos"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdHistorico", SqlDbType.VarChar));
                if (objEntidad.pIdHistorico > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdHistorico"].Value = objEntidad.pIdHistorico;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdHistorico"].Value = String.Empty;
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

        public List<Banco> consultar(Banco objEntidad)
        {
            DataSet datos = ejecutarConsulta(objEntidad);

            List<Banco> lista = new List<Banco>();
            Banco objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new Banco();
                objEntidad2.pId = Convertidor.aEntero32(fila[BancoDEF.Id]);
                objEntidad2.pCodigo = Convertidor.aCadena(fila[BancoDEF.Codigo]);
                objEntidad2.pNombre = Convertidor.aCadena(fila[BancoDEF.Nombre]);
                objEntidad2.pNit = Convertidor.aCadena(fila[BancoDEF.Nit]);
                objEntidad2.pActivo = Convertidor.aBooleano(fila[BancoDEF.Activo]);
                objEntidad2.pDebito = Convertidor.aBooleano(fila[BancoDEF.Debito]);
                objEntidad2.pCorreoControl = Convertidor.aCadena(fila[BancoDEF.CorreoControl]);
                objEntidad2.pCorreoEnvio = Convertidor.aCadena(fila[BancoDEF.CorreoEnvio]);
                objEntidad2.pRemitente = Convertidor.aCadena(fila[BancoDEF.Remitente]);
                objEntidad2.pIdRuta = Convertidor.aEntero32(fila[BancoDEF.IdRuta]);
                objEntidad2.pIdPrenota = Convertidor.aEntero32(fila[BancoDEF.IdPrenota]);
                objEntidad2.pIdPrenotaManual = Convertidor.aEntero32(fila[BancoDEF.IdPrenotaManual]);
                objEntidad2.pIdDebito = Convertidor.aEntero32(fila[BancoDEF.IdDebito]);
                objEntidad2.pIdDebitoManual = Convertidor.aEntero32(fila[BancoDEF.IdDebitoManual]);
                objEntidad2.pIdPagos = Convertidor.aEntero32(fila[BancoDEF.IdPagos]);
                objEntidad2.pIdErrores = Convertidor.aEntero32(fila[BancoDEF.IdErrores]);
                objEntidad2.pIdRecibidos = Convertidor.aEntero32(fila[BancoDEF.IdRecibidos]);
                objEntidad2.pIdHistorico = Convertidor.aEntero32(fila[BancoDEF.IdHistorico]);
                
                lista.Add(objEntidad2);
            }

            return lista;
        }

        public List<Banco> consultaridbanco(Banco objEntidad)
        {
            string nombre = objEntidad.pNombre;
            objEntidad.pId = consultaidbanco(nombre);
            DataSet datos = ejecutarConsulta(objEntidad);

            List<Banco> lista = new List<Banco>();
            Banco objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new Banco();
                objEntidad2.pId = Convertidor.aEntero32(fila[BancoDEF.Id]);
                objEntidad2.pCodigo = Convertidor.aCadena(fila[BancoDEF.Codigo]);
                objEntidad2.pNombre = Convertidor.aCadena(fila[BancoDEF.Nombre]);
                objEntidad2.pNit = Convertidor.aCadena(fila[BancoDEF.Nit]);
                objEntidad2.pActivo = Convertidor.aBooleano(fila[BancoDEF.Activo]);
                objEntidad2.pDebito = Convertidor.aBooleano(fila[BancoDEF.Debito]);
                objEntidad2.pCorreoControl = Convertidor.aCadena(fila[BancoDEF.CorreoControl]);
                objEntidad2.pCorreoEnvio = Convertidor.aCadena(fila[BancoDEF.CorreoEnvio]);
                objEntidad2.pRemitente = Convertidor.aCadena(fila[BancoDEF.Remitente]);
                objEntidad2.pIdRuta = Convertidor.aEntero32(fila[BancoDEF.IdRuta]);
                objEntidad2.pIdPrenota = Convertidor.aEntero32(fila[BancoDEF.IdPrenota]);
                objEntidad2.pIdPrenotaManual = Convertidor.aEntero32(fila[BancoDEF.IdPrenotaManual]);
                objEntidad2.pIdDebito = Convertidor.aEntero32(fila[BancoDEF.IdDebito]);
                objEntidad2.pIdDebitoManual = Convertidor.aEntero32(fila[BancoDEF.IdDebitoManual]);
                objEntidad2.pIdPagos = Convertidor.aEntero32(fila[BancoDEF.IdPagos]);
                objEntidad2.pIdErrores = Convertidor.aEntero32(fila[BancoDEF.IdErrores]);
                objEntidad2.pIdRecibidos = Convertidor.aEntero32(fila[BancoDEF.IdRecibidos]);
                objEntidad2.pIdHistorico = Convertidor.aEntero32(fila[BancoDEF.IdHistorico]);

                lista.Add(objEntidad2);
            }

            return lista;
        }

        public int ejecutarNoConsulta(Banco objEntidad)
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

        public int  consultaidbanco(string nombre)
        {
            int ID = 0;
            String query = "SELECT [ID]  FROM [dbo].[tb_DEB_BANCO] where NOMBRE ='" + nombre+"'";
            DataTable consuldaid = consultar(query);
            ID = Convert.ToInt32(consuldaid.Rows[0][0].ToString());
            return ID;
        }

        public DataSet consultarLugarPago(Banco objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_DEB_Consulta_LugarPago", conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pId", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@pId"].Value = objEntidad.pId;

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

        public DataTable consultar()
        {
            String query = "SELECT B.ID, B.CODIGO, B.NOMBRE, B.NOMBRE, B.NIT, B.ACTIVO, B.CORREOS_CONTROL,"
                + "  B.CORREOS_ENVIO, B.REMITENTE,B.ID_RUTA,B.ID_FIDUCIAS"
                + " FROM tb_DEB_BANCO AS B";
            //+ " INNER JOIN tb_DEB_RUTA AS RE ON B.Ruta_Archivos_Entrada = RE.OID"
            //+ " INNER JOIN tb_BAN_RUTA AS RS ON B.Ruta_Archivos_Salida_Epicor = RS.OID";

            return consultar(query);
        }

        public DataTable consultarBancoFiduciaAct()
        {
            String query = "SELECT (D.[NOMBRE] + case when F.FIDUCIA = 1 then ' FID1' ELSE ' FID2' END) as NOMBRE, D.[ID] "
                            + "FROM [tb_DEB_BANCO] D "
                            + "INNER JOIN dbo.tb_DEB_FIDUCIAS F on D.ID_FIDUCIAS = F.ID "
                            + "WHERE D.DEBITO = 1";
            return consultar(query);
        }

    }
}
