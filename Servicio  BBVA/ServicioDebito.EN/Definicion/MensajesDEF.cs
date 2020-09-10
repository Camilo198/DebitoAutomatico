using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicioDebito.EN.Definicion
{
    [Serializable()]
    public class MensajesDEF
    {
        public const String _NombreTabla = "tb_DEB_MENSAJES";
        public const String Id = "ID";
        public const String TipoContrato = "TIPO_CONTRATO";
        public const String EstadoDebito = "ESTADO_DEBITO";
        public const String Motivo = "MOTIVO";
        public const String Asunto = "ASUNTO";
        public const String Mensaje = "MENSAJE";     
    }
}
