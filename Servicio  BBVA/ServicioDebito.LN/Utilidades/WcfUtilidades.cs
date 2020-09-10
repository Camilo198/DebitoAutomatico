using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ServicioDebito.LN.Utilidades
{
    public class WcfUtilidades : IWcfUtilidades
    {
        private wsenviocorreos.Service EnvioCorreo = new wsenviocorreos.Service();
        private DataRow rowC;

        public DataTable AgregarTabla(String Valor, String Validacion)
        {
            DataTable dtImage = new DataTable();
            dtImage.TableName = "Resultado";
            dtImage.Columns.Add("Mensaje");
            dtImage.Columns.Add("Validacion");
            dtImage.Rows.Add(Valor, Validacion);
            return dtImage;
        }

        public DataSet AgregarCampo(DataSet Resultado, String Valor, String Validacion)
        {
            DataRow newCustomersRow = Resultado.Tables["Resultado"].NewRow();
            newCustomersRow["Mensaje"] = Valor;
            newCustomersRow["Validacion"] = Validacion;
            Resultado.Tables["Resultado"].Rows.Add(newCustomersRow);
            return Resultado;
        }

        public string EnvioMail(string _Adjunto, string _Asunto, string _Mensaje, string _Para, string _Desde, string _Copia)
        {
            try
            {
                DataSet DsCorreos = new DataSet();
                DsCorreos.Tables.Add("Reportes");
                DsCorreos.Tables["Reportes"].Columns.Add("strTo");
                DsCorreos.Tables["Reportes"].Columns.Add("strCc");
                DsCorreos.Tables["Reportes"].Columns.Add("strCo");
                DsCorreos.Tables["Reportes"].Columns.Add("strSubject");
                DsCorreos.Tables["Reportes"].Columns.Add("strMessaje");
                DsCorreos.Tables["Reportes"].Columns.Add("strPath");

                rowC = DsCorreos.Tables["Reportes"].NewRow();
                rowC["strTo"] = _Para;
                rowC["strCo"] = _Copia;
                rowC["strSubject"] = _Asunto;
                rowC["strMessaje"] = _Mensaje;
                rowC["strPath"] = _Adjunto.Trim();
                DsCorreos.Tables["Reportes"].Rows.Add(rowC);
                EnvioCorreo.EnvioCorreos(DsCorreos, _Desde, false);
                return "MENSAJE ENVIADO";
            }
            catch (Exception exc)
            {
                return exc.Message;
            }
        }
     
    }
}
