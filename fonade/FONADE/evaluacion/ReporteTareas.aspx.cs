#region Diego Quiñonez

// <Author>Diego Quiñonez</Author>
// <Fecha>10 - 06 - 2014</Fecha>
// <Archivo>ReporteTareas.cs</Archivo>

#endregion

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
    public partial class ReporteTareas1 : Negocio.Base_Page
    {
        #region Variables globales.

        /// <summary>
        /// Contiene consultas SQL.
        /// </summary>
        string txtSQL;

        #endregion

        /// <summary>
        ///  Diego Quiñonez
        /// 10 - 06 - 2014
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //metodo de cargue de la pagina
            if (!IsPostBack) { CargarReporteTareas(); }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 13/09/2014.
        /// Cargar el reporte de tareas "Grilla".
        /// </summary>
        private void CargarReporteTareas()
        {
            try
            {
                txtSQL = " select nomtareaprograma, nomproyecto, nomtareausuario, descripcion," +
                         " nombres+' '+apellidos as agendo,fecha, respuesta, fechacierre, codcontactoagendo, id_proyecto" +
                         " from tareausuario tu, tareaprograma, proyecto, contacto c, tareausuariorepeticion" +
                         " where id_tareaprograma = codtareaprograma and id_contacto=codcontactoagendo" +
                         " and id_proyecto=codproyecto and id_tareausuario=codtareausuario and tu.codcontacto=" + usuario.IdContacto + "ORDER BY fecha DESC";

                var rsTarea = consultas.ObtenerDataTable(txtSQL, "text");
                HttpContext.Current.Session["rsTarea"] = rsTarea;

                gvrtareas.DataSource = rsTarea;
                gvrtareas.DataBind();

                rsTarea = null;
            }
            catch { }
        }

        /// <summary>
        ///  Diego Quiñonez
        /// 10 - 06 - 2014
        /// metodo que
        /// realiza el paginado de la grilla.
        /// Mauricio Arias Olave.
        /// Modificado para reemplazar LINQ.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            #region LINQ.
            //gvrtareas.PageIndex = e.NewPageIndex;//nuevo paginado 
            #endregion

            #region SQL.

            var rt = HttpContext.Current.Session["rsTarea"] as DataTable;

            if (rt != null)
            {
                gvrtareas.PageIndex = e.NewPageIndex;
                gvrtareas.DataSource = rt;
                gvrtareas.DataBind();
            }

            #endregion
        }

        protected void gvrtareas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var rsTarea1 = new DataTable();
                var hdf = e.Row.FindControl("hdf_codcontactoagendo") as HiddenField;
                var lbl = e.Row.FindControl("lbl_agendo_") as Label;

                if (hdf != null)
                {
                    txtSQL = "select codrol from contacto c, proyectocontacto pc where id_contacto=" + hdf.Value + " and id_contacto=codcontacto ";
                    rsTarea1 = consultas.ObtenerDataTable(txtSQL, "text");

                    //Mostrar la palabra en negrilla "EVALUADOR" si se cumplen estas condiciones.
                    if (usuario.CodGrupo != 9 && usuario.CodGrupo != 10)
                    {
                        if (rsTarea1.Rows.Count > 0)
                        {
                            if (rsTarea1.Rows[0]["CodRol"].ToString() == "4")
                            { lbl.Text = "<strong>EVALUADOR</strong>"; }
                        }
                    }
                }
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
        /// Sortear la grilla.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvrtareas_Sorting(object sender, GridViewSortEventArgs e)
        {
            var dt = HttpContext.Current.Session["rsTarea"] as DataTable;

            if (dt != null)
            {
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvrtareas.DataSource = dt;
                gvrtareas.DataBind();
            }
        }

        protected void ddlRegistrosPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvrtareas.PageSize = int.Parse(ddlRegistrosPage.SelectedValue);
            CargarReporteTareas();
        }
    }
}