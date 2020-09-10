using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Definicion
{
    [Serializable()]
    public class HistorialProcesoUsuarioDEF
    {
        public const String _NombreTabla = "tb_DEB_HISTORIAL_PROCESO_USUARIO";
        public const String Id = "ID";
        public const String NumeroIdentificacion = "NUMERO_IDENTIFICACION";
        public const String TipoCuenta = "TIPO_CUENTA";
        public const String NumeroCuenta = "NUMERO_CUENTA";
        public const String NombreBanco = "BANCO";
        public const String TipoTransferencia = "TIPO_TRANSFERENCIA";
        public const String Fecha = "FECHA";
        public const String Valor = "VALOR";
        public const String Respuesta = "RESPUESTA";
        public const String Causal = "CAUSAL";
        public const String FechaProceso = "FECHA_PROCESO";
        public const String Usuario = "USUARIO";
        public const String Contrato = "CONTRATO";
        public const String NombreBancoDebita = "BANCO_DEBITA";
        public const String NombreCliente = "NOMBRE_CLIENTE";
        public const String NombreArchivo = "NOMBRE_ARCHIVO";
        public const String Estado = "ESTADO";

        public const String IdTipoCausal = "ID_TIPO_CAUSAL";
        public const String FechaGiro = "FECHA_GIRO";
        public const String IdCliente = "ID_CLIENTE";
        public const String EstadoCliente = "ESTADO_CLIENTE";
        public const String FechaDebito = "FECHA_DEBITO";
    }
}
