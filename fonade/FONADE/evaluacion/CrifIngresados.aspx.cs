using System;
using System.Web.UI;
using Fonade.Negocio;
using System.Web;

namespace Fonade.FONADE.evaluacion
{

    /// <summary>
    /// clase CrifIngresados --notificaciones
    /// </summary>
    public partial class CrifIngresados : Base_Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarCrif();
            }
        }

        private void CargarCrif()
        {
            try
            {
                lfecha.Text = DateTime.Now.ToShortDateString();

                if (HttpContext.Current.Session["dtcrif"] != null)
                {
                    GrvCrif.DataSource = HttpContext.Current.Session["dtcrif"];
                    GrvCrif.DataBind();
                }
                else
                {
                    GrvCrif.DataSource = HttpContext.Current.Session["dtcrif"];
                    GrvCrif.DataBind();
                }

            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        protected void GrvNotificaciones_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            GrvCrif.PageIndex = e.NewPageIndex;
            GrvCrif.DataSource = HttpContext.Current.Session["dtcrif"];
            GrvCrif.DataBind();
        }

        protected void btncerrar_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["dtcrif"] = null;
            RedirectPage(false, string.Empty, "cerrar");

        }
    }
}