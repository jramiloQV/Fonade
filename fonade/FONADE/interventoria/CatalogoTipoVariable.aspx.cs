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
    public partial class CatalogoTipoVariable : Negocio.Base_Page
    {
        protected int CodCriterio;

        protected void Page_Load(object sender, EventArgs e)
        {

            CodCriterio = Convert.ToInt32(HttpContext.Current.Session["id_TipoVariable"]);
            if (!IsPostBack)
            {
                if (CodCriterio == 0)
                {

                    //lbl_Titulo.Text = void_establecerTitulo("Nuevo Usuario Coordinador de Interventoria");
                    pnlPrincipal.Visible = true;

                    ValidarCriterio();

                }
                else
                {


                    PanelModificar.Visible = false;
                    pnlPrincipal.Visible = true;
                    ValidarCriterio();
                    llenarModificacion();

                }
            }

        }

        private void ValidarCriterio()
        {
            if (usuario.CodGrupo == Constantes.CONST_Interventor)
            {
            }

            try
            {


                var dtAmbito = consultas.ObtenerDataTable("MD_listarTipoVariable");

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
                    HttpContext.Current.Session["id_TipoVariable"] = e.CommandArgument.ToString();
                    CodCriterio = Convert.ToInt32(HttpContext.Current.Session["id_TipoVariable"]);
                    llenarModificacion();

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

            PanelModificar.Visible = true;
            pnlPrincipal.Visible = false;
        }

        protected void ibtn_crearCriterio_Click(object sender, ImageClickEventArgs e)
        {
            PanelModificar.Visible = true;
            validarInserUpdate("Create", 0, txt_Nombre.Text);

        }


        /*protected void llenarTipoAmbito(DropDownList lista)
        {



            string SQL = "select id_TipoAmbito, nomTipoAmbito from TipoAmbito";

            DataTable dt = consultas.ObtenerDataTable(SQL, "text");

            lista.DataSource = dt;
            lista.DataTextField = "nomTipoAmbito";
            lista.DataValueField = "id_TipoAmbito";
            lista.DataBind();

        }*/

        public void Guardar()
        {


        }
        protected void insertupdateCriterio(string caso, int id_Criterio, string nomTipoCriterio)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("MD_InsertUpdateTipoVariable", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nomTipoVariable", nomTipoCriterio);
                cmd.Parameters.AddWithValue("@IdTipoVariable", id_Criterio);
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
                var querysql = (from x in consultas.Db.TipoVariables
                                where x.NomTipoVariable == nomTipoCriterio

                                select new
                                {
                                    x.Id_TipoVariable

                                }).FirstOrDefault();


                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Creado exitosamente!'); window.location.href('CatalogoTipoVariable.aspx');", true);

            }

            else
            {

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Modificado exitosamente!'); window.location.href('CatalogoTipoVariable.aspx');", true);
            }
        }



        protected void btn_actualizar_Click(object sender, EventArgs e)
        {
            validarInserUpdate("Create", 0, txt_Nombre.Text);





        }
        protected void validarInserUpdate(string caso, int IdCriterio, string nomTipoAmbito)
        {
            if (IdCriterio == 0)
            {

                var querysql = (from x in consultas.Db.TipoVariables
                                where x.NomTipoVariable == nomTipoAmbito
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

                    pnlPrincipal.Visible = true;
                    PanelModificar.Visible = false;
                    PnlActualizar.Visible = false;


                }

                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La Variable se Encuentra Registrada')", true);
                }








            }

            else
            {
                var sql = (from x in consultas.Db.TipoVariables
                           where x.Id_TipoVariable != IdCriterio
                          && x.NomTipoVariable == nomTipoAmbito
                           select new { x }).Count();

                if (sql == 0)
                {
                    var query2 = (from x2 in consultas.Db.TipoVariables
                                  where x2.Id_TipoVariable != IdCriterio
                                  && x2.Id_TipoVariable== Convert.ToInt32(IdCriterio)

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
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La Variable se encuentra Registrada')", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Ya existe una variable con esa Descripcion !')", true);
                }
            }
        }
        protected void llenarModificacion()
        {
            try
            {
                var query = (from x in consultas.Db.TipoVariables
                             where x.Id_TipoVariable == CodCriterio
                             select new { x }).FirstOrDefault();

                txtNomUpd.Text = query.x.NomTipoVariable;



            }
            catch (Exception exc)
            { }




        }

        protected void btn_update_Click(object sender, EventArgs e)
        {
            validarInserUpdate("Update", CodCriterio, txtNomUpd.Text);

        }

        protected void btnBorrar_Click(object sender, EventArgs e)
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
                                                       ParameterName= "@IdTipoVariable",
                                                       Value= CodCriterio
                                                   }
                                           };
                try
                {
                    var dtCriterio = consultas.ObtenerDataTable("MD_DeleteTipoVariable");

                    if (dtCriterio.Rows.Count != 0)
                    {
                        HttpContext.Current.Session["Ambito"] = dtCriterio;
                        gvcCriterio.DataSource = dtCriterio;
                        gvcCriterio.DataBind();
                    }
                    else
                    {
                        var dtAmbito = consultas.ObtenerDataTable("MD_listarTipoVariable");
                        gvcCriterio.DataSource = dtAmbito;
                        gvcCriterio.DataBind();
                        pnlPrincipal.Visible = true;
                        PnlActualizar.Visible = false;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Registro Borrado con exito')", true);
                    }

                    txtNomUpd.Text = "";
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
        }
    }
}