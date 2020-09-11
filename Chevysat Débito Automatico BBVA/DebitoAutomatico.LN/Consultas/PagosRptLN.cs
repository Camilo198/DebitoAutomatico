using System;
using System.Collections.Generic;
using System.Linq;

using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN.Tablas;

namespace DebitoAutomatico.LN.Consultas
{
    public class PagosRptLN
    {
        private String SP_ConsultarPagosBanco = "pa_BAN_CON_RPT_BANCO_PAGOS";
        private String SP_ActualizarRptPagoSico = "pa_BAN_CON_ACT_RPT_PAGOS_SICO";
        private String SP_InsertaLogErrores = "pa_BAN_RPT_ERROR_LOG";
        public IList<RptPagosEN> ConsultarBancoFechaLN(RptPagosEN objEntidad)
        {

            IList<RptPagosEN> lista = new RptPagosAD().ConsultarBancoFechaAD(SP_ConsultarPagosBanco, objEntidad);
            return lista;
        }
        public string insertarBancoFechaLN(RptPagosEN objEntidad)
        {

            string resultado = new RptPagosAD().insertaBancoFechaAD(SP_ConsultarPagosBanco, objEntidad);
            return resultado;

        }
        public string actualizarBancoCantPagosRecaudoLN(RptPagosEN objEntidad)
        {

            string resultado = new RptPagosAD().actualizaBancoCantPagosRecAD(SP_ConsultarPagosBanco, objEntidad);
            return resultado;

        }
        public string actualizarRptPagoSicoLN(RptPagosEN objEntidad)
        {
            string resultado = new RptPagosAD().actualizarRptPagoSicoAD(SP_ActualizarRptPagoSico, objEntidad);
            return resultado;
        }
        public void almacenaRegistroSicoLN(WcfUtilidades Util, string ServidorSico, string NombreArchivoSico, string PathSystem,
            string UsuFTP, string PassFTP, int codBanco, string fechaRecaudo, DateTime FeModificacion)
        {
            string error_mensaje;
            IList<string> recaudoSico;
            IList<String> inconsistentes;
            IList<String> consistentes;
            List<string> busquedaPago;
            //SAU Revisar fichero en SICO
            string[] stringSeparators = new string[] { " " };
            RptPagosEN pagosEN = new RptPagosEN();

            IList<RptPagosEN> arrPagos = null;

            pagosEN.codigoBanco = codBanco;
            pagosEN.fechaPago = fechaRecaudo;
            pagosEN.fechaModificacionArch = FeModificacion;
            // Leo los resultados consistentes e inconsistentes de System
            try
            {
                inconsistentes = Util.LeerFicheroFTP(ServidorSico, "IR" + NombreArchivoSico, PathSystem, UsuFTP, PassFTP, pagosEN.fechaPago, pagosEN.codigoBanco);
                if (inconsistentes.Count > 0)
                {
                    try
                    {
                        string txtPago = inconsistentes.ToList().Find(x => x.Contains("Total Registros"));
                        if (txtPago != null)
                        {
                            recaudoSico = txtPago.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                            pagosEN.cantPagosSicoInc = Convert.ToInt32(recaudoSico.ElementAt(2));
                            pagosEN.valorMontoSicoInc = Convert.ToDouble(recaudoSico.ElementAt(3).Replace(',', '.'));
                            recaudoSico = null;
                        }
                        else
                        {
                            this.insertaLogErroresLN("Archivo cortado o vacío", pagosEN.fechaPago, pagosEN.codigoBanco);
                        }
                    }
                    catch (Exception e)
                    {
                        this.insertaLogErroresLN(e.Message.ToString(), pagosEN.fechaPago, pagosEN.codigoBanco);
                    }

                }

            }
            catch (Exception ex)
            {
                this.insertaLogErroresLN("Archivo " + NombreArchivoSico + " inexistente o cortado", pagosEN.fechaPago, codBanco);
                this.insertaLogErroresLN(ex.Message.ToString(), pagosEN.fechaPago, codBanco);
                inconsistentes = new List<string>();
            }
            try
            {
                consistentes = Util.LeerFicheroFTP(ServidorSico, "R" + NombreArchivoSico, PathSystem, UsuFTP, PassFTP, pagosEN.fechaPago, pagosEN.codigoBanco);
                if (consistentes.Count > 0)
                {
                    try
                    {
                        string txtPago = consistentes.ToList().Find(x => x.Contains("Total Registros"));
                        if (txtPago != null)
                        {
                            recaudoSico = txtPago.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                            pagosEN.cantPagosSicoCon = Convert.ToInt32(recaudoSico.ElementAt(2));
                            pagosEN.valorMontoSicoCon = Convert.ToDouble(recaudoSico.ElementAt(3).Replace(',', '.'));
                            recaudoSico = null;
                        }
                        else
                        {
                            this.insertaLogErroresLN("Archivo cortado o vacío", pagosEN.fechaPago, pagosEN.codigoBanco);
                        }
                    }
                    catch (Exception e)
                    {
                        this.insertaLogErroresLN(e.Message.ToString(), pagosEN.fechaPago, pagosEN.codigoBanco);
                    }
                }
            }
            catch (Exception ex)
            {
                this.insertaLogErroresLN("Archivo " + NombreArchivoSico + " inexistente o cortado", pagosEN.fechaPago, pagosEN.codigoBanco);
                this.insertaLogErroresLN(ex.Message.ToString(), pagosEN.fechaPago, pagosEN.codigoBanco);
                consistentes = new List<string>();
            }
            arrPagos = this.ConsultarBancoFechaLN(pagosEN);
            if (arrPagos.Count > 0) //Si existe
            {
                try
                {
                    int result = Convert.ToInt32(this.actualizarRptPagoSicoLN(pagosEN));
                    if (result == 0)
                    {
                        error_mensaje = "Error en la actualizacion Pagos/Montos Sico I/IR banco: " +
                                                pagosEN.codigoBanco + " " + pagosEN.fechaPago;
                        this.insertaLogErroresLN(error_mensaje, pagosEN.fechaPago, pagosEN.codigoBanco);
                        error_mensaje = String.Empty;
                    }
                }
                catch (Exception e)
                {
                    this.insertaLogErroresLN(e.Message.ToString(), pagosEN.fechaPago, pagosEN.codigoBanco);
                }
            }
        }
        public void insertaLogErroresLN(String mensaje, String fechaPago, int codigoBanco = 0)
        {
            String resultado = new RptPagosAD().insertaLogErroresAD(SP_InsertaLogErrores, mensaje, codigoBanco, fechaPago);
        }
    }
}
