using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos.Modelos
{
    public class ActaSeguimientoModel
    {
        public int idActa { get; set; }        
        public string Nombre { get; set; }
        public int idTipoActa { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public int idUsuarioCreacion { get; set;}
        public bool Publicado { get; set; }
        public string ArchivoActa { get; set; }
        public int idProyecto { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public DateTime FechaFinalVisita { get; set; }
        public int NumeroActa { get; set; }

    }
}
