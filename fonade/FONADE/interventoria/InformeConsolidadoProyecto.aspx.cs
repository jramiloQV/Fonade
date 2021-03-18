using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Fonade.FONADE.interventoria
{
    public partial class InformeConsolidadoProyecto : Negocio.Base_Page
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
            { CargarTablaInformesConsolidado(); }
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
        /// Cargar la grilla de informes de consolidado para su selección.
        /// </summary>
        private void CargarTablaInformesConsolidado()
        {
            //Inicializar variables.
            DataTable RS = new DataTable();

            try
            {
                //txtSQL = "SELECT * FROM InterventorInformeFinal WHERE Estado = 3 AND CodEmpresa = ((SELECT Id_Empresa FROM Empresa WHERE CodProyecto = " + CodProyecto + ")) order by NomInterventorInformeFinal DESC";
                txtSQL = "Select ifi.*, e.codproyecto from InterventorInformeFinal ifi Inner Join empresa e on e.id_empresa = ifi.CodEmpresa ";
                txtSQL += "Where e.codproyecto = "+ CodProyecto +" Order By NomInterventorInformeFinal DESC";
                RS = consultas.ObtenerDataTable(txtSQL, "text");

                #region Generar estado "nombre del estado" de acuerdo a FONADE clásico.

                //Añadir columna adicional "para mostrar el nombre del estado".
                RS.Columns.Add("NombreEstado", typeof(System.String));

                //Recorrer las filas del DataTable y añadir el nombre del estado en la columna creada.
                foreach (DataRow row in RS.Rows)
                {
                    switch (row["Estado"].ToString())
                    {
                        case "0":
                            row["NombreEstado"] = "Edición";
                            break;
                        case "1":
                            row["NombreEstado"] = "Enviado a Coordinador";
                            break;
                        case "2":
                            row["NombreEstado"] = "Aprobado Coordinador";
                            break;
                        case "3":
                            row["NombreEstado"] = "Aprobado Gerente Interventor";
                            break;
                        default:
                            break;
                    }
                }

                #endregion

                //Para mejorar el performance, sólo se agrega a la variable de sesión
                //la grilla cuando ésta tenga mas de 10* registros.
                //* Número de registros por página de la grilla.
                if (RS.Rows.Count > 10) { HttpContext.Current.Session["grilla_informes"] = RS; }

                gv_InformesConsolidados.DataSource = RS;
                gv_InformesConsolidados.DataBind();
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
        protected void gv_InformesConsolidados_Sorting(object sender, GridViewSortEventArgs e)
        {
            var dt = HttpContext.Current.Session["grilla_informes"] as DataTable;

            if (dt != null)
            {
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gv_InformesConsolidados.DataSource = dt;
                gv_InformesConsolidados.DataBind();
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 16/09/2014.
        /// Cambiar de página de la grilla.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_InformesConsolidados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            var dt = HttpContext.Current.Session["grilla_informes"] as DataTable;

            if (dt != null)
            {
                gv_InformesConsolidados.PageIndex = e.NewPageIndex;
                gv_InformesConsolidados.DataSource = dt;
                gv_InformesConsolidados.DataBind();
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 16/09/2014.
        /// RowCommand.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_InformesConsolidados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string[] parametros = e.CommandArgument.ToString().Split(';');

            switch (e.CommandName)
            {
                case "VerInformeConsolidado":
                    //Asignar los valores a las variables de sesión.
                    HttpContext.Current.Session["Accion"] = "Editar";
                    HttpContext.Current.Session["CodInforme"] = parametros[0];
                    HttpContext.Current.Session["CodEmpresa"] = parametros[1];
                    HttpContext.Current.Session["CodProyecto"] = parametros[2];

                    //Se usan los valores para consultar.
                    Response.Redirect("AgregarInformeFinalInterventoriaProyecto.aspx");
                    break;
                default:
                    break;
            }
        }
    }
}