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
    public partial class ImpresionNormatividad : System.Web.UI.UserControl
    {
        #region Variables

        public ProyectoNormatividad Formulario { get; set; }

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
                Formulario = new ProyectoNormatividad();
            }

            lblNormEmpresa.Text = Formulario.Empresarial;
            lblNormTribu.Text = Formulario.Tributaria;
            lblNormTecnica.Text = Formulario.Tecnica;
            lblNormLaboral.Text = Formulario.Laboral;
            lblNormAmbiental.Text = Formulario.Ambiental;
            lblMarcaProp.Text = Formulario.RegistroMarca;
            lblPregunta13.Text = Formulario.CondicionesTecnicas;
        }

        #endregion
    }
}