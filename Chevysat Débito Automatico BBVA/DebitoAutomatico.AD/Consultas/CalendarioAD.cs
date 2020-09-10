using DebitoAutomatico.AD.Conexion;
using DebitoAutomatico.EN.Definicion;
using DebitoAutomatico.EN.Tablas;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.AD.Consultas
{
    public class CalendarioAD
    {

        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public CalendarioAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        OdbcDataAdapter DataAdapterOdbc = new OdbcDataAdapter();
        ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
        OdbcConnection Sic = new System.Data.Odbc.OdbcConnection();
        OdbcCommand odbcCommand = new OdbcCommand();
        DataSet datos = null;

        public List<Calendario> consultar(Calendario objEntidad)
        {

            try
            {
                List<Calendario> listaCalendario = new List<Calendario>();

                Sic = objConexionDB .abrirConexionOdbc();
                DataAdapterOdbc = new OdbcDataAdapter("SELECT CaleAno, CaleMes, CaleDias, CaleTipo FROM CALE where CaleAno = " + objEntidad.pCaleAno + " and CaleMes = " + objEntidad.pCaleMes + "", Sic);
                datos = new DataSet();
                DataAdapterOdbc.Fill(datos, "CALE");
                DataAdapterOdbc.Dispose();

                foreach (DataRow fila in datos.Tables[0].Rows)
                {
                    Calendario obj = new Calendario();
                    obj.pCaleAno = Convertidor.aEntero32(fila[CalendarioDEF.CaleAno]);
                    obj.pCaleMes = Convertidor.aCadena(fila[CalendarioDEF.CaleMes]);
                    obj.pCaleDias = Convertidor.aEntero32(fila[CalendarioDEF.CaleDias]);
                    obj.pCaleTipo = Convertidor.aCadena(fila[CalendarioDEF.CaleTipo]);
                    listaCalendario.Add(obj);
                }

                return listaCalendario;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
