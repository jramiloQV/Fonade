using Datos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Fonade.FONADE.evaluacion
    {

    /// <summary>
    /// clase que actualiza la entidad catalogoProducto
    /// </summary>
    public partial class CatalogoProducto : System.Web.UI.Page
        {
        public int txtTab = Constantes.CONST_ProyeccionesVentas;
        public String codConvocatoria;
       
        [ContextStatic]
        protected static int idx;
        [ContextStatic]
        protected static int idx_;
        [ContextStatic]
        protected static GridViewRow gvrItem;
        [ContextStatic]
        protected static DataTable _dttProductos;
        [ContextStatic]
        protected static DataTable _dttRegistro;
        [ContextStatic]
        protected static bool editar;
        public int ndx = 1;
        [ContextStatic]
        public int ndx_;
        [ContextStatic]
        protected static string caption;
        [ContextStatic]
        protected static int lkj = 0;

        [ContextStatic]
        protected static bool ColVisible=false;
        /// <summary>
        /// carga  datos con variables de session
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
            {
                if (!Page.IsPostBack)
                {
                    int CodProyecto = Session["codProyecto"] == null ? 0 : Convert.ToInt32(Session["codProyecto"]);
                     if (HttpContext.Current.Session["codConvocatoria"] != null)
                        codConvocatoria = HttpContext.Current.Session["codConvocatoria"].ToString();
                    if (Request.QueryString["codConvocatoria"] != null)
                        codConvocatoria = Session["codConvocatoria"].ToString();
                    try
                    {
                        
                        if ("actualizar".Equals(HttpContext.Current.Session["OpcionMercadoProyecciones"].ToString()))
                        {
                         int Id_Producto=   Request.QueryString["Id_Producto"] == null ? 0 : Convert.ToInt32(Request.QueryString["Id_Producto"]);
                         Session["valordeId_Producto"] = Id_Producto;
                            llenarCampos(Id_Producto);
                        }
                    }
                    catch (Exception) { }
                    var sqlTiempoProyeccion = string.Format("SELECT [TiempoProyeccion] FROM [dbo].[ProyectoMercadoProyeccionVentas] WHERE [CodProyecto] = {0}",
                    Convert.ToInt32(HttpContext.Current.Session["codProyecto"] ?? 0));
                    HttpContext.Current.Session["TiempoProyeccion"] = GetValue(sqlTiempoProyeccion);
                  }
                else
                {
                    DatosPosback();
                } 
                ndx_ = Convert.ToInt32(HttpContext.Current.Session["TiempoProyeccion"]);
                RecorrerGrid(Convert.ToInt32(HttpContext.Current.Session["TiempoProyeccion"]));
                TotalYear.Value = HttpContext.Current.Session["TiempoProyeccion"].ToString();
             
         }
        private void RecorrerGrid(int TiempoProyeccion)
        {
           int TotalGridColumns = GridView1.Columns.Count;
           int index = 0;
           //HttpContext.Current.Session["valordeId_Producto"] = CodProducto;
           foreach (GridViewRow rowGrid in GridView1.Rows.Cast<GridViewRow>().ToList())
                {
                    foreach (DataControlFieldCell colGrid in rowGrid.Cells.OfType<DataControlFieldCell>().ToList())
                    {
                        int indexCol = 0;
        
                        if (Equals(colGrid.Controls[0].GetType().Name, "TextBox"))
                        {
                            if (index < 13 )
                            {
                                ((TextBox)colGrid.Controls[0]).Attributes.Add("onkeypress", "javascript:return validarNro(event)");
                                ((TextBox)colGrid.Controls[0]).Attributes.Add("onchange", "ClientValidate(this)");
                            }
                                
                                if (index == 12)
                                {
                                    ((TextBox)colGrid.Controls[0]).ForeColor = System.Drawing.Color.White;
                                    ((TextBox)colGrid.Controls[0]).BackColor = System.Drawing.ColorTranslator.FromHtml("#2980B9");
                               
                                } 
                                if (index == 13)
                                {
                                    ((TextBox)colGrid.Controls[0]).Enabled = false;
                                }
                        }
                        indexCol++;
                 }
                index++;
                }
           for (int IndexCol = TiempoProyeccion + 1; IndexCol < TotalGridColumns; IndexCol++)
           {
               if (GridView1.HeaderRow.Cells[IndexCol]!=null)
                     GridView1.HeaderRow.Cells[IndexCol].Visible = false;
              }
           var filas = GridView1.Rows.Count;

           GridView1.Rows[filas - 1].Cells[0].BackColor = System.Drawing.ColorTranslator.FromHtml("#2980B9");
           GridView1.Rows[filas - 1].Cells[0].ForeColor = System.Drawing.Color.White;
        }
        protected int GetValue(string QueryData)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand(QueryData, con);
            int Valor = 0;
            try
            {
                con.Open();
                Valor = Convert.ToInt32(cmd.ExecuteScalar());
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
            return Valor;
        }
        /// <summary>
        /// llena campos
        /// </summary>
        /// <param name="Id_Producto"></param>
        private void llenarCampos(int Id_Producto)
            {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT  Id_Producto CodProyecto,NomProducto, CONVERT(varchar, convert(money, isnull(PorcentajeIva,0)), 1)PorcentajeIva,");
            sql.Append(" CONVERT(varchar, convert(money, isnull(PorcentajeRetencion,0)), 1)PorcentajeRetencion, CONVERT(varchar, convert(money, isnull(PorcentajeVentasPlazo,0)), 1)");
            sql.Append(" PorcentajeVentasPlazo,p.PosicionArancelaria, CONVERT(varchar, convert(money, isnull(PrecioLanzamiento ,0)), 1)PrecioLanzamiento , A.[Descripcion]FROM [ProyectoProducto] AS P ");
            sql.Append(" LEFT JOIN [PosicionArancelaria] AS A ON A.PosicionArancelaria = P.PosicionArancelaria ");
            sql.Append(" WHERE [Id_Producto] = " + Id_Producto);

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(sql.ToString(), conn);

            try
                {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                reader.Read();
                NombreProductoServicio.Value= reader["NomProducto"].ToString();
                cb_identifier.Value = reader["PosicionArancelaria"].ToString() + " " + reader["Descripcion"].ToString();
                PrecioLanzamiento.Value = reader["PrecioLanzamiento"].ToString();
                Iva.Value = reader["PorcentajeIva"].ToString();
                Retencion.Value = reader["PorcentajeRetencion"].ToString();
                VentasCredito.Value = reader["PorcentajeVentasPlazo"].ToString();
                }
            catch (SqlException ex)
                {
                string lblError= ex.Message;
                }
            finally
                {
                     conn.Close();
                     conn.Dispose();
                }
            }
        /// <summary>
        /// actualiza o crea el producto ventas
        /// </summary>
        /// <param name="TotalYears"></param>
        /// <param name="CodProyecto"></param>
        /// <param name="msgResultado"></param>
        /// <param name="CodProducto"></param>
        private void CrearActualizarProductoVentas(int TotalYears, int CodProyecto, ref string msgResultado, ref int CodProducto)
            {
            string[] ArrArancelaria = cb_identifier.Value.Split(' ');
            String nombreproducto = NombreProductoServicio.Value;
            String posicionArancelarCodigo = cb_identifier.Value.Split(' ')[0];
            String posicionArancelarDescripcion =ArrArancelaria.Length>1? ArrArancelaria[1]:"";
            String precioLanzamiento = PrecioLanzamiento.Value.Replace(",","");
            String iva = Iva.Value;
            String retencioFuente = Retencion.Value;
            String VentaCredito = VentasCredito.Value==""?"0":VentasCredito.Value;
             try
            {
                bool Estado = false;
                InsertarProducto(ref Estado, ref  CodProducto, nombreproducto, iva, retencioFuente, VentaCredito, posicionArancelarCodigo, precioLanzamiento, CodProyecto);
                if (!Estado && "agregar".Equals(HttpContext.Current.Session["OpcionMercadoProyecciones"].ToString()))
                {
                    msgResultado = "Ya existe un Producto con ese Nombre";
                }
                else
                {
                    InsertarProductosVentas(CodProducto, TotalYears);
                    msgResultado = "Los datos del producto se guardaron de manera exitosa";
            
                }
            }
            catch (SqlException ex)
            {
                string lblError= ex.Message;
            }
         }
        private void DatosPosback()
        {
            if (hidCombo.Value == "1" && cb_identifier.Value != "")
            {
                StringBuilder ListaElementos = new StringBuilder();
                string Sql = "GetPosicionArancelaria " +cb_identifier.Value;
                DataTable Aranceles = GetListData(Sql);
                foreach (DataRow row in  Aranceles.Rows)
                {
                   string[] arrFormat =row["Descripcion"].ToString().Split(' ');
                   string TextoParrafo = "";
                   int TotalLines = 1;
                   int Caracteres = 130;
                   int height = (row["Descripcion"].ToString().Length / Caracteres + 1) * 15;
                   ListaElementos.Append("<a style='width:98%;height:" + height + "px;'>" + row["Descripcion"].ToString() + "</a>");
                }
                cmbArancelaria.InnerHtml = ListaElementos.ToString();
                cmbArancelaria.Style.Add("display", "block");
            }
            if (hidCombo.Value == "0")
            {
                GuardarDatosDB();
            }
        }
       private void InsertarProductosVentas(int CodProducto , int TotalYears  )
        {
            string Cadena = string.Empty;
            var qry = string.Empty;
            qry = "SELECT count(1) FROM [dbo].[ProyectoProductoUnidadesVentas] WHERE CodProducto =" + CodProducto;
            int Productosventas = GetValue(qry);// new Clases.genericQueries().executeQueryReader(qry);
            int Totalmes = 12;
            if(Productosventas==0)
            for (int i = 1; i <= Totalmes; i++)
            {
                for (int j = 1; j <= TotalYears; j++)
                {
                    Cadena = "INSERT INTO ProyectoProductoUnidadesVentas(CodProducto,Unidades,Mes,ano)values(" + CodProducto + ",0," + i + "," + j + ")";
                    InsertUpdateProductos(Cadena);
                }
            }
       }

        /// <summary>
        /// inserta producto
        /// </summary>
        /// <param name="EstCodigo"></param>
        /// <param name="Id_Producto"></param>
        /// <param name="nombreproducto"></param>
        /// <param name="iva"></param>
        /// <param name="retencioFuente"></param>
        /// <param name="VentaCredito"></param>
        /// <param name="posicionArancelarCodigo"></param>
        /// <param name="precioLanzamiento"></param>
        /// <param name="codProyecto"></param>
        private void InsertarProducto(ref bool EstCodigo,ref int Id_Producto,String nombreproducto, String iva, String retencioFuente, string VentaCredito,String posicionArancelarCodigo, string precioLanzamiento, int codProyecto)
        {
            string conexionStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            using (var con = new SqlConnection(conexionStr))
            {
                using (var com = con.CreateCommand())
                {
                    com.CommandText = "MD_Insertar_Actualizar_ProductosVentas";
                    com.CommandType = System.Data.CommandType.StoredProcedure;

                    com.Parameters.AddWithValue("@_Id_Producto", Id_Producto);
                    com.Parameters.AddWithValue("@_CodProyecto", codProyecto);
                    com.Parameters.AddWithValue("@_NomProducto", nombreproducto);
                    com.Parameters.AddWithValue("@_PorcentajeIva", iva);
                    com.Parameters.AddWithValue("@_PorcentajeRetencion", retencioFuente);
                    com.Parameters.AddWithValue("@_PorcentajeVentasPlazo", VentaCredito);
                    com.Parameters.AddWithValue("@_PosicionArancelaria", posicionArancelarCodigo);
                    com.Parameters.AddWithValue("@_PrecioLanzamiento", precioLanzamiento);
                    con.Open();
                   string  EstAccion = com.ExecuteScalar().ToString();
                   EstCodigo = Convert.ToBoolean(EstAccion.Split('-')[0]);
                   Id_Producto = Convert.ToInt32(EstAccion.Split('-')[1]);
                  
                    com.Dispose();
                    con.Close();
                    con.Dispose();
                }
            }
        }
        public System.Data.DataTable RegistroProducto(){
            string codProducto = HttpContext.Current.Session["valordeId_Producto"] == null ? "0" : HttpContext.Current.Session["valordeId_Producto"].ToString();
            _dttProductos = GetListData("GetDataPeriodosProductos " + codProducto);
            foreach(DataRow f in _dttProductos.Rows)
            {
                if(f["Periodos"].ToString().Contains("Ventas Esperadas13"))
                {
                    f["Periodos"] = "Ventas Esperadas";
                }
            }
             return _dttProductos;
            }
        protected DataTable GetListData(string QueryData)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlDataAdapter cmdDA = new SqlDataAdapter(QueryData, con);
            DataTable dt = new DataTable();
            try
            {
                cmdDA.Fill(dt);
            }
            catch (SqlException ex)
            {
                string Error = ex.Message;
            }
            return dt;
        }
        protected void LB_Buscar_Click(object sender, EventArgs e)
            {

            }
        private void UpdateGrid()
        {
            int IndexRow = 0;
            int IndexMes = 0;
            string UnidValue = "";
            int CodProducto = 0;
            string msgResultado = string.Empty;
            int CodProyecto= HttpContext.Current.Session["codProyecto"]==null?0: Convert.ToInt32(HttpContext.Current.Session["codProyecto"]);
            int TiempoProyeccion = HttpContext.Current.Session["TiempoProyeccion"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["TiempoProyeccion"]);
            CodProducto = Request.QueryString["Id_Producto"] == null ? 0 : Convert.ToInt32(Request.QueryString["Id_Producto"]);
            CrearActualizarProductoVentas(TiempoProyeccion, CodProyecto, ref msgResultado, ref CodProducto);
            if (CodProducto > 0)
             {
                 int TotalColl = _dttProductos.Columns.Count - 2;
                 foreach (GridViewRow GR in GridView1.Rows)
                 {
                     string QueryUpdate = "";
                     if (IndexRow < 12)
                     {
                         for (int IndexCol = 1; IndexCol <= TiempoProyeccion; IndexCol++)
                         {
                             string FieldAnio = _dttProductos.Columns[IndexCol].ColumnName;
                             FieldAnio = FieldAnio.Substring(3, 2).TrimStart();
                             string NomTextBox = "TextBox" + FieldAnio;
                             if (GR.RowType == DataControlRowType.DataRow)
                             {
                                 string FieldMes = _dttProductos.Columns[IndexCol].ColumnName;
                                 IndexMes = Convert.ToInt32(_dttProductos.Rows[IndexRow]["Periodos_"]);
                                 TextBox TextBoxValue = (TextBox)GR.FindControl(NomTextBox);
                                 TextBoxValue.Text = (TextBoxValue.Text == "" ? "0" : TextBoxValue.Text);
                                 UnidValue = TextBoxValue.Text.Replace(",", "");
                             }
                             QueryUpdate += "UPDATE ProyectoProductoUnidadesVentas SET Unidades =" + UnidValue + " WHERE Codproducto=" + CodProducto + " and  Mes=" + IndexMes + " and Ano=" + FieldAnio + "\n";
                         }
                         IndexRow++;
                         InsertUpdateProductos(QueryUpdate);
                     }
                 }
                 precios(CodProducto.ToString(), TiempoProyeccion);
             }
            ClientScriptManager cm = this.ClientScript;
            cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'> alert('" + msgResultado + "'); location.href = '../Proyecto/PProyectoMercadoProyecciones.aspx' </script>");
        }

        /// <summary>
        /// actualiza precios
        /// </summary>
        /// <param name="CodProducto"></param>
        /// <param name="TiempoProyeccion"></param>
        private void precios(string CodProducto, int TiempoProyeccion)
        {
            string UnidValue = "";
            int TotalColl = _dttProductos.Columns.Count - 2;
            int totalRows = GridView1.Rows.Count;
            GridViewRow GR = GridView1.Rows[totalRows - 2];
            string QueryUpdate = "";

            for (int IndexCol = 1; IndexCol <= TiempoProyeccion; IndexCol++)
                {
                    string FieldAnio = _dttProductos.Columns[IndexCol].ColumnName;
                    FieldAnio = FieldAnio.Substring(3, 2).TrimStart();
                    string NomTextBox = "TextBox" + FieldAnio;
                    if (GR.RowType == DataControlRowType.DataRow)
                    {
                        string FieldMes = _dttProductos.Columns[IndexCol].ColumnName;
                        TextBox TextBoxValue = (TextBox)GR.FindControl(NomTextBox);
                        TextBoxValue.Text = (TextBoxValue.Text == "" ? "0" : TextBoxValue.Text);
                        UnidValue = TextBoxValue.Text.Replace(",", "");
                    }
                    QueryUpdate = "InsertUpdateprecio " + CodProducto + ",'" + UnidValue + "'," + FieldAnio;
                    InsertUpdateProductos(QueryUpdate);
                }
         }
        protected void InsertUpdateProductos(string QueryData)
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

        protected void GuardarDatosDB()
         {
           if(hidCombo.Value == "0"  )
             UpdateGrid();
         }
      
        private double nvrt(string ygv)
            {
            double qwe = new double();
            string kjh = string.Empty;
            try
                {
                double.TryParse(ygv, out qwe);
                if (qwe == 0 && !Equals(ygv, "0"))
                    {
                    var edc = string.Empty;
                    foreach (char ujm in ygv.ToCharArray())
                        {
                        edc += ujm.ToString();
                        qwe = double.Parse(edc);
                        }
                    }
                }
            catch (Exception xcp)
                {
                var tyu = ygv.ToCharArray();
                for (int i = 0; i < tyu.Length; i++)
                    {
                    if (char.IsDigit(ygv.ToCharArray()[i]))
                        {
                        kjh += ygv.ToCharArray()[i].ToString();
                        }
                    }
                var ytr = Convert.ToInt32(kjh);
                qwe = Convert.ToDouble(ytr);
                }
            finally
                {
                if (qwe == 0 && !Equals(ygv, "0"))
                    {
                    qwe = Convert.ToDouble(ygv);
                    }
                }
            return qwe;
            }

       

        }
    }