using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.PlanDeNegocioV2.Ejecucion.Empresa
{
    public class Empresa
    {

        public static bool IsDataCompleteOnRegistroMercantil(int codigoProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                return db.Empresas.Any(filter => filter.codproyecto.Equals(codigoProyecto) && filter.Nit != "");
            }
        }

    }
}
