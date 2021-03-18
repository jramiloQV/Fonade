using Datos.Modelos;
using Fonade.Negocio.FonDBLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.Compromisos
{
    /// <summary>
    /// Compromisos
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class Compromisos : System.Web.UI.Page
    {
        /// <summary>
        /// Obtiene el codigo del proyecto
        /// </summary>
        /// <value>
        /// codigo proyecto.
        /// </value>
        public int CodigoProyecto
        {
            get { return Convert.ToInt32(Session["idProyecto"]); }
            set { }
        }


        private const string EstadoCERRADO = "CERRADO";
        private const string EstadoREPROGRAMADO = "REPROGRAMADO POR INCUMPLIMIENTO";
        private const string EstadoPENDIENTE = "PENDIENTE";

        /// <summary>
        /// Obtiene el numero del acta
        /// </summary>
        /// <value>
        /// numero acta.
        /// </value>
        public int NumeroActa
        {
            get { return Convert.ToInt32(Session["idActa"]); }
            set { }
        }

        /// <summary>
        /// Obtiene el codigo de la convocatoria
        /// </summary>
        /// <value>
        /// codigo convocatoria.
        /// </value>
        public int CodigoConvocatoria
        {
            get
            {
                return Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(CodigoProyecto, HttpContext.Current.Session["HistorialEvaluacion"] != null ? Convert.ToInt32(HttpContext.Current.Session["HistorialEvaluacion"]) : 0).GetValueOrDefault();
            }
            set { }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Establecer fechas de inicio y final
                //calendar.StartDate = DateTime.Now;
                //calendarCumplimiento.EndDate = DateTime.Now;
                //calendarEditEjecucion.StartDate = DateTime.Now;
                cargarGrids(CodigoProyecto, CodigoConvocatoria);
                //cargarDDL(ddlEditEstado);
            }
        }
        ddlCumplimientoController ddlCumplimientoController = new ddlCumplimientoController();

        /// <summary>
        /// cargar los dropdownlist
        /// </summary>
        /// <param name="ddl">DDL.</param>
        public void cargarDDL(DropDownList ddl)
        {
            var opciones = ddlCumplimientoController.opcionesCompromiso();

            ddl.DataSource = opciones;
            ddl.DataTextField = "valor"; // FieldName of Table in DataBase
            ddl.DataValueField = "id";
            ddl.DataBind();
        }

        ActaSeguimCompromisosController compromisosController = new ActaSeguimCompromisosController();

        /// <summary>
        /// Guardar compromiso
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = "";
            if (validarCampos(ref mensaje))
            {
                if (guardarCompromiso(CodigoProyecto, CodigoConvocatoria, NumeroActa))
                {
                    Alert("Se guardó Correctamente");
                    cargarGrids(CodigoProyecto, CodigoConvocatoria);
                    limpiarCampos();
                }
                else
                {
                    Alert("No pudo almacenar la informacion.");
                }
            }
            else
            {
                Alert(mensaje);
            }
        }

        /// <summary>
        /// Carga el listado de compromisos
        /// </summary>
        /// <param name="_codProyecto">The cod proyecto.</param>
        /// <param name="_codConvocatoria">The cod convocatoria.</param>
        private void cargarGrids(int _codProyecto, int _codConvocatoria)
        {
            var histCompromisos = compromisosController.getCompromisosHist(_codProyecto, _codConvocatoria);
            
            gvHistorialCompromisos.DataSource = histCompromisos;
            gvHistorialCompromisos.DataBind();
            
            var compromisos = compromisosController.getCompromisos(_codProyecto, _codConvocatoria);

            gvCompromisos.DataSource = compromisos;
            gvCompromisos.DataBind();
        }

        private void limpiarCampos()
        {
            txtCompromiso.Text = "";
            txtFecha.Text = "";
            txtObservacion.Text = "";
        }

        private bool guardarCompromiso(int _codProyecto, int _codConvocatoria, int _numActa)
        {
            bool guardado = false;
            DateTime dt = Convert.ToDateTime(txtFecha.Text);
            ActaSeguimCompromisosModel actaCompromiso = new ActaSeguimCompromisosModel()
            {
                codConvocatoria = _codConvocatoria,
                codProyecto = _codProyecto,
                numActa = _numActa,
                visita = (_numActa),
                compromiso = txtCompromiso.Text,
                estado = EstadoPENDIENTE,
                fechaModificado = DateTime.Now,
                fechaPropuestaEjec = dt,
                observacion = txtObservacion.Text,
                fechaCumpliCompromiso = null
            };

            guardado = compromisosController.InsertCompromisos(actaCompromiso);

            return guardado;
        }

        private bool validarCampos(ref string mensaje)
        {
            bool validado = true;
            mensaje = " ";
            if (txtCompromiso.Text == "")
            {
                mensaje = mensaje + "-El campo Compromiso es obligatorio; ";
                validado = false;
            }
            if (txtCompromiso.Text.Length > 500)
            {
                mensaje = mensaje + "-El campo Compromiso no puede superar los 500 caracteres; ";
                validado = false;
            }
            if (txtObservacion.Text == "")
            {
                mensaje = mensaje + "-El campo Observacion es obligatorio; ";
                validado = false;
            }
            if (txtObservacion.Text.Length >10000)
            {
                mensaje = mensaje + "-El campo Observacion no puede superar los 10000 caracteres; ";
                validado = false;
            }
            if (txtFecha.Text == "")
            {
                mensaje = mensaje + "-El campo Fecha es obligatorio; ";
                validado = false;
            }
            if (txtFecha.Text != "")
            {
                DateTime dt = Convert.ToDateTime(txtFecha.Text);
                //if (dt < DateTime.Now.Date)
                //{
                //    mensaje = mensaje + "-NO SE GUARDO EL COMPROMISO: La fecha seleccionada debe ser igual o mayor a la de hoy; ";
                //    validado = false;
                //}
            }


            return validado;
        }

        private void Alert(string mensaje)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
        }

        /// <summary>
        /// Handles the RowCommand event of the gvCompromisos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewCommandEventArgs"/> instance containing the event data.</param>
        protected void gvCompromisos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("EditarCompromiso"))
            {
                if (e.CommandArgument != null)
                {
                    string id = e.CommandArgument.ToString();

                    var compromisoID = compromisosController.getCompromisoByID(Convert.ToInt64(id));

                    txtEditCompromiso.Text = compromisoID.compromiso;
                    txtEditFechaEjecucion.Text = compromisoID.fechaPropuestaEjec.ToShortDateString();
                    lblIDCompromiso.Text = compromisoID.id.ToString();
                    ddlEditEstado.SelectedIndex = 0;

                    ModalEditarCompromiso.Show();
                }
            }
            if (e.CommandName.Equals("ELIMINAR"))
            {
                if (e.CommandArgument != null)
                {
                    string id = e.CommandArgument.ToString();

                    bool eliminado = compromisosController.EliminarCompromiso(Convert.ToInt64(id));
                    cargarGrids(CodigoProyecto, CodigoConvocatoria);
                    if (eliminado)
                        Alert("Compromiso Eliminado Correctamente!");
                    else
                        Alert("No se eliminó el compromiso.");

                }
            }
        }

        /// <summary>
        /// Handles the Click event of the btnEditGuardar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnEditGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = "";
            if (validarEditCampos(ref mensaje))
            {
                string fecha = "";
                if (mensaje == EstadoCERRADO)
                {
                    fecha = txtEditFechaCumplimiento.Text;
                }
                else if (mensaje == EstadoREPROGRAMADO)
                {
                    fecha = txtEditFechaEjecucion.Text;
                }

                string Id = lblIDCompromiso.Text;
                string Observacion = txtEditObservacion.Text;

                if (guardarEditCompromiso(Id, Observacion, mensaje, fecha))
                {
                    Alert("Se guardó Correctamente");
                    cargarGrids(CodigoProyecto, CodigoConvocatoria);
                    limpiarEditCampos();
                }
                else
                {
                    Alert("No pudo almacenar la informacion.");
                }
            }
            else
            {
                Alert(mensaje);
            }
        }

        private bool guardarEditCompromiso(string _id, string _observacion
                                , string _estado, string _fecha)
        {
            bool guardado = false;

            DateTime dt = Convert.ToDateTime(_fecha);
            ActaSeguimCompromisosModel actaCompromiso = new ActaSeguimCompromisosModel();

            actaCompromiso.id = Convert.ToInt64(_id);
            actaCompromiso.estado = _estado;
            actaCompromiso.fechaModificado = DateTime.Now;
            actaCompromiso.observacion = _observacion;
            actaCompromiso.numActa = NumeroActa;
            actaCompromiso.visita = (NumeroActa);

            if (_estado == EstadoCERRADO)
            {
                actaCompromiso.fechaCumpliCompromiso = dt;
            }
            else if (_estado == EstadoREPROGRAMADO)
            {
                actaCompromiso.fechaPropuestaEjec = dt;
                actaCompromiso.fechaCumpliCompromiso = null;
            }

            guardado = compromisosController.UpdateCompromisos(actaCompromiso);

            return guardado;
        }

        private void limpiarEditCampos()
        {
            txtEditCompromiso.Text = "";
            txtEditFechaCumplimiento.Text = "";
            txtEditFechaEjecucion.Text = "";
            txtEditObservacion.Text = "";
        }


        private bool validarEditCampos(ref string mensaje)
        {
            bool validado = true;
            mensaje = " ";
            if (txtEditCompromiso.Text == "")
            {
                mensaje = mensaje + "-El campo Compromiso es obligatorio; ";
                validado = false;
            }
            if (txtEditCompromiso.Text.Length > 500)
            {
                mensaje = mensaje + "-El campo Compromiso no puede superar los 500 caracteres; ";
                validado = false;
            }
            if (txtEditObservacion.Text == "")
            {
                mensaje = mensaje + "-El campo Observacion es obligatorio; ";
                validado = false;
            }
            if (txtEditObservacion.Text.Length > 10000)
            {
                mensaje = mensaje + "-El campo Observacion no puede superar los 10000 caracteres; ";
                validado = false;
            }

            string opcion = "";

            for (int i = 0; i <= ddlEditEstado.Items.Count - 1; i++)
            {
                if (ddlEditEstado.Items[i].Selected)
                    opcion = ddlEditEstado.Items[i].Text;
            }

            if (opcion == "Seleccione")
            {
                mensaje = mensaje + "-Debe Seleccionar un Estado; ";
                validado = false;
            }
            else if (opcion == EstadoCERRADO)
            {
                if (txtEditFechaCumplimiento.Text == "")
                {
                    mensaje = mensaje + "-El campo Fecha Cumplimiento es obligatorio; ";
                    validado = false;
                }
                if (txtEditFechaCumplimiento.Text != "")
                {
                    DateTime dt = Convert.ToDateTime(txtEditFechaCumplimiento.Text);
                    //if (dt > DateTime.Now.Date)
                    //{
                    //    mensaje = mensaje + "-NO SE GUARDO EL COMPROMISO: La fecha seleccionada debe ser igual o menor a la de hoy; ";
                    //    validado = false;
                    //}
                }
            }
            else if (opcion == EstadoREPROGRAMADO)
            {
                if (txtEditFechaEjecucion.Text == "")
                {
                    mensaje = mensaje + "-El campo Fecha Ejecucion es obligatorio; ";
                    validado = false;
                }
                if (txtEditFechaEjecucion.Text != "")
                {
                    DateTime dt = Convert.ToDateTime(txtEditFechaEjecucion.Text);
                    //if (dt < DateTime.Now.Date)
                    //{
                    //    mensaje = mensaje + "-NO SE GUARDO EL COMPROMISO: La fecha seleccionada debe ser igual o mayor a la de hoy; ";
                    //    validado = false;
                    //}
                }
            }

            if (validado)
                mensaje = opcion;

            return validado;
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the gvHistorialCompromisos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs"/> instance containing the event data.</param>
        protected void gvHistorialCompromisos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvHistorialCompromisos.PageIndex = e.NewPageIndex;
            var histCompromisos = compromisosController.getCompromisosHist(CodigoProyecto, CodigoConvocatoria);
            gvHistorialCompromisos.DataSource = histCompromisos;
            gvHistorialCompromisos.DataBind();
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the gvCompromisos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs"/> instance containing the event data.</param>
        protected void gvCompromisos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCompromisos.PageIndex = e.NewPageIndex;
            var Compromisos = compromisosController.getCompromisos(CodigoProyecto, CodigoConvocatoria);
            gvCompromisos.DataSource = Compromisos;
            gvCompromisos.DataBind();
        }

    }
}