using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using Fonade.Account;
using Fonade.Clases;
using System.Web.Security;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Fonade.PlanDeNegocioV2.Evaluacion.IndicadoresDeGestion
{
    public partial class IndicadorGestion : System.Web.UI.Page
    {
        public int CodigoProyecto { get { return Convert.ToInt32(Request.QueryString["codproyecto"]); } set { } }
        public int CodigoConvocatoria
        {
            get
            {
                return Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(CodigoProyecto, HttpContext.Current.Session["HistorialEvaluacion"] != null ? Convert.ToInt32(HttpContext.Current.Session["HistorialEvaluacion"]) : 0).GetValueOrDefault();
            }
            set { }
        }
        public int CodigoTab { get { return Constantes.Const_IndicadoresDeGestionV2; } set { } }
        public Boolean EsMiembro { get; set; }
        public Boolean EsRealizado { get; set; }
        protected FonadeUser Usuario { get { return HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true); } set { } }
        
        public Boolean AllowUpdate
        {
            get
            {
                return EsMiembro && !EsRealizado && Usuario.CodGrupo.Equals(Constantes.CONST_Evaluador);
            }
            set { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request["codproyecto"] == null)
                    throw new ApplicationException("No se pudo obtener el codigo del proyecto, intentenlo de nuevo.");

                EncabezadoEval.IdProyecto = CodigoProyecto;
                EncabezadoEval.IdConvocatoria = CodigoConvocatoria;
                EncabezadoEval.IdTabEvaluacion = CodigoTab;

                EsMiembro = Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.EsMienbroDelProyecto(CodigoProyecto, Usuario.IdContacto);
                EsRealizado = Negocio.PlanDeNegocioV2.Utilidad.TabEvaluacion.VerificarTabSiEsRealizado(CodigoTab, CodigoProyecto, CodigoConvocatoria);

                if (!Page.IsPostBack)
                    GetDetails(CodigoProyecto, CodigoConvocatoria);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }
        }

        protected void GetDetails(int codigoProyecto, int codigoConvocatoria)
        {
            ProyectoResumenEjecutivoV2 entResumen = Negocio.PlanDeNegocioV2.Formulacion.ResumenEjecutivo.Resumen.Get(codigoProyecto);

            if (entResumen != null)
            {                
                lblPeriodoImproductivo.Text = entResumen.PeriodoImproductivo.ToString();
                lblRecursosAportados.Text = entResumen.RecursosAportadosEmprendedor.ToString().Trim();
            }

            IndicadorGestionEvaluacion entidadIndicador = Negocio.PlanDeNegocioV2.Utilidad.IndicadorEvaluacion.GetIndicadores(codigoProyecto, codigoConvocatoria);
            if (entidadIndicador != null) {
                txtVentasProductosEvaluador.Text = entidadIndicador.Ventas.ToString("N0");
                                                                            
                txtPeriodoImproductivoEvaluador.Text = entidadIndicador.PeriodoImproductivo.ToString();
                txtRecursosAportadosEvaluador.Text = entidadIndicador.RecursosAportadosEmprendedor;
            }

            var contrapartidas = Negocio.PlanDeNegocioV2.Utilidad.IndicadorFormulacion.GetContrapartidas(codigoProyecto);
            var ejecucionPresupuestal = Negocio.PlanDeNegocioV2.Utilidad.IndicadorFormulacion.GetPresupuesto(codigoProyecto);
            var ventasTotales = FieldValidate.moneyFormat(Negocio.PlanDeNegocioV2.Utilidad.IndicadorFormulacion.GetVentas(codigoProyecto), true);            
            var idh = Negocio.PlanDeNegocioV2.Utilidad.IndicadorFormulacion.GetIDH(codigoProyecto);
            var salarioMinimo = Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetSalarioMinimoConvocatoria(codigoConvocatoria);
            var valorRecomendadoEvaluacion = Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetRecursosAprobados(codigoProyecto, codigoConvocatoria);

            lblContrapartidas.Text = lblContrapartidasEvaluador.Text = contrapartidas.ToString();
            lblEjecucionPresupuestal.Text = ejecucionPresupuestal + " (SMLV) " + "- " + FieldValidate.moneyFormat((ejecucionPresupuestal * salarioMinimo), true);
            lblEjecucionPresupuestalEvaluador.Text = valorRecomendadoEvaluacion + " (SMLV) "+ "- " + FieldValidate.moneyFormat(( valorRecomendadoEvaluacion * (double)salarioMinimo), true);
            lblVentasProductos.Text = ventasTotales;
            lblIdh.Text = idh.ToString();
            lblIdhEvaluador.Text = idh.ToString();
        }
        
        protected void ValidateFields()
        {
            //FieldValidate.ValidateString("Localización", txtLocalizacion.Text, true, 1500);
            //FieldValidate.ValidateString("Sector", txtSector.Text, true, 1500);
            //FieldValidate.ValidateString("Resumen concepto general - compromisos y condiciones", txtResumenConcepto.Text, true, 1500);
        }



        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                FieldValidate.ValidateString("Ventas", txtVentasProductosEvaluador.Text, true,12);
                FieldValidate.ValidateNumeric("Ventas", txtVentasProductosEvaluador.Text, true);
                FieldValidate.ValidateString("Periodo improductivo", txtPeriodoImproductivoEvaluador.Text, true, 2);
                FieldValidate.ValidateNumeric("Periodo improductivo", txtPeriodoImproductivoEvaluador.Text, true);
                FieldValidate.ValidateString("Recursos aportados", txtRecursosAportadosEvaluador.Text.Trim(), true, 12);
                FieldValidate.ValidateNumeric("Recursos aportados", txtRecursosAportadosEvaluador.Text, true);

                int? idProductoRepresentativoEvaluacion = GetProductoRepresentativo();

                if (idProductoRepresentativoEvaluacion == null)
                    throw new ApplicationException("Debe seleccionar el producto mas presentativo.");

                IndicadorGestionEvaluacion entity = new IndicadorGestionEvaluacion()
                {
                    PeriodoImproductivo = Convert.ToInt32(txtPeriodoImproductivoEvaluador.Text),
                    RecursosAportadosEmprendedor = txtRecursosAportadosEvaluador.Text,
                    Ventas = Convert.ToDecimal(txtVentasProductosEvaluador.Text),
                    IdProyecto = CodigoProyecto,
                    IdConvocatoria = CodigoConvocatoria,
                    ProductoMasRepresentativo = idProductoRepresentativoEvaluacion.GetValueOrDefault()
                };
                
                Negocio.PlanDeNegocioV2.Utilidad.IndicadorEvaluacion.InsertOrUpdate(entity);

                var produccionProducto = GetProduccionEvaluador();
                var mercadeo = GetMercadeoEvaluador();
                var cargos = GetCargosEvaluador();

                foreach (var producto in produccionProducto)
                {
                    Negocio.PlanDeNegocioV2.Utilidad.IndicadorEvaluacion.InsertOrUpdateProduccion(producto);
                }

                foreach (var actividad in mercadeo)
                {
                    Negocio.PlanDeNegocioV2.Utilidad.IndicadorEvaluacion.InsertOrUpdateMercadeo(actividad);
                }

                foreach (var cargo in cargos)
                {
                    Negocio.PlanDeNegocioV2.Utilidad.IndicadorEvaluacion.InsertOrUpdateCargo(cargo);
                }

                TabEvaluacionProyecto tabEvaluacion = new TabEvaluacionProyecto()
                {
                    CodProyecto = CodigoProyecto,
                    CodConvocatoria = CodigoConvocatoria,
                    CodTabEvaluacion = (Int16)CodigoTab,
                    CodContacto = Usuario.IdContacto,
                    FechaModificacion = DateTime.Now,
                    Realizado = false
                };

                string messageResult;
                Negocio.PlanDeNegocioV2.Utilidad.TabEvaluacion.SetUltimaActualizacion(tabEvaluacion, out messageResult);
                EncabezadoEval.GetUltimaActualizacion();

                Formulacion.Utilidad.Utilidades.PresentarMsj(messageResult, this, "Alert");

                lblError.Visible = false;
            }
            catch (ApplicationException ex)
            {
                lblError.Visible = true;
                lblError.Text = "Advertencia: " + ex.Message;
                Formulacion.Utilidad.Utilidades.PresentarMsj(ex.Message, this, "Alert");
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Error inesperado: " + ex.Message;
                Formulacion.Utilidad.Utilidades.PresentarMsj("Sucedio un error al guardar.", this, "Alert");
            }
        }

        public List<Negocio.PlanDeNegocioV2.Utilidad.ProduccionDTO> GetProductos(Int32 codigoProyecto)
        {
            var codigoConvocatoria = Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(codigoProyecto, HttpContext.Current.Session["HistorialEvaluacion"] != null ? Convert.ToInt32(HttpContext.Current.Session["HistorialEvaluacion"]) : 0).GetValueOrDefault();

            return Negocio.PlanDeNegocioV2.Utilidad.IndicadorEvaluacion.GetProductos(codigoProyecto, codigoConvocatoria);
        }
        
        public List<Negocio.PlanDeNegocioV2.Utilidad.MercadeoDTO> GetMercadeo(Int32 codigoProyecto)
        {
            var codigoConvocatoria = Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(codigoProyecto, HttpContext.Current.Session["HistorialEvaluacion"] != null ? Convert.ToInt32(HttpContext.Current.Session["HistorialEvaluacion"]) : 0).GetValueOrDefault();

            return Negocio.PlanDeNegocioV2.Utilidad.IndicadorEvaluacion.GetMercadeo(codigoProyecto, codigoConvocatoria);
        }

        public List<Negocio.PlanDeNegocioV2.Utilidad.CargoDTO> GetCargos(Int32 codigoProyecto)
        {
            var codigoConvocatoria = Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(codigoProyecto, HttpContext.Current.Session["HistorialEvaluacion"] != null ? Convert.ToInt32(HttpContext.Current.Session["HistorialEvaluacion"]) : 0).GetValueOrDefault();

            return Negocio.PlanDeNegocioV2.Utilidad.IndicadorEvaluacion.GetCargos(codigoProyecto, codigoConvocatoria);
        }

        public List<Datos.IndicadorProductoEvaluacion> GetProduccionEvaluador()
        {
            var produccionProductos = new List<Datos.IndicadorProductoEvaluacion>();
            foreach (GridViewRow currentRow in gvProductos.Rows)
            {
                HiddenField hdIdProducto = (HiddenField)currentRow.FindControl("hdCodigoProducto");
                Label nombreProducto = (Label)currentRow.FindControl("lblNombreProducto");
                TextBox unidadesEvaluador = (TextBox)currentRow.FindControl("txtProduccionEvaluador");

                FieldValidate.ValidateString("Producción/cantidad del producto " + nombreProducto.Text, unidadesEvaluador.Text, true,11);
                FieldValidate.ValidateNumeric("Producción/cantidad del producto " + nombreProducto.Text, unidadesEvaluador.Text, true);

                string patron = @"[^\w]";
                Regex regex = new Regex(patron);                
                string uniEvaluador = regex.Replace(unidadesEvaluador.Text, "");

                produccionProductos.Add(new IndicadorProductoEvaluacion
                {
                    IdConvocatoria = CodigoConvocatoria,
                    IdProducto = Convert.ToInt32(hdIdProducto.Value),
                    Unidades = Convert.ToInt32(uniEvaluador),
                    IdProyecto = CodigoProyecto
                });
            }

            return produccionProductos;
        }

        public List<Datos.IndicadorMercadeoEvaluacion> GetMercadeoEvaluador()
        {
            var mercadeo = new List<Datos.IndicadorMercadeoEvaluacion>();
            foreach (GridViewRow currentRow in gvMercadeo.Rows)
            {
                HiddenField hdId = (HiddenField)currentRow.FindControl("hdCodigo");
                Label nombre = (Label)currentRow.FindControl("lblNombre");
                TextBox unidadesEvaluador = (TextBox)currentRow.FindControl("txtUnidadesEvaluador");

                FieldValidate.ValidateString("Cantidad de la actividad " + nombre.Text, unidadesEvaluador.Text, true,3);
                FieldValidate.ValidateNumeric("Cantidad de la actividad " + nombre.Text, unidadesEvaluador.Text, true);

                mercadeo.Add(new IndicadorMercadeoEvaluacion
                {
                    IdConvocatoria = CodigoConvocatoria,
                    IdActividadMercadeo = Convert.ToInt32(hdId.Value),
                    Unidades = Convert.ToInt32(unidadesEvaluador.Text),
                    IdProyecto = CodigoProyecto
                });
            }

            return mercadeo;
        }

        public List<Datos.IndicadorCargoEvaluacion> GetCargosEvaluador()
        {
            var mercadeo = new List<Datos.IndicadorCargoEvaluacion>();
            foreach (GridViewRow currentRow in gvCargos.Rows)
            {
                HiddenField hdId = (HiddenField)currentRow.FindControl("hdCodigo");
                Label nombre = (Label)currentRow.FindControl("lblNombre");
                TextBox unidadesEvaluador = (TextBox)currentRow.FindControl("txtUnidadesEvaluador");

                FieldValidate.ValidateString("Cantidad del cargo " + nombre.Text, unidadesEvaluador.Text, true,3);
                FieldValidate.ValidateNumeric("Cantidad del cargo " + nombre.Text, unidadesEvaluador.Text, true);

                mercadeo.Add(new IndicadorCargoEvaluacion
                {
                    IdConvocatoria = CodigoConvocatoria,
                    IdCargo = Convert.ToInt32(hdId.Value),
                    Unidades = Convert.ToInt32(unidadesEvaluador.Text),
                    IdProyecto = CodigoProyecto
                });
            }

            return mercadeo;
        }

        public int? GetProductoRepresentativo()
        {
            int? idProducto = null;
            foreach (GridViewRow currentRow in gvProductos.Rows)
            {
                CheckBox rdbtnProducto = (RadioButton)currentRow.FindControl("rdProductoSeleccionadoEvaluacion");
                HiddenField hdIdProducto = (HiddenField)currentRow.FindControl("hdCodigoProducto");

                if (rdbtnProducto.Checked)
                {
                    idProducto = Convert.ToInt32(hdIdProducto.Value);
                }
            }

            return idProducto;
        }
    }
}