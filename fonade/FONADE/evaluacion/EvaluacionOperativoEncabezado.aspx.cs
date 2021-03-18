using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;

namespace Fonade.FONADE.evaluacion
{
    public partial class EvaluacionOperativoEncabezado : System.Web.UI.Page
    {
        public string codProyecto;
        public string codConvocatoria;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                codProyecto = HttpContext.Current.Session["codProyecto"].ToString();
                HttpContext.Current.Session["codProyectoval"] = codProyecto;

                codProyecto = Request.QueryString["codProyecto"].ToString();
                HttpContext.Current.Session["codProyectoval"] = codProyecto;
            }
            catch (Exception) { }
        }
    }
}