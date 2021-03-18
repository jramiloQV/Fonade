using Datos;
using Datos.Modelos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.FonDBLight
{
    public class ActaSeguimRiesgosController
    {
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public bool InsertOrUpdateGestionRiesgo(List<ActaSeguimRiesgosModel> gestionRiesgo)
        {
            bool insertado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                foreach (var r in gestionRiesgo)
                {
                    var actaRiesgo = (from g in db.ActaSeguimRiesgos
                                      where g.CodRiego == r.CodRiesgo
                                      && g.NumActa == r.NumActa
                                      select g).FirstOrDefault();

                    if (actaRiesgo != null)//Actualizar
                    {
                        actaRiesgo.GestionEmprendorRiesgo = r.GestionRiesgo;
                        actaRiesgo.FechaIngreso = DateTime.Now;
                    }
                    else//Insertar
                    {
                        ActaSeguimRiesgos gesRiesgo = new ActaSeguimRiesgos
                        {
                            NumActa = r.NumActa,
                            CodConvocatoria = r.CodConvocatoria,
                            CodProyecto = r.CodProyecto,
                            CodRiego = r.CodRiesgo,
                            GestionEmprendorRiesgo = r.GestionRiesgo,
                            FechaIngreso = DateTime.Now
                        };

                        db.ActaSeguimRiesgos.InsertOnSubmit(gesRiesgo);
                    }

                    db.SubmitChanges();
                }
                insertado = true;
            }

            return insertado;
        }

        public string GetGestionEmprendedorByIdRiesgoByActa(int _CodRiesgo, int _numActa)
        {
            string gestion = "";

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                gestion = (from g in db.ActaSeguimRiesgos
                           where g.CodRiego == _CodRiesgo && g.NumActa == _numActa
                           select g.GestionEmprendorRiesgo).FirstOrDefault();
            }

            return gestion;
        }

        public List<ActaSeguimRiesgosModel> GetGestionEmprendedorByIdRiesgo(int _CodRiesgo)
        {
            List<ActaSeguimRiesgosModel> list = new List<ActaSeguimRiesgosModel>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                list = (from g in db.ActaSeguimRiesgos
                        where g.CodRiego == _CodRiesgo
                        orderby g.NumActa
                        select new ActaSeguimRiesgosModel
                        {
                            id = g.idActaRiesgo,
                            CodConvocatoria = g.CodConvocatoria,
                            CodProyecto = g.CodProyecto,
                            CodRiesgo = g.CodRiego,
                            GestionRiesgo = g.GestionEmprendorRiesgo,
                            NumActa = g.NumActa,
                            FechaActualizado = g.FechaIngreso
                        }).ToList();
            }

            return list;
        }
    }
}
