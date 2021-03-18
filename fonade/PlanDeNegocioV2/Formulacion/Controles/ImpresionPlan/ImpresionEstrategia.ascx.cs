using Datos;
using Fonade.Negocio.Mensajes;
using Fonade.PlanDeNegocioV2.Formulacion.Utilidad;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan
{
    public partial class ImpresionEstrategia : System.Web.UI.UserControl
    {
        #region Variables

        public List<ProyectoEstrategiaActividade> ListPromocion { get; set; }

        public List<ProyectoEstrategiaActividade> ListComunicacion { get; set; }

        public List<ProyectoEstrategiaActividade> ListDistribucion { get; set; }

        public ProyectoFuturoNegocio Formulario { get; set; }

        public string TotalCosto { get; set; }

        public string TotalCostoCom { get; set; }

        public string TotalCostoDis { get; set; }

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
                Formulario = new ProyectoFuturoNegocio();
            }

            lblComunicacion.Text = Formulario.EstrategiaComunicacion;
            lblDistribucion.Text = Formulario.EstrategiaDistribucion;
            lblPromocion.Text = Formulario.EstrategiaPromocion;
            lblProposito.Text = Formulario.EstrategiaPromocionProposito;
            lblPropositoCom.Text = Formulario.EstrategiaComunicacionProposito;
            lblPropositoDis.Text = Formulario.EstrategiaDistribucionProposito;

            if (ListPromocion != null)
            {
                TotalCosto = ListPromocion.Sum(x => x.Costo).ToString("$ 0,0.00", CultureInfo.InvariantCulture);

            }
            else
            {
                ListPromocion = new List<ProyectoEstrategiaActividade>();
            }

            gwPromocion.DataSource = ListPromocion;
            gwPromocion.DataBind();

            if (ListComunicacion != null)
            {
                TotalCostoCom = ListComunicacion.Sum(x => x.Costo).ToString("$ 0,0.00", CultureInfo.InvariantCulture);
            }
            else
            {
                ListComunicacion = new List<ProyectoEstrategiaActividade>();
            }

            gwComunicacion.DataSource = ListComunicacion;
            gwComunicacion.DataBind();

            if (ListDistribucion != null)
            {
                TotalCostoDis = ListDistribucion.Sum(x => x.Costo).ToString("$ 0,0.00", CultureInfo.InvariantCulture);


            }

            else
            {
                ListDistribucion = new List<ProyectoEstrategiaActividade>();
            }

            gwDistribucion.DataSource = ListDistribucion;
            gwDistribucion.DataBind();

        }

        #endregion
    }
}