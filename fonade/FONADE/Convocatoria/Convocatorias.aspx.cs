using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.Convocatoria
{
    /// <summary>
    /// Convocatorias
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class Convocatorias : System.Web.UI.Page
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
        /// Obtiene el listado de convocatorias.
        /// </summary>
        /// <returns>Lista de convocatorias</returns>
        public List<Datos.Convocatoria> getConvocatorias(int startIndex, int maxRows)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from convocatorias in db.Convocatoria
                                       select convocatorias)
                                       .Skip(startIndex)
                                       .Take(maxRows)
                                       .ToList();

                return entities;
            }
        }

        /// <summary>
        /// Obtiene el numero de convocatorias de ese usuario.
        /// </summary>
        /// <returns>Numero de convocatorias</returns>
        public int getConvocatoriasCount()
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                //Consulta para obtener listado de planes de negocio
                var entities = (from convocatorias in db.Convocatoria
                                select convocatorias    
                               ).Count();

                return entities;
            }
        }

        /// <summary>
        /// Handles the RowCommand event of the gvPlanesDeNegocio control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewCommandEventArgs"/> instance containing the event data.</param>
        protected void gvPlanesDeNegocio_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("verPlanDeNegocio"))
            {
                if (e.CommandArgument != null)
                {
                    int codigoPlanDeNegocio = Convert.ToInt32(e.CommandArgument);

                    Session["codigoPlanDeNegocio"] = codigoPlanDeNegocio;
                    Response.Redirect("CrearPlanDeNegocio.aspx");
                }
            }
        }
    }
}