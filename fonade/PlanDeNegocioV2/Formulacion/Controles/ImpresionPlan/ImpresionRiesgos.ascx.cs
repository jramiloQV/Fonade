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
    public partial class ImpresionRiesgos : System.Web.UI.UserControl
    {
        #region Variables

        public ProyectoRiesgo Formulario { get; set; }

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
                Formulario = new ProyectoRiesgo();
            }

            lblActores.Text = Formulario.ActoresExternos;
            lblFactores.Text = Formulario.FactoresExternos;

        }

        #endregion
    }
}