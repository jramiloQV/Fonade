using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using Fonade.Clases;
using Fonade.Negocio;
using System.Data.SqlClient;
using System.Configuration;

namespace Fonade.FONADE.AdministrarPerfiles
{
    /// <summary>
    /// VerProyectosAsesor
    /// </summary>    
    public partial class VerProyectosAsesor : Base_Page
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["CodContacto"] != null)
            {
                int codContacto = int.Parse(Request.QueryString["CodContacto"].ToString());
                // LINQ query
                var query = from c in consultas.Db.Contacto
                            from p in consultas.Db.Proyecto1s
                            from pc in consultas.Db.ProyectoContactos
                            from r in consultas.Db.Rols
                            where pc.CodContacto == c.Id_Contacto
                                  & pc.CodRol == r.Id_Rol
                                  & pc.CodProyecto == p.Id_Proyecto
                                  & pc.FechaFin == null
                                  & pc.Inactivo == false
                                  & c.Id_Contacto == codContacto
                            orderby p.Id_Proyecto
                            select new
                            {
                                Nombres = c.Nombres,
                                Apellidos = c.Apellidos,
                                Id_Proyecto = p.Id_Proyecto,
                                NomProyecto = p.NomProyecto,
                                Nombre = r.Nombre,
                                FechaInicio = pc.FechaInicio
                            };
                gw_Asesores.DataSource = query;
                gw_Asesores.DataBind();
            }
        }

    }
}