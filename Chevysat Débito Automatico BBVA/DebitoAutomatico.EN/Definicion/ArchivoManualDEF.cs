using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Definicion
{
    [Serializable()]
    public class ArchivoManualDEF
    {
        public const String _NombreTabla = "tb_DEB_ARCHIVO_MANUAL";
        public const String Contrato = "CONTRATO";
        public const String Nombre = "NOMBRE_CLIENTE";
        public const String NumeroCuenta = "NUMERO_CUENTA";
        public const String TipoCuenta = "TIPO_CUENTA";
        public const String Valor = "VALOR";
        public const String Autorizar = "AUTORIZAR";
        public const String EstadoCliente = "ESTADO_CLIENTE";
        public const String Banco = "BANCO";
    }
}
