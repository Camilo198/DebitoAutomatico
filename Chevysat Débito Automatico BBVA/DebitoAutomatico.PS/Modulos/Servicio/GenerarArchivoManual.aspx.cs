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


using System.Data.Odbc;
using System.Configuration;

/// <summary>
/// Autor: Nicolas Alexander Larrotta
/// Fecha Ultima Actualización: 20 de Junio de 2018
/// Observacion: Modulo Generar Archivo Manual
/// </summary>
/// 
/// <summary>
/// Autor: Maikol Steven Ramirez
/// Fecha Ultima Actualización: 04 de Marzo de 2020
/// Observacion: Se agrega proceso de Prejurídica y convenio BBVA
/// </summary>

namespace DebitoAutomatico.PS.Modulos.Servicio
{
    public partial class GenerarArchivoManual : System.Web.UI.Page
    {
        private ServiceDebito.ServiceDebito SerDebito = new ServiceDebito.ServiceDebito();
        Dictionary<String, String> contratosDeb = new Dictionary<String, String>();
        DataSet dsConsulta = new DataSet();


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
                DataSet Idfechas = new DataSet();
                String Fechas = SerDebito.FechaDebito(Session["usuario"].ToString(), "");
                System.IO.StringReader xmlFd = new System.IO.StringReader(Fechas);
                Idfechas.ReadXml(xmlFd);
                this.ddlFechaDebito.DataSource = Idfechas;
                this.ddlFechaDebito.DataTextField = "VALOR";
                this.ddlFechaDebito.DataValueField = "ID";
                this.ddlFechaDebito.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlFechaDebito);

                Banco objB = new Banco();
                objB.pDebito = true;
                objB.pActivo = true;
                this.ddlBancoDebito.DataSource = new BancoLN().consultarBanco(objB);
                this.ddlBancoDebito.DataTextField = "pNombre";
                this.ddlBancoDebito.DataValueField = "pId";
                this.ddlBancoDebito.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlBancoDebito);

                this.ddlTipoIdentificacionT.DataSource =
                this.ddlTipoIdentificacionC.DataSource =
                new TipoDocumentoLN().consultarTipoD(new TipoDocumento());
                this.ddlTipoCuenta.DataSource = new TipoCuentaLN().consultarTipoC(new TipoCuenta());

                this.ddlTipoIdentificacionT.DataTextField =
                this.ddlTipoIdentificacionC.DataTextField = "pAbreviatura";
                this.ddlTipoCuenta.DataTextField = "pValor";

                this.ddlTipoIdentificacionT.DataValueField =
                this.ddlTipoIdentificacionC.DataValueField = "pId";
                this.ddlTipoCuenta.DataValueField = "pId";

                this.ddlTipoIdentificacionT.DataBind();
                this.ddlTipoIdentificacionC.DataBind();
                this.ddlTipoCuenta.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlTipoCuenta);
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlTipoIdentificacionC);
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlTipoIdentificacionT);

                this.ddlBanco.DataSource = new BancoLN().consultarBanco(new Banco());
                this.ddlBanco.DataTextField = "pNombre";
                this.ddlBanco.DataValueField = "pId";
                this.ddlBanco.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlBanco);

                this.ddlEstadoD.DataSource = new EstadosClientesLN().consultarEstado(new EstadosClientes());
                this.ddlEstadoD.DataTextField = "pValor";
                this.ddlEstadoD.DataValueField = "pId";
                this.ddlEstadoD.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlEstadoD);

                ArchivoManual objEntidad = new ArchivoManual();
                objEntidad.pAutorizar = false;
                gvDebitos.DataSource = new ArchivoManualLN().consultar(objEntidad);
                gvDebitos.DataBind();

                this.lbNManual.Text = gvDebitos.Rows.Count.ToString();

                if (gvDebitos.Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ArchivoManualPendiente + "');</script>", false);
                    this.pnlDebito.Visible = this.pnlGrilla.Visible = this.btnGenerarArchivo.Visible = true;
                }
            }
        }

        /// <summary>
        /// Agregar el contrato en la tabla de archivos manuales para mostrarlo en el gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            CargaConsultaManual();

            if (dsConsulta.Tables.Contains("TablaManual"))
            {
                if (dsConsulta.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt32(dsConsulta.Tables[0].Rows[0]["VALOR_PARCIAL"].ToString()) > 0)
                    {
                        if (Convert.ToInt32(this.txbValorD.Text) > Convert.ToInt32(dsConsulta.Tables[0].Rows[0]["VALOR_PARCIAL"].ToString()))
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ValorMaximo + dsConsulta.Tables[0].Rows[0]["VALOR_PARCIAL"].ToString() + "');</script>", false);
                            this.txbValorD.Text = dsConsulta.Tables[0].Rows[0]["VALOR_PARCIAL"].ToString();
                            return;
                        }
                    }
                }
            }

            foreach (GridViewRow dtgItem in this.gvDebitos.Rows)
            {
                if (Convert.ToString(gvDebitos.Rows[dtgItem.RowIndex].Cells[1].Text).Equals(this.txbContrato.Text))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ContratoManualYaExiste + "');</script>", false);
                    return;
                }

                Int32 Estado = Convert.ToInt32(gvDebitos.Rows[dtgItem.RowIndex].Cells[4].Text);

                if (Estado == 1) //Prenota
                {
                    if (Estado != Convert.ToInt32(this.ddlEstadoD.SelectedValue))
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ArchivosPendientes + this.ddlEstadoD.SelectedItem + "');</script>", false);
                        return;
                    }
                }

                if (Estado == 4 || Estado == 8) //Debito
                {
                    if (Convert.ToInt32(this.ddlEstadoD.SelectedValue) == 1)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ArchivosPendientes + this.ddlEstadoD.SelectedItem + "');</script>", false);
                        return;
                    }
                }
            }

            ArchivoManualLN objAM = new ArchivoManualLN();
            ArchivoManual objEntidad = new ArchivoManual();
            objEntidad.pAutorizar = false;
            objEntidad.pContrato = Convert.ToInt32(this.txbContrato.Text);

            if (chbTercero.Checked)
                objEntidad.pNombre = this.txbNombreTercero.Text;
            else
                objEntidad.pNombre = this.txbNombreCliente.Text;

            objEntidad.pValor = this.txbValorD.Text;
            objEntidad.pEstadoCliente = Convert.ToString(this.ddlEstadoD.SelectedValue);
            int valor = objAM.insertar(objEntidad);
            if (valor <= 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorProceso + "');</script>", false);
                return;
            }

            ArchivoManual objEntidad1 = new ArchivoManual();
            objEntidad1.pAutorizar = false;
            gvDebitos.DataSource = new ArchivoManualLN().consultar(objEntidad1);
            gvDebitos.DataBind();
            this.lbNManual.Text = gvDebitos.Rows.Count.ToString();

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ContratoManual + "');</script>", false);

            if (this.gvDebitos.Rows.Count > 0)
            {
                this.pnlGrilla.Visible = true;
                this.btnGenerarArchivo.Visible = true;
            }
        }

        /// <summary>
        /// Valida el numero de contrato por la rutina textoEsNumero para lugar ir al metodo buscar
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
        /// Redirecciona la pagina de nuevo para limpiar todos los elementos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgBtnNuevo_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Modulos/Servicio/GenerarArchivoManual.aspx");
        }

        /// <summary>
        /// Habilita el boton Agregar Contrato
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlBancoDebito_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlBancoDebito.SelectedIndex == 0)
            {
                this.btnAgregar.Enabled = false;
            }
            else if (this.ddlBancoDebito.SelectedIndex != 0)
            {
                this.btnAgregar.Enabled = true;
            }
        }

        /// <summary>
        /// Limpia la pantalla
        /// </summary>
        private void limpiar()
        {
            NContrato = String.Empty;

            this.txbDigito.Text = this.txbContratoB.Text =
            this.txbIdentClienteB.Text = this.txbIdentificacionC.Text = this.txbIdentificacionT.Text =
            this.txbNombreCliente.Text = this.txbNombreTercero.Text = this.txbNumCuenta.Text =
            this.txbTipoCliente.Text = this.txbEstadoCont.Text =
            this.txbValorD.Text = String.Empty;
            this.chbTercero.Checked = false;
            this.ddlBanco.SelectedIndex = this.ddlTipoCuenta.SelectedIndex = this.ddlTipoIdentificacionC.SelectedIndex =
            this.ddlFechaDebito.SelectedIndex = this.ddlEstadoD.SelectedIndex = 0;

            MenuTercero();
        }

        /// <summary>
        /// Busqueda del contrato en SICO y en debito automatico
        /// </summary>
        /// <param name="esBusqPorTxb"></param>
        private void buscar(bool esBusqPorTxb)
        {
            try
            {
                if (esBusqPorTxb)
                {
                    DataSet objCliente = new DataSet();
                    String Resultado = String.Empty;
                    DatosDebito objDatosD = new DatosDebito();

                    Resultado = SerDebito.ConsultaCliente(Convert.ToInt32(txbContrato.Text), true, 0, false, Session["usuario"].ToString(), "");
                    System.IO.StringReader xmlSR = new System.IO.StringReader(Resultado);
                    objCliente.ReadXml(xmlSR);

                    if (objCliente.Tables["ClienteSICO"].Rows.Count > 0)
                    {
                        switch (objCliente.Tables["ClienteSICO"].Rows[0]["TIPO_CUPO"].ToString())
                        {
                            case "Suscriptor":
                                if (!((objCliente.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString().Equals("Vigente") || objCliente.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString().Equals("Mora") || objCliente.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString().Equals("Reemplazado") || objCliente.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString().Equals("Prejurídica"))))
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.Contrato + " " + txbContrato.Text.Trim() + " se encuentra en estado: " + objCliente.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString() + "');</script>", false);
                                }
                                break;
                            case "Adjudicado":
                                if (!((objCliente.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString().Equals("Vigente") || objCliente.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString().Equals("Mora") || objCliente.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString().Equals("Reemplazado") || objCliente.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString().Equals("Prejurídica"))))
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.Contrato + " " + txbContrato.Text.Trim() + " se encuentra en estado: " + objCliente.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString() + "');</script>", false);
                                }
                                break;
                            case "Ganador":
                                if (!((objCliente.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString().Equals("Vigente") || objCliente.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString().Equals("Mora") || objCliente.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString().Equals("Reemplazado") || objCliente.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString().Equals("Prejurídica"))))
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.Contrato + " " + txbContrato.Text.Trim() + " se encuentra en estado: " + objCliente.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString() + "');</script>", false);
                                }
                                break;
                            case "CtasXDevolver":
                                if (!((objCliente.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString().Equals("Vigente") || objCliente.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString().Equals("Mora") || objCliente.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString().Equals("Reemplazado") || objCliente.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString().Equals("Prejurídica"))))
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.Contrato + " " + txbContrato.Text.Trim() + " se encuentra en estado: " + objCliente.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString() + "');</script>", false);
                                }
                                break;
                            default:
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.Contrato + " " + txbContrato.Text.Trim() + " se encuentra en estado: " + UtilidadesWeb.homologarEstado(objCliente.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString()) + "');</script>", false);
                                break;
                        }

                        #region DÍA HABIL

                        Fechas ObjFechas = new Fechas();
                        ObjFechas.pId = Convert.ToInt32(objCliente.Tables["ClienteDebito"].Rows[0]["FECHA_DEBITO"].ToString());
                        
                        List<Fechas> ListaFechas = new FechasLN().consultarFechas(ObjFechas);

                        if (ListaFechas.Count > 0)
                        {
                            Fechas ObjFec = new Fechas();
                            ObjFec = ListaFechas[0];
                            
                            int diahoyhabil = new LeerCalendario().diahabil();

                            if (ObjFec.pDia > diahoyhabil)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.DiaHabil + " " + ObjFec.pValor + "');</script>", false);
                            }
                        }

                        #endregion


                        if (objCliente.Tables["ClienteSICO"].Rows.Count > 0)
                        {
                            this.txbIdentificacionC.Text = objCliente.Tables["ClienteSICO"].Rows[0]["NUMERO_DOCUMENTO_CLIENTE"].ToString().TrimStart('0');
                            this.ddlTipoIdentificacionC.SelectedValue = Convert.ToString(UtilidadesWeb.homologarDocumento(objCliente.Tables["ClienteSICO"].Rows[0]["TIPO_DOCUMENTO"].ToString()));
                            this.txbEstadoCont.Text = objCliente.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString();
                            this.txbTipoCliente.Text = objCliente.Tables["ClienteSICO"].Rows[0]["TIPO_CUPO"].ToString();
                            this.txbNombreCliente.Text = objCliente.Tables["ClienteSICO"].Rows[0]["NOMBRE_CLIENTE"].ToString();
                        }


                        this.txbDigito.Text = UtilidadesWeb.calculoDigito(this.txbContrato.Text);

                        if (objCliente.Tables.Contains("ClienteInconsistente"))
                        {
                            this.pnlBanco.Enabled = this.pnlTercero.Enabled = false;
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('Contrato N°: " + this.txbContrato.Text + " ya fue ingresado como Débito Automático Inconsistente');</script>", false);
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
                            MenuTercero();

                            if (Convert.ToBoolean(objCliente.Tables["ClienteDebito"].Rows[0]["TERCERO"].ToString()) == true)
                            {
                                this.txbNombreTercero.Text = objCliente.Tables["Titular"].Rows[0]["NOMBRE"].ToString();
                                this.txbIdentificacionT.Text = objCliente.Tables["Titular"].Rows[0]["NUMERO_IDENTIFICACION"].ToString();
                                this.ddlTipoIdentificacionT.SelectedValue = objCliente.Tables["Titular"].Rows[0]["TIPO_IDENTIFICACION"].ToString();
                            }

                            this.ddlEstadoD.SelectedValue = objCliente.Tables["ClienteDebito"].Rows[0]["ESTADO"].ToString();

                            dsConsulta.Clear();
                            if (this.ddlEstadoD.SelectedItem.Text == "PRENOTA")
                            {
                                this.pnlDebito.Visible = this.btnAgregar.Visible = true;
                                this.lblValorDebitar.Visible = this.txbValorD.Visible = this.imgBuscar.Visible = false;

                                if (this.gvDebitos.Rows.Count > 0)
                                {
                                    this.btnGenerarArchivo.Visible = true;
                                }
                            }
                            else if (this.ddlEstadoD.SelectedItem.Text == "DEBITO" || this.ddlEstadoD.SelectedItem.Text == "DEBITADO")
                            {
                                //this.txbFechaProceso.Text = DateTime.Now.ToString("yyyy/MM/dd");
                                //ArchivoManual objArch = new ArchivoManual();

                                //objArch.pContrato = Convert.ToInt32(this.txbContrato.Text);
                                //objArch.pFecha = UtilidadesWeb.ConvertToJuliana(this.txbFechaProceso.Text);
                                //objArch.pBanco = this.ddlBanco.SelectedValue;
                                //dsConsulta = new ArchivoManualLN().ConsultarValorDebito(objArch);
                                CargaConsultaManual();
                                if (dsConsulta.Tables[0].Rows.Count > 0)
                                {
                                    this.txbValorD.Text = dsConsulta.Tables[0].Rows[0]["VALOR_CUOTA"].ToString();
                                }

                                this.btnAgregar.Visible = this.lblValorDebitar.Visible = this.txbValorD.Visible = this.imgBuscar.Visible = this.pnlDebito.Visible = true;

                                if (this.gvDebitos.Rows.Count > 0)
                                {
                                    this.btnGenerarArchivo.Visible = true;
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ArchivoManual + " ');</script>", false);
                                this.btnAgregar.Visible = this.lblValorDebitar.Visible = this.txbValorD.Visible = this.imgBuscar.Visible = false;
                            }

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
                    }
                }
            }
            catch (Exception)
            {
                this.pnlBanco.Enabled = false;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ContratoNoExiste + " " + this.txbContrato.Text + "');</script>", false);
                return;
            }
        }

        public void CargaConsultaManual()
        {
            this.txbFechaProceso.Text = DateTime.Now.ToString("dd/MM/yyyy");

            ArchivoManual objArch = new ArchivoManual();
            objArch.pContrato = Convert.ToInt32(this.txbContrato.Text);
            objArch.pFecha = Convert.ToDateTime(this.txbFechaProceso.Text).ToString("yyMMdd");
            objArch.pBanco = this.ddlBanco.SelectedValue;
            dsConsulta = new ArchivoManualLN().ConsultarValorDebito(objArch);
        }

        /// <summary>
        /// Valida si el cliente tiene un tercero para habilitar o deshabilitar el panel
        /// </summary>
        private void MenuTercero()
        {
            if (this.chbTercero.Checked)
                this.pnlTercero.Visible = true;
            else
                this.pnlTercero.Visible = false;
        }

        /// <summary>
        /// Genera el archivo manual de acuerdo a lo que exista en el gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGenerarArchivo_Click(object sender, EventArgs e)
        {
 
            ArchivoManual objEntidad = new ArchivoManual();
            String Estado = String.Empty;
            foreach (GridViewRow dtgItem in this.gvDebitos.Rows)
            {
                Estado = Convert.ToString(gvDebitos.Rows[dtgItem.RowIndex].Cells[4].Text);
            }

            if (Estado == "1") //Arhivo manual para prenota
            {
                Convenio objC = new Convenio();
                objC.pIdBancoDebito = Convert.ToInt32(this.ddlBancoDebito.SelectedValue);
                objC.pIdPrenota = "S";
                objC.pOperacion = TiposConsultas.CONSULTAR;
                ConveniosBancosDebito = new ConvenioLN().consultar(objC);

                ArrayList codBancos = new ArrayList();

                foreach (Convenio objBal in ConveniosBancosDebito)
                {
                    if (objBal.pIdPrenota == "S")
                    {
                        codBancos.Add(objBal.pIdBanco);
                    }
                }

                bool Manual = true;
                String men = String.Empty;
                GenerarPrenota prenota = new GenerarPrenota();
                men = prenota.ServicioPrenota(Convert.ToInt32(this.ddlBancoDebito.SelectedValue), Session["usuario"].ToString(), this.txbFechaProceso.Text, codBancos, Manual);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + men + "');</script>", false);
            }
            else if (Estado == "4" || Estado == "8") //Arhivo manual para debito y debitado
            {
                Convenio objC = new Convenio();
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

                if (!String.IsNullOrEmpty(this.txbFechaProceso.Text))
                {
                    if (DateTime.Now > Convert.ToDateTime(this.txbFechaProceso.Text).AddHours(24))
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('Fecha Incorrecta!!!');</script>", false);
                        return;
                    }
                }

                ArrayList men = new ArrayList();
                GenerarDebito debito = new GenerarDebito();
                String FechaProceso = Convert.ToString(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                men = debito.ServicioDebito(Convert.ToInt32(this.ddlBancoDebito.SelectedValue), this.ddlBancoDebito.SelectedItem.ToString(), Session["usuario"].ToString(), codBancos, false, false, false, false, "AñoDebito", "MesaDebitar", this.txbFechaProceso.Text,
                                            true, new Dictionary<String, String>(), FechaProceso);
                foreach (String Mensaje in men)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + Mensaje + "');</script>", false);
                }
            }

            //Consulta los archivos manuales realizados y los elimina de la tabla
            ArchivoManual objAM = new ArchivoManual();
            objAM.pAutorizar = true;
            List<EN.Tablas.ArchivoManual> listArch = new ArchivoManualLN().consultar(objAM);

            if (listArch.Count > 0)
            {
                ArchivoManual objAMObj = new ArchivoManual();

                for (int i = 0; i < listArch.Count; i++)
                {
                    objAMObj = listArch[i];

                    objAM.pContrato = objAMObj.pContrato;
                    int valor = new ArchivoManualLN().borrar(objAM);
                }
            }

            //Consulta solo los registros que quedaron pendientes
            objEntidad.pAutorizar = false;
            gvDebitos.DataSource = new ArchivoManualLN().consultar(objEntidad);
            gvDebitos.DataBind();
            limpiar();
            this.txbContrato.Text = String.Empty;
            if (this.gvDebitos.Rows.Count == 0)
                this.pnlDebito.Visible = this.pnlGrilla.Visible = false;

        }

        /// <summary>
        /// Elimina el registro de la tabla archivo manual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDebitos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Eliminar"))
            {
                //eliminar
                ArchivoManualLN objAM = new ArchivoManualLN();
                ArchivoManual objEntidad = new ArchivoManual();
                objEntidad.pContrato = Convert.ToInt32(HttpUtility.HtmlDecode(this.gvDebitos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[1].Text));
                int valor = objAM.borrar(objEntidad);
                if (valor > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorProceso + "');</script>", false);
                    return;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ContratoManualEliminado + "');</script>", false);
                }

                ArchivoManual objEntidad1 = new ArchivoManual();
                gvDebitos.DataSource = new ArchivoManualLN().consultar(objEntidad1);
                gvDebitos.DataBind();

                if (this.gvDebitos.Rows.Count > 0)
                    this.btnGenerarArchivo.Visible = true;
                else
                    this.btnGenerarArchivo.Visible = false;
            }
        }

        protected void imgBuscar_Click(object sender, ImageClickEventArgs e)
        {
            ArchivoManual objArch = new ArchivoManual();

            objArch.pContrato = Convert.ToInt32(this.txbContrato.Text);
            objArch.pFecha = Convert.ToDateTime(this.txbFechaProceso.Text).ToString("yyMMdd");
            objArch.pBanco = this.ddlBanco.SelectedValue;

            dsConsulta = new ArchivoManualLN().ConsultarValorDebito(objArch);

            if (dsConsulta.Tables[0].Rows.Count > 0)
            {
                this.txbValorD.Text = dsConsulta.Tables[0].Rows[0]["VALOR_CUOTA"].ToString();
            }
        }

    }
}