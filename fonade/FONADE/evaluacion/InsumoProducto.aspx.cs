using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.evaluacion
{
    public partial class InsumoProducto : Negocio.Base_Page
    {
        /// <summary>
        /// Diego Quiñonez, 19 de Enero de 2015
        /// </summary>
        int id_producto
        {
            get
            {
                return ViewState["id_producto"] != null && !string.IsNullOrEmpty(ViewState["id_producto"].ToString()) ? int.Parse(ViewState["id_producto"].ToString()) : 0;
            }
            set { ViewState["id_producto"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            #region codigo anterior
            //            String valor = Request["Aporte"];
//            String sql;
//            sql = @"SELECT [NomProducto], [NomTipoInsumo], [nomInsumo]
//                  FROM [ProyectoProducto], [ProyectoInsumo], [ProyectoProductoInsumo], [TipoInsumo]
//                  WHERE [Id_Producto] = [CodProducto] AND [Id_Insumo] = [CodInsumo] AND [Id_TipoInsumo] = [codTipoInsumo] AND [Id_Producto] = " + valor;

//            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
//            SqlCommand cmd = new SqlCommand(sql, conn);

//            try
//            {
//                conn.Open();
//                SqlDataReader reader = cmd.ExecuteReader();

//                if(reader.Read())
//                {
//                    NomProducto.Text = reader["NomProducto"].ToString();
//                    NomTipoInsumo.Text = reader["NomTipoInsumo"].ToString();
//                    nomInsumo.Text = reader["nomInsumo"].ToString();
//                }
//                reader.Close();
//            }
//            catch (SqlException se)
//            {
//            }
//            finally
//            {
//                conn.Close();
            //            }
            #endregion

            #region Diego Quiñonez - 19 de Enero de 2015
            id_producto = HttpContext.Current.Session["CodProducto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProducto"].ToString()) ? int.Parse(HttpContext.Current.Session["CodProducto"].ToString()) : 0;

            var idProd = 0;
            if (Session["id_producto"] != null)
            {
                idProd = int.Parse(Session["id_producto"].ToString());
            }

            lblNomProducto.Text = (from pp in consultas.Db.ProyectoProductos
                                   where pp.Id_Producto == idProd
                                       select pp.NomProducto).First();
            #endregion
        }

        #region Diego Quiñonez - 19 de Enero de 2015
        protected void ldsInsumos_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            id_producto = int.Parse(Session["id_producto"].ToString());

            var result = (from pp in consultas.Db.ProyectoProductos
                          from pi in consultas.Db.ProyectoInsumos
                          from ppi in consultas.Db.ProyectoProductoInsumos
                          from ti in consultas.Db.TipoInsumos
                          orderby ti.Id_TipoInsumo
                          where pp.Id_Producto == ppi.CodProducto
                          && pi.Id_Insumo == ppi.CodInsumo
                          && pi.codTipoInsumo == ti.Id_TipoInsumo
                          && pp.Id_Producto == id_producto
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

            if(gv!=null)
            {
                int idTipoInsumo = int.Parse(gvInsumos.DataKeys[e.Row.RowIndex].Value.ToString());

                var result = (from pi in consultas.Db.ProyectoInsumos
                              from ppi in consultas.Db.ProyectoProductoInsumos
                              from ti in consultas.Db.TipoInsumos
                              orderby ti.Id_TipoInsumo
                              where ppi.CodProducto == id_producto
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
        #endregion
    }
}