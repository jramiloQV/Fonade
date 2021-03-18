using Datos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace Fonade.PlanDeNegocioV2.Ejecucion.Seguimiento
{
    public partial class AportesEjecucion : Negocio.Base_Page
    {
        public int CodigoProyecto { get { return Convert.ToInt32(Request.QueryString["codproyecto"]); } set { } }
        public int CodigoConvocatoria
        {
            get
            {
                return Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(CodigoProyecto,0).GetValueOrDefault();
            }
            set { }
        }
        string anioConvocatoria;

        string txtSolicitado = "";
        string txtRecomendado = "";

        string txtSQL;

        protected void Page_Load(object sender, EventArgs e)
        {                        
            var dt = consultas.ObtenerDataTable(txtSQL, "text");
            txtSQL = "select year(fechainicio) from convocatoria where id_Convocatoria=" + CodigoConvocatoria;

            dt = consultas.ObtenerDataTable(txtSQL, "text");

            if (dt.Rows.Count > 0)
                anioConvocatoria = dt.Rows[0][0].ToString();

            llenarDta();
        }

        private void llenarDta()
        {

            txtSQL = @"SELECT P.Recursos, isnull(C.valorRecomendado,0) valorrecomendado, C.EquipoTrabajo " +
                        " FROM ProyectoFinanzasIngresos P LEFT JOIN EvaluacionObservacion C " +
                        " ON P.codProyecto = C.codProyecto and C.codConvocatoria = " + CodigoConvocatoria + "  " +
                        " WHERE P.codproyecto=" + CodigoProyecto;

            var dt = consultas.ObtenerDataTable(txtSQL, "text");

            if (dt.Rows.Count > 0)
            {
                txtSolicitado = dt.Rows[0]["Recursos"].ToString();
                txtRecomendado = dt.Rows[0]["valorRecomendado"].ToString();

                lbltxtSolicitado.Text = txtSolicitado;

                obtenerAportes();
            }
        }

        private void obtenerAportes()
        {
            double Total = 0;

            txtSQL = @"SELECT nomTipoIndicador, id_TipoIndicador, sum(solicitado) as TotalSolicitado, isnull(sum(Recomendado),0) as TotalRecomendado " +
                        " FROM TipoIndicadorGestion T, EvaluacionProyectoAporte E " +
                        " WHERE E.codProyecto=" + CodigoProyecto + " AND E.codConvocatoria=" + CodigoConvocatoria + " AND codTipoIndicador= id_tipoindicador " +
                        " GROUP BY nomTipoIndicador, id_TipoIndicador " +
                        " ORDER BY id_TipoIndicador";

            var dt = consultas.ObtenerDataTable(txtSQL, "text");

            if (dt.Rows.Count > 0)
            {
                Table tabla;

                foreach (DataRow fila in dt.Rows)
                {
                    tabla = new Table();

                    tabla.CssClass = "Grilla";
                    tabla.Width = 873;

                    TableHeaderRow filaHeader = new TableHeaderRow();

                    filaHeader.Cells.Add(crearceladtitulo("", 1, 1, ""));
                    filaHeader.Cells.Add(crearceladtitulo("<div class='table-Width'>Nombre</div>", 1, 1, ""));
                    filaHeader.Cells.Add(crearceladtitulo("Detalle", 1, 1, ""));
                    filaHeader.Cells.Add(crearceladtitulo("<div class='table-Width'>Fuente de financiación</div>", 1, 1, ""));
                    filaHeader.Cells.Add(crearceladtitulo("Total Solicitado", 1, 1, ""));
                    filaHeader.Cells.Add(crearceladtitulo("%", 1, 1, ""));
                    filaHeader.Cells.Add(crearceladtitulo("Total Recomendado", 1, 1, ""));
                    filaHeader.Cells.Add(crearceladtitulo("%", 1, 1, ""));

                    tabla.Rows.Add(filaHeader);

                    double numtotal = 0;
                   
                    txtSQL = @" SELECT id_Aporte, Nombre, Detalle, Solicitado, isnull(Recomendado,0) as recomendado, Protegido, FuenteFinanciacion.DescFuente as FuenteFinanciacion " +
                              " FROM EvaluacionProyectoAporte E LEFT JOIN FuenteFinanciacion on E.IdFuenteFinanciacion = FuenteFinanciacion.IdFuente" +
                              " WHERE E.codProyecto = " + CodigoProyecto + " AND E.codConvocatoria = " + CodigoConvocatoria + " AND codTipoIndicador = " + fila["id_TipoIndicador"].ToString() + " AND Nombre <> 'Nómina' " +
                              " UNION SELECT id_Aporte, Nombre, Detalle, Solicitado, isnull(Recomendado, 0) as recomendado, Protegido, CASE Detalle WHEN 'Gastos de personal (Emprendedor)' THEN 'Aportes Emprendedores' WHEN 'Gastos de personal (Fondo)' THEN 'Fondo Emprender' WHEN 'Gastos de personal (Ventas)' THEN 'Ingresos por Ventas' END FuenteFinanciacion " +
                              " FROM EvaluacionProyectoAporte E LEFT JOIN FuenteFinanciacion on E.IdFuenteFinanciacion = FuenteFinanciacion.IdFuente " +
                              " WHERE E.codProyecto = " + CodigoProyecto + " AND E.codConvocatoria = " + CodigoConvocatoria + " AND codTipoIndicador = " + fila["id_TipoIndicador"].ToString() + " AND Nombre = 'Nómina' ORDER BY  id_Aporte ";
                    
                    var dt1 = consultas.ObtenerDataTable(txtSQL, "text");

                    if (dt1.Rows.Count > 0)
                    {
                        TableRow filaBody;

                        foreach (DataRow fila1 in dt1.Rows)
                        {
                            filaBody = new TableRow();

                            filaBody.Cells.Add(crearceldanormal("", 1, 1, ""));
                            filaBody.Cells.Add(crearceldanormal("<span style='text-transform: uppercase !important;text-align:center;'>" + fila1["Nombre"].ToString() + "</span>", 1, 1, ""));
                            filaBody.Cells.Add(crearceldanormal(fila1["Detalle"].ToString(), 1, 1, ""));
                            filaBody.Cells.Add(crearceldanormal("<span style='text-align:center;'>" + fila1["FuenteFinanciacion"].ToString() + "</span>", 1, 1, ""));
                            filaBody.Cells.Add(crearceldanormal(double.Parse(fila1["Solicitado"].ToString()).ToString("N"), 1, 1, ""));

                            if (!fila["TotalSolicitado"].ToString().Equals("0"))
                                filaBody.Cells.Add(crearceldanormal("" + String.Format("{0:0.00}", (Convert.ToDouble(fila1["Solicitado"].ToString()) * 100 / Convert.ToDouble(fila["TotalSolicitado"].ToString()))), 1, 1, ""));
                            else
                                filaBody.Cells.Add(crearceldanormal("<span style='text-align: right !important;'>0</span>", 1, 1, ""));

                            filaBody.Cells.Add(crearceldanormal(string.Format("{0:n}", double.Parse(fila1["Recomendado"].ToString())), 1, 1, ""));

                            if (!fila["TotalRecomendado"].ToString().Equals("0"))
                                filaBody.Cells.Add(crearceldanormal("" + String.Format("{0:0.00}", (Convert.ToDouble(fila1["Recomendado"].ToString()) * 100 / Convert.ToDouble(fila["TotalRecomendado"].ToString()))), 1, 1, ""));
                            else
                                filaBody.Cells.Add(crearceldanormal("0", 1, 1, ""));

                            tabla.Rows.Add(filaBody);

                            numtotal = numtotal + Convert.ToDouble(fila1["Solicitado"].ToString());
                        }

                        filaBody = new TableRow();

                        filaBody.Cells.Add(crearceldanormal("", 1, 1, ""));
                        filaBody.Cells.Add(crearceldanormal("", 1, 1, ""));
                        filaBody.Cells.Add(crearceldanormal("", 1, 1, ""));                        
                        filaBody.Cells.Add(crearceldanormal("<span style='font-weight: bolder !important;'>Totales</span>", 1, 1, ""));
                        filaBody.Cells.Add(crearceldanormal("<span style='font-weight: bolder !important;'>" + double.Parse(fila["TotalSolicitado"].ToString()).ToString("N") + "</span>", 1, 1, ""));
                        filaBody.Cells.Add(crearceldanormal("<span style='font-weight: bolder !important;'>100</span>", 1, 1, ""));
                        filaBody.Cells.Add(crearceldanormal("<span style='font-weight: bolder !important;'>" + double.Parse(fila["TotalRecomendado"].ToString()).ToString("N") + "</span>", 1, 1, ""));
                        filaBody.Cells.Add(crearceldanormal("<span style='font-weight: bolder !important;'>100</span>", 1, 1, ""));

                        tabla.Rows.Add(filaBody);

                        Total = Total + Convert.ToDouble(fila["TotalRecomendado"].ToString());
                    }

                    p_aprotes.Controls.Add(tabla);
                }
            }

            txtSQL = @"SELECT distinct C.id_contacto, C.Nombres + ' ' + C.apellidos NomCompleto, isnull(PC.Beneficiario,0) Beneficiario, isnull(EC.AporteDinero,0) as AporteDinero, isnull(EC.AporteEspecie,0) as AporteEspecie , EC.DetalleEspecie " +
                        " FROM Contacto C " +
                        " INNER JOIN ProyectoContacto PC ON C.id_contacto = PC.codContacto and Pc.inactivo=0 and C.Inactivo = 0 and PC.codProyecto = " + CodigoProyecto + " and codRol = " + Constantes.CONST_RolEmprendedor + " " +
                        " LEFT JOIN EvaluacionContacto EC ON PC.codContacto = EC.codContacto and PC.codProyecto = EC.codProyecto  and EC.codConvocatoria=" + CodigoConvocatoria + " " +
                        " ORDER BY C.Nombres + ' ' + C.apellidos";

            dt = consultas.ObtenerDataTable(txtSQL, "text");

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow fila in dt.Rows)
                {
                    TableRow filaBody = new TableRow();

                    filaBody.Cells.Add(crearceldanormal("", 1, 1, ""));
                    filaBody.Cells.Add(crearceldanormal(fila["NomCompleto"].ToString(), 1, 1, ""));

                    if (Convert.ToBoolean(fila["Beneficiario"].ToString()))
                    {
                        filaBody.Cells.Add(crearceldanormal(new Image() { ImageUrl = "~/Images/chulo.gif" }, 1, 1, ""));
                        filaBody.Cells.Add(crearceldanormal("", 1, 1, ""));
                    }
                    else
                    {
                        filaBody.Cells.Add(crearceldanormal("", 1, 1, ""));
                        filaBody.Cells.Add(crearceldanormal(new Image() { ImageUrl = "~/Images/chulo.gif" }, 1, 1, ""));
                    }

                    filaBody.Cells.Add(crearceldanormal("" + ((Convert.ToDouble(fila["AporteDinero"].ToString()) + Convert.ToDouble(fila["AporteEspecie"].ToString())) / 100), 1, 1, ""));
                    filaBody.Cells.Add(crearceldanormal("" + (Convert.ToDouble(fila["AporteDinero"].ToString()) / 100), 1, 1, ""));
                    filaBody.Cells.Add(crearceldanormal("" + (Convert.ToDouble(fila["AporteEspecie"].ToString()) / 100), 1, 1, ""));
                    filaBody.Cells.Add(crearceldanormal(fila["DetalleEspecie"].ToString(), 1, 1, ""));

                    tfinnal.Rows.Add(filaBody);
                }
            }

            var salario = consultas.Db.SalariosMinimos.FirstOrDefault(s => s.AñoSalario == Convert.ToInt32(anioConvocatoria));

            if (CodigoConvocatoria.Equals("1"))
                Total = Convert.ToDouble(txtRecomendado) * salario.SalarioMinimo;
            else
            {
                if (!txtRecomendado.Equals(txtRecomendado))
                    if (Total > (224 * salario.SalarioMinimo))
                        Total = 224 * salario.SalarioMinimo;
            }

            lblvalorrecomendado.Text = "" + Math.Round(Total / salario.SalarioMinimo, 0);
        }

        private TableHeaderCell crearceladtitulo(string mensaje, int colspan, int rowspan, string cssestilo)
        {
            TableHeaderCell celda1 = new TableHeaderCell()
            {
                ColumnSpan = colspan,
                RowSpan = rowspan,
                CssClass = cssestilo
            };

            celda1.Controls.Add(new Label() { Text = mensaje });

            return celda1;
        }

        private TableCell crearceldanormal(string mensaje, int colspan, int rowspan, string cssestilo)
        {
            TableCell celda1 = new TableCell()
            {
                ColumnSpan = colspan,
                RowSpan = rowspan,
                CssClass = cssestilo
            };

            celda1.Controls.Add(new Label() { Text = mensaje });

            return celda1;
        }

        private TableCell crearceldanormal(Control mensaje, int colspan, int rowspan, string cssestilo)
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
    }
}