using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.IO;
using System.Text;
using Excel;

namespace DebitoAutomatico.PS.Codigo
{
    public class LectorArchivos
    {
        public List<String> leerArchivo(String rutaArchivo, int codbanco)
        {
            List<String> listaLineas = new List<String>();
            try
            {

                if (codbanco == 27)
                {
                    int tam_var = rutaArchivo.Length;
                    string var_sub = rutaArchivo.Substring((tam_var - 4), 4);
                    List<String> listaBBVA = new List<String>();

                    StreamReader objLector = new StreamReader(rutaArchivo, System.Text.Encoding.GetEncoding(1252));
                    String strLinea = "";
                    while (strLinea != null)
                    {
                        strLinea = objLector.ReadLine();
                        if (strLinea != null && strLinea != String.Empty)
                            listaBBVA.Add(strLinea);
                    }
                    objLector.Close();

                    listaBBVA.RemoveAt(0);
                    listaBBVA.RemoveAt(0);
                    listaBBVA.RemoveAt(0);
                    listaBBVA.RemoveAt(listaBBVA.Count-1);

                    for (int i=0; i<listaBBVA.Count ; i = i + 4) 
                    {

                        listaLineas.Add(listaBBVA[i] + listaBBVA[i + 1].Substring(42, 36) + listaBBVA[i + 1].Substring(78,36) + listaBBVA[i + 2].Substring(42, 80) + listaBBVA[i + 3].Substring(42, 40));
                    }
                }
                else
                {
                    int tam_var = rutaArchivo.Length;
                    string var_sub = rutaArchivo.Substring((tam_var - 4), 4);


                    StreamReader objLector = new StreamReader(rutaArchivo, System.Text.Encoding.GetEncoding(1252));
                    String strLinea = "";
                    while (strLinea != null)
                    {
                        strLinea = objLector.ReadLine();
                        if (strLinea != null && strLinea != String.Empty)
                            listaLineas.Add(strLinea);
                    }
                    objLector.Close();
                }
                
            }
            catch
            {
                throw new System.Exception("Archivo se encuentra ubicado en una ruta diferente a la parametrizada para este banco.");
            }
            return listaLineas;
        }

        public List<String> leerColumnas(String rutaArchivo, String separador)
        {
            StreamReader objLector = new StreamReader(rutaArchivo, System.Text.Encoding.GetEncoding(1252));
            String strLinea = "";
            String[] listaColumnas = null;

            strLinea = objLector.ReadLine();
            if (strLinea != null)
                listaColumnas = strLinea.Split(char.Parse(separador));
            objLector.Close();

            List<String> lista = new List<String>();
            lista.AddRange(listaColumnas);

            return lista;
        }

        public DataTable convertirATabla(String rutaArchivo, String separador, int limiteColumnas, bool completo)
        {
            int cod = 0;
            List<String> lineas = leerArchivo(rutaArchivo,cod);
            List<String> listaColumnas = leerColumnas(rutaArchivo, separador);

            DataTable objDT = new DataTable();
            DataColumn objColumna;

            foreach (String column in listaColumnas)
            {
                objColumna = new DataColumn();
                objColumna.ColumnName = column;
                objDT.Columns.Add(objColumna);
            }

            String[] datos;
            DataRow fila;

            if (lineas.Count < limiteColumnas)
            {
                limiteColumnas = lineas.Count;
            }

            if (completo)
            {
                limiteColumnas = lineas.Count;
            }

            for (int i = 0; i < limiteColumnas; i++)
            {
                datos = lineas[i].Split(char.Parse(separador));
                fila = objDT.NewRow();
                for (int j = 0; j < listaColumnas.Count; j++)
                {
                    fila[listaColumnas[j]] = datos[j];
                }
                objDT.Rows.Add(fila);
            }

            return objDT;
        }

        public List<List<String>> separarPorFilasColumnas(String rutaArchivo, String delimitador)
        {
            int cod = 0;
            List<List<String>> lista = new List<List<String>>();
            List<String> columnas;
            List<String> lineas = leerArchivo(rutaArchivo,cod);
            foreach (String linea in lineas)
            {
                columnas = new List<String>();
                columnas.AddRange(linea.Split(char.Parse(delimitador)));
                lista.Add(columnas);
            }

            return lista;
        }

        public String borrarArchivo(String rutaArchivo)
        {
            String error = String.Empty;
            try
            {
                File.Delete(rutaArchivo);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return error;
        }


        /// <summary>
        ///Metodo que Genera Digito de verificación para el NIT
        /// </summary>
        /// <returns>digito de verificacion</returns>
        public string Digito_Verificacion(string codigo)
        {
            var suma = 0;
            var evaluar = 0;
            var aprox = 0;
            var digito = 0;
            String cadena = codigo.Trim().PadLeft(9, '0');

            for (int i = 1; i < 9; i++)
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
    }
}