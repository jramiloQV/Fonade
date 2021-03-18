using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Account;
using System.Web.Security;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.Templates
{
    public partial class Acta : System.Web.UI.MasterPage
    {
        [ContextStatic]
        protected FonadeUser usuario;

        protected override void OnLoad(EventArgs e)
        {
            if (Session["usuarioLogged"] == null) { Response.Redirect("~/Account/Login.aspx"); return; }
            usuario = HttpContext.Current.Session["usuarioLogged"] != null ?
                (FonadeUser)HttpContext.Current.Session["usuarioLogged"] :
                (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true);
            base.OnLoad(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}