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

namespace Fonade.FONADE.Proyecto
{
    public partial class VerInsumos : Negocio.Base_Page
    {

        public int proyecto;
        public int producto;
        public int insumo;
        protected void Page_Load(object sender, EventArgs e)
        {
            proyecto = Convert.ToInt32(HttpContext.Current.Session["CodProyecto"]);
            producto = Convert.ToInt32(HttpContext.Current.Session["CodProducto"]);
            insumo = Convert.ToInt32(HttpContext.Current.Session["Insumo"]);
            if (!IsPostBack)
            {
                llenarTipoInsumo(ddl_tipoinsumo);
                ListItem itemTotal = new ListItem("Total", "0");
                ddl_tipoinsumo.Items.Add(itemTotal);
                ddl_tipoinsumo.SelectedIndex = ddl_tipoinsumo.Items.Count - 1;
            }
        }

        protected void btn_agregarprod_Click(object sender, EventArgs e)
        {

        }

        protected void lds_Insumos_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            try
            {
                var query = from P in consultas.listarCatalogoInsumo(proyecto, Convert.ToInt32(ddl_tipoinsumo.SelectedValue), producto)
                            select P;
                e.Result = query;
            }
            catch (Exception)
            { }
        }

        protected void btn_addinsumo_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["CodProyecto"] = "49781";
            HttpContext.Current.Session["CodProducto"] = "90824";
            HttpContext.Current.Session["Insumo"] = "0";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "window.open('CatalogoInsumo.aspx','_blank','width=602,height=600,toolbar=no, scrollbars=no, resizable=no');", true);
        }

        protected void ddl_tipoinsumo_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvInsumos.DataBind();
        }

        protected void llenarTipoInsumo(DropDownList dll_lista)
        {
            var query = from c in consultas.Db.TipoInsumos
                        select new
                        {
                            idtipo = c.Id_TipoInsumo,
                            nomtipo = c.NomTipoInsumo,
                        };
            dll_lista.DataSource = query.ToList();
            dll_lista.DataTextField = "nomtipo";
            dll_lista.DataValueField = "idtipo";
            dll_lista.DataBind();
        }
    }


}