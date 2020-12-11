using DebitoAutomatico.EN.Tablas;
using DebitoAutomatico.LN.Consultas;
using DebitoAutomatico.PS.Codigo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


/// <summary>
/// Autor: Nicolas Alexander Larrotta
/// Fecha Ultima Actualización: 20 de Junio de 2018
/// Observacion: Modulo de Reportes
/// </summary>
/// 
/// <summary>
/// Autor: Maikol Steven Ramirez    
/// Fecha Ultima Actualización: 04 de Marzo de 2020
/// Observacion: Se crea informe para seguimiento de correos
/// </summary>
namespace DebitoAutomatico.PS.Modulos.Servicio
{
    public partial class Reportes_Pagos : System.Web.UI.Page
    {
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

            }
        }

        protected void btnReportePagos_Click(object sender, EventArgs e)
        {
            Response.Redirect(ConfigurationManager.AppSettings["Reportes"]);
        }
    }
}