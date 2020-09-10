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
using DebitoAutomatico.LN.Utilidades;
using DebitoAutomatico.PS.Codigo;

using System.Data.Odbc;
using System.Configuration;


/// <summary>
/// Autor: Nicolas Alexander Larrotta
/// Fecha Ultima Actualización: 20 de Junio de 2018
/// Observacion: Modulo Cliente Inconsistente
/// </summary>

namespace DebitoAutomatico.PS.Modulos.Configuracion
{
    public partial class IngresarClienteInconsistente : System.Web.UI.Page
    {
        private ServiceDebito.ServiceDebito SerDebito = new ServiceDebito.ServiceDebito();
        string campos = String.Empty;
        int actualizarTitular = 0;
        int actualizarDebito = 0;
        Encriptador objEncriptador = new Encriptador();
        DataSet objCliente = new DataSet();

        DatosDebitoInconsistente objDatosDI = new DatosDebitoInconsistente();
        List<DatosDebitoInconsistente> listaDI = new List<DatosDebitoInconsistente>();
        DatosDebitoInconsistenteLN objlistDI = new DatosDebitoInconsistenteLN();
        TitularCuentaInconsistente objT = new TitularCuentaInconsistente();

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

        private int existeDatosDebito
        {
            get
            {
                int eDD = 0;
                if (ViewState["existeDatosDebito"] != null)
                    eDD = Convert.ToInt32(ViewState["existeDatosDebito"]);
                return eDD;
            }

            set
            {
                ViewState["existeDatosDebito"] = value;
            }
        }

        /// <summary>
        /// Carga los elementos en la pantalla primcipal
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

            if (!IsPostBack)
            {

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
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlTipoIdentificacionT);
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlTipoIdentificacionC);

                DataSet IdBanco = new DataSet();
                String Bancos = SerDebito.IdBanco(Session["usuario"].ToString(), "");
                System.IO.StringReader xmlSRB = new System.IO.StringReader(Bancos);
                IdBanco.ReadXml(xmlSRB);
                this.ddlBanco.DataSource = IdBanco;
                this.ddlBanco.DataTextField = "NOMBRE";
                this.ddlBanco.DataValueField = "ID";
                this.ddlBanco.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlBanco);

                this.ddlTipoInconsistencia.DataSource = new TipoInconsistenciaLN().consultarTipoIn(new TipoInconsistencia());
                this.ddlTipoInconsistencia.DataTextField = "pValor";
                this.ddlTipoInconsistencia.DataValueField = "pId";
                this.ddlTipoInconsistencia.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlTipoInconsistencia);

                this.ddlFormaDebito.DataSource = new TipoFormatoLN().consultarTipoF(new TipoFormato());
                this.ddlFormaDebito.DataTextField = "pValor";
                this.ddlFormaDebito.DataValueField = "pId";
                this.ddlFormaDebito.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlFormaDebito);

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
        /// Carga de nuevo la pagina de inicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgBtnNuevo_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Modulos/Configuracion/ClienteInconsistente.aspx?tv=dba%2fcg%2fcii");
        }

        /// <summary>
        /// Boton que direcciona al metodo de guardar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgBtnGuardar_Click(object sender, ImageClickEventArgs e)
        {
            guardar();
        }

        /// <summary>
        /// Caja de texto donde se valida si el contrato es un numero para lugar ir al metodo buscar
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
        /// Check que define si hay o no tercero para ir al metodo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chbTercero_CheckedChanged(object sender, EventArgs e)
        {
            MenuTercero();
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
        /// Limipa los elementos de la pantalla
        /// </summary>
        private void limpiar()
        {
            existeDatosDebito = 0;
            NContrato = String.Empty;
            this.txbObservaciones.Text = this.txbDigito.Text =
            this.txbContratoB.Text = this.txbIdentClienteB.Text =
            this.txbIdentificacionC.Text = this.txbIdentificacionT.Text =
            this.txbNombreCliente.Text = this.txbNombreTercero.Text = this.txbNumCuenta.Text =
            this.txbTipoCliente.Text = this.txbEstadoCont.Text = String.Empty;
            this.chbTercero.Checked = false;
            this.ddlTipoIdentificacionC.SelectedIndex = this.ddlBanco.SelectedIndex = this.ddlFormaDebito.SelectedIndex =
            this.ddlTipoCuenta.SelectedIndex = this.ddlTipoInconsistencia.SelectedIndex = this.ddlFechaDebito.SelectedIndex = 0;
            MenuTercero();
        }

        /// <summary>
        /// Metodo para buscar el número de contrato en SICO y en debito automatico
        /// </summary>
        /// <param name="esBusqPorTxb"></param>
        private void buscar(bool esBusqPorTxb)
        {
            try
            {
                String Nombre = String.Empty;
                String Documento = String.Empty;
                String TipoDoc = String.Empty;
                String FechaIngreso = String.Empty;
                String FechaFinalizacion = String.Empty;
                String MontoMaximo = String.Empty;

                if (esBusqPorTxb)
                {
                    String Resultado = String.Empty;
                    Resultado = SerDebito.ConsultaCliente(Convert.ToInt32(txbContrato.Text), true, 0, false, Session["usuario"].ToString(), "");

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
                                this.txbIdentificacionC.Text = objCliente.Tables["ClienteSICO"].Rows[0]["NUMERO_DOCUMENTO_CLIENTE"].ToString().TrimStart('0');
                                this.ddlTipoIdentificacionC.SelectedValue = Convert.ToString(UtilidadesWeb.homologarDocumento(objCliente.Tables["ClienteSICO"].Rows[0]["TIPO_DOCUMENTO"].ToString()));
                                this.txbEstadoCont.Text = objCliente.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString();
                                this.txbTipoCliente.Text = objCliente.Tables["ClienteSICO"].Rows[0]["TIPO_CUPO"].ToString();
                                this.txbNombreCliente.Text = objCliente.Tables["ClienteSICO"].Rows[0]["NOMBRE_CLIENTE"].ToString();
                                this.txbDigito.Text = UtilidadesWeb.calculoDigito(txbContrato.Text.Trim());
                            }
                        }
                        catch
                        {

                            if (objCliente.Tables.Contains("ClienteInconsistente"))
                            {
                                ConsultaTitularInconsistente();
                            }
                            else
                            {
                                this.pnlBanco.Enabled = this.pnlTercero.Enabled = true;
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + objCliente.Tables["Resultado"].Rows[0].ItemArray[0].ToString() + "');</script>", false);
                            }
                            return;
                        }

                        if (objCliente.Tables.Contains("ClienteDebito"))
                        {
                            this.imgBtnExportar.Enabled = this.imgBtnGuardar.Enabled = this.pnlBanco.Enabled = this.pnlTercero.Enabled = false;
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + objCliente.Tables["Resultado"].Rows[0].ItemArray[0].ToString() + "');</script>", false);
                            existeDatosDebito = 1;
                            return;
                        }

                        if (objCliente.Tables.Contains("ClienteInconsistente"))
                        {

                            if (objCliente.Tables["ClienteSICO"].Rows[0]["AFINIDAD"].ToString() == "99" || objCliente.Tables["ClienteSICO"].Rows[0]["AFINIDAD"].ToString() == "98") //TERMINACIÓN DE CONTRATO
                            {
                                this.pnlBanco.Enabled = false;
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.Contrato + this.txbContrato.Text + RecursosDebito.TerminacionContrato + "');</script>", false);
                                return;
                            }

                            ConsultaTitularInconsistente();
                        }
                        else
                        {
                            if (objCliente.Tables["Resultado"].Rows[0].ItemArray[1].ToString() == "0")
                            {
                              this.imgBtnExportar.Enabled = this.imgBtnGuardar.Enabled = this.pnlBanco.Enabled = this.pnlTercero.Enabled = false;
                            }
                            else
                            {
                                this.pnlBanco.Enabled = this.pnlTercero.Enabled = true;
                            }
                            
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + objCliente.Tables["Resultado"].Rows[0].ItemArray[0].ToString() + "');</script>", false);
                        }
                    }
                    else
                    {
                        this.pnlBanco.Enabled = false;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + objCliente.Tables["Resultado"].Rows[0].ItemArray[0].ToString() + "');</script>", false);
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + ex.Message.Replace("'", "").Replace("\n", "").Replace("\r", "") + "');</script>", false);
            }

        }

        /// <summary>
        /// Metodo que consulta si el cliente ya existe en la base de datos de clientes inconsistentes
        /// </summary>
        public void ConsultaTitularInconsistente()
        {
            IdDatosDebito = Convert.ToInt32(objCliente.Tables["ClienteInconsistente"].Rows[0]["ID"].ToString());
            NContrato = objCliente.Tables["ClienteInconsistente"].Rows[0]["CONTRATO"].ToString();

            try
            {
                this.ddlBanco.SelectedValue = objCliente.Tables["ClienteInconsistente"].Rows[0]["ID_BANCO"].ToString();
                this.ddlFechaDebito.SelectedValue = objCliente.Tables["ClienteInconsistente"].Rows[0]["FECHA_DEBITO"].ToString();
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + objCliente.Tables["Resultado"].Rows[1].ItemArray[0].ToString() + "');</script>", false);
            }
            this.ddlTipoCuenta.SelectedValue = objCliente.Tables["ClienteInconsistente"].Rows[0]["TIPO_CUENTA"].ToString();
            this.txbNumCuenta.Text = objCliente.Tables["ClienteInconsistente"].Rows[0]["NUMERO_CUENTA"].ToString();
            this.chbTercero.Checked = Convert.ToBoolean(objCliente.Tables["ClienteInconsistente"].Rows[0]["TERCERO"].ToString());
            this.ddlFormaDebito.SelectedValue = objCliente.Tables["ClienteInconsistente"].Rows[0]["FORMATO"].ToString();
            this.ddlTipoInconsistencia.SelectedValue = objCliente.Tables["ClienteInconsistente"].Rows[0]["TIPO_INCONSISTENCIA"].ToString();
            this.txbObservaciones.Text = objCliente.Tables["ClienteInconsistente"].Rows[0]["OBSERVACIONES"].ToString();
            MenuTercero();

            if (this.txbEstadoCont.Text == String.Empty)
            {
                this.txbIdentificacionC.Text = objCliente.Tables["Titular Inconsistente"].Rows[0]["NUMERO_IDENTIFICACION"].ToString();
                this.ddlTipoIdentificacionC.SelectedValue = Convert.ToString(objCliente.Tables["Titular Inconsistente"].Rows[0]["TIPO_IDENTIFICACION"].ToString());
                this.txbNombreCliente.Text = objCliente.Tables["Titular Inconsistente"].Rows[0]["NOMBRE"].ToString();
            }

            if (chbTercero.Checked)
            {
                if (objCliente.Tables.Contains("Titular Inconsistente"))
                {
                    this.txbNombreTercero.Text = objCliente.Tables["Titular Inconsistente"].Rows[0]["NOMBRE"].ToString();
                    this.txbIdentificacionT.Text = objCliente.Tables["Titular Inconsistente"].Rows[0]["NUMERO_IDENTIFICACION"].ToString();
                    this.ddlTipoIdentificacionT.SelectedValue = Convert.ToString(objCliente.Tables["Titular Inconsistente"].Rows[0]["TIPO_IDENTIFICACION"].ToString());
                }
            }
            IdTitularCuenta = Convert.ToInt32(objCliente.Tables["Titular Inconsistente"].Rows[0]["ID"].ToString());
            this.pnlBanco.Enabled = this.pnlTercero.Enabled = true;
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + objCliente.Tables["Resultado"].Rows[0].ItemArray[0].ToString() + "');</script>", false);
        }

        /// <summary>
        /// Metodo que se encarga de gurdar la información en la base de datos
        /// </summary>
        private void guardar()
        {
            try
            {
                actualizarTitular = 0;
                int valorT = 0;
                actualizarDebito = 0;

                if (this.chbTercero.Checked)
                {
                    objT.pNombre = this.txbNombreTercero.Text.Trim();
                    objT.pNumeroIdentificacion = this.txbIdentificacionT.Text.Trim();
                    objT.pTipoIdentificacion = Convert.ToInt32(this.ddlTipoIdentificacionT.SelectedValue);
                }
                else
                {
                    if (this.txbNombreCliente.Text == String.Empty)
                    {
                        objT.pNombre = "CLIENTE SIN INFORMACION";
                        objT.pNumeroIdentificacion = "1";
                        objT.pTipoIdentificacion = 1;
                    }
                    else
                    {
                        objT.pNombre = this.txbNombreCliente.Text.Trim();
                        objT.pNumeroIdentificacion = this.txbIdentificacionC.Text.Trim();
                        objT.pTipoIdentificacion = Convert.ToInt32(this.ddlTipoIdentificacionC.SelectedValue);
                    }
                }

                if (IdDatosDebito > 0)
                {
                    objT.pId = IdTitularCuenta;
                    valorT = new TitularCuentaInconsistenteLN().actualizar(objT);
                }
                else
                {
                    valorT = new TitularCuentaInconsistenteLN().insertar(objT);
                }

                if (valorT <= 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorProceso + "');</script>", false);
                    return;
                }



                DatosDebitoInconsistente objD = new DatosDebitoInconsistente();
                objD.pId = IdDatosDebito;
                objD.pContrato = this.txbContrato.Text.Trim();
                if (this.txbDigito.Text.Trim() == String.Empty)
                    objD.pDigito = 0;
                else
                    objD.pDigito = Convert.ToInt32(this.txbDigito.Text.Trim());
                objD.pIdBanco = Convert.ToInt32(this.ddlBanco.SelectedValue);
                objD.pTipoCuenta = Convert.ToInt32(this.ddlTipoCuenta.SelectedValue);
                objD.pNumeroCuenta = this.txbNumCuenta.Text.Trim();
                objD.pTercero = this.chbTercero.Checked;
                objD.pIdTitularCuenta = valorT;
                objD.pFormato = Convert.ToInt32(this.ddlFormaDebito.SelectedValue);
                objD.pDireccion_Ip = Request.UserHostName.ToString();
                objD.pTipoInconsistencia = Convert.ToInt32(this.ddlTipoInconsistencia.SelectedValue);
                objD.pObservaciones = this.txbObservaciones.Text;
                objD.pFechaDebito = Convert.ToInt32(this.ddlFechaDebito.SelectedValue); 
                objD.pEstado = 1;
                int valorD = 0;

                if (IdDatosDebito > 0)
                {
                    valorD = new DatosDebitoInconsistenteLN().actualizar(objD);
                    actualizarDebito = 1;
                }
                else
                {
                    valorD = new DatosDebitoInconsistenteLN().insertar(objD);
                }

                if (valorD > 0)
                {
                    #region (INFORMACION PARA LOG)
                    campos = string.Concat(objD.pContrato,
                        " con BANCO:", Convert.ToString(this.ddlBanco.SelectedItem).ToUpper(),
                        ", TIPO DE CUENTA:", Convert.ToString(this.ddlTipoCuenta.SelectedItem),
                        ", NÚMERO DE CUENTA:", Convert.ToString(objD.pNumeroCuenta),
                        ", TERCERO:", this.chbTercero.Checked ? "Verdadero" : "Falso",
                        ", NOMBRE:", Convert.ToString(objT.pNombre),
                        ", TIPO DE IDENTIFICACIÓN:", this.chbTercero.Checked ? Convert.ToString(this.ddlTipoIdentificacionT.SelectedItem) : Convert.ToString(this.ddlTipoIdentificacionC.SelectedItem),
                        ", IDENTIFICACIÓN:", Convert.ToString(objT.pNumeroIdentificacion),
                        ", OBSERVACIONES: ", objD.pObservaciones,
                        ", FORMATO DE INGRESO: ", this.ddlFormaDebito.SelectedItem,
                        ", DÉBITO A PARTIR DE: ", this.ddlFechaDebito.SelectedItem
                        
                        );
                    #endregion

                    if (actualizarDebito == 1)
                        Log(2, objD.pContrato);
                    else
                        Log(1, objD.pContrato);


                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ProcesoExistoso + "');</script>", false);
                    limpiar();
                    this.pnlBanco.Enabled = false;
                    this.txbContrato.Text = String.Empty;
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
        /// Botón que se encarga de direccionarse al boton exportar para enviar a prenotificar al cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgBtnExportar_Click(object sender, ImageClickEventArgs e)
        {
            Exportar();
        }

        /// <summary>
        /// Metodo que se encarga de eliminar el contrato de la tabla de clientes inconsistentes
        /// </summary>
        private void eliminar()
        {
            try
            {
                if ((IdDatosDebito <= 0) || (IdTitularCuenta <= 0) || (String.IsNullOrEmpty(NContrato)))
                {
                    return;
                }

                int valorT = 0;
                TitularCuentaInconsistente objT = new TitularCuentaInconsistente();
                objT.pId = IdTitularCuenta;
                valorT = new TitularCuentaInconsistenteLN().borrar(objT);

                int valorD = 0;
                DatosDebitoInconsistente objD = new DatosDebitoInconsistente();
                objD.pId = IdDatosDebito;
                valorD = new DatosDebitoInconsistenteLN().borrar(objD);

                if (valorD == 0 && valorT == 0)
                {
                    #region (INFORMACION PARA LOG)
                    campos = string.Concat(this.txbContrato.Text);
                    #endregion
                    Log(3, this.txbContrato.Text);
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ContratoEliminado + "');</script>", false);
                    limpiar();
                    this.pnlBanco.Enabled = false;
                    this.txbContrato.Text = String.Empty;
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
        /// /Metodo que envia al cliente a prenotificar
        /// </summary>
        private void Exportar()
        {
            try
            {
                if (this.txbNombreCliente.Text == "CLIENTE SIN INFORMACION")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.Contrato + " " + this.txbContrato.Text + " " + RecursosDebito.ContratoNoSICO + "');</script>", false);
                    return;
                }

                if (this.txbTipoCliente.Text.Equals("Suscriptor") || this.txbTipoCliente.Text.Equals("Adjudicado") || this.txbTipoCliente.Text.Equals("Ganador") || this.txbTipoCliente.Text.Equals("CtasXDevolver"))
                {
                    if (this.txbEstadoCont.Text.Equals("Vigente") || this.txbEstadoCont.Text.Equals("Mora") || this.txbEstadoCont.Text.Equals("Reemplazado"))
                    {
                        if (existeDatosDebito == 1)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.Contrato + " " + this.txbContrato.Text + " " + RecursosDebito.ContratoIngresado + "');</script>", false);
                            return;
                        }

                        if ((IdDatosDebito <= 0) || (IdTitularCuenta <= 0) || (String.IsNullOrEmpty(NContrato)))
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.Contrato + " " + this.txbContrato.Text + " " + RecursosDebito.ContratoNoGuardado + "');</script>", false);
                            return;
                        }

                        TitularCuenta objT = new TitularCuenta();
                        if (this.chbTercero.Checked)
                        {
                            objT.pNombre = this.txbNombreTercero.Text.Trim();
                            objT.pNumeroIdentificacion = this.txbIdentificacionT.Text.Trim();
                            objT.pTipoIdentificacion = Convert.ToInt32(this.ddlTipoIdentificacionT.SelectedValue);
                        }
                        else
                        {
                            objT.pNombre = this.txbNombreCliente.Text.Trim();
                            objT.pNumeroIdentificacion = this.txbIdentificacionC.Text.Trim();
                            objT.pTipoIdentificacion = Convert.ToInt32(this.ddlTipoIdentificacionC.SelectedValue);
                        }

                       
                        int valorT = 0;
                        valorT = new TitularCuentaLN().insertar(objT);
                        DatosDebito objD = new DatosDebito();
                        objD.pContrato = this.txbContrato.Text.Trim();
                        objD.pDigito = Convert.ToInt32(this.txbDigito.Text.Trim());
                        objD.pIdBanco = Convert.ToInt32(this.ddlBanco.SelectedValue);
                        objD.pIdFormatoDebito = Convert.ToInt32(this.ddlFormaDebito.SelectedValue);
                        objD.pTipoCuenta = Convert.ToInt32(this.ddlTipoCuenta.SelectedValue);
                        objD.pFechaDebito = Convert.ToInt32(this.ddlFechaDebito.SelectedValue);
                        objD.pNumeroCuenta = this.txbNumCuenta.Text.Trim();
                        objD.pTercero = this.chbTercero.Checked;
                        objD.pIdTitularCuenta = valorT;
                        objD.pDireccionIp = Request.UserHostName.ToString();
                        objD.pFechaInicioSus = String.Empty;
                        objD.pEstado = 1;
                        objD.pIntentos = 0;

                        DataSet objCliente = new DataSet();
                        String Resultado = String.Empty;

                        int varloD = 0;

                        varloD = new DatosDebitoLN().insertar(objD);

                        if (varloD > 0)
                        {
                            #region (INFORMACION PARA LOG)
                            campos = string.Concat(objD.pContrato, 
                                " con BANCO:", Convert.ToString(this.ddlBanco.SelectedItem).ToUpper(), 
                                ", TIPO DE CUENTA:",Convert.ToString(this.ddlTipoCuenta.SelectedItem), 
                                ", NUMERO DE CUENTA:",Convert.ToString(objD.pNumeroCuenta),
                                ", TERCERO:", this.chbTercero.Checked ? "Verdadero" : "Falso",
                                ", NOMBRE:", Convert.ToString(objT.pNombre),
                                ", TIPO DE IDENTIFICACIÓN:", this.chbTercero.Checked ? Convert.ToString(this.ddlTipoIdentificacionT.SelectedItem) : Convert.ToString(this.ddlTipoIdentificacionC.SelectedItem),
                                ", IDENTIFICACIÓN:", Convert.ToString(objT.pNumeroIdentificacion),
                                ", FORMATO DE INGRESO: ", this.ddlFormaDebito.SelectedItem,
                                ", DÉBITO A PARTIR DE: ", this.ddlFechaDebito.SelectedItem
                                
                                );
                            #endregion
                            Log(4, objD.pContrato);
                            eliminar();
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ContratoExportado + "');</script>", false);
                            limpiar();
                            this.pnlBanco.Enabled = false;
                            this.txbContrato.Text = String.Empty;
                        }
                        else
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorProceso + "');</script>", false);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.Contrato + " " + this.txbContrato.Text + " " + RecursosDebito.ContratoNoSICO + "');</script>", false);
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.Contrato + " " + this.txbContrato.Text + " " + RecursosDebito.ContratoNoSICO + "');</script>", false);
                    return;
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + ex.Message.Replace("'", "").Replace("\n", "").Replace("\r", "") + "');</script>", false);
            }
        }

        /// <summary>
        ///opcion 1:Crear Inconsistente,2:Actualizar Inconsistente,3:Eliminar Inconsistente,4:Crear para prenotificar 
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
                    objL.pFecha = fecha;
                    objL.pUsuario = Session["usuario"].ToString();
                    objL.pDetalle = "Se creó el contrato Inconsistente N°: " + campos;
                    objL.pContrato = ContratoCliente;
                    objL.pMovimiento = "CREACIÓN";
                    new LogsUsuarioLN().insertar(objL);
                    break;
                case 2:
                    objL.pFecha = fecha;
                    objL.pUsuario = Session["usuario"].ToString();
                    objL.pDetalle = "Se actualizó el contrato Inconsistente N°: " + campos;
                    objL.pContrato = ContratoCliente;
                    objL.pMovimiento = "ACTUALIZACIÓN";
                    new LogsUsuarioLN().insertar(objL);
                    break;
                case 3:
                    objL.pFecha = fecha;
                    objL.pUsuario = Session["usuario"].ToString();
                    objL.pDetalle = "Se eliminó el contrato Inconsistente N°: " + campos;
                    objL.pContrato = ContratoCliente;
                    objL.pMovimiento = "ELIMINACIÓN";
                    new LogsUsuarioLN().insertar(objL);
                    break;
                case 4:
                    objL.pFecha = fecha;
                    objL.pUsuario = Session["usuario"].ToString();
                    objL.pDetalle = "Se creo el contrato N°: " + campos;
                    objL.pContrato = ContratoCliente;
                    objL.pMovimiento = "CREACIÓN";
                    new LogsUsuarioLN().insertar(objL);
                    break;
            }
        }

    }
}