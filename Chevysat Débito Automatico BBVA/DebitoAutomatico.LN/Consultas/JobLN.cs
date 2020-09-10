using DebitoAutomatico.AD.Consultas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DebitoAutomatico.LN.Consultas
{
    public class JobLN
    {
        public DataSet TareaJob()
        {
            return new JobAD().EjecucionJob();
        }

        public DataSet JobConsulta()
        {
            return new JobAD().EjecucionJobConsulta();
        }

        public DataSet JobConsultaTiempo()
        {
            return new JobAD().EjecucionJobConsultaTiempo();
        }
    }
}
