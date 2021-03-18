using Datos;
using Datos.Modelos;
using Fonade.Negocio.FonDBLight;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.IndicadoresGestion
{
    public partial class EjecucionPresupuestal : System.Web.UI.Page
    {
        public int CodigoProyecto
        {
            get { return Convert.ToInt32(Session["idProyecto"]); }
            set { }
        }

        public int NumeroActa
        {
            get { return Convert.ToInt32(Session["idActa"]); }
            set { }
        }

        public int CodigoConvocatoria
        {
            get
            {
                return Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(CodigoProyecto, HttpContext.Current.Session["HistorialEvaluacion"] != null ? Convert.ToInt32(HttpContext.Current.Session["HistorialEvaluacion"]) : 0).GetValueOrDefault();
            }
            set { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    int visita = NumeroActa;

                    //Gestión en Ejecución Presupuestal
                    lblNumVisita.Text = (NumeroActa).ToString();
                    cargarDatos(CodigoProyecto, CodigoConvocatoria);
                    cargarGridsInfoPago(CodigoProyecto, CodigoConvocatoria);

                    //Inventarios y Contrato de Garantía
                    lblNumVisitaInventario.Text = visita.ToString();
                    calendarFechaCarga.EndDate = DateTime.Now;
                    cargarGridsInventario(CodigoProyecto, CodigoConvocatoria);


                    //Aportes Emprendedor
                    lblNumVisitaAport.Text = visita.ToString();
                    //buscar el aporte del emprendedor
                    txtMetaAportesEmp.Text = metaAporteEmprendedor(visita, CodigoProyecto);
                    //txtMetaAportesEmp.Text = infoPagosController.AporteEmpRecomendado(CodigoProyecto, CodigoConvocatoria);
                    cargarGridAporteEmp(CodigoProyecto, CodigoConvocatoria);

                    mostrarPanels(visita);
                }
                catch (Exception ex)
                {
                    Alert(ex.Message);
                }
               
            }
        }        

        private string metaAporteEmprendedor(int _visita, int _codProyecto)
        {
            string meta = "";
            int visitaAnterior = 0;
            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                var aporteEmprendedor = (from a in db.ActaSeguimAporteEmprendedor
                                         where a.visita == _visita && a.codProyecto == _codProyecto
                                         select a).FirstOrDefault();

                if(aporteEmprendedor == null || aporteEmprendedor.metaEmprendedor == null)
                {                                        
                    visitaAnterior = _visita - 1;
                    if(visitaAnterior == 0)
                    {
                        meta = infoPagosController.AporteEmpRecomendado(CodigoProyecto, CodigoConvocatoria);
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

        private void mostrarPanels(int _numVisita)
        {
            if (_numVisita > 1) //Si no es la primera visita
            {
                //Gestión en Ejecución Presupuestal
                pnlInfoEjePresupuestal.Visible = false;
                pnlGestionEjePresupuestal.Visible = true;

                //Inventarios y Contrato de Garantía
                pnlInfoInventario.Visible = false;
                pnlGestionInventario.Visible = true;
            }
            else
            {
                //Gestión en Ejecución Presupuestal
                pnlInfoEjePresupuestal.Visible = true;
                pnlGestionEjePresupuestal.Visible = false;

                //Inventarios y Contrato de Garantía
                pnlInfoInventario.Visible = true;
                pnlGestionInventario.Visible = false;
            }
        }

        private void cargarGridsInfoPago(int _codProyecto, int _codConvocatoria)
        {
            decimal SumaDesVisita = 0;
            decimal SumaDesVisitaAnteriores = 0;
            decimal SumaDesembolsoTot = 0;

            cargarGridInfoPago(_codProyecto, _codConvocatoria, ref SumaDesVisita, ref SumaDesVisitaAnteriores);

            SumaDesembolsoTot = SumaDesVisita + SumaDesVisitaAnteriores;

            lblDesembolsado.Text = SumaDesembolsoTot.ToString("C");
            lblDesVisita.Text = SumaDesVisita.ToString("C");


        }
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        private void cargarDatos(int _codProyecto, int _codConvocatoria)
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

                var qSalario = (from S in db.SalariosMinimos
                                where S.AñoSalario == Ano
                                orderby S.Id_SalariosMinimos descending
                                select S.SalarioMinimo).FirstOrDefault();

                lblValorPesos.Text = (Convert.ToInt64(lblSMLV.Text) * qSalario).ToString("C");

            }
        }
        ActaSeguimInfoPagosController infoPagosController = new ActaSeguimInfoPagosController();
        private void cargarGridInfoPago(int _codProyecto, int _codConvocatoria, ref decimal suma, ref decimal sumPagoAnt)
        {
            var infoPago = infoPagosController.getInfoPagos(_codProyecto, _codConvocatoria, ref sumPagoAnt);

            suma = infoPago.Sum(x => x.Valor);

            gvInfoPago.DataSource = infoPago;
            gvInfoPago.DataBind();

            var histEjecucion = infoPagosController.getHistoricoEjecucion(_codProyecto, _codConvocatoria);

            gvHistorialEjecucionPresupuesto.DataSource = histEjecucion;
            gvHistorialEjecucionPresupuesto.DataBind();
        }

        ddlCumplimientoController ddlController = new ddlCumplimientoController();
        protected void gvInfoPago_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddldocs = (DropDownList)e.Row.FindControl("ddlDocumentosOrig");
                ddldocs.DataSource = ddlController.ddlSeleccionSINO();
                ddldocs.DataTextField = "valor"; // FieldName of Table in DataBase
                ddldocs.DataValueField = "id";
                ddldocs.DataBind();

                DropDownList ddlEstActivo = (DropDownList)e.Row.FindControl("ddlActivosEstado");
                ddlEstActivo.DataSource = ddlController.ddlSelEstadoActivo();
                ddlEstActivo.DataTextField = "valor"; // FieldName of Table in DataBase
                ddlEstActivo.DataValueField = "id";
                ddlEstActivo.DataBind();
            }
        }

        protected void btnGuardarInfoPagos_Click(object sender, EventArgs e)
        {
            if (AgregarInfoPago(CodigoProyecto, CodigoConvocatoria))
            {
                cargarGridsInfoPago(CodigoProyecto, CodigoConvocatoria);
            }
        }

        private bool AgregarInfoPago(int _codProyecto, int _codConvocatoria)
        {
            bool insertado = false;
            try
            {
                foreach (GridViewRow row in gvInfoPago.Rows)
                {
                    bool validadoText = true;
                    bool validadoDocs = true;
                    bool validadoEstAct = true;

                    string mensaje = "";
                    DropDownList ddlDocumentosOrig = (DropDownList)row.FindControl("ddlDocumentosOrig");
                    DropDownList ddlActivosEstado = (DropDownList)row.FindControl("ddlActivosEstado");

                    TextBox txtObservacion = (TextBox)row.FindControl("txtObservInfoPago");
                    Label lblValDocumentos = (Label)row.FindControl("lblValDocumentos");
                    Label lblValEstActivo = (Label)row.FindControl("lblValEstActivo");
                    Label lblValObservacion = (Label)row.FindControl("lblValObservacion");

                    validadoText = validarCampos(txtObservacion, ref mensaje);
                    validadoDocs = validarCampos(ddlDocumentosOrig, ref mensaje);
                    validadoEstAct = validarCampos(ddlActivosEstado, ref mensaje);

                    if (!validadoDocs || !validadoText || !validadoEstAct)
                    {
                        lblValDocumentos.Text = mensaje;
                        lblValDocumentos.Visible = !validadoDocs;

                        lblValEstActivo.Text = mensaje;
                        lblValEstActivo.Visible = !validadoEstAct;

                        lblValObservacion.Text = mensaje;
                        lblValObservacion.Visible = !validadoText;
                    }
                    else
                    {
                        ActaSeguimInfoPagosModel pagosModel = new ActaSeguimInfoPagosModel();
                        pagosModel.idPagoActividad = Convert.ToInt32(gvInfoPago.DataKeys[row.RowIndex].Value);
                        pagosModel.codigoPago = row.Cells[0].Text; //codigoPago    
                        pagosModel.Actividad = row.Cells[1].Text; //Actividad    
                        pagosModel.Valor = Convert.ToDecimal(row.Cells[2].Text.Replace("$", "")); //Valor    
                        pagosModel.Concepto = row.Cells[3].Text; //Concepto   
                        pagosModel.codConvocatoria = CodigoConvocatoria;
                        pagosModel.codProyecto = CodigoProyecto;
                        pagosModel.numActa = NumeroActa;
                        pagosModel.Observacion = txtObservacion.Text;
                        pagosModel.verificoActivosEstado = ddlActivosEstado.SelectedItem.Text;
                        pagosModel.verificoDocumentos = ddlDocumentosOrig.SelectedItem.Text;
                        pagosModel.visita = NumeroActa - 1;

                        insertado = infoPagosController.insertPagoInfo(pagosModel);
                    }
                }
            }
            catch (Exception)
            {
                insertado = false;
            }
            return insertado;
        }

        private bool validarCampos(DropDownList ddl, ref string mensaje)
        {
            bool validado = true;
            string nombreCampo = "";

            if (ddl.ID == "ddlDocumentosOrig")
            {
                nombreCampo = ""; //¿Verificó documentos originales?
            }
            if (ddl.ID == "ddlActivosEstado")
            {
                nombreCampo = ""; //¿Verficó físicamente Activos y su estado?
            }

            if (ddl.SelectedIndex == 0)
            {
                validado = false;
                mensaje = "*Campo obligatorio"; //Debe seleccionar un valor para: 
                mensaje += nombreCampo;
            }

            return validado;
        }
        private bool validarCampos(TextBox txt, ref string mensaje)
        {
            bool validado = true;

            if (txt.Text == "")
            {
                validado = false;
                mensaje = "*Campo Obligatorio"; //
            }
            if (txt.Text.Length > 1000)
            {
                validado = false;
                mensaje = "la observacion no debe tener mas de 1000 caracteres";
            }

            return validado;
        }

        private void cargarGridsInventario(int _codProyecto, int _codConvocatoria)
        {
            var inventario = infoPagosController.getInventarioContrato(_codProyecto, _codConvocatoria);

            decimal suma = inventario.Sum(x => x.valorActivos);
            lblActivosPrendables.Text = "TOTAL VALOR ACTIVOS PRENDABLES "+suma.ToString("C");

            gvInventario.DataSource = inventario;
            gvInventario.DataBind();
        }

        private void cargarGridAporteEmp(int _codProyecto, int _codConvocatoria)
        {
            var aportes = infoPagosController.getAporteEmp(_codProyecto, _codConvocatoria);
            
            gvAportesEmp.DataSource = aportes;
            gvAportesEmp.DataBind();
        }
        private void Alert(string mensaje)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
        }

        protected void btnGuardarInventario_Click(object sender, EventArgs e)
        {
            if (validarCampos())
                if (guardarInventario(CodigoProyecto, CodigoConvocatoria, NumeroActa))
                {
                    cargarGridsInventario(CodigoProyecto, CodigoConvocatoria);
                    Alert("Guardado Corrrectamente");
                }
                else
                {
                    Alert("No Se Guardó La Información");
                }

        }

        private bool validarCampos()
        {
            bool valido = true;

            if (txtDescripcionRecursos.Text == "")
            {
                lblValDescripcionRecursos.Text = "*Campo Obligatorio";
                lblValDescripcionRecursos.Visible = true;
                valido = false;
            }
            if (txtDescripcionRecursos.Text.Length > 10000)
            {
                lblValDescripcionRecursos.Text = "*El campo no puede superar los 10000 caracteres.";
                lblValDescripcionRecursos.Visible = true;
                valido = false;
            }
            if (txtCantidad.Text == "")
            {
                lblValCantidad.Text = "*Campo Obligatorio";
                lblValCantidad.Visible = true;
                valido = false;
            }
            if (txtFechaCarga.Text == "")
            {
                lblValFechaCarga.Text = "*Campo Obligatorio";
                lblValFechaCarga.Visible = true;
                valido = false;
            }

            if (txtFechaCarga.Text != "")
                if (Convert.ToDateTime(txtFechaCarga.Text) > DateTime.Now.Date)
                {
                    lblValFechaCarga.Text = "*La fecha no puede ser mayor a la actual";
                    lblValFechaCarga.Visible = true;
                    valido = false;
                }

            return valido;
        }

        private bool guardarInventario(int _codProyecto, int _codConvocatoria, int _numActa)
        {
            bool insertado = false;
            DateTime dtFechaCarga = Convert.ToDateTime(txtFechaCarga.Text);

            decimal number;
            Decimal.TryParse(txtCantidad.Text, out number);

            if (number >= 0)
            {
                ActaSeguimInventarioContratoModel model = new ActaSeguimInventarioContratoModel()
                {
                    codConvocatoria = _codConvocatoria,
                    codProyecto = _codProyecto,
                    descripcionRecursos = txtDescripcionRecursos.Text,
                    fechaCargaAnexo = dtFechaCarga,
                    numActa = _numActa,
                    valorActivos = number,
                    visita = (_numActa)
                };

                insertado = infoPagosController.InsertOrUpdateInventario(model);
            }
            else
            {
                Alert("El valor a ingresar a cantidad debe ser mayor o igual que 0");
            }
            

            return insertado;
        }

        protected void btnGuardarAportEmp_Click(object sender, EventArgs e)
        {
            if (validarCamposAportes())
                if (guardarAportEmp(CodigoProyecto, CodigoConvocatoria, NumeroActa))
                {
                    cargarGridAporteEmp(CodigoProyecto, CodigoConvocatoria);
                    Alert("Guardado Corrrectamente");
                }
                else
                {
                    Alert("No Se Guardó La Información");
                }
        }

        private bool validarCamposAportes()
        {
            bool valido = true;

            if (txtDescAporteEmp.Text == "")
            {
                lblValDescripcionAporte.Text = "*Campo Obligatorio";
                lblValDescripcionAporte.Visible = true;
                valido = false;
            }
            if (txtDescAporteEmp.Text.Length > 10000)
            {
                lblValDescripcionAporte.Text = "*El campo no puede superar los 10000 caracteres.";
                lblValDescripcionAporte.Visible = true;
                valido = false;
            }
            if (txtMetaAportesEmp.Text.Length > 10000 || txtMetaAportesEmp.Text == "")
            {
                lblValDescripcionAporte.Text = "*El campo Meta Aportes Emprendedor no debe estar vacío y no debe superar los 10000 caracteres.";
                lblValDescripcionAporte.Visible = true;
                valido = false;
            }

            return valido;
        }

        private bool guardarAportEmp(int _codProyecto, int _codConvocatoria, int _numActa)
        {
            bool insertado = false;
           
            ActaSeguimAporteEmprendedorModel model = new ActaSeguimAporteEmprendedorModel()
            {
                codConvocatoria = _codConvocatoria,
                codProyecto = _codProyecto,                
                numActa = _numActa,               
                visita = (_numActa),
                descripcion = txtDescAporteEmp.Text,
                metaEmprendedor = txtMetaAportesEmp.Text
            };

            insertado = infoPagosController.InsertOrUpdateAporteEmp(model);

            return insertado;
        }

        protected void gvInfoPago_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            decimal sumPagoAnt = 0;
            gvInfoPago.PageIndex = e.NewPageIndex;
            var Compromisos = infoPagosController.getInfoPagos(CodigoProyecto, CodigoConvocatoria, ref sumPagoAnt);
            gvInfoPago.DataSource = Compromisos;
            gvInfoPago.DataBind();
        }

        protected void gvHistorialEjecucionPresupuesto_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvHistorialEjecucionPresupuesto.PageIndex = e.NewPageIndex;
            var Compromisos = infoPagosController.getHistoricoEjecucion(CodigoProyecto, CodigoConvocatoria);
            gvHistorialEjecucionPresupuesto.DataSource = Compromisos;
            gvHistorialEjecucionPresupuesto.DataBind();
        }

        protected void gvInventario_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvInventario.PageIndex = e.NewPageIndex;
            var Compromisos = infoPagosController.getInventarioContrato(CodigoProyecto, CodigoConvocatoria);
            gvInventario.DataSource = Compromisos;
            gvInventario.DataBind();
        }

        protected void gvAportesEmp_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAportesEmp.PageIndex = e.NewPageIndex;
            var Compromisos = infoPagosController.getAporteEmp(CodigoProyecto, CodigoConvocatoria);
            gvAportesEmp.DataSource = Compromisos;
            gvAportesEmp.DataBind();
        }
    }
}