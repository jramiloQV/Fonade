using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fonade.FONADE.evaluacion
{
    public class ValidarActividades
    {
        private string _cadena = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        public int validarPlanOperativo(int codProyecto, int numitem, string NomActividad)
        {
            //Validacion si ya existe la actividad PO
            int cantRegistros = 0;
            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                var query = (from po in db.ProyectoActividadPOInterventors
                             where po.CodProyecto == codProyecto
                                    && po.Item == numitem
                                    && po.NomActividad.ToUpper() == NomActividad.ToUpper()
                             select po).ToList();

                cantRegistros = query.Count();
            }

            return cantRegistros;
        }

        public int validarPlanOperativoTMP(int codProyecto, int numitem, string NomActividad)
        {
            //Validacion si ya existe la actividad TMP PO
            int cantRegistros = 0;
            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                var query = (from po in db.ProyectoActividadPOInterventorTMPs
                             where po.CodProyecto == codProyecto
                                    && po.Item == numitem
                                    && po.NomActividad.ToUpper() == NomActividad.ToUpper()
                                    && po.Tarea.ToUpper() == "Adicionar".ToUpper()
                             select po).ToList();

                cantRegistros = query.Count();
            }
            return cantRegistros;
        }
        public int validarNomina(int CodProyecto, string Item, string tipo)
        {
            //Validacion si ya existe la Nomina
            int cantRegistros = 0;
            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                var query = (from po in db.InterventorNominas
                             where po.CodProyecto == CodProyecto
                                    && po.Cargo.ToUpper() == Item.ToUpper()
                                    && po.Tipo.ToUpper() == tipo.ToUpper()
                             select po).ToList();

                cantRegistros = query.Count();
            }

            return cantRegistros;
        }

        public int validarNominaTMP(int codProyecto, string item, string tipo)
        {
            int cantRegistrosTMP = 0;
            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                var query = (from po in db.InterventorNominaTMPs
                             where po.CodProyecto == codProyecto
                                    && po.Cargo.ToUpper() == item.ToUpper()
                                    && po.Tipo.ToUpper() == tipo.ToUpper()
                                    && po.Tarea.ToUpper() == "Adicionar".ToUpper()
                             select po).ToList();

                cantRegistrosTMP = query.Count();
            }

            return cantRegistrosTMP;
        }

        public int validarProduccionTMP(int codProyecto, string NomProducto)
        {
            int cantRegistro = 0;

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                var query = (from po in db.InterventorProduccionTMPs
                             where po.CodProyecto == codProyecto
                                    && po.NomProducto.ToUpper() == NomProducto.ToUpper()
                                    && po.Tarea.ToUpper() == "Adicionar".ToUpper()
                             select po).ToList();

                cantRegistro = query.Count();
            }

            return cantRegistro;
        }

        public int validarProduccion(int codProyecto, string NomProducto)
        {
            int cantRegistro = 0;

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                var query = (from po in db.InterventorProduccions
                             where po.CodProyecto == codProyecto
                                    && po.NomProducto.ToUpper() == NomProducto.ToUpper()
                             select po).ToList();

                cantRegistro = query.Count();
            }

            return cantRegistro;
        }

        public int validarVentasTMP(int codProyecto, string NomProducto)
        {
            int cantRegistro = 0;

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                var query = (from po in db.InterventorVentasTMPs
                             where po.CodProyecto == codProyecto
                                    && po.NomProducto.ToUpper() == NomProducto.ToUpper()
                                    && po.Tarea.ToUpper() == "Adicionar".ToUpper()
                             select po).ToList();

                cantRegistro = query.Count();
            }

            return cantRegistro;
        }

        public int validarVentas(int codProyecto, string NomProducto)
        {
            int cantRegistro = 0;

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                var query = (from po in db.InterventorVentas
                             where po.CodProyecto == codProyecto
                                    && po.NomProducto.ToUpper() == NomProducto.ToUpper()
                             select po).ToList();

                cantRegistro = query.Count();
            }

            return cantRegistro;
        }

    }
}