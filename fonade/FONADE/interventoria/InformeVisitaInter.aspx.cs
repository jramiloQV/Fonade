using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;

namespace Fonade.FONADE.interventoria
{
    public partial class InformeVisitaInter : Negocio.Base_Page
    {
        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (usuario.CodGrupo == Constantes.CONST_Interventor || usuario.CodGrupo == Constantes.CONST_RolInterventorLider)
                { pnl_adicionar_informe_visita.Visible = true; CargarListaEmpresas(); }
            }
        }

        private void CargarListaEmpresas()
        {
            //Inicializar variables.
            String sqlConsulta = "";
            DataTable tabla = new DataTable();

            try
            {
                //Limpiar DropDownList "por si algo"...
                dd_Empresas.Items.Clear();

                //Consulta:
                sqlConsulta = " SELECT id_empresa, razonsocial " +
                              " FROM Empresa WHERE (id_empresa IN (SELECT CodEmpresa FROM EmpresaInterventor " +
                              " WHERE CodContacto = " + usuario.IdContacto + ")) ORDER BY razonsocial ";

                //Asignar resultados de la consulta a variable DataTable.
                tabla = consultas.ObtenerDataTable(sqlConsulta, "text");

                //Crear ítem por defecto.
                ListItem item_default = new ListItem();
                item_default.Value = "";
                item_default.Text = "Seleccione la Empresa para Adicionar el Informe de Visita";
                dd_Empresas.Items.Add(item_default);

                //Recorrer la lista y generar ListItems.
                for (int i = 0; i < tabla.Rows.Count; i++)
                {
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

        protected void gv_informesinterventoria_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "MostrarInforme")
            {
                //Separar los valores.
                var valores_command = new string[] { };
                valores_command = e.CommandArgument.ToString().Split(';');
                HttpContext.Current.Session["InformeIdVisita"] = valores_command[0]; //Convert.ToInt32(e.CommandArgument);
                HttpContext.Current.Session["Nuevo"] = "False";
                Response.Redirect("AdicionarInformeVisita.aspx?Accion=Consultar");
            }
            HttpContext.Current.Session["InformeIdVisita"] = "";
        }

        protected void gv_informesinterventoria_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_informesinterventoria.PageIndex = e.NewPageIndex;
        }

        protected void gv_informesinterventoria_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void btn_adicionar_informe_visita_Click(object sender, EventArgs e)
        {
            //Si ha seleccionado una empresa para generarle el informe de visita, redirige al usuario a la página
            //para crear el nuevo informe de visita.
            if (dd_Empresas.SelectedValue != "")
            {
                HttpContext.Current.Session["InformeIdVisita"] = dd_Empresas.SelectedValue;
                HttpContext.Current.Session["Nombre_Empresa"] = dd_Empresas.SelectedItem.Text;
                HttpContext.Current.Session["Nuevo"] = "Nuevo";
                Response.Redirect("AdicionarInformeVisita.aspx");
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Seleccione una Empresa para generar el informe de Visita')", true);
                return;
            }
        }

        protected void ddlbuscar_SelectedIndexChanged(object sender, EventArgs e)
        {
            gv_informesinterventoria.DataBind();
        }

        protected void lds_informes_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            switch (usuario.CodGrupo)
            {
                case Constantes.CONST_GerenteInterventor:
                    var result = (from i in consultas.Db.InformeVisitaInterventoria
                                  join c in consultas.Db.Contacto on i.CodInterventor equals c.Id_Contacto
                                  where c.codOperador == usuario.CodOperador
                                  select new
                                  {
                                      NUM = 0,
                                      i.Id_Informe,
                                      i.NombreInforme
                                  });

                    if (!string.IsNullOrEmpty(ddlbuscar.SelectedValue))
                        result = result.Where(x=> x.NombreInforme.StartsWith(ddlbuscar.SelectedValue));

                    e.Result = result.ToList();
                    break;
                case Constantes.CONST_CoordinadorInterventor:

                    var result2 = (from ii in consultas.Db.InformeVisitaInterventoria
                                   join i in consultas.Db.Interventors on ii.CodInterventor equals i.CodContacto
                                   where i.CodCoordinador == usuario.IdContacto
                                   orderby ii.NombreInforme
                                   select new
                                   {
                                       NUM = 0,
                                       ii.Id_Informe,
                                       ii.NombreInforme
                                   });

                    if (!string.IsNullOrEmpty(ddlbuscar.SelectedValue))
                        result2 = result2.Where(x => x.NombreInforme.StartsWith(ddlbuscar.SelectedValue));

                    e.Result = result2.ToList();

                    break;
                default:
                    var result3 = (from i in consultas.Db.InformeVisitaInterventoria
                                   where i.CodInterventor == usuario.IdContacto
                                   select new
                                   {
                                       NUM = 0,
                                       i.Id_Informe,
                                       i.NombreInforme
                                   });

                    if (!string.IsNullOrEmpty(ddlbuscar.SelectedValue))
                        result3 = result3.Where(x => x.NombreInforme.StartsWith(ddlbuscar.SelectedValue));

                    e.Result = result3.ToList();
                    break;
            }
        }

        protected void gv_informesinterventoria_DataBound(object sender, EventArgs e)
        {
            int i = 1;

            foreach(GridViewRow gvr in gv_informesinterventoria.Rows)
            {
                ((Label)gvr.FindControl("lbl_numero")).Text = i.ToString();
                i++;
            }
        }
    }
}