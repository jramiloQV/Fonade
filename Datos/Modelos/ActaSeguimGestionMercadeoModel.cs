using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos.Modelos
{
    public class ActaSeguimGestionMercadeoModel
    {
        public Int64 id { get; set; }
        public int codProyecto { get; set; }
        public int codConvocatoria { get; set; }
        public int numActa { get; set; }
        public int visita { get; set; }
        public int cantidad { get; set; }
        public string descripcionEvento { get; set; }
        public string publicidadLogos { get; set; }
        public DateTime fechaIngreso { get; set; }
    }
}
