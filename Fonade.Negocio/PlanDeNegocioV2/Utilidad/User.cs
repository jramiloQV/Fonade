using System;
using System.Linq;
using System.Web.UI.WebControls;
using Datos;
using System.Globalization;
using System.Web;
using System.Collections.Generic;

namespace Fonade.Negocio.PlanDeNegocioV2.Utilidad
{
    public static class User
    {
        public static void SaveLogin(int codigoContacto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = db.LogIngresos.FirstOrDefault(filter => filter.CodContacto == codigoContacto);

                if (entity != null)
                {
                    entity.FechaUltimoIngreso = DateTime.Now;
                }
                else
                {
                    var newEntity = new LogIngreso {
                        CodContacto = codigoContacto,
                        FechaUltimoIngreso = DateTime.Now
                    };

                    db.LogIngresos.InsertOnSubmit(newEntity);
                }
                db.SubmitChanges();
            }
        }

        public static void InactivateByTime(int codigoContacto, int maxMonths)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = db.LogIngresos.FirstOrDefault(filter => filter.CodContacto == codigoContacto);

                if (entity != null)
                {
                    var fechaUltimoIngreso = entity.FechaUltimoIngreso.GetValueOrDefault();
                    var fechaActual = DateTime.Now;

                    var difference = ((fechaActual.Year - fechaUltimoIngreso.Year) * 12) + fechaActual.Month - fechaUltimoIngreso.Month;

                    if (difference > maxMonths) {
                        entity.FechaUltimoIngreso = null;
                        var user = db.Contacto.FirstOrDefault(filterContacto => filterContacto.Id_Contacto == codigoContacto);
                        if (user != null)
                            user.Inactivo = true;

                        db.SubmitChanges();
                        throw new ApplicationException("El usuario ha sido inactivado por inactiviadd.");
                    }
                }
            }
        }

        public static bool contrasenaMigrada (int codigoContacto)
        {
            bool passmigrado = false;

            using (Datos.FonadeDBLightDataContext db = new Datos.FonadeDBLightDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var query = (from l in db.LogContrasena
                             where l.codContacto == codigoContacto
                             select l).Count();

                if (query > 0)
                    passmigrado = true;
            }

            return passmigrado;
        }

        public static Clave getClave(int codigoContacto)
        {
            Clave claveUser;
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                claveUser = db.Clave.FirstOrDefault(filter => filter.codContacto == codigoContacto);
            }
            return claveUser;
        }


        public static Clave getClaveActiva(int codigoContacto)
        {
            Clave claveUser;
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                claveUser = db.Clave.FirstOrDefault(filter => filter.codContacto == codigoContacto && filter.YaAvisoExpiracion == 0);
                //claveUser = db.Clave.FirstOrDefault(filter => filter.YaAvisoExpiracion == 0);
            }
            return claveUser;
        }
    }
}
