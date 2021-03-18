using Datos;
using Datos.Modelos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.FonDBLight
{
    public class ActaSeguimGestionMercadeoController
    {
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        public List<ActaSeguimGestionMercadeoModel> GetGestionMercadeo(int _codProyecto, int _codConvocatoria)
        {
            List<ActaSeguimGestionMercadeoModel> listGestionMercadeo = new List<ActaSeguimGestionMercadeoModel>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                listGestionMercadeo = (from e in db.ActaSeguimGestionMercadeo
                                     where e.codProyecto == _codProyecto && e.codConvocatoria == _codConvocatoria
                                     orderby e.numActa
                                     select new ActaSeguimGestionMercadeoModel
                                     {
                                         id = e.idActaGestionMercadeo,
                                         codProyecto = e.codProyecto,
                                         codConvocatoria = e.codConvocatoria,
                                         numActa = e.numActa,
                                         visita = e.Visita,
                                         cantidad = e.Cantidad,
                                         descripcionEvento = e.DescripcionEvento,
                                         publicidadLogos = e.PublicidadLogos,
                                         fechaIngreso = e.FechaIngreso
                                     }).ToList();
            }

            return listGestionMercadeo;

        }

        public bool InsertOrUpdateGestionMercadeo(ActaSeguimGestionMercadeoModel gestionMercadeo)
        {
            bool insertado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {

                var actaMercadeo = (from g in db.ActaSeguimGestionMercadeo
                                  where g.codConvocatoria == gestionMercadeo.codConvocatoria
                                  && g.codProyecto == gestionMercadeo.codProyecto
                                  && g.numActa == gestionMercadeo.numActa
                                  select g).FirstOrDefault();

                if (actaMercadeo != null)//Actualizar
                {
                    actaMercadeo.Cantidad = gestionMercadeo.cantidad;
                    actaMercadeo.DescripcionEvento = gestionMercadeo.descripcionEvento;
                    actaMercadeo.PublicidadLogos = gestionMercadeo.publicidadLogos;
                    actaMercadeo.FechaIngreso = DateTime.Now;
                }
                else//Insertar
                {
                    ActaSeguimGestionMercadeo gesMercadeo = new ActaSeguimGestionMercadeo
                    {
                        Cantidad = gestionMercadeo.cantidad,
                        PublicidadLogos = gestionMercadeo.publicidadLogos,
                        DescripcionEvento = gestionMercadeo.descripcionEvento,
                        codConvocatoria = gestionMercadeo.codConvocatoria,
                        codProyecto = gestionMercadeo.codProyecto,
                        numActa = gestionMercadeo.numActa,
                        Visita = gestionMercadeo.visita,
                        FechaIngreso = DateTime.Now
                    };

                    db.ActaSeguimGestionMercadeo.InsertOnSubmit(gesMercadeo);
                }

                db.SubmitChanges();

                insertado = true;
            }

            return insertado;
        }
    }
}
