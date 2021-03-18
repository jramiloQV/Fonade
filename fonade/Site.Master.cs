using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace Fonade
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)HeadLoginView.LoggedInTemplate = HeadLoginView.AnonymousTemplate;
        }

        protected void HeadLoginStatus_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            HeadLoginView.LoggedInTemplate = HeadLoginView.AnonymousTemplate;
            FormsAuthentication.SignOut();
            Session.RemoveAll();
            Response.Cookies.Clear();
            Session.Abandon();
        }
    }
}
