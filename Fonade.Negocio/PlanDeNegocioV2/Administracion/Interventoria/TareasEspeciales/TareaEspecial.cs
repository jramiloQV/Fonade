using System;
using System.Collections.Generic;
using System.Linq;
using Fonade.Negocio.Utility;

namespace Fonade.Negocio.PlanDeNegocioV2.Administracion.Interventoria.TareasEspeciales
{
    public class TareaEspecial
    {
        public static List<TareaEspecialDTO> GetTareasEspecialesByProyectoId(int codigoProyecto, int codigoUsuario, bool pendientes)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from tareasEspeciales in db.TareaEspecialInterventorias
                                join pagos in db.PagoActividad on tareasEspeciales.CodigoPago equals pagos.Id_PagoActividad
                                join proyectos in db.Proyecto on pagos.CodProyecto equals proyectos.Id_Proyecto
                                join contactoRemitente in db.Contacto on tareasEspeciales.Remitente equals contactoRemitente.Id_Contacto
                                join contactoDestinatario in db.Contacto on tareasEspeciales.Remitente equals contactoDestinatario.Id_Contacto
                                join estado in db.EstadoTareaEspecialInterventorias on tareasEspeciales.Estado equals estado.Id_estadoTareaEspecial
                                where proyectos.Id_Proyecto == codigoProyecto
                                      && (tareasEspeciales.Destinatario == codigoUsuario || tareasEspeciales.Remitente == codigoUsuario)
                                      && (pendientes || tareasEspeciales.Estado == Datos.Constantes.const_estado_tareaEspecial_pendiente)
                                select new TareaEspecialDTO
                                {
                                    Id = tareasEspeciales.Id_tareaEspecial,
                                    CodigoPago = tareasEspeciales.CodigoPago,
                                    CodigoProyecto = pagos.CodProyecto.GetValueOrDefault(),
                                    NombreEstado = estado.Descripcion,
                                    Historia = db.HistoriaTareaEspecials
                                                    .Where(filter => filter.IdTareaEspecialInterventoria == tareasEspeciales.Id_tareaEspecial)
                                                    .OrderBy(orderFilter => orderFilter.FechaCreacion)
                                                    .FirstOrDefault(),
                                    FechaInicio = tareasEspeciales.FechaInicio,
                                    CodigoRemitente = contactoRemitente.Id_Contacto,                                    
                                    NombreRemitente = contactoRemitente.Nombres + " " + contactoRemitente.Apellidos,
                                    CodigoDestinatario = contactoDestinatario.Id_Contacto,
                                    NombreDestinatario = contactoDestinatario.Nombres + " " + contactoDestinatario.Apellidos,
                                    Estado = estado.Id_estadoTareaEspecial,
                                    NombreProyecto = proyectos.NomProyecto
                                }).ToList();

                return entities;
            }
        }
        public static List<TareaEspecialDTO> GetTareasEspecialesByPagoId(int codigoPago, int codigoUsuario, bool pendientes)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from tareasEspeciales in db.TareaEspecialInterventorias
                                join pagos in db.PagoActividad on tareasEspeciales.CodigoPago equals pagos.Id_PagoActividad
                                join proyectos in db.Proyecto on pagos.CodProyecto equals proyectos.Id_Proyecto
                                join contactoRemitente in db.Contacto on tareasEspeciales.Remitente equals contactoRemitente.Id_Contacto
                                join contactoDestinatario in db.Contacto on tareasEspeciales.Remitente equals contactoDestinatario.Id_Contacto
                                join estado in db.EstadoTareaEspecialInterventorias on tareasEspeciales.Estado equals estado.Id_estadoTareaEspecial
                                where tareasEspeciales.CodigoPago == codigoPago
                                      && (tareasEspeciales.Destinatario == codigoUsuario || tareasEspeciales.Remitente == codigoUsuario)
                                      && (pendientes || tareasEspeciales.Estado == Datos.Constantes.const_estado_tareaEspecial_pendiente)
                                select new TareaEspecialDTO
                                {
                                    Id = tareasEspeciales.Id_tareaEspecial,
                                    CodigoPago = tareasEspeciales.CodigoPago,
                                    CodigoProyecto = pagos.CodProyecto.GetValueOrDefault(),
                                    NombreEstado = estado.Descripcion,
                                    Historia = db.HistoriaTareaEspecials
                                                    .Where(filter => filter.IdTareaEspecialInterventoria == tareasEspeciales.Id_tareaEspecial)
                                                    .OrderBy(orderFilter => orderFilter.FechaCreacion)
                                                    .FirstOrDefault(),
                                    FechaInicio = tareasEspeciales.FechaInicio,
                                    CodigoDestinatario = contactoRemitente.Id_Contacto,
                                    NombreRemitente = contactoRemitente.Nombres + " " + contactoRemitente.Apellidos,
                                    CodigoRemitente = contactoDestinatario.Id_Contacto,
                                    NombreDestinatario = contactoDestinatario.Nombres + " " + contactoDestinatario.Apellidos,
                                    Estado = estado.Id_estadoTareaEspecial,
                                    NombreProyecto = proyectos.NomProyecto
                                }).ToList();

                return entities;
            }
        }
        public static TareaEspecialDTO GetTareaEspecialeByTareaId(int codigoTarea)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from tareasEspeciales in db.TareaEspecialInterventorias
                                join pagos in db.PagoActividad on tareasEspeciales.CodigoPago equals pagos.Id_PagoActividad
                                join proyectos in db.Proyecto on pagos.CodProyecto equals proyectos.Id_Proyecto
                                join contactoRemitente in db.Contacto on tareasEspeciales.Remitente equals contactoRemitente.Id_Contacto
                                join contactoDestinatario in db.Contacto on tareasEspeciales.Remitente equals contactoDestinatario.Id_Contacto
                                join estado in db.EstadoTareaEspecialInterventorias on tareasEspeciales.Estado equals estado.Id_estadoTareaEspecial
                                where tareasEspeciales.Id_tareaEspecial == codigoTarea
                                select new TareaEspecialDTO
                                {
                                    Id = tareasEspeciales.Id_tareaEspecial,
                                    CodigoPago = tareasEspeciales.CodigoPago,
                                    CodigoProyecto = pagos.CodProyecto.GetValueOrDefault(),
                                    NombreEstado = estado.Descripcion,
                                    Historia = db.HistoriaTareaEspecials
                                                    .Where(filter => filter.IdTareaEspecialInterventoria == tareasEspeciales.Id_tareaEspecial)
                                                    .OrderBy(orderFilter => orderFilter.FechaCreacion)
                                                    .FirstOrDefault(),                                                                                       
                                    FechaInicio = tareasEspeciales.FechaInicio,
                                    CodigoDestinatario = contactoRemitente.Id_Contacto,
                                    NombreRemitente = contactoRemitente.Nombres + " " + contactoRemitente.Apellidos,
                                    CodigoRemitente = contactoDestinatario.Id_Contacto,
                                    NombreDestinatario = contactoDestinatario.Nombres + " " + contactoDestinatario.Apellidos,
                                    Estado = estado.Id_estadoTareaEspecial,
                                    NombreProyecto = proyectos.NomProyecto
                                }).FirstOrDefault();

                return entities;
            }
        }
        public static List<HistoriaTareaDTO> GetHistoriaTareaEspecialeByTareaId(int codigoTarea, int codigoUsuarioConsulta)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from historia in db.HistoriaTareaEspecials
                              join contacto in db.Contacto on historia.Remitente equals contacto.Id_Contacto
                              where historia.IdTareaEspecialInterventoria == codigoTarea
                              orderby historia.FechaCreacion descending
                              select new HistoriaTareaDTO
                              {
                                  Observacion = historia.Observacion,
                                  Archivo = historia.Archivo,
                                  codigoContactoRemitente = historia.Remitente,
                                  NombreRemitente = contacto.Nombres + " " + contacto.Apellidos,
                                  FechaCreacion = historia.FechaCreacion,
                                  CodigoUsuarioConsulta = codigoUsuarioConsulta,
                                  codigoContactoDestinatario = historia.Destinatario,
                                  FechaLectura = historia.FechaLecturaDestinatario
                                }).ToList();

                return entity;
            }
        }
        public static void Insert(Datos.TareaEspecialInterventoria entity)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                db.TareaEspecialInterventorias.InsertOnSubmit(entity);
                db.SubmitChanges();
            }
        }
        public static void cerrarTarea(int codigoTarea)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = db.TareaEspecialInterventorias.FirstOrDefault(filter => filter.Id_tareaEspecial == codigoTarea);

                if (entity != null)
                {
                    entity.Estado = Datos.Constantes.const_estado_tareaEspecial_cerrada;
                    db.SubmitChanges();
                }                
                
            }
        }
        public static void InsertHistoria(Datos.HistoriaTareaEspecial entity)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                db.HistoriaTareaEspecials.InsertOnSubmit(entity);
                db.SubmitChanges();
            }
        }

        public static int CountTareasEspecialesByProyectoId(int codigo, int codigoUsuario, bool pendientes, int? _codOperador)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from tareasEspeciales in db.TareaEspecialInterventorias
                                join pagos in db.PagoActividad on tareasEspeciales.CodigoPago equals pagos.Id_PagoActividad
                                join proyectos in db.Proyecto on pagos.CodProyecto equals proyectos.Id_Proyecto
                                join contactoRemitente in db.Contacto on tareasEspeciales.Remitente equals contactoRemitente.Id_Contacto
                                join contactoDestinatario in db.Contacto on tareasEspeciales.Remitente equals contactoDestinatario.Id_Contacto
                                join estado in db.EstadoTareaEspecialInterventorias on tareasEspeciales.Estado equals estado.Id_estadoTareaEspecial
                                where
                                     (codigo == 0 || proyectos.Id_Proyecto == codigo)
                                     && (pendientes == false || tareasEspeciales.Estado == Datos.Constantes.const_estado_tareaEspecial_pendiente)
                                     && (tareasEspeciales.Destinatario == codigoUsuario || tareasEspeciales.Remitente == codigoUsuario)
                                     && proyectos.codOperador == _codOperador
                                select tareasEspeciales
                                ).Count();

                return entities;
            }
        }

        public static int CountTareasEspecialesByPagoId(int codigo, int codigoUsuario, bool pendientes, int? _codOperador)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from tareasEspeciales in db.TareaEspecialInterventorias
                                join pagos in db.PagoActividad on tareasEspeciales.CodigoPago equals pagos.Id_PagoActividad
                                join proyectos in db.Proyecto on pagos.CodProyecto equals proyectos.Id_Proyecto
                                join contactoRemitente in db.Contacto on tareasEspeciales.Remitente equals contactoRemitente.Id_Contacto
                                join contactoDestinatario in db.Contacto on tareasEspeciales.Remitente equals contactoDestinatario.Id_Contacto
                                join estado in db.EstadoTareaEspecialInterventorias on tareasEspeciales.Estado equals estado.Id_estadoTareaEspecial
                                where 
                                      (codigo == 0 || tareasEspeciales.CodigoPago == codigo)
                                      && (pendientes == false || tareasEspeciales.Estado == Datos.Constantes.const_estado_tareaEspecial_pendiente)
                                      && (tareasEspeciales.Destinatario == codigoUsuario || tareasEspeciales.Remitente == codigoUsuario)                                      
                                      && proyectos.codOperador == _codOperador
                                select tareasEspeciales
                                ).Count();

                return entities;
            }
        }

        public static List<TareaEspecialDTO> GetTareasEspecialesByProyectoId(int codigo, int codigoUsuario, bool pendientes, int startIndex, int maxRows, int? _codOperador)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from tareasEspeciales in db.TareaEspecialInterventorias
                                join pagos in db.PagoActividad on tareasEspeciales.CodigoPago equals pagos.Id_PagoActividad
                                join proyectos in db.Proyecto on pagos.CodProyecto equals proyectos.Id_Proyecto
                                join contactoRemitente in db.Contacto on tareasEspeciales.Remitente equals contactoRemitente.Id_Contacto
                                join contactoDestinatario in db.Contacto on tareasEspeciales.Remitente equals contactoDestinatario.Id_Contacto
                                join estado in db.EstadoTareaEspecialInterventorias on tareasEspeciales.Estado equals estado.Id_estadoTareaEspecial
                                where
                                    (codigo == 0 || proyectos.Id_Proyecto == codigo)
                                     && (pendientes == false || tareasEspeciales.Estado == Datos.Constantes.const_estado_tareaEspecial_pendiente)
                                     && (tareasEspeciales.Destinatario == codigoUsuario || tareasEspeciales.Remitente == codigoUsuario)
                                     && proyectos.codOperador == _codOperador
                                select new TareaEspecialDTO
                                {
                                    Id = tareasEspeciales.Id_tareaEspecial,
                                    CodigoPago = tareasEspeciales.CodigoPago,
                                    CodigoProyecto = pagos.CodProyecto.GetValueOrDefault(),
                                    NombreEstado = estado.Descripcion,
                                    Historia = db.HistoriaTareaEspecials
                                                    .Where(filter => filter.IdTareaEspecialInterventoria == tareasEspeciales.Id_tareaEspecial)
                                                    .OrderBy(orderFilter => orderFilter.FechaCreacion)
                                                    .FirstOrDefault(),
                                    FechaInicio = tareasEspeciales.FechaInicio,
                                    CodigoRemitente = contactoRemitente.Id_Contacto,
                                    NombreRemitente = contactoRemitente.Nombres + " " + contactoRemitente.Apellidos,
                                    CodigoDestinatario = contactoDestinatario.Id_Contacto,
                                    NombreDestinatario = contactoDestinatario.Nombres + " " + contactoDestinatario.Apellidos,
                                    Estado = estado.Id_estadoTareaEspecial,
                                    NombreProyecto = proyectos.NomProyecto,
                                    HasUpdates = HasUpdatesTareaEspecialByTareaIdAndUser(tareasEspeciales.Id_tareaEspecial, codigoUsuario)
                                });

                entities = entities.Skip(startIndex).Take(maxRows);

                return entities.ToList();
            }
        }
        public static List<TareaEspecialDTO> GetTareasEspecialesByPagoId(int codigo, int codigoUsuario, bool pendientes, int startIndex, int maxRows, int? _codOperador)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from tareasEspeciales in db.TareaEspecialInterventorias
                                join pagos in db.PagoActividad on tareasEspeciales.CodigoPago equals pagos.Id_PagoActividad
                                join proyectos in db.Proyecto on pagos.CodProyecto equals proyectos.Id_Proyecto
                                join contactoRemitente in db.Contacto on tareasEspeciales.Remitente equals contactoRemitente.Id_Contacto
                                join contactoDestinatario in db.Contacto on tareasEspeciales.Remitente equals contactoDestinatario.Id_Contacto
                                join estado in db.EstadoTareaEspecialInterventorias on tareasEspeciales.Estado equals estado.Id_estadoTareaEspecial
                                where                                       
                                      ((codigo == 0 || tareasEspeciales.CodigoPago == codigo))
                                      && ((pendientes == false || tareasEspeciales.Estado == Datos.Constantes.const_estado_tareaEspecial_pendiente))
                                      && ((tareasEspeciales.Destinatario == codigoUsuario || tareasEspeciales.Remitente == codigoUsuario))         
                                      && proyectos.codOperador == _codOperador
                                select new TareaEspecialDTO
                                {
                                    Id = tareasEspeciales.Id_tareaEspecial,
                                    CodigoPago = tareasEspeciales.CodigoPago,
                                    CodigoProyecto = pagos.CodProyecto.GetValueOrDefault(),
                                    NombreEstado = estado.Descripcion,
                                    Historia = db.HistoriaTareaEspecials
                                                    .Where(filter => filter.IdTareaEspecialInterventoria == tareasEspeciales.Id_tareaEspecial)
                                                    .OrderBy(orderFilter => orderFilter.FechaCreacion)
                                                    .FirstOrDefault(),
                                    FechaInicio = tareasEspeciales.FechaInicio,
                                    CodigoDestinatario = contactoRemitente.Id_Contacto,
                                    NombreRemitente = contactoRemitente.Nombres + " " + contactoRemitente.Apellidos,
                                    CodigoRemitente = contactoDestinatario.Id_Contacto,
                                    NombreDestinatario = contactoDestinatario.Nombres + " " + contactoDestinatario.Apellidos,
                                    Estado = estado.Id_estadoTareaEspecial,
                                    NombreProyecto = proyectos.NomProyecto,
                                    HasUpdates = HasUpdatesTareaEspecialByTareaIdAndUser(tareasEspeciales.Id_tareaEspecial, codigoUsuario)
                                });

                entities = entities.Skip(startIndex).Take(maxRows);

                return entities.ToList();
            }
        }

        public static bool HasUpdatesTareaEspecialByTareaIdAndUser(int codigoTarea, int codigoUsuarioDestinatario)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from historia in db.HistoriaTareaEspecials
                              where historia.IdTareaEspecialInterventoria == codigoTarea
                                    && historia.Destinatario == codigoUsuarioDestinatario
                                    && historia.FechaLecturaDestinatario == null
                              orderby historia.FechaCreacion descending
                              select historia).Any();

                return entity;
            }
        }

        public static void MarkAsReadByTarea(int codigoTarea, int codigoUsuarioDestinatario)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from historia in db.HistoriaTareaEspecials
                                where historia.IdTareaEspecialInterventoria == codigoTarea
                                      && historia.Destinatario == codigoUsuarioDestinatario
                                      && historia.FechaLecturaDestinatario == null
                                orderby historia.FechaCreacion descending
                                select historia).ToList();

                foreach (var historia in entities)
                {
                    historia.FechaLecturaDestinatario = DateTime.Now;
                }

                if (entities.Any())
                    db.SubmitChanges();
            }
        }

        public static bool TieneTareasPendientes(int codigoUsuario)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from tareasEspeciales in db.TareaEspecialInterventorias
                                join pagos in db.PagoActividad on tareasEspeciales.CodigoPago equals pagos.Id_PagoActividad
                                join proyectos in db.Proyecto on pagos.CodProyecto equals proyectos.Id_Proyecto
                                join contactoRemitente in db.Contacto on tareasEspeciales.Remitente equals contactoRemitente.Id_Contacto
                                join contactoDestinatario in db.Contacto on tareasEspeciales.Remitente equals contactoDestinatario.Id_Contacto
                                join estado in db.EstadoTareaEspecialInterventorias on tareasEspeciales.Estado equals estado.Id_estadoTareaEspecial
                                where
                                    (tareasEspeciales.Destinatario == codigoUsuario || tareasEspeciales.Remitente == codigoUsuario)
                                    && tareasEspeciales.Estado == Datos.Constantes.const_estado_tareaEspecial_pendiente                                      
                                select tareasEspeciales
                                ).Any();

                return entities;
            }
        }
    }

    public class TareaEspecialDTO
    {
        public int Id { get; set; }
        public int CodigoPago { get; set; }
        public int CodigoProyecto { get; set; }
        public string NombreProyecto { get; set; }
        public string CodigoYNombreProyecto {
            get {
                return CodigoProyecto + "-" + NombreProyecto; }
            set { }
        }
        public Datos.HistoriaTareaEspecial Historia { get; set; }
        public string Descripcion { get { return Historia != null ? Historia.Observacion : String.Empty; } set { } }
        public string Archivo { get { return Historia != null ? Historia.Archivo : String.Empty; } set { } }
        public bool HasFile { get { return !String.IsNullOrEmpty(Archivo); } set { } }
        public DateTime FechaInicio { set; get; }
        public string FechaReintegroWithFormat
        {
            get
            {
                return FechaInicio.getFechaAbreviadaConFormato(false);
            }
            set { }
        }
        public int CodigoRemitente { get; set; }
        public string NombreRemitente { get; set; }
        public int CodigoDestinatario { get; set; }
        public string NombreDestinatario { get; set; }
        public int Estado { get; set; }
        public String NombreEstado { get; set; }
        public bool HasUpdates { get; set; }
    }

    public class HistoriaTareaDTO
    {
        public string Observacion { get; set; }               
        public string Archivo { get; set; }
        public bool HasFile { get { return !String.IsNullOrEmpty(Archivo); } set { } }
        public DateTime FechaCreacion { get; set; }
        public string FechaCreacionWithFormat
        {
            get
            {
                return FechaCreacion.getFechaAbreviadaConFormato(true);
            }
            set { }
        }
        public int codigoContactoRemitente { get; set; }
        public String NombreRemitente { get; set; }

        public DateTime? FechaLectura { get; set; }
        
        public bool FueLeido {
            get {
                return FechaLectura != null;
            }
            set { }
        }

        public int codigoContactoDestinatario { get; set; }
        public int CodigoUsuarioConsulta { get; set; }

        public bool IsNewMessage {
            get {
                return !FueLeido && codigoContactoDestinatario == CodigoUsuarioConsulta;
            }
            set { }
        }
    }    
}