using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

namespace Fonade
{
    /// <summary>
    /// _Default
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class _Default : System.Web.UI.Page
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("/FONADE/MiPerfil/Home.aspx");
        }

        /// <summary>
        /// Texto de ayuda.
        /// </summary>
        /// <param name="texto"> texto.</param>
        /// <returns></returns>
        [WebMethod]
        public static string textoAyuda(string texto)
        {
            Datos.FonadeDBDataContext db =  new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            var sql_texto = ( from txt in db.Textos
                              where 
                              txt.NomTexto.ToLower() == texto.ToLower()
                            select txt
                            ).FirstOrDefault();
            string response = (sql_texto != null) ? sql_texto.Texto1 : "";
            
            return response;
        }
    }
}
