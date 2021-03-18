using Datos.Modelos;
using Fonade.Negocio.FonDBLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.EstadoEmpresa
{
    public partial class EstadoEmpresa : System.Web.UI.Page
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
        ActaSeguimEstadoEmpresaController estadoEmpresaController = new ActaSeguimEstadoEmpresaController();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblnumV.Text = (NumeroActa).ToString();
                cargarGrids(CodigoProyecto, CodigoConvocatoria);
            }
        }

        private void cargarGrids(int _codProyecto, int _codConvocatoria)
        {
            var estadoEmpresa = estadoEmpresaController.getListEstadoEmpresa(_codProyecto, _codConvocatoria);

            gvEstadoEmpresa.DataSource = estadoEmpresa;
            gvEstadoEmpresa.DataBind();
           
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = "";
            if (validarCampos(ref mensaje))
            {
                if (guardarDescripcion(CodigoProyecto, CodigoConvocatoria, NumeroActa))
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

        private bool guardarDescripcion(int _codProyecto, int _codConvocatoria, int _numActa)
        {
            bool guardado = false;

            var idh = Negocio.PlanDeNegocioV2.Utilidad.IndicadorFormulacion.GetIDH(_codProyecto);
            var nbi = Negocio.PlanDeNegocioV2.Utilidad.IndicadorFormulacion.GetNBI(_codProyecto);

            ActaSeguimEstadoEmpresaModel estadoEmpresa = new ActaSeguimEstadoEmpresaModel()
            {
                codConvocatoria = _codConvocatoria,
                codProyecto = _codProyecto,
                numActa = _numActa,
                visita = (_numActa),
                Descripcion = txtDescripcion.Text,
                NBI = nbi
            };

            guardado = estadoEmpresaController.InsertOrUpdateEstadoEmpresa(estadoEmpresa);

            return guardado;
        }

        private bool validarCampos(ref string mensaje)
        {
            bool validado = true;
            mensaje = " ";
            if (txtDescripcion.Text == "")
            {
                mensaje = mensaje + "-El campo de Descripción es obligatorio; ";
                validado = false;
            }
            if (txtDescripcion.Text.Length > 10000)
            {
                mensaje = mensaje + "-El campo de Descripción no puede superar los 10000 caracteres; ";
                validado = false;
            }
            
            return validado;
        }

        private void Alert(string mensaje)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
        }

        protected void gvEstadoEmpresa_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            
            gvEstadoEmpresa.PageIndex = e.NewPageIndex;
            var Compromisos = estadoEmpresaController.getListEstadoEmpresa(CodigoProyecto, CodigoConvocatoria);
            gvEstadoEmpresa.DataSource = Compromisos;
            gvEstadoEmpresa.DataBind();
            
        }
    }
}