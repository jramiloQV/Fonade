using Datos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Empresas
{
    public class Empresa
    {
        public static List<ProyectoInterventoriaDTO> GetProyectos(int idInterventor, int idProyecto, int startIndex, int maxRows, int? _codOperador)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                int[] estados = new int[] { Constantes.CONST_Ejecucion };
                var entities = (from proyectos in db.Proyecto
                                join subsectores in db.SubSector on proyectos.CodSubSector equals subsectores.Id_SubSector
                                join sectores in db.Sector on subsectores.CodSector equals sectores.Id_Sector
                                join empresas in db.Empresas on proyectos.Id_Proyecto equals empresas.codproyecto
                                orderby proyectos.Id_Proyecto descending
                                where
                                    estados.Contains(proyectos.CodEstado)
                                    && proyectos.Inactivo == false
                                    && (idProyecto == 0 || proyectos.Id_Proyecto == idProyecto)
                                    && proyectos.codOperador == _codOperador
                                select new ProyectoInterventoriaDTO
                                {
                                    Id = proyectos.Id_Proyecto,
                                    IdEmpresa = empresas.id_empresa,
                                    Nombre = proyectos.NomProyecto,
                                    RazonSocial = empresas.razonsocial,
                                    IdSector = sectores.Id_Sector,
                                    Sector = sectores.NomSector,
                                    CurrentInterventor = idInterventor,                                   
                                    InterventorDelProyecto =
                                                        (from contactos in db.Contacto
                                                         join empresaInterventor in db.EmpresaInterventors on contactos.Id_Contacto equals empresaInterventor.CodContacto
                                                         where
                                                                  empresaInterventor.FechaFin == null
                                                               && empresaInterventor.Inactivo == false
                                                               && (empresaInterventor.Rol ==  Constantes.CONST_RolInterventor || empresaInterventor.Rol == Constantes.CONST_RolInterventorLider)
                                                               && empresaInterventor.CodEmpresa == empresas.id_empresa                                                               
                                                         orderby empresaInterventor.Rol descending
                                                         select
                                                           new InterventorProyectoDTO
                                                           {
                                                               Id = contactos.Id_Contacto,
                                                               NombreCompleto = contactos.Nombres + ' ' + contactos.Apellidos,
                                                               idContratoEntidad = empresaInterventor.IdContratoInterventoria
                                                           }
                                                        ).FirstOrDefault()
                                });                                                                


                entities = entities.Skip(startIndex).Take(maxRows);

                return entities.ToList();
            }

            

        }


        public static Int32 Count(int idInterventor, int idProyecto, int? _codOperador)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                int[] estados = new int[] { Constantes.CONST_Ejecucion };
                var entities = (from proyectos in db.Proyecto
                                join subsectores in db.SubSector on proyectos.CodSubSector equals subsectores.Id_SubSector
                                join sectores in db.Sector on subsectores.CodSector equals sectores.Id_Sector
                                join empresas in db.Empresas on proyectos.Id_Proyecto equals empresas.codproyecto
                                where
                                    estados.Contains(proyectos.CodEstado)
                                    && proyectos.Inactivo == false
                                    && (idProyecto == 0 || proyectos.Id_Proyecto == idProyecto)
                                    && proyectos.codOperador == _codOperador
                                select new ProyectoInterventoriaDTO
                                {
                                    Id = proyectos.Id_Proyecto,
                                    IdEmpresa = empresas.id_empresa,
                                    Nombre = proyectos.NomProyecto,
                                    RazonSocial = empresas.razonsocial,
                                    IdSector = sectores.Id_Sector,
                                    Sector = sectores.NomSector,
                                    CurrentInterventor = idInterventor,
                                    InterventorDelProyecto =
                                                        (from contactos in db.Contacto
                                                         join empresaInterventor in db.EmpresaInterventors on contactos.Id_Contacto equals empresaInterventor.CodContacto
                                                         where
                                                                  empresaInterventor.FechaFin == null
                                                               && empresaInterventor.Inactivo == false
                                                               && (empresaInterventor.Rol == Constantes.CONST_RolInterventor || empresaInterventor.Rol == Constantes.CONST_RolInterventorLider)
                                                               && empresaInterventor.CodEmpresa == empresas.id_empresa
                                                         orderby empresaInterventor.Rol descending
                                                         select
                                                           new InterventorProyectoDTO
                                                           {
                                                               Id = contactos.Id_Contacto,                                                                                                                          
                                                               NombreCompleto = contactos.Nombres + ' ' + contactos.Apellidos,
                                                               idContratoEntidad = empresaInterventor.IdContratoInterventoria                                                           }
                                                        ).FirstOrDefault()
                                }).Count();


                return entities;
            }
        }

        public static void AsignarInterventorEmpresa(int idInterventor, int idEmpresa,int? idContrato)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = db.EmpresaInterventors.Where(filter => filter.CodEmpresa == idEmpresa
                                                                      && filter.FechaFin == null
                                                                      && filter.Inactivo == false
                                                            ).ToList();

                foreach (var entity in entities)
                {
                    entity.FechaFin = DateTime.Now;
                    entity.Inactivo = true;
                    db.SubmitChanges();
                }

                var newEntity = new Datos.EmpresaInterventor
                {
                    CodEmpresa = idEmpresa,
                    CodContacto = idInterventor,
                    IdContratoInterventoria = idContrato,
                    FechaInicio = DateTime.Now,
                    Rol = Datos.Constantes.CONST_RolInterventorLider,
                    Inactivo = false
                };

                db.EmpresaInterventors.InsertOnSubmit(newEntity);
                db.SubmitChanges();
            }
        }

        public static void DesAsignarInterventorEmpresa(int idInterventor, int idEmpresa)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = db.EmpresaInterventors.Where(filter => filter.CodEmpresa == idEmpresa
                                                                       && filter.FechaFin == null
                                                                       && filter.Inactivo == false
                                                            ).ToList();
                foreach (var entity in entities)
                {
                    entity.FechaFin = DateTime.Now;
                    entity.Inactivo = true;
                    db.SubmitChanges();
                }
            }
        }


        public static List<ProyectoInterventoriaDTO> GetProyectoAsignadoByInterventor(int idProyecto, int idInterventor)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                int[] estados = new int[] { Constantes.CONST_Ejecucion };
                var entities = (from proyectos in db.Proyecto1s                                                                
                                join empresas in db.Empresas on proyectos.Id_Proyecto equals empresas.codproyecto                                
                                where
                                    estados.Contains(proyectos.CodEstado)
                                    && proyectos.Inactivo == false
                                    && proyectos.Id_Proyecto == idProyecto
                                select new ProyectoInterventoriaDTO
                                {
                                    Id = proyectos.Id_Proyecto,
                                    IdEmpresa = empresas.id_empresa,
                                    Nombre = proyectos.NomProyecto,
                                    RazonSocial = empresas.razonsocial,
                                    CurrentInterventor = idInterventor,
                                    IdSector = 0,
                                    Sector = string.Empty,
                                    InterventorDelProyecto =
                                                        (from contactos in db.Contacto
                                                         join empresaInterventor in db.EmpresaInterventors on contactos.Id_Contacto equals empresaInterventor.CodContacto
                                                         where
                                                                  empresaInterventor.FechaFin == null
                                                               && empresaInterventor.Inactivo == false
                                                               && (empresaInterventor.Rol == Constantes.CONST_RolInterventor || empresaInterventor.Rol == Constantes.CONST_RolInterventorLider)
                                                               && empresaInterventor.CodEmpresa == empresas.id_empresa
                                                         orderby empresaInterventor.Rol descending
                                                         select
                                                           new InterventorProyectoDTO
                                                           {
                                                               Id = contactos.Id_Contacto,
                                                               NombreCompleto = contactos.Nombres + ' ' + contactos.Apellidos,
                                                               idContratoEntidad = empresaInterventor.IdContratoInterventoria
                                                           }
                                                        ).FirstOrDefault()
                                });               

                return entities.ToList();
            }
        }

        public static List<ProyectoInterventoriaDTO> GetProyectoAsignadoByInterventorAndNombre(string nombreProyecto, int idInterventor)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                int[] estados = new int[] { Constantes.CONST_Ejecucion };
                var entities = (from proyectos in db.Proyecto1s
                                join empresas in db.Empresas on proyectos.Id_Proyecto equals empresas.codproyecto
                                where
                                    estados.Contains(proyectos.CodEstado)
                                    && proyectos.Inactivo == false
                                    && proyectos.NomProyecto.Contains(nombreProyecto) 
                                select new ProyectoInterventoriaDTO
                                {
                                    Id = proyectos.Id_Proyecto,
                                    IdEmpresa = empresas.id_empresa,
                                    Nombre = proyectos.NomProyecto,
                                    RazonSocial = empresas.razonsocial,
                                    CurrentInterventor = idInterventor,
                                    IdSector = 0,
                                    Sector = string.Empty,
                                    InterventorDelProyecto =
                                                        (from contactos in db.Contacto
                                                         join empresaInterventor in db.EmpresaInterventors on contactos.Id_Contacto equals empresaInterventor.CodContacto
                                                         where
                                                                  empresaInterventor.FechaFin == null
                                                               && empresaInterventor.Inactivo == false
                                                               && (empresaInterventor.Rol == Constantes.CONST_RolInterventor || empresaInterventor.Rol == Constantes.CONST_RolInterventorLider)
                                                               && empresaInterventor.CodEmpresa == empresas.id_empresa
                                                         orderby empresaInterventor.Rol descending
                                                         select
                                                           new InterventorProyectoDTO
                                                           {
                                                               Id = contactos.Id_Contacto,
                                                               NombreCompleto = contactos.Nombres + ' ' + contactos.Apellidos,
                                                               idContratoEntidad = empresaInterventor.IdContratoInterventoria
                                                           }
                                                        ).FirstOrDefault()
                                });

                return entities.ToList();
            }
        }

    }

    public class InterventorProyectoDTO
    {

        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public int? idContratoEntidad { get; set; }
    }

    public class ProyectoInterventoriaDTO
    {
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public string Nombre { get; set; }
        public string RazonSocial { get; set; }
        public int? CodigoInterventor
        {
            get
            {
                return InterventorDelProyecto != null ? InterventorDelProyecto.Id : (int?)null;
            }
            set { }
        }
        public string Interventor
        {
            get
            {
                return InterventorDelProyecto != null ? InterventorDelProyecto.NombreCompleto : "Sin interventor";
            }
            set { }
        }
        public int IdSector { get; set; }
        public string Sector { get; set; }
        public bool IsOwner
        {
            get
            {
                if (CodigoInterventor == null)
                    return false;
                return CodigoInterventor.Equals(CurrentInterventor);
            }
            set { }
        }
        public int CurrentInterventor { get; set; }        
        public InterventorProyectoDTO InterventorDelProyecto { get; set; }
        public Color currentColor
        {
            get
            {
                if (CodigoInterventor == null)
                    return Color.Red;
                else
                    return Color.Black;
            }
            set { }
        }

        public string CodigoContrato {
            get {
                if (InterventorDelProyecto != null)
                {
                    return InterventorDelProyecto.idContratoEntidad != null ? InterventorDelProyecto.idContratoEntidad.ToString() : string.Empty;
                }
                else
                    return string.Empty;               
            }
            set { }
        }
    }

}
