using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DebitoAutomatico.PS.Codigo;
using DebitoAutomatico.PS.Codigo.Prenota;
using DebitoAutomatico.PS.Codigo.Interprete;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;
using DebitoAutomatico.LN.Consultas;
using DebitoAutomatico.EN.Definicion;
using System.Net;
using DebitoAutomatico.PS.Codigo.Ftp;
using System.Configuration;
using DebitoAutomatico.LN.Utilidades;


/// <summary>
/// Autor: Nicolas Alexander Larrotta
/// Fecha Ultima Actualización: 20 de Junio de 2018
/// Observacion: Lectura de archivos
/// </summary>
/// 
/// <summary>
/// Autor: Maikol Steven Ramirez
/// Fecha Ultima Actualización: 04 de Marzo de 2020
/// Observacion: Se modifica el metodo que lee archivo para que 
/// pueda leer el convenio con el BBVA, tambien se saca de ese metodo
/// la opción de enviar correo y se crea una interfaz diferente para 
/// poder enviar correos.
/// </summary>
///
namespace DebitoAutomatico.PS.Modulos.Servicio
{
    public partial class LecturaArchivos : System.Web.UI.Page
    {
        String RutaFtp = String.Empty;
        String UserFtp = String.Empty;
        String PassFtp = String.Empty;
        private String rutaProcesados
        {
            get
            {
                String rutaF = String.Empty;
                if (ViewState["rutaProcesados"] != null)
                    rutaF = Convert.ToString(ViewState["rutaProcesados"]);
                return rutaF;
            }

            set
            {
                ViewState["rutaProcesados"] = value;
            }
        }
        private String rutaRecibidos
        {
            get
            {
                String rutaE = String.Empty;
                if (ViewState["rutaRecibidos"] != null)
                    rutaE = Convert.ToString(ViewState["rutaRecibidos"]);
                return rutaE;
            }
            set
            {
                ViewState["rutaRecibidos"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Usuario objUsuario = new Usuario();
                Session["usuario"] = "Nicolas.Larrotta";
                if (Session["usuario"] == null)
                    objUsuario.pUsuario = Request.QueryString[0].ToString();
                else
                    objUsuario.pUsuario = Session["usuario"].ToString();

                List<EN.Tablas.Usuario> listaU = new UsuarioLN().consultar(objUsuario);

                objUsuario = listaU[0];

                Session["usuario"] = listaU[0].pUsuario.ToString();
                Session["perfil"] = listaU[0].pIdPerfil.ToString();
            }
            catch (Exception)
            {
                Response.Redirect("~/Modulos/MenuPrincipal.aspx");
            }

            if ((Session["perfil"] == null) || (Session["usuario"] == null))
            {
                Response.Redirect("~/Modulos/MenuPrincipal.aspx");
            }

            if (!IsPostBack)
            {
                Banco objB = new Banco();
                objB.pDebito = true;
                objB.pActivo = true;

                this.ddlBancoDebito.DataSource = new BancoLN().consultarBanco(objB);
                this.ddlBancoDebito.DataTextField = "pNombre";
                this.ddlBancoDebito.DataValueField = "pId";
                this.ddlBancoDebito.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlBancoDebito);
            }
        }
        protected void btnProcesar_Click(object sender, EventArgs e)
        {
            separarArchivo();
        }
        protected void ddlBancoDebito_SelectedIndexChanged(object sender, EventArgs e)
        {
            rutaRecibidos = traerRuta()[0].ToString();
        }
        private List<String> traerRuta()
        {
            List<String> rutas = new List<String>();
            EN.Tablas.Banco objB = new EN.Tablas.Banco();
            BancoLN objBancoLN = new BancoLN();
            objB.pId = Convert.ToInt32(this.ddlBancoDebito.SelectedValue);
            List<EN.Tablas.Banco> listaB = objBancoLN.consultarBanco(objB);
            if (listaB.Count > 0)
            {
                objB = listaB[0];

                Int32 IdRutaRecibidos = objB.pIdRecibidos.Value;
                Int32 IdRutaProcesados = objB.pIdErrores.Value;
                RutasLN objRutaLN = new RutasLN();
                Rutas objRutaProcesados = new Rutas();
                Rutas objRutaRecibidos = new Rutas();

                objRutaRecibidos.pId = IdRutaRecibidos;
                objRutaProcesados.pId = IdRutaProcesados;
                rutas.Add(objRutaLN.consultarRuta(objRutaRecibidos)[0].pRuta);
                rutas.Add(objRutaLN.consultarRuta(objRutaProcesados)[0].pRuta);
                return rutas;
            }
            else
            {
                return new List<string>();
            }
        }
        /// <summary>
        /// Se encarga de leer el archivo XML
        /// </summary>
        /// <param name="Campo"></param>
        /// <returns></returns>
        private String leerXML(String Campo)
        {
            String respuesta = String.Empty;

            CamposXML objCampos = new CamposXML();
            objCampos.pTabla = "BD";
            objCampos.pCampo = Campo;

            LectorXML objLector = new LectorXML();
            objLector.RutaXML = Server.MapPath("~") + "\\Modulos\\XML\\Configuracion.xml";
            respuesta = objLector.leerDatosXML(objCampos);

            return respuesta;
        }
        private void separarArchivo()
        {
            lbArchCarg.Text = cargarArchivo.FileName;

            if (String.IsNullOrEmpty(rutaRecibidos))
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.RutaIncorrecta + "');</script>", false);
                return;
            }

            if (cargarArchivo.HasFile)
            {
                if (cargarArchivo.FileName.ToLower().Contains(".txt") || cargarArchivo.FileName.ToLower().Contains(".inf") || cargarArchivo.FileName.ToLower().Contains(".xls") || cargarArchivo.FileName.ToLower().Contains(".xlsx"))
                {
                    try
                    {
                        RutaFtp = leerXML("B");
                        UserFtp = leerXML("C");
                        PassFtp = leerXML("D");

                        LectorArchivos objLectorA = new LectorArchivos();
                        List<String> archivoR = new List<String>();

                        archivoR = objLectorA.leerArchivo(rutaRecibidos + cargarArchivo.FileName, Convert.ToInt32(this.ddlBancoDebito.SelectedValue));

                        if (Convert.ToInt32(this.ddlBancoDebito.SelectedValue) == 2)
                            this.lbTotReg.Text = Convert.ToString(archivoR.Count - 1);
                        else
                            this.lbTotReg.Text = Convert.ToString(archivoR.Count);

                        String Ruta = rutaRecibidos + cargarArchivo.FileName;

                        ArrayList interprete = new ArrayList();
                        InterpreteArchivo objInterpreteA = new InterpreteArchivo();

                        objInterpreteA.FeModificacion = File.GetLastWriteTime(rutaRecibidos + cargarArchivo.FileName);

                        String FechaProceso = Convert.ToString(DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("H:mm:ss"));
                        interprete = objInterpreteA.ServicioInterprete(Convert.ToInt32(this.ddlBancoDebito.SelectedValue),
                                                          Session["usuario"].ToString(), archivoR, Ruta, cargarArchivo.FileName,
                                                           FechaProceso, true, RutaFtp, UserFtp, PassFtp, chbNotificacion.Checked);

                        this.LbPNAceptada.Text = interprete[1].ToString();
                        this.LbPNRechazada.Text = interprete[2].ToString();
                        this.LbDNAceptada.Text = interprete[3].ToString();
                        this.LbDNRechazada.Text = interprete[4].ToString();
                        if (this.LbPNAceptada.Text == "0" && this.LbPNRechazada.Text == "0" && this.LbDNAceptada.Text == "0" && this.LbDNRechazada.Text == "0")
                        {
                            cargarArchivo.Enabled = false;
                            ddlBancoDebito.Enabled = false;
                            btnProcesar.Visible = false;
                            chbNotificacion.Enabled = false;
                        }
                        else
                        {
                            btnCorreo.Visible = true;
                            cargarArchivo.Enabled = false;
                            ddlBancoDebito.Enabled = false;
                            chbNotificacion.Enabled = false;
                            btnProcesar.Visible = false;
                        }

                        this.LbRErroneas.Text = interprete[5].ToString();
                        this.lblTotal.Text = Convert.ToDouble(interprete[6]).ToString("C");
                        //  this.lbTotReg.Text = interprete[7].ToString();
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + interprete[0].ToString() + "');</script>", false);

                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + ex.Message + "');</script>", false);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.TipoArchivo + "');</script>", false);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorProceso + "');</script>", false);
            }
        }
        /// <summary>
        /// Carga los elementos de la pantalla de nuevo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgBtnNuevo_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Modulos/Servicio/LecturaArchivos.aspx");
        }
        private List<HistorialArchivos> Historial
        {
            get
            {
                List<HistorialArchivos> lista = new List<HistorialArchivos>();
                if (ViewState["Historial"] != null)
                    lista = (List<HistorialArchivos>)ViewState["Historial"];
                return lista;
            }

            set
            {
                ViewState["Historial"] = value;
            }
        }
        private List<HistorialArchivos> Historial_Temporal
        {
            get
            {
                List<HistorialArchivos> lista = new List<HistorialArchivos>();
                if (ViewState["Historial_Temporal"] != null)
                    lista = (List<HistorialArchivos>)ViewState["Historial_Temporal"];
                return lista;
            }

            set
            {
                ViewState["Historial_Temporal"] = value;
            }
        }
        protected void btnSeleccion_Click(object sender, EventArgs e)
        {

            if (Historial_Temporal.Count == 0)
            {
                for (int i = 0; i < Historial.Count; i++)
                {
                    if (Historial[i].pMarca == true)
                    {
                        Historial[i].pMarca = false;

                    }
                    else
                    {
                        Historial[i].pMarca = true;
                    }
                }
                this.GVCorreo.DataSource = Historial;
            }
            else
            {
                for (int i = 0; i < Historial_Temporal.Count; i++)
                {
                    for (int j = 0; j < Historial.Count; j++)
                    {
                        if (Historial_Temporal[i].pContrato.Equals(Historial[j].pContrato))
                        {
                            if (Historial_Temporal[i].pMarca == true)
                            {
                                Historial[j].pMarca = false;
                                Historial_Temporal[i].pMarca = false;
                            }
                            else
                            {
                                Historial[j].pMarca = true;
                                Historial_Temporal[i].pMarca = true;
                            }

                        }

                    }

                }
                this.GVCorreo.DataSource = Historial_Temporal;
            }
            this.GVCorreo.DataBind();
            mpeBusquedaCorreo.Show();
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            HistorialArchivos obj = new HistorialArchivos();
            obj.pNombreArchivo = lbArchCarg.Text;
            obj.pOperacion = "C3";
            HistorialArchivosLN objln = new HistorialArchivosLN();
            Historial = objln.consultarArchivosCorreo(obj);
            GVCorreo.DataSource = Historial;
            GVCorreo.DataBind();
            mpeBusquedaCorreo.Show();
            btnBuscar.Enabled = false;
            btnLimpiar.Enabled = true;
        }
        protected void GVCorreo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            this.GVCorreo.PageIndex = e.NewPageIndex;
            if (Historial_Temporal.Count == 0)
            {
                this.GVCorreo.DataSource = Historial;
            }
            else
            {
                this.GVCorreo.DataSource = Historial_Temporal;
            }
            this.GVCorreo.DataBind();
            mpeBusquedaCorreo.Show();
        }
        protected void btnNuevaBusqueda_Click(object sender, EventArgs e)
        {
            if (txbContratoB.Text != "")
            {
                Historial_Temporal = null;
                Historial_Temporal = Historial.FindAll(i => i.pContrato.Equals(txbContratoB.Text));
                this.GVCorreo.DataSource = Historial_Temporal;
            }
            else
            {
                if (txbRespuesta.Text != "" || txbCausal.Text != "" || txbEstado.Text != "" || txbTipoTranferencia.Text != "")
                {
                    if (txbRespuesta.Text != "" && txbCausal.Text != "" && txbEstado.Text != "" && txbTipoTranferencia.Text != "")
                    {

                        Historial_Temporal = null;
                        Historial_Temporal = Historial.FindAll(i => i.pRespuesta.Equals(txbRespuesta.Text) && i.pCausal.Equals(txbCausal.Text) && i.pEstado.Equals(txbEstado.Text) && i.pTipo_transferencia.Equals(txbTipoTranferencia.Text));
                        this.GVCorreo.DataSource = Historial_Temporal;


                    }
                    else
                    {
                        if (txbRespuesta.Text != "" && txbCausal.Text != "" && txbEstado.Text != "")
                        {

                            Historial_Temporal = null;
                            Historial_Temporal = Historial.FindAll(i => i.pRespuesta.Equals(txbRespuesta.Text) && i.pCausal.Equals(txbCausal.Text) && i.pEstado.Equals(txbEstado.Text));
                            this.GVCorreo.DataSource = Historial_Temporal;
                        }
                        else
                        {
                            if (txbRespuesta.Text != "" && txbCausal.Text != "" && txbTipoTranferencia.Text != "")
                            {

                                Historial_Temporal = null;
                                Historial_Temporal = Historial.FindAll(i => i.pRespuesta.Equals(txbRespuesta.Text) && i.pCausal.Equals(txbCausal.Text) && i.pTipo_transferencia.Equals(txbTipoTranferencia.Text));
                                this.GVCorreo.DataSource = Historial_Temporal;

                            }
                            else
                            {
                                if (txbRespuesta.Text != "" && txbEstado.Text != "" && txbTipoTranferencia.Text != "")
                                {

                                    Historial_Temporal = null;
                                    Historial_Temporal = Historial.FindAll(i => i.pRespuesta.Equals(txbRespuesta.Text) && i.pEstado.Equals(txbEstado.Text) && i.pTipo_transferencia.Equals(txbTipoTranferencia.Text));
                                    this.GVCorreo.DataSource = Historial_Temporal;

                                }
                                else
                                {
                                    if (txbCausal.Text != "" && txbEstado.Text != "" && txbTipoTranferencia.Text != "")
                                    {

                                        Historial_Temporal = null;
                                        Historial_Temporal = Historial.FindAll(i => i.pCausal.Equals(txbCausal.Text) && i.pEstado.Equals(txbEstado.Text) && i.pTipo_transferencia.Equals(txbTipoTranferencia.Text));
                                        this.GVCorreo.DataSource = Historial_Temporal;

                                    }
                                    else
                                    {//
                                        if (txbRespuesta.Text != "" && txbCausal.Text != "")
                                        {

                                            Historial_Temporal = null;
                                            Historial_Temporal = Historial.FindAll(i => i.pRespuesta.Equals(txbRespuesta.Text) && i.pCausal.Equals(txbCausal.Text));
                                            this.GVCorreo.DataSource = Historial_Temporal;

                                        }
                                        else
                                        {
                                            if (txbRespuesta.Text != "" && txbEstado.Text != "")
                                            {

                                                Historial_Temporal = null;
                                                Historial_Temporal = Historial.FindAll(i => i.pRespuesta.Equals(txbRespuesta.Text) && i.pEstado.Equals(txbEstado.Text));
                                                this.GVCorreo.DataSource = Historial_Temporal;

                                            }
                                            else
                                            {
                                                if (txbRespuesta.Text != "" && txbTipoTranferencia.Text != "")
                                                {

                                                    Historial_Temporal = null;
                                                    Historial_Temporal = Historial.FindAll(i => i.pRespuesta.Equals(txbRespuesta.Text) && i.pTipo_transferencia.Equals(txbTipoTranferencia.Text));
                                                    this.GVCorreo.DataSource = Historial_Temporal;

                                                }
                                                else
                                                {
                                                    if (txbCausal.Text != "" && txbEstado.Text != "")
                                                    {

                                                        Historial_Temporal = null;
                                                        Historial_Temporal = Historial.FindAll(i => i.pCausal.Equals(txbCausal.Text) && i.pEstado.Equals(txbEstado.Text));
                                                        this.GVCorreo.DataSource = Historial_Temporal;

                                                    }
                                                    else
                                                    {
                                                        if (txbCausal.Text != "" && txbTipoTranferencia.Text != "")
                                                        {

                                                            Historial_Temporal = null;
                                                            Historial_Temporal = Historial.FindAll(i => i.pCausal.Equals(txbCausal.Text) && i.pTipo_transferencia.Equals(txbTipoTranferencia.Text));
                                                            this.GVCorreo.DataSource = Historial_Temporal;

                                                        }
                                                        else
                                                        {
                                                            if (txbEstado.Text != "" && txbTipoTranferencia.Text != "")
                                                            {

                                                                Historial_Temporal = null;
                                                                Historial_Temporal = Historial.FindAll(i => i.pEstado.Equals(txbEstado.Text) && i.pTipo_transferencia.Equals(txbTipoTranferencia.Text));
                                                                this.GVCorreo.DataSource = Historial_Temporal;

                                                            }
                                                            else
                                                            {//
                                                                if (txbRespuesta.Text != "")
                                                                {

                                                                    Historial_Temporal = null;
                                                                    Historial_Temporal = Historial.FindAll(i => i.pRespuesta.Equals(txbRespuesta.Text));
                                                                    this.GVCorreo.DataSource = Historial_Temporal;

                                                                }
                                                                else
                                                                {
                                                                    if (txbCausal.Text != "")
                                                                    {

                                                                        Historial_Temporal = null;
                                                                        Historial_Temporal = Historial.FindAll(i => i.pCausal.Equals(txbCausal.Text));
                                                                        this.GVCorreo.DataSource = Historial_Temporal;

                                                                    }
                                                                    else
                                                                    {
                                                                        if (txbEstado.Text != "")
                                                                        {

                                                                            Historial_Temporal = null;
                                                                            Historial_Temporal = Historial.FindAll(i => i.pEstado.Equals(txbEstado.Text));
                                                                            this.GVCorreo.DataSource = Historial_Temporal;

                                                                        }
                                                                        else
                                                                        {
                                                                            if (txbTipoTranferencia.Text != "")
                                                                            {

                                                                                Historial_Temporal = null;
                                                                                Historial_Temporal = Historial.FindAll(i => i.pTipo_transferencia.Equals(txbTipoTranferencia.Text));
                                                                                this.GVCorreo.DataSource = Historial_Temporal;

                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Historial_Temporal = null;
                    this.GVCorreo.DataSource = Historial;
                }
            };

            this.GVCorreo.DataBind();
            mpeBusquedaCorreo.Show();
        }
        protected void txbContratoB_TextChanged(object sender, EventArgs e)
        {
            if (txbContratoB.Text == "")
            {
                txbRespuesta.Enabled = true;
                txbCausal.Enabled = true;
                txbEstado.Enabled = true;
                txbTipoTranferencia.Enabled = true;
                txbContratoB.Enabled = true;


            }
            else
            {

                txbRespuesta.Enabled = false;
                txbCausal.Enabled = false;
                txbEstado.Enabled = false;
                txbTipoTranferencia.Enabled = false;
                txbContratoB.Enabled = true;

            }
            mpeBusquedaCorreo.Show();
        }
        protected void txbRespuesta_TextChanged(object sender, EventArgs e)
        {
            if (txbRespuesta.Text == "")
            {
                txbRespuesta.Enabled = true;
                txbCausal.Enabled = true;
                txbEstado.Enabled = true;
                txbTipoTranferencia.Enabled = true;
                txbContratoB.Enabled = true;


            }
            else
            {

                txbRespuesta.Enabled = true;
                txbCausal.Enabled = true;
                txbEstado.Enabled = true;
                txbTipoTranferencia.Enabled = true;
                txbContratoB.Enabled = false;

            }
            mpeBusquedaCorreo.Show();
        }
        protected void txbCausal_TextChanged(object sender, EventArgs e)
        {
            if (txbCausal.Text == "")
            {
                txbRespuesta.Enabled = true;
                txbCausal.Enabled = true;
                txbEstado.Enabled = true;
                txbTipoTranferencia.Enabled = true;
                txbContratoB.Enabled = true;


            }
            else
            {

                txbRespuesta.Enabled = true;
                txbCausal.Enabled = true;
                txbEstado.Enabled = true;
                txbTipoTranferencia.Enabled = true;
                txbContratoB.Enabled = false;

            }
            mpeBusquedaCorreo.Show();
        }
        protected void txbEstado_TextChanged(object sender, EventArgs e)
        {
            if (txbEstado.Text == "")
            {
                txbRespuesta.Enabled = true;
                txbCausal.Enabled = true;
                txbEstado.Enabled = true;
                txbTipoTranferencia.Enabled = true;
                txbContratoB.Enabled = true;


            }
            else
            {

                txbRespuesta.Enabled = true;
                txbCausal.Enabled = true;
                txbEstado.Enabled = true;
                txbTipoTranferencia.Enabled = true;
                txbContratoB.Enabled = false;

            }
            mpeBusquedaCorreo.Show();
        }
        protected void txbTipoTranferencia_TextChanged(object sender, EventArgs e)
        {
            if (txbTipoTranferencia.Text == "")
            {
                txbRespuesta.Enabled = true;
                txbCausal.Enabled = true;
                txbEstado.Enabled = true;
                txbTipoTranferencia.Enabled = true;
                txbContratoB.Enabled = true;
            }
            else
            {
                txbRespuesta.Enabled = true;
                txbCausal.Enabled = true;
                txbEstado.Enabled = true;
                txbTipoTranferencia.Enabled = true;
                txbContratoB.Enabled = false;
            }
            mpeBusquedaCorreo.Show();
        }
        protected void GVCorreo_PageIndexChanged(object sender, EventArgs e)
        {

        }
        protected void GVCorreo_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {

                GridViewRow row = GVCorreo.SelectedRow;
                if (Historial_Temporal.Count == 0)
                {

                    if (Historial[row.DataItemIndex].pMarca == true)
                    {
                        Historial[row.DataItemIndex].pMarca = false;
                    }
                    else
                    {
                        Historial[row.DataItemIndex].pMarca = true;
                    }
                    GVCorreo.DataSource = Historial;
                }
                else
                {
                    if (Historial_Temporal[row.DataItemIndex].pMarca == true)
                    {
                        for (int i = 0; i < Historial.Count; i++)
                        {
                            if (Historial_Temporal[row.DataItemIndex].pContrato.Equals(Historial[i].pContrato))
                            {
                                Historial[i].pMarca = false;
                            }
                        }
                        Historial_Temporal[row.DataItemIndex].pMarca = false;
                    }
                    else
                    {
                        for (int i = 0; i < Historial.Count; i++)
                        {
                            if (Historial_Temporal[row.DataItemIndex].pContrato.Equals(Historial[i].pContrato))
                            {
                                Historial[i].pMarca = true;
                            }
                        }
                        Historial_Temporal[row.DataItemIndex].pMarca = true;

                    }
                    GVCorreo.DataSource = Historial_Temporal;

                }

                GVCorreo.DataBind();
                mpeBusquedaCorreo.Show();
            }
            catch (Exception ex)
            {
                Response.Write("<script type='text/javascript'>alert('" + ex.Message.Replace("'", "").Replace("\n", "").Replace("\r", "") + "');</script>");
            }

        }
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            Historial_Temporal = null;
            this.GVCorreo.DataSource = Historial;
            this.GVCorreo.DataBind();
            mpeBusquedaCorreo.Show();
            limpiar();
        }
        public void limpiar()
        {
            txbContratoB.Text = "";
            txbContratoB.Enabled = true;
            txbRespuesta.Text = "";
            txbRespuesta.Enabled = true;
            txbCausal.Text = "";
            txbCausal.Enabled = true;
            txbEstado.Text = "";
            txbEstado.Enabled = true;
            txbTipoTranferencia.Text = "";
            txbTipoTranferencia.Enabled = true;
        }
        protected void btntramitar_Click(object sender, EventArgs e)
        {
            try
            {
                string confirmValue = Request.Form["confirm_value"];

                if (confirmValue == "Yes")
                {
                    InterpreteArchivo objInterpreteA = new InterpreteArchivo();
                    objInterpreteA.EnviarCorreo(Historial);
                    Historial.RemoveAll(borrar => borrar.pMarca == true);
                    Historial_Temporal = null;
                    this.GVCorreo.DataSource = Historial;
                    this.GVCorreo.DataBind();
                    mpeBusquedaCorreo.Show();
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('Se envio correo sin novedad');</script>", false);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('"+ex.Message+"');</script>", false);
                
            }
        }
        protected void btntramita_Click(object sender, EventArgs e)
        {
            mpeBusquedaCorreo.Show();
        }


    }
}