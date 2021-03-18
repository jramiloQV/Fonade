using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Formulacion.DesarrolloSolucion
{
    public partial class MenuDesarrolloSolucion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuarioLogged"] == null) {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "Rel", "window.parent.location.reload();", true); return;
            }
        }
    }
}