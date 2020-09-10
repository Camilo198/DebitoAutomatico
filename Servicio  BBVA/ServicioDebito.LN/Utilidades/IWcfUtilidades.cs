using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Data;

namespace ServicioDebito.LN.Utilidades
{
    [ServiceContract]
    public interface IWcfUtilidades
    {
        [OperationContract]
        DataTable AgregarTabla(String Valor, String Validacion);
        
        [OperationContract]
        String EnvioMail(string _Adjunto, string _Asunto, string _Mensaje, string _Para, string _Desde, string _Copia);
    }
}
