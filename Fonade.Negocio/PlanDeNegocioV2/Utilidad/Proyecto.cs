using System;
using System.Linq;
using System.Web.UI.WebControls;
using Datos;
using System.Globalization;
using System.Web;

namespace Fonade.Negocio.PlanDeNegocio2017.Utilidad
{
    public static class ProyectoGeneral
    {
        public static void GetUltimaActualizacion(Label lblUltimaActualizacion, Label lblFechaUltimaActualizacion, CheckBox chkEsRealizado, Button btnSaveTab, Int32 codigoTab, Int32 codigoProyecto)
        {
            var update = GetUpdateInfo(codigoTab, codigoProyecto);

            if (update == null)
            {
                lblUltimaActualizacion.Text = string.Empty;
                lblFechaUltimaActualizacion.Text = string.Empty;
                chkEsRealizado.Checked = false;
                btnSaveTab.Visible = false;
            }
            else
            {
                lblUltimaActualizacion.Text = update.nombres;
                lblFechaUltimaActualizacion.Text = update.fecha.getFechaAbreviadaConFormato(true);
                chkEsRealizado.Checked = update.realizado;
            }
        }

        public static Boolean VerificarTabSiEsRealizado(Int32 codigoTab, Int32 codigoProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from tabProyecto in db.TabProyectos
                              where
                                   tabProyecto.CodProyecto.Equals(codigoProyecto)
                                   && tabProyecto.CodTab.Equals(codigoTab)
                                   && tabProyecto.Realizado.Equals(true)
                              select
                                   tabProyecto.Realizado
                             ).Any();

                return entity;
            }
        }

        public static Boolean EsMienbroDelProyecto(Int32 codigoProyecto, Int32 codigoContacto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from proyectoContacto in db.ProyectoContactos
                              where
                                   proyectoContacto.CodProyecto == codigoProyecto
                                   && proyectoContacto.CodContacto == codigoContacto
                                   && proyectoContacto.Inactivo == false
                                   && proyectoContacto.FechaInicio.Date <= DateTime.Now.Date
                                   && proyectoContacto.FechaFin == null
                              select
                                   proyectoContacto
                          ).ToList().FirstOrDefault();

                return entity != null ? true : false;
            }
        }

        public static Datos.Consultas.InfoActualiza GetUpdateInfo(Int32 codigoTab, Int32 codigoProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from tabs in db.TabProyectos
                              join contactos in db.Contacto on tabs.CodContacto equals contactos.Id_Contacto
                              where tabs.CodProyecto == codigoProyecto
                                    && tabs.CodTab == codigoTab
                              select new Datos.Consultas.InfoActualiza()
                              {
                                  nombres = contactos.Nombres + " " + contactos.Apellidos,
                                  fecha = tabs.FechaModificacion,
                                  realizado = tabs.Realizado
                              }).SingleOrDefault();
                return entity;
            }
        }

        public static Boolean AllowCheckTab(int codigoGrupo, int codigoProyecto, int codigoTab, Boolean esMiembro)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var estadoValido = db.Proyecto.Any(filter => filter.CodEstado == Constantes.CONST_Registro_y_Asesoria && filter.Id_Proyecto == codigoProyecto);
                var isUpdated = GetUpdateInfo(codigoTab, codigoProyecto) != null;

                return estadoValido && esMiembro && codigoGrupo.Equals(Constantes.CONST_Asesor) && isUpdated;
            }
        }
       
        
        public static void UpdateTab(int codigoTab, int codigoProyecto, int codigoContacto, int codigoGrupo, Boolean estado = false)
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
                        Realizado = false
                    };
                    db.TabProyectos.InsertOnSubmit(entity);
                }
                else
                {
                    if (codigoGrupo.Equals(Constantes.CONST_Emprendedor))
                    {
                        entity.CodContacto = codigoContacto;
                        entity.FechaModificacion = DateTime.Now;
                    }

                    if (codigoGrupo.Equals(Constantes.CONST_Asesor))
                        entity.Realizado = estado;
                }

                db.SubmitChanges();

                UpdateTabParent(codigoTab, codigoProyecto, codigoContacto, codigoGrupo);
            }
        }

        public static string getFechaAbreviadaConFormato(this DateTime fechaSinFormato, Boolean showTime = false)
        {
            string diaSemana = CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(fechaSinFormato.DayOfWeek);
            string mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(fechaSinFormato.Month);
            string hora = showTime ? " " + fechaSinFormato.ToString("hh:mm") + " " + fechaSinFormato.ToString("tt", CultureInfo.CurrentCulture) : string.Empty;

            string fechaConFormato = fechaSinFormato.Day + " " + mes + " " + fechaSinFormato.Year + hora;

            return fechaConFormato;
        }

        /// <summary>
        /// Actualiza el tab padre a realizado si todos los hijos estan realizados.
        /// </summary>
        public static void UpdateTabParent(int codigoTab, int codigoProyecto, int codigoContacto, int codigoGrupo)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var tabDetalle = db.Tabs.SingleOrDefault(filter => filter.Id_Tab.Equals(codigoTab));

                if (tabDetalle.CodTab != null) // Verifica si existe un padre para ese tab
                {
                    var codigoTabPadre = tabDetalle.CodTab;

                    if (!db.TabProyectos.Any(filter => filter.CodProyecto.Equals(codigoProyecto)
                                       && filter.CodTab.Equals(codigoTabPadre)))
                    {
                        ///Inserta tab padre sino existe en tabProyecto
                        var newTab = new TabProyecto
                        {
                            CodContacto = codigoContacto,
                            CodProyecto = codigoProyecto,
                            FechaModificacion = DateTime.Now,
                            CodTab = (Int16)codigoTabPadre,
                            Realizado = false
                        };
                        db.TabProyectos.InsertOnSubmit(newTab);
                        db.SubmitChanges();
                    }

                    if (codigoGrupo.Equals(Constantes.CONST_Asesor))
                    {
                        var entity = (from tabProyecto in db.TabProyectos
                                      where
                                           tabProyecto.CodProyecto.Equals(codigoProyecto)
                                           && tabProyecto.CodTab.Equals(codigoTabPadre)
                                      select
                                           tabProyecto
                                     ).SingleOrDefault();

                        var childTabs = db.Tabs.Count(filter => filter.CodTab.Equals(codigoTabPadre)); // Verifica cuantos tabs hijas tiene ese tabPadre
                        var checkedTabs = (from tabProyecto in db.TabProyectos
                                           join tab in db.Tabs on tabProyecto.CodTab equals tab.Id_Tab
                                           where tab.CodTab.Equals(codigoTabPadre)
                                                 && tabProyecto.CodProyecto.Equals(codigoProyecto)
                                                 && tabProyecto.Realizado.Equals(true)
                                           select tabProyecto
                                               ).Count(); // Verifica cuantos hijos estan realizados

                        if (childTabs.Equals(checkedTabs))
                        {
                            entity.Realizado = true;
                            db.SubmitChanges();
                        }
                        else
                        {
                            if (entity.Realizado.Equals(true))
                            {
                                entity.Realizado = false;
                                db.SubmitChanges();
                            }
                        }
                    }
                }
            }
        }

        public static Datos.Proyecto GetProjectInfo(int codigoProyecto) {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                return db.Proyecto.FirstOrDefault(filter => filter.Id_Proyecto == codigoProyecto);
            }
        }
    }
}
