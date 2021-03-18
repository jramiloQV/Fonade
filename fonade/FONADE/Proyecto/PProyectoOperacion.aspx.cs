using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using Fonade.Negocio;
using Fonade.Clases;

namespace Fonade.FONADE.Proyecto
{
    public partial class PProyectoOperacion : Negocio.Base_Page
    {
        public string codProyecto;
        public int txtTab = Constantes.CONST_SubOperacion;
        public string codConvocatoria;
        public ProyectoOperacion po;
        Boolean esMiembro;
        /// <summary>
        /// Determina si está o no "realizado"...
        /// </summary>
        Boolean bRealizado;

        public bool vldt { get { if (usuario.CodGrupo == Constantes.CONST_Evaluador) { return false; } else {
            int codigoProyecto = HttpContext.Current.Session["CodProyecto"] != null ? Convert.ToInt32(HttpContext.Current.Session["CodProyecto"].ToString()) : 0;
            return new Clases.genericQueries().ValidateUserCode(usuario.IdContacto, codigoProyecto); } } }

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

            codProyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? HttpContext.Current.Session["CodProyecto"].ToString() : "0";
            codConvocatoria = HttpContext.Current.Session["CodConvocatoria"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodConvocatoria"].ToString()) ? HttpContext.Current.Session["CodConvocatoria"].ToString() : "0";

            //Consultar si es miembro.
            esMiembro = fnMiembroProyecto(usuario.IdContacto, codProyecto);

            //Consultar si está "realizado".
            //bRealizado = esRealizado(txtTab.ToString(), codProyecto, codConvocatoria);
            bRealizado = esRealizado(txtTab.ToString(), codProyecto, "");

            //if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_AdministradorFonade)
            if (esMiembro && !bRealizado)//!chk_realizado.Checked)
            {
                this.div_post_it_1.Visible = true;
                Post_It1._mostrarPost = true;

                this.div_post_it_2.Visible = true;
                Post_It2._mostrarPost = true;

                this.div_post_it_3.Visible = true;
                Post_It3._mostrarPost = true;

                this.div_post_it_4.Visible = true;
                Post_It4._mostrarPost = true;

                this.div_post_it_5.Visible = true;
                Post_It5._mostrarPost = true;

            }
            else
            {
                this.div_post_it_1.Visible = false;
                Post_It1._mostrarPost = false;

                this.div_post_it_2.Visible = false;
                Post_It2._mostrarPost = false;

                this.div_post_it_3.Visible = false;
                Post_It3._mostrarPost = false;

                this.div_post_it_4.Visible = false;
                Post_It4._mostrarPost = false;

                this.div_post_it_5.Visible = false;
                Post_It5._mostrarPost = false;
            }

            //se agrega control paa el gerente evaluador para que el check este deshabilitado
            if (usuario.CodGrupo == Constantes.CONST_GerenteEvaluador)
            {
                chk_realizado.Enabled = false;
            }

            //Iniciar encabezado...
            inicioEncabezado(codProyecto, codConvocatoria, txtTab);

            //Obtenemos el ProyectoMercadoProyeccionVenta
            po = getProyectoOperacion();

            if (!IsPostBack)
            {
                definirCampos();
                ObtenerDatosUltimaActualizacion();
                // 2014/12/05 RAlvaradoT Se verifica que chk_realizado
                if(chk_realizado.Checked)
                {
                    // oculto los controles de Texto de Edicion y muestro los divs
                    panel_fichaProducto.Visible = true;
                    panel_fichaProducto.InnerHtml = txt_fichaProducto.Text;
                    txt_fichaProducto.Visible = false;

                    panel_estadoDesarrollo.Visible = true;
                    panel_estadoDesarrollo.InnerHtml = txt_estadoDesarrollo.Text;
                    txt_estadoDesarrollo.Visible = false;

                    panel_descripcionProceso.Visible = true;
                    panel_descripcionProceso.InnerHtml = txt_descripcionProceso.Text;
                    txt_descripcionProceso.Visible = false;

                    panel_necesidades.Visible = true;
                    panel_necesidades.InnerHtml = txt_necesidades.Text;
                    txt_necesidades.Visible = false;

                    panel_planProduccion.Visible = true;
                    panel_planProduccion.InnerHtml = txt_planProduccion.Text;
                    txt_planProduccion.Visible = false;
                }
            }

            
            //if (po == null) { /* Response.Redirect("~/Default.aspx"); ó lanzar error  */ return; }
        }

        private void definirCampos()
        {

            //si encuentra el proyecto mercado investigacion
            if (po != null)
            {
                procesarCampo(ref txt_fichaProducto, ref panel_fichaProducto, po.FichaProducto, esMiembro, bRealizado, "");
                procesarCampo(ref txt_estadoDesarrollo, ref panel_estadoDesarrollo, po.EstadoDesarrollo, esMiembro, bRealizado, "");
                procesarCampo(ref txt_descripcionProceso, ref panel_descripcionProceso, po.DescripcionProceso, esMiembro, bRealizado, "");
                procesarCampo(ref txt_necesidades, ref panel_necesidades, po.Necesidades, esMiembro, bRealizado, "");
                procesarCampo(ref txt_planProduccion, ref panel_planProduccion, po.PlanProduccion, esMiembro, bRealizado, "");
            }
            else
            {
                procesarCampo(ref txt_fichaProducto, ref panel_fichaProducto, "", esMiembro, bRealizado, "");
                procesarCampo(ref txt_estadoDesarrollo, ref panel_estadoDesarrollo, "", esMiembro, bRealizado, "");
                procesarCampo(ref txt_descripcionProceso, ref panel_descripcionProceso, "", esMiembro, bRealizado, "");
                procesarCampo(ref txt_necesidades, ref panel_necesidades, "", esMiembro, bRealizado, "");
                procesarCampo(ref txt_planProduccion, ref panel_planProduccion, "", esMiembro, bRealizado, "");
            }

            if (esMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor && !bRealizado)
            {
                btm_guardarCambios.Visible = true;
                btn_limpiarCampos.Visible = true;
            }
            else
            {
                btm_guardarCambios.Visible = false;
                btn_limpiarCampos.Visible = false;
            }
        }

        private ProyectoOperacion getProyectoOperacion()
        {
            var query = from pmi in consultas.Db.ProyectoOperacions
                        where pmi.CodProyecto == Convert.ToInt32(codProyecto)
                        select pmi;

            return query.FirstOrDefault();
        }

        protected void btm_guardarCambios_Click(object sender, EventArgs e)
        {
            //Miramos si ya esxist el registro
            //var query = from p in consultas.Db.ProyectoOperacions
            //            where p.CodProyecto == Convert.ToInt32(codProyecto)
            //            select p;

            ProyectoOperacionNegocio proyOperNeg = new ProyectoOperacionNegocio();
            var query = getProyectoOperacion();

            ProyectoOperacion pmv = new ProyectoOperacion()
            {
                CodProyecto = Convert.ToInt32(codProyecto),
                FichaProducto = txt_fichaProducto.Text.htmlEncode(),
                EstadoDesarrollo = txt_estadoDesarrollo.Text.htmlEncode(),
                DescripcionProceso = txt_descripcionProceso.Text.htmlEncode(),
                Necesidades = txt_necesidades.Text.htmlEncode(),
                PlanProduccion = txt_planProduccion.Text.htmlEncode()
            };

            //if (query.Count() > 0)
            if (query == null)
            {
                //consultas.Db.ProyectoOperacions.InsertOnSubmit(pmv);
                proyOperNeg.Agregar(pmv);
                //Actualizar fecha modificación del tab.
                prActualizarTab(txtTab.ToString(), codProyecto);
                ObtenerDatosUltimaActualizacion();
            }
            else
            {
                proyOperNeg.Modificar(pmv);
            }
            //consultas.Db.SubmitChanges();
            //Actualizar fecha modificación del tab.
            prActualizarTab(txtTab.ToString(), codProyecto);
            ObtenerDatosUltimaActualizacion();
        }

        protected void btn_limpiarCampos_Click(object sender, EventArgs e)
        {
            txt_fichaProducto.Text = "";
            txt_estadoDesarrollo.Text = "";
            txt_descripcionProceso.Text = "";
            txt_necesidades.Text = "";
            txt_planProduccion.Text = "";            
        }

        #region Métodos de Mauricio Arias Olave.

        /// <summary>
        /// Mauricio Arias Olave.
        /// 06/06/2014.
        /// Obtener la información acerca de la última actualización realizada, ási como la habilitación del 
        /// CheckBox para el usuario dependiendo de su grupo / rol.
        /// </summary>
        private void ObtenerDatosUltimaActualizacion()
        {
            //Inicializar variables.

            List<String> tabla = new List<String>();
            bool bRealizado = false;
            bool EsMiembro = false;
            Int32 numPostIt = 0;
            Int32 CodigoEstado = 0;
            OperacionesRoleNegocio opRoleNeg = new OperacionesRoleNegocio();
            String codRole = String.Empty;

            try
            {
                //Consultar si es miembro.
                EsMiembro = fnMiembroProyecto(usuario.IdContacto, codProyecto);

                //Obtener número "numPostIt".
                //numPostIt = opRoleNeg.Obtener_numPostIt(Convert.ToInt32(codProyecto));
                numPostIt = opRoleNeg.Obtener_numPostIt(Convert.ToInt32(codProyecto), Convert.ToInt32(usuario.IdContacto));

                //Consultar el "Estado" del proyecto.
                CodigoEstado = CodEstado_Proyecto(txtTab.ToString(), codProyecto, codConvocatoria);

                #region Obtener el rol.

                var uhb = opRoleNeg.ObtenerRolUsuario(codProyecto, usuario.IdContacto) ?? string.Empty;
                codRole = uhb.ToString();
                HttpContext.Current.Session["CodRol"] = codRole;

                #endregion

                //Consultar los datos a mostrar en los campos correspondientes a la actualización.
                //Asignar resultados de la consulta a variable DataTable.
                //tabla = opRoleNeg.UltimaModificacion(codProyecto, txtTab);

                ////Si tiene datos "y debe tenerlos" ejecuta el siguiente código.
                //if (tabla.Count > 0)
                //{
                //    //Nombre del usuario quien hizo la actualización.
                //    lbl_nombre_user_ult_act.Text = tabla[0].ToUpperInvariant();

                //    #region Formatear la fecha.

                //    lbl_fecha_formateada.Text = tabla[1];

                //    #endregion

                //    //Valor "bRealziado".
                //    bRealizado = Convert.ToBoolean(tabla[2]);
                //}

                DateTime fecha = new DateTime();

                var usuActualizo = consultas.RetornarInformacionActualizaPPagina(int.Parse(codProyecto), txtTab);

                var act = usuActualizo.ToList();

                //if (usuActualizo != null)
                if(act.Count > 0)
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
                //Destruir variables.

                if (usuario.CodGrupo == Constantes.CONST_Asesor)
                {
                    visibleGuardar = true;
                }
                tabla = null;
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
        /// Mauricio Arias Olave.
        /// 24/06/2014.
        /// Guardar la información "Ultima Actualización".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_guardar_ultima_actualizacion_Click(object sender, EventArgs e)
        {
            int flag = 0;

            var chkRealizado = (Request.Form.Get("chk_realizado") == "on" ? true : false);
            flag = Marcar(txtTab.ToString(), codProyecto, "", chkRealizado);
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

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Session["TabInvoca"] = "OperOperiacion";
            HttpContext.Current.Session["CodProyecto"] = codProyecto;
            HttpContext.Current.Session["txtTab"] = txtTab.ToString();
            HttpContext.Current.Session["Accion"] = "Nuevo";
            Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            Session["TabInvoca"] = "OperOperiacion";
            HttpContext.Current.Session["CodProyecto"] = codProyecto;
            HttpContext.Current.Session["txtTab"] = txtTab.ToString();
            HttpContext.Current.Session["Accion"] = "Vista";
            Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        #endregion

    }
}