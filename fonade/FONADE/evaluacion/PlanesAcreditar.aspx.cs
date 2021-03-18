using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using Fonade.Negocio;

namespace Fonade.FONADE.evaluacion
{
    public partial class PlanesaAcreditar : Base_Page
    {
      

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarPlanesAcreditar();
            }
        }

        void CargarPlanesAcreditar()
        {
            try
            {
                consultas.Parameters = null;

                consultas.Parameters = new[]
                                            {
                                                new SqlParameter
                                                    {
                                                        ParameterName = "@idusuario",Value = usuario.IdContacto
                                                    }
                                            };

                var dtPlanesAcreditar = consultas.ObtenerDataTable("MD_PlanesAcreditarUsuario");

                if (dtPlanesAcreditar.Rows.Count!=0)
                {
                    HttpContext.Current.Session["dtPlanesAcreditar"] = dtPlanesAcreditar;
                    GrvPlanesAcreditar.DataSource = dtPlanesAcreditar;
                    GrvPlanesAcreditar.DataBind();
                }
                else
                {
                    GrvPlanesAcreditar.DataSource = dtPlanesAcreditar;
                    GrvPlanesAcreditar.DataBind();
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

        protected void GrvPlanesAcreditarRowcommad(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "frameset")
            {
                Response.Redirect("../Proyecto/ProyectoFrameSet.aspx?codProyecto=" + e.CommandArgument.ToString());

            }else if (e.CommandName == "acreditar")
            {
                string[] var = e.CommandArgument.ToString().Split('-');

                HttpContext.Current.Session["CodProyecto"] = var[0];
                HttpContext.Current.Session["CodConvocatoria"] = var[1];
                Response.Redirect("../interventoria/ProyectoAcreditacion.aspx");
            }
        }

        protected void GrvPlanesAcreditarPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrvPlanesAcreditar.PageIndex = e.NewPageIndex;
            GrvPlanesAcreditar.DataSource = HttpContext.Current.Session["dtPlanesAcreditar"];
            GrvPlanesAcreditar.DataBind();
        }

        protected void GrvPlanesAcreditarSorting(object sender, GridViewSortEventArgs e)
        {
            var dt = HttpContext.Current.Session["dtPlanesAcreditar"] as DataTable;

            if (dt != null)
            {

                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                GrvPlanesAcreditar.DataSource = HttpContext.Current.Session["dtPlanesAcreditar"];
                GrvPlanesAcreditar.DataBind();
            }
        }
    }
}