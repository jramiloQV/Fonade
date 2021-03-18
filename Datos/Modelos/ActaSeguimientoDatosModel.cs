using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos.Modelos
{
    public class ActaSeguimientoDatosModel
    {
        public int idActa { get; set; }
        public int numActa { get; set; }
        public string NumContrato { get; set; }
        public DateTime FechaActaInicio { get; set; }
        public string Prorroga { get; set; }
        public string NombrePlanNegocio { get; set; }
        public string NombreEmpresa { get; set; }
        public string NitEmpresa { get; set; }
        public string ContratoMarcoInteradmin { get; set; }
        public string ContratoInterventoria { get; set; }
        public string Contratista { get; set; }
        public string ValorAprobado { get; set; }
        public string DomicilioPrincipal { get; set; }
        public string Convocatoria { get; set; }
        public string SectorEconomico { get; set; }
        public string SubSectorEconomico { get; set; }
        public string ObjetoProyecto { get; set; }
        public string ObjetoVisita { get; set; }
        public string NombreGestorTecnicoSena { get; set; }
        public string EmailGestorTecnicoSena { get; set; }
        public string TelefonoGestorTecnicoSena { get; set; }
        public string NombreGestorOperativoSena { get; set; }
        public string EmailGestorOperativoSena { get; set; }
        public string TelefonoGestorOperativoSena { get; set; }
        public DateTime FechaActualizado { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public DateTime? FechaVisita { get; set; }
        public DateTime? FechaFinalVisita { get; set; }
    }
}
