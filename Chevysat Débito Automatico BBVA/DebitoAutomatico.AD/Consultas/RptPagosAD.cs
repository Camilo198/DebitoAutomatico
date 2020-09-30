using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DebitoAutomatico.EN.Tablas;
using DebitoAutomatico.AD.Servicios;
using System.Data;
using System.Data.SqlClient;

namespace DebitoAutomatico.AD.Consultas
{
    public class RptPagosAD
    {
        WcfData wsc = new WcfData();

        //Retorna en una lista los parametros de la fecha de aplicacion del banco.
        public IList<RptPagosEN> ConsultarPagoDebitoAD(string Procedimiento, RptPagosEN objEntidad)
        {
            List<RptPagosEN> listParametro = new List<RptPagosEN>();
            List<string[,]> lista = new List<string[,]>();

            try
            {
                string[,,] Param = new string[9, 3, 1]; // solo cuando el procedimiento almacenado tiene parametros


                Param[0, 0, 0] = objEntidad.codigoBanco.ToString();
                Param[0, 1, 0] = "@inCodBanco";
                Param[0, 2, 0] = "int";

                Param[1, 0, 0] = objEntidad.fechaPago.ToString();
                Param[1, 1, 0] = "@inFechPago";
                Param[1, 2, 0] = "date";

                Param[2, 0, 0] = objEntidad.fechaProceso.ToString();
                Param[2, 1, 0] = "@inFecProceso";
                Param[2, 2, 0] = "datetime";

                Param[3, 0, 0] = objEntidad.cantPagosArchivo.ToString();
                Param[3, 1, 0] = "@inCantPagosArch";
                Param[3, 2, 0] = "int";

                Param[4, 0, 0] = objEntidad.cantPagosReacudo.ToString();
                Param[4, 1, 0] = "@inCantPagosRec";
                Param[4, 2, 0] = "int";

                Param[5, 0, 0] = objEntidad.valorMontoArchivo.ToString();
                Param[5, 1, 0] = "@inMontoarchivo";
                Param[5, 2, 0] = "numeric(18,0)";

                Param[6, 0, 0] = objEntidad.fechaModificacionArch.ToString();
                Param[6, 1, 0] = "@inFecModArch";
                Param[6, 2, 0] = "datetime";

                Param[7, 0, 0] = "C";
                Param[7, 1, 0] = "@inOperacion";
                Param[7, 2, 0] = "varchar(2)";

                Param[8, 0, 0] = objEntidad.parteFija.ToString();
                Param[8, 1, 0] = "@inParteFija";
                Param[8, 2, 0] = "varchar(20)";

                lista = wsc.LlenarLista(Param, Procedimiento, "SQLBancos", "SP", "Sql");
                string[,] Valida;

                if (lista.Count > 0)
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        RptPagosEN objParametros = new RptPagosEN();
                        Valida = lista[i];

                        objParametros.codigoBanco = Convert.ToInt32(Valida[0, 1].ToString());
                        objParametros.fechaPago = Valida[1, 1].ToString();
                        objParametros.cantPagosReacudo = Convert.ToInt32(Valida[2, 1].ToString());
                        listParametro.Add(objParametros);
                    }
                }

                return listParametro;
            }
            catch (Exception)
            {
                return listParametro;
            }
        }
        public string insertaPagoDebitoAD(string procedimiento, RptPagosEN objEntidad)
        {
            try
            {
                string[,,] Param = new string[9, 3, 1];

                Param[0, 0, 0] = objEntidad.codigoBanco.ToString();
                Param[0, 1, 0] = "@inCodBanco";
                Param[0, 2, 0] = "int";

                Param[1, 0, 0] = objEntidad.fechaPago.ToString();
                Param[1, 1, 0] = "@inFechPago";
                Param[1, 2, 0] = "date";

                Param[2, 0, 0] = objEntidad.fechaProceso.ToString();
                Param[2, 1, 0] = "@inFecProceso";
                Param[2, 2, 0] = "datetime";

                Param[3, 0, 0] = objEntidad.cantPagosArchivo.ToString();
                Param[3, 1, 0] = "@inCantPagosArch";
                Param[3, 2, 0] = "int";

                Param[4, 0, 0] = objEntidad.cantPagosReacudo.ToString();
                Param[4, 1, 0] = "@inCantPagosRec";
                Param[4, 2, 0] = "int";

                Param[5, 0, 0] = objEntidad.valorMontoArchivo.ToString();
                Param[5, 1, 0] = "@inMontoarchivo";
                Param[5, 2, 0] = "numeric(18,0)";

                Param[6, 0, 0] = objEntidad.fechaModificacionArch.ToString();
                Param[6, 1, 0] = "@inFecModArch";
                Param[6, 2, 0] = "datetime";

                Param[7, 0, 0] = "I";
                Param[7, 1, 0] = "@inOperacion";
                Param[7, 2, 0] = "varchar(2)";

                Param[8, 0, 0] = objEntidad.parteFija.ToString();
                Param[8, 1, 0] = "@inParteFija";
                Param[8, 2, 0] = "varchar(20)";

                return wsc.Ejecutar(Param, procedimiento, "SQLBancos");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string actualizaPagoDebitoAD(string procedimiento, RptPagosEN objEntidad)
        {
            try
            {
                string[,,] Param = new string[9, 3, 1];

                Param[0, 0, 0] = objEntidad.codigoBanco.ToString();
                Param[0, 1, 0] = "@inCodBanco";
                Param[0, 2, 0] = "int";

                Param[1, 0, 0] = objEntidad.fechaPago.ToString();
                Param[1, 1, 0] = "@inFechPago";
                Param[1, 2, 0] = "date";

                Param[2, 0, 0] = objEntidad.fechaProceso.ToString();
                Param[2, 1, 0] = "@inFecProceso";
                Param[2, 2, 0] = "datetime";

                Param[3, 0, 0] = objEntidad.cantPagosArchivo.ToString();
                Param[3, 1, 0] = "@inCantPagosArch";
                Param[3, 2, 0] = "int";

                Param[4, 0, 0] = objEntidad.cantPagosReacudo.ToString();
                Param[4, 1, 0] = "@inCantPagosRec";
                Param[4, 2, 0] = "int";

                Param[5, 0, 0] = objEntidad.valorMontoArchivo.ToString();
                Param[5, 1, 0] = "@inMontoarchivo";
                Param[5, 2, 0] = "numeric(18,0)";

                Param[6, 0, 0] = objEntidad.fechaModificacionArch.ToString();
                Param[6, 1, 0] = "@inFecModArch";
                Param[6, 2, 0] = "datetime";

                Param[7, 0, 0] = "U";
                Param[7, 1, 0] = "@inOperacion";
                Param[7, 2, 0] = "varchar(2)";

                Param[8, 0, 0] = objEntidad.parteFija.ToString();
                Param[8, 1, 0] = "@inParteFija";
                Param[8, 2, 0] = "varchar(20)";

                return wsc.Ejecutar(Param, procedimiento, "SQLBancos");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public string actualizarRptPagoSicoAD(string procedimiento, RptPagosEN objEntidad)
        {
            try
            {
                string[,,] Param = new string[7, 3, 1];

                Param[0, 0, 0] = objEntidad.fechaPago.ToString();
                Param[0, 1, 0] = "@inFechPago";
                Param[0, 2, 0] = "date";

                Param[1, 0, 0] = objEntidad.codigoBanco.ToString();
                Param[1, 1, 0] = "@inCodBanco";
                Param[1, 2, 0] = "int";

                Param[2, 0, 0] = objEntidad.cantPagosSicoCon.ToString();
                Param[2, 1, 0] = "@inCantPagoSicoCon";
                Param[2, 2, 0] = "int";

                Param[3, 0, 0] = objEntidad.valorMontoSicoCon.ToString();
                Param[3, 1, 0] = "@inValorMontoSicoCon";
                Param[3, 2, 0] = "numeric(18,0)";

                Param[4, 0, 0] = objEntidad.cantPagosSicoInc.ToString();
                Param[4, 1, 0] = "@inCantPagoSicoInc";
                Param[4, 2, 0] = "int";

                Param[5, 0, 0] = objEntidad.valorMontoSicoInc.ToString();
                Param[5, 1, 0] = "@inValorMontoSicoInc";
                Param[5, 2, 0] = "numeric(18,0)";

                Param[6, 0, 0] = objEntidad.parteFija.ToString();
                Param[6, 1, 0] = "@inParteFija";
                Param[6, 2, 0] = "varchar(20)";

                return wsc.Ejecutar(Param, procedimiento, "SQLBancos");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string insertaLogErroresAD(string procedimiento, String mensaje, int codigoBanco, String fechaPago, String parteFija)
        {
            try
            {
                string[,,] Param = new string[4, 3, 1];

                Param[0, 0, 0] = codigoBanco.ToString();
                Param[0, 1, 0] = "@inCodigoBanco";
                Param[0, 2, 0] = "int";

                Param[1, 0, 0] = mensaje;
                Param[1, 1, 0] = "@inMensaje";
                Param[1, 2, 0] = "nvarchar(MAX)";

                Param[2, 0, 0] = fechaPago;
                Param[2, 1, 0] = "@inFechaPago";
                Param[2, 2, 0] = "date";

                Param[3, 0, 0] = parteFija;
                Param[3, 1, 0] = "@inParteFija";
                Param[3, 2, 0] = "varchar(20)";

                return wsc.Ejecutar(Param, procedimiento, "SQLBancos");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<String[,]> ConsultaParteFijaAD(string Fiducia, string Recaudo, string LugarPago)
        {
            List<string[,]> lista = new List<string[,]>();
            try
            {
                string[,,] Param = new string[3, 3, 1];

                Param[0, 0, 0] = Fiducia;
                Param[0, 1, 0] = "@inFiducia";
                Param[0, 2, 0] = "varchar(3)";

                Param[1, 0, 0] = LugarPago;
                Param[1, 1, 0] = "@inLugarPago";
                Param[1, 2, 0] = "int";

                Param[2, 0, 0] = Recaudo;
                Param[2, 1, 0] = "@inRecaudo";
                Param[2, 2, 0] = "char(10)";

                lista = wsc.LlenarLista(Param, "pa_BAN_CON_ConsultaBancosParteFija", "SQLBancos", "SP", "Sql");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                
            }
            return lista;
        }
    }
}
