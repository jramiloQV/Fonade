using Datos;
using Fonade.Negocio;
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
    public partial class ImpresionIngresosEf : System.Web.UI.UserControl
    {
        public int CodigoProyecto { get; set; }

        public string TotalVlrAportesEmp { get; set; }

        public string TotalRecCapital { get; set; }

        public int txtTiempoProyeccion;
        int codigoConvocatoria;
        public string NumeroSMLVNV;

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
                    CargarTiempoProyeccion();
                    CargarNumeroSMLVNV();
                    CargarTxtRecursosSolicitados();
                    CargarRecursosCapital();
                    CargarAportesEmprendedores();
                    CargarIngresosVentas();
                }
            }
            catch (Exception ex)
            {
                Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
        }

        /// <summary>
        /// Cargar el tiempo de proyección
        /// </summary>
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

        /// <summary>
        /// Carga el numero de salarios minimos prestados dependiendo de el numero de empleos generados
        /// </summary>
        protected void CargarNumeroSMLVNV()
        {
            int numeroEmpleosNV = 0;
            codigoConvocatoria = 0;

            var qry = "select (select COUNT(*) from proyectoinsumo inner join ProyectoProductoInsumo on CodInsumo = Id_Insumo LEFT OUTER JOIN" + " proyectoempleomanoobra  on id_insumo=codmanoobra where codtipoinsumo=2 and codproyecto={0}) + " +
                  "(select COUNT(*) from proyectoempleocargo right OUTER JOIN proyectogastospersonal  on id_cargo=codcargo where codproyecto={0}) as Conteototal, " +
                  "(select COUNT(*) from proyectoinsumo  inner join ProyectoProductoInsumo  on CodInsumo = Id_Insumo LEFT OUTER JOIN proyectoempleomanoobra on id_insumo=codmanoobra" + " where codtipoinsumo=2 and codproyecto={0} and GeneradoPrimerAno  is not null and GeneradoPrimerAno!=0) + " +
                  "(select COUNT(*) from proyectoempleocargo right OUTER JOIN proyectogastospersonal on id_cargo=codcargo  where codproyecto={0} and GeneradoPrimerAno is not null" + " and GeneradoPrimerAno!=0) as ConteoAño";
            var xct = new Clases.genericQueries().executeQueryReader(string.Format(qry, CodigoProyecto));
            DataTable dtEmpTtl = new DataTable();
            dtEmpTtl.Load(xct, LoadOption.OverwriteChanges);
            var total = dtEmpTtl.Rows[0]["Conteototal"].ToString();

            numeroEmpleosNV = Convert.ToInt32(total);

            try
            {
                using (Datos.FonadeDBDataContext consultas = new Datos.FonadeDBDataContext(cadenaConexion))
                {
                    var queryConvoca = (from cp in consultas.ConvocatoriaProyectos
                                        where cp.CodProyecto == Convert.ToInt32(CodigoProyecto)
                                        select new { cp.CodConvocatoria }
                                            ).FirstOrDefault();
                    if (queryConvoca != null)
                        codigoConvocatoria = queryConvoca.CodConvocatoria;
                    else
                        codigoConvocatoria = 1;
                }
            }
            catch
            {
                throw;
            }

            ConsultarSalarioSMLVNV(1, numeroEmpleosNV, codigoConvocatoria);
            ConsultarSalarioSMLVNV(2, numeroEmpleosNV, codigoConvocatoria);
            ConsultarSalarioSMLVNV(3, numeroEmpleosNV, codigoConvocatoria);
            ConsultarSalarioSMLVNV(4, numeroEmpleosNV, codigoConvocatoria);
            ConsultarSalarioSMLVNV(5, numeroEmpleosNV, codigoConvocatoria);
            ConsultarSalarioSMLVNV(6, numeroEmpleosNV, codigoConvocatoria);

        }

        /// <summary>
        /// Consultar el numero de empleos generados
        /// </summary>
        /// <param name="regla"> Numero de regla de empleos generados </param>
        /// <param name="numeroEmpleosNV"> Numero de empleos generados </param>
        /// <param name="codigoConvocatoria"> Codigo de convocatoria </param>
        private void ConsultarSalarioSMLVNV(int regla, int numeroEmpleosNV, int codigoConvocatoria)
        {

            try
            {
                using (Datos.FonadeDBDataContext consultas = new Datos.FonadeDBDataContext(cadenaConexion))
                {
                    var queryRegla = (from p in consultas.ConvocatoriaReglaSalarios where p.NoRegla == regla && p.CodConvocatoria == codigoConvocatoria select p).FirstOrDefault();

                    if (queryRegla == null)
                        return;

                    int empv1 = queryRegla.EmpleosGenerados1;
                    int? empv11 = queryRegla.EmpleosGenerados2;
                    string lista1 = queryRegla.ExpresionLogica;
                    int Salmin1 = queryRegla.SalariosAPrestar;

                    switch (lista1)
                    {
                        case "=":
                            if (numeroEmpleosNV == empv1)
                                NumeroSMLVNV = Salmin1.ToString();
                            break;
                        case "<":
                            if (numeroEmpleosNV < empv1)
                                NumeroSMLVNV = Salmin1.ToString();
                            break;
                        case ">":
                            if (numeroEmpleosNV > empv1)
                                NumeroSMLVNV = Salmin1.ToString();

                            break;
                        case "<=":
                            if (numeroEmpleosNV <= empv1)
                                NumeroSMLVNV = Salmin1.ToString();
                            break;
                        case ">=":
                            if (numeroEmpleosNV >= empv1)
                                NumeroSMLVNV = Salmin1.ToString();
                            break;
                        case "Entre":
                            if (numeroEmpleosNV >= empv1 && numeroEmpleosNV <= empv11)
                                NumeroSMLVNV = Salmin1.ToString();
                            break;
                    }
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// Cargar los recursos solicitados del capital
        /// </summary>
        protected void CargarTxtRecursosSolicitados()
        {
            try
            {
                using (Datos.FonadeDBDataContext consultas = new Datos.FonadeDBDataContext(cadenaConexion))
                {

                    var query = (from p in consultas.ProyectoFinanzasIngresos
                                 where p.CodProyecto == Convert.ToInt32(CodigoProyecto)
                                 select new { p.Recursos }).First();
                    lblRecursosSolicitados.Text = query.Recursos.ToString();
                }
            }
            catch
            {
                lblRecursosSolicitados.Text = "";
            }

        }

        /// <summary>
        /// Cargar los recursos de capital
        /// </summary>
        protected void CargarRecursosCapital()
        {
            using (Datos.FonadeDBDataContext consultas = new Datos.FonadeDBDataContext(cadenaConexion))
            {
                var query = (from p in consultas.ProyectoRecursos where p.CodProyecto == Convert.ToInt32(this.CodigoProyecto) orderby p.Tipo ascending select new { p.Id_Recurso, p.Tipo, p.Cuantia, p.Plazo, p.Formapago, p.Interes, p.Destinacion });

                DataTable datos = new DataTable();
                datos.Columns.Add("aux");
                datos.Columns.Add("cuantia");
                datos.Columns.Add("plazo");
                datos.Columns.Add("formaPago");
                datos.Columns.Add("intereses");
                datos.Columns.Add("destinacion");
                datos.Columns.Add("Id_Recurso");

                string tipoActual = "";
                double total = 0;
                foreach (var item in query)
                {
                    if (tipoActual != item.Tipo)
                    {
                        DataRow drTitulo = datos.NewRow();
                        drTitulo["aux"] = item.Tipo;
                        tipoActual = item.Tipo;
                        datos.Rows.Add(drTitulo);
                    }

                    DataRow dr = datos.NewRow();
                    dr["cuantia"] = item.Cuantia.ToString("$ 0,0.00", CultureInfo.InvariantCulture);
                    dr["plazo"] = item.Plazo;
                    dr["formaPago"] = item.Formapago;
                    dr["intereses"] = item.Interes;
                    dr["destinacion"] = item.Destinacion;
                    dr["Id_Recurso"] = item.Id_Recurso;
                    total += Convert.ToDouble(item.Cuantia);
                    datos.Rows.Add(dr);

                }

                TotalRecCapital = total.ToString("$ 0,0.00", CultureInfo.InvariantCulture);

                gw_RecursosCapital.DataSource = datos;
                gw_RecursosCapital.DataBind();
            }
        }

        /// <summary>
        /// Cargar los aportes de emprendedores del plan de negocio
        /// </summary>
        protected void CargarAportesEmprendedores()
        {
            using (Datos.FonadeDBDataContext consultas = new Datos.FonadeDBDataContext(cadenaConexion))
            {
                var query = (from p in consultas.ProyectoAportes
                             where p.CodProyecto == Convert.ToInt32(this.CodigoProyecto)
                             orderby p.TipoAporte, p.Nombre ascending
                             select new { p.Id_Aporte, p.Nombre, p.Valor, p.TipoAporte, p.Detalle });

                DataTable datos = new DataTable();
                datos.Columns.Add("nombre");
                datos.Columns.Add("valor");
                datos.Columns.Add("detalle");
                datos.Columns.Add("");
                datos.Columns.Add("Id_Aporte");

                string aporteActual = "";
                double total = 0;
                foreach (var item in query)
                {
                    if (aporteActual != item.TipoAporte)
                    {
                        DataRow drTitulo = datos.NewRow();
                        drTitulo["valor"] = string.Empty;
                        drTitulo["nombre"] = item.TipoAporte;
                        aporteActual = item.TipoAporte;
                        datos.Rows.Add(drTitulo);
                    }

                    DataRow dr = datos.NewRow();
                    dr["nombre"] = item.Nombre;
                    dr["valor"] = item.Valor.ToString("$ 0,0.00", CultureInfo.InvariantCulture); ;
                    dr["detalle"] = item.Detalle;
                    dr["Id_Aporte"] = item.Id_Aporte;
                    total += Convert.ToDouble(item.Valor);
                    datos.Rows.Add(dr);
                }

                TotalVlrAportesEmp = total.ToString("$ 0,0.00", CultureInfo.InvariantCulture);

                gw_AporteEmprendedores.DataSource = datos;
                gw_AporteEmprendedores.DataBind();
            }
        }

        /// <summary>
        /// Cargar los datos de ingreso de ventas del plan de negocio
        /// </summary>
        private void CargarIngresosVentas()
        {
            String txtSQL;
            DataTable rsProducto = new DataTable();
            DataTable datos = new DataTable();
            Consultas consultas = new Consultas();
            Base_Page obj = new Base_Page();
            double[] totalPt = new double[txtTiempoProyeccion + 1];
            double[] totalIvaPt = new double[txtTiempoProyeccion + 1];
            int[] ivas = new int[txtTiempoProyeccion];
            string errorMessage = string.Empty;

            txtSQL = " select id_producto, nomproducto, porcentajeiva " +
                     " from proyectoproducto " +
                     " where codproyecto = " + CodigoProyecto;

            rsProducto = consultas.ObtenerDataTable(txtSQL, "text");

            datos.Columns.Add("Producto");

            //Dependiendo del tiempo de proyección 
            for (int i = 1; i <= txtTiempoProyeccion; i++)
            { 
                datos.Columns.Add("Año " + i);
            }

            foreach (DataRow row in rsProducto.Rows)
            {
                DataRow dr = datos.NewRow();
                dr["producto"] = row["nomproducto"].ToString();

                txtSQL = " select sum(unidades) as unidades, precio, sum(unidades)*precio as total, ano " +
                         " from proyectoproductounidadesventas u, proyectoproductoprecio p " +
                         " where p.codproducto=u.codproducto and periodo=ano and p.codproducto = " + row["id_Producto"].ToString() +
                         " group by ano,precio order by ano";

                var rsUnidades = consultas.ObtenerDataTable(txtSQL, "text");

                int incr = 1;
                foreach (DataRow row_unidades in rsUnidades.Rows)
                {
                    try
                    {
                        double unidades = Double.Parse(row_unidades["Unidades"].ToString());
                        double precio = Double.Parse(row_unidades["Precio"].ToString());
                        double total2 = double.Parse(row_unidades["total"].ToString());
                        if (incr == 0)
                        {
                            dr["Producto"] = (unidades * precio);
                        }
                        else
                        {
                            //dr["Año " + incr.ToString()] = (unidades * precio).ToString("0,0.00", CultureInfo.CreateSpecificCulture("es-CO"));
                            //dr["Año " + incr.ToString()] = (unidades * precio).ToString("0,0.00", CultureInfo.InvariantCulture);
                            dr["Año " + incr.ToString()] = total2.ToString("$ 0,0.00", CultureInfo.InvariantCulture);
                        }

                        double total = Double.Parse(row_unidades["total"].ToString());
                        double anio = Double.Parse(row_unidades["ano"].ToString());
                        double porcentajeIva = Double.Parse(row["porcentajeiva"].ToString());
                        totalPt[incr] += total;

                        totalIvaPt[incr] += (total * porcentajeIva / 100);

                        incr++;
                    }
                    catch (Exception ex) { errorMessage = ex.Message; }
                }

                datos.Rows.Add(dr);
            }

            DataRow drTotal = datos.NewRow();
            DataRow drTotalIva = datos.NewRow();
            DataRow drTotalMasIva = datos.NewRow();
            drTotal["Producto"] = "Total";
            drTotalIva["Producto"] = "Iva";
            drTotalMasIva["Producto"] = "Total con Iva";

            for (int i = 0; i <= txtTiempoProyeccion + 1; i++)
            {
                if (i != 0)
                {
                    if (i <= txtTiempoProyeccion)
                    {
                        //drTotal["Año " + i] = (totalPt[i] ).ToString("0,0.00", CultureInfo.CreateSpecificCulture("es-CO"));
                        //drTotalIva["Año " + i] = (totalIvaPt[i] ).ToString("N2", CultureInfo.CreateSpecificCulture("es-CO"));
                        //drTotalMasIva["Año " + i] = ((totalPt[i] + totalIvaPt[i]) ).ToString("0,0.00", CultureInfo.CreateSpecificCulture("es-CO"));
                        drTotal["Año " + i] = (totalPt[i]).ToString("$ 0,0.00", CultureInfo.InvariantCulture);
                        drTotalIva["Año " + i] = (totalIvaPt[i]).ToString("$ 0,0.00", CultureInfo.InvariantCulture);
                        drTotalMasIva["Año " + i] = ((totalPt[i] + totalIvaPt[i])).ToString("$ 0,0.00", CultureInfo.InvariantCulture);
                    }
                }
            }

            datos.Rows.Add(drTotal);
            datos.Rows.Add(drTotalIva);
            datos.Rows.Add(drTotalMasIva);

            //Cargar los datos en el grid de ingreso de ventas
            gw_IngresosVentas.DataSource = datos;
            gw_IngresosVentas.DataBind();

            PintarFilasGrid(gw_IngresosVentas, 0, new string[] { "Total", "Iva", "Total con Iva" });

            for (int i = (gw_IngresosVentas.Rows.Count - 3); i < gw_IngresosVentas.Rows.Count; i++)
            {
                gw_IngresosVentas.Rows[i].Font.Bold = true;
            }
        }

        protected void PintarFilasGrid(GridView obj, int posicion, string[] texto)
        {
            for (int i = 0; i < obj.Rows.Count; i++)
            {
                if (texto.Any(ext => obj.Rows[i].Cells[posicion].Text.EndsWith(ext)))
                {
                    obj.Rows[i].Cells[posicion].Text = "<label><b>" + obj.Rows[i].Cells[posicion].Text + "</b></label>";
                }
            }
        }

        protected void gw_IngresosVentas_RowDataBound(object sender, GridViewRowEventArgs e)
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