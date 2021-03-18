using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Datos;

namespace Fonade.Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio
{
    public class PeriodoArranque
    {


        public static bool Insertar(ProyectoPeriodoArranque entArranque, out string msg)
        {
            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    var entArranquedb = (from row in db.ProyectoPeriodoArranques
                                         where row.IdProyecto == entArranque.IdProyecto
                                         select row).FirstOrDefault();

                    //insert-update
                    if (entArranquedb == null)
                        db.ProyectoPeriodoArranques.InsertOnSubmit(entArranque);
                    else
                    {
                        entArranquedb.PeriodoArranque = entArranque.PeriodoArranque;
                        entArranquedb.PeriodoImproductivo = entArranque.PeriodoImproductivo;

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

        public static ProyectoPeriodoArranque Get(int idProyecto)
        {

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {

                var entArranquedb = (from row in db.ProyectoPeriodoArranques
                                     where row.IdProyecto == idProyecto
                                     select row).FirstOrDefault();

                return entArranquedb;
            }
        }


    }
}
