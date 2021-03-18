using Datos;
using Fonade.Negocio;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.Proyecto
{
    public partial class ProyectoOperacionCompras : Negocio.Base_Page
    {
        #region Variables globales.

        public String codProyecto;
        public int txtTab = Constantes.CONST_Compras;
        public String codConvocatoria;
        String txtSQL;
        Boolean esMiembro;
        Boolean bRealizado;

        //Diego Quiñonez  18 de Diciembre de 2014
        private int CantidadRegistros
        {
            get
            {
                return ViewState["CantidadRegistros"] != null
                    && !string.IsNullOrEmpty(ViewState["CantidadRegistros"].ToString())
                    ? int.Parse(ViewState["CantidadRegistros"].ToString()) : 0;
            }
            set {  ViewState["CantidadRegistros"] = value; }
        }

        #endregion

        public bool vldt { get { if (usuario.CodGrupo == Constantes.CONST_Evaluador) { return false; } else { return new Clases.genericQueries().ValidateUserCode(usuario.IdContacto, HttpContext.Current.Session["CodProyecto"]); } } }

        public bool visibleGuardar { get; set; }

        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //codProyecto = HttpContext.Current.Session["CodProyecto"] != null ? codProyecto = HttpContext.Current.Session["CodProyecto"].ToString() : "0";
            codProyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? HttpContext.Current.Session["CodProyecto"].ToString() : "0";
            codConvocatoria = HttpContext.Current.Session["CodConvocatoria"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodConvocatoria"].ToString()) ? HttpContext.Current.Session["CodConvocatoria"].ToString() : "0";
            //Consultar si es miembro.
            esMiembro = fnMiembroProyecto(usuario.IdContacto, codProyecto);

            //Consultar si está "realizado".
            bRealizado = esRealizado(txtTab.ToString(), codProyecto, ""); //);

            if (esMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor && !bRealizado)
            {
                div_post_it_1.Visible = true;
                tabla_docs.Visible = true;
                btnGuardar.Visible = true;
                Post_It1._mostrarPost = true;
                //tr_buttons.Visible = true;
                //tr_1.Visible = true;
            }
            else
            {
                div_post_it_1.Visible = false;
                tabla_docs.Visible = false;
                btnGuardar.Visible = false;
                Post_It1._mostrarPost = false;
                //tr_buttons.Visible = false;
                //tr_1.Visible = false;
            }

            if (!IsPostBack)
            {
                      
                ObtenerDatosUltimaActualizacion();
                Session["OpenerInsumo"] = "false";
            }
                
            if (codProyecto != "0")
            {
                //ObtenerDatosUltimaActualizacion();
                GenerarTablaInsumos();
            }
        }

        #region Diego Quiñonez - 18 de diciembre de 2014

        private void GenerarTablaInsumos()
        {
            if (!codProyecto.Equals("0"))
            {
                var proyectoProducto = (from pp in consultas.Db.ProyectoProductos
                                        orderby pp.NomProducto
                                        where pp.CodProyecto == int.Parse(codProyecto)
                                        select new
                                        {
                                            pp.Id_Producto,
                                            pp.NomProducto
                                        });

                foreach (var pp in proyectoProducto)
                {
    
                    string codT_Insumo = pp.Id_Producto.ToString();
                    TableRow fila = new TableRow();
                    #region adicionar insumo
                    if (esMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor && !bRealizado)
                    {
                        fila = new TableRow();
                        ImageButton imgBtn = new ImageButton();
                        imgBtn.ID = "imgBtn_AddInsumo" + pp.Id_Producto + ";" + pp.NomProducto.ToUpper();
                        imgBtn.ImageUrl = "~/Images/icoAdicionarUsuario.gif";
                        imgBtn.CommandArgument = pp.Id_Producto.ToString();
                        imgBtn.CommandName = "Lista";

                        LinkButton addNewInsumo = new LinkButton();
                        addNewInsumo.ID = "linkButton" + pp.Id_Producto + ";" + pp.NomProducto.ToUpper();
                        addNewInsumo.Text = "Agregar insumo al producto o servicio";
                        addNewInsumo.CommandArgument = pp.Id_Producto.ToString();
                        addNewInsumo.CommandName = "Lista";
                        addNewInsumo.Click += new EventHandler(lnkBtn_AddInsumo_Click);
                        addNewInsumo.ToolTip = "haga click aqui para adicionar Insumo al producto o servicio";


                        imgBtn.Click += new ImageClickEventHandler(lnkBtn_AddInsumo_Click);
                        imgBtn.ToolTip = "haga click aqui para adicionar Insumo al producto o servicio";
                        fila.Cells.Add(celda("", 1, 1, imgBtn));
                        fila.Cells.Add(celda("", 2, 1, addNewInsumo));
                        fila.Cells.Add(celda(pp.NomProducto, 6));
                        fila.Attributes.Add("class", "FondoCelda");
                        tbl.Rows.Add(fila);
                    }
                    #endregion

                    TableHeaderRow Headerfila = new TableHeaderRow();

                    Headerfila.Cells.Add(CeldaTitulo());
                    Headerfila.Cells.Add(CeldaTitulo("Materia Prima, Insumo o Requerimiento"));
                    Headerfila.Cells.Add(CeldaTitulo("Unidad"));
                    Headerfila.Cells.Add(CeldaTitulo("Cantidad", 160));
                    Headerfila.Cells.Add(CeldaTitulo("Presentacion", 160));
                    Headerfila.Cells.Add(CeldaTitulo("Margen de Desperdicio (%)", 160));

                    tbl.Rows.Add(Headerfila);

                    var ProyectoProductoYTipoInsumo = (from ppi in consultas.Db.ProyectoProductoInsumos
                                                       from pi in consultas.Db.ProyectoInsumos
                                                       from pti in consultas.Db.TipoInsumos
                                                       orderby pti.NomTipoInsumo, pi.nomInsumo
                                                       where pi.codTipoInsumo == pti.Id_TipoInsumo
                                                       && ppi.CodInsumo == pi.Id_Insumo
                                                       && ppi.CodProducto == int.Parse(codT_Insumo)
                                                       select new
                                                       {
                                                           ppi,
                                                           pi.Id_Insumo,
                                                           pi.Unidad,
                                                           pi.nomInsumo,
                                                           pti
                                                       });

                    CantidadRegistros = ProyectoProductoYTipoInsumo.Count();

                    byte tipoInsumo = 0;
                    foreach(var ppti in ProyectoProductoYTipoInsumo)
                    {
                        if (tipoInsumo != ppti.pti.Id_TipoInsumo)
                        {
                            tipoInsumo = ppti.pti.Id_TipoInsumo;
                            fila = new TableRow();
                            fila.Cells.Add(celda());
                            fila.Cells.Add(celda(ppti.pti.NomTipoInsumo, 5));
                            fila.Attributes.Add("class", "FondoCelda2");
                            tbl.Rows.Add(fila);
                        }
                        fila = new TableRow();

                        if (esMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor && !bRealizado)
                        {
                            #region si es miembro y no esta realizado y esta logeado como emprendedor
                            ImageButton imgBtn = new ImageButton();
                            imgBtn.ID = "imgBtn_BInsumo" + ppti.Id_Insumo + "_" + ppti.ppi.CodProducto;
                            imgBtn.ImageUrl = "~/Images/icoBorrar.gif";
                            imgBtn.AlternateText = "Eliminar insumo del producto";
                            imgBtn.CommandArgument = ppti.Id_Insumo + ";" + ppti.ppi.CodProducto;
                            imgBtn.CommandName = "Borrar";
                            imgBtn.OnClientClick = "return confirm('¿Esta seguro que desea borrar el insumo seleccionado?')";
                            imgBtn.Click += new ImageClickEventHandler(imgBtn_AddInsumo_Click);
                            fila.Cells.Add(celda("", 1, 1, imgBtn));
                            Label lnkBtn = new Label();
                            lnkBtn.ID = "lnkBtn_EInsumo" + pp.Id_Producto + "_" + ppti.Id_Insumo;
                            lnkBtn.Text = "<a href=\"javascript:OpenPage('../Proyecto/CatalogoInsumo.aspx?CodProducto=" + ppti.ppi.CodProducto + "&NombreProducto=" + pp.NomProducto + "&Insumo=" + ppti.Id_Insumo +"')\">" + ppti.nomInsumo + "</a>";
                            fila.Cells.Add(celda("", 1, 1, lnkBtn));
                            fila.Cells.Add(celda(ppti.Unidad));

                            Label txt = new Label();
                            txt.ID = "Cantidad_" + ppti.Id_Insumo + "_" + ppti.ppi.CodProducto;
                            txt.Width = 150;
                            txt.Text = Convert.ToDecimal(ppti.ppi.Cantidad.Value).ToString();
                            fila.Cells.Add(celda("", 1, 1, txt, 160));

                            txt = new Label();
                            txt.ID = "Presentacion_" + ppti.Id_Insumo + "_" + ppti.ppi.CodProducto;
                            txt.Width = 150;
                            txt.Text = ppti.ppi.Presentacion.ToString();

                            fila.Cells.Add(celda("", 1, 1, txt, 160));

                            txt = new Label();
                            txt.ID = "Desperdicio_" + ppti.Id_Insumo + "_" + ppti.ppi.CodProducto;
                            txt.Width = 150;
                            txt.Text = ppti.ppi.Desperdicio.ToString();

                            fila.Cells.Add(celda("", 1, 1, txt, 160));
                            #endregion
                        }
                        else
                        {
                            #region si no es miembro y/o ya esta realizado o no esta logeado como emprendedor solo puede ver
                            fila.Cells.Add(celda());
                            fila.Cells.Add(celda(ppti.nomInsumo));
                            fila.Cells.Add(celda(ppti.Unidad));
                            var cant = Convert.ToDecimal(ppti.ppi.Cantidad);
                            fila.Cells.Add(celda(cant.ToString()));
                            fila.Attributes.Add("class", "alineacion");
                            fila.Cells.Add(celda(ppti.ppi.Presentacion));
                            fila.Attributes.Add("class", "alineacion");
                            fila.Cells.Add(celda(ppti.ppi.Desperdicio.ToString()));
                            fila.Attributes.Add("class", "alineacion");
                            #endregion
                        }

                        tbl.Rows.Add(fila);
                    }
                }
            }
        }

        /// <summary>
        /// devuelve un TableCell (<td></td>)
        /// </summary>
        /// <param name="texto"></param>
        /// <param name="colspan"></param>
        /// <param name="rowspan"></param>
        /// <param name="control"></param>
        /// <returns></returns>
        private TableCell celda(string texto = "", int colspan = 1, int rowspan = 1, Control control = null, int width = -1)
        {
            TableCell celda = new TableCell();

            if (control != null)
                celda.Controls.Add(control);

            if(!string.IsNullOrEmpty(texto))
                celda.Text = texto;

            if (width != -1)
                celda.Width = width;

            celda.ColumnSpan = colspan;
            celda.RowSpan = rowspan;

            return celda;
        }

        /// <summary>
        /// devuelve un TableHeaderCell (<th></th>)
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        private TableHeaderCell CeldaTitulo(string texto = "", int width = -1)
        {
            TableHeaderCell HeaderCelda = new TableHeaderCell();

            var textoSplit = texto.Split(',');

            if(textoSplit.Count() == 0)
            {
                if (!string.IsNullOrEmpty(texto))
                    HeaderCelda.Text = texto;

                if (width != -1)
                    HeaderCelda.Width = width;
            }
            else
            {
                if (!string.IsNullOrEmpty(textoSplit[0]))
                    HeaderCelda.ColumnSpan = 1;
                    HeaderCelda.Text = textoSplit[0];

                if (width != -1)
                    HeaderCelda.Width = width;
            }

            return HeaderCelda;
        }


        protected void imgBtn_AddInsumo_Click(object sender, ImageClickEventArgs e)
        {
            var imgBtn = (ImageButton)sender;
            switch (imgBtn.CommandName)
            {
                case "Lista":
                    HttpContext.Current.Session["codProducto"] = imgBtn.CommandArgument;
                    HttpContext.Current.Session["CodProyecto"] = codProyecto;
                     Session["OpenerInsumo"] = "false";
         
                    Response.Redirect("CatalogoInsumo.aspx");
                    break;
                case "Borrar":

                    #region borrar el insumo

                    int Id_Insumo;
                    int CodProducto;

                    string[] parametros = imgBtn.CommandArgument.ToString().Split(';');
                    Id_Insumo = int.Parse(parametros[0]);
                    CodProducto = int.Parse(parametros[1]);

                    var insumoNv = (from pi in consultas.Db.ProyectoInsumos
                                    where pi.codTipoInsumo == 2 && pi.Id_Insumo == Id_Insumo
                                    select pi).Count();

                    //if (insumoNv == 1)
                    //{
                    //    int NumeroSMLVNV;
                    //    byte recauctual = (from pfi in consultas.Db.ProyectoFinanzasIngresos
                    //                       where pfi.CodProyecto == int.Parse(codProyecto)
                    //                       select pfi.Recursos).FirstOrDefault();

                    //    int codigo_conv = (from cp in consultas.Db.ConvocatoriaProyectos
                    //                       where cp.CodProyecto == int.Parse(codProyecto)
                    //                       select cp.CodConvocatoria).FirstOrDefault();

                    //    int NumeroEmpleosNV = (from pgp in consultas.Db.ProyectoGastosPersonals where pgp.CodProyecto == int.Parse(codProyecto) select pgp).Count()
                    //        + (from pi in consultas.Db.ProyectoInsumos
                    //           join ppi in consultas.Db.ProyectoProductoInsumos on pi.Id_Insumo equals ppi.CodInsumo
                    //           where pi.codTipoInsumo == 2 && pi.CodProyecto == int.Parse(codProyecto)
                    //           select pi).Count() - 1;

                    //    if (NumeroEmpleosNV < 0)
                    //        NumeroEmpleosNV = 0;

                    //    NumeroSMLVNV = obtenerNumeroSMLVNV(1, codigo_conv, NumeroEmpleosNV);
                    //    NumeroSMLVNV = obtenerNumeroSMLVNV(2, codigo_conv, NumeroEmpleosNV);
                    //    NumeroSMLVNV = obtenerNumeroSMLVNV(3, codigo_conv, NumeroEmpleosNV);
                    //    NumeroSMLVNV = obtenerNumeroSMLVNV(4, codigo_conv, NumeroEmpleosNV);
                    //    NumeroSMLVNV = obtenerNumeroSMLVNV(5, codigo_conv, NumeroEmpleosNV);
                    //    NumeroSMLVNV = obtenerNumeroSMLVNV(6, codigo_conv, NumeroEmpleosNV);

                    //    if (recauctual > NumeroSMLVNV)
                    //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se puede borrar. La cantidad de recursos solicitados (smlv) son superiores a los permitidos según la cantidad de empleos generados. Modifíquelos y asegúrese que sea menor o igual a <%=NumeroSMLVNV%> (smlv)');", true);
                    //    else
                    //    {
                    //        var varProyectoProductoInsumo = (from ppi in consultas.Db.ProyectoProductoInsumos
                    //                                         where ppi.CodProducto == CodProducto
                    //                                         && ppi.CodInsumo == Id_Insumo
                    //                                         select ppi).First();

                    //        consultas.Db.ProyectoProductoInsumos.DeleteOnSubmit(varProyectoProductoInsumo);
                    //        consultas.Db.SubmitChanges();
                    //        Response.Redirect("~/FONADE/Proyecto/ProyectoOperacionCompras.aspx");
                    //    }
                    //}
                    //else
                    //{

                    var projectProduInsumo = (from ppi in consultas.Db.ProyectoProductoInsumos
                                              where ppi.CodProducto == CodProducto && ppi.CodInsumo == Id_Insumo
                                              select ppi).FirstOrDefault();

                    consultas.Db.ProyectoProductoInsumos.DeleteOnSubmit(projectProduInsumo);
                    consultas.Db.SubmitChanges();

                    var projectProducPrecio = (from ppp in consultas.Db.ProyectoInsumoPrecios
                                               where ppp.CodInsumo == Id_Insumo
                                               select ppp).ToList();
                    consultas.Db.ProyectoInsumoPrecios.DeleteAllOnSubmit(projectProducPrecio);
                    consultas.Db.SubmitChanges();

                    //var insumo = (from i in consultas.Db.ProyectoInsumos
                    //              where i.Id_Insumo == Id_Insumo
                    //              select i).FirstOrDefault();
                    //consultas.Db.ProyectoInsumos.DeleteOnSubmit(insumo);
                    //consultas.Db.SubmitChanges();
                        //string Sqlquery = "DeleteTablesLinkedtoInsumo " + Id_Insumo;
                        //InsertUpdatePrecioInsumo(Sqlquery);
                        Response.Redirect("~/FONADE/Proyecto/ProyectoOperacionCompras.aspx");
                    //}

                    consultas.Db.SubmitChanges();

                    prActualizarTab(txtTab.ToString(), codProyecto);
                    ObtenerDatosUltimaActualizacion();

                    #endregion

                    break;
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
       
        private int obtenerNumeroSMLVNV(int cod, int codigo_conv, int NumeroEmpleosNV)
        {
            int NumeroSMLVNV = 0;

            var varConvocatoriaReglaSalarios = (from crs in consultas.Db.ConvocatoriaReglaSalarios
                                                where crs.NoRegla == cod
                                                && crs.CodConvocatoria == codigo_conv
                                                select new
                                                {
                                                    crs.ExpresionLogica,
                                                    crs.EmpleosGenerados1,
                                                    crs.EmpleosGenerados2,
                                                    crs.SalariosAPrestar
                                                }).FirstOrDefault();

            if (varConvocatoriaReglaSalarios != null)
            {
                NumeroSMLVNV = condiciones(varConvocatoriaReglaSalarios.ExpresionLogica, varConvocatoriaReglaSalarios.SalariosAPrestar, varConvocatoriaReglaSalarios.EmpleosGenerados1, int.Parse(varConvocatoriaReglaSalarios.EmpleosGenerados2.ToString()), NumeroEmpleosNV);
            }

            return NumeroSMLVNV;
        }

        private int condiciones(string lista, int Salmin, int empv, int empv1, int NumeroEmpleosNV)
        {
            int NumeroSMLVNV = 0;

            switch (lista)
            {
                case "=":
                    if (NumeroEmpleosNV == empv)
                        NumeroSMLVNV = Salmin;
                    break;
                case "<":
                    if (NumeroEmpleosNV < empv)
                        NumeroSMLVNV = Salmin;
                    break;
                case ">":
                    if (NumeroEmpleosNV > empv)
                        NumeroSMLVNV = Salmin;
                    break;
                case "<=":
                    if (NumeroEmpleosNV <= empv)
                        NumeroSMLVNV = Salmin;
                    break;
                case ">=":
                    if (NumeroEmpleosNV >= empv)
                        NumeroSMLVNV = Salmin;
                    break;
                case "Entre":
                    if (NumeroEmpleosNV >= empv && NumeroEmpleosNV <= empv1)
                        NumeroSMLVNV = Salmin;
                    break;
            }

            return NumeroSMLVNV;
        }

        protected void lnkBtn_AddInsumo_Click(object sender, EventArgs e)
        {
            if (sender is ImageButton )
            {
                var lnkBtn = (ImageButton)sender;
                if (lnkBtn.CommandName.Equals("Lista")) 
                {
                    string nomproducto = lnkBtn.ID.Split(';')[1];
                    HttpContext.Current.Session["codProducto"] = lnkBtn.CommandArgument;
                    HttpContext.Current.Session["CodProyecto"] = codProyecto;
                    HttpContext.Current.Session["NombreProducto"] = nomproducto;

                    Response.Redirect("Intermedia.aspx");
                }                                                
            }
            else if( sender is LinkButton ) {
                var lnkBtn = (LinkButton)sender;                
                if (lnkBtn.CommandName.Equals("Lista"))
                {
                    string nomproducto = lnkBtn.ID.Split(';')[1];
                    HttpContext.Current.Session["codProducto"] = lnkBtn.CommandArgument;
                    HttpContext.Current.Session["CodProyecto"] = codProyecto;
                    HttpContext.Current.Session["NombreProducto"] = nomproducto;

                    Response.Redirect("Intermedia.aspx");
                }
            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
           

            prActualizarTab(txtTab.ToString(), codProyecto.ToString());
            //Marcar(txtTab.ToString(), codProyecto, "", chk_realizado.Checked);
            Response.Redirect(Request.RawUrl);
        }

        #endregion

       
        #region Métodos de .

        /// <summary>
        
        /// 06/06/2014.
        /// Obtener la información acerca de la última actualización realizada, ási como la habilitación del 
        /// CheckBox para el usuario dependiendo de su grupo / rol.
        /// </summary>
        private void ObtenerDatosUltimaActualizacion()
        {
            //Inicializar variables.
            //String txtSQL;
            //DateTime fecha = new DateTime();
            //DataTable tabla = new DataTable();
            List<String> tabla = new List<String>();
            bool bRealizado = false;
            bool EsMiembro = false;
            Int32 numPostIt = 0;
            Int32 CodigoEstado = 0;
            OperacionesRoleNegocio opRoleNeg = new OperacionesRoleNegocio();
            btn_guardar_ultima_actualizacion.Visible = false;

            try
            {
                //Consultar si es miembro.
                EsMiembro = fnMiembroProyecto(usuario.IdContacto, codProyecto);

                //Obtener número "numPostIt".
                //numPostIt = Obtener_numPostIt();
                //numPostIt = opRoleNeg.Obtener_numPostIt(Convert.ToInt32(codProyecto));
                numPostIt = opRoleNeg.Obtener_numPostIt(Convert.ToInt32(codProyecto), Convert.ToInt32(usuario.IdContacto));

                //Consultar el "Estado" del proyecto.
                CodigoEstado = CodEstado_Proyecto(txtTab.ToString(), codProyecto, codConvocatoria);

                #region Obtener el rol.

                String codRole = String.Empty;

                var avk = opRoleNeg.ObtenerRolUsuario(codProyecto, usuario.IdContacto) ?? string.Empty;
                codRole = avk.ToString();
                HttpContext.Current.Session["CodRol"] = codRole;

                #endregion
                
                //validar aqui la actualización del emprendedor

                if (chk_realizado.Checked == false)
                {                    
                    DateTime fecha = new DateTime();

                    var usuActualizo = consultas.RetornarInformacionActualizaPPagina(int.Parse(codProyecto), txtTab).FirstOrDefault();

                    if (usuActualizo != null)
                    {
                        lbl_nombre_user_ult_act.Text = usuActualizo.nombres.ToUpper();

                        //Convertir fecha.
                        try { fecha = Convert.ToDateTime(usuActualizo.fecha); }
                        catch { fecha = DateTime.Today; }
                        //Obtener el nombre del mes (las primeras tres letras).
                        string sMes = fecha.ToString("MMM", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                        //Obtener la hora en minúscula.
                        string hora = fecha.ToString("hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture).ToLowerInvariant();
                        //Reemplazar el valor "am" o "pm" por "a.m" o "p.m" respectivamente.
                        if (hora.Contains("am")) { hora = hora.Replace("am", "a.m"); } if (hora.Contains("pm")) { hora = hora.Replace("pm", "p.m"); }
                        //Formatear la fecha según manejo de FONADE clásico. "Ej: Nov 19 de 2013 07:36:26 p.m.".
                        lbl_fecha_formateada.Text = UppercaseFirst(sMes) + " " + fecha.Day + " de " + fecha.Year + " " + hora + ".";

                        //Realizado
                        bRealizado = usuActualizo.realizado;
                    }

                    //Asignar check de acuerdo al valor obtenido en "bRealizado".
                    chk_realizado.Checked = bRealizado;
                }
                //Determinar si el usuario actual puede o no "chequear" la actualización.
                //if (!(EsMiembro && numPostIt == 0 && ((usuario.CodGrupo == Constantes.CONST_RolAsesorLider && CodigoEstado == Constantes.CONST_Inscripcion) || (CodigoEstado == Constantes.CONST_Evaluacion && usuario.CodGrupo == Constantes.CONST_RolEvaluador && es_bNuevo(codProyecto)))) || lbl_nombre_user_ult_act.Text.Trim() == "")
                if (!(EsMiembro && numPostIt == 0 && ((codRole == Constantes.CONST_RolAsesorLider.ToString() && CodigoEstado == Constantes.CONST_Inscripcion) || (CodigoEstado == Constantes.CONST_Evaluacion && codRole == Constantes.CONST_RolEvaluador.ToString() && es_bNuevo(codProyecto)))) || lbl_nombre_user_ult_act.Text.Trim() == "")
                { chk_realizado.Enabled = false; }

                //Mostrar el botón de guardar.
                //if (EsMiembro && numPostIt == 0 && lbl_nombre_user_ult_act.Text != "" && (usuario.CodGrupo == Constantes.CONST_RolAsesorLider && CodigoEstado == Constantes.CONST_Inscripcion) || (usuario.CodGrupo == Constantes.CONST_RolEvaluador && CodigoEstado == Constantes.CONST_Evaluacion && es_bNuevo(codProyecto)))
                if (EsMiembro && numPostIt == 0 && lbl_nombre_user_ult_act.Text != "" && (codRole == Constantes.CONST_RolAsesorLider.ToString() && CodigoEstado == Constantes.CONST_Inscripcion) || (codRole == Constantes.CONST_RolEvaluador.ToString() && CodigoEstado == Constantes.CONST_Evaluacion && es_bNuevo(codProyecto)))
                {
                    if (usuario.CodGrupo == Constantes.CONST_Evaluador)
                    {
                        visibleGuardar = false;
                    }
                    else
                    {
                        visibleGuardar = true;
                    }
                }

                //Mostrar los enlaces para adjuntar documentos.
                if (EsMiembro && codRole == Constantes.CONST_RolEmprendedor.ToString() && !bRealizado)
                {
                    tabla_docs.Visible = true;
                }

                visibleGuardar = visibleGuardar || Constantes.CONST_CoordinadorEvaluador == usuario.CodGrupo;
                if (usuario.CodGrupo == Constantes.CONST_Asesor)
                {
                    visibleGuardar = true;
                }
                
                //Destruir variables.
                tabla = null;
                txtSQL = null;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }

        }


        static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        /// <summary>
        
        /// 24/06/2014.
        /// Guardar la información "Ultima Actualización".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_guardar_ultima_actualizacion_Click(object sender, EventArgs e)
        {
            var chkRealizado = (Request.Form.Get("chk_realizado") == "on" ? true : false);
            Marcar(txtTab.ToString(), codProyecto, "", chkRealizado);
            ObtenerDatosUltimaActualizacion();
            GenerarTablaInsumos();
            Response.Redirect(Request.RawUrl);
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Session["TabInvoca"] = "OperCompras";
            HttpContext.Current.Session["CodProyecto"] = codProyecto;
            HttpContext.Current.Session["txtTab"] = txtTab.ToString();
            HttpContext.Current.Session["Accion"] = "Nuevo";
            Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            Session["TabInvoca"] = "OperCompras";
            HttpContext.Current.Session["CodProyecto"] = codProyecto;
            HttpContext.Current.Session["txtTab"] = txtTab.ToString();
            HttpContext.Current.Session["Accion"] = "Vista";
            Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        #endregion

        
    }
}