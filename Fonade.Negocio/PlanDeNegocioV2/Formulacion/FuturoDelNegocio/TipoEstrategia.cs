using System.ComponentModel;
namespace Fonade.Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio
{
    public enum TipoEstrategia
    {
        [Description("Estrategia de Promoción")]
        Promocion = 1,
        [Description("Estrategia de Comunicación")]
        Comunicacion,
        [Description("Estrategia de Distribución")]
        Distribucion
    };
}
