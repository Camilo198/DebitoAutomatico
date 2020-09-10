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
using DebitoAutomatico.PS.Codigo.Prenota;
using System.Collections;
using DebitoAutomatico.LN.Utilidades;

/// <summary>
/// Autor: Nicolas Alexander Larrotta
/// Fecha Ultima Actualización: 20 de Junio de 2018
/// Observacion: Modulo Prenotificar Clientes
/// </summary>

namespace DebitoAutomatico.PS.Modulos.Servicio
{
    public partial class AutorizacionCuentas : System.Web.UI.Page
    {
        private ServiceDebito.ServiceDebito SerDebito = new ServiceDebito.ServiceDebito();
        UtilidadesWeb wcf = new UtilidadesWeb();
        Banco objB = new Banco();
        Convenio objC = new Convenio();
        List<Banco> ListBanco = new List<Banco>();
        Encriptador objEncriptador = new Encriptador();
        ArrayList codBancos = new ArrayList();
        int contp = 0;
        DataSet prueba = new DataSet();
        DataTable dtprueba = new DataTable();

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
               // Session["usuario"] = "maikol.ramirez";
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

            RutasLN objRutaLN = new RutasLN();
            Rutas objRuta = new Rutas();
            objRuta.pId = objB.pIdPrenota;
            String path = objRutaLN.consultarRuta(objRuta)[0].pRuta;

            if (!IsPostBack)
            {

                objB.pDebito = true;
                objB.pActivo = true;
                this.ddlBancoDebito.DataSource = new BancoLN().consultarBanco(objB);
                this.ddlBancoDebito.DataTextField = "pNombre";
                this.ddlBancoDebito.DataValueField = "pId";
                this.ddlBancoDebito.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlBancoDebito);

                cargarGridview();
            }
        }

        /// <summary>
        /// Genera el archivo con los clientes en estado PRENOTA
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                HistorialArchivos objHa = new HistorialArchivos();
                objHa.pFecha = Convert.ToDateTime(this.txbFechaInicial.Text).ToString("dd/MM/yyyy");
                objHa.pCodigoBanco = this.ddlBancoDebito.SelectedValue;
                objHa.pEstado = "V";
                objHa.pTipoArchivo = "GPN";
                objHa.pManual = false; ///Incluir para pasar a produccion
                List<EN.Tablas.HistorialArchivos> listHa = new HistorialArchivosLN().consultar(objHa);

                if (listHa.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ArchivoGenerado + this.ddlBancoDebito.SelectedItem.Text + " ');</script>", false);
                    return;
                }

                //Cosulta los convevios que tiene el banco seleccionado
                objC.pIdBancoDebito = Convert.ToInt32(this.ddlBancoDebito.SelectedValue);
                objC.pIdPrenota = "S";
                objC.pOperacion = TiposConsultas.CONSULTAR;
                ConveniosBancosDebito = new ConvenioLN().consultar(objC);

                foreach (Convenio objBal in ConveniosBancosDebito)
                {
                    if (objBal.pIdPrenota == "S")
                    {
                        codBancos.Add(objBal.pIdBanco);
                    }
                }

                DataTable tablaPrenotas = new DataTable();
                DataSet DsPrenotas = new DataSet();

                DsPrenotas = new DatosDebitoLN().consultar("1"); //Clientes en estado prenota
                tablaPrenotas = DsPrenotas.Tables[0];

                foreach (DataRow row in tablaPrenotas.Rows)
                {
                    DataSet resultado = new DataSet();
                    String Consulta = String.Empty;
                   Consulta = SerDebito.ConsultaClientePrenota(Convert.ToInt32(row[1].ToString()), true);
                    System.IO.StringReader xmlSR = new System.IO.StringReader(Consulta);
                    resultado.ReadXml(xmlSR);

                    if (resultado.Tables.Count > 0)
                    {
                        if (resultado.Tables["ClienteSICO"].Rows.Count > 0)
                        {
                            switch (resultado.Tables["ClienteSICO"].Rows[0]["TIPO_CUPO"].ToString())
                            {
                                case "Suscriptor": //Suscriptor
                                    if (!((resultado.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString().Equals("Vigente") || resultado.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString().Equals("Mora") || resultado.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString().Equals("Reemplazado") || resultado.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString().Equals("Prejurídica"))))
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.Contrato + " " + row[1].ToString() + " se encuentra en estado: " + resultado.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString() + "');</script>", false);
                                    }
                                    break;
                                case "Adjudicado": //Adjudicado
                                    if (!((resultado.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString().Equals("Vigente") || resultado.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString().Equals("Mora") || resultado.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString().Equals("Reemplazado") || resultado.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString().Equals("Prejurídica"))))
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.Contrato + " " + row[1].ToString() + " se encuentra en estado: " + resultado.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString() + "');</script>", false);
                                    }
                                    break;
                                case "Ganador": //Ganador
                                    if (!((resultado.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString().Equals("Vigente") || resultado.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString().Equals("Mora") || resultado.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString().Equals("Reemplazado") || resultado.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString().Equals("Prejurídica"))))
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.Contrato + " " + row[1].ToString() + " se encuentra en estado: " + resultado.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString() + "');</script>", false);
                                    }
                                    break;
                                case "CtasXDevolver": //Cuotas por devolver
                                    if (!((resultado.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString().Equals("Vigente") || resultado.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString().Equals("Mora") || resultado.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString().Equals("Reemplazado") || resultado.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString().Equals("Prejurídica"))))
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.Contrato + " " + row[1].ToString() + " se encuentra en estado: " + resultado.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString() + "');</script>", false);
                                    }
                                    break;
                                default:
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.Contrato + " " + row[1].ToString() + " se encuentra en estado: " + resultado.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString() + "');</script>", false);
                                    break;
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.Contrato + " " + row[1].ToString() + " no existe ');</script>", false);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.Contrato + " " + row[1].ToString() + " no existe ');</script>", false);
                    }
                }

                String Resultado = wcf.ValidaFecha(this.txbFechaInicial.Text.Trim());

                if (Resultado == "Inferior" || Resultado == "Errada")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + "Fecha " + Resultado + " ');</script>", false);
                    return;
                }

                String men = String.Empty;
                GenerarPrenota prenota = new GenerarPrenota();
                bool Manual = false;
                men = prenota.ServicioPrenota(Convert.ToInt32(this.ddlBancoDebito.SelectedValue), Session["usuario"].ToString(), this.txbFechaInicial.Text, codBancos, Manual);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'> alert('" + men + "');</script>", false);
                Limpiar();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + ex.Message.Replace("'", "").Replace("\n", "").Replace("\r", "") + "');</script>", false);
            }
        }

        /// <summary>
        /// Carga los elementos de la pantalla de nuevo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgBtnNuevo_Click(object sender, ImageClickEventArgs e)
        {
            Limpiar();
        }

        public void Limpiar()
        {
            this.ddlBancoDebito.SelectedIndex = 0;
            this.txbFechaInicial.Text = String.Empty;
            cargarGridview();
        }

        /// <summary>
        /// Carga los gridview de los clientes en PRENOTA y PRENOTA EN PROCESO
        /// </summary>
        public void cargarGridview()
        {
            DataSet dsPrenota = new DataSet();
            DataSet dsPrenotaProceso = new DataSet();

            dsPrenota = new DatosDebitoLN().consultar("1"); //Clientes en estado prenota

            if (dsPrenota.Tables["tabla"].Rows.Count > 0)
            {
                gvPrenotificar.DataSource = dsPrenota; //Clientes en estado prenota
                gvPrenotificar.DataBind();
                lbNPrenotificar.Text = gvPrenotificar.Rows.Count.ToString();
            }
            else
            {
                this.btnEnviar.Visible = false;
                this.gvPrenotificar.DataSource = null;
                this.gvPrenotificar.DataBind();
            }

            dsPrenotaProceso = new DatosDebitoLN().consultar("2");

            if (dsPrenotaProceso.Tables["tabla"].Rows.Count > 0)
            {
                gvPrenotificarProceso.DataSource = dsPrenotaProceso; //Clientes en estado prenota en proceso
                gvPrenotificarProceso.DataBind();
                lblPrenotificarProceso.Text = gvPrenotificarProceso.Rows.Count.ToString();
            }
            else
            {
                this.gvPrenotificarProceso.DataSource = null;
                this.gvPrenotificarProceso.DataBind();
            }
        }
    }
}