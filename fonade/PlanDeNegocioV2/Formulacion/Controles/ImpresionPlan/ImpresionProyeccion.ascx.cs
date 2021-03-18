using Datos;
using Fonade.Negocio.Mensajes;
using Fonade.Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion;
using Fonade.PlanDeNegocioV2.Formulacion.Utilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan
{
    public partial class ImpresionProyeccion : System.Web.UI.UserControl
    {
        #region Variables

        public int CodigoProyecto { get; set; }

        public List<ProyectoProducto> ListadoProductos { get; set; }

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
            if (ListadoProductos == null)
            {
                ListadoProductos = new List<ProyectoProducto>();
            }

            GetTiempoProyeccion();

            gvProductos.DataSource = ListadoProductos;
            gvProductos.DataBind();

        }

        protected void GetTiempoProyeccion()
        {
            var proyeccion = Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.GetTiempoProyeccion(CodigoProyecto);

            if (proyeccion != null)
            {
                ShowIngresosPorVentas((int)proyeccion.TiempoProyeccion);
            }
            else
            {
                gvIngresosPorVentas.DataSource = new List<IngresosPorVentas>();
                gvIngresosPorVentas.DataBind();
            }
        }

        public void ShowIngresosPorVentas(int tiempoProyeccion)
        {
            VisibilidadTiempoProyeccion(tiempoProyeccion);

            var ingresosPorVentas = Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.GetIngresosPorVentas(CodigoProyecto);
            gvIngresosPorVentas.DataSource = ingresosPorVentas;
            gvIngresosPorVentas.DataBind();

            for (int i = (gvIngresosPorVentas.Rows.Count - 3); i < gvIngresosPorVentas.Rows.Count; i++)
            {
                gvIngresosPorVentas.Rows[i].Font.Bold = true;
            }
        }

        protected void VisibilidadTiempoProyeccion(int tiempoProyeccion)
        {
            for (int i = 1; i <= 10; i++)
            {
                if (i <= tiempoProyeccion)
                    gvIngresosPorVentas.Columns[i].Visible = true;
                else
                    gvIngresosPorVentas.Columns[i].Visible = false;
            }
        }

        #endregion

    }
}