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
    public partial class ImpresionCostosAdmin : System.Web.UI.UserControl
    {
        public int CodigoProyecto { get; set; }

        public string TotalGastosPuestaMarca { get; set; }

        public string TotalGastosAnuales { get; set; }

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
                if (CodigoProyecto != 0)
                {
                    CargarGastosPuestaMarca();
                    CargarGastosAnuales();
                }
                else
                {
                    gw_GastosPuestaMarca.DataSource = new List<ProyectoGasto>() ;
                    gw_GastosAnuales.DataSource = new List<ProyectoGasto>();
                    gw_GastosAnuales.DataBind();
                    gw_GastosPuestaMarca.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
        }

        protected void CargarGastosPuestaMarca()
        {
            using (Datos.FonadeDBDataContext consultas = new Datos.FonadeDBDataContext(cadenaConexion))
            {
                var query = (from p in consultas.ProyectoGastos
                             where p.Tipo == "Arranque" &&
                             p.CodProyecto == Convert.ToInt32(this.CodigoProyecto)
                             orderby p.Descripcion ascending
                             select new { p.Id_Gasto, p.Descripcion, p.Valor, p.Protegido });

                decimal valores = 0;
                foreach (var result in query)
                {
                    valores += result.Valor;
                }

                var query2 = (from q1 in query
                              select new
                              {
                                  q1.Id_Gasto,
                                  q1.Descripcion,
                                  Valor = q1.Valor.ToString("0,0.00", CultureInfo.InvariantCulture),
                                  q1.Protegido
                              });

                TotalGastosPuestaMarca = valores.ToString("$ 0,0.00", CultureInfo.InvariantCulture);
                gw_GastosPuestaMarca.DataSource = query2;
                gw_GastosPuestaMarca.DataBind();
            }
        }

        protected void CargarGastosAnuales()
        {
            using (Datos.FonadeDBDataContext consultas = new Datos.FonadeDBDataContext(cadenaConexion))
            {

                var query = (from p in consultas.ProyectoGastos
                             where p.Tipo == "Anual" &&
                             p.CodProyecto == Convert.ToInt32(this.CodigoProyecto)
                             orderby p.Descripcion ascending
                             select new { p.Id_Gasto, p.Descripcion, p.Valor, p.Protegido });

                decimal valores = 0;
                foreach (var result in query)
                {
                    valores += result.Valor;
                }

                var query2 = (from q1 in query
                              select new
                              {
                                  q1.Id_Gasto,
                                  q1.Descripcion,
                                  Valor = q1.Valor.ToString("0,0.00", CultureInfo.InvariantCulture),
                                  q1.Protegido
                              });

                TotalGastosAnuales = valores.ToString("$ 0,0.00", CultureInfo.InvariantCulture);
                gw_GastosAnuales.DataSource = query2;
                gw_GastosAnuales.DataBind();
            }
            
            
        }
    }
}