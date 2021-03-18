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
    public partial class GestionProduccion : System.Web.UI.Page
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
                lblnumVisita.Text = (NumeroActa).ToString();
                cargarProductoMasRepresentativo(CodigoProyecto, CodigoConvocatoria);
                cargarGridMetaProduccion(CodigoProyecto, CodigoConvocatoria);
                cargarGridIndicador(CodigoProyecto, CodigoConvocatoria);
            }            
        }

        MetasProduccionControllerDTO metasProduccionController = new MetasProduccionControllerDTO();
        ActaSeguimGestionProduccionController produccionController = new ActaSeguimGestionProduccionController();
        private void cargarGridMetaProduccion(int _codproyecto, int _codConvocatoria)
        {
            var metaProduccion = metasProduccionController.ListMetasProduccion(_codproyecto, _codConvocatoria);

            gvMetasProduccion.DataSource = metaProduccion;
            gvMetasProduccion.DataBind();

            lblUnidadMedida.Text = metaProduccion.Select(x=>x.UnidadMedida).FirstOrDefault();
            lblMedida.Text = metaProduccion.Select(x => x.UnidadMedida).FirstOrDefault();
        }

        private void cargarProductoMasRepresentativo(int _codproyecto, int _codConvocatoria)
        {
            string producto = metasProduccionController.productoMasRepresentativo(_codproyecto, _codConvocatoria);

            lblProductoMasRep.Text = producto;
        }

        private void cargarGridIndicador(int _codProyecto, int _codConvocatoria)
        {
            var gestion = produccionController.GetGestionProduccion(_codProyecto, _codConvocatoria);

            gvIndicador.DataSource = gestion;
            gvIndicador.DataBind();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txtCantidad.Text)>=0)
            {
                ActaSeguimGestionProduccionModel actaSegProduccion = new ActaSeguimGestionProduccionModel()
                {
                    cantidad = Convert.ToInt32(txtCantidad.Text),
                    Descripcion = txtDescripcion.Text,
                    medida = lblUnidadMedida.Text,
                    codConvocatoria = CodigoConvocatoria,
                    codProyecto = CodigoProyecto,
                    numActa = NumeroActa,
                    visita = NumeroActa
                };
                string mensaje = "";
                if (Guardar(actaSegProduccion, ref mensaje))
                {
                    Alert("Se registraron los datos correctamente");
                    cargarGridIndicador(CodigoProyecto, CodigoConvocatoria);
                    LimpiarCampos();
                }
                else if (mensaje != "")
                {
                    Alert(mensaje);
                }
                else
                {
                    Alert("No logró guardar la informacion");
                }
            }
            else
            {
                Alert("El valor a ingresar debe ser mayor o igual que 0");
            }
        }

        private bool Guardar(ActaSeguimGestionProduccionModel actaProduccion, ref string mensaje)
        {
            bool guardado = false;

            guardado = produccionController.InsertOrUpdateGestionProduccion(actaProduccion, ref mensaje);

            return guardado;
        }

        private void LimpiarCampos()
        {
            txtCantidad.Text = "";
            txtDescripcion.Text = "";
        }

        private void Alert(string mensaje)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
        }

        protected void gvMetasProduccion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMetasProduccion.PageIndex = e.NewPageIndex;
            var Compromisos = metasProduccionController.ListMetasProduccion(CodigoProyecto, CodigoConvocatoria);
            gvMetasProduccion.DataSource = Compromisos;
            gvMetasProduccion.DataBind();
        }

        protected void gvIndicador_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvIndicador.PageIndex = e.NewPageIndex;
            var Compromisos = produccionController.GetGestionProduccion(CodigoProyecto, CodigoConvocatoria);
            gvIndicador.DataSource = Compromisos;
            gvIndicador.DataBind();
        }
    }
}