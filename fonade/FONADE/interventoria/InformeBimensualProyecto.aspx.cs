using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Datos;

namespace Fonade.FONADE.interventoria
{
    public partial class InformeBimensualProyecto : Negocio.Base_Page
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
            {
                CargarTablaInformesBimensual();

                #region Obtener el rol.

                //Consulta.
                txtSQL = " SELECT CodContacto, CodRol From ProyectoContacto " +
                         " Where CodProyecto = " + CodProyecto + " And CodContacto = " + usuario.IdContacto +
                         " and inactivo=0 and FechaInicio<=getdate() and FechaFin is null ";

                //Asignar variables a DataTable.
                var rs = consultas.ObtenerDataTable(txtSQL, "text");

                //Crear la variable de sesión.
                HttpContext.Current.Session["CodRol"] = rs.Rows.Count>0? rs.Rows[0]["CodRol"].ToString():string.Empty;

                //Destruir la variable.
                rs = null;

                #endregion

                //Cargar las empresas y mostrar el control sólo si se cumple esta condición.
                if (usuario.CodGrupo == Constantes.CONST_Interventor && HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolInterventorLider.ToString())
                { CargarEmpresas(); tr_adicionar.Visible = true; }
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
        /// Mauricio Arias Olave.
        /// 16/09/2014.
        /// Cargar la grilla de informes bimensuales para su selección.
        /// </summary>
        private void CargarTablaInformesBimensual()
        {
            //Inicializar variables.
            DataTable rs = new DataTable();

            try
            {
                //txtSQL = "SELECT * FROM InformeBimensual WHERE Estado = 3 AND CodEmpresa = (SELECT Id_Empresa FROM Empresa WHERE CodProyecto = " + CodProyecto + ") order by periodo desc";
                txtSQL = "Select *, e.codProyecto from InformeBimensual ib Inner Join Empresa e on e.id_Empresa = ib.CodEmpresa ";
                txtSQL += "where e.codproyecto = "+ CodProyecto +" Order by Periodo desc";
                rs = consultas.ObtenerDataTable(txtSQL, "text");

                //Para mejorar el performance, sólo se agrega a la variable de sesión
                //la grilla cuando ésta tenga mas de 10* registros.
                //* Número de registros por página de la grilla.
                if (rs.Rows.Count > 10) { HttpContext.Current.Session["grilla_informes"] = rs; }

                gv_InformesBimensuales.DataSource = rs;
                gv_InformesBimensuales.DataBind();
            }
            catch { }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 16/09/2014.
        /// Cargar el DropDownList para la selección de empresas.
        /// </summary>
        private void CargarEmpresas()
        {
            //Inicializar variables
            ListItem item = new ListItem();
            DataTable RS = new DataTable();

            try
            {
                txtSQL = "SELECT id_empresa, razonsocial FROM Empresa WHERE (id_empresa IN (SELECT CodEmpresa FROM EmpresaInterventor WHERE Inactivo=0 AND CodContacto = " + usuario.IdContacto + ")) ORDER BY razonsocial";
                RS = consultas.ObtenerDataTable(txtSQL, "text");

                //Limpiar posibles ítems que tenga el DropDownLists.
                CodEmpresa.Items.Clear();

                //Añadir ítem defualt.
                item = new ListItem();
                item.Value = "";
                item.Text = "Seleccione la Empresa para Adicionar el Informe Bimensual";
                CodEmpresa.Items.Add(item);

                foreach (DataRow row in RS.Rows)
                {
                    item = new ListItem();
                    item.Value = row["id_empresa"].ToString();
                    item.Text = row["razonsocial"].ToString();
                    CodEmpresa.Items.Add(item);
                }
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
        protected void gv_InformesBimensuales_Sorting(object sender, GridViewSortEventArgs e)
        {
            var dt = HttpContext.Current.Session["grilla_informes"] as DataTable;

            if (dt != null)
            {
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gv_InformesBimensuales.DataSource = dt;
                gv_InformesBimensuales.DataBind();
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 16/09/2014.
        /// Cambiar de página de la grilla.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_InformesBimensuales_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            var dt = HttpContext.Current.Session["grilla_informes"] as DataTable;

            if (dt != null)
            {
                gv_InformesBimensuales.PageIndex = e.NewPageIndex;
                gv_InformesBimensuales.DataSource = dt;
                gv_InformesBimensuales.DataBind();
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 16/09/2014.
        /// RowCommand.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_InformesBimensuales_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string[] parametros = e.CommandArgument.ToString().Split(';');

            switch (e.CommandName)
            {
                case "VerInformeBimensual":

                    //Asignar los valores a las variables de sesión.
                    HttpContext.Current.Session["CodInforme"] = parametros[0];
                    HttpContext.Current.Session["Periodo"] = parametros[1];
                    HttpContext.Current.Session["CodEmpresa"] = parametros[2];
                    HttpContext.Current.Session["CodProyecto"] = parametros[3];

                    //Se usan los valores para consultar.
                    Response.Redirect("AdicionarInformeBimensualProyecto.aspx");
                    break;
                default:
                    break;
            }
        }
    }
}