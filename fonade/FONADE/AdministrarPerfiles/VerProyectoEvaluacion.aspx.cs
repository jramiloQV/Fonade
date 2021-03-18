using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;

namespace Fonade.FONADE.AdministrarPerfiles
{
    /// <summary>
    /// VerProyecto
    /// </summary>    
    public partial class VerProyecto : Negocio.Base_Page
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the Selecting event of the lds_proyectos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LinqDataSourceSelectEventArgs"/> instance containing the event data.</param>
        protected void lds_proyectos_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            // LINQ query

            var query = from p in consultas.Db.Proyecto
                        from c in consultas.Db.Ciudad
                        from d in consultas.Db.departamento
                        from i in consultas.Db.Institucions
                        where c.Id_Ciudad == p.CodCiudad
                        & c.CodDepartamento == d.Id_Departamento
                        & p.CodInstitucion == i.Id_Institucion
                        select new
                        {
                            IdProyecto = p.Id_Proyecto,
                            NombreProyecto = p.NomProyecto,
                            CodigoInstitucion = i.Id_Institucion,
                            CodigoEstado = p.CodEstado,
                            NombreUnidad = i.NomUnidad,
                            NombreInstitucion = i.NomInstitucion,
                            NombreCiudad = c.NomCiudad,
                            NombreDepartamento = d.NomDepartamento,
                            Inactivo = p.Inactivo
                        };

            switch (usuario.CodGrupo)
            {
                case Constantes.CONST_AdministradorSistema:
                case Constantes.CONST_AdministradorSena:
                    query = query.Where(p => p.Inactivo == false);
                    break;
                case Constantes.CONST_JefeUnidad:
                    query = query.Where(p => p.CodigoInstitucion == usuario.CodInstitucion);
                    break;
                case Constantes.CONST_Asesor:
                case Constantes.CONST_Emprendedor:
                    query = query.Where(v => (consultas.Db.ProyectoContactos.Where(p => p.Proyecto.Id_Proyecto == p.CodProyecto
                        && p.CodContacto == usuario.IdContacto
                        && p.Inactivo == false).Select(t => t.CodProyecto)).Contains(v.IdProyecto)
                        && v.Inactivo == false
                        && v.CodigoInstitucion == usuario.CodInstitucion);
                    break;
                case Constantes.CONST_Evaluador:
                case Constantes.CONST_CoordinadorEvaluador:

                    query = query.Where(v => (consultas.Db.ProyectoContactos.Where(p => p.Proyecto.Id_Proyecto == p.CodProyecto
                        && p.CodContacto == usuario.IdContacto
                        && p.Inactivo == false).Select(t => t.CodProyecto)).Contains(v.IdProyecto)
                       && v.Inactivo == false
                       && v.CodigoEstado == Constantes.CONST_Evaluacion);
                    break;
                case Constantes.CONST_GerenteEvaluador:
                    query = query.Where(p => p.Inactivo == false
                        && (p.CodigoEstado == Constantes.CONST_Convocatoria
                        || p.CodigoEstado == Constantes.CONST_Evaluacion));
                    break;
                default:
                    break;
            }

            switch (gw_proyectos.SortExpression)
            {
                case "NombreCiudad":
                    if (gw_proyectos.SortDirection == SortDirection.Ascending)
                        query = query.OrderBy(t => t.NombreCiudad);
                    else
                        query = query.OrderByDescending(t => t.NombreCiudad);
                    break;
                case "NombreProyecto":
                    if (gw_proyectos.SortDirection == SortDirection.Ascending)
                        query = query.OrderBy(t => t.NombreProyecto);
                    else
                        query = query.OrderByDescending(t => t.NombreProyecto);
                    break;
            }

            // Do advanced query logic here (dynamically build WHERE clause, etc.)     

            // Set the total count     
            // so GridView knows how many pages to create    
            e.Arguments.TotalRowCount = query.Count();

            // Get only the rows we need for the page requested
            query = query.Skip(gw_proyectos.PageIndex * 10).Take(10);

            e.Result = query;
        }

        /// <summary>
        /// Handles the RowDataBound event of the gw_proyectos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void gw_proyectos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
             

                if (usuario.CodGrupo == Constantes.CONST_GerenteEvaluador)
                {
                    var query = (from cp in consultas.Db.ConvocatoriaProyectos
                                 from c in consultas.Db.Convocatoria
                                 where c.Id_Convocatoria == cp.CodConvocatoria
                                 && cp.CodProyecto == Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Id_Proyecto"))
                                 select new { cp.CodConvocatoria, c.NomConvocatoria, cp.Fecha }).OrderByDescending(t => t.Fecha).Take(1);

                    //Make an adition

                    var firstOrDefault = query.FirstOrDefault();
                    if (firstOrDefault != null)
                        ((HyperLink)e.Row.Cells[0].FindControl("hl_evaluacion")).NavigateUrl = "EvaluacionFrameSet.asp?CodProyecto="
                                                                                               + DataBinder.Eval(e.Row.DataItem, "IdProyecto")
                                                                                               + "&CodConvocatoria=" + firstOrDefault.CodConvocatoria;
                }

                #region metodos quitados
                //Unidad Reasignar
                //if (usuario.CodGrupo == Constantes.CONST_AdministradorFonade || usuario.CodGrupo == Constantes.CONST_AdministradorSena)
                //{
                //    if (usuario.CodInstitucion != Constantes.CONST_UnidadTemporal)
                //    {
                //        ((LinkButton)e.Row.Cells[0].FindControl("lbtn_reasignar")).Enabled = false;
                //    }
                //}

                //inactivo
                //if (usuario.CodGrupo == Constantes.CONST_JefeUnidad)
                //{
                //    if (Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "Inactivo")) == false)
                //    {
                //        LinkButton lkb = ((LinkButton)e.Row.Cells[0].FindControl("lbtn_inactivo"));
                //        lkb.Text = "Activo";
                //        lkb.Enabled = false;
                //    }
                //}

                //

                //if (usuario.CodGrupo == Constantes.CONST_JefeUnidad)
                //{
                //    if (Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "CodigoEstado")) == Constantes.CONST_Inscripcion)
                //    {
                //        if (Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "Inactivo")) == false)
                //        {
                //            ((ImageButton)e.Row.Cells[0].FindControl("ibtn_Inactivar")).Visible = true;
                //        }
                //        else
                //        {
                //            ((ImageButton)e.Row.Cells[0].FindControl("ibtn_Activar")).Visible = true;
                //        }
                //    }
                //}
                #endregion
            }
        }

        /// <summary>
        /// Handles the DataBound event of the gw_proyectos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void gw_proyectos_DataBound(object sender, EventArgs e)
        {
            //
            if (usuario.CodGrupo == Constantes.CONST_GerenteEvaluador)
            {
                gw_proyectos.Columns[2].Visible = true;
                gw_proyectos.Columns[3].Visible = false;
            }
            else
            {
                gw_proyectos.Columns[2].Visible = false;
                gw_proyectos.Columns[3].Visible = true;
            }

            //inactivo
            if (usuario.CodGrupo == Constantes.CONST_JefeUnidad)
            {
                gw_proyectos.Columns[5].Visible = true;
            }
            else
            {
                gw_proyectos.Columns[5].Visible = false;
            }




        }

        /// <summary>
        /// Handles the RowCommand event of the gw_proyectos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewCommandEventArgs"/> instance containing the event data.</param>
        protected void gw_proyectos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string IdProyecto = e.CommandArgument.ToString();
           
        }
    }
}