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
using LinqKit;
using AjaxControlToolkit;
using System.ComponentModel;
using Fonade.Negocio;

namespace Fonade.FONADE.Proyecto
{
    public partial class PProyectoResumen : Negocio.Base_Page
    {
        public string codProyecto;
        public string codConvocatoria;
        public int txtTab = Constantes.CONST_SubResumenEjecutivo;
        Boolean esMiembro;
        Boolean bRealizado;

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
            //codigo = Convert.ToInt32(Request.QueryString["codProyecto"]);
            codProyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? HttpContext.Current.Session["CodProyecto"].ToString() : "0";
            codConvocatoria = HttpContext.Current.Session["CodConvocatoria"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodConvocatoria"].ToString()) ? HttpContext.Current.Session["codConvocatoria"].ToString() : "0";

            //Consultar si es miembro.
            esMiembro = fnMiembroProyecto(usuario.IdContacto, codProyecto);

            //Consultar si está "realizado".
            bRealizado = esRealizado(txtTab.ToString(), codProyecto, "");

            //Pintar post it.
            if (esMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor && !bRealizado)
            {
                td_Post_It1.Visible = true;
                Post_It1._mostrarPost = true;

                td_Post_It2.Visible = true;
                Post_It2._mostrarPost = true;

                td_Post_It3.Visible = true;
                Post_It3._mostrarPost = true;

                td_Post_It4.Visible = true;
                Post_It4._mostrarPost = true;

                td_Post_It5.Visible = true;
                Post_It5._mostrarPost = true;

                td_Post_It6.Visible = true;
                Post_It6._mostrarPost = true;

            }

            if (esMiembro && usuario.CodGrupo == Constantes.CONST_Asesor && !bRealizado)
            {
                td_Post_It1.Visible = true;
                Post_It1._mostrarPost = true;

                td_Post_It2.Visible = true;
                Post_It2._mostrarPost = true;

                td_Post_It3.Visible = true;
                Post_It3._mostrarPost = true;

                td_Post_It4.Visible = true;
                Post_It4._mostrarPost = true;

                td_Post_It5.Visible = true;
                Post_It5._mostrarPost = true;

                td_Post_It6.Visible = true;
                Post_It6._mostrarPost = true;

            }

            if (esMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor && !bRealizado)
            {
                //Mostrar la información editable.
                PanelGuardar.Visible = true;
                PanelGuardar.Enabled = true;
                btn_guardar.Visible = true;
                btn_guardar.Enabled = true;
                tr_concepto.Visible = true;
                tr_potencial.Visible = true;
                tr_ventajas.Visible = true;
                tr_resumen.Visible = true;
                tr_proyecciones.Visible = true;
                tr_conclusiones.Visible = true;
                tabla_docs.Visible = true;
            }
            else
            {
                //Mostrar los div's SOLO LECTURA
                tr_concepto.Controls.Clear();
                tr_concepto_vi.Visible = true;
                div_concepto.Visible = true;

                tr_potencial.Controls.Clear();
                tr_potencial_vi.Visible = true;
                div_potencial.Visible = true;

                tr_ventajas.Controls.Clear();
                tr_ventajas_vi.Visible = true;
                div_ventajas.Visible = true;

                tr_resumen.Controls.Clear();
                tr_resumen_vi.Visible = true;
                div_resumen.Visible = true;

                tr_proyecciones.Controls.Clear();
                tr_proyecciones_vi.Visible = true;
                div_proyecciones.Visible = true;

                tr_conclusiones.Controls.Clear();
                tr_conclusiones_vi.Visible = true;
                div_conclusiones.Visible = true;
            }

            if (!IsPostBack)
            {
                ObtenerDatosUltimaActualizacion();
                llenarcampos(Int32.Parse(codProyecto));
            }
        }

        private void llenarcampos(int codigo)
        {
            var sqlQuery = (from resumen in consultas.Db.ProyectoResumenEjecutivos
                            where resumen.CodProyecto == codigo
                            select new
                            {
                                resumen,
                            }).FirstOrDefault();
            if (sqlQuery != null)
            {
                #region Habilitar visualziación de Justificación y Políticas de acuerdo al rol del usuario. COMENTADO.
                //if (usuario.CodGrupo != Constantes.CONST_Emprendedor)
                //{
                //    #region Campos de solo-lectura visibles.
                //    tr_concepto_vi.Visible = true;
                //    div_concepto.Visible = true;

                //    tr_potencial_vi.Visible = true;
                //    div_potencial.Visible = true;

                //    tr_ventajas_vi.Visible = true;
                //    div_ventajas.Visible = true;

                //    tr_resumen_vi.Visible = true;
                //    div_resumen.Visible = true;

                //    tr_proyecciones_vi.Visible = true;
                //    div_proyecciones.Visible = true;

                //    tr_conclusiones_vi.Visible = true;
                //    div_conclusiones.Visible = true;
                //    #endregion

                //    //Filas invisibles.
                //    tr_concepto.Visible = false;
                //    tr_potencial.Visible = false;
                //    tr_ventajas.Visible = false;
                //    tr_resumen.Visible = false;
                //    tr_proyecciones.Visible = false;
                //    tr_conclusiones.Visible = false;

                //    div_concepto.InnerHtml = sqlQuery.resumen.ConceptoNegocio;
                //    div_conclusiones.InnerHtml = sqlQuery.resumen.ConclusionesFinancieras;
                //    div_potencial.InnerHtml = sqlQuery.resumen.PotencialMercados;
                //    div_proyecciones.InnerHtml = sqlQuery.resumen.Proyecciones;
                //    div_resumen.InnerHtml = sqlQuery.resumen.ResumenInversiones;
                //    div_ventajas.InnerHtml = sqlQuery.resumen.VentajasCompetitivas;

                //    //Otros controles ocultos.
                //    Post_It1.Visible = false;
                //    Post_It2.Visible = false;
                //    Post_It3.Visible = false;
                //    Post_It4.Visible = false;
                //    Post_It5.Visible = false;
                //    Post_It6.Visible = false;
                //    btn_guardar.Visible = false;
                //}
                //else
                //{
                //    #region Campos de solo-lectura invisibles.
                //    tr_concepto_vi.Visible = false;
                //    div_concepto.Visible = false;

                //    tr_potencial_vi.Visible = false;
                //    div_potencial.Visible = false;

                //    tr_ventajas_vi.Visible = false;
                //    div_ventajas.Visible = false;

                //    tr_resumen_vi.Visible = false;
                //    div_resumen.Visible = false;

                //    tr_proyecciones_vi.Visible = false;
                //    div_proyecciones.Visible = false;

                //    tr_conclusiones_vi.Visible = false;
                //    div_conclusiones.Visible = false;
                //    #endregion

                //    //Filas visibles.
                //    tr_concepto.Visible = true;
                //    tr_potencial.Visible = true;
                //    tr_ventajas.Visible = true;
                //    tr_resumen.Visible = true;
                //    tr_proyecciones.Visible = true;
                //    tr_conclusiones.Visible = true;

                //    txt_concepto.Text = sqlQuery.resumen.ConceptoNegocio;
                //    txt_Conclusiones.Text = sqlQuery.resumen.ConclusionesFinancieras;
                //    txt_potencial.Text = sqlQuery.resumen.PotencialMercados;
                //    txt_Proyecciones.Text = sqlQuery.resumen.Proyecciones;
                //    txt_ResumenInversiones.Text = sqlQuery.resumen.ResumenInversiones;
                //    txt_Ventajas.Text = sqlQuery.resumen.VentajasCompetitivas;
                //}
                #endregion

                if (esMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor && !bRealizado)
                {
                    txt_concepto.Text = sqlQuery.resumen.ConceptoNegocio;
                    txt_Conclusiones.Text = sqlQuery.resumen.ConclusionesFinancieras;
                    txt_potencial.Text = sqlQuery.resumen.PotencialMercados;
                    txt_Proyecciones.Text = sqlQuery.resumen.Proyecciones;
                    txt_ResumenInversiones.Text = sqlQuery.resumen.ResumenInversiones;
                    txt_Ventajas.Text = sqlQuery.resumen.VentajasCompetitivas;
                }
                else
                {
                    div_concepto.InnerHtml = sqlQuery.resumen.ConceptoNegocio;
                    div_conclusiones.InnerHtml = sqlQuery.resumen.ConclusionesFinancieras;
                    div_potencial.InnerHtml = sqlQuery.resumen.PotencialMercados;
                    div_proyecciones.InnerHtml = sqlQuery.resumen.Proyecciones;
                    div_resumen.InnerHtml = sqlQuery.resumen.ResumenInversiones;
                    div_ventajas.InnerHtml = sqlQuery.resumen.VentajasCompetitivas;
                }
            }
        }

        protected void btn_guardar_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("MD_InsertUpdateproyectoresumenejecutivo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codigoProyecto", codProyecto);
                cmd.Parameters.AddWithValue("@txtConcepto", txt_concepto.Text);
                cmd.Parameters.AddWithValue("@txtPotencial", txt_potencial.Text);
                cmd.Parameters.AddWithValue("@txtVentajas", txt_Ventajas.Text);
                cmd.Parameters.AddWithValue("@txtInversiones", txt_ResumenInversiones.Text);
                cmd.Parameters.AddWithValue("@txtProyecciones", txt_Proyecciones.Text);
                cmd.Parameters.AddWithValue("@txtConclusiones", txt_Conclusiones.Text);
                SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
                con.Open();
                cmd2.ExecuteNonQuery();
                cmd.ExecuteNonQuery();
                //con.Close();
                //con.Dispose();
                cmd2.Dispose();
                cmd.Dispose();
                //Actualizar fecha modificación del tab.
                prActualizarTab(txtTab.ToString(), codProyecto);
                ObtenerDatosUltimaActualizacion();
                Response.Redirect(Request.RawUrl);
            }
            finally {
                con.Close();
                con.Dispose();
            }
        }

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

                //Consultar los datos a mostrar en los campos correspondientes a la actualización.
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
                if (!(EsMiembro && numPostIt == 0 && ((codRole == Constantes.CONST_RolAsesorLider.ToString() && CodigoEstado == Constantes.CONST_Inscripcion) || (CodigoEstado == Constantes.CONST_Evaluacion && codRole == Constantes.CONST_RolEvaluador.ToString() && es_bNuevo(codProyecto)))) || lbl_nombre_user_ult_act.Text.Trim() == "")
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
                //txtSQL = null;
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
            prActualizarTab(txtTab.ToString(), codProyecto.ToString());
            var chkRealizado = (Request.Form.Get("chk_realizado") == "on" ? true : false);
            Marcar(txtTab.ToString(), codProyecto, "", chkRealizado);
            ObtenerDatosUltimaActualizacion();
            Response.Redirect(Request.RawUrl);
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Session["TabInvoca"] = "ResumenEjecutivo";
            HttpContext.Current.Session["CodProyecto"] = codProyecto;
            HttpContext.Current.Session["txtTab"] = txtTab.ToString();
            HttpContext.Current.Session["Accion"] = "Nuevo";
            Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            Session["TabInvoca"] = "ResumenEjecutivo";
            HttpContext.Current.Session["CodProyecto"] = codProyecto;
            HttpContext.Current.Session["txtTab"] = txtTab.ToString();
            HttpContext.Current.Session["Accion"] = "Vista";
            Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        #endregion
    }
}