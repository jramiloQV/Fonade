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
    public partial class ImpresionOportunidad : System.Web.UI.UserControl
    {
        #region Variables

        public List<ProyectoOportunidadMercadoCompetidore> ListCompetidores { get; set; }

        public ProyectoOportunidadMercado Oportunidad { get; set; }

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
            if (Oportunidad == null)
            {
                Oportunidad = new ProyectoOportunidadMercado();
            }

            if (ListCompetidores == null)
            {
                ListCompetidores = new List<ProyectoOportunidadMercadoCompetidore>();
            }

            lblTendencia.Text = Oportunidad.TendenciaCrecimiento;

            gwCompetidores.DataSource = ListCompetidores;
            gwCompetidores.DataBind();
        }

        #endregion
    }
}