using Datos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.interventoria
{
    public partial class PagoRechazado : Negocio.Base_Page
    {
        string Id_PagoActividadRechazo;
        string txtSQL = String.Empty;
        String URLc = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            Id_PagoActividadRechazo = HttpContext.Current.Session["Id_PagoActividadRechazo"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["Id_PagoActividadRechazo"].ToString()) ? HttpContext.Current.Session["Id_PagoActividadRechazo"].ToString() : "0";

            if (!IsPostBack)
            {
                txtSQL = @"SELECT CodProyecto, NomProyecto, CantidadDinero, NomPagoConcepto, FechaCoordinador " +
                 " FROM PagoActividad, Proyecto, PagoConcepto WHERE Id_PagoActividad = " + Id_PagoActividadRechazo + " AND CodProyecto = Id_Proyecto AND Id_PagoConcepto = CodPagoConcepto";

                var result = consultas.ObtenerDataTable(txtSQL, "text");

                if (result != null)
                {
                    if (result.Rows.Count > 0)
                    {
                        lblnumsolicitud.Text = Id_PagoActividadRechazo;
                        lblnumproyecto.Text = result.Rows[0]["CodProyecto"].ToString();
                        lblnomproyecto.Text = result.Rows[0]["NomProyecto"].ToString();
                        lblvalorsolicitud.Text = result.Rows[0]["CantidadDinero"].ToString();
                        lblconceptosolicitud.Text = result.Rows[0]["NomPagoConcepto"].ToString();
                        //Se elimina la hora de el campo de fecha
                        string dt = result.Rows[0]["FechaCoordinador"].ToString();
                        lblfechasolicitud.Text = dt.Substring(0,10);

                        txtSQL = "SELECT NomPagoActividadArchivo, URL FROM PagoActividadarchivo WHERE codPagoActividad = " + Id_PagoActividadRechazo;

                        result = consultas.ObtenerDataTable(txtSQL, "text");

                        if (result.Rows.Count > 0)
                        {
                            //NavigateUrl = fila[@"file:\\10.3.3.118\" + "URL"].ToString()
                            //NavigateUrl = fila[HttpContext.Current.Session["Rutaarchivos"].ToString() + "URL"].ToString()
                            foreach (DataRow fila in result.Rows)
                            {
                                TableRow filaNeA2 = new TableRow();
                                URLc = "http://www.fondoemprender.com:8080/" + fila["URL"].ToString();
                                HyperLink linkanex = new HyperLink()
                                {
                                    Text = fila["NomPagoActividadArchivo"].ToString(),
                                    Target = "_blank",
                                    NavigateUrl =URLc.ToString()
                                };
                                filaNeA2.Cells.Add(celdaNormal(linkanex, 1, 1, ""));
                                t_table.Rows.Add(filaNeA2);
                            }
                            t_table.DataBind();
                        }
                    }
                }

                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                {
                    txtSQL = "SELECT Estado, FechaRtaFA, CodigoPago, ObservacionesFA FROM PagoActividad WHERE Id_PagoActividad = " + Id_PagoActividadRechazo;

                    result = consultas.ObtenerDataTable(txtSQL, "text");

                    if (result.Rows.Count > 0)
                    {
                        string dfFa = result.Rows[0]["FechaRtaFA"].ToString();
                        lblfechaFA.Text = dfFa.Substring(0, 10);

                        switch (Convert.ToInt32(result.Rows[0]["Estado"].ToString()))
                        {
                            case Constantes.CONST_EstadoAprobadoFA:
                                lblpagado.Text = "SI";
                                break;
                            case Constantes.CONST_EstadoRechazadoFA:
                                lblpagado.Text = "NO";
                                break;
                        }

                        lblcodigopago.Text = result.Rows[0]["CodigoPago"].ToString();
                        lblobservaciones.Text = result.Rows[0]["ObservacionesFA"].ToString();
                    }
                }
            }
        }

        private TableCell celdaNormal(Control mensaje, Int32 colspan, Int32 rowspan, String cssestilo)
        {
            var celda1 = new TableCell { ColumnSpan = colspan, RowSpan = rowspan, CssClass = cssestilo };
            celda1.Controls.Add(mensaje);
            return celda1;
        }
    }
}