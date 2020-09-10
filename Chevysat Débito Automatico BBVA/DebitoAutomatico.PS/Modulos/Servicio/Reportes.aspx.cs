using DebitoAutomatico.EN.Tablas;
using DebitoAutomatico.LN.Consultas;
using DebitoAutomatico.PS.Codigo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


/// <summary>
/// Autor: Nicolas Alexander Larrotta
/// Fecha Ultima Actualización: 20 de Junio de 2018
/// Observacion: Modulo de Reportes
/// </summary>
/// 
/// <summary>
/// Autor: Maikol Steven Ramirez    
/// Fecha Ultima Actualización: 04 de Marzo de 2020
/// Observacion: Se crea informe para seguimiento de correos
/// </summary>
namespace DebitoAutomatico.PS.Modulos.Servicio
{
    public partial class Reportes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Usuario objUsuario = new Usuario();
               // Session["usuario"] = "maikol.ramirez";

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

                this.ddlEstadoDebito.DataSource = new EstadosClientesLN().consultarEstado(new EstadosClientes());
                this.ddlEstadoDebito.DataTextField = "pValor";
                this.ddlEstadoDebito.DataValueField = "pId";
                this.ddlEstadoDebito.DataBind();
                UtilidadesWeb.agregarTodosDDL(this.ddlEstadoDebito);
            }
        }

        protected void imgBtnNuevo_Click(object sender, ImageClickEventArgs e)
        {
            limpiar();
        }

        protected void imgBtnReportes_Click(object sender, ImageClickEventArgs e)
        {

            if (this.txbFechaInicio.Text == String.Empty || this.txbFechaFinal.Text == String.Empty)
            {
                this.txbFechaInicio.Text = "01/01/1950";
                this.txbFechaFinal.Text = "31/12/2100";
            }

            switch (this.ddlReporte.SelectedValue)
            {
                case "1": //Clientes por tipo
                    Response.Redirect(ConfigurationManager.AppSettings["ServidorRP"].ToString() + "/DEB_Rpt_Clientes&Estado=" + this.ddlEstadoDebito.SelectedValue);
                    break;
                case "2": //Inconsistencia
                    Response.Redirect(ConfigurationManager.AppSettings["ServidorRP"].ToString() + "/DEB_Rpt_Cliente_Inconsistente&FecDesde=" + this.txbFechaInicio.Text + "&FecHasta=" + this.txbFechaFinal.Text);
                    break;
                case "3": //Historico de transacciones
                    Response.Redirect(ConfigurationManager.AppSettings["ServidorRP"].ToString() + "/DEB_Rpt_Historico_Transacciones&FecDesde=" + this.txbFechaInicio.Text + "&FecHasta=" + this.txbFechaFinal.Text + "&Contrato=" + this.txbContrato.Text);
                    break;
                case "4": //Logs por contrato
                    Response.Redirect(ConfigurationManager.AppSettings["ServidorRP"].ToString() + "/DEB_Rpt_Logs_Usuario&FecDesde=" + this.txbFechaInicio.Text + "&FecHasta=" + this.txbFechaFinal.Text + "&Contrato=" + this.txbContrato.Text);
                    break;
                case "5": //Logs por archivos
                    Response.Redirect(ConfigurationManager.AppSettings["ServidorRP"].ToString() + "/DEB_Rpt_Logs_Archivos&Banco=" + this.ddlBancoDebito.SelectedValue);
                    break;
                case "6": //Clientes actualizados
                    Response.Redirect(ConfigurationManager.AppSettings["ServidorRP"].ToString() + "/DEB_Rpt_Actualiza_Clientes");
                    break;
                case "7": //Correo enviados
                    Response.Redirect(ConfigurationManager.AppSettings["ServidorRP"].ToString() + "/DEB_Rpt_Correos&FecDesde=" + this.txbFechaInicio.Text + "&FecHasta=" + this.txbFechaFinal.Text + "&Contrato=" + this.txbContrato.Text + "&NombreArchivo=" + this.txbNombreArchivo.Text);
                    break;
                default:
                    limpiar();
                    break;
            }
        }

        protected void ddlReporte_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.ddlReporte.SelectedValue)
            {
                case "1": //Clientes por tipo
                    this.lblEstadoD.Visible = this.ddlEstadoDebito.Visible = true;
                    this.lblFechaInicio.Visible = this.lblFechaFin.Visible =
                    this.txbFechaInicio.Visible = this.txbFechaFinal.Visible =
                    this.imgFechaInicio.Visible = this.imgBtnFechaFinal.Visible =
                    this.lblContrato.Visible = this.txbContrato.Visible =
                    this.lblBanco.Visible = this.ddlBancoDebito.Visible =
                    this.lblNombreArchivo.Visible = this.txbNombreArchivo.Visible = false;
                    this.txbFechaInicio.Text = this.txbFechaFinal.Text = this.txbContrato.Text = String.Empty;
                    this.ddlBancoDebito.SelectedIndex = this.ddlEstadoDebito.SelectedIndex = 0;
                    break;
                case "2": //Inconsistencia
                    this.lblFechaInicio.Visible = this.lblFechaFin.Visible =
                    this.txbFechaInicio.Visible = this.txbFechaFinal.Visible =
                    this.imgFechaInicio.Visible = this.imgBtnFechaFinal.Visible = true;
                    this.lblEstadoD.Visible = this.ddlEstadoDebito.Visible =
                    this.lblContrato.Visible = this.txbContrato.Visible =
                    this.lblBanco.Visible = this.ddlBancoDebito.Visible =
                    this.lblNombreArchivo.Visible = this.txbNombreArchivo.Visible = false;
                    this.txbFechaInicio.Text = this.txbFechaFinal.Text = this.txbContrato.Text = String.Empty;
                    this.ddlBancoDebito.SelectedIndex = this.ddlEstadoDebito.SelectedIndex = 0;
                    break;
                case "3": //Historico de transacciones
                    this.lblFechaInicio.Visible = this.lblFechaFin.Visible =
                    this.txbFechaInicio.Visible = this.txbFechaFinal.Visible =
                    this.imgFechaInicio.Visible = this.imgBtnFechaFinal.Visible =
                    this.lblContrato.Visible = this.txbContrato.Visible = true;
                    this.ddlEstadoDebito.Visible = this.lblBanco.Visible =
                    this.ddlBancoDebito.Visible = this.lblEstadoD.Visible =
                    this.lblNombreArchivo.Visible = this.txbNombreArchivo.Visible = false;
                    this.txbFechaInicio.Text = this.txbFechaFinal.Text = this.txbContrato.Text = String.Empty;
                    this.ddlBancoDebito.SelectedIndex = this.ddlEstadoDebito.SelectedIndex = 0;
                    break;
                case "4": //Logs por contrato
                    this.lblFechaInicio.Visible = this.lblFechaFin.Visible =
                    this.txbFechaInicio.Visible = this.txbFechaFinal.Visible =
                    this.imgFechaInicio.Visible = this.imgBtnFechaFinal.Visible =
                    this.lblContrato.Visible = this.txbContrato.Visible = true;
                    this.ddlEstadoDebito.Visible = this.lblBanco.Visible =
                    this.ddlBancoDebito.Visible = this.lblEstadoD.Visible =
                    this.lblNombreArchivo.Visible = this.txbNombreArchivo.Visible = false;
                    this.txbFechaInicio.Text = this.txbFechaFinal.Text = this.txbContrato.Text = String.Empty;
                    this.ddlBancoDebito.SelectedIndex = this.ddlEstadoDebito.SelectedIndex = 0;
                    break;
                case "5": //Logs por archivos
                    this.lblBanco.Visible = this.ddlBancoDebito.Visible = true;
                    this.lblFechaInicio.Visible = this.lblFechaFin.Visible =
                    this.txbFechaInicio.Visible = this.txbFechaFinal.Visible =
                    this.imgFechaInicio.Visible = this.imgBtnFechaFinal.Visible =
                    this.lblEstadoD.Visible = this.ddlEstadoDebito.Visible =
                    this.lblContrato.Visible = this.txbContrato.Visible =
                    this.lblNombreArchivo.Visible = this.txbNombreArchivo.Visible = false;
                    this.txbFechaInicio.Text = this.txbFechaFinal.Text = this.txbContrato.Text = String.Empty;
                    this.ddlBancoDebito.SelectedIndex = this.ddlEstadoDebito.SelectedIndex = 0;
                    break;
                case "6":  //Clientes actualizados
                    this.ddlReporte.Enabled = true;
                    this.lblBanco.Visible = this.ddlBancoDebito.Visible =
                    this.lblFechaInicio.Visible = this.lblFechaFin.Visible =
                    this.txbFechaInicio.Visible = this.txbFechaFinal.Visible =
                    this.imgFechaInicio.Visible = this.imgBtnFechaFinal.Visible =
                    this.lblEstadoD.Visible = this.ddlEstadoDebito.Visible =
                    this.lblContrato.Visible = this.txbContrato.Visible =
                    this.lblNombreArchivo.Visible = this.txbNombreArchivo.Visible = false;
                    this.txbFechaInicio.Text = this.txbFechaFinal.Text = this.txbContrato.Text = String.Empty;
                    this.ddlBancoDebito.SelectedIndex = this.ddlEstadoDebito.SelectedIndex = 0;
                    break;
                case "7":  //Envio_Correo
                    this.lblFechaInicio.Visible = this.lblFechaFin.Visible =
                    this.txbFechaInicio.Visible = this.txbFechaFinal.Visible =
                    this.lblContrato.Visible = this.txbContrato.Visible =
                    this.lblNombreArchivo.Visible = this.txbNombreArchivo.Visible =
                    this.imgFechaInicio.Visible = this.imgBtnFechaFinal.Visible = true;
                    this.ddlEstadoDebito.Visible = this.lblBanco.Visible =
                    this.lblEstadoD.Visible = this.ddlEstadoDebito.Visible =
                    this.lblBanco.Visible = this.ddlBancoDebito.Visible = false;
                    this.txbFechaInicio.Text = this.txbFechaFinal.Text = this.txbContrato.Text = String.Empty;
                    this.ddlBancoDebito.SelectedIndex = this.ddlEstadoDebito.SelectedIndex = 0;
                    break;
                default:
                    limpiar();
                    break;
            }
        }

        public void limpiar()
        {
            this.lblFechaInicio.Visible = this.lblFechaFin.Visible =
            this.txbFechaInicio.Visible = this.txbFechaFinal.Visible =
            this.imgFechaInicio.Visible = this.imgBtnFechaFinal.Visible =
            this.lblEstadoD.Visible = this.ddlEstadoDebito.Visible = this.ddlBancoDebito.Visible =
            this.lblContrato.Visible = this.txbContrato.Visible = this.lblBanco.Visible = false;
            this.txbFechaInicio.Text = this.txbFechaFinal.Text = this.txbContrato.Text = String.Empty;
            this.ddlBancoDebito.SelectedIndex = this.ddlEstadoDebito.SelectedIndex = this.ddlReporte.SelectedIndex = 0;
        }
    }
}