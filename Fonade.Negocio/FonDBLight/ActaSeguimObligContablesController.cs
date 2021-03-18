using Datos;
using Datos.Modelos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.FonDBLight
{
    public class ActaSeguimObligContablesController
    {
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        public List<ActaSeguimObligContablesModel> GetObligContable(int _codProyecto, int _codConvocatoria)
        {
            List<ActaSeguimObligContablesModel> listObligContable = new List<ActaSeguimObligContablesModel>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                listObligContable = (from e in db.ActaSeguimObligacionesContables
                                     where e.codProyecto == _codProyecto && e.codConvocatoria == _codConvocatoria
                                     orderby e.numActa
                                     select new ActaSeguimObligContablesModel
                                     {
                                         conciliacionBancaria = e.conciliacionBancaria,
                                         codConvocatoria = e.codConvocatoria,
                                         codProyecto = e.codProyecto,
                                         cuentaBancaria = e.cuentaBancaria,
                                         estadosFinancieros = e.estadosFinancieros,
                                         id = e.idActaSegObligTipicas,
                                         numActa = e.numActa,
                                         visita = e.visita,
                                         fechaIngresado = e.fechaIngresado,
                                         librosComerciales = e.librosComerciales,
                                         librosContabilidad = e.librosContabilidad,
                                         observObligacionContable = e.observObligacionContable
                                     }).ToList();
            }

            return listObligContable;
        }

        public List<ActaSeguimObligTributariasModel> GetObligTributaria(int _codProyecto, int _codConvocatoria)
        {
            List<ActaSeguimObligTributariasModel> listObligTributaria = new List<ActaSeguimObligTributariasModel>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                listObligTributaria = (from e in db.ActaSeguimObligacionesTributarias
                                       where e.codProyecto == _codProyecto && e.codConvocatoria == _codConvocatoria
                                       orderby e.numActa
                                       select new ActaSeguimObligTributariasModel
                                       {
                                           autorretencionRenta = e.autorretencionRenta,
                                           codConvocatoria = e.codConvocatoria,
                                           codProyecto = e.codProyecto,
                                           declaImpConsumo = e.declaImpConsumo,
                                           declaIndustriaComercio = e.declaIndustriaComercio,
                                           id = e.idObligTributaria,
                                           numActa = e.numActa,
                                           visita = e.visita,
                                           fechaIngresado = e.fechaIngresado,
                                           declaInfoExogena = e.declaInfoExogena,
                                           declaraIva = e.declaraIva,
                                           declaraReteFuente = e.declaraReteFuente,
                                           declaRenta = e.declaRenta,
                                           declaRetencionImpIndusComercio = e.declaRetencionImpIndusComercio,
                                           observObligacionTributaria = e.observObligacionTributaria
                                       }).ToList();
            }

            return listObligTributaria;
        }

        public List<ActaSeguimObligLaboralModel> GetObligLaboral(int _codProyecto, int _codConvocatoria)
        {
            List<ActaSeguimObligLaboralModel> listObligLaboral = new List<ActaSeguimObligLaboralModel>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                listObligLaboral = (from e in db.ActaSeguimObligacionesLaborales
                                    where e.codProyecto == _codProyecto && e.codConvocatoria == _codConvocatoria
                                    orderby e.numActa
                                    select new ActaSeguimObligLaboralModel
                                    {
                                        afiliacionSegSocial = e.afiliacionSegSocial,
                                        codConvocatoria = e.codConvocatoria,
                                        codProyecto = e.codProyecto,
                                        certParafiscalesSegSocial = e.certParafiscalesSegSocial,
                                        contratosLaborales = e.contratosLaborales,
                                        id = e.idObligLaboral,
                                        numActa = e.numActa,
                                        visita = e.visita,
                                        fechaIngresado = e.fechaIngresado,
                                        observObligacionLaboral = e.observObligacionLaboral,
                                        pagoSegSocial = e.pagoSegSocial,
                                        pagosNomina = e.pagosNomina,
                                        pagoPrestacionesSociales = e.pagoPrestacionesSociales,
                                        reglaInternoTrab = e.reglaInternoTrab,
                                        sisGestionSegSaludTrabajo = e.sisGestionSegSaludTrabajo
                                    }).ToList();
            }

            return listObligLaboral;
        }

        public List<ActaSeguimObligTramitesModel> GetObligTramite(int _codProyecto, int _codConvocatoria)
        {
            List<ActaSeguimObligTramitesModel> listObligTramites = new List<ActaSeguimObligTramitesModel>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                listObligTramites = (from e in db.ActaSeguimObligacionesTramites
                                     where e.codProyecto == _codProyecto && e.codConvocatoria == _codConvocatoria
                                     orderby e.numActa
                                     select new ActaSeguimObligTramitesModel
                                     {
                                         DocumentoIdoneidad = e.DocumentoIdoneidad,
                                         codConvocatoria = e.codConvocatoria,
                                         codProyecto = e.codProyecto,
                                         certBomberos = e.certBomberos,
                                         certLibertadTradicion = e.certLibertadTradicion,
                                         id = e.idObligTramites,
                                         numActa = e.numActa,
                                         visita = e.visita,
                                         fechaIngresado = e.fechaIngresado,
                                         insCamaraComercio = e.insCamaraComercio,
                                         observRegistroTramiteLicencia = e.observRegistroTramiteLicencia,
                                         otrosPermisos = e.otrosPermisos,
                                         permisoUsoSuelo = e.permisoUsoSuelo,
                                         regMarca = e.regMarca,
                                         renovaRegistroMercantil = e.renovaRegistroMercantil,
                                         resolFacturacion = e.resolFacturacion,
                                         rut = e.rut,
                                         contratoArrendamiento = e.contratoArrendamiento
                                     }).ToList();
            }

            return listObligTramites;
        }

        public bool InsertOrUpdateObligContable(ActaSeguimObligContablesModel obligacion)
        {
            bool insertado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {

                var actaObligacion = (from g in db.ActaSeguimObligacionesContables
                                      where g.codConvocatoria == obligacion.codConvocatoria
                                      && g.codProyecto == obligacion.codProyecto
                                      && g.numActa == obligacion.numActa
                                      select g).FirstOrDefault();

                if (actaObligacion != null)//Actualizar
                {
                    actaObligacion.conciliacionBancaria = obligacion.conciliacionBancaria;
                    actaObligacion.cuentaBancaria = obligacion.cuentaBancaria;
                    actaObligacion.estadosFinancieros = obligacion.estadosFinancieros;
                    actaObligacion.fechaIngresado = DateTime.Now;
                    actaObligacion.librosComerciales = obligacion.librosComerciales;
                    actaObligacion.librosContabilidad = obligacion.librosContabilidad;
                    actaObligacion.observObligacionContable = obligacion.observObligacionContable;
                }
                else//Insertar
                {
                    ActaSeguimObligacionesContables gesObligacion = new ActaSeguimObligacionesContables
                    {
                        codConvocatoria = obligacion.codConvocatoria,
                        observObligacionContable = obligacion.observObligacionContable,
                        numActa = obligacion.numActa,
                        librosContabilidad = obligacion.librosContabilidad,
                        librosComerciales = obligacion.librosComerciales,
                        visita = obligacion.visita,
                        fechaIngresado = DateTime.Now,
                        estadosFinancieros = obligacion.estadosFinancieros,
                        cuentaBancaria = obligacion.cuentaBancaria,
                        conciliacionBancaria = obligacion.conciliacionBancaria,
                        codProyecto = obligacion.codProyecto
                    };

                    db.ActaSeguimObligacionesContables.InsertOnSubmit(gesObligacion);
                }

                db.SubmitChanges();

                insertado = true;


                return insertado;
            }
        }

        public bool InsertOrUpdateObligTributaria(ActaSeguimObligTributariasModel obligacion)
        {
            bool insertado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {

                var actaObligacion = (from g in db.ActaSeguimObligacionesTributarias
                                      where g.codConvocatoria == obligacion.codConvocatoria
                                      && g.codProyecto == obligacion.codProyecto
                                      && g.numActa == obligacion.numActa
                                      select g).FirstOrDefault();

                if (actaObligacion != null)//Actualizar
                {
                    actaObligacion.autorretencionRenta = obligacion.autorretencionRenta;
                    actaObligacion.declaImpConsumo = obligacion.declaImpConsumo;
                    actaObligacion.declaIndustriaComercio = obligacion.declaIndustriaComercio;
                    actaObligacion.declaInfoExogena = obligacion.declaInfoExogena;
                    actaObligacion.declaraIva = obligacion.declaraIva;
                    actaObligacion.declaraReteFuente = obligacion.declaraReteFuente;
                    actaObligacion.declaRenta = obligacion.declaRenta;
                    actaObligacion.declaRetencionImpIndusComercio = obligacion.declaRetencionImpIndusComercio;
                    actaObligacion.fechaIngresado = DateTime.Now;
                    actaObligacion.observObligacionTributaria = obligacion.observObligacionTributaria;

                }
                else//Insertar
                {
                    ActaSeguimObligacionesTributarias gesObligacion = new ActaSeguimObligacionesTributarias
                    {
                        autorretencionRenta = obligacion.autorretencionRenta,
                        declaImpConsumo = obligacion.declaImpConsumo,
                        declaIndustriaComercio = obligacion.declaIndustriaComercio,
                        declaInfoExogena = obligacion.declaInfoExogena,
                        declaraIva = obligacion.declaraIva,
                        declaraReteFuente = obligacion.declaraReteFuente,
                        declaRenta = obligacion.declaRenta,
                        declaRetencionImpIndusComercio = obligacion.declaRetencionImpIndusComercio,
                        fechaIngresado = DateTime.Now,
                        observObligacionTributaria = obligacion.observObligacionTributaria,
                        codConvocatoria = obligacion.codConvocatoria,
                        codProyecto = obligacion.codProyecto,
                        numActa = obligacion.numActa,
                        visita = obligacion.visita

                    };

                    db.ActaSeguimObligacionesTributarias.InsertOnSubmit(gesObligacion);
                }

                db.SubmitChanges();

                insertado = true;


                return insertado;
            }
        }

        public bool InsertOrUpdateObligLaboral(ActaSeguimObligLaboralModel obligacion)
        {
            bool insertado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {

                var actaObligacion = (from g in db.ActaSeguimObligacionesLaborales
                                      where g.codConvocatoria == obligacion.codConvocatoria
                                      && g.codProyecto == obligacion.codProyecto
                                      && g.numActa == obligacion.numActa
                                      select g).FirstOrDefault();

                if (actaObligacion != null)//Actualizar
                {
                    actaObligacion.afiliacionSegSocial = obligacion.afiliacionSegSocial;
                    actaObligacion.certParafiscalesSegSocial = obligacion.certParafiscalesSegSocial;
                    actaObligacion.contratosLaborales = obligacion.contratosLaborales;
                    actaObligacion.fechaIngresado = DateTime.Now;
                    actaObligacion.observObligacionLaboral = obligacion.observObligacionLaboral;
                    actaObligacion.pagoSegSocial = obligacion.pagoSegSocial;
                    actaObligacion.pagosNomina = obligacion.pagosNomina;
                    actaObligacion.reglaInternoTrab = obligacion.reglaInternoTrab;
                    actaObligacion.sisGestionSegSaludTrabajo = obligacion.sisGestionSegSaludTrabajo;
                    actaObligacion.pagoPrestacionesSociales = obligacion.pagoPrestacionesSociales;

                }
                else//Insertar
                {
                    ActaSeguimObligacionesLaborales gesObligacion = new ActaSeguimObligacionesLaborales
                    {
                        afiliacionSegSocial = obligacion.afiliacionSegSocial,
                        certParafiscalesSegSocial = obligacion.certParafiscalesSegSocial,
                        contratosLaborales = obligacion.contratosLaborales,
                        fechaIngresado = DateTime.Now,
                        observObligacionLaboral = obligacion.observObligacionLaboral,
                        pagoSegSocial = obligacion.pagoSegSocial,
                        pagosNomina = obligacion.pagosNomina,
                        pagoPrestacionesSociales = obligacion.pagoPrestacionesSociales,
                        reglaInternoTrab = obligacion.reglaInternoTrab,
                        sisGestionSegSaludTrabajo = obligacion.sisGestionSegSaludTrabajo,
                        codConvocatoria = obligacion.codConvocatoria,
                        codProyecto = obligacion.codProyecto,
                        numActa = obligacion.numActa,
                        visita = obligacion.visita
                    };

                    db.ActaSeguimObligacionesLaborales.InsertOnSubmit(gesObligacion);
                }

                db.SubmitChanges();

                insertado = true;

                return insertado;
            }
        }

        public bool InsertOrUpdateObligTramite(ActaSeguimObligTramitesModel obligacion)
        {
            bool insertado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {

                var actaObligacion = (from g in db.ActaSeguimObligacionesTramites
                                      where g.codConvocatoria == obligacion.codConvocatoria
                                      && g.codProyecto == obligacion.codProyecto
                                      && g.numActa == obligacion.numActa
                                      select g).FirstOrDefault();

                if (actaObligacion != null)//Actualizar
                {
                    actaObligacion.DocumentoIdoneidad = obligacion.DocumentoIdoneidad;
                    actaObligacion.certBomberos = obligacion.certBomberos;
                    actaObligacion.certLibertadTradicion = obligacion.certLibertadTradicion;
                    actaObligacion.fechaIngresado = DateTime.Now;
                    actaObligacion.insCamaraComercio = obligacion.insCamaraComercio;
                    actaObligacion.observRegistroTramiteLicencia = obligacion.observRegistroTramiteLicencia;
                    actaObligacion.otrosPermisos = obligacion.otrosPermisos;
                    actaObligacion.permisoUsoSuelo = obligacion.permisoUsoSuelo;
                    actaObligacion.regMarca = obligacion.regMarca;
                    actaObligacion.renovaRegistroMercantil = obligacion.renovaRegistroMercantil;
                    actaObligacion.resolFacturacion = obligacion.resolFacturacion;
                    actaObligacion.rut = obligacion.rut;
                    actaObligacion.contratoArrendamiento = obligacion.contratoArrendamiento;

                }
                else//Insertar
                {
                    ActaSeguimObligacionesTramites gesObligacion = new ActaSeguimObligacionesTramites
                    {
                        DocumentoIdoneidad = obligacion.DocumentoIdoneidad,
                        certBomberos = obligacion.certBomberos,
                        certLibertadTradicion = obligacion.certLibertadTradicion,
                        fechaIngresado = DateTime.Now,
                        insCamaraComercio = obligacion.insCamaraComercio,
                        observRegistroTramiteLicencia = obligacion.observRegistroTramiteLicencia,
                        otrosPermisos = obligacion.otrosPermisos,
                        permisoUsoSuelo = obligacion.permisoUsoSuelo,
                        regMarca = obligacion.regMarca,
                        renovaRegistroMercantil = obligacion.renovaRegistroMercantil,
                        resolFacturacion = obligacion.resolFacturacion,
                        rut = obligacion.rut,
                        codConvocatoria = obligacion.codConvocatoria,
                        codProyecto = obligacion.codProyecto,
                        numActa = obligacion.numActa,
                        visita = obligacion.visita,
                        contratoArrendamiento = obligacion.contratoArrendamiento
                    };

                    db.ActaSeguimObligacionesTramites.InsertOnSubmit(gesObligacion);
                }

                db.SubmitChanges();

                insertado = true;

                return insertado;
            }
        }
    }
}