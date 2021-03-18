using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Evaluacion.Informes
{
    public partial class EvaluacionReportes : System.Web.UI.Page
    {

        public int IdProyecto
        {
            set { }
            get { return Request.QueryString.AllKeys.Contains("codproyecto") ? int.Parse(Request.QueryString["codproyecto"]) : 0; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IdProyecto == 0)
            {
                IdProyecto = Convert.ToInt32(Session["CodProyecto"].ToString());
            }
        }

        protected void lnkinforme_Click(object sender, EventArgs e)
        {            
            int pos = ((int.Parse(HiddenWidth.Value) - 750) / 2) - 20;
            
            if (IdProyecto != 0)
            {
                Fonade.Proyecto.Proyectos.Redirect(Response, "InformeEvaluacion.aspx?IdProyecto=" + IdProyecto, "_Blank", "scrollbars=yes,resizable=yes,width=750,height=700,top=100,left=" + pos);
            }
            else
            {
                string codProyecto = HttpContext.Current.Session["CodProyecto"].ToString();
                Fonade.Proyecto.Proyectos.Redirect(Response, "InformeEvaluacion.aspx?IdProyecto=" + codProyecto, "_Blank", "scrollbars=yes,resizable=yes,width=750,height=700,top=100,left=" + pos);
            }
            
        }
    }
}