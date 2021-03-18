using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos.Modelos
{
    public class ActaSeguimGestionProduccionModel
    {
        public Int64 id { get; set; }
        public int codProyecto { get; set; }
        public int codConvocatoria { get; set; }
        public int numActa { get; set; }
        public int visita { get; set; }
        public int cantidad { get; set; }
        public string medida { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string cantidadMedida { get; set; }
        public int codProducto { get; set; }
        public string NomProducto { get; set; }
        public bool productoRepresentativo { get; set; }
    }
}
