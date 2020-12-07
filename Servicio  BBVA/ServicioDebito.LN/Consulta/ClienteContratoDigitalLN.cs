using ServicioDebito.AD.Consultas;
using ServicioDebito.EN.Tablas;
using System;
using System.Data;

namespace ServicioDebito.LN.Consulta
{
    public class ClienteContratoDigitalLN
    {
        public DataSet consultarClienteContratoDigitalLN(ClienteContratoDigitalEN objEntidad, String Procedimiento)
        {
            DataSet Cliente = new DataSet();
            return Cliente = new ClienteContratoDigitalAD().ejecutarConsultaContratoDigital(objEntidad, Procedimiento);
        }
    }
}
