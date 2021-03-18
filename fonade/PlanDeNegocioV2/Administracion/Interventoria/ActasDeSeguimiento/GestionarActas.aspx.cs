using Fonade.Account;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using SelectPdf;
using Fonade.Clases;
using System.Globalization;
using Fonade.PlanDeNegocioV2.Formulacion.Utilidad;
using Datos;
using Fonade.Negocio.FonDBLight;
using System.Drawing;
using Microsoft.Reporting.WebForms;
using static Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.Report.dsActaSeguimientoUNO;
using Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.Report;
using Fonade.Negocio.PlanDeNegocioV2.Administracion.Operador;
using Fonade.Negocio.Utilidades;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento
{
    public partial class GestionarActas : System.Web.UI.Page
    {
        protected FonadeUser Usuario
        {
            get
            {
                return HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true);
            }
            set
            {
            }
        }

        public int CodigoProyecto
        {
            get
            {
                if (Request.QueryString["codigo"] != null)
                    return Convert.ToInt32(Request.QueryString["codigo"]);
                else
                    return 0;
            }
            set { }
        }

        public string baseDirectory = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual");

        public string FileName
        {
            get
            {
                return "Acta_Inicio_" + CodigoProyecto + ".pdf";
            }
            set
            {
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CodigoProyecto.Equals(0))
                {
                    var Actas = GetActas(CodigoProyecto);
                    if (Actas.Count() > 0)
                    {
                        lnkNew.Visible = false;
                        lnkAddActaSeguimiento.Visible = true;
                        var datos = GetIndicadores(CodigoProyecto);
                        gvIndicadores.DataSource = datos;
                        gvIndicadores.DataBind();
                    }


                }
                else
                {
                    Alert("Debe seleccionar un proyecto!.");
                }

            }
        }

        protected void lnkNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PlanDeNegocioV2/Administracion/Interventoria/ActasDeSeguimiento/CrearActaInicio.aspx?codigo=" + CodigoProyecto);
        }

        public List<Negocio.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimientos.ActaSeguimientoDTO> GetActas(int codigoProyecto)
        {
            return Negocio.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimientos.ActaSeguimiento.GetActasByProyecto(codigoProyecto);
        }

        /// <summary>
        /// Generacion ACTA Seguimiento
        /// </summary>

        protected override void OnLoad(EventArgs e)
        {
            if (Session["usuarioLogged"] == null) { Response.Redirect("~/Account/Login.aspx"); return; }
            usuario = HttpContext.Current.Session["usuarioLogged"] != null ?
                (FonadeUser)HttpContext.Current.Session["usuarioLogged"] :
                (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true);
            base.OnLoad(e);
        }

        [ContextStatic]
        protected FonadeUser usuario;

        #region Llamados

        DatosActaDTOController datosActaDTOController = new DatosActaDTOController();
        ActaSeguimientoDatosController datosController = new ActaSeguimientoDatosController();
        EvaluacionRiesgoController evaluacionRiesgoController = new EvaluacionRiesgoController();
        ActaSeguimRiesgosController actaSeguimRiesgosController = new ActaSeguimRiesgosController();
        MetasEmpleoControllerDTO metasEmpleoController = new MetasEmpleoControllerDTO();
        ActaSeguimGestionEmpleoController empleoController = new ActaSeguimGestionEmpleoController();
        ActaSeguimInfoPagosController infoPagosController = new ActaSeguimInfoPagosController();
        MetasActividadControllerDTO metasActividadController = new MetasActividadControllerDTO();
        ActaSeguimGestionMercadeoController actaSeguimGestionMercadeoController = new ActaSeguimGestionMercadeoController();
        ActaSeguimContrapartidaController contrapartidaController = new ActaSeguimContrapartidaController();
        ActaSeguimGestionProduccionController produccionController = new ActaSeguimGestionProduccionController();
        MetasVentasControllerDTO metasVentasController = new MetasVentasControllerDTO();
        ActaSeguimGestionVentasController gestionVentasController = new ActaSeguimGestionVentasController();
        ActaSeguimObligContablesController obligContablesController = new ActaSeguimObligContablesController();
        ActaSeguimOtrosAspectosController otrosAspectosController = new ActaSeguimOtrosAspectosController();
        ActaSeguimOtrasObligacionesController otrasObligacionesController = new ActaSeguimOtrasObligacionesController();
        ActaSeguimEstadoEmpresaController estadoEmpresaController = new ActaSeguimEstadoEmpresaController();
        ActaSeguimCompromisosController compromisosController = new ActaSeguimCompromisosController();
        ActaSeguimientoInterventoriaController interventoriaController = new ActaSeguimientoInterventoriaController();
        ActaSeguimientoDatosController actaSeguimientoDatosController = new ActaSeguimientoDatosController();
        ActaSeguimNotasController actaSeguimNotasController = new ActaSeguimNotasController();

        OperadorController operadorController = new OperadorController();
        #endregion

        private string metaAporteEmprendedor(int _visita, int _codProyecto, int _codConvocatoria)
        {
            string meta = "";
            int visitaAnterior = 0;
            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                var aporteEmprendedor = (from a in db.ActaSeguimAporteEmprendedor
                                         where a.visita == _visita && a.codProyecto == _codProyecto
                                         select a).FirstOrDefault();

                if (aporteEmprendedor == null || aporteEmprendedor.metaEmprendedor == null)
                {
                    visitaAnterior = _visita - 1;
                    if (visitaAnterior == 0)
                    {
                        meta = infoPagosController.AporteEmpRecomendado(_codProyecto, _codConvocatoria);
                    }
                    else
                    {
                        meta = (from p in db.ActaSeguimAporteEmprendedor
                                where p.codProyecto == _codProyecto && p.visita == visitaAnterior
                                select p.metaEmprendedor).FirstOrDefault();
                    }
                }
                else
                {
                    meta = aporteEmprendedor.metaEmprendedor;
                }
            }

            return meta;
        }

        private void GenerarActaSeguimiento(int _codProyecto, int _numActa, int _codConvocatoria, bool borrador = false)
        {
            //datos de entidad
            var entidadInterv = datosActaDTOController.getInfoEntidadInteventora(_codProyecto, usuario.IdContacto);

            dsActaSeguimientoUNO dsActa = new dsActaSeguimientoUNO();

            #region DatosGenerales
            //datos de acta
            var datos = datosController.obtenerDatosActa(_numActa, _codProyecto);


            dtInfoGeneralRow dtInfoRow = dsActa.dtInfoGeneral.NewdtInfoGeneralRow();

            dtInfoRow.NumeroActa = datos.numActa;
            dtInfoRow.FechaActa = datos.FechaPublicacion.Value.ToShortDateString();
            dtInfoRow.NumContrato = datos.NumContrato;
            dtInfoRow.FechaActaInicio = datos.FechaActaInicio.ToShortDateString();
            dtInfoRow.Prorroga = datos.Prorroga;
            dtInfoRow.NombrePlanNegocio = datos.NombrePlanNegocio;
            dtInfoRow.NombreEmpresa = datos.NombreEmpresa;
            dtInfoRow.NitEmpresa = datos.NitEmpresa;
            dtInfoRow.ContratoMarcoAdmin = datos.ContratoMarcoInteradmin;
            dtInfoRow.ContratoInterventoria = datos.ContratoInterventoria;
            dtInfoRow.Contratista = datos.Contratista;
            dtInfoRow.ValorAprobado = datos.ValorAprobado;
            dtInfoRow.DomicilioPrincipal = datos.DomicilioPrincipal;
            dtInfoRow.Convocatoria = datos.Convocatoria;
            dtInfoRow.Sector = datos.SectorEconomico;
            dtInfoRow.ObjetoProyecto = datos.ObjetoProyecto;
            dtInfoRow.ObjetoVisita = datos.ObjetoVisita;
            dtInfoRow.SubSector = datos.SubSectorEconomico;

            dsActa.dtInfoGeneral.AdddtInfoGeneralRow(dtInfoRow);
            #endregion

            #region ActasFechas

            //ActasFechas
            var actasFechas = datosController.fechasActas(_codProyecto);

            foreach (var a in actasFechas)
            {
                dtActasFechasRow dtActaFechRow = dsActa.dtActasFechas.NewdtActasFechasRow();

                dtActaFechRow.NumeroActa = a.numActa.ToString() == "0"
                                                ? "Acta de Inicio"
                                                : "Acta de Seguimiento " + (a.numActa);
                dtActaFechRow.FechaActa = a.FechaPublicacion.ToString() ?? "";

                dsActa.dtActasFechas.AdddtActasFechasRow(dtActaFechRow);
            }

            #endregion

            #region Datos Indicadores y Metas

            //metas de indicadores
            IndicadoresEjecucionActasSeguimiento ejecActas = new IndicadoresEjecucionActasSeguimiento();
            var indicadores = GetIndicadores(_codProyecto, ref ejecActas, _numActa);
            int numeroVisita = 0;
            foreach (var i in indicadores)
            {
                dtIndicadoresMetasRow dtIndMetasRow = dsActa.dtIndicadoresMetas.NewdtIndicadoresMetasRow();

                dtIndMetasRow.Visita = i.Visita;
                dtIndMetasRow.Cargos = i.Cargos.ToString();
                dtIndMetasRow.Presupuesto = i.PresupuestoWithFormat;
                dtIndMetasRow.Mercadeo = i.Mercadeo.ToString();
                dtIndMetasRow.Idh = Math.Round(i.Nbi, 2).ToString();
                dtIndMetasRow.Contrapartidas = i.Contrapartidas.ToString();
                dtIndMetasRow.Produccion = i.Produccion;
                dtIndMetasRow.Ventas = i.VentasWithFormat;

                dsActa.dtIndicadoresMetas.AdddtIndicadoresMetasRow(dtIndMetasRow);

                int.TryParse(i.Visita, out numeroVisita);
            }

            dtIndicadoresMetasRow dtIndMetasRowFinal = dsActa.dtIndicadoresMetas.NewdtIndicadoresMetasRow();

            dtIndMetasRowFinal.Visita = ejecActas.Visita;
            dtIndMetasRowFinal.Cargos = ejecActas.Cargos + "%";
            dtIndMetasRowFinal.Presupuesto = ejecActas.Presupuesto + "%";
            dtIndMetasRowFinal.Mercadeo = ejecActas.Mercadeo.ToString() + "%";
            dtIndMetasRowFinal.Idh = ejecActas.Nbi.ToString();
            dtIndMetasRowFinal.Contrapartidas = ejecActas.Contrapartidas + "%";
            dtIndMetasRowFinal.Produccion = ejecActas.Produccion + "%";
            dtIndMetasRowFinal.Ventas = ejecActas.Ventas.ToString() + "%";

            dsActa.dtIndicadoresMetas.AdddtIndicadoresMetasRow(dtIndMetasRowFinal);

            #endregion

            #region Riegos

            //Riesgos
            var evalRiegos = evaluacionRiesgoController
                                .GetSeguimientoRiesgoByCodProyectoByCodConvocatoria
                                (CodigoProyecto, _codConvocatoria);

            int contRiegos = 0;
            foreach (var i in evalRiegos)
            {
                contRiegos++;
                dtRiesgosRow dtRiegRow = dsActa.dtRiesgos.NewdtRiesgosRow();

                dtRiegRow.NumCtrl = contRiegos.ToString();
                dtRiegRow.Riesgo = i.Riesgo;
                dtRiegRow.Mitigacion = i.Mitigacion;
                int codRiesgo = i.idRiesgoInterventoria;
                string gestionEmp = actaSeguimRiesgosController
                                    .GetGestionEmprendedorByIdRiesgoByActa(codRiesgo, _numActa);
                dtRiegRow.gestionEmp = gestionEmp;

                dsActa.dtRiesgos.AdddtRiesgosRow(dtRiegRow);
            }

            #endregion

            #region Metas Empleos

            int totalMetasEmp = 0;

            var MetasEmpleos = metasEmpleoController
                .ListMetasEmpleo(_codProyecto, _codConvocatoria, ref totalMetasEmp);

            foreach (var i in MetasEmpleos)
            {
                dtMetaEmpleoRow dtMetaEmpRow = dsActa.dtMetaEmpleo.NewdtMetaEmpleoRow();

                dtMetaEmpRow.Cantidad = i.Unidades;
                dtMetaEmpRow.Cargo = i.Cargo;
                dtMetaEmpRow.Condicion = i.Condicion;

                dsActa.dtMetaEmpleo.AdddtMetaEmpleoRow(dtMetaEmpRow);
            }

            #endregion

            #region Empleos

            var indicadorEmpleo = empleoController
                .GetGestionEmploByCodProyectoByCodConvocatoria(_codProyecto, _codConvocatoria)
                .Where(x => x.Visita <= _numActa).ToList();

            foreach (var i in indicadorEmpleo)
            {
                dtEmpleoRow dtEmpRow = dsActa.dtEmpleo.NewdtEmpleoRow();

                dtEmpRow.Visita = i.Visita.ToString(); ;
                dtEmpRow.VerificaIndicador = i.verificaIndicador.ToString();
                dtEmpRow.DesarrolloIndicador = i.desarrolloIndicador;

                dsActa.dtEmpleo.AdddtEmpleoRow(dtEmpRow);
            }

            #endregion

            #region Datos ejecucion presupuestal

            dtDatosEjecPresupestalRow dtDatosEjecPreRow = dsActa.dtDatosEjecPresupestal.NewdtDatosEjecPresupestalRow();

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                var query = (from E in db.EvaluacionObservacions
                             where E.CodConvocatoria == _codConvocatoria && E.CodProyecto == _codProyecto
                             select new { E.ValorRecomendado }).FirstOrDefault();

                var SMLV = query.ValorRecomendado.HasValue ?
                                    query.ValorRecomendado.Value.ToString() : "0";

                dtDatosEjecPreRow.SMLV = SMLV;

                DateTime qAno = (from C in db.Convocatoria
                                 where C.Id_Convocatoria == _codConvocatoria
                                 orderby C.Id_Convocatoria descending
                                 select C.FechaInicio).FirstOrDefault();

                int Ano = qAno.Year;
                dtDatosEjecPreRow.Ano = Ano.ToString();
                var qSalario = (from S in db.SalariosMinimos
                                where S.AñoSalario == Ano
                                orderby S.Id_SalariosMinimos descending
                                select S.SalarioMinimo).FirstOrDefault();

                dtDatosEjecPreRow.ValorPesos = (Convert.ToInt64(SMLV) * qSalario).ToString("C");

            }

            dsActa.dtDatosEjecPresupestal.AdddtDatosEjecPresupestalRow(dtDatosEjecPreRow);

            #endregion

            #region Hist ejecPresupuestal

            var histEjecucion = infoPagosController.getHistoricoEjecucion(_codProyecto, _codConvocatoria)
                                    .Where(x => x.visita <= _numActa).ToList();

            decimal sum = histEjecucion.Sum(x => x.Valor);

            foreach (var i in histEjecucion)
            {
                dtHistEjecPresupuestalRow dtEjecPresRow = dsActa.dtHistEjecPresupuestal.NewdtHistEjecPresupuestalRow();

                dtEjecPresRow.Visita = i.visita.ToString();
                dtEjecPresRow.idPagoActividad = i.idPagoActividad.ToString();
                dtEjecPresRow.Actividad = i.Actividad;
                dtEjecPresRow.Valor = i.Valor.ToString("C");
                dtEjecPresRow.Concepto = i.Concepto;
                dtEjecPresRow.verificoDocumentos = i.verificoDocumentos;
                dtEjecPresRow.verificoActivosEstado = i.verificoActivosEstado;
                dtEjecPresRow.Observacion = i.Observacion;
                dtEjecPresRow.TotalDesembolsado = sum.ToString("C");

                dsActa.dtHistEjecPresupuestal.AdddtHistEjecPresupuestalRow(dtEjecPresRow);
            }

            #endregion

            #region Inventario

            var inventario = infoPagosController.getInventarioContrato(_codProyecto, _codConvocatoria)
                .Where(x => x.visita <= _numActa).ToList();

            decimal suma = inventario.Sum(x => x.valorActivos);

            foreach (var i in inventario)
            {
                dtInventarioRow dtInvRow = dsActa.dtInventario.NewdtInventarioRow();

                dtInvRow.Visita = i.visita.ToString();
                dtInvRow.descripcionRecursos = i.descripcionRecursos.ToString();
                dtInvRow.valorActivos = i.valorActivos.ToString("C");
                dtInvRow.fechaCargaAnexo = i.fechaCargaAnexo.ToShortDateString();
                dtInvRow.ActivosPrendables = suma.ToString("C");

                dsActa.dtInventario.AdddtInventarioRow(dtInvRow);
            }


            #endregion

            #region Aportes Emprendedor

            string AportEmp = metaAporteEmprendedor(_numActa, _codProyecto, _codConvocatoria);

            var aportes = infoPagosController.getAporteEmp(_codProyecto, _codConvocatoria).Where(x => x.visita <= _numActa).ToList();

            foreach (var i in aportes)
            {
                dtAportesEmprendedorRow dtAporEmpRow = dsActa.dtAportesEmprendedor.NewdtAportesEmprendedorRow();

                dtAporEmpRow.visita = i.visita.ToString();
                dtAporEmpRow.descripcion = i.descripcion;
                dtAporEmpRow.MetaAporteEmp = AportEmp;


                dsActa.dtAportesEmprendedor.AdddtAportesEmprendedorRow(dtAporEmpRow);
            }

            #endregion

            #region Metas Mercadeo

            int sumMetas = 0;
            var consulta = metasActividadController.ListMetasMercadeoInterventoria(_codProyecto, _codConvocatoria, ref sumMetas);

            foreach (var i in consulta)
            {
                dtMetasMercadeoRow dtMEtaMErcRow = dsActa.dtMetasMercadeo.NewdtMetasMercadeoRow();

                dtMEtaMErcRow.Unidades = i.Unidades.ToString();
                dtMEtaMErcRow.Actividad = i.Actividad;
                dtMEtaMErcRow.sumMetas = sumMetas.ToString(); ;

                dsActa.dtMetasMercadeo.AdddtMetasMercadeoRow(dtMEtaMErcRow);
            }

            #endregion

            #region Gestion Mercadeo

            var gestion = actaSeguimGestionMercadeoController.GetGestionMercadeo(_codProyecto, _codConvocatoria)
                .Where(x => x.visita <= _numActa).ToList();

            foreach (var i in gestion)
            {
                dtGestionMercadeoRow dtGesMErcRow = dsActa.dtGestionMercadeo.NewdtGestionMercadeoRow();

                dtGesMErcRow.cantidad = i.cantidad.ToString();
                dtGesMErcRow.visita = i.visita.ToString();
                dtGesMErcRow.descripcionEvento = i.descripcionEvento;
                dtGesMErcRow.publicidadLogos = i.publicidadLogos;

                dsActa.dtGestionMercadeo.AdddtGestionMercadeoRow(dtGesMErcRow);
            }

            #endregion

            #region Contrapartidas

            var gestionContrapartidas = contrapartidaController.GetContrapartidas(_codProyecto, _codConvocatoria)
                .Where(x => x.visita <= _numActa).ToList();

            foreach (var i in gestionContrapartidas)
            {
                dtContrapartidasRow dtContraRow = dsActa.dtContrapartidas.NewdtContrapartidasRow();

                dtContraRow.visita = i.visita.ToString();
                dtContraRow.cantContrapartida = i.cantContrapartida.ToString();
                dtContraRow.descripcion = i.descripcion;

                dsActa.dtContrapartidas.AdddtContrapartidasRow(dtContraRow);
            }

            #endregion

            #region Meta Produccion

            var metaProduccion = metasProduccionController.ListAllMetasProduccionInterventoria(_codProyecto, _codConvocatoria);

            foreach (var i in metaProduccion)
            {
                dtMetaProduccionRow dtMetaProRow = dsActa.dtMetaProduccion.NewdtMetaProduccionRow();

                dtMetaProRow.Cantidad = i.Cantidad;
                dtMetaProRow.NomProducto = i.NomProducto;

                dsActa.dtMetaProduccion.AdddtMetaProduccionRow(dtMetaProRow);
            }

            #endregion

            #region Gestion Produccion

            var gestionProduccion = produccionController.GetGestionProduccion(_codProyecto, _codConvocatoria)
                    .Where(x => x.visita <= _numActa).ToList();

            foreach (var i in gestionProduccion)
            {
                dtGestionProduccionRow dtGesProRow = dsActa.dtGestionProduccion.NewdtGestionProduccionRow();

                dtGesProRow.visita = i.visita.ToString();
                dtGesProRow.cantidad = i.cantidad.ToString();
                dtGesProRow.Descripcion = i.Descripcion;
                dtGesProRow.Producto = i.NomProducto;
                dtGesProRow.Medida = i.medida;

                dsActa.dtGestionProduccion.AdddtGestionProduccionRow(dtGesProRow);
            }

            #endregion

            #region Gestion Ventas

            var gestionVentas = gestionVentasController.GetGestionVentas(_codProyecto, _codConvocatoria).Where(x => x.visita <= _numActa).ToList();

            string ventaMeta = MetaVenta(CodigoProyecto, _codConvocatoria).ToString("C");

            foreach (var i in gestionVentas)
            {
                dtGestionVentasRow dtGesVentasRow = dsActa.dtGestionVentas.NewdtGestionVentasRow();

                dtGesVentasRow.visita = i.visita.ToString();
                dtGesVentasRow.valorFormato = i.valorFormato;
                dtGesVentasRow.descripcion = i.descripcion;
                dtGesVentasRow.MetaVentas = ventaMeta;

                dsActa.dtGestionVentas.AdddtGestionVentasRow(dtGesVentasRow);
            }

            #endregion

            #region Oblicacion Contable

            var obligacion = obligContablesController.GetObligContable(_codProyecto, _codConvocatoria).Where(x => x.visita <= _numActa).ToList();

            foreach (var i in obligacion)
            {
                dtObligContablesRow dtObligRow = dsActa.dtObligContables.NewdtObligContablesRow();

                dtObligRow.visita = i.visita.ToString();
                dtObligRow.observObligacionContable = i.observObligacionContable;
                dtObligRow.librosContabilidad = i.librosContabilidad;
                dtObligRow.librosComerciales = i.librosComerciales;
                dtObligRow.estadosFinancieros = i.estadosFinancieros;
                dtObligRow.cuentaBancaria = i.cuentaBancaria;
                dtObligRow.conciliacionBancaria = i.conciliacionBancaria;

                dsActa.dtObligContables.AdddtObligContablesRow(dtObligRow);
            }

            #endregion

            #region Obligacion Tributaria

            var obligacionTributaria = obligContablesController.GetObligTributaria(_codProyecto, _codConvocatoria).Where(x => x.visita <= _numActa).ToList();

            foreach (var i in obligacionTributaria)
            {
                dtObligTributariaRow dtObligTribuRow = dsActa.dtObligTributaria.NewdtObligTributariaRow();

                dtObligTribuRow.visita = i.visita.ToString();
                dtObligTribuRow.observObligacionTributaria = i.observObligacionTributaria;
                dtObligTribuRow.declaRetencionImpIndusComercio = i.declaRetencionImpIndusComercio;
                dtObligTribuRow.declaRenta = i.declaRenta;
                dtObligTribuRow.declaraReteFuente = i.declaraReteFuente;
                dtObligTribuRow.declaraIva = i.declaraIva;
                dtObligTribuRow.declaInfoExogena = i.declaInfoExogena;
                dtObligTribuRow.declaIndustriaComercio = i.declaIndustriaComercio;
                dtObligTribuRow.declaImpConsumo = i.declaImpConsumo;
                dtObligTribuRow.autorretencionRenta = i.autorretencionRenta;

                dsActa.dtObligTributaria.AdddtObligTributariaRow(dtObligTribuRow);
            }

            #endregion

            #region Obligacion Laboral

            var obligacionLaboral = obligContablesController.GetObligLaboral(_codProyecto, _codConvocatoria).Where(x => x.visita <= _numActa).ToList();

            foreach (var i in obligacionLaboral)
            {
                dtObligLaboralesRow dtObligLaboralRow = dsActa.dtObligLaborales.NewdtObligLaboralesRow();

                dtObligLaboralRow.visita = i.visita.ToString();
                dtObligLaboralRow.sisGestionSegSaludTrabajo = i.sisGestionSegSaludTrabajo;
                dtObligLaboralRow.reglaInternoTrab = i.reglaInternoTrab;
                dtObligLaboralRow.pagosNomina = i.pagosNomina;
                dtObligLaboralRow.pagoPrestacionesSociales = i.pagoPrestacionesSociales;
                dtObligLaboralRow.pagoSegSocial = i.pagoSegSocial;
                dtObligLaboralRow.observObligacionLaboral = i.observObligacionLaboral;
                dtObligLaboralRow.contratosLaborales = i.contratosLaborales;
                dtObligLaboralRow.certParafiscalesSegSocial = i.certParafiscalesSegSocial;
                dtObligLaboralRow.afiliacionSegSocial = i.afiliacionSegSocial;

                dsActa.dtObligLaborales.AdddtObligLaboralesRow(dtObligLaboralRow);
            }

            #endregion

            #region Verifica Tramite

            var obligacionVerificaTramite = obligContablesController.GetObligTramite(_codProyecto, _codConvocatoria)
                                                .Where(x => x.visita <= _numActa).ToList()
                                                ;

            foreach (var i in obligacionVerificaTramite)
            {
                dtVerficaTramitesRow dtVerficaTraRow = dsActa.dtVerficaTramites.NewdtVerficaTramitesRow();

                dtVerficaTraRow.visita = i.visita.ToString();
                dtVerficaTraRow.rut = i.rut;
                dtVerficaTraRow.resolFacturacion = i.resolFacturacion;
                dtVerficaTraRow.renovaRegistroMercantil = i.renovaRegistroMercantil;
                dtVerficaTraRow.regMarca = i.regMarca;
                dtVerficaTraRow.permisoUsoSuelo = i.permisoUsoSuelo;
                dtVerficaTraRow.otrosPermisos = i.otrosPermisos;
                dtVerficaTraRow.observRegistroTramiteLicencia = i.observRegistroTramiteLicencia;
                dtVerficaTraRow.insCamaraComercio = i.insCamaraComercio;
                dtVerficaTraRow.certLibertadTradicion = i.certLibertadTradicion;
                dtVerficaTraRow.certBomberos = i.certBomberos;
                dtVerficaTraRow.documentoIdoneidad = i.DocumentoIdoneidad;
                dtVerficaTraRow.contratoArrendamiento = i.contratoArrendamiento;

                dsActa.dtVerficaTramites.AdddtVerficaTramitesRow(dtVerficaTraRow);
            }

            #endregion

            #region Compnente Innovador

            var descripOtrosAps = otrosAspectosController.getDescripcionOtrosAspectos(_codProyecto, _codConvocatoria);

            string CompAmbiental = "";
            string CompInnovador = "";

            if (descripOtrosAps == null)
            {
                CompAmbiental = "";
                CompInnovador = "";
            }
            else
            {
                CompAmbiental = descripOtrosAps.DescripCompAmbiental;
                CompInnovador = descripOtrosAps.DescripCompInnovador;
            }

            var innovacion = otrosAspectosController.getOtrosAspectosInnovador(_codProyecto, _codConvocatoria)
                                .Where(x => x.visita <= _numActa).ToList();

            if (innovacion.Count == 0)
            {
                dtCompInnovadorRow dCompInnovRow = dsActa.dtCompInnovador.NewdtCompInnovadorRow();

                dCompInnovRow.visita = "";
                dCompInnovRow.valoracion = "";
                dCompInnovRow.observacion = "";
                dCompInnovRow.Meta = CompInnovador;

                dsActa.dtCompInnovador.AdddtCompInnovadorRow(dCompInnovRow);
            }

            foreach (var i in innovacion)
            {
                dtCompInnovadorRow dCompInnovRow = dsActa.dtCompInnovador.NewdtCompInnovadorRow();

                dCompInnovRow.visita = i.visita.ToString();
                dCompInnovRow.valoracion = i.valoracion;
                dCompInnovRow.observacion = i.observacion;
                dCompInnovRow.Meta = CompInnovador;

                dsActa.dtCompInnovador.AdddtCompInnovadorRow(dCompInnovRow);
            }

            #endregion

            #region COmponente Ambiental

            var ambiental = otrosAspectosController.getOtrosAspectosAmbiental(_codProyecto, _codConvocatoria)
                                .Where(x => x.visita <= _numActa).ToList();

            if (ambiental.Count == 0)
            {
                dtCompAmbientalRow dCompAmbiRow = dsActa.dtCompAmbiental.NewdtCompAmbientalRow();

                dCompAmbiRow.visita = "";
                dCompAmbiRow.valoracion = "";
                dCompAmbiRow.observacion = "";
                dCompAmbiRow.Meta = CompAmbiental;

                dsActa.dtCompAmbiental.AdddtCompAmbientalRow(dCompAmbiRow);
            }

            foreach (var i in ambiental)
            {
                dtCompAmbientalRow dCompAmbiRow = dsActa.dtCompAmbiental.NewdtCompAmbientalRow();

                dCompAmbiRow.visita = i.visita.ToString();
                dCompAmbiRow.valoracion = i.valoracion;
                dCompAmbiRow.observacion = i.observacion;
                dCompAmbiRow.Meta = CompAmbiental;

                dsActa.dtCompAmbiental.AdddtCompAmbientalRow(dCompAmbiRow);
            }

            #endregion

            #region Cargar descripciones Punto 6

            string AcompAsesoria = "";
            string DedicacionEmprendedor = "";
            string InformacionPlataforma = "";

            var descripOtrasOblig = otrasObligacionesController.getDescripcionOtrasObligaciones(_codProyecto, _codConvocatoria);

            if (descripOtrasOblig == null)
            {
                AcompAsesoria = "";
                DedicacionEmprendedor = "";
                InformacionPlataforma = "";
            }
            else
            {
                AcompAsesoria = descripOtrasOblig.DescripAcomAsesoria;
                DedicacionEmprendedor = descripOtrasOblig.DescripTiempoEmprendedor;
                InformacionPlataforma = descripOtrasOblig.DescripInfoPlataforma;
            }

            if (DedicacionEmprendedor == "")
            {
                DedicacionEmprendedor = otrasObligacionesController.GetPerfilEmprendedor(CodigoProyecto, Constantes.CONST_RolEmprendedor);
            }

            #endregion

            #region Reporte Info Plataforma

            var infoPlataforma = otrasObligacionesController
           .GetOtrasObligInfoPlataforma(_codProyecto, _codConvocatoria)
           .Where(x => x.visita <= _numActa).ToList();

            if (infoPlataforma.Count == 0)
            {
                dtReportInfoPlataformaRow dtReportRow = dsActa.dtReportInfoPlataforma.NewdtReportInfoPlataformaRow();

                dtReportRow.visita = "";
                dtReportRow.valoracion = "";
                dtReportRow.observacion = "";
                dtReportRow.Descripcion = InformacionPlataforma;

                dsActa.dtReportInfoPlataforma.AdddtReportInfoPlataformaRow(dtReportRow);
            }

            foreach (var i in infoPlataforma)
            {
                dtReportInfoPlataformaRow dtReportRow = dsActa.dtReportInfoPlataforma.NewdtReportInfoPlataformaRow();

                dtReportRow.visita = i.visita.ToString();
                dtReportRow.valoracion = i.valoracion;
                dtReportRow.observacion = i.observacion;
                dtReportRow.Descripcion = InformacionPlataforma;

                dsActa.dtReportInfoPlataforma.AdddtReportInfoPlataformaRow(dtReportRow);
            }

            #endregion

            #region Tiempo dedicacion

            var dedicaEmp = otrasObligacionesController
               .GetOtrasObligDedicaEmprendedor(_codProyecto, _codConvocatoria)
               .Where(x => x.visita <= _numActa).ToList();

            if (dedicaEmp.Count == 0)
            {
                dtTiempoDedicacionRow dtTiempoDedRow = dsActa.dtTiempoDedicacion.NewdtTiempoDedicacionRow();

                dtTiempoDedRow.visita = "";
                dtTiempoDedRow.valoracion = "";
                dtTiempoDedRow.observacion = "";
                dtTiempoDedRow.descripcion = DedicacionEmprendedor;

                dsActa.dtTiempoDedicacion.AdddtTiempoDedicacionRow(dtTiempoDedRow);
            }

            foreach (var i in dedicaEmp)
            {
                dtTiempoDedicacionRow dtTiempoDedRow = dsActa.dtTiempoDedicacion.NewdtTiempoDedicacionRow();

                dtTiempoDedRow.visita = i.visita.ToString();
                dtTiempoDedRow.valoracion = i.valoracion;
                dtTiempoDedRow.observacion = i.observacion;
                dtTiempoDedRow.descripcion = DedicacionEmprendedor;

                dsActa.dtTiempoDedicacion.AdddtTiempoDedicacionRow(dtTiempoDedRow);
            }

            #endregion

            #region Acompañamiento y Asesoria

            var acomAsesoria = otrasObligacionesController
                 .GetOtrasObligAcomAsesoria(_codProyecto, _codConvocatoria)
                 .Where(x => x.visita <= _numActa).ToList();

            if (acomAsesoria.Count == 0)
            {
                dtAcomAsesoriaRow dtAcomAseRow = dsActa.dtAcomAsesoria.NewdtAcomAsesoriaRow();

                dtAcomAseRow.visita = "";
                dtAcomAseRow.valoracion = "";
                dtAcomAseRow.observacion = "";
                dtAcomAseRow.descripcion = AcompAsesoria;

                dsActa.dtAcomAsesoria.AdddtAcomAsesoriaRow(dtAcomAseRow);
            }

            foreach (var i in acomAsesoria)
            {
                dtAcomAsesoriaRow dtAcomAseRow = dsActa.dtAcomAsesoria.NewdtAcomAsesoriaRow();

                dtAcomAseRow.visita = i.visita.ToString();
                dtAcomAseRow.valoracion = i.valoracion;
                dtAcomAseRow.observacion = i.observacion;
                dtAcomAseRow.descripcion = AcompAsesoria;

                dsActa.dtAcomAsesoria.AdddtAcomAsesoriaRow(dtAcomAseRow);
            }

            #endregion

            #region Estado Empresa

            var estadoEmpresa = estadoEmpresaController
                .getListEstadoEmpresa(_codProyecto, _codConvocatoria)
                .Where(x => x.visita <= _numActa).ToList();

            foreach (var i in estadoEmpresa)
            {
                dtEstadoEmpresaRow dtEstadoEmpRow = dsActa.dtEstadoEmpresa.NewdtEstadoEmpresaRow();

                dtEstadoEmpRow.visita = i.visita.ToString();
                dtEstadoEmpRow.descripcion = i.Descripcion;

                dsActa.dtEstadoEmpresa.AdddtEstadoEmpresaRow(dtEstadoEmpRow);
            }


            #endregion

            #region Compromisos

            var histCompromisos = compromisosController.getAllCompromisos(_codProyecto, _codConvocatoria)
                .Where(x => x.visita <= _numActa).ToList();

            foreach (var i in histCompromisos)
            {
                dtCompromisosRow dtCompRow = dsActa.dtCompromisos.NewdtCompromisosRow();

                dtCompRow.visita = i.visita.ToString();
                dtCompRow.compromiso = i.compromiso;
                dtCompRow.fechaPropuestaEjec = i.fechaPropuestaEjec.ToShortDateString();
                dtCompRow.estado = i.estado;
                dtCompRow.fechaCumpliCompromiso = i.fechaCumpliCompromiso.HasValue ?
                                           i.fechaCumpliCompromiso.Value.ToShortDateString() : "";
                dtCompRow.observacion = i.observacion;

                dsActa.dtCompromisos.AdddtCompromisosRow(dtCompRow);
            }

            #endregion

            #region Informacion Firmas

            var actaInterventor = interventoriaController.GetActaInterventoria(_codProyecto, _numActa);

            dtPublicacionActaRow dtPublicActaRow = dsActa.dtPublicacionActa.NewdtPublicacionActaRow();

            dtPublicActaRow.FechaPublicacion = actaInterventor.FechaFinalVisita.HasValue ?
                                            actaInterventor.FechaFinalVisita.Value.ToShortDateString() : "";

            dtPublicActaRow.HoraPublicacion = actaInterventor.FechaFinalVisita.HasValue ?
                                            actaInterventor.FechaFinalVisita.Value.ToShortTimeString() : "";

            dsActa.dtPublicacionActa.AdddtPublicacionActaRow(dtPublicActaRow);

            var datosContac = datosActaDTOController.GetContactosProyecto(_codProyecto);

            int codRolAsesorLider = 1;//Codigo Asesor Lider
            int codRolAsesor = 2;//Codigo Asesor
            int codRolEmprendedor = 3;//Codigo Emprendedor
            int codRolInterventor = 6;//Codigo Interventor
            int codRolCoordinadorInterventor = 7;//Codigo Coordinador Interventor
            int codRolInterventorLider = 8;//Codigo Interventor Lider

            //Cargar Contratista

            var contratista = datosContac.Where(x => x.codRol == codRolEmprendedor).ToList();

            foreach (var c in contratista)
            {
                dtContratistaRow dtContraRow = dsActa.dtContratista.NewdtContratistaRow();

                dtContraRow.Nombre = c.Nombres.ToUpper() + " " + c.Apellidos.ToUpper();
                dtContraRow.Rol = "Contratista/Emprendedor";
                dtContraRow.Correo = c.Email;
                dtContraRow.Telefono = c.Telefono;

                dsActa.dtContratista.AdddtContratistaRow(dtContraRow);
            }

            dtDatosFirmasRow dtDatosFirRow = dsActa.dtDatosFirmas.NewdtDatosFirmasRow();

            //Cargar info ASESOR
            var actaInfo = actaSeguimientoDatosController.obtenerDatosActa(_numActa, _codProyecto);

            if (!String.IsNullOrEmpty(actaInfo.NombreGestorOperativoSena))
            {
                dtDatosFirRow.NombreGestorOperativo = actaInfo.NombreGestorOperativoSena.ToUpper();
                dtDatosFirRow.CorreoGestorOperativo = actaInfo.EmailGestorOperativoSena;
                dtDatosFirRow.TelefonoGestorOperativo = actaInfo.TelefonoGestorOperativoSena;
            }
            else
            {
                DatosActaDTOController datosActaDTOController = new DatosActaDTOController();
                var datosContacto = datosActaDTOController.GetContactosProyecto(CodigoProyecto);
                var Asesor = datosContacto.Where(x => (x.codRol == Constantes.CONST_RolAsesor || x.codRol == Constantes.CONST_RolAsesorLider)).FirstOrDefault();

                if (Asesor != null)
                {
                    dtDatosFirRow.NombreGestorOperativo = Asesor.Nombres.ToUpper() + " " + Asesor.Apellidos.ToUpper();
                    dtDatosFirRow.CorreoGestorOperativo = Asesor.Email;
                    dtDatosFirRow.TelefonoGestorOperativo = Asesor.Telefono;
                }
                else
                {
                    Alert("El proyecto no cuenta con un asesor asignado.");
                    return;
                }
            }



            //var Asesor = datosContac.Where(x => (x.codRol == codRolAsesor || x.codRol == codRolAsesorLider)).FirstOrDefault();

            //if (Asesor != null)
            //{
            //    dtDatosFirRow.NombreGestorOperativo = Asesor.Nombres.ToUpper() + " " + Asesor.Apellidos.ToUpper();
            //    dtDatosFirRow.CorreoGestorOperativo = Asesor.Email;
            //    dtDatosFirRow.TelefonoGestorOperativo = Asesor.Telefono;
            //}
            //else
            //{
            //    Alert("No se puede generar el acta: El proyecto no cuenta con un asesor asignado.");
            //    return;
            //}           

            //Cargar Info Gestor Tecnico SENA
            dtDatosFirRow.NombreGestorTecnico = actaInfo.NombreGestorTecnicoSena.ToUpper();
            dtDatosFirRow.CorreoGestorTecnico = actaInfo.EmailGestorTecnicoSena;
            dtDatosFirRow.TelefonoGestorTecnico = actaInfo.TelefonoGestorTecnicoSena;

            // Cargar info interventor
            var datosInterventor = datosActaDTOController.GetContactosInterventor(_codProyecto);

            var dataInter = datosInterventor.Where(x => (x.codRol == codRolInterventor)
                                                        || (x.codRol == codRolInterventorLider)
                                                        || (x.codRol == codRolCoordinadorInterventor)).FirstOrDefault();

            dtDatosFirRow.NombreInterventor = dataInter.Nombres.ToUpper() + " " + dataInter.Apellidos.ToUpper();
            dtDatosFirRow.CorreoInterventor = dataInter.Email;
            dtDatosFirRow.TelefonoInterventor = dataInter.Telefono;

            var entidadInterventora = datosActaDTOController.getInfoEntidadInteventora(_codProyecto, usuario.IdContacto);

            dtDatosFirRow.UniversidadInterventor = entidadInterventora.nombreInterventor;

            dsActa.dtDatosFirmas.AdddtDatosFirmasRow(dtDatosFirRow);

            #endregion

            #region NotasActas

            //NotasActas
            var Nota = actaSeguimNotasController
                            .getListNotas(_codProyecto, _codConvocatoria);

            foreach (var n in Nota)
            {
                dtNotasActasRow dtNotActRow = dsActa.dtNotasActas.NewdtNotasActasRow();

                dtNotActRow.Visita = n.visita.ToString() ?? "";
                dtNotActRow.Nota = n.Notas.ToString() ?? "";

                dsActa.dtNotasActas.AdddtNotasActasRow(dtNotActRow);
            }

            #endregion

            //infoGeneral
            ReportDataSource reportDataActaInfo = new ReportDataSource();
            reportDataActaInfo.Value = dsActa.dtInfoGeneral;
            reportDataActaInfo.Name = "dsActaReport";

            //Actas Fechas
            ReportDataSource reportDataActasFechas = new ReportDataSource();
            reportDataActasFechas.Value = dsActa.dtActasFechas;
            reportDataActasFechas.Name = "dsActasFechas";

            //Indicadores y Metas
            ReportDataSource reportDataMetasIndicadores = new ReportDataSource();
            reportDataMetasIndicadores.Value = dsActa.dtIndicadoresMetas;
            reportDataMetasIndicadores.Name = "dsIndicadoresMetas";

            //Riesgos
            ReportDataSource reportDataRiesgos = new ReportDataSource();
            reportDataRiesgos.Value = dsActa.dtRiesgos;
            reportDataRiesgos.Name = "dsRiesgos";

            //Metas Empleos
            ReportDataSource reportDataMetaEmpleo = new ReportDataSource();
            reportDataMetaEmpleo.Value = dsActa.dtMetaEmpleo;
            reportDataMetaEmpleo.Name = "dsMetaEmpleo";

            //Empleos
            ReportDataSource reportDataEmpleo = new ReportDataSource();
            reportDataEmpleo.Value = dsActa.dtEmpleo;
            reportDataEmpleo.Name = "dsEmpleo";

            //Datos ejecPresupuestal
            ReportDataSource reportDataEjecPresupuestal = new ReportDataSource();
            reportDataEjecPresupuestal.Value = dsActa.dtDatosEjecPresupestal;
            reportDataEjecPresupuestal.Name = "dsDatosEjePresupuestal";

            //Hist EjecPResupuestal
            ReportDataSource reportDataHistEjecPresupuestal = new ReportDataSource();
            reportDataHistEjecPresupuestal.Value = dsActa.dtHistEjecPresupuestal;
            reportDataHistEjecPresupuestal.Name = "dsHistEjecPresupuestal";

            //Inventario
            ReportDataSource reportDataInventario = new ReportDataSource();
            reportDataInventario.Value = dsActa.dtInventario;
            reportDataInventario.Name = "dsInventario";

            //Aportes Emprendedor
            ReportDataSource reportDataAporteEmp = new ReportDataSource();
            reportDataAporteEmp.Value = dsActa.dtAportesEmprendedor;
            reportDataAporteEmp.Name = "dsAporteEmprendedor";

            //Metas Mercadeo
            ReportDataSource reportDataMetaMercadeo = new ReportDataSource();
            reportDataMetaMercadeo.Value = dsActa.dtMetasMercadeo;
            reportDataMetaMercadeo.Name = "dsMetaMercadeo";

            //Gestion Mercadeo
            ReportDataSource reportDatagestionMercadeo = new ReportDataSource();
            reportDatagestionMercadeo.Value = dsActa.dtGestionMercadeo;
            reportDatagestionMercadeo.Name = "dsGestionMercadeo";

            //Contrapartidas
            ReportDataSource reportDataContrapartdias = new ReportDataSource();
            reportDataContrapartdias.Value = dsActa.dtContrapartidas;
            reportDataContrapartdias.Name = "dsContrapartidas";

            //Meta Produccion
            ReportDataSource reportDataMetaProduccion = new ReportDataSource();
            reportDataMetaProduccion.Value = dsActa.dtMetaProduccion;
            reportDataMetaProduccion.Name = "dsMetaProduccion";

            //Gestion Produccion
            ReportDataSource reportDataGestionProduccion = new ReportDataSource();
            reportDataGestionProduccion.Value = dsActa.dtGestionProduccion;
            reportDataGestionProduccion.Name = "dsGestionProduccion";

            //Gestion Ventas
            ReportDataSource reportDataGestionVentas = new ReportDataSource();
            reportDataGestionVentas.Value = dsActa.dtGestionVentas;
            reportDataGestionVentas.Name = "dsGestionVentas";

            //Obligacion Contable
            ReportDataSource reportDataObligContable = new ReportDataSource();
            reportDataObligContable.Value = dsActa.dtObligContables;
            reportDataObligContable.Name = "dsObligContable";

            //Obligacion Tributaria
            ReportDataSource reportDataObligTribituaria = new ReportDataSource();
            reportDataObligTribituaria.Value = dsActa.dtObligTributaria;
            reportDataObligTribituaria.Name = "dsObligTributaria";

            //Obligacion Laboral
            ReportDataSource reportDataObligLaboral = new ReportDataSource();
            reportDataObligLaboral.Value = dsActa.dtObligLaborales;
            reportDataObligLaboral.Name = "dsObligLaborales";

            //Verifica Tramites
            ReportDataSource reportDataVerificaTram = new ReportDataSource();
            reportDataVerificaTram.Value = dsActa.dtVerficaTramites;
            reportDataVerificaTram.Name = "dsVerficaTramites";

            //Componente Innovador
            ReportDataSource reportDataCompInnovador = new ReportDataSource();
            reportDataCompInnovador.Value = dsActa.dtCompInnovador;
            reportDataCompInnovador.Name = "dsCompInnovador";

            //Componente Ambiental
            ReportDataSource reportDataCompAmbiental = new ReportDataSource();
            reportDataCompAmbiental.Value = dsActa.dtCompAmbiental;
            reportDataCompAmbiental.Name = "dsCompAmbiental";

            //Reporte Info Plataforma
            ReportDataSource reportDataREportPlataforma = new ReportDataSource();
            reportDataREportPlataforma.Value = dsActa.dtReportInfoPlataforma;
            reportDataREportPlataforma.Name = "dsReportInfoPlataforma";

            //Tiempo Dedicacion
            ReportDataSource reportDataTiempoDedicacion = new ReportDataSource();
            reportDataTiempoDedicacion.Value = dsActa.dtTiempoDedicacion;
            reportDataTiempoDedicacion.Name = "dsTiempoDedicacion";

            //Acompañamiento y Asesoria
            ReportDataSource reportDataAcomAsesoria = new ReportDataSource();
            reportDataAcomAsesoria.Value = dsActa.dtAcomAsesoria;
            reportDataAcomAsesoria.Name = "dsAcomAsesoria";

            //Estado Empresa
            ReportDataSource reportDataEstadoEmpresa = new ReportDataSource();
            reportDataEstadoEmpresa.Value = dsActa.dtEstadoEmpresa;
            reportDataEstadoEmpresa.Name = "dsEstadoEmpresa";

            //Compromisos
            ReportDataSource reportDataCompromiso = new ReportDataSource();
            reportDataCompromiso.Value = dsActa.dtCompromisos;
            reportDataCompromiso.Name = "dsCompromisos";

            //NotasActas
            ReportDataSource reportDataNotasActas = new ReportDataSource();
            reportDataNotasActas.Value = dsActa.dtNotasActas;
            reportDataNotasActas.Name = "dsNotasActas";

            //Informacion Firmas
            //Contratista
            ReportDataSource reportDataContratista = new ReportDataSource();
            reportDataContratista.Value = dsActa.dtContratista;
            reportDataContratista.Name = "dsContratista";
            //Datos Firmas
            ReportDataSource reportDataFirmas = new ReportDataSource();
            reportDataFirmas.Value = dsActa.dtDatosFirmas;
            reportDataFirmas.Name = "dsDatosFirmas";
            //FEchaPublicacion ACta
            ReportDataSource reportDataPublicacion = new ReportDataSource();
            reportDataPublicacion.Value = dsActa.dtPublicacionActa;
            reportDataPublicacion.Name = "dsPublicacionActa";

            LocalReport report = new LocalReport();

            report.DataSources.Add(reportDataActaInfo);
            report.DataSources.Add(reportDataActasFechas);
            report.DataSources.Add(reportDataMetasIndicadores);
            report.DataSources.Add(reportDataRiesgos);
            report.DataSources.Add(reportDataMetaEmpleo);
            report.DataSources.Add(reportDataEmpleo);
            report.DataSources.Add(reportDataEjecPresupuestal);
            report.DataSources.Add(reportDataHistEjecPresupuestal);
            report.DataSources.Add(reportDataInventario);
            report.DataSources.Add(reportDataAporteEmp);
            report.DataSources.Add(reportDataMetaMercadeo);
            report.DataSources.Add(reportDatagestionMercadeo);
            report.DataSources.Add(reportDataContrapartdias);
            report.DataSources.Add(reportDataMetaProduccion);
            report.DataSources.Add(reportDataGestionProduccion);
            report.DataSources.Add(reportDataGestionVentas);
            report.DataSources.Add(reportDataObligContable);
            report.DataSources.Add(reportDataObligTribituaria);
            report.DataSources.Add(reportDataObligLaboral);
            report.DataSources.Add(reportDataVerificaTram);
            report.DataSources.Add(reportDataCompInnovador);
            report.DataSources.Add(reportDataCompAmbiental);
            report.DataSources.Add(reportDataREportPlataforma);
            report.DataSources.Add(reportDataTiempoDedicacion);
            report.DataSources.Add(reportDataAcomAsesoria);
            report.DataSources.Add(reportDataEstadoEmpresa);
            report.DataSources.Add(reportDataCompromiso);
            report.DataSources.Add(reportDataContratista);
            report.DataSources.Add(reportDataFirmas);
            report.DataSources.Add(reportDataPublicacion);
            report.DataSources.Add(reportDataNotasActas);

            report.EnableExternalImages = true;

            if (borrador)
            {
                report.ReportPath = @"PlanDeNegocioV2\Administracion\Interventoria\ActasDeSeguimiento\Report\ActaSeguimientoBorrador.rdlc";
            }
            else
            {
                report.ReportPath = @"PlanDeNegocioV2\Administracion\Interventoria\ActasDeSeguimiento\Report\ActaSeguimiento.rdlc";
            }


            string path = string.Concat(baseDirectory + entidadInterv.rutaLogo.Trim());//"M:\\ baseDirectory" <-Produccion

            //get Nombre del operador
            var operador = operadorController.getOperador(usuario.CodOperador);

            string pathLogo = string.Concat(baseDirectory + operador.Rutalogo.Trim());//"M:\\ baseDirectory" <-Produccion

            string nombreOperador = operador.NombreOperador.ToUpper();

            //var Nota = actaSeguimNotasController
            //                .getListNotas(_codProyecto, _codConvocatoria)
            //                .Where(x=>x.numActa == _numActa).FirstOrDefault();
            //string notaValor = "";
            //if (Nota != null)
            //{
            //    notaValor = Nota.Notas;
            //}
            string notaValor = "";

            ReportParameter[] param = new ReportParameter[5];
            param[0] = new ReportParameter("rpUrlImage", path);
            param[1] = new ReportParameter("rpNumVisita", numeroVisita.ToString());
            param[2] = new ReportParameter("rpNotas", notaValor);
            param[3] = new ReportParameter("rpLogoOperador", pathLogo);
            param[4] = new ReportParameter("rpNombreOperador", nombreOperador);
            report.SetParameters(param);



            byte[] fileBytes = report.Render("PDF");
            MemoryStream ms = new MemoryStream(fileBytes, 0, 0, true, true);
            Response.AddHeader("content-disposition", "attachment;filename= ActaSeguimiento_" + _numActa + "_" + _codProyecto + ".pdf");
            Response.Buffer = true;
            Response.Clear();
            Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.End();
        }

        private decimal MetaVenta(int _codProyecto, int _codConvocatoria)
        {
            decimal meta = 0;

            var listmetas = metasVentasController.ListMetasVentas(_codProyecto, _codConvocatoria);

            meta = listmetas.Select(x => x.ventas).FirstOrDefault();

            return meta;
        }

        public List<IndicadoresActasSeguimiento> GetIndicadores(int codigoProyecto, ref IndicadoresEjecucionActasSeguimiento indicadoresActas, int _numActa = 0)
        {
            try
            {
                var indicadores = new List<IndicadoresActasSeguimiento>();

                var codigoConvocatoria = Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(codigoProyecto, 0).GetValueOrDefault(0);
                var meta = GetMeta(codigoProyecto, codigoConvocatoria);

                indicadores.Add(meta);

                var indiActa = new List<IndicadoresActasSeguimiento>();
                //Agregar Indicadores Actas Seguimiento
                if (_numActa != 0)
                {
                    indiActa = GetListIndicadoresActas(codigoProyecto, codigoConvocatoria)
                   .Where(x => Convert.ToInt32(x.Visita) <= _numActa).ToList();
                }
                else
                {
                    indiActa = GetListIndicadoresActas(codigoProyecto, codigoConvocatoria);
                }


                foreach (var item in indiActa)
                {
                    indicadores.Add(item);
                }
                //Agregar Porcentaje Ejecucion
                indicadoresActas = GetPorcentEjecucion(meta, indiActa, codigoProyecto, codigoConvocatoria);

                return indicadores;
            }
            catch (Exception ex)
            {
                return new List<IndicadoresActasSeguimiento>();
            }
        }
        protected void gvMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            //try
            //{
            if (e.CommandName.Equals("Ver"))
            {
                if (e.CommandArgument != null)
                {
                    string[] data = e.CommandArgument.ToString().Split(';');
                    var idActa = data[0];
                    var idProyecto = data[1];
                    if (Convert.ToInt32(idActa) > 0)//Acta de seguimiento
                    {

                        Session["idProyecto"] = idProyecto;
                        Session["idActa"] = idActa;
                        Response.Redirect("~/PlanDeNegocioV2/Administracion/Interventoria/ActasDeSeguimiento/Master/MainMenuActasSeg.aspx");
                    }
                    else //Acta de inicio
                    {
                        Response.Redirect("~/PlanDeNegocioV2/Administracion/Interventoria/ActasDeSeguimiento/ActaInicio.aspx?acta=" + idActa + "&codigo=" + idProyecto);
                    }

                }
            }
            else if (e.CommandName.Equals("GenerarActa"))
            {
                if (e.CommandArgument != null)
                {
                    var codigoConvocatoria = Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(CodigoProyecto, 0).GetValueOrDefault(0);
                    var id = Convert.ToInt32(e.CommandArgument.ToString());
                    if (id == 0)
                    {
                        //GenerarActa(id);
                        GenerarActaInicio(CodigoProyecto, id, codigoConvocatoria);
                    }
                    else
                    {
                        GenerarActaSeguimiento(CodigoProyecto, id, codigoConvocatoria);
                        //Session["idProyecto"] = CodigoProyecto;
                        //Session["idActa"] = id;
                        //Fonade.Proyecto.Proyectos.Redirect(Response
                        //    , "~/PlanDeNegocioV2/Administracion/Interventoria/ActasDeSeguimiento/Templates/ActaPDF.aspx"
                        //    , "_Blank", "scrollbars=no,width=800,height=800");
                    }

                }
            }
            else if (e.CommandName.Equals("CargarActa"))
            {
                if (e.CommandArgument != null)
                {
                    var id = e.CommandArgument.ToString();
                    if (id == "0")//Acta Inicial
                    {
                        Session["idProyecto"] = CodigoProyecto;
                        Fonade.Proyecto.Proyectos.Redirect(Response, "CargarActa.aspx?acta=" + id, "_Blank", "scrollbars=no,width=600,height=600");
                    }
                    else
                    {
                        Session["idProyecto"] = CodigoProyecto;
                        Fonade.Proyecto.Proyectos.Redirect(Response, "CargarActa.aspx?acta=" + id, "_Blank", "scrollbars=no,width=600,height=600");
                    }
                }
            }
            else if (e.CommandName.Equals("VerDocumento"))
            {
                var archivoActa = e.CommandArgument.ToString();
                string url = baseDirectory + archivoActa;

                Utilidades.DescargarArchivo(url);
            }
            else if (e.CommandName.Equals("GenerarActaBorrador"))
            {
                if (e.CommandArgument != null)
                {
                    var codigoConvocatoria = Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(CodigoProyecto, 0).GetValueOrDefault(0);
                    var id = Convert.ToInt32(e.CommandArgument.ToString());
                    if (id == 0)
                    {
                        //GenerarActa(id);
                        GenerarActaInicio(CodigoProyecto, id, codigoConvocatoria, true);//true para generar el borrador
                    }
                    else
                    {
                        GenerarActaSeguimiento(CodigoProyecto, id, codigoConvocatoria, true);//true para generar el borrador
                    }

                }
            }
            //}
            //catch (Exception ex)
            //{
            //    lblError.Visible = true;
            //    lblError.Text = "Sucedio un error detalle :" + ex.Message;
            //}
        }

        ProyectoController ProyectoController = new ProyectoController();

        private void GenerarActaInicio(int _codProyecto, int _numActa, int _codConvocatoria, bool borrador = false)
        {
            dsActaInicio dsActa = new dsActaInicio();

            var actaInicio = Negocio.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimientos.ActaSeguimiento.GetActaById(_numActa, _codProyecto);

            var contrato = Negocio.PlanDeNegocioV2.Administracion.Interventoria.Abogado.GetContratoByProyecto(CodigoProyecto, usuario.CodOperador);

            var infoContrato = contrato.First().Contrato;

            var emprendedores = Negocio.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimientos.ActaSeguimiento.GetEmprendedoresYEquipoTrabajo(CodigoProyecto);

            var convenioConvocatoriaProyecto = ProyectoController.getConvenioXProyecto(_codProyecto);

            string contratistas = string.Empty;
            foreach (var emprendedor in emprendedores)
            {
                if (string.IsNullOrEmpty(contratistas))
                {
                    contratistas += emprendedor.Nombres + "-" + emprendedor.Identificacion;
                }
                else
                {
                    contratistas += "," + emprendedor.Nombres + "-" + emprendedor.Identificacion;
                }

                //firmasUsuarios.Add(tdContainer
                //                   .Replace("[USUARIO]", emprendedor.Nombres)
                //                   .Replace("[ROL]", "Contratista"));
            }

            //get Nombre del operador
            var operador = operadorController.getOperador(usuario.CodOperador);

            //TipoActa
            dsActaInicio.dtTipoActaRow dtTipoRow = dsActa.dtTipoActa.NewdtTipoActaRow();

            dtTipoRow.Codigo = actaInicio.TipoActaSeguimiento.Codigo;
            dtTipoRow.Version = actaInicio.TipoActaSeguimiento.Version;
            dtTipoRow.Vigencia = actaInicio.TipoActaSeguimiento.Vigencia;
            dtTipoRow.Operador = operador.NombreOperador;

            dsActa.dtTipoActa.AdddtTipoActaRow(dtTipoRow);

            //Datos
            dsActaInicio.dtDatosActaRow dtDatoRow = dsActa.dtDatosActa.NewdtDatosActaRow();

            string cedulasEmprendedores = string.Empty;
            string nombresEmprendedoresDatos = string.Empty;

            foreach (var emprendedor in emprendedores)
            {
                nombresEmprendedoresDatos += emprendedor.Nombres + ", ";
                cedulasEmprendedores += emprendedor.Identificacion + ", ";
            }
            nombresEmprendedoresDatos = nombresEmprendedoresDatos.Remove(nombresEmprendedoresDatos.Length - 2);
            cedulasEmprendedores = cedulasEmprendedores.Remove(cedulasEmprendedores.Length - 2);

            dtDatoRow.NumContrato = infoContrato.NumeroContrato.Trim();
            dtDatoRow.TipoContrato = infoContrato.TipoContrato;
            dtDatoRow.Objeto = infoContrato.ObjetoContrato;
            dtDatoRow.Valor = FieldValidate.moneyFormat((double)infoContrato.ValorInicialEnPesos);
            dtDatoRow.Contratista = contratistas;
            dtDatoRow.Plazo = infoContrato.PlazoContratoMeses.ToString().Trim() + " Meses";
            dtDatoRow.CodProyecto = _codProyecto.ToString();
            dtDatoRow.NumActaRecursos = ProyectoController.buscarNumActaXProyecto(_codProyecto);
            dtDatoRow.ValorPalabras = valorEnPalabras((double)infoContrato.ValorInicialEnPesos);
            dtDatoRow.NombresEmprendedores = nombresEmprendedoresDatos.ToUpper();
            dtDatoRow.CedulasEmprendedores = cedulasEmprendedores;
            dtDatoRow.EntidadInterventorDesignado = ProyectoController.buscarEntidadInterventoria(_codProyecto).ToUpper();
            dtDatoRow.lugarEjecucion = ProyectoController.ciudadYDepartamento(_codProyecto);
            dtDatoRow.CiudadEjecucion = ProyectoController.ciudadxProyecto(_codProyecto);
            dtDatoRow.NombreInterventor = actaInicio.Contacto.Nombres.ToUpper() + " " + actaInicio.Contacto.Apellidos.ToUpper();
            dtDatoRow.CedulaInterventor = actaInicio.Contacto.Identificacion.ToString();
            if (actaInicio.Contacto.LugarExpedicionDI != null)
            {
                dtDatoRow.LugarExpCedInterventor = " expedida en " + ProyectoController.ciudadxID(actaInicio.Contacto.LugarExpedicionDI);
            }
            else
            {
                dtDatoRow.LugarExpCedInterventor = "";
            }

            var fechaActual = DateTime.Now;
            dtDatoRow.DiaActa = fechaActual.Day.ToString();
            dtDatoRow.MesActa = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(fechaActual.Month).ToUpper();
            dtDatoRow.AnoActa = fechaActual.Year.ToString();

            dsActa.dtDatosActa.AdddtDatosActaRow(dtDatoRow);

            //Fechas

            dsActaInicio.dtFechasRow dtFechaRow = dsActa.dtFechas.NewdtFechasRow();

            var fechaPublicacion = actaInicio.FechaCreacion;
            var fechaTerminacion = actaInicio.FechaCreacion.AddMonths(Convert.ToInt32(infoContrato.PlazoContratoMeses));

            dtFechaRow.DiaInicioActa = fechaPublicacion.Day.ToString();
            dtFechaRow.MesInicioActa = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(fechaPublicacion.Month).ToUpper();
            dtFechaRow.AnoInicioActa = fechaPublicacion.Year.ToString();
            dtFechaRow.DiaFinalActa = fechaTerminacion.Day.ToString();
            dtFechaRow.MesFinalActa = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(fechaTerminacion.Month).ToUpper();
            dtFechaRow.AnoFinalActa = fechaTerminacion.Year.ToString();

            dsActa.dtFechas.AdddtFechasRow(dtFechaRow);

            //Firmas
            foreach (var emprendedor in emprendedores)
            {
                dsActaInicio.dtFirmasRow dtFirmasRowEmprendedor = dsActa.dtFirmas.NewdtFirmasRow();

                dtFirmasRowEmprendedor.Nombres = emprendedor.Nombres.ToUpper();
                dtFirmasRowEmprendedor.Rol = "Contratista\\Emprendedor";

                dsActa.dtFirmas.AdddtFirmasRow(dtFirmasRowEmprendedor);
            }
            dsActaInicio.dtFirmaInterventorRow dtFirmasRow = dsActa.dtFirmaInterventor.NewdtFirmaInterventorRow();

            dtFirmasRow.Nombres = actaInicio.Contacto.Nombres.ToUpper() + " " + actaInicio.Contacto.Apellidos.ToUpper();
            dtFirmasRow.Rol = "Interventor\\Supervisor";

            dsActa.dtFirmaInterventor.AdddtFirmaInterventorRow(dtFirmasRow);

            //Convenio
            dsActaInicio.dtConvenioRow dtConvenioRow = dsActa.dtConvenio.NewdtConvenioRow();
            dtConvenioRow.Ano = convenioConvocatoriaProyecto.Anoinicio;
            dtConvenioRow.Convenio = convenioConvocatoriaProyecto.nomConvenio;

            string nombresEmprendedores = "";
            foreach (var emprendedor in emprendedores)
            {
                nombresEmprendedores = emprendedor.Nombres.ToUpper() + ", ";
            }
            nombresEmprendedores = nombresEmprendedores.Remove(nombresEmprendedores.Length - 2);

            dtConvenioRow.Emprendedores = nombresEmprendedores;

            dsActa.dtConvenio.AdddtConvenioRow(dtConvenioRow);

            //TipoActa
            ReportDataSource reportDataTipoActa = new ReportDataSource();
            reportDataTipoActa.Value = dsActa.dtTipoActa;
            reportDataTipoActa.Name = "dsTipoActa";

            //Datos
            ReportDataSource reportDataDatos = new ReportDataSource();
            reportDataDatos.Value = dsActa.dtDatosActa;
            reportDataDatos.Name = "dsDatosActa";

            //Fecha
            ReportDataSource reportDataFecha = new ReportDataSource();
            reportDataFecha.Value = dsActa.dtFechas;
            reportDataFecha.Name = "dsFechas";

            //Firmas
            ReportDataSource reportDataFirmas = new ReportDataSource();
            reportDataFirmas.Value = dsActa.dtFirmas;
            reportDataFirmas.Name = "dsFirmas";

            //Firmas Interventor
            ReportDataSource reportDataFirmasInterventor = new ReportDataSource();
            reportDataFirmasInterventor.Value = dsActa.dtFirmaInterventor;
            reportDataFirmasInterventor.Name = "dsFirmaInterventor";

            //Convenios
            ReportDataSource reportDataConvenios = new ReportDataSource();
            reportDataConvenios.Value = dsActa.dtConvenio;
            reportDataConvenios.Name = "dsConvenio";

            LocalReport report = new LocalReport();

            //Agregar los report DataSource
            report.DataSources.Add(reportDataTipoActa);
            report.DataSources.Add(reportDataDatos);
            report.DataSources.Add(reportDataFecha);
            report.DataSources.Add(reportDataFirmas);
            report.DataSources.Add(reportDataFirmasInterventor);
            report.DataSources.Add(reportDataConvenios);

            if (borrador)
            {
                if (operador.idOperador == Constantes.const_OperadorUniversidadNacional)
                {
                    report.ReportPath = @"PlanDeNegocioV2\Administracion\Interventoria\ActasDeSeguimiento\Report\ActaInicioUNALBorrador.rdlc";
                }
                else
                {
                    report.ReportPath = @"PlanDeNegocioV2\Administracion\Interventoria\ActasDeSeguimiento\Report\ActaInicioBorrador.rdlc";
                }

            }
            else
            {
                //validar operador del proyecto
                if (operador.idOperador == Constantes.const_OperadorUniversidadNacional)
                {
                    report.ReportPath = @"PlanDeNegocioV2\Administracion\Interventoria\ActasDeSeguimiento\Report\ActaInicioUNAL.rdlc";
                }
                else
                {
                    report.ReportPath = @"PlanDeNegocioV2\Administracion\Interventoria\ActasDeSeguimiento\Report\ActaInicio.rdlc";
                }

            }

            report.EnableExternalImages = true;

            string path = string.Concat(baseDirectory + operador.Rutalogo.Trim());//"M:\\ baseDirectory" <-Produccion

            ReportParameter[] param = new ReportParameter[1];
            param[0] = new ReportParameter("rpLogoOperador", path);
            report.SetParameters(param);

            byte[] fileBytes = report.Render("PDF");
            MemoryStream ms = new MemoryStream(fileBytes, 0, 0, true, true);
            Response.AddHeader("content-disposition", "attachment;filename= ActaInicio_" + _codProyecto + ".pdf");
            Response.Buffer = true;
            Response.Clear();
            Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.End();

        }

        private string valorEnPalabras(double valorInicialEnPesos)
        {
            return ConvertirMonedasEnLetras.enletras(valorInicialEnPesos.ToString());

        }

        private void ImprimirPagina(string ruta)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "window.open('" + ruta + "');", true);
        }

        protected string UploadFileToServer(string _directorioBase, string _directorioDestino, string fileName)
        {
            // ¿ Carpeta de destino existe ?
            if (!Directory.Exists(_directorioBase + _directorioDestino))
                Directory.CreateDirectory(_directorioBase + _directorioDestino);
            if (File.Exists(_directorioBase + _directorioDestino + fileName))
                File.Delete(_directorioBase + _directorioDestino + fileName);

            return _directorioBase + _directorioDestino + fileName;
        }

        private void CrearActaSeguimiento()
        {
            int hashDirectorioUsuario = CodigoProyecto / 2000;
            string dirBase = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual");
            string dirDest = "ActasSeguimiento\\" + hashDirectorioUsuario + "\\Actas_" + CodigoProyecto + "\\"; //Directorio destino archivo           
            string nameFile = "TestPage.pdf";
            string ruta = UploadFileToServer(dirBase, dirDest, nameFile);

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=TestPage.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StreamWriter sw = new StreamWriter(ruta);
            //Dim sw As New StringWriter()
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            //Dim hw As New HtmlTextWriter(sw)
            //Templates.ActaSeguimientoPDF actaSeguimientoPDF = new Templates.ActaSeguimientoPDF();
            //actaSeguimientoPDF.Page.RenderControl(hw);
            //Me.Page.RenderControl(hw)
            StringReader sr = new StringReader(sw.ToString());
            //Dim sr As New StringReader(sw.ToString())
            Document pdfDoc = new Document(PageSize.A4, 10.0F, 10.0F, 100.0F, 0.0F);
            //Dim pdfDoc As New Document(PageSize.A4, 10.0F, 10.0F, 100.0F, 0.0F)
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            //Dim htmlparser As New HTMLWorker(pdfDoc)
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();
        }

        public void GenerarActa(int idActa)
        {
            var actaInicio = Negocio.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimientos.ActaSeguimiento.GetActaById(idActa, CodigoProyecto);

            var contrato = Negocio.PlanDeNegocioV2.Administracion.Interventoria.Abogado.GetContratoByProyecto(CodigoProyecto, usuario.CodOperador);

            var infoContrato = contrato.First().Contrato;

            var emprendedores = Negocio.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimientos.ActaSeguimiento.GetEmprendedoresYEquipoTrabajo(CodigoProyecto);

            var tdContainer = "<td class=\"bold\"> <br/> <br/> ____________________________ <br/> Firma <br/> Nombre: [USUARIO] <br/> [ROL] </td>";
            List<String> firmasUsuarios = new List<string>();
            string contratistas = string.Empty;
            foreach (var emprendedor in emprendedores)
            {
                if (string.IsNullOrEmpty(contratistas))
                {
                    contratistas += emprendedor.Nombres + "-" + emprendedor.Identificacion;
                }
                else
                {
                    contratistas += "," + emprendedor.Nombres + "-" + emprendedor.Identificacion;
                }

                firmasUsuarios.Add(tdContainer
                                   .Replace("[USUARIO]", emprendedor.Nombres)
                                   .Replace("[ROL]", "Contratista")
                    );
            }

            firmasUsuarios.Add(tdContainer
                                   .Replace("[USUARIO]", actaInicio.Contacto.Nombres + " " + actaInicio.Contacto.Apellidos)
                                   .Replace("[ROL]", "Interventor / Supervisor")
                              );

            var firmas = "";
            var openContainer = "<tr>";
            var closeContainer = "</tr>";
            var counter = 0;
            foreach (var firma in firmasUsuarios)
            {
                if (counter % 2 == 0)
                    firmas += openContainer;

                firmas += firma;

                if (counter % 2 != 0 || firma.Equals(firmasUsuarios.Last()))
                    firmas += closeContainer;

                counter++;
            }

            var fechaPublicacion = actaInicio.FechaPublicacion.GetValueOrDefault();
            var fechaTerminacion = actaInicio.FechaPublicacion.GetValueOrDefault().AddMonths(12);
            var codigo = actaInicio.TipoActaSeguimiento.Codigo;
            var version = actaInicio.TipoActaSeguimiento.Version;
            var vigencia = actaInicio.TipoActaSeguimiento.Vigencia;

            var htmlTerminosYCondiciones =
                File.ReadAllText(
                HttpContext.Current.Server.MapPath(@"~/PlanDeNegocioV2/Administracion/Interventoria/ActasDeSeguimiento/Templates/ActaInicio.html"))
                .Replace("[CODIGO]", codigo)
                .Replace("[VERSION]", version)
                .Replace("[VIGENCIA]", vigencia)
                .Replace("[CONTRATO]", infoContrato.NumeroContrato)
                .Replace("[TIPOCONTRATO]", infoContrato.Estado)
                .Replace("[OBJETO]", infoContrato.ObjetoContrato)
                .Replace("[VALOR]", FieldValidate.moneyFormat((double)infoContrato.ValorInicialEnPesos))
                .Replace("[CONTRATISTA]", contratistas)
                .Replace("[PLAZO]", "12")
                .Replace("[DIAINICIOACTA]", fechaPublicacion.Day.ToString())
                .Replace("[MESINICIOACTA]", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(fechaPublicacion.Month))
                .Replace("[ANNOINICIOACTA]", fechaPublicacion.Year.ToString())
                .Replace("[DIAFINALACTA]", fechaTerminacion.Day.ToString())
                .Replace("[MESFINALACTA]", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(fechaTerminacion.Month))
                .Replace("[ANNOFINALACTA]", fechaTerminacion.Year.ToString())
                .Replace("[FIRMAS]", firmas);

            HtmlToPdf(htmlTerminosYCondiciones);
        }

        void HtmlToPdf(string actaHtml)
        {

            string htmlString = actaHtml;

            string pdf_page_size = "A4";
            PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize),
                pdf_page_size, true);

            string pdf_orientation = "Portrait";
            PdfPageOrientation pdfOrientation =
                (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
                pdf_orientation, true);

            int webPageWidth = 1024;
            try
            {
                webPageWidth = 1024;
            }
            catch { }

            int webPageHeight = 0;
            try
            {
                webPageHeight = 1024;
            }
            catch { }

            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();

            // set converter options
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.WebPageWidth = webPageWidth;
            converter.Options.WebPageHeight = webPageHeight;

            // create a new pdf document converting an url
            SelectPdf.PdfDocument doc = converter.ConvertHtmlString(htmlString, "");

            // save pdf document
            doc.Save(Response, false, FileName);

            // close pdf document
            doc.Close();
        }

        public string CreateDirectory(int codigoProyecto)
        {
            int hashDirectorioUsuario = Convert.ToInt32(codigoProyecto) / 2000;
            var partialDirectory = "\\ActasSeguimiento\\" + hashDirectorioUsuario + "\\Actas_" + codigoProyecto + "\\"; ;
            var finalDirectory = baseDirectory + partialDirectory;
            var virtualDirectory = partialDirectory + FileName;

            if (!Directory.Exists(finalDirectory))
                Directory.CreateDirectory(finalDirectory);

            if (File.Exists(finalDirectory + FileName))
                File.Delete(finalDirectory + FileName);

            return virtualDirectory;
        }
        MetasProduccionControllerDTO metasProduccionController = new MetasProduccionControllerDTO();
        public IndicadoresActasSeguimiento GetMeta(int codigoProyecto, int codigoConvocatoria)
        {

            IndicadorGestionEvaluacion entidadIndicador = Negocio.PlanDeNegocioV2.Utilidad.IndicadorEvaluacion.GetIndicadores(codigoProyecto, codigoConvocatoria);
            if (entidadIndicador == null)
                throw new ApplicationException("El Proyecto ID: " + codigoProyecto + " no tiene información de indicadores, por favor actualicela.");

            var ventas = entidadIndicador.Ventas;
            var contrapartidas = Negocio.PlanDeNegocioV2.Utilidad.IndicadorFormulacion.GetContrapartidas(codigoProyecto);
            //double IDHInicio = 0;
            double NBIInicio = 0;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                //IDHInicio = (from e in db.ActaSeguimEstadoEmpresa
                // where e.codProyecto == codigoProyecto && e.numActa == 1 //Siempre ACta seguimiento 1
                // select e.IDH ?? 0).FirstOrDefault();

                NBIInicio = (from e in db.ActaSeguimEstadoEmpresa
                             where e.codProyecto == codigoProyecto && e.numActa == 1 //Siempre ACta seguimiento 1
                             select e.NBI ?? 0).FirstOrDefault();
            }

            //double idh = 0;
            double nbi = 0;

            //if (IDHInicio == 0)
            //    idh = Negocio.PlanDeNegocioV2.Utilidad.IndicadorFormulacion.GetIDH(codigoProyecto);
            //else
            //    idh = IDHInicio;

            if (NBIInicio == 0)
                nbi = Negocio.PlanDeNegocioV2.Utilidad.IndicadorFormulacion.GetNBI(codigoProyecto);
            else
                nbi = NBIInicio;

            var salarioMinimo = Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetSalarioMinimoConvocatoria(codigoConvocatoria);
            var valorRecomendadoEvaluacion = Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetRecursosAprobados(codigoProyecto, codigoConvocatoria);
            var presupuestoEvaluacion = (valorRecomendadoEvaluacion * (double)salarioMinimo);

            var produccion = Negocio.PlanDeNegocioV2.Utilidad.IndicadorEvaluacion.CountProductoRepresentativo(codigoProyecto, codigoConvocatoria);


            var mercadeo = Negocio.PlanDeNegocioV2.Utilidad.IndicadorEvaluacion.CountMercadeo(codigoProyecto, codigoConvocatoria);

            var cargos = Negocio.PlanDeNegocioV2.Utilidad.IndicadorEvaluacion.CountCargos(codigoProyecto, codigoConvocatoria);

            //Obtener la unidad de medida
            var metaProduccion = metasProduccionController.ListMetasProduccion(codigoProyecto, codigoConvocatoria);

            string unidad = metaProduccion.Where(x=>x.productoRepresentativo == true).Select(x => x.UnidadMedida).FirstOrDefault();
            string nomproducto = metaProduccion.Where(x => x.productoRepresentativo == true).Select(x => x.NomProducto).FirstOrDefault();

            var meta = new IndicadoresActasSeguimiento
            {
                Visita = "Meta",
                Cargos = cargos,
                Presupuesto = presupuestoEvaluacion,
                Mercadeo = mercadeo,
                //Idh = idh,
                Nbi = nbi,
                Contrapartidas = contrapartidas,
                Produccion = produccion + " " + unidad + " - " + nomproducto,
                Ventas = ventas
            };

            return meta;

        }

        public List<IndicadoresActasSeguimiento> GetIndicadores(int codigoProyecto)
        {
            IndicadoresActasSeguimiento meta = new IndicadoresActasSeguimiento();
            try
            {
                var indicadores = new List<IndicadoresActasSeguimiento>();

                var codigoConvocatoria = Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(codigoProyecto, 0).GetValueOrDefault(0);

                meta = GetMeta(codigoProyecto, codigoConvocatoria);

                indicadores.Add(meta);

                //Agregar Indicadores Actas Seguimiento
                var indiActa = GetListIndicadoresActas(codigoProyecto, codigoConvocatoria);

                foreach (var item in indiActa)
                {
                    indicadores.Add(item);
                }

                //Agregar Porcentaje Ejecucion
                if (indiActa.Count > 0)
                {
                    var porcEjecucion = GetPorcentEjecucion(meta, indiActa, codigoProyecto, codigoConvocatoria);

                    if (porcEjecucion.Cargos < 66)
                        lblEjecEmpleo.ForeColor = Color.Red;
                    if (porcEjecucion.Presupuesto < 70)
                        lblEjecPresupuesto.ForeColor = Color.Red;
                    if (porcEjecucion.Mercadeo < 100)
                        lblEjecMercadeo.ForeColor = Color.Red;
                    //if (porcEjecucion.Idh < 100)
                    //    lblEjecIDH.ForeColor = Color.Red;
                    if (porcEjecucion.Nbi < 1)
                        lblEjecIDH.ForeColor = Color.Red;
                    if (porcEjecucion.Contrapartidas < 100)
                        lblEjecContrapartidas.ForeColor = Color.Red;
                    if (porcEjecucion.Produccion < 60)
                        lblEjecProduccion.ForeColor = Color.Red;
                    //if (porcEjecucion.Ventas < Convert.ToDecimal(0.55))
                    //    lblEjecVentas.ForeColor = Color.Red;
                    if (porcEjecucion.Ventas < Convert.ToDecimal(55))
                        lblEjecVentas.ForeColor = Color.Red;

                    lblEjecEmpleo.Text = porcEjecucion.Cargos.ToString() + " %";
                    lblEjecPresupuesto.Text = porcEjecucion.Presupuesto.ToString() + " %";
                    //lblEjecMercadeo.Text = porcEjecucion.Mercadeo.ToString() + " %";
                    lblEjecMercadeo.Text = porcEjecucion.Mercadeo.ToString() + " %";
                    //lblEjecIDH.Text = porcEjecucion.Idh.ToString() + " %";
                    lblEjecIDH.Text = porcEjecucion.Nbi.ToString();
                    lblEjecContrapartidas.Text = porcEjecucion.Contrapartidas.ToString() + " %";
                    lblEjecProduccion.Text = porcEjecucion.Produccion.ToString() + " %";
                    lblEjecVentas.Text = porcEjecucion.Ventas.ToString() + " %";
                }

                return indicadores;
            }
            catch (Exception ex)
            {
                //Response.Redirect("~/PlanDeNegocioV2/Administracion/Interventoria/ActasDeSeguimiento/ProyectosAsignados.aspx", true);                
                if (meta.Produccion == null)
                {
                    Alert("No se puede realizar el calculo de las metas para este proyecto: No se seleccionó el producto más representativo en evaluación.");
                }
                else if (meta.Mercadeo == 0)
                {
                    Alert("No se ha completado el indicador de mercadeo para este proyecto!.");
                }
                
                else
                {
                    Alert(ex.Message);
                }

                return new List<IndicadoresActasSeguimiento>();
            }
        }

        public IndicadoresEjecucionActasSeguimiento GetPorcentEjecucion(IndicadoresActasSeguimiento _meta,
                                                            List<IndicadoresActasSeguimiento> _indicadores,
                                                            int _codProyecto, int _codConvocatoria)
        {
            IndicadoresEjecucionActasSeguimiento indicador = new IndicadoresEjecucionActasSeguimiento();
            int cantDecimal = 2;

            indicador.Visita = "% Ejecución";
            //Empleo
            if (_meta.Cargos > 0)
            {
                indicador.Cargos = (Convert.ToDecimal(_indicadores.Last().Cargos)
                                / Convert.ToDecimal(_meta.Cargos)) * 100;

                indicador.Cargos = Decimal.Round(indicador.Cargos, cantDecimal);
            }
            if (_meta.Presupuesto > 0)
            {
                indicador.Presupuesto =
                    (Convert.ToDecimal(_indicadores.Sum(x => x.Presupuesto))
                                / Convert.ToDecimal(_meta.Presupuesto)) * 100;

                indicador.Presupuesto = Decimal.Round(indicador.Presupuesto, cantDecimal);
            }
            if (_meta.Mercadeo > 0)
            {
                indicador.Mercadeo =
                    (Convert.ToDecimal(_indicadores.Last().Mercadeo)
                                / Convert.ToDecimal(_meta.Mercadeo)) * 100;//

                indicador.Mercadeo = Decimal.Round(indicador.Mercadeo, cantDecimal);
            }
            //if (_meta.Idh > 0)
            //{
            //    decimal valorIDHLast = Convert.ToDecimal(_indicadores.Last().Idh);

            //    indicador.Idh =
            //        Convert.ToDecimal(_meta.Idh)
            //                    / (valorIDHLast ) * 100;

            //    indicador.Idh = Decimal.Round(indicador.Idh, cantDecimal);
            //}


            //Ecuación de Cálculo: 
            //Siendo GIMINBI= Grado de Inversión en Municipios según Índice necesidades básicas insatisfechas; 
            //INBIE= Índice de Necesidades básicas insatisfechas Municipio donde se ejecutó el proyecto; 
            //INBIP= Índice de necesidades básicas insatisfechas municipio donde se programó el Proyecto.
            //GIMNBI = INBIE / INBIP

            if (_meta.Nbi > 0)
            {
                var INBIE = Convert.ToDecimal(_indicadores.Last().Nbi);
                var INBIP = Convert.ToDecimal(_meta.Nbi);

                var GIMNBI = INBIP != 0 ? INBIE / INBIP : 0;

                indicador.Nbi = GIMNBI;

                indicador.Nbi = Decimal.Round(indicador.Nbi, cantDecimal);
            }

            if (_meta.Contrapartidas > 0)
            {
                indicador.Contrapartidas =
                    (Convert.ToDecimal(_indicadores.Last().Contrapartidas)
                                / Convert.ToDecimal(_meta.Contrapartidas)) * 100;

                indicador.Contrapartidas = Decimal.Round(indicador.Contrapartidas, cantDecimal);
            }
            string[] metaProduccion = _meta.Produccion.Split(' ');
            decimal metaProduc = Convert.ToDecimal(metaProduccion[0]);

            string[] indicaProduccion = _indicadores.Last().Produccion.Split(' ');
            decimal indiProduccion = Convert.ToDecimal(indicaProduccion[0]);

            if (metaProduc > 0)
            {
                indicador.Produccion =
                    ((indiProduccion / metaProduc) * 100);

                indicador.Produccion = Decimal.Round(indicador.Produccion, cantDecimal);
            }

            if (_meta.Ventas > 0)
            {
                indicador.Ventas =
                    (Convert.ToDecimal((_indicadores.Last().Ventas)
                    / Convert.ToDecimal(_meta.Ventas))) * 100;// 

                indicador.Ventas = Decimal.Round(indicador.Ventas, cantDecimal);
            }

            return indicador;
        }
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public List<IndicadoresActasSeguimiento> GetListIndicadoresActas(int _codProyecto, int _codConvocatoria)
        {
            List<IndicadoresActasSeguimiento> listIndicador = new List<IndicadoresActasSeguimiento>();

            List<int> numActas = new List<int>();

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                numActas = (from i in db.ActaSeguimientoInterventoria
                            where i.IdProyecto == _codProyecto && i.NumeroActa > 0 //&& i.Publicado == true
                            orderby i.NumeroActa
                            select i.NumeroActa).ToList();
            }

            foreach (var nActa in numActas)
            {
                int visita = nActa;
                int Empleo = 0;
                decimal Ejecucion = 0;
                int Mercadeo = 0;
                int Contrapartida = 0;
                int Produccion = 0;
                string unidadProduccion = "";
                decimal ventas = 0;
                //double IDH = 0;
                double NBI = 0;
                IndicadoresActasSeguimiento indicador = new IndicadoresActasSeguimiento();

                using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
                {
                    Empleo = (from e in db.ActaSeguimGestionEmpleo
                              where e.codProyecto == _codProyecto && e.numActa == nActa
                              select e.VerificaIndicador).FirstOrDefault();
                    try
                    {
                        Ejecucion = (from e in db.ActaSeguimInfoPagos
                                     where e.codProyecto == _codProyecto && e.numActa == nActa
                                     select e.Valor).Sum();
                    }
                    catch (Exception)
                    {
                        Ejecucion = 0;
                    }


                    Mercadeo = (from e in db.ActaSeguimGestionMercadeo
                                where e.codProyecto == _codProyecto && e.numActa == nActa
                                select e.Cantidad).FirstOrDefault();



                    Contrapartida = (from e in db.ActaSeguimContrapartida
                                     where e.codProyecto == _codProyecto && e.numActa == nActa
                                     select e.cantContrapartida).FirstOrDefault();

                    Produccion = (from e in db.ActaSeguimGestionProduccion
                                  where e.codProyecto == _codProyecto && e.numActa == nActa
                                  && e.productoRepresentativo == true
                                  select e.Cantidad).FirstOrDefault();

                    unidadProduccion = (from e in db.ActaSeguimGestionProduccion
                                        where e.codProyecto == _codProyecto && e.numActa == nActa
                                        && e.productoRepresentativo == true
                                        select e.Medida).FirstOrDefault();

                    ventas = (from e in db.ActaSeguimGestionVentas
                              where e.codProyecto == _codProyecto && e.numActa == nActa
                              select e.valor).FirstOrDefault();

                    //IDH = (from e in db.ActaSeguimEstadoEmpresa
                    //       where e.codProyecto == _codProyecto && e.numActa == nActa
                    //       select e.IDH ?? 0).FirstOrDefault();

                    NBI = (from e in db.ActaSeguimEstadoEmpresa
                           where e.codProyecto == _codProyecto && e.numActa == nActa
                           select e.NBI ?? 0).FirstOrDefault();
                }

                indicador.Visita = visita.ToString();
                indicador.Cargos = Empleo;
                indicador.Presupuesto = Convert.ToDouble(Ejecucion);
                indicador.Mercadeo = Mercadeo;
                indicador.Contrapartidas = Contrapartida;
                indicador.Produccion = Produccion + " " + unidadProduccion;
                indicador.Ventas = ventas;
                //indicador.Idh = IDH;
                indicador.Nbi = NBI;

                listIndicador.Add(indicador);
            }



            return listIndicador;
        }

        private void Alert(string mensaje)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
        }

        protected void lnkAddActaSeguimiento_Click(object sender, EventArgs e)
        {
            //try
            //{
            var Actas = GetActas(CodigoProyecto);

            if (Actas.Count() > 0)
            {
                var ultimaActa = Actas
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();

                if (ultimaActa.Publicado)
                {
                    //Generar acta de seguimiento
                    Response.Redirect("~/PlanDeNegocioV2/Administracion/Interventoria/ActasDeSeguimiento/CrearActaSeguimiento.aspx?codigo=" + CodigoProyecto);
                }
                else
                {
                    Alert("El acta número " + ultimaActa.Id + ", No ha sido publicada.");
                }
            }
            else
            {
                Alert("No se ha generado el acta de inicio para este proyecto.");
            }

            //}
            //catch (Exception ex)
            //{
            //    Alert("Evento inesperado:" + ex.Message);
            //}


        }
    }

    public class IndicadoresActasSeguimiento
    {
        public string Visita { get; set; }
        public int Cargos { get; set; }
        public double Presupuesto { get; set; }
        public String PresupuestoWithFormat
        {
            get
            {
                if (Visita != "% Ejecución")
                    return FieldValidate.moneyFormat(Presupuesto, true);
                else
                    return Presupuesto.ToString();
            }
            set
            {
            }
        }
        public int Mercadeo { get; set; }
        public double Idh { get; set; }
        public double Nbi { get; set; }
        public int Contrapartidas { get; set; }
        public string Produccion { get; set; }
        public Decimal Ventas { get; set; }

        public String VentasWithFormat
        {
            get
            {
                if (Visita != "% Ejecución")
                    return FieldValidate.moneyFormat(Ventas, true);
                else
                    return Ventas.ToString();
            }
            set
            {
            }
        }
    }


    public class IndicadoresEjecucionActasSeguimiento
    {
        public string Visita { get; set; }
        public decimal Cargos { get; set; }
        public decimal Presupuesto { get; set; }
        public decimal Mercadeo { get; set; }
        public decimal Idh { get; set; }
        public decimal Nbi { get; set; }
        public decimal Contrapartidas { get; set; }
        public decimal Produccion { get; set; }
        public decimal Ventas { get; set; }

    }
}