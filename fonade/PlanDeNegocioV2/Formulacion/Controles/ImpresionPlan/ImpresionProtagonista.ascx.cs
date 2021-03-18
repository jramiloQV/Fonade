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
    public partial class ImpresionProtagonista : System.Web.UI.UserControl
    {
        #region Variables

        public List<ProyectoProtagonistaCliente> ListClientes { get; set; }

        public ProyectoProtagonista Protagonista { get; set; }

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

            if (Protagonista == null)
            {
                Protagonista = new ProyectoProtagonista();
            }

            if (ListClientes == null)
            {
                ListClientes = new List<ProyectoProtagonistaCliente>();
            }

            lblPerConsumidor.Text = Protagonista.PerfilConsumidor;
            lblNecConsumidor.Text = Protagonista.NecesidadesPotencialesConsumidores;
            lblNecCliente.Text = Protagonista.NecesidadesPotencialesClientes;

            if (Protagonista.IdProyecto != 0)
            {
                lblPerDiferent.Text = Protagonista.PerfilesDiferentes ? "SI" : "NO";
                lblPerConsumidor.Visible = Protagonista.PerfilesDiferentes;
                lblNecConsumidor.Visible = Protagonista.PerfilesDiferentes;
                lbNecConsumidor.Visible = Protagonista.PerfilesDiferentes;
                lbPerConsumidor.Visible = Protagonista.PerfilesDiferentes;
            }


            gwClientes.DataSource = ListClientes;
            gwClientes.DataBind();
        }

        #endregion
    }
}