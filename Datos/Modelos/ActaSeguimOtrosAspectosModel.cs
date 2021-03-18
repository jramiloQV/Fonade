using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos.Modelos
{
    public class ActaSeguimOtrosAspectosModel
    {
        public Int64 id { get; set; }
        public int codProyecto { get; set; } 
        public int codConvocatoria { get; set; }
        public int numActa { get; set; }
        public int visita { get; set; }
        public string DescripCompInnovador { get; set; }
        public string DescripCompAmbiental { get; set; }

    }

    public class ActaSeguimOtrosAspInnovadorModel
    {
        public Int64 id { get; set; }
        public int codProyecto { get; set; }
        public int codConvocatoria { get; set; }
        public int numActa { get; set; }
        public int visita { get; set; }
        public string valoracion { get; set; }
        public string observacion { get; set; }
    }

    public class ActaSeguimOtrosAspAmbientalModel
    {
        public Int64 id { get; set; }
        public int codProyecto { get; set; }
        public int codConvocatoria { get; set; }
        public int numActa { get; set; }
        public int visita { get; set; }
        public string valoracion { get; set; }
        public string observacion { get; set; }
    }
}
