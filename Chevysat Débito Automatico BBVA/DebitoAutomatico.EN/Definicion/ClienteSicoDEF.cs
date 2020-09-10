using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebitoAutomatico.EN.Definicion
{
      [Serializable()]
    public class ClienteSicoDEF
    {
         public const String _NombreTabla = "CHP_CUPO_PRINCIPAL";

        public const String Contrato = "CONTRATO";
        public const String TipoDocumento = "TIPO_DOCUMENTO";
        public const String NroDocumento = "NUMERO_DOCUMENTO_CLIENTE";
        public const String NombreCliente = "NOMBRE_CLIENTE";
        public const String Estado = "ESTADO_PAGO_PLAN";
        public const String Tipo = "TIPO_CUPO";
        public const String Ciudad = "CIUDAD";
        public const String Enail = "EMAIL_CLIENTE";
        public const String Celular = "CELULAR";
    }
}
