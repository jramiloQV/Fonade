using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Fonade.FONADE.interventoria
{
    public partial class InformeVisitaProyecto : Negocio.Base_Page
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
            { CargarTablaInformesVisita(); }
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
        /// Cargar la grilla de informes de visita para su selección.
        /// </summary>
        private void CargarTablaInformesVisita()
        {
            //Inicializar variables.
            DataTable RS = new DataTable();

            //try
            //{
                txtSQL = "SELECT * FROM InformeVisitaInterventoria WHERE CodEmpresa = (SELECT Id_Empresa FROM Empresa WHERE CodProyecto = " + CodProyecto + ") order by NombreInforme DESC";
                RS = consultas.ObtenerDataTable(txtSQL, "text");

                #region Generar columna con valor numérico.

                //Inicializar variable que indica el inicio del incremento que se evalúa mas adelante.
                int inicial = 1;

                //Añadir columna adicional "para mostrar números".
                RS.Columns.Add("NUM", typeof(System.Int32));

                //Recorrer las filas del DataTable y añadir el valor incremental.
                foreach (DataRow row in RS.Rows)
                {
                    row["NUM"] = inicial;
                    inicial++;
                }

                #endregion

                //Para mejorar el performance, sólo se agrega a la variable de sesión
                //la grilla cuando ésta tenga mas de 10* registros.
                //* Número de registros por página de la grilla.
                if (RS.Rows.Count > 10) { HttpContext.Current.Session["grilla_informes"] = RS; }

                gv_InformesVisita.DataSource = RS;
                gv_InformesVisita.DataBind();
            //}
            //catch { }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 16/09/2014.
        /// Sortear la grilla.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_InformesVisita_Sorting(object sender, GridViewSortEventArgs e)
        {
            var dt = HttpContext.Current.Session["grilla_informes"] as DataTable;

            if (dt != null)
            {
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gv_InformesVisita.DataSource = dt;
                gv_InformesVisita.DataBind();
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 16/09/2014.
        /// Cambiar de página de la grilla.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_InformesVisita_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            var dt = HttpContext.Current.Session["grilla_informes"] as DataTable;

            if (dt != null)
            {
                gv_InformesVisita.PageIndex = e.NewPageIndex;
                gv_InformesVisita.DataSource = dt;
                gv_InformesVisita.DataBind();
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 16/09/2014.
        /// RowCommand.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_InformesVisita_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string[] paramentros = e.CommandArgument.ToString().Split(';');

            switch (e.CommandName)
            {
                case "VerInformeVisita":
                    HttpContext.Current.Session["CodInforme"] = paramentros[0];
                    HttpContext.Current.Session["CodAspecto"] = "1";
                    if (Int32.Parse(paramentros[0]) == 0)
                    {
                        //Se le envía el código de la empresa.
                        HttpContext.Current.Session["CodEmpresa"] = CodEmpresa.Value;

                        //Se usan los valores para consultar.
                        Response.Redirect("AdicionarInformeVisitaProyecto.aspx");
                    }
                    else
                    {
                        //Determinar la acción a realizar en la siguiente página.
                        HttpContext.Current.Session["Accion"] = "Consultar";

                        //Se usan los valores para consultar.
                        Response.Redirect("AdicionarInformeVisitaProyecto.aspx");
                    }
                    break;
                default:
                    break;
            }
        }
    }
}