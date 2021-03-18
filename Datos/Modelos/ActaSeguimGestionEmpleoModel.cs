using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos.Modelos
{
    public class ActaSeguimGestionEmpleoModel
    {
        public Int64 id { get; set; }
        public int codProyecto { get; set; }
        public int codConvocatoria { get; set; }
        public int numActa { get; set; }
        public int Visita { get; set; }
        public int verificaIndicador { get; set; }
        public string desarrolloIndicador { get; set; }
        public DateTime FechaUpdate { get; set; }

    }
}
