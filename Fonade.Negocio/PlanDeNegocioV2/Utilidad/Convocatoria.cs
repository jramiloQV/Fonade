using System;
using System.Linq;
using System.Web.UI.WebControls;
using Datos;
using System.Globalization;
using System.Web;
using System.Collections.Generic;

namespace Fonade.Negocio.PlanDeNegocioV2.Utilidad
{
    public static class Convocatoria
    {                               
        public static Boolean ConvocatoriaExist(int codigoConvocatoria)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                return db.Convocatoria.Any(filter => filter.Id_Convocatoria.Equals(codigoConvocatoria));
            }
        }

        public static bool VerificarVersionConvocatoria(int codigoConvocatoria, int codigoProyecto)
        {
            var codigoVersionConvocatoria = VersionConvocatoria(codigoConvocatoria);
            var codigoVersionProyecto = ProyectoGeneral.VersionProyecto(codigoProyecto);

            return codigoVersionConvocatoria.Equals(codigoVersionProyecto);
        }

        public static int VersionConvocatoria(int codigoConvocatoria)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from versionProyecto in db.Convocatoria
                              where versionProyecto.Id_Convocatoria.Equals(codigoConvocatoria)
                              select versionProyecto.IdVersionProyecto).SingleOrDefault();

                return entity != null ? entity.GetValueOrDefault(1) : 1;
            }
        }

        public static int? GetConvocatoriaByProyecto(int codigoProyecto, int codigoConvocatoriaHistorial)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {               
                if (codigoConvocatoriaHistorial != 0) {

                    int? entity = (from convocatorias in db.ConvocatoriaProyectos
                                   where convocatorias.CodProyecto.Equals(codigoProyecto)
                                         && convocatorias.CodConvocatoria.Equals(codigoConvocatoriaHistorial)
                                   orderby convocatorias.CodConvocatoria descending
                                   select convocatorias.CodConvocatoria
                              ).FirstOrDefault();

                    return entity;
                }
                else
                {
                    int? entity = (from convocatorias in db.ConvocatoriaProyectos
                                   where convocatorias.CodProyecto.Equals(codigoProyecto)                                         
                                   orderby convocatorias.CodConvocatoria descending
                                   select convocatorias.CodConvocatoria
                              ).FirstOrDefault();

                    return entity;
                }             
            }
        } 

        public static Datos.Convocatoria GetConvocatoriaDetails(int codigoConvocatoria) {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from convocatorias in db.Convocatoria
                               where convocatorias.Id_Convocatoria.Equals(codigoConvocatoria)                               
                               select convocatorias
                              ).FirstOrDefault();

                return entity;
            }
        }

        public static Boolean ExistAspectoByConvocatoria(int codigoConvocatoria, int codigoAspecto) {

            if (codigoAspecto == Constantes.Const_AspectoResumenEjecutivoV2New)
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    codigoAspecto = (int)(from C in db.Campo
                                   join CC in db.ConvocatoriaCampos
                                   on C.id_Campo equals CC.codCampo
                                   where CC.codConvocatoria == codigoConvocatoria
                                   && C.Campo1.Contains("Resumen")
                                   orderby C.id_Campo descending
                                   select C.id_Campo).FirstOrDefault();
                }
            }

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                return db.ConvocatoriaCampos.Any(filter => filter.codConvocatoria.Equals(codigoConvocatoria)
                                                                 && filter.codCampo.Equals(codigoAspecto)
                                                      );
            }
        }

        public static void UpdateTopeConvocatoria(int codigoConvocatoria, int topeConvocatoria)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = db.Convocatoria.Single(filter => filter.Id_Convocatoria.Equals(codigoConvocatoria));
                entity.TopeConvocatoria = Convert.ToInt16(topeConvocatoria);

                db.SubmitChanges();
            }
        }

        public static int GetTopeConvocatoria(int codigoConvocatoria)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = db.Convocatoria.Single(filter => filter.Id_Convocatoria.Equals(codigoConvocatoria));
                
                return Convert.ToInt32(entity.TopeConvocatoria.GetValueOrDefault(0));
            }
        }

        public static Int32 GetPresupuestoFormalizado(int codigoConvocatoria)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var salariosSolicitados = (from proyectosFormalizados in db.ProyectoFormalizacions
                              join proyectoConvocatoria in db.Proyecto on proyectosFormalizados.codProyecto equals proyectoConvocatoria.Id_Proyecto
                              join valoresSolicitados in db.ProyectoFinanzasIngresos on proyectoConvocatoria.Id_Proyecto equals valoresSolicitados.CodProyecto
                              where
                                proyectosFormalizados.CodConvocatoria.Equals(codigoConvocatoria)
                              select
                                valoresSolicitados.Recursos
                              ).Sum(sumatory => (Int32?) sumatory) ?? 0;
                
                var totales = ( from Convocatorias in db.Convocatoria
                                join SalarioVigente in db.SalariosMinimos on Convocatorias.FechaInicio.Year equals SalarioVigente.AñoSalario
                                where Convocatorias.Id_Convocatoria.Equals(codigoConvocatoria)                                    
                                select   
                                    Convert.ToInt32( (Convocatorias.Presupuesto / SalarioVigente.SalarioMinimo) * Convocatorias.TopeConvocatoria.GetValueOrDefault(0) / 100)
                              ).FirstOrDefault();

                return totales - salariosSolicitados;
            }
        }

        public static List<Datos.Convocatoria>  GetConvocatorias(int? _codOperador)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                if (_codOperador != null) {
                    var entities = (from convocatorias in db.Convocatoria
                                    where convocatorias.codOperador == _codOperador
                                    orderby convocatorias.FechaFin descending
                                    select convocatorias
                             ).ToList();

                    return entities;
                }
                else
                {
                    var entities = (from convocatorias in db.Convocatoria                                    
                                    orderby convocatorias.FechaFin descending
                                    select convocatorias
                             ).ToList();

                    return entities;
                }
            }
        }

        public static List<Datos.sp_presupuestoDisponiblePorConvocatoriaResult> GetPresupuestoPorConvocatoria(int codigoConvocatoria)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                return db.sp_presupuestoDisponiblePorConvocatoria(codigoConvocatoria).ToList();
            }
        }

        public static decimal GetSalarioMinimoConvocatoria(int codigoConvocatoria)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var Presupuesto = (from Convocatorias in db.Convocatoria
                                   join SalarioVigente in db.SalariosMinimos on Convocatorias.FechaInicio.Year equals SalarioVigente.AñoSalario
                                   where Convocatorias.Id_Convocatoria == codigoConvocatoria
                                   select SalarioVigente.SalarioMinimo).FirstOrDefault();

                return Presupuesto;
            }
        }

        public static double GetRecursosAprobados(int codigoProyecto,int codigoConvocatoria)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from recursos in db.EvaluacionObservacions                                                                                                         
                                   where recursos.CodConvocatoria == codigoConvocatoria
                                         && recursos.CodProyecto == codigoProyecto
                                   select recursos.ValorRecomendado).FirstOrDefault();

                return entity.GetValueOrDefault(0);
            }
        }       
    }   
}
