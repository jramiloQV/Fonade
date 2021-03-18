using Datos;
using Datos.DataType;
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
    public partial class ImpresionIngresos : System.Web.UI.UserControl
    {
        #region Variables

        public List<CondicionesCliente> ListCondiciones { get; set; }

        public ProyectoDesarrolloSolucion Formulario { get; set; }

        public Boolean EsClienteConsumidor { get; set; }

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
                Formulario = new ProyectoDesarrolloSolucion();
            }
            lblPregunta9.Text = Formulario.Ingresos;

            if (EsClienteConsumidor)
            {
                lblPtaConsumidor1.Text = Formulario.DondeCompra;
                lblPtaConsumidor2.Text = Formulario.CaracteristicasCompra;
                lblPtaConsumidor3.Text = Formulario.FrecuenciaCompra;
                lblPtaConsumidor4.Text = Formulario.Precio;
                pnlPtasConsumidor.Visible = true;
            }

            if (ListCondiciones != null)
            {
                gw_pregunta10.DataSource = ListCondiciones.Select(x => new Datos.DataType.CondicionesCliente()
                {
                    CaracteristicasCompra = x.CaracteristicasCompra,
                    Cliente = x.Cliente,
                    FormaPago = x.FormaPago,
                    FrecuenciaCompra = x.FrecuenciaCompra,
                    Garantias = x.Garantias,
                    Margen = x.Margen,
                    PrecioCadena = x.Precio.ToString("$ 0,0.00", CultureInfo.InvariantCulture),
                    RequisitosPostVenta = x.RequisitosPostVenta,
                    SitioCompra = x.SitioCompra
                }).ToList();
            }
            else
            {
                ListCondiciones = new List<CondicionesCliente>();

                gw_pregunta10.DataSource = ListCondiciones;
            }

            gw_pregunta10.DataBind();
        }

        #endregion
    }
}