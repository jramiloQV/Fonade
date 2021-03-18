using Datos;
using Datos.DataType;
using Fonade.Account;
using Fonade.Negocio.Mensajes;
using Fonade.Negocio.PlanDeNegocioV2.DesarrolloSolucion;
using Fonade.Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion;
using Fonade.Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio;
using Fonade.Negocio.PlanDeNegocioV2.Formulacion.ResumenEjecutivo;
using Fonade.Negocio.PlanDeNegocioV2.Formulacion.Riesgos;
using Fonade.Negocio.PlanDeNegocioV2.Utilidad;
using Fonade.PlanDeNegocioV2.Ejecucion.Empresa.Avance;
using Fonade.PlanDeNegocioV2.Formulacion.Solucion;
using Fonade.PlanDeNegocioV2.Formulacion.Utilidad;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.ImpresionPlanNegocio
{
    public partial class VerImpresion : System.Web.UI.Page
    {
        /// <summary>
        /// Código del proyecto
        /// </summary>
        public int CodigoProyecto
        {
            get
            {
                return Request.QueryString.AllKeys.Contains("CodigoProyecto") ? int.Parse(Request.QueryString["CodigoProyecto"].ToString()) : 0;
            }
        }

        public string[] ListaSel
        {
            get
            {
                return Request.QueryString["ListaSel"].ToString().Split(',');
            }
        }

        public string NombreProyecto
        {
            get
            {
                return Request.QueryString.AllKeys.Contains("NomProyecto") ? Request.QueryString["NomProyecto"].ToString() : "";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblimpresion.Text = NombreProyecto;

                if (CodigoProyecto > 0)
                {
                    //Se Muestra la información del proyecto en el panel principal
                    CargarInfoProyecto();

                    //Se recorre las secciones / subsecciones seleccionadas y se imprimen
                    if (!ListaSel[0].Equals("-1"))
                    {
                        ImprimirSecciones();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
        }

        private void CargarInfoProyecto()
        {
            //Se consulta la información del proyecto
            var result = Negocio.PlanDeNegocioV2.ImpresionPlanNeg.ImpresionPlanNeg.getInfoProyecto(CodigoProyecto);

            if (result != null)
            {
                if (result.Id_Proyecto <= 0)
                {
                    pnlinfoproyecto.Visible = false;
                }
                else
                {
                    

                    lblnombreproyecto.Text = result.NomProyecto;
                    lblinstitucionproyecto.Text = result.nomunidad + " (" + result.nomInstitucion + ")";
                    lblsubsectorproyecto.Text = result.nomSubsector;
                    lblciudadproyecto.Text = result.Lugar;                   
                    lblrecursosproyecto.Text = result.Recursos.ToString();

                    lblfechacreacionproyecto.Text = result.FechaCreacion.ToString();
                    lblsumarioproyecto.Text = result.Sumario.ToString();
                }
            }
            else
            {
                pnlinfoproyecto.Visible = false;
            }
        }

        private void ImprimirSecciones()
        {
            foreach (var item in ListaSel)
            {
                ArmarSeccion(int.Parse(item));
            }
        }

        private void ArmarSeccion(int codTab)
        {

            switch (codTab)
            {
                case Constantes.CONST_Protagonista:

                    List<ProyectoProtagonistaCliente> listClientes = Negocio.PlanDeNegocioV2.Formulacion.Protagonista.Protagonista.GetClientes(CodigoProyecto);
                    ProyectoProtagonista protago = Negocio.PlanDeNegocioV2.Formulacion.Protagonista.Protagonista.GetProtagonista(CodigoProyecto);

                    ImpresionProtagonista.ListClientes = listClientes;
                    ImpresionProtagonista.Protagonista = protago;
                    ImpresionProtagonista.Visible = true;
                    break;

                case Constantes.CONST_OportunidadMercado:

                    List<ProyectoOportunidadMercadoCompetidore> listCompetidores = Negocio.PlanDeNegocioV2.Formulacion.OportunidadMercado.Competidores.GetCompetidores(CodigoProyecto);
                    ProyectoOportunidadMercado oportunidad = Negocio.PlanDeNegocioV2.Formulacion.OportunidadMercado.Oportunidad.GetOportunidad(CodigoProyecto);

                    ImpresionOportunidad.ListCompetidores = listCompetidores;
                    ImpresionOportunidad.Oportunidad = oportunidad;
                    ImpresionOportunidad.Visible = true;
                    break;

                case Constantes.CONST_Parte1Solucion:

                    ProyectoSolucion solucion = Negocio.PlanDeNegocioV2.Formulacion.Solucion.Solucion.Get(CodigoProyecto);

                    ImpresionPPalSolucion.ImpresionSolucionPta1.Solucion = solucion;
                    ImpresionPPalSolucion.ImpresionSolucionPta1.Visible = true;
                    ImpresionPPalSolucion.Visible = true;
                    break;

                case Constantes.CONST_Parte2FichaTecnica:

                    List<ProyectoProducto> listProductos = Negocio.PlanDeNegocioV2.Formulacion.Solucion.Producto.GetProductosByProyecto(CodigoProyecto);

                    ImpresionPPalSolucion.ImpresionSolucionPta2.ListadoProductos = listProductos;
                    ImpresionPPalSolucion.ImpresionSolucionPta2.Visible = true;
                    ImpresionPPalSolucion.Visible = true;
                    break;

                case Constantes.CONST_Paso1IngresoCondicionesComerciales:

                    List<CondicionesCliente> listCondiciones = IngresosYCondicionesComercio.getCondicionesClientes(CodigoProyecto);
                    Boolean esConsumidor = IngresosYCondicionesComercio.esConsumidor(CodigoProyecto);
                    ProyectoDesarrolloSolucion formulario = IngresosYCondicionesComercio.getFormulario(CodigoProyecto);

                    ImpresionPpalDesarrollo.ImpresionIngresoCondiciones.ListCondiciones = listCondiciones;
                    ImpresionPpalDesarrollo.ImpresionIngresoCondiciones.Formulario = formulario;
                    ImpresionPpalDesarrollo.ImpresionIngresoCondiciones.EsClienteConsumidor = esConsumidor;
                    ImpresionPpalDesarrollo.ImpresionIngresoCondiciones.Visible = true;
                    ImpresionPpalDesarrollo.Visible = true;
                    break;

                case Constantes.CONST_Paso2Proyeccion:

                    List<ProyectoProducto> lstProductos = Negocio.PlanDeNegocioV2.Formulacion.Solucion.Producto.GetProductosByProyecto(CodigoProyecto);

                    ImpresionPpalDesarrollo.ImpresionProyeccions.ListadoProductos = lstProductos;
                    ImpresionPpalDesarrollo.ImpresionProyeccions.CodigoProyecto = CodigoProyecto;
                    ImpresionPpalDesarrollo.ImpresionProyeccions.Visible = true;
                    ImpresionPpalDesarrollo.Visible = true;
                    break;

                case Constantes.CONST_Paso3NormatividadCondicionesTecnicas:

                    ProyectoNormatividad normatividad = Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.NormatividadYCondicionTech.getFormulario(CodigoProyecto);

                    ImpresionPpalDesarrollo.ImpresionNormas.Formulario = normatividad;
                    ImpresionPpalDesarrollo.ImpresionNormas.Visible = true;
                    ImpresionPpalDesarrollo.Visible = true;
                    break;

                case Constantes.CONST_Paso4Requerimientos:

                    List<RequerimientosNeg> listReqNegocio = RequerimientosNegocio.getRequerimientos(CodigoProyecto, Constantes.CONST_PlanV2); ;
                    ProyectoRequerimiento requerimientos = RequerimientosNegocio.getFormulario(CodigoProyecto);

                    ImpresionPpalDesarrollo.ImpresionReqNeg.ListRequerimientos = listReqNegocio;
                    ImpresionPpalDesarrollo.ImpresionReqNeg.Formulario = requerimientos;
                    ImpresionPpalDesarrollo.ImpresionReqNeg.Visible = true;
                    ImpresionPpalDesarrollo.Visible = true;
                    break;

                case Constantes.CONST_Paso5Produccion:

                    List<ProductoProceso> listProcesos = Negocio.PlanDeNegocioV2.Formulacion.Solucion.Producto.GetProductosProceso(CodigoProyecto);
                    ProyectoProduccion produccion = ProduccionNegocio.getFormulario(CodigoProyecto);

                    ImpresionPpalDesarrollo.ImpresionProd.ListProcesos = listProcesos;
                    ImpresionPpalDesarrollo.ImpresionProd.Formulario = produccion;
                    ImpresionPpalDesarrollo.ImpresionProd.Visible = true;
                    ImpresionPpalDesarrollo.Visible = true;
                    break;

                case Constantes.CONST_Paso6ProductividadEquipoDeTrabajo:

                    List<ProyectoGastosPersonal> listCargos = Productividad.getCargos(CodigoProyecto);
                    List<EquipoTrabajo> listEquipo = General.getEquipoTrabajo(CodigoProyecto);
                    ProyectoProductividad productividad = Productividad.getFormulario(CodigoProyecto);

                    ImpresionPpalDesarrollo.ImpresionProductiv.ListCargos = listCargos;
                    ImpresionPpalDesarrollo.ImpresionProductiv.ListEmprendedores = listEquipo;
                    ImpresionPpalDesarrollo.ImpresionProductiv.Formulario = productividad;
                    ImpresionPpalDesarrollo.ImpresionProductiv.Visible = true;
                    ImpresionPpalDesarrollo.Visible = true;
                    break;

                case Constantes.CONST_PeriododeArranqueEImproductivo:

                    ProyectoPeriodoArranque periodo = PeriodoArranque.Get(CodigoProyecto);

                    ImpresionPPalFuturo.ImpresionPeriodoArran.Formulario = periodo;
                    ImpresionPPalFuturo.ImpresionPeriodoArran.Visible = true;
                    ImpresionPPalFuturo.Visible = true;

                    break;

                case Constantes.CONST_Estrategias:

                    ProyectoFuturoNegocio estrategia = FuturoNegocio.Get(CodigoProyecto);
                    List<ProyectoEstrategiaActividade> ListPromocion = Actividades.Get(CodigoProyecto, (int)Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.TipoEstrategia.Promocion);
                    List<ProyectoEstrategiaActividade> ListComunicacion = Actividades.Get(CodigoProyecto, (int)Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.TipoEstrategia.Comunicacion);
                    List<ProyectoEstrategiaActividade> ListDistribucion = Actividades.Get(CodigoProyecto, (int)Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.TipoEstrategia.Distribucion);

                    ImpresionPPalFuturo.ImpresionEstrategias.ListComunicacion = ListComunicacion;
                    ImpresionPPalFuturo.ImpresionEstrategias.ListDistribucion = ListDistribucion;
                    ImpresionPPalFuturo.ImpresionEstrategias.ListPromocion = ListPromocion;
                    ImpresionPPalFuturo.ImpresionEstrategias.Formulario = estrategia;
                    ImpresionPPalFuturo.ImpresionEstrategias.Visible = true;
                    ImpresionPPalFuturo.Visible = true;

                    break;

                case Constantes.CONST_Riesgos:

                    ProyectoRiesgo riesgo = Riesgos.Get(CodigoProyecto);

                    ImpresionRiesgo.Formulario = riesgo;
                    ImpresionRiesgo.Visible = true;


                    break;

                case Constantes.CONST_ResumenEjecutivoV2:

                    ProyectoResumenEjecutivoV2 resumen = Resumen.Get(CodigoProyecto);
                    List<Emprendedor> lstEmprendedores = Resumen.GetEmprendedores(CodigoProyecto);

                    ImpresionResumen.Formulario = resumen;
                    ImpresionResumen.ListEmprendedor = lstEmprendedores;
                    ImpresionResumen.Visible = true;

                    break;

                case Constantes.CONST_PlanDeComprasV2:


                    ImpresionPPalEstructura.ImpresionPlandeCompras.CodigoProyecto = CodigoProyecto;
                    ImpresionPPalEstructura.ImpresionPlandeCompras.Visible = true;
                    ImpresionPPalEstructura.Visible = true;

                    break;

                case Constantes.CONST_CostosDeProduccionV2:

                    ImpresionPPalEstructura.ImpresionCostosProduc.CodigoProyecto = CodigoProyecto;
                    ImpresionPPalEstructura.ImpresionCostosProduc.Visible = true;
                    ImpresionPPalEstructura.Visible = true;

                    break;

                case Constantes.CONST_CostosAdministrativosV2:

                    ImpresionPPalEstructura.ImpresionCostosAdmini.CodigoProyecto = CodigoProyecto;
                    ImpresionPPalEstructura.ImpresionCostosAdmini.Visible = true;
                    ImpresionPPalEstructura.Visible = true;

                    break;

                case Constantes.CONST_IngresosV2:

                    ImpresionPPalEstructura.ImpresionIngreso.CodigoProyecto = CodigoProyecto;
                    ImpresionPPalEstructura.ImpresionIngreso.Visible = true;
                    ImpresionPPalEstructura.Visible = true;

                    break;

                case Constantes.CONST_EgresosV2:

                    ImpresionPPalEstructura.ImpresionEgreso.CodigoProyecto = CodigoProyecto;
                    ImpresionPPalEstructura.ImpresionEgreso.Visible = true;
                    ImpresionPPalEstructura.Visible = true;

                    break;

                case Constantes.CONST_CapitalDeTrabajoV2:

                    if (CodigoProyecto != 0)
                    {
                        ImpresionPPalEstructura.ImpresionCapital.CodigoProyecto = CodigoProyecto;
                        ImpresionPPalEstructura.ImpresionCapital.Visible = true;
                        ImpresionPPalEstructura.Visible = true;
                    }

                    break;
                default:
                    break;
            }
        }

    }
}