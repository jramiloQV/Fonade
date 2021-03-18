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

namespace Fonade.FONADE.interventoria
{
    public partial class DesactivaInterventor : Negocio.Base_Page
    {
        #region Variables.
        protected int CodContacto;
        protected int CodGrupo;
        protected string accion;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            l_fechaActual.Text = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");
            lbl_Titulo.Text = "DESACTIVAR INTERVENTOR";

            string versesion = HttpContext.Current.Session["ContactoInterventor"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["ContactoInterventor"].ToString()) ? HttpContext.Current.Session["ContactoInterventor"].ToString() : string.Empty;

            if (!string.IsNullOrEmpty(versesion))
            {
                string[] palabras = versesion.Split(',');
                CodContacto = Convert.ToInt32(palabras[0]);
                CodGrupo = Convert.ToInt32(palabras[1]);
                accion = palabras[2];

                #region Consultar el grupo del usuario seleccionado y asignar el texto del ConfirmButton.

                String sql_txt = "SELECT CodGrupo FROM GrupoContacto WHERE CodContacto = " + CodContacto;
                var dt = consultas.ObtenerDataTable(sql_txt, "text");
                if (dt.Rows.Count > 0)
                {
                    //Sin texto! TXT_CONFIRMA_DESACTIVAR_INTERVENTOR
                    if (dt.Rows[0]["CodGrupo"].ToString() == Constantes.CONST_Interventor.ToString())
                    { cbe.ConfirmText = "¿Desea desactivar este interventor?"; }
                    //Sin texto! TXT_CONFIRMA_DESACTIVAR_COORDINADOR_INTERVENTORIA
                    if (dt.Rows[0]["CodGrupo"].ToString() == Constantes.CONST_CoordinadorInterventor.ToString())
                    { cbe.ConfirmText = "¿Desea desactivar este coordinador de interventores?"; }
                }
                sql_txt = null; dt = null;

                #endregion

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
                        txt_fechFin.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        btn_desactivar.Visible = true;
                        TraerDatos();
                    }
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

            if (query != null)
                txt_motivo.Text = query.comment;
            else
                txt_motivo.Text = "";
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

            DateTime fecha_futura = DateTime.Today.AddDays(1);
            txt_fechFin.Text = fecha_futura.ToString("dd/MM/yyyy");
            CalendarExtender1.SelectedDate = fecha_futura;
        }

        protected void btn_desactivar_Click(object sender, EventArgs e)
        {
            if (txt_motivo.Text == "" || txt_fechFin.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error: Algunos campos están vacíos!')", true);
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
                        return;
                    }
                    else
                    {
                        var fechaFinalSql = fFinalSql.Date.ToString("yyyy-MM-dd HH:mm:ss");
                        var fechaFinalSql1 = fFinalSql.Date.ToString("yyyy-dd-MM HH:mm:ss");

                        if (CodGrupo == Constantes.CONST_Interventor)
                        {
                            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                            try
                            {
                                SqlCommand cmd = new SqlCommand("MD_DesactivarInterventor", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@CONST_Convocatoria", Constantes.CONST_Convocatoria);
                                cmd.Parameters.AddWithValue("@CONST_Interventor", Constantes.CONST_Evaluacion);
                                cmd.Parameters.AddWithValue("@CONST_RolInterventor", Constantes.CONST_RolEvaluador);
                                cmd.Parameters.AddWithValue("@CodInterventor", CodContacto);
                                cmd.Parameters.AddWithValue("@CONST_RolCoordinadorInterventor", Constantes.CONST_RolCoordinadorInterventor);
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
                                try
                                {
                                    cmd.ExecuteNonQuery();
                                }
                                catch (SqlException)
                                {
                                    cmd.Parameters[5].Value = fechaFinalSql1;
                                    cmd.ExecuteNonQuery();
                                }
                                //con.Close();
                                //con.Dispose();
                                cmd2.Dispose();
                                cmd.Dispose();
                            }
                            finally {

                                con.Close();
                                con.Dispose();
                            }
                        }
                        if (CodGrupo == Constantes.CONST_CoordinadorInterventor)
                        {
                            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                            try
                            {
                                SqlCommand cmd = new SqlCommand("MD_DesactivarCoordinadorInterventor", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@CodInterventor", CodContacto);
                                cmd.Parameters.AddWithValue("@CONST_RolCoordinadorInterventor", Constantes.CONST_RolCoordinadorInterventor);
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
                                try
                                {
                                    cmd.ExecuteNonQuery();
                                }
                                catch (SqlException)
                                {
                                    cmd.Parameters[5].Value = fechaFinalSql1;
                                    cmd.ExecuteNonQuery();
                                }
                                //con.Close();
                                //con.Dispose();
                                cmd2.Dispose();
                                cmd.Dispose();
                            }
                            finally {
                                con.Close();
                                con.Dispose();
                            }
                        }


                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.reload(); window.close();", true);
                    }
                }
            }
        }
    }
}