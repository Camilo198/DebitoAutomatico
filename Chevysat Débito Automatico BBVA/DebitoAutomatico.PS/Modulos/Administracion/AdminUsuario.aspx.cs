using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;
using DebitoAutomatico.LN.Consultas;
using DebitoAutomatico.LN.Utilidades;
using DebitoAutomatico.PS.Codigo;

using System.Data.Odbc;
using System.Configuration;
using System.Data.SqlClient;

/// <summary>
/// Autor: Nicolas Alexander Larrotta
/// Fecha Ultima Actualización: 20 de Junio de 2018
/// Observacion: Modulo Administracion Usuario Debito Automatico
/// </summary>

namespace DebitoAutomatico.PS.Modulos.Administracion
{
    public partial class AdminUsuario : System.Web.UI.Page
    {
        private Int32 IdUsuario
        {
            get
            {
                int valor = 0;
                if (ViewState["pUsuario"] != null)
                    valor = Convert.ToInt32(ViewState["pUsuario"]);
                return valor;
            }

            set
            {
                ViewState["pUsuario"] = value;
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

                Session["IdUsuario"] = listaU[0].pId.ToString();
                Session["usuario"] = listaU[0].pUsuario.ToString();
                Session["perfil"] = listaU[0].pIdPerfil.ToString();


                if ((Session["perfil"].ToString() != "1"))
                {
                    Response.Redirect("~/Modulos/MenuPrincipal.aspx");
                }
            }
            catch (Exception)
            {
                Response.Redirect("~/Modulos/MenuPrincipal.aspx");
            }

            if (!IsPostBack)
            {
                this.ddlUsuarioChevy.DataSource = new UsuarioLN().consultarUsuarioChevy(new Usuario());
                this.ddlUsuarioChevy.DataTextField = "pUsuario";
                this.ddlUsuarioChevy.DataValueField = "pUsuario";
                this.ddlUsuarioChevy.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlUsuarioChevy);

                this.ddlPerfil.DataSource = new PerfilLN().consultar(new Perfil());
                this.ddlPerfil.DataTextField = "pPerfil";
                this.ddlPerfil.DataValueField = "pId";
                this.ddlPerfil.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlPerfil);

                this.gvUsuario.DataSource = new UsuarioLN().consultar(new Usuario());
                this.gvUsuario.DataBind();
            }
        }

        /// <summary>
        /// Carga la pantalla de inicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgBtnNuevo_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Modulos/Administracion/AdminUsuario.aspx");
        }

        /// <summary>
        /// Guarda o actualiza el perfil del usuario que se selecciona
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgBtnGuardar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Encriptador objEncriptador = new Encriptador();
                Usuario objUsuario = new Usuario();
                UsuarioLN objConsultaUsuario = new UsuarioLN();

                int Usuario = 0;

                if (IdUsuario == 0)
                {
                    objUsuario.pUsuario = this.ddlUsuarioChevy.SelectedValue;
                    objUsuario.pIdPerfil = Convert.ToInt32(this.ddlPerfil.SelectedValue);
                    objUsuario.pPassword = objEncriptador.encriptar("");
                    objUsuario.pHabilita = this.chbHabilita.Checked;

                    Usuario = new UsuarioLN().insertar(objUsuario);

                    if (Usuario > 0)
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('"+ RecursosDebito.ProcesoExistoso +"');</script>", false);
                    else
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('"+ RecursosDebito.ErrorProceso +"');</script>", false);
                }
                else
                {
                    objUsuario.pId = IdUsuario;
                    objUsuario.pIdPerfil = Convert.ToInt32(this.ddlPerfil.SelectedValue);
                    objUsuario.pHabilita = this.chbHabilita.Checked;
                    objUsuario.pUsuario = this.ddlUsuarioChevy.SelectedValue;
                    objUsuario.pPassword = objEncriptador.encriptar("");

                    Usuario = new UsuarioLN().actualizar(objUsuario);

                    if (Usuario > 0)
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('"+RecursosDebito.UsuarioActualizado+"');</script>", false);
                    else
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorProceso + "');</script>", false);
                }

                limpiar();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + ex.Message.Replace("'", "").Replace("\n", "").Replace("\r", "") + "');</script>", false);
            }
        }

        /// <summary>
        /// Carga la informacion del gridview en la pantalla o elimina definitvamente el registro del gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUsuario_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int EliminarUsuario = 1;
                if (e.CommandName.Equals("Editar"))
                {
                    this.IdUsuario = Convert.ToInt32(HttpUtility.HtmlDecode(gvUsuario.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text.Trim()));
                    this.ddlUsuarioChevy.SelectedValue = HttpUtility.HtmlDecode(gvUsuario.Rows[Convert.ToInt32(e.CommandArgument)].Cells[3].Text.Trim());
                    this.ddlPerfil.SelectedValue = HttpUtility.HtmlDecode(gvUsuario.Rows[Convert.ToInt32(e.CommandArgument)].Cells[4].Text.Trim());
                    this.chbHabilita.Checked = ((CheckBox)this.gvUsuario.Rows[Convert.ToInt32(e.CommandArgument)].Cells[5].Controls[0]).Checked;
                }
                else if (e.CommandName.Equals("Eliminar"))
                {
                    Usuario objUsuario = new Usuario();
                    objUsuario.pId = Convert.ToInt32(HttpUtility.HtmlDecode(gvUsuario.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text.Trim()));

                    if (Convert.ToString(objUsuario.pId) == Session["IdUsuario"].ToString())
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('"+RecursosDebito.EliminarMismoUsuario+"');</script>", false);
                        return;
                    }

                    EliminarUsuario = new UsuarioLN().eliminar(objUsuario);
                    if (EliminarUsuario == 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('"+RecursosDebito.BorrarUsuario+"');</script>", false);
                        EliminarUsuario = 1;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorProceso + "');</script>", false);
                        EliminarUsuario = 1;
                    }
                    limpiar();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + ex.Message.Replace("'", "").Replace("\n", "").Replace("\r", "") + "');</script>", false);
            }
        }

        /// <summary>
        /// Limpia los campos de la pantalla y carga el gridview
        /// </summary>
        public void limpiar()
        {
            this.ddlUsuarioChevy.SelectedIndex = this.ddlPerfil.SelectedIndex = 0;
            this.chbHabilita.Checked = false;
            this.gvUsuario.DataSource = new UsuarioLN().consultar(new Usuario());
            this.gvUsuario.DataBind();
        }
    }
}