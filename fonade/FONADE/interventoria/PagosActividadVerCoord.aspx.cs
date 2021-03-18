using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.interventoria
{
    public partial class PagosActividadVerCoord : Negocio.Base_Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var dt = new DataTable();

                dt.Columns.Add("Id_Acta");
                dt.Columns.Add("Fecha");
                dt.Columns.Add("NumSolicitudes");
                dt.Columns.Add("DatosFirma");
                dt.Columns.Add("CodRechazoFirmaDigital");

                var result = from pa in consultas.Db.MD_PagosActividad(usuario.IdContacto) select pa;

                foreach (var res in result)
                {
                    string txtSQL = "SELECT COUNT(CodPagosActaSolicitudes) AS NumSolicitudes FROM PagosActaSolicitudPagos WHERE CodPagosActaSolicitudes = " + res.Id_Acta;

                    int numSolicitudes = 0;

                    var dte = consultas.ObtenerDataTable(txtSQL, "text");

                    if (dte.Rows.Count > 0)
                    {
                        numSolicitudes = Convert.ToInt32(dte.Rows[0]["NumSolicitudes"].ToString());
                    }

                    DataRow fila = dt.NewRow();

                    fila["Id_Acta"] = "" + res.Id_Acta;
                    fila["Fecha"] = "" + res.Fecha;
                    fila["NumSolicitudes"] = "" + numSolicitudes;
                    fila["DatosFirma"] = "" + res.DatosFirma;

                    if (res.CodRechazoFirmaDigital == 1)
                        fila["CodRechazoFirmaDigital"] = "No Procesada";
                    else
                        fila["CodRechazoFirmaDigital"] = "Procesada";

                    dt.Rows.Add(fila);
                }

                gv_pagosactividad.DataSource = dt;
                gv_pagosactividad.DataBind();
            }
        }

        protected void gv_pagosactividad_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            HttpContext.Current.Session["Id_Acta"] = e.CommandArgument;
            Response.Redirect("DescargarCoord.aspx");
        }
    }
}