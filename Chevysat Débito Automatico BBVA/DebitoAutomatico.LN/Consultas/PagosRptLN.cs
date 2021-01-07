using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN.Tablas;

namespace DebitoAutomatico.LN.Consultas
{
    public class PagosRptLN
    {
        private String SP_ProcesarPagosDebito = "pa_BAN_CON_Procesa_Pagos_debito";
        private String SP_ActualizarRptPagoSico = "pa_BAN_CON_ACT_RPT_PAGOS_SICO";
        private String SP_LogErrores = "pa_BAN_RPT_ERROR_LOG";
        public string insertarPagoDebitoLN(RptPagosEN objEntidad)
        {

            string resultado = new RptPagosAD().insertaPagoDebitoAD(SP_ProcesarPagosDebito, objEntidad);
            return resultado;

        }
        public IList<RptPagosEN> ConsultarPagoDebitoLN(RptPagosEN objEntidad)
        {

            IList<RptPagosEN> lista = new RptPagosAD().ConsultarPagoDebitoAD(SP_ProcesarPagosDebito, objEntidad);
            return lista;
        }
       
        public string actualizarPagoDebitoLN(RptPagosEN objEntidad)
        {

            string resultado = new RptPagosAD().actualizaPagoDebitoAD(SP_ProcesarPagosDebito, objEntidad);
            return resultado;

        }
        public string actualizarRptPagoSicoLN(RptPagosEN objEntidad)
        {
            string resultado = new RptPagosAD().actualizarRptPagoSicoAD(SP_ActualizarRptPagoSico, objEntidad);
            return resultado;
        }
        public void almacenaRegistroSicoLN(WcfUtilidades Util, string NombreArchivoSico, string UsuFTP, string PassFTP, RptPagosEN pagosEN)
        {
            String error_mensaje;
            IList<string> recaudoSico;
            IList<String> inconsistentes;
            IList<String> consistentes;
            //SAU Revisar fichero en SICO
            string[] stringSeparators = new string[] { " " };
    
            IList<RptPagosEN> arrPagos = null;

            // Leo los resultados consistentes e inconsistentes de System
            try
            {
                //inconsistentes = Util.LeerFicheroFTP(ServidorSico, "IR" + NombreArchivoSico, PathSystem, UsuFTP, PassFTP, pagosEN.fechaPago, pagosEN.codigoBanco);
                inconsistentes = Util.LeerFicheroFTP("IR" + NombreArchivoSico, UsuFTP, PassFTP, pagosEN.fechaPago, pagosEN.codigoBanco);
                if (inconsistentes.Count > 0)
                {
                    try
                    {
                        string txtPago = inconsistentes.ToList().Find(x => x.Contains("Total Registros"));
                        if (txtPago != null)
                        {
                            recaudoSico = txtPago.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                            pagosEN.cantPagosSicoInc = Convert.ToInt32(recaudoSico.ElementAt(2).Replace(',', '.')) + pagosEN.cantPagosSicoInc;
                            pagosEN.valorMontoSicoInc = Convert.ToDouble(recaudoSico.ElementAt(3).Replace(',', '.')) + pagosEN.valorMontoSicoInc;
                            recaudoSico = null;
                        }
                        else
                        {
                            String res = this.consultaLogErroresLN("", pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                            if (res == "1")
                            {
                                this.actualizaLogErroresLN("DA: " + "Archivo cortado o vacío" + "IR" + NombreArchivoSico, pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                            }
                            else
                            {
                                this.insertaLogErroresLN("DA: " + "Archivo cortado o vacío"+ "IR" + NombreArchivoSico, pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        
                        String res = this.consultaLogErroresLN("", pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                        if (res == "1")
                        {
                            this.actualizaLogErroresLN("DA: " + e.Message.ToString(), pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                        }
                        else
                        {
                            this.insertaLogErroresLN("DA: " + e.Message.ToString(), pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                String res = this.consultaLogErroresLN("", pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                if (res == "1")
                {
                    this.actualizaLogErroresLN("DA: " + ex.Message.ToString(), pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                }
                else
                {
                    this.insertaLogErroresLN("DA: " + ex.Message.ToString(), pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                }
                inconsistentes = new List<string>();
            }
            try
            {
                //consistentes = Util.LeerFicheroFTP(ServidorSico, "R" + NombreArchivoSico, PathSystem, UsuFTP, PassFTP, pagosEN.fechaPago, pagosEN.codigoBanco);
                consistentes = Util.LeerFicheroFTP("R" + NombreArchivoSico, UsuFTP, PassFTP, pagosEN.fechaPago, pagosEN.codigoBanco);
                if (consistentes.Count > 0)
                {
                    try
                    {
                        string txtPago = consistentes.ToList().Find(x => x.Contains("Total Registros"));
                        if (txtPago != null)
                        {
                            recaudoSico = txtPago.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                            pagosEN.cantPagosSicoCon = Convert.ToInt32(recaudoSico.ElementAt(2).Replace(',', '.')) + pagosEN.cantPagosSicoCon;
                            pagosEN.valorMontoSicoCon = Convert.ToDouble(recaudoSico.ElementAt(3).Replace(',', '.')) + pagosEN.valorMontoSicoCon;
                            recaudoSico = null;
                        }
                        else
                        {
                            String res = this.consultaLogErroresLN("", pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                            if (res == "1")
                            {
                                this.actualizaLogErroresLN("DA: " + "Archivo cortado o vacío" + "R" + NombreArchivoSico, pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                            }
                            else
                            {
                                this.insertaLogErroresLN("DA: " + "Archivo cortado o vacío" + "R" + NombreArchivoSico, pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        String res = this.consultaLogErroresLN("", pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                        if (res == "1")
                        {
                            this.actualizaLogErroresLN("DA: " + e.Message.ToString(), pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                        }
                        else
                        {
                            this.insertaLogErroresLN("DA: " + e.Message.ToString(), pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                String res = this.consultaLogErroresLN("", pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                if (res == "1")
                {
                    this.actualizaLogErroresLN("DA: " + ex.Message.ToString(), pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                }
                else
                {
                    this.insertaLogErroresLN("DA: " + ex.Message.ToString(), pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                }
                consistentes = new List<string>();
            }
            arrPagos = this.ConsultarPagoDebitoLN(pagosEN);
            if (arrPagos.Count > 0) //Si existe
            {
                try
                {
                    int result = Convert.ToInt32(this.actualizarRptPagoSicoLN(pagosEN));
                    if (result == 0)
                    {
                        error_mensaje = "Error en la actualizacion Pagos/Montos Sico I/IR banco: " +
                                                pagosEN.codigoBanco + " " + pagosEN.fechaPago;

                        String res = this.consultaLogErroresLN("", pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                        if (res == "1")
                        {
                            this.actualizaLogErroresLN("DA: " + error_mensaje, pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                        }
                        else
                        {
                            this.insertaLogErroresLN("DA: " + error_mensaje, pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                        }

                        error_mensaje = String.Empty;
                    }
                }
                catch (Exception e)
                {
                    String res = this.consultaLogErroresLN("", pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                    if (res == "1")
                    {
                        this.actualizaLogErroresLN("DA: " + e.Message.ToString(), pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                    }
                    else
                    {
                        this.insertaLogErroresLN("DA: " + e.Message.ToString(), pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                    }
                    
                }
            }
        }
        public void insertaLogErroresLN(String mensaje, String fechaPago, int codigoBanco = 0, String parteFija = "")
        {
            String resultado = new RptPagosAD().insertaLogErroresAD(SP_LogErrores, mensaje, codigoBanco, fechaPago, parteFija);
        }
        public String consultaLogErroresLN(String mensaje, String fechaPago, int codigoBanco = 0, String parteFija = "")
        {
            return new RptPagosAD().consultaLogErroresAD(SP_LogErrores, mensaje, codigoBanco, fechaPago, parteFija);
        }
        public String actualizaLogErroresLN(String mensaje, String fechaPago, int codigoBanco = 0, String parteFija = "")
        {
            return new RptPagosAD().actualizaLogErroresAD(SP_LogErrores, mensaje, codigoBanco, fechaPago, parteFija);
        }
        public List<String[,]> ConsultaParteFijaLN(string Fiducia, string Recaudo, string LugarPago)
        {      
            try
            {
                return new RptPagosAD().ConsultaParteFijaAD(Fiducia, Recaudo, LugarPago);
            }

            catch (Exception)
            {
                return null;
            }
        }
    }
}
