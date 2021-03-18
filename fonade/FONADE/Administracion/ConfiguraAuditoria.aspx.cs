using Fonade.Clases;
using Fonade.Negocio.Administracion;
using Fonade.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.Administracion
{
    /// <summary>
    /// ConfiguraAuditoria
    /// </summary>    
    public partial class ConfiguraAuditoria : Negocio.Base_Page
    {

        string txtSQL;
        #region Eventos

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                CargarCombo();
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddltablas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ddltablas_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarCamposTabla();
        }

        /// <summary>
        /// Handles the Click event of the btnguardar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnguardar_Click(object sender, EventArgs e)
        {
            GuardarCriterios();
        }
        #endregion

        #region Metodos
        private void CargarCombo()
        {
            txtSQL = "SELECT TABLE_NAME FROM Fonade.INFORMATION_SCHEMA.TABLES ORDER BY TABLE_NAME";
            var dt = consultas.ObtenerDataTable(txtSQL, "text");

            if(dt.Rows.Count > 0)
            {
                ddltablas.DataSource = dt;
                ddltablas.DataValueField = "TABLE_NAME";
                ddltablas.DataTextField = "TABLE_NAME";
                ddltablas.DataBind();
                ddltablas.Items.Insert(0, new ListItem("Seleccione", "0"));
            }
        }

        private void CargarCamposTabla()
        {
            txtSQL = "SELECT COLUMN_NAME, Data_Type FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + ddltablas.SelectedValue + "' ORDER BY ORDINAL_POSITION";
            var dt = consultas.ObtenerDataTable(txtSQL, "text");

            grvCampos.DataSource = dt;
            grvCampos.DataBind();

            MarcarCampos();
        }

        private void MarcarCampos()
        {
            var camposSeleccionados = (from ra in consultas.Db.ReporteAuditoria
                                       where ra.Tabla == ddltablas.SelectedValue
                                       select ra).ToList();

            if(camposSeleccionados.Count > 0)
            {
                foreach(var campo in camposSeleccionados)
                {
                    foreach(GridViewRow fila in grvCampos.Rows)
                    {
                        var item = (Label)fila.FindControl("lblCampo");
                        var chk = (CheckBox)fila.FindControl("ckbSeleccionar");
                        //var tipodato = (Label)fila.FindControl("lblTipoDato");
                        if(campo.Campo == item.Text)
                        {
                            chk.Checked = true;
                        }
                    }
                }
            }
        }

        private void GuardarCriterios()
        {
            var seleccionado = false;
            if(ddltablas.SelectedIndex > 0)
            {
                foreach (GridViewRow fila in grvCampos.Rows)
                {
                    var chk = (CheckBox)fila.FindControl("ckbSeleccionar");
                    if (chk.Checked)
                    {
                        seleccionado = true;
                        break;
                    }
                }
                if(seleccionado)
                {
                    var items = (from ra in consultas.Db.ReporteAuditoria
                                 where ra.Tabla == ddltablas.SelectedValue
                                 select ra).ToList();

                    if (items.Count > 0)
                    {
                        foreach (var item in items)
                        {
                            consultas.Db.ReporteAuditoria.DeleteOnSubmit(item);
                            consultas.Db.SubmitChanges();
                        }
                    }

                    foreach (GridViewRow fila in grvCampos.Rows)
                    {
                        var chk = (CheckBox)fila.FindControl("ckbSeleccionar");
                        var campo = (System.Web.UI.WebControls.Label)fila.FindControl("lblCampo");
                        var tipoDato = (System.Web.UI.WebControls.Label)fila.FindControl("lblTipoDato");
                        if(chk.Checked)
                        {
                            var criterio = new Datos.ReporteAuditoria();
                            criterio.Campo = campo.Text;
                            criterio.Tabla = ddltablas.SelectedValue;
                            criterio.TipoDato = criterio.TipoDato = tipoDato.Text;
                            consultas.Db.ReporteAuditoria.InsertOnSubmit(criterio);
                            consultas.Db.SubmitChanges();
                        }
                    }
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Criterios creados exitosamente!'); location.reload();", true);
                }
                else
                {
                    //chlCampos.Focus();
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe seleccionar al menos un criterio!'); ", true);
                }
            }
            else
            {
                ddltablas.Focus();
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe seleccionar al menos una tabla!'); ", true);
            }
        }
        #endregion
    }

    /// <summary>
    /// Correos
    /// </summary>    
    public class Correos : Negocio.Base_Page
    {
        private DataTable dt;

        /// <summary>
        /// Envio de correos
        /// </summary>
        public void DOCorreos()
        {
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    Correo correo = new Correo(usuario.Email.Trim(),
                                               "Fondo Emprender",
                                               dr["Email"].ToString().Trim(),
                                               dr["Nombres"].ToString() + " " + dr["Apellidos"].ToString(),
                                               "Fondo Emprender Actualizacion Masiva",
                                               "Registro a Fondo Emprender");
                    //correo.Enviar();
                }
                catch { }
            }
        }

        /// <summary>
        /// carga el dt con la auditoria
        /// </summary>
        public void inicio()
        {
            dt = consultas.ObtenerDataTable("MD_AuditoriaAdministrador");
        }
    }
}