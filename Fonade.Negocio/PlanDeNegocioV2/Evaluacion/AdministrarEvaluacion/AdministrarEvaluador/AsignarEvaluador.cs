using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Datos;
using System.Drawing;

namespace Fonade.Negocio.PlanDeNegocioV2.Evaluacion.AdministrarEvaluacion.AdministrarEvaluador
{
    public class AsignarEvaluador
    {
        public static List<EvaluadorProyecto> GetEvaluadores(int? codOperador)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from evaluadores in db.Evaluadors
                                join contactos in db.Contacto on evaluadores.CodContacto equals contactos.Id_Contacto
                                join grupos in db.GrupoContactos on contactos.Id_Contacto equals grupos.CodContacto
                                where grupos.CodGrupo.Equals(Constantes.CONST_Evaluador)
                                      && contactos.Inactivo.Equals(false) && contactos.codOperador == codOperador
                                select
                                  new EvaluadorProyecto
                                  {
                                      Id = contactos.Id_Contacto,
                                      NombreCompleto = contactos.Nombres + ' ' + contactos.Apellidos
                                  }
                              ).OrderBy(ordering => ordering.NombreCompleto).ToList();


                return entities;
            }
        }

        public static List<ProyectoEvaluacion> GetProyectosPorEvaluador(int codigoEvaluador, int? codOperador)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                int[] estados = new int[] { Constantes.CONST_Evaluacion, Constantes.CONST_Acreditado };
                var entities = (from proyectos in db.Proyecto
                                join subsectores in db.SubSector on proyectos.CodSubSector equals subsectores.Id_SubSector
                                join sectores in db.Sector on subsectores.CodSector equals sectores.Id_Sector
                                where
                                    estados.Contains(proyectos.CodEstado)
                                    && proyectos.Inactivo == false && proyectos.codOperador == codOperador
                                select new ProyectoEvaluacion
                                {
                                    Id = proyectos.Id_Proyecto,
                                    Nombre = proyectos.NomProyecto,
                                    IdSector = sectores.Id_Sector,
                                    Sector = sectores.NomSector,
                                    CurrentEvaluador = codigoEvaluador,
                                    IsValidSector = db.EvaluadorSectors.Any(filter => filter.CodContacto.Equals(codigoEvaluador) && filter.CodSector.Equals(sectores.Id_Sector)),
                                    EvaludorDelProyecto =
                                                        (from contactos in db.Contacto
                                                         join proyectoContacto in db.ProyectoContactos on contactos.Id_Contacto equals proyectoContacto.CodContacto
                                                         where proyectoContacto.FechaFin == null
                                                               && proyectoContacto.Inactivo.Equals(false)
                                                               && proyectoContacto.CodRol.Equals(Constantes.CONST_RolEvaluador)
                                                               && proyectoContacto.CodProyecto.Equals(proyectos.Id_Proyecto)
                                                         select
                                                           new EvaluadorProyecto
                                                           {
                                                               Id = contactos.Id_Contacto,
                                                               NombreCompleto = contactos.Nombres + ' ' + contactos.Apellidos
                                                           }
                                                        ).FirstOrDefault()
                                })
                                .OrderBy(ordering => ordering.Nombre)
                                .ToList();

                return entities;
            }
        }

        public static EvaluadorProyecto GetEvaluadorProyecto(int codigoProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from contactos in db.Contacto
                              join proyectoContacto in db.ProyectoContactos on contactos.Id_Contacto equals proyectoContacto.CodContacto
                              where proyectoContacto.FechaFin == null
                                    && proyectoContacto.Inactivo.Equals(false)
                                    && proyectoContacto.CodRol.Equals(Constantes.CONST_RolEvaluador)
                                    && proyectoContacto.CodProyecto.Equals(codigoProyecto)
                              select
                                new EvaluadorProyecto
                                {
                                    Id = contactos.Id_Contacto,
                                    NombreCompleto = contactos.Nombres + ' ' + contactos.Apellidos,
                                    Email = contactos.Email
                                }
                              ).FirstOrDefault();

                return entity;
            }
        }

        public static bool VerificarSectorEvaluador(int codigoEvaluador, int codigoSector)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = db.EvaluadorSectors.Any(filter => filter.CodContacto.Equals(codigoEvaluador)
                                                               && filter.CodSector.Equals(codigoSector)
                );

                return entity;
            }
        }

        public static void AddSectorEvaluador(int codigoEvaluador, int codigoSector)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = db.EvaluadorSectors.FirstOrDefault(filter => filter.CodContacto.Equals(codigoEvaluador)
                                                                          && filter.CodSector.Equals(codigoSector)
                                                               );
                if (entity == null)
                {

                    entity = new EvaluadorSector
                    {
                        CodContacto = codigoEvaluador,
                        CodSector = codigoSector,
                        Experiencia = "A",
                        fechaActualizacion = DateTime.Now
                    };

                    db.EvaluadorSectors.InsertOnSubmit(entity);
                    db.SubmitChanges();
                }
            }
        }

        public static void AsignarProyectoEvaluador(int codigoEvaluador, int codigoProyecto, bool desasignar = false)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = db.ProyectoContactos.Where(filter =>
                                                                 filter.CodProyecto.Equals(codigoProyecto)
                                                                 && filter.CodRol.Equals(Constantes.CONST_RolEvaluador)
                                                                 && filter.FechaFin == null
                                                               );
                if (!entities.Any())
                {
                    var entity = new ProyectoContacto
                    {
                        CodContacto = codigoEvaluador,
                        CodProyecto = codigoProyecto,
                        CodRol = Constantes.CONST_RolEvaluador,
                        FechaInicio = DateTime.Now,
                        FechaFin = null,
                        Inactivo = false,
                        Beneficiario = false,
                        Participacion = 0,
                        HorasProyecto = null,
                        Acreditador = false,
                        CodConvocatoria = PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(codigoProyecto, 0).GetValueOrDefault(0)
                    };

                    db.ProyectoContactos.InsertOnSubmit(entity);

                    var proyecto = db.Proyecto1s.FirstOrDefault(filter => filter.Id_Proyecto.Equals(codigoProyecto) && filter.CodEstado.Equals(Constantes.CONST_Acreditado));
                    if (proyecto != null)
                        proyecto.CodEstado = Constantes.CONST_Evaluacion;

                    AsignarCoordinador(codigoEvaluador, codigoProyecto);

                    db.SubmitChanges();
                }
                else
                {
                    foreach (var entity in entities)
                    {
                        if (!entity.CodContacto.Equals(codigoEvaluador) || desasignar)
                        {
                            entity.FechaFin = DateTime.Now;
                            entity.Inactivo = true;
                        }
                    }

                    if (!desasignar)
                    {
                        var Newentity = new ProyectoContacto
                        {
                            CodContacto = codigoEvaluador,
                            CodProyecto = codigoProyecto,
                            CodRol = Constantes.CONST_RolEvaluador,
                            FechaInicio = DateTime.Now,
                            FechaFin = null,
                            Inactivo = false,
                            Beneficiario = false,
                            Participacion = 0,
                            HorasProyecto = null,
                            Acreditador = false,
                            CodConvocatoria = PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(codigoProyecto, 0).GetValueOrDefault(0)
                        };

                        db.ProyectoContactos.InsertOnSubmit(Newentity);

                        var proyecto = db.Proyecto1s.FirstOrDefault(filter => filter.Id_Proyecto.Equals(codigoProyecto) && filter.CodEstado.Equals(Constantes.CONST_Acreditado));
                        if (proyecto != null)
                            proyecto.CodEstado = Constantes.CONST_Evaluacion;

                        AsignarCoordinador(codigoEvaluador, codigoProyecto);
                    }
                    else
                    {
                        var entitiesCoordinadores = db.ProyectoContactos.Where(filter =>
                                                                filter.CodProyecto.Equals(codigoProyecto)
                                                                && filter.CodRol.Equals(Constantes.CONST_RolCoordinadorEvaluador)
                                                                && filter.FechaFin == null
                                                              );

                        foreach (var entity in entitiesCoordinadores)
                        {
                            entity.FechaFin = DateTime.Now;
                            entity.Inactivo = true;
                        }

                    }

                    db.SubmitChanges();
                }
            }
        }

        public static void AsignarCoordinador(int codigoEvaluador, int codigoProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var coordinador = db.Evaluadors.SingleOrDefault(filter => filter.CodContacto.Equals(codigoEvaluador) && filter.CodCoordinador != null);

                if (coordinador != null)
                {
                    // Se agrega Desctivar Coordinadores de Evaluadores, que No estan asociado al evaluador
                    var entities = db.ProyectoContactos.Where(filter =>
                                                                 filter.CodProyecto.Equals(codigoProyecto)
                                                                 && filter.CodRol.Equals(Constantes.CONST_RolCoordinadorEvaluador)
                                                                 && filter.FechaFin == null
                                                               );

                    foreach (var entity in entities)
                    {
                        if (!entity.CodContacto.Equals(coordinador.CodCoordinador.GetValueOrDefault(0)))
                        {
                            entity.FechaFin = DateTime.Now;
                            entity.Inactivo = true;
                        }
                    }



                    if (!db.ProyectoContactos.Any(filter => filter.CodContacto.Equals(coordinador.CodCoordinador)
                                                           && filter.CodProyecto.Equals(codigoProyecto)
                                                           && filter.CodConvocatoria.Equals(PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(codigoProyecto, 0).GetValueOrDefault(0))
                                                           && filter.FechaFin == null
                                                           && filter.Inactivo == false))
                    {
                        var NewentityCoordinador = new ProyectoContacto
                        {
                            CodContacto = coordinador.CodCoordinador.GetValueOrDefault(0),
                            CodProyecto = codigoProyecto,
                            CodRol = Constantes.CONST_RolCoordinadorEvaluador,
                            FechaInicio = DateTime.Now,
                            FechaFin = null,
                            Inactivo = false,
                            Beneficiario = false,
                            Participacion = 0,
                            HorasProyecto = null,
                            Acreditador = false,
                            CodConvocatoria = PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(codigoProyecto, 0).GetValueOrDefault(0)
                        };

                        db.ProyectoContactos.InsertOnSubmit(NewentityCoordinador);
                      
                    }

                    db.SubmitChanges();
                }
                else
                {
                    throw new ApplicationException("Este evaluador no tiene coordinador de evaluador asignado, por favor asignelo primero para poder continuar.");
                }
            }
        }

    }

    public class EvaluadorProyecto
    {

        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
    }

    public class ProyectoEvaluacion
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int? CodigoEvaluador
        {
            get
            {
                return EvaludorDelProyecto != null ? EvaludorDelProyecto.Id : (int?)null;
            }
            set { }
        }
        public string Evaluador
        {
            get
            {
                return EvaludorDelProyecto != null ? EvaludorDelProyecto.NombreCompleto : "Sin Evaluador";
            }
            set { }
        }
        public int IdSector { get; set; }
        public string Sector { get; set; }
        public bool IsOwner
        {
            get
            {
                if (CodigoEvaluador == null)
                    return false;
                return CodigoEvaluador.Equals(CurrentEvaluador);
            }
            set { }
        }
        public int CurrentEvaluador { get; set; }
        public bool IsValidSector { get; set; }
        public EvaluadorProyecto EvaludorDelProyecto { get; set; }
        public Color currentColor
        {
            get
            {
                if (CodigoEvaluador == null)
                    return Color.Red;
                else
                    return Color.Black;
            }
            set { }
        }
    }
}