using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.PlanDeNegocioV2.Interventoria
{
    public class RecomendacionInterventoriaBLL
    {
        static string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public static List<RecomendacionInterventoriaModel> getRecomendaciones()
        {
            List<RecomendacionInterventoriaModel> recomendaciones = new List<RecomendacionInterventoriaModel>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                recomendaciones = (from r in db.RecomendacionInterventoria
                                   select new RecomendacionInterventoriaModel
                                   {
                                       idRecomendacion = r.idRecomendacion,
                                       Recomendacion = r.RecomendacionInterventoria1
                                   }).ToList();
            }

            return recomendaciones;
        }
    }

    public class RecomendacionInterventoriaModel
    {
        public int idRecomendacion { get; set; }
        public string Recomendacion { get; set; }
    }
}
