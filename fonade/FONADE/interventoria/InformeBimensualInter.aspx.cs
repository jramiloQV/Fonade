using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;

namespace Fonade.FONADE.interventoria
{
    public partial class InformeBimensualInter : Negocio.Base_Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = "FONDO EMPRENDER - Mis Proyectos para Seguimiento de INTERVENTORÍA";

            if (!IsPostBack)
            {
                llenarData();

                if (usuario.CodGrupo == Constantes.CONST_Interventor || usuario.CodGrupo == Constantes.CONST_RolInterventorLider)
                { pnl_adicionar_informe_visita.Visible = true; CargarListaEmpresas(); }
            }
        }

        private void llenarData(string filtro = null)
        {
            if (!string.IsNullOrEmpty(dd_Empresas.SelectedValue))
                filtro = dd_Empresas.SelectedValue;

            String sqlConsulta = "";
            DataTable tabla = new DataTable();

            try
            {
                if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                {
                    sqlConsulta = "";
                    sqlConsulta = " SELECT InformeBimensual.* FROM InformeBimensual " +
                                  " INNER JOIN Interventor ON InformeBimensual.codinterventor = Interventor.CodContacto " +
                                  " inner join contacto c on Interventor.CodContacto = c.id_contacto " +
                                  " WHERE c.codOperador = " + usuario.CodOperador +
                                  " and (UPPER(InformeBimensual.NomInformeBimensual) LIKE '%%') ";
                }
                else if (usuario.CodGrupo == Constantes.CONST_Interventor)
                    {
                        sqlConsulta = "";
                        sqlConsulta = " SELECT * FROM InformeBimensual WHERE codinterventor = " + usuario.IdContacto +
                                      " AND (UPPER (NomInformeBimensual) LIKE '%%') ";
                    }
                    else
                    {
                        //Para cualquier otro Rol "al parecer"...
                        sqlConsulta = "";
                        sqlConsulta = " SELECT InformeBimensual.* FROM InformeBimensual " +
                                      " INNER JOIN Interventor ON InformeBimensual.codinterventor = Interventor.CodContacto " +
                                      " WHERE (UPPER(InformeBimensual.NomInformeBimensual) LIKE '%%') " +
                                      " AND (Interventor.CodCoordinador = " + usuario.IdContacto + ") ";
                    }
                if (!string.IsNullOrEmpty(sqlConsulta))
                {
                    sqlConsulta = sqlConsulta + " and InformeBimensual.NomInformeBimensual like '" + filtro + "%'";
                }

                sqlConsulta = sqlConsulta + " ORDER BY InformeBimensual.periodo";

                if (sqlConsulta.Trim() != "")
                {
                    //Asignar resultados de la consulta a variable DataTable.
                    tabla = consultas.ObtenerDataTable(sqlConsulta, "text");

                    HttpContext.Current.Session["dtEmpresas"] = tabla;
                    gv_informesinterventoria.DataSource = tabla;
                    gv_informesinterventoria.DataBind();
                }
            }
            catch { }

            #region Comentarios.
            //if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor || usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
            //{
            //    var result = from r in consultas.VerInformeBimensualInterventoria(usuario.CodGrupo, usuario.IdContacto)

            //                 select new
            //                 {
            //                     Id_InformeBimensual = r.id_informeBimensual,
            //                     Periodo = r.Periodo,
            //                     CodEmpresa = r.codempresa,
            //                     NomInformeBimensual = r.NomInformeBimensual
            //                 };

            //    gv_informesinterventoria.DataSource = result;
            //    gv_informesinterventoria.DataBind();
            //}  
            #endregion
        }

        private void CargarListaEmpresas()
        {
            String sqlConsulta = "";
            DataTable tabla = new DataTable();

            try
            {
                dd_Empresas.Items.Clear();
                sqlConsulta = " SELECT id_empresa, razonsocial FROM Empresa " +
                              " WHERE (id_empresa IN (SELECT CodEmpresa FROM EmpresaInterventor " +
                              " WHERE Inactivo=0 AND CodContacto = " + usuario.IdContacto + ")) ORDER BY razonsocial ";


                tabla = consultas.ObtenerDataTable(sqlConsulta, "text");

                ListItem item_default = new ListItem();
                item_default.Value = "";
                item_default.Text = "Seleccione la Empresa para Adicionar el Informe Bimensual";
                dd_Empresas.Items.Add(item_default);

                for (int i = 0; i < tabla.Rows.Count; i++)
                {
                    string valor = tabla.Rows[i]["razonsocial"].ToString();
                    if (valor.Contains("CONFECCIONES KIOTO")) { tabla.Rows[i]["razonsocial"] = HttpUtility.HtmlEncode(valor); }
                    ListItem item = new ListItem();
                    item.Value = tabla.Rows[i]["id_empresa"].ToString();
                    item.Text = tabla.Rows[i]["razonsocial"].ToString();
                    dd_Empresas.Items.Add(item);
                }
            }
            catch
            {
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo cargar el listado de empresas.')", true);
                return;
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

        protected void gv_informesinterventoria_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "mostrar")
            {
                try
                {
                    string valoresArgument = Convert.ToString(e.CommandArgument);

                    string[] argument = valoresArgument.Split(';');

                    HttpContext.Current.Session["CodInforme"] = argument[0];
                    HttpContext.Current.Session["PeriodoBimensual"] = argument[1];
                    HttpContext.Current.Session["CodEmpresa"] = argument[2];

                    if (usuario.CodGrupo == Constantes.CONST_Interventor)
                    {
                        Response.Redirect("AdicionarInformeBimensual.aspx");
                    }
                    else
                    {
                        Response.Redirect("AdicionarInformeBimensualInter.aspx");
                    }
                }
                catch (IndexOutOfRangeException) { }
            }
        }

        protected void gv_informesinterventoria_RowCreated(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gv_informesinterventoria_Sorting(object sender, GridViewSortEventArgs e)
        {
            var dt = HttpContext.Current.Session["dtEmpresas"] as DataTable;

            if (dt != null)
            {
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gv_informesinterventoria.DataSource = dt;
                gv_informesinterventoria.DataBind();
            }
        }

        //protected void ldsavancebimensula_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        //{
        //    if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor || usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
        //    {
        //        var result = from r in consultas.VerInformeBimensualInterventoria(usuario.CodGrupo, usuario.IdContacto)

        //                     select new
        //                     {
        //                         Id_InformeBimensual = r.id_informeBimensual,
        //                         Periodo = r.Periodo,
        //                         CodEmpresa = r.codempresa,
        //                         NomInformeBimensual = r.NomInformeBimensual
        //                     };
        //        e.Result = result.ToList();
        //    }
        //} 

        protected void gv_informesinterventoria_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            var dt = HttpContext.Current.Session["dtEmpresas"] as DataTable;

            if (dt != null)
            {
                gv_informesinterventoria.PageIndex = e.NewPageIndex;
                gv_informesinterventoria.DataSource = dt;
                gv_informesinterventoria.DataBind();
            }
        }

        protected void btn_adicionar_informe_visita_Click(object sender, EventArgs e)
        {
            if (dd_Empresas.SelectedValue != "")
            {
                HttpContext.Current.Session["CodEmpresa"] = dd_Empresas.SelectedValue;
                HttpContext.Current.Session["NEW_INFORME"] = "NUEVO";

                if (usuario.CodGrupo == Constantes.CONST_Interventor)
                { Response.Redirect("AdicionarInformeBimensual.aspx"); }
                else
                { Response.Redirect("AdicionarInformeBimensualInter.aspx"); }
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Seleccione la Empresa para Adicionar el Informe Bimensual')", true);
                return;
            }
        }

        protected void ddlbuscar_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenarData(ddlbuscar.SelectedValue);
        }
    }
}