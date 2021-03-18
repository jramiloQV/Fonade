using Datos;
using Datos.Modelos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.FonDBLight
{
    public class ActaSeguimOtrasObligacionesController
    {
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public ActaSeguimOtrasObligacionesModel getDescripcionOtrasObligaciones(int _codProyecto, int _codConvocatoria)
        {
            ActaSeguimOtrasObligacionesModel descripciones = new ActaSeguimOtrasObligacionesModel();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                descripciones = (from e in db.ActaSeguimOtrasObligaciones
                                 where e.codProyecto == _codProyecto && e.codConvocatoria == _codConvocatoria
                                 orderby e.numActa
                                 select new ActaSeguimOtrasObligacionesModel
                                 {
                                     id = e.idOtrasObligaciones,
                                     codProyecto = e.codProyecto,
                                     codConvocatoria = e.codConvocatoria,
                                     numActa = e.numActa,
                                     visita = e.visita,
                                     DescripAcomAsesoria = e.DescripAcomAsesoria,
                                     DescripInfoPlataforma = e.DescripInfoPlataforma,
                                     DescripTiempoEmprendedor = e.DescripTiempoEmprendedor
                                 }).FirstOrDefault();
            }

            return descripciones;
        }

        public string GetPerfilEmprendedor(int _codProyecto, int _codRol)
        {
            string perfil = "";

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                perfil = (from pc in db.ProyectoContactos
                          join pe in db.ProyectoEmprendedorPerfils
                          on pc.CodContacto equals pe.IdContacto
                        where pc.CodProyecto == _codProyecto 
                        && pc.CodRol == _codRol                        
                        select pe.Perfil).FirstOrDefault();
            }

            return perfil;
        }

        public List<ActaSeguimOtrasObligInfoPlataformaModel> GetOtrasObligInfoPlataforma(int _codProyecto, int _codConvocatoria)
        {
            List<ActaSeguimOtrasObligInfoPlataformaModel> listOtrasOblig = new List<ActaSeguimOtrasObligInfoPlataformaModel>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                listOtrasOblig = (from e in db.ActaSeguimOtrasObligInfoPlataforma
                                       where e.codProyecto == _codProyecto && e.codConvocatoria == _codConvocatoria
                                       orderby e.numActa
                                       select new ActaSeguimOtrasObligInfoPlataformaModel
                                       {                                           
                                           codConvocatoria = e.codConvocatoria,
                                           codProyecto = e.codProyecto,                                           
                                           id = e.idOtrasObligInfoPlataforma,
                                           numActa = e.numActa,
                                           visita = e.visita,
                                           observacion = e.observacion,
                                           valoracion = e.valoracion
                                       }).ToList();
            }

            return listOtrasOblig;
        }

        public List<ActaSeguimOtrasObligTiempoEmpModel> GetOtrasObligDedicaEmprendedor(int _codProyecto, int _codConvocatoria)
        {
            List<ActaSeguimOtrasObligTiempoEmpModel> listOtrasOblig = new List<ActaSeguimOtrasObligTiempoEmpModel>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                listOtrasOblig = (from e in db.ActaSeguimOtrasObligTiempoEmp
                                  where e.codProyecto == _codProyecto && e.codConvocatoria == _codConvocatoria
                                  orderby e.numActa
                                  select new ActaSeguimOtrasObligTiempoEmpModel
                                  {
                                      codConvocatoria = e.codConvocatoria,
                                      codProyecto = e.codProyecto,
                                      id = e.idOtrasObligTiempoEmprendedor,
                                      numActa = e.numActa,
                                      visita = e.visita,
                                      observacion = e.observacion,
                                      valoracion = e.valoracion
                                  }).ToList();
            }

            return listOtrasOblig;
        }

        public List<ActaSeguimOtrasObligAcomAsesoriaModel> GetOtrasObligAcomAsesoria(int _codProyecto, int _codConvocatoria)
        {
            List<ActaSeguimOtrasObligAcomAsesoriaModel> listOtrasOblig = new List<ActaSeguimOtrasObligAcomAsesoriaModel>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                listOtrasOblig = (from e in db.ActaSeguimOtrasObligAcomAsesoria
                                  where e.codProyecto == _codProyecto && e.codConvocatoria == _codConvocatoria
                                  orderby e.numActa
                                  select new ActaSeguimOtrasObligAcomAsesoriaModel
                                  {
                                      codConvocatoria = e.codConvocatoria,
                                      codProyecto = e.codProyecto,
                                      id = e.idOtrasObligAcomAsesoria,
                                      numActa = e.numActa,
                                      visita = e.visita,
                                      observacion = e.observacion,
                                      valoracion = e.valoracion
                                  }).ToList();
            }

            return listOtrasOblig;
        }
        public bool InsertOrUpdateOtrasObligaciones(ActaSeguimOtrasObligacionesModel obligacion)
        {
            bool insertado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {

                var actaObligacion = (from g in db.ActaSeguimOtrasObligaciones
                                      where g.codConvocatoria == obligacion.codConvocatoria
                                      && g.codProyecto == obligacion.codProyecto
                                      && g.numActa == obligacion.numActa
                                      select g).FirstOrDefault();

                if (actaObligacion != null)//Actualizar
                {
                    actaObligacion.DescripAcomAsesoria = obligacion.DescripAcomAsesoria;
                    actaObligacion.DescripInfoPlataforma = obligacion.DescripInfoPlataforma;
                    actaObligacion.DescripTiempoEmprendedor = obligacion.DescripTiempoEmprendedor;
                    actaObligacion.FechaIngresado = DateTime.Now;                    
                }
                else//Insertar
                {
                    ActaSeguimOtrasObligaciones gesObligacion = new ActaSeguimOtrasObligaciones
                    {
                        codConvocatoria = obligacion.codConvocatoria,                        
                        numActa = obligacion.numActa,                       
                        visita = obligacion.visita,
                        FechaIngresado = DateTime.Now,                        
                        codProyecto = obligacion.codProyecto,
                        DescripAcomAsesoria = obligacion.DescripAcomAsesoria,
                        DescripInfoPlataforma = obligacion.DescripInfoPlataforma,
                        DescripTiempoEmprendedor = obligacion.DescripTiempoEmprendedor
                    };

                    db.ActaSeguimOtrasObligaciones.InsertOnSubmit(gesObligacion);
                }

                db.SubmitChanges();

                insertado = true;

                return insertado;
            }
        }

        public bool InsertOrUpdateOtrasObliInfoPlataforma(ActaSeguimOtrasObligInfoPlataformaModel obligacion)
        {
            bool insertado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {

                var actaObligacion = (from g in db.ActaSeguimOtrasObligInfoPlataforma
                                      where g.codConvocatoria == obligacion.codConvocatoria
                                      && g.codProyecto == obligacion.codProyecto
                                      && g.numActa == obligacion.numActa
                                      select g).FirstOrDefault();

                if (actaObligacion != null)//Actualizar
                {
                    actaObligacion.observacion = obligacion.observacion;
                    actaObligacion.valoracion = obligacion.valoracion;                    
                    actaObligacion.FechaIngreso = DateTime.Now;
                }
                else//Insertar
                {
                    ActaSeguimOtrasObligInfoPlataforma gesObligacion = new ActaSeguimOtrasObligInfoPlataforma
                    {
                        codConvocatoria = obligacion.codConvocatoria,
                        numActa = obligacion.numActa,
                        visita = obligacion.visita,
                        FechaIngreso = DateTime.Now,
                        codProyecto = obligacion.codProyecto,
                        observacion = obligacion.observacion,
                        valoracion = obligacion.valoracion
                    };

                    db.ActaSeguimOtrasObligInfoPlataforma.InsertOnSubmit(gesObligacion);
                }

                db.SubmitChanges();

                insertado = true;

                return insertado;
            }
        }

        public bool InsertOrUpdateOtrasObliDedicaEmp(ActaSeguimOtrasObligTiempoEmpModel obligacion)
        {
            bool insertado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {

                var actaObligacion = (from g in db.ActaSeguimOtrasObligTiempoEmp
                                      where g.codConvocatoria == obligacion.codConvocatoria
                                      && g.codProyecto == obligacion.codProyecto
                                      && g.numActa == obligacion.numActa
                                      select g).FirstOrDefault();

                if (actaObligacion != null)//Actualizar
                {
                    actaObligacion.observacion = obligacion.observacion;
                    actaObligacion.valoracion = obligacion.valoracion;
                    actaObligacion.FechaIngreso = DateTime.Now;
                }
                else//Insertar
                {
                    ActaSeguimOtrasObligTiempoEmp gesObligacion = new ActaSeguimOtrasObligTiempoEmp
                    {
                        codConvocatoria = obligacion.codConvocatoria,
                        numActa = obligacion.numActa,
                        visita = obligacion.visita,
                        FechaIngreso = DateTime.Now,
                        codProyecto = obligacion.codProyecto,
                        observacion = obligacion.observacion,
                        valoracion = obligacion.valoracion
                    };

                    db.ActaSeguimOtrasObligTiempoEmp.InsertOnSubmit(gesObligacion);
                }

                db.SubmitChanges();

                insertado = true;

                return insertado;
            }
        }

        public bool InsertOrUpdateOtrasObliAcomAsesoria(ActaSeguimOtrasObligAcomAsesoriaModel obligacion)
        {
            bool insertado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {

                var actaObligacion = (from g in db.ActaSeguimOtrasObligAcomAsesoria
                                      where g.codConvocatoria == obligacion.codConvocatoria
                                      && g.codProyecto == obligacion.codProyecto
                                      && g.numActa == obligacion.numActa
                                      select g).FirstOrDefault();

                if (actaObligacion != null)//Actualizar
                {
                    actaObligacion.observacion = obligacion.observacion;
                    actaObligacion.valoracion = obligacion.valoracion;
                    actaObligacion.FechaIngreso = DateTime.Now;
                }
                else//Insertar
                {
                    ActaSeguimOtrasObligAcomAsesoria gesObligacion = new ActaSeguimOtrasObligAcomAsesoria
                    {
                        codConvocatoria = obligacion.codConvocatoria,
                        numActa = obligacion.numActa,
                        visita = obligacion.visita,
                        FechaIngreso = DateTime.Now,
                        codProyecto = obligacion.codProyecto,
                        observacion = obligacion.observacion,
                        valoracion = obligacion.valoracion
                    };

                    db.ActaSeguimOtrasObligAcomAsesoria.InsertOnSubmit(gesObligacion);
                }

                db.SubmitChanges();

                insertado = true;

                return insertado;
            }
        }
    }
}
