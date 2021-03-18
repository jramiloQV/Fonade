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
    /// <summary>
    /// implementacion clase modelo financiero descarga excel
    /// </summary>
    public partial class DescargarModeloFInanciero : System.Web.UI.Page
    {
        private string codProyecto;

        protected void Page_Load(object sender, EventArgs e)
        {
            codProyecto = HttpContext.Current.Session["CodProyecto"].ToString();

            string nombrearchivo = "modelofinanciero" + codProyecto + ".xls";
            
            string rutamodeloFin = ConfigurationManager.AppSettings.Get("RutaDocumentosEvaluacion") + Math.Abs(Convert.ToInt32(codProyecto) / 2000) + @"/EvaluacionProyecto_" + codProyecto + @"/" + nombrearchivo;


            string attachment = "attachment; filename=" + "modelofinanciero" + ".xls";

            Response.ClearContent();

            Response.AddHeader("content-disposition", attachment);

            Response.ContentType = "application/ms-excel"; Response.WriteFile(rutamodeloFin); Response.End();
        }
    }
}