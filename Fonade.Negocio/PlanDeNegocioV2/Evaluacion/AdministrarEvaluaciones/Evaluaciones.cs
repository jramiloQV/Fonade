using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fonade.Negocio.PlanDeNegocioV2.Utilidad;
using Fonade.Negocio.PlanDeNegocioV2.Evaluacion.AdministrarEvaluacion.AdministrarEvaluador;

namespace Fonade.Negocio.PlanDeNegocioV2.Evaluacion.AdministrarEvaluaciones
{
    public class Evaluaciones
    {
        public static List<EvaluacionProyecto> Get(int codigoContacto, int codigoGrupo, int startIndex, int maxRows, int? codOperador, int? codigoProyecto = null)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var query = from proyectos in db.Proyecto
                            join ciudades in db.Ciudad on proyectos.CodCiudad equals ciudades.Id_Ciudad
                            join departamentos in db.departamento on ciudades.CodDepartamento equals departamentos.Id_Departamento
                            where
                                proyectos.Inactivo == false
                                && proyectos.CodEstado == Constantes.CONST_Evaluacion && proyectos.codOperador == codOperador
                            //orderby
                            //    proyectos.Id_Proyecto descending
                            select new EvaluacionProyecto()
                            {
                                CodigoProyecto = proyectos.Id_Proyecto,
                                NombreProyecto = proyectos.NomProyecto,
                                Ciudad = ciudades.NomCiudad,
                                Departamento = departamentos.NomDepartamento,
                                ConvocatoriaProyecto = Utilidad.Convocatoria.GetConvocatoriaDetails(Utilidad.Convocatoria.GetConvocatoriaByProyecto(proyectos.Id_Proyecto, 0).GetValueOrDefault(0)),
                                Avalado = IsAvalado(proyectos.Id_Proyecto),
                                Evaluador = AsignarEvaluador.GetEvaluadorProyecto(proyectos.Id_Proyecto)
                            };

                if (codigoGrupo == Constantes.CONST_Evaluador || codigoGrupo == Constantes.CONST_CoordinadorEvaluador)
                {
                    query = query.Where(filterSelector =>
                                       db.ProyectoContactos.Any(
                                                                 filter => filter.CodProyecto == filterSelector.CodigoProyecto
                                                                   && filter.CodContacto == codigoContacto
                                                                   && filter.Inactivo == (false)
                                                                   && filter.FechaFin == null));
                }

                if (codigoProyecto != null)
                {
                    query = query.Where(filter => filter.CodigoProyecto.Equals(codigoProyecto));
                }

                return query.Skip(startIndex)
                            .Take(maxRows)
                            .ToList(); 
            }
        }

        public static int Count(int codigoContacto, int codigoGrupo, int?codOperador, int? codigoProyecto = null)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var query = from proyectos in db.Proyecto
                            join ciudades in db.Ciudad on proyectos.CodCiudad equals ciudades.Id_Ciudad
                            join departamentos in db.departamento on ciudades.CodDepartamento equals departamentos.Id_Departamento
                            where
                                proyectos.Inactivo == false
                                && proyectos.CodEstado == Constantes.CONST_Evaluacion && proyectos.codOperador == codOperador
                            //orderby
                            //    proyectos.Id_Proyecto descending
                            select new EvaluacionProyecto()
                            {
                                CodigoProyecto = proyectos.Id_Proyecto,                                
                            };

                if (codigoGrupo == Constantes.CONST_Evaluador || codigoGrupo == Constantes.CONST_CoordinadorEvaluador)
                {
                    query = query.Where(filterSelector =>
                                       db.ProyectoContactos.Any(
                                                                 filter => filter.CodProyecto == filterSelector.CodigoProyecto
                                                                   && filter.CodContacto == codigoContacto
                                                                   && filter.Inactivo == (false)
                                                                   && filter.FechaFin == null));
                }

                if (codigoProyecto != null)
                {
                    query = query.Where(filter => filter.CodigoProyecto.Equals(codigoProyecto));
                }

                return query.Count(); ;
            }
        }

        public static bool IsAvalado(int codigoProyecto) {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                return db.MD_obtenerTabs(codigoProyecto).First().codproyecto == codigoProyecto;
            }
        }
    }

    public class EvaluacionProyecto {
        public int CodigoProyecto { get; set; }
        public string NombreProyecto { get; set; }
        public string Ciudad { get; set; }
        public string Departamento { get; set; }
        public Datos.Convocatoria ConvocatoriaProyecto { get; set; }

        public int CodigoConvocatoria { get {
                return ConvocatoriaProyecto != null ? ConvocatoriaProyecto.Id_Convocatoria : 0;
            } set { } }
        public String NombreConvocatoria { get {
                return ConvocatoriaProyecto != null ? ConvocatoriaProyecto.NomConvocatoria : "";
            } set { } }
        public bool Avalado { get; set; }
        public EvaluadorProyecto Evaluador { get; set; }        
    }
}
