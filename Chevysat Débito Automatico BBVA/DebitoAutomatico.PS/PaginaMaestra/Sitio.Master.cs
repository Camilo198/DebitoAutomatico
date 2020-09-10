using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DebitoAutomatico.EN;
using DebitoAutomatico.LN.Utilidades;
using DebitoAutomatico.PS.Codigo;


namespace DebitoAutomatico.PS.PaginaMaestra
{
    public partial class Sitio : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    this.lbUsuario.Text = Session["usuario"].ToString();
                }
                catch (Exception)
                {
                }
            }
        }
    }
}