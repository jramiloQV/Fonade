using Fonade.Error;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.evaluacion
{
    /// <summary>
    /// CatalogoEvaluacionConceptosAdicionar
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class CatalogoEvaluacionConceptosAdicionar : System.Web.UI.Page
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
        /// actualizacion evaluacion conceptos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void B_Nuevo_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand("INSERT INTO EvaluacionConceptos (NomEvaluacionConceptos) VALUES ('" + TB_Nuevo.Text + "')", conn);
            try
            {

                conn.Open();
                cmd.ExecuteReader();
            }
            catch (SqlException ex)
            {
                string url = Request.Url.ToString();

                string mensaje = ex.Message.ToString();
                string data = ex.Data.ToString();
                string stackTrace = ex.StackTrace.ToString();
                string innerException = ex.InnerException == null ? "" : ex.InnerException.Message.ToString();

                // Log the error
                ErrHandler.WriteError(mensaje, url, data, stackTrace, innerException, "CatalogoEvaluacionConceptosAdicionar", "CatalogoEvaluacionConceptosAdicionar");
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            Response.Redirect("CatalogoEvaluacionConceptos.aspx");
        }
    }
}