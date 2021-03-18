using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan
{
    public partial class ImpresionPPalDesarrollo : System.Web.UI.UserControl
    {
        public ImpresionIngresos ImpresionIngresoCondiciones
        {
            get { return ImpresionIngreso; }
        }

        public ImpresionProyeccion ImpresionProyeccions
        {
            get { return ImpresionProyecciones; }
        }

        public ImpresionNormatividad ImpresionNormas
        {
            get { return ImpresionNormatividades; }
        }

        public ImpresionRequerimientos ImpresionReqNeg
        {
            get { return ImpresionRequerimientosNeg; }
        }

        public ImpresionProduccion ImpresionProd
        {
            get { return ImpresionProduccions; }
        }

        public ImpresionProductividad ImpresionProductiv
        {
            get { return ImpresionProducti; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}