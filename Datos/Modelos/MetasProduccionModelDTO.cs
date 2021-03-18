using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos.Modelos
{
    public class MetasProduccionModelDTO
    {
        public int Id_ProductoInterventoria { get; set; }
        public int Id_Producto { get; set; }
        public string Cantidad { get; set; }
        public string NomProducto { get; set; }
        public int Unidades { get; set; }
        public string UnidadMedida { get; set; }
        public int codConvocatoria { get; set; }
        public int codProyecto { get; set; }
        public int visita { get; set; }
        public bool productoRepresentativo { get; set; }
    }
}
