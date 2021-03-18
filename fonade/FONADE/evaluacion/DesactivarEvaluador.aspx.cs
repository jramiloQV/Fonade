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
using System.Globalization;

namespace Fonade.FONADE.evaluacion
{

    /// <summary>
    /// clase para desactivar evaluador
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    
    public partial class DesactivarEvaluador : Negocio.Base_Page
    {
        protected int CodContacto;
        protected int CodGrupo;
        protected string accion;
        protected void Page_Load(object sender, EventArgs e)
        {

            l_fechaActual.Text = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");
            lbl_Titulo.Text = void_establecerTitulo("DESACTIVAR EVALUADOR");

            string versesion = HttpContext.Current.Session["ContactoEvaluador"].ToString();
            string[] palabras = versesion.Split(',');
            CodContacto = Convert.ToInt32(palabras[0]);
            CodGrupo = Convert.ToInt32(palabras[1]);
            accion = palabras[2];

            if (!IsPostBack)
            {
                if (accion == "Ver")
                {
                    btn_cerrar.Visible = true;
                    TraerMotivo();
                }
                if (accion == "Desactivar")
                {
                    PanelDesactivar.Visible = true;
                    txt_motivo.Enabled = true;
                    DateTime hoy = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                    txt_fechFin.Text = hoy.AddDays(1).ToString("dd/MM/yyyy",CultureInfo.InvariantCulture);
                    btn_desactivar.Visible = true;
                    TraerDatos();
                }
            }
        }

        protected void TraerMotivo()
        {
            var query = (from x in consultas.Db.ContactoDesactivacions
                         orderby x.FechaInicio descending
                         where x.CodContacto == CodContacto
                         select new
                         {
                             comment = x.Comentario
                         }).FirstOrDefault();

            if (query.comment != null)
            {
                txt_motivo.Text = query.comment;
            }
            else
            {
                txt_motivo.Text = "";
            }
            
        }

        protected void btn_cerrar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "window.close();", true);
        }

        protected void TraerDatos()
        {
            var query = (from x in consultas.Db.Contacto
                         where x.Id_Contacto == CodContacto
                         select new
                         {
                             nombre = x.Nombres,
                             apellidos = x.Apellidos,
                             cedula = x.Identificacion
                         }).FirstOrDefault();
            l_nombre.Text = query.nombre + " " + query.apellidos;
            l_cedula.Text = query.cedula.ToString();
        }

        protected void btn_desactivar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_motivo.Text) || string.IsNullOrEmpty(txt_fechFin.Text))
            {
                if (string.IsNullOrEmpty(txt_motivo.Text))
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error: El campo Motivo es requerido!')", true);
                else
                    if (string.IsNullOrEmpty(txt_fechFin.Text))
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error: La Fecha es requerida!')", true);
            }
            else 
            {
                if (txt_motivo.Text.Length > 250)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error: El limite de caracteres para el motivo es de 250!')", true);
                }
                else
                {

                    DateTime fFinalSql = DateTime.ParseExact(txt_fechFin.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime ahora = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    if (fFinalSql <= ahora)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error: La fecha final debe ser superior a la actual!')", true);
                    }
                    else
                    {
                        var fechaFinalSql = fFinalSql.Date.ToString("yyyy-MM-dd HH:mm:ss",CultureInfo.InvariantCulture);

                        if (CodGrupo == Constantes.CONST_Evaluador)
                        {
                            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                            SqlCommand cmd = new SqlCommand("MD_DesactivarEvaluador", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@CONST_Convocatoria", Constantes.CONST_Convocatoria);
                            cmd.Parameters.AddWithValue("@CONST_Evaluacion", Constantes.CONST_Evaluacion);
                            cmd.Parameters.AddWithValue("@CONST_RolEvaluador", Constantes.CONST_RolEvaluador);
                            cmd.Parameters.AddWithValue("@CodEvaluador", CodContacto);
                            cmd.Parameters.AddWithValue("@CONST_RolCoordinadorEvaluador", Constantes.CONST_RolCoordinadorEvaluador);
                            if (ch_indefinidamente.Checked)
                            {
                                cmd.Parameters.AddWithValue("@fecFin", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@fecFin", fechaFinalSql);
                            }
                            cmd.Parameters.AddWithValue("@Motivo", txt_motivo.Text);
                            SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
                            con.Open();
                            cmd2.ExecuteNonQuery();
                            cmd.ExecuteNonQuery();
                            con.Close();
                            con.Dispose();
                            cmd2.Dispose();
                            cmd.Dispose();
                        }
                        if (CodGrupo == Constantes.CONST_CoordinadorEvaluador)
                        {
                            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                            SqlCommand cmd = new SqlCommand("MD_DesactivarCoordinadorEvaluador", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@CodEvaluador", CodContacto);
                            cmd.Parameters.AddWithValue("@CONST_RolCoordinadorEvaluador", Constantes.CONST_RolCoordinadorEvaluador);
                            if (ch_indefinidamente.Checked)
                            {
                                cmd.Parameters.AddWithValue("@fecFin", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@fecFin", fechaFinalSql);
                            }
                            cmd.Parameters.AddWithValue("@Motivo", txt_motivo.Text);
                            SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
                            con.Open();
                            cmd2.ExecuteNonQuery();
                            cmd.ExecuteNonQuery();
                            con.Close();
                            con.Dispose();
                            cmd2.Dispose();
                            cmd.Dispose();
                        }

                        
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.reload(); window.close();", true);
                    }
                }
            }
        }
    }
}