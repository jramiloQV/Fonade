#region Diego Quiñonez

// <Author>Diego Quiñonez</Author>
// <Fecha>24 - 03 - 2014</Fecha>
// <Archivo>SeguimientoEmpresas.aspx.cs</Archivo>

#endregion

#region

using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Datos;
using Fonade.Negocio;
using System.Web;

#endregion

namespace Fonade.FONADE.interventoria
{
    public partial class SeguimientoEmpresas : Base_Page

    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ValidarGrupo();
            }
        }

        #region metodos

        private void ValidarGrupo()
        {
            if (usuario.CodGrupo == Constantes.CONST_Interventor)
            {
            }

            try
            {
                consultas.Parameters = null;

                consultas.Parameters = new[]
                                           {
                                               new SqlParameter
                                                   {
                                                       ParameterName = "@codusuario",
                                                       Value = usuario.IdContacto
                                                   }
                                           };

                var dtEmpresas = consultas.ObtenerDataTable("MD_ListarEmpresaInterventor");

                if (dtEmpresas.Rows.Count != 0)
                {
                    HttpContext.Current.Session["dtEmpresas"] = dtEmpresas;
                    GrvEmpresas.DataSource = dtEmpresas;
                    GrvEmpresas.DataBind();
                }
                else
                {
                    GrvEmpresas.DataSource = dtEmpresas;
                    GrvEmpresas.DataBind();
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

        #endregion

        #region GridView

        protected void GrvEmpresasPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrvEmpresas.PageIndex = e.NewPageIndex;
            GrvEmpresas.DataSource = HttpContext.Current.Session["dtEmpresas"];
            GrvEmpresas.DataBind();
        }

        protected void GrvEmpresasRowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "frameset")
            {
                string[] par = e.CommandArgument.ToString().Split(';');
                HttpContext.Current.Session["CodProyecto"] = par[0];
                HttpContext.Current.Session["CodEmpresa"] = par[1];
                Response.Redirect("../Proyecto/ProyectoFrameSet.aspx");
            }
            else if (e.CommandName == "empresa")
            {
                string[] par = e.CommandArgument.ToString().Split(';');
                HttpContext.Current.Session["CodProyecto"] = par[0];
                HttpContext.Current.Session["CodEmpresa"] = par[1];
                Response.Redirect("SeguimientoFrameset.aspx");
            }
        }

        protected void GrvEmpresasSorting(object sender, GridViewSortEventArgs e)
        {
            var dt = HttpContext.Current.Session["dtEmpresas"] as DataTable;

            if (dt != null)
            {
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                GrvEmpresas.DataSource = HttpContext.Current.Session["dtEmpresas"];
                GrvEmpresas.DataBind();
            }
        }

        #endregion
    }
}