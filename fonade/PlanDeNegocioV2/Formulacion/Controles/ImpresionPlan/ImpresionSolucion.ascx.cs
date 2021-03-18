using Datos;
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
    public partial class ImpresionSolucion : System.Web.UI.UserControl
    {
        public ProyectoSolucion Solucion { get; set; }

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
            if (Solucion == null)
            {
                Solucion = new ProyectoSolucion();
            }

            lblConceptoNegocio.Text = Solucion.ConceptoNegocio;
            lblConceptoNegocio2.Text = Solucion.InnovadorConceptoNegocio;
            lblComoValido.Text = Solucion.AceptacionProyecto;
            lblComercial.Text = Solucion.Comercial;
            lblLegal.Text = Solucion.Legal;
            lblProceso.Text = Solucion.Proceso;
            lblProductoServicio.Text = Solucion.ProductoServicio;
            lblTecnicoproductivo.Text = Solucion.TecnicoProductivo;
        }
        #endregion
    }
}