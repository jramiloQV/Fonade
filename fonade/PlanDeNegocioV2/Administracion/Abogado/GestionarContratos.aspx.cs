using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Administracion.Abogado
{
    public partial class GestionarContratos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void linkCargueArchivo_Click(object sender, EventArgs e)
        {            
            Fonade.Proyecto.Proyectos.Redirect(Response, "~/PlanDeNegocioV2/Administracion/Abogado/CargueMasivo/CargarContratos.aspx", "_Blank", "width=700,height=540,top=100,left=100");
        }
    }
}