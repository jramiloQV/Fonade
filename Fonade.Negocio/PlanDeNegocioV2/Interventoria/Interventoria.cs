using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.PlanDeNegocioV2.Interventoria
{
    public static class Interventoria
    {
        public static double PresupuestoDisponible(int codigoProyecto, int codigoPago)
        {
            try
            {
                var PresupuestoRecomendado = PresupuestoRecomendadoInterventoria(codigoProyecto);
                var PresupuestoEjecutado = PresupuestoAprobadoInterventoria(codigoProyecto,codigoPago);

                return PresupuestoRecomendado - PresupuestoEjecutado;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private static double PresupuestoRecomendadoInterventoria(int codigoProyecto) {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var Presupuesto = (from Observacion in db.EvaluacionObservacions
                                   join Convocatorias in db.Convocatoria on Observacion.CodConvocatoria equals Convocatorias.Id_Convocatoria
                                   join SalarioVigente in db.SalariosMinimos on Convocatorias.FechaInicio.Year equals SalarioVigente.AñoSalario
                                   where Observacion.CodProyecto.Equals(codigoProyecto)
                                   orderby Observacion.CodConvocatoria descending
                                   select new
                                   {
                                       salarioRecomendado = Observacion.ValorRecomendado,
                                       salarioAnno = SalarioVigente.SalarioMinimo
                                   }).FirstOrDefault();

                if (Presupuesto == null)
                    throw new ApplicationException("Error al calcular el presupuesto.");

                return Presupuesto != null ? (Presupuesto.salarioAnno * Presupuesto.salarioRecomendado).GetValueOrDefault(0) : 0;
            }            
        }

        public static double PresupuestoAprobadoInterventoria(int codigoProyecto, int? codigoPagoIgnorado = null)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var presupuestoAprobado = db.PagoActividad.Where(filter =>
                    filter.Estado >= 1
                    && filter.Estado < 5
                    && filter.CodProyecto.Equals(codigoProyecto)
                    && (codigoPagoIgnorado == null || (codigoPagoIgnorado != null && !filter.Id_PagoActividad.Equals(codigoPagoIgnorado)))
                ).Sum(sumatory => (Decimal?) sumatory.CantidadDinero) ?? 0;

                return (double)presupuestoAprobado;
            }
        }
    }
}
