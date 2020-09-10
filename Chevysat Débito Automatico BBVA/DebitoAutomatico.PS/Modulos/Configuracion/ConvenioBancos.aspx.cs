using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DebitoAutomatico.PS.Codigo;
using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;
using DebitoAutomatico.LN.Consultas;
using DebitoAutomatico.EN.Definicion;

/// <summary>
/// Autor: Nicolas Alexander Larrotta
/// Fecha Ultima Actualización: 20 de Junio de 2018
/// Observacion: Modulo Convenio Bancos
/// </summary>


namespace DebitoAutomatico.PS.Modulos.Configuracion
{
    public partial class ConvenioBancos : System.Web.UI.Page
    {
        Banco objB = new Banco();
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

        /// <summary>
        /// Carga los elementos en la pagina de inicio 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                Usuario objUsuario = new Usuario();

                //Session["usuario"] = "maikol.ramirez";
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

                objB.pDebito = true;
                objB.pActivo = true;
                this.ddlBancoDebito.DataSource = new BancoLN().consultarBanco(objB);
                this.ddlBancoDebito.DataTextField = "pNombre";
                this.ddlBancoDebito.DataValueField = "pId";
                this.ddlBancoDebito.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlBancoDebito);

                this.ddlTipoProceso.DataSource = new EstadosClientesLN().consultarTipoEstado(new EstadosClientes());
                this.ddlTipoProceso.DataTextField = "pValor";
                this.ddlTipoProceso.DataValueField = "pId";
                this.ddlTipoProceso.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlTipoProceso);
            }
        }

        /// <summary>
        /// Bloque la lista del banco a debitar y habilita el tipo de proceso que se va a realizar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlBancoDebito_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddlBancoDebito.Enabled = false;
            this.ddlTipoProceso.Enabled = true;
        }

        /// <summary>
        /// Carga la pantalla de nuevo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgBtnNuevo_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Modulos/Configuracion/ConvenioBancos.aspx?tv=dba%2fcg%2fccb");            
        }

        /// <summary>
        /// Redirecciona al metodo administrarConvenio para guardar los convenios marcados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgBtnGuardar_Click(object sender, ImageClickEventArgs e)
        {
            administrarConvenio();
        }

        /// <summary>
        /// Metodo que realiza la consulta de los convenios
        /// </summary>
        private void buscar()
        {
            objB.pActivo = true;
            this.gvConvenios.DataSource = new BancoLN().consultarBanco(objB);
            this.gvConvenios.DataBind();
            this.btnSeleccion.Visible = true;       
            Convenio objC = new Convenio();
            objC.pIdBancoDebito = Convert.ToInt32(this.ddlBancoDebito.SelectedValue);
            objC.pIdProceso = Convert.ToInt32(this.ddlTipoProceso.SelectedValue);

            if (objC.pIdProceso == 1)
            {
                objC.pIdPrenota = "S";
                objC.pOperacion = TiposConsultas.CONSULTAR;
            }
            else if (objC.pIdProceso == 4)
            {
                objC.pOperacion = TiposConsultas.CONSULTAR_2;
                objC.pIdDebito = "S";
            }

            ConveniosBancosDebito = new ConvenioLN().consultar(objC);


            foreach (GridViewRow dtgItem in this.gvConvenios.Rows)
            {
                foreach (Convenio objBal in ConveniosBancosDebito)
                {
                    if (objBal.pIdBanco == Convert.ToInt32(gvConvenios.Rows[dtgItem.RowIndex].Cells[0].Text))
                    {
                        ((CheckBox)gvConvenios.Rows[dtgItem.RowIndex].FindControl("chbConvenio")).Checked = true;
                    }
                }
            }
        }

        /// <summary>
        /// Guarda o borra los convenios de acuerdo a los que estan marcados
        /// </summary>
        private void administrarConvenio()
        {
            foreach (GridViewRow dtgItem in this.gvConvenios.Rows)
            {

                if (((CheckBox)gvConvenios.Rows[dtgItem.RowIndex].FindControl("chbConvenio")).Checked == true)
                {
                    bool esta = false;
                    foreach (Convenio objBal in ConveniosBancosDebito)
                    {
                        if (objBal.pIdBanco == Convert.ToInt32(gvConvenios.Rows[dtgItem.RowIndex].Cells[0].Text))
                        {
                            esta = true;
                        }
                    }
                    if (esta == false)
                    {
                        insertar(Convert.ToInt32(gvConvenios.Rows[dtgItem.RowIndex].Cells[0].Text));
                    }
                }
                else
                {
                    bool esta = false;
                    foreach (Convenio objBal in ConveniosBancosDebito)
                    {
                        if (objBal.pIdBanco == Convert.ToInt32(gvConvenios.Rows[dtgItem.RowIndex].Cells[0].Text))
                        {
                            esta = true;
                        }
                    }
                    if (esta == true)
                    {
                        borrar(Convert.ToInt32(gvConvenios.Rows[dtgItem.RowIndex].Cells[0].Text));
                    }
                }                
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('"+RecursosDebito.ProcesoExistoso+"');</script>", false);                                                           
            limpiar();
            this.ddlBancoDebito.SelectedIndex = this.ddlTipoProceso.SelectedIndex = 0;
            this.ddlBancoDebito.Enabled = true;
            this.ddlTipoProceso.Enabled = false;
        }

        /// <summary>
        /// Metodo que guarda el convenio en la base de datos
        /// </summary>
        /// <param name="IdBanco"></param>
        private void insertar(int IdBanco)
        {
            Convenio objC = new Convenio();
            objC.pIdBanco = IdBanco;
            objC.pIdBancoDebito = Convert.ToInt32(this.ddlBancoDebito.SelectedValue);
            objC.pIdProceso = Convert.ToInt32(this.ddlTipoProceso.SelectedValue);
            if (objC.pIdProceso == 1)
            {
                objC.pIdPrenota = "S";
                objC.pIdDebito = "N";
            }
            else if (objC.pIdProceso == 4)
            {
                objC.pIdDebito = "S";
                objC.pIdPrenota = "N";
            }

            int valorC = 0;
            valorC = new ConvenioLN().insertar(objC);            
        }

        /// <summary>
        /// Metodo que borra el convenio de la base de datos
        /// </summary>
        /// <param name="IdBanco"></param>
        private void borrar(int IdBanco)
        {
            Convenio objC = new Convenio();
            objC.pIdBanco = IdBanco;
            objC.pIdBancoDebito = Convert.ToInt32(this.ddlBancoDebito.SelectedValue);

            int valorC = 0;
            valorC = new ConvenioLN().borrar(objC);
        }

        /// <summary>
        /// Limpia los elementos de la pantalla
        /// </summary>
        private void limpiar()
        {
            this.gvConvenios.DataSource = null;
            this.gvConvenios.DataBind();
            ConveniosBancosDebito = null;
            this.btnSeleccion.Visible = false;
        }

        /// <summary>
        /// Selecciona el proceso que se va a realizar para ir al metodo buscar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipoProceso_SelectedIndexChanged(object sender, EventArgs e)
        {
            limpiar();
            if (this.ddlTipoProceso.SelectedIndex != 0)
            {
                buscar();
            }
        }

        /// <summary>
        /// Marca o desmarca todos los convenios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSeleccion_Click(object sender, EventArgs e)
        {
            bool marcado = true;

            foreach (GridViewRow dtgItem in this.gvConvenios.Rows)
            {
                if (((CheckBox)gvConvenios.Rows[dtgItem.RowIndex].FindControl("chbConvenio")).Checked == true)
                    marcado = false;
            }

            foreach (GridViewRow dtgItem in this.gvConvenios.Rows)
            {
                ((CheckBox)gvConvenios.Rows[dtgItem.RowIndex].FindControl("chbConvenio")).Checked = marcado;
            }
        }

    }
}