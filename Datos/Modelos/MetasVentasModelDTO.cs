using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos.Modelos
{
    public class MetasVentasModelDTO
    {
        public int id { get; set; }
        public decimal ventas { get; set; }
        public int periodoImproductivo { get; set; }
        public string recursosAprobadosEmprendedor { get; set; }
        public int idProyecto { get; set; }
        public int idConvocatoria { get; set;}

    }
}
