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


/// <summary>
/// Autor: Nicolas Alexander Larrotta
/// Fecha Ultima Actualización: 20 de Junio de 2018
/// Observacion: Modulo Respuesta Transacciones
/// </summary>

namespace DebitoAutomatico.PS.Modulos.Configuracion
{

    public partial class RespuestaTransacciones : System.Web.UI.Page
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

        private int IdRespuesta
        {
            get
            {
                int IdRe = 0;
                if (ViewState["IdRespuesta"] != null)
                    IdRe = Convert.ToInt32(ViewState["IdRespuesta"]);
                return IdRe;
            }

            set
            {
                ViewState["IdRespuesta"] = value;
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

        private DataTable Respuestas
        {
            get
            {
                DataTable tabla = new DataTable();
                if (ViewState["Respuestas"] != null)
                    tabla = (DataTable)ViewState["Respuestas"];
                return tabla;
            }

            set
            {
                ViewState["Respuestas"] = value;
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
                this.ddlEstadoD.DataSource = new EstadosClientesLN().consultarEstado(new EstadosClientes());
                this.ddlEstadoP.DataSource = new EstadosClientesLN().consultarEstado(new EstadosClientes());
                this.ddlEstadoD.DataTextField = this.ddlEstadoP.DataTextField = "pValor";
                this.ddlEstadoD.DataValueField = this.ddlEstadoP.DataValueField = "pId";
                this.ddlEstadoD.DataBind();
                this.ddlEstadoP.DataBind();

                UtilidadesWeb.agregarSeleccioneDDL(this.ddlEstadoD);
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlEstadoP);
            }

        }

        /// <summary>
        /// Caja de texto donde se ingresa el codigo del banco para ir al metodo buscar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txbCodigoBanco_TextChanged(object sender, EventArgs e)
        {
            buscar(true);
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
        /// Limpia todos los campos de la pantalla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgBtnNuevo_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Modulos/Configuracion/RespuestaTransacciones.aspx?tv=dba%2fcg%2fcrt");
        }

        /// <summary>
        /// Metodo que realiza la busqueda del banco en la base de datos
        /// </summary>
        /// <param name="esBusqPorTxb"></param>
        private void buscar(bool esBusqPorTxb)
        {            
            EN.Tablas.Banco objB = new EN.Tablas.Banco();
            BancoLN objBancoLN = new BancoLN();

            if (esBusqPorTxb)
            {
                objB.pActivo = true;
                objB.pDebito = true; 
                objB.pCodigo = this.txbCodigoBanco.Text.Trim();
                List<EN.Tablas.Banco> listaB = objBancoLN.consultarBanco(objB);
                if (listaB.Count > 0)
                {
                    objB = listaB[0];
                    IdBanco = objB.pId.Value;
                    this.txbCodigoBanco.Text = objB.pCodigo;
                    this.txbNombreBanco.Text = objB.pNombre;
                    llenarGrillaCampos();
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.EntidadBancaria + " " + objB.pNombre + ".');</script>", false);                    
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('"+RecursosDebito.BancoNoExiste+"');</script>", false);
                    limpiar();
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(this.txbCodigoBancoB.Text.Trim()))
                    objB.pCodigo = this.txbCodigoBancoB.Text.Trim();

                if (!String.IsNullOrEmpty(this.txbNombreBancoB.Text.Trim()))
                    objB.pNombre = this.txbNombreBancoB.Text.Trim();
                objB.pActivo = true;
                objB.pDebito = true; 
                BancosEncontrados = objBancoLN.consultarBanco(objB);
                this.gvBusquedaBanco.DataSource = BancosEncontrados;
                this.gvBusquedaBanco.DataBind();

                mpeBusquedaBanco.Show();
            }
        }

        /// <summary>
        /// Metodo que se encarga de limpiar los elementos de la pantalla
        /// </summary>
        private void limpiar()
        {
            this.gvRespuestas.DataSource = null;
            this.gvRespuestas.DataBind();
            this.txbNombreBanco.Text = this.txbCodigoBanco.Text = String.Empty;
             
            IdBanco = 0;
            limpiarCampos();
        }

        /// <summary>
        /// Boton que se direcciona al metodo agregarCampo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgBtnAddField_Click(object sender, ImageClickEventArgs e)
        {
            agregarCampo();
        }

        /// <summary>
        /// Metodo que agrega o actualiza la respuesta
        /// </summary>
        private void agregarCampo()
        {
            RespuestaTransaccion objRT = new RespuestaTransaccion();
            objRT.pCodigo  = this.txbCodigo.Text;
            objRT.pRespuesta = this.txbRespuesta.Text;
            objRT.pBanco = IdBanco;
            objRT.pEstadoRespuesta = this.ddlEstadoRespuesta.SelectedItem.Text;
            objRT.pEstadoPrenota =  Convert.ToInt32(this.ddlEstadoP.SelectedValue);
            objRT.pEstadoDebito  = Convert.ToInt32(this.ddlEstadoD.SelectedValue);
            objRT.pEnvioCorreo = Convert.ToInt32(this.DDLEnvioC.SelectedValue.Equals("SI") ? "1" : "0");
            
            int valor = 0;
            RespuestaTransaccionLN objEA = new RespuestaTransaccionLN();
            String add = "actualizado";
            if (IdRespuesta > 0)
            {
                objRT.pId = IdRespuesta;
                valor = objEA.actualizar(objRT);
            }
            else
            {
                valor = objEA.insertar(objRT);
                add = "agregado";
            }
            if (valor > 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('Se ha " + add + " la Respuesta con Codigo " + objRT.pCodigo + ".');</script>", false);                
                limpiarCampos();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorCodigo + " " + objRT.pCodigo + ".');</script>", false);                
            }
        }

        /// <summary>
        /// Metodo que se ecarga de cargar el gridview de nuevo
        /// </summary>
        private void limpiarCampos()
        {
            llenarGrillaCampos();
            imgBtnAddField.ImageUrl = "~/MarcaVisual/iconos/agregar.png";
        }

        /// <summary>
        /// Se encarga de editar el registro del gridview cargandolo en pantalla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRespuestas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                IdRespuesta = Convert.ToInt32(HttpUtility.HtmlDecode(gvRespuestas.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text));
                this.txbCodigo.Text = HttpUtility.HtmlDecode(gvRespuestas.Rows[Convert.ToInt32(e.CommandArgument)].Cells[3].Text);
                this.txbRespuesta.Text = HttpUtility.HtmlDecode(gvRespuestas.Rows[Convert.ToInt32(e.CommandArgument)].Cells[4].Text);
                this.ddlEstadoP.SelectedValue = HttpUtility.HtmlDecode(this.gvRespuestas.Rows[Convert.ToInt32(e.CommandArgument)].Cells[5].Text);
                this.ddlEstadoD.SelectedValue = HttpUtility.HtmlDecode(this.gvRespuestas.Rows[Convert.ToInt32(e.CommandArgument)].Cells[7].Text);
                this.ddlEstadoRespuesta.SelectedValue = HttpUtility.HtmlDecode(this.gvRespuestas.Rows[Convert.ToInt32(e.CommandArgument)].Cells[9].Text);
                this.DDLEnvioC.SelectedValue = HttpUtility.HtmlDecode(this.gvRespuestas.Rows[Convert.ToInt32(e.CommandArgument)].Cells[10].Text);
                imgBtnAddField.ImageUrl = "~/MarcaVisual/iconos/aceptar.png";
            }
            else if (e.CommandName.Equals("Eliminar"))
            {
                RespuestaTransaccion objRT = new RespuestaTransaccion();
                objRT.pId = Convert.ToInt32(HttpUtility.HtmlDecode(gvRespuestas.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text));
                int valor = new RespuestaTransaccionLN().borrar(objRT);

                if (valor == 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.CampoEliminado + "');</script>", false);                    
                    llenarGrillaCampos();
                }
                else
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('"+RecursosDebito.ErrorProceso+"');</script>", false);                    
            }
        }

        /// <summary>
        /// Cambio de pagina del gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRespuestas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvRespuestas.PageIndex = e.NewPageIndex;
            this.gvRespuestas.DataSource = Respuestas;
            this.gvRespuestas.DataBind();
        }

        /// <summary>
        /// Carga la información del gridview
        /// </summary>
        private void llenarGrillaCampos()
        {
            RespuestaTransaccion objRT = new RespuestaTransaccion();
            objRT.pBanco = IdBanco;

            DataSet DsRespuestas = new DataSet();
            DsRespuestas = new RespuestaTransaccionLN().consultar(objRT);
            Respuestas = DsRespuestas.Tables[0];
            gvRespuestas.DataSource = Respuestas;
            gvRespuestas.DataBind();

            IdRespuesta = 0;
            this.txbRespuesta.Text = this.txbCodigo.Text = String.Empty;
            this.ddlEstadoD.SelectedIndex = this.ddlEstadoP.SelectedIndex = this.ddlEstadoRespuesta.SelectedIndex = 0 ;
            this.DDLEnvioC.SelectedIndex = 0;
        }

    }
}