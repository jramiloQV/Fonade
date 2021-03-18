using Datos;
using Datos.Modelos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.FonDBLight
{
    public class EvaluacionRiesgoController
    {
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public List<EvaluacionRiesgoModel> GetEvaluacionRiesgoByCodProyectoByCodConvocatoria(int _codProyecto, int _codConvocatoria)
        {
            List<EvaluacionRiesgoModel> listEvalRiegos = new List<EvaluacionRiesgoModel>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                listEvalRiegos = (from e in db.EvaluacionRiesgos
                                  where e.CodProyecto == _codProyecto && e.CodConvocatoria == _codConvocatoria
                                  select new EvaluacionRiesgoModel
                                  {
                                      idRiesgo = e.Id_Riesgo,
                                      codConvocatoria = e.CodConvocatoria,
                                      codProyecto = e.CodProyecto,
                                      Mitigacion = e.Mitigacion,
                                      Riesgo = e.Riesgo
                                  }).ToList();
            }

            return listEvalRiegos;

        }

        public List<EvaluacionRiesgoModel> GetSeguimientoRiesgoByCodProyectoByCodConvocatoria(int _codProyecto, int _codConvocatoria)
        {
            List<EvaluacionRiesgoModel> listEvalRiegos = new List<EvaluacionRiesgoModel>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                listEvalRiegos = (from e in db.ActaSeguimRiesgosEvaluacion
                                  where e.codProyecto == _codProyecto && e.codConvocatoria == _codConvocatoria
                                  && e.Ocultar == false
                                  select new EvaluacionRiesgoModel
                                  {
                                      idRiesgo = e.idRiesgoEvaluacion,
                                      codConvocatoria = e.codConvocatoria,
                                      codProyecto = e.codProyecto,
                                      Mitigacion = e.Mitigacion,
                                      Riesgo = e.Riesgo,
                                      idRiesgoInterventoria = e.idRiesgoInterventoria
                                  }).ToList();
            }

            return listEvalRiegos;

        }

        public void copiarInformacionRiesgos(List<EvaluacionRiesgoModel> evaluacionRiesgos, int _codContacto
                        , int _codConvocatoria, int _codProyecto)
        {
            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                List<ActaSeguimRiesgosEvaluacion> riesgosEvaluacions = new List<ActaSeguimRiesgosEvaluacion>();

                foreach (var er in evaluacionRiesgos)
                {
                    var cant = (from a in db.ActaSeguimRiesgosEvaluacion
                                where a.idRiesgoEvaluacion == er.idRiesgo
                                select a).Count();

                    if (cant==0)
                    {
                        ActaSeguimRiesgosEvaluacion riesgos = new ActaSeguimRiesgosEvaluacion
                        {
                            codContactoModifica = _codContacto,
                            codConvocatoria = _codConvocatoria,
                            codProyecto = _codProyecto,
                            fechaUltimaModificacion = DateTime.Now,
                            idRiesgoEvaluacion = er.idRiesgo,
                            Mitigacion = er.Mitigacion,
                            Riesgo = er.Riesgo,
                            Ocultar = false
                        };

                        riesgosEvaluacions.Add(riesgos);
                    }
                   
                }

                db.ActaSeguimRiesgosEvaluacion.InsertAllOnSubmit(riesgosEvaluacions);
                db.SubmitChanges();
            }
        }

        public bool AddRiesgoInterventoria(EvaluacionRiesgoModel riesgo, int _codcontacto)
        {
            bool ingresado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                ActaSeguimRiesgosEvaluacion r = new ActaSeguimRiesgosEvaluacion
                {
                    codContactoModifica = _codcontacto,
                    codConvocatoria = riesgo.codConvocatoria,
                    codProyecto = riesgo.codProyecto,
                    fechaUltimaModificacion = DateTime.Now,
                    idRiesgoEvaluacion = 0,
                    Mitigacion = riesgo.Mitigacion,
                    Riesgo = riesgo.Riesgo,
                    Ocultar = false
                };

                db.ActaSeguimRiesgosEvaluacion.InsertOnSubmit(r);
                db.SubmitChanges();

                ingresado = true;
            }

            return ingresado;

        }

        public bool ocultarRiesgo(int _idRiesgoInterventoria,int _codcontacto)
        {
            bool ocultado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                var query = (from r in db.ActaSeguimRiesgosEvaluacion
                             where r.idRiesgoInterventoria == _idRiesgoInterventoria
                             select r).FirstOrDefault();

                query.Ocultar = true;
                query.fechaUltimaModificacion = DateTime.Now;
                query.codContactoModifica = _codcontacto;

                db.SubmitChanges();

                ocultado = true;
            }

            return ocultado;
        }

        public bool actualizarRiesgo(int _idRiesgoInterventoria, int _codcontacto, string _riesgo, string _mitigacion)
        {
            bool actualizado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                var query = (from r in db.ActaSeguimRiesgosEvaluacion
                             where r.idRiesgoInterventoria == _idRiesgoInterventoria
                             select r).FirstOrDefault();

               
                query.fechaUltimaModificacion = DateTime.Now;
                query.codContactoModifica = _codcontacto;
                query.Riesgo = _riesgo;
                query.Mitigacion = _mitigacion;

                db.SubmitChanges();

                actualizado = true;
            }

            return actualizado;
        }
    }
}
