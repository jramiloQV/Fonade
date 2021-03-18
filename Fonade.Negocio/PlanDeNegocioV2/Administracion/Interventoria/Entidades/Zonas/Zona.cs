using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Zonas
{
    public static class Zona
    {
        public static List<Datos.Zona> Get(int idDepartamento)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from zonas in db.Zonas
                                where
                                      zonas.IdDepartamento == idDepartamento
                                select zonas);
                return entities.ToList();
            }
        }

        public static void DeleteAllByEntidad(Int32 idEntidad) {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = db.EntidadDepartamentos.Where(filter => filter.IdEntidad == idEntidad).ToList();

                db.EntidadDepartamentos.DeleteAllOnSubmit(entities);
                db.SubmitChanges();
            }
        }

        public static void InsertOrUpdateDepartamento(Datos.EntidadDepartamento departamentoEntidad)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var currentEntity = db.EntidadDepartamentos.FirstOrDefault(filter => filter.IdEntidad == departamentoEntidad.IdEntidad
                                                                          && filter.IdDepartamento == departamentoEntidad.IdDepartamento                                                                
                                                                           );
                if (currentEntity == null)
                {
                    db.EntidadDepartamentos.InsertOnSubmit(departamentoEntidad);
                    db.SubmitChanges();
                }
                else
                {
                    currentEntity.IdZona = departamentoEntidad.IdZona;
                    db.SubmitChanges();
                }
            }
        }
    }
}
