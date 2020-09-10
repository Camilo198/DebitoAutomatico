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
using DebitoAutomatico.LN.Consulta;

/// <summary>
/// Autor: Nicolas Alexander Larrotta
/// Fecha Ultima Actualización: 20 de Junio de 2018
/// Observacion: Modulo Modificacion Cliente
/// </summary>

/// <summary>
/// Autor: Nicolas Alexander Larrotta
/// Fecha Ultima Actualización: 08 de Febrero de 2019
/// Observacion: Se ajusta la consulta de Cesiones
/// Abrebiatura: CESI
/// </summary>

namespace DebitoAutomatico.PS.Modulos.Configuracion
{
    public partial class ModificacionCliente : System.Web.UI.Page
    {
        Encriptador objEncriptador = new Encriptador();
        private ServiceDebito.ServiceDebito SerDebito = new ServiceDebito.ServiceDebito();


        private int IdDatosDebito
        {
            get
            {
                int IdDD = 0;
                if (ViewState["IdDatosDebito"] != null)
                    IdDD = Convert.ToInt32(ViewState["IdDatosDebito"]);
                return IdDD;
            }

            set
            {
                ViewState["IdDatosDebito"] = value;
            }
        }

        private int IdTitularCuenta
        {
            get
            {
                int IdTC = 0;
                if (ViewState["IdTitularCuenta"] != null)
                    IdTC = Convert.ToInt32(ViewState["IdTitularCuenta"]);
                return IdTC;
            }

            set
            {
                ViewState["IdTitularCuenta"] = value;
            }
        }

        private String EstadoCliente
        {
            get
            {
                String EstadoCliente = String.Empty;
                if (ViewState["EstadoCliente"] != null)
                    EstadoCliente = Convert.ToString(ViewState["EstadoCliente"]);
                return EstadoCliente;
            }

            set
            {
                ViewState["EstadoCliente"] = value;
            }
        }

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

        private String FechaSus
        {
            get
            {
                String fecha = String.Empty;
                if (ViewState["FechaSus"] != null)
                    fecha = Convert.ToString(ViewState["FechaSus"]);
                return fecha;
            }

            set
            {
                ViewState["FechaSus"] = value;
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

        string campos = string.Empty;


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

                if (Session["perfil"].ToString() != "2")
                {
                    this.pnlBanco.Enabled = this.pnlTercero.Enabled = this.txbContrato.Enabled = this.imgBtnGuardar.Enabled = this.imgBtnNuevo.Enabled = false;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.PerfilAutorizado + "');</script>", false);
                    return;
                }

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
                this.ddlTipoCuenta.DataValueField = "pId";


                this.ddlTipoIdentificacionT.DataBind();
                this.ddlTipoIdentificacionC.DataBind();

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
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlTipoIdentificacionC);
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlTipoIdentificacionT);

                DataSet IdFormato = new DataSet();
                String Formato = SerDebito.CanalIngreso(Session["usuario"].ToString(), "");
                System.IO.StringReader xmlSRF = new System.IO.StringReader(Formato);
                IdFormato.ReadXml(xmlSRF);
                this.ddlFormaDebito.DataSource = this.ddlCancelación.DataSource = IdFormato;
                this.ddlFormaDebito.DataTextField = this.ddlCancelación.DataTextField = "VALOR";
                this.ddlFormaDebito.DataValueField = this.ddlCancelación.DataValueField = "ID";
                this.ddlFormaDebito.DataBind();
                this.ddlCancelación.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlFormaDebito);
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlCancelación);

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
        /// Boton que direcciona al metodo guardar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgBtnGuardar_Click(object sender, ImageClickEventArgs e)
        {
            guardar();
        }

        /// <summary>
        /// Carga de nuevo la pantalla de inicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgBtnNuevo_Click(object sender, ImageClickEventArgs e)
        {
            this.gvHistorial.DataSource = this.gvCesion.DataSource = null;
            this.gvHistorial.DataBind();
            this.gvCesion.DataBind();
            Response.Redirect("~/Modulos/Configuracion/ModificacionCliente.aspx?tv=dba%2fcg%2fcmc");

        }

        /// <summary>
        /// Realiza la consulta del contrato para ir al metodo buscar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txbContrato_TextChanged(object sender, EventArgs e)
        {
            limpiar();
            if (UtilidadesWeb.textoEsNumero(this.txbContrato.Text))
                buscar(true);
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
        /// Limpia los elementos de la pantalla
        /// </summary>
        private void limpiar()
        {
            IdDatosDebito = IdTitularCuenta = 0;
            NContrato = String.Empty;
            this.txbDigito.Text = this.txbContratoB.Text = this.txbFechaFinSuspension.Text =
            this.txbFechaInicialSuspesion.Text = this.txbIdentClienteB.Text = this.txbEstadoCont.Text =
            this.txbIdentificacionC.Text = this.txbIdentificacionT.Text = this.txbTipoCliente.Text =
            this.txbNombreCliente.Text = this.txbNombreTercero.Text = this.txbNumCuenta.Text = String.Empty;
            this.chbTercero.Checked = this.chbSuspender.Checked = false;
            this.ddlBanco.SelectedIndex = this.ddlTipoIdentificacionC.SelectedIndex = this.ddlFormaDebito.SelectedIndex =
            this.ddlCancelación.SelectedIndex = this.ddlTipoCuenta.SelectedIndex = this.ddlFechaDebito.SelectedIndex =
            this.ddlEstadoD.SelectedIndex = 0;
            this.imgBtnFechaInicial.Visible = this.imgBtnFechaFin.Visible = this.txbFechaInicialSuspesion.Visible =
            this.txbFechaFinSuspension.Visible = this.lbFechaInicio.Visible = lbFechaFin.Visible =
            this.pnlCesion.Visible = this.pnlBanco.Enabled = false;
            this.gvCesion.DataSource = this.gvHistorial.DataSource = null;
            this.gvHistorial.DataBind();

            MenuTercero();
        }

        /// <summary>
        /// Metodo que se encarga de buscar el contrato en SICO como la bd de debito automatico
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

            this.gvHistorial.DataSource = this.gvCesion.DataSource = null;
            this.gvHistorial.DataBind();
            this.gvCesion.DataBind();

            if (esBusqPorTxb)
            {
                DataSet objCliente = new DataSet();
                String Resultado = String.Empty;

                Resultado = SerDebito.ConsultaCliente(Convert.ToInt32(txbContrato.Text), true, 0, true, Session["usuario"].ToString(), "");

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
                        IdDatosDebito = Convert.ToInt32(objCliente.Tables["ClienteDebito"].Rows[0]["ID"].ToString());
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
                        

                        this.ddlFormaDebito.SelectedValue = objCliente.Tables["ClienteDebito"].Rows[0]["ID_FORMATO_DEBITO"].ToString();
                        MenuTercero();

                        if (this.chbSuspender.Checked == true)
                        {
                            this.lblSuspender.Visible = this.chbSuspender.Visible =
                            this.lbFechaInicio.Visible = this.imgBtnFechaInicial.Visible = this.imgBtnFechaFin.Visible = this.txbFechaInicialSuspesion.Visible =
                            this.lbFechaFin.Visible = this.txbFechaFinSuspension.Visible = true;

                            this.txbFechaInicialSuspesion.Text = Convert.ToDateTime(objCliente.Tables["ClienteDebito"].Rows[0]["FECHA_INICIO_SUSPENSION"].ToString()).ToString("dd/MM/yyyy"); ;
                            this.txbFechaFinSuspension.Text = Convert.ToDateTime(objCliente.Tables["ClienteDebito"].Rows[0]["FECHA_FIN_SUSPENSION"].ToString()).ToString("dd/MM/yyyy");
                        }

                        if (Convert.ToBoolean(objCliente.Tables["ClienteDebito"].Rows[0]["TERCERO"].ToString()) == true)
                        {
                            this.txbNombreTercero.Text = Nombre;
                            this.txbIdentificacionT.Text = Documento;
                            this.ddlTipoIdentificacionT.SelectedValue = TipoDoc;
                        }
                        this.ddlEstadoD.SelectedValue = EstadoCliente = objCliente.Tables["ClienteDebito"].Rows[0]["ESTADO"].ToString();

                        IdTitularCuenta = Convert.ToInt32(objCliente.Tables["ClienteDebito"].Rows[0]["ID_TITULAR_CUENTA"].ToString());

                        if (Convert.ToInt32(this.ddlEstadoD.SelectedItem.Value) == 12 || Convert.ToInt32(this.ddlEstadoD.SelectedItem.Value) == 15)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.Contrato + this.txbContrato.Text + RecursosDebito.EstadoContrato + this.ddlEstadoD.SelectedItem.Text + "');</script>", false);
                            this.lbFormaDebito.Visible = this.ddlCancelación.Visible = true;
                            this.ddlCancelación.SelectedValue = objCliente.Tables["ClienteDebito"].Rows[0]["ID_FORMATO_CANCELACION"].ToString();
                        }
                        else
                        {
                            this.ddlCancelación.Visible = this.ddlCancelación.Visible = false;
                            this.ddlFormaDebito.SelectedValue = objCliente.Tables["ClienteDebito"].Rows[0]["ID_FORMATO_DEBITO"].ToString();
                        }

                        HistorialProcesoUsuario objHPU = new HistorialProcesoUsuario();
                        objHPU.pContrato = NContrato;
                        //objHPU.pNumeroIdentificacion = objCliente.Tables["Titular"].Rows[0]["NUMERO_IDENTIFICACION"].ToString();
                        Historial = new HistorialProcesoUsuarioLN().consultar(objHPU);
                        Historial.Reverse();
                        this.gvHistorial.DataSource = Historial;
                        this.gvHistorial.DataBind();

                        if (Convert.ToInt32(this.ddlEstadoD.SelectedItem.Value) == 2 || Convert.ToInt32(this.ddlEstadoD.SelectedItem.Value) == 5)
                        {
                            ActualizaCliente objAc = new ActualizaCliente();
                            objAc.pContrato = this.txbContrato.Text;
                            objAc.pIdTitularCuenta = IdTitularCuenta;

                            List<EN.Tablas.ActualizaCliente> listaB = new ActualizaClienteLN().consultarDatos(objAc);

                            if (listaB.Count > 0)
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.Contrato + " " + this.txbContrato.Text + " " + RecursosDebito.PendienteActualizacion + "');</script>", false);
                            else
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.Contrato + " " + this.txbContrato.Text + " " + RecursosDebito.ContratoDebitoProceso + "');</script>", false);   

                            this.pnlTercero.Enabled = this.ddlBanco.Enabled = false;
                            return;
                        }

                        this.txbFechaInicioTransaccion.Enabled = this.imgFechaInicioTransaccion.Enabled = this.txbFechaFinTransaccion.Enabled =
                        this.imgFechaFinTransaccion.Enabled = this.imgBuscar.Enabled = true;

                        this.txbFechaInicioTransaccion.Enabled = this.imgFechaInicioTransaccion.Enabled = this.txbFechaFinTransaccion.Enabled =
                        this.imgFechaFinTransaccion.Enabled = this.imgBuscar.Enabled = true;

                        this.pnlBanco.Enabled = this.pnlTercero.Enabled = true;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + objCliente.Tables["Resultado"].Rows[0].ItemArray[0].ToString() + "');</script>", false);

                    }
                    else
                    {
                        this.pnlBanco.Enabled = this.pnlTercero.Enabled = false;
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
        /// Se encarga de actualizar la informacion del cliente 
        /// </summary>
        private void guardar()
        {
            if (ddlEstadoD.SelectedItem.Text == "PRENOTA EN PROCESO" || ddlEstadoD.SelectedItem.Text == "DEBITO EN PROCESO")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.NoCambioEstado + ddlEstadoD.SelectedItem.Text + "');</script>", false);
                return;
            }

            if (ddlEstadoD.SelectedItem.Text == "CANCELADO" && ddlCancelación.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.Formato + "');</script>", false);
                this.ddlCancelación.Focus();
                return;
            }

            if (Convert.ToInt32(this.ddlEstadoD.SelectedItem.Value) == 10 && !this.txbTipoCliente.Text.Equals("Suscriptor") && !this.txbTipoCliente.Text.Equals("Cuotas Por Devolver"))
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.SuspendidoTemporal + "');</script>", false);
                this.ddlEstadoD.Focus();
                return;
            }

            if (Convert.ToInt32(this.ddlEstadoD.SelectedItem.Value) == 10 && this.chbSuspender.Checked == false)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.FechaSuspension + "');</script>", false);
                return;
            }

            if (this.chbSuspender.Checked == true && (this.txbFechaInicialSuspesion.Text == String.Empty && this.txbFechaFinSuspension.Text == String.Empty))
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.FechaSuspension + "');</script>", false);
                return;
            }

            if ((IdDatosDebito <= 0) || (IdTitularCuenta <= 0) || (String.IsNullOrEmpty(NContrato)))
            {
                return;
            }

            TitularCuenta objT = new TitularCuenta();
            if (this.chbTercero.Checked)
            {
                objT.pNombre = this.txbNombreTercero.Text.Trim().ToUpper();
                objT.pNumeroIdentificacion = this.txbIdentificacionT.Text.Trim();
                objT.pTipoIdentificacion = Convert.ToInt32(this.ddlTipoIdentificacionT.SelectedValue);
            }
            else
            {
                objT.pNombre = this.txbNombreCliente.Text.Trim().ToUpper();
                objT.pNumeroIdentificacion = this.txbIdentificacionC.Text.Trim();
                objT.pTipoIdentificacion = Convert.ToInt32(this.ddlTipoIdentificacionC.SelectedValue);
            }

            
            objT.pId = IdTitularCuenta;
            int valorT = 0;
            valorT = new TitularCuentaLN().actualizar(objT);
            DatosDebito objD = new DatosDebito();
            objD.pContrato = this.txbContrato.Text.Trim();
            objD.pDigito = Convert.ToInt32(this.txbDigito.Text.Trim());
            objD.pIdBanco = Convert.ToInt32(this.ddlBanco.SelectedValue);
            objD.pTipoCuenta = Convert.ToInt32(this.ddlTipoCuenta.SelectedValue);
            objD.pIdFormatoCancelacion = Convert.ToInt32(this.ddlCancelación.SelectedValue);
            objD.pNumeroCuenta = this.txbNumCuenta.Text.Trim();
            objD.pTercero = this.chbTercero.Checked;
            objD.pIdTitularCuenta = IdTitularCuenta;
            objD.pId = IdDatosDebito;
            objD.pFechaDebito = Convert.ToInt32(this.ddlFechaDebito.SelectedValue);
            objD.pSuspendido = this.chbSuspender.Checked;
            

            if (this.chbSuspender.Checked == false)
            {
                objD.pEstado = Convert.ToInt32(this.ddlEstadoD.SelectedValue);
                objD.pFechaInicioSus = String.Empty;
                objD.pFechaFinSus = String.Empty;
            }
            else
            {
                DateTime fecha = DateTime.Now.Date;
                objD.pFechaInicioSus = Convert.ToDateTime(this.txbFechaInicialSuspesion.Text).ToString("dd/MM/yyyy");
                objD.pFechaFinSus = Convert.ToDateTime(this.txbFechaFinSuspension.Text).ToString("dd/MM/yyyy");

                if (Convert.ToDateTime(objD.pFechaInicioSus) > fecha)
                {
                    objD.pEstado = Convert.ToInt32(EstadoCliente);
                }
                else
                {
                    objD.pEstado = Convert.ToInt32(this.ddlEstadoD.SelectedValue);
                }
            }


            objD.pIdFormatoDebito = Convert.ToInt32(this.ddlFormaDebito.SelectedValue);
            int valorD = 0;
            valorD = new DatosDebitoLN().actualizar(objD);

            if (valorD > 0)
            {

                #region (INFORMACION PARA LOG)
                campos = string.Concat(objD.pContrato,
                    " con BANCO:", Convert.ToString(this.ddlBanco.SelectedItem).ToUpper(),
                    ", TIPO DE CUENTA:", Convert.ToString(this.ddlTipoCuenta.SelectedItem),
                    ", NUMERO DE CUENTA:", Convert.ToString(objD.pNumeroCuenta),
                    ", FORMATO DE INGRESO:", Convert.ToString(this.ddlFormaDebito.SelectedItem),
                    ", FORMATO DE CANCELACIÓN:", this.ddlCancelación.SelectedIndex == 0 ? "No aplica" : Convert.ToString(this.ddlCancelación.SelectedItem),
                    ", TERCERO:", this.chbTercero.Checked ? "Verdadero" : "Falso",
                    ", NOMBRE:", Convert.ToString(objT.pNombre),
                    ", TIPO DE IDENTIFICACIÓN:", this.chbTercero.Checked ? Convert.ToString(this.ddlTipoIdentificacionT.SelectedItem) : Convert.ToString(this.ddlTipoIdentificacionC.SelectedItem),
                    ", IDENTIFICACIÓN:", Convert.ToString(objT.pNumeroIdentificacion),
                    ", ESTADO DEBITO:", Convert.ToString(this.ddlEstadoD.SelectedItem),
                    ", SUSPENDIDO:", this.chbSuspender.Checked ? "Verdadero" : "Falso",
                    ", FECHA INICIO SUSPENSIÓN:", objD.pFechaInicioSus == String.Empty ? "No aplica" : Convert.ToString(objD.pFechaInicioSus),
                    ", FECHA FIN SUSPENSIÓN:", objD.pFechaFinSus == String.Empty ? "No aplica" : Convert.ToString(objD.pFechaFinSus),
                    ", DIRECCION_IP:", Request.UserHostName.ToString(),
                    ", DÉBITO A PARTIR DEL:",  Convert.ToString(this.ddlFechaDebito.SelectedItem)
                    
                    );
                #endregion
                Log(1, objD.pContrato);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ProcesoExistoso + "');</script>", false);
                limpiar();
                this.txbContrato.Text = String.Empty;

            }
            else
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorProceso + "');</script>", false);

        }

        /// <summary>
        /// Check que define si hay o no tercero para ir al metodo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chbTercero_CheckedChanged(object sender, EventArgs e)
        {
            MenuTercero();
        }

        /// <summary>
        /// Meotod Log que guarda la actualizacion del cliente en la tabla tb_DEB_LOGS_USUARIO
        /// </summary>
        /// <param name="opcion"></param>
        /// <param name="ContratoCliente"></param>
        private void Log(int opcion, String ContratoCliente)
        {
            string fecha = Convert.ToString(DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("H:mm:ss"));
            LogsUsuario objL = new LogsUsuario();
            switch (opcion)
            {
                case 1:
                    objL.pFecha = String.Empty;
                    objL.pUsuario = Session["usuario"].ToString();
                    objL.pDetalle = "Se actualizó el contrato N°: " + campos;
                    objL.pContrato = ContratoCliente;
                    objL.pMovimiento = "ACTUALIZACIÓN";
                    new LogsUsuarioLN().insertar(objL);
                    break;
            }
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

        /// <summary>
        /// Habilita algunos campos en caso de seleccionar el estado CANCELADO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlEstadoD_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (Convert.ToInt32(this.ddlEstadoD.SelectedItem.Value) == 10 && (this.txbTipoCliente.Text.Equals("Suscriptor") || this.txbTipoCliente.Text.Equals("Cuotas Por Devolver")))
            {
                this.lblSuspender.Visible = this.chbSuspender.Visible = true;
            }
            else
            {
                this.lblSuspender.Visible = this.chbSuspender.Visible = this.chbSuspender.Checked =
                this.lbFechaInicio.Visible = this.txbFechaInicialSuspesion.Visible = this.imgBtnFechaInicial.Visible = this.lbFechaFin.Visible =
                this.txbFechaFinSuspension.Visible = this.imgBtnFechaFin.Visible = false;
            }

            if (Convert.ToInt32(this.ddlEstadoD.SelectedItem.Value) == 12 || Convert.ToInt32(this.ddlEstadoD.SelectedItem.Value) == 15)
            {
                this.lbFormaDebito.Visible = this.ddlCancelación.Visible = true;
                this.chbSuspender.Checked = false;
                this.imgBtnFechaInicial.Visible = this.imgBtnFechaFin.Visible = this.txbFechaInicialSuspesion.Visible =
                this.txbFechaFinSuspension.Visible = this.lbFechaInicio.Visible = lbFechaFin.Visible = false;
                this.txbFechaInicialSuspesion.Text = this.txbFechaFinSuspension.Text = String.Empty;
            }
            else
            {
                this.lbFormaDebito.Visible = this.ddlCancelación.Visible = false;
                this.ddlCancelación.SelectedIndex = 0;
            }
        }

        protected void chbSuspender_CheckedChanged(object sender, EventArgs e)
        {
            if (chbSuspender.Checked)
            {
                this.lbFechaInicio.Visible = this.txbFechaInicialSuspesion.Visible = this.imgBtnFechaInicial.Visible = this.lbFechaFin.Visible =
                this.txbFechaFinSuspension.Visible = this.imgBtnFechaFin.Visible = true;
            }
            else
            {
                this.lbFechaInicio.Visible = this.txbFechaInicialSuspesion.Visible = this.imgBtnFechaInicial.Visible = this.lbFechaFin.Visible =
                this.txbFechaFinSuspension.Visible = this.imgBtnFechaFin.Visible = false;
            }
        }

    }
}