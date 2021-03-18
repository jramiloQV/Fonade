using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan
{
    public partial class ImpresionPpalEstructura : System.Web.UI.UserControl
    {
        public ImpresionPlanCompras ImpresionPlandeCompras
        {
            get { return ImpresionPlanCompra; }
        }

        public ImpresionCostosProd ImpresionCostosProduc
        {
            get { return ImpresionCostosProduccion; }
        }

        public ImpresionCostosAdmin ImpresionCostosAdmini
        {
            get { return ImpresionCostosAdministrativos; }
        }

        public ImpresionIngresosEf ImpresionIngreso
        {
            get { return ImpresionIngresosEstrucFin; }
        }

        public ImpresionEgresos ImpresionEgreso
        {
            get { return ImpresionEgresosEstrucFin; }
        }

        public ImpresionCapitalTrabajo ImpresionCapital
        {
            get { return ImpresionCapitalTrab; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}