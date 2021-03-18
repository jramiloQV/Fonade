using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using System.Configuration;
using Datos.Modelos;
using Fonade.Negocio.PlanDeNegocioV2.Administracion.Operador;

namespace Fonade.FONADE.Convocatoria
{
    /// <summary>
    /// CatalogoConvocatoria1
    /// </summary>    
    public partial class CatalogoConvocatoria1 : Negocio.Base_Page
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
        /// Handles the RowCommand event of the GridViewConvoct control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewCommandEventArgs"/> instance containing the event data.</param>
        protected void GridViewConvoct_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToString().Equals("VerProyectosConvatoria"))
            {
                string IdConvoct = e.CommandArgument.ToString();
                HttpContext.Current.Session["Id_ProyPorConvoct"] = IdConvoct;
                Response.Redirect("ProyectosPorConvocatoria.aspx");
            }
            if (e.CommandName.ToString().Equals("VerConvocatoria"))
            {
                string IdConvoct = e.CommandArgument.ToString();
                HttpContext.Current.Session["IdConvocatoria"] = IdConvoct;
                Response.Redirect("Convocatoria.aspx");
            }
            if (e.CommandName.ToString().Equals("VerEvalConvatoria"))
            {
                string IdConvoct = e.CommandArgument.ToString();
                HttpContext.Current.Session["Id_EvalConvocatoria"] = IdConvoct;
                Response.Redirect("EvaluacionConvocatoria.aspx?IdConvoct=" + IdConvoct + "&IdVersionProyecto=" + ((Button)e.CommandSource).Attributes["IdVersionProyecto"]);
            }
            
        }

        /// <summary>
        /// Handles the Click event of the LinkButton1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["IdConvocatoria"] = "0";
            Response.Redirect("Convocatoria.aspx");
        }

        /// <summary>
        /// Gets the convocatorias.
        /// </summary>
        /// <param name="orderBy">The order by.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="maxRows">The maximum rows.</param>
        /// <returns></returns>
        public List<convocatoriaModelINT> getConvocatorias(string orderBy, int startIndex, int maxRows)
        {        
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                IEnumerable<convocatoriaModelINT> entities = (from c in db.Convocatoria
                                select new convocatoriaModelINT
                                {
                                    codOperador = c.codOperador,
                                    FechaFin = c.FechaFin,
                                    FechaInicio = c.FechaInicio,
                                    IdVersionProyecto = c.IdVersionProyecto,
                                    Id_Convocatoria = c.Id_Convocatoria,
                                    NomConvocatoria = c.NomConvocatoria,
                                    Publicado = c.Publicado
                                }).ToList();

                if (orderBy.ToLower().Contains("nomconvocatoria"))
                {
                    if (orderBy.ToLower().Contains("desc"))
                    {
                        entities = entities.OrderByDescending(filter => filter.NomConvocatoria);
                    }
                    else
                    {
                        entities = entities.OrderBy(filter => filter.NomConvocatoria);
                    }
                } else if (orderBy.ToLower().Contains("fechainicio"))
                {
                    if (orderBy.ToLower().Contains("desc"))
                    {
                        entities = entities.OrderByDescending(filter => filter.FechaInicio);
                    }
                    else
                    {
                        entities = entities.OrderBy(filter => filter.FechaInicio);
                    }
                } else if (orderBy.ToLower().Contains("fechafin"))
                {
                    if (orderBy.ToLower().Contains("desc"))
                    {
                        entities = entities.OrderByDescending(filter => filter.FechaFin);
                    }
                    else
                    {
                        entities = entities.OrderBy(filter => filter.FechaFin);
                    }
                } else if (orderBy.ToLower().Contains("publicado"))
                {
                    if (orderBy.ToLower().Contains("desc"))
                    {
                        entities = entities.OrderByDescending(filter => filter.Publicado);
                    }
                    else
                    {
                        entities = entities.OrderBy(filter => filter.Publicado);
                    }
                }
                else if (orderBy.ToLower().Contains("nomoperador"))
                {
                    if (orderBy.ToLower().Contains("desc"))
                    {
                        entities = entities.OrderByDescending(filter => filter.Publicado);
                    }
                    else
                    {
                        entities = entities.OrderBy(filter => filter.Publicado);
                    }
                }
                else
                {
                    entities = entities.OrderByDescending(filter => filter.FechaInicio);
                }
                
                entities = entities.Skip(startIndex).Take(maxRows);

                foreach (var e in entities)
                {
                    if(e.codOperador!= null)
                    e.NomOperador = operadorController.getOperador(e.codOperador).NombreOperador;
                }

                return entities.ToList();
            }
        }

        OperadorController operadorController = new OperadorController();

        /// <summary>
        /// Gets the convocatorias count.
        /// </summary>
        /// <returns></returns>
        public int getConvocatoriasCount()
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {                
                var entities = (from convocatorias in db.Convocatoria
                                select convocatorias
                               ).Count();

                return entities;
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cmbRegistrosPorPagina control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void cmbRegistrosPorPagina_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvConvocatorias.PageSize = Convert.ToInt32(cmbRegistrosPorPagina.SelectedValue);
            gvConvocatorias.DataBind();
        }  
        
        public class convocatoriaModelINT
        {
            public int Id_Convocatoria { get; set; }
            public string NomConvocatoria { get; set; }
            public DateTime FechaInicio { get; set; }
            public DateTime FechaFin { get; set; }
            public bool? Publicado { get; set; }
            public int? IdVersionProyecto { get; set; }
            public int? codOperador { get; set; }
            public string NomOperador { get; set; }
        }
    }
}