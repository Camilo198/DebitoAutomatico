using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ServicioDebito.LN.Utilidades
{
    public class UtilidadesWeb
    {

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

        public static Int32 homologarDocumento(String letra)
        {
            Dictionary<String, Int32> Tipo = new Dictionary<String, Int32>();
            Tipo.Add("C", 1);
            Tipo.Add("E", 2);
            Tipo.Add("T", 4);
            Tipo.Add("N", 5);
            Tipo.Add("P", 7);

            return Tipo[letra];
        }

        public static String homologarDocumentoAbrebiatura(Int32 numero)
        {
            Dictionary<Int32, String> Tipo = new Dictionary<Int32, String>();
            Tipo.Add(1, "CC");
            Tipo.Add(2, "TE");
            Tipo.Add(4, "TI");
            Tipo.Add(5, "NIT");
            Tipo.Add(7, "PA");

            return Tipo[numero];
        }


        public static String homologarDocumentoSico(Int32 numero)
        {
            Dictionary<Int32, String> Tipo = new Dictionary<Int32, String>();
            Tipo.Add(1, "C");
            Tipo.Add(2, "E");
            Tipo.Add(4, "T");
            Tipo.Add(5, "N");
            Tipo.Add(7, "P");

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

    }
}

