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
    /// CatalogoEvaluadorCoord
    /// </summary>
    
    public partial class CatalogoEvaluadorCoord : Negocio.Base_Page
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            lbl_Titulo.Text = void_establecerTitulo("COORDINADORES DE EVALUACIÓN");
            if (usuario.CodGrupo != Constantes.CONST_GerenteEvaluador)
            {
                Response.Redirect(@"\FONADE\MiPerfil\Home.aspx");
            }
        }

        /// <summary>
        /// cirterios de ordenacion de la grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lds_listaCoordEval_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {

            try
            {
                var query = from P in consultas.VerCoordinadoresDeEvaluacion(usuario.CodOperador)
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
            { }

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
                int inactivo= Convert.ToInt32(((HiddenField)grd_Row.FindControl("Hiddeninactivo")).Value);

                int NumEvaluadores = Convert.ToInt32(((HiddenField)grd_Row.FindControl("HiddenNumevals")).Value);
                if (NumEvaluadores==0)
                {
                    ((Button)grd_Row.FindControl("hlcuantos")).Enabled = false;

                    if (inactivo==0)
                    {
                        ((ImageButton)grd_Row.FindControl("ibtninactivar")).Visible = true;
                    }
                    else
                    {
                        ((ImageButton)grd_Row.FindControl("ibtnreactivar")).Visible = true;
                    }
                }
            }
        }
        /// <summary>
        /// redirecciona a formulario de acuerdo al criterio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvcCoordinadoresEval_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.ToString())
            {
                case "vercuantos":
                    HttpContext.Current.Session["ContactoEvaluador"] = e.CommandArgument.ToString();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "window.open('VerEvaluadoresCoordinador.aspx','_blank','width=602,height=390,toolbar=no, scrollbars=no, resizable=no');", true);
                break;

                case "reactivarcoorEval":
                    reactivarperfilEvaluador(Convert.ToInt32(e.CommandArgument));
                break;

                case "editacontacto":
                    HttpContext.Current.Session["ContactoEvaluador"] = e.CommandArgument.ToString();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "window.open('CoordinadorEvaluador.aspx','_blank','width=562,height=420,toolbar=no, scrollbars=1, resizable=no');", true);
                    break;

                case "verestador":
                    string enviarsesion = e.CommandArgument.ToString() + "," + Constantes.CONST_CoordinadorEvaluador.ToString() + ",Ver";
                    HttpContext.Current.Session["ContactoEvaluador"] = enviarsesion;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "window.open('DesactivarEvaluador.aspx','_blank','width=502,height=200,toolbar=no, scrollbars=no, resizable=no');", true);
                    break;

                case "inactivarcoorEval":
                    string enviarsesion2 = e.CommandArgument.ToString() + "," + Constantes.CONST_CoordinadorEvaluador.ToString() + ",Desactivar";
                    HttpContext.Current.Session["ContactoEvaluador"] = enviarsesion2;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "window.open('DesactivarEvaluador.aspx','_blank','width=502,height=350,toolbar=no, scrollbars=no, resizable=no');", true);
                    break;
            }
        }

        /// <summary>
        /// reactiva perfil del evaluador
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
        /// Handles the Click event of the lbtn_crearCoorE control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbtn_crearCoorE_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["ContactoEvaluador"] = "0";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "window.open('CoordinadorEvaluador.aspx','_blank','width=562,height=420,toolbar=no, scrollbars=1, resizable=no');", true);
        }

        /// <summary>
        /// Handles the Click event of the btn_asignar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btn_asignar_Click(object sender, EventArgs e)
        {
            //Response.Redirect("AsignacionCoordinadorEvaluador.aspx");
            Redirect(null, "AsignacionCoordinadorEvaluador.aspx", "_Blank", "width=850,height=550");
        }
    }
}