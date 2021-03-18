using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Datos.DataType;
using Datos;
using Fonade.Negocio.Utility;
using System.Data.Linq;
using System.IO;

namespace Fonade.Negocio.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimientos
{
    public class ActaSeguimiento
    {

        public static List<EmprendedorDTO> GetEmprendedoresYEquipoTrabajo(int idProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var query = (from con in db.Contacto
                             join proCon in db.ProyectoContactos
                             on con.Id_Contacto equals proCon.CodContacto
                             join rol in db.Rols
                             on proCon.CodRol equals rol.Id_Rol
                             join ciu in db.Ciudad on con.CodCiudad equals ciu.Id_Ciudad into ciudades
                             from ciudadEquipo in ciudades.DefaultIfEmpty()
                             join dep in db.departamento on ciudadEquipo.CodDepartamento equals dep.Id_Departamento into departamentos
                             from departamentoEquipo in departamentos.DefaultIfEmpty()
                             where
                                (rol.Id_Rol == Constantes.CONST_RolEmprendedor)
                                && proCon.CodProyecto == idProyecto
                                && proCon.FechaFin == null
                                && proCon.Inactivo.Equals(false)
                             orderby rol.Id_Rol descending
                             select new EmprendedorDTO()
                             {
                                 Identificacion = con.Identificacion,
                                 Nombres = con.Nombres + " " + con.Apellidos,
                                 Telefono = con.Telefono,
                                 Email = con.Email
                             }).ToList();

                return query;
            }
        }

        public static void InsertOrUpdateActa(ActaSeguimientoInterventoria entity)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var currentEntity = (from actas in db.ActaSeguimientoInterventoria
                                     where actas.IdProyecto == entity.IdProyecto
                                           && actas.NumeroActa == entity.NumeroActa
                                     select actas
                                     ).FirstOrDefault();

                if (currentEntity != null)
                {
                    currentEntity.Nombre = entity.Nombre;
                    currentEntity.FechaActualizacion = DateTime.Now;
                    currentEntity.Publicado = entity.Publicado;
                    currentEntity.ArchivoActa = entity.ArchivoActa;
                    currentEntity.FechaCreacion = entity.FechaCreacion;
                    if (entity.Publicado)
                        currentEntity.FechaPublicacion = DateTime.Now;
                    db.SubmitChanges();
                    entity.Id = currentEntity.Id;
                }
                else
                {
                    db.ActaSeguimientoInterventoria.InsertOnSubmit(entity);
                    db.SubmitChanges();
                }
            }
        }

        public static Datos.ActaSeguimientoInterventoria GetActaById(int idActa, int _idProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                DataLoadOptions options = new DataLoadOptions();
                options.LoadWith<ActaSeguimientoInterventoria>(p => p.TipoActaSeguimiento);
                options.LoadWith<ActaSeguimientoInterventoria>(p => p.Contacto);
                db.LoadOptions = options;

                return db.ActaSeguimientoInterventoria.FirstOrDefault(
                    filter =>
                filter.NumeroActa == idActa && filter.IdProyecto == _idProyecto);
            }
        }

        public static List<ActaSeguimientoDTO> GetActasByProyecto(int idProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = db.ActaSeguimientoInterventoria.Where(filter => filter.IdProyecto == idProyecto).ToList();

                return entities.Select(filter => new ActaSeguimientoDTO
                {
                    Id = filter.NumeroActa,
                    Nombre = filter.Nombre,
                    Tipo = filter.TipoActaSeguimiento,
                    FechaCreacion = filter.FechaCreacion,
                    FechaPublicacion = filter.FechaPublicacion,
                    Publicado = filter.Publicado,
                    ArchivoActa = filter.ArchivoActa,
                    IdProyecto = filter.IdProyecto,
                    Numero = filter.NumeroActa
                }).ToList();
            }
        }

        public static List<InfoActaSeguimientoDTO> GetInfoActasByProyecto(int idProyecto)
        {
            List<InfoActaSeguimientoDTO> list = new List<InfoActaSeguimientoDTO>();

            using (FonadeDBDataContext db = new FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                list = (from a in db.ActaSeguimientoInterventoria
                        where a.IdProyecto == idProyecto
                        orderby a.NumeroActa
                        select new InfoActaSeguimientoDTO
                        {
                            idActa = a.Id,
                            Acta = a.NumeroActa == 0 ? a.Nombre : a.Nombre + " " + a.NumeroActa,
                            codContactoInterventor = a.IdUsuarioCreacion,
                            codProyecto = a.IdProyecto,
                            FechaActualizacion = a.FechaActualizacion,
                            FechaPublicacion = a.FechaPublicacion,
                            FechaCreacion = a.FechaCreacion,
                            Publicado = a.Publicado == true ? "Si" : "No",
                            actaPublicada = a.Publicado,
                            nombreInterventor = a.Contacto.Nombres + " " + a.Contacto.Apellidos
                        }).ToList();
            }

            return list;
        }

        public static void InsertActaContrato(int codigoProyecto, string fileName, int idContacto)
        {
            string _nombreArchivo = Path.GetFileName(fileName);
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var currentActa = db.ContratosArchivosAnexos
                                    .FirstOrDefault
                                    (filter =>
                                        filter.CodProyecto == codigoProyecto
                                        && filter.NombreArchivo.Contains(_nombreArchivo)
                                    );
                if (currentActa != null)
                {
                    currentActa.NombreArchivo = fileName;
                    currentActa.FechaIngreso = DateTime.Now;
                    db.SubmitChanges();
                }
                else
                {
                    var archivoContrato = new ContratosArchivosAnexo
                    {
                        CodProyecto = codigoProyecto,
                        NombreArchivo = Path.GetFileName(fileName),
                        ruta = fileName,
                        CodContacto = idContacto,
                        FechaIngreso = DateTime.Now
                    };
                    db.ContratosArchivosAnexos.InsertOnSubmit(archivoContrato);
                    db.SubmitChanges();
                }

            }
        }

        public static List<InterventoresProyecto> interventoresPorProyecto(int _codCoordinador)
        {
            string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            List<InterventoresProyecto> list = new List<InterventoresProyecto>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                list = (from e in db.MD_ActasSeguimientoInterventor(_codCoordinador)
                        select new InterventoresProyecto
                        {
                            codProyecto = e.IdProyecto,
                            idContacto = e.codInterventor,
                            nombreInterventor = e.Interventor
                        }).ToList();
            }

            return list;
        }

        public static bool DespublicarActa(int _idActa)
        {
            bool despublicado = false;

            string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                var actaSeg = (from a in db.ActaSeguimientoInterventoria
                               where a.Id == _idActa
                               select a).FirstOrDefault();

                actaSeg.Publicado = false;
                db.SubmitChanges();

                despublicado = true;
            }

            return despublicado;
        }

        public static bool InsertJustActaDespublicada(int _codContacto, string _Justificacion)
        {
            bool despublicado = false;

            string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                ActaSeguimientoDespublicada acta = new ActaSeguimientoDespublicada
                {
                    codContacto = _codContacto,
                    fechaActaDespublicada = DateTime.Now,
                    Justificacion = _Justificacion
                };

                db.ActaSeguimientoDespublicada.InsertOnSubmit(acta);
                db.SubmitChanges();

                despublicado = true;
            }

            return despublicado;
        }
    }

    public class InterventoresProyecto
    {
        public int idContacto { get; set; }
        public string nombreInterventor { get; set; }
        public int codProyecto { get; set; }
    }

    public class EmprendedorDTO
    {
        public double Identificacion { get; set; }
        public string Nombres { get; set; }

        public string Telefono { get; set; }
        public string Email { get; set; }
    }

    public class InfoActaSeguimientoDTO
    {
        public int idActa { get; set; }
        public int codProyecto { get; set; }
        public int codContactoInterventor { get; set; }
        public string nombreInterventor { get; set; }
        public string Acta { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public string Publicado { get; set; }
        public bool actaPublicada { get; set; }
    }

    public class ActaSeguimientoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public Datos.TipoActaSeguimiento Tipo { get; set; }
        public int IdProyecto { get; set; }
        public int Numero { get; set; }
        public string NombreTipo
        {
            get { return Tipo.Tipo; }
            set { }
        }
        public DateTime FechaCreacion { get; set; }
        public string FechaCreacionWithFormat
        {
            get
            {
                return FechaCreacion.getFechaAbreviadaConFormato(true);
            }
            set { }
        }
        public DateTime? FechaPublicacion { get; set; }
        public string FechaPublicacionWithFormat
        {
            get
            {
                if (FechaPublicacion != null)
                    return FechaPublicacion.GetValueOrDefault().getFechaAbreviadaConFormato(true);
                else
                    return string.Empty;
            }
            set { }
        }
        public bool Publicado { get; set; }
        public string ArchivoActa { get; set; }
        public bool HasActaCharged
        {
            get
            {
                return !String.IsNullOrEmpty(ArchivoActa);
            }
            set { }
        }
    }
}
