using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicioDebito.AD.Consultas;
using ServicioDebito.EN;
using ServicioDebito.EN.Tablas;


namespace ServicioDebito.LN.Consulta
{
    public class ClienteSICOLN
    {
        public String Error { get; set; }

        public DataSet consultarClienteSICO(ClienteSico objEntidad, String Procedimiento)
        {
            DataSet Cliente = new DataSet();
            return Cliente = new ClienteSICOAD().ejecutarConsultaSICO(objEntidad, Procedimiento);
        }

        public DataSet consultarClienteCesion(ClienteSico objEntidad, String Procedimiento)
        {
            DataSet Cliente = new DataSet();
            return Cliente = new ClienteSICOAD().ejecutarConsultaCesion(objEntidad, Procedimiento);
        }
    }
}
