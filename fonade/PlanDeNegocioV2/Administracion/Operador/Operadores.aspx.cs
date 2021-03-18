using Fonade.Account;
using Fonade.Negocio.PlanDeNegocioV2.Administracion.Operador;
using Microsoft.ReportingServices.Diagnostics.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Negocio.Utilidades;

namespace Fonade.PlanDeNegocioV2.Administracion.Operador
{
    public partial class Operadores : Negocio.Base_Page
    {
        protected FonadeUser Usuario
        {
            get
            {
                return HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true);
            }
            set
            {
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ValidateUsers();

                cargarOperadores();

            }

        }

        public void ValidateUsers()
        {
            try
            {
                if (!(Usuario.CodGrupo == Datos.Constantes.CONST_AdministradorSistema))
                {
                    Response.Redirect("~/FONADE/evaluacion/AccesoDenegado.aspx");
                }
            }
            catch (Exception)
            {
                Response.Redirect("~/FONADE/evaluacion/AccesoDenegado.aspx");
            }
        }

        OperadorController operadorController = new OperadorController();

        private void cargarOperadores()
        {
            var query = operadorController.getAllOperador();
            gvOperadores.DataSource = query;
            Session["dtOperadores"] = query;
            gvOperadores.DataBind();
        }

        protected void gvOperadores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                if (e.CommandArgument != null)
                {
                    var idOperador = Convert.ToInt32(e.CommandArgument.ToString());
                    Session["idOperador"] = idOperador;
                    Response.Redirect("~/PlanDeNegocioV2/Administracion/Operador/NuevoOperador.aspx");
                }
            }
            if (e.CommandName.Equals("Eliminar"))
            {
                if (e.CommandArgument != null)
                {
                    var idOperador = Convert.ToInt32(e.CommandArgument.ToString());
                    string mensaje = "";
                    if (desactivarOperador(idOperador, ref mensaje))
                    {
                        Alert("Se eliminó exitosamente el operador seleccionado", "Operadores.aspx");
                    }
                    else
                    {
                        Alert(mensaje);
                    }
                }
            }
        }

        private void Alert(string mensaje, string ruta = "")
        {
            if (ruta == "")
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
            else
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');" +
                    "window.location.href='" + ruta + "';", true);
        }

        private bool desactivarOperador(int _codOperador, ref string mensaje)
        {
            bool desactivado = operadorController.desactivarOperador(_codOperador, ref mensaje);
            return desactivado;
        }

        protected void lnk_Click(object sender, EventArgs e)
        {
            Session["idOperador"] = null;
            Response.Redirect("~/PlanDeNegocioV2/Administracion/Operador/NuevoOperador.aspx");
        }

        protected void gvOperadores_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<OperadorModel> query = (IEnumerable<OperadorModel>)Session["dtOperadores"];

            e.SortDirection = GetSortDirection(e.SortExpression);

            query = query.OrderBy(e.SortExpression, e.SortDirection);
            
            gvOperadores.DataSource = query.ToArray();
            gvOperadores.DataBind();
        }

        private SortDirection GetSortDirection(string column)
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

            return sortDirection == "ASC" ? SortDirection.Ascending:SortDirection.Descending;
        }
    }
}