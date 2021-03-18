using Datos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.PlanDeNegocioV2.Evaluacion.AdministrarEvaluacion.AdministrarCoordinador
{
    public class AsignarCoordinador
    {
        public static List<CoordinadorEvaluacion> GetCoordinadores(int? codOperador)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from contactos in db.Contacto
                                join grupos in db.GrupoContactos on contactos.Id_Contacto equals grupos.CodContacto
                                where grupos.CodGrupo.Equals(Constantes.CONST_CoordinadorEvaluador)
                                      && contactos.Inactivo.Equals(false) && contactos.codOperador == codOperador
                                select
                                  new CoordinadorEvaluacion
                                  {
                                      Id = contactos.Id_Contacto,
                                      NombreCompleto = contactos.Nombres + ' ' + contactos.Apellidos
                                  }
                              ).OrderBy(ordering => ordering.NombreCompleto).ToList();

                return entities;
            }
        }

        public static List<EvaluadorEvaluacion> GetEvaluadores(int codigoCoordinador, int?codOperador)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from contactos in db.Contacto
                                join grupos in db.GrupoContactos on contactos.Id_Contacto equals grupos.CodContacto                               
                                where grupos.CodGrupo.Equals(Constantes.CONST_Evaluador)
                                      && contactos.Inactivo.Equals(false) && contactos.codOperador == codOperador
                                select
                                  new EvaluadorEvaluacion
                                  {
                                      IdEvaluador = contactos.Id_Contacto,
                                      NombreCompleto = contactos.Nombres + " " + contactos.Apellidos,                                      
                                      CurrentCoordinador = codigoCoordinador
                                  }
                               ).OrderBy(ordering => ordering.NombreCompleto).ToList();

                return entities;
            }
        }

        public static CoordinadorEvaluacion GetCoordinadorByEvaluador(int codigoEvaluador)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from evaluador in db.Evaluadors
                              join contactos in db.Contacto on evaluador.CodCoordinador equals contactos.Id_Contacto
                              join grupoContacto in db.GrupoContactos on contactos.Id_Contacto equals grupoContacto.CodContacto
                              where grupoContacto.CodGrupo.Equals(Constantes.CONST_CoordinadorEvaluador)
                                    && evaluador.CodContacto.Equals(codigoEvaluador)
                              select
                                new CoordinadorEvaluacion
                                {
                                    Id = contactos.Id_Contacto,
                                    NombreCompleto = contactos.Nombres + ' ' + contactos.Apellidos,
                                    Email = contactos.Email
                                }
                              ).FirstOrDefault();

                return entity;
            }
        }

        public static void AsignarCoordinadorAEvaluador(int codigoEvaluador, int codigoCoordinador, bool desasignar = false)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var coordinadorActual = db.Evaluadors.SingleOrDefault(filter => filter.CodContacto.Equals(codigoEvaluador));

                if (coordinadorActual.CodCoordinador != null)
                {
                    var proyectosParaAsignar = new List<ProyectoContacto>();

                    if (coordinadorActual.CodCoordinador.Equals(codigoCoordinador) && desasignar)
                    {
                        proyectosParaAsignar = (from proyectos in db.ProyectoContactos
                                                where
                                                    proyectos.CodContacto.Equals(codigoEvaluador)
                                                    && proyectos.FechaFin == null
                                                    && proyectos.Inactivo.Equals(false)
                                                    && proyectos.CodRol.Equals(Constantes.CONST_RolEvaluador)
                                                select proyectos
                                                ).ToList();

                    }
                    else
                    {
                        proyectosParaAsignar = (from proyectos in db.ProyectoContactos
                                                where
                                                    proyectos.CodContacto.Equals(codigoEvaluador)
                                                    && proyectos.FechaFin == null
                                                    && proyectos.Inactivo.Equals(false)
                                                    && proyectos.CodRol.Equals(Constantes.CONST_RolEvaluador)
                                                select proyectos
                                                ).ToList();
                    }

                    foreach (var proyecto in proyectosParaAsignar)
                    {
                        if (coordinadorActual.CodCoordinador.Equals(codigoCoordinador) && desasignar)
                        {
                            var proyectoPorCoordinadorActual = (from proyectos in db.ProyectoContactos
                                                                where
                                                                    proyectos.CodProyecto.Equals(proyecto.CodProyecto)
                                                                    && proyectos.FechaFin == null
                                                                    && proyectos.Inactivo.Equals(false)
                                                                    && proyectos.CodRol.Equals(Constantes.CONST_RolCoordinadorEvaluador)
                                                                select proyectos
                                               ).FirstOrDefault();
                            if (proyectoPorCoordinadorActual != null)
                            {
                                proyectoPorCoordinadorActual.FechaFin = DateTime.Now;
                                proyectoPorCoordinadorActual.Inactivo = true;
                            }
                            db.SubmitChanges();
                        }
                        else
                        {
                            var proyectoPorCoordinadorActual = (from proyectos in db.ProyectoContactos
                                                                where
                                                                    proyectos.CodProyecto.Equals(proyecto.CodProyecto)
                                                                    && proyectos.FechaFin == null
                                                                    && proyectos.Inactivo.Equals(false)
                                                                    && proyectos.CodRol.Equals(Constantes.CONST_RolCoordinadorEvaluador)
                                                                select proyectos
                                                ).FirstOrDefault();
                            if (proyectoPorCoordinadorActual != null)
                            {
                                proyectoPorCoordinadorActual.FechaFin = DateTime.Now;
                                proyectoPorCoordinadorActual.Inactivo = true;
                            }

                            var NewentityCoordinador = new ProyectoContacto
                            {
                                CodContacto = codigoCoordinador,
                                CodProyecto = proyecto.CodProyecto,
                                CodRol = Constantes.CONST_RolCoordinadorEvaluador,
                                FechaInicio = DateTime.Now,
                                FechaFin = null,
                                Inactivo = false,
                                Beneficiario = false,
                                Participacion = 0,
                                HorasProyecto = null,
                                Acreditador = false,
                                CodConvocatoria = PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(proyecto.CodProyecto,0).GetValueOrDefault(0)
                            };

                            db.ProyectoContactos.InsertOnSubmit(NewentityCoordinador);
                            db.SubmitChanges();
                        }
                    }

                    if (coordinadorActual.CodCoordinador.Equals(codigoCoordinador) && desasignar)
                    {
                        coordinadorActual.CodCoordinador = null;
                        db.SubmitChanges();
                    }
                    else {
                        coordinadorActual.CodCoordinador = codigoCoordinador;
                        db.SubmitChanges();
                    }
                }
                else
                {
                    var proyectosParaAsignar = (from proyectos in db.ProyectoContactos
                                                where
                                                    proyectos.CodContacto.Equals(codigoEvaluador)
                                                    && proyectos.FechaFin == null
                                                    && proyectos.Inactivo.Equals(false)
                                                    && proyectos.CodRol.Equals(Constantes.CONST_RolEvaluador)
                                                select proyectos
                                                ).ToList();

                    foreach (var proyecto in proyectosParaAsignar)
                    {

                        var proyectoPorCoordinadorActual = (from proyectos in db.ProyectoContactos
                                                    where
                                                        proyectos.CodProyecto.Equals(proyecto.CodProyecto)
                                                        && proyectos.FechaFin == null
                                                        && proyectos.Inactivo.Equals(false)
                                                        && proyectos.CodRol.Equals(Constantes.CONST_RolCoordinadorEvaluador)
                                                    select proyectos
                                                ).FirstOrDefault();
                        if (proyectoPorCoordinadorActual != null) {
                            proyectoPorCoordinadorActual.FechaFin = DateTime.Now;
                            proyectoPorCoordinadorActual.Inactivo = true;
                        }

                        var NewentityCoordinador = new ProyectoContacto
                        {
                            CodContacto = codigoCoordinador,
                            CodProyecto = proyecto.CodProyecto,
                            CodRol = Constantes.CONST_RolCoordinadorEvaluador,
                            FechaInicio = DateTime.Now,
                            FechaFin = null,
                            Inactivo = false,
                            Beneficiario = false,
                            Participacion = 0,
                            HorasProyecto = null,
                            Acreditador = false,
                            CodConvocatoria = PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(proyecto.CodProyecto,0).GetValueOrDefault(0)
                        };

                        db.ProyectoContactos.InsertOnSubmit(NewentityCoordinador);
                        coordinadorActual.CodCoordinador = codigoCoordinador;

                        db.SubmitChanges();
                    }

                    coordinadorActual.CodCoordinador = codigoCoordinador;
                    db.SubmitChanges();
                }
            }
        }        
    }

    public class CoordinadorEvaluacion
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
    }

    public class EvaluadorEvaluacion
    {
        public int IdEvaluador { get; set; }
        public string NombreCompleto { get; set; }
        public int? CodigoCoordinador
        {
            get
            {
                return Coordinador != null ? Coordinador.Id : (int?)null;
            }
            set { }
        }
        public string NombreCoordinador
        {
            get
            {
                return Coordinador != null ? Coordinador.NombreCompleto : "Sin Coordinador";
            }
            set { }
        }
        public bool IsOwner
        {
            get
            {
                if (CodigoCoordinador == null)
                    return false;
                return CodigoCoordinador.Equals(CurrentCoordinador);
            }
            set { }
        }
        public int CurrentCoordinador { get; set; }
        
        public CoordinadorEvaluacion Coordinador
        {
            get
            {
                return AsignarCoordinador.GetCoordinadorByEvaluador(IdEvaluador);
            }
            set { }
        }

        public Color currentColor
        {
            get
            {
                if (CodigoCoordinador == null)
                    return Color.Red;
                else
                    return Color.Black;
            }
            set { }
        }
    }
}
