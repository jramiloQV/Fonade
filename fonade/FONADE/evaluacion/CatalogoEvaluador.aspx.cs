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
namespace Fonade.FONADE.evaluacion
{
    /// <summary>
    /// CatalogoEvaluador
    /// </summary>
    
    public partial class CatalogoEvaluador : Negocio.Base_Page
    {

        /// <summary>
        /// The message error detail
        /// </summary>
        protected String MessageErrorDetail = "";

        /// <summary>
        ///   si no pertenece a grupo redirige a pagina home
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>    
        protected void Page_Load(object sender, EventArgs e)
        {
            lbl_Titulo.Text = void_establecerTitulo("EVALUADORES");
            if (usuario.CodGrupo != Constantes.CONST_GerenteEvaluador)
            {
                Response.Redirect(@"\FONADE\MiPerfil\Home.aspx");
            }
        }
        /// <summary>
        /// sortear datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lds_listaCoordEval_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {

            try
            {
                var query = from P in consultas.VerEvaluadores(usuario.CodOperador)
                            select P;



                switch (gvcCoordinadoresEval.SortExpression)
                {
                    case "nombre":
                        if (gvcCoordinadoresEval.SortDirection == SortDirection.Ascending)
                            query = query.OrderBy(t => t.nombre);
                        else
                            query = query.OrderByDescending(t => t.nombre);
                        break;
                    case "email":
                        if (gvcCoordinadoresEval.SortDirection == SortDirection.Ascending)
                            query = query.OrderBy(t => t.email);
                        else
                            query = query.OrderByDescending(t => t.email);
                        break;

                    case "Cuantos":
                        if (gvcCoordinadoresEval.SortDirection == SortDirection.Ascending)
                            query = query.OrderBy(t => t.Cuantos);
                        else
                            query = query.OrderByDescending(t => t.Cuantos);
                        break;
                    case "inactividad":
                        if (gvcCoordinadoresEval.SortDirection == SortDirection.Ascending)
                            query = query.OrderBy(t => t.inactividad);
                        else
                            query = query.OrderByDescending(t => t.inactividad);
                        break;
                    default:
                        query = query.OrderBy(t => t.nombre);
                        break;
                }


                e.Arguments.TotalRowCount = query.Count();

                e.Result = query;
            }
            catch (Exception exc)
            {
                MessageErrorDetail = exc.Message;
            }
        }

        /// <summary>
        /// Handles the Load event of the gvcCoordinadoresEval control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void gvcCoordinadoresEval_Load(object sender, EventArgs e)
        {
            foreach (GridViewRow grd_Row in this.gvcCoordinadoresEval.Rows)
            {
                int inactivo = Convert.ToInt32(((HiddenField)grd_Row.FindControl("Hiddeninactivo")).Value);

                int NumEvaluadores = Convert.ToInt32(((HiddenField)grd_Row.FindControl("HiddenNumevals")).Value);

                int habilitado = Convert.ToInt32(((HiddenField)grd_Row.FindControl("Inhabilitado")).Value);

                if (NumEvaluadores == 0)
                {
                    ((Button)grd_Row.FindControl("hlcuantos")).Enabled = false;

                    if (inactivo == 0)
                    {
                        ((ImageButton)grd_Row.FindControl("ibtninactivar")).Visible = true;
                    }
                    else
                    {
                        ((ImageButton)grd_Row.FindControl("ibtnreactivar")).Visible = true;
                    }
                }

                if (habilitado == 0)
                {
                    ((Button)grd_Row.FindControl("hinhabilitado")).Text = "Inhabilitado";
                }
            }
        }

        /// <summary>
        /// Handles the RowCommand event of the gvcCoordinadoresEval control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewCommandEventArgs"/> instance containing the event data.</param>
        protected void gvcCoordinadoresEval_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.ToString())
            {
                case "vercuantos":
                    HttpContext.Current.Session["ContactoEvaluador"] = e.CommandArgument.ToString();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "window.open('VerProyectosEvaluador.aspx','_blank','width=602,height=390,toolbar=no, scrollbars=no, resizable=no');", true);
                    break;

                case "reactivarcoorEval":
                    reactivarperfilEvaluador(Convert.ToInt32(e.CommandArgument));
                    break;

                case "verestador":
                    string enviarsesion = e.CommandArgument.ToString() + "," + Constantes.CONST_Evaluador.ToString() + ",Ver";
                    HttpContext.Current.Session["ContactoEvaluador"] = enviarsesion;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "window.open('DesactivarEvaluador.aspx','_blank','width=502,height=200,toolbar=no, scrollbars=no, resizable=no');", true);
                    break;

                case "inactivarcoorEval":
                    string enviarsesion2 = e.CommandArgument.ToString() + "," + Constantes.CONST_Evaluador.ToString() + ",Desactivar";
                    HttpContext.Current.Session["ContactoEvaluador"] = enviarsesion2;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "window.open('DesactivarEvaluador.aspx','_blank','width=502,height=350,toolbar=no, scrollbars=no, resizable=no');", true);
                    break;

                case "editacontacto":
                    HttpContext.Current.Session["ContactoEvaluador"] = e.CommandArgument.ToString();
                    Response.Redirect("Evaluador.aspx");
                    break;

                case "verhabilitado":

                    string verpalabras = e.CommandArgument.ToString();
                    string[] palabras = verpalabras.Split(',');

                    if (palabras[1]=="0")
                    {
                        HttpContext.Current.Session["ContactoEvaluador"] = palabras[0];
                        HttpContext.Current.Session["ContratoEvaluador"] = "0";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "window.open('ContratoEvaluador.aspx','_blank','width=502,height=200,toolbar=no, scrollbars=no, resizable=no');", true);
                    }
                    else
                    {
                        HttpContext.Current.Session["ContactoEvaluador"] = palabras[0];
                        Response.Redirect("CatalogoContratoEvaluador.aspx");
                    }
                    
                    break;
            }
        }

        /// <summary>
        /// Reactiva el perfil del evaluador
        /// </summary>
        /// <param name="evaluador"></param>
        protected void reactivarperfilEvaluador(int evaluador)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand("MD_ActivarEvaluadores", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CodContacto", evaluador);
            SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
            con.Open();
            cmd2.ExecuteNonQuery();
            cmd.ExecuteNonQuery();
            con.Close();
            con.Dispose();
            cmd2.Dispose();
            cmd.Dispose();
            Response.Redirect(Request.RawUrl);
        }
        /// <summary>
        /// crea evaluador
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtn_crearEval_Click(object sender, EventArgs e)
        {
            CrearEvaluador();
        }
        /// <summary>
        /// redirige a pantalla evaluador
        /// </summary>
        private void CrearEvaluador()
        {
            HttpContext.Current.Session["ContactoEvaluador"] = "0";
            Response.Redirect("Evaluador.aspx");
        }

        /// <summary>
        /// Handles the Click event of the ibtn_crearCoorE control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ImageClickEventArgs"/> instance containing the event data.</param>
        protected void ibtn_crearCoorE_Click(object sender, ImageClickEventArgs e)
        {
            CrearEvaluador();
        }

        /// <summary>
        /// redirige a pantalla asignacion de evaluador
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_asignar_Click(object sender, EventArgs e)
        {
            Response.Redirect("AsignarEvaluador.aspx");
        }


    }
}