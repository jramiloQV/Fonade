using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using Fonade.Account;

namespace Fonade.FONADE.Proyecto
{
    public partial class ProyectoFormalizar : Negocio.Base_Page
    {
        public const int PAGE_SIZE = 10;
        ValidacionCuenta validacionCuenta = new ValidacionCuenta();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string pathRuta = HttpContext.Current.Request.Url.AbsolutePath;

                if (!validacionCuenta.validarPermiso(usuario.IdContacto, pathRuta))//Se valida si el usuario tiene permisos de acceso a la pagina, en caso de no serlo se redirige al Home
                {
                    Response.Redirect(validacionCuenta.rutaHome(), true);
                }
                else
                {
                    lbl_Titulo.Text = void_establecerTitulo("PROYECTOS A FORMALIZAR");
                }
            }
        }

        protected void lds_proyectos_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            // LINQ query

            try
            {
                var query = from P in consultas.FormalizarProyecto(usuario.IdContacto, Constantes.CONST_Anexos, usuario.CodInstitucion, Constantes.CONST_Inscripcion)
                            select P;

                switch (GridViewProyectos.SortExpression)
                {
                    case "Lugar":
                        if (GridViewProyectos.SortDirection == SortDirection.Ascending)
                            query = query.OrderBy(t => t.Lugar);
                        else
                            query = query.OrderByDescending(t => t.nomconvocatoria);
                        break;
                    case "nomproyecto":
                        if (GridViewProyectos.SortDirection == SortDirection.Ascending)
                            query = query.OrderBy(t => t.nomproyecto);
                        else
                            query = query.OrderByDescending(t => t.nomconvocatoria);
                        break;
                    case "nomconvocatoria":
                        if (GridViewProyectos.SortDirection == SortDirection.Ascending)
                            query = query.OrderBy(t => t.nomconvocatoria);
                        else
                            query = query.OrderByDescending(t => t.nomconvocatoria);
                        break;
                }


                e.Arguments.TotalRowCount = query.Count();

                // Get only the rows we need for the page requested
                query = query.Skip(GridViewProyectos.PageIndex * PAGE_SIZE).Take(PAGE_SIZE);

                e.Result = query;
            }
            catch (Exception exc)
            { }

        }

        protected void GridViewProyectos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Proyecto"))
            {
                HttpContext.Current.Session["CodProyecto"] = e.CommandArgument.ToString();
                Response.Redirect("ProyectoFrameSet.aspx");
            }
            else
            {
                if (e.CommandName.Equals("adiocional"))
                {
                    Response.Redirect(e.CommandArgument.ToString());
                }
                else
                {
                    //Diego Quiñonez - 26 de Diciembre de 2014
                    if (e.CommandName.Equals("VerAnexos"))
                    {
                        HttpContext.Current.Session["CodProyecto"] = e.CommandArgument;
                        HttpContext.Current.Session["Accion"] = "Vista";
                        Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
                    }
                    if (e.CommandName.Equals("VerAnexosAcreditacion"))
                    {
                        HttpContext.Current.Session["CodProyecto"] = e.CommandArgument;
                        HttpContext.Current.Session["Accion"] = "Acreditacion";
                        Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
                    }
                }
            }
        }

        /// <summary>
        /// Diego Quiñonez - 26 de Diciembre de 2014
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridViewProyectos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var btnAnexos = (Button)e.Row.FindControl("btnAnexos");
                var btnAdicional = (Button)e.Row.FindControl("btnadicional");

                if (btnAdicional.Text.Contains("Formalizar"))
                {
                    btnAnexos.Enabled = false;
                    btnAnexos.Visible = false;
                }
                else
                {
                    btnAnexos.Enabled = true;
                    btnAnexos.Visible = true;
                }
            }
        }

    }
}