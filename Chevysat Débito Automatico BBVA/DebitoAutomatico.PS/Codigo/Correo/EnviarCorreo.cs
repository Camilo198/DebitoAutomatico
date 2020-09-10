using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Data;

namespace DebitoAutomatico.PS.Codigo.Correo
{
    public class EnviarCorreo
    {
        private wsenviocorreos.Service EnvioCorreo = new wsenviocorreos.Service();
        private DataRow rowC;
        
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

        private Boolean email_valido(String email)
        {
            String expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}