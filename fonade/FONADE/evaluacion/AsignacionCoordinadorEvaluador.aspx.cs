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
using Fonade.Error;

namespace Fonade.FONADE.evaluacion
{
    /// <summary>
    /// AsignacionCoordinadorEvaluador
    /// </summary>
    
    public partial class AsignacionCoordinadorEvaluador : Negocio.Base_Page
    {
        /// <summary>
        /// The coordinador e value asignado
        /// </summary>
        protected int CoordinadorEValAsignado;
        private int CodEvaluador
        {
            set { ViewState["CodEvaluador"] = value; }
            get { var ijn = ViewState["CodEvaluador"] ?? 0; return Convert.ToInt32(ijn); }
        }
        private string Accion
        {
            set { ViewState["Accion"] = value; }
            get { return ViewState["Accion"] != null && !string.IsNullOrEmpty(ViewState["Accion"].ToString()) ? ViewState["Accion"].ToString() : string.Empty; }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack){
                if (CodEvaluador == 0 && string.IsNullOrEmpty(Accion)){
                    lblinfoEvaluador.Text = "Para ver el coordinador de un evaluador, seleccione uno a la izquierda.";
                    dtlProyectos.Visible = false;
                    gvrCoordinadorDeEvaluador.Visible = false;
                    lnkAsignarCoordinador.Visible = false;
                    gvrListaCoordinadores.Visible = false;
                    btnActualizar.Visible = false;
                }
                else
                    {
                    lblinfoEvaluador.Text = "";
                    consultarInfoEvaluador(CodEvaluador);
                }
                if (Session["CodEvaluador"] != null){
                lblinfoEvaluador.Text = "";
                CodEvaluador = Convert.ToInt32(Session["CodEvaluador"]);
                consultarInfoEvaluador(CodEvaluador);
                Session["CodEvaluador"] = null;
                }
            }
        }

        /// <summary>
        /// Handles the Selecting event of the ldsEvaluadores control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LinqDataSourceSelectEventArgs"/> instance containing the event data.</param>
        protected void ldsEvaluadores_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var result = (from c in consultas.Db.Contacto
                          join ec in consultas.Db.EvaluadorContratos on c.Id_Contacto equals ec.CodContacto
                          join er in consultas.Db.Evaluadors on c.Id_Contacto equals er.CodContacto
                          join pc in consultas.Db.ProyectoContactos on c.Id_Contacto equals pc.CodContacto
                          join gc in consultas.Db.GrupoContactos on c.Id_Contacto equals gc.CodContacto
                          where c.Inactivo == false
                          && gc.CodGrupo == Constantes.CONST_Evaluador
                          select new
                          {
                              c.Id_Contacto,
                              nombre = c.Nombres + " " + c.Apellidos,
                              er.CodCoordinador
                          }
                          ).Distinct();

            e.Result = result.ToList();
        }

        /// <summary>
        /// Handles the Selecting event of the ldsProyectos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LinqDataSourceSelectEventArgs"/> instance containing the event data.</param>
        protected void ldsProyectos_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var result = (from p in consultas.Db.Proyecto
                          join pc in consultas.Db.ProyectoContactos on p.Id_Proyecto equals pc.CodProyecto
                          where pc.Inactivo == false
                          && pc.CodRol == Constantes.CONST_RolEvaluador
                          && pc.CodContacto == CodEvaluador
                          select new
                          {
                              p.Id_Proyecto,
                              p.NomProyecto,
                              p.Sumario
                          });

            e.Result = result.ToList();
        }

        /// <summary>
        /// Handles the Selecting event of the ldsCoordinadores control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LinqDataSourceSelectEventArgs"/> instance containing the event data.</param>
        protected void ldsCoordinadores_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var result = (from c in consultas.Db.Contacto
                          from ec in consultas.Db.Evaluadors
                          where c.Id_Contacto == ec.CodCoordinador
                          && ec.CodContacto == CodEvaluador
                          select new
                          {
                              nombre = c.Nombres + " " + c.Apellidos,
                              c.Email
                          });

            e.Result = result.ToList();
        }
        /// <summary>
        /// lista coordinadores
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ldsListaCoordinadores_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var result = (from c in consultas.Db.Contacto
                              join gc in consultas.Db.GrupoContactos on c.Id_Contacto equals gc.CodContacto
                              where c.Inactivo == false
                              && gc.CodGrupo == Constantes.CONST_CoordinadorEvaluador
                              select new{
                                  c.Id_Contacto,
                                  nombre = c.Nombres + " " + c.Apellidos
                              });

            e.Result = result.ToList();
        }
        /// <summary>
        /// consultar informacion evaluador
        /// </summary>
        /// <param name="CodEvaluador"></param>
        private void consultarInfoEvaluador(int CodEvaluador)
        {
            if (Accion.Equals("Editar"))
            {
                lblinfoEvaluador.Text = string.Empty;
                lblinfoEvaluador.Controls.Add(new LiteralControl("<h1>"));
                lblinfoEvaluador.Controls.Add(new LiteralControl("Planes de Negocio de " +
                    (from c in consultas.Db.Contacto
                     where c.Id_Contacto == CodEvaluador
                     select new
                     {
                         nombre = c.Nombres + " " + c.Apellidos
                     }).First().ToString().Split('=')[1]));
                lblinfoEvaluador.Controls.Add(new LiteralControl("</h1>"));

                dtlProyectos.Visible = true;
                dtlProyectos.DataBind();
                gvrCoordinadorDeEvaluador.Visible = false;
                lnkAsignarCoordinador.Visible = false;
                gvrListaCoordinadores.Visible = true;
                gvrListaCoordinadores.DataBind();
                btnActualizar.Visible = true;
                return;
            }

            if (CodEvaluador != 0 && string.IsNullOrEmpty(Accion))
            {
                lblinfoEvaluador.Text = string.Empty;
                lblinfoEvaluador.Controls.Add(new LiteralControl("<h1>"));
                lblinfoEvaluador.Controls.Add(new LiteralControl("Planes de Negocio de " +
                    (from c in consultas.Db.Contacto
                     where c.Id_Contacto == CodEvaluador
                     select new
                     {
                         nombre = c.Nombres + " " + c.Apellidos
                     }).First().ToString().Split('=')[1]));
                lblinfoEvaluador.Controls.Add(new LiteralControl("</h1>"));

                dtlProyectos.Visible = true;
                dtlProyectos.DataBind();
                gvrCoordinadorDeEvaluador.Visible = true;
                dtlProyectos.DataBind();
                lnkAsignarCoordinador.Visible = true;
                gvrListaCoordinadores.Visible = false;
                btnActualizar.Visible = false;
                return;
            }
        }

        /// <summary>
        /// Handles the RowDataBound event of the gvEvaluadores control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void gvEvaluadores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var img = (ImageButton)e.Row.Cells[1].FindControl("imgAdmiracion");

            if (img != null)
            {
                if (img.CommandArgument == null || string.IsNullOrEmpty(img.CommandArgument))
                {
                    img.Visible = true;
                }
            }
        }

        /// <summary>
        /// Handles the RowCommand event of the gvEvaluadores control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewCommandEventArgs"/> instance containing the event data.</param>
        protected void gvEvaluadores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            CodEvaluador = int.Parse(e.CommandArgument.ToString());
            Accion = string.Empty;

            consultarInfoEvaluador(CodEvaluador);
        }

        /// <summary>
        /// Handles the RowDataBound event of the gvrListaCoordinadores control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void gvrListaCoordinadores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int codigo = int.Parse(gvrListaCoordinadores.DataKeys[e.Row.RowIndex].Value.ToString());

                int result = (from ec in consultas.Db.Evaluadors
                              where ec.CodContacto == CodEvaluador
                              && ec.CodCoordinador == codigo
                              select ec.CodContacto).FirstOrDefault();

                if (result != 0)
                    ((RadioButton)e.Row.Cells[0].FindControl("rdbCoordinador")).Checked = true;
                else
                    ((RadioButton)e.Row.Cells[0].FindControl("rdbCoordinador")).Checked = false;
            }
        }

        /// <summary>
        /// Handles the Click event of the lnkAsignarCoordinador control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnkAsignarCoordinador_Click(object sender, EventArgs e)
        {
            Accion = "Editar";
            consultarInfoEvaluador(CodEvaluador);
        }
        /// <summary>
        /// consultas e insercion proyecto contactos 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow gvr in gvrListaCoordinadores.Rows)
            {
                RadioButton rb = (RadioButton)gvr.Cells[0].FindControl("rdbCoordinador");

                if (rb.Checked)
                {
                    try
                    {                       
                        int codigoNew = int.Parse(gvrListaCoordinadores.DataKeys[gvr.RowIndex].Value.ToString());
                        int? codigoOLD = (from ec in consultas.Db.Evaluadors
                                          where ec.CodContacto == CodEvaluador
                                          select ec.CodCoordinador).FirstOrDefault();

                        if (codigoNew != codigoOLD)
                        {
                            Datos.Evaluador evaluador = (from ec in consultas.Db.Evaluadors
                                                         where ec.CodContacto == CodEvaluador
                                                         select ec).First();

                            evaluador.CodCoordinador = codigoNew;

                            var proyectos = (from pc in consultas.Db.ProyectoContactos
                                             where pc.Inactivo == false
                                             && pc.CodContacto == CodEvaluador
                                             && pc.CodRol == Constantes.CONST_RolEvaluador
                                             select pc.CodProyecto);

                            foreach (int proyecto in proyectos)
                            {
                                if (codigoOLD != 0)
                                {
                                    Datos.ProyectoContacto proyectocontacto = (from pc in consultas.Db.ProyectoContactos
                                                                               where pc.Inactivo == false
                                                                               && pc.CodProyecto == proyecto
                                                                               && pc.CodRol == Constantes.CONST_RolCoordinadorEvaluador
                                                                               select pc).FirstOrDefault();

                                    if (proyectocontacto != null)
                                    {
                                        proyectocontacto.Inactivo = true;
                                        proyectocontacto.FechaFin = DateTime.Now;
                                    }
                                }

                                int codConvocatoria = (from cp in consultas.Db.ConvocatoriaProyectos
                                                       orderby cp.Fecha descending
                                                       where cp.CodProyecto == proyecto
                                                       select cp.CodConvocatoria).First();

                                Datos.ProyectoContacto pContacto = new Datos.ProyectoContacto();

                                pContacto.CodProyecto = proyecto;
                                pContacto.CodContacto = codigoNew;
                                pContacto.CodRol = Constantes.CONST_RolCoordinadorEvaluador;
                                pContacto.FechaInicio = DateTime.Now;
                                pContacto.CodConvocatoria = codConvocatoria;

                                consultas.Db.ProyectoContactos.InsertOnSubmit(pContacto);
                            }                            
                            consultas.Db.SubmitChanges();                            
                        }

                        Accion = string.Empty;
                        consultarInfoEvaluador(CodEvaluador);
                        Session["CodEvaluador"] = CodEvaluador;

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Información guardada correctamente !');", true);                                        

                        //Response.Redirect(Request.RawUrl, false);
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo !');", true);

                        string url = Request.Url.ToString();

                        string mensaje = ex.Message.ToString();
                        string data = ex.Data.ToString();
                        string stackTrace = ex.StackTrace.ToString();
                        string innerException = ex.InnerException == null ? "" : ex.InnerException.Message.ToString();

                        // Log the error
                        ErrHandler.WriteError(mensaje, url, data, stackTrace, innerException, usuario.Email, usuario.IdContacto.ToString());
                    }
                }
            }
        }
        /// <summary>
        /// validacion comportamiento radiobutton en gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdbCoordinador_CheckedChanged(object sender, EventArgs e)
        {
            var row = ((GridViewRow)((RadioButton)sender).NamingContainer);

            foreach (GridViewRow gvr in gvrListaCoordinadores.Rows)
            {
                var rb = (RadioButton)(gvr.Cells[0].FindControl("rdbCoordinador"));
                if (gvr.RowIndex != row.RowIndex)
                {
                    if (rb != null)
                        rb.Checked = false;
                }
                else
                    rb.Checked = true;
            }
        }
    }
}