using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Datos;

namespace Fonade.Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio
{
    public class Actividades
    {
        public static bool Insertar(ProyectoEstrategiaActividade entActividad, out string msg)
        {
            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    if ((from row in db.ProyectoEstrategiaActividades
                         where row.Actividad == entActividad.Actividad
                         where row.IdProyecto == entActividad.IdProyecto
                         where row.IdTipoEstrategia == entActividad.IdTipoEstrategia
                         select row).Count() > 0)
                    {
                        msg = Mensajes.Mensajes.GetMensaje(5);
                        return false;
                    }
                    db.ProyectoEstrategiaActividades.InsertOnSubmit(entActividad);
                    db.SubmitChanges();
                    msg = null;
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

        public static bool Editar(ProyectoEstrategiaActividade entActividad, out string msg)
        {
            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {

                    if ((from row in db.ProyectoEstrategiaActividades
                         where row.Actividad == entActividad.Actividad
                         where row.IdActividad != entActividad.IdActividad
                         where row.IdProyecto == entActividad.IdProyecto
                         where row.IdTipoEstrategia == entActividad.IdTipoEstrategia
                         select row).Count() > 0)
                    {
                        msg = Mensajes.Mensajes.GetMensaje(5);
                        return false;
                    }

                    var entActividaddb = (from row in db.ProyectoEstrategiaActividades
                                          where row.IdActividad == entActividad.IdActividad
                                          select row).First();

                    entActividaddb.Actividad = entActividad.Actividad;
                    entActividaddb.Costo = entActividad.Costo;
                    entActividaddb.MesEjecucion = entActividad.MesEjecucion;
                    entActividaddb.RecursosRequeridos = entActividad.RecursosRequeridos;
                    entActividaddb.Responsable = entActividad.Responsable;

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

        public static List<ProyectoEstrategiaActividade> Get(int idProyecto, int idTipoEstrategia)
        {

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {

                var listActividades = (from row in db.ProyectoEstrategiaActividades
                                       where row.IdProyecto == idProyecto
                                       where row.IdTipoEstrategia == idTipoEstrategia
                                       select row);

                return listActividades.ToList();

            }

        }

        public static ProyectoEstrategiaActividade Get(int idActividad)
        {

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {

                var listActividades = (from row in db.ProyectoEstrategiaActividades
                                       where row.IdActividad == idActividad
                                       select row);

                return listActividades.First();

            }

        }

        public static bool Eliminar(int IdActividad, out string msg)
        {
            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    var indicador = (from i in db.IndicadorMercadeoEvaluacions
                                where i.IdActividadMercadeo == IdActividad
                                select i).FirstOrDefault();

                    if (indicador != null)
                    {
                        db.IndicadorMercadeoEvaluacions.DeleteOnSubmit(indicador);
                        db.SubmitChanges();
                    }

                    var entActividaddb = (from row in db.ProyectoEstrategiaActividades
                                           where row.IdActividad == IdActividad
                                           select row).First();

                    db.ProyectoEstrategiaActividades.DeleteOnSubmit(entActividaddb);
                    db.SubmitChanges();
                    msg = Mensajes.Mensajes.GetMensaje(9);
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

    }
}
