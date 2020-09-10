using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Definicion
{
    [Serializable()]
    public class RespuestaTransaccionDEF
    {
        public const String _NombreTabla = "tb_DEB_RESPUESTA_TRANSACCION";
        public const String Id = "ID";
        public const String Banco = "BANCO";
        public const String Codigo = "CODIGO";
        public const String Respuesta = "RESPUESTA";
        public const String EstadoRespuesta = "ESTADO_RESPUESTA";
        public const String EstadoPrenota = "ESTADO_PRENOTA";
        public const String EstadoDebito = "ESTADO_DEBITO";
        public const String EnvioCorreo = "ENVIO_CORREO";
    }
}

