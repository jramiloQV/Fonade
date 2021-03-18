using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.Controles
{
    public partial class DateTimePicker : System.Web.UI.UserControl
    {
        public string DateTime
        {
            get { return txtDateTime.Text; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DateTimePicker picker = this;
            ScriptManager.RegisterClientScriptBlock(picker, picker.GetType(), "message", "<script type=\"text/javascript\" language=\"javascript\">getDateTimePicker();</script>", false);
        }
    }
}