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

using System.Data.Odbc;
using System.Configuration;
using DebitoAutomatico.LN.Utilidades;
using System.Text;
using System.IO;
using System.Security.Cryptography;

/// <summary>
/// Autor: Nicolas Alexander Larrotta
/// Fecha Ultima Actualización: 20 de Junio de 2018
/// Observacion: Modulo Consulta Cliente
/// </summary>

/// <summary>
/// Autor: Nicolas Alexander Larrotta
/// Fecha Ultima Actualización: 08 de Febrero de 2019
/// Observacion: Se ajusta la consulta de Cesiones
/// Abrebiatura: CESI
/// </summary>

namespace DebitoAutomatico.PS.Modulos.Configuracion
{
    public partial class ConsultaCliente : System.Web.UI.Page
    {
        private ServiceDebito.ServiceDebito SerDebito = new ServiceDebito.ServiceDebito();

        Encriptador objEncriptador = new Encriptador();

        private String NContrato
        {
            get
            {
                String Ncont = String.Empty;
                if (ViewState["NContrato"] != null)
                    Ncont = Convert.ToString(ViewState["NContrato"]);
                return Ncont;
            }

            set
            {
                ViewState["NContrato"] = value;
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

        private int IdTitular
        {
            get
            {
                int IdRe = 0;
                if (ViewState["IdTitular"] != null)
                    IdRe = Convert.ToInt32(ViewState["IdTitular"]);
                return IdRe;
            }

            set
            {
                ViewState["IdTitular"] = value;
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
             //   Session["usuario"] = "maikol.ramirez";
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
                DataSet tipoc = new DataSet();
                String TipoCuenta = SerDebito.TipoCuenta(Session["usuario"].ToString(), "");
                System.IO.StringReader xmlSR = new System.IO.StringReader(TipoCuenta);
                tipoc.ReadXml(xmlSR);
                this.ddlTipoCuenta.DataSource = tipoc;
                this.ddlTipoCuenta.DataTextField = "VALOR";
                this.ddlTipoCuenta.DataValueField = "ID";
                this.ddlTipoCuenta.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlTipoCuenta);

                this.ddlTipoIdentificacionT.DataSource =
                this.ddlTipoIdentificacionC.DataSource =
                new TipoDocumentoLN().consultarTipoD(new TipoDocumento());

                this.ddlTipoIdentificacionT.DataTextField =
                this.ddlTipoIdentificacionC.DataTextField = "pAbreviatura";


                this.ddlTipoIdentificacionT.DataValueField =
                this.ddlTipoIdentificacionC.DataValueField = "pId";

                this.ddlTipoIdentificacionT.DataBind();
                this.ddlTipoIdentificacionC.DataBind();

                UtilidadesWeb.agregarSeleccioneDDL(this.ddlTipoIdentificacionC);
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlTipoIdentificacionT);

                DataSet IdBanco = new DataSet();
                String Bancos = SerDebito.IdBanco(Session["usuario"].ToString(), "");
                System.IO.StringReader xmlSRB = new System.IO.StringReader(Bancos);
                IdBanco.ReadXml(xmlSRB);
                this.ddlBanco.DataSource = IdBanco;
                this.ddlBanco.DataTextField = "NOMBRE";
                this.ddlBanco.DataValueField = "ID";
                this.ddlBanco.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlBanco);

                this.ddlEstadoD.DataSource = new EstadosClientesLN().consultarEstado(new EstadosClientes());
                this.ddlEstadoD.DataTextField = "pValor";
                this.ddlEstadoD.DataValueField = "pId";
                this.ddlEstadoD.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlEstadoD);

                DataSet IdFormato = new DataSet();
                String Formato = SerDebito.CanalIngreso(Session["usuario"].ToString(), "");
                System.IO.StringReader xmlSRF = new System.IO.StringReader(Formato);
                IdFormato.ReadXml(xmlSRF);
                this.ddlFormaDebito.DataSource = this.ddlFormatoCancelacion.DataSource = IdFormato;
                this.ddlFormaDebito.DataTextField = this.ddlFormatoCancelacion.DataTextField = "VALOR";
                this.ddlFormaDebito.DataValueField = this.ddlFormatoCancelacion.DataValueField = "ID";
                this.ddlFormaDebito.DataBind();
                this.ddlFormatoCancelacion.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlFormaDebito);
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlFormatoCancelacion);

                DataSet Idfechas = new DataSet();
                String Fechas = SerDebito.FechaDebito(Session["usuario"].ToString(), "");
                System.IO.StringReader xmlFd = new System.IO.StringReader(Fechas);
                Idfechas.ReadXml(xmlFd);
                this.ddlFechaDebito.DataSource = Idfechas;
                this.ddlFechaDebito.DataTextField = "VALOR";
                this.ddlFechaDebito.DataValueField = "ID";
                this.ddlFechaDebito.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlFechaDebito);
            }
        }

        /// <summary>
        /// Redirecciona la pagina de nuevo para limpiar todos los elementos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgBtnNuevo_Click(object sender, ImageClickEventArgs e)
        {
            this.gvHistorial.DataSource = this.gvCesion.DataSource = null;
            this.gvHistorial.DataBind();
            this.gvCesion.DataBind();
            Response.Redirect("~/Modulos/Configuracion/ConsultaCliente.aspx?tv=dba%2fcg%2fccc");
        }

        /// <summary>
        /// Realiza la busqueda del contrato en la base de datos de debito automatico
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            int Contrato = 0;
            int Identificacion = 0;
            int Noexiste = 0;
            if (!String.IsNullOrEmpty(txbContratoB.Text.Trim()) || !String.IsNullOrEmpty(txbIdentClienteB.Text.Trim()))
            {
                DataSet DsDebito = new DataSet();
                DataSet DsDebitoInconsistente = new DataSet();
                DsDebito = new DatosDebitoLN().consultarContratosBusqueda(1, txbContratoB.Text.Trim(), txbIdentClienteB.Text.Trim());
                DsDebitoInconsistente = new DatosDebitoLN().consultarContratosBusqueda(2, txbContratoB.Text.Trim(), txbIdentClienteB.Text.Trim());

                if (DsDebito.Tables[0].Rows.Count > 0)
                {
                    Respuestas = DsDebito.Tables[0];
                    gvBusquedaContrato.DataSource = Respuestas;
                    gvBusquedaContrato.DataBind();
                    mpeBusquedaContrato.Show();
                    this.btnLimpiar.Enabled = true;
                }
                else if (DsDebitoInconsistente.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < DsDebitoInconsistente.Tables[0].Rows.Count ; i++)
                    {
                        if (DsDebitoInconsistente.Tables[0].Rows[i]["CONTRATO"].ToString() == txbContratoB.Text.Trim())
                        {
                            Contrato++;
                        }
                        else if (DsDebitoInconsistente.Tables[0].Rows[i]["NUMERO_IDENTIFICACION"].ToString() == txbIdentClienteB.Text.Trim() && DsDebitoInconsistente.Tables[0].Rows[i]["NUMERO_IDENTIFICACION"].ToString() != String.Empty)
                        {
                            Identificacion++;
                        }
                        else
                        {
                            Noexiste++;
                        }
                    }

                    if (Contrato > 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.Contrato + txbContratoB.Text + RecursosDebito.ContratoInconsistente + "');</script>", false);
                        this.txbContratoB.Text = txbIdentClienteB.Text = String.Empty;
                        return;
                    }

                    if (Identificacion > 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + "Cliente" + RecursosDebito.ContratoInconsistente + "');</script>", false);
                        this.txbContratoB.Text = txbIdentClienteB.Text = String.Empty;
                        return;
                    }

                    if (Noexiste > 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.NoExisteInformacion + "');</script>", false);
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.NoExisteInformacion + "');</script>", false);
                    return;
                }
            
            }
        }
                                
        /// <summary>
        /// Limpia los campos del pop-up para una nueva busqueda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevaBusqueda_Click(object sender, EventArgs e)
        {
            this.btnLimpiar.Enabled = false;
            this.txbContratoB.Text = this.txbIdentClienteB.Text = String.Empty;
            this.gvBusquedaContrato.DataSource = null;
            this.gvBusquedaContrato.DataBind();
            this.mpeBusquedaContrato.Show();
        }

        /// <summary>
        /// Limpia los elementos del pop-up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            this.btnLimpiar.Enabled = false;
            this.gvBusquedaContrato.DataSource = null;
            this.gvBusquedaContrato.DataBind();
            this.mpeBusquedaContrato.Show();
        }

        /// <summary>
        /// Cierra el pop-up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Al seleccionar el contrato o el número de documento se direcciona al metodo buscar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvBusquedaContrato_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("sel"))
            {
                this.txbContrato.Text = HttpUtility.HtmlDecode(this.gvBusquedaContrato.Rows[Convert.ToInt32(e.CommandArgument)].Cells[1].Text.TrimStart('0'));
                IdTitular = Convert.ToInt32(HttpUtility.HtmlDecode(gvBusquedaContrato.Rows[Convert.ToInt32(e.CommandArgument)].Cells[5].Text));
                buscar(true);
            }
        }

        /// <summary>
        /// LLena el gridview con la informacion que tenga el cliente en debito automatico
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvHistorial_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvHistorial.PageIndex = e.NewPageIndex;
            this.gvHistorial.DataSource = Historial;
            this.gvHistorial.DataBind();
        }

        /// <summary>
        /// Limpia los elmentos de la pantalla
        /// </summary>
        private void limpiar()
        {
            NContrato = String.Empty;
            this.txbCelularC.Text = this.txbValorMax.Text = this.txbDigito.Text =
            this.txbContratoB.Text = this.txbCorreoC.Text =
            this.txbFechaFin.Text = this.txbFechaInicial.Text = this.txbIdentClienteB.Text =
            this.txbIdentificacionC.Text = this.txbIdentificacionT.Text =
            this.txbNombreCliente.Text = this.txbNombreTercero.Text = this.txbNumCuenta.Text =
            this.txbTipoCliente.Text = this.txbEstadoCont.Text =
            this.txbValorMax.Text = String.Empty;
            this.chbTercero.Checked = false;
            this.ddlBanco.SelectedIndex = this.ddlFechaDebito.SelectedIndex = this.ddlTipoCuenta.SelectedIndex = this.ddlEstadoD.SelectedIndex = 0;
            this.gvHistorial.DataSource = null;
            this.gvHistorial.DataBind();

            MenuTercero();
        }

        /// <summary>
        /// Metodo para buscar el número de contrato en SICO y en debito automatico
        /// </summary>
        /// <param name="esBusqPorTxb"></param>
        private void buscar(bool esBusqPorTxb)
        {
            String Nombre = String.Empty;
            String Documento = String.Empty;
            String TipoDoc = String.Empty;
            String FechaIngreso = String.Empty;
            String FechaFinalizacion = String.Empty;
            String MontoMaximo = String.Empty;

            this.gvHistorial.DataSource =  this.gvCesion.DataSource = null;
            this.gvHistorial.DataBind();
            this.gvCesion.DataBind();

            if (esBusqPorTxb)
            {
                DataSet objCliente = new DataSet();
                String Resultado = String.Empty;
                Resultado = SerDebito.ConsultaCliente(Convert.ToInt32(txbContrato.Text), true, IdTitular, true, Session["usuario"].ToString(), "");

                System.IO.StringReader xmlSR = new System.IO.StringReader(Resultado);
                objCliente.ReadXml(xmlSR);

                if (objCliente.Tables.Count > 0)
                {
                    try
                    {

                        if (objCliente.Tables["ClienteSICO"].Rows[0]["AFINIDAD"].ToString() == "99" || objCliente.Tables["ClienteSICO"].Rows[0]["AFINIDAD"].ToString() == "98") //TERMINACIÓN DE CONTRATO
                        {
                            if (objCliente.Tables.Contains("ClienteDebito"))
                            {
                                TitularCuenta objTerc = new TitularCuenta();
                                objTerc.pId = Convert.ToInt32(objCliente.Tables["ClienteDebito"].Rows[0]["ID_TITULAR_CUENTA"].ToString());

                                List<TitularCuenta> lista = new TitularCuentaLN().consultarTerceros(objTerc);

                                TitularCuenta objConsult = new TitularCuenta();
                                objConsult = lista[0];

                                Nombre = objConsult.pNombre;
                                Documento = objConsult.pNumeroIdentificacion.TrimStart('0');
                                TipoDoc = Convert.ToString(objConsult.pTipoIdentificacion);
                                FechaIngreso = Convert.ToDateTime(objConsult.pFechaIngreso).ToString("dd/MM/yyyy");
                                FechaFinalizacion = Convert.ToDateTime(objConsult.pFechaFinalizacion).ToString("dd/MM/yyyy");
                                MontoMaximo = Convert.ToString(objConsult.pMontoMaximo);
                            }

                        }
                        else
                        {
                            if (objCliente.Tables.Contains("ClienteDebito"))
                            {
                                Nombre = objCliente.Tables["Titular"].Rows[0]["NOMBRE"].ToString();
                                Documento = objCliente.Tables["Titular"].Rows[0]["NUMERO_IDENTIFICACION"].ToString().TrimStart('0');
                                TipoDoc = objCliente.Tables["Titular"].Rows[0]["TIPO_IDENTIFICACION"].ToString();
                                FechaIngreso = Convert.ToDateTime(objCliente.Tables["Titular"].Rows[0]["FECHA_INGRESO"].ToString()).ToString("dd/MM/yyyy");
                                FechaFinalizacion = Convert.ToDateTime(objCliente.Tables["Titular"].Rows[0]["FECHA_FINALIZACION"].ToString()).ToString("dd/MM/yyyy");
                                MontoMaximo = objCliente.Tables["Titular"].Rows[0]["MONTO_MAXIMO"].ToString();
                            }
                        }


                        if (objCliente.Tables["ClienteSICO"].Rows.Count > 0)
                        {
                            if (objCliente.Tables.Contains("InfoCesion"))  //CESI
                            {
                                this.txbNombreCliente.Text = Nombre;
                                this.txbIdentificacionC.Text = Documento;
                                this.ddlTipoIdentificacionC.SelectedValue = TipoDoc;
                                this.txbEstadoCont.Text = objCliente.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString();
                                this.txbTipoCliente.Text = objCliente.Tables["ClienteSICO"].Rows[0]["TIPO_CUPO"].ToString();

                                this.lbCelularC.Visible = this.lbCorreoC.Visible = this.txbCorreoC.Visible = this.txbCelularC.Visible = false;

                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.Cesiones + "');</script>", false);

                                this.pnlCesion.Visible = true;
                                this.gvCesion.DataSource = objCliente.Tables["InfoCesion"].DefaultView;
                                this.gvCesion.DataBind();
                            }
                            else
                            {
                                this.txbIdentificacionC.Text = objCliente.Tables["ClienteSICO"].Rows[0]["NUMERO_DOCUMENTO_CLIENTE"].ToString().TrimStart('0');
                                this.ddlTipoIdentificacionC.SelectedValue = Convert.ToString(UtilidadesWeb.homologarDocumento(objCliente.Tables["ClienteSICO"].Rows[0]["TIPO_DOCUMENTO"].ToString()));
                                this.txbEstadoCont.Text = objCliente.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString();
                                this.txbTipoCliente.Text = objCliente.Tables["ClienteSICO"].Rows[0]["TIPO_CUPO"].ToString();
                                this.txbCorreoC.Text = objCliente.Tables["ClienteSICO"].Rows[0]["EMAIL_CLIENTE"].ToString();
                                this.txbCelularC.Text = objCliente.Tables["ClienteSICO"].Rows[0]["CELULAR"].ToString();
                                this.txbNombreCliente.Text = objCliente.Tables["ClienteSICO"].Rows[0]["NOMBRE_CLIENTE"].ToString();
                            }
                        }
                    }
                    catch
                    {
                        this.pnlBanco.Enabled = false;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + objCliente.Tables["Resultado"].Rows[0].ItemArray[0].ToString() + "');</script>", false);
                        return;
                    }
                    this.txbDigito.Text = UtilidadesWeb.calculoDigito(txbContrato.Text.Trim());


                    if (objCliente.Tables.Contains("ClienteInconsistente"))
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + objCliente.Tables["Resultado"].Rows[0].ItemArray[0].ToString() + "');</script>", false);
                        return;
                    }

                    if (objCliente.Tables.Contains("ClienteDebito"))
                    {
                        NContrato = objCliente.Tables["ClienteDebito"].Rows[0]["CONTRATO"].ToString();
                        try
                        {
                            this.ddlBanco.SelectedValue = objCliente.Tables["ClienteDebito"].Rows[0]["ID_BANCO"].ToString();
                            this.ddlFechaDebito.SelectedValue = objCliente.Tables["ClienteDebito"].Rows[0]["FECHA_DEBITO"].ToString();
                        }
                        catch (Exception)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + objCliente.Tables["Resultado"].Rows[1].ItemArray[0].ToString() + "');</script>", false);
                        }

                        this.ddlTipoCuenta.SelectedValue = objCliente.Tables["ClienteDebito"].Rows[0]["TIPO_CUENTA"].ToString();
                        this.txbNumCuenta.Text = objCliente.Tables["ClienteDebito"].Rows[0]["NUMERO_CUENTA"].ToString();
                        this.chbTercero.Checked = Convert.ToBoolean(objCliente.Tables["ClienteDebito"].Rows[0]["TERCERO"].ToString());
                        this.chbSuspender.Checked = Convert.ToBoolean(objCliente.Tables["ClienteDebito"].Rows[0]["SUSPENDIDO"].ToString());

                        if (this.chbSuspender.Checked == true)
                        {
                            this.txbFechaInicialSuspesion.Visible = this.imgFechaSusIni.Visible = this.imgBtnFechaFinSuspension.Visible = this.txbFechaFinSuspension.Visible =
                            this.lbFechaInicioSuspension.Visible = this.lbFechaFinSuspension.Visible = true;

                            this.txbFechaInicialSuspesion.Text = Convert.ToDateTime(objCliente.Tables["ClienteDebito"].Rows[0]["FECHA_INICIO_SUSPENSION"].ToString()).ToString("dd/MM/yyyy"); 
                            this.txbFechaFinSuspension.Text = Convert.ToDateTime(objCliente.Tables["ClienteDebito"].Rows[0]["FECHA_FIN_SUSPENSION"].ToString()).ToString("dd/MM/yyyy");
                        }

                        MenuTercero();

                        if (Convert.ToBoolean(objCliente.Tables["ClienteDebito"].Rows[0]["TERCERO"].ToString()) == true)
                        {
                            this.txbNombreTercero.Text = Nombre;
                            this.txbIdentificacionT.Text = Documento;
                            this.ddlTipoIdentificacionT.SelectedValue = TipoDoc;
                        }

                        this.ddlEstadoD.SelectedValue = objCliente.Tables["ClienteDebito"].Rows[0]["ESTADO"].ToString();
                        this.txbFechaInicial.Text = FechaIngreso;
                        this.txbFechaFin.Text = FechaFinalizacion;
                        this.txbValorMax.Text = MontoMaximo;
                        this.ddlFormaDebito.SelectedValue = objCliente.Tables["ClienteDebito"].Rows[0]["ID_FORMATO_DEBITO"].ToString();

                        if (Convert.ToInt32(this.ddlEstadoD.SelectedItem.Value) == 12 || Convert.ToInt32(this.ddlEstadoD.SelectedItem.Value) == 15)
                        {
                            this.lblFormatoCancelacion.Visible = this.ddlFormatoCancelacion.Visible = true;
                            this.ddlFormatoCancelacion.SelectedValue = objCliente.Tables["ClienteDebito"].Rows[0]["ID_FORMATO_CANCELACION"].ToString();
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.Contrato + this.txbContrato.Text + RecursosDebito.EstadoContrato + this.ddlEstadoD.SelectedItem.Text  + "');</script>", false);
                        }
                        else
                        {
                            this.lblFormatoCancelacion.Visible = this.ddlFormatoCancelacion.Visible = false;
                            this.ddlFormaDebito.SelectedValue = objCliente.Tables["ClienteDebito"].Rows[0]["ID_FORMATO_DEBITO"].ToString();
                        }


                        this.txbFechaInicioTransaccion.Enabled = this.imgFechaInicioTransaccion.Enabled = this.txbFechaFinTransaccion.Enabled =
                        this.imgFechaFinTransaccion.Enabled = this.imgBuscar.Enabled = true;

                        this.txbFechaInicioTransaccion.Enabled = this.imgFechaInicioTransaccion.Enabled = this.txbFechaFinTransaccion.Enabled =
                        this.imgFechaFinTransaccion.Enabled = this.imgBuscar.Enabled = true;

                        HistorialProcesoUsuario objHPU = new HistorialProcesoUsuario();
                        objHPU.pContrato = NContrato;
                        //objHPU.pNumeroIdentificacion = objCliente.Tables["Titular"].Rows[0]["NUMERO_IDENTIFICACION"].ToString();
                        Historial = new HistorialProcesoUsuarioLN().consultar(objHPU);
                        Historial.Reverse();
                        this.gvHistorial.DataSource = Historial;
                        this.gvHistorial.DataBind();

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + objCliente.Tables["Resultado"].Rows[0].ItemArray[0].ToString() + "');</script>", false);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + objCliente.Tables["Resultado"].Rows[0].ItemArray[0].ToString() + "');</script>", false);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + objCliente.Tables["Resultado"].Rows[0].ItemArray[0].ToString() + "');</script>", false);
                    this.pnlBanco.Enabled = false;

                }
            }
        }

        /// <summary>
        /// Habilita o deshabilita el menu del tercero
        /// </summary>
        private void MenuTercero()
        {
            if (this.chbTercero.Checked)
                this.pnlTercero.Visible = true;
            else
                this.pnlTercero.Visible = false;
        }

        /// <summary>
        /// Realiza la busqueda del historial del cliente de acuerdo a las fechas ingresadas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgBuscar_Click(object sender, ImageClickEventArgs e)
        {
            if (this.txbFechaFinTransaccion.Text != String.Empty)
            {
                HistorialProcesoUsuario objHPU = new HistorialProcesoUsuario();
                objHPU.pContrato = NContrato;
                objHPU.pFecha = this.txbFechaInicioTransaccion.Text;
                objHPU.pFechaFin = this.txbFechaFinTransaccion.Text;
                objHPU.pNumeroIdentificacion = this.txbIdentificacionC.Text;
                Historial = new HistorialProcesoUsuarioLN().consultar(objHPU);
                if (Historial.Count > 0)
                {
                    Historial.Reverse();
                    this.gvHistorial.DataSource = Historial;
                    this.gvHistorial.DataBind();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.NoExisteInformacion + "');</script>", false);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.NoExisteInformacion + "');</script>", false);
            }
        }

    }
}