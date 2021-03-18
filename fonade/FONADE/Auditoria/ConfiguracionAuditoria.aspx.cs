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

namespace Fonade.FONADE.Auditoria
{
    /// <summary>
    /// ConfiguracionAuditoria
    /// </summary>
    public partial class ConfiguracionAuditoria : Negocio.Base_Page
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                lbl_Titulo.Text = void_establecerTitulo("CONFIGURACIÓN DE AUDITORÍA");
                llenarInfo();
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the Ch_activar_auditoria control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Ch_activar_auditoria_CheckedChanged(object sender, EventArgs e)
        {
            if (Ch_activar_auditoria.Checked)
            {
                panelEventos.Visible = true;
            }
            else
            {
                panelEventos.Visible = false;
            }
        }

        /// <summary>
        /// Llenars the information.
        /// </summary>
        protected void llenarInfo()
        {
            try
            {
                var query = (from audit in consultas.Db.HistoricoAuditorias
                             from cont in consultas.Db.Contacto
                             where audit.ultima_configuracion == true
                             && audit.Usuario_configuracion== cont.Id_Contacto
                             select new
                             {
                                 audit,
                                 cont,
                             }).FirstOrDefault();

                Ch_activar_auditoria.Checked = query.audit.Auditar_Activo;
                ch_actualizacion.Checked = query.audit.Modificar;
                ch_eliminacion.Checked = query.audit.Eliminar;
                ch_insercion.Checked = query.audit.Insertar;
                string nombre = query.cont.Nombres + " " + query.cont.Apellidos;
                string fecha= query.audit.fecha_configuracion.ToString("dd 'de' MMMM 'de' yyyy");
                string hora=query.audit.fecha_configuracion.ToString("hh:mm tt");
                l_ultima_actualizacion.Text = "Última configuración realizada el " + fecha + " a las " + hora + " por: " + nombre;

                Ch_activar_auditoria_CheckedChanged(null, null);
            }
            catch (Exception)
            {}

        }

        /// <summary>
        /// Handles the Click event of the btn_Guardar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btn_Guardar_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand("MD_historicoAuditoria", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AuditarActivoV", Ch_activar_auditoria.Checked);
            if (Ch_activar_auditoria.Checked)
	        {
		        cmd.Parameters.AddWithValue("@InsertarV", ch_insercion.Checked);
                cmd.Parameters.AddWithValue("@ModificarV", ch_actualizacion.Checked);
                cmd.Parameters.AddWithValue("@EliminarV", ch_eliminacion.Checked);
                cmd.Parameters.AddWithValue("@Condition", "enable");
	        }
            else
	        {
                cmd.Parameters.AddWithValue("@InsertarV", false);
                cmd.Parameters.AddWithValue("@ModificarV", false);
                cmd.Parameters.AddWithValue("@EliminarV", false);
                cmd.Parameters.AddWithValue("@Condition", "disable");
	        }

            cmd.Parameters.AddWithValue("@ultimaconfiguracionV", true);
            cmd.Parameters.AddWithValue("@UsuarioconfiguracionV", usuario.IdContacto);
            SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
            con.Open();
            cmd2.ExecuteNonQuery();
            cmd.ExecuteNonQuery();
            con.Close();
            con.Dispose();
            cmd2.Dispose();
            cmd.Dispose();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Se ha guardado correctamente!')", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.location.href  ='ConfiguracionAuditoria.aspx';", true);
        }
    }
}
    
