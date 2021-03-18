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

namespace Fonade.FONADE.Proyecto
{
    public partial class PProyectoOrganizacionCostos : Negocio.Base_Page
    {
        public string codProyecto;
        public string codConvocatoria;
        public int txtTab = Constantes.CONST_Presupuestos;
        public Boolean esMiembro;
        /// <summary>
        /// Determina si está o no "realizado"...
        /// </summary>
        public Boolean bRealizado;

        public bool vldt { get { if (usuario.CodGrupo == Constantes.CONST_Evaluador) { return false; } else { return new Clases.genericQueries().ValidateUserCode(usuario.IdContacto, HttpContext.Current.Session["CodProyecto"]); } } }

        public bool ejecucion{
            get{
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador) { return (usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador && !vldt); }
                return
                new Clases.genericQueries().getItemsProyectoMercadoProyeccionVentas(Convert.ToInt32(HttpContext.Current.Session["CodProyecto"].ToString())).Count > 0
                && usuario.CodGrupo != Constantes.CONST_Emprendedor && usuario.CodGrupo != Constantes.CONST_Evaluador;
            }
        }

        public bool visibleGuardar { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["codProyecto"] != null)
                codProyecto = HttpContext.Current.Session["codProyecto"].ToString();

            inicioEncabezado(codProyecto, codConvocatoria, txtTab);

            //Consultar si es miembro.
            esMiembro = fnMiembroProyecto(usuario.IdContacto, codProyecto);

            //Consultar si está "realizado".
            bRealizado = esRealizado(txtTab.ToString(), codProyecto, "");

            //if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_AdministradorFonade)
            if (esMiembro && !bRealizado)//!chk_realizado.Checked)
            {
                this.div_Post_It_1.Visible = true;
                Post_It1._mostrarPost = true;
                this.div_Post_It_2.Visible = true;
                Post_It2._mostrarPost = true;
                this.div_Post_It_3.Visible = true;
                Post_It3._mostrarPost = true;
            }

            //Catalogo
            ((Button)CatalogoCargo1.FindControl("btnCargo")).Command += new CommandEventHandler(btnCrearCargo_Click);
            ((Button)CatalogoCargo1.FindControl("btnCancelarCargo")).Command += new CommandEventHandler(btnCancelarCargo_Click);

            //Gastos
            ((Button)CatalogoGasto1.FindControl("btnGasto")).Command += new CommandEventHandler(btnCrearGasto_Click);
            ((Button)CatalogoGasto1.FindControl("btnCancelarGasto")).Command += new CommandEventHandler(btnCancelarGasto_Click);

            if (esMiembro == true && usuario.CodGrupo == Constantes.CONST_Emprendedor && !bRealizado)
            {
                pnlBotonAdicionarCargo.Visible = true;
                pnlAdicionarGasto.Visible = true;
                pnlAdicionarGastoAnual.Visible = true;
            }
            if (!IsPostBack)
            {
                CargarGastosPersonales();
                CargarGastosPuestaMarca();
                CargarGastosAnuales();
                ObtenerDatosUltimaActualizacion();
            }

            //if (!chk_realizado.Checked && esMiembro)
            //{
            //    btn_guardar_ultima_actualizacion.Visible = true;

            //}
            //else
            //{
            //    btn_guardar_ultima_actualizacion.Visible = false;
            //}

            if (!chk_realizado.Checked && esMiembro)
            {
                tabla_docs.Visible = true;
            }

            if(usuario.CodGrupo == Constantes.CONST_Emprendedor)
            {
                btn_guardar_ultima_actualizacion.Visible = false;
                btm_guardarCambios.Visible = true;
            }
        }

        protected void CargarGastosPersonales()
        {
            var query = (from p in consultas.Db.ProyectoGastosPersonals
                         where p.CodProyecto == Convert.ToInt32(this.codProyecto)
                         orderby p.Cargo ascending
                         select new
                         {
                             p.Id_Cargo,
                             p.Cargo,
                             p.Dedicacion,
                             p.TipoContratacion,
                             p.ValorMensual,
                             p.ValorAnual,
                             p.OtrosGastos
                             //ValorMensual = p.ValorMensual.ToString("0,0.00", CultureInfo.InvariantCulture),
                             //ValorAnual = p.ValorAnual.ToString("0,0.00", CultureInfo.InvariantCulture),
                             //OtrosGastos = p.OtrosGastos.ToString("0,0.00", CultureInfo.InvariantCulture)
                         });

            decimal[] valores = new decimal[4];
            foreach (var result in query)
            {
                valores[1] += result.ValorMensual;
                valores[2] += result.ValorAnual;
                valores[3] += result.OtrosGastos;
            }

            var query2 = (from q1 in query
                          select new
                          {
                              q1.Id_Cargo,
                              q1.Cargo,
                              q1.Dedicacion,
                              q1.TipoContratacion,
                              //q1.ValorMensual,
                              //q1.ValorAnual,
                              //q1.OtrosGastos


                              ValorMensual = q1.ValorMensual.ToString("0,0.00", CultureInfo.InvariantCulture),
                              ValorAnual = q1.ValorAnual.ToString("0,0.00", CultureInfo.InvariantCulture),
                              OtrosGastos = q1.OtrosGastos.ToString("0,0.00", CultureInfo.InvariantCulture)
                          });

            gw_GastosPersonal.DataSource = query2;
            gw_GastosPersonal.DataBind();

            //decimal[] valores = new decimal[4];

            for (int i = 0; i < gw_GastosPersonal.Rows.Count; i++)
            {
                if (miembro == true && usuario.CodGrupo == Constantes.CONST_Emprendedor && realizado == false)
                {
                    ((ImageButton)gw_GastosPersonal.Rows[i].FindControl("btn_BorrarPersonal")).Visible = true;
                    ((LinkButton)gw_GastosPersonal.Rows[i].FindControl("btnEditarPersonal")).Visible = true;
                    //((Label)gw_GastosPersonal.Rows[i].FindControl("lblEditarPersonal")).Visible = false;
                }

                //valores[1] += Convert.ToDecimal(gw_GastosPersonal.Rows[i].Cells[4].Text);
                //valores[2] += Convert.ToDecimal(gw_GastosPersonal.Rows[i].Cells[5].Text);
                //valores[3] += Convert.ToDecimal(gw_GastosPersonal.Rows[i].Cells[6].Text);
                //gw_GastosPersonal.Rows[i].Cells[4].Text = Convert.ToDecimal(gw_GastosPersonal.Rows[i].Cells[4].Text).ToString("0,0.00", CultureInfo.InvariantCulture);
                //gw_GastosPersonal.Rows[i].Cells[5].Text = Convert.ToDecimal(gw_GastosPersonal.Rows[i].Cells[5].Text).ToString("0,0.00", CultureInfo.InvariantCulture);
                //gw_GastosPersonal.Rows[i].Cells[6].Text = Convert.ToDecimal(gw_GastosPersonal.Rows[i].Cells[6].Text).ToString("0,0.00", CultureInfo.InvariantCulture);
            }

            lblTotalMensual.Text = valores[1].ToString("0,0.00", CultureInfo.InvariantCulture);
            lblTotalAnual.Text = valores[2].ToString("0,0.00", CultureInfo.InvariantCulture);
            lblTotalGastos.Text = valores[3].ToString("0,0.00", CultureInfo.InvariantCulture);
        }

        protected void CargarGastosPuestaMarca()
        {
            var query = (from p in consultas.Db.ProyectoGastos
                         where p.Tipo == "Arranque" &&
                         p.CodProyecto == Convert.ToInt32(this.codProyecto)
                         orderby p.Descripcion ascending
                         select new { p.Id_Gasto, p.Descripcion, p.Valor, p.Protegido });

            decimal valores = 0;
            foreach (var result in query)
            {
                valores += result.Valor;
            }

            var query2 = (from q1 in query
                          select new
                          {
                              q1.Id_Gasto,
                              q1.Descripcion,
                              Valor = q1.Valor.ToString("0,0.00", CultureInfo.InvariantCulture),
                              q1.Protegido
                          });

            gw_GastosPuestaMarca.DataSource = query2;
            gw_GastosPuestaMarca.DataBind();


            for (int i = 0; i < gw_GastosPuestaMarca.Rows.Count; i++)
            {
                if (((bool)gw_GastosPuestaMarca.DataKeys[i].Value) == true || !(miembro == true && usuario.CodGrupo == Constantes.CONST_Emprendedor) || realizado)
                    ((ImageButton)gw_GastosPuestaMarca.Rows[i].FindControl("btn_BorrarMarcha")).Visible = false;
                else
                    ((ImageButton)gw_GastosPuestaMarca.Rows[i].FindControl("btn_BorrarMarcha")).Visible = true;

                if (miembro == true && usuario.CodGrupo == Constantes.CONST_Emprendedor && realizado == false)
                {
                    ((LinkButton)gw_GastosPuestaMarca.Rows[i].FindControl("btnEditarMarcha")).Visible = true;
                    //((Label)gw_GastosPuestaMarca.Rows[i].FindControl("lblEditarMarcha")).Visible = false;
                }

                //valores += Convert.ToDecimal(gw_GastosPuestaMarca.Rows[i].Cells[2].Text);
                //gw_GastosPuestaMarca.Rows[i].Cells[2].Text = Convert.ToDecimal(gw_GastosPuestaMarca.Rows[i].Cells[2].Text).ToString("0,0.00", CultureInfo.InvariantCulture);
            }
            lblTotalGastosPuestaMarca.Text = valores.ToString("0,0.00", CultureInfo.InvariantCulture);
        }

        protected void CargarGastosAnuales()
        {
            var query = (from p in consultas.Db.ProyectoGastos
                         where p.Tipo == "Anual" &&
                         p.CodProyecto == Convert.ToInt32(this.codProyecto)
                         orderby p.Descripcion ascending
                         select new { p.Id_Gasto, p.Descripcion, p.Valor, p.Protegido });

            decimal valores = 0;
            foreach (var result in query)
            {
                valores += result.Valor;
            }

            var query2 = (from q1 in query
                          select new
                          {
                              q1.Id_Gasto,
                              q1.Descripcion,
                              Valor = q1.Valor.ToString("0,0.00", CultureInfo.InvariantCulture),
                              q1.Protegido
                          });

            gw_GastosAnuales.DataSource = query2;
            gw_GastosAnuales.DataBind();

            for (int i = 0; i < gw_GastosAnuales.Rows.Count; i++)
            {
                if (((bool)gw_GastosAnuales.DataKeys[i].Value) == true || !(miembro == true && usuario.CodGrupo == Constantes.CONST_Emprendedor) || realizado)
                {
                    ((ImageButton)gw_GastosAnuales.Rows[i].FindControl("btn_BorrarAnual")).Visible = false;
                }
                else
                {
                    ((ImageButton)gw_GastosAnuales.Rows[i].FindControl("btn_BorrarAnual")).Visible = true;
                }

                if (miembro == true && usuario.CodGrupo == Constantes.CONST_Emprendedor && realizado == false)
                {
                    ((LinkButton)gw_GastosAnuales.Rows[i].FindControl("btnEditarAnual")).Visible = true;
                    //((Label)gw_GastosAnuales.Rows[i].FindControl("lblEditarAnual")).Visible = false;
                }

                //valores += Convert.ToDecimal(gw_GastosAnuales.Rows[i].Cells[2].Text);
                //gw_GastosAnuales.Rows[i].Cells[2].Text = Convert.ToDecimal(gw_GastosAnuales.Rows[i].Cells[2].Text).ToString("0,0.00", CultureInfo.InvariantCulture);
            }
            lblTotalGastosAnuales.Text = valores.ToString("0,0.00", CultureInfo.InvariantCulture);
        }

        protected void btnAdicionarCargo_Click(object sender, EventArgs e)
        {
            pnlPrincipal.Visible = false;
            pnlCargo.Visible = true;
            CatalogoCargo1.Cargar(Controles.CatalogoCargo.Accion.Nuevo, codProyecto, "0");
        }

        protected void btnAdicionarGasto_Click(object sender, EventArgs e)
        {
            pnlPrincipal.Visible = false;
            pnlGastos.Visible = true;
            CatalogoGasto1.Cargar(Controles.CatalogoGasto.Accion.Nuevo, codProyecto, "0", "Arranque");
        }

        protected void btnAdicionarGastoAnual_Click(object sender, EventArgs e)
        {
            pnlPrincipal.Visible = false;
            pnlGastos.Visible = true;
            CatalogoGasto1.Cargar(Controles.CatalogoGasto.Accion.Nuevo, codProyecto, "0", "Anual");
        }

        protected void gw_GastosPersonal_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "BorrarPersonal")
            {
                string idCargo = e.CommandArgument.ToString();
                string respuesta = CatalogoCargo1.Cargar(Controles.CatalogoCargo.Accion.Borrar, codProyecto, idCargo);
                if (respuesta == "OK")
                {
                    //CargarGastosPersonales();
                }
                else
                    Alert1.Ver(respuesta, true);
            }
            if (e.CommandName == "EditarPersonal")
            {
                pnlPrincipal.Visible = false;
                pnlCargo.Visible = true;
                string idCargo = e.CommandArgument.ToString();
                CatalogoCargo1.Cargar(Controles.CatalogoCargo.Accion.Editar, codProyecto, idCargo);
            }

            CargarGastosPersonales();
            CargarGastosPuestaMarca();
            CargarGastosAnuales();
            ObtenerDatosUltimaActualizacion();
        }

        protected void gw_GastosPuestaMarca_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "BorrarMarcha")
            {
                string idGasto = e.CommandArgument.ToString();
                string respuesta = CatalogoGasto1.Cargar(Controles.CatalogoGasto.Accion.Borrar, codProyecto, idGasto, "Arranque");
                if (respuesta == "OK")
                {
                    //CargarGastosPuestaMarca();
                }
                else
                {
                    Alert1.Ver(respuesta, true);
                }
            }
            if (e.CommandName == "EditarMarcha")
            {
                pnlPrincipal.Visible = false;
                pnlGastos.Visible = true;
                string idGasto = e.CommandArgument.ToString();
                CatalogoGasto1.Cargar(Controles.CatalogoGasto.Accion.Editar, codProyecto, idGasto, "Arranque");
            }

            CargarGastosPersonales();
            CargarGastosPuestaMarca();
            CargarGastosAnuales();
            ObtenerDatosUltimaActualizacion();
        }

        protected void gw_GastosAnuales_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "BorrarAnual")
            {
                string idGasto = e.CommandArgument.ToString();
                string respuesta = CatalogoGasto1.Cargar(Controles.CatalogoGasto.Accion.Borrar, codProyecto, idGasto, "Anual");
                if (respuesta == "OK")
                {
                    //CargarGastosAnuales();
                }
                else
                {
                    Alert1.Ver(respuesta, true);
                }
            }
            if (e.CommandName == "EditarAnual")
            {
                pnlPrincipal.Visible = false;
                pnlGastos.Visible = true;
                string idGasto = e.CommandArgument.ToString();
                CatalogoGasto1.Cargar(Controles.CatalogoGasto.Accion.Editar, codProyecto, idGasto, "Anual");
            }

            CargarGastosPersonales();
            CargarGastosPuestaMarca();
            CargarGastosAnuales();
            ObtenerDatosUltimaActualizacion();
        }

        void btnCrearCargo_Click(object sender, CommandEventArgs e)
        {
            if (CatalogoCargo1.Error == "OK")
            {
                pnlPrincipal.Visible = true;
                pnlCargo.Visible = false;
                //CargarGastosPersonales();
                CargarGastosPersonales();
                CargarGastosPuestaMarca();
                CargarGastosAnuales();
                ObtenerDatosUltimaActualizacion();
            }
        }

        void btnCancelarCargo_Click(object sender, CommandEventArgs e)
        {
            pnlPrincipal.Visible = true;
            pnlCargo.Visible = false;
            CargarGastosPersonales();
            CargarGastosPuestaMarca();
            CargarGastosAnuales();
            ObtenerDatosUltimaActualizacion();
        }

        void btnCancelarGasto_Click(object sender, CommandEventArgs e)
        {
            pnlPrincipal.Visible = true;
            pnlGastos.Visible = false;
            CargarGastosPersonales();
            CargarGastosPuestaMarca();
            CargarGastosAnuales();
            ObtenerDatosUltimaActualizacion();
        }

        void btnCrearGasto_Click(object sender, CommandEventArgs e)
        {
            if (CatalogoGasto1.Error == "OK")
            {
                pnlPrincipal.Visible = true;
                pnlGastos.Visible = false;
                CargarGastosPersonales();
                CargarGastosPuestaMarca();
                CargarGastosAnuales();
                ObtenerDatosUltimaActualizacion();
            }
        }

        protected void gw_GastosPuestaMarca_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        #region Métodos de .

        /// <summary>
        /// Establecer el primer valor en mayúscula, retornando un string con la primera en maýsucula.
        /// </summary>
        /// <param name="s">String a procesar</param>
        /// <returns>String procesado.</returns>
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
        
        /// 06/06/2014.
        /// Obtener la información acerca de la última actualización realizada, ási como la habilitación del 
        /// CheckBox para el usuario dependiendo de su grupo / rol.
        /// </summary>
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
                var usuActualizo = consultas.RetornarInformacionActualizaPPagina(int.Parse(codProyecto), txtTab);

                if (usuActualizo != null)
                {
                    lbl_nombre_user_ult_act.Text = usuActualizo.SingleOrDefault().nombres.ToUpper();

                    //Convertir fecha.
                    try { fecha = Convert.ToDateTime(usuActualizo.SingleOrDefault().fecha); }
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
                    bRealizado = usuActualizo.SingleOrDefault().realizado;
                }

                //Asignar check de acuerdo al valor obtenido en "bRealizado".
                chk_realizado.Checked = bRealizado;

                //Determinar si el usuario actual puede o no "chequear" la actualización.
                //if (!(EsMiembro && numPostIt == 0 && ((usuario.CodGrupo == Constantes.CONST_RolAsesorLider && CodigoEstado == Constantes.CONST_Inscripcion) || (CodigoEstado == Constantes.CONST_Evaluacion && usuario.CodGrupo == Constantes.CONST_RolEvaluador && es_bNuevo(codProyecto)))) || lbl_nombre_user_ult_act.Text.Trim() == "")
                if (!(EsMiembro && numPostIt == 0 && ((HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolAsesorLider.ToString() && CodigoEstado == Constantes.CONST_Inscripcion) || (CodigoEstado == Constantes.CONST_Evaluacion && HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolEvaluador.ToString() && es_bNuevo(codProyecto)))) || lbl_nombre_user_ult_act.Text.Trim() == "")
                { chk_realizado.Enabled = false; }

                if (EsMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor && !bRealizado)
                {
                    btm_guardarCambios.Visible = true;
                }
                else
                {
                    btm_guardarCambios.Visible = false;
                }

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
                visibleGuardar = visibleGuardar || Constantes.CONST_CoordinadorEvaluador == usuario.CodGrupo;
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

        /// <summary>
        
        /// 06/06/2014.
        /// Obtener el número "numPostIt" usado en la condicional de "obtener última actualización".
        /// El código se encuentra en "Base_Page" línea "116", método "inicioEncabezado".
        /// Ya se le están enviado por parámetro en el método el código del proyecto y la constante "CONST_PostIt".
        /// </summary>
        /// <returns>numPostIt.</returns>
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

        /// <summary>
        
        /// 24/06/2014.
        /// Guardar la información "Ultima Actualización".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_guardar_ultima_actualizacion_Click(object sender, EventArgs e)
        { 
            prActualizarTab(txtTab.ToString(), codProyecto.ToString()); 
            Marcar(txtTab.ToString(), codProyecto, "", chk_realizado.Checked); 
            ObtenerDatosUltimaActualizacion();
            Response.Redirect(Request.RawUrl);
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Session["TabInvoca"] = "OrganizaCosto";
            HttpContext.Current.Session["CodProyecto"] = codProyecto;
            HttpContext.Current.Session["txtTab"] = txtTab.ToString();
            HttpContext.Current.Session["Accion"] = "Nuevo";
            Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            Session["TabInvoca"] = "OrganizaCosto";
            HttpContext.Current.Session["CodProyecto"] = codProyecto;
            HttpContext.Current.Session["txtTab"] = txtTab.ToString();
            HttpContext.Current.Session["Accion"] = "Vista";
            Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        #endregion

        /// <summary>
        
        /// 11/09/2014.
        /// Validar el si puede eliminar datos y editar los detalles del cargo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gw_GastosPersonal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var btnEliminar = e.Row.FindControl("btn_BorrarPersonal") as ImageButton;
                var btnCargo = e.Row.FindControl("btnEditarPersonal") as LinkButton;

                if (btnEliminar != null && btnCargo != null)
                {
                    if (!(esMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor && !bRealizado))
                    {
                        btnEliminar.Style.Add(HtmlTextWriterStyle.Display, "none");
                        btnCargo.Enabled = false;
                    }
                }
            }
        }

        /// <summary>
        
        /// 11/09/2014.
        /// Validar el acceso a ciertos controles de la grilla.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gw_GastosPuestaMarca_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var hdf = e.Row.FindControl("hdf_gastoMarcha") as HiddenField;
                var btnEliminar = e.Row.FindControl("btn_BorrarMarcha") as ImageButton;
                var btnCargo = e.Row.FindControl("btnEditarMarcha") as LinkButton;
                bool Protegido;

                if (hdf != null && btnEliminar != null && btnCargo != null)
                {
                    //Asignar el valor a la variable booleana.
                    Protegido = bool.Parse(hdf.Value);

                    if (Protegido || !(esMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor) || bRealizado)
                    { btnEliminar.Style.Add(HtmlTextWriterStyle.Display, "none"); }
                    else
                    { btnEliminar.Visible = true; }

                    if (!(esMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor && !bRealizado))
                    { btnCargo.Enabled = false; }
                }
            }
        }

        /// <summary>
        
        /// 11/09/2014.
        /// Validar el acceso a ciertos controles de la grilla.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gw_GastosAnuales_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var img = e.Row.FindControl("btn_BorrarAnual") as ImageButton;
                var hdf = e.Row.FindControl("hdf_gastoAnual") as HiddenField;
                var btn = e.Row.FindControl("btnEditarAnual") as LinkButton;
                bool Protegido;

                if (img != null && hdf != null && btn != null)
                {
                    //Asignar el valor a la variable booleana.
                    Protegido = bool.Parse(hdf.Value);

                    if (Protegido || !(esMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor) || bRealizado)
                    { img.Style.Add(HtmlTextWriterStyle.Display, "none"); }
                    else { img.Visible = true; }

                    if (!(esMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor && !bRealizado))
                    { btn.Enabled = false; }
                }
            }
        }

        protected void btm_guardarCambios_Click(object sender, EventArgs e)
        {            
            int flag = 0;
            prActualizarTab(txtTab.ToString(), codProyecto.ToString());
            flag = Marcar(Constantes.CONST_Presupuestos.ToString(), codProyecto, codConvocatoria, chk_realizado.Checked);
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
    }
}
