
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
using System.Text;
using Fonade.Negocio;

namespace Fonade.FONADE.Convocatoria
{
    public partial class CatalogoInsumo : Negocio.Base_Page
    {
        public int proyecto;
        public int producto;
        public int id_insumo;
        public int carguetxt;
        public int tiempo_Proyeccion;
       
        public  string insumotipos;
        public  string nombreinsumo;
        public  string ivainsumo;
        public  string unidadinsumo;
        public  string presentacioninsumo;
        public  string creditoinsumo;
        public  GridViewRow gvrProyeccionVentas;
        public  string unidadmedida;
        public  string prcMsg;
        public string UnidadMedida;
        public string Cantidad;
        public string Desperdicio;
        protected void Page_Load(object sender, EventArgs e)
        {
             if (!IsPostBack)
                {
                    proyecto = Convert.ToInt32(HttpContext.Current.Session["CodProyecto"] ?? 0);
                    lblTitulo.Text += Request.QueryString["NombreProducto"] == null ? "" : Request.QueryString["NombreProducto"].ToString();
                    id_insumo = Request.QueryString["Insumo"] == null ? 0 : Convert.ToInt32(Request.QueryString["Insumo"]);
                    Session["Insumo"] = id_insumo;
                    string IdCountSession = Application["UsersOnline"].ToString();
                    ProyectoInsumoPrecioODS = null;
                    GuardarButton.Attributes.Add("onclick", "javascript:SaveButton()");
                    txt_ivainsumo.Attributes.Add("onkeypress", "javascript:return validarNro(event)");
                    txt_ivainsumo.Attributes.Add("onchange", "javascript:MoneyFormat(this)");
                    //txtCantidad.Attributes.Add("onkeypress", "javascript:return validarNro(event)");
                    //txtCantidad.Attributes.Add("onchange", "javascript:MoneyFormat(this)");
                    txtDesperdicio.Attributes.Add("onkeypress", "javascript:return validarNro(event)");
                    txtDesperdicio.Attributes.Add("onchange", "javascript:MoneyFormat(this)");
                    txt_creditoinsumo.Attributes.Add("onkeypress", "javascript:return validarNro(event)");
                    txt_creditoinsumo.Attributes.Add("onchange", "javascript:MoneyFormat(this)");
                    var varproyectoinsumo = (from p in consultas.Db.ProyectoInsumos
                                             where p.CodProyecto == proyecto
                                             && p.Id_Insumo == id_insumo
                                             select p).FirstOrDefault();
                    if (varproyectoinsumo != null)
                    {
                        var codProducto = int.Parse(Request.QueryString["CodProducto"]);
                        var pjct = consultas.Db.ProyectoProductoInsumos.Where(p => p.CodInsumo == id_insumo && p.CodProducto == codProducto).FirstOrDefault();
                        txt_nombreinsumo.Text = varproyectoinsumo.nomInsumo;
                        ddl_insumotipos.Text = varproyectoinsumo.TipoInsumo.Id_TipoInsumo.ToString();
                        txt_ivainsumo.Text = varproyectoinsumo.IVA.ToString();
                        UnidadMedidaTextBox.Text = varproyectoinsumo.Unidad;
                        txtCantidad.Text = Convert.ToDecimal(pjct.Cantidad.Value).ToString().Replace(",",".");
                        txtDesperdicio.Text = pjct.Desperdicio.ToString();
                        txt_presentacioninsumo.Text = varproyectoinsumo.Presentacion;
                        txt_creditoinsumo.Text = varproyectoinsumo.CompraCredito.ToString();

                    }
                    gvProyeccionVentas.EditIndex = 0;
                    RecorrerGrid();

                 //var tipoInsumo = Session["idTipoInsumo"].ToString(); // == null ? "0" : Session["idTipoInsumo"].ToString(); 
                 //Session["idTipoInsumo"].ToString();
                 if(Session["idTipoInsumo"] != null)
                 {
                     var tipoInsumo = Session["idTipoInsumo"].ToString();
                     if (tipoInsumo != "TODOS" && tipoInsumo != "0")
                     {
                         ddl_insumotipos.SelectedValue = tipoInsumo;
                         Session["idTipoInsumo"] = null;
                     }
                 }
                 
                 if (id_insumo > 0)
                     UnidadesInsumoGrd();
                }
        }
        public DataTable ProyectoInsumoPrecio()
        {
            var dttProyectoInsumoPrecio = new DataTable();
            int CodInsumo = 0;
            var qry = string.Empty;
            qry = string.Format("SELECT [TiempoProyeccion] FROM [dbo].[ProyectoMercadoProyeccionVentas] WHERE [CodProyecto] = {0}",
                Convert.ToInt32(HttpContext.Current.Session["CodProyecto"] ?? 0)); var okm = new Clases.genericQueries().executeQueryReader(qry);
            okm.Read();
            int TiempoProyeccion = okm["TiempoProyeccion"]==null ?0:Convert.ToInt32(okm["TiempoProyeccion"]);
            Session["TiempoProyeccion"] = TiempoProyeccion;
            CodInsumo = Session["Insumo"] == null ? 0 : Convert.ToInt32(Session["Insumo"]);
            string Camposformat = "SELECT CONVERT(varchar, convert(money, isnull([1],0)), 1) [Costo Año1] ,CONVERT(varchar, convert(money, isnull([2],0)), 1) [Costo Año2], CONVERT(varchar, convert(money, isnull([3],0)), 1)  [Costo Año3]," +
                "CONVERT(varchar, convert(money, isnull([4],0)), 1) [Costo Año4], CONVERT(varchar, convert(money, isnull([5],0)), 1)  [Costo Año5],CONVERT(varchar, convert(money, isnull([6],0)), 1) [Costo Año6]," +
                "CONVERT(varchar, convert(money, isnull([6],0)), 1)  [Costo Año7],isnull([8],0) [Costo Año8],CONVERT(varchar, convert(money, isnull([9],0)), 1)  [Costo Año9],CONVERT(varchar, convert(money, isnull([10],0)), 1) [Costo Año10]  ";
            Camposformat += "FROM (SELECT CodInsumo, Periodo, Precio FROM [dbo].[ProyectoInsumoPrecio]) N PIVOT(SUM(Precio) FOR [Periodo] IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10]) ) AS Periodo WHERE CodInsumo = " + CodInsumo;
            dttProyectoInsumoPrecio.Load(new Clases.genericQueries().executeQueryReader(Camposformat));
            var idx = dttProyectoInsumoPrecio.Columns.Count;
            var inx = TiempoProyeccion;
            while (idx > inx)
            {
                --idx;
                dttProyectoInsumoPrecio.Columns.RemoveAt(idx);
            }
            if (dttProyectoInsumoPrecio.Rows.Count == 0)
            {
                var ijn = dttProyectoInsumoPrecio.NewRow();
                for (int k = 0; k < dttProyectoInsumoPrecio.Columns.Count; k++)
                {
                    ijn[k] = 0;
                }
                dttProyectoInsumoPrecio.Rows.Add(ijn);
            }
            return dttProyectoInsumoPrecio;
        }
        protected void lds_cargartxt_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            try
            {
                var ujm = new object[0];
                Array.Resize<object>(ref ujm, tiempo_Proyeccion);
                var lkj = new List<object[]>();
                int i = -1;
                foreach (object vgy in ujm) { lkj[++i] = null; }
                lkj.Add(ujm);
                e.Result = lkj.ToList();
            }
            catch (Exception ex)
            { 
                lblError.Text = ex.Message;
            }
        }
        protected void gvProyeccionVentas_Load(object sender, EventArgs e)
        {
            var query = (from pmpv in consultas.Db.ProyectoMercadoProyeccionVentas
                         where pmpv.CodProyecto == proyecto
                         select new
                         {
                             pmpv.CodPeriodo,
                             pmpv.TiempoProyeccion,
                         }).FirstOrDefault();
            if (query != null)
            {
                gvProyeccionVentas.Columns.Clear();
                
                TemplateField nuevaColumna = new TemplateField();
                nuevaColumna.HeaderText = "Periodo: ";
                gvProyeccionVentas.Columns.Add(nuevaColumna);

                for (int i = 0; i < query.TiempoProyeccion; i++)
                {
                    nuevaColumna = new TemplateField();
                    nuevaColumna.HeaderText = "Año " + (i + 1).ToString();
                    nuevaColumna.ItemTemplate = new labelasigado();
                    gvProyeccionVentas.Columns.Add(nuevaColumna);
                }
                tiempo_Proyeccion = Convert.ToInt32(query.TiempoProyeccion);
            }
            if(!IsPostBack)
            llenarcolumnas();
        }

        protected void llenarcolumnas()
        {
            for (int i = 1; i <= tiempo_Proyeccion; i++)
            {
                foreach (GridViewRow grd_Row in this.gvProyeccionVentas.Rows)
                {
                    //try
                    //{
                    string nombre2 = "txt_valor" + i.ToString();
                    TextBox tx1 = new TextBox();
                    tx1.ID = nombre2;
                    //tx1.Text = "";
                    tx1.Width = 70;

                    if (!IsPostBack)
                    {
                        #region Diego Quiñonez - 22 de Diciembre de 2014

                        var varProyectoInsumoPrecio = (from pip in consultas.Db.ProyectoInsumoPrecios
                                                       where pip.CodInsumo == id_insumo
                                                       && pip.Periodo == i
                                                       select pip).FirstOrDefault();

                        if (varProyectoInsumoPrecio != null)
                        {
                            tx1.Text = varProyectoInsumoPrecio.Precio.ToString();
                        }
                        else
                        {
                            tx1.Text = string.Empty;
                        }

                        #endregion
                    }
                    grd_Row.Cells[i].Controls.Add(tx1);

                    //}
                    //catch (Exception)
                    //{
                    //}
                }
            }
        }
       private void InsertProyectoProductoInsumo(int producto, int idInsumo, string unidad, int cant, int desperdicio)
        {
            CatalogoInsumoNegocio catInsumoNeg = new CatalogoInsumoNegocio();
            try
            {
                int reg = catInsumoNeg.InsertProyectoProductoInsumo(producto, idInsumo, unidad, cant, desperdicio);

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        /// <summary>
        /// Diego Quiñonez
        /// 22 de Diciembre de 2014
        /// actualiza el insumo actual
        /// en operacion de compras
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public string CadenaMensajeError(string MensajeError)
        {
            MensajeError = "<script type='text/javascript'>function window_onload() { CerrarVentana() }</script>";
            return MensajeError;
        }
        protected void InsertUpdateDelete(ref int CodInsumo, int c_proyecto, int c_tipo, string nom_insumo, string iva, string unidad, string presentacion, string credito, string Cantidad, string Desperdicio, int Producto, string caso)
        {
           SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand("MD_InsertUpdateDeleteProyectoInsumoPrecio", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CodInsumo", CodInsumo);
            cmd.Parameters.AddWithValue("@codProyecto", c_proyecto);
            cmd.Parameters.AddWithValue("@Tipo", c_tipo);
            cmd.Parameters.AddWithValue("@NomInsumo", nom_insumo);
            cmd.Parameters.AddWithValue("@IVA", iva);
            cmd.Parameters.AddWithValue("@Unidad", unidad);
            cmd.Parameters.AddWithValue("@Presentacion", presentacion);
            cmd.Parameters.AddWithValue("@Credito", credito);
            cmd.Parameters.AddWithValue("@caso", caso);
            cmd.Parameters.AddWithValue("@Cantidad", Cantidad);
            cmd.Parameters.AddWithValue("@Desperdicio", Desperdicio);
            cmd.Parameters.AddWithValue("@CodProducto", Producto);
            try
            {
              con.Open();
              CodInsumo= (int)cmd.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                CodInsumo = -1;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
          
        }
           /// <summary>
        /// Diego Quiñonez
        /// 19 de Diciembre de 2014
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lds_tipoInsumos_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var result = (from ti in consultas.Db.TipoInsumos
                          select new
                          {
                              ti.Id_TipoInsumo,
                              ti.NomTipoInsumo
                          });


            e.Result = result.ToList();
        }

        public Converter<object, int> initVal { get; set; }

        private double inputConverter(DataControlFieldCell input)
        {
            var wsx = !Equals(input.ContainingField.ToString(), "CodInsumo")?((TextBox)input.Controls[0]).Text ?? "0":"-1";
            if (wsx == null || wsx == "")
            {
                wsx = "0";
            }
           
            return Convert.ToDouble(wsx);
        }
        private void RecorrerGrid()
        {
            int TiempoProyeccion = Session["TiempoProyeccion"] == null ? 0 : Convert.ToInt32(Session["TiempoProyeccion"]);
            int TotalGridColumns = gvProyeccionVentas.Columns.Count;
            int index = 0;
             foreach (GridViewRow rowGrid in gvProyeccionVentas.Rows.Cast<GridViewRow>().ToList())
            {
                foreach (DataControlFieldCell colGrid in rowGrid.Cells.OfType<DataControlFieldCell>().ToList())
                {
                    if (Equals(colGrid.Controls[0].GetType().Name, "TextBox"))
                    {
                        
                            ((TextBox)colGrid.Controls[0]).Attributes.Add("onkeypress", "javascript:return validarNro(event)");
                            ((TextBox)colGrid.Controls[0]).Attributes.Add("onchange", "javascript:MoneyFormat(this)");
                           ((TextBox)colGrid.Controls[0]).MaxLength = 12;
                        
                    }
                }
                index++;
            }
        }

        protected void GuardarButton_Click(object sender, EventArgs e){
            Desperdicio = (txtDesperdicio.Text == "" ? "0" : txtDesperdicio.Text).Replace(",", "").Split('.')[0];
            UnidadMedida = (UnidadMedidaTextBox.Text == "" ? "" : UnidadMedidaTextBox.Text);
            Cantidad = txtCantidad.Text.Replace(",",".");
            gvrProyeccionVentas = gvProyeccionVentas.Rows[0];
            insumotipos = ddl_insumotipos.SelectedValue;
            nombreinsumo = txt_nombreinsumo.Text;
            ivainsumo = txt_ivainsumo.Text.Replace(",", ""); 
            presentacioninsumo = (txt_presentacioninsumo.Text == "" ? "0" : txt_presentacioninsumo.Text);
            creditoinsumo =(txt_creditoinsumo.Text==""?"0":txt_creditoinsumo.Text).Replace(",", "");
            int CodInsumo = Request.QueryString["Insumo"] == null ? 0 : Convert.ToInt32(Request.QueryString["Insumo"]);
            proyecto = Convert.ToInt32(HttpContext.Current.Session["CodProyecto"] ?? 0);
            producto = (Session["CodProducto"] != null) ? int.Parse(Session["CodProducto"].ToString()) : 0;
            producto = (producto > 0) ? producto : Convert.ToInt32(Request.QueryString["CodProducto"].ToString());
            //producto = int.Parse(Session["CodProducto"].ToString());
            //producto = Convert.ToInt32(Request.QueryString["CodProducto"].ToString());
            string query = CodInsumo==0? "Create": "Update";
            string OpenerPage = string.Empty;
            OpenerPage = "<script type='text/javascript'> ";
            if (CodInsumo == 0)
            {
                 query = "Create";
            }
            else 
            {
                OpenerPage += "opener.location.reload();";
                query = "Update";
            }
            if (txt_nombreinsumo.Text =="" || txtCantidad.Text == "" || txt_ivainsumo.Text == "")
            {
                OpenerPage += "alert('Los campos Nombre de insumo, Iva y Cantidad son requeridos')";
            }
            else
            {
                InsertUpdateDelete(ref CodInsumo, proyecto, Convert.ToInt32(insumotipos), nombreinsumo,
                                   ivainsumo, UnidadMedida, presentacioninsumo,
                                   creditoinsumo, Cantidad, Desperdicio, producto, query);
                if (CodInsumo == -1)
                {
                    prcMsg = "No fue posible guardar la información referente al insumo"; return;
                }
                else
                {
                    string SqlQuery = "";
                    int Periodo = 1;
                    foreach (DataControlFieldCell colGrid in gvrProyeccionVentas.Cells.OfType<DataControlFieldCell>().ToList())
                    {
                        string dato = ((TextBox)colGrid.Controls[0]).Text;
                        SqlQuery = "InsertUpdateInsumoPrecio " + CodInsumo + ',' + Periodo.ToString() + ',' + dato.Replace(",", "");
                        InsertUpdatePrecioInsumo(SqlQuery);
                        Periodo++;
                    }
                    prcMsg = "Los datos se guardaron de manera exitosa";
                    UpdateProyectoProductoInsumo(producto, CodInsumo, Cantidad, presentacioninsumo, Desperdicio);
                }
                OpenerPage += "alert('" + prcMsg + "'); window.opener.document.getElementById('hidInsumo').value='1';window.close();";
            }
              OpenerPage += " </script>";
            ClientScriptManager cm = this.ClientScript;
            cm.RegisterClientScriptBlock(this.GetType(), "", OpenerPage);
        }
        private void UnidadesInsumoGrd()
        {
            int TiempoProyeccion = Convert.ToInt32(Session["TiempoProyeccion"]);
           int id_insumo = Request.QueryString["Insumo"] == null ? 0 : Convert.ToInt32(Request.QueryString["Insumo"]);
           DataTable UnidadesInsumo = GetDataTable("GetPrecioUnidadesInsumo " + id_insumo);
           grdUnidadesInsumo.DataSource = UnidadesInsumo;
           grdUnidadesInsumo.DataBind();
         
            for (int IndexCol = TiempoProyeccion + 1; IndexCol < UnidadesInsumo.Columns.Count-1; IndexCol++)
           {
                    grdUnidadesInsumo.Columns[IndexCol].Visible = false;
           }
          
        }
        protected void InsertUpdatePrecioInsumo(string QueryData)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand(QueryData, con);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                string Error = ex.Message;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

        protected DataTable GetDataTable(string QueryData)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlDataAdapter cmd = new SqlDataAdapter(QueryData, con);
            DataTable dtInsumos= new DataTable();
            try
            {
                con.Open();
                cmd.Fill(dtInsumos);
            }
            catch (SqlException ex)
            {
                string Error = ex.Message;
            }
            finally
            {
                con.Close();
                con.Dispose();
                }
            return dtInsumos;
        }
    
        protected void UpdateProyectoProductoInsumo(int CodProducto, int CodInsumo,string Cantidad, string presentacioninsumo, string Desperdicio)
        {
            var qry = "UPDATE [dbo].[ProyectoProductoInsumo] SET Presentacion = '" + presentacioninsumo + "' ,Cantidad ='" + Cantidad + "', " +
                                    "Desperdicio='" + Desperdicio + "' WHERE [CodProducto] = '" + CodProducto + "' AND CodInsumo ='" + CodInsumo + "'";
            new Clases.genericQueries().executeQueryReader(qry, 2);
        }
    }
       class labelasigado : ITemplate
    {
        public void InstantiateIn(System.Web.UI.Control container)
        {
            //TextBox AAsignar = new TextBox();
            //AAsignar.ID = "l_asignar";
            //AAsignar.Width = 70;
            //container.Controls.Add(AAsignar);
        }
    }


}