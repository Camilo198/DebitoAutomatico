using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Definicion
{
     [Serializable()]
    public class BancoDEF
    {

        public const String _NombreTabla = "tb_DEB_BANCO";
        public const String Id = "ID";
        public const String Codigo = "CODIGO";
        public const String Nombre = "NOMBRE";
        public const String Nit = "NIT";
        public const String Activo= "ACTIVO";
        public const String Debito = "DEBITO";
        public const String CorreoControl = "CORREOS_CONTROL";
        public const String CorreoEnvio = "CORREOS_ENVIO";
        public const String Remitente = "REMITENTE";
        public const String IdRuta = "ID_RUTA";
        public const String IdPrenota = "ID_PRENOTA";
        public const String IdPrenotaManual = "ID_PRENOTA_MANUAL";
        public const String IdDebito = "ID_DEBITO";
        public const String IdDebitoManual = "ID_DEBITO_MANUAL";
        public const String IdPagos = "ID_PAGOS";
        public const String IdErrores = "ID_ERRORES";
        public const String IdRecibidos = "ID_RECIBIDOS";
        public const String IdHistorico = "ID_HISTORICO";
        

    }
}
