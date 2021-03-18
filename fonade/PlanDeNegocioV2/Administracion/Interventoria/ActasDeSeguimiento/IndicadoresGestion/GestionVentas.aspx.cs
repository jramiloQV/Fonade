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
    public partial class GestionVentas : System.Web.UI.Page
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
            lblnumVisita.Text = (NumeroActa).ToString();
            lblMetaVentas.Text = MetaVenta(CodigoProyecto, CodigoConvocatoria).ToString("C");
            cargarGridIndicador(CodigoProyecto, CodigoConvocatoria);
        }

        MetasVentasControllerDTO metasVentasController = new MetasVentasControllerDTO();
        ActaSeguimGestionVentasController gestionVentasController = new ActaSeguimGestionVentasController();
        private decimal MetaVenta(int _codProyecto, int _codConvocatoria)
        {
            decimal meta = 0;

            var listmetas = metasVentasController.ListMetasVentas(_codProyecto, _codConvocatoria);

            meta = listmetas.Select(x => x.ventas).FirstOrDefault();
            
            return meta;
        }

        private void cargarGridIndicador(int _codProyecto, int _codConvocatoria)
        {
            var gestion = gestionVentasController.GetGestionVentas(_codProyecto, _codConvocatoria);

            gvIndicador.DataSource = gestion;
            gvIndicador.DataBind();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            ActaSeguimGestionVentasModel actaSegVentas = new ActaSeguimGestionVentasModel();

            //actaSegVentas.valor = Convert.ToDecimal(txtCantidad.Text);
            decimal number;
            if (Decimal.TryParse(txtCantidad.Text, out number))
                actaSegVentas.valor = number;
            actaSegVentas.descripcion = txtDescripcionEvento.Text;
            actaSegVentas.codConvocatoria = CodigoConvocatoria;
            actaSegVentas.codProyecto = CodigoProyecto;
            actaSegVentas.numActa = NumeroActa;
            actaSegVentas.visita = NumeroActa;

            if (number>=0)
            {
                string mensaje = "";
                if (Guardar(actaSegVentas, ref mensaje))
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
        
        private bool Guardar(ActaSeguimGestionVentasModel actaVentas, ref string mensaje)
        {
            bool guardado = false;

            guardado = gestionVentasController.InsertOrUpdateGestionVentas(actaVentas, ref mensaje);

            return guardado;
        }

        private void LimpiarCampos()
        {
            txtCantidad.Text = "";
            txtDescripcionEvento.Text = "";
        }

        private void Alert(string mensaje)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
        }

        protected void gvIndicador_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvIndicador.PageIndex = e.NewPageIndex;
            var Compromisos = gestionVentasController.GetGestionVentas(CodigoProyecto, CodigoConvocatoria);
            gvIndicador.DataSource = Compromisos;
            gvIndicador.DataBind();
        }
    }
}