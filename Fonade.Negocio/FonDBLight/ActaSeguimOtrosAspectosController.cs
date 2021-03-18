using Datos;
using Datos.Modelos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.FonDBLight
{
    public class ActaSeguimOtrosAspectosController
    {
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public ActaSeguimOtrosAspectosModel getDescripcionOtrosAspectos(int _codProyecto, int _codConvocatoria)
        {
            ActaSeguimOtrosAspectosModel descripciones = new ActaSeguimOtrosAspectosModel();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                descripciones = (from e in db.ActaSeguimOtrosAspectos
                                 where e.codProyecto == _codProyecto && e.codConvocatoria == _codConvocatoria
                                 orderby e.numActa
                                 select new ActaSeguimOtrosAspectosModel
                                 {
                                     id = e.idOtrosAspectos,
                                     codProyecto = e.codProyecto,
                                     codConvocatoria = e.codConvocatoria,
                                     numActa = e.numActa,
                                     visita = e.visita,
                                     DescripCompAmbiental = e.DescripCompAmbiental,
                                     DescripCompInnovador = e.DescripCompInnovador
                                 }).FirstOrDefault();
            }

            return descripciones;
        }

        public List<ActaSeguimOtrosAspInnovadorModel> getOtrosAspectosInnovador(int _codProyecto, int _codConvocatoria)
        {
            List<ActaSeguimOtrosAspInnovadorModel> descripciones = new List<ActaSeguimOtrosAspInnovadorModel>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                descripciones = (from e in db.ActaSeguimOtrosAspInnovador
                                 where e.codProyecto == _codProyecto && e.codConvocatoria == _codConvocatoria
                                 orderby e.numActa
                                 select new ActaSeguimOtrosAspInnovadorModel
                                 {
                                     id = e.idOtroAspInnovador,
                                     codProyecto = e.codProyecto,
                                     codConvocatoria = e.codConvocatoria,
                                     numActa = e.numActa,
                                     visita = e.visita,
                                     observacion = e.observacion,
                                     valoracion = e.valoracion
                                 }).ToList();
            }

            return descripciones;
        }

        public List<ActaSeguimOtrosAspAmbientalModel> getOtrosAspectosAmbiental(int _codProyecto, int _codConvocatoria)
        {
            List<ActaSeguimOtrosAspAmbientalModel> descripciones = new List<ActaSeguimOtrosAspAmbientalModel>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                descripciones = (from e in db.ActaSeguimOtrosAspAmbiental
                                 where e.codProyecto == _codProyecto && e.codConvocatoria == _codConvocatoria
                                 orderby e.numActa
                                 select new ActaSeguimOtrosAspAmbientalModel
                                 {
                                     id = e.idOtrosAspAmbiental,
                                     codProyecto = e.codProyecto,
                                     codConvocatoria = e.codConvocatoria,
                                     numActa = e.numActa,
                                     visita = e.visita,
                                     observacion = e.observacion,
                                     valoracion = e.valoracion
                                 }).ToList();
            }

            return descripciones;
        }
        public bool InsertOrUpdateDescripcionOtrosAspectos(ActaSeguimOtrosAspectosModel descripOtrosAsp)
        {
            bool insertado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                var actaDescrOtrosAsp = (from g in db.ActaSeguimOtrosAspectos
                                         where g.codConvocatoria == descripOtrosAsp.codConvocatoria
                                         && g.codProyecto == descripOtrosAsp.codProyecto
                                         && g.numActa == descripOtrosAsp.numActa
                                         select g).FirstOrDefault();

                if (actaDescrOtrosAsp != null)//Actualizar
                {
                    actaDescrOtrosAsp.DescripCompAmbiental = descripOtrosAsp.DescripCompAmbiental;
                    actaDescrOtrosAsp.DescripCompInnovador = descripOtrosAsp.DescripCompInnovador;
                    actaDescrOtrosAsp.FechaIngresado = DateTime.Now;

                }
                else//Insertar
                {
                    ActaSeguimOtrosAspectos gesOtrosApsectos = new ActaSeguimOtrosAspectos
                    {
                        DescripCompAmbiental = descripOtrosAsp.DescripCompAmbiental,
                        codConvocatoria = descripOtrosAsp.codConvocatoria,
                        codProyecto = descripOtrosAsp.codProyecto,
                        numActa = descripOtrosAsp.numActa,
                        visita = descripOtrosAsp.visita,
                        FechaIngresado = DateTime.Now,
                        DescripCompInnovador = descripOtrosAsp.DescripCompInnovador
                    };

                    db.ActaSeguimOtrosAspectos.InsertOnSubmit(gesOtrosApsectos);
                }

                db.SubmitChanges();

                insertado = true;
            }

            return insertado;
        }

        public bool InsertOrUpdateOtrosAspInnovador(ActaSeguimOtrosAspInnovadorModel otrosAspInnovador)
        {
            bool insertado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                var actaOtrosAspInnovador = (from g in db.ActaSeguimOtrosAspInnovador
                                         where g.codConvocatoria == otrosAspInnovador.codConvocatoria
                                         && g.codProyecto == otrosAspInnovador.codProyecto
                                         && g.numActa == otrosAspInnovador.numActa
                                         select g).FirstOrDefault();

                if (actaOtrosAspInnovador != null)//Actualizar
                {
                    actaOtrosAspInnovador.valoracion = otrosAspInnovador.valoracion;
                    actaOtrosAspInnovador.observacion = otrosAspInnovador.observacion;
                    actaOtrosAspInnovador.FechaIngreso = DateTime.Now;
                }
                else//Insertar
                {
                    ActaSeguimOtrosAspInnovador gesOtrosApsectos = new ActaSeguimOtrosAspInnovador
                    {
                        
                        codConvocatoria = otrosAspInnovador.codConvocatoria,
                        codProyecto = otrosAspInnovador.codProyecto,
                        numActa = otrosAspInnovador.numActa,
                        visita = otrosAspInnovador.visita,
                        FechaIngreso = DateTime.Now,
                        observacion = otrosAspInnovador.observacion,
                        valoracion = otrosAspInnovador.valoracion
                    };

                    db.ActaSeguimOtrosAspInnovador.InsertOnSubmit(gesOtrosApsectos);
                }

                db.SubmitChanges();

                insertado = true;
            }

            return insertado;
        }

        public bool InsertOrUpdateOtrosAspAmbiental(ActaSeguimOtrosAspAmbientalModel otrosAspAmbiental)
        {
            bool insertado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                var actaOtrosAspAmbiental = (from g in db.ActaSeguimOtrosAspAmbiental
                                             where g.codConvocatoria == otrosAspAmbiental.codConvocatoria
                                             && g.codProyecto == otrosAspAmbiental.codProyecto
                                             && g.numActa == otrosAspAmbiental.numActa
                                             select g).FirstOrDefault();

                if (actaOtrosAspAmbiental != null)//Actualizar
                {
                    actaOtrosAspAmbiental.valoracion = otrosAspAmbiental.valoracion;
                    actaOtrosAspAmbiental.observacion = otrosAspAmbiental.observacion;
                    actaOtrosAspAmbiental.FechaIngreso = DateTime.Now;
                }
                else//Insertar
                {
                    ActaSeguimOtrosAspAmbiental gesOtrosApsectos = new ActaSeguimOtrosAspAmbiental
                    {

                        codConvocatoria = otrosAspAmbiental.codConvocatoria,
                        codProyecto = otrosAspAmbiental.codProyecto,
                        numActa = otrosAspAmbiental.numActa,
                        visita = otrosAspAmbiental.visita,
                        FechaIngreso = DateTime.Now,
                        observacion = otrosAspAmbiental.observacion,
                        valoracion = otrosAspAmbiental.valoracion
                    };

                    db.ActaSeguimOtrosAspAmbiental.InsertOnSubmit(gesOtrosApsectos);
                }

                db.SubmitChanges();

                insertado = true;
            }

            return insertado;
        }
    }
}
