using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Datos;

namespace Fonade.Negocio.PlanDeNegocioV2.Evaluacion.ConceptoFinal
{
    public static class Aportes
    {
        public static void Insert(EvaluacionProyectoAporte aporte)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                db.EvaluacionProyectoAportes.InsertOnSubmit(aporte);
                db.SubmitChanges();
            }
        }

    }
}
