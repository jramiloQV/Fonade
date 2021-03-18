using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Datos;

namespace Fonade.Negocio.PlanDeNegocioV2.Formulacion.OportunidadMercado
{
    public class Oportunidad
    {
        /// <summary>
        /// InsertarOportunidad
        /// </summary>
        /// <param name="entOportunidad">ProyectoOportunidadMercado</param>
        /// <param name="msg">string</param>
        /// <returns>bool</returns>
        public static bool InsertarOportunidad(ProyectoOportunidadMercado entOportunidad, out string msg)
        {
            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    var entOportunidaddb = (from row in db.ProyectoOportunidadMercados
                                            where row.IdProyecto == entOportunidad.IdProyecto
                                            select row).FirstOrDefault();

                    //insert-update
                    if (entOportunidaddb == null)
                        db.ProyectoOportunidadMercados.InsertOnSubmit(entOportunidad);
                    else
                    {
                        entOportunidaddb.TendenciaCrecimiento = entOportunidad.TendenciaCrecimiento;
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
        /// GetOportunidad
        /// </summary>
        /// <param name="IdOportunidadMercado">int</param>
        /// <returns>ProyectoOportunidadMercado</returns>
        public static ProyectoOportunidadMercado GetOportunidad(int idProyecto)
        {

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {

                var entOportunidaddb = (from row in db.ProyectoOportunidadMercados
                                        where row.IdProyecto == idProyecto
                                        select row).FirstOrDefault();

                return entOportunidaddb;
            }
        }


    }
}
