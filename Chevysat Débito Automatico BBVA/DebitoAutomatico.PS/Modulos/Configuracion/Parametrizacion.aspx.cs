using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DebitoAutomatico.EN;
using DebitoAutomatico.EN.Tablas;
using DebitoAutomatico.LN.Consultas;
using DebitoAutomatico.PS.Codigo;

using System.Data.Odbc;
using System.Configuration;
using System.Data.SqlClient;


/// <summary>
/// Autor: Nicolas Alexander Larrotta
/// Fecha Ultima Actualización: 20 de Junio de 2018
/// Observacion: Modulo de parametrizacion de las listas
/// </summary>

/// <summary>
/// Autor: Nicolas Alexander Larrotta
/// Fecha Ultima Actualización: 17 de Junio de 2019
/// Observacion: Parametrización dias habiles
/// Etiqueta: FECD
/// </summary>

namespace DebitoAutomatico.PS.Modulos.Configuracion
{

    public partial class Parametrizacion : System.Web.UI.Page
    {

        List<EstadosClientes> ListEstado = new List<EstadosClientes>();
        List<TipoFormato> ListFormato = new List<TipoFormato>();
        List<TipoCuenta> ListTipoC = new List<TipoCuenta>();
        List<TipoCausales> ListTipoCausal = new List<TipoCausales>();
        List<TipoInconsistencia> ListTipoInconsistencia = new List<TipoInconsistencia>();
        List<Fechas> ListFechas = new List<Fechas>();

        EstadosClientes objEstadosClientes = new EstadosClientes();
        TipoFormato objTipoFormato = new TipoFormato();
        TipoCuenta objTipoC = new TipoCuenta();
        TipoCausales objTipoCausal = new TipoCausales();
        TipoInconsistencia objTipoInconsistencia = new TipoInconsistencia();

        List<EstadosClientes> UpdateListEstado = new List<EstadosClientes>();
        List<TipoFormato> UpdateListFormato = new List<TipoFormato>();
        List<TipoCuenta> UpdateListTipoC = new List<TipoCuenta>();
        List<TipoCausales> UpdateListTipoCausal = new List<TipoCausales>();
        List<TipoInconsistencia> UpdateListTipoInconsistencia = new List<TipoInconsistencia>();

        TextBox txbCausal;

        static int indexGV;
        String campos;

        private Int32 IdFecha
        {
            get
            {
                int valor = 0;
                if (ViewState["pId"] != null)
                    valor = Convert.ToInt32(ViewState["pId"]);
                return valor;
            }

            set
            {
                ViewState["pId"] = value;
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
        /// Adiciona un nuevo registro a la base de datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCausales_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int Estado = 0;
                txbCausal = (TextBox)gvCausales.FooterRow.FindControl("txtCausal");

                if (e.CommandName.Equals("AddNew"))
                {
                    string Valor = ddlListas.SelectedValue;

                    switch (Valor)
                    {
                        case "1":
                            objEstadosClientes.pValor = txbCausal.Text.ToUpper();
                            ListEstado = new EstadosClientesLN().consultarEstado(objEstadosClientes);
                            if (ListEstado.Count > 0)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ParametroExiste + "');</script>", false);
                            }
                            else
                            {
                                Estado = new EstadosClientesLN().insertar(objEstadosClientes);
                                if (Estado > 0)
                                {
                                    ListEstado = new EstadosClientesLN().consultarEstado(new EstadosClientes());
                                    gvCausales.DataSource = ListEstado;
                                    gvCausales.DataBind();
                                    Estado = 0;

                                    #region (INFORMACION PARA LOG)
                                    campos = string.Concat(
                                        " OPCIÓN: ", this.ddlListas.SelectedItem,
                                        ", VALOR NUEVO: ", objEstadosClientes.pValor);
                                    Log(1, campos);
                                    #endregion
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ProcesoExistoso + "');</script>", false);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorProceso + "');</script>", false);
                                }
                            }
                            break;
                        case "2":
                            objTipoFormato.pValor = txbCausal.Text.ToUpper();
                            ListFormato = new TipoFormatoLN().consultarTipoF(objTipoFormato);
                            if (ListFormato.Count > 0)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ParametroExiste + "');</script>", false);
                            }
                            else
                            {
                                Estado = new TipoFormatoLN().insertar(objTipoFormato);
                                if (Estado > 0)
                                {
                                    ListFormato = new TipoFormatoLN().consultarTipoF(new TipoFormato());
                                    gvCausales.DataSource = ListFormato;
                                    gvCausales.DataBind();
                                    Estado = 0;

                                    #region (INFORMACION PARA LOG)
                                    campos = string.Concat(
                                        " OPCIÓN: ", this.ddlListas.SelectedItem,
                                        ", VALOR NUEVO: ", objTipoFormato.pValor);
                                    Log(1, campos);
                                    #endregion
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ProcesoExistoso + "');</script>", false);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorProceso + "');</script>", false);
                                }
                            }

                            break;
                        case "3":
                            objTipoC.pValor = txbCausal.Text.ToUpper();
                            ListTipoC = new TipoCuentaLN().consultarTipoC(objTipoC);
                            if (ListTipoC.Count > 0)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ParametroExiste + "');</script>", false);
                            }
                            else
                            {
                                Estado = new TipoCuentaLN().insertar(objTipoC);
                                if (Estado > 0)
                                {
                                    ListTipoC = new TipoCuentaLN().consultarTipoC(new TipoCuenta());
                                    gvCausales.DataSource = ListTipoC;
                                    gvCausales.DataBind();
                                    Estado = 0;
                                    #region (INFORMACION PARA LOG)
                                    campos = string.Concat(
                                        " OPCIÓN: ", this.ddlListas.SelectedItem,
                                        ", VALOR NUEVO: ", objTipoC.pValor);
                                    Log(1, campos);
                                    #endregion
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ProcesoExistoso + "');</script>", false);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorProceso + "');</script>", false);
                                }
                            }

                            break;
                        case "4":
                            objTipoCausal.pValor = txbCausal.Text.ToUpper();
                            ListTipoCausal = new TipoCausalesLN().consultarTipoCausal(objTipoCausal);
                            if (ListTipoCausal.Count > 0)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ParametroExiste + "');</script>", false);
                            }
                            else
                            {
                                Estado = new TipoCausalesLN().insertar(objTipoCausal);
                                if (Estado > 0)
                                {
                                    ListTipoCausal = new TipoCausalesLN().consultarTipoCausal(new TipoCausales());
                                    gvCausales.DataSource = ListTipoCausal;
                                    gvCausales.DataBind();
                                    Estado = 0;
                                    #region (INFORMACION PARA LOG)
                                    campos = string.Concat(
                                        " OPCIÓN: ", this.ddlListas.SelectedItem,
                                        ", VALOR NUEVO: ", objTipoCausal.pValor);
                                    Log(1, campos);
                                    #endregion
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ProcesoExistoso + "');</script>", false);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorProceso + "');</script>", false);
                                }
                            }
                            break;
                        case "5":
                            objTipoInconsistencia.pValor = txbCausal.Text.ToUpper();
                            ListTipoInconsistencia = new TipoInconsistenciaLN().consultarTipoIn(objTipoInconsistencia);
                            if (ListTipoInconsistencia.Count > 0)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ParametroExiste + "');</script>", false);
                            }
                            else
                            {
                                Estado = new TipoInconsistenciaLN().insertar(objTipoInconsistencia);
                                if (Estado > 0)
                                {
                                    ListTipoInconsistencia = new TipoInconsistenciaLN().consultarTipoIn(new TipoInconsistencia());
                                    gvCausales.DataSource = ListTipoInconsistencia;
                                    gvCausales.DataBind();
                                    Estado = 0;
                                    #region (INFORMACION PARA LOG)
                                    campos = string.Concat(
                                        " OPCIÓN: ", this.ddlListas.SelectedItem,
                                        ", VALOR NUEVO: ", objTipoInconsistencia.pValor);
                                    Log(1, campos);
                                    #endregion
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ProcesoExistoso + "');</script>", false);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorProceso + "');</script>", false);
                                }
                            }
                            break;
                        default:
                            gvCausales.DataSource = null;
                            gvCausales.DataBind();
                            break;
                    }


                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + ex.Message.Replace("'", "").Replace("\n", "").Replace("\r", "") + "');</script>", false);
            }
        }

        /// <summary>
        /// Se devuelve a su estado original, sin afectar ningún cambio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCausales_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

            try
            {
                string Valor = ddlListas.SelectedValue;

                switch (Valor)
                {
                    case "1":
                        ListEstado = new EstadosClientesLN().consultarEstado(new EstadosClientes());
                        gvCausales.EditIndex = -1;
                        gvCausales.DataSource = ListEstado;
                        gvCausales.DataBind();
                        break;
                    case "2":
                        ListFormato = new TipoFormatoLN().consultarTipoF(new TipoFormato());
                        gvCausales.EditIndex = -1;
                        gvCausales.DataSource = ListFormato;
                        gvCausales.DataBind();
                        break;
                    case "3":
                        ListTipoC = new TipoCuentaLN().consultarTipoC(new TipoCuenta());
                        gvCausales.EditIndex = -1;
                        gvCausales.DataSource = ListTipoC;
                        gvCausales.DataBind();
                        break;
                    case "4":
                        ListTipoCausal = new TipoCausalesLN().consultarTipoCausal(new TipoCausales());
                        gvCausales.EditIndex = -1;
                        gvCausales.DataSource = ListTipoCausal;
                        gvCausales.DataBind();
                        break;
                    case "5":
                        ListTipoInconsistencia = new TipoInconsistenciaLN().consultarTipoIn(new TipoInconsistencia());
                        gvCausales.EditIndex = -1;
                        gvCausales.DataSource = ListTipoInconsistencia;
                        gvCausales.DataBind();
                        break;
                    default:
                        gvCausales.DataSource = null;
                        gvCausales.DataBind();
                        break;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + ex.Message.Replace("'", "").Replace("\n", "").Replace("\r", "") + "');</script>", false);
            }
        }

        /// <summary>
        /// Elimina un registro de la base de datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCausales_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {

                if (HidValParametro.Value == "1")
                {
                    int EliminarEstado = 1;
                    string Valor = ddlListas.SelectedValue;

                    switch (Valor)
                    {
                        case "1":
                            EliminarEstado = 1;
                            ListEstado = new EstadosClientesLN().consultarEstado(new EstadosClientes());
                            objEstadosClientes.pValor = ListEstado[e.RowIndex].pValor;
                            EliminarEstado = new EstadosClientesLN().eliminar(objEstadosClientes);
                            if (EliminarEstado == 0)
                            {
                                ListEstado = new EstadosClientesLN().consultarEstado(new EstadosClientes());
                                gvCausales.DataSource = ListEstado;
                                gvCausales.DataBind();
                                EliminarEstado = 1;

                                #region (INFORMACION PARA LOG)
                                campos = string.Concat(
                                    " OPCIÓN: ", this.ddlListas.SelectedItem,
                                    ", VALOR: ", objEstadosClientes.pValor);
                                Log(3, campos);
                                #endregion
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.BorrarParametro + "');</script>", false);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorProceso + "');</script>", false);
                            }
                            break;
                        case "2":
                            EliminarEstado = 1;
                            ListFormato = new TipoFormatoLN().consultarTipoF(new TipoFormato());
                            objTipoFormato.pValor = ListFormato[e.RowIndex].pValor;
                            EliminarEstado = new TipoFormatoLN().eliminar(objTipoFormato);
                            if (EliminarEstado == 0)
                            {
                                ListFormato = new TipoFormatoLN().consultarTipoF(new TipoFormato());
                                gvCausales.DataSource = ListFormato;
                                gvCausales.DataBind();
                                EliminarEstado = 1;

                                #region (INFORMACION PARA LOG)
                                campos = string.Concat(
                                    " OPCIÓN: ", this.ddlListas.SelectedItem,
                                    ", VALOR: ", objTipoFormato.pValor);
                                Log(3, campos);
                                #endregion
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.BorrarParametro + "');</script>", false);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorProceso + "');</script>", false);
                            }
                            break;
                        case "3":
                            EliminarEstado = 1;
                            ListTipoC = new TipoCuentaLN().consultarTipoC(new TipoCuenta());
                            objTipoC.pValor = ListTipoC[e.RowIndex].pValor;
                            EliminarEstado = new TipoCuentaLN().eliminar(objTipoC);
                            if (EliminarEstado == 0)
                            {
                                ListTipoC = new TipoCuentaLN().consultarTipoC(new TipoCuenta());
                                gvCausales.DataSource = ListTipoC;
                                gvCausales.DataBind();
                                EliminarEstado = 1;

                                #region (INFORMACION PARA LOG)
                                campos = string.Concat(
                                    " OPCIÓN: ", this.ddlListas.SelectedItem,
                                    ", VALOR: ", objTipoC.pValor);
                                Log(3, campos);
                                #endregion
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.BorrarParametro + "');</script>", false);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorProceso + "');</script>", false);
                            }
                            break;
                        case "4":
                            EliminarEstado = 1;
                            ListTipoCausal = new TipoCausalesLN().consultarTipoCausal(new TipoCausales());
                            objTipoCausal.pValor = ListTipoCausal[e.RowIndex].pValor;
                            EliminarEstado = new TipoCausalesLN().eliminar(objTipoCausal);
                            if (EliminarEstado == 0)
                            {
                                ListTipoCausal = new TipoCausalesLN().consultarTipoCausal(new TipoCausales());
                                gvCausales.DataSource = ListTipoCausal;
                                gvCausales.DataBind();
                                EliminarEstado = 1;

                                #region (INFORMACION PARA LOG)
                                campos = string.Concat(
                                    " OPCIÓN: ", this.ddlListas.SelectedItem,
                                    ", VALOR: ", objTipoCausal.pValor);
                                Log(3, campos);
                                #endregion
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.BorrarParametro + "');</script>", false);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorProceso + "');</script>", false);
                            }
                            break;
                        case "5":
                            EliminarEstado = 1;
                            ListTipoInconsistencia = new TipoInconsistenciaLN().consultarTipoIn(new TipoInconsistencia());
                            objTipoInconsistencia.pValor = ListTipoInconsistencia[e.RowIndex].pValor;
                            EliminarEstado = new TipoInconsistenciaLN().eliminar(objTipoInconsistencia);
                            if (EliminarEstado == 0)
                            {
                                ListTipoInconsistencia = new TipoInconsistenciaLN().consultarTipoIn(new TipoInconsistencia());
                                gvCausales.DataSource = ListTipoInconsistencia;
                                gvCausales.DataBind();
                                EliminarEstado = 1;

                                #region (INFORMACION PARA LOG)
                                campos = string.Concat(
                                    " OPCIÓN: ", this.ddlListas.SelectedItem,
                                    ", VALOR: ", objTipoInconsistencia.pValor);
                                Log(3, campos);
                                #endregion
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.BorrarParametro + "');</script>", false);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorProceso + "');</script>", false);
                            }
                            break;
                        default:
                            gvCausales.DataSource = null;
                            gvCausales.DataBind();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + ex.Message.Replace("'", "").Replace("\n", "").Replace("\r", "") + "');</script>", false);
            }
        }

        /// <summary>
        /// Deja la caja de texto, en estado de edición
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCausales_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvCausales.EditIndex = e.NewEditIndex;
                indexGV = gvCausales.EditIndex;
            }
            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + ex.Message.Replace("'", "").Replace("\n", "").Replace("\r", "") + "');</script>", false);
            }
        }


        /// <summary>
        /// Actualiza la informacion del texto seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCausales_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int Estado = 0;
                txbCausal = (TextBox)gvCausales.Rows[e.RowIndex].FindControl("txbCausal");
                string Valor = ddlListas.SelectedValue;

                switch (Valor)
                {
                    case "1":
                        ListEstado = new EstadosClientesLN().consultarEstado(new EstadosClientes());
                        objEstadosClientes.pValorNuevo = txbCausal.Text.ToUpper();
                        objEstadosClientes.pValor = ListEstado[e.RowIndex].pValor;
                        UpdateListEstado = new EstadosClientesLN().consultarEstadoExiste(objEstadosClientes);
                        if (UpdateListEstado.Count > 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ParametroExiste + "');</script>", false);
                        }
                        else
                        {
                            Estado = new EstadosClientesLN().actualizar(objEstadosClientes);
                            if (Estado > 0)
                            {
                                ListEstado = new EstadosClientesLN().consultarEstado(new EstadosClientes());
                                gvCausales.EditIndex = -1;
                                gvCausales.DataSource = ListEstado;
                                gvCausales.DataBind();
                                Estado = 0;

                                #region (INFORMACION PARA LOG)
                                campos = string.Concat(
                                    " OPCIÓN: ", this.ddlListas.SelectedItem,
                                    ", VALOR ANTERIOR: ", objEstadosClientes.pValor,
                                    ", VALOR NUEVO: ", objEstadosClientes.pValorNuevo);
                                Log(2, campos);
                                #endregion
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('El parametro se actualizo correctamente');</script>", false);

                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('Ocurrio un error, por favor volver a intentarlo');</script>", false);
                            }
                        }
                        break;
                    case "2":
                        ListFormato = new TipoFormatoLN().consultarTipoF(new TipoFormato());
                        objTipoFormato.pValorNuevo = txbCausal.Text.ToUpper();
                        objTipoFormato.pValor = ListFormato[e.RowIndex].pValor;
                        UpdateListFormato = new TipoFormatoLN().consultarFormatoExiste(objTipoFormato);
                        if (UpdateListFormato.Count > 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ParametroExiste + "');</script>", false);
                        }
                        else
                        {
                            Estado = new TipoFormatoLN().actualizar(objTipoFormato);
                            if (Estado > 0)
                            {
                                ListFormato = new TipoFormatoLN().consultarTipoF(new TipoFormato());
                                gvCausales.EditIndex = -1;
                                gvCausales.DataSource = ListFormato;
                                gvCausales.DataBind();
                                Estado = 0;

                                #region (INFORMACION PARA LOG)
                                campos = string.Concat(
                                    " OPCIÓN: ", this.ddlListas.SelectedItem,
                                    ", VALOR ANTERIOR: ", objTipoFormato.pValor,
                                    ", VALOR NUEVO: ", objTipoFormato.pValorNuevo);
                                Log(2, campos);
                                #endregion
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ActualizarParametro + "');</script>", false);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorProceso + "');</script>", false);
                            }
                        }
                        break;
                    case "3":
                        ListTipoC = new TipoCuentaLN().consultarTipoC(new TipoCuenta());
                        objTipoC.pValorNuevo = txbCausal.Text.ToUpper();
                        objTipoC.pValor = ListTipoC[e.RowIndex].pValor;
                        UpdateListTipoC = new TipoCuentaLN().consultarTipoCuentaExiste(objTipoC);

                        if (UpdateListTipoC.Count > 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ParametroExiste + "');</script>", false);
                        }
                        else
                        {
                            Estado = new TipoCuentaLN().actualizar(objTipoC);
                            if (Estado > 0)
                            {
                                ListTipoC = new TipoCuentaLN().consultarTipoC(new TipoCuenta());
                                gvCausales.EditIndex = -1;
                                gvCausales.DataSource = ListTipoC;
                                gvCausales.DataBind();
                                Estado = 0;

                                #region (INFORMACION PARA LOG)
                                campos = string.Concat(
                                    " OPCIÓN: ", this.ddlListas.SelectedItem,
                                    ", VALOR ANTERIOR: ", objTipoC.pValor,
                                    ", VALOR NUEVO: ", objTipoC.pValorNuevo);
                                Log(2, campos);
                                #endregion
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ActualizarParametro + "');</script>", false);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorProceso + "');</script>", false);
                            }
                        }
                        break;
                    case "4":
                        ListTipoCausal = new TipoCausalesLN().consultarTipoCausal(new TipoCausales());
                        objTipoCausal.pValorNuevo = txbCausal.Text.ToUpper();
                        objTipoCausal.pValor = ListTipoCausal[e.RowIndex].pValor;
                        UpdateListTipoCausal = new TipoCausalesLN().consultarTipoCausalesExiste(objTipoCausal);

                        if (UpdateListTipoCausal.Count > 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ParametroExiste + "');</script>", false);
                        }
                        else
                        {
                            Estado = new TipoCausalesLN().actualizar(objTipoCausal);
                            if (Estado > 0)
                            {
                                ListTipoCausal = new TipoCausalesLN().consultarTipoCausal(new TipoCausales());
                                gvCausales.EditIndex = -1;
                                gvCausales.DataSource = ListTipoCausal;
                                gvCausales.DataBind();
                                Estado = 0;

                                #region (INFORMACION PARA LOG)
                                campos = string.Concat(
                                    " OPCIÓN: ", this.ddlListas.SelectedItem,
                                    ", VALOR ANTERIOR: ", objTipoCausal.pValor,
                                    ", VALOR NUEVO: ", objTipoCausal.pValorNuevo);
                                Log(2, campos);
                                #endregion
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ActualizarParametro + "');</script>", false);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorProceso + "');</script>", false);
                            }
                        }
                        break;
                    case "5":
                        ListTipoInconsistencia = new TipoInconsistenciaLN().consultarTipoIn(new TipoInconsistencia());
                        objTipoInconsistencia.pValorNuevo = txbCausal.Text.ToUpper();
                        objTipoInconsistencia.pValor = ListTipoInconsistencia[e.RowIndex].pValor;
                        UpdateListTipoInconsistencia = new TipoInconsistenciaLN().consultarTipoInconsistenciaExiste(objTipoInconsistencia);

                        if (UpdateListTipoInconsistencia.Count > 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ParametroExiste + "');</script>", false);
                        }
                        else
                        {
                            Estado = new TipoInconsistenciaLN().actualizar(objTipoInconsistencia);
                            if (Estado > 0)
                            {
                                ListTipoInconsistencia = new TipoInconsistenciaLN().consultarTipoIn(new TipoInconsistencia());
                                gvCausales.EditIndex = -1;
                                gvCausales.DataSource = ListTipoInconsistencia;
                                gvCausales.DataBind();
                                Estado = 0;

                                #region (INFORMACION PARA LOG)
                                campos = string.Concat(
                                    " OPCIÓN: ", this.ddlListas.SelectedItem,
                                    ", VALOR ANTERIOR: ", objTipoInconsistencia.pValor,
                                    ", VALOR NUEVO: ", objTipoInconsistencia.pValorNuevo);
                                Log(2, campos);
                                #endregion
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ActualizarParametro + "');</script>", false);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorProceso + "');</script>", false);
                            }
                        }
                        break;
                    default:
                        gvCausales.DataSource = null;
                        gvCausales.DataBind();
                        break;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + ex.Message.Replace("'", "").Replace("\n", "").Replace("\r", "") + "');</script>", false);
            }

        }

        /// <summary>
        /// Muestra las listas de acuerdo a la que se selecciono
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlListas_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.pnlMovimientos.Visible = true;
                this.pnlFechas.Visible = false;
                string Valor = ddlListas.SelectedValue;

                switch (Valor)
                {
                    case "1":
                        ListEstado = new EstadosClientesLN().consultarEstado(new EstadosClientes());
                        gvCausales.DataSource = ListEstado;
                        gvCausales.DataBind();
                        break;
                    case "2":
                        ListFormato = new TipoFormatoLN().consultarTipoF(new TipoFormato());
                        gvCausales.DataSource = ListFormato;
                        gvCausales.DataBind();
                        break;
                    case "3":
                        ListTipoC = new TipoCuentaLN().consultarTipoC(new TipoCuenta());
                        gvCausales.DataSource = ListTipoC;
                        gvCausales.DataBind();
                        break;
                    case "4":
                        ListTipoCausal = new TipoCausalesLN().consultarTipoCausal(new TipoCausales());
                        gvCausales.DataSource = ListTipoCausal;
                        gvCausales.DataBind();
                        break;
                    case "5":
                        ListTipoInconsistencia = new TipoInconsistenciaLN().consultarTipoIn(new TipoInconsistencia());
                        gvCausales.DataSource = ListTipoInconsistencia;
                        gvCausales.DataBind();
                        break;
                    case "6": /*FECD*/
                        this.pnlMovimientos.Visible = false;
                        this.pnlFechas.Visible = true;
                        ListFechas = new FechasLN().consultarFechas(new Fechas());
                        gvFechas.DataSource = ListFechas;
                        gvFechas.DataBind();
                        this.ddldiaHabil.SelectedIndex = 0;
                        break;
                    default:
                        gvCausales.DataSource = null;
                        gvCausales.DataBind();
                        break;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + ex.Message.Replace("'", "").Replace("\n", "").Replace("\r", "") + "');</script>", false);
            }
        }

        /// <summary>
        /// Logs para migración
        /// opcion 1:Insertar,2:Actualizar,3:Eliminar
        /// campos: Informacion que se guardara como Log
        /// </summary>
        /// <param name="opcion"></param>
        /// <param name="campos"></param>
        private void Log(int opcion, String campos)
        {
            LogsParametros objL = new LogsParametros();
            switch (opcion)
            {
                case 1:
                    objL.pFecha = String.Empty;
                    objL.pUsuario = Session["usuario"].ToString();
                    objL.pDetalle = "Se inserto el parametro: " + campos;
                    objL.pMovimiento = "INSERTAR";
                    new LogsParametrosLN().insertar(objL);
                    break;
                case 2:
                    objL.pFecha = String.Empty;
                    objL.pUsuario = Session["usuario"].ToString();
                    objL.pDetalle = "Se actualizó el parametro: " + campos;
                    objL.pMovimiento = "ACTUALIZACIÓN";
                    new LogsParametrosLN().insertar(objL);
                    break;
                case 3:
                    objL.pFecha = String.Empty;
                    objL.pUsuario = Session["usuario"].ToString();
                    objL.pDetalle = "Se eliminó el parametro: " + campos;
                    objL.pMovimiento = "ELIMINACIÓN";
                    new LogsParametrosLN().insertar(objL);
                    break;
            }
        }

        /// <summary>
        /// Guarda o actualiza el perfil del usuario que se selecciona
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgBtnGuardar_Click(object sender, ImageClickEventArgs e)
        {
            Fechas ObjEntidad = new Fechas();

            ObjEntidad.pId = IdFecha;
            ObjEntidad.pDia = Convert.ToInt32(this.ddldiaHabil.SelectedValue);
            ObjEntidad.pValor = this.ddldiaHabil.SelectedItem.Text;
            ObjEntidad.pHabilita = this.chbHabilita.Checked;

            if (IdFecha == 0)
            {
                Fechas ObjEntidadExiste = new Fechas();

                ObjEntidadExiste.pDia = Convert.ToInt32(this.ddldiaHabil.SelectedValue);

                List<Fechas> ListConsultaFechas = new FechasLN().consultarFechas(ObjEntidadExiste);

                if (ListConsultaFechas.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ParametroExiste + "');</script>", false);
                    this.ddldiaHabil.SelectedIndex = 0;
                    IdFecha = 0;
                    this.chbHabilita.Checked = false;
                    return;
                }

                int guardar = new FechasLN().insertar(ObjEntidad);

                if (guardar > 0)
                {
                    #region (INFORMACION PARA LOG)
                    campos = string.Concat(
                        " OPCIÓN: ", this.ddlListas.SelectedItem,
                        ", VALOR NUEVO: ", this.ddldiaHabil.SelectedItem);
                    Log(1, campos);
                    #endregion

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ProcesoExistoso + "');</script>", false);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorProceso + "');</script>", false);
                }

            }
            else
            {
                int actualizar = new FechasLN().actualizar(ObjEntidad);

                if (actualizar > 0)
                {
                    #region (INFORMACION PARA LOG)
                    campos = string.Concat(
                        " OPCIÓN: ", this.ddlListas.SelectedItem,
                        ", VALOR NUEVO: ", this.ddldiaHabil.SelectedItem);
                    Log(1, campos);
                    #endregion

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ProcesoExistoso + "');</script>", false);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorProceso + "');</script>", false);
                }
            }


            ListFechas = new FechasLN().consultarFechas(new Fechas());
            gvFechas.DataSource = ListFechas;
            gvFechas.DataBind();
            this.ddldiaHabil.SelectedIndex = 0;
            this.chbHabilita.Checked = false;
            IdFecha = 0;
            this.ddldiaHabil.Enabled = true;

        }

        /// <summary>
        /// Carga la informacion del gridview en la pantalla o elimina definitvamente el registro del gridview /*FECD*/
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFechas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Editar"))
                {
                    this.IdFecha = Convert.ToInt32(HttpUtility.HtmlDecode(gvFechas.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text.Trim()));
                    this.ddldiaHabil.SelectedValue = HttpUtility.HtmlDecode(gvFechas.Rows[Convert.ToInt32(e.CommandArgument)].Cells[3].Text.Trim());
                    this.chbHabilita.Checked = ((CheckBox)this.gvFechas.Rows[Convert.ToInt32(e.CommandArgument)].Cells[5].Controls[0]).Checked;
                    this.ddldiaHabil.Enabled = false;
                }
                else if (e.CommandName.Equals("Eliminar"))
                {
                    DatosDebito objDatos = new DatosDebito();
                    objDatos.pFechaDebito = Convert.ToInt32(HttpUtility.HtmlDecode(gvFechas.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text.Trim()));

                    List<DatosDebito> listadebito = new DatosDebitoLN().consultar(objDatos);

                    if (listadebito.Count > 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.FechaDebito + "');</script>", false);
                        return;
                    }

                    Fechas ObjFechas = new Fechas();
                    ObjFechas.pId = Convert.ToInt32(HttpUtility.HtmlDecode(gvFechas.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text.Trim()));
                    ObjFechas.pValor = HttpUtility.HtmlDecode(gvFechas.Rows[Convert.ToInt32(e.CommandArgument)].Cells[4].Text.Trim());

                    int eliminar = new FechasLN().eliminar(ObjFechas);

                    if (eliminar == 0)
                    {

                        #region (INFORMACION PARA LOG)
                        campos = string.Concat(
                            " OPCIÓN: ", this.ddlListas.SelectedItem,
                            ", VALOR: ", ObjFechas.pValor);
                        Log(3, campos);
                        #endregion

                        ListFechas = new FechasLN().consultarFechas(new Fechas());
                        gvFechas.DataSource = ListFechas;
                        gvFechas.DataBind();
                        this.ddldiaHabil.SelectedIndex = 0;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ProcesoExistoso + "');</script>", false);
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

        protected void imgBtnNuevo_Click(object sender, ImageClickEventArgs e)
        {
            ListFechas = new FechasLN().consultarFechas(new Fechas());
            gvFechas.DataSource = ListFechas;
            gvFechas.DataBind();
            this.ddldiaHabil.SelectedIndex = 0;
            this.ddldiaHabil.Enabled = true;
            this.chbHabilita.Checked = false;
            IdFecha = 0;
        }

    }
}