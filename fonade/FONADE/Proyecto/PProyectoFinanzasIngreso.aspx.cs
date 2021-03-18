using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using System.Globalization;
using System.Configuration;
using System.IO;
using System.Text;
using Fonade.Clases;

namespace Fonade.FONADE.Proyecto
{
    public partial class PProyectoFinanzasIngreso : Negocio.Base_Page
    {
        public string codProyecto;
        public string codConvocatoria;
        public int txtTab = Constantes.CONST_Ingresos;
        public string idAporteEdita;
        public string idRecursoEdita;
        public string accion;
        public string controlAccion;
        public int txtTiempoProyeccion;
        public string NumeroSMLVNV;
        int codigoConvocatoria;
        private Datos.ProyectoFinanzasIngreso registroActual;
        public Boolean esMiembro;
        
        /// <summary>
        /// Determina si está o no "realizado"
        /// </summary>
        public Boolean bRealizado;

        public bool vldt { get { if (usuario.CodGrupo == Constantes.CONST_Evaluador) { return false; } else { return new Clases.genericQueries().ValidateUserCode(usuario.IdContacto, HttpContext.Current.Session["CodProyecto"].ToString()); } } }

        public bool vislzItem { get { return (esMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor && !bRealizado); } }

        public bool ejecucion{
            get{
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador) { return (usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador && !vldt); }
                int codigoProyecto = HttpContext.Current.Session["CodProyecto"] != null ? Convert.ToInt32(HttpContext.Current.Session["CodProyecto"].ToString()) : 0;
                return
                new Clases.genericQueries().getItemsProyectoMercadoProyeccionVentas(codigoProyecto).Count > 0
                && usuario.CodGrupo != Constantes.CONST_Emprendedor && usuario.CodGrupo != Constantes.CONST_Evaluador;
            }
        }

        public bool visibleGuardar { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["CodProyecto"] == null)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "refreshParent", "window.top.location.reload();", true);
            }
            codProyecto = HttpContext.Current.Session["codProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["codProyecto"].ToString()) ? HttpContext.Current.Session["codProyecto"].ToString() : "0";
            accion = HttpContext.Current.Session["Accion"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["Accion"].ToString()) ? HttpContext.Current.Session["Accion"].ToString() : string.Empty;
            controlAccion = HttpContext.Current.Session["controlAccion"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["controlAccion"].ToString()) ? HttpContext.Current.Session["controlAccion"].ToString() : string.Empty;
            idAporteEdita = HttpContext.Current.Session["idAporte"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["idAporte"].ToString()) ? HttpContext.Current.Session["idAporte"].ToString() : string.Empty;
            idRecursoEdita = HttpContext.Current.Session["idRecurso"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["idRecurso"].ToString()) ? HttpContext.Current.Session["idRecurso"].ToString() : string.Empty;

            inicioEncabezado(codProyecto, codConvocatoria, txtTab);

            //Consultar si es miembro.
            esMiembro = fnMiembroProyecto(usuario.IdContacto, codProyecto);

            //Consultar si está "realizado".
            bRealizado = esRealizado(txtTab.ToString(), codProyecto, "");

            CargarNumeroSMLVNV();

            if (!IsPostBack)
            {                               
                if (esMiembro && !bRealizado)
                {
                    this.div_post_it_1.Visible = true;
                    this.div_post_it_2.Visible = true;
                    Post_It1._mostrarPost = true;
                    Post_It2._mostrarPost = true;
                    if (usuario.CodGrupo == Constantes.CONST_Emprendedor)
                    {
                    btnGuardar.Visible = true;
                    }
                    txtRecursosSolicitados.Enabled = true;
                }

                if (usuario.CodGrupo == Constantes.CONST_Asesor && bRealizado)
                {
                    btnLinkVerModeloFinanciero.Visible = true;
                    btn_guardar_ultima_actualizacion.Visible = true;
                }
              
                if (usuario.CodGrupo == Constantes.CONST_Emprendedor)
                {
                    if (File.Exists(ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual") + @"Proyecto\" + Math.Abs(Convert.ToInt32(codProyecto) / 2000) + @"\Proyecto_" + codProyecto + @"\FORMATOSFINANCIEROS" + codProyecto + ".xls"))
                    {
                        btnLinkVerModeloFinanciero.Visible = true;
                        btnLinkSubirModeloFinanciero.Text = "Reemplazar modelo financiero";
                    }
                }

                if (usuario.CodGrupo == Constantes.CONST_Evaluador 
                    || usuario.CodGrupo == Constantes.CONST_CallCenter
                    || usuario.CodGrupo == Constantes.CONST_CallCenterOperador
                    || usuario.CodGrupo == Constantes.CONST_AdministradorSistema 
                    || usuario.CodGrupo == Constantes.CONST_Asesor 
                    || usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador 
                    || usuario.CodGrupo == Constantes.CONST_GerenteEvaluador)
                {
                    //btnLinkVerModeloFinanciero.Visible = true;
                    if (File.Exists(ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual") + @"Proyecto\" + Math.Abs(Convert.ToInt32(codProyecto) / 2000) + @"\Proyecto_" + codProyecto + @"\FORMATOSFINANCIEROS" + codProyecto + ".xls"))
                    {
                        btnLinkVerModeloFinanciero.Visible = true;
                    }
                }

                if (!(esMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor) || bRealizado)
                    txtRecursosSolicitados.Enabled = false;

                if (esMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor && !bRealizado)
                {                  
                    Image1.Visible = true;
                    btnAdicionarAporte.Visible = true;

                    Image2.Visible = true;
                    btnAdicionarRecurso.Visible = true;

                    btnLinkBajarModeloFinanciero.Visible = false;
                    btnLinkSubirModeloFinanciero.Visible = false;

                    btnLinkGuiaLlenarModeloFinanciero.Visible = true;

                    btnGuardar.Visible = true;
                    pnlBotonAdicionarAporte.Visible = true;
                    pnlBotonAdicionarRecurso.Visible = true;
                    btnLinkBajarModeloFinanciero.Visible = false;
                    btnLinkSubirModeloFinanciero.Visible = false;
                }

                if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                    btnLinkVerModeloFinanciero.Visible = true;

                ((Button)CargarArchivos1.FindControl("btnSubirDocumento")).Command += new CommandEventHandler(SubirArchivo_Click);
                ((Button)CargarArchivos1.FindControl("btnCancelar")).Command += new CommandEventHandler(CancelarArchivo_Click);

                lblErrorRecursos.Text = "";
                CargarTiempoProyeccion();
                
                if (accion == "Editar")
                {
                    if (controlAccion == "Aporte")
                        CargarControlesEditarAporte();
                    else if (controlAccion == "Recurso")
                        CargarControlesEditarRecurso();
                }

                CargarTxtRecursosSolicitados();
                CargarListadoTipoRecurso();
                CargarRecursosCapital();

                CargarAportesEmprendedores();
                CargarListadoTipoAporte();

                CargarIngresosVentas();

                ObtenerDatosUltimaActualizacion();
            }

            string ruta = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual") + @"Proyecto\" + Math.Abs(Convert.ToInt32(codProyecto) / 2000) + @"\Proyecto_" + codProyecto + @"\";
            if (File.Exists(ruta + "FORMATOSFINANCIEROS.xls") || File.Exists(ruta + "FORMATOSFINANCIEROS" + codProyecto + ".xls") || File.Exists(ruta + "FORMATOSFINANCIEROS.xlsx") || File.Exists(ruta + "FORMATOSFINANCIEROS" + codProyecto + ".xlsx"))
            {
                pnlVerPlan.Visible = true;
            }
            else
            {
                pnlVerPlan.Visible = false;
            }

            //if (System.IO.Directory.Exists(ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual") + ConfigurationManager.AppSettings.Get("ModeloFinanciero") + "Emprendedor"))
            //{
            //    pnlVerPlan.Visible = true;
            //}
            //else
            //{
            //    pnlVerPlan.Visible = false;
            //}

            if(!chk_realizado.Checked && esMiembro)
            {
                div_post_it_2.Visible = true;
                Post_It1.Visible = true;
            }
        }

        /// <summary>
        /// Guardar un recurso solicitado
        /// </summary>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarRecursos(txtRecursosSolicitados, lblErrorRecursos))
            {         
                return; 
            }

            var query = (from p in consultas.Db.ProyectoFinanzasIngresos where p.CodProyecto == Convert.ToInt32(codProyecto) select new { p.CodProyecto }).ToList();
            
           if (query.Count == 0){
               Datos.ProyectoFinanzasIngreso datosNuevos = new ProyectoFinanzasIngreso()
                {
                    CodProyecto = Convert.ToInt32(codProyecto),
                    Recursos = Convert.ToByte(txtRecursosSolicitados.Text)  
                };

                var qryInsrt = string.Format("INSERT INTO [dbo].[ProyectoFinanzasIngresos] ([CodProyecto] ,[Recursos]) VALUES ({0},{1})", 
                    datosNuevos.CodProyecto, datosNuevos.Recursos);
                new Clases.genericQueries().executeQueryReader(qryInsrt,1);

                ObtenerDatosUltimaActualizacion();
            }
            else{                
                registroActual = getProyectoFinanzasIngreso();
                registroActual.Recursos =  Convert.ToByte(txtRecursosSolicitados.Text);
                var qryInsrt = string.Format("UPDATE [dbo].[ProyectoFinanzasIngresos] SET [Recursos] = {1} WHERE [CodProyecto] = {0}",
                registroActual.CodProyecto, registroActual.Recursos);
                new Clases.genericQueries().executeQueryReader(qryInsrt, 2);                
            }
            consultas.Db.SubmitChanges();

            if (txtRecursosSolicitados.Text.Trim() != "")
            {
                prActualizarTab(txtTab.ToString(), codProyecto);
                ObtenerDatosUltimaActualizacion();
            }
            
            Response.Redirect(Request.RawUrl);
                
        }

        /// <summary>
        /// Carga el numero de salarios minimos prestados dependiendo de el numero de empleos generados
        /// </summary>
        protected void CargarNumeroSMLVNV()
        {
            int numeroEmpleosNV = 0;
            codigoConvocatoria = 0;

            var qry = "select (select COUNT(*) from proyectoinsumo inner join ProyectoProductoInsumo on CodInsumo = Id_Insumo LEFT OUTER JOIN" + " proyectoempleomanoobra  on id_insumo=codmanoobra where codtipoinsumo=2 and codproyecto={0}) + " +
                  "(select COUNT(*) from proyectoempleocargo right OUTER JOIN proyectogastospersonal  on id_cargo=codcargo where codproyecto={0}) as Conteototal, " +
                  "(select COUNT(*) from proyectoinsumo  inner join ProyectoProductoInsumo  on CodInsumo = Id_Insumo LEFT OUTER JOIN proyectoempleomanoobra on id_insumo=codmanoobra" + " where codtipoinsumo=2 and codproyecto={0} and GeneradoPrimerAno  is not null and GeneradoPrimerAno!=0) + " +
                  "(select COUNT(*) from proyectoempleocargo right OUTER JOIN proyectogastospersonal on id_cargo=codcargo  where codproyecto={0} and GeneradoPrimerAno is not null" + " and GeneradoPrimerAno!=0) as ConteoAño";
                   var xct = new Clases.genericQueries().executeQueryReader(string.Format(qry, codProyecto));
                   DataTable dtEmpTtl = new DataTable();
                   dtEmpTtl.Load(xct, LoadOption.OverwriteChanges);
                   var total = dtEmpTtl.Rows[0]["Conteototal"].ToString();

                   numeroEmpleosNV = Convert.ToInt32(total);

            try
            {
                var queryConvoca = (from cp in consultas.Db.ConvocatoriaProyectos
                                    where cp.CodProyecto == Convert.ToInt32(codProyecto)
                                    select new { cp.CodConvocatoria }
                                        ).FirstOrDefault();
                if (queryConvoca != null)
                    codigoConvocatoria = queryConvoca.CodConvocatoria;
                else
                    codigoConvocatoria = 1;
            }
            catch
            {
                throw;                
            }

            ConsultarSalarioSMLVNV(1, numeroEmpleosNV, codigoConvocatoria);
            ConsultarSalarioSMLVNV(2, numeroEmpleosNV, codigoConvocatoria);
            ConsultarSalarioSMLVNV(3, numeroEmpleosNV, codigoConvocatoria);
            ConsultarSalarioSMLVNV(4, numeroEmpleosNV, codigoConvocatoria);
            ConsultarSalarioSMLVNV(5, numeroEmpleosNV, codigoConvocatoria);
            ConsultarSalarioSMLVNV(6, numeroEmpleosNV, codigoConvocatoria);

        }

        /// <summary>
        /// Consultar el numero de empleos generados
        /// </summary>
        /// <param name="regla"> Numero de regla de empleos generados </param>
        /// <param name="numeroEmpleosNV"> Numero de empleos generados </param>
        /// <param name="codigoConvocatoria"> Codigo de convocatoria </param>
        private void ConsultarSalarioSMLVNV(int regla, int numeroEmpleosNV, int codigoConvocatoria)
        {

            try
            {
                var queryRegla = (from p in consultas.Db.ConvocatoriaReglaSalarios where p.NoRegla == regla && p.CodConvocatoria == codigoConvocatoria select p).FirstOrDefault();

                if (queryRegla == null)
                    return;

                int empv1 = queryRegla.EmpleosGenerados1;
                int? empv11 = queryRegla.EmpleosGenerados2;
                string lista1 = queryRegla.ExpresionLogica;
                int Salmin1 = queryRegla.SalariosAPrestar;

                switch (lista1)
                {
                    case "=":
                        if (numeroEmpleosNV == empv1)
                            NumeroSMLVNV = Salmin1.ToString();
                        break;
                    case "<":
                        if (numeroEmpleosNV < empv1)
                            NumeroSMLVNV = Salmin1.ToString();
                        break;
                    case ">":
                        if (numeroEmpleosNV > empv1)
                            NumeroSMLVNV = Salmin1.ToString();

                        break;
                    case "<=":
                        if (numeroEmpleosNV <= empv1)
                            NumeroSMLVNV = Salmin1.ToString();
                        break;
                    case ">=":
                        if (numeroEmpleosNV >= empv1)
                            NumeroSMLVNV = Salmin1.ToString();
                        break;
                    case "Entre":
                        if (numeroEmpleosNV >= empv1 && numeroEmpleosNV <= empv11)
                            NumeroSMLVNV = Salmin1.ToString();
                        break;
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// Validar la información de un recurso.
        /// </summary>
        /// <param name="campo"> Campo a validar </param>
        /// <param name="lblError"> Label para mostrar error </param>
        /// <returns> Si es valido o no </returns>
        private bool ValidarRecursos(TextBox campo, Label lblError)
        {
            if (codigoConvocatoria == 0)
            {
                lblError.Text = "Proyecto aún no pertenece a ninguna Convocatoria.";
                return false;

            } else if (campo.Text == "" || campo.Text == "0")
            {
                lblError.Text = "Debe ingresar los Recursos solicitados al Fondo Emprender";
                return false;
            }
            else
            {
                int respeusta = 0;
                if (!int.TryParse(campo.Text, out respeusta))
                {
                    lblError.Text = "El valor debe ser numérico";
                    return false;
                }
                else if (Convert.ToInt32(campo.Text) > Convert.ToInt32(NumeroSMLVNV))
                {                                        
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Los Recursos solicitados al Fondo Emprender no pueden ser mayores a  " + Convert.ToInt32(NumeroSMLVNV) + " smlv.')", true);
                    return false;
                }
            }
            lblError.Text = "";
            return true;
        }
        
        /// <summary>
        /// Cargar los datos de ingreso de ventas del plan de negocio
        /// </summary>
        private void CargarIngresosVentas()
        {                                   
            String txtSQL;
            DataTable rsProducto = new DataTable();
            DataTable datos = new DataTable();
            //DataTable rsUnidades = new DataTable();
            double[] totalPt = new double[txtTiempoProyeccion + 1];
            double[] totalIvaPt = new double[txtTiempoProyeccion + 1];
            int[] ivas = new int[txtTiempoProyeccion];
            string errorMessage = string.Empty;

            txtSQL = " select id_producto, nomproducto, porcentajeiva " +
                     " from proyectoproducto " +
                     " where codproyecto = " + codProyecto;

            rsProducto = consultas.ObtenerDataTable(txtSQL, "text");
            
            datos.Columns.Add("Producto");

            //Dependiendo del tiempo de proyección 
            for (int i = 1; i <= txtTiempoProyeccion; i++)
            { datos.Columns.Add("Año " + i); }

            foreach (DataRow row in rsProducto.Rows)
            {
                DataRow dr = datos.NewRow();
                dr["producto"] = row["nomproducto"].ToString();

                txtSQL = " select sum(unidades) as unidades, precio, sum(unidades)*precio as total, ano " +
                         " from proyectoproductounidadesventas u, proyectoproductoprecio p " +
                         " where p.codproducto=u.codproducto and periodo=ano and p.codproducto = " + row["id_Producto"].ToString() +
                         " group by ano,precio order by ano";

                var rsUnidades = consultas.ObtenerDataTable(txtSQL, "text");

                int incr = 1;
                foreach (DataRow row_unidades in rsUnidades.Rows)
                {
                    try
                    {
                        double unidades = Double.Parse(row_unidades["Unidades"].ToString());
                        double precio = Double.Parse(row_unidades["Precio"].ToString());
                        double total2 = double.Parse(row_unidades["total"].ToString());
                        if (incr == 0)
                        {                           
                            dr["Producto"] = (unidades * precio);
                        }
                        else
                        {
                            //dr["Año " + incr.ToString()] = (unidades * precio).ToString("0,0.00", CultureInfo.CreateSpecificCulture("es-CO"));
                            //dr["Año " + incr.ToString()] = (unidades * precio).ToString("0,0.00", CultureInfo.InvariantCulture);
                            dr["Año " + incr.ToString()] = total2.ToString("0,0.00", CultureInfo.InvariantCulture);
                        }

                        double total = Double.Parse(row_unidades["total"].ToString());
                        double anio = Double.Parse(row_unidades["ano"].ToString());
                        double porcentajeIva = Double.Parse(row["porcentajeiva"].ToString());
                        totalPt[incr] += total;
                      
                        totalIvaPt[incr] += (total * porcentajeIva / 100);
                      
                        incr++;
                    }
                    catch (Exception ex) { errorMessage = ex.Message; }
                }
                
                datos.Rows.Add(dr);
            }
           
            DataRow drTotal = datos.NewRow();
            DataRow drTotalIva = datos.NewRow();
            DataRow drTotalMasIva = datos.NewRow();
            drTotal["Producto"] = "Total";
            drTotalIva["Producto"] = "Iva";
            drTotalMasIva["Producto"] = "Total con Iva";

            for (int i = 0; i <= txtTiempoProyeccion + 1; i++)
            {
                if (i != 0)
                {
                    if (i <= txtTiempoProyeccion)
                    {                        
                        //drTotal["Año " + i] = (totalPt[i] ).ToString("0,0.00", CultureInfo.CreateSpecificCulture("es-CO"));
                        //drTotalIva["Año " + i] = (totalIvaPt[i] ).ToString("N2", CultureInfo.CreateSpecificCulture("es-CO"));
                        //drTotalMasIva["Año " + i] = ((totalPt[i] + totalIvaPt[i]) ).ToString("0,0.00", CultureInfo.CreateSpecificCulture("es-CO"));
                        drTotal["Año " + i] = (totalPt[i]).ToString("0,0.00", CultureInfo.InvariantCulture);
                        drTotalIva["Año " + i] = (totalIvaPt[i]).ToString("N2", CultureInfo.InvariantCulture);
                        drTotalMasIva["Año " + i] = ((totalPt[i] + totalIvaPt[i])).ToString("0,0.00", CultureInfo.InvariantCulture);
                    }
                }
            }
           
            datos.Rows.Add(drTotal);
            datos.Rows.Add(drTotalIva);
            datos.Rows.Add(drTotalMasIva);

            //Cargar los datos en el grid de ingreso de ventas
            gw_IngresosVentas.DataSource = datos;
            gw_IngresosVentas.DataBind();
            PintarFilasGrid(gw_IngresosVentas, 0, new string[] { "Total", "Iva", "Total con Iva" });
            
        }

        /// <summary>
        /// Cargar el tiempo de proyección
        /// </summary>
        protected void CargarTiempoProyeccion()
        {
            try
            {
                var query = (from p in consultas.Db.ProyectoMercadoProyeccionVentas
                             where p.CodProyecto == Convert.ToInt32(codProyecto)
                             select new { p.TiempoProyeccion }).First();
                txtTiempoProyeccion = Convert.ToInt32(query.TiempoProyeccion);
            }
            catch
            {
                txtTiempoProyeccion = 3;
            }
        }

        /// <summary>
        /// Obtiene los datos de ingresos de finanzas del plan de negocio
        /// </summary>
        /// <returns> Listado de ingresos de finanzas del plan de negocio </returns>
        private ProyectoFinanzasIngreso getProyectoFinanzasIngreso()
        {
            var query = (from p in consultas.Db.ProyectoFinanzasIngresos where p.CodProyecto == Convert.ToInt32(codProyecto) select p).First();

            return query;
        }       

        /// <summary>
        /// Cargar el listado de tipos de aporte
        /// </summary>
        protected void CargarListadoTipoAporte()
        {
            ddlTipoAporte.Items.Clear();
            ddlTipoAporte.Items.Add(new ListItem() { Value = Constantes.CONST_Dinero, Text = Constantes.CONST_Dinero });
            ddlTipoAporte.Items.Add(new ListItem() { Value = Constantes.CONST_Bien, Text = Constantes.CONST_Bien });
            ddlTipoAporte.Items.Add(new ListItem() { Value = Constantes.CONST_Servicio, Text = Constantes.CONST_Servicio });
        }

        /// <summary>
        /// Cargar los aportes de emprendedores del plan de negocio
        /// </summary>
        protected void CargarAportesEmprendedores()
        {
            var query = (from p in consultas.Db.ProyectoAportes
                         where p.CodProyecto == Convert.ToInt32(this.codProyecto)
                         orderby p.TipoAporte, p.Nombre ascending
                         select new { p.Id_Aporte, p.Nombre, p.Valor, p.TipoAporte, p.Detalle });

            DataTable datos = new DataTable();
            datos.Columns.Add("nombre");
            datos.Columns.Add("valor");
            datos.Columns.Add("detalle");
            datos.Columns.Add("");
            datos.Columns.Add("Id_Aporte");

            string aporteActual = "";
            double total = 0;
            foreach (var item in query)
            {
                if (aporteActual != item.TipoAporte)
                {
                    DataRow drTitulo = datos.NewRow();
                    drTitulo["valor"] = string.Empty;
                    drTitulo["nombre"] = item.TipoAporte;
                    aporteActual = item.TipoAporte;
                    datos.Rows.Add(drTitulo);
                }

                DataRow dr = datos.NewRow();
                dr["nombre"] = item.Nombre;
                dr["valor"] = item.Valor.ToString("0,0.00", CultureInfo.InvariantCulture); ;
                dr["detalle"] = item.Detalle;
                dr["Id_Aporte"] = item.Id_Aporte;
                total += Convert.ToDouble(item.Valor);
                datos.Rows.Add(dr);
            }

            DataRow drTotal = datos.NewRow();
            drTotal["nombre"] = "Total";
            drTotal["valor"] = total.ToString("0,0.00", CultureInfo.InvariantCulture);
            datos.Rows.Add(drTotal);

            gw_AporteEmprendedores.DataSource = datos;
            gw_AporteEmprendedores.DataBind();          
        }
        /// <summary>
        /// Mostrar el formulario para crear un nuevo aporte
        /// </summary>
        protected void btnAdicionarAporte_Click(object sender, EventArgs e)
        {
            pnlPrincipal.Visible = false;
            pnlAporte.Visible = true;
            btnCrearAporte.Text = "Crear";

        }
        
        /// <summary>
        /// Eliminar un aporte
        /// </summary>        
        protected void gw_AporteEmprendedores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                string idAporte = e.CommandArgument.ToString();
                consultas.Db.ExecuteCommand("delete from ProyectoAporte where Id_Aporte={0}", Convert.ToInt32(idAporte));
            }
            if (e.CommandName.Equals("Editar"))
            {
                accion = "Editar";
                HttpContext.Current.Session["Accion"] = accion;
                controlAccion = "Aporte";
                HttpContext.Current.Session["controlAccion"] = controlAccion;
                idAporteEdita = e.CommandArgument.ToString();
                HttpContext.Current.Session["idAporte"] = idAporteEdita;
                CargarControlesEditarAporte();
            }
            CargarTxtRecursosSolicitados();
            CargarListadoTipoRecurso();
            CargarRecursosCapital();
            CargarAportesEmprendedores();
            CargarListadoTipoAporte();
            CargarIngresosVentas();

            ObtenerDatosUltimaActualizacion();
        }

        /// <summary>
        /// Cancelar la creación de un nuevo aporte
        /// </summary>
        protected void btnCancelarAporte_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["Accion"] = string.Empty;
            HttpContext.Current.Session["controlAccion"] = string.Empty;
            HttpContext.Current.Session["idAporte"] = string.Empty;
            pnlPrincipal.Visible = true;
            pnlAporte.Visible = false;

            txtNombreAporte.Text = string.Empty;
            txtValorAportes.Text = "0";
            txtDetalleAporte.Text = string.Empty;

            CargarTxtRecursosSolicitados();
            CargarListadoTipoRecurso();
            CargarRecursosCapital();

            CargarAportesEmprendedores();
            CargarListadoTipoAporte();

            CargarIngresosVentas();

            ObtenerDatosUltimaActualizacion();
            Response.Redirect(Request.RawUrl);
        }

        /// <summary>
        /// Cargar los controles para editar aportes
        /// </summary>
        private void CargarControlesEditarAporte()
        {
            pnlPrincipal.Visible = false;
            pnlAporte.Visible = true;

            var query = (from p in consultas.Db.ProyectoAportes where p.CodProyecto == Convert.ToInt32(codProyecto) && p.Id_Aporte == Convert.ToInt32(idAporteEdita) select p ).First();
            
            txtNombreAporte.Text = query.Nombre;
            txtValorAportes.Text = query.Valor.ToString("0,0.00", CultureInfo.InvariantCulture);
            ddlTipoAporte.SelectedValue = query.TipoAporte;
            txtDetalleAporte.Text = query.Detalle;
            btnCrearAporte.Text = "Actualizar";
        }

        /// <summary>
        /// Crear un nuevo aporte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCrearAporte_Click(object sender, EventArgs e)
        {
            try
            {
                FieldValidate.ValidateString("Nombre del aporte", txtNombreAporte.Text, true, 100);
                FieldValidate.ValidateNumeric("Valor del aporte", txtValorAportes.Text, true);
                FieldValidate.ValidateString("Tipo de aporte", ddlTipoAporte.SelectedValue, true, 30);
                FieldValidate.ValidateString("Detalle", txtDetalleAporte.Text, false, 1000);

                if (accion == "Editar")
                {                   
                    var query = (from p in consultas.Db.ProyectoAportes
                                 where p.CodProyecto == Convert.ToInt32(codProyecto) &&
                                 p.Id_Aporte == Convert.ToInt32(idAporteEdita)
                                 select p
                          ).First();

                    query.Nombre = txtNombreAporte.Text;
                    query.Valor = Convert.ToDecimal(txtValorAportes.Text.Replace(",", "").Replace(".", ","));
                    query.TipoAporte = ddlTipoAporte.SelectedValue;
                    query.Detalle = txtDetalleAporte.Text;
                    //consultas.Db.ExecuteCommand(UsuarioActual());
                    consultas.Db.SubmitChanges();
                }
                else
                {                    
                    Datos.ProyectoAporte datosNuevos = new ProyectoAporte()
                    {
                        CodProyecto = Convert.ToInt32(codProyecto),
                        Nombre = txtNombreAporte.Text,
                        Valor = Convert.ToDecimal(txtValorAportes.Text.Replace(",", "").Replace(".", ",")),
                        TipoAporte = ddlTipoAporte.SelectedValue,
                        Detalle = txtDetalleAporte.Text
                    };

                    consultas.Db.ProyectoAportes.InsertOnSubmit(datosNuevos);
                    consultas.Db.SubmitChanges();
                }

                HttpContext.Current.Session["Accion"] = string.Empty;
                HttpContext.Current.Session["controlAccion"] = string.Empty;
                HttpContext.Current.Session["idAporte"] = string.Empty;
                pnlPrincipal.Visible = true;
                pnlAporte.Visible = false;

                txtNombreAporte.Text = string.Empty;
                txtValorAportes.Text = "";
                txtDetalleAporte.Text = string.Empty;

                CargarTxtRecursosSolicitados();
                CargarListadoTipoRecurso();
                CargarRecursosCapital();

                CargarAportesEmprendedores();
                CargarListadoTipoAporte();

                CargarIngresosVentas();

                ObtenerDatosUltimaActualizacion();
                Response.Redirect(Request.RawUrl,true);
            }
            catch (ApplicationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + ex.Message + "');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }
        }      

        /// <summary>
        /// Cargar los tipos de recursos
        /// </summary>
        protected void CargarListadoTipoRecurso()
        {
            ddlTipoRecurso.Items.Clear();                      
            ddlTipoRecurso.Items.Add(new ListItem() { Value = Constantes.CONST_Credito, Text = Constantes.CONST_Credito });
            ddlTipoRecurso.Items.Add(new ListItem() { Value = Constantes.CONST_Donancion, Text = Constantes.CONST_Donancion });
        }

        /// <summary>
        /// Cargar los recursos solicitados del capital
        /// </summary>
        protected void CargarTxtRecursosSolicitados()
        {

            try
            {
                var query = (from p in consultas.Db.ProyectoFinanzasIngresos
                             where p.CodProyecto == Convert.ToInt32(codProyecto)
                             select new { p.Recursos }).First();
                txtRecursosSolicitados.Text = query.Recursos.ToString();
            }
            catch
            {
                txtRecursosSolicitados.Text = "";
            }

        }

        /// <summary>
        /// Cargar los recursos de capital
        /// </summary>
        protected void CargarRecursosCapital()
        {
            var query = (from p in consultas.Db.ProyectoRecursos where p.CodProyecto == Convert.ToInt32(this.codProyecto) orderby p.Tipo ascending select new { p.Id_Recurso, p.Tipo, p.Cuantia, p.Plazo, p.Formapago, p.Interes, p.Destinacion });

            DataTable datos = new DataTable();
            datos.Columns.Add("cuantia");
            datos.Columns.Add("plazo");
            datos.Columns.Add("formaPago");
            datos.Columns.Add("intereses");
            datos.Columns.Add("destinacion");
            datos.Columns.Add("Id_Recurso");

            string tipoActual = "";
            double total = 0;
            foreach (var item in query)
            {
                if (tipoActual != item.Tipo)
                {
                    DataRow drTitulo = datos.NewRow();
                    drTitulo["cuantia"] = item.Tipo;
                    tipoActual = item.Tipo;
                    datos.Rows.Add(drTitulo);
                }

                DataRow dr = datos.NewRow();
                dr["cuantia"] = item.Cuantia.ToString("0,0.00", CultureInfo.InvariantCulture); ;
                dr["plazo"] = item.Plazo;
                dr["formaPago"] = item.Formapago;
                dr["intereses"] = item.Interes;
                dr["destinacion"] = item.Destinacion;
                dr["Id_Recurso"] = item.Id_Recurso;
                total += Convert.ToDouble(item.Cuantia);
                datos.Rows.Add(dr);

            }

            DataRow drTotal = datos.NewRow();
            drTotal["cuantia"] = "Total";
            drTotal["plazo"] = total.ToString("0,0.00", CultureInfo.InvariantCulture);
            datos.Rows.Add(drTotal);

            gw_RecursosCapital.DataSource = datos;
            gw_RecursosCapital.DataBind();         
        }

        /// <summary>
        /// Mostrar un formulario para crear un nuevo recurso.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdicionarRecurso_Click(object sender, EventArgs e)
        {
            pnlPrincipal.Visible = false;
            pnlRecurso.Visible = true;
            btnCrearRecurso.Text = "Crear";
        }

        /// <summary>
        /// Eliminar un recurso
        /// </summary>
        protected void gw_RecursosCapital_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                string idRecurso = e.CommandArgument.ToString();
                consultas.Db.ExecuteCommand("delete from ProyectoRecurso where Id_Recurso={0}", Convert.ToInt32(idRecurso));                
            }
            if (e.CommandName.Equals("Editar"))
            {
                accion = "Editar";
                HttpContext.Current.Session["Accion"] = accion;
                controlAccion = "Recurso";
                HttpContext.Current.Session["controlAccion"] = controlAccion;
                idRecursoEdita = e.CommandArgument.ToString();
                HttpContext.Current.Session["idRecurso"] = idRecursoEdita;
                CargarControlesEditarRecurso();
            }
            CargarTxtRecursosSolicitados();
            CargarListadoTipoRecurso();
            CargarRecursosCapital();

            CargarAportesEmprendedores();
            CargarListadoTipoAporte();

            CargarIngresosVentas();

            ObtenerDatosUltimaActualizacion();
            Response.Redirect(Request.RawUrl);
        }

        /// <summary>
        /// Cancelar la creación de un nuevo recurso
        /// </summary>
        protected void btnCancelarRecurso_Click(object sender, EventArgs e)
        {           
            HttpContext.Current.Session["Accion"] = string.Empty;
            HttpContext.Current.Session["controlAccion"] = string.Empty;
            HttpContext.Current.Session["idAporte"] = string.Empty;
            pnlPrincipal.Visible = true;
            pnlRecurso.Visible = false;

            txtCuantiaRecurso.Text = "0";
            txtPlazoRecurso.Text = string.Empty;
            txtFormaPagoRecurso.Text = string.Empty;
            txtInteresRecurso.Text = "0";
            txtDestinacionRecurso.Text = string.Empty;

            CargarTxtRecursosSolicitados();
            CargarListadoTipoRecurso();
            CargarRecursosCapital();

            CargarAportesEmprendedores();
            CargarListadoTipoAporte();

            CargarIngresosVentas();

            ObtenerDatosUltimaActualizacion();
            Response.Redirect(Request.RawUrl);
        }

        /// <summary>
        /// Mostrar los controles para cargar nuevos recursos
        /// </summary>
        private void CargarControlesEditarRecurso()
        {
            pnlPrincipal.Visible = false;
            pnlRecurso.Visible = true;

            var query = (from p in consultas.Db.ProyectoRecursos where p.CodProyecto == Convert.ToInt32(codProyecto) && p.Id_Recurso == Convert.ToInt32(idRecursoEdita) select p ).First();
  
            ddlTipoRecurso.SelectedValue = query.Tipo;
            txtCuantiaRecurso.Text = query.Cuantia.ToString("0,0.00", CultureInfo.InvariantCulture);
            txtPlazoRecurso.Text = query.Plazo;
            txtFormaPagoRecurso.Text = query.Formapago;
            txtInteresRecurso.Text = query.Interes.ToString(); ;
            txtDestinacionRecurso.Text = query.Destinacion;
            btnCrearRecurso.Text = "Actualizar";
        }

        /// <summary>
        /// Crear nuevo recurso a finanzas
        /// </summary>        
        protected void btnCrearRecurso_Click(object sender, EventArgs e)
        {
            try
            {
                FieldValidate.ValidateString("Tipo de recurso", ddlTipoRecurso.SelectedValue, true, 10);
                FieldValidate.ValidateNumeric("Cuantia", txtCuantiaRecurso.Text, true);
                FieldValidate.ValidateString("Plazo", txtPlazoRecurso.Text, false, 30);
                FieldValidate.ValidateString("Forma de pago", txtFormaPagoRecurso.Text, false, 50);
                FieldValidate.ValidateNumeric("Interes", txtInteresRecurso.Text, true);
                FieldValidate.ValidateString("Destinación", txtDestinacionRecurso.Text, false, 1000);

                if (accion == "Editar")
                {
                    var query = (from p in consultas.Db.ProyectoRecursos where p.CodProyecto == Convert.ToInt32(codProyecto) && p.Id_Recurso == Convert.ToInt32(idRecursoEdita) select p ).First();

                    query.Tipo = ddlTipoRecurso.SelectedValue;
                    query.Cuantia = Convert.ToDecimal(txtCuantiaRecurso.Text.Replace(",", "").Replace(".", ","));
                    query.Plazo = txtPlazoRecurso.Text;
                    query.Formapago = txtFormaPagoRecurso.Text;
                    query.Interes = Convert.ToInt16(txtInteresRecurso.Text.Replace(".", ""));
                    query.Destinacion = txtDestinacionRecurso.Text;

                    consultas.Db.SubmitChanges();                
                }
                else
                {                
                    Datos.ProyectoRecurso datosNuevos = new ProyectoRecurso()
                    {
                        CodProyecto = Convert.ToInt32(codProyecto),
                        Tipo = ddlTipoRecurso.SelectedValue,
                        Cuantia = Convert.ToDecimal(txtCuantiaRecurso.Text.Replace(",", "").Replace(".", ",")),
                        Plazo = txtPlazoRecurso.Text,
                        Formapago = txtFormaPagoRecurso.Text,
                        Interes = Convert.ToInt16(txtInteresRecurso.Text.Replace(".", "")),
                        Destinacion = txtDestinacionRecurso.Text
                    };

                    consultas.Db.ProyectoRecursos.InsertOnSubmit(datosNuevos);
                    consultas.Db.SubmitChanges();                 
                }

                HttpContext.Current.Session["Accion"] = string.Empty;
                HttpContext.Current.Session["controlAccion"] = string.Empty;
                HttpContext.Current.Session["idAporte"] = string.Empty;
                pnlPrincipal.Visible = true;
                pnlRecurso.Visible = false;

                txtCuantiaRecurso.Text = "0";
                txtPlazoRecurso.Text = string.Empty;
                txtFormaPagoRecurso.Text = string.Empty;
                txtInteresRecurso.Text = "0";
                txtDestinacionRecurso.Text = string.Empty;

                CargarTxtRecursosSolicitados();
                CargarListadoTipoRecurso();
                CargarRecursosCapital();

                CargarAportesEmprendedores();
                CargarListadoTipoAporte();

                CargarIngresosVentas();

                ObtenerDatosUltimaActualizacion();
                Response.Redirect(Request.RawUrl);
            }
            catch (ApplicationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Advertencia : " + ex.Message + "');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }
        }        

        /// <summary>
        /// Descargar archivo de modelo financiero
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLinkBajarModeloFinanciero_Click(object sender, EventArgs e)
        {            
            ClientScriptManager cm = this.ClientScript;

            if (System.IO.Directory.Exists(ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual") + ConfigurationManager.AppSettings.Get("ModeloFinanciero") + "Emprendedor"))
            {
                DescargarArchivo(ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual") + ConfigurationManager.AppSettings.Get("ModeloFinanciero") + @"Emprendedor\FORMATOSFINANCIEROS.xls"); 
            }
            else
            {
                cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('La ruta relacionada no existe.');</script>");
                return;
            }
        }

        /// <summary>
        /// Mostrar el formulario de subir el modelo financiero
        /// </summary>
        protected void btnLinkSubirModeloFinanciero_Click(object sender, EventArgs e)
        {
            pnlPrincipal.Style.Add("display", "none");
            pnlCargueDocumento.Visible = true;
            string ruta = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual") +@"Proyecto\"+ Math.Abs(Convert.ToInt32(codProyecto) / 2000) + @"\Proyecto_" + codProyecto + @"\";
            string archivo = @"\FORMATOSFINANCIEROS" + codProyecto;
            CargarArchivos1.show(new string[] { "xls" }, ruta, archivo);
        }

        /// <summary>
        /// Ver el archivo del modelo financiero
        /// </summary>
        protected void btnLinkVerModeloFinanciero_Click(object sender, EventArgs e)
        {
            ClientScriptManager cm = this.ClientScript;
            string ruta = ConfigurationManager.AppSettings.Get("RutaWebSite") + ConfigurationManager.AppSettings.Get("DirVirtual2") + @"Proyecto/" + Math.Abs(Convert.ToInt32(codProyecto) / 2000) + @"\Proyecto_" + codProyecto + @"\FORMATOSFINANCIEROS" + codProyecto + ".xls";
 
            if (File.Exists(ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual") + @"Proyecto\" + Math.Abs(Convert.ToInt32(codProyecto) / 2000) + @"\Proyecto_" + codProyecto + @"\FORMATOSFINANCIEROS" + codProyecto + ".xls"))
            {
                DescargarArchivo(ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual") + @"Proyecto\" + Math.Abs(Convert.ToInt32(codProyecto) / 2000) + @"\Proyecto_" + codProyecto + @"\FORMATOSFINANCIEROS" + codProyecto + ".xls");
            }
            else
            {
                cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('El archivo no existe.');</script>");
                return;
            }
        }

        /// <summary>
        /// Descargar la guía para llenar modelo financiero.
        /// </summary>
        protected void btnLinkGuiaLlenarModeloFinanciero_Click(object sender, EventArgs e)
        {           
            ClientScriptManager cm = this.ClientScript;
             if (System.IO.Directory.Exists(ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual") + ConfigurationManager.AppSettings.Get("ModeloFinanciero") + "Emprendedor"))
            {
                 DescargarArchivo(ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual") + ConfigurationManager.AppSettings.Get("ModeloFinanciero") + @"Emprendedor\GuiaModelo.doc");
            }
             else
            {
                cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('La ruta relacionada no existe.');</script>");
                return;
            }
        }        

        /// <summary>
        /// Subir el formato de modelo financiero al sistema de archivo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SubirArchivo_Click(object sender, CommandEventArgs e)
        {            
            ClientScriptManager cm = this.ClientScript;

            string rutaHttpDestino = "Documentos/Proyecto/" + Math.Abs(Convert.ToInt32(codProyecto) / 2000) + "/" + "Proyecto_" + codProyecto + "/" + "FORMATOSFINANCIEROS" + codProyecto + ".xls";

            Controles.RespuestaCargue respuesta = CargarArchivos1.Resultado();
            if (respuesta.Mensaje == "OK")
            {               
                var query = (from d in consultas.Db.Documentos where d.CodTab == txtTab && d.CodProyecto == Convert.ToInt32(codProyecto) && d.NomDocumento.Contains("Formato Financieros") select d );
                if (query.ToList().Count == 0)
                {
                    Datos.Documento datosNuevos = new Documento()
                    {
                        CodProyecto = Convert.ToInt32(codProyecto),
                        NomDocumento = "Formato Financieros",
                        CodDocumentoFormato = 3,
                        URL = rutaHttpDestino,
                        Comentario = "Formato Financieros",
                        Fecha = DateTime.Now,
                        CodContacto = usuario.IdContacto,
                        CodTab = (Int16?)txtTab
                    };

                    consultas.Db.Documentos.InsertOnSubmit(datosNuevos);
                    consultas.Db.SubmitChanges();
                }
                else
                {
                    var queryUpdate = query.First();
                    queryUpdate.Fecha = DateTime.Now;
                    queryUpdate.URL = rutaHttpDestino;
                    queryUpdate.CodDocumentoFormato = 3;
                    queryUpdate.Borrado = false;
                    queryUpdate.CodContacto = usuario.IdContacto;
                    consultas.Db.SubmitChanges();

                }
                pnlPrincipal.Visible = true;
                pnlCargueDocumento.Visible = false;                
            }
            else
            {
                cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('Atención: " + respuesta.Mensaje + "');</script>");
                return;
            }
        }

        /// <summary>
        /// Cancelar subir archivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CancelarArchivo_Click(object sender, CommandEventArgs e)
        {
            pnlPrincipal.Visible = true;
            pnlCargueDocumento.Visible = false;
        }        

        /// <summary>
        /// Mostrar datos de proyección de ventas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gw_IngresosVentas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                for (int i = 1; i < txtTiempoProyeccion + 1; i++)
                { e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right; }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                for (int i = 1; i < txtTiempoProyeccion + 1; i++)
                { e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right; }
            }
        }

        /// <summary>
        /// Primera letra en mayuscula de un string
        /// </summary>
        /// <param name="s">String a procesar</param>
        /// <returns> String con primera Letra en mayuscula </returns>
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
       /// Obtiene los datos de la ultima actualización de la pestaña
       /// </summary>
        private void ObtenerDatosUltimaActualizacion()
        {           
            String txtSQL;
            DateTime fecha = new DateTime();
            DataTable tabla = new DataTable();
            bool bRealizado = false;
            bool EsMiembro = false;
            Int32 numPostIt = 0;
            Int32 CodigoEstado = 0;

            try
            {
                //Consultar si es miembro.
                EsMiembro = fnMiembroProyecto(usuario.IdContacto, codProyecto);

                //Obtener número "numPostIt".
                numPostIt = Obtener_numPostIt();

                //Consultar el "Estado" del proyecto.
                CodigoEstado = CodEstado_Proyecto(txtTab.ToString(), codProyecto, "");

                                
                txtSQL = " SELECT CodContacto, CodRol From ProyectoContacto " +
                         " Where CodProyecto = " + codProyecto + " And CodContacto = " + usuario.IdContacto +
                         " and inactivo=0 and FechaInicio<=getdate() and FechaFin is null ";

                //Asignar variables a DataTable.
                var rs = consultas.ObtenerDataTable(txtSQL, "text");

                //Crear la variable de sesión.
                if (rs.Rows.Count > 0) { HttpContext.Current.Session["CodRol"] = rs.Rows[0]["CodRol"].ToString(); }
                else { HttpContext.Current.Session["CodRol"] = ""; }

                //Destruir la variable.
                rs = null;
                
                //Consultar los datos a mostrar en los campos correspondientes a la actualización.
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

                //Determinar si el usuario actual puede o no "chequear" la actualización.                
                if (!(EsMiembro && numPostIt == 0 && ((HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolAsesorLider.ToString() && CodigoEstado == Constantes.CONST_Inscripcion) || (CodigoEstado == Constantes.CONST_Evaluacion && HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolEvaluador.ToString() && es_bNuevo(codProyecto)))) || lbl_nombre_user_ult_act.Text.Trim() == "")
                { chk_realizado.Enabled = false; }

                //Mostrar el botón de guardar.               
                if (EsMiembro && numPostIt == 0 && lbl_nombre_user_ult_act.Text != "" && (HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolAsesorLider.ToString() && CodigoEstado == Constantes.CONST_Inscripcion) || (HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolEvaluador.ToString() && CodigoEstado == Constantes.CONST_Evaluacion && es_bNuevo(codProyecto)))
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
                if (EsMiembro && HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolEmprendedor.ToString() && !bRealizado)
                {
                    tabla_docs.Visible = true;
                }

                if(usuario.CodGrupo == Constantes.CONST_Asesor)
                {
                    visibleGuardar = true;
                }
                
                visibleGuardar = visibleGuardar || Constantes.CONST_CoordinadorEvaluador == usuario.CodGrupo;
                
                //Destruir variables.
                tabla = null;
                txtSQL = null;

            }
            catch (Exception)
            {               
                return;
            }
        }

        /// <summary>
        /// Obtener el numero de postit
        /// </summary>
        /// <returns> Numero de postit </returns>
        private int Obtener_numPostIt()
        {
            Int32 numPosIt = 0;
           
            var query = from tur in consultas.Db.TareaUsuarioRepeticions
                        from tu in consultas.Db.TareaUsuarios
                        from tp in consultas.Db.TareaProgramas
                        where tp.Id_TareaPrograma == tu.CodTareaPrograma
                        && tu.Id_TareaUsuario == tur.CodTareaUsuario
                        && tu.CodProyecto == Convert.ToInt32(codProyecto)
                        && tp.Id_TareaPrograma == Constantes.CONST_PostIt
                        && tur.FechaCierre == null
                        select tur;

            numPosIt = query.Count();

            return numPosIt;
        }

        /// <summary>
        /// Actualizar los datos de ultima actualización de la pestaña
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_guardar_ultima_actualizacion_Click(object sender, EventArgs e)
        {
            int flag = 0;
            flag = Marcar(txtTab.ToString(), codProyecto, "", chk_realizado.Checked);
            CargarTxtRecursosSolicitados();
            CargarListadoTipoRecurso();
            CargarRecursosCapital();
            CargarAportesEmprendedores();
            CargarListadoTipoAporte();
            CargarIngresosVentas();
            ObtenerDatosUltimaActualizacion();
            
            if (flag == 1)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "refreshParent", "window.top.location.reload();", true);
            }
            else
            {
                Response.Redirect(Request.RawUrl);
            }    
        }

        /// <summary>
        /// Nuevo documento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Session["TabInvoca"] = "FinanzasIngresos";
            HttpContext.Current.Session["CodProyecto"] = codProyecto;
            HttpContext.Current.Session["txtTab"] = txtTab.ToString();
            HttpContext.Current.Session["Accion"] = "Nuevo";
            Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        /// <summary>
        /// Ver documentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            Session["TabInvoca"] = "FinanzasIngresos";
            HttpContext.Current.Session["CodProyecto"] = codProyecto;
            HttpContext.Current.Session["txtTab"] = txtTab.ToString();
            HttpContext.Current.Session["Accion"] = "Vista";
            Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        /// <summary>
        /// Eliminar emprendedores.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gw_AporteEmprendedores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var btn = e.Row.FindControl("btn_BorrarEmprendedor") as ImageButton;

                var lnk = (LinkButton)e.Row.FindControl("lnkNombre");

                if (!(esMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor && !bRealizado))
                {
                    btn.Style.Add(HtmlTextWriterStyle.Display, "none");
                    btn.Enabled = false;
                    btn.Visible = false;
                    lnk.Enabled = false;
                }

                if (lnk.Text.Equals("Total") || string.IsNullOrEmpty(e.Row.Cells[2].Text) || e.Row.Cells[2].Text.Equals("&nbsp;"))
                {
                    btn.Enabled = false;
                    btn.Visible = false;
                    lnk.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Eliminar recursos de capital.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gw_RecursosCapital_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var btn = e.Row.FindControl("btn_BorrarRecursosCapital") as ImageButton;

                var lnk = (LinkButton)e.Row.FindControl("lnkCuantia");

                if (!(esMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor && !bRealizado))
                {
                    btn.Style.Add(HtmlTextWriterStyle.Display, "none");
                    btn.Enabled = false;
                    btn.Visible = false;
                    lnk.Enabled = false;
                }

                if (lnk.Text.Equals("Total"))
                {
                    btn.Enabled = false;
                    btn.Visible = false;
                    lnk.Enabled = false;
                }
            }
        }
    }

    public class BORespuestaDetalleProducto
    {
        public decimal Unidades { get; set; }
        public decimal Precio { get; set; }
        public decimal Total { get; set; }
        public int anio { get; set; }
    }
}
