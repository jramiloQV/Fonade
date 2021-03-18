using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Datos;
namespace Fonade.Negocio.PlanDeNegocioV2.Formulacion.OportunidadMercado
{
    public class Competidores
    {
        /// <summary>
        /// InsertarCompetidores
        /// </summary>
        /// <param name="entCliente"></param>
        /// <param name="msg">string</param>
        /// <returns>bool</returns>
        public static bool InsertarCompetidores(ProyectoOportunidadMercadoCompetidore entCompetidor, out string msg)
        {
            try
            {

                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    if ((from row in db.ProyectoOportunidadMercadoCompetidores where row.IdProyecto == entCompetidor.IdProyecto select row).Count() > 9)
                    {
                        msg = Mensajes.Mensajes.GetMensaje(13);
                        return false;
                    }
                    if ((from row in db.ProyectoOportunidadMercadoCompetidores
                         where row.Nombre == entCompetidor.Nombre
                         where row.IdProyecto == entCompetidor.IdProyecto
                         select row).Count() > 0)
                    {
                        msg = Mensajes.Mensajes.GetMensaje(5);
                        return false;
                    }

                    db.ProyectoOportunidadMercadoCompetidores.InsertOnSubmit(entCompetidor);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdProyecto"></param>
        /// <returns></returns>
        public static List<ProyectoOportunidadMercadoCompetidore> GetCompetidores(int IdProyecto)
        {

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {

                var listCompetidores = (from row in db.ProyectoOportunidadMercadoCompetidores
                                        where row.IdProyecto == IdProyecto
                                        select row);

                return listCompetidores.ToList();

            }

        }


        public static ProyectoOportunidadMercadoCompetidore GetCompetidor(int idCompetidor)
        {

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {

                var listCompetidores = (from row in db.ProyectoOportunidadMercadoCompetidores
                                    where row.IdCompetidor == idCompetidor 
                                    select row);

                return listCompetidores.First();

            }

        }

        /// <summary>
        /// EliminarClientes
        /// </summary>
        /// <param name="IdCliente">int</param>
        /// <param name="msg">string</param>
        /// <returns>bool</returns>
        public static bool EliminarCompetidor(int IdCompetidor, out string msg)
        {
            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    var entCompetidordb = (from row in db.ProyectoOportunidadMercadoCompetidores
                                           where row.IdCompetidor == IdCompetidor
                                           select row).First();

                    db.ProyectoOportunidadMercadoCompetidores.DeleteOnSubmit(entCompetidordb);
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

        public static bool EditarCompetidor(ProyectoOportunidadMercadoCompetidore entCompetidor, out string msg)
        {
            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {

                    if ((from row in db.ProyectoOportunidadMercadoCompetidores
                         where row.Nombre == entCompetidor.Nombre
                         where row.IdCompetidor != entCompetidor.IdCompetidor
                         where row.IdProyecto == entCompetidor.IdProyecto
                         select row).Count() > 0)
                    {
                        msg = Mensajes.Mensajes.GetMensaje(5);
                        return false;
                    }

                    var entCompetidordb = (from row in db.ProyectoOportunidadMercadoCompetidores
                                        where row.IdCompetidor == entCompetidor.IdCompetidor
                                        select row).First();

                    entCompetidordb.Nombre = entCompetidor.Nombre;
                    entCompetidordb.IdCompetidor = entCompetidor.IdCompetidor;
                    entCompetidordb.Localizacion = entCompetidor.Localizacion;
                    entCompetidordb.LogisticaDistribucion = entCompetidor.LogisticaDistribucion;
                    entCompetidordb.OtroCual = entCompetidor.OtroCual;
                    entCompetidordb.Precios = entCompetidor.Precios;
                    entCompetidordb.ProductosServicios = entCompetidor.ProductosServicios;
                    
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

    }
}
