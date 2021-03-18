using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.Entidades
{
    public class InterventoriaNominaEntity
    {
        public int Id_Nomina { get; set; }

        public int CodProyecto { get; set; }

        public String Cargo { get; set; }

        public String Tipocargo { get; set; }

        public int Mes { get; set; }

        public Double Valor { get; set; }

        public int Tipo { get; set; }
    }
}
