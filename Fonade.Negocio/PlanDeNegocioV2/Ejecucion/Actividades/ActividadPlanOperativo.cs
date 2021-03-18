using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Datos;

namespace Fonade.Negocio.PlanDeNegocioV2.Ejecucion.Actividades
{
    public class ProyectoActividadPOInterventorTMPModel {

        public string Tarea { get;set; }
        public bool? ChequeoCoordinador { get; set; }
        public bool? ChequeoGerente { get; set; }
        public int CodProyecto { get; set; }
        public int Id { get; set; }
        public int Id_Actividad { get; set; }
        public short? Item { get; set; }
        public string Metas { get; set; }
        public string NomActividad { get; set; }

    }

    public static class ActividadPlanOperativo
    {
        public static List<ProyectoActividadPOInterventor> Get(int codigoProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                return db.ProyectoActividadPOInterventors.Where(filter=> filter.CodProyecto.Equals(codigoProyecto)).ToList();
            }
        }
        public static List<ProyectoActividadPOMesInterventor> GetMeses(int codigoActividad)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                return db.ProyectoActividadPOMesInterventors.Where(filter => filter.CodActividad.Equals(codigoActividad)).ToList();
            }
        }
        public static List<ProyectoActividadPOInterventorTMPModel> GetTemporal(int? _codOperador)
        {
            List<ProyectoActividadPOInterventorTMPModel> actividades = new List<ProyectoActividadPOInterventorTMPModel>();

            string _conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(_conexion))
            {
                actividades = (from a in db.ProyectoActividadPOInterventorTMPs
                                   join p in db.Proyecto on a.CodProyecto equals p.Id_Proyecto
                                   where a.ChequeoCoordinador == true && p.codOperador == _codOperador
                                   select new ProyectoActividadPOInterventorTMPModel
                                   {
                                       Tarea = a.Tarea,
                                       ChequeoCoordinador = a.ChequeoCoordinador,
                                       ChequeoGerente = a.ChequeoGerente,
                                       CodProyecto = a.CodProyecto,
                                       Id = a.Id,
                                       Id_Actividad = a.Id_Actividad,
                                       Item = a.Item,
                                       Metas = a.Metas,
                                       NomActividad = a.NomActividad
                                   }).ToList();

                //return db.ProyectoActividadPOInterventorTMPs.Where(filter => filter.ChequeoCoordinador == true).OrderByDescending(ordering => ordering.Id).ToList();
            }

            return actividades;
        }

        public static int CountTemporal(int? _codOperador)
        {
            int cantidad = 0;
            string _conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(_conexion))
            {
                var proyectos = (from a in db.ProyectoActividadPOInterventorTMPs
                                 join p in db.Proyecto on a.CodProyecto equals p.Id_Proyecto
                                 where a.ChequeoCoordinador == true && p.codOperador == _codOperador
                                 select new
                                 {
                                     a.CodProyecto
                                 }).ToList();

                cantidad = proyectos.Count();

                 //return db.ProyectoActividadPOInterventorTMPs.Count(filter => filter.ChequeoCoordinador == true);
            }

            return cantidad;
        }

        public static List<ProyectoActividadPOMesInterventorTMP> GetMesesTemporal(int codigoActividad) {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                return db.ProyectoActividadPOMesInterventorTMPs.Where(filter => filter.CodActividad.Equals(codigoActividad)).ToList();
            }
        }
        public static Int32 Add(string nombre, int codigoProyecto,int item, string metas){
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var newEntity = new ProyectoActividadPOInterventor()
                {                    
                    CodProyecto = codigoProyecto,
                    Item = (Int16)item,
                    NomActividad = nombre,
                    Metas = metas
                };

                db.ProyectoActividadPOInterventors.InsertOnSubmit(newEntity);
                db.SubmitChanges();

                return newEntity.Id_Actividad;
            }
        }
        public static void AddMeses(int codigoActividad, int mes, int tipoFinanciacion, Decimal valor){
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = db.ProyectoActividadPOMesInterventors.SingleOrDefault(filter => filter.CodActividad.Equals(codigoActividad)
                                                                                            && filter.Mes.Equals(mes)
                                                                                            && filter.CodTipoFinanciacion.Equals(tipoFinanciacion)
                                                                                  );
                if (entity == null) {
                    var newEntity = new ProyectoActividadPOMesInterventor()
                    {
                        CodActividad = codigoActividad,
                        CodTipoFinanciacion = (byte)tipoFinanciacion,
                        Mes = (byte)mes,
                        Valor = valor
                    };

                    db.ProyectoActividadPOMesInterventors.InsertOnSubmit(newEntity);
                    db.SubmitChanges();
                }
                else
                {
                    UpdateMes(codigoActividad, mes, tipoFinanciacion, valor);
                }                
            }
        }
        public static void Update(int codigoActividad,string nombre,int item, string metas) {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = db.ProyectoActividadPOInterventors.SingleOrDefault(filter => filter.Id_Actividad.Equals(codigoActividad));

                if (entity != null) {
                    entity.NomActividad = nombre;
                    entity.Item = (Int16)item;
                    entity.Metas = metas;
                    db.SubmitChanges();
                }
                else
                {
                    throw new Exception("No se puede modificar la actividad porque no existe.");
                }
            }
        }
        public static void UpdateMes(int codigoActividad,int mes, int tipoFinanciacion, decimal valor)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = db.ProyectoActividadPOMesInterventors.SingleOrDefault(filter => filter.CodActividad.Equals(codigoActividad)
                                                                                             && filter.Mes.Equals(mes)
                                                                                             && filter.CodTipoFinanciacion.Equals(tipoFinanciacion)
                                                                                   );
                if (entity != null)
                {
                    entity.Valor = valor;
                    db.SubmitChanges();
                }
                else
                {
                    AddMeses(codigoActividad, mes, tipoFinanciacion, valor);
                }
            }
        }
        public static bool Exist(int codigoActividad) {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                return db.ProyectoActividadPOInterventors.Any(filter => filter.Id_Actividad.Equals(codigoActividad));
            }
        }
        public static bool ExistByName(int codigoProyecto, string nombre, int item)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                return db.ProyectoActividadPOInterventors.Any(filter => 
                                                                        filter.CodProyecto.Equals(codigoProyecto)
                                                                        && filter.Item.Equals(item)
                                                                        && filter.NomActividad.Equals(nombre) 
                );
            }
        }
        public static void DeleteTemporal(int codigoActividad)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = db.ProyectoActividadPOInterventorTMPs.Where(filter => filter.Id_Actividad.Equals(codigoActividad)).ToList();
                db.ProyectoActividadPOInterventorTMPs.DeleteAllOnSubmit(entities);
                db.SubmitChanges();
            }
        }
        public static void DeleteTemporalById(int id)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = db.ProyectoActividadPOInterventorTMPs.Single(filter => filter.Id.Equals(id));
                db.ProyectoActividadPOInterventorTMPs.DeleteOnSubmit(entity);
                db.SubmitChanges();
            }
        }
        public static void DeleteMesesTemporal(int codigoActividad)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = db.ProyectoActividadPOMesInterventorTMPs.Where(filter => filter.CodActividad.Equals(codigoActividad)).ToList();

                db.ProyectoActividadPOMesInterventorTMPs.DeleteAllOnSubmit(entities);
                db.SubmitChanges();
            }
        }
        public static void Delete(int codigoActividad)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = db.ProyectoActividadPOInterventors.Single(filter => filter.Id_Actividad.Equals(codigoActividad));
                db.ProyectoActividadPOInterventors.DeleteOnSubmit(entities);
                db.SubmitChanges();
            }
        }
        public static void DeleteMeses(int codigoActividad)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = db.ProyectoActividadPOMesInterventors.Where(filter => filter.CodActividad.Equals(codigoActividad)).ToList();

                db.ProyectoActividadPOMesInterventors.DeleteAllOnSubmit(entities);
                db.SubmitChanges();
            }
        }
        public static int? GetInterventorId(int codigoProyecto) {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from empresas in db.Empresas
                                join interventores in db.EmpresaInterventors on empresas.id_empresa equals interventores.CodEmpresa
                                where empresas.codproyecto.Equals(codigoProyecto)
                                      && interventores.Inactivo.Equals(false)
                                      && interventores.FechaFin == null
                                orderby interventores.Rol descending
                                select
                                    interventores.CodContacto
                                ).FirstOrDefault();

                return entity != null ? entity : null;
            }
        }
    }
}
