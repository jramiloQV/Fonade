using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;
using System.Web.Configuration;

namespace Fonade.FONADE.evaluacion
{
    public partial class DownloadScript : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string plantillaModelo = WebConfigurationManager.AppSettings["RutaDocumentos"].ToString() + "modelofinanciero.xls";
            string attachment = "attachment; filename=" + "modelofinanciero" + ".xls";

            Response.ClearContent();

            Response.AddHeader("content-disposition", attachment);

            Response.ContentType = "application/ms-excel"; Response.WriteFile(plantillaModelo); Response.End();
        }
    }
}