using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Formulacion.Finanzas
{
    public partial class Intermedia : Negocio.Base_Page
    {
        public int CodigoProducto {
            get {
                return Convert.ToInt32(Request.QueryString["codproducto"]);
            }
            set { }
        }
        public int CodigoProyecto {
            get
            {
                return Convert.ToInt32(Request.QueryString["codproyecto"]);
            }
            set { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
                        
            lblTitulo.Text = (from pp in consultas.Db.ProyectoProductos
                              where pp.Id_Producto == CodigoProducto && pp.CodProyecto == CodigoProyecto
                              select pp.NomProducto).FirstOrDefault();

            if (!Page.IsPostBack)
            {
                CargarCombo();
            }
        }

        protected void ddlTipoInsumo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["idTipoInsumo"] = ddlTipoInsumo.SelectedValue;
            gvrProyectoInsumo.DataBind();
        }

        protected void lds_ProyectoInsumo_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var result = (from pi in consultas.Db.ProyectoInsumos
                          from ti in consultas.Db.TipoInsumos
                          where pi.codTipoInsumo == ti.Id_TipoInsumo
                          && pi.CodProyecto == CodigoProyecto
                          && !(from ppi in consultas.Db.ProyectoProductoInsumos where ppi.CodProducto == CodigoProducto select ppi.CodInsumo).Contains(pi.Id_Insumo)
                          orderby 2
                          select new
                          {
                              pi.Id_Insumo,
                              ti.NomTipoInsumo,
                              pi.nomInsumo,
                              pi.Presentacion,
                              pi.Unidad,
                              ti.Id_TipoInsumo
                          });

            if (ddlTipoInsumo.SelectedValue != "TODOS")
            {
                result = result.Where(r => r.Id_TipoInsumo == int.Parse(ddlTipoInsumo.SelectedValue));
            }

            e.Result = result.ToList();
        }

 

        private void CargarCombo()
        {
            var tiposInsumos = (from ti in consultas.Db.TipoInsumos
                                orderby ti.NomTipoInsumo
                                select ti).ToList();
            ddlTipoInsumo.DataSource = tiposInsumos;
            ddlTipoInsumo.DataValueField = "Id_TipoInsumo";
            ddlTipoInsumo.DataTextField = "NomTipoInsumo";
            ddlTipoInsumo.DataBind();
            ddlTipoInsumo.Items.Insert(0, new ListItem("Seleccione", "0"));
            ddlTipoInsumo.Items.Insert(1, new ListItem("TODOS", "TODOS"));
        }

        protected void lnkAgregarInsumo_Click(object sender, EventArgs e)
        {
            Redirect(null, "Insumo.aspx?codproyecto=" + CodigoProyecto + "&codproducto=" + CodigoProducto + "&codinsumo=" + 0, "_blank", "width=800,height=500,top=0,left=0,scrollbars=yes,resizable=yes,toolbar=yes");
        }

        protected void imgBtnAgregarInsumo_Click(object sender, ImageClickEventArgs e)
        {            
            Redirect(null, "Insumo.aspx?codproyecto=" + CodigoProyecto + "&codproducto=" + CodigoProducto + "&codinsumo=" + 0, "_blank", "width=800,height=500,top=0,left=0,scrollbars=yes,resizable=yes,toolbar=yes");
        }

        protected void btnAgrgarAProducto_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow gvr in gvrProyectoInsumo.Rows)
            {
                CheckBox checkbox = (CheckBox)gvr.Cells[1].FindControl("chk_insumo");

                if (checkbox != null)
                {
                    if (checkbox.Checked)
                    {
                        string txtSQL = "INSERT INTO ProyectoProductoInsumo (CodProducto, CodInsumo, Presentacion) " +
                         "SELECT " + CodigoProducto + ", id_insumo, Presentacion " +
                         "FROM ProyectoInsumo WHERE id_insumo in (" + gvrProyectoInsumo.DataKeys[gvr.RowIndex].Value.ToString() + ") and id_insumo not in (SELECT codInsumo FROM ProyectoProductoInsumo WHERE codProducto = " + CodigoProducto + ")";
                        ejecutaReader(txtSQL, 2);
                    }
                }
            }

            Response.Redirect("PlanDeCompras.aspx?codproyecto="+CodigoProyecto);
        }

        protected void gvrProyectoInsumo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "borrar":
                    string txtSQL = "Delete from ProyectoEmpleoManoObra where codmanoobra=" + e.CommandArgument;
                    ejecutaReader(txtSQL, 0);
                    txtSQL = "DELETE FROM ProyectoInsumoUnidadesCompras WHERE codInsumo=" + e.CommandArgument;
                    ejecutaReader(txtSQL, 0);
                    txtSQL = "DELETE FROM ProyectoInsumoPrecio WHERE codInsumo=" + e.CommandArgument;
                    ejecutaReader(txtSQL, 0);
                    txtSQL = "DELETE FROM ProyectoProductoInsumo WHERE codInsumo=" + e.CommandArgument;
                    ejecutaReader(txtSQL, 0);
                    txtSQL = "DELETE FROM ProyectoInsumo WHERE id_Insumo=" + e.CommandArgument;
                    ejecutaReader(txtSQL, 0);

                    gvrProyectoInsumo.DataBind();
                    break;
                case "Editar":                    
                    Redirect(null, "Insumo.aspx?codproyecto="+CodigoProyecto+"&codproducto="+CodigoProducto+"&codinsumo="+ e.CommandArgument, "_blank", "width=800,height=500,top=0,left=0,scrollbars=yes,resizable=yes,toolbar=yes");
                    break;
            }
        }

        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            Response.Redirect("PlanDeCompras.aspx?codproyecto="+ CodigoProyecto);
        }
    }
}