using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.PlanDeNegocioV2.Administracion.Interventoria.Reportes
{
    [Serializable]
    public class EstadisticasAvancesModel
    {
        public int idProyecto { get; set; }
        public int codActividad { get; set; }
        public string nomActividad { get; set; }
        public int item { get; set; }
        public int mes { get; set; }
        public DateTime? fechaAvanceEmprendedor { get; set; }
        public string observacionesEmprendedor { get; set; }
        public DateTime? fechaAprobacionInterventor { get; set; }
        public string observacionesInterventor { get; set; }
        public string Aprobada { get; set; }
        public int codInterventor { get; set; }
        public string nomInterventor { get; set; }
        public string nomEntidad { get; set; }
        public string nomOperador { get; set; }
    }
}
