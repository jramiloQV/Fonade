using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.interventoria
{
    public partial class DescargarCoord : Negocio.Base_Page
    {
        string Id_Acta;
        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (!IsPostBack)
            {
                Id_Acta = HttpContext.Current.Session["Id_Acta"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["Id_Acta"].ToString()) ? HttpContext.Current.Session["Id_Acta"].ToString() : "0";

                if (!string.IsNullOrEmpty(Id_Acta))
                {
                    lblseract.Text = "NúmeroSolicitud: " + Id_Acta;
                    //Se agrega el orden de la consulta sql
                    string txtSQL = "SELECT DatosFirma,numSolicitudes,Fecha,CodContacto FROM PagosActaSolicitudes  WHERE Id_Acta=" + Id_Acta ;

                    var pagosacasol = consultas.ObtenerDataTable(txtSQL, "text");

                    if (pagosacasol != null)
                    {
                        lblidacta001.Text = "" + Id_Acta;

                        lblnomemo.Text = "" + Id_Acta;
                        lblnumsolici.Text = "" + pagosacasol.Rows[0]["numSolicitudes"].ToString();
                        lblfecha.Text = "" + pagosacasol.Rows[0]["Fecha"].ToString();
                        lbldatosfirmdigita.Text = "" + pagosacasol.Rows[0]["DatosFirma"].ToString();
                    }

                    var result = (from pa in consultas.Db.MD_DetalleSolicitudesPago(Convert.ToInt32(Id_Acta)) orderby pa.Id_PagoActividad select pa);

                    if (result != null)
                    {
                        gv_pagosactividad.DataSource = result;
                        gv_pagosactividad.DataBind();

                        foreach (GridViewRow fila in gv_pagosactividad.Rows)
                        {
                            Label valoApro = (Label)fila.FindControl("lblaprobado");

                            if (Convert.ToBoolean(valoApro.Text))
                            {
                                valoApro.Text = "SI";
                            }
                            else
                            {
                                valoApro.Text = "NO";
                            }
                        }

                        lblcantipagos.Text = "" + (from pa in consultas.Db.MD_DetalleSolicitudesPago(Convert.ToInt32(Id_Acta)) select pa).Count() + " Solicitudes de pago";
                    }
                }
            }
        }

        protected void gv_pagosactividad_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            HttpContext.Current.Session["Id_PagoActividad"] = e.CommandArgument;


            Redirect(null, "CoodinadorPago.aspx", "_Blank", "width=500,height=400");
        }
    }
}