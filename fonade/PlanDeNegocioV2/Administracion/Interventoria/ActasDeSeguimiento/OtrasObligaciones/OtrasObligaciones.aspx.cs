using Datos;
using Datos.Modelos;
using Fonade.Negocio.FonDBLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.OtrasObligaciones
{
    public partial class OtrasObligaciones : System.Web.UI.Page
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
                lblNumVD.Text = (NumeroActa).ToString();
                cargarDescripciones(CodigoProyecto, CodigoConvocatoria);
                CargarDrops();
                cargarGrids(CodigoProyecto, CodigoConvocatoria);
                mostrarinfo(NumeroActa);
            }
        }

        private void mostrarinfo(int _numActa)
        {
            if (_numActa == 1)//Primera Visita
            {
                pnlGridAcomAsesoria.Visible = false;
                pnlGridDedicaEmprendedor.Visible = false;
                pnlGridInformacionPlataforma.Visible = false;
            }
            else
            {
                btnGuardarDescripciones.Visible = false;
                txtDedicacionEmprendedor.Enabled = false;
                txtInformacionPlataforma.Enabled = false;
                txtAcompAsesoria.Enabled = false;
            }
        }

        private void cargarGrids(int _codProyecto, int _codConvocatoria)
        {
            var infoPlataforma = otrasObligacionesController.GetOtrasObligInfoPlataforma(_codProyecto, _codConvocatoria);

            gvInformacionPlataforma.DataSource = infoPlataforma;
            gvInformacionPlataforma.DataBind();

            var dedicaEmp = otrasObligacionesController.GetOtrasObligDedicaEmprendedor(_codProyecto, _codConvocatoria);

            gvDedicaEmprendedor.DataSource = dedicaEmp;
            gvDedicaEmprendedor.DataBind();

            var acomAsesoria = otrasObligacionesController.GetOtrasObligAcomAsesoria(_codProyecto, _codConvocatoria);

            gvAcomAsesoria.DataSource = acomAsesoria;
            gvAcomAsesoria.DataBind();
        }

        private void CargarDrops()
        {
            //InfoPlataforma
            cargarDDL(ddlValoracionInfoPlataforma);
            cargarDDL(ddlValAcomAsesoria);
            cargarDDL(ddlValDedicaEmprendedor);
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
        ActaSeguimOtrasObligacionesController otrasObligacionesController = new ActaSeguimOtrasObligacionesController();
        private void cargarDescripciones(int _CodProyecto, int _codConvocatoria)
        {
            var descripOtrasOblig = otrasObligacionesController.getDescripcionOtrasObligaciones(_CodProyecto, _codConvocatoria);

            if (descripOtrasOblig == null)
            {
                txtAcompAsesoria.Text = "";
                txtDedicacionEmprendedor.Text = "";
                txtInformacionPlataforma.Text = "";
            }
            else
            {
                txtAcompAsesoria.Text = descripOtrasOblig.DescripAcomAsesoria;
                txtDedicacionEmprendedor.Text = descripOtrasOblig.DescripTiempoEmprendedor;
                txtInformacionPlataforma.Text = descripOtrasOblig.DescripInfoPlataforma;
            }

            if (txtDedicacionEmprendedor.Text == "")
            {
                txtDedicacionEmprendedor.Text = otrasObligacionesController.GetPerfilEmprendedor(CodigoProyecto, Constantes.CONST_RolEmprendedor);
            }
        }

        protected void btnGuardarDescripciones_Click(object sender, EventArgs e)
        {
            string mensaje = "";
            if (validarCampos(ref mensaje))
            {
                if (guardarDescripciones(CodigoProyecto, CodigoConvocatoria, NumeroActa))
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

        private void Alert(string mensaje)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
        }

        private bool validarCampos(ref string mensaje)
        {
            bool validado = true;
            mensaje = "Favor corregir: ";
            if (txtDedicacionEmprendedor.Text == "")
            {
                mensaje = mensaje + "-El campo de Descripcion Tiempo de Dedicación del Emprendedor no puede estar vacío; ";
                validado = false;
            }
            if (txtAcompAsesoria.Text == "")
            {
                mensaje = mensaje + "-El campo de Descripcion Acompañamiento y Asesoría no puede estar vacío; ";
                validado = false;
            }
            if (txtInformacionPlataforma.Text == "")
            {
                mensaje = mensaje + "-El campo de Descripcion Información en Plataforma no puede estar vacío; ";
                validado = false;
            }
            if (txtAcompAsesoria.Text.Length > 10000)
            {
                mensaje = mensaje + "-El campo de Descripcion Acompañamiento y Asesoría no puede superar los 10000 caracteres; ";
                validado = false;
            }
            if (txtDedicacionEmprendedor.Text.Length > 10000)
            {
                mensaje = mensaje + "-El campo de Descripcion Tiempo de Dedicación del Emprendedor no puede superar los 10000 caracteres; ";
                validado = false;
            }
            if (txtInformacionPlataforma.Text.Length > 10000)
            {
                mensaje = mensaje + "-El campo de Descripcion Información en Plataforma no puede superar los 10000 caracteres; ";
                validado = false;
            }

            return validado;
        }

        private bool guardarDescripciones(int _codProyecto, int _codConvocatoria, int _numActa)
        {
            bool guardado = false;

            ActaSeguimOtrasObligacionesModel otrasObligModel = new ActaSeguimOtrasObligacionesModel()
            {
                codConvocatoria = _codConvocatoria,
                codProyecto = _codProyecto,
                numActa = _numActa,
                visita = (_numActa),
                DescripAcomAsesoria = txtAcompAsesoria.Text,
                DescripInfoPlataforma = txtInformacionPlataforma.Text,
                DescripTiempoEmprendedor = txtDedicacionEmprendedor.Text
            };

            guardado = otrasObligacionesController.InsertOrUpdateOtrasObligaciones(otrasObligModel);

            return guardado;
        }

        protected void btnSaveInfoPlataforma_Click(object sender, EventArgs e)
        {
            string mensaje = "";
            if (validarCamposInfoPlataforma(ref mensaje))
            {
                if (guardarValInfoPlataforma(CodigoProyecto, CodigoConvocatoria, NumeroActa))
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

        private bool validarCamposInfoPlataforma(ref string mensaje)
        {
            bool validado = true;
            mensaje = "Favor corregir: ";
            if (txtObserInfoPlataforma.Text == "")
            {
                mensaje = mensaje + "-El campo de Observacion es obligatorio; ";
                validado = false;
            }
            if (ddlValoracionInfoPlataforma.SelectedIndex == 0)
            {
                mensaje = mensaje + "-Debe seleccionar un valor Si/No/Parcial; ";
                validado = false;
            }

            return validado;
        }

        private bool guardarValInfoPlataforma(int _codProyecto, int _codConvocatoria, int _numActa)
        {
            bool guardado = false;

            ActaSeguimOtrasObligInfoPlataformaModel otrasObligModel = new ActaSeguimOtrasObligInfoPlataformaModel()
            {
                codConvocatoria = _codConvocatoria,
                codProyecto = _codProyecto,
                numActa = _numActa,
                visita = (_numActa),
                observacion = txtObserInfoPlataforma.Text,
                valoracion = ddlValoracionInfoPlataforma.SelectedItem.Text
            };

            guardado = otrasObligacionesController.InsertOrUpdateOtrasObliInfoPlataforma(otrasObligModel);

            return guardado;
        }

        protected void btnSaveDedicaEmprendedor_Click(object sender, EventArgs e)
        {
            string mensaje = "";
            if (validarCamposDedicaEmp(ref mensaje))
            {
                if (guardarValDedicaEmp(CodigoProyecto, CodigoConvocatoria, NumeroActa))
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

        private bool validarCamposDedicaEmp(ref string mensaje)
        {
            bool validado = true;
            mensaje = "Favor corregir: ";
            if (txtObservDedicaEmprendedor.Text == "")
            {
                mensaje = mensaje + "-El campo de Observacion es obligatorio; ";
                validado = false;
            }
            if (ddlValDedicaEmprendedor.SelectedIndex == 0)
            {
                mensaje = mensaje + "-Debe seleccionar un valor Si/No/Parcial; ";
                validado = false;
            }

            return validado;
        }

        private bool guardarValDedicaEmp(int _codProyecto, int _codConvocatoria, int _numActa)
        {
            bool guardado = false;

            ActaSeguimOtrasObligTiempoEmpModel otrasObligModel = new ActaSeguimOtrasObligTiempoEmpModel()
            {
                codConvocatoria = _codConvocatoria,
                codProyecto = _codProyecto,
                numActa = _numActa,
                visita = (_numActa),
                observacion =txtObservDedicaEmprendedor.Text,
                valoracion = ddlValoracionInfoPlataforma.SelectedItem.Text
            };

            guardado = otrasObligacionesController.InsertOrUpdateOtrasObliDedicaEmp(otrasObligModel);

            return guardado;
        }

        protected void btnSaveAcomAsesoria_Click(object sender, EventArgs e)
        {
            string mensaje = "";
            if (validarCamposAcomAsesoria(ref mensaje))
            {
                if (guardarValAcomAsesoria(CodigoProyecto, CodigoConvocatoria, NumeroActa))
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

        private bool validarCamposAcomAsesoria(ref string mensaje)
        {
            bool validado = true;
            mensaje = "Favor corregir: ";
            if (txtObservAcomAsesoria.Text == "")
            {
                mensaje = mensaje + "-El campo de Observacion es obligatorio; ";
                validado = false;
            }
            if (ddlValAcomAsesoria.SelectedIndex == 0)
            {
                mensaje = mensaje + "-Debe seleccionar un valor Si/No/Parcial; ";
                validado = false;
            }

            return validado;
        }

        private bool guardarValAcomAsesoria(int _codProyecto, int _codConvocatoria, int _numActa)
        {
            bool guardado = false;

            ActaSeguimOtrasObligAcomAsesoriaModel otrasObligModel = new ActaSeguimOtrasObligAcomAsesoriaModel()
            {
                codConvocatoria = _codConvocatoria,
                codProyecto = _codProyecto,
                numActa = _numActa,
                visita = (_numActa),
                observacion = txtObservAcomAsesoria.Text,
                valoracion = ddlValoracionInfoPlataforma.SelectedItem.Text
            };

            guardado = otrasObligacionesController.InsertOrUpdateOtrasObliAcomAsesoria(otrasObligModel);

            return guardado;
        }

        protected void gvInformacionPlataforma_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvInformacionPlataforma.PageIndex = e.NewPageIndex;
            var Compromisos = otrasObligacionesController.GetOtrasObligInfoPlataforma(CodigoProyecto, CodigoConvocatoria);
            gvInformacionPlataforma.DataSource = Compromisos;
            gvInformacionPlataforma.DataBind();
        }

        protected void gvDedicaEmprendedor_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDedicaEmprendedor.PageIndex = e.NewPageIndex;
            var Compromisos = otrasObligacionesController.GetOtrasObligDedicaEmprendedor(CodigoProyecto, CodigoConvocatoria);
            gvDedicaEmprendedor.DataSource = Compromisos;
            gvDedicaEmprendedor.DataBind();
        }

        protected void gvAcomAsesoria_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAcomAsesoria.PageIndex = e.NewPageIndex;
            var Compromisos = otrasObligacionesController.GetOtrasObligAcomAsesoria(CodigoProyecto, CodigoConvocatoria);
            gvAcomAsesoria.DataSource = Compromisos;
            gvAcomAsesoria.DataBind();
        }
    }
}