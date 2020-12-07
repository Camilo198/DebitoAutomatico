using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WcfServicioDebito
{
    public partial class PruebaServicio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ServiceDebito client = new ServiceDebito();
            string Resultado = "";
            String Usuario = "APP";
            String Password = "APP";

           Resultado = client.ModificarDatos(1002318, "2347542054", 1, 34, "172.16.29.9", 12, Usuario, Password);
            // Resultado = client.ConsultaCliente(1028280, 0, Usuario, Password);
            Resultado = client.ModificarDatosContratoDigital(4000987, "2347542055", 1, 34, "172.16.29.9", 2, Usuario, Password);
            string Resultado2=client.ConsultaClienteContratoDigital(4000987, Usuario, Password);
          //String   Resultado3 = client.GuardarClienteContratoDigital(4000987, 27, 2, "123456780", 24, "172.16.29.9", 4, Usuario, Password);
            //   Resultado = client.TipoCuenta(Usuario, Password);
            // Resultado = client.IdBanco(Usuario, Password);
            //  Resultado = client.CanalIngreso(Usuario, Password);
            //   Resultado = client.GuardarCliente(1028280, 2, 2, "123456780", 3, false, "", 0, 1, "172.16.29.9", 4, Usuario, Password);
        }
    }
}