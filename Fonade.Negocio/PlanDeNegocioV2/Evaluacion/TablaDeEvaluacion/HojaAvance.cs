using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Datos;

namespace Fonade.Negocio.PlanDeNegocioV2.Evaluacion.TablaDeEvaluacion
{
    public class HojaAvance
    {

        public static bool InsertarAvance(EvaluacionSeguimientoV2 entity, out string msg)
        {
            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    var entitydb = (from row in db.EvaluacionSeguimientoV2s
                                    where row.IdProyecto == entity.IdProyecto &&
                                    row.IdConvocatoria == entity.IdConvocatoria
                                    select row).FirstOrDefault();

                    //insert-update
                    if (entitydb == null)
                        db.EvaluacionSeguimientoV2s.InsertOnSubmit(entity);
                    else
                    {
                        entitydb.Antecedentes = entity.Antecedentes;
                        entitydb.Competencia = entity.Competencia;
                        entitydb.CondicionesComercializacion = entity.CondicionesComercializacion;
                        entitydb.EquipoTrabajo = entity.EquipoTrabajo;
                        entitydb.EstrategiasComercializacion = entity.EstrategiasComercializacion;
                        entitydb.FechaActualizacion = entity.FechaActualizacion;
                        entitydb.FuerzaMercado = entity.FuerzaMercado;
                        entitydb.IdContacto = entity.IdContacto;
                        entitydb.IdentificacionMercado = entity.IdentificacionMercado;
                        entitydb.IndicadoresGestion = entity.IndicadoresGestion;
                        entitydb.IndicadoresSeguimiento = entity.IndicadoresSeguimiento;
                        entitydb.IndiceRentabilidad = entity.IndiceRentabilidad;
                        entitydb.LecturaPlan = entity.LecturaPlan;
                        entitydb.Modelo = entity.Modelo;
                        entitydb.NecesidadClientes = entity.NecesidadClientes;
                        entitydb.Normatividad = entity.Normatividad;
                        entitydb.OperacionNegocio = entity.OperacionNegocio;
                        entitydb.PeriodoImproductivo = entity.PeriodoImproductivo;
                        entitydb.PlanOperativo = entity.PlanOperativo;
                        entitydb.PlanOperativo2 = entity.PlanOperativo2;
                        entitydb.PropuestaValor = entity.PropuestaValor;
                        entitydb.Riesgos = entity.Riesgos;
                        entitydb.SolicitudInformacion = entity.SolicitudInformacion;
                        entitydb.Sostenibilidad = entity.Sostenibilidad;
                        entitydb.TendenciasMercado = entity.TendenciasMercado;
                        entitydb.ValidacionMercado = entity.ValidacionMercado;
                        entitydb.Viabilidad = entity.Viabilidad;
                        entitydb.InformeEvaluacion = entity.InformeEvaluacion;
                    }
                    db.SubmitChanges();

                    msg = Mensajes.Mensajes.GetMensaje(8);
                    return true;
                }
            }
            catch (Exception ex)
            {
                //todo guardar log
                msg = Mensajes.Mensajes.GetMensaje(7);
                return false;
            }
        }

        public static EvaluacionSeguimientoV2 GetAvance(EvaluacionSeguimientoV2 entity)
        {
            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    var entitydb = (from row in db.EvaluacionSeguimientoV2s
                                    where row.IdProyecto == entity.IdProyecto &&
                                    row.IdConvocatoria == entity.IdConvocatoria
                                    select row).FirstOrDefault();

                    db.SubmitChanges();

                    return entitydb;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<MD_HojaAvanceEvaluacionV2Result> GetEvaluacionSeguimiento(int IdOpcion,int IdCoordinadorEval, out string mensaje)
        {

            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    mensaje = "true";
                    return db.MD_HojaAvanceEvaluacionV2(IdCoordinadorEval,IdOpcion).ToList();

                }
            }
            catch (Exception ex)
            {
                mensaje = string.Format("se presento un error obteniendo la hoja de avance : {0}, de tipo : {1}", ex.Message, ex.StackTrace);
                return null;
            }

        }


    }
}
