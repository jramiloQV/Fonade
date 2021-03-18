using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.PlanDeNegocioV2.Administracion.Interventoria
{
    public class Abogado
    {
        public static void Insert(Datos.ContratoEmpresa currentEntity)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from contratos in db.ContratoEmpresas
                              join empresas in db.Empresas on contratos.CodEmpresa equals empresas.id_empresa
                              where empresas.codproyecto == currentEntity.CodEmpresa
                              orderby empresas.id_empresa descending
                              select contratos
                               ).FirstOrDefault();

                if (entity != null)
                {
                    entity.NumeroContrato = currentEntity.NumeroContrato;
                    entity.ObjetoContrato = currentEntity.ObjetoContrato;
                    entity.FechaAP = currentEntity.FechaAP;
                    entity.ValorInicialEnPesos = currentEntity.ValorInicialEnPesos;
                    entity.PlazoContratoMeses = currentEntity.PlazoContratoMeses;
                    entity.NumeroAPContrato = currentEntity.NumeroAPContrato;
                    entity.NumeroActaConcejoDirectivo = currentEntity.NumeroActaConcejoDirectivo;
                    entity.FechaActaConcejoDirectivo = currentEntity.FechaActaConcejoDirectivo;
                    entity.ValorEnte = currentEntity.ValorEnte;
                    entity.Valorsena = currentEntity.Valorsena;
                    entity.CertificadoDisponibilidad = currentEntity.CertificadoDisponibilidad;
                    entity.FechaCertificadoDisponibilidad = currentEntity.FechaCertificadoDisponibilidad;
                    entity.Estado = currentEntity.Estado;
                    entity.TipoContrato = currentEntity.TipoContrato;
                    db.SubmitChanges();
                }
                else
                {
                    var empresa = db.Empresas.Where(filter => filter.codproyecto == currentEntity.CodEmpresa)
                                             .OrderByDescending(orderFilter => orderFilter.id_empresa)
                                             .FirstOrDefault();

                    if (empresa == null)
                        throw new Exception("No se logro encontrar los datos de la empresa.");

                    var newEntity = new Datos.ContratoEmpresa
                    {
                        NumeroContrato = currentEntity.NumeroContrato,
                        ObjetoContrato = currentEntity.ObjetoContrato,
                        FechaAP = currentEntity.FechaAP,
                        ValorInicialEnPesos = currentEntity.ValorInicialEnPesos,
                        PlazoContratoMeses = currentEntity.PlazoContratoMeses,
                        NumeroAPContrato = currentEntity.NumeroAPContrato,
                        NumeroActaConcejoDirectivo = currentEntity.NumeroActaConcejoDirectivo,
                        FechaActaConcejoDirectivo = currentEntity.FechaActaConcejoDirectivo,
                        ValorEnte = currentEntity.ValorEnte,
                        Valorsena = currentEntity.Valorsena,
                        CertificadoDisponibilidad = currentEntity.CertificadoDisponibilidad,
                        FechaCertificadoDisponibilidad = currentEntity.FechaCertificadoDisponibilidad,
                        Estado = currentEntity.Estado,
                        CodEmpresa = empresa.id_empresa,
                        TipoContrato = currentEntity.TipoContrato
                    };

                    db.ContratoEmpresas.InsertOnSubmit(newEntity);
                    db.SubmitChanges();
                }
            }
        }

        public static bool ExistContrato(string codigo, int idProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from contratos in db.ContratoEmpresas
                              join empresas in db.Empresas on contratos.CodEmpresa equals empresas.id_empresa
                              where empresas.codproyecto != idProyecto
                                    && contratos.NumeroContrato == codigo.Trim()
                              orderby empresas.id_empresa descending
                              select contratos).Any();

                return entity;                
            }
        }

        public static List<ContratoEmpresaDTO> GetContratoByProyecto(int codigoProyecto, int? _codOperador)
        {
            Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);

            List<ContratoEmpresaDTO> entities = new List<ContratoEmpresaDTO>();

            ContratoEmpresaDTO contratoDTO = new ContratoEmpresaDTO();

            //entities = (from empresas in db.Empresas
            //            join proyectos in db.Proyectos on empresas.codproyecto equals proyectos.Id_Proyecto
            //            where empresas.codproyecto == codigoProyecto
            //            orderby empresas.id_empresa descending
            //            select new ContratoEmpresaDTO
            //            {
            //                CodigoProyecto = codigoProyecto,
            //                Contrato = db.ContratoEmpresas.FirstOrDefault(filter => filter.CodEmpresa == empresas.id_empresa),
            //                NombreProyecto = proyectos.NomProyecto
            //            }).ToList();

            var emp = (from e in db.Empresas
                       where e.codproyecto == codigoProyecto
                       select e).FirstOrDefault();

            var proy = (from p in db.Proyecto
                        where p.Id_Proyecto == codigoProyecto
                        select p).FirstOrDefault();

            var contrato = (from c in db.ContratoEmpresas
                            where c.CodEmpresa == emp.id_empresa
                            select c).FirstOrDefault();

            contratoDTO.CodigoProyecto = codigoProyecto;
            contratoDTO.Contrato = contrato;
            contratoDTO.NombreProyecto = proy.NomProyecto;
            contratoDTO.codOperador = proy.codOperador;

            if (_codOperador != null)
            {
                if (proy.codOperador == _codOperador)
                {
                    entities.Add(contratoDTO);
                }
            }
            else
            {
                entities.Add(contratoDTO);
            }

            if (entities.Count>0)
            {
                entities = buscarOperador(entities);
            }

            return entities;
        }

        public static List<ContratoEmpresaDTO> GetContratoLast50(int? _codOperador)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                List<ContratoEmpresaDTO> entities = new List<ContratoEmpresaDTO>();

                if (_codOperador == null)
                {
                    entities = (from empresas in db.Empresas
                                join proyectos in db.Proyecto on empresas.codproyecto equals proyectos.Id_Proyecto
                                join contratoMasivo in db.ContratoEmpresas on empresas.id_empresa equals contratoMasivo.CodEmpresa
                                where contratoMasivo.Estado != null
                                select new ContratoEmpresaDTO
                                {
                                    CodigoProyecto = empresas.codproyecto.GetValueOrDefault(0),
                                    Contrato = db.ContratoEmpresas.FirstOrDefault(filter => filter.CodEmpresa == empresas.id_empresa),
                                    NombreProyecto = proyectos.NomProyecto,
                                    codOperador = proyectos.codOperador
                                }
                             ).Take(50)
                              .ToList()
                              .OrderByDescending(orderFilter => orderFilter.Contract)
                              .ToList();
                }
                else
                {
                    entities = (from empresas in db.Empresas
                                join proyectos in db.Proyecto on empresas.codproyecto equals proyectos.Id_Proyecto
                                join contratoMasivo in db.ContratoEmpresas on empresas.id_empresa equals contratoMasivo.CodEmpresa
                                where contratoMasivo.Estado != null && proyectos.codOperador == _codOperador
                                select new ContratoEmpresaDTO
                                {
                                    CodigoProyecto = empresas.codproyecto.GetValueOrDefault(0),
                                    Contrato = db.ContratoEmpresas.FirstOrDefault(filter => filter.CodEmpresa == empresas.id_empresa),
                                    NombreProyecto = proyectos.NomProyecto,
                                    codOperador = proyectos.codOperador
                                }
                            ).Take(50)
                             .ToList()
                             .OrderByDescending(orderFilter => orderFilter.Contract)
                             .ToList();
                }

                entities = buscarOperador(entities);

                return entities;                
            }
        }

        private static List<ContratoEmpresaDTO> buscarOperador(List<ContratoEmpresaDTO> contratoEmpresas)
        {
            using (Datos.FonadeDBLightDataContext dbLight = new Datos.FonadeDBLightDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                foreach (var e in contratoEmpresas)
                {
                    e.Operador = (from o in dbLight.Operador
                                  where o.IdOperador == e.codOperador
                                  select o.NombreOperador).FirstOrDefault();
                }
            }

            return contratoEmpresas;
        }

        public static int GetProrroga(int codigoProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var prorroga = 12;
                Int32? entities = db.ProyectoProrrogas
                                 .Where(filter => filter.CodProyecto == codigoProyecto)                                 
                                 .Sum(summatory => (Int32?)summatory.Prorroga);

                //return entities != null ? prorroga + entities : prorroga;  
                return prorroga + entities.GetValueOrDefault(0);
            }
        }

        public static List<ContratoEmpresaDTO> GetContratoByContratoNumber(string codigoContrato, int? _codOperador)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                List<ContratoEmpresaDTO> entities = new List<ContratoEmpresaDTO>();

                if (_codOperador == null)
                {
                    entities = (from empresas in db.Empresas
                                join contratos in db.ContratoEmpresas on empresas.id_empresa equals contratos.CodEmpresa
                                join proyectos in db.Proyecto on empresas.codproyecto equals proyectos.Id_Proyecto
                                where contratos.NumeroContrato == codigoContrato
                                orderby empresas.id_empresa descending
                                select new ContratoEmpresaDTO
                                {
                                    CodigoProyecto = proyectos.Id_Proyecto,
                                    Contrato = contratos,
                                    NombreProyecto = proyectos.NomProyecto,
                                    codOperador = proyectos.codOperador
                                }
                            ).ToList();
                }
                else
                {
                    entities = (from empresas in db.Empresas
                                join contratos in db.ContratoEmpresas on empresas.id_empresa equals contratos.CodEmpresa
                                join proyectos in db.Proyecto on empresas.codproyecto equals proyectos.Id_Proyecto
                                where contratos.NumeroContrato == codigoContrato
                                && proyectos.codOperador == _codOperador
                                orderby empresas.id_empresa descending
                                select new ContratoEmpresaDTO
                                {
                                    CodigoProyecto = proyectos.Id_Proyecto,
                                    Contrato = contratos,
                                    NombreProyecto = proyectos.NomProyecto,
                                    codOperador = proyectos.codOperador
                                }
                            ).ToList();
                }

                if (entities.Count>0)
                {
                    entities = buscarOperador(entities);
                }

                return entities;
            }
        }

        public static void UpdateExtension(Datos.ContratoEmpresa currentEntity)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from contratos in db.ContratoEmpresas
                              join empresas in db.Empresas on contratos.CodEmpresa equals empresas.id_empresa
                              where empresas.id_empresa == currentEntity.CodEmpresa
                              orderby empresas.id_empresa descending
                              select contratos
                               ).FirstOrDefault();

                if (entity != null)
                {
                    entity.NumeroContrato = currentEntity.NumeroContrato;
                    entity.FechaFirmaDelContrato = currentEntity.FechaFirmaDelContrato;                              
                    entity.CertificadoDisponibilidad = currentEntity.CertificadoDisponibilidad;
                    entity.FechaCertificadoDisponibilidad = currentEntity.FechaCertificadoDisponibilidad;
                    entity.NumeroActaConcejoDirectivo = currentEntity.NumeroActaConcejoDirectivo;            
                    entity.ValorEnte = currentEntity.ValorEnte;
                    entity.Valorsena = currentEntity.Valorsena;
                    entity.NumeroPoliza = currentEntity.NumeroPoliza;
                    entity.ValorInicialEnPesos = currentEntity.ValorInicialEnPesos;
                    entity.TipoContrato = currentEntity.TipoContrato;
                    entity.Estado = currentEntity.Estado;
                    entity.FechaActaConcejoDirectivo = currentEntity.FechaActaConcejoDirectivo;

                    db.SubmitChanges();
                }
                else
                {
                    throw new Exception("No se logro encontrar los datos de la empresa.");                    
                }
            }
        }

        public static void InsertLog(string dataLog) {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var log = new Datos.LogCargueMasicoContrato()
                {
                    DataLog = dataLog
                };

                db.LogCargueMasicoContratos.InsertOnSubmit(log);
                db.SubmitChanges();
            }
        }

    }

    public class ContratoEmpresaDTO {
        public int CodigoProyecto { get; set; }
        public string NombreProyecto { get; set; }
        public string Operador { get; set; }
        public int ? codOperador { get; set; }
        public Datos.ContratoEmpresa Contrato { get; set; }
        public bool HasInfoCompleted {
            get {
                if (Contrato != null)
                {
                    if (!string.IsNullOrEmpty(Contrato.NumeroContrato) && Contrato.NumeroActaConcejoDirectivo != null)
                        return true;
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            } set { }
        }
        public string NumeroContrato {
            get {
                return Contrato != null ? Contrato.NumeroContrato : "Sin Contrato";
            }
            set { }
        }

        public Int64 Contract { get
            {
                Int64 number = 0;
                if (Int64.TryParse(NumeroContrato, out number))
                    return number;
                else
                    return 0;
            } set
            {

            }
        }
    }
}
