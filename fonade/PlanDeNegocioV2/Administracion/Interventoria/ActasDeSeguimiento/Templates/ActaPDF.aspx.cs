using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Account;
using System.Web.Security;
using Fonade.Negocio.FonDBLight;
using Datos;
using System.Configuration;
using Datos.Modelos;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.Templates
{
    public partial class ActaPDF : System.Web.UI.Page
    {

        [ContextStatic]
        protected FonadeUser usuario;

        protected override void OnLoad(EventArgs e)
        {
            if (Session["usuarioLogged"] == null) { Response.Redirect("~/Account/Login.aspx"); return; }
            usuario = HttpContext.Current.Session["usuarioLogged"] != null ?
                (FonadeUser)HttpContext.Current.Session["usuarioLogged"] :
                (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true);
            base.OnLoad(e);
        }

        public int CodigoProyecto
        {
            get
            {
                int id = Convert.ToInt32(Session["idProyecto"]);
                return id;
            }
            set { }
        }

        public int CodigoActa
        {
            get
            {
                int id = Convert.ToInt32(Session["idActa"]);
                return id;
            }
        }

        public int CodigoConvocatoria
        {
            get
            {
                int id = Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(CodigoProyecto, 0).GetValueOrDefault(0);
                return id;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Cargar Logo

                //Pagina 1
                cargarInfoActaPag1(CodigoProyecto, CodigoActa);
                cargarGridVerificaIndicadoresyMetas(CodigoProyecto);

                //Pagina 2
                cargarInfoActaPag2(CodigoProyecto, CodigoActa);

                //Gestion Empleos Pag2
                cargarGestionEmpleos(CodigoProyecto, CodigoActa);
                cargarEmpleosActas(CodigoProyecto, CodigoConvocatoria);

                //PAGINA 3
                cargarDatosEjecPresupuestal(CodigoProyecto, CodigoConvocatoria);
                cargarGesEjecPresupuestal(CodigoProyecto, CodigoConvocatoria);
                mostrarPanels(CodigoActa - 1);//Visita

                //Pagina 4
                cargarTbInventario(CodigoProyecto, CodigoConvocatoria);

                //Pagina 5
                cargarAporteEmpren(CodigoProyecto, CodigoConvocatoria);
                cargarMetaGestMercadeo(CodigoProyecto, CodigoConvocatoria);
                cargarGestMercadero(CodigoProyecto, CodigoConvocatoria);
                cargarContraprtidas(CodigoProyecto, CodigoConvocatoria);

                //PAgina  6
                cargarMetaProducion(CodigoProyecto, CodigoConvocatoria);
                cargarGestProduccion(CodigoProyecto, CodigoConvocatoria);
                cargarGestVentas(CodigoProyecto, CodigoConvocatoria);

                //Pagina 7
                cargarCumplObligContables(CodigoProyecto, CodigoConvocatoria);
                cargarCumpObliTributaria(CodigoProyecto, CodigoConvocatoria);
                cargarCumpObligLaboral(CodigoProyecto, CodigoConvocatoria);

                //Pagina 8
                cargarVerficaRegTramites(CodigoProyecto, CodigoConvocatoria);
                cargarCompInnovadorOtrosAspectos(CodigoProyecto, CodigoConvocatoria);
                cargarOtrosAspectosCompInnova(CodigoProyecto, CodigoConvocatoria);
                cargarOtrosAspectosAmbiental(CodigoProyecto, CodigoConvocatoria);

                //PAgina 9
                cargarReportInforPlataforma(CodigoProyecto, CodigoConvocatoria);
                cargarTiempoDedicaEmp(CodigoProyecto, CodigoConvocatoria);
                cargarAcompUnidadEmp(CodigoProyecto, CodigoConvocatoria);
                cargarEstadoEmpresa(CodigoProyecto, CodigoConvocatoria);

                //Pagina 10
                cargaCompromisos(CodigoProyecto, CodigoConvocatoria);

                //Pagina 11
                cargarInfoFirmas(CodigoProyecto, CodigoActa);
            }
        }

        //PAgina 1
        #region PAgina 1
        ActaSeguimientoDatosController datosController = new ActaSeguimientoDatosController();

        private void cargarGridVerificaIndicadoresyMetas(int _codigoProyecto)
        {
            IndicadoresEjecucionActasSeguimiento ejecActas = new IndicadoresEjecucionActasSeguimiento();

            var datos = GetIndicadores(_codigoProyecto, ref ejecActas);


            // Total number of rows.
            int rowCnt;
            // Current row count.
            int rowCtr;
            // Total number of cells per row (columns).
            int cellCtr;
            // Current cell counter
            int cellCnt;

            rowCnt = datos.Count();
            cellCnt = 8;

            for (rowCtr = 1; rowCtr <= rowCnt; rowCtr++)
            {
                // Create new row and add it to the table.
                TableRow tRow = new TableRow();
                tblMetas.Rows.Add(tRow);
                int columna = 0;
                for (cellCtr = 1; cellCtr <= cellCnt; cellCtr++)
                {
                    // Create a new cell and add it to the row.
                    TableCell tCell = new TableCell();
                    // BorderStyle="Solid" BorderWidth="1px"
                    tCell.BorderStyle = BorderStyle.Solid;
                    tCell.BorderWidth = 1;
                    int indice = rowCtr - 1;

                    switch (columna)
                    {
                        case 0:
                            tCell.Text = datos[indice].Visita;
                            break;
                        case 1:
                            tCell.Text = datos[indice].Cargos.ToString();
                            break;
                        case 2:
                            tCell.Text = datos[indice].PresupuestoWithFormat;
                            break;
                        case 3:
                            tCell.Text = datos[indice].Mercadeo.ToString();
                            break;
                        case 4:
                            tCell.Text = datos[indice].Idh.ToString();
                            break;
                        case 5:
                            tCell.Text = datos[indice].Contrapartidas.ToString();
                            break;
                        case 6:
                            tCell.Text = datos[indice].Produccion;
                            break;
                        case 7:
                            tCell.Text = datos[indice].VentasWithFormat;
                            break;
                        default:
                            tCell.Text = "";
                            break;
                    }
                    columna++;
                    tRow.Cells.Add(tCell);
                }
            }

            TableRow tRowEjec = new TableRow();
            tRowEjec.BackColor = System.Drawing.Color.Silver;
            tRowEjec.BorderStyle = BorderStyle.Solid;
            tRowEjec.BorderWidth = 1;
            tblMetas.Rows.Add(tRowEjec);
            int columnaRowFinal = 0;
            for (cellCtr = 1; cellCtr <= cellCnt; cellCtr++)
            {
                // Create a new cell and add it to the row.
                TableCell tCell = new TableCell();
                // BorderStyle="Solid" BorderWidth="1px"
                tCell.BorderStyle = BorderStyle.Solid;
                tCell.BorderWidth = 1;

                switch (columnaRowFinal)
                {
                    case 0:
                        tCell.Text = ejecActas.Visita;
                        break;
                    case 1:
                        tCell.Text = ejecActas.Cargos + " %";
                        break;
                    case 2:
                        tCell.Text = ejecActas.Presupuesto + " %";
                        break;
                    case 3:
                        tCell.Text = ejecActas.Mercadeo + " %";
                        break;
                    case 4:
                        tCell.Text = ejecActas.Idh + " %";
                        break;
                    case 5:
                        tCell.Text = ejecActas.Contrapartidas + " %";
                        break;
                    case 6:
                        tCell.Text = ejecActas.Produccion + " %";
                        break;
                    case 7:
                        tCell.Text = ejecActas.Ventas + " %";
                        break;
                    default:
                        tCell.Text = "";
                        break;
                }
                columnaRowFinal++;
                tRowEjec.Cells.Add(tCell);
            }
        }

        public List<IndicadoresActasSeguimiento> GetIndicadores(int codigoProyecto, ref IndicadoresEjecucionActasSeguimiento indicadoresActas)
        {
            try
            {
                var indicadores = new List<IndicadoresActasSeguimiento>();

                var codigoConvocatoria = Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(codigoProyecto, 0).GetValueOrDefault(0);
                var meta = GetMeta(codigoProyecto, codigoConvocatoria);

                indicadores.Add(meta);

                //Agregar Indicadores Actas Seguimiento
                var indiActa = GetListIndicadoresActas(codigoProyecto, codigoConvocatoria);

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
        MetasProduccionControllerDTO metasProduccionController = new MetasProduccionControllerDTO();
        public IndicadoresActasSeguimiento GetMeta(int codigoProyecto, int codigoConvocatoria)
        {

            IndicadorGestionEvaluacion entidadIndicador = Negocio.PlanDeNegocioV2.Utilidad.IndicadorEvaluacion.GetIndicadores(codigoProyecto, codigoConvocatoria);
            if (entidadIndicador == null)
                throw new ApplicationException("Este proyecto no tiene información de indicadores, por favor actualicela.");

            var ventas = entidadIndicador.Ventas;
            var contrapartidas = Negocio.PlanDeNegocioV2.Utilidad.IndicadorFormulacion.GetContrapartidas(codigoProyecto);
            var idh = Negocio.PlanDeNegocioV2.Utilidad.IndicadorFormulacion.GetIDH(codigoProyecto);
            var salarioMinimo = Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetSalarioMinimoConvocatoria(codigoConvocatoria);
            var valorRecomendadoEvaluacion = Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetRecursosAprobados(codigoProyecto, codigoConvocatoria);
            var presupuestoEvaluacion = (valorRecomendadoEvaluacion * (double)salarioMinimo);
            var produccion = Negocio.PlanDeNegocioV2.Utilidad.IndicadorEvaluacion.CountProductos(codigoProyecto, codigoConvocatoria);
            var mercadeo = Negocio.PlanDeNegocioV2.Utilidad.IndicadorEvaluacion.CountMercadeo(codigoProyecto, codigoConvocatoria);
            var cargos = Negocio.PlanDeNegocioV2.Utilidad.IndicadorEvaluacion.CountCargos(codigoProyecto, codigoConvocatoria);

            //Obtener la unidad de medida
            var metaProduccion = metasProduccionController.ListMetasProduccion(codigoProyecto, codigoConvocatoria);
            string unidad = metaProduccion.Select(x => x.UnidadMedida).FirstOrDefault();



            var meta = new IndicadoresActasSeguimiento
            {
                Visita = "Meta",
                Cargos = cargos,
                Presupuesto = presupuestoEvaluacion,
                Mercadeo = mercadeo,
                Idh = idh,
                Contrapartidas = contrapartidas,
                Produccion = produccion + " " + unidad,
                Ventas = ventas
            };

            return meta;
        }

        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public List<IndicadoresActasSeguimiento> GetListIndicadoresActas(int _codProyecto, int _codConvocatoria)
        {
            List<IndicadoresActasSeguimiento> listIndicador = new List<IndicadoresActasSeguimiento>();

            List<int> numActas = new List<int>();

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                numActas = (from i in db.ActaSeguimientoInterventoria
                            where i.IdProyecto == _codProyecto && i.NumeroActa > 1
                            orderby i.NumeroActa
                            select i.NumeroActa).ToList();
            }

            foreach (var nActa in numActas)
            {
                int visita = nActa - 1;
                int Empleo = 0;
                decimal Ejecucion = 0;
                int Mercadeo = 0;
                int Contrapartida = 0;
                int Produccion = 0;
                string unidadProduccion = "";
                decimal ventas = 0;
                double IDH = 0;
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
                                  select e.Cantidad).FirstOrDefault();

                    unidadProduccion = (from e in db.ActaSeguimGestionProduccion
                                        where e.codProyecto == _codProyecto && e.numActa == nActa
                                        select e.Medida).FirstOrDefault();

                    ventas = (from e in db.ActaSeguimGestionVentas
                              where e.codProyecto == _codProyecto && e.numActa == nActa
                              select e.valor).FirstOrDefault();

                    IDH = (from e in db.ActaSeguimEstadoEmpresa
                           where e.codProyecto == _codProyecto && e.numActa == nActa
                           select e.IDH ?? 0).FirstOrDefault();

                    
                }

                indicador.Visita = visita.ToString();
                indicador.Cargos = Empleo;
                indicador.Presupuesto = Convert.ToDouble(Ejecucion);
                indicador.Mercadeo = Mercadeo;
                indicador.Contrapartidas = Contrapartida;
                indicador.Produccion = Produccion + " " + unidadProduccion;
                indicador.Ventas = ventas;
                indicador.Idh = IDH;

                listIndicador.Add(indicador);
            }



            return listIndicador;
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
                                / Convert.ToDecimal(_meta.Mercadeo)) * 100;

                indicador.Mercadeo = Decimal.Round(indicador.Mercadeo, cantDecimal);
            }
            if (_meta.Idh > 0)
            {
                indicador.Idh =
                    (Convert.ToDecimal(_indicadores.Last().Idh)
                                / Convert.ToDecimal(_meta.Idh)) * 100;

                indicador.Idh = Decimal.Round(indicador.Idh, cantDecimal);
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
                    / Convert.ToDecimal(_meta.Ventas)) * 100);

                indicador.Ventas = Decimal.Round(indicador.Ventas, cantDecimal);
            }

            return indicador;
        }
        private void cargarInfoActaPag1(int _codProyecto, int _numActa)
        {
            var datos = datosController.obtenerDatosActa(_numActa, _codProyecto);

            lblNumActa.Text = datos.numActa.ToString();
            lblFechaActa.Text = datos.FechaActualizado.ToShortDateString();
            lblNumContrato.Text = datos.NumContrato;
            lblFechaActaInicio.Text = datos.FechaActaInicio.ToShortDateString();
            lblProrroga.Text = datos.Prorroga;
            lblNomPlanNegocio.Text = datos.NombrePlanNegocio;
            lblNomEmpresa.Text = datos.NombreEmpresa;
            lblNitEmpresa.Text = datos.NitEmpresa;
            lblContratoMarcoAdmin.Text = datos.ContratoMarcoInteradmin;
            lblContratoInterventoria.Text = datos.ContratoInterventoria;
            lblContratista.Text = datos.Contratista;
            lblValorAprobado.Text = datos.ValorAprobado;
            lblDomPrincipal.Text = datos.DomicilioPrincipal;
            lblConvocatoria.Text = datos.Convocatoria;
            lblSector.Text = datos.SectorEconomico;
            lblObjetoProyecto.Text = datos.ObjetoProyecto;
            lblObjetoVisita.Text = datos.ObjetoVisita;


        }
        #endregion

        //Pagina 2
        #region Pagina 2
        //Cargar Info Pagina 2
        EvaluacionRiesgoController evaluacionRiesgoController = new EvaluacionRiesgoController();
        ActaSeguimRiesgosController actaSeguimRiesgosController = new ActaSeguimRiesgosController();
        private void cargarInfoActaPag2(int _codProyecto, int _numActa)
        {
            var codigoConvocatoria = Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(_codProyecto, 0).GetValueOrDefault(0);

            var evalRiegos = evaluacionRiesgoController
                                .GetEvaluacionRiesgoByCodProyectoByCodConvocatoria
                                (CodigoProyecto, codigoConvocatoria);

            // Total number of rows.
            int rowCnt;
            // Current row count.
            int rowCtr;
            // Total number of cells per row (columns).
            int cellCtr;
            // Current cell counter
            int cellCnt;

            rowCnt = evalRiegos.Count();
            cellCnt = 4; //Numero de columnas

            for (rowCtr = 1; rowCtr <= rowCnt; rowCtr++)
            {
                // Create new row and add it to the table.
                TableRow tRow = new TableRow();
                tbRiegosIdentificados.Rows.Add(tRow);
                int columna = 0;
                for (cellCtr = 1; cellCtr <= cellCnt; cellCtr++)
                {
                    // Create a new cell and add it to the row.
                    TableCell tCell = new TableCell();
                    // BorderStyle="Solid" BorderWidth="1px"
                    tCell.BorderStyle = BorderStyle.Solid;
                    tCell.BorderWidth = 1;
                    int indice = rowCtr - 1;

                    switch (columna)
                    {
                        case 0:
                            tCell.Text = rowCtr.ToString();
                            break;
                        case 1:
                            tCell.Text = evalRiegos[indice].Riesgo;
                            break;
                        case 2:
                            tCell.Text = evalRiegos[indice].Mitigacion;
                            break;
                        case 3:
                            int codRiesgo = evalRiegos[indice].idRiesgo;
                            string gestionEmp = actaSeguimRiesgosController
                                                .GetGestionEmprendedorByIdRiesgoByActa(codRiesgo, CodigoActa);
                            tCell.Text = gestionEmp;
                            break;
                        default:
                            tCell.Text = "";
                            break;
                    }
                    columna++;
                    tRow.Cells.Add(tCell);
                }
            }

        }

        MetasEmpleoControllerDTO metasEmpleoController = new MetasEmpleoControllerDTO();
        private void cargarGestionEmpleos(int _codProyecto, int _numActa)
        {
            int totalMetasEmp = 0;

            var Metas = metasEmpleoController.ListMetasEmpleo(_codProyecto, CodigoConvocatoria, ref totalMetasEmp);

            lblMetasEmpleos.Text = "META TOTAL DE EMPLEOS A GENERAR: " + totalMetasEmp.ToString();

            // Total number of rows.
            int rowCnt;
            // Current row count.
            int rowCtr;
            // Total number of cells per row (columns).
            int cellCtr;
            // Current cell counter
            int cellCnt;

            rowCnt = Metas.Count();
            cellCnt = 3; //Numero de columnas

            for (rowCtr = 1; rowCtr <= rowCnt; rowCtr++)
            {
                // Create new row and add it to the table.
                TableRow tRow = new TableRow();
                tbMetaEmpleos.Rows.Add(tRow);
                int columna = 0;
                for (cellCtr = 1; cellCtr <= cellCnt; cellCtr++)
                {
                    // Create a new cell and add it to the row.
                    TableCell tCell = new TableCell();
                    // BorderStyle="Solid" BorderWidth="1px"
                    tCell.BorderStyle = BorderStyle.Solid;
                    tCell.BorderWidth = 1;
                    int indice = rowCtr - 1;

                    switch (columna)
                    {
                        case 0:
                            tCell.Text = Metas[indice].Unidades.ToString();
                            break;
                        case 1:
                            tCell.Text = Metas[indice].Cargo;
                            break;
                        case 2:
                            tCell.Text = Metas[indice].Condicion;
                            break;

                        default:
                            tCell.Text = "";
                            break;
                    }
                    columna++;
                    tRow.Cells.Add(tCell);
                }
            }

        }

        ActaSeguimGestionEmpleoController empleoController = new ActaSeguimGestionEmpleoController();
        private void cargarEmpleosActas(int _codProyecto, int _codConvocatoria)
        {
            var indicador = empleoController
                .GetGestionEmploByCodProyectoByCodConvocatoria(_codProyecto, _codConvocatoria);

            // Total number of rows.
            int rowCnt;
            // Current row count.
            int rowCtr;
            // Total number of cells per row (columns).
            int cellCtr;
            // Current cell counter
            int cellCnt;

            rowCnt = indicador.Count();
            cellCnt = 3; //Numero de columnas

            for (rowCtr = 1; rowCtr <= rowCnt; rowCtr++)
            {
                // Create new row and add it to the table.
                TableRow tRow = new TableRow();
                tbIndicadorEmpleo.Rows.Add(tRow);
                int columna = 0;
                for (cellCtr = 1; cellCtr <= cellCnt; cellCtr++)
                {
                    // Create a new cell and add it to the row.
                    TableCell tCell = new TableCell();
                    // BorderStyle="Solid" BorderWidth="1px"
                    tCell.BorderStyle = BorderStyle.Solid;
                    tCell.BorderWidth = 1;
                    int indice = rowCtr - 1;

                    switch (columna)
                    {
                        case 0:
                            tCell.Text = indicador[indice].Visita.ToString();
                            break;
                        case 1:
                            tCell.Text = indicador[indice].verificaIndicador.ToString();
                            break;
                        case 2:
                            tCell.Text = indicador[indice].desarrolloIndicador;
                            break;

                        default:
                            tCell.Text = "";
                            break;
                    }
                    columna++;
                    tRow.Cells.Add(tCell);
                }
            }

        }

        #endregion

        //PAgina 3
        #region Pagina 3

        private void cargarDatosEjecPresupuestal(int _codProyecto, int _codConvocatoria)
        {
            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                var query = (from E in db.EvaluacionObservacions
                             where E.CodConvocatoria == _codConvocatoria && E.CodProyecto == _codProyecto
                             select new { E.ValorRecomendado }).FirstOrDefault();

                lblSMLV.Text = query.ValorRecomendado.HasValue ?
                                    query.ValorRecomendado.Value.ToString() : "0";

                DateTime qAno = (from C in db.Convocatoria
                                 where C.Id_Convocatoria == _codConvocatoria
                                 orderby C.Id_Convocatoria descending
                                 select C.FechaInicio).FirstOrDefault();

                int Ano = qAno.Year;
                lblAnoSLMV.Text = Ano.ToString();
                var qSalario = (from S in db.SalariosMinimos
                                where S.AñoSalario == Ano
                                orderby S.Id_SalariosMinimos descending
                                select S.SalarioMinimo).FirstOrDefault();

                lblValorPesos.Text = (Convert.ToInt64(lblSMLV.Text) * qSalario).ToString("C");

            }
        }

        private void mostrarPanels(int _numVisita)
        {
            if (_numVisita > 1) //Si no es la primera visita
            {
                //Gestión en Ejecución Presupuestal                
                pnlGestionEjePresupuestal.Visible = true;

                //Inventarios y Contrato de Garantía
                pnlInfoInventario.Visible = false;
                pnlGestionInventario.Visible = true;
            }
            else
            {
                //Gestión en Ejecución Presupuestal                
                pnlGestionEjePresupuestal.Visible = false;

                //Inventarios y Contrato de Garantía
                pnlInfoInventario.Visible = true;
                pnlGestionInventario.Visible = false;
            }
        }
        ActaSeguimInfoPagosController infoPagosController = new ActaSeguimInfoPagosController();
        private void cargarGesEjecPresupuestal(int _codProyecto, int _codConvocatoria)
        {
            var histEjecucion = infoPagosController.getHistoricoEjecucion(_codProyecto, _codConvocatoria);

            decimal sum = histEjecucion.Sum(x => x.Valor);

            lblTotalDesembolsado.Text = sum.ToString("C");

            // Total number of rows.
            int rowCnt;
            // Current row count.
            int rowCtr;
            // Total number of cells per row (columns).
            int cellCtr;
            // Current cell counter
            int cellCnt;

            rowCnt = histEjecucion.Count();
            cellCnt = 8; //Numero de columnas

            for (rowCtr = 1; rowCtr <= rowCnt; rowCtr++)
            {
                // Create new row and add it to the table.
                TableRow tRow = new TableRow();
                tbEjecuPresupuestal.Rows.Add(tRow);
                int columna = 0;
                for (cellCtr = 1; cellCtr <= cellCnt; cellCtr++)
                {
                    // Create a new cell and add it to the row.
                    TableCell tCell = new TableCell();
                    // BorderStyle="Solid" BorderWidth="1px"
                    tCell.BorderStyle = BorderStyle.Solid;
                    tCell.BorderWidth = 1;
                    int indice = rowCtr - 1;

                    switch (columna)
                    {
                        case 0:
                            tCell.Text = histEjecucion[indice].visita.ToString();
                            break;
                        case 1:
                            tCell.Text = histEjecucion[indice].idPagoActividad.ToString();
                            break;
                        case 2:
                            tCell.Text = histEjecucion[indice].Actividad;
                            break;
                        case 3:
                            tCell.Text = histEjecucion[indice].Valor.ToString("C");
                            break;
                        case 4:
                            tCell.Text = histEjecucion[indice].Concepto;
                            break;
                        case 5:
                            tCell.Text = histEjecucion[indice].verificoDocumentos;
                            break;
                        case 6:
                            tCell.Text = histEjecucion[indice].verificoActivosEstado;
                            break;
                        case 7:
                            tCell.Text = histEjecucion[indice].Observacion;
                            break;
                        default:
                            tCell.Text = "";
                            break;
                    }
                    columna++;
                    tRow.Cells.Add(tCell);
                }
            }
        }

        #endregion

        //Pagina 4
        #region Pagina 4

        private void cargarTbInventario(int _codProyecto, int _codConvocatoria)
        {
            var inventario = infoPagosController.getInventarioContrato(_codProyecto, _codConvocatoria);

            decimal suma = inventario.Sum(x => x.valorActivos);
            lblActivosPrendables.Text = "TOTAL VALOR ACTIVOS PRENDABLES " + suma.ToString("C");

            // Total number of rows.
            int rowCnt;
            // Current row count.
            int rowCtr;
            // Total number of cells per row (columns).
            int cellCtr;
            // Current cell counter
            int cellCnt;

            rowCnt = inventario.Count();
            cellCnt = 4; //Numero de columnas

            for (rowCtr = 1; rowCtr <= rowCnt; rowCtr++)
            {
                // Create new row and add it to the table.
                TableRow tRow = new TableRow();
                tbInventario.Rows.Add(tRow);
                int columna = 0;
                for (cellCtr = 1; cellCtr <= cellCnt; cellCtr++)
                {
                    // Create a new cell and add it to the row.
                    TableCell tCell = new TableCell();
                    // BorderStyle="Solid" BorderWidth="1px"
                    tCell.BorderStyle = BorderStyle.Solid;
                    tCell.BorderWidth = 1;
                    int indice = rowCtr - 1;

                    switch (columna)
                    {
                        case 0:
                            tCell.Text = inventario[indice].visita.ToString();
                            break;
                        case 1:
                            tCell.Text = inventario[indice].descripcionRecursos.ToString();
                            break;
                        case 2:
                            tCell.Text = inventario[indice].valorActivos.ToString("C");
                            break;
                        case 3:
                            tCell.Text = inventario[indice].fechaCargaAnexo.ToShortDateString();
                            break;

                        default:
                            tCell.Text = "";
                            break;
                    }
                    columna++;
                    tRow.Cells.Add(tCell);
                }
            }

        }

        #endregion

        //Pagina 5
        #region Pagina 5

        private void cargarAporteEmpren(int _codProyecto, int _codConvocatoria)
        {
            lblMetaAporteEmp.Text = infoPagosController.AporteEmpRecomendado(_codProyecto, _codConvocatoria);

            var aportes = infoPagosController.getAporteEmp(_codProyecto, _codConvocatoria);

            // Total number of rows.
            int rowCnt;
            // Current row count.
            int rowCtr;
            // Total number of cells per row (columns).
            int cellCtr;
            // Current cell counter
            int cellCnt;

            rowCnt = aportes.Count();
            cellCnt = 2; //Numero de columnas

            for (rowCtr = 1; rowCtr <= rowCnt; rowCtr++)
            {
                // Create new row and add it to the table.
                TableRow tRow = new TableRow();
                tbAporteEmp.Rows.Add(tRow);
                int columna = 0;
                for (cellCtr = 1; cellCtr <= cellCnt; cellCtr++)
                {
                    // Create a new cell and add it to the row.
                    TableCell tCell = new TableCell();
                    // BorderStyle="Solid" BorderWidth="1px"
                    tCell.BorderStyle = BorderStyle.Solid;
                    tCell.BorderWidth = 1;
                    int indice = rowCtr - 1;

                    switch (columna)
                    {
                        case 0:
                            tCell.Text = aportes[indice].visita.ToString();
                            break;
                        case 1:
                            tCell.Text = aportes[indice].descripcion.ToString();
                            break;
                        default:
                            tCell.Text = "";
                            break;
                    }
                    columna++;
                    tRow.Cells.Add(tCell);
                }
            }
        }

        MetasActividadControllerDTO metasActividadController = new MetasActividadControllerDTO();
        private void cargarMetaGestMercadeo(int _codProyecto, int _codConvocatoria)
        {
            int sumMetas = 0;
            var consulta = metasActividadController.ListMetasMercadeo(_codProyecto, _codConvocatoria, ref sumMetas);

            lblMetaMercadeo.Text = "META TOTAL DE EVENTOS: " + sumMetas.ToString();

            // Total number of rows.
            int rowCnt;
            // Current row count.
            int rowCtr;
            // Total number of cells per row (columns).
            int cellCtr;
            // Current cell counter
            int cellCnt;

            rowCnt = consulta.Count();
            cellCnt = 2; //Numero de columnas

            for (rowCtr = 1; rowCtr <= rowCnt; rowCtr++)
            {
                // Create new row and add it to the table.
                TableRow tRow = new TableRow();
                tbMetaGestMercadeo.Rows.Add(tRow);
                int columna = 0;
                for (cellCtr = 1; cellCtr <= cellCnt; cellCtr++)
                {
                    // Create a new cell and add it to the row.
                    TableCell tCell = new TableCell();
                    // BorderStyle="Solid" BorderWidth="1px"
                    tCell.BorderStyle = BorderStyle.Solid;
                    tCell.BorderWidth = 1;
                    int indice = rowCtr - 1;

                    switch (columna)
                    {
                        case 0:
                            tCell.Text = consulta[indice].Unidades.ToString();
                            break;
                        case 1:
                            tCell.Text = consulta[indice].Actividad.ToString();
                            break;
                        default:
                            tCell.Text = "";
                            break;
                    }
                    columna++;
                    tRow.Cells.Add(tCell);
                }
            }
        }

        ActaSeguimGestionMercadeoController actaSeguimGestionMercadeoController = new ActaSeguimGestionMercadeoController();
        private void cargarGestMercadero(int _codProyecto, int _codConvocatoria)
        {
            var gestion = actaSeguimGestionMercadeoController.GetGestionMercadeo(_codProyecto, _codConvocatoria);

            // Total number of rows.
            int rowCnt;
            // Current row count.
            int rowCtr;
            // Total number of cells per row (columns).
            int cellCtr;
            // Current cell counter
            int cellCnt;

            rowCnt = gestion.Count();
            cellCnt = 4; //Numero de columnas

            for (rowCtr = 1; rowCtr <= rowCnt; rowCtr++)
            {
                // Create new row and add it to the table.
                TableRow tRow = new TableRow();
                tbGestMercadeo.Rows.Add(tRow);
                int columna = 0;
                for (cellCtr = 1; cellCtr <= cellCnt; cellCtr++)
                {
                    // Create a new cell and add it to the row.
                    TableCell tCell = new TableCell();
                    // BorderStyle="Solid" BorderWidth="1px"
                    tCell.BorderStyle = BorderStyle.Solid;
                    tCell.BorderWidth = 1;
                    int indice = rowCtr - 1;

                    switch (columna)
                    {
                        case 0:
                            tCell.Text = gestion[indice].visita.ToString();
                            break;
                        case 1:
                            tCell.Text = gestion[indice].cantidad.ToString();
                            break;
                        case 2:
                            tCell.Text = gestion[indice].descripcionEvento;
                            break;
                        case 3:
                            tCell.Text = gestion[indice].publicidadLogos;
                            break;
                        default:
                            tCell.Text = "";
                            break;
                    }
                    columna++;
                    tRow.Cells.Add(tCell);
                }
            }

        }

        ActaSeguimContrapartidaController contrapartidaController = new ActaSeguimContrapartidaController();
        private void cargarContraprtidas(int _codProyecto, int _codConvocatoria)
        {
            var gestion = contrapartidaController.GetContrapartidas(_codProyecto, _codConvocatoria);

            // Total number of rows.
            int rowCnt;
            // Current row count.
            int rowCtr;
            // Total number of cells per row (columns).
            int cellCtr;
            // Current cell counter
            int cellCnt;

            rowCnt = gestion.Count();
            cellCnt = 3; //Numero de columnas

            for (rowCtr = 1; rowCtr <= rowCnt; rowCtr++)
            {
                // Create new row and add it to the table.
                TableRow tRow = new TableRow();
                tbContrapartidas.Rows.Add(tRow);
                int columna = 0;
                for (cellCtr = 1; cellCtr <= cellCnt; cellCtr++)
                {
                    // Create a new cell and add it to the row.
                    TableCell tCell = new TableCell();
                    // BorderStyle="Solid" BorderWidth="1px"
                    tCell.BorderStyle = BorderStyle.Solid;
                    tCell.BorderWidth = 1;
                    int indice = rowCtr - 1;

                    switch (columna)
                    {
                        case 0:
                            tCell.Text = gestion[indice].visita.ToString();
                            break;
                        case 1:
                            tCell.Text = gestion[indice].cantContrapartida.ToString();
                            break;
                        case 2:
                            tCell.Text = gestion[indice].descripcion;
                            break;

                        default:
                            tCell.Text = "";
                            break;
                    }
                    columna++;
                    tRow.Cells.Add(tCell);
                }
            }
        }

        #endregion

        //Pagina 6
        #region Pagina 6

        private void cargarMetaProducion(int _codProyecto, int _codConvocatoria)
        {
            var metaProduccion = metasProduccionController.ListMetasProduccion(_codProyecto, _codConvocatoria);

            // Total number of rows.
            int rowCnt;
            // Current row count.
            int rowCtr;
            // Total number of cells per row (columns).
            int cellCtr;
            // Current cell counter
            int cellCnt;

            rowCnt = metaProduccion.Count();
            cellCnt = 3; //Numero de columnas

            for (rowCtr = 1; rowCtr <= rowCnt; rowCtr++)
            {
                // Create new row and add it to the table.
                TableRow tRow = new TableRow();
                tbMetaProduccion.Rows.Add(tRow);
                int columna = 0;
                for (cellCtr = 1; cellCtr <= cellCnt; cellCtr++)
                {
                    // Create a new cell and add it to the row.
                    TableCell tCell = new TableCell();
                    // BorderStyle="Solid" BorderWidth="1px"
                    tCell.BorderStyle = BorderStyle.Solid;
                    tCell.BorderWidth = 1;
                    int indice = rowCtr - 1;

                    switch (columna)
                    {
                        case 0:
                            tCell.Text = "META";
                            break;
                        case 1:
                            tCell.Text = metaProduccion[indice].Cantidad.ToString();
                            break;
                        case 2:
                            tCell.Text = metaProduccion[indice].NomProducto;
                            break;

                        default:
                            tCell.Text = "";
                            break;
                    }
                    columna++;
                    tRow.Cells.Add(tCell);
                }
            }
        }

        ActaSeguimGestionProduccionController produccionController = new ActaSeguimGestionProduccionController();
        private void cargarGestProduccion(int _codProyecto, int _codConvocatoria)
        {
            var gestion = produccionController.GetGestionProduccion(_codProyecto, _codConvocatoria);

            // Total number of rows.
            int rowCnt;
            // Current row count.
            int rowCtr;
            // Total number of cells per row (columns).
            int cellCtr;
            // Current cell counter
            int cellCnt;

            rowCnt = gestion.Count();
            cellCnt = 3; //Numero de columnas <----- OJO

            for (rowCtr = 1; rowCtr <= rowCnt; rowCtr++)
            {
                // Create new row and add it to the table.
                TableRow tRow = new TableRow();
                tbGestionProduccion.Rows.Add(tRow);
                int columna = 0;
                for (cellCtr = 1; cellCtr <= cellCnt; cellCtr++)
                {
                    // Create a new cell and add it to the row.
                    TableCell tCell = new TableCell();
                    // BorderStyle="Solid" BorderWidth="1px"
                    tCell.BorderStyle = BorderStyle.Solid;
                    tCell.BorderWidth = 1;
                    int indice = rowCtr - 1;

                    switch (columna)
                    {
                        case 0:
                            tCell.Text = gestion[indice].visita.ToString();
                            break;
                        case 1:
                            tCell.Text = gestion[indice].cantidad.ToString();
                            break;
                        case 2:
                            tCell.Text = gestion[indice].Descripcion;
                            break;

                        default:
                            tCell.Text = "";
                            break;
                    }
                    columna++;
                    tRow.Cells.Add(tCell);
                }
            }

        }

        ActaSeguimGestionVentasController gestionVentasController = new ActaSeguimGestionVentasController();
        MetasVentasControllerDTO metasVentasController = new MetasVentasControllerDTO();
        private decimal MetaVenta(int _codProyecto, int _codConvocatoria)
        {
            decimal meta = 0;

            var listmetas = metasVentasController.ListMetasVentas(_codProyecto, _codConvocatoria);

            meta = listmetas.Select(x => x.ventas).FirstOrDefault();

            return meta;
        }
        private void cargarGestVentas(int _codProyecto, int _codConvocatoria)
        {
            var gestion = gestionVentasController.GetGestionVentas(_codProyecto, _codConvocatoria);

            lblMetaVentas.Text = MetaVenta(CodigoProyecto, CodigoConvocatoria).ToString("C");

            // Total number of rows.
            int rowCnt;
            // Current row count.
            int rowCtr;
            // Total number of cells per row (columns).
            int cellCtr;
            // Current cell counter
            int cellCnt;

            rowCnt = gestion.Count();
            cellCnt = 3; //Numero de columnas <----- OJO

            for (rowCtr = 1; rowCtr <= rowCnt; rowCtr++)
            {
                // Create new row and add it to the table.
                TableRow tRow = new TableRow();
                tbGestionVentas.Rows.Add(tRow);
                int columna = 0;
                for (cellCtr = 1; cellCtr <= cellCnt; cellCtr++)
                {
                    // Create a new cell and add it to the row.
                    TableCell tCell = new TableCell();
                    // BorderStyle="Solid" BorderWidth="1px"
                    tCell.BorderStyle = BorderStyle.Solid;
                    tCell.BorderWidth = 1;
                    int indice = rowCtr - 1;

                    switch (columna)
                    {
                        case 0:
                            tCell.Text = gestion[indice].visita.ToString();
                            break;
                        case 1:
                            tCell.Text = gestion[indice].valorFormato;
                            break;
                        case 2:
                            tCell.Text = gestion[indice].descripcion;
                            break;

                        default:
                            tCell.Text = "";
                            break;
                    }
                    columna++;
                    tRow.Cells.Add(tCell);
                }
            }

        }

        #endregion

        //Pagina 7
        #region Pagina 7

        ActaSeguimObligContablesController obligContablesController = new ActaSeguimObligContablesController();
        private void cargarCumplObligContables(int _codProyecto, int _codConvocatoria)
        {
            var obligacion = obligContablesController.GetObligContable(_codProyecto, _codConvocatoria);

            // Total number of rows.
            int rowCnt;
            // Current row count.
            int rowCtr;
            // Total number of cells per row (columns).
            int cellCtr;
            // Current cell counter
            int cellCnt;

            rowCnt = obligacion.Count();
            cellCnt = 7; //Numero de columnas <----- OJO

            for (rowCtr = 1; rowCtr <= rowCnt; rowCtr++)
            {
                // Create new row and add it to the table.
                TableRow tRow = new TableRow();
                tbcumplObligContable.Rows.Add(tRow);
                int columna = 0;
                for (cellCtr = 1; cellCtr <= cellCnt; cellCtr++)
                {
                    // Create a new cell and add it to the row.
                    TableCell tCell = new TableCell();
                    // BorderStyle="Solid" BorderWidth="1px"
                    tCell.BorderStyle = BorderStyle.Solid;
                    tCell.BorderWidth = 1;
                    int indice = rowCtr - 1;

                    switch (columna)
                    {
                        case 0:
                            tCell.Text = obligacion[indice].visita.ToString();
                            break;
                        case 1:
                            tCell.Text = obligacion[indice].estadosFinancieros;
                            break;
                        case 2:
                            tCell.Text = obligacion[indice].librosComerciales;
                            break;
                        case 3:
                            tCell.Text = obligacion[indice].librosContabilidad;
                            break;
                        case 4:
                            tCell.Text = obligacion[indice].conciliacionBancaria;
                            break;
                        case 5:
                            tCell.Text = obligacion[indice].cuentaBancaria;
                            break;
                        case 6:
                            tCell.Text = obligacion[indice].observObligacionContable;
                            break;

                        default:
                            tCell.Text = "";
                            break;
                    }
                    columna++;
                    tRow.Cells.Add(tCell);
                }
            }

        }

        private void cargarCumpObliTributaria(int _codProyecto, int _codConvocatoria)
        {
            var obligacion = obligContablesController.GetObligTributaria(_codProyecto, _codConvocatoria);

            // Total number of rows.
            int rowCnt;
            // Current row count.
            int rowCtr;
            // Total number of cells per row (columns).
            int cellCtr;
            // Current cell counter
            int cellCnt;

            rowCnt = obligacion.Count();
            cellCnt = 10; //Numero de columnas <----- OJO

            for (rowCtr = 1; rowCtr <= rowCnt; rowCtr++)
            {
                // Create new row and add it to the table.
                TableRow tRow = new TableRow();
                tbCumpObligTributaria.Rows.Add(tRow);
                int columna = 0;
                for (cellCtr = 1; cellCtr <= cellCnt; cellCtr++)
                {
                    // Create a new cell and add it to the row.
                    TableCell tCell = new TableCell();
                    // BorderStyle="Solid" BorderWidth="1px"
                    tCell.BorderStyle = BorderStyle.Solid;
                    tCell.BorderWidth = 1;
                    int indice = rowCtr - 1;

                    switch (columna)
                    {
                        case 0:
                            tCell.Text = obligacion[indice].visita.ToString();
                            break;
                        case 1:
                            tCell.Text = obligacion[indice].declaraReteFuente;
                            break;
                        case 2:
                            tCell.Text = obligacion[indice].autorretencionRenta;
                            break;
                        case 3:
                            tCell.Text = obligacion[indice].declaraIva;
                            break;
                        case 4:
                            tCell.Text = obligacion[indice].declaImpConsumo;
                            break;
                        case 5:
                            tCell.Text = obligacion[indice].declaRenta;
                            break;
                        case 6:
                            tCell.Text = obligacion[indice].declaInfoExogena;
                            break;
                        case 7:
                            tCell.Text = obligacion[indice].declaIndustriaComercio;
                            break;
                        case 8:
                            tCell.Text = obligacion[indice].declaRetencionImpIndusComercio;
                            break;
                        case 9:
                            tCell.Text = obligacion[indice].observObligacionTributaria;
                            break;

                        default:
                            tCell.Text = "";
                            break;
                    }
                    columna++;
                    tRow.Cells.Add(tCell);
                }
            }
        }

        private void cargarCumpObligLaboral(int _codProyecto, int _codConvocatoria)
        {
            var obligacion = obligContablesController.GetObligLaboral(_codProyecto, _codConvocatoria);

            // Total number of rows.
            int rowCnt;
            // Current row count.
            int rowCtr;
            // Total number of cells per row (columns).
            int cellCtr;
            // Current cell counter
            int cellCnt;

            rowCnt = obligacion.Count();
            cellCnt = 9; //Numero de columnas <----- OJO

            for (rowCtr = 1; rowCtr <= rowCnt; rowCtr++)
            {
                // Create new row and add it to the table.
                TableRow tRow = new TableRow();
                tbCumpObligLaborales.Rows.Add(tRow);
                int columna = 0;
                for (cellCtr = 1; cellCtr <= cellCnt; cellCtr++)
                {
                    // Create a new cell and add it to the row.
                    TableCell tCell = new TableCell();
                    // BorderStyle="Solid" BorderWidth="1px"
                    tCell.BorderStyle = BorderStyle.Solid;
                    tCell.BorderWidth = 1;
                    int indice = rowCtr - 1;

                    switch (columna)
                    {
                        case 0:
                            tCell.Text = obligacion[indice].visita.ToString();
                            break;
                        case 1:
                            tCell.Text = obligacion[indice].contratosLaborales;
                            break;
                        case 2:
                            tCell.Text = obligacion[indice].pagosNomina;
                            break;
                        case 3:
                            tCell.Text = obligacion[indice].afiliacionSegSocial;
                            break;
                        case 4:
                            tCell.Text = obligacion[indice].pagoSegSocial;
                            break;
                        case 5:
                            tCell.Text = obligacion[indice].certParafiscalesSegSocial;
                            break;
                        case 6:
                            tCell.Text = obligacion[indice].reglaInternoTrab;
                            break;
                        case 7:
                            tCell.Text = obligacion[indice].sisGestionSegSaludTrabajo;
                            break;
                        case 8:
                            tCell.Text = obligacion[indice].observObligacionLaboral;
                            break;

                        default:
                            tCell.Text = "";
                            break;
                    }
                    columna++;
                    tRow.Cells.Add(tCell);
                }
            }
        }
        #endregion

        //Pagina 8
        #region Pagina 8
        private void cargarVerficaRegTramites(int _codProyecto, int _codConvocatoria)
        {
            var obligacion = obligContablesController.GetObligTramite(_codProyecto, _codConvocatoria);

            // Total number of rows.
            int rowCnt;
            // Current row count.
            int rowCtr;
            // Total number of cells per row (columns).
            int cellCtr;
            // Current cell counter
            int cellCnt;

            rowCnt = obligacion.Count();
            cellCnt = 12; //Numero de columnas <----- OJO

            for (rowCtr = 1; rowCtr <= rowCnt; rowCtr++)
            {
                // Create new row and add it to the table.
                TableRow tRow = new TableRow();
                tbVerificaRegTramites.Rows.Add(tRow);
                int columna = 0;
                for (cellCtr = 1; cellCtr <= cellCnt; cellCtr++)
                {
                    // Create a new cell and add it to the row.
                    TableCell tCell = new TableCell();
                    // BorderStyle="Solid" BorderWidth="1px"
                    tCell.BorderStyle = BorderStyle.Solid;
                    tCell.BorderWidth = 1;
                    int indice = rowCtr - 1;

                    switch (columna)
                    {
                        case 0:
                            tCell.Text = obligacion[indice].visita.ToString();
                            break;
                        case 1:
                            tCell.Text = obligacion[indice].insCamaraComercio;
                            break;
                        case 2:
                            tCell.Text = obligacion[indice].renovaRegistroMercantil;
                            break;
                        case 3:
                            tCell.Text = obligacion[indice].rut;
                            break;
                        case 4:
                            tCell.Text = obligacion[indice].resolFacturacion;
                            break;
                        case 5:
                            tCell.Text = obligacion[indice].certLibertadTradicion;
                            break;
                        case 6:
                            tCell.Text = obligacion[indice].DocumentoIdoneidad;
                            break;
                        case 7:
                            tCell.Text = obligacion[indice].permisoUsoSuelo;
                            break;
                        case 8:
                            tCell.Text = obligacion[indice].certBomberos;
                            break;
                        case 9:
                            tCell.Text = obligacion[indice].regMarca;
                            break;
                        case 10:
                            tCell.Text = obligacion[indice].otrosPermisos;
                            break;
                        case 11:
                            tCell.Text = obligacion[indice].observRegistroTramiteLicencia;
                            break;

                        default:
                            tCell.Text = "";
                            break;
                    }
                    columna++;
                    tRow.Cells.Add(tCell);
                }
            }
        }

        ActaSeguimOtrosAspectosController otrosAspectosController = new ActaSeguimOtrosAspectosController();
        private void cargarCompInnovadorOtrosAspectos(int _codProyecto, int _codConvocatoria)
        {
            var descripOtrosAps = otrosAspectosController.getDescripcionOtrosAspectos(_codProyecto, _codConvocatoria);

            if (descripOtrosAps == null)
            {
                lblCompAmbiental.Text = "";
                lblCompInnovador.Text = "";
            }
            else
            {
                lblCompAmbiental.Text = descripOtrosAps.DescripCompAmbiental;
                lblCompInnovador.Text = descripOtrosAps.DescripCompInnovador;
            }
        }

        private void cargarOtrosAspectosCompInnova(int _codProyecto, int _codConvocatoria)
        {
            var innovacion = otrosAspectosController.getOtrosAspectosInnovador(_codProyecto, _codConvocatoria);

            // Total number of rows.
            int rowCnt;
            // Current row count.
            int rowCtr;
            // Total number of cells per row (columns).
            int cellCtr;
            // Current cell counter
            int cellCnt;

            rowCnt = innovacion.Count();
            cellCnt = 3; //Numero de columnas <----- OJO

            for (rowCtr = 1; rowCtr <= rowCnt; rowCtr++)
            {
                // Create new row and add it to the table.
                TableRow tRow = new TableRow();
                tbCompInnovador.Rows.Add(tRow);
                int columna = 0;
                for (cellCtr = 1; cellCtr <= cellCnt; cellCtr++)
                {
                    // Create a new cell and add it to the row.
                    TableCell tCell = new TableCell();
                    // BorderStyle="Solid" BorderWidth="1px"
                    tCell.BorderStyle = BorderStyle.Solid;
                    tCell.BorderWidth = 1;
                    int indice = rowCtr - 1;

                    switch (columna)
                    {
                        case 0:
                            tCell.Text = innovacion[indice].visita.ToString();
                            break;
                        case 1:
                            tCell.Text = innovacion[indice].valoracion;
                            break;
                        case 2:
                            tCell.Text = innovacion[indice].observacion;
                            break;

                        default:
                            tCell.Text = "";
                            break;
                    }
                    columna++;
                    tRow.Cells.Add(tCell);
                }
            }
        }

        private void cargarOtrosAspectosAmbiental(int _codProyecto, int _codConvocatoria)
        {
            var ambiental = otrosAspectosController.getOtrosAspectosAmbiental(_codProyecto, _codConvocatoria);

            // Total number of rows.
            int rowCnt;
            // Current row count.
            int rowCtr;
            // Total number of cells per row (columns).
            int cellCtr;
            // Current cell counter
            int cellCnt;

            rowCnt = ambiental.Count();
            cellCnt = 3; //Numero de columnas <----- OJO

            for (rowCtr = 1; rowCtr <= rowCnt; rowCtr++)
            {
                // Create new row and add it to the table.
                TableRow tRow = new TableRow();
                tbCompAmbiental.Rows.Add(tRow);
                int columna = 0;
                for (cellCtr = 1; cellCtr <= cellCnt; cellCtr++)
                {
                    // Create a new cell and add it to the row.
                    TableCell tCell = new TableCell();
                    // BorderStyle="Solid" BorderWidth="1px"
                    tCell.BorderStyle = BorderStyle.Solid;
                    tCell.BorderWidth = 1;
                    int indice = rowCtr - 1;

                    switch (columna)
                    {
                        case 0:
                            tCell.Text = ambiental[indice].visita.ToString();
                            break;
                        case 1:
                            tCell.Text = ambiental[indice].valoracion;
                            break;
                        case 2:
                            tCell.Text = ambiental[indice].observacion;
                            break;

                        default:
                            tCell.Text = "";
                            break;
                    }
                    columna++;
                    tRow.Cells.Add(tCell);
                }
            }
        }
        #endregion

        ActaSeguimOtrasObligacionesController otrasObligacionesController = new ActaSeguimOtrasObligacionesController();
        //Pagina 9
        #region Pagina 9 
        private void cargarReportInforPlataforma(int _codProyecto, int _codConvocatoria)
        {

            var infoPlataforma = otrasObligacionesController
           .GetOtrasObligInfoPlataforma(_codProyecto, _codConvocatoria);

            // Total number of rows.
            int rowCnt;
            // Current row count.
            int rowCtr;
            // Total number of cells per row (columns).
            int cellCtr;
            // Current cell counter
            int cellCnt;

            rowCnt = infoPlataforma.Count();
            cellCnt = 3; //Numero de columnas <----- OJO

            for (rowCtr = 1; rowCtr <= rowCnt; rowCtr++)
            {
                // Create new row and add it to the table.
                TableRow tRow = new TableRow();
                tbRepInfoPlataforma.Rows.Add(tRow);
                int columna = 0;
                for (cellCtr = 1; cellCtr <= cellCnt; cellCtr++)
                {
                    // Create a new cell and add it to the row.
                    TableCell tCell = new TableCell();
                    // BorderStyle="Solid" BorderWidth="1px"
                    tCell.BorderStyle = BorderStyle.Solid;
                    tCell.BorderWidth = 1;
                    int indice = rowCtr - 1;

                    switch (columna)
                    {
                        case 0:
                            tCell.Text = infoPlataforma[indice].visita.ToString();
                            break;
                        case 1:
                            tCell.Text = infoPlataforma[indice].valoracion;
                            break;
                        case 2:
                            tCell.Text = infoPlataforma[indice].observacion;
                            break;

                        default:
                            tCell.Text = "";
                            break;
                    }
                    columna++;
                    tRow.Cells.Add(tCell);
                }
            }
        }

        private void cargarTiempoDedicaEmp(int _codProyecto, int _codConvocatoria)
        {

            var dedicaEmp = otrasObligacionesController
                .GetOtrasObligDedicaEmprendedor(_codProyecto, _codConvocatoria);

            // Total number of rows.
            int rowCnt;
            // Current row count.
            int rowCtr;
            // Total number of cells per row (columns).
            int cellCtr;
            // Current cell counter
            int cellCnt;

            rowCnt = dedicaEmp.Count();
            cellCnt = 3; //Numero de columnas <----- OJO

            for (rowCtr = 1; rowCtr <= rowCnt; rowCtr++)
            {
                // Create new row and add it to the table.
                TableRow tRow = new TableRow();
                tbTiempoDedicaEmp.Rows.Add(tRow);
                int columna = 0;
                for (cellCtr = 1; cellCtr <= cellCnt; cellCtr++)
                {
                    // Create a new cell and add it to the row.
                    TableCell tCell = new TableCell();
                    // BorderStyle="Solid" BorderWidth="1px"
                    tCell.BorderStyle = BorderStyle.Solid;
                    tCell.BorderWidth = 1;
                    int indice = rowCtr - 1;

                    switch (columna)
                    {
                        case 0:
                            tCell.Text = dedicaEmp[indice].visita.ToString();
                            break;
                        case 1:
                            tCell.Text = dedicaEmp[indice].valoracion;
                            break;
                        case 2:
                            tCell.Text = dedicaEmp[indice].observacion;
                            break;

                        default:
                            tCell.Text = "";
                            break;
                    }
                    columna++;
                    tRow.Cells.Add(tCell);
                }
            }
        }

        private void cargarAcompUnidadEmp(int _codProyecto, int _codConvocatoria)
        {

            var acomAsesoria = otrasObligacionesController
                .GetOtrasObligAcomAsesoria(_codProyecto, _codConvocatoria);

            // Total number of rows.
            int rowCnt;
            // Current row count.
            int rowCtr;
            // Total number of cells per row (columns).
            int cellCtr;
            // Current cell counter
            int cellCnt;

            rowCnt = acomAsesoria.Count();
            cellCnt = 3; //Numero de columnas <----- OJO

            for (rowCtr = 1; rowCtr <= rowCnt; rowCtr++)
            {
                // Create new row and add it to the table.
                TableRow tRow = new TableRow();
                tbAcomUnidadEmp.Rows.Add(tRow);
                int columna = 0;
                for (cellCtr = 1; cellCtr <= cellCnt; cellCtr++)
                {
                    // Create a new cell and add it to the row.
                    TableCell tCell = new TableCell();
                    // BorderStyle="Solid" BorderWidth="1px"
                    tCell.BorderStyle = BorderStyle.Solid;
                    tCell.BorderWidth = 1;
                    int indice = rowCtr - 1;

                    switch (columna)
                    {
                        case 0:
                            tCell.Text = acomAsesoria[indice].visita.ToString();
                            break;
                        case 1:
                            tCell.Text = acomAsesoria[indice].valoracion;
                            break;
                        case 2:
                            tCell.Text = acomAsesoria[indice].observacion;
                            break;

                        default:
                            tCell.Text = "";
                            break;
                    }
                    columna++;
                    tRow.Cells.Add(tCell);
                }
            }
        }

        ActaSeguimEstadoEmpresaController estadoEmpresaController = new ActaSeguimEstadoEmpresaController();
        private void cargarEstadoEmpresa(int _codProyecto, int _codConvocatoria)
        {
            var estadoEmpresa = estadoEmpresaController
                .getListEstadoEmpresa(_codProyecto, _codConvocatoria);

            // Total number of rows.
            int rowCnt;
            // Current row count.
            int rowCtr;
            // Total number of cells per row (columns).
            int cellCtr;
            // Current cell counter
            int cellCnt;

            rowCnt = estadoEmpresa.Count();
            cellCnt = 2; //Numero de columnas <----- OJO

            for (rowCtr = 1; rowCtr <= rowCnt; rowCtr++)
            {
                // Create new row and add it to the table.
                TableRow tRow = new TableRow();
                tbEstadoEmpresa.Rows.Add(tRow);
                int columna = 0;
                for (cellCtr = 1; cellCtr <= cellCnt; cellCtr++)
                {
                    // Create a new cell and add it to the row.
                    TableCell tCell = new TableCell();
                    // BorderStyle="Solid" BorderWidth="1px"
                    tCell.BorderStyle = BorderStyle.Solid;
                    tCell.BorderWidth = 1;
                    int indice = rowCtr - 1;

                    switch (columna)
                    {
                        case 0:
                            tCell.Text = estadoEmpresa[indice].visita.ToString();
                            break;
                        case 1:
                            tCell.Text = estadoEmpresa[indice].Descripcion;
                            break;

                        default:
                            tCell.Text = "";
                            break;
                    }
                    columna++;
                    tRow.Cells.Add(tCell);
                }
            }
        }

        #endregion

        //Pagina 10
        #region Pagina 10
        ActaSeguimCompromisosController compromisosController = new ActaSeguimCompromisosController();
        private void cargaCompromisos(int _codProyecto, int _codConvocatoria)
        {
            var histCompromisos = compromisosController.getCompromisosHist(_codProyecto, _codConvocatoria);

            // Total number of rows.
            int rowCnt;
            // Current row count.
            int rowCtr;
            // Total number of cells per row (columns).
            int cellCtr;
            // Current cell counter
            int cellCnt;

            rowCnt = histCompromisos.Count();
            cellCnt = 6; //Numero de columnas <----- OJO

            for (rowCtr = 1; rowCtr <= rowCnt; rowCtr++)
            {
                // Create new row and add it to the table.
                TableRow tRow = new TableRow();
                tbCompromisos.Rows.Add(tRow);
                int columna = 0;
                for (cellCtr = 1; cellCtr <= cellCnt; cellCtr++)
                {
                    // Create a new cell and add it to the row.
                    TableCell tCell = new TableCell();
                    // BorderStyle="Solid" BorderWidth="1px"
                    tCell.BorderStyle = BorderStyle.Solid;
                    tCell.BorderWidth = 1;
                    int indice = rowCtr - 1;

                    switch (columna)
                    {
                        case 0:
                            tCell.Text = histCompromisos[indice].visita.ToString();
                            break;
                        case 1:
                            tCell.Text = histCompromisos[indice].compromiso;
                            break;
                        case 2:
                            tCell.Text = histCompromisos[indice].fechaPropuestaEjec.ToShortDateString();
                            break;
                        case 3:
                            tCell.Text = histCompromisos[indice].estado;
                            break;
                        case 4:
                            tCell.Text = histCompromisos[indice].fechaCumpliCompromiso.HasValue ?
                                           histCompromisos[indice].fechaCumpliCompromiso.Value.ToShortDateString() : "";
                            break;
                        case 5:
                            tCell.Text = histCompromisos[indice].observacion;
                            break;

                        default:
                            tCell.Text = "";
                            break;
                    }
                    columna++;
                    tRow.Cells.Add(tCell);
                }
            }
        }

        #endregion

        //Pagina 11
        #region Pagina 11
        DatosActaDTOController datosActaDTOController = new DatosActaDTOController();
        ActaSeguimientoInterventoriaController interventoriaController = new ActaSeguimientoInterventoriaController();
        ActaSeguimientoDatosController actaSeguimientoDatosController = new ActaSeguimientoDatosController();
        private void cargarInfoFirmas(int _codProyecto, int _numActa)
        {
            var actaInterventor = interventoriaController.GetActaInterventoria(_codProyecto, _numActa);

            lblFechaPublicacionActa.Text = actaInterventor.FechaPublicacion.HasValue ?
                                            actaInterventor.FechaPublicacion.Value.ToShortDateString() : "";

            lblHoraPublicacionActa.Text = actaInterventor.FechaPublicacion.HasValue ?
                                            actaInterventor.FechaPublicacion.Value.ToShortTimeString() : "";

            var datosContac = datosActaDTOController.GetContactosProyecto(_codProyecto);

            int codRolAsesorLider = 1;//Codigo Asesor Lider
            int codRolAsesor = 2;//Codigo Asesor
            int codRolEmprendedor = 3;//Codigo Emprendedor
            int codRolInterventor = 6;//Codigo Interventor
            int codRolCoordinadorInterventor = 7;//Codigo Coordinador Interventor
            int codRolInterventorLider = 8;//Codigo Interventor Lider

            //Cargar Contratista

            string htmlContratista =
                "<table style=\"width: 100%;\" class=\"borderTabla\" >" +
                   "<tr>"+
                       "<td class=\"auto-style7\" ><strong>NOMBRE:</strong></td>"+
                        "<td class=\"auto-style9\" >"+
                          "[NOMBRE]"+
                        "</td>"+
                        "<td>&nbsp;</td>"+
                    "</tr>"+
                    "<tr>"+
                        "<td class=\"auto-style7\" > Rol:</td>" +
                        "<td class=\"auto-style9\" > Contratista/Emprendedor </td>"+
                        "<td>&nbsp;</td>"+
                    "</tr>"+
                    "<tr>"+
                        "<td class=\"auto-style7\" > Correo:</td>"+
                        "<td class=\"auto-style9\">"+
                           "[CORREO]" +
                        "</td>"+
                        "<td>&nbsp;</td>"+
                    "</tr>"+
                    "<tr>"+
                        "<td class=\"auto -style7\" > Teléfono:</td>" +
                        "<td class=\"auto-style9\" > " +
                          "[TELEFONO]" +
                        "</td>"+
                        "<td class=\"centered\" > Firma</td>" +
                    "</tr>"+
                "</table>";

            var contratista = datosContac.Where(x => x.codRol == codRolEmprendedor).ToList();

            foreach(var c in contratista)
            {
                string html = htmlContratista
                                .Replace("[NOMBRE]", c.Nombres + " " + c.Apellidos)
                                .Replace("[CORREO]", c.Email)
                                .Replace("[TELEFONO]", c.Telefono);

                datosContratista.InnerHtml = html;
            }

            //Cargar info ASESOR
            var Asesor = datosContac.Where(x => (x.codRol == codRolAsesor || x.codRol == codRolAsesorLider)).FirstOrDefault();

            lblNombreGestorOperativo.Text = Asesor.Nombres + " " + Asesor.Apellidos;
            lblCorreoGesOperativo.Text = Asesor.Email;
            lblTelefonoGesOpera.Text = Asesor.Telefono;

            //Cargar Info Gestor Tecnico SENA
            var actaInfo = actaSeguimientoDatosController.obtenerDatosActa(_numActa, _codProyecto);

            lblNombreGestorTecnico.Text = actaInfo.NombreGestorTecnicoSena;
            lblCorreoGesTecnico.Text = actaInfo.EmailGestorTecnicoSena;
            lblTelefonoGesTecnico.Text = actaInfo.TelefonoGestorOperativoSena;

            // Cargar info interventor
            var datosInterventor = datosActaDTOController.GetContactosInterventor(_codProyecto);

            var dataInter = datosInterventor.Where(x => (x.codRol == codRolInterventor)
                                                        ||(x.codRol == codRolInterventorLider)
                                                        ||(x.codRol == codRolCoordinadorInterventor)).FirstOrDefault();

            lblNombreInterventor.Text = dataInter.Nombres + " " + dataInter.Apellidos;
            lblCorreoInterventor.Text = dataInter.Email;
            lblTelefonoInterventor.Text = dataInter.Telefono;

            var entidadInterv = datosActaDTOController.getInfoEntidadInteventora(_codProyecto,usuario.IdContacto);

            lblInterventorUniversidad.Text = entidadInterv.nombreInterventor;
            //@"file:///M:/" + entidadInterv.rutaLogo.Trim()
            //string path = Server.MapPath(@"file:///M:/" + entidadInterv.rutaLogo.Trim());
            //CargarLogos
            imgInterventorPAG1.ImageUrl = "Image.aspx?FileName="+entidadInterv.rutaLogo.Trim();
            imgInterventorPAG2.ImageUrl = "Image.aspx?FileName=" + entidadInterv.rutaLogo.Trim();
            imgPagina3.ImageUrl = "Image.aspx?FileName=" + entidadInterv.rutaLogo.Trim();
            imgPag4.ImageUrl = "Image.aspx?FileName=" + entidadInterv.rutaLogo.Trim();
            imgPag5.ImageUrl = "Image.aspx?FileName=" + entidadInterv.rutaLogo.Trim();
            imgPag6.ImageUrl = "Image.aspx?FileName=" + entidadInterv.rutaLogo.Trim();
            imgPag7.ImageUrl = "Image.aspx?FileName=" + entidadInterv.rutaLogo.Trim();
            imgPag8.ImageUrl = "Image.aspx?FileName=" + entidadInterv.rutaLogo.Trim();
            imgPag9.ImageUrl = "Image.aspx?FileName=" + entidadInterv.rutaLogo.Trim();
            imgPag10.ImageUrl = "Image.aspx?FileName=" + entidadInterv.rutaLogo.Trim();
            imgPag11.ImageUrl = "Image.aspx?FileName=" + entidadInterv.rutaLogo.Trim();

        }
        public string baseDirectory = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual");
        #endregion
    }
}