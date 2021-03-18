using System;
using System.Linq;
using System.Web.UI.WebControls;
using Datos;
using System.Globalization;
using System.Web;

namespace Fonade.Negocio.PlanDeNegocioV2.Utilidad
{
    public static class TabEvaluacion
    {
        #region Metodos

        /// <summary>
        /// Verifica si el tab esta realizado
        /// </summary>
        /// <param name="codigoTab">int</param>
        /// <param name="codigoProyecto">Int32</param>
        /// <param name="codigoConvocatoria">Int32</param>
        /// <returns>Boolean</returns>
        public static Boolean VerificarTabSiEsRealizado(Int32 codigoTab, Int32 codigoProyecto, Int32 codigoConvocatoria)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from tabProyecto in db.TabEvaluacionProyectos
                              where
                                   tabProyecto.CodProyecto.Equals(codigoProyecto)
                                   && tabProyecto.CodTabEvaluacion.Equals(codigoTab)
                                   && tabProyecto.CodConvocatoria.Equals(codigoConvocatoria)
                                   && tabProyecto.Realizado.Equals(true)
                              select
                                   tabProyecto.Realizado
                             ).Any();

                return entity;
            }
        }

        /// <summary>
        /// Marca un tab como realizado
        /// </summary>
        /// <param name="entTabEvaluacionProyecto">TabEvaluacionProyecto</param>
        /// <param name="msg">string</param>
        /// <returns>bool?</returns>
        public static bool? SetRealizado(TabEvaluacionProyecto entTabEvaluacionProyecto, out string msg)
        {
            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    var entitydb = (from row in db.TabEvaluacionProyectos
                                    where row.CodProyecto == entTabEvaluacionProyecto.CodProyecto &&
                                    row.CodConvocatoria == entTabEvaluacionProyecto.CodConvocatoria &&
                                    row.CodTabEvaluacion == entTabEvaluacionProyecto.CodTabEvaluacion
                                    select row).FirstOrDefault();

                    //insert-update marcar tab realizado
                    if (entitydb == null)
                    {
                        db.TabEvaluacionProyectos.InsertOnSubmit(entTabEvaluacionProyecto);
                    }
                    else
                    {
                        entitydb.Realizado = entTabEvaluacionProyecto.Realizado;
                    }

                    db.SubmitChanges();

                    var hijo = (from row in db.TabEvaluacions
                                where row.Id_TabEvaluacion == entTabEvaluacionProyecto.CodTabEvaluacion
                                select row).FirstOrDefault();
                    //obtiene el padre del tab
                    var entityPadre = (from row in db.TabEvaluacionProyectos
                                       where row.CodTabEvaluacion == hijo.CodTabEvaluacion &&
                                       row.CodProyecto == entTabEvaluacionProyecto.CodProyecto
                                       && row.CodConvocatoria == entTabEvaluacionProyecto.CodConvocatoria
                                       select row).FirstOrDefault();
                    //cantidad de hijos
                    var entityHijos = (from row in db.TabEvaluacions
                                       where row.CodTabEvaluacion == hijo.CodTabEvaluacion
                                       select row).ToList();
                    //cantidad de hijos realizados
                    var entityHijosRealizado = (from row in db.TabEvaluacions
                                                join tep in db.TabEvaluacionProyectos
                                                on row.Id_TabEvaluacion equals tep.CodTabEvaluacion
                                                where row.CodTabEvaluacion == hijo.CodTabEvaluacion && tep.Realizado == true &&
                                                tep.CodProyecto == entTabEvaluacionProyecto.CodProyecto
                                                && tep.CodConvocatoria == entTabEvaluacionProyecto.CodConvocatoria
                                                select row).ToList();

                    //marcar padre como realizado
                    bool padreRealizado = entityHijos.Count().Equals(entityHijosRealizado.Count());
                    if (entityPadre == null)
                    {
                        TabEvaluacionProyecto entPadre = new TabEvaluacionProyecto()
                        {
                            CodContacto = (int)entTabEvaluacionProyecto.CodContacto,
                            CodConvocatoria = entTabEvaluacionProyecto.CodConvocatoria,
                            CodProyecto = entTabEvaluacionProyecto.CodProyecto,
                            CodTabEvaluacion = (short)hijo.CodTabEvaluacion,
                            Realizado = padreRealizado,
                            FechaModificacion = DateTime.Now
                        };
                        db.TabEvaluacionProyectos.InsertOnSubmit(entPadre);
                    }
                    else
                    {
                        entityPadre.CodContacto = entTabEvaluacionProyecto.CodContacto;
                        entityPadre.FechaModificacion = DateTime.Now;
                        entityPadre.Realizado = padreRealizado;
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

        /// <summary>
        /// Obtiene la ultima actualización del tab
        /// </summary>
        /// <param name="entTabEvaluacionProyecto">TabEvaluacionProyecto</param>
        /// <returns>TabEvaluacionProyecto</returns>
        public static TabEvaluacionProyecto GetUltimaActualizacion(TabEvaluacionProyecto entTabEvaluacionProyecto)
        {
            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    var entitydb = (from row in db.TabEvaluacionProyectos
                                    where row.CodProyecto == entTabEvaluacionProyecto.CodProyecto &&
                                    row.CodConvocatoria == entTabEvaluacionProyecto.CodConvocatoria &&
                                    row.CodTabEvaluacion == entTabEvaluacionProyecto.CodTabEvaluacion
                                    select row).FirstOrDefault();
                    if (entitydb == null)
                        return null;

                    entitydb.Contacto = (from row in db.Contacto
                                         where row.Id_Contacto == entitydb.CodContacto
                                         select row).FirstOrDefault();

                    return entitydb;

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Estblece la fecha de la ultima actualización del tab
        /// </summary>
        /// <param name="entTabEvaluacionProyecto">TabEvaluacionProyecto</param>
        /// <param name="msg">out string</param>
        /// <returns>bool</returns>
        public static bool SetUltimaActualizacion(TabEvaluacionProyecto entTabEvaluacionProyecto, out string msg)
        {
            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    var entitydb = (from row in db.TabEvaluacionProyectos
                                    where row.CodProyecto == entTabEvaluacionProyecto.CodProyecto &&
                                    row.CodConvocatoria == entTabEvaluacionProyecto.CodConvocatoria &&
                                    row.CodTabEvaluacion == entTabEvaluacionProyecto.CodTabEvaluacion
                                    select row).FirstOrDefault();

                    //insert-update marcar tab realizado
                    if (entitydb == null)
                    {
                        db.TabEvaluacionProyectos.InsertOnSubmit(entTabEvaluacionProyecto);
                    }
                    else
                    {
                        entitydb.CodContacto = entTabEvaluacionProyecto.CodContacto;
                        entitydb.FechaModificacion = DateTime.Now;
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


        public static bool TabEvaluacionCompleto(TabEvaluacionProyecto entTabEvaluacionProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entitydb = (from row in db.TabEvaluacionProyectos
                                where row.CodProyecto == entTabEvaluacionProyecto.CodProyecto &&
                                row.CodConvocatoria == entTabEvaluacionProyecto.CodConvocatoria &&
                                row.CodTabEvaluacion == entTabEvaluacionProyecto.CodTabEvaluacion
                                select row).FirstOrDefault();
                if (entitydb == null)
                    return false;
                else
                    return true;
            }
        }

        #endregion

    }
}
