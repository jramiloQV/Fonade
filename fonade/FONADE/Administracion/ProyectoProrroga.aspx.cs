using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Datos;

namespace Fonade.FONADE.Administracion
{
    /// <summary>
    /// ProyectoProrroga
    /// </summary>    
    public partial class ProyectoProrroga : Negocio.Base_Page
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string query = "select Id_proyecto,nomproyecto,prorroga,NombreOperador " +
                    " from ProyectoProrroga,Proyecto,operador " +
                    " where id_proyecto=codProyecto and codOperador = idOperador";

                Int64 numero;
                if (Int64.TryParse(txtBuscarProyecto.Text, out numero))
                {
                    query += " AND Proyecto.id_proyecto = " + numero.ToString();
                }

                if (usuario.CodGrupo != Constantes.CONST_AdministradorSistema) {
                    query += " AND Proyecto.codOperador = " + usuario.CodOperador;
                }

                var listProyectoProrroga = consultas.ObtenerDataTable(query, "text");

                gv_proyectosProrroga.DataSource = listProyectoProrroga;
                gv_proyectosProrroga.DataBind();

                //Limpiar variables relacionadas con prorroga en los formularios 
                //de adicionar prorrogar y busqueda de proyectos.
                HttpContext.Current.Session["CodigoProyecto"] = null;
                HttpContext.Current.Session["NombreProyecto"] = null;
            }
            catch (Exception)
            {

            }



            //var listProyectoProrroga = consultas.ObtenerDataTable("select Id_proyecto,nomproyecto,prorroga from ProyectoProrroga,Proyecto where id_proyecto=codProyecto", "text");

            //gv_proyectosProrroga.DataSource = listProyectoProrroga;
            //gv_proyectosProrroga.DataBind();

            ////Limpiar variables relacionadas con prorroga en los formularios 
            ////de adicionar prorrogar y busqueda de proyectos.
            //HttpContext.Current.Session["CodigoProyecto"] = null;
            //HttpContext.Current.Session["NombreProyecto"] = null;
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the gv_proyectosProrroga control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs"/> instance containing the event data.</param>
        protected void gv_proyectosProrroga_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_proyectosProrroga.PageIndex = e.NewPageIndex;
            gv_proyectosProrroga.DataBind();
        }

        private void ModifcarFecha(int id_tareausario)
        {
            int repeticion;

            repeticion = id_tareausario;

            var result = (from tu in consultas.Db.TareaUsuarioRepeticions
                          where tu.Id_TareaUsuarioRepeticion == repeticion
                          select tu).FirstOrDefault();
        }        
    }
}