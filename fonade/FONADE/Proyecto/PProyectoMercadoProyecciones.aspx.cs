using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using Fonade.Negocio;
using Fonade.Negocio.Proyecto;
using Fonade.Negocio.Entidades;
using System.Web;
using System.Globalization;
using Fonade.Clases;

namespace Fonade.FONADE.Proyecto
{
    public partial class PProyectoMercadoProyecciones : Negocio.Base_Page
    {
        public int CodigoProyecto { get; set; }
        public int CodigoTab
        {
            get
            {
                return Constantes.CONST_ProyeccionesVentas;
            }
            set { }
        }
        public int CodigoConvocatoria { get; set; }
        public Boolean EsMiembro { get; set; }
        public Boolean EsRealizado { get; set; }
        public Boolean PostitVisible
        {
            get
            {
                return EsMiembro && !EsRealizado;
            }
            set { }
        }
        public Boolean AllowUpdate
        {
            get
            {
                return EsMiembro && !EsRealizado && usuario.CodGrupo.Equals(Constantes.CONST_Emprendedor);
            }
            set { }
        }
        public Boolean AllowCheckTab { get; set; }

        String txtCodPeriodo;
        Int32 txtTiempoProyeccion;
        String txtMetodoProyeccion;
        String txtCostoVenta;

        String txtSQL;
        Boolean bTiempo;


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["CodProyecto"] == null)
                    throw new ApplicationException("No se pudo obtener el codigo del proyecto, intentenlo de nuevo.");

                CodigoProyecto = Convert.ToInt32(HttpContext.Current.Session["CodProyecto"]);
                CodigoConvocatoria = Session["CodConvocatoria"] != null ? Convert.ToInt32(Session["CodConvocatoria"]) : 0;

                EsMiembro = ProyectoGeneral.EsMienbroDelProyecto(CodigoProyecto, usuario.IdContacto);
                EsRealizado = ProyectoGeneral.VerificarTabSiEsRealizado(CodigoTab, CodigoProyecto);
                AllowCheckTab = ProyectoGeneral.AllowCheckTab(usuario.CodGrupo, CodigoProyecto, CodigoTab, EsMiembro);

                if (!IsPostBack)
                {
                    CargarPeriodos();

                    CargarProyeccionesDeVentas();
                    GV_productoServicio.Columns[0].Visible = AllowUpdate;
                    GV_productoServicio.Columns[9].Visible = AllowUpdate;

                    var entity = getProyectoMercadoProyeccionVenta(CodigoProyecto);
                    inicioEncabezado(CodigoProyecto.ToString(), CodigoConvocatoria.ToString(), CodigoTab);
                    SetDatos(entity);

                    llenarGridView();
                    Tabla_VentasUnidades();
                    Tabla_IngresosVenta();

                    ProyectoGeneral.GetUltimaActualizacion(lblUltimaActualizacion, lblFechaUltimaActualizacion, chkEsRealizado, btnUpdateTab, CodigoTab, CodigoProyecto);
                }
                
            }
            catch (ApplicationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Advertencia, detalle : " + ex.Message + "');", true);

                if (Session["CodProyecto"] == null)
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "refreshParent", "window.top.location.reload();", true);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }

        }

        private void SetDatos(ProyectoMercadoProyeccionVenta entity)
        {
            if (entity != null)
            {
                TB_JusProVen.Text = entity.justificacion.htmlDecode();
                TB_PoliCarte.Text = entity.PoliticaCartera.htmlDecode();
            }
        }

        private Boolean Validar()
        {
            Boolean resultado = true;


            if (txtTiempoProyeccion.ToString() != DropDownList1.SelectedValue || txtCodPeriodo != DD_Periodo.SelectedValue)
            {
                var resul = System.Windows.Forms.MessageBox.Show("Si cambia el periodo de proyección y/o el tamaño del periodo se borraran las proyecciones de ventas para los productos actuales.  Esta seguro de realizar este cambio?", "Advrtencia", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);
                if (resul == System.Windows.Forms.DialogResult.No)
                {
                    resultado = true;
                }
                else
                {
                    resultado = false;
                }
            }

            return resultado;
        }

        protected void B_Guardar_Click(object sender, EventArgs e)
        {

            if (bTiempo == true)
            {
                if (Validar() == true)
                { return; }
            }

            if (string.IsNullOrEmpty(DDL_Dia.SelectedValue) || string.IsNullOrEmpty(DDL_Mes.SelectedValue) || string.IsNullOrEmpty(DD_Anio.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ERROR", "<script type=text/javascript>alert('la fecha no es valida');</script>", false);
                return;
            }

            String FechaInicioDia = DDL_Dia.SelectedValue;
            String FechainicioMes = DDL_Mes.SelectedValue;
            String FechaInicioAnio = DD_Anio.SelectedValue;

            String periodo = DD_Periodo.SelectedValue;
            String tiempo = DropDownList1.SelectedValue;

            String metodo = DD_MetProy.SelectedValue;

            String costoVenta = TB_CostoVenta.Text;

            String JusProVentas = TB_JusProVen.Text;

            String PoliCarte = TB_PoliCarte.Text;

            Int32 valor;

            String sql;
            sql = "SELECT COUNT(*) as resul FROM [ProyectoMercadoProyeccionVentas] WHERE [codproyecto] = " + CodigoProyecto;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(sql, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                reader.Read();
                valor = Int32.Parse(reader["resul"].ToString());
                conn.Close();

                DateTime txtfecha = DateTime.Parse(FechaInicioAnio + "/" + FechainicioMes + "/" + FechaInicioDia);

                string conexionStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                using (var con = new SqlConnection(conexionStr))
                {
                    using (var com = con.CreateCommand())
                    {
                        com.CommandText = "MD_Insertar_Actualizar_ProyectoMercadoProyeccionVentas";
                        com.CommandType = System.Data.CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@_CodProyecto", CodigoProyecto);
                        com.Parameters.AddWithValue("@_FechaArranque", txtfecha);
                        com.Parameters.AddWithValue("@_CodPeriodo", periodo);
                        com.Parameters.AddWithValue("@_TiempoProyeccion", tiempo);
                        com.Parameters.AddWithValue("@_MetodoProyeccion", metodo);
                        com.Parameters.AddWithValue("@_PoliticaCartera", PoliCarte);
                        com.Parameters.AddWithValue("@_CostoVenta", costoVenta);
                        com.Parameters.AddWithValue("@_justificacion", JusProVentas);

                        if (valor > 0) com.Parameters.AddWithValue("@_caso", "UPDATE");
                        else com.Parameters.AddWithValue("@_caso", "CREATE");
                        // Validar que no guarde espacios en blanco
                        try
                        {
                            con.Open();
                            com.ExecuteReader();
                            //Actualizar fecha modificación del tab.
                            prActualizarTab(CodigoTab.ToString(), CodigoProyecto.ToString());
                            ProyectoGeneral.GetUltimaActualizacion(lblUltimaActualizacion, lblFechaUltimaActualizacion, chkEsRealizado, btnUpdateTab, CodigoTab, CodigoProyecto);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        finally
                        {
                            com.Dispose();
                            con.Close();
                            con.Dispose();
                        }
                    }
                }

                reader.Close();
            }
            catch (SqlException se)
            {
                throw se;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            llenarGridView();
            Tabla_VentasUnidades();
            Tabla_IngresosVenta();

            Response.Redirect(Request.RawUrl);
        }

        protected void B_AgregarProductoServicio_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["OpcionMercadoProyecciones"] = "agregar";
            HttpContext.Current.Session["valordeId_Producto"] = 0;
            Response.Redirect("~/FONADE/evaluacion/CatalogoProducto2.aspx");
        }

        private void llenarGridView()
        {
            DataTable tabla = new System.Data.DataTable();
            try
            {
                txtSQL = "SELECT * FROM ProyectoProducto WHERE CodProyecto = " + CodigoProyecto;
                tabla = consultas.ObtenerDataTable(txtSQL, "text");
                DataColumn dtcoll = new DataColumn("AdicionarInsumo");
                tabla.Columns.Add(dtcoll);
                HttpContext.Current.Session["OpcionMercadoProyecciones"] = "actualizar";
                foreach (DataRow dtrow in tabla.Rows)
                {
                    dtrow["AdicionarInsumo"] = "<a href=\"javascript:OpenPage('../Proyecto/CatalogoInsumo.aspx?CodProducto=" + dtrow["Id_Producto"] + "&NombreProducto=" + dtrow["NomProducto"] + "&Insumo=0')\">Adicionar</a>";
                    if (AllowUpdate)
                    {
                        dtrow["NomProducto"] = "<a href=\"../evaluacion/CatalogoProducto2.aspx?Id_Producto=" + dtrow["Id_Producto"] + "\">" + dtrow["NomProducto"] + "</a>";
                    }
                }
                GV_productoServicio.DataSource = tabla;
                GV_productoServicio.DataBind();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                tabla = null; txtSQL = null;
            }
        }

        protected void LB_Insumo_Click(object sender, EventArgs e)
        {
            var indicefila = ((GridViewRow)((Control)sender).NamingContainer).RowIndex;
            GridViewRow GVInventario = GV_productoServicio.Rows[indicefila];
            var lnk = (LinkButton)sender;
            string NombreProducto = GV_productoServicio.DataKeys[GVInventario.RowIndex].Values[1].ToString();
            HttpContext.Current.Session["CodProducto"] = GV_productoServicio.DataKeys[GVInventario.RowIndex].Value.ToString();
            HttpContext.Current.Session["Insumo"] = 0;
            HttpContext.Current.Session["CodProyecto"] = CodigoProyecto;
            HttpContext.Current.Session["NombreProducto"] = NombreProducto;
            string opener = Session["OpenerInsumo"] == null ? "" : Session["OpenerInsumo"].ToString();
            if (Session["OpenerInsumo"] != null && Session["OpenerInsumo"].ToString() == "false")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "mdl",
                  string.Format("window.open('{0}',null,'status:false;dialogWidth:900px;dialogHeight:1500px')",
                    Request.Url.AbsoluteUri.Remove(Request.Url.AbsoluteUri.LastIndexOf("/")) + "/CatalogoInsumo.aspx"), true);
                llenarGridView();
                Tabla_VentasUnidades();
                Tabla_IngresosVenta();
            }
        }

        protected void IB_AgregarProductoServicio_Click(object sender, ImageClickEventArgs e)
        {
            HttpContext.Current.Session["OpcionMercadoProyecciones"] = "agregar";
            Response.Redirect("~/FONADE/evaluacion/CatalogoProducto2.aspx");
        }

        protected void LB_ProductoServicio_Click(object sender, EventArgs e)
        {
            var indicefila = ((GridViewRow)((Control)sender).NamingContainer).RowIndex;
            GridViewRow GVInventario = GV_productoServicio.Rows[indicefila];
            String Id_Producto = GV_productoServicio.DataKeys[GVInventario.RowIndex].Value.ToString();
            HttpContext.Current.Session["OpcionMercadoProyecciones"] = "actualizar";
            Response.Redirect("~/FONADE/evaluacion/CatalogoProducto2.aspx");
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            var indicefila = ((GridViewRow)((Control)sender).NamingContainer).RowIndex;
            GridViewRow GVInventario = GV_productoServicio.Rows[indicefila];

            String Id_Producto = GV_productoServicio.DataKeys[GVInventario.RowIndex].Value.ToString();

            var qry = string.Format("SELECT  ProyectoProductoInsumo.CodInsumo, ProyectoInsumo.nomInsumo FROM ProyectoProductoInsumo INNER JOIN " +
                                    "ProyectoInsumo ON ProyectoProductoInsumo.CodInsumo = ProyectoInsumo.Id_Insumo WHERE " +
                                    "(ProyectoProductoInsumo.CodProducto = {0})", Id_Producto);
            var tyu = new Clases.genericQueries().executeQueryReader(qry);
            var iop = string.Empty;
            while (tyu.Read())
            {
                iop += string.Format("{0} - {1}", tyu["CodInsumo"], tyu["nomInsumo"]);
            }
            qry = string.Format("Error durante la eliminación del producto, tiene asignados los siguientes insumos:{0}", iop);
            if (!string.IsNullOrEmpty(iop))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "msg", string.Format("alert('{0}');", qry), true);
            }
            else
            {
                SqlCommand cmd;
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());

                try
                {
                    conn.Open();
                    cmd = new SqlCommand("DELETE FROM [proyectoproductoprecio] WHERE [codproducto] = " + Id_Producto, conn);
                    //conn.Open();
                    cmd.ExecuteNonQuery();
                    //conn.Close();
                    cmd = new SqlCommand("DELETE FROM [proyectoproductounidadesventas] WHERE [codproducto] = " + Id_Producto, conn);
                    //conn.Open();
                    cmd.ExecuteNonQuery();
                    //conn.Close();
                    cmd = new SqlCommand("DELETE FROM [ProyectoProducto] WHERE [Id_Producto] = " + Id_Producto, conn);
                    //conn.Open();
                    cmd.ExecuteNonQuery();
                    //conn.Close();
                    ProyectoGeneral.GetUltimaActualizacion(lblUltimaActualizacion, lblFechaUltimaActualizacion, chkEsRealizado, btnUpdateTab, CodigoTab, CodigoProyecto);
                }
                catch (SqlException se)
                {
                    throw se;
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            llenarGridView();
            Tabla_VentasUnidades();
            Tabla_IngresosVenta();
        }

        private void enviar()
        {
            ClientScriptManager cm = this.ClientScript;
            cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>open('../Ayuda/Mensaje.aspx', 'Proyección de ventas', 'width=500,height=400');</script>");
        }

        protected void I_AyudaProVentas_Click(object sender, ImageClickEventArgs e)
        {
            HttpContext.Current.Session["mensaje"] = "1"; enviar();
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            HttpContext.Current.Session["mensaje"] = "2"; enviar();
        }

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            HttpContext.Current.Session["mensaje"] = "3"; enviar();
        }

        private void CargarPeriodos()
        {
            OperacionesComunes operCommon = new OperacionesComunes();
            ProyectoMercadoProyeccionVentasNegocio proyVentas = new ProyectoMercadoProyeccionVentasNegocio();

            try
            {
                DD_Periodo.Items.Clear();

                List<Periodo> lst = new List<Periodo>();
                lst = operCommon.Periodos();

                DD_Periodo.DataSource = lst;
                DD_Periodo.DataTextField = "NomPeriodo";
                DD_Periodo.DataValueField = "Id_Periodo";
                DD_Periodo.DataBind();

                List<ProyectoMercadoProyeccionVenta> lstproy = proyVentas.GetProyeccionesVenta(Convert.ToInt32(CodigoProyecto));

            }
            catch (Exception ex)
            {
            }
        }

        private void CargarProyeccionesDeVentas()
        {
            //Inicializar variables.
            DataTable tabla = new System.Data.DataTable();
            try
            {
                txtSQL = "SELECT * FROM ProyectoMercadoProyeccionVentas WHERE CodProyecto = " + CodigoProyecto;
                tabla = consultas.ObtenerDataTable(txtSQL, "text");

                if (tabla.Rows.Count > 0)
                {
                    //Inicializar variables.
                    DateTime fecha_arranque = new DateTime();
                    String tiempo_proyeccion = "";

                    DD_Periodo.SelectedValue = tabla.Rows[0]["CodPeriodo"].ToString();

                    foreach (ListItem item in DropDownList1.Items)
                    {
                        tiempo_proyeccion = tabla.Rows[0]["TiempoProyeccion"].ToString();
                        if (item.Value == tiempo_proyeccion)
                        { item.Selected = true; break; }
                    }
                    DD_MetProy.SelectedValue = DD_MetProy.Items.FindByValue(tabla.Rows[0]["MetodoProyeccion"].ToString()) != null ?
                        tabla.Rows[0]["MetodoProyeccion"].ToString() : "Otro";

                    //Consultar la fecha.
                    try { fecha_arranque = Convert.ToDateTime(tabla.Rows[0]["FechaArranque"].ToString()); }
                    catch { fecha_arranque = DateTime.Today; }

                    //Establecer selección de fecha de arranque.
                    DDL_Dia.SelectedValue = fecha_arranque.Day.ToString();
                    DDL_Mes.SelectedValue = fecha_arranque.Month <= 9 ? string.Format("0{0}", fecha_arranque.Month) : fecha_arranque.Month.ToString();
                    DD_Anio.SelectedValue = fecha_arranque.Year.ToString();

                    TB_CostoVenta.Text = tabla.Rows[0]["CostoVenta"].ToString();

                    txtCodPeriodo = tabla.Rows[0]["CodPeriodo"].ToString();
                    txtTiempoProyeccion = Convert.ToInt32(tabla.Rows[0]["TiempoProyeccion"].ToString());
                    txtMetodoProyeccion = tabla.Rows[0]["MetodoProyeccion"].ToString();
                    txtCostoVenta = tabla.Rows[0]["CostoVenta"].ToString();
                    bTiempo = true;
                }
                else
                { bTiempo = false; }
            }
            catch { bTiempo = false; }

            if (!chkEsRealizado.Checked && !string.IsNullOrEmpty(TB_CostoVenta.Text.Trim()))
            {
                B_AgregarProductoServicio.Visible = true;
                IB_AgregarProductoServicio.Visible = true;
            }
            else
            {
                B_AgregarProductoServicio.Visible = false;
                IB_AgregarProductoServicio.Visible = false;
            }
        }

        private Int32 Cargar_numAnios()
        {
            Int32 int_txtTiempoProyeccion = 0;
            try
            {
                txtSQL = " SELECT TiempoProyeccion FROM ProyectoMercadoProyeccionVentas WHERE codProyecto = " + CodigoProyecto;

                var dt = consultas.ObtenerDataTable(txtSQL, "text");

                if (dt.Rows.Count > 0)
                {
                    int_txtTiempoProyeccion = Int32.Parse(dt.Rows[0]["TiempoProyeccion"].ToString());
                    dt = null;
                    txtSQL = null;
                    HttpContext.Current.Session["int_txtTiempoProyeccion"] = int_txtTiempoProyeccion;
                    txtTiempoProyeccion = int_txtTiempoProyeccion;
                    return int_txtTiempoProyeccion;
                }
                else
                {
                    dt = null;
                    txtSQL = null;
                    HttpContext.Current.Session["int_txtTiempoProyeccion"] = int_txtTiempoProyeccion;
                    txtTiempoProyeccion = int_txtTiempoProyeccion;
                    return int_txtTiempoProyeccion;
                }
            }
            catch
            {
                HttpContext.Current.Session["int_txtTiempoProyeccion"] = int_txtTiempoProyeccion;
                txtTiempoProyeccion = int_txtTiempoProyeccion;
                return int_txtTiempoProyeccion;
            }
        }

        private void Tabla_VentasUnidades()
        {
            //Inicializar variables.
            Table Tabla_Unidades = new Table();
            Tabla_Unidades.CssClass = "Grilla";
            Tabla_Unidades.Attributes.Add("width", "100%");
            Tabla_Unidades.Attributes.Add("cellspacing", "1");
            TableCell celdaEncabezado = new TableCell();
            TableCell celdaDatos = new TableCell();
            TableCell celdaDatosda = new TableCell();
            TableHeaderRow fila1 = new TableHeaderRow();
            TableRow fila = new TableRow();
            DataTable rsProducto = new System.Data.DataTable();
            DataTable rsUnidades = new System.Data.DataTable();

            try
            {
                #region Generar fila "Proyección de Ventas (Unidades)".

                fila1 = new TableHeaderRow();
                fila1.Attributes.Add("bgcolor", "#3D5A87");
                fila1.Attributes.Add("align", "center");
                celdaEncabezado = new TableHeaderCell();
                celdaEncabezado.Attributes.Add("colspan", "6");
                celdaEncabezado.Attributes.Add("color", "white");
                celdaEncabezado.Style.Add("text-align", "center");
                celdaEncabezado.Text = "Proyección de Ventas (Unidades)";
                fila1.Cells.Add(celdaEncabezado);
                Tabla_Unidades.Rows.Add(fila1);

                #endregion

                #region Generar encabezados de "Producto o Servicio" y las celdas de Años.

                fila1 = new TableHeaderRow();
                celdaEncabezado = new TableHeaderCell();
                celdaEncabezado.Attributes.Add("text-align", "left");
                celdaEncabezado.Text = "Producto o Servicio";
                fila1.Cells.Add(celdaEncabezado);

                for (int i = 1; i < Cargar_numAnios() + 1; i++)
                {
                    celdaEncabezado = new TableHeaderCell();
                    celdaEncabezado.Attributes.Add("text-align", "left");
                    celdaEncabezado.Text = "Año " + i.ToString();
                    fila1.Cells.Add(celdaEncabezado);
                }
                Tabla_Unidades.Rows.Add(fila1);

                #endregion

                //Consultar productos.
                txtSQL = " select id_producto, nomproducto from proyectoproducto where codproyecto = " + CodigoProyecto;

                //Asignar resultados a variable DataTable.
                rsProducto = consultas.ObtenerDataTable(txtSQL, "text");

                //Generar filas.
                foreach (DataRow row_rsProducto in rsProducto.Rows)
                {
                    #region Inicializar la fila con la primera celda "Nombre del producto".
                    //Inicializar fila.
                    fila = new TableHeaderRow();
                    fila.Attributes.Add("bgcolor", "#3D5A87");
                    //Inicializar la celda del encabezado.
                    celdaDatos = new TableCell();
                    celdaDatos.Attributes.Add("text-align", "left");
                    //Continuar con la generación de la celda.
                    celdaDatos.Text = row_rsProducto["nomproducto"].ToString();
                    //Agregar la celda a la fila.
                    fila.Cells.Add(celdaDatos);
                    #endregion

                    #region Consultar y asignar los resultados a la variable "rsUnidades".
                    txtSQL = " select sum(unidades) as unidades,ano from proyectoproductounidadesventas " +
                                         " where codproducto = " + row_rsProducto["id_Producto"].ToString() +
                                         " group by ano order by ano ";
                    rsUnidades = consultas.ObtenerDataTable(txtSQL, "text");
                    #endregion

                    #region Generar las celdas con las unidades.
                    foreach (DataRow row_rsUnidades in rsUnidades.Rows)
                    {
                        //<td align='right'>"&rsUnidades("unidades")&"</td>"&VbCrLf
                        //Inicializar la celda.
                        celdaDatosda = new TableCell();
                        celdaDatosda.Attributes.Add("align", "right");
                        celdaDatosda.Style.Add("text-align", "right");
                        //Continuar con la generación de la celda.
                        celdaDatosda.Text = row_rsUnidades["unidades"].ToString();
                        //Añadir la celda a la fila.
                        fila.Cells.Add(celdaDatosda);
                    }
                    #endregion

                    //Agregar la fila a la tabla.
                    Tabla_Unidades.Rows.Add(fila);
                }

                //Agregar la tabla.
                pnl_Datos.Controls.Add(Tabla_Unidades);

            }
            catch (Exception ex)
            {
                //lblError.Text = ex.Message;
            }
            finally
            {
                // 2014/11/06 RAlvaradoT, esto se cambia a finally porque en ambos caso hacia lo mismo y tenia el codigo repetido
                // se coloca en 1 sola parte y se optimiza el codigo
                #region Destruir variables.

                Tabla_Unidades = null;
                celdaEncabezado = null;
                celdaDatos = null;
                celdaDatosda = null;
                fila1 = null;
                fila = null;
                rsProducto = null;
                rsUnidades = null;

                #endregion
            }
        }

        private void Tabla_IngresosVenta()
        {
            //Inicializar variables.
            Table Tabla_Unidades = new Table();
            Tabla_Unidades.CssClass = "Grilla";
            Tabla_Unidades.Attributes.Add("width", "100%");
            Tabla_Unidades.Attributes.Add("cellspacing", "1");
            TableCell celdaEncabezado = new TableCell();
            TableCell celdaDatos = new TableCell();
            TableCell celdaDatosda = new TableCell();
            TableCell celdaEspecial = new TableCell();
            TableHeaderRow fila1 = new TableHeaderRow();
            TableRow fila = new TableRow();
            DataTable rsProducto = new System.Data.DataTable();
            DataTable rsUnidades = new System.Data.DataTable();
            Double[] TotalPt = new Double[15];
            Double[] TotalIvaPt = new Double[15];
            Label lbl = new Label();
            lbl.ID = "lbl_b";
            lbl.Text = "<br/><br/>";
            pnl_Datos.Controls.Add(lbl);
            lbl = null;
            String[] arr_totales = { "&nbsp;", "Total", "IVA", "Total mas IVA" };

            try
            {
                #region Generar fila "Proyección de Ingresos por Ventas".

                fila1 = new TableHeaderRow();
                fila1.Attributes.Add("bgcolor", "#3D5A87");
                fila1.Attributes.Add("align", "center");
                celdaEncabezado = new TableHeaderCell();
                celdaEncabezado.Attributes.Add("colspan", "6");
                celdaEncabezado.Attributes.Add("color", "white");
                celdaEncabezado.Style.Add("text-align", "center");
                celdaEncabezado.Text = "Proyección de Ingresos por Ventas";
                fila1.Cells.Add(celdaEncabezado);
                Tabla_Unidades.Rows.Add(fila1);

                #endregion

                #region Generar encabezados de "Producto o Servicio" y las celdas de Años.

                fila1 = new TableHeaderRow();
                celdaEncabezado = new TableHeaderCell();
                celdaEncabezado.Attributes.Add("text-align", "left");
                celdaEncabezado.Text = "Producto o Servicio";
                fila1.Cells.Add(celdaEncabezado);

                for (int i = 1; i < Cargar_numAnios() + 1; i++)
                {
                    celdaEncabezado = new TableHeaderCell();
                    celdaEncabezado.Attributes.Add("text-align", "left");
                    celdaEncabezado.Text = "Año " + i.ToString();
                    fila1.Cells.Add(celdaEncabezado);
                }
                Tabla_Unidades.Rows.Add(fila1);

                #endregion

                //Consultar productos.
                txtSQL = " select id_producto, nomproducto, porcentajeiva from proyectoproducto where codproyecto = " + CodigoProyecto;

                //Asignar resultados a variable DataTable.
                rsProducto = consultas.ObtenerDataTable(txtSQL, "text");

                foreach (DataRow row_rsProducto in rsProducto.Rows)
                {
                    #region Consultar y asignar resultados a la variable "rsUnidades".
                    txtSQL = " select sum(unidades) as unidades, precio, sum(unidades)*replace(rtrim(Precio),',','') as total, ano " +
                                         " from proyectoproductounidadesventas u, proyectoproductoprecio p " +
                                         " where p.codproducto=u.codproducto and periodo=ano " +
                                         " and p.codproducto = " + row_rsProducto["id_Producto"].ToString() +
                                         " group by ano,precio order by ano";

                    rsUnidades = consultas.ObtenerDataTable(txtSQL, "text");
                    #endregion

                    foreach (DataRow rw in rsUnidades.Rows)
                    {
                        #region Generar la primera celda con el valor "Nombre del Producto".
                        //Inicializar fila.
                        fila = new TableHeaderRow();
                        fila.Attributes.Add("bgcolor", "#3D5A87");
                        //Inicializar la celda del encabezado.
                        celdaDatos = new TableCell();
                        celdaDatos.Attributes.Add("text-align", "left");
                        //Continuar con la generación de la celda.
                        celdaDatos.Text = row_rsProducto["nomproducto"].ToString();
                        //Agregar la celda a la fila.
                        fila.Cells.Add(celdaDatos);
                        #endregion

                        Int32 yyyy = Int32.Parse(rw["ano"].ToString());
                        try
                        {
                            TotalPt[(yyyy)] = TotalPt[(yyyy)] + Double.Parse(rw["total"].ToString());
                        }
                        catch(Exception ex)
                        {

                        }

                        Int32 ivas = Int32.Parse(rw["ano"].ToString());
                        TotalIvaPt[(yyyy)] = TotalIvaPt[(yyyy)] + (Double.Parse(rw["total"].ToString()) * Double.Parse(row_rsProducto["porcentajeiva"].ToString()) / 100);
                    }

                    #region Generar las celdas acerca de la multiplicación de las unidades por los precios.
                    foreach (DataRow row_rsUnidades in rsUnidades.Rows)
                    {
                        //Inicializar la celda.
                        celdaDatosda = new TableCell();
                        celdaDatosda.Attributes.Add("align", "right");
                        celdaDatosda.Style.Add("text-align", "right");
                        //Continuar con la generación de la celda.
                        //celdaDatosda.Text = ( Double.Parse(row_rsUnidades["unidades"].ToString()) *
                        //    (row_rsUnidades["precio"] != null && !string.IsNullOrEmpty(row_rsUnidades["precio"].ToString()) ? Double.Parse(row_rsUnidades["precio"].ToString()) : 1)
                        //    ).ToString("N2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                        Double ggg = Convert.ToDouble(row_rsUnidades["Total"]); // 34533.89;
                        celdaDatosda.Text = "<strong>" + ggg.ToString("N2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</strong>";

                        //  celdaDatosda.Text = Double.Parse(row_rsUnidades["Total"].ToString()).ToString();
                        fila.Cells.Add(celdaDatosda);
                    }
                    #endregion

                    //Agregar la fila a la tabla.
                    Tabla_Unidades.Rows.Add(fila);
                }

                #region Generar filas de totales.

                //Recorrer la variable "arr_totates" que contiene los títulos pre-definidos.
                for (int i = 0; i < arr_totales.Count(); i++)
                {
                    #region Generar la fila única de acuerdo al valor almacenado en el arreglo "arr_totales".

                    #region Generar la primera celda de la fila.
                    fila = new TableRow();
                    celdaEspecial = new TableCell();
                    celdaEspecial.Text = "<strong>" + arr_totales[i] + "</strong>";
                    fila.Cells.Add(celdaEspecial);
                    #endregion

                    #region Establecer iteración de datos.

                    if (i == 1)
                    {
                        #region Iterar la variable "TotalPt" = Años.
                        for (int j = 1; j < txtTiempoProyeccion + 1; j++)
                        {
                            if (j < TotalIvaPt.Length)
                            {
                                celdaEspecial = new TableCell();
                                celdaEspecial.Attributes.Add("align", "right");
                                celdaEspecial.Text = "<strong>" + TotalPt[j].ToString("N2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</strong>";
                                fila.Cells.Add(celdaEspecial);
                            }
                        }
                        #endregion
                    }
                    if (i == 2)
                    {
                        #region Iterar la variable "numIVA" = IVA's.
                        for (int j = 1; j < txtTiempoProyeccion + 1; j++)
                        {
                            if (j < TotalIvaPt.Length)
                            {
                                celdaEspecial = new TableCell();
                                celdaEspecial.Attributes.Add("align", "right");
                                celdaEspecial.Text = "<strong>" + TotalIvaPt[j].ToString("N2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</strong>";
                                fila.Cells.Add(celdaEspecial);
                            }
                        }
                        #endregion
                    }
                    if (i == 3)
                    {
                        #region Iterar para sumar los valores de las variables "TotalPt" y "numIVA".
                        for (int j = 1; j < txtTiempoProyeccion + 1; j++)
                        {
                            if (j < TotalIvaPt.Length)
                            {
                                celdaEspecial = new TableCell();
                                celdaEspecial.Attributes.Add("align", "right");
                                celdaEspecial.Text = "<strong>" + (TotalIvaPt[j] + TotalPt[j]).ToString("N2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</strong>";
                                fila.Cells.Add(celdaEspecial);
                            }
                        }
                        #endregion
                    }

                    #endregion

                    //Añadir la celda generada a la fila.
                    fila.Cells.Add(celdaEspecial);

                    //Agregar fila a la tabla.
                    Tabla_Unidades.Rows.Add(fila);

                    #endregion
                }

                #endregion

                //Agregar la tabla.
                pnl_Datos.Controls.Add(Tabla_Unidades);

                #region Destruir variables.

                Tabla_Unidades = null;
                celdaEncabezado = null;
                celdaDatos = null;
                celdaDatosda = null;
                fila1 = null;
                fila = null;
                rsProducto = null;
                rsUnidades = null;

                #endregion
            }
            catch
            {
                #region Destruir variables.

                Tabla_Unidades = null;
                celdaEncabezado = null;
                celdaDatos = null;
                celdaDatosda = null;
                fila1 = null;
                fila = null;
                rsProducto = null;
                rsUnidades = null;

                throw;

                #endregion
            }
        }

        protected void btnUpdateTab_Click(object sender, EventArgs e)
        {
            try
            {
                ProyectoGeneral.UpdateTab(CodigoTab, CodigoProyecto, usuario.IdContacto, usuario.CodGrupo, chkEsRealizado.Checked);
                CargarPeriodos();
                CargarProyeccionesDeVentas();

                llenarGridView();
                Tabla_VentasUnidades();
                Tabla_IngresosVenta();
            }
            catch (ApplicationException ex)
            {
                chkEsRealizado.Checked = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Advertencia, detalle : " + ex.Message + "');", true);
            }
            catch (Exception ex)
            {
                chkEsRealizado.Checked = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }
        }

        protected void ImageButton11_Click(object sender, ImageClickEventArgs e)
        {
            Session["TabInvoca"] = "MercaProyect";
            HttpContext.Current.Session["CodProyecto"] = CodigoProyecto;
            HttpContext.Current.Session["txtTab"] = CodigoTab.ToString();
            HttpContext.Current.Session["Accion"] = "Nuevo";
            Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        protected void ImageButton22_Click(object sender, ImageClickEventArgs e)
        {
            Session["TabInvoca"] = "MercaProyect";
            HttpContext.Current.Session["CodProyecto"] = CodigoProyecto;
            HttpContext.Current.Session["txtTab"] = CodigoTab.ToString();
            HttpContext.Current.Session["Accion"] = "Vista";
            Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        protected void DD_MetProy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DD_MetProy.SelectedValue == "Otro")
                td_otroMedio.Visible = true;
            else
                td_otroMedio.Visible = false;
        }

        private ProyectoMercadoProyeccionVenta getProyectoMercadoProyeccionVenta(int codigoProyecto)
        {
            var entity = (from mercadoProyeccion in consultas.Db.ProyectoMercadoProyeccionVentas
                          where mercadoProyeccion.CodProyecto == Convert.ToInt32(codigoProyecto)
                          select mercadoProyeccion).FirstOrDefault();

            return entity;
        }

        protected void GV_productoServicio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GV_productoServicio.PageIndex = e.NewPageIndex;
            llenarGridView();
        }
    }
}