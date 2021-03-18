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
using System.Data.Linq.SqlClient;


namespace Fonade.FONADE.interventoria
{
    public partial class CatalogoAmbito : Negocio.Base_Page
    {
        protected int CodAmbito;
        String txtSQL = String.Empty;

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                LlenarGrid();
                pnlPrincipal.Visible = true;
            }
        }

        protected void gvcAmbitoPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvcAmbito.PageIndex = e.NewPageIndex;
            LlenarGrid();
        }

        protected void gvcAmbitoRowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "editacontacto")
            {
                var idAmbito = int.Parse(e.CommandArgument.ToString());

                Session["idAmbito"] = idAmbito.ToString();
                DatosModificar(idAmbito);
            }
        }

        protected void btn_actualizar_Click(object sender, EventArgs e)
        {
            InsertarTipoAmbito();
        }

        protected void ibtn_crearAmbito_Click(object sender, ImageClickEventArgs e)
        {
            InsertarTipoAmbito();
        }

        protected void Btnupdate_Click(object sender, EventArgs e)
        {
            ActualizarTipoAmbito();
        }

        protected void BtnBorrar_Click(object sender, EventArgs e)
        {
            ElimminarTipoAmbito();
        }

        protected void lbtn_crearAmbito_Click(object sender, EventArgs e)
        {

            PanelModificar.Visible = true;
            pnlPrincipal.Visible = false;
        }
        #endregion

        #region Metodos
        private void LlenarGrid()
        {
            var dtAmbito = consultas.ObtenerDataTable("MD_listarTipoAmbitos");
            gvcAmbito.DataSource = dtAmbito;
            gvcAmbito.DataBind();
        }

        private void InsertarTipoAmbito()
        {
            var nomTipoAmbito = txt_Nombre.Text.Trim();
            var ambito = (from ta in consultas.Db.TipoAmbitos
                          where SqlMethods.Like(ta.NomTipoAmbito, nomTipoAmbito)
                          select ta).FirstOrDefault();
            if (ambito == null)
            {
                var objAmbito = new Datos.TipoAmbito
                {
                    NomTipoAmbito = nomTipoAmbito
                };
                var query = "Insert TipoAmbito Values('" + nomTipoAmbito + "')";
                ejecutaReader(query, 2);
                LlenarGrid();
                pnlPrincipal.Visible = true;
                PanelModificar.Visible = false;
                
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Tipo de ambito creado satisfactoriamente!'); ", true);
            }
            else
            {
                txt_Nombre.Focus();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El nombre del tipo de ambito ya se encuentra creado')", true);
            }
        }

        protected void DatosModificar(int idAmbito)
        {
            var query = (from ta in consultas.Db.TipoAmbitos
                         where ta.Id_TipoAmbito == idAmbito
                         select ta).FirstOrDefault();

            txtNomUpd.Text = query.NomTipoAmbito;
            pnlPrincipal.Visible = false;
            PnlActualizar.Visible = true;
        }

        private void ActualizarTipoAmbito()
        {
            var nomTipoAmbito = txtNomUpd.Text.Trim();
            var IdAmbito = int.Parse(Session["idAmbito"].ToString());

            var ambitoNombre = (from ta in consultas.Db.TipoAmbitos
                                where SqlMethods.Like(ta.NomTipoAmbito, nomTipoAmbito)
                                select ta).FirstOrDefault();

            if (ambitoNombre == null)
            {
                var ambitoId = (from ta in consultas.Db.TipoAmbitos
                                where ta.Id_TipoAmbito == IdAmbito
                                select ta).FirstOrDefault();
                if (ambitoId != null)
                {
                    var query = "Update TipoAmbito set NomTipoAmbito = '" + nomTipoAmbito + "' Where Id_TipoAmbito = " + IdAmbito;
                    ejecutaReader(query, 2);
                    LlenarGrid();
                    pnlPrincipal.Visible = true;
                    PnlActualizar.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Tipo de ambito actualizado satisfactoriamente!')", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No existe un tipo de ambito con el Id ingrsado!')", true);
                }
            }
            else
            {
                txtNomUpd.Focus();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Ya existe un tipo de ambito con el nombre ingresado!')", true);
            }
        }

        private void ElimminarTipoAmbito()
        {
            var idAmbito = int.Parse(Session["idAmbito"].ToString());
            var query = "Delete from TipoAmbito where Id_TipoAmbito = " + idAmbito;
            ejecutaReader(query, 2);

            LlenarGrid();
            pnlPrincipal.Visible = true;
            PnlActualizar.Visible = false;

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El tipo de ambito se eliminó satisfactoriamente')", true);
        }
        #endregion
    }


}
 