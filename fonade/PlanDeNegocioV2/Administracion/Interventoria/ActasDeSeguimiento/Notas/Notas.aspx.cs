using Datos.Modelos;
using Fonade.Negocio.FonDBLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.Notas
{
    public partial class Notas : System.Web.UI.Page
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
                cargarGrids(CodigoProyecto, CodigoConvocatoria);
            }
        }
        ActaSeguimNotasController actaSeguimNotasController = new ActaSeguimNotasController();
        private void cargarGrids(int _codProyecto, int _codConvocatoria)
        {
            var Notas = actaSeguimNotasController
                            .getListNotas(_codProyecto, _codConvocatoria);

            gvNotas.DataSource = Notas;
            gvNotas.DataBind();
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

        private bool validarCampos(ref string mensaje)
        {
            bool validado = true;
            mensaje = " ";
            if (txtNota.Text == "")
            {
                mensaje = mensaje + "-El campo de Nota es obligatorio; ";
                validado = false;
            }
           
            return validado;
        }

        private void Alert(string mensaje)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
        }

        private bool guardarDescripcion(int _codProyecto, int _codConvocatoria, int _numActa)
        {
            bool guardado = false;
            
            ActaSeguimNotasModel Nota = new ActaSeguimNotasModel()
            {
                codConvocatoria = _codConvocatoria,
                codProyecto = _codProyecto,
                numActa = _numActa,
                visita = (_numActa),
                Notas = txtNota.Text
            };

            guardado = actaSeguimNotasController.InsertOrUpdateNotas(Nota);

            return guardado;
        }

        protected void gvNota_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvNotas.PageIndex = e.NewPageIndex;
            var Compromisos = actaSeguimNotasController.getListNotas(CodigoProyecto, CodigoConvocatoria);
            gvNotas.DataSource = Compromisos;
            gvNotas.DataBind();
        }

    }
}