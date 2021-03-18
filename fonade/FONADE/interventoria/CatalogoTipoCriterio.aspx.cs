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
    public partial class CatalogoTipoCriterio : Negocio.Base_Page
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

            if (!IsPostBack)
            {
                if (CodCriterio == 0 && accion == "")
                {
                    if (CodCriterio != 0 && accion != "")
                    {
                        pnlPrincipal.Visible = false;
                        PanelModificar.Visible = false;
                        PnlActualizar.Visible = true;
                        llenarModificacion();
                    }
                    else
                    {
                        pnlPrincipal.Visible = true;
                        PnlActualizar.Visible = false;
                        PanelModificar.Visible = false;
                        ValidarCriterio();
                        lbltitulo.Text = "CRITERIO";
                        HttpContext.Current.Session["amb_accion"] = "";
                        CodCriterio = 0;
                    }
                }
                else
                {
                    pnlPrincipal.Visible = true;
                    PnlActualizar.Visible = false;
                    PanelModificar.Visible = false;
                    ValidarCriterio();
                    HttpContext.Current.Session["amb_accion"] = "";
                    CodCriterio = 0;
                    lbltitulo.Text = "CRITERIO";
                }
            }

            HttpContext.Current.Session["amb_accion"] = "";
            lbltitulo.Text = "CRITERIO";
        }

        private void ValidarCriterio()
        {
            try
            {


                var dtAmbito = consultas.ObtenerDataTable("MD_listarTipoCriterio");

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
                    HttpContext.Current.Session["id_InterventorInformeFinalCriterio"] = e.CommandArgument.ToString();
                    HttpContext.Current.Session["amb_accion"] = "EDITAR";
                    CodCriterio = Convert.ToInt32(HttpContext.Current.Session["id_InterventorInformeFinalCriterio"]);
                    llenarModificacion();

                    //Establecer título.
                    lbl_id_tipoCriterio.Text = "Id: " + CodCriterio.ToString();
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
            pnlPrincipal.Visible = false;
            PanelModificar.Visible = true;
        }

        protected void ibtn_crearCriterio_Click(object sender, ImageClickEventArgs e)
        {
            lbltitulo.Text = "NUEVO";
            pnlPrincipal.Visible = false;
            PanelModificar.Visible = true;
        }

        public void Guardar()
        {


        }

        protected void insertupdateCriterio(string caso, int id_Criterio, string nomTipoCriterio)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("MD_InsertUpdateTipoCriterio", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nomTipoCriterio", nomTipoCriterio);
                cmd.Parameters.AddWithValue("@IdTipoCriterio ", id_Criterio);
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
                var querysql = (from x in consultas.Db.InterventorInformeFinalCriterios
                                where x.NomInterventorInformeFinalCriterio == nomTipoCriterio

                                select new
                                {
                                    x.Id_InterventorInformeFinalCriterio

                                }).FirstOrDefault();


                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Creado exitosamente!'); window.location.href('CatalogoTipoCriterio.aspx');", true);
                txt_Nombre.Text = "";
            }

            else
            {

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Modificado exitosamente!'); window.location.href('CatalogoTipoCriterio.aspx');", true);
                txtNomUpd.Text = "";
            }
        }

        protected void btn_actualizar_Click(object sender, EventArgs e)
        {
            if (txt_Nombre.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe digitar el nombre del criterio.')", true);
                return;
            }
            else
            { validarInserUpdate("Create", 0, txt_Nombre.Text); lbltitulo.Text = "CRITERIO"; }
        }

        protected void validarInserUpdate(string caso, int IdCriterio, string nomTipoAmbito)
        {
            if (IdCriterio == 0)
            {

                var querysql = (from x in consultas.Db.InterventorInformeFinalCriterios
                                where x.NomInterventorInformeFinalCriterio == nomTipoAmbito
                                /*select new
                                 {
                                     x.NomTipoAmbito

                                }).FirstOrDefault();*/
                                select new { x }).Count();

                if (querysql == 0)
                {

                    insertupdateCriterio(caso, 0, nomTipoAmbito);

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Creado Exitosamente')", true);
                    ValidarCriterio();
                    txt_Nombre.Text = "";
                    pnlPrincipal.Visible = true;
                    lbltitulo.Text = "CRITERIO";
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
                        insertupdateCriterio(caso, IdCriterio, nomTipoAmbito);
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
                var query = (from x in consultas.Db.InterventorInformeFinalCriterios
                             where x.Id_InterventorInformeFinalCriterio == CodCriterio
                             select x).FirstOrDefault();

                txtNomUpd.Text = query.NomInterventorInformeFinalCriterio;



            }
            catch (Exception exc)
            { }




        }

        private void DeleteCriterio()
        {
            var obj = (from c in consultas.Db.InterventorInformeFinalCriterios
                       where c.Id_InterventorInformeFinalCriterio == CodCriterio
                       select c).FirstOrDefault();
            consultas.Db.InterventorInformeFinalCriterios.DeleteOnSubmit(obj);
            consultas.Db.SubmitChanges();

            ValidarCriterio();
            txt_Nombre.Text = "";
            pnlPrincipal.Visible = true;
            lbltitulo.Text = "CRITERIO";
            PanelModificar.Visible = false;
            PnlActualizar.Visible = false;
        }

        protected void btn_update_Click(object sender, EventArgs e)
        {
            if (txtNomUpd.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe digitar el nombre del criterio.')", true);
                return;
            }
            else
            { validarInserUpdate("Update", CodCriterio, txtNomUpd.Text); lbltitulo.Text = "CRITERIO"; }

        }

        protected void btnEliminat_Click(object sender, EventArgs e)
        {
            DeleteCriterio();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Registro eliminado exitosamente!');", true);
        }
    }
}