using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DebitoAutomatico.EN;


namespace DebitoAutomatico.PS.Codigo
{
    public class UtilidadesWeb
    {
        public static void ajustarMensaje(Label etiqueta, String mensaje, TipoMensaje tipoMsg)
        {
            if (tipoMsg.Equals(TipoMensaje.Informacion))
            {
                etiqueta.Font.Bold = false;
                etiqueta.ForeColor = Color.Black;

                mensaje = "<b>[INFO]</b> " + mensaje;
            }
            else if (tipoMsg.Equals(TipoMensaje.Error))
            {
                etiqueta.Font.Bold = true;
                etiqueta.ForeColor = Color.Red;

                mensaje = "<b>[ERROR]</b> " + mensaje;
            }
            etiqueta.Text = mensaje;
        }

        /// <summary>
        /// Agrega el valor de seleccione a una lista desplegable.
        /// </summary>
        /// <param name="lista">Lista desplegable</param>
        public static void agregarSeleccioneDDL(DropDownList lista)
        {
            lista.Items.Insert(0, new ListItem("[Seleccione]", "0"));
        }

        /// <summary>
        /// Agrega el valor de seleccione a una lista desplegable.
        /// </summary>
        /// <param name="lista">Lista desplegable</param>
        public static void agregarSeleccioneDDLNA(DropDownList lista)
        {
            lista.Items.Insert(0, new ListItem("[NO APLICA]", "0"));
        }

        /// <summary>
        /// Agrega el valor de seleccione a una lista desplegable.
        /// </summary>
        /// <param name="lista">Lista desplegable</param>
        public static void agregarSeleccioneDDL(AjaxControlToolkit.ComboBox lista)
        {
            lista.Items.Insert(0, new ListItem("[Seleccione]", "0"));
        }

        /// <summary>
        /// Agrega el valor de todos a una lista desplegable.
        /// </summary>
        /// <param name="lista">Lista desplegable</param>
        public static void agregarTodosDDL(DropDownList lista)
        {
            lista.Items.Insert(0, new ListItem("[TODO]", "0"));
        }

        /// <summary>
        /// Agrega el valor de todos a una lista desplegable.
        /// </summary>
        /// <param name="lista">Lista desplegable</param>
        public static void agregarTodosDDL(AjaxControlToolkit.ComboBox lista)
        {
            lista.Items.Insert(1, new ListItem("[Todo]", "TODO"));
        }

        public static String homologarTipo(String letra)
        {
            Dictionary<String, String> Tipo = new Dictionary<String, String>();
            Tipo.Add("S", "Suscriptor");
            Tipo.Add("A", "Adjudicado");
            Tipo.Add("G", "Ganador");
            Tipo.Add("C", "Cuotas Por Devolver");

            return Tipo[letra];
        }

        public static String homologarEstado(String letra)
        {
            Dictionary<String, String> Tipo = new Dictionary<String, String>();
            Tipo.Add("V", "Vigente");
            Tipo.Add("M", "Mora");
            Tipo.Add("C", "Cancelado");
            Tipo.Add("J", "Juridico");
            Tipo.Add("P", "Prejuridico");
            Tipo.Add("D", "Dudoso Recaudo");
            Tipo.Add("T", "Traslado");
            Tipo.Add("R", "Reemplazado");
            Tipo.Add("X", "Venta Caida");
            Tipo.Add("Y", "Devolucion por tabla de restitucion");
            Tipo.Add("Z", "Devolucion Total");
            Tipo.Add("K", "Cartera Castigada");

            return Tipo[letra];
        }

        public static String homologarMes(String numero)
        {
            Dictionary<String, String> Tipo = new Dictionary<String, String>();
            Tipo.Add("1", "Enero");
            Tipo.Add("2", "Febrero");
            Tipo.Add("3", "Marzo");
            Tipo.Add("4", "Abril");
            Tipo.Add("5", "Mayo");
            Tipo.Add("6", "Junio");
            Tipo.Add("7", "Julio");
            Tipo.Add("8", "Agosto");
            Tipo.Add("9", "Septiembre");
            Tipo.Add("10", "Octubre");
            Tipo.Add("11", "Noviembre");
            Tipo.Add("12", "Diciembre");

            return Tipo[numero];
        }

        public static Int32 homologarDocumento(String letra)
        {
            Dictionary<String, Int32> Tipo = new Dictionary<String, Int32>();
            Tipo.Add( "C",1);
            Tipo.Add( "E",2);
            Tipo.Add( "T",4);
            Tipo.Add( "N",5);
            Tipo.Add( "P",7); 

            return Tipo[letra];
        }

        public static String homologarletra(String numero)
        {
            Dictionary<String, String> Tipo = new Dictionary<String, String>();
            Tipo.Add("1", "A");
            Tipo.Add("2", "B");
            Tipo.Add("3", "C");
            Tipo.Add("4", "D");
            Tipo.Add("5", "E");
            Tipo.Add("6", "F");
            Tipo.Add("7", "G");
            Tipo.Add("8", "H");
            Tipo.Add("9", "I");
            Tipo.Add("10", "J");
            Tipo.Add("11", "K");
            Tipo.Add("12", "L");
            Tipo.Add("13", "M");
            Tipo.Add("14", "N");
            Tipo.Add("15", "O");
            Tipo.Add("16", "P");
            Tipo.Add("17", "Q");
            Tipo.Add("18", "R");
            Tipo.Add("19", "S");
            Tipo.Add("20", "T");
            Tipo.Add("21", "U");
            Tipo.Add("22", "V");
            Tipo.Add("23", "W");
            Tipo.Add("24", "X");
            Tipo.Add("25", "Y");
            Tipo.Add("26", "Z");

            return Tipo[numero];
        }


        public string ValidaFecha(string Fecha)
        {
            try
            {
                if (Convert.ToDateTime(Fecha) < DateTime.Now.Date)
                {
                    return "Inferior";
                }
                else
                {
                    if (Convert.ToDateTime(Fecha) > DateTime.Now.Date)
                    {
                        return "Superior";
                    }
                    else
                        return "Igual";
                }
            }
            catch (Exception)
            {
                return "Errada";
            }
        }

        public static String calculoDigito(String codigo)
        {
            var suma = 0;
            var evaluar = 0;
            var aprox = 0;
            var digito = 0;
            String cadena = codigo.Trim().PadLeft(12, '0');
            for (int i = 1; i < 13; i++)
            {
                if (i % 2 == 0)
                {
                    evaluar = Convert.ToInt32(cadena.Substring(i - 1, 1)) * 2;
                    if (evaluar.ToString().Trim().Length == 1)
                        suma = suma + evaluar;
                    else
                    {
                        if (evaluar.ToString().Trim().Length == 2)
                        {
                            suma = suma + Convert.ToInt32(evaluar.ToString().Trim().Substring(0, 1));
                            suma = suma + Convert.ToInt32(evaluar.ToString().Trim().Substring(1, 1));
                        }
                    }
                }
                else
                {

                    evaluar = Convert.ToInt32(cadena.Substring(i - 1, 1));
                    suma = suma + evaluar;
                }
            }

            aprox = (int)Math.Ceiling(suma / 10d) * 10;

            if (aprox >= suma)
                digito = aprox - suma;
            else
                digito = (aprox + 10) - suma;

            return digito.ToString().Trim();
        }

        public static bool textoEsNumero(string texto)
        {
            try
            {
                Convert.ToInt32(texto);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static String fechaJulianaToGregoriana(long fechaJuliana)
        {
            if (fechaJuliana == 0)
                return "";

            return DateTime.FromOADate(Convert.ToDouble(fechaJuliana + 10594)).ToString("dd/MM/yyyy");
        }

        public static String ConvertToJuliana(String Date)
        {
            return Convert.ToString(Math.Truncate(Convert.ToDateTime(Date).ToOADate() - 10594));
        }

    }
}