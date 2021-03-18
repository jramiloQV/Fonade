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
    public partial class ImpresionCapitalTrabajo : System.Web.UI.UserControl
    {
        public int CodigoProyecto { get; set; }

        public string TotalCapital { get; set; }


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
                    CargarGridCapitalTrabajo();
                }
            }
            catch (Exception ex)
            {
                Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
        }

        protected void CargarGridCapitalTrabajo()
        {
            using (Datos.FonadeDBDataContext consultas = new Datos.FonadeDBDataContext(cadenaConexion))
            {
                var query = (from p in consultas.ProyectoCapitals
                             join fuente in consultas.FuenteFinanciacions on p.codFuenteFinanciacion equals fuente.IdFuente
                             where p.CodProyecto == CodigoProyecto
                             orderby p.Componente
                             select new { p.Id_Capital, p.Componente, p.Valor, p.Observacion, fuente.DescFuente });

                DataTable datos = new DataTable();
                datos.Columns.Add("CodProyecto");
                datos.Columns.Add("id_Capital");
                datos.Columns.Add("componente");
                datos.Columns.Add("valor");
                datos.Columns.Add("FuenteFinanciacion");
                datos.Columns.Add("observacion");

                double total = 0;
                foreach (var item in query)
                {

                    DataRow dr = datos.NewRow();
                    dr["CodProyecto"] = CodigoProyecto;
                    dr["id_Capital"] = item.Id_Capital;
                    dr["componente"] = item.Componente;
                    dr["valor"] = item.Valor.ToString("$ 0,0.00", CultureInfo.InvariantCulture);
                    dr["FuenteFinanciacion"] = item.DescFuente;
                    dr["observacion"] = item.Observacion;
                    total += Convert.ToDouble(item.Valor);
                    datos.Rows.Add(dr);
                }

                TotalCapital = total.ToString("$ 0,0.00", CultureInfo.InvariantCulture);

                gw_CapitalTrabajo.DataSource = datos;
                gw_CapitalTrabajo.DataBind();
            }
        }
    }
}