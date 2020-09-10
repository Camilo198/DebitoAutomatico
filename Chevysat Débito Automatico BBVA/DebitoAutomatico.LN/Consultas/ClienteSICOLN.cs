using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DebitoAutomatico.AD.Consultas;
using DebitoAutomatico.EN.Tablas;

namespace DebitoAutomatico.LN.Consultas
{
    public class ClienteSICOLN
    {

        public DataSet consultarClienteLectura(ClienteSico objEntidad)
        {
            DataSet Cliente = new DataSet();
            Cliente = new ClienteSicoAD().ejecutarConsultaLectura(objEntidad);
            return Cliente;
        }


        public DataSet consultarCliente(ClienteSico objEntidad)
        {
            DataSet Cliente = new DataSet();
            Cliente = new ClienteSicoAD().ejecutarConsulta(objEntidad);
            return Cliente;
        }

        public DataSet consultarClienteCorreo(ClienteSico objEntidad)
        {
            DataSet Cliente = new DataSet();
            Cliente = new ClienteSicoAD().ejecutarConsultaCorreo(objEntidad);
            return Cliente;
        }

        public DataSet ConsultarTablaDebito(ClienteSico objEntidad)
        {
            DataSet Cliente = new DataSet();
            Cliente = new ClienteSicoAD().ejecutarConsultaDebito(objEntidad);
            return Cliente;
        }

        //public DataSet ConsultaDebito(String FechaDebito, bool sus, bool adj, bool gana, bool cuoxdevol, Int32 vencimiento)
        //{
        //    DataSet ClienteDebito = new DataSet();
        //    ClienteDebito = new ClienteSicoAD().Sico_ConsultaTablaDebito(FechaDebito, sus, adj, gana, cuoxdevol, vencimiento);
        //    return ClienteDebito;
        //}


    }
}
