using Datos.Modelos;
using Fonade.Negocio.FonDBLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.Riesgos
{
    public partial class GeneralRiesgos : Negocio.Base_Page//System.Web.UI.Page
    {
        public int CodigoProyecto
        {
            get { return Convert.ToInt32(Session["idProyecto"]); }
            set { }
        }

        public int numActa
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
                CargarGridRiesgos();
              
            }
        }

        EvaluacionRiesgoController evaluacionRiesgoController = new EvaluacionRiesgoController();
        ActaSeguimRiesgosController actaSeguimRiesgosController = new ActaSeguimRiesgosController();
        private void CargarGridRiesgos()
        {
            var evalRiegos = evaluacionRiesgoController
                                .GetSeguimientoRiesgoByCodProyectoByCodConvocatoria(CodigoProyecto, CodigoConvocatoria);

            gvRiesgos.DataSource = evalRiegos.ToList();
            gvRiesgos.DataBind();

            CargarDatosRiesgos();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (guardarDatos())
            {
                Alert("Datos registrados exitosamente.");
            }
            else
            {
                Alert("No se guardaron los datos.");
            }
        }

        private bool validarCampos()
        {
            bool validado = true;

            foreach (GridViewRow row in gvRiesgos.Rows)
            {                
                string texto = ((TextBox)(row.FindControl("txtGestionRiesgo"))).Text;

                if (texto.Trim()==""||texto.Trim().Equals(String.Empty))
                {
                    validado = false;
                }
            }

            return validado;
        }

        private bool guardarDatos()
        {
            bool guardado = false;
            try
            {
                if (!validarCampos())
                    throw new ApplicationException("los campos (Gestion del emprendedor) no deben estar vacios.");

                List<ActaSeguimRiesgosModel> actaRiesgos = new List<ActaSeguimRiesgosModel>();

                foreach (GridViewRow row in gvRiesgos.Rows)
                {
                    ActaSeguimRiesgosModel actariesgo = new ActaSeguimRiesgosModel();

                    actariesgo.GestionRiesgo = ((TextBox)(row.FindControl("txtGestionRiesgo"))).Text;
                    actariesgo.CodConvocatoria = Convert.ToInt32(((Label)(row.FindControl("lblCodConvocatoria"))).Text);
                    actariesgo.CodProyecto = Convert.ToInt32(((Label)(row.FindControl("lblCodProyecto"))).Text);
                    actariesgo.CodRiesgo = Convert.ToInt32(gvRiesgos.DataKeys[row.RowIndex].Values["idRiesgoInterventoria"].ToString());
                    actariesgo.NumActa = numActa;

                    actaRiesgos.Add(actariesgo);
                }

                guardado = actaSeguimRiesgosController.InsertOrUpdateGestionRiesgo(actaRiesgos);
            }
            catch (ApplicationException ex)
            {
                guardado = false;
                Alert(ex.Message);
            }
            return guardado;
        }

        private void CargarDatosRiesgos()
        {
            foreach (GridViewRow row in gvRiesgos.Rows)
            {
                int codRiesgo = Convert.ToInt32(gvRiesgos.DataKeys[row.RowIndex].Values["idRiesgoInterventoria"].ToString());

                string gestionEmp = actaSeguimRiesgosController.GetGestionEmprendedorByIdRiesgoByActa(codRiesgo, numActa);

                ((TextBox)(row.FindControl("txtGestionRiesgo"))).Text = gestionEmp;

            }
        }

        private void Alert(string mensaje)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
        }

        protected void gvRiesgos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Historial")
            {
                string[] Riesgo = e.CommandArgument.ToString().Split('|');
                lblRiesgo.Text = "Riesgo "+Riesgo[1]+": "+Riesgo[2];

                CargarGestionRiesgos(Convert.ToInt32(Riesgo[0]));

                ScriptManager.RegisterStartupScript(Page, typeof(Page), 
                    "msg", "mostrarHistorico();", true);                
            }

            if (e.CommandName == "Eliminar")
            {
                int Riesgo = Convert.ToInt32(e.CommandArgument.ToString());

                if (evaluacionRiesgoController.ocultarRiesgo(Riesgo, usuario.IdContacto))
                {
                    CargarGridRiesgos();
                   
                    Alert("Se eliminó el riesgo.");
                }
                else
                {
                    Alert("No se logró eliminar el riesgo.");
                }

            }
        }

        private void CargarGestionRiesgos(int _idRiesgo)
        {
            var gestionRiegos = actaSeguimRiesgosController.GetGestionEmprendedorByIdRiesgo(_idRiesgo);

            gvGestionEmprendedor.DataSource = gestionRiegos.ToList();
            gvGestionEmprendedor.DataBind();
        }
       
        protected void gvRiesgos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRiesgos.PageIndex = e.NewPageIndex;
            var Compromisos = evaluacionRiesgoController
                                .GetSeguimientoRiesgoByCodProyectoByCodConvocatoria(CodigoProyecto, CodigoConvocatoria);
            gvRiesgos.DataSource = Compromisos;
            gvRiesgos.DataBind();
        }

        protected void btnAgregarRiesgo_Click(object sender, EventArgs e)
        {
            if (txtDescripcionRiesgo.Text != "" && txtMitigacion.Text != "")
            {
                EvaluacionRiesgoModel erm = new EvaluacionRiesgoModel
                {
                    codConvocatoria = CodigoConvocatoria,
                    codProyecto = CodigoProyecto,
                    idRiesgo = 0,
                    Mitigacion = txtMitigacion.Text,
                    Riesgo = txtDescripcionRiesgo.Text
                };

                if (evaluacionRiesgoController
                                    .AddRiesgoInterventoria(erm, usuario.IdContacto))
                {
                    txtMitigacion.Text = "";
                    txtDescripcionRiesgo.Text = "";
                    CargarGridRiesgos();
             
                }
            }
            else
            {
                Alert("Los campos no pueden estar vacios.");
            }
        }

        protected void gvRiesgos_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvRiesgos.EditIndex = e.NewEditIndex;
            CargarGridRiesgos();
            mostrarBotones(false);
        }

        private void mostrarBotones(bool mostrar)
        {
            btnGuardar.Visible = mostrar;
        }

        protected void gvRiesgos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvRiesgos.EditIndex = -1;
            CargarGridRiesgos();
            mostrarBotones(true);
        }

        protected void gvRiesgos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = (int)e.Keys["idRiesgoInterventoria"];
            
           
            string riesgo = ((TextBox)gvRiesgos.Rows[e.RowIndex].FindControl("txtRiesgoEditar")).Text;
            string mitigacion = ((TextBox)gvRiesgos.Rows[e.RowIndex].FindControl("txtMitigacionEditar")).Text; 

            if (riesgo != "" && mitigacion != "")
            {
                if (evaluacionRiesgoController.actualizarRiesgo(id, usuario.IdContacto, riesgo, mitigacion))
                {
                    Alert("Se actualizó el riesgo.");
                }
                else
                {
                    Alert("No se logró actualizar el riesgo.");
                }
            }
            else
            {
                Alert("Los campos no deben estar vacíos.");

            }
            
            gvRiesgos.EditIndex = -1;
            CargarGridRiesgos();

            mostrarBotones(true);
        }
    }
}