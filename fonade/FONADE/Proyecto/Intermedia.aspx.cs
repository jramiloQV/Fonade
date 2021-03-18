using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.Proyecto
{
    public partial class Intermedia : Negocio.Base_Page
    {
        int codProducto;
        int codProyecto;

        protected void Page_Load(object sender, EventArgs e)
        {
            codProducto = HttpContext.Current.Session["codProducto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["codProducto"].ToString()) ? int.Parse(HttpContext.Current.Session["codProducto"].ToString()) : 0;
            codProyecto = HttpContext.Current.Session["codProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["codProyecto"].ToString()) ? int.Parse(HttpContext.Current.Session["codProyecto"].ToString()) : 0;

            lblTitulo.Text =  (from pp in consultas.Db.ProyectoProductos
                                                      where pp.Id_Producto == codProducto && pp.CodProyecto == codProyecto 
                                                      select pp.NomProducto).FirstOrDefault();

            if(!Page.IsPostBack)
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
                          && pi.CodProyecto == codProyecto
                          && !(from ppi in consultas.Db.ProyectoProductoInsumos where ppi.CodProducto == codProducto select ppi.CodInsumo).Contains(pi.Id_Insumo)
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

        //protected void ldsTipoInsumo_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        //{
        //    var tipoInsumo = (from ti in consultas.Db.TipoInsumos
        //                      orderby 2
        //                      select new
        //                      {
        //                          ti.Id_TipoInsumo,
        //                          ti.NomTipoInsumo
        //                      });

        //    e.Result = tipoInsumo.ToList();
        //    ddlTipoInsumo.Items.Insert(0, new ListItem { Text = "TODOS", Value = string.Empty });
        //}

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
            ddlTipoInsumo.Items.Insert(1, new ListItem("TODOS","TODOS"));
        }

        protected void lnkAgregarInsumo_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["CodProyecto"] = codProyecto;
            HttpContext.Current.Session["Id_Producto"] = codProducto;
            HttpContext.Current.Session["Insumo"] = null;
            HttpContext.Current.Session["NombreProducto"] = lblTitulo.Text;
            Redirect(null, "CatalogoInsumo.aspx?CodProducto=" + codProducto + "&NombreProducto=" + lblTitulo.Text, "_blank", "width=1000,height=800,resizable=yes");
        }

        protected void imgBtnAgregarInsumo_Click(object sender, ImageClickEventArgs e)
        {
            HttpContext.Current.Session["CodProyecto"] = codProyecto;
            HttpContext.Current.Session["Id_Producto"] = codProducto;
            HttpContext.Current.Session["Insumo"] = null;

            Redirect(null, "CatalogoInsumo.aspx", "_blank", "width=1000,height=800,resizable=yes");
        }

        protected void btnAgrgarAProducto_Click(object sender, EventArgs e)
        {
            foreach(GridViewRow gvr in gvrProyectoInsumo.Rows)
            {
                CheckBox checkbox = (CheckBox)gvr.Cells[1].FindControl("chk_insumo");

                if (checkbox!=null)
                {
                    if (checkbox.Checked)
                    {
                        string txtSQL = "INSERT INTO ProyectoProductoInsumo (CodProducto, CodInsumo, Presentacion) " +
                         "SELECT " + codProducto + ", id_insumo, Presentacion " +
                         "FROM ProyectoInsumo WHERE id_insumo in (" + gvrProyectoInsumo.DataKeys[gvr.RowIndex].Value.ToString() + ") and id_insumo not in (SELECT codInsumo FROM ProyectoProductoInsumo WHERE codProducto = " + codProducto + ")";
                        ejecutaReader(txtSQL, 2);
                    }
                }
            }

            Response.Redirect("ProyectoOperacionCompras.aspx");
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
                    HttpContext.Current.Session["CodProyecto"] = codProyecto;
                    HttpContext.Current.Session["Id_Producto"] = codProducto;
                    HttpContext.Current.Session["Insumo"] = e.CommandArgument;

                    Redirect(null, "CatalogoInsumo.aspx", "_blank", "width=1000,height=800,resizable=yes");
                    break;
            }
        }

        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProyectoOperacionCompras.aspx");
        }
    }
}