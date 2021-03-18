using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Datos;
using Datos.DataType;

namespace Fonade.Negocio.PlanDeNegocioV2.Evaluacion.TablaDeEvaluacion
{
    public static class DatosGenerales
    {		
		public static EvaluacionObservacion Get(int codigoProyecto,int codigoConvocatoria)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                return db.EvaluacionObservacions
                         .Where(filter => filter.CodProyecto.Equals(codigoProyecto)
                                          && filter.CodConvocatoria.Equals(codigoConvocatoria))                                          
                         .SingleOrDefault();
            }
        }

        public static void Update(EvaluacionObservacion observacion)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var currenEntity = db.EvaluacionObservacions
                         .Where(filter => filter.CodProyecto.Equals(observacion.CodProyecto)
                                          && filter.CodConvocatoria.Equals(observacion.CodConvocatoria))
                         .SingleOrDefault();

                if (currenEntity != null) {
                    currenEntity.Sector = observacion.Sector;
                    currenEntity.Localizacion = observacion.Localizacion;
                    currenEntity.ResumenConcepto = observacion.ResumenConcepto;
                }
                else
                {
                    db.EvaluacionObservacions.InsertOnSubmit(observacion);
                }

                db.SubmitChanges();
            }
        }

		

	}
}
