using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos.Modelos
{
    public class ActaSeguimientoInterventoriaModelDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int IdTipoActa { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public bool Publicado { get; set; }
        public string ArchivoActa { get; set; }
        public int IdProyecto { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public int NumeroActa { get; set; }

    }
}
