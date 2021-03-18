using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.Entidades
{
    public class ProyectoActividadPOInterventorTMPEntity
    {
        /// <summary> Clase de tipo Entidad que hace referecia a la tabla
        /// ProyectoActividadPOInterventorTMP
        // Noviembre 25 de 2014 - Alex Flautero
        public int Id_Actividad {get; set;}
        public String NomActividad {get; set;}
        public int CodProyecto {get; set;}
        public Double Item { get; set; }
        public String Metas {get; set;}
        public Boolean ChequeoCoordinador {get; set;}
        public String Tarea {get; set;}
        public Boolean ChequeoGerente { get; set; }
    }
}
