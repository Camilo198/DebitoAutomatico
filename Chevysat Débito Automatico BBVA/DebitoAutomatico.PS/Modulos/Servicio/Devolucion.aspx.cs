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
/// Observacion: Modulo Devoluciones
/// </summary>
/// 
namespace DebitoAutomatico.PS.Modulos.Servicio
{
    public partial class Devolucion : System.Web.UI.Page
    {

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
                this.ddlTipoCausales.DataSource = new TipoCausalesLN().consultarTipoCausal(new TipoCausales());
                this.ddlTipoCausales.DataTextField = "pValor";
                this.ddlTipoCausales.DataValueField = "pId";
                this.ddlTipoCausales.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlTipoCausales);
            }

        }

        protected void btnContrato_Click(object sender, EventArgs e)
        {
            if (this.txbContrato.Text == String.Empty && this.txbFechaInicial.Text == String.Empty)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.SinFiltro + "');</script>", false);
                return;
            }
            
            ConsultarInformacion();
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

        public void ConsultarInformacion()
        {
            HistorialProcesoUsuario objHPU = new HistorialProcesoUsuario();
            objHPU.pContrato = this.txbContrato.Text;
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

        public void limpiar()
        {
            this.ddlTipoCausales.SelectedIndex = 0;
            this.txbContrato.Text = this.txbFechaGiro.Text = this.txbFechaInicial.Text = String.Empty;
            this.pnlMovimientos.Visible = this.pnlCambio.Visible = false;
            this.gvMovimientos.DataSource = null;
        }

        protected void imgBtnGuardar_Click(object sender, ImageClickEventArgs e)
        {
            Devoluciones objD = new Devoluciones();

            int Cont = 0;
            int Ins = 0;
            string Valor = String.Empty;
            string Contrato = String.Empty;

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

            objD.pIdTipoCausal = Convert.ToInt32(this.ddlTipoCausales.SelectedValue);
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
                logs = Log(Contrato);
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

        /// <summary>
        /// Meotod Log que guarda la actualizacion del cliente en la tabla tb_DEB_LOGS_USUARIO
        /// </summary>
        /// <param name="opcion"></param>
        /// <param name="ContratoCliente"></param>
        private int Log(string ContratoCliente)
        {
            int val = 0;
            LogsUsuario objL = new LogsUsuario();

            objL.pMovimiento = "DEVOLUCION";
            objL.pFecha = String.Empty;
            objL.pUsuario = Session["usuario"].ToString();
            objL.pDetalle = "Se actualizó el contrato N°: " + campos;
            objL.pContrato = ContratoCliente;
            val = new LogsUsuarioLN().insertar(objL);
            return val;
        }
    }
}