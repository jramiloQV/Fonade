using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Datos;

namespace Fonade.Negocio.PlanDeNegocioV2.Utilidad
{
    public class TabFormulacion
    {
        public static Boolean VerificarTabSiEstaCompleta(Int32 codigoTab, Int32 codigoProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from tabProyecto in db.TabProyectos
                              where
                                   tabProyecto.CodProyecto.Equals(codigoProyecto)
                                   && tabProyecto.CodTab.Equals(codigoTab)
                                   && tabProyecto.Completo.Equals(true)
                              select
                                   tabProyecto.Completo
                             ).Any();
                return entity;
            }
        }

        public static void UpdateTabCompleto(int codigoTab, int codigoProyecto, int codigoContacto, Boolean estado)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from tabProyecto in db.TabProyectos
                              where
                                   tabProyecto.CodProyecto.Equals(codigoProyecto)
                                   && tabProyecto.CodTab.Equals(codigoTab)
                              select
                                   tabProyecto
                             ).SingleOrDefault();

                if (entity == null)
                {
                    entity = new TabProyecto
                    {
                        CodContacto = codigoContacto,
                        CodProyecto = codigoProyecto,
                        FechaModificacion = DateTime.Now,
                        CodTab = (Int16)codigoTab,
                        Completo = estado,
                        Realizado = false
                    };
                    db.TabProyectos.InsertOnSubmit(entity);
                }
                else
                {
                    entity.Completo = estado;
                }

                db.SubmitChanges();                
            }
        }

        public static int NumerosTabsCompletos(Int32 codigoProyecto) {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                return db.TabProyectos.Where(filter => filter.CodProyecto.Equals(codigoProyecto)
                                                       && filter.Realizado.Equals(true)).Count();
            }
        }
    }
}
