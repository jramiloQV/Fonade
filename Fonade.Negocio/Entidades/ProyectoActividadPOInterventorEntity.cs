using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.Entidades
{
    public class ProyectoActividadPOInterventorEntity
    {
        /// <summary> Clase de tipo Entidad que hace referecia a la tabla
        /// ProyectoActividadPOInterventor
        /// <remarks>2014/10/25 Alex Flautero</remarks>
        public int Id_Actividad {get; set;}
        public String NomActividad {get; set;}
        public int CodProyecto {get; set;}
        public Double Item {get; set;}
        public String Metas {get; set;}
    }
}
