using Datos;
using Datos.Modelos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.FonDBLight
{
    public class ActaSeguimientoDatosController
    {
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;


        public bool InsertActaSeguimientoDatos(ActaSeguimientoDatosModel acta, ref string Error)
        {
            bool insertado = false;
            try
            {
                using (Datos.FonadeDBLightDataContext db = new Datos.FonadeDBLightDataContext(_cadena))
                {
                    ActaSeguimientoDatos datoActa = new ActaSeguimientoDatos
                    {
                        idActa = acta.idActa,
                        NumActa = acta.numActa,
                        NumContrato = acta.NumContrato,
                        FechaActaInicio = acta.FechaActaInicio,
                        Prorroga = acta.Prorroga,
                        NombrePlanNegocio = acta.NombrePlanNegocio,
                        NombreEmpresa = acta.NombreEmpresa,
                        NitEmpresa = acta.NitEmpresa,
                        ContratoMarcoInteradmin = acta.ContratoMarcoInteradmin,
                        ContratoInterventoria = acta.ContratoInterventoria,
                        Contratista = acta.Contratista,
                        ValorAprobado = acta.ValorAprobado,
                        DomicilioPrincipal = acta.DomicilioPrincipal,
                        Convocatoria = acta.Convocatoria,
                        SectorEconomico = acta.SectorEconomico,
                        ObjetoProyecto = acta.ObjetoProyecto,
                        ObjetoVisita = acta.ObjetoVisita,
                        NombreGestorTecnicoSena = acta.NombreGestorTecnicoSena,
                        EmailGestorTecnicoSena = acta.EmailGestorTecnicoSena,
                        TelefonoGestorTecnicoSena = acta.TelefonoGestorTecnicoSena,
                        NombreGestorOperativoSena = acta.NombreGestorOperativoSena,
                        EmailGestorOperativoSena = acta.EmailGestorOperativoSena,
                        TelefonoGestorOperativoSena = acta.TelefonoGestorOperativoSena,
                        FechaIngresado = acta.FechaActualizado                       
                        
                    };

                    db.ActaSeguimientoDatos.InsertOnSubmit(datoActa);
                    db.SubmitChanges();
                    insertado = true;
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                insertado = false;
            }
            return insertado;
        }

        public List<ActaSeguimientoDatosModel> fechasActas(int _codProyecto)
        {
            List<ActaSeguimientoDatosModel> actas = new List<ActaSeguimientoDatosModel>();

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                actas = (from a in db.ActaSeguimientoInterventoria
                               where a.IdProyecto == _codProyecto
                               orderby a.NumeroActa
                               select new ActaSeguimientoDatosModel { 
                                   numActa = a.NumeroActa,
                                   FechaPublicacion = a.FechaPublicacion
                               }).ToList();
            }

            return actas;
        }


        public ActaSeguimientoDatosModel obtenerDatosActa(int _numActa, int _codProyecto)
        {
            ActaSeguimientoDatosModel acta;
           
            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                var idDatosActa = (from a in db.ActaSeguimientoInterventoria
                               where a.NumeroActa == _numActa && a.IdProyecto == _codProyecto
                               select a).FirstOrDefault();

                int codSubSector = (from p in db.Proyecto
                                    where p.Id_Proyecto == _codProyecto
                                    select p.CodSubSector).FirstOrDefault();

                string nombreSubSector = (from ss in db.SubSector
                                          where ss.Id_SubSector == codSubSector
                                          select ss.NomSubSector).FirstOrDefault();

                int codSector = (from ss in db.SubSector
                                          where ss.Id_SubSector == codSubSector
                                          select ss.CodSector).FirstOrDefault();

                string nombreSector = (from s in db.Sector
                                       where s.Id_Sector == codSector
                                       select s.NomSector).FirstOrDefault();

                using (FonadeDBLightDataContext db2 = new FonadeDBLightDataContext(_cadena))
                {
                    acta = (from a in db2.ActaSeguimientoDatos
                            where a.idActa == idDatosActa.Id
                            select new ActaSeguimientoDatosModel
                            {
                                idActa = a.idActa,
                                numActa = a.NumActa.Value,
                                NumContrato = a.NumContrato,
                                FechaActaInicio = a.FechaActaInicio.Value,
                                Prorroga = a.Prorroga,
                                NombrePlanNegocio = a.NombrePlanNegocio,
                                NombreEmpresa = a.NombreEmpresa,
                                NitEmpresa = a.NitEmpresa,
                                ContratoMarcoInteradmin = a.ContratoMarcoInteradmin,
                                ContratoInterventoria = a.ContratoInterventoria,
                                Contratista = a.Contratista,
                                ValorAprobado = a.ValorAprobado,
                                DomicilioPrincipal = a.DomicilioPrincipal,
                                Convocatoria = a.Convocatoria,
                                SectorEconomico = nombreSector,
                                SubSectorEconomico = nombreSubSector,
                                ObjetoProyecto = SumarioXProyecto(_codProyecto),
                                ObjetoVisita = a.ObjetoVisita,
                                NombreGestorTecnicoSena = a.NombreGestorTecnicoSena,
                                EmailGestorTecnicoSena = a.EmailGestorTecnicoSena,
                                TelefonoGestorTecnicoSena = a.TelefonoGestorTecnicoSena,
                                NombreGestorOperativoSena = a.NombreGestorOperativoSena,
                                EmailGestorOperativoSena = a.EmailGestorOperativoSena,
                                TelefonoGestorOperativoSena = a.TelefonoGestorOperativoSena,
                                FechaActualizado = a.FechaIngresado,
                                FechaPublicacion = idDatosActa.FechaPublicacion ?? DateTime.Now,
                                FechaFinalVisita = idDatosActa.FechaFinalVisita
                            }).FirstOrDefault();
                }
            }

            
            return acta;
        }

        public string SumarioXProyecto(int _codProyecto)
        {
            string sumario = "";

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena)) {
                sumario = (from p in db.Proyecto
                           where p.Id_Proyecto == _codProyecto
                           select p.Sumario).FirstOrDefault();
            }

                return sumario;
        }

        public bool ActualizarDatosGestor(ActaSeguimientoDatosModel acta)
        {
            bool actualizado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                var actaDatos = (from c in db.ActaSeguimientoDatos
                                 where c.idActa == acta.idActa
                                 select c).FirstOrDefault();

                actaDatos.NombreGestorTecnicoSena = acta.NombreGestorTecnicoSena;
                actaDatos.EmailGestorTecnicoSena = acta.EmailGestorTecnicoSena;
                actaDatos.TelefonoGestorTecnicoSena = acta.TelefonoGestorTecnicoSena;
                actaDatos.NombreGestorOperativoSena = acta.NombreGestorOperativoSena;
                actaDatos.EmailGestorOperativoSena = acta.EmailGestorOperativoSena;
                actaDatos.TelefonoGestorOperativoSena = acta.TelefonoGestorOperativoSena;
               

                actaDatos.FechaIngresado = DateTime.Now;
                db.SubmitChanges();

                actualizado = true;
            }

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                var actaInterventor = (from c in db.ActaSeguimientoInterventoria
                                 where c.Id == acta.idActa
                                 select c).FirstOrDefault();

                actaInterventor.FechaPublicacion = acta.FechaPublicacion;
                actaInterventor.FechaFinalVisita = acta.FechaFinalVisita;

                db.SubmitChanges();

                actualizado = true;
            }

                return actualizado;
        }

    }
}
