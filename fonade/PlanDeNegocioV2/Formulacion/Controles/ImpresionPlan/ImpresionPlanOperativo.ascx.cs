using Datos;
using Fonade.Negocio.Mensajes;
using Fonade.PlanDeNegocioV2.Formulacion.Utilidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan
{
    public partial class ImpresionPlanOperativo : System.Web.UI.UserControl
    {
        public int CodigoProyecto { get; set; }

        /// <summary>
        /// Cadena de conexión a la base de datos
        /// </summary>
        static string cadenaConexion
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                CargarGridActividades();
            }
            catch (Exception ex)
            {
                Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
            
        }

        protected void CargarGridActividades()
        {
            using (Datos.FonadeDBDataContext consultas = new Datos.FonadeDBDataContext(cadenaConexion))
            {
                var query = (from p in consultas.ProyectoActividadPOs
                             where p.CodProyecto == Convert.ToInt32(CodigoProyecto)
                             orderby p.Item ascending
                             select new { p.Id_Actividad, p.Item, p.NomActividad });

                string consultaDetalle = "select id_actividad as CodActividad, mes,codtipofinanciacion,valor ";
                consultaDetalle += "from proyectoactividadpomes LEFT OUTER JOIN proyectoactividadPO ";
                consultaDetalle += "on id_actividad=codactividad where codproyecto={0}";
                consultaDetalle += " order by item, codactividad,mes,codtipofinanciacion";
                IEnumerable<ProyectoActividadPOMe> respuestaDetalle = consultas.ExecuteQuery<ProyectoActividadPOMe>(consultaDetalle, Convert.ToInt32(CodigoProyecto));


                DataTable datos = new DataTable();
                DataTable detalle = new DataTable();
                datos.Columns.Add("CodProyecto");
                datos.Columns.Add("Id_Actividad");
                datos.Columns.Add("Item");
                datos.Columns.Add("Actividad");
                for (int i = 1; i <= 12; i++)
                {
                    detalle.Columns.Add("fondo" + i);
                    detalle.Columns.Add("emprendedor" + i);
                }
                detalle.Columns.Add("fondoTotal");
                detalle.Columns.Add("emprendedorTotal");

                foreach (var item in query)
                {
                    DataRow dr = datos.NewRow();

                    dr["CodProyecto"] = CodigoProyecto;
                    dr["Id_Actividad"] = item.Id_Actividad;
                    dr["Item"] = item.Item;
                    dr["Actividad"] = item.NomActividad;
                    datos.Rows.Add(dr);
                }
                int actividadActual = 0;
                DataRow drDet = detalle.NewRow();

                decimal totalFondo = 0;
                decimal totalEmprendedor = 0;

                foreach (ProyectoActividadPOMe registro in respuestaDetalle)
                {
                    if (actividadActual != registro.CodActividad)
                    {
                        if (actividadActual != 0)
                        {

                            drDet["fondoTotal"] = "$ " + String.Format("{0:0.00}", totalFondo);
                            drDet["emprendedorTotal"] = "$ " + String.Format("{0:0.00}", totalEmprendedor);
                            totalFondo = 0;
                            totalEmprendedor = 0;
                            detalle.Rows.Add(drDet);
                        }
                        drDet = detalle.NewRow();
                        actividadActual = registro.CodActividad;
                    }

                    if (registro.CodTipoFinanciacion == 1)
                    {
                        drDet["fondo" + registro.Mes] = "$ " + String.Format("{0:0.00}", registro.Valor);
                        totalFondo += registro.Valor;
                    }
                    else if (registro.CodTipoFinanciacion == 2)
                    {
                        drDet["emprendedor" + registro.Mes] = "$ " + String.Format("{0:0.00}", registro.Valor);
                        totalEmprendedor += registro.Valor;
                    }
                }

                drDet["fondoTotal"] = "$ " + totalFondo.ToString("0,0.00", CultureInfo.InvariantCulture);
                drDet["emprendedorTotal"] = "$ " + totalEmprendedor.ToString("0,0.00", CultureInfo.InvariantCulture);
                detalle.Rows.Add(drDet);

                gw_Anexos.DataSource = datos;
                gw_Anexos.DataBind();

                gw_AnexosActividadA.DataSource = detalle;
                gw_AnexosActividadB.DataSource = detalle;
                gw_AnexosActividadC.DataSource = detalle;
                gw_AnexosActividadD.DataSource = detalle;
                gw_AnexosActividadA.DataBind();
                gw_AnexosActividadB.DataBind();
                gw_AnexosActividadC.DataBind();
                gw_AnexosActividadD.DataBind();
            }

            
        }
    }
}