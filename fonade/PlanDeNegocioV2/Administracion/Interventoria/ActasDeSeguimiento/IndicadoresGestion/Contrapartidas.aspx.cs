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
    public partial class Contrapartidas : System.Web.UI.Page
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
        ActaSeguimContrapartidaController contrapartidaController = new ActaSeguimContrapartidaController();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {               
                cargarGridIndicador(CodigoProyecto, CodigoConvocatoria);
                lblnumVisita.Text = (NumeroActa).ToString();
                var contrapartidas = Negocio.PlanDeNegocioV2.Utilidad.IndicadorFormulacion.GetContrapartidas(CodigoProyecto);
                lblContrapartidas.Text = contrapartidas.ToString();
            }
        }

        private void cargarGridIndicador(int _codProyecto, int _codConvocatoria)
        {
            var gestion = contrapartidaController.GetContrapartidas(_codProyecto, _codConvocatoria);

            gvIndicador.DataSource = gestion;
            gvIndicador.DataBind();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txtContrapartidas.Text)>=0)
            {
                ActaSeguimContrapartidaModel actaSegContrapartida = new ActaSeguimContrapartidaModel()
                {
                    cantContrapartida = Convert.ToInt32(txtContrapartidas.Text),
                    codConvocatoria = CodigoConvocatoria,
                    codProyecto = CodigoProyecto,
                    descripcion = txtDescripcion.Text,
                    numActa = NumeroActa,
                    visita = NumeroActa
                };
                string mensaje = "";
                if (Guardar(actaSegContrapartida, ref mensaje))
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
                Alert("El valor de la contrapartida debe ser mayor o igual que 0");
            }
            
        }

        private bool Guardar(ActaSeguimContrapartidaModel actaContrapartida, ref string mensaje)
        {
            bool guardado = false;

            guardado = contrapartidaController.InsertOrUpdateContrapartida(actaContrapartida,ref mensaje);

            return guardado;
        }

        private void LimpiarCampos()
        {
            txtContrapartidas.Text = "";
            txtDescripcion.Text = "";
        }

        private void Alert(string mensaje)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
        }

        protected void gvIndicador_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvIndicador.PageIndex = e.NewPageIndex;
            var Compromisos = contrapartidaController.GetContrapartidas(CodigoProyecto, CodigoConvocatoria);
            gvIndicador.DataSource = Compromisos;
            gvIndicador.DataBind();
        }
    }
}