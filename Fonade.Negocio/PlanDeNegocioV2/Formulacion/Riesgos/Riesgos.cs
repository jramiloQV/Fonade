using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Datos;
namespace Fonade.Negocio.PlanDeNegocioV2.Formulacion.Riesgos
{
   public class Riesgos
    {
        public static bool Insertar(ProyectoRiesgo entRiesgo, out string msg)
        {
            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    var entRiesgodb = (from row in db.ProyectoRiesgos
                                         where row.IdProyecto == entRiesgo.IdProyecto
                                         select row).FirstOrDefault();

                    //insert-update
                    if (entRiesgodb == null)
                        db.ProyectoRiesgos.InsertOnSubmit(entRiesgo);
                    else
                    {
                        entRiesgodb.ActoresExternos = entRiesgo.ActoresExternos;
                        entRiesgodb.FactoresExternos = entRiesgo.FactoresExternos;

                    }
                    db.SubmitChanges();

                    msg = Mensajes.Mensajes.GetMensaje(8);
                    return true;

                }
            }
            catch (Exception ex)
            {
                //todo guardar log
                msg = Mensajes.Mensajes.GetMensaje(7);
                return false;
            }
        }

        public static ProyectoRiesgo Get(int idProyecto)
        {

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {

                var entRiesgodb = (from row in db.ProyectoRiesgos
                                     where row.IdProyecto == idProyecto
                                     select row).FirstOrDefault();

                return entRiesgodb;
            }
        }
    }
}
