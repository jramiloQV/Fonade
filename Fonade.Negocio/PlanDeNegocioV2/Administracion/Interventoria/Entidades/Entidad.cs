using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades
{
    public class EntidadDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string NombreCorto { get; set; }
        public string NumerpPoliza { get; set; }
        public DateTime FechaPoliza { get; set; }
        public string PersonaACargo { get; set; }
        public string TelefonoOficina { get; set; }
        public string TelefonoCelular { get; set; }
        public string Direccion { get; set; }
        public string Dependencia { get; set; }
        public string Email { get; set; }
        public string ImagenLogo { get; set; }
        public int UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public int? codOperador { get; set; }
        public string Operador { get; set; }
    }
    public class Entidad
    {
        
        public static int Count(string codigo)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from entidades in db.EntidadInterventoria                               
                                where
                                    (codigo == "" || entidades.Nombre.Contains(codigo))                                      
                                select entidades
                                ).Count();

                return entities;
            }
        }

        public static List<Datos.Operador> GetOperadores(int? _codOperador)
        {
            using (Datos.FonadeDBLightDataContext db = new Datos.FonadeDBLightDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                if (_codOperador == null)
                {
                    var entities = (from entidades in db.Operador                                   
                                    select entidades);

                    return entities.ToList();

                }
                else
                {
                    var entities = (from entidades in db.Operador
                                    where entidades.IdOperador == _codOperador
                                    select entidades);

                    return entities.ToList();
                }
            }
        }

        public static List<Datos.EntidadInterventoria> Get(string codigo, int startIndex, int maxRows, int? _codOperador)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                if (_codOperador == null)
                {
                    var entities = (from entidades in db.EntidadInterventoria
                                    where
                                          (codigo == "" || entidades.Nombre.Contains(codigo))
                                    select entidades);

                    entities = entities.Skip(startIndex).Take(maxRows);

                    return entities.ToList();

                }
                else
                {
                    var entities = (from entidades in db.EntidadInterventoria
                                    where
                                          (codigo == "" || entidades.Nombre.Contains(codigo))
                                          && entidades.codOperador == _codOperador
                                    select entidades);

                    entities = entities.Skip(startIndex).Take(maxRows);

                    return entities.ToList();
                }
            }
        }

        public static List<Datos.EntidadInterventoria> Get(int? _codOperador)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from entidades in db.EntidadInterventoria
                                where entidades.codOperador == _codOperador
                                orderby entidades.Nombre ascending
                                select entidades);                

                return entities.ToList();
            }
        }

        public static List<Datos.EntidadInterventoria> GetXOperador(int? _codOperador)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {                
                var entities = (from entidades in db.EntidadInterventoria
                                where entidades.codOperador == _codOperador
                                orderby entidades.Nombre ascending
                                select entidades);

                return entities.ToList();
            }
        }
        public static string _cadena = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public static bool ExistenEntidades(int? _codOperador)
        {
            using(Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(_cadena))
            {
                var entidades = (from i in db.EntidadInterventoria                                    
                                     where i.codOperador == _codOperador
                                     select new
                                     {
                                         i.Id
                                     }).ToList();

                return entidades.Count > 0;
            }
        }

        public static Datos.EntidadInterventoria FirstOrDefault(int codigoEntidad)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from entidades in db.EntidadInterventoria
                                where
                                    entidades.Id == codigoEntidad
                                select entidades).FirstOrDefault();
                
                return entities;
            }
        }

        public static bool ExistEntidadByName(string codigo, int? _codOperador, int? idEntidad = null)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                if (idEntidad != null) {

                    var entities = (from entidades in db.EntidadInterventoria
                                    where
                                          entidades.Nombre == codigo
                                          && entidades.Id != idEntidad.GetValueOrDefault()
                                          && entidades.codOperador == _codOperador
                                    select entidades).Any();
                    return entities;
                }
                else
                {
                    var entities = (from entidades in db.EntidadInterventoria
                                    where
                                          entidades.Nombre == codigo
                                          && entidades.codOperador == _codOperador
                                    select entidades).Any();
                    return entities;
                }                
            }
        }

        public static bool ExistEntidadByShortName(string codigo, int? _codOperador, int? idEntidad = null)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                if (idEntidad != null)
                {
                    var entities = (from entidades in db.EntidadInterventoria
                                    where
                                          entidades.NombreCorto == codigo
                                          && entidades.Id != idEntidad.GetValueOrDefault()
                                          && entidades.codOperador == _codOperador
                                    select entidades).Any();
                    return entities;
                }
                else
                {
                    var entities = (from entidades in db.EntidadInterventoria
                                    where
                                          entidades.NombreCorto == codigo
                                          && entidades.codOperador == _codOperador
                                    select entidades).Any();
                    return entities;
                }                    
            }
        }

        public static void Insert(Datos.EntidadInterventoria entity)
        {            
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                db.EntidadInterventoria.InsertOnSubmit(entity);
                db.SubmitChanges();
            }
        }

        public static void Update(Datos.EntidadInterventoria entity)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var currentEntity = db.EntidadInterventoria.FirstOrDefault(filter => filter.Id == entity.Id);

                if (currentEntity != null)
                {
                    currentEntity.Nombre = entity.Nombre;
                    currentEntity.NombreCorto = entity.NombreCorto;
                    currentEntity.NumeroPoliza = entity.NumeroPoliza;
                    currentEntity.FechaPoliza = entity.FechaPoliza;
                    currentEntity.PersonaACargo = entity.PersonaACargo;
                    currentEntity.TelefonoOficina = entity.TelefonoOficina;
                    currentEntity.TelefonoCelular = entity.TelefonoCelular;
                    currentEntity.Direccion = entity.Direccion;
                    currentEntity.Dependencia = entity.Dependencia;
                    currentEntity.Email = entity.Email;

                    if(entity.ImagenLogo!=string.Empty)
                    currentEntity.ImagenLogo = entity.ImagenLogo;

                    currentEntity.FechaActualizacion = DateTime.Now;
                    
                    db.SubmitChanges();
                }                
            }
        }

        public static List<EntidadDepartamentoDTO> GetDepartamentosByEntidad(int idEntidad)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                return (from departamentos in db.departamento
                        orderby departamentos.NomDepartamento ascending
                        select new EntidadDepartamentoDTO() {
                            Id = departamentos.Id_Departamento,
                            NombreDepartamento = departamentos.NomDepartamento,
                            IdEntidad = idEntidad,
                            entidadDepartmento = db.EntidadDepartamentos.FirstOrDefault(filter => filter.IdEntidad == idEntidad
                                                                                      && filter.IdDepartamento == departamentos.Id_Departamento
                                                                                        )                            
                        })
                        .ToList();
            }
        }

        public static List<InterventorEntidadDTO> GetInterventores(int idEntidad, int? _codOperador)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from contactos in db.Contacto
                                join gruposContacto in db.GrupoContactos on contactos.Id_Contacto equals gruposContacto.CodContacto
                                join grupos in db.Grupos on gruposContacto.CodGrupo equals grupos.Id_Grupo
                                where contactos.Inactivo == false
                                      && grupos.Id_Grupo == Datos.Constantes.CONST_Interventor
                                      && contactos.codOperador == _codOperador
                                orderby contactos.Nombres ascending
                                select new InterventorEntidadDTO
                                {
                                    IdContacto = contactos.Id_Contacto,
                                    NombreCompleto = contactos.Nombres + " " + contactos.Apellidos,
                                    IdCurrentEntidad = idEntidad,
                                    EntidadOwner = db.EntidadInterventors.FirstOrDefault(filter => filter.IdEntidad == idEntidad
                                                                                         && filter.IdContactoInterventor == contactos.Id_Contacto),
                                    Entidad = db.EntidadInterventors.Any(filter2 => filter2.IdContactoInterventor == contactos.Id_Contacto)
                                                                        ? db.EntidadInterventoria.First(filter3 => filter3.Id == db.EntidadInterventors.FirstOrDefault(filter => filter.IdContactoInterventor == contactos.Id_Contacto).IdEntidad).Nombre 
                                                                        : "Sin entidad"
                                }
                    ).ToList();

                return entities;
            }
        }

        public static List<InterventorEntidadDTO> GetInterventoresByEntidad(int idEntidad)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from contactos in db.Contacto
                                join gruposContacto in db.GrupoContactos on contactos.Id_Contacto equals gruposContacto.CodContacto
                                join grupos in db.Grupos on gruposContacto.CodGrupo equals grupos.Id_Grupo
                                join interventores in db.EntidadInterventors on contactos.Id_Contacto equals interventores.IdContactoInterventor
                                orderby contactos.Nombres ascending
                                where interventores.IdEntidad == idEntidad
                                      && contactos.Inactivo == false
                                      && grupos.Id_Grupo == Datos.Constantes.CONST_Interventor
                                      
                                select new InterventorEntidadDTO
                                {
                                    IdContacto = contactos.Id_Contacto,
                                    NombreCompleto = contactos.Nombres + " " + contactos.Apellidos
                                }
                    ).ToList();

                return entities;
            }
        }

        public static void AsignarInterventorEntidad(int idEntidad, int idInterventor, int idUsuario)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var currentStatus = db.EntidadInterventors.Where(filter => filter.IdContactoInterventor == idInterventor).ToList();

                if(currentStatus.Any())
                db.EntidadInterventors.DeleteAllOnSubmit(currentStatus);
                db.SubmitChanges();

                var newEntity = new Datos.EntidadInterventor
                {
                    IdContactoInterventor = idInterventor,
                    IdEntidad = idEntidad,
                    UsuarioCreacion = idUsuario
                };

                db.EntidadInterventors.InsertOnSubmit(newEntity);
                db.SubmitChanges();            
            }
        }

        public static void DesAsignarInterventorEntidad(int idInterventor, int idUsuario)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var currentStatus = db.EntidadInterventors.Where(filter => filter.IdContactoInterventor == idInterventor).ToList();

                db.EntidadInterventors.DeleteAllOnSubmit(currentStatus);
                db.SubmitChanges();
            }
        }

    }

    public class EntidadDepartamentoDTO {
        public int Id { get; set; }
        public string NombreDepartamento { get; set; }
        public int IdEntidad { get; set; }
        public Datos.EntidadDepartamento entidadDepartmento { get;set;}
        public bool CheckDepartamento {
            get {
                return entidadDepartmento != null;
            } set { } }
        public int? ZonaDepartamento {
            get {
                return entidadDepartmento != null ? entidadDepartmento.IdZona : (Int32?)null;
            } set { } }
    }

    public class InterventorEntidadDTO {
        public int IdContacto { get; set; }
        public string NombreCompleto { get; set; }
        public string Entidad { get; set; }
        public int IdCurrentEntidad { get; set; }
        public Datos.EntidadInterventor EntidadOwner { get; set; }
        public bool IsOwner {
            get {
                return EntidadOwner != null ? IdCurrentEntidad == EntidadOwner.IdEntidad : false;
            }
            set { }
        }

        public Color currentColor
        {
            get
            {
                if (Entidad == "Sin entidad")
                    return Color.Red;
                else
                    return Color.Black;
            }
            set { }
        }
    }
}
