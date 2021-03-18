using Datos;
using Datos.Modelos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.FonDBLight
{
    public class ActaSeguimGestionVentasController
    {
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        public List<ActaSeguimGestionVentasModel> GetGestionVentas(int _codProyecto, int _codConvocatoria)
        {
            List<ActaSeguimGestionVentasModel> listGestionVentas = new List<ActaSeguimGestionVentasModel>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                listGestionVentas = (from e in db.ActaSeguimGestionVentas
                                         where e.codProyecto == _codProyecto && e.codConvocatoria == _codConvocatoria
                                         orderby e.numActa
                                         select new ActaSeguimGestionVentasModel
                                         {
                                             id = e.idActaSegVentas,
                                             codProyecto = e.codProyecto,
                                             codConvocatoria = e.codConvocatoria,
                                             numActa = e.numActa,
                                             visita = e.visita,
                                             descripcion = e.descripcion,
                                             fechaIngreso = e.fechaIngreso,
                                             valor = e.valor                                            
                                         }).ToList();

                foreach (var i in listGestionVentas)
                {
                    i.valorFormato = i.valor.ToString("C");
                }
            }

            return listGestionVentas;
        }

        public bool InsertOrUpdateGestionVentas(ActaSeguimGestionVentasModel ventas, ref string mensaje)
        {
            bool insertado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {

                decimal ValorAnt = (from g in db.ActaSeguimGestionVentas
                                   where g.codConvocatoria == ventas.codConvocatoria
                                      && g.codProyecto == ventas.codProyecto
                                      && g.numActa == (ventas.numActa - 1)
                                   select g.valor).FirstOrDefault();

                if (ventas.valor >= ValorAnt)
                {
                    var actaVentas = (from g in db.ActaSeguimGestionVentas
                                          where g.codConvocatoria == ventas.codConvocatoria
                                          && g.codProyecto == ventas.codProyecto
                                          && g.numActa == ventas.numActa
                                          select g).FirstOrDefault();

                    if (actaVentas != null)//Actualizar
                    {
                        actaVentas.descripcion = ventas.descripcion;
                        actaVentas.valor = ventas.valor;
                        actaVentas.fechaIngreso = DateTime.Now;

                    }
                    else//Insertar
                    {
                        ActaSeguimGestionVentas gesVentas = new ActaSeguimGestionVentas
                        {
                            descripcion = ventas.descripcion,
                            codConvocatoria = ventas.codConvocatoria,
                            codProyecto = ventas.codProyecto,                           
                            numActa = ventas.numActa,
                            visita = ventas.visita,
                            fechaIngreso = DateTime.Now,
                            valor = ventas.valor
                        };

                        db.ActaSeguimGestionVentas.InsertOnSubmit(gesVentas);
                    }

                    db.SubmitChanges();

                    insertado = true;
                }
                else
                {
                    mensaje = "La cantidad ingresada de valor en ventas acumulada no puede ser menor al de la última visita.";
                }

            }

            return insertado;
        }

    }
}
