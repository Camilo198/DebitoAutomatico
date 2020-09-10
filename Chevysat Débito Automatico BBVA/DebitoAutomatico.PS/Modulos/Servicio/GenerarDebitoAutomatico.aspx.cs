using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DebitoAutomatico.PS.Codigo;
using DebitoAutomatico.PS.Codigo.Prenota;
using DebitoAutomatico.PS.Codigo.Debito;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;
using DebitoAutomatico.LN.Consultas;
using DebitoAutomatico.EN.Definicion;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Text.RegularExpressions;

/// <summary>
/// Autor: Nicolas Alexander Larrotta
/// Fecha Ultima Actualización: 20 de Junio de 2018
/// Observacion: Modulo Generar Debito Automatico
/// </summary>


namespace DebitoAutomatico.PS.Modulos.Servicio
{
    public partial class GenerarArchivos : System.Web.UI.Page
    {
        Convenio objC = new Convenio();

        private List<Convenio> ConveniosBancosDebito
        {
            get
            {
                List<Convenio> lista = new List<Convenio>();
                if (ViewState["ConveniosBancosDebito"] != null)
                    lista = (List<Convenio>)ViewState["ConveniosBancosDebito"];
                return lista;
            }

            set
            {
                ViewState["ConveniosBancosDebito"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Usuario objUsuario = new Usuario();
                Session["usuario"] = "cristian.munoz";
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

        private void limpiar()
        {
            this.ddlBancoDebito.SelectedIndex = this.ddlMesadebitar.SelectedIndex = 0;
            this.chbAdjudicados.Checked = this.chbGanadores.Checked =
            this.chbSuscriptores.Checked = this.chbCuotasxDevolver.Checked = false;
            this.txbFechaInicial.Text = String.Empty;
        }

        /// <summary>
        /// Carga los elementos de la pantalla de nuevo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgBtnNuevo_Click(object sender, ImageClickEventArgs e)
        {
            limpiar();
        }

        protected void btnGenerar_Click(object sender, EventArgs e)
        {

            HistorialArchivos objHa = new HistorialArchivos();
            objHa.pFecha = Convert.ToDateTime(this.txbFechaInicial.Text).ToString("dd/MM/yyyy");
            objHa.pCodigoBanco = this.ddlBancoDebito.SelectedValue;
            objHa.pEstado = "V";
            objHa.pTipoArchivo = "GDA";
            objHa.pManual = false;
            List<EN.Tablas.HistorialArchivos> listHa = new HistorialArchivosLN().consultar(objHa);

            if (listHa.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ArchivoGenerado + this.ddlBancoDebito.SelectedItem.Text + " ');</script>", false);
                return;
            }

            if (this.chbSuscriptores.Checked == false && this.chbGanadores.Checked == false && this.chbCuotasxDevolver.Checked == false && this.chbAdjudicados.Checked == false)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('"+RecursosDebito.TiposContrato+"');</script>", false);
                return;
            }

            objC.pIdBancoDebito = Convert.ToInt32(this.ddlBancoDebito.SelectedValue);
            objC.pIdDebito = "S";
            objC.pOperacion = TiposConsultas.CONSULTAR;
            ConveniosBancosDebito = new ConvenioLN().consultar(objC);

            ArrayList codBancos = new ArrayList();

            foreach (Convenio objBal in ConveniosBancosDebito)
            {
                if (objBal.pIdDebito == "S")
                {
                    codBancos.Add(objBal.pIdBanco);
                }
            }


            
            if (!String.IsNullOrEmpty(this.txbFechaInicial.Text))
            {
                if (DateTime.Now > Convert.ToDateTime(this.txbFechaInicial.Text).AddHours(24))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('Fecha Incorrecta!!!');</script>", false);
                    return;
                }
            }

            ArrayList men = new ArrayList();
            GenerarDebito debito = new GenerarDebito();
            String FechaProceso = Convert.ToString(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            men = debito.ServicioDebito(Convert.ToInt32(this.ddlBancoDebito.SelectedValue), this.ddlBancoDebito.SelectedItem.ToString(), Session["usuario"].ToString(), codBancos,
                                      this.chbSuscriptores.Checked, this.chbAdjudicados.Checked, this.chbGanadores.Checked, this.chbCuotasxDevolver.Checked, this.txbAnoDebito.Text.Substring(2, 2), this.ddlMesadebitar.SelectedValue,
                                      this.txbFechaInicial.Text,
                                      false, new Dictionary<String, String>(), FechaProceso);

            foreach (String Mensaje in men)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + Mensaje + "');</script>", false);
            }

            limpiar();
        }
    }
}