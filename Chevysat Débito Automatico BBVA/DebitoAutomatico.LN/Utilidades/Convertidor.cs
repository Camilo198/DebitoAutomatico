using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.LN.Utilidades
{
    public class Convertidor
    {
        public static String aCadena(Object objeto)
        {
            String valor = String.Empty;
            if (objeto != DBNull.Value)
                valor = Convert.ToString(objeto);
            return valor;
        }

        /// <summary>
        /// Convierte el valor del objeto especificado en un entero de 32 bits con signo.
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public static Decimal aDecimal(Object objeto)
        {
            Decimal valor = -1;
            if (objeto != DBNull.Value)
                valor = Convert.ToDecimal(objeto);
            return valor;
        }

        /// <summary>
        /// Convierte el valor del objeto especificado en un entero de 32 bits con signo.
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public static Int32 aEntero32(Object objeto)
        {
            Int32 valor = -1;
            if (objeto != DBNull.Value)
                valor = Convert.ToInt32(objeto);
            return valor;
        }

        /// <summary>
        /// Convierte el valor del objeto especificado en un entero de 16 bits con signo.
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public static Int16 aEntero16(Object objeto)
        {
            Int16 valor = -1;
            if (objeto != DBNull.Value)
                valor = Convert.ToInt16(objeto);
            return valor;
        }

        /// <summary>
        /// Convierte el valor del objeto especificado en un valor booleano equivalente.
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public static bool aBooleano(Object objeto)
        {
            bool valor = false;
            if (objeto != DBNull.Value)
                valor = Convert.ToBoolean(objeto);
            return valor;
        }

        /// <summary>
        /// Convierte el valor del objeto especificado en un objeto System.DateTime.
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public static DateTime aFechaYHora(Object objeto)
        {
            DateTime valor = DateTime.Now;
            if (objeto != DBNull.Value)
                valor = Convert.ToDateTime(objeto);
            return valor;
        }

        /// <summary>
        /// Convierte el valor de la cadena en un objeto System.DateTime.
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public static DateTime aFechaYHora(String objeto)
        {
            DateTime valor = DateTime.Now;
            if (objeto.Length == 10)
                valor = new DateTime(Convert.ToInt32(objeto.Substring(6, 4)), Convert.ToInt32(objeto.Substring(3, 2)), Convert.ToInt32(objeto.Substring(0, 2)));
            return valor;
        }
    }
}
