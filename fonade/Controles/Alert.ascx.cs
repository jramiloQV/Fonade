using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.Controles
{
    /// <summary>
    /// Control de usuario alerta
    /// </summary>
    /// <seealso cref="System.Web.UI.UserControl" />
    public partial class Alert : System.Web.UI.UserControl
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Vers the specified texto.
        /// </summary>
        /// <param name="texto">The texto.</param>
        /// <param name="mostrar">if set to <c>true</c> [mostrar].</param>
        public void Ver(string texto, bool mostrar)
        {
            lbl_popup.Visible = mostrar;
            lbl_popup.Text = texto;
            mpe1.Enabled = mostrar;
            mpe1.Show();
        }
    }
}