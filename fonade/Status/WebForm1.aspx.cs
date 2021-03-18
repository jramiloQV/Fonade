using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.Status
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(20000);
        }

        protected void btnAsync_Click(object sender, EventArgs e)
        {
            lblResults.Text = "Async Complete";
        }

        protected void btnFullPost_Click(object sender, EventArgs e)
        {
            lblResults.Text = "Non async Complete";

        }
    }
}