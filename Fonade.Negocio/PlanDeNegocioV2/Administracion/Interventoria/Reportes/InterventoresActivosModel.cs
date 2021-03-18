using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.PlanDeNegocioV2.Administracion.Interventoria.Reportes
{
    public class InterventoresActivosModel
    {
        public int codInterventor { get; set; }
        public string nomInterventor { get; set; }
        public int? codCoordinador { get; set; }
        public int? codOperador { get; set; }
    }
}
