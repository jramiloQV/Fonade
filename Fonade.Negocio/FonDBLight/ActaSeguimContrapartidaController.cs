using Datos;
using Datos.Modelos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.FonDBLight
{
    public class ActaSeguimContrapartidaController
    {
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        public List<ActaSeguimContrapartidaModel> GetContrapartidas(int _codProyecto, int _codConvocatoria)
        {
            List<ActaSeguimContrapartidaModel> listContrapartidas = new List<ActaSeguimContrapartidaModel>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                listContrapartidas = (from e in db.ActaSeguimContrapartida
                                       where e.codProyecto == _codProyecto && e.codConvocatoria == _codConvocatoria
                                       orderby e.numActa
                                       select new ActaSeguimContrapartidaModel
                                       {
                                          cantContrapartida = e.cantContrapartida,
                                          codConvocatoria = e.codConvocatoria,
                                          codProyecto = e.codProyecto,
                                          descripcion = e.Descripcion,
                                          FechaIngresado = e.FechaIngresado,
                                          id = e.idActaSegContrapartida,
                                          numActa = e.numActa,
                                          visita = e.visita
                                       }).ToList();
            }

            return listContrapartidas;
        }

        public bool InsertOrUpdateContrapartida(ActaSeguimContrapartidaModel contrapartida, ref string mensaje)
        {
            bool insertado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {

                int ContrapartidaAnt = (from g in db.ActaSeguimContrapartida
                                        where g.codConvocatoria == contrapartida.codConvocatoria
                                           && g.codProyecto == contrapartida.codProyecto
                                           && g.numActa == (contrapartida.numActa - 1)
                                        select g.cantContrapartida).FirstOrDefault();

                if (contrapartida.cantContrapartida >= ContrapartidaAnt)
                {
                    var actaContrapartida = (from g in db.ActaSeguimContrapartida
                                             where g.codConvocatoria == contrapartida.codConvocatoria
                                             && g.codProyecto == contrapartida.codProyecto
                                             && g.numActa == contrapartida.numActa
                                             select g).FirstOrDefault();

                    if (actaContrapartida != null)//Actualizar
                    {
                        actaContrapartida.cantContrapartida = contrapartida.cantContrapartida;
                        actaContrapartida.Descripcion = contrapartida.descripcion;
                        actaContrapartida.FechaIngresado = DateTime.Now;

                    }
                    else//Insertar
                    {
                        ActaSeguimContrapartida gesContrapartida = new ActaSeguimContrapartida
                        {
                            cantContrapartida = contrapartida.cantContrapartida,
                            codConvocatoria = contrapartida.codConvocatoria,
                            codProyecto = contrapartida.codProyecto,
                            Descripcion = contrapartida.descripcion,
                            numActa = contrapartida.numActa,
                            visita = contrapartida.visita,
                            FechaIngresado = DateTime.Now
                        };

                        db.ActaSeguimContrapartida.InsertOnSubmit(gesContrapartida);
                    }

                    db.SubmitChanges();

                    insertado = true;
                }
                else
                {
                    mensaje = "El valor de las contrapartidas no puede ser menor al de la última visita.";
                }
                
            }

            return insertado;
        }
    }
}
