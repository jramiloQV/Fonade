using Datos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Fonade.PlanDeNegocioV2.Evaluacion.PlanOperativo
{
    public partial class Insumo : Negocio.Base_Page
    {
        int IdProducto
        {
            get { return Convert.ToInt32(Request.QueryString["codproducto"]); }
            set { }
        }
                
        public int CodigoTab { get { return Constantes.Const_ProduccionV2; } set { } }


        protected void Page_Load(object sender, EventArgs e)
        {                                  
            lblNomProducto.Text = (from pp in consultas.Db.ProyectoProductos
                                   where pp.Id_Producto == IdProducto
                                   select pp.NomProducto).First();
        }

        protected void ldsInsumos_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {            
            var result = (from pp in consultas.Db.ProyectoProductos
                          from pi in consultas.Db.ProyectoInsumos
                          from ppi in consultas.Db.ProyectoProductoInsumos
                          from ti in consultas.Db.TipoInsumos
                          orderby ti.Id_TipoInsumo
                          where pp.Id_Producto == ppi.CodProducto
                          && pi.Id_Insumo == ppi.CodInsumo
                          && pi.codTipoInsumo == ti.Id_TipoInsumo
                          && pp.Id_Producto == IdProducto
                          select new
                          {
                              ti.Id_TipoInsumo,
                              ti.NomTipoInsumo
                          }).Distinct();

            e.Result = result.ToList();
        }

        protected void gvInsumos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView gv = (GridView)e.Row.Cells[0].FindControl("gvProyectoInsumo");

            if (gv != null)
            {
                int idTipoInsumo = int.Parse(gvInsumos.DataKeys[e.Row.RowIndex].Value.ToString());

                var result = (from pi in consultas.Db.ProyectoInsumos
                              from ppi in consultas.Db.ProyectoProductoInsumos
                              from ti in consultas.Db.TipoInsumos
                              orderby ti.Id_TipoInsumo
                              where ppi.CodProducto == IdProducto
                              && pi.Id_Insumo == ppi.CodInsumo
                              && pi.codTipoInsumo == idTipoInsumo
                              select new
                              {
                                  pi.nomInsumo
                              }).Distinct();

                gv.DataSource = result.ToList();
                gv.DataBind();
            }
        }
                
    }
}