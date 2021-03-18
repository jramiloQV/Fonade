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

namespace Fonade.FONADE.interventoria
{
    public partial class CatalogoCriterioDetalle : Negocio.Base_Page
    {
        protected int CodCriterio;
        /// <summary>
        /// Variable declarada en el RowCommand, para establecer visibilidad de páneles.
        /// </summary>
        String accion;

        protected void Page_Load(object sender, EventArgs e)
        {
            CodCriterio = Convert.ToInt32(HttpContext.Current.Session["id_InterventorInformeFinalCriterio"]);
            accion = HttpContext.Current.Session["amb_accion"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["amb_accion"].ToString()) ? HttpContext.Current.Session["amb_accion"].ToString() : "";
            //ValidarCriterio();
            //llenarTipoVerificacion(ddlTipoVerificacion);

            if (!IsPostBack)
            {
                if (CodCriterio == 0 && accion == "")
                {
                    if (CodCriterio != 0 && accion != "")
                    {
                        pnlPrincipal.Visible = false;
                        PanelModificar.Visible = false;
                        PnlActualizar.Visible = true;
                        llenarTipoVerificacion(ddlTipoVerificacion1);
                        llenarModificacion();
                    }
                    else
                    {
                        pnlPrincipal.Visible = true;
                        PanelModificar.Visible = false;
                        PnlActualizar.Visible = false;
                        ValidarCriterio();
                        llenarTipoVerificacion(ddlTipoVerificacion);
                        HttpContext.Current.Session["amb_accion"] = "";
                        CodCriterio = 0;
                        lbltitulo.Text = "Cumplimiento a Verificar";
                    }
                }
                else
                {
                    pnlPrincipal.Visible = true;
                    PanelModificar.Visible = false;
                    PnlActualizar.Visible = false;
                    ValidarCriterio();
                    llenarTipoVerificacion(ddlTipoVerificacion);
                    llenarModificacion();
                    HttpContext.Current.Session["amb_accion"] = "";
                    CodCriterio = 0;
                    lbltitulo.Text = "Cumplimiento a Verificar";
                }
            }

            HttpContext.Current.Session["amb_accion"] = "";
            lbltitulo.Text = "Cumplimiento a Verificar";
        }

        private void ValidarCriterio()
        {
            /*if (usuario.CodGrupo == Constantes.CONST_Interventor)
            {
            }*/

            try
            {
                var dtAmbito = consultas.ObtenerDataTable("MD_listarTipoVerificacion");

                if (dtAmbito.Rows.Count != 0)
                {
                    HttpContext.Current.Session["dtAmbito"] = dtAmbito;
                    gvcCriterio.DataSource = dtAmbito;
                    gvcCriterio.DataBind();
                }
                else
                {
                    gvcCriterio.DataSource = dtAmbito;
                    gvcCriterio.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void gvcCriterioPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvcCriterio.PageIndex = e.NewPageIndex;
            gvcCriterio.DataSource = HttpContext.Current.Session["dtAmbito"];
            gvcCriterio.DataBind();
        }

        protected void gvcCriterioRowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.ToString())
            {
                case "editacontacto":
                    //Establecer valores en variable de sesión y demás campos.
                    HttpContext.Current.Session["Id_InterventorInformeFinalItem"] = e.CommandArgument.ToString();
                    CodCriterio = Convert.ToInt32(HttpContext.Current.Session["Id_InterventorInformeFinalItem"]);
                    llenarTipoVerificacion(ddlTipoVerificacion1);
                    llenarModificacion();
                    

                    //Establecer título.
                    lbl_id_TipoAmbito.Text = "Id: " + CodCriterio.ToString();
                    lbltitulo.Text = "EDITAR";

                    //Ocultar y mostrar ciertos páneles.
                    PnlActualizar.Visible = true;
                    PanelModificar.Visible = false;
                    pnlPrincipal.Visible = false;
                    break;
            }
        }

        protected void gvcCriterioSorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void lbtn_crearCriterio_Click(object sender, EventArgs e)
        {
            lbltitulo.Text = "NUEVO";
            PanelModificar.Visible = true;
            pnlPrincipal.Visible = false;
        }

        protected void ibtn_crearCriterio_Click(object sender, ImageClickEventArgs e)
        {
            lbltitulo.Text = "NUEVO";
            PanelModificar.Visible = true;
            pnlPrincipal.Visible = false;
        }

        protected void llenarTipoVerificacion(DropDownList lista)
        {
            string SQL = "select Id_InterventorInformeFinalCriterio, NomInterventorInformeFinalCriterio from InterventorInformeFinalCriterio";

            DataTable dt = consultas.ObtenerDataTable(SQL, "text");

            lista.DataSource = dt;
            lista.DataTextField = "NomInterventorInformeFinalCriterio";
            lista.DataValueField = "Id_InterventorInformeFinalCriterio";
            lista.DataBind();
        }

        public void Guardar()
        {}

        protected void insertupdateCriterio(string caso, int CodCriterio, int idTipoInforme, string nomTipoCriterio)
        {
            //Inicializar variables.
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            String txtSQL = "";
            bool correcto = false;

            try
            {
                if (caso == "Delete")
                {
                    #region Eliminación.
                    txtSQL = " DELETE FROM InterventorInformeFinalItem WHERE Id_InterventorInformeFinalItem = " + CodCriterio;

                    cmd = new SqlCommand(txtSQL, connection);
                    correcto = EjecutarSQL(connection, cmd);

                    if (correcto == true)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Eliminado exitosamente!'); window.location.href('CatalogoCriterioDetalle.aspx');", true);
                        txtNomUpd.Text = "";
                        HttpContext.Current.Session["amb_accion"] = "";
                        CodCriterio = 0;
                        pnlPrincipal.Visible = true;
                        PanelModificar.Visible = false;
                        PnlActualizar.Visible = false;
                        ValidarCriterio();
                        llenarTipoVerificacion(ddlTipoVerificacion);
                        lbltitulo.Text = "Cumplimiento a Verificar";
                        txtCriterio.Text = "";
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se puedo eliminar el registro seleccionado.');", true);
                        return;
                    }
                    #endregion
                }
                if (caso == "Create")
                {
                    #region Inserción.

                    var querysql = (from x in consultas.Db.InterventorInformeFinalItems
                                    where x.NomInterventorInformeFinalItem == txt_Nombre.Text.Trim()
                                    select new { x }).Count();

                    if (querysql == 0)
                    {
                        #region Hace la inserción.
                        txtSQL = " Insert into InterventorInformeFinalItem (NomInterventorInformeFinalItem, CodInformeFinalCriterio) " +
                                                         " values ('" + txt_Nombre.Text.Trim() + "', " + ddlTipoVerificacion.SelectedValue + ")";

                        cmd = new SqlCommand(txtSQL, connection);
                        correcto = EjecutarSQL(connection, cmd);

                        if (correcto == true)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Creado exitosamente!'); window.location.href('CatalogoCriterioDetalle.aspx');", true);
                            txt_Nombre.Text = "";
                            HttpContext.Current.Session["amb_accion"] = "";
                            lbltitulo.Text = "Cumplimiento a Verificar";
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se puedo generar el nuevo registro.');", true);
                            return;
                        }
                        #endregion
                        querysql = 0;
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El criterio ya se encuentra registrado')", true);
                        querysql = 0;
                        return;
                    }

                    #endregion
                }
                if (caso == "Update")
                {
                    #region Actualización.
                    txtSQL = " Update InterventorInformeFinalItem set NomInterventorInformeFinalItem = '" + txtNomUpd.Text + "' , " +
                             " CodInformeFinalCriterio = " + ddlTipoVerificacion1.SelectedValue +
                             " WHERE Id_InterventorInformeFinalItem = " + HttpContext.Current.Session["Id_InterventorInformeFinalItem"].ToString();

                    cmd = new SqlCommand(txtSQL, connection);
                    correcto = EjecutarSQL(connection, cmd);

                    if (correcto == true)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Modificado exitosamente!'); window.location.href('CatalogoCriterioDetalle.aspx');", true);
                        txtNomUpd.Text = "";
                        HttpContext.Current.Session["amb_accion"] = "";
                        CodCriterio = 0;
                        pnlPrincipal.Visible = true;
                        PanelModificar.Visible = false;
                        PnlActualizar.Visible = false;
                        ValidarCriterio();
                        llenarTipoVerificacion(ddlTipoVerificacion);
                        lbltitulo.Text = "Cumplimiento a Verificar";
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se puedo actualizar el registro seleccionado.');", true);
                        return;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error: " + ex.Message + "');", true);
                return;
            }
        }

        protected void btn_actualizar_Click(object sender, EventArgs e)
        {
            if (txt_Nombre.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe digitar el nombre del cumplimiento a verificar.')", true);
                return;
            }
            if (ddlTipoVerificacion.SelectedValue == "" || ddlTipoVerificacion.SelectedValue == null)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe Seleccionar el criterio.')", true);
                return;
            }
            else
            {
                validarInserUpdate("Create", 0, Convert.ToInt32(ddlTipoVerificacion.SelectedValue), txt_Nombre.Text);
                lbltitulo.Text = "Cumplimiento a Verificar";
            }
        }

        protected void validarInserUpdate(string caso, int IdCriterio, int IdtipoInforme, string nomTipoAmbito)
        {
            if (IdCriterio == 0)
            {

                var querysql = (from x in consultas.Db.InterventorInformeFinalItems
                                where x.NomInterventorInformeFinalItem == nomTipoAmbito
                                select new { x }).Count();

                if (querysql == 0)
                {
                    insertupdateCriterio(caso, 0, Convert.ToInt32(ddlTipoVerificacion.SelectedValue), nomTipoAmbito);

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Creado Exitosamente')", true);
                    ValidarCriterio();
                    txt_Nombre.Text = "";
                    pnlPrincipal.Visible = true;
                    PanelModificar.Visible = false;
                    PnlActualizar.Visible = false;
                }

                else
                {
                    //insertupdateCriterio(caso, IdCriterio, Convert.ToInt32(ddlTipoVerificacion.SelectedValue), nomTipoAmbito);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El criterio ya se encuentra registrado')", true);
                }
            }

            else
            {
                var sql = (from x in consultas.Db.InterventorInformeFinalCriterios
                           where x.Id_InterventorInformeFinalCriterio != IdCriterio
                          && x.NomInterventorInformeFinalCriterio == nomTipoAmbito
                           select new { x }).Count();

                if (sql == 0)
                {
                    var query2 = (from x2 in consultas.Db.InterventorInformeFinalCriterios
                                  where x2.Id_InterventorInformeFinalCriterio != IdCriterio
                                  && x2.Id_InterventorInformeFinalCriterio == Convert.ToInt32(IdCriterio)

                                  select new { x2 }).Count();

                    if (query2 == 0)
                    {
                        insertupdateCriterio(caso, IdCriterio, IdtipoInforme, nomTipoAmbito);
                        ValidarCriterio();
                        pnlPrincipal.Visible = true;
                        PanelModificar.Visible = false;
                        PnlActualizar.Visible = false;
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El codigo de Usuario ya se encuentra registrado')", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Ya existe un usuario con ese correo electrónico!')", true);
                }
            }
        }

        protected void llenarModificacion()
        {
            try
            {

                // Se realizan cambios a esta consulta para que el sistema siempre este mostrando el criterio 
                // al que corresponde el cumplimiento a verificar
                // Novembre 24 de 2014 - Alex Flautero

                var queryContacto = (from x in consultas.Db.InterventorInformeFinalItems
                                     from x2 in consultas.Db.InterventorInformeFinalCriterios
                                     where x.Id_InterventorInformeFinalItem == CodCriterio
                                     && x.CodInformeFinalCriterio == x2.Id_InterventorInformeFinalCriterio
                                     select new { x, x2.Id_InterventorInformeFinalCriterio,x2.NomInterventorInformeFinalCriterio}).FirstOrDefault();
                // en la línea anterior se agregó el campo NomInterventorInformeFinalCriterio
                // Alex Flautero - nov 24 de 2014

                txtNomUpd.Text = queryContacto.x.NomInterventorInformeFinalItem;
                ddlTipoVerificacion.SelectedValue = queryContacto.x.CodInformeFinalCriterio.ToString();

                // Se agrega la asignación del criterio del cuplimiento  verificar
                // Alex Flautero - nov 24 de 2014
                txtCriterio.Text = queryContacto.NomInterventorInformeFinalCriterio.ToString();
                ddlTipoVerificacion1.SelectedValue = queryContacto.x.CodInformeFinalCriterio.ToString();
            }
            catch { }
        }

        protected void btn_update_Click(object sender, EventArgs e)
        {
            if (txtNomUpd.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe digitar el nombre del cumplimiento a verificar.')", true);
                return;
            }
            if (ddlTipoVerificacion1.SelectedValue == "" || ddlTipoVerificacion1.SelectedValue == null)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe Seleccionar el criterio.')", true);
                return;
            }
            else
            { 
                insertupdateCriterio("Update", CodCriterio, Convert.ToInt32(ddlTipoVerificacion1.Text), txtNomUpd.Text);
                lbltitulo.Text = "Cumplimiento a Verificar"; 
            }
        }

        protected void btn_delete_Click(object sender, EventArgs e)
        {
            var id = lbl_id_TipoAmbito.Text.Split(':');
            var obj = (from c in consultas.Db.InterventorInformeFinalItems
                       where c.Id_InterventorInformeFinalItem == int.Parse(id[1].Trim())
                       select c).FirstOrDefault();
            consultas.Db.InterventorInformeFinalItems.DeleteOnSubmit(obj);
            consultas.Db.SubmitChanges();

            ValidarCriterio();
            txt_Nombre.Text = "";
            pnlPrincipal.Visible = true;
            PanelModificar.Visible = false;
            PnlActualizar.Visible = false;
        }
    }
}