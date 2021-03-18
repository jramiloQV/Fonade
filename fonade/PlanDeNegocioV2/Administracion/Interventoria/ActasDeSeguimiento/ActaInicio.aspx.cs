using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Negocio.PlanDeNegocioV2.Administracion.Interventoria.Reintegros;
using Fonade.Negocio.PlanDeNegocioV2.Interventoria;
using Fonade.Clases;
using System.IO;
using System.Configuration;
using Fonade.Account;
using System.Web.Security;
using Fonade.PlanDeNegocioV2.Formulacion.Utilidad;
using Fonade.Negocio.Mensajes;
using Fonade.Negocio.FonDBLight;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento
{
    /// <summary>
    /// ActaInicio 
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class ActaInicio : System.Web.UI.Page
    {
        /// <summary>
        /// Obtener el codigo del acta
        /// </summary>
        /// <value>
        /// codigo acta.
        /// </value>
        public int CodigoActa
        {
            get
            {
                if (Request.QueryString["acta"] != null)
                    return Convert.ToInt32(Request.QueryString["acta"]);
                else
                    return 0;
            }
            set { }
        }

        /// <summary>
        /// Obtener el codigo del proyecto
        /// </summary>
        /// <value>
        /// codigo proyecto.
        /// </value>
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

        /// <summary>
        /// Obtener el usuario logueado
        /// </summary>
        /// <value>
        /// The usuario.
        /// </value>
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

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtFechaInicio.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
                ValidateUsers();
                GetInfo();

            }
        }
        /// <summary>
        /// Obtener la informacion del acta
        /// </summary>
        /// <exception cref="ApplicationException">
        /// No se logro encontrar información de esta acta
        /// or
        /// Este proyecto no tiene información de contratos completa
        /// or
        /// Este proyecto no tiene emprendedores activos.
        /// </exception>
        public void GetInfo()
        {
            try
            {
                var actaInicio = Negocio.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimientos.ActaSeguimiento.GetActaById(CodigoActa, CodigoProyecto);

                if (actaInicio == null)
                    throw new ApplicationException("No se logro encontrar información de esta acta");

                //lblMainTitle.Text = "Acta de inicio N° " + CodigoActa;

                var contrato = Negocio.PlanDeNegocioV2.Administracion.Interventoria.Abogado.GetContratoByProyecto(actaInicio.IdProyecto, Usuario.CodOperador);

                if (contrato.Any())
                {
                    if (!contrato.First().HasInfoCompleted)
                    {
                        throw new ApplicationException("Este proyecto no tiene información de contratos completa");
                    }
                    else
                    {
                        var infoContrato = contrato.First().Contrato;

                        var emprendedores = Negocio.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimientos.ActaSeguimiento.GetEmprendedoresYEquipoTrabajo(actaInicio.IdProyecto);

                        if (!emprendedores.Any())
                            throw new ApplicationException("Este proyecto no tiene emprendedores activos.");

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
                        }

                        lblNumeroContrato.Text = infoContrato.NumeroContrato;
                        lblTipoDeContrato.Text = infoContrato.TipoContrato;
                        lblObjeto.Text = infoContrato.ObjetoContrato;
                        lblValor.Text = FieldValidate.moneyFormat((double)infoContrato.ValorInicialEnPesos.GetValueOrDefault(0), true);
                        lblPlazo.Text = infoContrato.PlazoContratoMeses + " meses";
                        lblContratistas.Text = contratistas;
                    }
                }
            }
            catch (ApplicationException ex)
            {
                btnAdicionar.Visible = false;
                lblError.Visible = true;
                lblError.Text = "Advertencia: " + ex.Message;
            }
            catch (Exception ex)
            {
                btnAdicionar.Visible = false;
                lblError.Visible = true;
                lblError.Text = "Error inesperado: " + ex.Message;
            }
        }

        /// <summary>
        /// Validar usuario
        /// </summary>
        public void ValidateUsers()
        {
            try
            {
                if (!(Usuario.CodGrupo == Datos.Constantes.CONST_Interventor || Usuario.CodGrupo == Datos.Constantes.CONST_CoordinadorInterventor || Usuario.CodGrupo == Datos.Constantes.CONST_GerenteInterventor))
                {
                    Response.Redirect("~/FONADE/evaluacion/AccesoDenegado.aspx");
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("~/FONADE/evaluacion/AccesoDenegado.aspx");
            }
        }

        Negocio.FonDBLight.ConvocatoriaController convocatoriaController = new Negocio.FonDBLight.ConvocatoriaController();

        /// <summary>
        /// Inserta la informacion del acta generada
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <exception cref="ApplicationException">No se logro encontrar información de esta acta</exception>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtFechaInicio.Text != "")
                {


                    var actaInicio = Negocio.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimientos.ActaSeguimiento.GetActaById(CodigoActa, CodigoProyecto);

                    if (actaInicio == null)
                        throw new ApplicationException("No se logro encontrar información de esta acta");

                    actaInicio.FechaCreacion = Convert.ToDateTime(txtFechaInicio.Text);
                    actaInicio.Publicado = true;
                    actaInicio.FechaActualizacion = DateTime.Now;

                    Negocio.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimientos.ActaSeguimiento.InsertOrUpdateActa(actaInicio);

                    var convocatoria = convocatoriaController.GetConvocatoriaByProyecto(CodigoProyecto);
                    //copiar informacion de evaluacion
                    copiarInfoEvaluacion(CodigoProyecto, convocatoria.idConvocatoria);

                    Response.Redirect("~/PlanDeNegocioV2/Administracion/Interventoria/ActasDeSeguimiento/GestionarActas.aspx?codigo=" + actaInicio.IdProyecto, true);
                }
                else
                {
                    throw new ApplicationException("Debe seleccionar una fecha de inicio.");
                }
            }
            catch (ApplicationException ex)
            {
                lblError.Visible = true;
                lblError.Text = "Advertencia: " + ex.Message;
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Error inesperado: " + ex.Message;
            }
        }
        EvaluacionRiesgoController evaluacionRiesgoController = new EvaluacionRiesgoController();
        MetasActividadControllerDTO metasActividadController = new MetasActividadControllerDTO();
        ActaSeguimGestionProduccionController produccionController = new ActaSeguimGestionProduccionController();
        private void copiarInfoEvaluacion(int _codProyecto, int _codConvocatoria)
        {
            //copiarRiesgos
            var evalRiegos = evaluacionRiesgoController
                                .GetEvaluacionRiesgoByCodProyectoByCodConvocatoria(_codProyecto, _codConvocatoria);

            evaluacionRiesgoController.copiarInformacionRiesgos(evalRiegos, Usuario.IdContacto, _codConvocatoria, _codProyecto);


            //Copiar Produccion
            var evalProduccion = produccionController.getProduccionEvaluacion(_codProyecto);

            produccionController.copiarInformacionProduccion(evalProduccion, Usuario.IdContacto);

            //Copiar Mercadeo
            int sumMetas = 0;
            var evalMercadeo = metasActividadController.ListMetasMercadeo(_codProyecto, _codConvocatoria, ref sumMetas);

            metasActividadController.copiarInformacionMercadeo(evalMercadeo, Usuario.IdContacto, _codProyecto);
        }

        /// <summary>
        /// Handles the Click event of the btnCancelar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PlanDeNegocioV2/Administracion/Interventoria/ActasDeSeguimiento/GestionarActas.aspx?codigo=" + CodigoProyecto, true);
        }
    }
}