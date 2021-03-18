using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using System.Globalization;
using Fonade.Clases;

namespace Fonade.FONADE.Proyecto
{
    public partial class PProyectoFinanzasEgreso : Negocio.Base_Page
    {
        public string codProyecto;
        public string codConvocatoria;
        public int txtTab = Constantes.CONST_Egresos;
        public string idInversionEdita;
        public string accion;
        public int txtTiempoProyeccion;
        private Datos.ProyectoFinanzasEgreso registroActual;
        private Datos.ProyectoInversion registroInversion;
        public Boolean esMiembro;
        int flag = 0;
        /// <summary>
        /// Determina si está o no "realizado"...
        /// </summary>
        public Boolean bRealizado;
        
        public bool vldt { get { if (usuario.CodGrupo == Constantes.CONST_Evaluador) { return false; } else { return new Clases.genericQueries().ValidateUserCode(usuario.IdContacto, HttpContext.Current.Session["CodProyecto"].ToString()); } } }

        public bool ejecucion
        {
            get
            {
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador) { return (usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador && !vldt); }
                int codigoProyecto = HttpContext.Current.Session["CodProyecto"] != null ? Convert.ToInt32(HttpContext.Current.Session["CodProyecto"].ToString()) : 0;
                return
                new Clases.genericQueries().getItemsProyectoMercadoProyeccionVentas(codigoProyecto).Count > 0
                && usuario.CodGrupo != Constantes.CONST_Emprendedor && usuario.CodGrupo != Constantes.CONST_Evaluador;
            }
        }

        public bool visibleGuardar { get; set; }

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            txtValor.Attributes.Add("onkeypress", "javascript:return validarNro(event)");
            txtValor.Attributes.Add("onchange", "MoneyFormat(this)");

            if (HttpContext.Current.Session["CodProyecto"] == null)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "refreshParent", "window.top.location.reload();", true);
            }

            codProyecto = Session["codProyecto"].ToString();
            accion = Session["Accion"] != null && !string.IsNullOrEmpty(Session["Accion"].ToString()) ? Session["Accion"].ToString() : string.Empty;
            idInversionEdita = Session["idInversion"] != null && !string.IsNullOrEmpty(Session["idInversion"].ToString()) ? Session["idInversion"].ToString() : "0";

            inicioEncabezado(codProyecto, codConvocatoria, txtTab);

            //Consultar si es miembro.
            esMiembro = fnMiembroProyecto(usuario.IdContacto, codProyecto);

            //Consultar si está "realizado".
            bRealizado = esRealizado(txtTab.ToString(), codProyecto, "");

            if (esMiembro && !bRealizado)
            { 
                this.div_Post_It_2.Visible = true; 
            }

            if (!(esMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor) || bRealizado)
            {
                txtActualizacionMonetaria.Enabled = false;
            }
            else
            {
                txtActualizacionMonetaria.Enabled = true;
            }

            if (esMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor && bRealizado)
            {
                btnGuardar.Visible = false;
                pnlAdicionar.Visible = false;
                btnGuardar.Visible = false;
            }

            if (usuario.CodGrupo != Constantes.CONST_Emprendedor)
            {
                pnlAdicionar.Visible = false;
                btnGuardar.Visible = false;
            }

            if(!Page.IsPostBack)
            {
                CargarDatos();
            }
        }

        protected void btn_guardar_ultima_actualizacion_Click(object sender, EventArgs e)
        {
            prActualizarTab(txtTab.ToString(), codProyecto.ToString()); 
            Marcar(txtTab.ToString(), codProyecto, "", chk_realizado.Checked);
            ObtenerDatosUltimaActualizacion();
            Response.Redirect(Request.RawUrl);
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Session["TabInvoca"] = "FinanzasEgreso";
            HttpContext.Current.Session["CodProyecto"] = codProyecto;
            HttpContext.Current.Session["txtTab"] = txtTab.ToString();
            HttpContext.Current.Session["Accion"] = "Nuevo";
            Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            Session["TabInvoca"] = "FinanzasEgreso";
            HttpContext.Current.Session["CodProyecto"] = codProyecto;
            HttpContext.Current.Session["txtTab"] = txtTab.ToString();
            HttpContext.Current.Session["Accion"] = "Vista";
            Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                FieldValidate.ValidateNumeric("Índice de actualización monetaria", txtActualizacionMonetaria.Text, true);

                registroActual = getProyectoFinanzasEgreso();
                if (registroActual == null)
                {
                    Datos.ProyectoFinanzasEgreso datosNuevos = new ProyectoFinanzasEgreso()
                    {
                        CodProyecto = Convert.ToInt32(codProyecto),
                        ActualizacionMonetaria = Convert.ToDouble(txtActualizacionMonetaria.Text.Replace('.', ','))
                    };
                    consultas.Db.ProyectoFinanzasEgresos.InsertOnSubmit(datosNuevos);
                }
                else
                {
                    registroActual.ActualizacionMonetaria = Convert.ToDouble(txtActualizacionMonetaria.Text.Replace('.', ','));
                }
            
                consultas.Db.SubmitChanges();                
                prActualizarTab(txtTab.ToString(), codProyecto);
                Cancelar();
            }
            catch (ApplicationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Advertencia : " + ex.Message + " ');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }
        }

        protected void btnAdicionarInversion_Click(object sender, EventArgs e)
        {
            //CargarInversionesFijas();
            CargarTipoFuenteInversion();
            pnlEgresos.Visible = false;
            pnlCrearInversion.Visible = true;

            accion = "Crear";
            idInversionEdita = "0";
            HttpContext.Current.Session["Accion"] = accion;
            HttpContext.Current.Session["idInversion"] = idInversionEdita;

            btnCrearInversion.Text = "Crear";

        }

        protected void gw_InversionesFijas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Borrar":
                    string idInversion = e.CommandArgument.ToString();
                    consultas.Db.ExecuteCommand("delete from ProyectoInversion where Id_Inversion={0}", Convert.ToInt32(idInversion));
                    Cancelar();
                    break;
                case "Editar":
                    accion = e.CommandName;
                    idInversionEdita = e.CommandArgument.ToString();
                    HttpContext.Current.Session["Accion"] = accion;
                    HttpContext.Current.Session["idInversion"] = idInversionEdita;
                    CargarTipoFuenteInversion();
                    //if (string.IsNullOrEmpty(codProyecto) && string.IsNullOrEmpty(idInversionEdita)) EditarInversion(codProyecto, idInversionEdita);
                    EditarInversion(codProyecto, idInversionEdita);
                    break;
            }
        }

        protected void btnCrearInversion_Click(object sender, EventArgs e)
        {
            try
            {
                if (accion == "Editar")
                {

                    var query = (from p in consultas.Db.ProyectoInversions
                                 where p.CodProyecto == Convert.ToInt32(codProyecto) &&
                                 p.Id_Inversion == Convert.ToInt32(idInversionEdita)
                                 select p
                          ).First();



                    query.Concepto = txtConcepto.Text;
                    query.Valor = Convert.ToDecimal(txtValor.Text.Replace(",", "").Replace(".", ","));
                    if (!string.IsNullOrEmpty(txtSemana.Text))
                    {
                        query.Semanas = Convert.ToInt16(txtSemana.Text);
                    }
                    else
                    {
                        query.Semanas = 0;
                    }

                    query.AportadoPor = ddlTipoFuente.SelectedValue;
                    consultas.Db.SubmitChanges();
                    //ObtenerDatosUltimaActualizacion();
                    //Response.Redirect("PProyectoFinanzasEgreso.aspx?CodProyecto=" + codProyecto + "");
                }
                else
                {

                    Datos.ProyectoInversion datosNuevos = new ProyectoInversion()
                    {
                        CodProyecto = Convert.ToInt32(codProyecto),
                        Concepto = txtConcepto.Text,
                        Valor = Convert.ToDecimal(txtValor.Text.Replace(",", "").Replace(".", ",")),
                        Semanas = Convert.ToInt16(txtSemana.Text),
                        AportadoPor = ddlTipoFuente.SelectedValue,
                        TipoInversion = "Diferida"
                    };
                    consultas.Db.ProyectoInversions.InsertOnSubmit(datosNuevos);

                    consultas.Db.SubmitChanges();
                }

                Cancelar();


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

        protected void btnCancelarNuevaInversion_Click(object sender, EventArgs e)
        {
            Cancelar();
        }

        
        #endregion

        #region Eventos
        private void CargarDatos()
        {
            ObtenerDatosUltimaActualizacion();  
            CargarTipoFuenteInversion();
            CargarActualizacionMonetaria();
            CargarTiempoProyeccion();
            CargarCostosPuestaEnMarca();
            CargarCostosAnualizados();
            CargarGastosPersonales();
            CargarInversionesFijas();

            if (accion == "Editar")
            { 
                EditarInversion(codProyecto, idInversionEdita); 
            }
        }

        private void ObtenerDatosUltimaActualizacion()
        {
            //Inicializar variables.
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
                CodigoEstado = CodEstado_Proyecto(txtTab.ToString(), codProyecto, ""); //codConvocatoria);

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
                //if (!(EsMiembro && numPostIt == 0 && ((usuario.CodGrupo == Constantes.CONST_RolAsesorLider && CodigoEstado == Constantes.CONST_Inscripcion) || (CodigoEstado == Constantes.CONST_Evaluacion && usuario.CodGrupo == Constantes.CONST_RolEvaluador && es_bNuevo(codProyecto)))) || lbl_nombre_user_ult_act.Text.Trim() == "")
                if (!(EsMiembro && numPostIt == 0 && ((HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolAsesorLider.ToString() && CodigoEstado == Constantes.CONST_Inscripcion) || (CodigoEstado == Constantes.CONST_Evaluacion && HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolEvaluador.ToString() && es_bNuevo(codProyecto)))) || lbl_nombre_user_ult_act.Text.Trim() == "")
                { chk_realizado.Enabled = false; }

                //Mostrar el botón de guardar.
                //if (EsMiembro && numPostIt == 0 && lbl_nombre_user_ult_act.Text != "" && (usuario.CodGrupo == Constantes.CONST_RolAsesorLider && CodigoEstado == Constantes.CONST_Inscripcion) || (usuario.CodGrupo == Constantes.CONST_RolEvaluador && CodigoEstado == Constantes.CONST_Evaluacion && es_bNuevo(codProyecto)))
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
                visibleGuardar = visibleGuardar || Constantes.CONST_Asesor == usuario.CodGrupo;
                //Destruir variables.
                tabla = null;
                txtSQL = null;
            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error: " + ex.Message + ".')", true);
                //Destruir variables.
                //tabla = null;
                //txtSQL = null;
                //return;
            }
        }

        private int Obtener_numPostIt()
        {
            Int32 numPosIt = 0;

            //Hallar numero de post it por tab
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

        private void Cancelar()
        {
            accion = string.Empty;
            idInversionEdita = "0";
            HttpContext.Current.Session["Accion"] = null;
            HttpContext.Current.Session["idInversion"] = null;
            pnlEgresos.Visible = true;
            pnlCrearInversion.Visible = false;

            txtConcepto.Text = "";
            txtValor.Text = "0";
            txtSemana.Text = "0";

            CargarTipoFuenteInversion();
            CargarActualizacionMonetaria();
            CargarTiempoProyeccion();
            CargarInversionesFijas();
            CargarCostosPuestaEnMarca();
            CargarCostosAnualizados();
            CargarGastosPersonales();

            ObtenerDatosUltimaActualizacion();
        }

        protected void CargarTipoFuenteInversion()
        {
            ddlTipoFuente.Items.Clear();
            ddlTipoFuente.Items.Add(new ListItem() { Value = Constantes.CONST_FondoEmprender, Text = Constantes.CONST_FondoEmprender });
            ddlTipoFuente.Items.Add(new ListItem() { Value = Constantes.CONST_AporteEmprendedores, Text = Constantes.CONST_AporteEmprendedores });
            ddlTipoFuente.Items.Add(new ListItem() { Value = Constantes.CONST_RecursosCapital, Text = Constantes.CONST_RecursosCapital });
            ddlTipoFuente.Items.Add(new ListItem() { Value = Constantes.CONST_IngresosVentas, Text = Constantes.CONST_IngresosVentas });
        }

        protected void CargarActualizacionMonetaria()
        {
            try
            {
                var query = (from p in consultas.Db.ProyectoFinanzasEgresos
                             where p.CodProyecto == Convert.ToInt32(codProyecto)
                             select new { p.ActualizacionMonetaria }).First();
                txtActualizacionMonetaria.Text = query.ActualizacionMonetaria.ToString().Replace(',', '.');
            }
            catch
            {
                txtActualizacionMonetaria.Text = "1";
            }
        }

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

        protected void CargarInversionesFijas()
        {
            var txtSql2 = "select id_inversion,p.codProyecto, concepto, valor, semanas mes, AportadoPor tipoFuente,TipoInversion from ProyectoInversion p  " +
                "inner join tipoinfraestructura t on p.concepto=t.nomtipoinfraestructura right join ( select distinct(codtipoinfraestructura) as codtipo, " +
                "a.codproyecto from proyectoinfraestructura a ) b on p.codproyecto= " + codProyecto + "  where t.id_tipoinfraestructura=b.codtipo and b.codproyecto = " + codProyecto +
                " union select id_inversion,codProyecto ,concepto, valor, semanas mes, AportadoPor tipoFuente,TipoInversion " +
                "from ProyectoInversion where codproyecto = "+ codProyecto + " and tipoinversion='Diferida' order by TipoInversion desc";
            var datos = consultas.ObtenerDataTable(txtSql2, "text");


            var total2 = datos.AsEnumerable().Sum(row => row.Field<decimal>("valor"));
            var  total = Convert.ToInt64(total2);

            var drTotal = datos.NewRow();
            drTotal["concepto"] = "Total";
            drTotal["valor"] = total2.ToString();
            datos.Rows.Add(drTotal);

            gw_InversionesFijas.DataSource = datos;
            gw_InversionesFijas.DataBind();
        }

        private void CargarCostosPuestaEnMarca()
        {
            decimal total = 0;

            var query = (from p in consultas.Db.ProyectoGastos
                         where p.Tipo == "Arranque"
                         && p.CodProyecto == Convert.ToInt32(codProyecto)
                         orderby p.Descripcion ascending
                         select new { p.Id_Gasto, p.Descripcion, p.Valor, p.Protegido });

            DataTable datos = new DataTable();
            datos.Columns.Add("Descripcion");
            datos.Columns.Add("Valor");

            foreach (var item in query)
            {
                decimal valor = item.Valor;

                DataRow dr = datos.NewRow();
                dr["Descripcion"] = item.Descripcion;
                dr["Valor"] = valor.ToString("0,0.00", CultureInfo.InvariantCulture);

                total += valor;

                datos.Rows.Add(dr);
            }

            DataRow drTotal = datos.NewRow();

            drTotal["Descripcion"] = "Total";
            drTotal["Valor"] = total.ToString("0,0.00", CultureInfo.InvariantCulture);

            datos.Rows.Add(drTotal);

            gw_CostosPuestaMarcha.DataSource = datos;
            gw_CostosPuestaMarcha.DataBind();

            //Ajustar la alineación de los registros que muestra la grilla.
            if (gw_CostosPuestaMarcha.Columns.Count > 0)
            {
                gw_CostosPuestaMarcha.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            }

            PintarFilasGrid(gw_CostosPuestaMarcha, 0, new string[] { "Total" });
        }

        private void CargarCostosAnualizados()
        {
            var query = (from p in consultas.Db.ProyectoGastos
                         where p.Tipo == "Anual"
                         && p.CodProyecto == Convert.ToInt32(codProyecto)
                         orderby p.Descripcion ascending
                         select new { p.Id_Gasto, p.Descripcion, p.Valor, p.Protegido });

            DataTable datos = new DataTable();
            datos.Columns.Add("Descripcion");
            for (int i = 1; i <= txtTiempoProyeccion; i++)
            {
                datos.Columns.Add("Año " + i);
            }

            decimal[] total = new decimal[txtTiempoProyeccion + 1];
            foreach (var item in query)
            {
                DataRow dr = datos.NewRow();

                dr["Descripcion"] = item.Descripcion;

                decimal valor = item.Valor;
                for (int i = 1; i <= txtTiempoProyeccion; i++)
                {
                    dr["Año " + i] = valor.ToString("0,0.00", CultureInfo.InvariantCulture); ;
                    total[i] += valor;
                    valor = Convert.ToDecimal(txtActualizacionMonetaria.Text.Replace('.', ',')) * valor;
                }
                datos.Rows.Add(dr);
            }

            DataRow drTotal = datos.NewRow();
            drTotal["Descripcion"] = "Total";
            for (int i = 1; i <= txtTiempoProyeccion; i++)
            {
                drTotal["Año " + i] = total[i].ToString("0,0.00", CultureInfo.InvariantCulture);
            }
            datos.Rows.Add(drTotal);

            gw_CostosAnualizados.DataSource = datos;
            gw_CostosAnualizados.DataBind();

            //Ajustar la alineación de los registros que muestra la grilla.
            if (gw_CostosAnualizados.Columns.Count > 0)
            {
                gw_CostosAnualizados.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            }

            PintarFilasGrid(gw_CostosAnualizados, 0, new string[] { "Total" });
        }

        private void CargarGastosPersonales()
        {
            var query = (from p in consultas.Db.ProyectoGastosPersonals
                         where p.CodProyecto == Convert.ToInt32(codProyecto)
                         orderby p.Cargo ascending
                         select new { p.Cargo, p.ValorAnual });

            DataTable datos = new DataTable();
            datos.Columns.Add("Cargo");
            for (int i = 1; i <= txtTiempoProyeccion; i++)
            {
                datos.Columns.Add("Año " + i);
            }

            decimal[] total = new decimal[txtTiempoProyeccion + 1];
            foreach (var item in query)
            {
                DataRow dr = datos.NewRow();

                dr["Cargo"] = item.Cargo;

                decimal valor = item.ValorAnual;
                for (int i = 1; i <= txtTiempoProyeccion; i++)
                {
                    dr["Año " + i] = valor.ToString("0,0.00", CultureInfo.InvariantCulture); ;
                    total[i] += valor;
                    valor = Convert.ToDecimal(txtActualizacionMonetaria.Text.Replace('.', ',')) * valor;
                }
                datos.Rows.Add(dr);
            }

            DataRow drTotal = datos.NewRow();
            drTotal["Cargo"] = "Total";
            for (int i = 1; i <= txtTiempoProyeccion; i++)
            {
                drTotal["Año " + i] = total[i].ToString("0,0.00", CultureInfo.InvariantCulture);
            }
            datos.Rows.Add(drTotal);

            gw_GastosPersonales.DataSource = datos;
            gw_GastosPersonales.DataBind();

            //Ajustar la alineación de los registros que muestra la grilla.
            if (gw_GastosPersonales.Columns.Count > 0)
            { gw_GastosPersonales.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Right; }

            PintarFilasGrid(gw_GastosPersonales, 0, new string[] { "Total" });
        }

        private ProyectoFinanzasEgreso getProyectoFinanzasEgreso()
        {
            var query = (from p in consultas.Db.ProyectoFinanzasEgresos
                         where p.CodProyecto == Convert.ToInt32(codProyecto)
                         select p).FirstOrDefault();

            return query;

        }

        private void EditarInversion(string codProyecto, string idInversionEdita)
        {
            //CargarInversionesFijas();
            pnlEgresos.Visible = false;
            pnlCrearInversion.Visible = true;

            var query = (from p in consultas.Db.ProyectoInversions
                         where p.CodProyecto == Convert.ToInt32(codProyecto) &&
                         p.Id_Inversion == Convert.ToInt32(idInversionEdita)
                         select p
                             ).First();

            txtConcepto.Text = query.Concepto;
            txtValor.Text = query.Valor.ToString("0,0.00", CultureInfo.InvariantCulture);
            txtSemana.Text = query.Semanas.ToString();
            if (!string.IsNullOrEmpty(query.AportadoPor))
            {
                ddlTipoFuente.SelectedValue = query.AportadoPor.ToString();
            }
            btnCrearInversion.Text = "Actualizar";
        }
        #endregion

        protected void gw_InversionesFijas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((esMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor) || bRealizado)
            {
                if (e.Row.RowType != DataControlRowType.DataRow)
                    return;

                var lnk = (LinkButton)e.Row.Cells[1].FindControl("lnkConcepto");
                var imgbtn = (ImageButton)e.Row.Cells[0].FindControl("btn_Borrar");

                if (!esMiembro && usuario.CodGrupo == Constantes.CONST_AdministradorSistema)
                {
                    if (txtActualizacionMonetaria.Enabled == false)
                    {
                        imgbtn = (ImageButton)e.Row.Cells[0].FindControl("btn_Borrar");
                        imgbtn.Visible = false;
                    }
                }

                if (lnk.Text == "Diferida" || lnk.Text == "Fija" || lnk.Text == "Total" || lnk.Text == "Terrenos" || lnk.Text == "Planta e instalaciones" ||
                     lnk.Text == "Maquinaria, Equipos y herramientas" || lnk.Text == "Muebles y enceres" || lnk.Text == "Equipos de transporte, carga y almacenamiento" ||
                     lnk.Text == "Semovientes y material Vegetal" || lnk.Text == "Remodelación y/o Adecuación de instalaciones")
                {
                    e.Row.Cells[0].Controls.Clear();
                }

                if (lnk.Text == "Diferida" || lnk.Text == "Fija" || lnk.Text == "Total" || bRealizado)
                {
                    lnk.Enabled = false;
                }
                //else
                //{
                //    if (flag == 1)
                //    {
                //        //e.Row.Cells[0].Controls.Remove(imgbtn);
                //    }
                //}

                //if (lnk.Text == "Fija")
                //{
                //    flag = 1;
                //}
            }
            else
            {
                e.Row.Cells[0].Controls.Clear();
            }            
            
        }
    }
}