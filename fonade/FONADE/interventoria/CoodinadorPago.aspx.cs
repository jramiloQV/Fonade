using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.interventoria
{
    public partial class CoodinadorPago : Negocio.Base_Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string CodPagoActividad = HttpContext.Current.Session["Id_PagoActividad"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["Id_PagoActividad"].ToString()) ? HttpContext.Current.Session["Id_PagoActividad"].ToString() : "0";

                string txtSQL = @"SELECT CodProyecto, NomProyecto, CantidadDinero, NomPagoConcepto, FechaInterventor " +
                 " FROM PagoActividad, Proyecto, PagoConcepto  WHERE Id_PagoActividad = " + CodPagoActividad + " AND CodProyecto = Id_Proyecto  AND Id_PagoConcepto = CodPagoConcepto";

                var result = consultas.ObtenerDataTable(txtSQL, "text");

                if (result != null)
                {
                    lblnumsolicitud.Text = CodPagoActividad;
                    lblnumproyecto.Text = result.Rows[0]["CodProyecto"].ToString();
                    lblnomproyecto.Text = result.Rows[0]["NomProyecto"].ToString();
                    //lblvalorsolicitud.Text = result.Rows[0]["CantidadDinero"].ToString();
                    lblconceptosolicitud.Text = result.Rows[0]["NomPagoConcepto"].ToString();
                    lblfechasolicitud.Text = result.Rows[0]["FechaInterventor"].ToString();

                    #region Mauricio Arias Olave. "13/05/2014": Formater la moneda según como se muestra en FONADE clásico.
                    try
                    {
                        Double valor = Convert.ToDouble(result.Rows[0]["CantidadDinero"].ToString());
                        lblvalorsolicitud.Text = valor.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                    }
                    catch { lblfechasolicitud.Text = result.Rows[0]["CantidadDinero"].ToString(); }
                    #endregion

                    #region Mauricio Arias Olave. "13/05/2014": Formater la fecha según como se muestra en FONADE clásico.
                    try
                    {
                        //Inicializar variable DateTime.
                        DateTime fecha = Convert.ToDateTime(result.Rows[0]["FechaInterventor"].ToString());

                        //Obtener el nombre del mes (las primeras tres letras).
                        string sMes = fecha.ToString("MMM", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));

                        //Formatear la fecha según manejo de FONADE clásico. "Ej: Nov 19 de 2013".
                        lblfechasolicitud.Text = UppercaseFirst(sMes) + " " + fecha.Day + " de " + fecha.Year;
                    }
                    catch { lblfechasolicitud.Text = result.Rows[0]["FechaInterventor"].ToString(); }
                    #endregion

                    txtSQL = "SELECT NomPagoActividadArchivo, URL FROM PagoActividadarchivo WHERE codPagoActividad = " + CodPagoActividad;

                    result = consultas.ObtenerDataTable(txtSQL, "text");

                    if (result.Rows.Count > 0)
                    {
                        foreach (DataRow fila in result.Rows)
                        {
                            TableRow filaNeA2 = new TableRow();
                            HyperLink linkanex = new HyperLink()
                            {
                                Text = fila["NomPagoActividadArchivo"].ToString(),
                                Target = "_blank",
                                NavigateUrl = ConfigurationManager.AppSettings["RutaWebSite"] + "/" + fila["URL"].ToString()
                            };
                            filaNeA2.Cells.Add(celdaNormal(linkanex, 1, 1, ""));
                            t_table.Rows.Add(filaNeA2);
                        }

                        t_table.DataBind();
                    }
                }
            }
        }

        #region Métodos generales.

        /// <summary>
        /// Establecer el primer valor en mayúscula, retornando un string con la primera en maýsucula.
        /// </summary>
        /// <param name="s">String a procesar</param>
        /// <returns>String procesado.</returns>
        static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        #endregion

        private TableCell celdaNormal(Control mensaje, Int32 colspan, Int32 rowspan, String cssestilo)
        {
            var celda1 = new TableCell { ColumnSpan = colspan, RowSpan = rowspan, CssClass = cssestilo };
            celda1.Controls.Add(mensaje);
            return celda1;
        }
    }
}