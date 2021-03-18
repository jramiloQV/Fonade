using Datos.Modelos;
using Fonade.Negocio.FonDBLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.OtrosAspectos
{
    public partial class OtrosAspectos : System.Web.UI.Page
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
                lblnumV.Text = (NumeroActa).ToString();
                lblNumVA.Text = (NumeroActa).ToString();
                cargarDescripciones(CodigoProyecto, CodigoConvocatoria);
                CargarDrops();
                cargarGrids(CodigoProyecto,CodigoConvocatoria);
                mostrarinfo(NumeroActa);
            }
        }

        private void mostrarinfo(int _numActa)
        {
            if (_numActa == 1)//Primera Visita
            {
                pnlGridAmbiental.Visible = false;
                pnlGridInnovador.Visible = false;
            }
            else
            {
                btnGuardarDescripciones.Visible = false;
                txtDescripcionAmbiental.Enabled = false;
                txtDescripcionInnovador.Enabled = false;
            }
        }

        private void CargarDrops()
        {
            //Innovador
            cargarDDL(ddlValoracionInnovador);
            //Ambiental
            cargarDDL(ddlValoracionAmbiental);
        }

        private void cargarGrids(int _codProyecto,int _codConvocatoria)
        {
            var innovacion = otrosAspectosController.getOtrosAspectosInnovador(_codProyecto, _codConvocatoria);

            gvComponenteInnovador.DataSource = innovacion;
            gvComponenteInnovador.DataBind();

            var ambiental = otrosAspectosController.getOtrosAspectosAmbiental(_codProyecto, _codConvocatoria);

            gvComponenteAmbiental.DataSource = ambiental;
            gvComponenteAmbiental.DataBind();
        }

        private void cargarDDL(DropDownList ddl)
        {
            var opciones = ddlCumplimientoController.ddlDatosOtrosAspectos();

            ddl.DataSource = opciones;
            ddl.DataTextField = "valor"; // FieldName of Table in DataBase
            ddl.DataValueField = "id";
            ddl.DataBind();
        }
        ddlCumplimientoController ddlCumplimientoController = new ddlCumplimientoController();
        ActaSeguimOtrosAspectosController otrosAspectosController = new ActaSeguimOtrosAspectosController();
        private void cargarDescripciones(int _CodProyecto, int _codConvocatoria)
        {
            var descripOtrosAps = otrosAspectosController.getDescripcionOtrosAspectos(_CodProyecto, _codConvocatoria);

            if (descripOtrosAps == null)
            {
                txtDescripcionAmbiental.Text = "";
                txtDescripcionInnovador.Text = "";
            }
            else
            {
                txtDescripcionAmbiental.Text = descripOtrosAps.DescripCompAmbiental;
                txtDescripcionInnovador.Text = descripOtrosAps.DescripCompInnovador;
            }
        }

        protected void btnGuardarDescripciones_Click(object sender, EventArgs e)
        {
            string mensaje = "";
            if (validarCampos(ref mensaje))
            {
                if (guardarDescripciones(CodigoProyecto,CodigoConvocatoria,NumeroActa))
                {
                    Alert("Se guardó Correctamente");
                    cargarDescripciones(CodigoProyecto, CodigoConvocatoria);
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

        private bool validarCampos(ref string mensaje)
        {
            bool validado = true;
            mensaje = "Favor corregir: ";
            if (txtDescripcionAmbiental.Text == "")
            {
                mensaje = mensaje + "-El campo de Descripcion Ambiental no puede estar vacío; ";
                validado = false;
            }
            if (txtDescripcionInnovador.Text =="")
            {
                mensaje = mensaje + "-El campo de Descripcion Innovador no puede estar vacío; ";
                validado = false;
            }
            if (txtDescripcionAmbiental.Text.Length > 10000)
            {
                mensaje = mensaje + "-El campo de Descripcion Ambiental no puede superar los 10000 caracteres; ";
                validado = false;
            }
            if (txtDescripcionInnovador.Text.Length > 10000)
            {
                mensaje = mensaje + "-El campo de Descripcion Innovador no puede superar los 10000 caracteres; ";
                validado = false;
            }

            return validado;
        }

        private void Alert(string mensaje)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
        }

        private bool guardarDescripciones(int _codProyecto, int _codConvocatoria, int _numActa)
        {
            bool guardado = false;

            ActaSeguimOtrosAspectosModel otrosAspectosModel = new ActaSeguimOtrosAspectosModel()
            {
                codConvocatoria = _codConvocatoria,
                codProyecto = _codProyecto,
                numActa = _numActa,
                visita = (_numActa),
                DescripCompAmbiental = txtDescripcionAmbiental.Text,
                DescripCompInnovador = txtDescripcionInnovador.Text
            };

            guardado = otrosAspectosController.InsertOrUpdateDescripcionOtrosAspectos(otrosAspectosModel);

            return guardado;
        }

        protected void btnSaveValorInnovador_Click(object sender, EventArgs e)
        {
            string mensaje = "";
            if (validarCamposInnovador(ref mensaje))
            {
                if (guardarInnovador(CodigoProyecto, CodigoConvocatoria, NumeroActa))
                {
                    Alert("Se guardó Correctamente");
                    cargarGrids(CodigoProyecto, CodigoConvocatoria);
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

        private bool validarCamposInnovador(ref string mensaje)
        {
            bool validado = true;
            mensaje = "Favor corregir: ";
            if (txtObserInnovador.Text == "")
            {
                mensaje = mensaje + "-El campo de Observacion no puede estar vacío; ";
                validado = false;
            }
            if (ddlValoracionInnovador.SelectedIndex == 0)
            {
                mensaje = mensaje + "-Debe seleccionar una valoracion Si/No/Parcial; ";
                validado = false;
            }
            

            return validado;
        }

        private bool guardarInnovador(int _codProyecto, int _codConvocatoria, int _numActa)
        {
            bool guardado = false;

            ActaSeguimOtrosAspInnovadorModel otrosAspectosModel = new ActaSeguimOtrosAspInnovadorModel()
            {
                codConvocatoria = _codConvocatoria,
                codProyecto = _codProyecto,
                numActa = _numActa,
                visita = (_numActa),
                observacion = txtObserInnovador.Text,
                valoracion = ddlValoracionInnovador.SelectedItem.Text
            };

            guardado = otrosAspectosController.InsertOrUpdateOtrosAspInnovador(otrosAspectosModel);

            return guardado;
        }

        protected void btnSaveValorAmbiental_Click(object sender, EventArgs e)
        {
            string mensaje = "";
            if (validarCamposAmbiental(ref mensaje))
            {
                if (guardarAmbiental(CodigoProyecto, CodigoConvocatoria, NumeroActa))
                {
                    Alert("Se guardó Correctamente");
                    cargarGrids(CodigoProyecto, CodigoConvocatoria);
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

        private bool validarCamposAmbiental(ref string mensaje)
        {
            bool validado = true;
            mensaje = "Favor corregir: ";
            if (txtObservAmbiental.Text == "")
            {
                mensaje = mensaje + "-El campo de Observacion no puede estar vacío; ";
                validado = false;
            }
            if (ddlValoracionAmbiental.SelectedIndex == 0)
            {
                mensaje = mensaje + "-Debe seleccionar una valoracion Si/No/Parcial; ";
                validado = false;
            }


            return validado;
        }

        private bool guardarAmbiental(int _codProyecto, int _codConvocatoria, int _numActa)
        {
            bool guardado = false;

            ActaSeguimOtrosAspAmbientalModel otrosAspectosModel = new ActaSeguimOtrosAspAmbientalModel()
            {
                codConvocatoria = _codConvocatoria,
                codProyecto = _codProyecto,
                numActa = _numActa,
                visita = (_numActa),
                observacion = txtObservAmbiental.Text,
                valoracion = ddlValoracionAmbiental.SelectedItem.Text
            };

            guardado = otrosAspectosController.InsertOrUpdateOtrosAspAmbiental(otrosAspectosModel);

            return guardado;
        }

        protected void gvComponenteInnovador_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvComponenteInnovador.PageIndex = e.NewPageIndex;
            var Compromisos = otrosAspectosController.getOtrosAspectosInnovador(CodigoProyecto, CodigoConvocatoria);
            gvComponenteInnovador.DataSource = Compromisos;
            gvComponenteInnovador.DataBind();
        }

        protected void gvComponenteAmbiental_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvComponenteAmbiental.PageIndex = e.NewPageIndex;
            var Compromisos = otrosAspectosController.getOtrosAspectosAmbiental(CodigoProyecto, CodigoConvocatoria);
            gvComponenteAmbiental.DataSource = Compromisos;
            gvComponenteAmbiental.DataBind();
        }
    }
}