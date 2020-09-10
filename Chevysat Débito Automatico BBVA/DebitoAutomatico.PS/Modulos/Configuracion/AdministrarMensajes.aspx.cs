using DebitoAutomatico.EN.Tablas;
using DebitoAutomatico.LN.Consultas;
using DebitoAutomatico.PS.Codigo;
using DebitoAutomatico.PS.Codigo.Correo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Autor: Nicolas Alexander Larrotta
/// Fecha Ultima Actualización: 20 de Junio de 2018
/// Observacion: Modulo Administracioón de Mensajes
/// </summary>


namespace DebitoAutomatico.PS.Modulos.Configuracion
{
    public partial class AdministrarMensajes : System.Web.UI.Page
    {
        Mensajes objM = new Mensajes();


        private int IdMensaje
        {
            get
            {
                int IdBa = 0;
                if (ViewState["IdMensaje"] != null)
                    IdBa = Convert.ToInt32(ViewState["IdMensaje"]);
                return IdBa;
            }

            set
            {
                ViewState["IdMensaje"] = value;
            }
        }

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
                this.ddlEstadoD.DataSource = new EstadosClientesLN().consultarTipoEstado(new EstadosClientes());
                this.ddlEstadoD.DataTextField = "pValor";
                this.ddlEstadoD.DataValueField = "pId";
                this.ddlEstadoD.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlEstadoD);
            }

        }

        protected void imgBtnNuevo_Click(object sender, ImageClickEventArgs e)
        {
            limpiar();
        }

        protected void imgBtnGuardar_Click(object sender, ImageClickEventArgs e)
        {
            int men = 0;
            if (this.ddlTipoContrato.SelectedIndex == 0 || this.ddlEstadoD.SelectedIndex == 0 || this.txbAsunto.Text == String.Empty || this.txbMensaje.Text == String.Empty)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.InformacionIncompleta + "');</script>", false);
                return;
            }

            objM.pTipoContrato = this.ddlTipoContrato.SelectedItem.Value;
            objM.pEstadoDebito = this.ddlEstadoD.SelectedIndex;
            objM.pMotivo = Convert.ToInt32(this.ddlMotivo.SelectedItem.Value);
            objM.pAsunto = this.txbAsunto.Text;
            objM.pMensaje = this.txbMensaje.Text.Trim();

            if (IdMensaje > 0)
            {
                objM.pId = IdMensaje;
                men = new MensajesLN().actualizar(objM);
            }
            else
            {   
                men = new MensajesLN().insertar(objM);
            }

            if (men > 0)
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ProcesoExistoso + "');</script>", false);
            else
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorProceso + "');</script>", false);
            limpiar();
        }

        public void limpiar()
        {
            this.ddlTipoContrato.SelectedIndex = this.ddlEstadoD.SelectedIndex = IdMensaje = 0;
            this.txbAsunto.Text = this.txbMensaje.Text = String.Empty;
        }

        protected void ddlMotivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddlMotivo.Enabled = false;
            if (this.ddlTipoContrato.SelectedIndex == 0 && this.ddlEstadoD.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.InformacionIncompleta + "');</script>", false);
                return;
            }

            objM.pTipoContrato = this.ddlTipoContrato.SelectedItem.Value;
            objM.pEstadoDebito = this.ddlEstadoD.SelectedIndex;
            objM.pMotivo = Convert.ToInt32(this.ddlMotivo.SelectedItem.Value);
            List<EN.Tablas.Mensajes> listaM = new MensajesLN().consultar(objM);

            if (listaM.Count > 0)
            {
                objM = listaM[0];
                IdMensaje = Convert.ToInt32(objM.pId);
                this.txbAsunto.Text = objM.pAsunto;
                this.txbMensaje.Text = objM.pMensaje;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.NoExisteInformacion + "');</script>", false);
                this.txbMensaje.Text = String.Empty;
            }
        }

        protected void ddlTipoContrato_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddlMotivo.SelectedIndex = this.ddlEstadoD.SelectedIndex = IdMensaje = 0;
            this.txbAsunto.Text = this.txbMensaje.Text = String.Empty;
            this.ddlEstadoD.Enabled = this.ddlMotivo.Enabled = true;
        }

        protected void ddlEstadoD_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddlEstadoD.Enabled = false;
        }
    }
}