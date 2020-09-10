using DebitoAutomatico.EN.Tablas;
using DebitoAutomatico.LN.Consultas;
using DebitoAutomatico.PS.Codigo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Autor: Nicolas Alexander Larrotta
/// Fecha Ultima Actualización: 20 de Junio de 2018
/// Observacion: Modulo Reanudar Debito
/// </summary>
/// 
namespace DebitoAutomatico.PS.Modulos.Configuracion
{
    public partial class ReanudarDebito : System.Web.UI.Page
    {


        private DataSet dsConsulta
        {
            get
            {
                DataSet tabla = new DataSet();
                if (ViewState["dsConsulta"] != null)
                    tabla = (DataSet)ViewState["dsConsulta"];
                return tabla;
            }

            set
            {
                ViewState["dsConsulta"] = value;
            }
        }

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

            if (!IsPostBack)
            {
                Gridview();
            }
        }

        protected void btnReanudarDebito_Click(object sender, EventArgs e)
        {
            Reanudacion objRe = new Reanudacion();
            objRe.pMes = DateTime.Now.Month.ToString();
            objRe.pAño = DateTime.Now.Year.ToString();

            objRe.pEstado = true;

            List<EN.Tablas.Reanudacion> listaRe = new ReanudacionLN().consultarMes(objRe);

            if (listaRe.Count > 0)
            {
                int val = 0;
                objRe = listaRe[0];

                objRe.pUsuario = Session["usuario"].ToString();
                objRe.pObservaciones = "No puede reanudar de nuevo el mes de " + UtilidadesWeb.homologarMes(objRe.pMes) + " del año " + objRe.pAño + "";
                objRe.pEstado = false;
                val = new ReanudacionLN().insertar(objRe);

                if (val > 0)
                {
                    Gridview();
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + objRe.pObservaciones + " ');</script>", false);
                }
            }
            else
            {
                //Valida si existe clientes en DEBITO EN PROCESO 
                DataSet dsDebitoProceso = new DataSet();
                dsDebitoProceso = new DatosDebitoLN().consultar("5");

                //Consulta clientes en estado DEBITADO
                DataSet DsDebitado = new DataSet();
                DsDebitado = new DatosDebitoLN().consultar("8");

                DatosDebito objT = new DatosDebito();
                int act = 0;

                if (dsDebitoProceso.Tables["tabla"].Rows.Count > 0)
                {
                    int deb = 0;
                    objRe.pUsuario = Session["usuario"].ToString();
                    objRe.pObservaciones = RecursosDebito.ReversionNo;
                    objRe.pEstado = false;

                    deb = new ReanudacionLN().insertar(objRe);

                    if (deb > 0)
                    {
                        Gridview();
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ReversionNo + " ');</script>", false);
                    }                    
                }
                else if (DsDebitado.Tables["tabla"].Rows.Count > 0)
                {
                    for (int i = 0; i < DsDebitado.Tables["tabla"].Rows.Count; i++)
                    {
                        objT.pContrato = DsDebitado.Tables["tabla"].Rows[i]["CONTRATO"].ToString();
                        objT.pEstado = 4;

                        act = 0;
                        act = new DatosDebitoLN().actualizarEstado(objT);

                        if (act < 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.ErrorActualizaEstado + DsDebitado.Tables["tabla"].Rows[i]["CONTRATO"].ToString() + " ');</script>", false);
                        }
                    }

                    if (act > 0)
                    {
                        int val = 0;
                        objRe.pUsuario = Session["usuario"].ToString();
                        objRe.pObservaciones = RecursosDebito.Reanudacion + UtilidadesWeb.homologarMes(objRe.pMes) + " del año " + objRe.pAño + "";
                        objRe.pEstado = true;

                        val = new ReanudacionLN().insertar(objRe);

                        if (val > 0)
                        {
                            Gridview();
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + objRe.pObservaciones + " ');</script>", false);
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>alert('" + RecursosDebito.NoHayRenovaciones + " ');</script>", false);
                }
            }
        }

        protected void gvReanudacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvReanudacion.PageIndex = e.NewPageIndex;
            this.gvReanudacion.DataSource = dsConsulta;
            this.gvReanudacion.DataBind();
        }

        public void Gridview()
        {
            dsConsulta = new ReanudacionLN().consultar(new Reanudacion());

            if (dsConsulta.Tables["tabla"].Rows.Count > 0)
            {
                for (int i = 0; i < dsConsulta.Tables["tabla"].Rows.Count; i++)
                {
                    dsConsulta.Tables["tabla"].Rows[i]["MES_REANUDADO"] = UtilidadesWeb.homologarMes(dsConsulta.Tables["tabla"].Rows[i]["MES_REANUDADO"].ToString());
                }

                gvReanudacion.DataSource = dsConsulta;
                gvReanudacion.DataBind();
            }
            else
            {
                this.gvReanudacion.DataSource = null;
                this.gvReanudacion.DataBind();
            }
        }
    }
}