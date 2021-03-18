using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Datos;
using System.Data.SqlClient;
using System.Configuration;

namespace Fonade.FONADE.Proyecto
{
    public partial class InactivarProyecto : Negocio.Base_Page //System.Web.UI.Page
    {
        #region Variables globales.

        String CodProyecto;
        String txtSQL;
        Boolean bInactivo;

        #endregion

        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try { CodProyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? HttpContext.Current.Session["CodProyecto"].ToString() : "0"; }
            catch { ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.close();", true); }

            //Se valida que si tenga datos "válidos".
            if (CodProyecto == "0") { ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.close();", true); }
            else
            {
                if (!IsPostBack)
                {
                    //MotivoDesactivacion.
                    txtSQL = "select motivodesactivacion, inactivo from proyecto where id_proyecto=" + CodProyecto;
                    var RS = consultas.ObtenerDataTable(txtSQL, "text");
                    if (RS.Rows.Count > 0)
                    {
                        MotivoInactivacion.Text = RS.Rows[0]["motivodesactivacion"].ToString();
                        bInactivo = Boolean.Parse(RS.Rows[0]["inactivo"].ToString());
                        if (!bInactivo) { btnInactivar.Visible = true; }
                    }
                    RS = null;
                }
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 05/08/2014.
        /// Validar.
        /// </summary>
        /// <returns>String sin datos = puede continuar. // string con datos = Error.</returns>
        private string Validar()
        {
            String msg = "";

            try
            {
                if (MotivoInactivacion.Text == "")
                { msg = Texto("TXT_MotivoDesactivacion_REQ"); }
                if (MotivoInactivacion.Text.Trim().Length > 300)
                { msg = "El Motivo de Inactivación no debe tener mas de 300 caracteres"; }

                return msg;
            }
            catch
            { msg = "Error inesperado"; return msg; }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 05/08/2014.
        /// Inactivar el proyecto...
        /// </summary>
        private void Inactivar()
        {
            //Inicializar variables.
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            try
            {
                #region Inactivar proyecto.

                txtSQL = "update proyecto set fechadesactivacion=getdate(), MotivoDesactivacion='" + MotivoInactivacion.Text + "', inactivo=1 where id_proyecto=" + CodProyecto;

                try
                {
                    //NEW RESULTS:
                    cmd = new SqlCommand(txtSQL, con);

                    if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    //con.Close();
                    //con.Dispose();
                    cmd.Dispose();
                }
                catch { }
                finally {
                    con.Close();
                    con.Dispose();
                }

                #endregion

                #region Inactivar usuarios emprendedores del proyecto.

                txtSQL = " update contacto set inactivo=1 where id_contacto in " +
                         " (select p.codcontacto from proyectocontacto p, grupocontacto g " +
                         " where p.codcontacto=g.codcontacto and codgrupo=" + Constantes.CONST_Emprendedor +
                         " and codproyecto=" + CodProyecto + " and inactivo=0)";
                try
                {
                    //NEW RESULTS:
                    cmd = new SqlCommand(txtSQL, con);

                    if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    //con.Close();
                    //con.Dispose();
                    cmd.Dispose();
                }
                catch { }
                finally {
                    con.Close();
                    con.Dispose();
                }

                #endregion

                #region Inactivar usuarios emprendedores. Los asesores no se inactivan porque pueden tener otros proyectos.

                txtSQL = " delete from grupocontacto where codgrupo = " + Constantes.CONST_Emprendedor + " and codcontacto in " +
                         " (select codcontacto from proyectocontacto where codproyecto=" + CodProyecto + " and inactivo=0)";
                try
                {
                    //NEW RESULTS:
                    cmd = new SqlCommand(txtSQL, con);

                    if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    //con.Close();
                    //con.Dispose();
                    cmd.Dispose();
                }
                catch { }
                finally { 
                
                    con.Close();
                    con.Dispose();
                }
                #endregion

                #region Inactivar usuarios dentro del proyecto.

                txtSQL = "update proyectocontacto set inactivo=1, fechafin=getdate() where inactivo=0 and codproyecto=" + CodProyecto;

                try
                {
                    //NEW RESULTS:
                    cmd = new SqlCommand(txtSQL, con);

                    if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    //con.Close();
                    //con.Dispose();
                    cmd.Dispose();
                }
                catch { }
                finally {

                    con.Close();
                    con.Dispose();
                
                }
                #endregion

                #region Cerrar Tareas Pendientes relacionadas con el proyecto.

                txtSQL = "update tareausuariorepeticion set respuesta = 'Cerrada por Inactivacion proyecto', fechacierre=getdate() where codtareausuario in " +
                            "(select id_tareausuario from tareausuario where codproyecto=" + CodProyecto + ") and fechacierre is null";

                try
                {
                    //NEW RESULTS:
                    cmd = new SqlCommand(txtSQL, con);

                    if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    //con.Close();
                    //con.Dispose();
                    cmd.Dispose();
                }
                catch { }
                finally {
                    con.Close();
                    con.Dispose();
                    
                }
                #endregion

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.reload();window.close();", true);
            }
            catch { }
        }

        /// <summary>
        /// Cerrar ventana.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCerrar_Click(object sender, EventArgs e)
        { ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.close();", true); }

        /// <summary>
        /// Llamada al método "Inactivar".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInactivar_Click(object sender, EventArgs e)
        {
            //Inicializar variables.
            String validado = "";

            validado = Validar();
            if (validado == "")
            { Inactivar(); }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + validado + "')", true);
                return;
            }
        }
    }
}