using Datos;
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
    public partial class GestionMercadeo : Negocio.Base_Page
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

        MetasActividadControllerDTO metasActividadController = new MetasActividadControllerDTO();
        ActaSeguimGestionMercadeoController actaSeguimGestionMercadeoController = new ActaSeguimGestionMercadeoController();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                cargarGridMetasACtividades(CodigoProyecto, CodigoConvocatoria);
                cargarGridIndicador(CodigoProyecto, CodigoConvocatoria);
                lblnumVisita.Text = (NumeroActa).ToString();
            }
        }

        private void cargarGridMetasACtividades(int _codProyecto, int _codConvocatoria)
        {
            int sumMetas = 0;
            var consulta = metasActividadController.ListMetasMercadeoInterventoria(_codProyecto, _codConvocatoria, ref sumMetas);

            gvMetasMercadeo.DataSource = consulta;
            gvMetasMercadeo.DataBind();

            lblMetaTotalActividad.Text = sumMetas.ToString();
        }

        private void cargarGridIndicador(int _codProyecto, int _codConvocatoria)
        {
            var gestion = actaSeguimGestionMercadeoController.GetGestionMercadeo(_codProyecto, _codConvocatoria);

            gvIndicador.DataSource = gestion;
            gvIndicador.DataBind();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txtCantidad.Text)>=0)
            {
                ActaSeguimGestionMercadeoModel actaSegMercadeo = new ActaSeguimGestionMercadeoModel()
                {
                    codConvocatoria = CodigoConvocatoria,
                    codProyecto = CodigoProyecto,
                    cantidad = Convert.ToInt32(txtCantidad.Text),
                    numActa = NumeroActa,
                    descripcionEvento = txtDescripcionEvento.Text,
                    publicidadLogos = txtPublicidadLogos.Text,
                    visita = NumeroActa
                };

                if (Guardar(actaSegMercadeo))
                {
                    Alert("Se registraron los datos correctamente");
                    cargarGridIndicador(CodigoProyecto, CodigoConvocatoria);
                    LimpiarCampos();
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
        private bool Guardar(ActaSeguimGestionMercadeoModel actaMercadeo)
        {
            bool guardado = false;

            guardado = actaSeguimGestionMercadeoController.InsertOrUpdateGestionMercadeo(actaMercadeo);

            return guardado;
        }

        private void LimpiarCampos()
        {
            txtCantidad.Text = "";
            txtDescripcionEvento.Text = "";
            txtPublicidadLogos.Text = "";
        }

        private void Alert(string mensaje)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
        }

        protected void gvMetasMercadeo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            int sumMetas = 0;            
            gvMetasMercadeo.PageIndex = e.NewPageIndex;
            var Compromisos = metasActividadController.ListMetasMercadeoInterventoria(CodigoProyecto, CodigoConvocatoria, ref sumMetas);
            gvMetasMercadeo.DataSource = Compromisos;
            gvMetasMercadeo.DataBind();
        }

        protected void gvIndicador_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvIndicador.PageIndex = e.NewPageIndex;
            var Compromisos = actaSeguimGestionMercadeoController.GetGestionMercadeo(CodigoProyecto, CodigoConvocatoria);
            gvIndicador.DataSource = Compromisos;
            gvIndicador.DataBind();
        }

        protected void gvMetasMercadeo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                int idMercadeo = Convert.ToInt32(e.CommandArgument.ToString());

                if (metasActividadController.ocultarMetaMercadeo(idMercadeo, usuario.IdContacto))
                {
                    cargarGridMetasACtividades(CodigoProyecto, CodigoConvocatoria);
                    cargarGridIndicador(CodigoProyecto, CodigoConvocatoria);

                    Alert("Se eliminó la actividad.");
                }
                else
                {
                    Alert("No se logró eliminar la actividad.");
                }

            }
        }

        protected void gvMetasMercadeo_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvMetasMercadeo.EditIndex = -1;
            cargarGridMetasACtividades(CodigoProyecto, CodigoConvocatoria);
            cargarGridIndicador(CodigoProyecto, CodigoConvocatoria);
        }

        protected void gvMetasMercadeo_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvMetasMercadeo.EditIndex = e.NewEditIndex;
            cargarGridMetasACtividades(CodigoProyecto, CodigoConvocatoria);
            cargarGridIndicador(CodigoProyecto, CodigoConvocatoria);
        }

        protected void gvMetasMercadeo_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = (int)e.Keys["idActividadInterventoria"];


            int cantidad = Convert.ToInt32(((TextBox)gvMetasMercadeo.Rows[e.RowIndex].FindControl("txtUnidadesEditar")).Text);
            string actividad = ((TextBox)gvMetasMercadeo.Rows[e.RowIndex].FindControl("txtActividadEditar")).Text;

            if (actividad != "")
            {
                if (metasActividadController.actualizarMetaMercadeo(id,usuario.IdContacto, cantidad, actividad))
                {
                    Alert("Se actualizó la actividad.");
                }
                else
                {
                    Alert("No se logró actualizar la actividad.");
                }
            }
            else
            {
                Alert("Los campos no deben estar vacíos.");
            }

            gvMetasMercadeo.EditIndex = -1;
            cargarGridMetasACtividades(CodigoProyecto, CodigoConvocatoria);
            cargarGridIndicador(CodigoProyecto, CodigoConvocatoria);
        }

        protected void btnGuardarMercadeoAdd_Click(object sender, EventArgs e)
        {
            if (validaciones())
            {
                if (InsertarProducto(usuario.IdContacto, CodigoProyecto))
                {
                    cargarGridMetasACtividades(CodigoProyecto, CodigoConvocatoria);
                    cargarGridIndicador(CodigoProyecto, CodigoConvocatoria);
                    Alert("Meta agregada correctamente.");
                }
            }
        }

        private bool InsertarProducto(int _codContacto, int _codProyecto)
        {            
            ActaSeguimGestionMercadeoEvaluacion mercadeo = new ActaSeguimGestionMercadeoEvaluacion
            {
                codContacto = _codContacto,
                codProyecto = _codProyecto,
                actividad = txtDescripcionMercadeo.Text,
                fechaUltimaActualizacion = DateTime.Now,
                idActividad = 0,
                ocultar = false,
                unidades = Convert.ToInt32(txtCantidadMercadeo.Text)
            };

            return metasActividadController.insertarMercadeo(mercadeo);
                    
        }

        private bool validaciones()
        {
            bool valido = true;

            if (!validarTextbox(txtCantidadMercadeo))
            {
                Alert("El campo cantidad es obligatorio.");
                valido = false;
            }
            if (!validarTextbox(txtDescripcionMercadeo))
            {
                Alert("El campo descripción es obligatorio.");
                valido = false;
            }
           
            return valido;
        }

        private bool validarTextbox(TextBox textBox)
        {
            bool validado = false;

            if (textBox.Text != "")
            {
                validado = true;
            }

            return validado;
        }
    }
}