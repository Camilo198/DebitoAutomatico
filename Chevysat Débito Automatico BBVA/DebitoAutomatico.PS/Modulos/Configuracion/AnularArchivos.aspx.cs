using DebitoAutomatico.EN.Tablas;
using DebitoAutomatico.LN.Consultas;
using DebitoAutomatico.PS.Codigo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Autor: Nicolas Alexander Larrotta
/// Fecha Ultima Actualización: 20 de Junio de 2018
/// Observacion: Modulo Anulación de archivos
/// </summary>
/// 
namespace DebitoAutomatico.PS.Modulos.Configuracion
{
    public partial class AnularArchivos : System.Web.UI.Page
    {
        private DataSet DsPremotas
        {
            get
            {
                DataSet tabla = new DataSet();
                if (ViewState["DsPremotas"] != null)
                    tabla = (DataSet)ViewState["DsPremotas"];
                return tabla;
            }

            set
            {
                ViewState["DsPremotas"] = value;
            }
        }

        private DataSet DsDebitos
        {
            get
            {
                DataSet tabla = new DataSet();
                if (ViewState["DsDebitos"] != null)
                    tabla = (DataSet)ViewState["DsDebitos"];
                return tabla;
            }

            set
            {
                ViewState["DsDebitos"] = value;
            }
        }

        Banco objB = new Banco();
        HistorialArchivos objHa = new HistorialArchivos();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Usuario objUsuario = new Usuario();
                
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

            RutasLN objRutaLN = new RutasLN();
            Rutas objRuta = new Rutas();
            objRuta.pId = objB.pIdPrenota;
            String path = objRutaLN.consultarRuta(objRuta)[0].pRuta;

            if (!IsPostBack)
            {
                TipoArchivo objTa = new TipoArchivo();
                this.ddlTipoMovimiento.DataSource = new TipoArchivoLN().consultarProceso(objTa);
                this.ddlTipoMovimiento.DataTextField = "pNombre";
                this.ddlTipoMovimiento.DataValueField = "pId";
                this.ddlTipoMovimiento.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlTipoMovimiento);
            }

        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            objHa.pFecha = Convert.ToDateTime(this.txbFechaInicial.Text).ToString("dd/MM/yyyy");
            objHa.pTipoArchivo = this.ddlTipoMovimiento.SelectedValue;
            objHa.pManual = this.chbManual.Checked;

            if (this.ddlTipoMovimiento.SelectedValue == "GPN") //Prenota
            {
                DsPremotas = new HistorialArchivosLN().consultarArchivos(objHa);

                if (DsPremotas.Tables[0].Rows.Count > 0)
                {
                    this.pnlPrenotas.Visible = true;
                    gvHistorial.DataSource = DsPremotas;
                    gvHistorial.DataBind();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.NoExisteInformacion + "');</script>", false);
                    this.pnlPrenotas.Visible = this.pnlDebito.Visible = false;
                }
            }
            else if (this.ddlTipoMovimiento.SelectedValue == "GDA") //Debito
            {
                DsDebitos = new HistorialArchivosLN().consultarArchivos(objHa);

                if (DsDebitos.Tables[0].Rows.Count > 0)
                {
                    this.pnlDebito.Visible = true;
                    gvRespuestas.DataSource = DsDebitos;
                    gvRespuestas.DataBind();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.NoExisteInformacion + "');</script>", false);
                    this.pnlPrenotas.Visible = this.pnlDebito.Visible = false;
                }
            }
        }

        protected void imgBtnNuevo_Click(object sender, ImageClickEventArgs e)
        {
            Nuevo();
        }

        public void Nuevo()
        {
            this.ddlTipoMovimiento.SelectedIndex = 0;
            this.txbFechaInicial.Text = String.Empty;
            this.chbManual.Checked = false;
            this.gvHistorial.DataSource = null;
            this.pnlPrenotas.Visible = this.pnlDebito.Visible = false;
        }

        //Prenotas
        protected void btnAnular_Click(object sender, EventArgs e)
        {
            HistorialArchivos objHAct = new HistorialArchivos();
            objHa.pFecha = Convert.ToDateTime(this.txbFechaInicial.Text).ToString("dd/MM/yyyy");
            objHa.pTipoArchivo = this.ddlTipoMovimiento.SelectedValue;
            objHa.pManual = this.chbManual.Checked;
            List<EN.Tablas.HistorialArchivos> listHistA = new HistorialArchivosLN().consultar(objHa);

            if (listHistA.Count > 0)
            {
                int histv = 0;
                int titular = 0;
                objHa = listHistA[0];
                objHAct.pFecha = objHa.pFecha;
                objHAct.pTipoArchivo = objHa.pTipoArchivo;
                objHAct.pEstado = "A";
                objHAct.pManual = this.chbManual.Checked;
                objHAct.pUsuarioModifico = Session["usuario"].ToString();
                histv = new HistorialArchivosLN().actualizarPrenota(objHAct);

                if (histv > 0)
                {
                    DatosDebito objT = new DatosDebito();
                    ArchivoManualLN objAM = new ArchivoManualLN();
                    ArchivoManual objEntidad = new ArchivoManual();

                    for (int i = 0; i < listHistA.Count; i++)
                    {
                        objHa = listHistA[i];
                        objT.pContrato = objHa.pContrato;
                        objT.pEstado = 1;
                        objT.pIntentos = 0;
                        titular = new DatosDebitoLN().actualizarEstado(objT);
                        
                        if (this.chbManual.Checked)
                        {
                            objEntidad.pContrato = Convert.ToInt32(objHa.pContrato);
                            int valor = objAM.borrar(objEntidad);    
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorProceso + "');</script>", false);
                }

                if (titular > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ProcesoExistoso + "');</script>", false);
                    Nuevo();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorProceso + "');</script>", false);
                }
            }
        }

        //Debitos
        protected void gvRespuestas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Eliminar"))
            {
                
                objHa.pFecha = Convert.ToDateTime(this.txbFechaInicial.Text).ToString("dd/MM/yyyy");
                objHa.pTipoArchivo = this.ddlTipoMovimiento.SelectedValue;
                objHa.pCodigoBanco = HttpUtility.HtmlDecode(gvRespuestas.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text.Trim());
                objHa.pNombreArchivo = HttpUtility.HtmlDecode(gvRespuestas.Rows[Convert.ToInt32(e.CommandArgument)].Cells[7].Text.Trim());
                objHa.pManual = this.chbManual.Checked;
                List<EN.Tablas.HistorialArchivos> listHistA = new HistorialArchivosLN().consultar(objHa);

                if (listHistA.Count > 0)
                {
                    int histv = 0;
                    int titular = 0;

                    HistorialArchivos objHActD = new HistorialArchivos();
                    objHActD.pCodigoBanco = HttpUtility.HtmlDecode(gvRespuestas.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text.Trim());
                    objHActD.pNombreArchivo = HttpUtility.HtmlDecode(gvRespuestas.Rows[Convert.ToInt32(e.CommandArgument)].Cells[7].Text.Trim());
                    objHActD.pFecha = this.txbFechaInicial.Text;
                    objHActD.pTipoArchivo = this.ddlTipoMovimiento.SelectedValue;
                    objHActD.pEstado = "A";
                    objHActD.pUsuarioModifico = Session["usuario"].ToString();
                    objHActD.pManual = this.chbManual.Checked;
     
                    histv = new HistorialArchivosLN().actualizarDebito(objHActD);

                    if (histv > 0)
                    {
                        PagoParcial objP = new PagoParcial();
                        DatosDebito objT = new DatosDebito();
                        ArchivoManualLN objAM = new ArchivoManualLN();
                        ArchivoManual objEntidad = new ArchivoManual();
                        for (int i = 0; i < listHistA.Count; i++)
                        {
                            objHa = listHistA[i];

                            objT.pContrato = objP.pContrato = objHa.pContrato;
                            objT.pEstado = 4;
                            objT.pIntentos = 0;
                            titular = new DatosDebitoLN().actualizarEstado(objT);

                            if (this.chbManual.Checked)
                            {
                                objEntidad.pContrato = Convert.ToInt32(objHa.pContrato);
                                int valor = objAM.borrar(objEntidad);
                            }


                            List<EN.Tablas.PagoParcial> listPP = new PagoParcialLN().consultar(objP);

                            if (listPP.Count > 0)
                            {
                                 int borrar = new PagoParcialLN().borrar(objP);
                            }

                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorProceso + "');</script>", false);
                    }

                    if (titular > 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ProcesoExistoso + "');</script>", false);
                        
                        DsDebitos = new HistorialArchivosLN().consultarArchivos(objHa);

                        if (DsDebitos.Tables[0].Rows.Count > 0)
                        {
                            this.pnlDebito.Visible = true;
                            gvRespuestas.DataSource = null;
                            gvRespuestas.DataSource = DsDebitos;
                            gvRespuestas.DataBind();
                        }
                        else
                        {
                            Nuevo();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorProceso + "');</script>", false);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorProceso + "');</script>", false);
                }

               
            }
        }

    }
}