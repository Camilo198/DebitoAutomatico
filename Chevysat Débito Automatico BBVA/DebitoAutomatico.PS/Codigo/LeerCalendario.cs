using DebitoAutomatico.EN.Tablas;
using DebitoAutomatico.LN.Consultas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DebitoAutomatico.PS.Codigo
{
    public class LeerCalendario
    {
        public int diahabil()
        {

            Calendario objCalendario = new Calendario();

            int dia = 0;
            int diahabil = 0;
            int diahoyhabil = 0;
            objCalendario.pCaleAno = Convert.ToInt32(DateTime.Now.Year.ToString().PadLeft(4, '0'));
            objCalendario.pCaleMes = DateTime.Now.Month.ToString().PadLeft(2, '0');

            List<Calendario> listaCale = new CalendarioLN().consultarCalendario(objCalendario);

            if (listaCale.Count > 0)
            {
                Calendario ObjResult = new Calendario();

                ObjResult = listaCale[0];

                for (int i = 0; i < ObjResult.pCaleDias; i++)
                {
                    ObjResult = listaCale[i];

                    dia++;

                    if (ObjResult.pCaleTipo == "H")
                    {
                        diahabil++;
                    }

                    if (dia == Convert.ToInt32(DateTime.Now.Day.ToString()))
                    {
                        diahoyhabil = diahabil;
                        break;
                    }
                }
            }

            return diahoyhabil;
        
        }
    }
}