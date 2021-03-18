using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos.Modelos
{
    public class ActaSeguimEstadoEmpresaModel
    {
        public Int64 id { get; set; }
        public int codProyecto { get; set; }
        public int codConvocatoria { get; set; }
        public int numActa { get; set; }
        public int visita { get; set; }
        public DateTime FechaIngresado { get; set; }
        public string Descripcion { get; set; }
        public double IDH { get; set; }
        public double NBI { get; set; }
    }
}
