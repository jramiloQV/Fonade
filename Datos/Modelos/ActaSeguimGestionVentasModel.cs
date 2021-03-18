using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos.Modelos
{
    public class ActaSeguimGestionVentasModel
    {
        public Int64 id { get; set; }
        public int codProyecto { get; set; }
        public int codConvocatoria { get; set; }
        public int numActa { get; set; }
        public int visita { get; set; }
        public decimal valor { get; set; }
        public string descripcion { get; set; }
        public DateTime fechaIngreso { get; set; }
        public string valorFormato { get; set; }

    }
}
