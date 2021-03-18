using Datos.Modelos;
using Fonade.Negocio.FonDBLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.ObligTipicas
{
    public partial class ObligacionesTipicas : System.Web.UI.Page
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
                cargarGrids(CodigoProyecto, CodigoConvocatoria);
                CargarDrops();
            }
        }

        ActaSeguimObligContablesController obligContablesController = new ActaSeguimObligContablesController();
        ddlCumplimientoController ddlCumplimientoController = new ddlCumplimientoController();

        public void CargarDrops()
        {
            //Obligacion Contable
            cargarDDL(ddlConciliacionesBancarias);
            cargarDDL(ddlCuentaBancaria);
            cargarDDL(ddlEstadoFinanciero);
            cargarDDL(ddlLibrosComerciales);
            cargarDDL(ddlLibrosContabilidad);

            //Obligacion Tributaria
            cargarDDL(ddlDeclaReteFuente);
            cargarDDL(ddlAutoRenta);
            cargarDDL(ddlDeclaIVA);
            cargarDDL(ddlDeclaImpConsumo);
            cargarDDL(ddlDeclaRenta);
            cargarDDL(ddlDeclaExogena);
            cargarDDL(ddlDeclaIndustria);
            cargarDDL(ddlDeclaReteImpIndustria);

            //Obligacion Laboral
            cargarDDL(ddlContratoLaboral);
            cargarDDL(ddlPagoNomina);
            cargarDDL(ddlPagoPrestaciones);
            cargarDDL(ddlAfilSegSocial);
            cargarDDL(ddlPagoSegSocial);
            cargarDDL(ddlCertParafiscal);
            cargarDDL(ddlRegIntTrabajo);
            cargarDDL(ddlGestSegSalud);

            //Obligacion Tramites
            cargarDDL(ddlOtrosPermisos);
            cargarDDL(ddlContratoArrendamiento);
            cargarDDL(ddlRegMarca);
            cargarDDL(ddlCertBomberos);
            cargarDDL(ddlPermisoSuelo);
            cargarDDL(ddlAvalEmprendimiento);
            cargarDDL(ddlCertLibertad);
            cargarDDL(ddlResFacturacion);
            cargarDDL(ddlRUT);
            cargarDDL(ddlRenMercantil);
            cargarDDL(ddlInsCamara);
        }

        public void cargarDDL(DropDownList ddl)
        {
            var opciones = ddlCumplimientoController.ddlDatosCumplimiento();

            ddl.DataSource = opciones;
            ddl.DataTextField = "valor"; // FieldName of Table in DataBase
            ddl.DataValueField = "id";
            ddl.DataBind();
        }
        public void cargarGrids(int _codProyecto, int _codConvocatoria)
        {
            lblnumV.Text = (NumeroActa).ToString();
            lblNumVL.Text = (NumeroActa).ToString();
            lblNumVT.Text = (NumeroActa).ToString();
            lblNumVR.Text = (NumeroActa).ToString();

            CargarGridObligoContable(_codProyecto, _codConvocatoria);
            CargarGridObligTributaria(_codProyecto, _codConvocatoria);
            CargarGridObligLaboral(_codProyecto, _codConvocatoria);
            CargarGridObligTramite(_codProyecto, _codConvocatoria);
        }
        public void CargarGridObligTramite(int _codProyecto, int _codConvocatoria)
        {
            var obligacion = obligContablesController.GetObligTramite(_codProyecto, _codConvocatoria);

            gvRegistrosTramitesLicencias.DataSource = obligacion;
            gvRegistrosTramitesLicencias.DataBind();
        }
        public void CargarGridObligLaboral(int _codProyecto, int _codConvocatoria)
        {
            var obligacion = obligContablesController.GetObligLaboral(_codProyecto, _codConvocatoria);

            gvObligacionLaboral.DataSource = obligacion;
            gvObligacionLaboral.DataBind();
        }
        public void CargarGridObligTributaria(int _codProyecto, int _codConvocatoria)
        {
            var obligacion = obligContablesController.GetObligTributaria(_codProyecto, _codConvocatoria);

            gvObligacionTributaria.DataSource = obligacion;
            gvObligacionTributaria.DataBind();
        }
        public void CargarGridObligoContable(int _codProyecto, int _codConvocatoria)
        {
            var obligacion = obligContablesController.GetObligContable(_codProyecto, _codConvocatoria);

            gvObligacionContable.DataSource = obligacion;
            gvObligacionContable.DataBind();
        }

        protected void btnGuardarOblContable_Click(object sender, EventArgs e)
        {
            string mensaje = "";
            if (!validarCamposContable(ref mensaje))
            {
                Alert(mensaje);
            }
            else
            {
                if (GuardarObligContable(CodigoProyecto,CodigoConvocatoria,NumeroActa))
                {
                    Alert("Guardado Obligacion Contable Correctamente.");
                    CargarGridObligoContable(CodigoProyecto, CodigoConvocatoria);
                }             
            }
        }

        private bool GuardarObligContable(int _codProyecto, int _codConvocatoria, int _numActa)
        {
            bool guardado = false;

            ActaSeguimObligContablesModel actaContable = new ActaSeguimObligContablesModel()
            {
                codConvocatoria = _codConvocatoria,
                codProyecto = _codProyecto,
                numActa = _numActa,
                conciliacionBancaria = ddlConciliacionesBancarias.SelectedItem.Text,
                cuentaBancaria = ddlCuentaBancaria.SelectedItem.Text,
                estadosFinancieros = ddlEstadoFinanciero.SelectedItem.Text,
                librosComerciales = ddlLibrosComerciales.SelectedItem.Text,
                librosContabilidad = ddlLibrosContabilidad.SelectedItem.Text,
                observObligacionContable = txtObservacionContable.Text,
                visita = (_numActa)            
            };

            guardado = obligContablesController.InsertOrUpdateObligContable(actaContable);

            return guardado;
        }

        private bool validarCamposContable(ref string mensaje)
        {
            bool validar = true;
            mensaje = "Seleccione un valor para: ";
            if(ddlEstadoFinanciero.SelectedIndex == 0)
            {
                validar = false;
                mensaje = mensaje + "- Estado Financiero; ";
            }
            if (ddlLibrosComerciales.SelectedIndex == 0)
            {
                validar = false;
                mensaje = mensaje + "- Libros Comerciales; ";
            }
            if (ddlLibrosContabilidad.SelectedIndex == 0)
            {
                validar = false;
                mensaje = mensaje + "- Libros de Contabilidad; ";
            }
            if (ddlConciliacionesBancarias.SelectedIndex == 0)
            {
                validar = false;
                mensaje = mensaje + "- Conciliaciones Bancarias; ";
            }
            if (ddlCuentaBancaria.SelectedIndex == 0)
            {
                validar = false;
                mensaje = mensaje + "- Cuenta Bancaria; ";
            }
            if (txtObservacionContable.Text == "")
            {
                validar = false;
                mensaje = mensaje + "- Observacion; ";
            }

            return validar;
        }

        private void Alert(string mensaje)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
        }

        protected void btnGuardarOblTributaria_Click(object sender, EventArgs e)
        {
            string mensaje = "";
            if (!validarCamposTributaria(ref mensaje))
            {
                Alert(mensaje);
            }
            else
            {
                if (GuardarObligTributaria(CodigoProyecto, CodigoConvocatoria, NumeroActa))
                {
                    Alert("Guardado Obligacion Tributaria Correctamente.");
                    CargarGridObligTributaria(CodigoProyecto, CodigoConvocatoria);
                }
            }
        }

        private bool validarCamposTributaria(ref string mensaje)
        {
            bool validar = true;
            mensaje = "Seleccione un valor para: ";
            if (ddlDeclaReteFuente.SelectedIndex == 0)
            {
                validar = false;
                mensaje = mensaje + "- Declaraciones Retención en la Fuente; ";
            }
            if (ddlAutoRenta.SelectedIndex == 0)
            {
                validar = false;
                mensaje = mensaje + "- Autorretención Renta; ";
            }
            if (ddlDeclaIVA.SelectedIndex == 0)
            {
                validar = false;
                mensaje = mensaje + "- Decla. de IVA; ";
            }
            if (ddlDeclaImpConsumo.SelectedIndex == 0)
            {
                validar = false;
                mensaje = mensaje + "- Decla. de Impuesto al Consumo; ";
            }
            if (ddlDeclaRenta.SelectedIndex == 0)
            {
                validar = false;
                mensaje = mensaje + "- Decla. de Renta; ";
            }
            if (ddlDeclaExogena.SelectedIndex == 0)
            {
                validar = false;
                mensaje = mensaje + "- Decla. Info Exogena; ";
            }
            if (ddlDeclaIndustria.SelectedIndex == 0)
            {
                validar = false;
                mensaje = mensaje + "- Decla. Industria y C/cio; ";
            }
            if (ddlDeclaReteImpIndustria.SelectedIndex == 0)
            {
                validar = false;
                mensaje = mensaje + "- Decla. Retención Impuesto Industria y C/cio; ";
            }
            if (txtObservTributaria.Text == "")
            {
                validar = false;
                mensaje = mensaje + "- Observacion; ";
            }

            return validar;
        }

        private bool GuardarObligTributaria(int _codProyecto, int _codConvocatoria, int _numActa)
        {
            bool guardado = false;

            ActaSeguimObligTributariasModel actaTributaria = new ActaSeguimObligTributariasModel()
            {
                codConvocatoria = _codConvocatoria,
                codProyecto = _codProyecto,
                numActa = _numActa,                
                visita = (_numActa),
                autorretencionRenta = ddlAutoRenta.SelectedItem.Text,
                declaImpConsumo = ddlDeclaImpConsumo.SelectedItem.Text,
                declaIndustriaComercio = ddlDeclaIndustria.SelectedItem.Text,
                declaInfoExogena = ddlDeclaExogena.SelectedItem.Text,
                declaraIva = ddlDeclaIVA.SelectedItem.Text,
                declaraReteFuente = ddlDeclaReteFuente.SelectedItem.Text,
                declaRenta = ddlDeclaRenta.SelectedItem.Text,
                declaRetencionImpIndusComercio = ddlDeclaReteImpIndustria.SelectedItem.Text,
                observObligacionTributaria = txtObservTributaria.Text
            };

            guardado = obligContablesController.InsertOrUpdateObligTributaria(actaTributaria);

            return guardado;
        }

        protected void btnGuardarOblLaboral_Click(object sender, EventArgs e)
        {
            string mensaje = "";
            if (!validarCamposLaboral(ref mensaje))
            {
                Alert(mensaje);
            }
            else
            {
                if (GuardarObligLaboral(CodigoProyecto, CodigoConvocatoria, NumeroActa))
                {
                    Alert("Guardado Obligacion Laboral Correctamente.");
                    CargarGridObligLaboral(CodigoProyecto, CodigoConvocatoria);
                }
            }
        }

        private bool validarCamposLaboral(ref string mensaje)
        {
            bool validar = true;
            mensaje = "Seleccione un valor para: ";
            if (ddlContratoLaboral.SelectedIndex == 0)
            {
                validar = false;
                mensaje = mensaje + "- Contratos Laborales; ";
            }
            if (ddlPagoNomina.SelectedIndex == 0)
            {
                validar = false;
                mensaje = mensaje + "- Pagos de Nomina; ";
            }
            if (ddlPagoPrestaciones.SelectedIndex == 0)
            {
                validar = false;
                mensaje = mensaje + "- Pago de Prestaciones sociales; ";
            }
            if (ddlAfilSegSocial.SelectedIndex == 0)
            {
                validar = false;
                mensaje = mensaje + "- Afiliacion Seguridad Social; ";
            }
            if (ddlPagoSegSocial.SelectedIndex == 0)
            {
                validar = false;
                mensaje = mensaje + "- Pagos Seguridad Social; ";
            }
            if (ddlCertParafiscal.SelectedIndex == 0)
            {
                validar = false;
                mensaje = mensaje + "- Certificado Paz y Salvo de Parafiscales y Seg. Social; ";
            }
            if (ddlRegIntTrabajo.SelectedIndex == 0)
            {
                validar = false;
                mensaje = mensaje + "- Reglamento Interno de Trabajo; ";
            }
            if (ddlGestSegSalud.SelectedIndex == 0)
            {
                validar = false;
                mensaje = mensaje + "- Sis. Gest. de Seg. y Salud en el Trabajo; ";
            }           
            if (txtObservacionLaboral.Text == "")
            {
                validar = false;
                mensaje = mensaje + "- Observacion; ";
            }

            return validar;
        }

        private bool GuardarObligLaboral(int _codProyecto, int _codConvocatoria, int _numActa)
        {
            bool guardado = false;

            ActaSeguimObligLaboralModel actaLaboral = new ActaSeguimObligLaboralModel()
            {
                codConvocatoria = _codConvocatoria,
                codProyecto = _codProyecto,
                numActa = _numActa,
                visita = (_numActa),
                afiliacionSegSocial = ddlAfilSegSocial.SelectedItem.Text,
                certParafiscalesSegSocial = ddlCertParafiscal.SelectedItem.Text,
                contratosLaborales = ddlContratoLaboral.SelectedItem.Text,
                observObligacionLaboral = txtObservacionLaboral.Text,
                pagoSegSocial = ddlPagoSegSocial.SelectedItem.Text,
                pagosNomina = ddlPagoNomina.SelectedItem.Text,
                pagoPrestacionesSociales = ddlPagoPrestaciones.SelectedItem.Text,
                reglaInternoTrab = ddlRegIntTrabajo.SelectedItem.Text,
                sisGestionSegSaludTrabajo = ddlGestSegSalud.SelectedItem.Text                
            };

            guardado = obligContablesController.InsertOrUpdateObligLaboral(actaLaboral);

            return guardado;
        }

        protected void btnGuardarOblTramites_Click(object sender, EventArgs e)
        {
            string mensaje = "";
            if (!validarCamposTramites(ref mensaje))
            {
                Alert(mensaje);
            }
            else
            {
                if (GuardarObligTramites(CodigoProyecto, CodigoConvocatoria, NumeroActa))
                {
                    Alert("Guardado Registros, Trámites y Licencias Correctamente.");
                    CargarGridObligTramite(CodigoProyecto, CodigoConvocatoria);
                }
            }
        }

        private bool validarCamposTramites(ref string mensaje)
        {
            bool validar = true;
            mensaje = "Seleccione un valor para: ";
            if (ddlInsCamara.SelectedIndex == 0)
            {
                validar = false;
                mensaje = mensaje + "- Inscripción Cámara de C/cio; ";
            }
            if (ddlRenMercantil.SelectedIndex == 0)
            {
                validar = false;
                mensaje = mensaje + "- Renovación de Registro Mercantil; ";
            }
            if (ddlRUT.SelectedIndex == 0)
            {
                validar = false;
                mensaje = mensaje + "- RUT; ";
            }
            if (ddlResFacturacion.SelectedIndex == 0)
            {
                validar = false;
                mensaje = mensaje + "- Resolución Facturación; ";
            }
            if (ddlCertLibertad.SelectedIndex == 0)
            {
                validar = false;
                mensaje = mensaje + "- Certificado de Libertad y Tradición; ";
            }
            if (ddlAvalEmprendimiento.SelectedIndex == 0)
            {
                validar = false;
                mensaje = mensaje + "- Aval Unidad de Emprendi. del Predio; ";
            }
            if (ddlPermisoSuelo.SelectedIndex == 0)
            {
                validar = false;
                mensaje = mensaje + "- Permiso Uso de Suelo; ";
            }
            if (ddlCertBomberos.SelectedIndex == 0)
            {
                validar = false;
                mensaje = mensaje + "- Certificado de Bomberos; ";
            }
            if (ddlRegMarca.SelectedIndex == 0)
            {
                validar = false;
                mensaje = mensaje + "- Reg. de Marca; ";
            }
            if (ddlOtrosPermisos.SelectedIndex == 0)
            {
                validar = false;
                mensaje = mensaje + "- Otros Permisos; ";
            }
            if (ddlContratoArrendamiento.SelectedIndex == 0)
            {
                validar = false;
                mensaje = mensaje + "- Contrato Arrendamiento; ";
            }
            if (txtObservTramites.Text == "")
            {
                validar = false;
                mensaje = mensaje + "- Observacion; ";
            }

            return validar;
        }

        private bool GuardarObligTramites(int _codProyecto, int _codConvocatoria, int _numActa)
        {
            bool guardado = false;

            ActaSeguimObligTramitesModel actaTramite = new ActaSeguimObligTramitesModel()
            {
                codConvocatoria = _codConvocatoria,
                codProyecto = _codProyecto,
                numActa = _numActa,
                visita = (_numActa),
                DocumentoIdoneidad = ddlAvalEmprendimiento.SelectedItem.Text,
                certBomberos = ddlCertBomberos.SelectedItem.Text,
                certLibertadTradicion = ddlCertLibertad.SelectedItem.Text,
                insCamaraComercio = ddlInsCamara.SelectedItem.Text,
                observRegistroTramiteLicencia = txtObservTramites.Text,
                otrosPermisos = ddlOtrosPermisos.SelectedItem.Text,
                permisoUsoSuelo = ddlPermisoSuelo.SelectedItem.Text,
                regMarca = ddlRegMarca.SelectedItem.Text,
                renovaRegistroMercantil = ddlRenMercantil.SelectedItem.Text,
                resolFacturacion = ddlResFacturacion.SelectedItem.Text,
                rut = ddlRUT.SelectedItem.Text,
                contratoArrendamiento = ddlContratoArrendamiento.SelectedItem.Text
            };

            guardado = obligContablesController.InsertOrUpdateObligTramite(actaTramite);

            return guardado;
        }

        protected void gvObligacionContable_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvObligacionContable.PageIndex = e.NewPageIndex;
            var Compromisos = obligContablesController.GetObligContable(CodigoProyecto, CodigoConvocatoria);
            gvObligacionContable.DataSource = Compromisos;
            gvObligacionContable.DataBind();
        }

        protected void gvObligacionTributaria_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvObligacionTributaria.PageIndex = e.NewPageIndex;
            var Compromisos = obligContablesController.GetObligTributaria(CodigoProyecto, CodigoConvocatoria);
            gvObligacionTributaria.DataSource = Compromisos;
            gvObligacionTributaria.DataBind();
        }

        protected void gvObligacionLaboral_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvObligacionLaboral.PageIndex = e.NewPageIndex;
            var Compromisos = obligContablesController.GetObligLaboral(CodigoProyecto, CodigoConvocatoria);
            gvObligacionLaboral.DataSource = Compromisos;
            gvObligacionLaboral.DataBind();
        }

        protected void gvRegistrosTramitesLicencias_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRegistrosTramitesLicencias.PageIndex = e.NewPageIndex;
            var Compromisos = obligContablesController.GetObligTramite(CodigoProyecto, CodigoConvocatoria);
            gvRegistrosTramitesLicencias.DataSource = Compromisos;
            gvRegistrosTramitesLicencias.DataBind();
        }
    }
}