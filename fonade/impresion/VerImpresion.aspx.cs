using Datos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Clases;

namespace Fonade.impresion
{
    public partial class VerImpresion : Negocio.Base_Page
    {
        TreeView listaTap;
        string codProye;

        string txtSQL;

        protected void Page_Load(object sender, EventArgs e)
        {
            listaTap = HttpContext.Current.Session["listaTap"] != null ? ((TreeView)Session["listaTap"]) : null;
            codProye = HttpContext.Current.Session["codProye"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["codProye"].ToString()) ? HttpContext.Current.Session["codProye"].ToString() : "0";

            if (codProye != null && !codProye.Equals("0"))
            {
                txtSQL = "SELECT nomproyecto FROM proyecto WHERE id_proyecto = " + codProye;

                var dt = consultas.ObtenerDataTable(txtSQL, "text");

                if (dt.Rows.Count > 0)
                    lblimpresion.Text = dt.Rows[0][0].ToString();

                infoProyecto(Convert.ToInt32(codProye));

                Tab();
            }
        }

        private void infoProyecto(int CodProyecto)
        {
            var result = (from p in consultas.Db.MD_InformacionProyecto(CodProyecto)
                          select p).FirstOrDefault();

            int cuantos = (from p in consultas.Db.MD_InformacionProyecto(CodProyecto)
                           select p).Count();

            if (cuantos <= 0)
            {
                pnlinfoproyecto.Visible = false;
                pnlinfoproyecto.Enabled = false;
            }
            else
            {
                txtSQL = "SELECT SalarioMinimo FROM SalariosMinimos where AñoSalario = " + DateTime.Now.Year;

                var dt = consultas.ObtenerDataTable(txtSQL, "text");

                lblnombreproyecto.Text = result.NomProyecto;
                lblinstitucionproyecto.Text = result.nomunidad + " (" + result.nomInstitucion + ")";
                lblsubsectorproyecto.Text = result.nomSubsector;
                lblciudadproyecto.Text = result.Lugar;

                if (dt.Rows.Count > 0)
                    lblrecursosproyecto.Text = "$" + (result.Recursos * Convert.ToDouble(dt.Rows[0][0].ToString())).ToString();

                lblfechacreacionproyecto.Text = result.FechaCreacion.ToString();
                lblsumarioproyecto.Text = result.Sumario.ToString();
            }
        }

        private void Tab()
        {
            foreach (TreeNode nodo in listaTap.Nodes)
            {
                if (nodo.Checked)
                {
                    validarNodo(nodo);
                }

                foreach (TreeNode nodo2 in nodo.ChildNodes)
                {
                    if (nodo2.Checked)
                    {
                        validarNodo(nodo2);
                    }
                }
            }
        }

        private void validarNodo(TreeNode nodo)
        {
            txtSQL = "SELECT  isnull(codtab, id_tab) orden, id_tab, nomTab FROM Tab WHERE id_tab = " + nodo.Value + " ORDER BY orden, id_tab";
            var dt = consultas.ObtenerDataTable(txtSQL, "text");

            if (dt.Rows.Count > 0)
            {
                TableHeaderRow filaTitulo = new TableHeaderRow();

                if (dt.Rows[0]["orden"].ToString().Equals(dt.Rows[0]["id_tab"].ToString()))
                    filaTitulo.Cells.Add(crearCeldaTitulo(dt.Rows[0]["nomTab"].ToString(), 2, 1, ""));
                else
                    filaTitulo.Cells.Add(crearCeldaTitulo(dt.Rows[0]["nomTab"].ToString(), 1, 1, ""));

                tbltab.Rows.Add(filaTitulo);

                prImprimeTab(Convert.ToInt32(dt.Rows[0]["id_tab"].ToString()), Convert.ToInt32(codProye));
            }
        }

        private void prImprimeTab(int codTab, int codProyecto)
        {
            string txtTabla = string.Empty;
            string txtCampos = string.Empty;
            string[] txtTitulos = null;

            bool bolRegular = true;

            switch (codTab)
            {
                case Constantes.CONST_Impacto:
                    txtTabla = "ProyectoImpacto";
                    txtCampos = "Impacto";
                    txtTitulos = "Impacto Econ&oacute;mico, Regional, Social, Ambiental".Split('|');
                    break;
                case Constantes.CONST_InvestigacionMercados:
                    txtTabla = "ProyectoMercadoInvestigacion";
                    txtCampos = "Objetivos,Justificacion,AnalisisSector, AnalisisMercado, AnalisisCompetencia";
                    txtTitulos = "DEFINICIÓN DE OBJETIVOS|JUSTIFICACIÓN Y ANTECEDENTES DEL PROYECTO|AN&Aacute;LISIS DE SECTOR|AN&Aacute;LISIS DE MERCADO|AN&Aacute;LISIS DE COMPETENCIA".Split('|');
                    break;
                case Constantes.CONST_EstrategiasMercado:
                    txtTabla = "ProyectoMercadoEstrategias";
                    txtCampos = "ConceptoProducto, EstrategiasDistribucion, EstrategiasPrecio, EstrategiasPromocion, EstrategiasComunicacion, EstrategiasServicio, PresupuestoMercado";
                    txtTitulos = "Concepto del Producto o Servicio|Estrategias de Distribuci&oacute;n|Estrategias de Precio|Estrategías de Promoción|Estrategías de Comunicaci&oacute;n|Estrategias de Servicio|Presupuesto de la Mezcla de Mercadeo".Split('|');
                    break;
                case Constantes.CONST_SubOperacion:
                    txtTabla = "ProyectoOperacion";
                    txtCampos = "FichaProducto, EstadoDesarrollo, DescripcionProceso, Necesidades, PlanProduccion";
                    txtTitulos = "Ficha T&eacute;cnica del Producto o Servicio|Estado de Desarrollo|Descripci&oacute;n del Proceso|Necesidades y Requerimientos|Plan de Producci&oacute;n|Plan de Compras".Split('|');
                    break;
                case Constantes.CONST_EstrategiaOrganizacional:
                    txtTabla = "ProyectoOrganizacionEstrategia";
                    txtCampos = "AnalisisDOFA, OrganismosApoyo ";
                    txtTitulos = "An&aacute;lisis DOFA|Organismos de Apoyo ".Split('|');
                    break;
                case Constantes.CONST_EstructuraOrganizacional:
                    txtTabla = "ProyectoOrganizacionEstructura";
                    txtCampos = "EstructuraOrganizacional";
                    txtTitulos = "Estructura Organizacional".Split('|');
                    break;
                case Constantes.CONST_AspectosLegales:
                    txtTabla = "ProyectoOrganizacionLegal";
                    txtCampos = "AspectosLegales";
                    txtTitulos = "Constituci&oacute;n Empresa y Aspectos Legales".Split('|');
                    break;
                case Constantes.CONST_SubResumenEjecutivo:
                    txtTabla = "ProyectoResumenEjecutivo";
                    txtCampos = "ConceptoNegocio, PotencialMercados, VentajasCompetitivas, ResumenInversiones, Proyecciones, ConclusionesFinancieras";
                    txtTitulos = "Concepto del Negocio|Potencial del Mercado en Cifras|Ventajas Competitivas y Propuesta de Valor|Resumen de las Inversiones Requeridas|Proyecciones de Ventas y Rentabilidad|Conclusiones Financieras y Evaluación de Viabilidad".Split('|');
                    break;
                case Constantes.CONST_ProyeccionesVentas: bolRegular = false; break;
                case Constantes.CONST_CostosInsumos: bolRegular = false; break;
                case Constantes.CONST_Presupuestos: bolRegular = false; break;
                case Constantes.CONST_EquipoTrabajo: bolRegular = false; break;
                case Constantes.CONST_Ingresos: bolRegular = false; break;
                case Constantes.CONST_Egresos: bolRegular = false; break;
                case Constantes.CONST_CapitalTrabajo: bolRegular = false; break;
                case Constantes.CONST_Compras: bolRegular = false; break;
                case Constantes.CONST_Infraestructura: bolRegular = false; break;
            }

            if (!string.IsNullOrEmpty(txtTabla) && bolRegular)
            {
                txtSQL = "SELECT " + txtCampos + " FROM " + txtTabla + " WHERE codProyecto = " + codProyecto;

                var dt = consultas.ObtenerDataTable(txtSQL, "text");

                if (dt.Rows.Count > 0)
                {
                    int i = 0;
                    foreach (DataColumn dc in dt.Columns)
                    {
                        try
                        {
                            TableRow filaTitulo = new TableRow();

                            filaTitulo.Cells.Add(crearCeldaTexto(txtTitulos[i], 1, 1, ""));
                            tbltab.Rows.Add(filaTitulo);

                            filaTitulo = new TableRow();

                            filaTitulo.Cells.Add(crearCeldaTexto(dt.Rows[0][dc].ToString(), 1, 1, ""));
                            tbltab.Rows.Add(filaTitulo);
                        }
                        catch (ArgumentOutOfRangeException) { }

                        i++;
                    }
                }
            }
            else
            {
                if (!bolRegular)
                {
                    switch (codTab)
                    {
                        case Constantes.CONST_ProyeccionesVentas:
                            prImprimeProyVentas(codProyecto);
                            break;
                        case Constantes.CONST_Compras:
                            prImprimeCompras(codProyecto);
                            break;
                        case Constantes.CONST_Presupuestos:
                            prImprimePresupuesto(codProyecto);
                            break;
                        case Constantes.CONST_Ingresos:
                            spImprimeIngreso(codProyecto);
                            break;
                        case Constantes.CONST_Egresos:
                            spImprimeEgreso(codProyecto);
                            break;
                        case Constantes.CONST_CapitalTrabajo:
                            spImprimeCapital(codProyecto);
                            break;
                        case Constantes.CONST_EquipoTrabajo:
                            prImprimirEquipo(codProyecto);
                            break;
                        case Constantes.CONST_Infraestructura:
                            prImprimeInfraestructura(codProyecto);
                            break;
                        case Constantes.CONST_CostosInsumos:
                            prImprimeCostosProduccion(codProyecto);
                            break;
                    }
                }
            }
        }

        private TableHeaderCell crearCeldaTitulo(string mensaje, int colspan, int rowspan, string cssestilo)
        {
            TableHeaderCell celda1 = new TableHeaderCell()
            {
                ColumnSpan = colspan,
                RowSpan = rowspan,
                CssClass = cssestilo,
                Text = mensaje
            };

            return celda1;
        }

        private TableCell crearCeldaTexto(string mensaje, int colspan, int rowspan, string cssestilo)
        {
            TableCell celda1 = new TableCell()
            {
                ColumnSpan = colspan,
                RowSpan = rowspan,
                CssClass = cssestilo,
                Text = mensaje.htmlDecode()
            };

            return celda1;
        }

        private TableCell crearCeldaTexto(Control mensaje, int colspan, int rowspan, string cssestilo)
        {
            TableCell celda1 = new TableCell()
            {
                ColumnSpan = colspan,
                RowSpan = rowspan,
                CssClass = cssestilo
            };

            celda1.Controls.Add(mensaje);

            return celda1;
        }

        private void prImprimeProyVentas(int codProyecto)
        {
            int numTiempo;
            int numFactor = 0;
            string txtPeriodo = string.Empty;
            string txtSelect = string.Empty;

            double[] TotalPesos = new double[12];
            double[] TotalIva = new double[12];

            //string txtTotal = string.Empty;
            //string txtPrecio = string.Empty;
            //string txtTotalPrecio = string.Empty;

            txtSQL = "SELECT * FROM ProyectoMercadoProyeccionVentas WHERE codproyecto = " + codProyecto;
            var dt = consultas.ObtenerDataTable(txtSQL, "text");

            Table tablaVentas = new Table();

            TableRow fila = new TableRow();

            fila.Cells.Add(crearCeldaTexto("Arranque de la empresa :", 1, 1, ""));

            numTiempo = 0;
            if (dt.Rows.Count > 0)
            {
                numTiempo = Convert.ToInt32(dt.Rows[0]["TiempoProyeccion"].ToString());

                fila.Cells.Add(crearCeldaTexto(Convert.ToDateTime(dt.Rows[0]["FechaArranque"].ToString()).ToString(), 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto("Tamaño del Período :", 1, 1, ""));

                switch (Convert.ToInt32(dt.Rows[0]["codPeriodo"].ToString()))
                {
                    case Constantes.CONST_Semestre:
                        fila.Cells.Add(crearCeldaTexto("Semestre", 1, 1, ""));
                        numFactor = 6;
                        txtPeriodo = "Semestre";
                        break;
                    case Constantes.CONST_Trimestre:
                        fila.Cells.Add(crearCeldaTexto("Trimestre", 1, 1, ""));
                        numFactor = 3;
                        txtPeriodo = "Trimestre";
                        break;
                    case Constantes.CONST_Bimestre:
                        fila.Cells.Add(crearCeldaTexto("Bimestre", 1, 1, ""));
                        numFactor = 2;
                        txtPeriodo = "Bimestre";
                        break;
                    default:
                        fila.Cells.Add(crearCeldaTexto("Mensual", 1, 1, ""));
                        numFactor = 1;
                        txtPeriodo = "Mes";
                        break;
                }

                fila.Cells.Add(crearCeldaTexto("Tiempo Proyectado :", 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(dt.Rows[0]["TiempoProyeccion"].ToString(), 1, 1, ""));

                tablaVentas.Rows.Add(fila);

                fila = new TableRow();

                fila.Cells.Add(crearCeldaTexto("Método de proyección :", 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(dt.Rows[0]["MetodoProyeccion"].ToString(), 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto("Costo de venta :", 2, 1, ""));
                fila.Cells.Add(crearCeldaTexto(dt.Rows[0]["costoVenta"].ToString(), 2, 1, ""));

                tablaVentas.Rows.Add(fila);

                fila = new TableRow();

                fila.Cells.Add(crearCeldaTexto("Polótica de Cartera", 6, 1, ""));

                tablaVentas.Rows.Add(fila);

                fila = new TableRow();

                fila.Cells.Add(crearCeldaTexto(dt.Rows[0]["PoliticaCartera"].ToString(), 6, 1, ""));

                tablaVentas.Rows.Add(fila);
            }

            TableRow filaTitulo = new TableRow();
            filaTitulo.Cells.Add(crearCeldaTexto(tablaVentas, 1, 1, ""));
            tbltab.Rows.Add(filaTitulo);

            Table tablaproductos = new Table();

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("Productos", 7, 1, ""));
            tablaproductos.Rows.Add(fila);

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("Producto", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Posicion Arancelaria", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("RTF", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("IVA", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Precio Inicial (miles)", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("%Contado", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("%Crédito", 1, 1, ""));
            tablaproductos.Rows.Add(fila);

            txtSQL = "SELECT  * FROM  proyectoproducto WHERE codproyecto=" + codProyecto;
            dt = consultas.ObtenerDataTable(txtSQL, "text");

            foreach (DataRow dr in dt.Rows)
            {
                fila = new TableRow();
                fila.Cells.Add(crearCeldaTexto(dr["nomproducto"].ToString(), 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(dr["PosicionArancelaria"].ToString(), 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(dr["PorcentajeRetencion"].ToString(), 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(dr["PorcentajeIva"].ToString(), 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(string.Format((Convert.ToDouble("0" + dr["PrecioLanzamiento"].ToString()) / 1000).ToString(), 2), 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(dr["PorcentajeVentasContado"].ToString(), 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(dr["PorcentajeVentasPlazo"].ToString(), 1, 1, ""));
                tablaproductos.Rows.Add(fila);

                Table tablaproductosunidaes = new Table();

                fila = new TableRow();
                fila.Cells.Add(crearCeldaTexto("Proyección de Ventas (unidades)", numTiempo + 1, 1, ""));
                tablaproductosunidaes.Rows.Add(fila);

                fila = new TableRow();
                fila.Cells.Add(crearCeldaTexto("Periodo", 1, 1, ""));

                for (int i = 0; i < numTiempo; i++)
                {
                    fila.Cells.Add(crearCeldaTexto("Año " + (i + 1), 1, 1, ""));
                }

                tablaproductosunidaes.Rows.Add(fila);

                txtSelect = "";
                for (int i = 0; i < numTiempo; i++)
                {
                    txtSelect = txtSelect + ", sum(case when ano=" + (i + 1) + " then unidades else 0 end) as Ano" + (i + 1);
                }

                txtSQL = "SELECT mes" + txtSelect + " FROM proyectoproductounidadesventas U WHERE mes%" + numFactor + "=0 AND codProducto = " + dr["id_Producto"].ToString() + " GROUP BY mes ORDER BY mes";
                var dt1 = consultas.ObtenerDataTable(txtSQL, "text");

                foreach (DataRow dr1 in dt1.Rows)
                {
                    fila = new TableRow();
                    fila.Cells.Add(crearCeldaTexto(txtPeriodo + ((Convert.ToInt32(dr1["mes"].ToString())) / numFactor), 1, 1, ""));

                    for (int i = 0; i < numTiempo; i++)
                    {
                        fila.Cells.Add(crearCeldaTexto(dr1["Ano" + (i + 1)].ToString(), 1, 1, ""));
                    }

                    tablaproductosunidaes.Rows.Add(fila);
                }

                txtSQL = "SELECT ano, Precio, sum(unidades) Unidades " +
                    "FROM proyectoproductounidadesventas U, ProyectoProductoPrecio P  " +
                    "WHERE U.ano=P.periodo and U.codProducto=P.codProducto and ano between 1 and " + numTiempo + " and P.codProducto = " + dr["id_Producto"].ToString() + " " +
                    "GROUP BY ano, Precio " +
                    "ORDER BY ano";

                dt1 = consultas.ObtenerDataTable(txtSQL, "text");

                //txtTotal = "";
                //txtPrecio = "";
                //txtTotalPrecio = "";

                fila = new TableRow();
                fila.Cells.Add(crearCeldaTexto("Total", 1, 1, ""));

                foreach (DataRow dr1 in dt1.Rows)
                {
                    fila.Cells.Add(crearCeldaTexto(dr1["Unidades"].ToString().Trim(), 1, 1, ""));
                }

                tablaproductosunidaes.Rows.Add(fila);

                fila = new TableRow();
                fila.Cells.Add(crearCeldaTexto("Precio", 1, 1, ""));

                foreach (DataRow dr1 in dt1.Rows)
                {
                    fila.Cells.Add(crearCeldaTexto(dr1["Precio"].ToString().Trim(), 1, 1, ""));
                }

                tablaproductosunidaes.Rows.Add(fila);

                fila = new TableRow();
                fila.Cells.Add(crearCeldaTexto("Ventas Esperadas", 1, 1, ""));

                foreach (DataRow dr1 in dt1.Rows)
                {
                    fila.Cells.Add(crearCeldaTexto((Convert.ToInt32(dr1["Unidades"]) * Convert.ToDouble(dr1["Precio"])).ToString(), 1, 1, ""));
                }

                tablaproductosunidaes.Rows.Add(fila);

                filaTitulo = new TableRow();
                filaTitulo.Cells.Add(crearCeldaTexto(tablaproductosunidaes, 1, 1, ""));
                tablaproductos.Rows.Add(filaTitulo);
            }

            filaTitulo = new TableRow();
            filaTitulo.Cells.Add(crearCeldaTexto(tablaproductos, 1, 1, ""));
            tbltab.Rows.Add(filaTitulo);

            Table tablaproductosproyeccion = new Table();
            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("Proyecci&oacute;n de ventas (unidades)", numTiempo + 1, 1, ""));
            tablaproductosproyeccion.Rows.Add(fila);

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("Producto", 1, 1, ""));

            for (int i = 0; i < numTiempo; i++)
            {
                fila.Cells.Add(crearCeldaTexto("Año " + (i + 1), 1, 1, ""));
            }

            tablaproductosproyeccion.Rows.Add(fila);

            txtSQL = "SELECT nomProducto, ano, Sum(unidades) TotalU, sum(Precio*Unidades) TotalP, Sum(Precio*Unidades*porcentajeIVA/100) TotalI " +
                 "FROM ProyectoProducto P, proyectoproductounidadesventas U, ProyectoProductoPrecio R " +
                 "WHERE	id_Producto = U.codProducto and	U.codProducto = R.codProducto and U.Ano=R.periodo and P. codProyecto=" + codProyecto + " and ano <= " + numTiempo + " " +
                 "GROUP BY nomProducto, ano " +
                 "ORDER BY nomProducto, ano";

            dt = consultas.ObtenerDataTable(txtSQL, "text");

            string nombreAux = string.Empty;

            foreach (DataRow dr in dt.Rows)
            {
                if (!dr["NomProducto"].ToString().Equals(nombreAux))
                {
                    nombreAux = dr["NomProducto"].ToString();
                    fila = new TableRow();
                    fila.Cells.Add(crearCeldaTexto(dr["NomProducto"].ToString(), 1, 1, ""));

                    for (int i = 0; i < numTiempo; i++)
                    {
                        fila.Cells.Add(crearCeldaTexto(dr["TotalU"].ToString(), 1, 1, ""));
                    }

                    tablaproductosproyeccion.Rows.Add(fila);
                }
            }

            filaTitulo = new TableRow();
            filaTitulo.Cells.Add(crearCeldaTexto(tablaproductosproyeccion, 1, 1, ""));
            tbltab.Rows.Add(filaTitulo);

            tablaproductosproyeccion = new Table();
            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("Proyecci&oacute;n de ingesos por ventas (miles de pesos)", numTiempo + 1, 1, ""));
            tablaproductosproyeccion.Rows.Add(fila);

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("Producto", 1, 1, ""));

            for (int i = 0; i < numTiempo; i++)
            {
                TotalPesos[i] = 0;
                TotalIva[i] = 0;
                fila.Cells.Add(crearCeldaTexto("Año " + (i + 1), 1, 1, ""));
            }

            tablaproductosproyeccion.Rows.Add(fila);

            nombreAux = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                if (!dr["NomProducto"].ToString().Equals(nombreAux))
                {
                    nombreAux = dr["NomProducto"].ToString();
                    fila = new TableRow();
                    fila.Cells.Add(crearCeldaTexto(dr["NomProducto"].ToString(), 1, 1, ""));

                    for (int i = 0; i < numTiempo; i++)
                    {
                        TotalPesos[i] = TotalPesos[i] + Convert.ToDouble(dr["TotalP"].ToString());
                        TotalIva[i] = TotalIva[i] + Convert.ToDouble(dr["TotalI"].ToString());
                        fila.Cells.Add(crearCeldaTexto(dr["TotalP"].ToString(), 1, 1, ""));
                    }

                    tablaproductosproyeccion.Rows.Add(fila);
                }
            }

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("Total", 1, 1, ""));

            for (int i = 0; i < numTiempo; i++)
            {
                fila.Cells.Add(crearCeldaTexto(TotalPesos[(i)].ToString(), 1, 1, ""));
            }

            tablaproductosproyeccion.Rows.Add(fila);

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("IVA", 1, 1, ""));

            for (int i = 0; i < numTiempo; i++)
            {
                fila.Cells.Add(crearCeldaTexto(TotalIva[(i)].ToString(), 1, 1, ""));
            }

            tablaproductosproyeccion.Rows.Add(fila);

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("Total con IVA", 1, 1, ""));

            for (int i = 0; i < numTiempo; i++)
            {
                fila.Cells.Add(crearCeldaTexto((TotalIva[(i)] + TotalPesos[(i)]).ToString(), 1, 1, ""));
            }

            tablaproductosproyeccion.Rows.Add(fila);

            filaTitulo = new TableRow();
            filaTitulo.Cells.Add(crearCeldaTexto(tablaproductosproyeccion, 1, 1, ""));
            tbltab.Rows.Add(filaTitulo);
        }

        private void prImprimeCompras(int codProyecto)
        {

            TableRow filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto("Consumos Unitarios por Producto:",1,1,""));
            tbltab.Rows.Add(filatitulo);

            txtSQL = "select id_producto,nomproducto from proyectoproducto where codproyecto =" + codProyecto + " order by nomproducto";

            var dt = consultas.ObtenerDataTable(txtSQL, "text");

            foreach (DataRow dr in dt.Rows)
            {
                Table tabla = new Table();

                TableRow fila = new TableRow();

                fila.Cells.Add(crearCeldaTexto(dr["nomproducto"].ToString(), 6, 1, ""));
                tabla.Rows.Add(fila);

                fila = new TableRow();
                fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto("Materia Prima, Insumo o Requerimiento", 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto("Unidad", 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto("Presentación", 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto("Cantidad x Unidad", 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto("Margen de Desperdicio %", 1, 1, ""));
                tabla.Rows.Add(fila);

                txtSQL = "SELECT  nomInsumo, unidad, p.Presentacion, Cantidad, Desperdicio FROM ProyectoProductoInsumo P, ProyectoInsumo I WHERE p.CodInsumo=I.id_insumo and codproducto =" + dr["id_producto"].ToString() + " order by nominsumo";

                var dt1 = consultas.ObtenerDataTable(txtSQL, "text");

                foreach (DataRow dr1 in dt1.Rows)
                {
                    fila = new TableRow();
                    fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
                    fila.Cells.Add(crearCeldaTexto(dr1["nominsumo"].ToString(), 1, 1, ""));
                    fila.Cells.Add(crearCeldaTexto(dr1["unidad"].ToString(), 1, 1, ""));
                    fila.Cells.Add(crearCeldaTexto(dr1["Presentacion"].ToString(), 1, 1, ""));
                    fila.Cells.Add(crearCeldaTexto(dr1["Cantidad"].ToString(), 1, 1, ""));
                    fila.Cells.Add(crearCeldaTexto(dr1["Desperdicio"].ToString(), 1, 1, ""));
                    tabla.Rows.Add(fila);
                }

                TableRow filaTitulo = new TableRow();
                filaTitulo.Cells.Add(crearCeldaTexto(tabla, 1, 1, ""));
                tbltab.Rows.Add(filaTitulo);
            }
        }

        private void prImprimePresupuesto(int codProyecto)
        {
            TableRow filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto("Gastos de Personal:", 1, 1, ""));
            tbltab.Rows.Add(filatitulo);

            Table tabla = new Table();

            TableRow fila = new TableRow();

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Cargo", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Dedicación", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Tipo de Contratación", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Valor Mensual (miles)", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Valor Anual (miles)", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Otros Gastos (miles)", 1, 1, ""));
            tabla.Rows.Add(fila);

            txtSQL = "select id_cargo, cargo, dedicacion, tipocontratacion, valormensual, valoranual, otrosgastos from proyectogastospersonal where codproyecto =" + codProyecto + " order by cargo";

            var dt = consultas.ObtenerDataTable(txtSQL, "text");

            double TotalMensual = 0;
            double TotalAnual = 0;

            foreach (DataRow dr in dt.Rows)
            {
                fila = new TableRow();
                fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(dr["Cargo"].ToString(), 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(dr["Dedicacion"].ToString(), 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(dr["TipoContratacion"].ToString(), 1, 1, ""));

                fila.Cells.Add(crearCeldaTexto(string.Format((Convert.ToDouble(dr["ValorMensual"].ToString()) / 1000).ToString(), 2), 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(string.Format((Convert.ToDouble(dr["ValorAnual"].ToString()) / 1000).ToString(), 2), 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(string.Format((Convert.ToDouble(dr["OtrosGastos"].ToString()) / 1000).ToString(), 2), 1, 1, ""));
                tabla.Rows.Add(fila);

                TotalMensual = TotalMensual + Convert.ToDouble(dr["ValorMensual"].ToString());
                TotalAnual = TotalAnual + Convert.ToDouble(dr["ValorAnual"].ToString());
            }

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Total", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto(string.Format(TotalMensual.ToString(), 2), 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto(string.Format(TotalAnual.ToString(), 2), 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            tabla.Rows.Add(fila);

            filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto(tabla, 1, 1, ""));
            tbltab.Rows.Add(filatitulo);

            filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto("Gastos de Arranque:", 1, 1, ""));
            tbltab.Rows.Add(filatitulo);

            tabla = new Table();

            fila = new TableRow();

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Descripción", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Valor (miles)", 1, 1, ""));
            tabla.Rows.Add(fila);

            txtSQL = "select id_gasto, descripcion, valor, protegido from proyectogastos where tipo='Arranque' and codproyecto =" + codProyecto + " order by descripcion";

            dt = consultas.ObtenerDataTable(txtSQL, "text");

            double Total = 0;

            foreach (DataRow dr in dt.Rows)
            {
                fila = new TableRow();
                fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(dr["Descripcion"].ToString(), 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(string.Format((Convert.ToDouble(dr["Valor"].ToString()) / 1000).ToString(), 2), 1, 1, ""));
                tabla.Rows.Add(fila);

                Total = Total + Convert.ToDouble(dr["Valor"].ToString());
            }

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Total", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto(string.Format(Total.ToString(), 2), 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            tabla.Rows.Add(fila);

            filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto(tabla, 1, 1, ""));
            tbltab.Rows.Add(filatitulo);

            filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto("Gastos Anuales de Administraci&oacute;n:", 1, 1, ""));
            tbltab.Rows.Add(filatitulo);

            tabla = new Table();

            fila = new TableRow();

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Descripción", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Valor (miles)", 1, 1, ""));
            tabla.Rows.Add(fila);

            txtSQL = "select id_gasto, descripcion, valor, protegido from proyectogastos where tipo='Anual' and codproyecto =" + codProyecto + " order by descripcion";

            dt = consultas.ObtenerDataTable(txtSQL, "text");

            Total = 0;

            foreach (DataRow dr in dt.Rows)
            {
                fila = new TableRow();
                fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(dr["Descripcion"].ToString(), 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(string.Format((Convert.ToDouble(dr["Valor"].ToString()) / 1000).ToString(), 2), 1, 1, ""));
                tabla.Rows.Add(fila);

                Total = Total + Convert.ToDouble(dr["Valor"].ToString());
            }

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Total", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto(string.Format(Total.ToString(), 2), 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            tabla.Rows.Add(fila);

            filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto(tabla, 1, 1, ""));
            tbltab.Rows.Add(filatitulo);
        }

        private void spImprimeIngreso(int codProyecto)
        {
            double txttiempoProyeccion;
            double Total = 0;

            double[] TotalPesos = new double[12];
            double[] TotalIva = new double[12];

            txtSQL = "select tiempoproyeccion from proyectomercadoproyeccionventas where codproyecto=" + codProyecto;

            var dt = consultas.ObtenerDataTable(txtSQL, "text");

            if (dt.Rows.Count > 0)
                txttiempoProyeccion = Convert.ToInt32(dt.Rows[0]["tiempoproyeccion"].ToString());
            else
                txttiempoProyeccion = 3;

            TableRow filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto("Fuentes de Financiación:", 1, 1, ""));
            tbltab.Rows.Add(filatitulo);

            filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            tbltab.Rows.Add(filatitulo);

            filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto("Recursos solicitados al fondo emprender en (smlv)", 1, 1, ""));
            tbltab.Rows.Add(filatitulo);

            Table tabla = new Table();

            TableRow fila = new TableRow();

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("Aporte de los Emprendedores", 4, 1, ""));
            tabla.Rows.Add(fila);

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Nombre", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Valor (Miles)", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Detalle", 1, 1, ""));
            tabla.Rows.Add(fila);

            txtSQL = "select id_aporte,nombre, valor, tipoaporte, detalle from proyectoaporte where codproyecto =" + codProyecto + " order by tipoaporte, nombre";

            dt = consultas.ObtenerDataTable(txtSQL, "text");

            string txtTipoAporte = string.Empty;

            Total = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (!txtTipoAporte.Equals(dr["TipoAporte"].ToString()))
                {
                    txtTipoAporte = dr["TipoAporte"].ToString();

                    fila = new TableRow();
                    fila.Cells.Add(crearCeldaTexto(dr["TipoAporte"].ToString(), 3, 1, ""));
                    tabla.Rows.Add(fila);
                }

                fila = new TableRow();
                fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(dr["nombre"].ToString(), 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(string.Format((Convert.ToDouble(dr["Valor"].ToString()) / 1000).ToString(), 2), 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(dr["detalle"].ToString(), 1, 1, ""));
                tabla.Rows.Add(fila);

                Total = Total + Convert.ToDouble(dr["Valor"].ToString());
            }

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            tabla.Rows.Add(fila);

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Total", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto(string.Format((Total / 1000).ToString(), 2), 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            tabla.Rows.Add(fila);

            filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto(tabla, 1, 1, ""));
            tbltab.Rows.Add(filatitulo);

            filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            tbltab.Rows.Add(filatitulo);

            tabla = new Table();

            fila = new TableRow();

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("Recursos de Capital", 6, 1, ""));
            tabla.Rows.Add(fila);

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Cuantía (miles)", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Plazo", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Forma de Pago", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Interes", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Destinación", 1, 1, ""));
            tabla.Rows.Add(fila);

            txtSQL = "select id_recurso,tipo,cuantia,plazo, formapago, interes, destinacion from proyectorecurso where codproyecto =" + codProyecto + " order by tipo";

            dt = consultas.ObtenerDataTable(txtSQL, "text");

            string txtTipo = string.Empty;
            Total = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (!txtTipo.Equals(dr["Tipo"].ToString()))
                {
                    txtTipo = dr["Tipo"].ToString();

                    fila = new TableRow();
                    fila.Cells.Add(crearCeldaTexto(dr["Tipo"].ToString(), 4, 1, ""));
                    tabla.Rows.Add(fila);
                }

                fila = new TableRow();
                fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(string.Format((Convert.ToDouble(dr["Cuantia"].ToString()) / 1000).ToString(), 2), 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(dr["Plazo"].ToString(), 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(dr["FormaPago"].ToString(), 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(dr["interes"].ToString(), 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(dr["destinacion"].ToString(), 1, 1, ""));
                tabla.Rows.Add(fila);

                Total = Total + Convert.ToDouble(dr["Cuantia"].ToString());
            }

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            tabla.Rows.Add(fila);

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("Total", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto(string.Format((Total / 1000).ToString(), 2), 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            tabla.Rows.Add(fila);

            filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto(tabla, 1, 1, ""));
            tbltab.Rows.Add(filatitulo);

            filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            tbltab.Rows.Add(filatitulo);

            tabla = new Table();

            fila = new TableRow();

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("Recursos de Capital", (Convert.ToInt32(txttiempoProyeccion) + 1), 1, ""));
            tabla.Rows.Add(fila);

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("Producto", 1, 1, ""));

            for (int i = 0; i < txttiempoProyeccion; i++)
            {
                fila.Cells.Add(crearCeldaTexto("Año " + (i + 1).ToString(), 1, 1, ""));
            }
            tabla.Rows.Add(fila);

            txtSQL = "select id_producto, nomproducto, porcentajeiva from proyectoproducto where codproyecto= " + codProyecto;

            dt = consultas.ObtenerDataTable(txtSQL, "text");

            for (int i = 0; i < txttiempoProyeccion; i++)
            {
                TotalPesos[i] = 0;
                TotalIva[i] = 0;
            }

            foreach (DataRow dr in dt.Rows)
            {
                fila = new TableRow();
                fila.Cells.Add(crearCeldaTexto(dr["NomProducto"].ToString(), 1, 1, ""));

                txtSQL = "select sum(unidades) as unidades, precio, sum(unidades)*precio as total, ano " +
                        "from proyectoproductounidadesventas u, proyectoproductoprecio p " +
                        "where p.codproducto=u.codproducto and periodo=ano and p.codproducto=" + dr["id_Producto"].ToString() +
                        " group by ano,precio order by ano";

                var dt1 = consultas.ObtenerDataTable(txtSQL, "text");

                int i = 0;
                foreach (DataRow dr1 in dt1.Rows)
                {
                    fila.Cells.Add(crearCeldaTexto(string.Format(((Convert.ToDouble(dr1["unidades"].ToString()) * Convert.ToDouble(dr1["Precio"].ToString())) / 1000).ToString(), 2), 1, 1, ""));

                    TotalPesos[i] = TotalPesos[i] + Convert.ToDouble(dr1["Total"].ToString());
                    TotalIva[i] = TotalIva[i] + (Convert.ToDouble(dr1["Total"].ToString()) * Convert.ToDouble(dr["porcentajeiva"].ToString()));

                    i++;
                }

                tabla.Rows.Add(fila);
            }

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            tabla.Rows.Add(fila);

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("Total", 1, 1, ""));

            for (int i = 0; i < txttiempoProyeccion; i++)
            {
                fila.Cells.Add(crearCeldaTexto(TotalPesos[(i)].ToString(), 1, 1, ""));
            }

            tabla.Rows.Add(fila);

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("IVA", 1, 1, ""));

            for (int i = 0; i < txttiempoProyeccion; i++)
            {
                fila.Cells.Add(crearCeldaTexto(TotalIva[(i)].ToString(), 1, 1, ""));
            }

            tabla.Rows.Add(fila);

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("Total con IVA", 1, 1, ""));

            for (int i = 0; i < txttiempoProyeccion; i++)
            {
                fila.Cells.Add(crearCeldaTexto((TotalIva[(i)] + TotalPesos[(i)]).ToString(), 1, 1, ""));
            }

            tabla.Rows.Add(fila);

            filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto(tabla, 1, 1, ""));
            tbltab.Rows.Add(filatitulo);
        }

        private void spImprimeEgreso(int codProyecto)
        {
            double txttiempoProyeccion;
            double txtActualizacionMonetaria;
            double Total = 0;
            double[] TotalGasto = new double[12];

            txtSQL = "select tiempoproyeccion from proyectomercadoproyeccionventas where codproyecto=" + codProyecto;

            var dt = consultas.ObtenerDataTable(txtSQL, "text");

            if (dt.Rows.Count > 0)
                txttiempoProyeccion = Convert.ToInt32(dt.Rows[0]["tiempoproyeccion"].ToString());
            else
                txttiempoProyeccion = 3;

            txtSQL = "select * from ProyectoFinanzasEgresos where codproyecto=" + codProyecto;

            dt = consultas.ObtenerDataTable(txtSQL, "text");

            if (dt.Rows.Count > 0)
                txtActualizacionMonetaria = Convert.ToDouble(dt.Rows[0]["ActualizacionMonetaria"].ToString());
            else
                txtActualizacionMonetaria = 3;

            TableRow filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto("Índice de Actualización monetaria :" + txtActualizacionMonetaria, 1, 1, ""));
            tbltab.Rows.Add(filatitulo);

            filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            tbltab.Rows.Add(filatitulo);

            Table tabla = new Table();

            TableRow fila = new TableRow();

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("Inversiones Fjas y Diferidas:", 5, 1, ""));
            tabla.Rows.Add(fila);

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Concepto", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Valor (Miles)", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Meses", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Tipo de Fuente", 1, 1, ""));
            tabla.Rows.Add(fila);

            txtSQL = "select id_inversion,concepto, valor, semanas, AportadoPor,TipoInversion from ProyectoInversion where codproyecto =" + codProyecto + " order by TipoInversion desc";

            dt = consultas.ObtenerDataTable(txtSQL, "text");

            string txtTipoInversion = string.Empty;

            Total = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (!txtTipoInversion.Equals(dr["TipoInversion"].ToString()))
                {
                    txtTipoInversion = dr["TipoInversion"].ToString();

                    fila = new TableRow();
                    fila.Cells.Add(crearCeldaTexto(dr["TipoInversion"].ToString(), 4, 1, ""));
                    tabla.Rows.Add(fila);
                }

                fila = new TableRow();
                fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(dr["concepto"].ToString(), 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(string.Format((Convert.ToDouble(dr["Valor"].ToString()) / 1000).ToString(), 2), 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(dr["Semanas"].ToString(), 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(dr["AportadoPor"].ToString(), 1, 1, ""));
                tabla.Rows.Add(fila);

                Total = Total + Convert.ToDouble(dr["Valor"].ToString());
            }

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            tabla.Rows.Add(fila);

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Total", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto(string.Format((Total / 1000).ToString(), 2), 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            tabla.Rows.Add(fila);

            filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto(tabla, 1, 1, ""));
            tbltab.Rows.Add(filatitulo);

            filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            tbltab.Rows.Add(filatitulo);

            tabla = new Table();

            fila = new TableRow();

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("Costos de Puesta en Marcha($ Miles)", 2, 1, ""));
            tabla.Rows.Add(fila);

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("Descripción", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Año 1", 1, 1, ""));
            tabla.Rows.Add(fila);

            txtSQL = "select id_gasto, descripcion, valor, protegido from proyectogastos where tipo='Arranque' and codproyecto =" + codProyecto + " order by descripcion";

            dt = consultas.ObtenerDataTable(txtSQL, "text");
            Total = 0;
            foreach (DataRow dr in dt.Rows)
            {

                fila = new TableRow();
                fila.Cells.Add(crearCeldaTexto(dr["Descripcion"].ToString(), 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(string.Format((Convert.ToDouble(dr["Valor"].ToString()) / 1000).ToString(), 2), 1, 1, ""));
                tabla.Rows.Add(fila);

                Total = Total + Convert.ToDouble(dr["Valor"].ToString());
            }

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            tabla.Rows.Add(fila);

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("Total", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto(string.Format((Total / 1000).ToString(), 2), 1, 1, ""));
            tabla.Rows.Add(fila);

            filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto(tabla, 1, 1, ""));
            tbltab.Rows.Add(filatitulo);

            filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            tbltab.Rows.Add(filatitulo);

            tabla = new Table();

            fila = new TableRow();

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("Costos Anualizados Administrativos($ Miles)", (Convert.ToInt32(txttiempoProyeccion) + 1), 1, ""));
            tabla.Rows.Add(fila);

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("Descripción", 1, 1, ""));

            for (int i = 0; i < txttiempoProyeccion; i++)
            {
                fila.Cells.Add(crearCeldaTexto("Año " + (i+1), 1, 1, ""));
            }
            tabla.Rows.Add(fila);

            txtSQL = "select id_gasto, descripcion, valor, protegido from proyectogastos where tipo='Anual' and codproyecto =" + codProyecto + " order by descripcion";

            for (int i = 0; i < txttiempoProyeccion; i++)
            {
                TotalGasto[i] = 0;
            }

            dt = consultas.ObtenerDataTable(txtSQL, "text");
            Total = 0;
            foreach (DataRow dr in dt.Rows)
            {

                fila = new TableRow();
                fila.Cells.Add(crearCeldaTexto(dr["Descripcion"].ToString(), 1, 1, ""));

                Total = Convert.ToDouble(dr["Valor"].ToString());
                for (int i = 0; i < txttiempoProyeccion; i++)
                {
                    fila.Cells.Add(crearCeldaTexto(Total.ToString(), 1, 1, ""));

                    Total = txtActualizacionMonetaria * Total;

                    TotalGasto[i] = TotalGasto[i] + Total;
                }

                tabla.Rows.Add(fila);
            }

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            tabla.Rows.Add(fila);

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("Total", 1, 1, ""));
            for (int i = 0; i < txttiempoProyeccion; i++)
            {
                fila.Cells.Add(crearCeldaTexto(TotalGasto[i].ToString(), 1, 1, ""));
            }
            tabla.Rows.Add(fila);

            filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto(tabla, 1, 1, ""));
            tbltab.Rows.Add(filatitulo);

            filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            tbltab.Rows.Add(filatitulo);

            tabla = new Table();

            fila = new TableRow();

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("Gastos de Personal($ Miles)", (Convert.ToInt32(txttiempoProyeccion) + 1), 1, ""));
            tabla.Rows.Add(fila);

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("Cargo", 1, 1, ""));

            for (int i = 0; i < txttiempoProyeccion; i++)
            {
                fila.Cells.Add(crearCeldaTexto("Año " + (i + 1), 1, 1, ""));
            }
            tabla.Rows.Add(fila);

            txtSQL = "select  cargo, valoranual from proyectogastospersonal where codproyecto =" + codProyecto + " order by cargo";

            for (int i = 0; i < txttiempoProyeccion; i++)
            {
                TotalGasto[i] = 0;
            }

            dt = consultas.ObtenerDataTable(txtSQL, "text");
            Total = 0;
            foreach (DataRow dr in dt.Rows)
            {

                fila = new TableRow();
                fila.Cells.Add(crearCeldaTexto("cargo", 1, 1, ""));

                Total = Convert.ToDouble(dr["valoranual"].ToString());
                for (int i = 0; i < txttiempoProyeccion; i++)
                {
                    fila.Cells.Add(crearCeldaTexto(Total.ToString(), 1, 1, ""));

                    Total = txtActualizacionMonetaria * Total;

                    TotalGasto[i] = TotalGasto[i] + Total;
                }

                tabla.Rows.Add(fila);
            }

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            tabla.Rows.Add(fila);

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("Total", 1, 1, ""));
            for (int i = 0; i < txttiempoProyeccion; i++)
            {
                fila.Cells.Add(crearCeldaTexto(TotalGasto[i].ToString(), 1, 1, ""));
            }
            tabla.Rows.Add(fila);

            filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto(tabla, 1, 1, ""));
            tbltab.Rows.Add(filatitulo);

            filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            tbltab.Rows.Add(filatitulo);
        }

        private void spImprimeCapital(int codProyecto)
        {
            double Total = 0;

            Table tabla = new Table();

            TableRow fila = new TableRow();
            
            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Componente", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Valor (Miles)", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Observación", 1, 1, ""));
            tabla.Rows.Add(fila);

            txtSQL = "select id_Capital,Componente, valor, Observacion from proyectoCapital where codproyecto =" + codProyecto + " order by Componente";

            var dt = consultas.ObtenerDataTable(txtSQL, "text");

            Total = 0;
            foreach (DataRow dr in dt.Rows)
            {
                fila = new TableRow();
                fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(dr["Componente"].ToString(), 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(string.Format((Convert.ToDouble(dr["Valor"].ToString()) / 1000).ToString(), 2), 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(dr["Observacion"].ToString(), 1, 1, ""));
                tabla.Rows.Add(fila);

                Total = Total + Convert.ToDouble(dr["Valor"].ToString());
            }

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            tabla.Rows.Add(fila);

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Total", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto(string.Format((Total / 1000).ToString(), 2), 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            tabla.Rows.Add(fila);

            TableRow filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto(tabla, 1, 1, ""));
            tbltab.Rows.Add(filatitulo);

            filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            tbltab.Rows.Add(filatitulo);
        }

        private void prImprimirEquipo(int codProyecto)
        {
            TableRow filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto("Equipo de Trabajo", 1, 1, ""));
            tbltab.Rows.Add(filatitulo);

            filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            tbltab.Rows.Add(filatitulo);

            Table tabla = new Table();

            TableRow fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("Nombre", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Email", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Rol", 1, 1, ""));
            tabla.Rows.Add(fila);

            txtSQL = "select id_contacto, nombres + ' ' +apellidos as nombre, email, id_rol, r.nombre as rol " +
                    "from proyectocontacto pc, contacto, rol r " +
                    "where id_rol=codrol and id_contacto=codcontacto and  pc.inactivo=0 " +
                    "and codrol in (" + Constantes.CONST_RolEmprendedor + "," + Constantes.CONST_RolAsesor + "," + Constantes.CONST_RolAsesorLider + ") " +
                    "and codproyecto =" + codProyecto + " order by rol,nombres";

            var dt = consultas.ObtenerDataTable(txtSQL, "text");

            foreach (DataRow dr in dt.Rows)
            {
                fila = new TableRow();
                fila.Cells.Add(crearCeldaTexto(dr["nombre"].ToString(), 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(dr["Email"].ToString(), 1, 1, ""));
                fila.Cells.Add(crearCeldaTexto(dr["Rol"].ToString(), 1, 1, ""));
                tabla.Rows.Add(fila);
            }

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            tabla.Rows.Add(fila);
            
            filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto(tabla, 1, 1, ""));
            tbltab.Rows.Add(filatitulo);

            filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            tbltab.Rows.Add(filatitulo);
        }

        private void prImprimeInfraestructura(int codProyecto)
        {
            double Total;

            TableRow filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto("Infraestructura:", 1, 1, ""));
            tbltab.Rows.Add(filatitulo);

            filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            tbltab.Rows.Add(filatitulo);

            Table tabla = new Table();

            TableRow fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Nombre", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Fecha de Compra", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Periodos de Amortización", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Sis. de Depreciación y/o Agotamiento", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("% Crédito", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Unidad", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Cantidad", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Precio/Unidad (miles)", 1, 1, ""));
            tabla.Rows.Add(fila);

            txtSQL = "SELECT id_TipoInfraestructura, nomTipoInfraestructura FROM tipoInfraestructura ORDER BY 1";

            var dt = consultas.ObtenerDataTable(txtSQL, "text");

            Total = 0;
            foreach (DataRow dr in dt.Rows)
            {
                txtSQL = "SELECT Id_ProyectoInfraestructura, NomInfraestructura, CodTipoInfraestructura, Unidad, ValorUnidad, Cantidad, FechaCompra,  " +
                            "ValorCredito, PeriodosAmortizacion, SistemaDepreciacion, T.nomTipoInfraestructura " +
                     "FROM proyectoInfraestructura P, TipoInfraestructura T WHERE T.id_TipoInfraestructura = P.codTipoInfraestructura " +
                      " AND codProyecto = " + codProyecto + " AND CodTipoInfraestructura = " + dr["id_TipoInfraestructura"].ToString();

                var dt1 = consultas.ObtenerDataTable(txtSQL, "text");

                if (dt1.Rows.Count > 0)
                {
                    fila = new TableRow();
                    fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
                    fila.Cells.Add(crearCeldaTexto(dr["nomTipoInfraestructura"].ToString(), 8, 1, ""));
                    tabla.Rows.Add(fila);
                }

                foreach (DataRow dr1 in dt1.Rows)
                {
                    fila = new TableRow();
                    fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
                    fila.Cells.Add(crearCeldaTexto(dr1["nomInfraestructura"].ToString(), 1, 1, ""));
                    fila.Cells.Add(crearCeldaTexto(dr1["FechaCompra"].ToString(), 1, 1, ""));
                    fila.Cells.Add(crearCeldaTexto(dr1["PeriodosAmortizacion"].ToString(), 1, 1, ""));
                    fila.Cells.Add(crearCeldaTexto(dr1["SistemaDepreciacion"].ToString(), 1, 1, ""));
                    fila.Cells.Add(crearCeldaTexto(dr1["ValorCredito"].ToString() + "%", 1, 1, ""));
                    fila.Cells.Add(crearCeldaTexto(dr1["Unidad"].ToString(), 1, 1, ""));
                    fila.Cells.Add(crearCeldaTexto(dr1["Cantidad"].ToString(), 1, 1, ""));
                    fila.Cells.Add(crearCeldaTexto(string.Format((Convert.ToDouble(dr1["ValorUnidad"].ToString()) / 1000).ToString(), 0), 1, 1, ""));
                    tabla.Rows.Add(fila);

                    Total = Total + Convert.ToDouble(dr1["ValorUnidad"].ToString()) * Convert.ToInt32(dr1["Cantidad"].ToString());
                }
            }

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            tabla.Rows.Add(fila);

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("", 6, 1, ""));
            fila.Cells.Add(crearCeldaTexto("Total", 1, 1, ""));
            fila.Cells.Add(crearCeldaTexto(string.Format((Total / 1000).ToString(), 2), 1, 1, ""));
            tabla.Rows.Add(fila);

            filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto(tabla, 1, 1, ""));
            tbltab.Rows.Add(filatitulo);

            filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            tbltab.Rows.Add(filatitulo);
        }

        private void prImprimeCostosProduccion(int codProyecto)
        {
            int numAnios;
            string strSelect = string.Empty;
            double []numTotPesos = new double[12];
            double[] numIVA = new double[12];

            txtSQL = "SELECT TiempoProyeccion FROM ProyectoMercadoProyeccionVentas WHERE codProyecto=" + codProyecto;

            var result = consultas.ObtenerDataTable(txtSQL, "text");

            if (result.Rows.Count > 0)
                numAnios = Convert.ToInt32(result.Rows[0]["TiempoProyeccion"].ToString());
            else
                numAnios = 0;

            Table tabla = new Table();

            TableRow fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("Tabla de costos de producci&oacute;n en pesos(incluido IVA)", (numAnios+1), 1, ""));
            tabla.Rows.Add(fila);

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("Tipo de Insumo", 1, 1, ""));

            strSelect = "";
            for (int i = 0; i < numAnios; i++)
            {
                fila.Cells.Add(crearCeldaTexto("Año" + (i + 1), 1, 1, ""));

                strSelect = strSelect + ", sum(case when Ano=" + (i + 1) + " then V.unidades*cantidad*((100+desperdicio)/100.0)*(Precio*((100+IVA)/100.0)) else 0 end) Total" + (i + 1);
                numTotPesos[i] = 0;
            }
            tabla.Rows.Add(fila);

            txtSQL = "SELECT T.id_TipoInsumo, T.nomTipoInsumo " + strSelect + " " + "FROM TipoInsumo T, ProyectoInsumo I, ProyectoProductoInsumo PI, ProyectoProductoUnidadesVentas V, ProyectoInsumoPrecio P " +
                 "WHERE	I.codTipoInsumo=T.id_TipoInsumo AND PI.codInsumo=I.id_Insumo AND V.codProducto=PI.codProducto AND P.codInsumo=I.id_Insumo AND V.codProducto=PI.codProducto AND P.periodo=V.ano AND Unidades<>0 AND " +
                        "I.codProyecto=" + codProyecto + " GROUP BY T.id_TipoInsumo, T.nomTipoInsumo ORDER BY nomTipoInsumo";

            var dt = consultas.ObtenerDataTable(txtSQL, "text");

            foreach (DataRow dr in dt.Rows)
            {
                fila = new TableRow();
                fila.Cells.Add(crearCeldaTexto(dr["nomTipoInsumo"].ToString(), 1, 1, ""));

                for (int i = 0; i < numAnios; i++)
                {
                    fila.Cells.Add(crearCeldaTexto(string.Format((Convert.ToDouble(dr["Total" + (i + 1)].ToString()) / 1000).ToString(), 0), 1, 1, ""));
                    numTotPesos[i] = numTotPesos[i] + Convert.ToDouble(dr["Total" + (i + 1)].ToString());
                }

                tabla.Rows.Add(fila);
            }

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            tabla.Rows.Add(fila);

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("Totales", 1, 1, ""));
            for (int i = 0; i < numAnios; i++)
            {
                fila.Cells.Add(crearCeldaTexto(string.Format(numTotPesos[i].ToString(),0), 1, 1, ""));
            }
            tabla.Rows.Add(fila);

            TableRow filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto(tabla, 1, 1, ""));
            tbltab.Rows.Add(filatitulo);

            filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto("", 1, 1, ""));
            tbltab.Rows.Add(filatitulo);

            tabla = new Table();

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("Proyección de Compras (Unidades)", (numAnios + 1), 1, ""));
            tabla.Rows.Add(fila);

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("Tipo de Insumo", 1, 1, ""));

            strSelect = "";
            for (int i = 0; i < numAnios; i++)
            {
                fila.Cells.Add(crearCeldaTexto("Año" + (i + 1), 1, 1, ""));

                strSelect = strSelect + ", SUM(case when ano=" + (i + 1) + " then V.Unidades*PI.Cantidad*(1+(desperdicio/100.0)) else 0 end) Unidades" + (i + 1);
            }
            tabla.Rows.Add(fila);

            foreach (DataRow dr in dt.Rows)
            {
                txtSQL = "SELECT I.nomInsumo " + strSelect + " FROM ProyectoInsumo I, ProyectoProductoInsumo PI, ProyectoProductoUnidadesVentas V " +
                         "WHERE PI.codInsumo = I.id_insumo AND V.codProducto=PI.codProducto AND I.codProyecto=" + codProyecto + " AND codTipoInsumo=" + dr["id_tipoInsumo"].ToString() + " " +
                         "GROUP BY I.nomInsumo HAVING Sum(V.Unidades)>0";

                var dt1 = consultas.ObtenerDataTable(txtSQL, "text");

                if (dt1.Rows.Count > 0)
                {
                    fila = new TableRow();
                    fila.Cells.Add(crearCeldaTexto(dr["nomTipoInsumo"].ToString(), (numAnios + 1), 1, ""));
                    tabla.Rows.Add(fila);
                }

                foreach (DataRow dr1 in dt1.Rows)
                {
                    fila = new TableRow();
                    fila.Cells.Add(crearCeldaTexto(dr1["nomInsumo"].ToString(), 1, 1, ""));

                    for (int i = 0; i < numAnios; i++)
                    {
                        fila.Cells.Add(crearCeldaTexto(string.Format((Convert.ToDouble(dr1["Unidades" + (i + 1)].ToString()) / 1000).ToString(), 0), 1, 1, ""));
                    }

                    tabla.Rows.Add(fila);
                }
            }

            filatitulo = new TableRow();

            filatitulo.Cells.Add(crearCeldaTexto(tabla, 1, 1, ""));
            tbltab.Rows.Add(filatitulo);

            tabla = new Table();

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("Proyección de Compras (Pesos)", (numAnios + 1), 1, ""));
            tabla.Rows.Add(fila);

            fila = new TableRow();
            fila.Cells.Add(crearCeldaTexto("Tipo de Insumo", 1, 1, ""));

            strSelect = "";
            for (int i = 0; i < numAnios; i++)
            {
                fila.Cells.Add(crearCeldaTexto("Año" + (i + 1), 1, 1, ""));

                strSelect = strSelect + ", sum(case when Ano=" + (i + 1) + " then V.unidades*cantidad*((100+desperdicio)/100.0)*precio else 0 end) Pesos" + (i + 1) + ", sum(case when Ano=" + (i + 1) + " then V.unidades*cantidad*((100+desperdicio)/100.0)*precio*IVA/100.0 else 0 end) IVA" + (i + 1);

                numTotPesos[i] = 0;
                numIVA[i] = 0;
            }
            tabla.Rows.Add(fila);

            foreach (DataRow dr in dt.Rows)
            {
                txtSQL = "SELECT I.nomInsumo " + strSelect + " " +
                     "FROM ProyectoInsumo I, ProyectoProductoInsumo PI, ProyectoProductoUnidadesVentas V, ProyectoInsumoPrecio P " +
                     "WHERE	PI.codInsumo=I.id_Insumo AND V.codProducto=PI.codProducto AND " +
                            "P.codInsumo=I.id_Insumo AND V.codProducto=PI.codProducto AND P.periodo=V.ano AND Unidades<>0 AND " +
                            "I.codProyecto=" + codProyecto + " AND I.codTipoInsumo=" + dr["id_tipoInsumo"].ToString() + " " +
                     "GROUP BY I.nomInsumo";

                var dt1 = consultas.ObtenerDataTable(txtSQL, "text");

                if (dt1.Rows.Count > 0)
                {
                    fila = new TableRow();
                    fila.Cells.Add(crearCeldaTexto(dr["nomTipoInsumo"].ToString(), (numAnios + 1), 1, ""));
                    tabla.Rows.Add(fila);
                }

                foreach (DataRow dr1 in dt1.Rows)
                {
                    fila = new TableRow();
                    fila.Cells.Add(crearCeldaTexto(dr1["nomInsumo"].ToString(), 1, 1, ""));

                    for (int i = 0; i < numAnios; i++)
                    {
                        fila.Cells.Add(crearCeldaTexto(string.Format((Convert.ToDouble(dr1["Pesos" + (i + 1)].ToString()) / 1000).ToString(), 0), 1, 1, ""));

                        numTotPesos[i]=numTotPesos[i]+Convert.ToDouble(dr1["Pesos" + (i + 1)].ToString());
                        numIVA[i] = numIVA[i] + Convert.ToDouble(dr1["IVA" + (i + 1)].ToString());
                    }

                    tabla.Rows.Add(fila);
                }

                fila = new TableRow();
                fila.Cells.Add(crearCeldaTexto("", 1, 1, ""));
                tabla.Rows.Add(fila);

                fila = new TableRow();
                fila.Cells.Add(crearCeldaTexto("Total", 1, 1, ""));

                for (int i = 0; i < numAnios; i++)
                {
                    fila.Cells.Add(crearCeldaTexto(string.Format((numTotPesos[i]).ToString(), 0), 1, 1, ""));
                }

                tabla.Rows.Add(fila);

                fila = new TableRow();
                fila.Cells.Add(crearCeldaTexto("IVA", 1, 1, ""));

                for (int i = 0; i < numAnios; i++)
                {
                    fila.Cells.Add(crearCeldaTexto(string.Format((numIVA[i]).ToString(), 0), 1, 1, ""));
                }

                tabla.Rows.Add(fila);

                fila = new TableRow();
                fila.Cells.Add(crearCeldaTexto("Total mas IVA", 1, 1, ""));

                for (int i = 0; i < numAnios; i++)
                {
                    fila.Cells.Add(crearCeldaTexto(string.Format((numIVA[i] + numTotPesos[i]).ToString(), 0), 1, 1, ""));
                }

                tabla.Rows.Add(fila);

                filatitulo = new TableRow();

                filatitulo.Cells.Add(crearCeldaTexto(tabla, 1, 1, ""));
                tbltab.Rows.Add(filatitulo);

                filatitulo = new TableRow();

                filatitulo.Cells.Add(crearCeldaTexto("", 1, 1, ""));
                tbltab.Rows.Add(filatitulo);
            }
        }
    }
}