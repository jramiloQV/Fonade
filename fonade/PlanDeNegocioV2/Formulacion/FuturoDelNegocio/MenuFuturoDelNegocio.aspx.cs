using Fonade.Negocio.PlanDeNegocioV2.Utilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Formulacion.FuturoDelNegocio
{
    public partial class MenuFuturoDelNegocio : System.Web.UI.Page
    {
        public int CodigoProyecto
        {
            get
            {
                return Convert.ToInt32(Request.QueryString["codproyecto"]);
            }
            set { }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuarioLogged"] == null) { ScriptManager.RegisterStartupScript(Page, typeof(Page), "Rel", "window.parent.location.reload();", true); return; }
        }

        protected string GetTabStatus(int codigoTab)
        {
            return ProyectoGeneral.VerificarTabSiEsRealizado(codigoTab, CodigoProyecto) ? "✔" : string.Empty;
        }
    }
}