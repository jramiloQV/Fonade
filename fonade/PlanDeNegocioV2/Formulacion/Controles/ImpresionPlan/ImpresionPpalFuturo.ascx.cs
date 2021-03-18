using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan
{
    public partial class ImpresionPpalFuturo : System.Web.UI.UserControl
    {
        public ImpresionPeriodoArranque ImpresionPeriodoArran
        {
            get { return ImpresionPeriodo; }
        }

        public ImpresionEstrategia ImpresionEstrategias
        {
            get { return ImpresionEstrateg; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}