
namespace Fonade.Negocio.Entidades
{
    /// <summary>
    /// Clase Entidad para los datos del reporte  ImprimirPlanOperativo
    /// </summary>
    /// <remarks>2014711/27  RAlvaradoT </remarks>
    public class PlanOperativoEntity
    {
       
        public int CodActividad { get; set; }
        public short Mes { get; set; }
        public short CodTipoFinanciacion { get; set; }
        public double Valor { get; set; }
        public int Id_Actividad { get; set; }
        public string NomActividad { get; set; }
        public int CodProyecto { get; set; }
        public short Item { get; set; }
        public string Metas { get; set; }

    }
}
