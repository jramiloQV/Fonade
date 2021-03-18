using Datos;
using Datos.Modelos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.FonDBLight
{
    public class ActaSeguimNotasController
    {
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        public List<ActaSeguimNotasModel> getListNotas(int _codProyecto, int _codConvocatoria)
        {
            List<ActaSeguimNotasModel> listNotas = new List<ActaSeguimNotasModel>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                listNotas = (from e in db.ActaSeguimNotas
                              where e.codProyecto == _codProyecto && e.codConvocatoria == _codConvocatoria
                              orderby e.numActa
                              select new ActaSeguimNotasModel
                              {
                                  id = e.idActaNotas,
                                  codProyecto = e.codProyecto,
                                  codConvocatoria = e.codConvocatoria,
                                  numActa = e.numActa,
                                  visita = e.visita,
                                  Notas = e.Nota,
                                  FechaIngresado = e.FechaIngresado
                              }).ToList();
            }

            return listNotas;
        }

        public bool InsertOrUpdateNotas(ActaSeguimNotasModel _Notas)
        {
            bool insertado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {

                var actaNotas = (from g in db.ActaSeguimNotas
                                         where g.codConvocatoria == _Notas.codConvocatoria
                                         && g.codProyecto == _Notas.codProyecto
                                         && g.numActa == _Notas.numActa
                                         select g).FirstOrDefault();

                if (actaNotas != null)//Actualizar
                {
                    actaNotas.Nota = _Notas.Notas;
                    actaNotas.FechaIngresado = DateTime.Now;
                }
                else//Insertar
                {
                    ActaSeguimNota gesNotas = new ActaSeguimNota
                    {
                        codConvocatoria = _Notas.codConvocatoria,
                        codProyecto = _Notas.codProyecto,
                        numActa = _Notas.numActa,
                        FechaIngresado = DateTime.Now,
                        Nota = _Notas.Notas,
                        visita = _Notas.visita
                    };

                    db.ActaSeguimNotas.InsertOnSubmit(gesNotas);
                }

                db.SubmitChanges();

                insertado = true;
            }

            return insertado;
        }
    }
}
