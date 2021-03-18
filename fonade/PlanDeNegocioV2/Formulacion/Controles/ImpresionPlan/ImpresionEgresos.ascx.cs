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
    public partial class ImpresionEgresos : System.Web.UI.UserControl
    {
        public int CodigoProyecto { get; set; }

        public string TotalInvFijas { get; set; }

        public int txtTiempoProyeccion;

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
                CargarDatos();
                
            }
            catch (Exception ex)
            {
                Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
        }

        private void CargarDatos()
        {
            if (CodigoProyecto != 0)
            {
                CargarActualizacionMonetaria();
                CargarTiempoProyeccion();
                CargarCostosPuestaEnMarca();
                CargarCostosAnualizados();
                CargarGastosPersonales();
                CargarInversionesFijas();
            }
        }

        protected void CargarActualizacionMonetaria()
        {
            try
            {
                using (Datos.FonadeDBDataContext consultas = new Datos.FonadeDBDataContext(cadenaConexion))
                {
                    var query = (from p in consultas.ProyectoFinanzasEgresos
                                 where p.CodProyecto == Convert.ToInt32(CodigoProyecto)
                                 select new { p.ActualizacionMonetaria }).First();
                    lblActualizacionMonetaria.Text = query.ActualizacionMonetaria.ToString().Replace(',', '.');
                }
            }
            catch
            {
                lblActualizacionMonetaria.Text = "1";
            }
        }

        protected void CargarTiempoProyeccion()
        {
            try
            {
                using (Datos.FonadeDBDataContext consultas = new Datos.FonadeDBDataContext(cadenaConexion))
                {
                    var query = (from p in consultas.ProyectoMercadoProyeccionVentas
                                 where p.CodProyecto == Convert.ToInt32(CodigoProyecto)
                                 select new { p.TiempoProyeccion }).First();
                    txtTiempoProyeccion = Convert.ToInt32(query.TiempoProyeccion);
                }
            }
            catch
            {
                txtTiempoProyeccion = 3;
            }
        }

        protected void CargarInversionesFijas()
        {
            Consultas consultas = new Consultas();

            var txtSql2 = "select id_inversion,p.codProyecto, concepto, valor, semanas mes, AportadoPor tipoFuente,TipoInversion from ProyectoInversion p  " +
                "inner join tipoinfraestructura t on p.concepto=t.nomtipoinfraestructura right join ( select distinct(codtipoinfraestructura) as codtipo, " +
                "a.codproyecto from proyectoinfraestructura a ) b on p.codproyecto= " + CodigoProyecto + "  where t.id_tipoinfraestructura=b.codtipo and b.codproyecto = " + CodigoProyecto +
                " union select id_inversion,codProyecto ,concepto, valor, semanas mes, AportadoPor tipoFuente,TipoInversion " +
                "from ProyectoInversion where codproyecto = " + CodigoProyecto + " and tipoinversion='Diferida' order by TipoInversion desc";
            var datos = consultas.ObtenerDataTable(txtSql2, "text");


            var total2 = datos.AsEnumerable().Sum(row => row.Field<decimal>("valor"));
            var total = Convert.ToInt64(total2);

            TotalInvFijas = total2.ToString("$ 0,0.00", CultureInfo.InvariantCulture);

            gw_InversionesFijas.DataSource = datos;
            gw_InversionesFijas.DataBind();
        }

        private void CargarCostosPuestaEnMarca()
        {
            decimal total = 0;
            using (Datos.FonadeDBDataContext consultas = new Datos.FonadeDBDataContext(cadenaConexion))
            {

                var query = (from p in consultas.ProyectoGastos
                             where p.Tipo == "Arranque"
                             && p.CodProyecto == Convert.ToInt32(CodigoProyecto)
                             orderby p.Descripcion ascending
                             select new { p.Id_Gasto, p.Descripcion, p.Valor, p.Protegido });

                DataTable datos = new DataTable();
                datos.Columns.Add("Descripcion");
                datos.Columns.Add("Valor");

                foreach (var item in query)
                {
                    decimal valor = item.Valor;

                    DataRow dr = datos.NewRow();
                    dr["Descripcion"] = item.Descripcion;
                    dr["Valor"] = valor.ToString("$ 0,0.00", CultureInfo.InvariantCulture);
                    
                    total += valor;

                    datos.Rows.Add(dr);
                }

                DataRow drTotal = datos.NewRow();

                drTotal["Descripcion"] = "Total";
                drTotal["Valor"] = total.ToString("$ 0,0.00", CultureInfo.InvariantCulture);

                datos.Rows.Add(drTotal);

                gw_CostosPuestaMarcha.DataSource = datos;
                gw_CostosPuestaMarcha.DataBind();

               
                PintarFilasGrid(gw_CostosPuestaMarcha, 0, new string[] { "Total" });
            }
        }

        private void CargarCostosAnualizados()
        {
            using (Datos.FonadeDBDataContext consultas = new Datos.FonadeDBDataContext(cadenaConexion))
            {
                var query = (from p in consultas.ProyectoGastos
                             where p.Tipo == "Anual"
                             && p.CodProyecto == Convert.ToInt32(CodigoProyecto)
                             orderby p.Descripcion ascending
                             select new { p.Id_Gasto, p.Descripcion, p.Valor, p.Protegido });

                DataTable datos = new DataTable();
                datos.Columns.Add("Descripcion");
                for (int i = 1; i <= txtTiempoProyeccion; i++)
                {
                    datos.Columns.Add("Año " + i);
                }

                decimal[] total = new decimal[txtTiempoProyeccion + 1];
                foreach (var item in query)
                {
                    DataRow dr = datos.NewRow();

                    dr["Descripcion"] = item.Descripcion;

                    decimal valor = item.Valor;
                    for (int i = 1; i <= txtTiempoProyeccion; i++)
                    {
                        dr["Año " + i] = valor.ToString("$ 0,0.00", CultureInfo.InvariantCulture); ;
                        total[i] += valor;
                        valor = Convert.ToDecimal(lblActualizacionMonetaria.Text.Replace('.', ',')) * valor;
                    }
                    datos.Rows.Add(dr);
                }

                DataRow drTotal = datos.NewRow();
                drTotal["Descripcion"] = "Total";
                for (int i = 1; i <= txtTiempoProyeccion; i++)
                {
                    drTotal["Año " + i] = total[i].ToString("$ 0,0.00", CultureInfo.InvariantCulture);
                }
                datos.Rows.Add(drTotal);

                gw_CostosAnualizados.DataSource = datos;
                gw_CostosAnualizados.DataBind();

                
                PintarFilasGrid(gw_CostosAnualizados, 0, new string[] { "Total" });
            }
        }

        private void CargarGastosPersonales()
        {
            using (Datos.FonadeDBDataContext consultas = new Datos.FonadeDBDataContext(cadenaConexion))
            {
                var query = (from p in consultas.ProyectoGastosPersonals
                             where p.CodProyecto == Convert.ToInt32(CodigoProyecto)
                             orderby p.Cargo ascending
                             select new { p.Cargo, p.ValorAnual });

                DataTable datos = new DataTable();
                datos.Columns.Add("Cargo");
                for (int i = 1; i <= txtTiempoProyeccion; i++)
                {
                    datos.Columns.Add("Año " + i);
                }

                decimal[] total = new decimal[txtTiempoProyeccion + 1];
                foreach (var item in query)
                {
                    DataRow dr = datos.NewRow();

                    dr["Cargo"] = item.Cargo;

                    decimal valor = item.ValorAnual;
                    for (int i = 1; i <= txtTiempoProyeccion; i++)
                    {
                        dr["Año " + i] = valor.ToString("$ 0,0.00", CultureInfo.InvariantCulture); ;
                        total[i] += valor;
                        valor = Convert.ToDecimal(lblActualizacionMonetaria.Text.Replace('.', ',')) * valor;
                    }
                    datos.Rows.Add(dr);
                }

                DataRow drTotal = datos.NewRow();
                drTotal["Cargo"] = "Total";
                for (int i = 1; i <= txtTiempoProyeccion; i++)
                {
                    drTotal["Año " + i] = total[i].ToString("$ 0,0.00", CultureInfo.InvariantCulture);
                }
                datos.Rows.Add(drTotal);

                gw_GastosPersonales.DataSource = datos;
                gw_GastosPersonales.DataBind();

                //Ajustar la alineación de los registros que muestra la grilla.
                if (gw_GastosPersonales.Columns.Count > 0)
                { gw_GastosPersonales.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Right; }

                PintarFilasGrid(gw_GastosPersonales, 0, new string[] { "Total" });
            }
        }

        protected void PintarFilasGrid(GridView obj, int posicion, string[] texto)
        {
            for (int i = 0; i < obj.Rows.Count; i++)
            {
                if (texto.Any(ext => obj.Rows[i].Cells[posicion].Text.EndsWith(ext)))
                {
                    obj.Rows[i].Cells[posicion].Text = "<strong><b>" + obj.Rows[i].Cells[posicion].Text + "</b><strong>";
                }
            }
        }

        protected void gw_CostosPuestaMarcha_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.Cells[0].Text.Contains(Mensajes.GetMensaje(101)))
            {
                if (e.Row.Cells[1].Text.Contains("$"))
                {
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;
                }
            }
        }

        protected void gw_CostosAnualizados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int i = 1; i <= txtTiempoProyeccion; i++)
            {
                if (e.Row.Cells[i].Text.Contains("$"))
                {
                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right;
                }
            }

        }

        protected void gw_GastosPersonales_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int i = 1; i <= txtTiempoProyeccion; i++)
            {
                if (e.Row.Cells[i].Text.Contains("$"))
                {
                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right;
                }
            }
        }

    }
}