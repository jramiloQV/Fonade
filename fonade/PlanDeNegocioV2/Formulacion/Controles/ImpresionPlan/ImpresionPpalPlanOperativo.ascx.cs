using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan
{
    public partial class ImpresionPpalPlanOperativo : System.Web.UI.UserControl
    {
        public ImpresionPlanOperativo ImpresionPlan
        {
            get { return ImpresionPlanOper; }
        }

        public ImpresionMetasSociales ImpresionMeta
        {
            get { return ImpresionMetaSocial; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}