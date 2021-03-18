using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Fonade.FONADE.interventoria
{
    public partial class InformeEjecucionProyecto : Negocio.Base_Page
    {
        #region Variables globales.

        Int32 CodProyecto;
        Int32 CodConvocatoria;
        DataTable rsAux;
        String txtSQL;
        //Dim numTabs
        //Dim txtLista

        #endregion

        /// <summary>
        /// Mauricio Arias Olave.
        /// 16/09/2014.
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Obtener los valores almacenados en variables de sesión.
            CodProyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? Convert.ToInt32(HttpContext.Current.Session["CodProyecto"].ToString()) : 0;
            CodConvocatoria = HttpContext.Current.Session["CodConvocatoria"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodConvocatoria"].ToString()) ? Convert.ToInt32(HttpContext.Current.Session["CodConvocatoria"].ToString()) : 0;

            if (!IsPostBack)
            { CargarTablaInformesEjecucion(); }
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
        /// Mauricio Arias Olave.
        /// 16/09/2014.
        /// Cargar la grilla de informes de ejecución para su selección.
        /// </summary>
        private void CargarTablaInformesEjecucion()
        {
            //Inicializar variables.
            DataTable RS = new DataTable();

            try
            {
                //txtSQL = "SELECT * FROM InformePresupuestal WHERE Estado = 3 AND CodEmpresa = ((SELECT Id_Empresa FROM Empresa WHERE CodProyecto = " + CodProyecto + ")) order by NomInformePresupuestal DESC";
                txtSQL = "Select ip.*, e.codProyecto from InformePresupuestal ip Inner Join Empresa e on e.id_Empresa = ip.CodEmpresa ";
                txtSQL  += "where e.codproyecto = "+ CodProyecto + " order by NomInformePresupuestal DESC";
                RS = consultas.ObtenerDataTable(txtSQL, "text");

                //Para mejorar el performance, sólo se agrega a la variable de sesión
                //la grilla cuando ésta tenga mas de 10* registros.
                //* Número de registros por página de la grilla.
                if (RS.Rows.Count > 10) { HttpContext.Current.Session["grilla_informes"] = RS; }

                gv_InformesEjecucion.DataSource = RS;
                gv_InformesEjecucion.DataBind();
            }
            catch { }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 16/09/2014.
        /// Sortear la grilla.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_InformesEjecucion_Sorting(object sender, GridViewSortEventArgs e)
        {
            var dt = HttpContext.Current.Session["grilla_informes"] as DataTable;

            if (dt != null)
            {
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gv_InformesEjecucion.DataSource = dt;
                gv_InformesEjecucion.DataBind();
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 16/09/2014.
        /// Cambiar de página de la grilla.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_InformesEjecucion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            var dt = HttpContext.Current.Session["grilla_informes"] as DataTable;

            if (dt != null)
            {
                gv_InformesEjecucion.PageIndex = e.NewPageIndex;
                gv_InformesEjecucion.DataSource = dt;
                gv_InformesEjecucion.DataBind();
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 16/09/2014.
        /// RowCommand.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_InformesEjecucion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string[] parametros = e.CommandArgument.ToString().Split(';');

            switch (e.CommandName)
            {
                case "VerInformeEjecucion":
                    //Asignar los valores a las variables de sesión.
                    HttpContext.Current.Session["CodInforme"] = parametros[0];
                    HttpContext.Current.Session["Periodo"] = parametros[1];
                    HttpContext.Current.Session["CodEmpresa"] = parametros[2];
                    HttpContext.Current.Session["CodProyecto"] = parametros[3];

                    //Se usan los valores para consultar.
                    Response.Redirect("AdicionarInformePresupuestalProyecto.aspx");
                    break;
                default:
                    break;
            }
        }
    }
}