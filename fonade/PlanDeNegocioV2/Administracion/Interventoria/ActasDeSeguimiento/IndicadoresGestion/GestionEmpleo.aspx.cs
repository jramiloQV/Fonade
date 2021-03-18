using Datos.Modelos;
using Fonade.Negocio.FonDBLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.IndicadoresGestion
{
    public partial class GestionEmpleo : System.Web.UI.Page
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
                CargarGridMetas(CodigoProyecto, CodigoConvocatoria);
                cargarGridGestionEmpleo(CodigoProyecto, CodigoConvocatoria);
                //MostrarPanelIndicador(CodigoProyecto, CodigoConvocatoria, NumeroActa);
                numeracionVisita(CodigoProyecto,CodigoConvocatoria);
            }
        }

        MetasEmpleoControllerDTO metasEmpleoController = new MetasEmpleoControllerDTO();
        ActaSeguimGestionEmpleoController empleoController = new ActaSeguimGestionEmpleoController();

        private void CargarGridMetas(int _codProyecto, int _codConvocatoria)
        {
            int totalMetasEmp = 0;

            var Metas = metasEmpleoController.ListMetasEmpleo(_codProyecto, _codConvocatoria, ref totalMetasEmp);
            gvMetasEmpleos.DataSource = Metas;
            gvMetasEmpleos.DataBind();

            lblMetaTotalEmpleos.Text = totalMetasEmp.ToString();

        }

        private void cargarGridGestionEmpleo(int _codProyecto, int _codConvocatoria)
        {
            var indicador = empleoController.GetGestionEmploByCodProyectoByCodConvocatoria(_codProyecto, _codConvocatoria);
            gvIndicador.DataSource = indicador;
            gvIndicador.DataBind();
        }

        private void MostrarPanelIndicador(int _codProyecto, int _codConvocatoria, int _numActa)
        {
            pnlAddIndicador.Visible = !empleoController.registroIndicadorExist(_codProyecto, _codConvocatoria, _numActa);
        }

        private void numeracionVisita(int _codProyecto, int _codConvocatoria)
        {
            int numVisita = NumeroActa;
                //empleoController.numVisita(_codProyecto, _codConvocatoria);
            lblnumVisita.Text = numVisita.ToString();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txtVerificacion.Text)>=0)
            {
                ActaSeguimGestionEmpleoModel actaSegEmpleo = new ActaSeguimGestionEmpleoModel()
                {
                    codConvocatoria = CodigoConvocatoria,
                    codProyecto = CodigoProyecto,
                    desarrolloIndicador = txtDesarrollo.Text,
                    numActa = NumeroActa,
                    verificaIndicador = Convert.ToInt32(txtVerificacion.Text),
                    Visita = NumeroActa
                };

                if (Guardar(actaSegEmpleo))
                {
                    Alert("Se registraron los datos correctamente");
                    cargarGridGestionEmpleo(CodigoProyecto, CodigoConvocatoria);
                    LimpiarCampos();
                }
                else
                {
                    Alert("No logró guardar la informacion");
                }
            }
            else
            {
                Alert("El valor en verificado debe ser mayor o igual que 0");
            }
           
        }

        private bool Guardar(ActaSeguimGestionEmpleoModel actaEmpleo)
        {
            bool guardado = false;

            guardado = empleoController.InsertOrUpdateGestionEmpleo(actaEmpleo);

            return guardado;
        }

        private void LimpiarCampos()
        {
            txtDesarrollo.Text = "";
            txtVerificacion.Text = "";
        }

        private void Alert(string mensaje)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
        }

        protected void gvMetasEmpleos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            int totalMetasEmp = 0;
            gvMetasEmpleos.PageIndex = e.NewPageIndex;
            var Compromisos = metasEmpleoController.ListMetasEmpleo(CodigoProyecto, CodigoConvocatoria, ref totalMetasEmp);
            gvMetasEmpleos.DataSource = Compromisos;
            gvMetasEmpleos.DataBind();
        }

        protected void gvIndicador_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvIndicador.PageIndex = e.NewPageIndex;
            var Compromisos = empleoController.GetGestionEmploByCodProyectoByCodConvocatoria(CodigoProyecto, CodigoConvocatoria);
            gvIndicador.DataSource = Compromisos;
            gvIndicador.DataBind();
        }
    }
}