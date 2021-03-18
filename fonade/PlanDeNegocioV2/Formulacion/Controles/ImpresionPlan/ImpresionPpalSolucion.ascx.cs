using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan
{
    public partial class ImpresionPpalSolucion : System.Web.UI.UserControl
    {
        public ImpresionSolucion ImpresionSolucionPta1
        {
            get { return ImpresionSolucion; }
        }

        public ImpresionFichaTecnica ImpresionSolucionPta2
        {
            get { return ImpresionFichaTec; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}