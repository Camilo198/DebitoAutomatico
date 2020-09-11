using System;
using System.Web.UI;
using DebitoAutomatico.EN;
using DebitoAutomatico.LN.Utilidades;
using DebitoAutomatico.PS.Codigo;
using DebitoAutomatico.EN.Tablas;
using DebitoAutomatico.LN.Consultas;
using System.Collections.Generic;

/// <summary>
/// Autor: Nicolas Alexander Larrotta
/// Fecha Ultima Actualización: 20 de Junio de 2018
/// Observacion: Modulo Cadena de Conexión
/// </summary>
/// 
namespace DebitoAutomatico.PS.Modulo.Administracion
{
    public partial class CadenaCX : System.Web.UI.Page
    {
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
                //Session["usuario"] = "darlin.chacon";
                //Session["usuario"] = "nicolas.larrotta";
                Session["usuario"] = "nicolas.larrotta";
                //Session["usuario"] = "camilo.munoz";
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
                inicializarComponentes();
        }

        /// <summary>
        /// Boton que direcciona al metodo guardarDatos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgBtnGuardar_Click(object sender, ImageClickEventArgs e)
        {
            guardarDatos();
        }

        /// <summary>
        /// Inicializa los componentes cargando la informacion de la cadena de conexión
        /// </summary>
        private void inicializarComponentes()
        {
            txbCadenaCX.Attributes.Add("onkeypress", "validarCaracteres(this, 200);");
            txbCadenaCX.Attributes.Add("onkeyup", "validarCaracteres(this, 200);");
            cargarDatosIniciales();
        }

        /// <summary>
        /// Lee el archivo XML que contiene la cadena de conexion cifrada
        /// </summary>
        private void cargarDatosIniciales()
        {
            txbCadenaCX.Text = leerXML("A");
            txbRutaFtp.Text = leerXML("B");
            txbUserFtp.Text = leerXML("C");
            txbPassFtp.Text = leerXML("D");
            txbCadenaSico.Text = leerXML("E");
        }

        /// <summary>
        /// Envia al metodo escribirXML la cadena de conexion que contenga las cajas de texto
        /// </summary>
        private void guardarDatos()
        {
            escribirXML("A", txbCadenaCX.Text.Trim());
            escribirXML("B", txbRutaFtp.Text.Trim());
            escribirXML("C", txbUserFtp.Text.Trim());
            escribirXML("D", txbPassFtp.Text.Trim());
            escribirXML("E", txbCadenaSico.Text.Trim());

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ProcesoExistoso + "');</script>", false);
        }

        /// <summary>
        /// Se encarga de leer el archivo XML
        /// </summary>
        /// <param name="Campo"></param>
        /// <returns></returns>
        private String leerXML(String Campo)
        {
            String respuesta = String.Empty;

            CamposXML objCampos = new CamposXML();
            objCampos.pTabla = "BD";
            objCampos.pCampo = Campo;

            LectorXML objLector = new LectorXML();
            objLector.RutaXML = Server.MapPath("~") + "\\Modulos\\XML\\Configuracion.xml";
            respuesta = objLector.leerDatosXML(objCampos);

            return respuesta;
        }

        /// <summary>
        /// Escribe en el archivo XML de acuerdo a los parametros recibidos
        /// </summary>
        /// <param name="Campo"></param>
        /// <param name="Valor"></param>
        private void escribirXML(String Campo, String Valor)
        {
            CamposXML objCampos = new CamposXML();
            objCampos.pTabla = "BD";
            objCampos.pCampo = Campo;
            objCampos.pValor = Valor;

            String valorOriginal = leerXML(Campo);

            if (!valorOriginal.Equals(Valor))
            {
                LectorXML objLector = new LectorXML();
                objLector.RutaXML = Server.MapPath("~") + "\\Modulos\\XML\\Configuracion.xml";
                objLector.modificarXML(objCampos);
            }
        }

    }
}