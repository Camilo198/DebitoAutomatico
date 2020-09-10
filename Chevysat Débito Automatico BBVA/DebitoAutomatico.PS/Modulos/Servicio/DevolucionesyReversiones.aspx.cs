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
/// Fecha Ultima Actualización: 18 de Abril de 2018
/// Observacion: Modulo Devolucioes y reversiones
/// </summary>
/// 
namespace DebitoAutomatico.PS.Modulos.Servicio
{
    public partial class DevolucionesyReversiones : System.Web.UI.Page
    {
        private ServiceDebito.ServiceDebito SerDebito = new ServiceDebito.ServiceDebito();

        private List<HistorialProcesoUsuario> Historial
        {
            get
            {
                List<HistorialProcesoUsuario> lista = new List<HistorialProcesoUsuario>();
                if (ViewState["Historial"] != null)
                    lista = (List<HistorialProcesoUsuario>)ViewState["Historial"];
                return lista;
            }

            set
            {
                ViewState["Historial"] = value;
            }
        }

        String campos = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Usuario objUsuario = new Usuario();
                 Session["usuario"] = "nicolas.larrotta";
                if (Session["usuario"] == null)
                    objUsuario.pUsuario = Request.QueryString[0].ToString();
                else
                    objUsuario.pUsuario = Session["usuario"].ToString();

                List<EN.Tablas.Usuario> listaU = new UsuarioLN().consultar(objUsuario);

                objUsuario = listaU[0];

                Session["usuario"] = listaU[0].pUsuario.ToString();
                Session["perfil"] = listaU[0].pIdPerfil.ToString();


                if (Session["perfil"].ToString() == "3") //Perfil Auxiliar Debito
                {
                    this.ddlProceso.SelectedValue = "2";
                    this.ddlProceso.Enabled = false;
                    this.pnlFiltros.Visible = this.pnlFiltros.Visible = this.lblBancoDebito.Visible = this.ddlBancoDebito.Visible = this.lblConvenio.Visible =
                    this.ddlConvenios.Visible = this.lblEstadoDebito.Visible = this.ddlEstadoDebito.Visible =
                    this.lblEstadoActual.Visible = this.ddlEstadoTransaccionInicial.Visible = btnSeleccion.Visible =
                    this.lblEstadoNuevo.Visible = this.ddlEstadoTransaccionFinal.Visible = true;

                    this.lblFechaGrio.Visible = this.txbFechaGiro.Visible = this.imgBtnFechaGiro.Visible = false;
                }
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
                this.ddlBancoDebito.DataSource = new BancoLN().consultarBanco(objB);
                this.ddlBancoDebito.DataTextField = "pNombre";
                this.ddlBancoDebito.DataValueField = "pId";
                this.ddlBancoDebito.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlBancoDebito);

                DataSet IdBanco = new DataSet();
                String Bancos = SerDebito.IdBanco(Session["usuario"].ToString(), "");
                System.IO.StringReader xmlSRB = new System.IO.StringReader(Bancos);
                IdBanco.ReadXml(xmlSRB);
                this.ddlConvenios.DataSource = IdBanco;
                this.ddlConvenios.DataTextField = "NOMBRE";
                this.ddlConvenios.DataValueField = "ID";
                this.ddlConvenios.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlConvenios);

                this.ddlEstadoDebito.DataSource = new EstadosClientesLN().consultarEstado(new EstadosClientes());
                this.ddlEstadoDebito.DataTextField = "pValor";
                this.ddlEstadoDebito.DataValueField = "pId";
                this.ddlEstadoDebito.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlEstadoDebito);

                this.ddlTipoCausales.DataSource = new TipoCausalesLN().consultarTipoCausal(new TipoCausales());
                this.ddlTipoCausales.DataTextField = "pValor";
                this.ddlTipoCausales.DataValueField = "pId";
                this.ddlTipoCausales.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlTipoCausales);
            }
        }

        protected void btnContrato_Click(object sender, EventArgs e)
        {
            if (this.txbContrato.Text == String.Empty && this.txbFechaInicial.Text == String.Empty && this.ddlProceso.SelectedValue == "1")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.SinFiltro + "');</script>", false);
                return;
            }

            if (this.ddlBancoDebito.SelectedIndex == 0 && this.ddlConvenios.SelectedIndex == 0 && this.ddlEstadoDebito.SelectedIndex == 0
                && this.txbContrato.Text == String.Empty && this.txbFechaInicial.Text == String.Empty)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.SinFiltro + "');</script>", false);
                return;
            }

            ConsultarInformacion();
        }

        public void ConsultarInformacion()
        {
            HistorialProcesoUsuario objHPU = new HistorialProcesoUsuario();
            objHPU.pContrato = this.txbContrato.Text;
            objHPU.pNombreBancoDebita = this.ddlBancoDebito.SelectedItem.Text;
            objHPU.pNombreBanco = this.ddlConvenios.SelectedItem.Text;
            objHPU.pEstadoCliente = this.ddlEstadoDebito.SelectedValue;

            if (Session["perfil"].ToString() == "2")
                objHPU.pValidacion = true;

            if (this.txbFechaInicial.Text != String.Empty)
            {
                objHPU.pFecha = Convert.ToDateTime(this.txbFechaInicial.Text).ToString("dd/MM/yyyy");
            }

            Historial = new HistorialProcesoUsuarioLN().consultar(objHPU);

            if (Historial.Count > 0)
            {
                Historial.Reverse();
                this.gvMovimientos.DataSource = Historial;
                this.gvMovimientos.DataBind();
                this.lbDebProceso.Text = gvMovimientos.Rows.Count.ToString();
                this.pnlMovimientos.Visible = this.pnlCambio.Visible = true;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.NoExisteInformacion + "');</script>", false);
                limpiar();
            }
        }

        protected void imgBtnNuevo_Click(object sender, ImageClickEventArgs e)
        {
            limpiar();
        }

        protected void btnSeleccion_Click(object sender, EventArgs e)
        {
            bool marcado = true;
            foreach (GridViewRow dtgItem in this.gvMovimientos.Rows)
            {
                if (((CheckBox)gvMovimientos.Rows[dtgItem.RowIndex].FindControl("chbReversar")).Checked == true)
                    marcado = false;
            }

            foreach (GridViewRow dtgItem in this.gvMovimientos.Rows)
            {
                ((CheckBox)gvMovimientos.Rows[dtgItem.RowIndex].FindControl("chbReversar")).Checked = marcado;
            }
        }

        protected void gvMovimientos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvMovimientos.PageIndex = e.NewPageIndex;
            this.gvMovimientos.DataSource = Historial;
            this.gvMovimientos.DataBind();
        }

        public void limpiar()
        {
            if (Session["perfil"].ToString() == "2")
            {
                this.pnlFiltros.Visible = false;
                this.ddlProceso.SelectedIndex = 0;
            }

            this.ddlBancoDebito.SelectedIndex = this.ddlEstadoDebito.SelectedIndex = this.ddlConvenios.SelectedIndex =
            this.ddlEstadoTransaccionInicial.SelectedIndex = this.ddlEstadoTransaccionFinal.SelectedIndex = 0;
            this.ddlTipoCausales.SelectedIndex = 0;
            this.txbContrato.Text = this.txbFechaGiro.Text = this.txbFechaInicial.Text = String.Empty;
            this.pnlMovimientos.Visible = this.pnlCambio.Visible = false;
            this.gvMovimientos.DataSource = null;
        }

        protected void imgBtnGuardar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                Devoluciones objD = new Devoluciones();

                int Cont = 0;
                int Ins = 0;
                string Valor = String.Empty;
                string Contrato = String.Empty;
                if (this.ddlProceso.SelectedValue == "1") //Devoluciones Perfil Administrador Debito
                {
                    if (this.ddlTipoCausales.SelectedIndex == 0 || this.txbFechaGiro.Text == String.Empty)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.InformacionIncompleta + "');</script>", false);
                        return;
                    }

                    foreach (GridViewRow dtgItem in this.gvMovimientos.Rows)
                    {
                        if (((CheckBox)gvMovimientos.Rows[dtgItem.RowIndex].FindControl("chbReversar")).Checked == true)
                        {
                            Cont++;
                            objD.pHistCliente = Convert.ToInt32(gvMovimientos.Rows[dtgItem.RowIndex].Cells[0].Text);
                            Valor = gvMovimientos.Rows[dtgItem.RowIndex].Cells[11].Text;
                            Contrato = gvMovimientos.Rows[dtgItem.RowIndex].Cells[2].Text;
                        }
                    }

                    if (Cont > 1)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.Devoluciones + "');</script>", false);
                        return;
                    }

                    if (Cont == 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.InformacionIncompleta + "');</script>", false);
                        return;
                    }

                    objD.pIdTipoCausal = this.ddlTipoCausales.SelectedIndex;
                    objD.pFechaGiro = Convert.ToDateTime(this.txbFechaGiro.Text).ToString("yyyy-dd-MM");

                    Ins = new DevolucionesLN().insertar(objD);

                    if (Ins > 0)
                    {

                        int logs = 0;
                        #region (INFORMACION PARA LOG)
                        campos = string.Concat(this.txbContrato.Text,
                            " con la CAUSAL:", this.ddlTipoCausales.SelectedItem.Text.ToString().ToUpper(),
                            ", FECHA DE GIRO:", this.txbFechaGiro.Text,
                            ", VALOR:", Valor == String.Empty ? "0" : Valor);
                        #endregion
                        logs = Log(1, Contrato);
                        if (logs > 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ProcesoExistoso + "');</script>", false);
                            ConsultarInformacion();
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
                else if (this.ddlProceso.SelectedValue == "2") //Reversiones Perfil Auxiliar Debito
                {

                    int conth = 0;
                    int contlog = 0;
                    int contact = 0;

                    HistorialProcesoUsuario objH = new HistorialProcesoUsuario();

                    foreach (GridViewRow dtgItem in this.gvMovimientos.Rows)
                    {
                        if (((CheckBox)gvMovimientos.Rows[dtgItem.RowIndex].FindControl("chbReversar")).Checked == true)
                        {
                            Cont++;
                            objH.pId = Convert.ToInt32(gvMovimientos.Rows[dtgItem.RowIndex].Cells[0].Text);
                            objH.pEstado = Convert.ToString(this.ddlEstadoTransaccionFinal.SelectedItem.Text);
                            objH.pCausal = ddlTipoCausales.SelectedItem.Text;

                            int his = 0;
                            his = new HistorialProcesoUsuarioLN().actualizar(objH);

                            if (his > 0)
                            {
                                conth++;
                                DatosDebito objT = new DatosDebito();
                                objT.pContrato = gvMovimientos.Rows[dtgItem.RowIndex].Cells[2].Text;

                                if (this.ddlEstadoTransaccionFinal.SelectedItem.Text == "RECHAZADO")
                                    objT.pEstado = 4;
                                else
                                    objT.pEstado = 8;

                                int act = 0;
                                act = new DatosDebitoLN().actualizarEstado(objT);

                                if (act > 0)
                                {
                                    contact++;
                                    int logs = 0;
                                    #region (INFORMACION PARA LOG)
                                    campos = string.Concat(objT.pContrato,
                                        " con la CAUSAL:", this.ddlTipoCausales.SelectedItem.Text.ToString().ToUpper(),
                                        ", ESTADO INICIAL:", this.ddlEstadoTransaccionInicial.SelectedItem.Text,
                                        ", ESTADO FINAL:", this.ddlEstadoTransaccionFinal.SelectedItem.Text,
                                        ", VALOR:", Valor == String.Empty ? "0" : Valor);
                                    #endregion
                                    logs = Log(2, objT.pContrato);

                                    if (logs > 0)
                                    {
                                        contlog++;
                                    }
                                }
                            }
                        }
                    }

                    if (Cont == 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.InformacionIncompleta + "');</script>", false);
                        return;
                    }

                    if (conth > 0 && contlog > 0 && contact > 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ProcesoExistoso + "');</script>", false);
                        ConsultarInformacion();
                        this.ddlTipoCausales.SelectedIndex = this.ddlEstadoTransaccionInicial.SelectedIndex = this.ddlEstadoTransaccionFinal.SelectedIndex = 0;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorProceso + "');</script>", false);
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + ex.Message.Replace("'", "").Replace("\n", "").Replace("\r", "") + "');</script>", false);
            }
        }

        /// <summary>
        /// Meotod Log que guarda la actualizacion del cliente en la tabla tb_DEB_LOGS_USUARIO
        /// </summary>
        /// <param name="opcion"></param>
        /// <param name="ContratoCliente"></param>
        private int Log(int opcion, string ContratoCliente)
        {
            int val = 0;
            LogsUsuario objL = new LogsUsuario();

            if (opcion == 1)
                objL.pMovimiento = "DEVOLUCION";
            else
                objL.pMovimiento = "REVERSIÓN";

            objL.pFecha = String.Empty;
            objL.pUsuario = Session["usuario"].ToString();
            objL.pDetalle = "Se actualizó el contrato N°: " + campos;
            objL.pContrato = ContratoCliente;
            val = new LogsUsuarioLN().insertar(objL);
            return val;
        }

        protected void ddlEstadoTransaccionInicial_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlEstadoTransaccionInicial.SelectedItem.Text == "[Seleccione]")
            {
                this.ddlEstadoTransaccionFinal.SelectedItem.Text = "[Seleccione]";
            }

            if (this.ddlEstadoTransaccionInicial.SelectedItem.Text == "ACEPTADO")
            {
                this.ddlEstadoTransaccionFinal.SelectedItem.Text = "RECHAZADO";
                this.ddlEstadoTransaccionFinal.Enabled = false;
            }
            else if (this.ddlEstadoTransaccionInicial.SelectedItem.Text == "RECHAZADO")
            {
                this.ddlEstadoTransaccionFinal.SelectedItem.Text = "ACEPTADO";
                this.ddlEstadoTransaccionFinal.Enabled = false;
            }
        }

        protected void ddlProceso_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProceso.SelectedItem.Value == "1") //Devolución
            {
                this.pnlFiltros.Visible = this.btnSeleccion.Visible = this.lblFechaGrio.Visible = this.txbFechaGiro.Visible = this.imgBtnFechaGiro.Visible = true;
                this.lblBancoDebito.Visible = this.ddlBancoDebito.Visible = this.lblConvenio.Visible =
                this.ddlConvenios.Visible = this.lblEstadoDebito.Visible = this.ddlEstadoDebito.Visible =
                this.lblEstadoActual.Visible = this.ddlEstadoTransaccionInicial.Visible =
                this.lblEstadoNuevo.Visible = this.ddlEstadoTransaccionFinal.Visible = false;
            }
            else if (ddlProceso.SelectedItem.Value == "2") //Reversiones
            {
                this.lblFechaGrio.Visible = this.txbFechaGiro.Visible = this.imgBtnFechaGiro.Visible = false;

                this.pnlFiltros.Visible = this.lblBancoDebito.Visible = this.ddlBancoDebito.Visible = this.lblConvenio.Visible =
                this.ddlConvenios.Visible = this.lblEstadoDebito.Visible = this.ddlEstadoDebito.Visible =
                this.lblEstadoActual.Visible = this.ddlEstadoTransaccionInicial.Visible = btnSeleccion.Visible =
                this.lblEstadoNuevo.Visible = this.ddlEstadoTransaccionFinal.Visible = true;
            }

            this.ddlConvenios.SelectedIndex = this.ddlBancoDebito.SelectedIndex = this.ddlEstadoDebito.SelectedIndex =
            this.ddlTipoCausales.SelectedIndex = this.ddlEstadoTransaccionInicial.SelectedIndex = this.ddlEstadoTransaccionFinal.SelectedIndex = 0;

            this.txbContrato.Text = this.txbFechaGiro.Text = this.txbFechaInicial.Text = String.Empty;
            this.pnlMovimientos.Visible = this.pnlCambio.Visible = false;
        }
    }
}