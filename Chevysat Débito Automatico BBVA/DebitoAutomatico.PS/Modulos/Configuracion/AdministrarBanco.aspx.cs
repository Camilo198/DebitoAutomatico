using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;
using DebitoAutomatico.LN.Consultas;
using DebitoAutomatico.PS.Codigo;
using System.Globalization;

/// <summary>
/// Autor: Nicolas Alexander Larrotta
/// Fecha Ultima Actualización: 20 de Junio de 2018
/// Observacion: Modulo Administración de Bancos
/// </summary>

namespace DebitoAutomatico.PS.Modulos.Configuracion
{
    public partial class AdministrarBanco : System.Web.UI.Page
    {

        private int IdBanco
        {
            get
            {
                int IdBa = 0;
                if (ViewState["IdBanco"] != null)
                    IdBa = Convert.ToInt32(ViewState["IdBanco"]);
                return IdBa;
            }

            set
            {
                ViewState["IdBanco"] = value;
            }
        }

        private int IdFiducias
        {
            get
            {
                int IdFi = 0;
                if (ViewState["IdFiducias"] != null)
                    IdFi = Convert.ToInt32(ViewState["IdFiducias"]);
                return IdFi;
            }

            set
            {
                ViewState["IdFiducias"] = value;
            }
        }


        private int IdDebitoParcial
        {
            get
            {
                int IdRE = 0;
                if (ViewState["IdDebitoParcial"] != null)
                    IdRE = Convert.ToInt32(ViewState["IdDebitoParcial"]);
                return IdRE;
            }

            set
            {
                ViewState["IdDebitoParcial"] = value;
            }
        }

        private List<EN.Tablas.Banco> BancosEncontrados
        {
            get
            {
                List<EN.Tablas.Banco> lista = new List<EN.Tablas.Banco>();
                if (ViewState["BancosEncontrados"] != null)
                    lista = (List<EN.Tablas.Banco>)ViewState["BancosEncontrados"];
                return lista;
            }

            set
            {
                ViewState["BancosEncontrados"] = value;
            }
        }

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

        protected void gvBusquedaBanco_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string _activo = DataBinder.Eval(e.Row.DataItem, "pactivo").ToString();

                if (_activo == "True")
                    e.Row.Cells[3].Text = "Activo";
                else
                    e.Row.Cells[3].Text = "Inactivo";
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
        }

        /// <summary>
        /// Se encarga de guardar y actualizar la informacion del banco 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgBtnGuardar_Click(object sender, ImageClickEventArgs e)
        {
            guardar();
        }

        /// <summary>
        /// Limpia todos los campos de la pantalla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgBtnNuevo_Click(object sender, ImageClickEventArgs e)
        {
            limpiar();
            this.txbCodigoBanco.Text = String.Empty;
        }

        /// <summary>
        /// Realiza la busqueda del banco por codigo, si es verdadero lo hace por la accion del texbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txbCodigoBanco_TextChanged(object sender, EventArgs e)
        {
            buscar(true);
        }

        /// <summary>
        /// Agrega los correos en el campo CorreoControl
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgAgregarCorreoControl_Click(object sender, ImageClickEventArgs e)
        {
            if (this.txbCorreoControl.Text != string.Empty)
            {
                this.LtbCorreoControl.Items.Add(this.txbCorreoControl.Text);
                this.txbCorreoControl.Text = string.Empty;
            }
        }

        /// <summary>
        /// Borra el correo de la posicion que se seleccion del campo CorreoControl
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgBorrarCorreoControl_Click(object sender, ImageClickEventArgs e)
        {
            if (this.LtbCorreoControl.SelectedIndex != -1)
            {
                this.LtbCorreoControl.Items.Remove(this.LtbCorreoControl.SelectedItem);
            }
        }

        /// <summary>
        /// Agrega los correos en el campo CorreoEnvio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgAgregarCorreoEnvio_Click(object sender, ImageClickEventArgs e)
        {
            if (this.txbCorreoEnvio.Text != string.Empty)
            {
                this.LtbCorreoEnvio.Items.Add(this.txbCorreoEnvio.Text);
                this.txbCorreoEnvio.Text = string.Empty;
            }
        }

        /// <summary>
        /// Borra el correo de la posicion que se seleccion del campo CorreoEnvio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgBorrarCorreoEnvio_Click(object sender, ImageClickEventArgs e)
        {
            if (this.LtbCorreoEnvio.SelectedIndex != -1)
            {
                this.LtbCorreoEnvio.Items.Remove(this.LtbCorreoEnvio.SelectedItem);
            }
        }

        /// <summary>
        /// Realiza la busqueda del banco desde el pop-up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            buscar(false);
            this.btnLimpiar.Enabled = true;
        }

        /// <summary>
        /// Boton que limpia el pop-up para la busqueda nueva del banco
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevaBusqueda_Click(object sender, EventArgs e)
        {
            this.btnLimpiar.Enabled = false;
            this.txbCodigoBancoB.Text = this.txbNombreBancoB.Text = String.Empty;
            this.gvBusquedaBanco.DataSource = null;
            this.gvBusquedaBanco.DataBind();
            this.mpeBusquedaBanco.Show();
        }

        /// <summary>
        /// Limipa los campos del pop-up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            this.btnLimpiar.Enabled = false;
            this.gvBusquedaBanco.DataSource = null;
            this.gvBusquedaBanco.DataBind();
            this.mpeBusquedaBanco.Show();
        }

        /// <summary>
        /// Cierra el pop-up para volver a la pantalla inicial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //inicializarPanelB();
        }

        /// <summary>
        /// Carga la informacion del banco que se selecciono
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvBusquedaBanco_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("sel"))
            {
                this.txbCodigoBanco.Text = HttpUtility.HtmlDecode(this.gvBusquedaBanco.Rows[Convert.ToInt32(e.CommandArgument)].Cells[1].Text);
                buscar(true);
            }
        }

        /// <summary>
        /// Cambio de pagina del gridview del pop-up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvBusquedaBanco_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvBusquedaBanco.PageIndex = e.NewPageIndex;
            this.gvBusquedaBanco.DataSource = BancosEncontrados;
            this.gvBusquedaBanco.DataBind();
            this.mpeBusquedaBanco.Show();
        }

        /// <summary>
        /// Metodo para guardar el banco
        /// </summary>
        private void guardar()
        {
            try
            {

                Banco objB = new Banco();
                objB.pCodigo = this.txbCodigoBanco.Text.Trim();
                objB.pNombre = this.txbNombreBanco.Text.Trim();
                objB.pNit = this.txbNit.Text.Trim();
                objB.pActivo = this.chbEstaActivo.Checked;
                objB.pDebito = this.chbBancoDeb.Checked;

                String CorreoControlG = string.Empty;
                foreach (object itemCorreoC in this.LtbCorreoControl.Items)
                {
                    CorreoControlG = CorreoControlG + itemCorreoC + ";";
                }
                String CorreoEnvioG = string.Empty;
                foreach (object itemCorreoE in this.LtbCorreoEnvio.Items)
                {
                    CorreoEnvioG = CorreoEnvioG + itemCorreoE + ";";
                }
                objB.pCorreoControl = CorreoControlG;
                objB.pCorreoEnvio = CorreoEnvioG;

                objB.pRemitente = this.txbRemitente.Text.Trim();

                int valor = 0;

                if ((IdBanco == 0) && (IdDebitoParcial == 0))
                {
                    int valRuta = 0;

                    objB.pIdRuta = valRuta;

                    valor = new BancoLN().insertar(objB);

                    if (chbDebitoParcial.Checked)
                    {
                        DebitoParcial objDP = new DebitoParcial();
                        objDP.pIdBanco = valor;
                        objDP.pValorParcial = txbValorParcial.Text.Replace(",","");
                        int val = new DebitoParcialLN().insertar(objDP);
                    }
                }
                else
                {
                    objB.pId = IdBanco;
                    valor = new BancoLN().actualizar(objB);
                    int val = 0;
                    DebitoParcial objDP = new DebitoParcial();
                    objDP.pId = IdDebitoParcial;
                    objDP.pIdBanco = IdBanco;
                    objDP.pValorParcial = txbValorParcial.Text.Replace(",", "");

                    if ((chbDebitoParcial.Checked) && (IdDebitoParcial == 0))
                    {
                        val = new DebitoParcialLN().insertar(objDP);
                    }
                    else
                    {
                        val = new DebitoParcialLN().actualizar(objDP);
                    }
                }

                if ((chbDebitoParcial.Checked == false) && (IdDebitoParcial > 0))
                {
                    DebitoParcial objDB = new DebitoParcial();
                    objDB.pId = IdDebitoParcial;
                    objDB.pIdBanco = IdBanco;
                    objDB.pValorParcial = this.txbValorParcial.Text.Replace(",", "");
                    new DebitoParcialLN().borrar(objDB);
                }

                if (valor > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ProcesoExistoso + "');</script>", false);
                    limpiar();
                    this.txbCodigoBanco.Text = String.Empty;
                }
                else
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorProceso + "');</script>", false);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + ex.Message.Replace("'", "").Replace("\n", "").Replace("\r", "") + "');</script>", false);
            }
        }

        /// <summary>
        /// Metodo que realiza la busqueda del banco en la base de datos
        /// </summary>
        /// <param name="esBusqPorTxb"></param>
        private void buscar(bool esBusqPorTxb)
        {
            try
            {
                limpiar();
                EN.Tablas.Banco objB = new EN.Tablas.Banco();
                BancoLN objBancoLN = new BancoLN();

                if (esBusqPorTxb) //Busqueda por texbox
                {
                    objB.pCodigo = this.txbCodigoBanco.Text.Trim();
                    List<EN.Tablas.Banco> listaB = objBancoLN.consultarBanco(objB);
                    if (listaB.Count > 0)
                    {
                        objB = listaB[0];
                        IdBanco = objB.pId.Value;

                        if (objB.pDebito == true)
                        {
                            this.lblNit.Visible = this.txbNit.Visible = true;
                        }
                        else
                        {
                            this.lblNit.Visible = this.txbNit.Visible = false;
                        }

                        this.txbCodigoBanco.Text = objB.pCodigo;
                        this.txbNombreBanco.Text = objB.pNombre;
                        this.txbNit.Text = objB.pNit;

                        this.chbEstaActivo.Checked = objB.pActivo.Value;
                        this.chbBancoDeb.Checked = objB.pDebito.Value;
                        string[] CorreoControlB = objB.pCorreoControl.Split(';');
                        string[] CorreoEnvioB = objB.pCorreoEnvio.Split(';');
                        this.LtbCorreoControl.Items.Clear();
                        this.LtbCorreoEnvio.Items.Clear();
                        foreach (string cc in CorreoControlB)
                        {
                            if (string.IsNullOrEmpty(cc)) break;
                            this.LtbCorreoControl.Items.Add(cc);
                        }
                        foreach (string ce in CorreoEnvioB)
                        {
                            if (string.IsNullOrEmpty(ce)) break;
                            this.LtbCorreoEnvio.Items.Add(ce);
                        }
                        this.txbRemitente.Text = objB.pRemitente;

                        DebitoParcial objDP = new DebitoParcial();
                        objDP.pIdBanco = IdBanco;
                        List<EN.Tablas.DebitoParcial> listaDP = new DebitoParcialLN().consultar(objDP);

                        if (listaDP.Count > 0)
                        {
                            objDP = listaDP[0];
                            IdDebitoParcial = Convert.ToInt32(objDP.pId);
                            this.chbDebitoParcial.Checked = this.lblMonto.Visible = this.txbValorParcial.Visible = true;
                            ConvertMonedad(objDP.pValorParcial);
                        }

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.EntidadBancaria + " " + objB.pNombre + ".');</script>", false);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.CrearBanco + " " + objB.pCodigo + ".');</script>", false);
                        limpiar();
                    }
                    bancoDebito();
                }
                else //Busqueda por el boton buscar
                {
                    if (!String.IsNullOrEmpty(this.txbCodigoBancoB.Text.Trim()))
                        objB.pCodigo = this.txbCodigoBancoB.Text.Trim();

                    if (!String.IsNullOrEmpty(this.txbNombreBancoB.Text.Trim()))
                        objB.pNombre = this.txbNombreBancoB.Text.Trim();

                    BancosEncontrados = objBancoLN.consultarBanco(objB);

                    this.gvBusquedaBanco.DataSource = BancosEncontrados;
                    this.gvBusquedaBanco.DataBind();
                    mpeBusquedaBanco.Show();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + ex.Message.Replace("'", "").Replace("\n", "").Replace("\r", "") + "');</script>", false);
            }
        }

        /// <summary>
        /// Metodo para eliminar el banco de la base de datos
        /// </summary>
        private void eliminar()
        {
            try
            {
                if ((IdBanco != 0))
                {
                    Banco objB = new Banco();
                    objB.pId = IdBanco;
                    int valor = 0;
                    valor = new BancoLN().borrar(objB);

                    DebitoParcial objDB = new DebitoParcial();
                    objDB.pId = IdDebitoParcial;
                    objDB.pIdBanco = IdBanco;
                    objDB.pValorParcial = this.txbValorParcial.Text;
                    new DebitoParcialLN().borrar(objDB);

                    if (valor == 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.BorrarBanco + "');</script>", false);
                        limpiar();
                        this.txbCodigoBanco.Text = String.Empty;
                    }
                    else
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorProceso + "');</script>", false);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + ex.Message.Replace("'", "").Replace("\n", "").Replace("\r", "") + "');</script>", false);
            }
        }

        /// <summary>
        /// Metodo que se encarga de limpiar los elementos de la pantalla
        /// </summary>
        private void limpiar()
        {
            IdBanco = IdDebitoParcial = 0;
            this.txbRemitente.Text = this.txbCorreoControl.Text = this.txbCorreoEnvio.Text = this.txbValorParcial.Text =
            this.txbNombreBanco.Text = this.txbNit.Text = String.Empty;
            this.LtbCorreoEnvio.Items.Clear();
            this.LtbCorreoControl.Items.Clear();
            this.txbValorParcial.Visible = this.lblMonto.Visible = this.chbDebitoParcial.Checked = this.chbEstaActivo.Checked = this.chbBancoDeb.Checked = false;
            this.gvBusquedaBanco.DataSource = null;
            this.gvBusquedaBanco.DataBind();
        }

        /// <summary>
        /// Check para habilitar o deshabilitar el banco
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chbBancoDeb_CheckedChanged(object sender, EventArgs e)
        {
            bancoDebito();
        }

        /// <summary>
        /// Metodo que se encarga de validar el check de habilitado o no el banco
        /// </summary>
        private void bancoDebito()
        {
            if (this.chbBancoDeb.Checked)
            {
                this.lblNit.Visible = this.txbNit.Visible = true;
                this.rfvRemitente.ValidationGroup = "1";
                
            }
            else
            {
                this.rfvRemitente.ValidationGroup = "2";
                this.lblNit.Visible = this.txbNit.Visible = false;
            }
        }

        /// <summary>
        /// Valida si el banco tiene convenios para deshabilitarlo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chbEstaActivo_CheckedChanged(object sender, EventArgs e)
        {
            Convenio objC = new Convenio();

            objC.pIdBanco = IdBanco;
            objC.pOperacion = TiposConsultas.CONSULTAR;

            ConveniosBancosDebito = new ConvenioLN().consultar(objC);

            if (ConveniosBancosDebito.Count == 1)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ExisteConvenio + "');</script>", false);
                chbEstaActivo.Checked = true;
            }
        }

        protected void chbDebitoParcial_CheckedChanged(object sender, EventArgs e)
        {
            if (chbDebitoParcial.Checked)
            {
                this.txbValorParcial.Visible = this.lblMonto.Visible = true;
            }
            else
            {
                this.txbValorParcial.Visible = this.lblMonto.Visible = false;
            }
        }

        public void ConvertMonedad(String ValorConvert)
        {
            double valor = Convert.ToDouble(ValorConvert);
            this.txbValorParcial.Text = valor.ToString("0,0", CultureInfo.InvariantCulture);
            this.txbValorParcial.Text = String.Format(CultureInfo.InvariantCulture, "{0:0,0}", valor);
        }
        

        protected void txbValorParcial_TextChanged(object sender, EventArgs e)
        {
            ConvertMonedad(this.txbValorParcial.Text);
        }
    }
}