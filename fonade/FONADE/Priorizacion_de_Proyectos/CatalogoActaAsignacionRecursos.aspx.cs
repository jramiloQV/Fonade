using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Fonade.Account;
using LinqKit;
using AjaxControlToolkit;
using System.ComponentModel;

namespace Fonade.FONADE.Priorizacion_de_Proyectos
{
    public partial class CatalogoActaAsignacionRecursos : Negocio.Base_Page
    {

        public const int PAGE_SIZE = 50;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lbl_Titulo.Text = void_establecerTitulo("LISTADO DE ACTAS");
            }
        }

        protected void GridViewProyectos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "VerActa")
            {
                string IdVerActa = e.CommandArgument.ToString();
                HttpContext.Current.Session["Id_Acta"] = IdVerActa;
                Response.Redirect("VerActaAsignacionRecursos.aspx");
            }
        }

        protected void lds_listadoActas_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {

            try
            {
                var query = from P in consultas.VerListadoActas()
                            select P;

                switch (GridViewActas.SortExpression)
                {
                    case "numacta":
                        if (GridViewActas.SortDirection == SortDirection.Ascending)
                            query = query.OrderBy(t => t.numacta);
                        else
                            query = query.OrderByDescending(t => t.numacta);
                        break;
                    case "nomacta":
                        if (GridViewActas.SortDirection == SortDirection.Ascending)
                            query = query.OrderBy(t => t.nomacta);
                        else
                            query = query.OrderByDescending(t => t.numacta);
                        break;
                    case "nomconvocatoria":
                        if (GridViewActas.SortDirection == SortDirection.Ascending)
                            query = query.OrderBy(t => t.nomconvocatoria);
                        else
                            query = query.OrderByDescending(t => t.numacta);
                        break;
                    case "NombreOperador":
                        if (GridViewActas.SortDirection == SortDirection.Ascending)
                            query = query.OrderBy(t => t.nomconvocatoria);
                        else
                            query = query.OrderByDescending(t => t.numacta);
                        break;
                }


                e.Arguments.TotalRowCount = query.Count();

                query = query.Skip(GridViewActas.PageIndex * PAGE_SIZE).Take(PAGE_SIZE);

                e.Result = query;
            }
            catch (Exception exc)
            { }

        }
    }
}