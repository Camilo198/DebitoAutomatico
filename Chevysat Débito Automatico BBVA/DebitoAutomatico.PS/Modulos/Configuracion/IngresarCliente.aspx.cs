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
using System.Data.SqlClient;
using System.Reflection;
using System.Text.RegularExpressions;


/// <summary>
/// Autor: Nicolas Alexander Larrotta
/// Fecha Ultima Actualización: 20 de Junio de 2018
/// Observacion: Modulo Ingreso Cliente Debito Automatico
/// </summary>

namespace DebitoAutomatico.PS.Modulos.Configuracion
{

    public partial class IngresarCliente : System.Web.UI.Page
    {
        private ServiceDebito.ServiceDebito SerDebito = new ServiceDebito.ServiceDebito();


        string campos = string.Empty;
        Banco ObjBan = new Banco();
        Encriptador objEncriptador = new Encriptador();

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

        private int Estado
        {
            get
            {
                int Estado = 0;
                if (ViewState["Estado"] != null)
                    Estado = Convert.ToInt32(ViewState["Estado"]);
                return Estado;
            }

            set
            {
                ViewState["Estado"] = value;
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
                DataSet objCliente = new DataSet();

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

            if (Session["perfil"].ToString() != "5")
            {
                this.lbTercero.Visible = this.chbTercero.Visible = true;
            }

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>habilitarControles();</script>", false);

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


                DataSet IdBanco = new DataSet();
                String Bancos = SerDebito.IdBanco(Session["usuario"].ToString(), "");
                System.IO.StringReader xmlSRB = new System.IO.StringReader(Bancos);
                IdBanco.ReadXml(xmlSRB);
                this.ddlBanco.DataSource = IdBanco;
                this.ddlBanco.DataTextField = "NOMBRE";
                this.ddlBanco.DataValueField = "ID";
                this.ddlBanco.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlBanco);

                DataSet IdFormato = new DataSet();
                String Formato = SerDebito.CanalIngreso(Session["usuario"].ToString(), "");
                System.IO.StringReader xmlSRF = new System.IO.StringReader(Formato);
                IdFormato.ReadXml(xmlSRF);
                this.ddlFormaDebito.DataSource = IdFormato;
                this.ddlFormaDebito.DataTextField = "VALOR";
                this.ddlFormaDebito.DataValueField = "ID";
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

            }
        }

        /// <summary>
        /// Redirecciona la pagina de nuevo para limpiar todos los elementos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgBtnNuevo_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Modulos/Configuracion/IngresarCliente.aspx?tv=dba%2fcg%2fcgc");
        }

        /// <summary>
        /// Boton para ir al meotodo guardar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgBtnGuardar_Click(object sender, ImageClickEventArgs e)
        {
            guardar();
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
        /// Limipa los elementos de la pantalla
        /// </summary>
        private void limpiar()
        {
            IdDatosDebito = IdTitularCuenta = 0;
            NContrato = String.Empty;
            this.txbDigito.Text = this.txbContratoB.Text =
            this.txbIdentClienteB.Text = this.txbIdentificacionC.Text = this.txbIdentificacionT.Text =
            this.txbEstadoCont.Text = this.txbTipoCliente.Text =
            this.txbNombreCliente.Text = this.txbNombreTercero.Text = this.txbNumCuenta.Text = String.Empty;
            this.chbTercero.Checked = false;
            this.ddlFormaDebito.SelectedIndex = this.ddlBanco.SelectedIndex = this.ddlFechaDebito.SelectedIndex =
            this.ddlTipoCuenta.SelectedIndex = this.ddlTipoIdentificacionC.SelectedIndex = 0;
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

            DataSet objCliente = new DataSet();
            String Resultado = String.Empty;
            DatosDebito objDatosD = new DatosDebito();

            Resultado = SerDebito.ConsultaCliente(Convert.ToInt32(txbContrato.Text), true, 0, false, Session["usuario"].ToString(), "");

            System.IO.StringReader xmlSR = new System.IO.StringReader(Resultado);
            objCliente.ReadXml(xmlSR);

            if (esBusqPorTxb)
            {
                if (objCliente.Tables.Count > 0)
                {
                    try
                    {
                        if (objCliente.Tables["ClienteSICO"].Rows.Count > 0)
                        {
                            this.txbNombreCliente.Text = objCliente.Tables["ClienteSICO"].Rows[0]["NOMBRE_CLIENTE"].ToString();
                            this.txbIdentificacionC.Text = objCliente.Tables["ClienteSICO"].Rows[0]["NUMERO_DOCUMENTO_CLIENTE"].ToString().TrimStart('0');
                            this.ddlTipoIdentificacionC.SelectedValue = Convert.ToString(UtilidadesWeb.homologarDocumento(objCliente.Tables["ClienteSICO"].Rows[0]["TIPO_DOCUMENTO"].ToString()));
                            this.txbEstadoCont.Text = objCliente.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString();
                            this.txbTipoCliente.Text = objCliente.Tables["ClienteSICO"].Rows[0]["TIPO_CUPO"].ToString();


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
                        }
                    }
                    catch
                    {
                        this.pnlBanco.Enabled = false;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + objCliente.Tables["Resultado"].Rows[0].ItemArray[0].ToString() + "');</script>", false);
                        return;
                    }

                    this.txbDigito.Text = UtilidadesWeb.calculoDigito(txbContrato.Text.Trim());

                    if (objCliente.Tables.Contains("ClienteDebito"))
                    {
                        IdDatosDebito = Convert.ToInt32(objCliente.Tables["ClienteDebito"].Rows[0]["ID"].ToString());
                        IdTitularCuenta = Convert.ToInt32(objCliente.Tables["ClienteDebito"].Rows[0]["ID_TITULAR_CUENTA"].ToString());
                        Estado = Convert.ToInt32(objCliente.Tables["ClienteDebito"].Rows[0]["ESTADO"].ToString());

                        if (Convert.ToInt32(objCliente.Tables["ClienteDebito"].Rows[0]["ESTADO"].ToString()) == 15)
                        {
                            this.pnlBanco.Enabled = this.pnlTercero.Enabled = true;
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.Contrato + this.txbContrato.Text + RecursosDebito.EstadoContrato + "CANCELADO POR CESIÓN."+ "');</script>", false);
                            return;
                        }

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
                        this.ddlFormaDebito.SelectedValue = objCliente.Tables["ClienteDebito"].Rows[0]["ID_FORMATO_DEBITO"].ToString();
                        this.chbTercero.Checked = Convert.ToBoolean(objCliente.Tables["ClienteDebito"].Rows[0]["TERCERO"].ToString());
                        

                        MenuTercero();

                        if (chbTercero.Checked)
                        {
                            this.txbNombreTercero.Text = Nombre;
                            this.txbIdentificacionT.Text = Documento;
                            this.ddlTipoIdentificacionT.SelectedValue = TipoDoc;
                        }

                        if (Session["perfil"].ToString() == "5")
                        {
                            this.pnlBanco.Enabled = this.pnlTercero.Enabled = true;
                        }
                        else
                        {
                            this.pnlBanco.Enabled = this.pnlTercero.Enabled = false;
                        }
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + objCliente.Tables["Resultado"].Rows[0].ItemArray[0].ToString() + "');</script>", false);
                    }
                    else if (objCliente.Tables.Contains("ClienteInconsistente"))
                    {

                        this.pnlBanco.Enabled = this.pnlTercero.Enabled = false;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + objCliente.Tables["Resultado"].Rows[0].ItemArray[0].ToString() + "');</script>", false);
                        return;
                    }
                    else
                    {
                        if (objCliente.Tables["Resultado"].Rows[0].ItemArray[1].ToString() == "0")
                        {
                            this.imgBtnGuardar.Enabled = this.pnlBanco.Enabled = this.pnlTercero.Enabled = false;
                        }
                        else
                        {
                            this.pnlBanco.Enabled = this.pnlTercero.Enabled = true;
                        }

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + objCliente.Tables["Resultado"].Rows[0].ItemArray[0].ToString() + "');</script>", false);
                    }
                }
            }
        }

        /// <summary>
        /// Meotodo para guardar la información en debito automatico
        /// </summary>
        private void guardar()
        {
            try
            {
                DataSet objCliente = new DataSet();
                String Resultado = String.Empty;

                if (IdDatosDebito > 0 && IdTitularCuenta > 0 && Estado != 15) //Modifica la información
                {
                    if (Session["perfil"].ToString() == "5")
                    {
                        Resultado = SerDebito.ModificarDatos(Convert.ToInt32(this.txbContrato.Text), true, //Numero de contrato
                                                            this.txbNumCuenta.Text.Trim(),  //Numero de cuenta
                                                             Convert.ToInt32(this.ddlTipoCuenta.SelectedValue), true, //IdBanco
                                                             Convert.ToInt32(this.ddlBanco.SelectedValue), true, //Tipo cuenta
                                                             Request.UserHostName.ToString(),
                                                              Convert.ToInt32(this.ddlFechaDebito.SelectedValue), true, //Fecha débito
                                                             Session["usuario"].ToString(), //Usuario
                                                             ""); //Password
                    }
                }
                else
                {
                    if (this.chbTercero.Checked)
                    {
                        Resultado = SerDebito.GuardarCliente(Convert.ToInt32(this.txbContrato.Text), true, //Numero de contrato
                                                             Convert.ToInt32(this.ddlBanco.SelectedValue), true, //IdBanco
                                                             Convert.ToInt32(this.ddlTipoCuenta.SelectedValue), true, //Tipo cuenta
                                                             this.txbNumCuenta.Text.Trim(), //Numero de cuenta
                                                             Convert.ToInt32(this.ddlFormaDebito.SelectedValue), true, //Formato
                                                             true, true, //Tercero
                                                             this.txbNombreTercero.Text.Trim(), //Nombre del tercero
                                                             Convert.ToInt64(this.txbIdentificacionT.Text.Trim()), true, //Nombre del tercero
                                                             Convert.ToInt32(this.ddlTipoIdentificacionT.SelectedValue), true, //Tipo ident tercero
                                                             Request.UserHostName.ToString(),
                                                             Convert.ToInt32(this.ddlFechaDebito.SelectedValue), true, //Fecha débito
                                                             Session["usuario"].ToString(), //Usuario
                                                             ""); //Password
                    }
                    else
                    {
                        Resultado = SerDebito.GuardarCliente(Convert.ToInt32(this.txbContrato.Text), true, //Numero de contrato
                                                             Convert.ToInt32(this.ddlBanco.SelectedValue), true, //IdBanco
                                                             Convert.ToInt32(this.ddlTipoCuenta.SelectedValue), true, //Tipo cuenta
                                                             this.txbNumCuenta.Text.Trim(), //Numero de cuenta
                                                             Convert.ToInt32(this.ddlFormaDebito.SelectedValue), true, //Formato
                                                             false, false, //Tercero
                                                             "", //Nombre del tercero
                                                             0, false, ///Nombre del tercero
                                                             0, true, //Tipo ident tercero
                                                             Request.UserHostName.ToString(), //Direccion IP
                                                             Convert.ToInt32(this.ddlFechaDebito.SelectedValue), true, //Fecha débito
                                                             Session["usuario"].ToString(), //Usuario
                                                             ""); //Password
                    }
                }

                System.IO.StringReader xmlSR = new System.IO.StringReader(Resultado);
                objCliente.ReadXml(xmlSR);
                limpiar();
                this.pnlBanco.Enabled = false;
                this.txbContrato.Text = String.Empty;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + objCliente.Tables["Resultado"].Rows[0].ItemArray[0].ToString() + "');</script>", false);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + ex.Message.Replace("'", "").Replace("\n", "").Replace("\r", "") + "');</script>", false);
            }
        }

        protected void txbNumCuenta_TextChanged(object sender, EventArgs e)
        {
           string er = @"1{10,}|2{10,}|3{10,}|4{10,}|5{10,}|6{10,}|7{10,}|8{10,}|9{10,}|0{10,}";

           if (Regex.IsMatch(txbNumCuenta.Text, er))
           {
               ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('El Número de Cuenta: " + txbNumCuenta.Text + " no es valido');</script>", false);
               txbNumCuenta.Text = "";
           }
        }
    }
}