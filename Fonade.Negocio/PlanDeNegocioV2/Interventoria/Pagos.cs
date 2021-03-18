using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.PlanDeNegocioV2.Interventoria
{
    public class Pagos
    {
        public static PagoActividad GetInfoPago(int codigoPago)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from infoPago in db.PagoActividad
                              where
                                infoPago.Id_PagoActividad == codigoPago
                              select
                                infoPago
                              ).FirstOrDefault();

                return entity;
            }
        }

        public static Datos.Contacto GetInterventorByPagoId(int codigoPago)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from pagos in db.PagoActividad
                              join proyectos in db.Proyecto on pagos.CodProyecto equals proyectos.Id_Proyecto
                              join empresas in db.Empresas on proyectos.Id_Proyecto equals empresas.codproyecto
                              join empresaInterventor in db.EmpresaInterventors on empresas.id_empresa equals empresaInterventor.CodEmpresa
                              join contacto in db.Contacto on empresaInterventor.CodContacto equals contacto.Id_Contacto
                              where pagos.Id_PagoActividad == codigoPago
                                    && empresaInterventor.FechaFin == null
                                    && empresaInterventor.Inactivo == false
                              orderby
                                empresaInterventor.Rol descending,
                                empresaInterventor.FechaInicio descending
                              select contacto
                              ).FirstOrDefault();

                return entity;
            }
        }

        public static void Reintegrar(int codigoPago, decimal valorPagoConReintegro) {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from infoPago in db.PagoActividad
                              where
                                infoPago.Id_PagoActividad == codigoPago
                              select
                                infoPago
                              ).FirstOrDefault();

                if (entity != null)
                {
                    entity.CantidadDinero = valorPagoConReintegro;
                    db.SubmitChanges();
                }                
            }
        }
    }
}
