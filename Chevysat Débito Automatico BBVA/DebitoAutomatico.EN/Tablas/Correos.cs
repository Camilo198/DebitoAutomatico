using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.EN.Tablas
{
    public class Correos
    {

        public string Operacion { get; set; }
        public int Id { get; set; }
        public string Contrato { get; set; }
        public string NombreArchivo { get; set; }
        public bool? Envio { get; set; }
        public string Obervaciones { get; set; }
        public string Error { get; set; }
    }
}
