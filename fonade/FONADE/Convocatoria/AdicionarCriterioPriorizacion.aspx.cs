using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Fonade.Account;
using LinqKit;
using AjaxControlToolkit;
using System.ComponentModel;

namespace Fonade.FONADE.Convocatoria
{
    /// <summary>
    /// AdicionarCriterioPriorizacion
    /// </summary>    
    public partial class AdicionarCriterioPriorizacion : Negocio.Base_Page
    {
        /// <summary>
        /// The identifier convocatoria
        /// </summary>
        public int idConvocatoria;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
             idConvocatoria = Convert.ToInt32(HttpContext.Current.Session["Id_ConvocatCriterios"]);
             if (!IsPostBack)
             {
                 lbl_Titulo.Text = void_establecerTitulo("ADICIONAR CRITERIO DE PRIORIZACIÓN");
                 l_fechaActual.Text = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");
             }
        }

        /// <summary>
        /// Handles the Selecting event of the lds_criterioPriorizacion control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LinqDataSourceSelectEventArgs"/> instance containing the event data.</param>
        protected void lds_criterioPriorizacion_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            try
            {
                var query = from P in consultas.Mostrar_ListadoCriterios(idConvocatoria)
                            select P;
                e.Result = query;
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// Handles the Click event of the btn_adicionar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btn_adicionar_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow grd_Row in this.gvcriteriosPriorizacion.Rows)
            {
                if (((CheckBox)grd_Row.FindControl("ch_criterio")).Checked)
                {
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                    SqlCommand cmd = new SqlCommand("MD_convocatoria_criterios_priorizacion", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@Id_usuario", usuario.IdContacto.ToString());
                    cmd.Parameters.AddWithValue("@IdConvocatoria", idConvocatoria);
                    cmd.Parameters.AddWithValue("@IdCriterioPriorizacion", Convert.ToInt32(((HiddenField)grd_Row.FindControl("hiddenID")).Value));
                    cmd.Parameters.AddWithValue("@parametro", "");
                    cmd.Parameters.AddWithValue("@incidencias", 0);
                    cmd.Parameters.AddWithValue("@caso", "Create");
                    SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
                    con.Open();
                    cmd2.ExecuteNonQuery();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    con.Dispose();
                    cmd2.Dispose();
                    cmd.Dispose();
                } 
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "window.opener.location.reload(); window.close();", true);
        }

    }
}