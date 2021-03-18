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
    public partial class ImpresionProduccion : System.Web.UI.UserControl
    {
        #region Variables

        public List<ProductoProceso> ListProcesos { get; set; }

        public ProyectoProduccion Formulario { get; set; }

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
                Formulario = new ProyectoProduccion();
            }

            lblPregunta143.Text = Formulario.CondicionesTecnicas;

            if (Formulario.IdProyecto != 0)
            {
                lblSel144.Text = Formulario.RealizaImportacion ? "SI" : "NO";
            }

            lblPregunta144.Text = Formulario.Justificacion;
            lblPregunta1441.Text = Formulario.ActivosProveedores;
            lblPregunta1442.Text = Formulario.IncrementoValor;

            CargarProcesosProducto();

        }

        private void CargarProcesosProducto()
        {
            int cont = 1;
            string nombrectrl = "lblProducto";
            string header = "lblHeader";

            if (ListProcesos != null)
            {
                //Se realiza la creación de controles para presentar los productos en la pregunta 15
                foreach (ProductoProceso item in ListProcesos)
                {
                    Label titulo = new Label();
                    Label ctrl = new Label();

                    ctrl.ID = nombrectrl + cont.ToString();
                    titulo.ID = header + cont.ToString();

                    titulo.Font.Bold = true;
                    titulo.Text = string.Format("{0}. {1} - {2}", cont.ToString(), item.NomProducto, item.Unidad);

                    if (item.Id_Proceso != null)
                    {
                        ctrl.Text = item.DescProceso;
                    }

                    pnProcesosProducto.Controls.Add(titulo);
                    pnProcesosProducto.Controls.Add(ctrl);
                    cont++;
                }
            }
        }

        #endregion
    }
}