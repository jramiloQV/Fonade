using Datos;
using Datos.Modelos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.FonDBLight
{
    public class ActaSeguimGestionEmpleoController
    {
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        public List<ActaSeguimGestionEmpleoModel> GetGestionEmploByCodProyectoByCodConvocatoria(int _codProyecto, int _codConvocatoria)
        {
            List<ActaSeguimGestionEmpleoModel> listGestionEmpleo = new List<ActaSeguimGestionEmpleoModel>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                listGestionEmpleo = (from e in db.ActaSeguimGestionEmpleo
                                     where e.codProyecto == _codProyecto && e.codConvocatoria == _codConvocatoria
                                     orderby e.numActa
                                     select new ActaSeguimGestionEmpleoModel
                                     {
                                         id = e.idActaGestionEmpleo,
                                         codProyecto = e.codProyecto,
                                         codConvocatoria = e.codConvocatoria,
                                         numActa = e.numActa,
                                         Visita = e.Visita,
                                         desarrolloIndicador = e.DesarrolloIndicador,
                                         verificaIndicador = e.VerificaIndicador,
                                         FechaUpdate = e.FechaIngreso
                                     }).ToList();
            }

            return listGestionEmpleo;

        }

        public bool registroIndicadorExist(int _codProyecto, int _codConvocatoria, int _numActa)
        {
            bool registrado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                int cant = (from a in db.ActaSeguimGestionEmpleo
                            where a.codProyecto == _codProyecto
                            && a.codConvocatoria == _codConvocatoria
                            && a.numActa == _numActa
                            select a.idActaGestionEmpleo).Count();

                if (cant > 0)
                {
                    registrado = true;
                }
            }

            return registrado;
        }

        public int numVisita(int _codProyecto, int _codConvocatoria)
        {
            int num = 0;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                int cant = (from a in db.ActaSeguimGestionEmpleo
                            where a.codProyecto == _codProyecto
                            && a.codConvocatoria == _codConvocatoria
                            orderby a.Visita descending
                            select a.Visita).FirstOrDefault();

                num = cant + 1;
            }

            return num;
        }

        public bool InsertOrUpdateGestionEmpleo(ActaSeguimGestionEmpleoModel gestionEmpleo)
        {
            bool insertado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {

                var actaEmpleo = (from g in db.ActaSeguimGestionEmpleo
                                  where g.codConvocatoria == gestionEmpleo.codConvocatoria
                                  && g.codProyecto == gestionEmpleo.codProyecto
                                  && g.numActa == gestionEmpleo.numActa
                                  select g).FirstOrDefault();

                if (actaEmpleo != null)//Actualizar
                {
                    actaEmpleo.VerificaIndicador = gestionEmpleo.verificaIndicador;
                    actaEmpleo.DesarrolloIndicador = gestionEmpleo.desarrolloIndicador;
                    actaEmpleo.FechaIngreso = DateTime.Now;
                }
                else//Insertar
                {
                    ActaSeguimGestionEmpleo gesEmpleo = new ActaSeguimGestionEmpleo
                    {
                        Visita = gestionEmpleo.Visita,
                        codConvocatoria = gestionEmpleo.codConvocatoria,
                        codProyecto = gestionEmpleo.codProyecto,
                        DesarrolloIndicador = gestionEmpleo.desarrolloIndicador,
                        numActa = gestionEmpleo.numActa,
                        VerificaIndicador = gestionEmpleo.verificaIndicador,
                        FechaIngreso = DateTime.Now
                    };

                    db.ActaSeguimGestionEmpleo.InsertOnSubmit(gesEmpleo);
                }

                db.SubmitChanges();

                insertado = true;
            }

            return insertado;
        }

    }
}
