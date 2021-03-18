using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Datos;
namespace Fonade.Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio
{
    public class FuturoNegocio
    {

        public static bool Insertar(ProyectoFuturoNegocio entfuturo, out string msg)
        {
            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    var entFuturodb = (from row in db.ProyectoFuturoNegocios
                                       where row.IdProyecto == entfuturo.IdProyecto
                                       select row).FirstOrDefault();

                    //insert-update
                    if (entFuturodb == null)
                        db.ProyectoFuturoNegocios.InsertOnSubmit(entfuturo);
                    else
                    {
                        entFuturodb.EstrategiaComunicacion = entfuturo.EstrategiaComunicacion;
                        entFuturodb.EstrategiaComunicacionProposito = entfuturo.EstrategiaComunicacionProposito;
                        entFuturodb.EstrategiaDistribucion = entfuturo.EstrategiaDistribucion;
                        entFuturodb.EstrategiaDistribucionProposito = entfuturo.EstrategiaDistribucionProposito;
                        entFuturodb.EstrategiaPromocion = entfuturo.EstrategiaPromocion;
                        entFuturodb.EstrategiaPromocionProposito = entfuturo.EstrategiaPromocionProposito;
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

        public static ProyectoFuturoNegocio Get(int idProyecto)
        {

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {

                var entFuturodb = (from row in db.ProyectoFuturoNegocios
                                        where row.IdProyecto == idProyecto
                                   select row).FirstOrDefault();

                return entFuturodb;
            }
        }

    }
}
