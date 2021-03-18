using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.PlanDeNegocioV2.Administracion.ConcejoDirectivo
{
    public static class ConcejoDirectivo
    {
        public static List<ActaConcejo> Get(int startIndex, int maxRows, int? _codOperador)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from actas in db.ConcejoDirectivoActas
                                join convocatorias in db.Convocatoria
                                on actas.CodConvocatoria equals convocatorias.Id_Convocatoria
                                where convocatorias.codOperador == _codOperador
                                select new ActaConcejo {
                                    Id = actas.Id_acta,
                                    Numero = actas.Numero,
                                    Nombre = actas.Nombre,
                                    Fecha = actas.Fecha,
                                    Observacion = actas.Observaciones,
                                    CodigoConvocatoria = actas.CodConvocatoria,
                                    Convocatoria = convocatorias.NomConvocatoria,
                                    Publicado = actas.Publicado.GetValueOrDefault(false)
                                }
                                ).Skip(startIndex).Take(maxRows).ToList();

                return entities;
            }
        }

        public static ActaConcejo GetById(int codigoActa)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from actas in db.ConcejoDirectivoActas
                                join convocatorias in db.Convocatoria on actas.CodConvocatoria equals convocatorias.Id_Convocatoria
                                where actas.Id_acta.Equals(codigoActa)
                                select new ActaConcejo
                                {
                                    Id = actas.Id_acta,
                                    Numero = actas.Numero,
                                    Nombre = actas.Nombre,
                                    Fecha = actas.Fecha,
                                    Observacion = actas.Observaciones,
                                    CodigoConvocatoria = actas.CodConvocatoria,
                                    Convocatoria = convocatorias.NomConvocatoria,
                                    Publicado = actas.Publicado.GetValueOrDefault(false)
                                }
                                ).FirstOrDefault();

                return entity;
            }
        }

        public static bool Exist(string nombre)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                return db.ConcejoDirectivoActas.Any(filter => filter.Numero.Equals(nombre));
            }
        }

        public static List<ActaConcejoProyecto> GetProyectosByActa(int codigoActa)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from actas in db.ConcejoDirectivoActaProyectos
                                join proyectos in db.Proyecto on actas.CodProyecto equals proyectos.Id_Proyecto
                                where actas.CodActa.Equals(codigoActa)
                                select new ActaConcejoProyecto
                                {
                                    IdActa = actas.CodActa,
                                    NombreProyecto = proyectos.NomProyecto,
                                    CodigoProyecto = proyectos.Id_Proyecto
                                }
                               ).ToList();

                return entities;
            }
        }

        public static List<Datos.Proyecto> GetProyectosConcejoDirectivo(int codigoConvocatoria)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {               
                var entities = (from actas in db.ConcejoDirectivoActas
                                join proyectos in db.ConcejoDirectivoActaProyectos on actas.Id_acta equals proyectos.CodActa
                                where actas.CodConvocatoria.Equals(codigoConvocatoria)                                
                                select proyectos.CodProyecto                                
                               ).Distinct().ToList();

                var proyectosNoAcreditados = (from actas in db.AcreditacionActa
                                join proyectos in db.AcreditacionActaProyecto on actas.Id_Acta equals proyectos.CodActa
                                where actas.CodConvocatoria.Equals(codigoConvocatoria)
                                select proyectos.CodProyecto
               ).Distinct().ToList();

                var proyectosConcejoDirecto = db.Proyecto.Where(filter => filter.CodEstado.Equals(Constantes.CONST_concejo_directivo)
																		   && !entities.Contains(filter.Id_Proyecto)
																		   && proyectosNoAcreditados.Contains(filter.Id_Proyecto)
                                                                           ).ToList();
				


				return proyectosConcejoDirecto;
            }
        }

        public static int Count()
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                return db.ConcejoDirectivoActas.Count();
            }
        }

        public static void Insert(Datos.ConcejoDirectivoActa entity)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {              
                db.ConcejoDirectivoActas.InsertOnSubmit(entity);
                db.SubmitChanges();
            }
        }

        public static void Delete(int codigoActa) {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = db.ConcejoDirectivoActas.Single(filter => filter.Id_acta.Equals(codigoActa));

                db.ConcejoDirectivoActas.DeleteOnSubmit(entity);
                db.SubmitChanges();
            }
        }

        public static void DeleteProyectoByActa(int codigoActa, int codigoProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = db.ConcejoDirectivoActaProyectos.Single(filter => filter.CodActa.Equals(codigoActa) && filter.CodProyecto.Equals(codigoProyecto));

                db.ConcejoDirectivoActaProyectos.DeleteOnSubmit(entity);
                db.SubmitChanges();
            }
        }

        public static void InsertProyectoActa(Datos.ConcejoDirectivoActaProyecto entity)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                db.ConcejoDirectivoActaProyectos.InsertOnSubmit(entity);
                db.SubmitChanges();
            }
        }

        public static void Update(Datos.ConcejoDirectivoActa acta)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = db.ConcejoDirectivoActas.Single(filter => filter.Id_acta.Equals(acta.Id_acta));

                entity.Publicado = acta.Publicado;
                entity.Observaciones = acta.Observaciones;
                
                db.SubmitChanges();
            }
        }
    }

    public class ActaConcejo {
        public int Id { get; set; }
        public string Numero { get; set; }
        public string Nombre{ get; set; }
        public DateTime Fecha { get; set; }
        public string Observacion { get; set; }
        public int CodigoConvocatoria { get; set; }
        public string Convocatoria { get; set; }
        public bool Publicado { get; set; }
    }

    public class ActaConcejoProyecto
    {
        public int IdActa { get; set; }
        public int CodigoProyecto { get; set; }
        public string NombreProyecto { get; set; }        
    }
}
