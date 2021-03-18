using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Datos;
using System.Net;
using Fonade.Negocio;

namespace Fonade.FONADE.Proyecto
{
    public partial class PProyectoOrganizacionEstructura : Negocio.Base_Page
    {
        public string codProyecto;
        public string codConvocatoria = "";
        public int txtTab = Constantes.CONST_EstructuraOrganizacional;
        public ProyectoOrganizacionEstructura poe;
        public Boolean esMiembro;
        /// <summary>
        /// Determina si está o no "realizado"...
        /// </summary>
        public Boolean bRealizado;

        public bool vldt { get { if (usuario.CodGrupo == Constantes.CONST_Evaluador) { return false; } else { return new Clases.genericQueries().ValidateUserCode(usuario.IdContacto, HttpContext.Current.Session["CodProyecto"]); } } }

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
            
            codProyecto = HttpContext.Current.Session["codProyecto"].ToString();

            codConvocatoria = HttpContext.Current.Session["codConvocatoria"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["codConvocatoria"].ToString()) ? HttpContext.Current.Session["codConvocatoria"].ToString() : "";

            //Consultar si es miembro.
            esMiembro = fnMiembroProyecto(usuario.IdContacto, codProyecto);

            //Consultar si está "realizado".
            //bRealizado = esRealizado(txtTab.ToString(), codProyecto, codConvocatoria);
            bRealizado = esRealizado(txtTab.ToString(), codProyecto, "");

            //if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_AdministradorFonade)
            if (esMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor && !bRealizado || usuario.CodGrupo == Constantes.CONST_Asesor && !bRealizado)//!chk_realizado.Checked)
            { this.div_Post_It_2.Visible = true; tabla_docs.Visible = true; this.Post_It2.Visible = true; Post_It2._mostrarPost = true; }

            if (String.IsNullOrEmpty(codConvocatoria))
            {
                codConvocatoria = ObtenerCodigoConvocatoria(codProyecto);
                HttpContext.Current.Session["codConvocatoria"] = codConvocatoria;
            }
            else
            { codConvocatoria = HttpContext.Current.Session["codConvocatoria"].ToString(); }

            inicioEncabezado(codProyecto, codConvocatoria, txtTab);

            poe = getProyectoOrganizacionEstrategia();

            if (!IsPostBack)
            {
                definirCampos();
                ObtenerDatosUltimaActualizacion();
            }
            if (chk_realizado.Checked)
            {
                txt_estructura.Enabled = false;
            }
            //visibleGuardar = true;
        }

        private ProyectoOrganizacionEstructura getProyectoOrganizacionEstrategia()
        {
            var query = from pmi in consultas.Db.ProyectoOrganizacionEstructuras
                        where pmi.CodProyecto == Convert.ToInt32(codProyecto)
                        select pmi;

            return query.FirstOrDefault();
        }

        private void definirCampos()
        {
            //if (poe != null)
            //{
                procesarCampo(ref txt_estructura, ref panel_estructura, poe != null ? poe.EstructuraOrganizacional : "", esMiembro, bRealizado, "");
            //}
            //else
            //{
            //    procesarCampo(ref txt_estructura, ref panel_estructura, "", miembro, realizado, codConvocatoria);
            //}

            if (miembro == true && usuario.CodGrupo == Constantes.CONST_Emprendedor && realizado == false)
            {
                btm_guardarCambios.Visible = true;
            }
        }

        protected void btm_guardarCambios_Click(object sender, EventArgs e)
        {
            ProyectoOrganizacionEstructura query = getProyectoOrganizacionEstrategia();
            if (query != null)
            {
                string insert = "update ProyectoOrganizacionEstructura set EstructuraOrganizacional={0} where codproyecto={1}";
                consultas.Db.ExecuteCommand(insert, WebUtility.HtmlDecode(txt_estructura.Text), Convert.ToInt32(codProyecto));
                consultas.Db.SubmitChanges();
                //Actualizar fecha modificación del tab.
                prActualizarTab(txtTab.ToString(), codProyecto);
                ObtenerDatosUltimaActualizacion();
            }
            else
            {

                string insert = "insert into ProyectoOrganizacionEstructura (CodProyecto,EstructuraOrganizacional)";
                insert += " values ({0},{1})";
                consultas.Db.ExecuteCommand(insert, Convert.ToInt32(codProyecto), WebUtility.HtmlDecode(txt_estructura.Text));
                consultas.Db.SubmitChanges();
                //Actualizar fecha modificación del tab.
                prActualizarTab(txtTab.ToString(), codProyecto);
                ObtenerDatosUltimaActualizacion();
            }

            poe = getProyectoOrganizacionEstrategia();
            definirCampos();
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
                    btm_guardarCambios.Visible = true;
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
            int flag = 0;
            prActualizarTab(txtTab.ToString(), codProyecto.ToString());
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
            Session["TabInvoca"] = "OrganizaEstruc";
            HttpContext.Current.Session["CodProyecto"] = codProyecto;
            HttpContext.Current.Session["txtTab"] = txtTab.ToString();
            HttpContext.Current.Session["Accion"] = "Nuevo";
            Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            Session["TabInvoca"] = "OrganizaEstruc";
            HttpContext.Current.Session["CodProyecto"] = codProyecto;
            HttpContext.Current.Session["txtTab"] = txtTab.ToString();
            HttpContext.Current.Session["Accion"] = "Vista";
            Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        #endregion
    }
}