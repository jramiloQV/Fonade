#region Diego Quiñonez

// <Author>Diego Quiñonez</Author>
// <Fecha>08 - 07 - 2014</Fecha>
// <Archivo>ListarPostIt1.cs</Archivo>

#endregion

using Datos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.Controles
{
    /// <summary>
    /// ListarPostIt1
    /// </summary>    
    public partial class ListarPostIt1 : Negocio.Base_Page
    {        
        private Int32 CONST_PostIt;
        private String codProyecto;
        private Int32 CodUsuario;
        private String tabEval;
        private String txtCampo;
        private Int32 codGrupo;        
        private String txtSQL = string.Empty;        
        //private DataTable dtContacto;
        //private DataTable datatable;
        private DataTable dt;
        
        /// <summary>
        /// Diego Quiñonez
        /// retorna la cantidad de tareas encontradas
        /// por el usuario
        /// </summary>
        public Int32 PublicData
        {
            get { 
                return dt.Rows.Count; 
            }
        }

        //objeto consulta que 
        //permite acceder a la capa datos
        //y obtener lo requerido por la BD
        private Consultas consulta = new Consultas();

        /// <summary>
        /// Diego Quiñonez
        /// metodo de carga
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            recogeSession();

            //busca el nombre del usuario en la base de datos
            var contacto = (from c in consulta.Db.Contacto
                            where c.Id_Contacto == CodUsuario
                            select new
                            {
                                c.Nombres,
                                c.Apellidos
                            }).FirstOrDefault();

            
            L_Nombreusuario.Text = contacto.Nombres + " " + contacto.Apellidos;
            
            llenarGridTarea();
            llenarGridHisorico();

        }

        /// <summary>
        /// Diego Quiñonez
        /// recoge los datos de entrada de la session
        /// en caso de no existir cierra el post_it
        /// </summary>
        public void recogeSession()
        {
            try
            {
                codProyecto = HttpContext.Current.Session["EvalCodProyectoPOst"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["EvalCodProyectoPOst"].ToString()) ? HttpContext.Current.Session["EvalCodProyectoPOst"].ToString() : string.Empty;
                CodUsuario = HttpContext.Current.Session["EvalCodUsuario"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["EvalCodUsuario"].ToString()) ? Convert.ToInt32(HttpContext.Current.Session["EvalCodUsuario"].ToString()) : usuario.IdContacto;
                tabEval = HttpContext.Current.Session["tabEval"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["tabEval"].ToString()) ? HttpContext.Current.Session["tabEval"].ToString() : string.Empty;
                CONST_PostIt = HttpContext.Current.Session["EvalConsPOST"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["EvalConsPOST"].ToString()) ? Convert.ToInt32(HttpContext.Current.Session["EvalConsPOST"].ToString()) : Constantes.CONST_PostIt;
                txtCampo = HttpContext.Current.Session["Campo"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["Campo"].ToString()) ? HttpContext.Current.Session["Campo"].ToString() : "nulo";
                codProyecto = string.IsNullOrEmpty(codProyecto) && HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? HttpContext.Current.Session["CodProyecto"].ToString() : string.Empty;
                
                try { 
                    codGrupo = usuario.CodGrupo; }
                catch (Exception) { 
                    codGrupo = -1; 
                }
            }
            catch (Exception)
            {
                ClientScriptManager cm = this.ClientScript;                
                cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>window.close();</script>");
            }
        }
        
        /// <summary>
        /// Alex Flautero
        /// </summary>
        public void llenarGridTarea()
        {            
            DataTable dt1 = new DataTable();
            
            txtSQL = "Select tr.Id_TareaUsuarioRepeticion as Id_tareaRepeticion, t.codcontacto, t.CodContactoAgendo, tr.Fecha, tr.fechacierre, t.NomTareaUsuario,";
            txtSQL += "c.Nombres + ' ' + c.Apellidos as Agendadoa, c2.Nombres + ' ' + c2.Apellidos as Agendo ";
            txtSQL += "From tareausuariorepeticion tr INNER JOIN tareausuario t ON T.Id_TareaUsuario = TR.CodTareaUsuario ";
            txtSQL += "INNER JOIN Contacto c  ON c.id_contacto = t.codcontactoagendo ";
            txtSQL += "INNER JOIN Contacto c2 ON c2.id_contacto = t.codcontacto,tareaprograma tp ";
            txtSQL += "Where tr.fechacierre is null and tp.Id_TareaPrograma = 5 and  t.CodTareaPrograma = 5 and ";
            txtSQL += "t.CodProyecto = " + codProyecto + " AND (t.codcontactoagendo = " + usuario.IdContacto + " or t.codcontacto = " + usuario.IdContacto + ")  ORDER BY tr.Id_TareaUsuarioRepeticion desc";
            try
            {
                dt1 = consulta.ObtenerDataTable(txtSQL, "text");
                gw_Tareas.DataSource = dt1;
                gw_Tareas.DataBind();
            }
            catch (Exception e) { 
                throw new Exception("No pudo ser consultado el listado de tareas",e); 
            }
        }

        private void llenarGridHisorico()
        {
            txtSQL = "Select tr.Id_TareaUsuarioRepeticion as Id_tareaRepeticion, t.codcontacto, t.CodContactoAgendo, tr.Fecha, tr.fechacierre, t.NomTareaUsuario,";
            txtSQL += "c.Nombres + ' ' + c.Apellidos as Agendadoa, c2.Nombres + ' ' + c2.Apellidos as Agendo ";
            txtSQL += "From tareausuariorepeticion tr INNER JOIN tareausuario t ON T.Id_TareaUsuario = TR.CodTareaUsuario ";
            txtSQL += "INNER JOIN Contacto c  ON c.id_contacto = t.codcontactoagendo ";
            txtSQL += "INNER JOIN Contacto c2 ON c2.id_contacto = t.codcontacto,tareaprograma tp ";
            txtSQL += "Where tr.fechacierre is NOT null and tp.Id_TareaPrograma = 5 and  t.CodTareaPrograma = 5 and ";
            txtSQL += "t.CodProyecto = " + codProyecto + " AND (t.codcontactoagendo = " + usuario.IdContacto + " or t.codcontacto = " + usuario.IdContacto + ")  ORDER BY tr.Id_TareaUsuarioRepeticion desc";
            try
            {
                var dt1 = consulta.ObtenerDataTable(txtSQL, "text");
                grvHistoPost.DataSource = dt1;
                grvHistoPost.DataBind();
            }
            catch (Exception e) { 
                throw new Exception("No pudo ser consultado el listado de tareas", e); 
            }
        }

        /// <summary>
        /// Handles the RowCommand event of the gw_Tareas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewCommandEventArgs"/> instance containing the event data.</param>
        protected void gw_Tareas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "mostrar_tarea" && e.CommandSource.GetType().Name == "LinkButton")
            {
                HttpContext.Current.Session["Id_tareaRepeticion"] = ((LinkButton)e.CommandSource).CommandArgument ?? "0";
                Response.Redirect("~/Fonade/Tareas/TareasAgendar.aspx");
            }
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the gw_Tareas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs"/> instance containing the event data.</param>
        protected void gw_Tareas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gw_Tareas.PageIndex = e.NewPageIndex;
            llenarGridTarea();
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the grvHistoPost control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs"/> instance containing the event data.</param>
        protected void grvHistoPost_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvHistoPost.PageIndex = e.NewPageIndex;
            llenarGridHisorico();
        }

    }
}