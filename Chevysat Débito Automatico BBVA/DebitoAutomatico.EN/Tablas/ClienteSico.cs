using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebitoAutomatico.EN.Tablas
{
    [Serializable()]
    public class ClienteSico
    {
        public String pOperacion { get; set; }
        public int? pContrato { get; set; }
        public String pTipoDocumento { get; set; }
        public Int64? pNroDocumento { get; set; }
        public String pNombreCliente { get; set; }
        public String pEstado { get; set; }
        public String pTipo { get; set; }
        public String pCiudad { get; set; }
        public String pEmail { get; set; }
        public String pCelular { get; set; }
        public String pFecha { get; set; }

    }
}
