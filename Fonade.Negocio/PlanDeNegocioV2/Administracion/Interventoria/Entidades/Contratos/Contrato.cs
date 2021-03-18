using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Fonade.Negocio.Utility;

namespace Fonade.Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Contratos
{
    public class Contrato
    {
        public static int Count(int idEntidad, string numeroContrato)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from contratos in db.ContratoEntidads
                                where
                                    contratos.IdEntidad == idEntidad
                                    && (numeroContrato == "" || contratos.NumeroContrato.Contains(numeroContrato))
                                select contratos
                                ).Count();

                return entities;
            }
        }

        public static List<Datos.ContratoEntidad> Get(int idEntidad, string numeroContrato, int startIndex, int maxRows)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from contratos in db.ContratoEntidads
                                where
                                     contratos.IdEntidad == idEntidad
                                     && (numeroContrato == "" || contratos.NumeroContrato.Contains(numeroContrato))
                                select contratos);

                entities = entities.Skip(startIndex).Take(maxRows);

                return entities.ToList();
            }
        }

        public static List<ContratoEntidadDTO> Get(int idEntidad)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from contratos in db.ContratoEntidads
                                where contratos.IdEntidad == idEntidad
                                orderby contratos.FechaTerminacion ascending
                                select new ContratoEntidadDTO {
                                    Id = contratos.Id,
                                    NumeroContrato = contratos.NumeroContrato,
                                    FechaInicio = contratos.FechaInicio,
                                    FechaFin = contratos.FechaTerminacion
                                });
                
                return entities.ToList();
            }
        }

        public static Datos.ContratoEntidad FirstOrDefault(int idContrato)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {                
                return db.ContratoEntidads.FirstOrDefault(filter => filter.Id == idContrato);
            }
        }

        public static bool ExistContrato(string codigo, int idEntidad)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                return db.ContratoEntidads.Any(filter => filter.IdEntidad == idEntidad
                                                        && filter.NumeroContrato == codigo.Trim()
                );
            }
        }

        public static bool ExistContrato(string codigo, int idEntidad, int idContrato)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                return db.ContratoEntidads.Any(filter => filter.IdEntidad == idEntidad
                                                        && filter.NumeroContrato == codigo.Trim()
                                                        && filter.Id != idContrato
                );
            }
        }

        public static void Insert(Datos.ContratoEntidad newEntity)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                db.ContratoEntidads.InsertOnSubmit(newEntity);
                db.SubmitChanges();
            }
        }

        public static void Update(Datos.ContratoEntidad currentEntity)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = db.ContratoEntidads.FirstOrDefault(filter => filter.Id == currentEntity.Id);

                if (entity != null)
                {
                    entity.NumeroContrato = currentEntity.NumeroContrato;
                    entity.FechaInicio = currentEntity.FechaInicio;
                    entity.FechaTerminacion = currentEntity.FechaTerminacion;
                    entity.FechaActualizacion = DateTime.Now;

                    db.SubmitChanges();
                }
            }
        }

        public static List<InterventorContratoDTO> GetInterventoresByContratoEntidad(int idEntidad,int idContrato)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from contactos in db.Contacto
                                join gruposContacto in db.GrupoContactos on contactos.Id_Contacto equals gruposContacto.CodContacto
                                join grupos in db.Grupos on gruposContacto.CodGrupo equals grupos.Id_Grupo
                                join entidadInterventores in db.EntidadInterventors on contactos.Id_Contacto equals entidadInterventores.IdContactoInterventor
                                where contactos.Inactivo == false
                                      && grupos.Id_Grupo == Datos.Constantes.CONST_Interventor
                                      && entidadInterventores.IdEntidad == idEntidad
                                orderby contactos.Nombres
                                select new InterventorContratoDTO
                                {
                                    IdContacto = contactos.Id_Contacto,
                                    NombreCompleto = contactos.Nombres + " " + contactos.Apellidos,                                    
                                    IsOwner = db.ContratoInterventors.Any(filter => filter.IdContrato == idContrato
                                                                                    && filter.IdContactoInterventor == contactos.Id_Contacto),
                                    HasContract = db.ContratoInterventors.Any(filter => filter.IdContactoInterventor == contactos.Id_Contacto)
                                }
                    ).ToList();

                return entities;
            }
        }

        public static void AsignarInterventorContrato(int idContrato, int idInterventor, int idUsuario)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var currentStatus = !db.ContratoInterventors.Any(filter => filter.IdContactoInterventor == idInterventor
                                                                            && filter.IdContrato == idContrato
                                                                );
                if (currentStatus) {
                    var newEntity = new Datos.ContratoInterventor
                    {
                        IdContactoInterventor = idInterventor,
                        IdContrato = idContrato,
                        UsuarioCreacion = idUsuario,
                        FechaCreacion = DateTime.Now
                    };

                    db.ContratoInterventors.InsertOnSubmit(newEntity);
                    db.SubmitChanges();
                }
            }
        }

        public static void DesAsignarInterventorEntidad(int idInterventor,int idContrato)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var currentStatus = db.ContratoInterventors.Where(filter => filter.IdContactoInterventor == idInterventor
                                                                            && filter.IdContrato == idContrato
                                                                  ).ToList();

                db.ContratoInterventors.DeleteAllOnSubmit(currentStatus);
                db.SubmitChanges();
            }
        }

        public static List<ContratoEntidadDTO> GetContratosByInterventor(int idInterventor)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
               var entities = ( from contratos in db.ContratoEntidads
                                join contratoInterventor in db.ContratoInterventors on contratos.Id equals contratoInterventor.IdContrato
                                where contratoInterventor.IdContactoInterventor == idInterventor
                                select new ContratoEntidadDTO
                                {
                                    Id = contratos.Id,
                                    NumeroContrato = contratos.NumeroContrato,
                                    FechaInicio = contratos.FechaInicio,
                                    FechaFin = contratos.FechaTerminacion
                                }).ToList();

                return entities;
            }
        }
    }
    
    public class InterventorContratoDTO
    {
        public int IdContacto { get; set; }
        public string NombreCompleto { get; set; }
        public bool IsOwner { get; set; }
        public bool HasContract { get; set; }
    }

    public class ContratoEntidadDTO
    {
        public int Id { get; set; }
        public string NumeroContrato { get; set; }
        public DateTime FechaInicio { get; set; }
        public string FechaInicioWithFormat
        {
            get
            {
                return FechaInicio.getFechaAbreviadaConFormato(false);
            }
            set { }
        }
        public DateTime FechaFin { get; set; }
        public string FechaFinWithFormat {
            get {
                return FechaFin.getFechaAbreviadaConFormato(false);
            }
            set { }
        }
        public bool ContratoVencido {
            get {
                return DateTime.Now > FechaFin;
            }
            set { }
        }

        public string NumeroContratoWithFormat
        {
            get
            {
                return ContratoVencido ? NumeroContrato + " - Vencido" : NumeroContrato;
            }
            set { }
        }
    }
}
