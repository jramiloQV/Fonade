using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.Entidades
{
    public class ProyectoActividadPOMesInterventorEntity
    {
        /// <summary> Clase de tipo Entidad que hace referecia a la tabla
        /// ProyectoActividadPOMesInterventor 
        /// </summary>
        public int CodActividad {get; set;}
        public int Mes {get; set;}
        public int CodTipoFinanciacion {get; set;}
        public Double Valor { get; set; }

    }
}
