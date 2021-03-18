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
    public partial class CatalogoAmbito1 : Negocio.Base_Page
    {
        protected int CodAmbito;
        /// <summary>
        /// Variable declarada en el RowCommand, para establecer visibilidad de páneles.
        /// </summary>
        String accion;

        Datos.Ambito ambitoModificar;

        protected void Page_Load(object sender, EventArgs e)
        {
            CodAmbito = Convert.ToInt32(HttpContext.Current.Session["id_TipoAmbito"]);
            accion = HttpContext.Current.Session["amb_accion"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["amb_accion"].ToString()) ? HttpContext.Current.Session["amb_accion"].ToString() : "";
            if (!IsPostBack)
            {
                if (CodAmbito == 0 && accion == "")
                {
                    if (CodAmbito != 0 && accion != "")
                    {
                        PanelModificar.Visible = true;
                        pnlPrincipal.Visible = false;
                        PnlActualizar.Visible = false;
                        llenarModificacion();
                        HttpContext.Current.Session["amb_accion"] = "";
                    }
                    else
                    {
                        pnlPrincipal.Visible = true;
                        PanelModificar.Visible = false;
                        PnlActualizar.Visible = false;
                        HttpContext.Current.Session["amb_accion"] = "";
                        CodAmbito = 0;

                        ValidarAmbito();
                        //llenarTipoAmbito(ddlAmbito);
                        lbltitulo.Text = "ÁMBITO";
                    }
                }
                else
                {
                    pnlPrincipal.Visible = true;
                    PanelModificar.Visible = false;
                    PnlActualizar.Visible = false;
                    HttpContext.Current.Session["amb_accion"] = "";
                    CodAmbito = 0;

                    ValidarAmbito();
                    //llenarTipoAmbito(ddlAmbito);
                    lbltitulo.Text = "ÁMBITO";
                }
                llenarTipoAmbito(ddlTipo);
                llenarTipoAmbito(ddlAmbito);
            }

            HttpContext.Current.Session["amb_accion"] = "";
            lbltitulo.Text = "ÁMBITO";
        }

        private void ValidarAmbito()
        {
            if (usuario.CodGrupo == Constantes.CONST_Interventor)
            {
            }

            try
            {


                var dtAmbito = consultas.ObtenerDataTable("MD_listarAmbitos");
           
                if (dtAmbito.Rows.Count != 0)
                {
                    HttpContext.Current.Session["dtAmbito"] = dtAmbito;
                    gvcAmbito.DataSource = dtAmbito;
                    gvcAmbito.DataBind();
                }
                else
                {
                    gvcAmbito.DataSource = dtAmbito;
                    gvcAmbito.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void gvcAmbitoPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvcAmbito.PageIndex = e.NewPageIndex;
            gvcAmbito.DataSource = HttpContext.Current.Session["dtAmbito"];
            gvcAmbito.DataBind();
        }

        protected void gvcAmbitoRowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.ToString())
            {
                case "editacontacto":
                    //Establecer valores en variable de sesión y demás datos.
                    HttpContext.Current.Session["id_TipoAmbito"] = e.CommandArgument.ToString();
                    HttpContext.Current.Session["amb_accion"] = "EDITAR";
                    CodAmbito = Convert.ToInt32(HttpContext.Current.Session["id_TipoAmbito"]);
                    llenarModificacion();

                    //Establecer título.
                    lbl_id_ambito.Text = "Id: " + CodAmbito.ToString();
                    lbltitulo.Text = "EDITAR";

                    //Ocultar y mostrar ciertos páneles.
                    PnlActualizar.Visible = true;
                    PanelModificar.Visible = false;
                    pnlPrincipal.Visible = false;
                    break;
            }
        }

        protected void gvcAmbitoSorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void lbtn_crearAmbito_Click(object sender, EventArgs e)
        {
            lbltitulo.Text = "NUEVO";
            PanelModificar.Visible = true;
            pnlPrincipal.Visible = false;
        }

        protected void ibtn_crearAmbito_Click(object sender, ImageClickEventArgs e)
        {
            lbltitulo.Text = "NUEVO";
            pnlPrincipal.Visible = false;
            PanelModificar.Visible = true;
        }

        protected void llenarTipoAmbito(DropDownList lista)
        {



            string SQL = "select id_TipoAmbito, nomTipoAmbito from TipoAmbito";

            DataTable dt = consultas.ObtenerDataTable(SQL, "text");

            lista.DataSource = dt;
            lista.DataTextField = "nomTipoAmbito";
            lista.DataValueField = "id_TipoAmbito";
            lista.DataBind();


        }

        public void Guardar()
        {


        }

        protected void insertupdateAmbito(string caso, int id_Ambito, string nomTipoAmbito)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("MD_InsertUpdateAmbito", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nomAmbito", nomTipoAmbito);
                cmd.Parameters.AddWithValue("@IdAmbito ", id_Ambito);
                cmd.Parameters.AddWithValue("@caso", caso);


                SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
                con.Open();
                cmd2.ExecuteNonQuery();
                cmd.ExecuteNonQuery();
                //con.Close();
                //con.Dispose();
                cmd2.Dispose();
                cmd.Dispose();
            }
            finally {
                con.Close();
                con.Dispose();
            
            }
            if (caso == "Create")
            {
                var querysql = (from x in consultas.Db.Ambitos
                                where x.NomAmbito == nomTipoAmbito

                                select new
                                {
                                    x.id_ambito

                                }).FirstOrDefault();


                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Creado exitosamente!'); window.location.href('CatalogoTipoAmbito.aspx');", true);
                txt_Nombre.Text = "";
            }

            else
            {

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Modificado exitosamente!'); window.location.href('CatalogoTipoAmbito.aspx');", true);
                txtNomUpd.Text = "";
            }
        }

        protected void btn_actualizar_Click(object sender, EventArgs e)
        {
            if (txt_Nombre.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe digitar el nombre del ámbito.')", true);
                return;
            }
            if (ddlAmbito.SelectedValue == "" || ddlAmbito.SelectedValue == null)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe Seleccionar el tipo de ámbito.')", true);
                return;
            }
            else
            { 
                //validarInserUpdate("Create", 0, txt_Nombre.Text); lbltitulo.Text = "ÁMBITO"; 
                var ambito = new Datos.Ambito
                {
                    NomAmbito = txt_Nombre.Text.Trim(),
                    CodTipoAmbito = int.Parse(ddlAmbito.SelectedValue)
                };

                consultas.Db.Ambitos.InsertOnSubmit(ambito);
                consultas.Db.SubmitChanges();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Creado Exitosamente')", true);
                ValidarAmbito();
                txt_Nombre.Text = "";
                pnlPrincipal.Visible = true;
                lbltitulo.Text = "ÁMBITO";
                PanelModificar.Visible = false;
                PnlActualizar.Visible = false;
            }

            HttpContext.Current.Session["amb_accion"] = "";
        }

        protected void validarInserUpdate(string caso, int IdAmbito, string nomTipoAmbito)
        {
            if (IdAmbito == 0)
            {

                var querysql = (from x in consultas.Db.Ambitos
                                where x.NomAmbito == nomTipoAmbito
                                /*select new
                                 {
                                     x.NomTipoAmbito

                                }).FirstOrDefault();*/
                                select new { x }).Count();

                if (querysql == 0)
                {

                    insertupdateAmbito(caso, 0, nomTipoAmbito);

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Creado Exitosamente')", true);
                    ValidarAmbito();
                    txt_Nombre.Text = "";
                    pnlPrincipal.Visible = true;
                    lbltitulo.Text = "ÁMBITO";
                    PanelModificar.Visible = false;
                    PnlActualizar.Visible = false;


                }

                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El codigo del Usuario ya se encuentra registrado')", true);
                }



            }

            else
            {
                var sql = (from x in consultas.Db.Ambitos
                           where x.id_ambito != IdAmbito
                          && x.NomAmbito == nomTipoAmbito
                           select new { x }).Count();

                if (sql == 0)
                {
                    var query2 = (from x2 in consultas.Db.Ambitos
                                  where x2.id_ambito != IdAmbito
                                  && x2.id_ambito == Convert.ToInt32(IdAmbito)

                                  select new { x2 }).Count();

                    if (query2 == 0)
                    {
                        insertupdateAmbito(caso, IdAmbito, nomTipoAmbito);
                        ValidarAmbito();
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
                var query = (from x in consultas.Db.Ambitos
                             where x.id_ambito == CodAmbito
                             select x).FirstOrDefault();

                Session["AmbitoModificar"] = query;

                txtNomUpd.Text = query.NomAmbito;

                //llenarTipoAmbito(ddlTipo);
                ddlTipo.SelectedValue = query.CodTipoAmbito.ToString();

            }
            catch (Exception exc)
            { }




        }

        protected void btn_update_Click(object sender, EventArgs e)
        {
            if (txtNomUpd.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe digitar el nombre del ámbito.')", true);
                return;
            }
            if (ddlTipo.SelectedValue == "" || ddlTipo.SelectedValue == null)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe Seleccionar el tipo de ámbito.')", true);
                return;
            }
            else
            {
                ambitoModificar = (Datos.Ambito)Session["AmbitoModificar"];
                var ambito = (from a in consultas.Db.Ambitos
                              where a.id_ambito == ambitoModificar.id_ambito
                              select a).FirstOrDefault();

                ambito.NomAmbito = txtNomUpd.Text.Trim();
                ambito.CodTipoAmbito = int.Parse(ddlTipo.SelectedValue);
                consultas.Db.SubmitChanges();
                lbltitulo.Text = "ÁMBITO";
                ValidarAmbito();
                pnlPrincipal.Visible = true;
                PanelModificar.Visible = false;
                PnlActualizar.Visible = false;
            }

            HttpContext.Current.Session["amb_accion"] = "";
        }

        protected void btn_Delete_Click(object sender, EventArgs e)
        {
            if (usuario.CodGrupo == Constantes.CONST_Interventor)
            {
            }

            try
            {
                consultas.Parameters = null;

                consultas.Parameters = new[]
                                           {
                                               new SqlParameter
                                                   {
                                                       ParameterName = "@caso",
                                                       Value = "Delete"
                                                   },
                                                   new SqlParameter
                                                   {
                                                       ParameterName= "@IdAmbito",
                                                       Value= CodAmbito
                                                   }
                                           };
                try
                {
                    var dtAmbito = consultas.ObtenerDataTable("MD_DeleteAmbito");

                    if (dtAmbito.Rows.Count != 0)
                    {
                        HttpContext.Current.Session["Ambito"] = dtAmbito;
                        gvcAmbito.DataSource = dtAmbito;
                        gvcAmbito.DataBind();
                    }
                    else
                    {
                        gvcAmbito.DataSource = dtAmbito;
                        gvcAmbito.DataBind();
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Registro Borrado con exito')", true);
                        lbltitulo.Text = "ÁMBITO";
                        ValidarAmbito();
                        pnlPrincipal.Visible = true;
                        PanelModificar.Visible = false;
                        PnlActualizar.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Registro No pudo ser eliminado')", true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            HttpContext.Current.Session["amb_accion"] = "";
        }
    }
}