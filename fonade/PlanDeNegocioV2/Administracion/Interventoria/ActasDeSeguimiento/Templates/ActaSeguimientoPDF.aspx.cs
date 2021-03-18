using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.Templates
{
    public partial class ActaSeguimientoPDF : System.Web.UI.Page
    {
        public int CodigoProyecto
        {
            get
            {
                int id = Convert.ToInt32(Session["idProyecto"]);
                return id;
            }
            set { }
        }

        public int CodigoActa
        {
            get
            {
                int id = Convert.ToInt32(Session["idActa"]);
                return id;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Label1.Text = CodigoProyecto.ToString() + " - " + CodigoActa.ToString();
        }
    }
}