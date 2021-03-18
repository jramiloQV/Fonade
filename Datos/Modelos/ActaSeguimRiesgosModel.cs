using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos.Modelos
{
    public class ActaSeguimRiesgosModel
    {
        public Int64 id { get; set; }
        public int CodProyecto { get; set; }
        public int CodConvocatoria { get; set; }
        public int CodRiesgo { get; set; }
        public int NumActa { get; set; }
        public string GestionRiesgo { get; set; }
        public DateTime FechaActualizado { get; set; }
    }
}
