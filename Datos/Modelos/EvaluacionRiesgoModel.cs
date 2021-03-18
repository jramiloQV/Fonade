using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos.Modelos
{
    public class EvaluacionRiesgoModel
    {
        public int idRiesgo { get; set; }
        public int idRiesgoInterventoria { get; set; }
        public int codProyecto { get; set; }
        public int codConvocatoria { get; set; }
        public string Riesgo { get; set; }
        public string Mitigacion { get; set; }
    }
}
