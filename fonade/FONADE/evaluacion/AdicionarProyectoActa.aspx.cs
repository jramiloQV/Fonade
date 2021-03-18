using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Linq;
using Datos;
using Fonade.Negocio;
using System.Web.UI;
using System.Configuration;
using System.Web;

namespace Fonade.FONADE.evaluacion
{
    /// <summary>
    /// AdicionarProyectoActa
    /// </summary>

    public partial class AdicionarProyectoActa : Base_Page
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarProyecto();
            }
        }

        /// <summary>
        /// carga proyecto
        /// </summary>
        private void CargarProyecto()
        {
            try
            {

                ViewState["convocatoria"] = HttpContext.Current.Session["CodConvocatoria"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodConvocatoria"].ToString()) ? HttpContext.Current.Session["CodConvocatoria"].ToString() : "0";
                ViewState["actaId"] = HttpContext.Current.Session["CodActa"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodActa"].ToString()) ? HttpContext.Current.Session["CodActa"].ToString() : "0";

                if (ViewState["convocatoria"] != null)
                {
                    int codigo = !string.IsNullOrEmpty(ViewState["convocatoria"].ToString()) ? Convert.ToInt32(ViewState["convocatoria"].ToString()) :
                    0;

                    consultas.Parameters = new[] { new SqlParameter
                                                   { 
                                                        ParameterName = "@codconvocatoria",
                                                        Value = codigo
                                                   }};
                    DataTable dtActas = consultas.ObtenerDataTable("MD_ObtenerProyectosNegociosActas");

                    if (dtActas.Rows.Count != 0)
                    {
                        HttpContext.Current.Session["dtproyectoActa"] = dtActas;
                        GrvProyectoActas.DataSource = dtActas;
                        GrvProyectoActas.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// se crea proyecto evaluacion
        /// </summary>
        void Crearproyectoevaluacion()
        {
            try
            {
                if (GrvProyectoActas.Rows.Count != 0)
                {
                    foreach (GridViewRow rows in GrvProyectoActas.Rows)
                    {
                        var idproyecto = rows.FindControl("idproyecto") as Label;
                        var chkproyecto = rows.FindControl("cidproyecto") as CheckBox;
                        var evaluacionActa = new EvaluacionActaProyecto();
                        if (idproyecto != null)
                        {
                            if (chkproyecto != null && chkproyecto.Checked)
                            {
                                string txtSQL = "INSERT INTO evaluacionactaproyecto (CodActa, CodProyecto, Viable) VALUES (" + HttpContext.Current.Session["idacta"].ToString() + "," + idproyecto.Text + ",0)";

                                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                                try
                                {
                                    //se agrega parametros para insercion en la base de datos
                                    
                                    SqlCommand cmd = new SqlCommand(txtSQL, con);
                                    con.Open();
                                    cmd.CommandType = CommandType.Text;
                                    cmd.ExecuteNonQuery();
                                    
                                    cmd.Dispose();
                                }
                                catch (Exception)
                                {
                                    ClientScriptManager cm = this.ClientScript;
                                    cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('Se ha producido un error en la base de datos');</script>");
                                }
                                finally
                                {
                                    con.Close();
                                    con.Dispose();
                                }
                            }
                        }
                    }
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "Mensaje", "window.opener.location = window.opener.location; window.close();", true);
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string GetSortDirection(string column)
        {
            string sortDirection = "ASC";
            var sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;

                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }

        /// <summary>
        /// GRVs the actas page index changing.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs"/> instance containing the event data.</param>
        protected void GrvActasPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrvProyectoActas.PageIndex = e.NewPageIndex;
            GrvProyectoActas.DataSource = HttpContext.Current.Session["dtproyectoActa"];
            GrvProyectoActas.DataBind();
        }
        /// <summary>
        /// crea variable de session tipo datatable y la relaciona al grdiview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrvProyectoActasSorting(object sender, GridViewSortEventArgs e)
        {
            var dt = HttpContext.Current.Session["dtproyectoActa"] as DataTable;

            if (dt != null)
            {
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                GrvProyectoActas.DataSource = HttpContext.Current.Session["dtproyectoActa"];
                GrvProyectoActas.DataBind();
            }
        }

        /// <summary>
        /// llama procedimiento Crearproyectoevaluacion 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Unnamed1_Click(object sender, EventArgs e)
        {
            Crearproyectoevaluacion();
        }
    }
}