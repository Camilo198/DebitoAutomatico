using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Tablas
{
    [Serializable()]
    public class HistorialProcesoUsuario
    {
        public String pOperacion { get; set; }
        public int? pId { get; set; }
        public String pNumeroIdentificacion { get; set; }
        public String pTipoCuenta { get; set; }
        public String pNumeroCuenta { get; set; }
        public String pNombreBanco { get; set; }
        public String pTipoTransferencia { get; set; }
        public String pFecha { get; set; }
        public String pFechaFin { get; set; }
        public String pValor { get; set; }
        public String pRespuesta { get; set; }
        public String pCausal { get; set; }
        public String pFechaProceso { get; set; }
        public String pUsuario { get; set; }
        public String pContrato { get; set; }
        public String pNombreBancoDebita { get; set; }
        public String pNombreCliente { get; set; }
        public String pNombreArchivo { get; set; }
        public String pEstado { get; set; }
        public bool? pValidacion { get; set; }
        public int? pFechaDebito { get; set; }

        public String pIdTipoCausal { get; set; }
        public String pFechaGiro { get; set; }
        public String pEstadoCliente { get; set; }
        public Int64 pIdCliente { get; set; }
    }
}
