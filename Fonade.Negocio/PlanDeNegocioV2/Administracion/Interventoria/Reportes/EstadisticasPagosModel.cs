using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.PlanDeNegocioV2.Administracion.Interventoria.Reportes
{
    [Serializable]
    public class EstadisticasPagosModel
    {
        public int idProyecto { get; set; }
        public string nomProyecto { get; set; }
        public int codInterventor { get; set; }
        public string nombreInterventor { get; set; }
        public DateTime? fechaAprobInterventor { get; set; }
        public DateTime? fechaAprobORechaCoordinador { get; set; }
        public DateTime? fechaRespuestaFiducia { get; set; }
        public int idPagoActividad { get; set; }
        public string nomPagoActividad { get; set; }
        public decimal? cantidadDinero { get; set; }
        public string estado { get; set; }
        public string observacionFiduciaOCoordinador { get; set; }
        public int codCoordinador { get; set; }
        public string nombreCoordinador { get; set; }
        public int? codOperador { get; set; }
        public string Operador { get; set; }
    }
}
