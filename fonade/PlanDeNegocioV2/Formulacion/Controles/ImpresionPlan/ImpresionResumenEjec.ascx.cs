using Datos;
using Datos.DataType;
using Fonade.Negocio.Mensajes;
using Fonade.PlanDeNegocioV2.Formulacion.Utilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan
{
    public partial class ImpresionResumenEjec : System.Web.UI.UserControl
    {
        #region Variables

        public List<Emprendedor> ListEmprendedor { get; set; }

        public ProyectoResumenEjecutivoV2 Formulario { get; set; }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Enlazar();
            }
            catch (Exception ex)
            {
                Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Enlaza los datos consultados a los diferentes controles
        /// </summary>
        public void Enlazar()
        {
            if (Formulario == null)
            {
                Formulario = new ProyectoResumenEjecutivoV2();
            }

            if (ListEmprendedor == null)
            {
                ListEmprendedor = new List<Emprendedor>();
            }

            lblConcepto.Text = Formulario.ConceptoNegocio;
            lblEmpleo.Text = Formulario.IndicadorEmpleos;
            lblIndirectos.Text = Formulario.IndicadorEmpleosDirectos;
            lblMercadeo.Text = Formulario.IndicadorMercadeo;
            lblSena.Text = Formulario.IndicadorContraPartido;
            lblVentas.Text = Formulario.IndicadorVentas;

            gwEmprendedores.DataSource = ListEmprendedor;
            gwEmprendedores.DataBind();

        }

        #endregion
    }
}