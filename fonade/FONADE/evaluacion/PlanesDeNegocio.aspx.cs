using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.evaluacion
{
    public partial class PlanesDeNegocio : Negocio.Base_Page
    {
        #region Variables globales.

        /// <summary>
        /// Variable que contiene las consultas SQL.
        /// </summary>
        String txtSQL;

        #endregion

        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //L_Fecha.Text = "" + DateTime.Now.Day + " Del Mes " + DateTime.Now.Month + " De " + DateTime.Now.Year;

            if (!IsPostBack)
            {
                try
                {
                    if (String.IsNullOrEmpty(HttpContext.Current.Session["idCodigoProUser"].ToString()))
                    { Response.Redirect("ListadoAcreditadores.aspx"); }
                    else
                    {
                        #region Cargar el nombre del "acreditador" que tiene los planes de negocio a consultar".

                        txtSQL = " SELECT ID_CONTACTO, (NOMBRES + ' ' + APELLIDOS) 'NOMBRE' FROM CONTACTO WHERE Id_Contacto = " + HttpContext.Current.Session["idCodigoProUser"].ToString();
                        var eje = consultas.ObtenerDataTable(txtSQL, "text");

                        if (eje.Rows.Count > 0)
                        { Label1.Text = "Planes de negocio asociados a: " + eje.Rows[0]["NOMBRE"].ToString(); }

                        eje = null;

                        #endregion

                        //Cargar la tabla.
                        CargarTabla();
                    }
                }
                catch { Response.Redirect("ListadoAcreditadores.aspx"); }
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 18/06/2014.
        /// Cargar la grilla "GridView1" tabla (2).
        /// </summary>
        private void CargarTabla()
        {
            try
            {
                txtSQL = " SELECT DISTINCT P.ID_PROYECTO 'CODIGO', P.NOMPROYECTO 'NOMBRE' " +
                         " FROM PROYECTO P JOIN PROYECTOCONTACTO PC " +
                         " ON (PC.CODCONTACTO = " + HttpContext.Current.Session["idCodigoProUser"].ToString() + " AND PC.INACTIVO=0 " +
                         " AND PC.ACREDITADOR = 1 AND PC.CODPROYECTO= P.ID_PROYECTO AND P.INACTIVO =0) " +
                         " ORDER BY 'NOMBRE' ASC ";

                var dt = consultas.ObtenerDataTable(txtSQL, "text");

                HttpContext.Current.Session["dt_Empresas_2"] = dt;
                GridView1.DataSource = dt;
                GridView1.DataBind();
                dt = null;
            }
            catch (Exception ex)
            {
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error al cargar la tabla de acreditadores. Error: '" + ex.Message + ".)", true);
                return;
            }
        }

        /// <summary>
        /// Se debe enviar la información de la tabla en uan variable se sesión
        /// para poder sortearlo.
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
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
        /// Paginación de la grilla.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            var dt = HttpContext.Current.Session["dt_Empresas_2"] as DataTable;

            if (dt != null)
            {
                GridView1.PageIndex = e.NewPageIndex;
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }

        /// <summary>
        /// Sortear la grilla.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            var dt = HttpContext.Current.Session["dt_Empresas_2"] as DataTable;

            if (dt != null)
            {
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                GridView1.DataSource = HttpContext.Current.Session["dt_Empresas_2"];
                GridView1.DataBind();
            }
        }
    }
}