using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.evaluacion
{
    public partial class ReporteTareas : Negocio.Base_Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lnkinforme_Click(object sender, EventArgs e)
        {
            Redirect(null, "InformeEvaluacion.aspx", "_blank", "width=750,height=700,scrollbars=yes,resizable=yes");
        }
    }
}