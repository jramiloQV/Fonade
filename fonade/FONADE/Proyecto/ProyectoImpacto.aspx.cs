using System;
using Fonade.Clases;
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

namespace Fonade.FONADE.Proyecto
{
    public partial class ProyectoImpacto : Negocio.Base_Page
    {
        //NOTA: En FONADE clásico, el valor que almacena el TextBox "empleosgenerados" está directamente puesto en el
        //código fuente, el valor es "20" y es editable.

        //14/08/2014: Se reporta que este campo "empleadosgenerados" no debe ser visible para el usuario 
        //"alexis.landazabal@gmail.com", el código fuente que poseemos NO TIENE validación sobre quien/quienes
        //pueden ver este campo.

        //public int txtTab = Constantes.CONST_SubOperacion;
        public int txtTab = Constantes.CONST_Impacto;

        /// <summary>
        /// Diego Quiñonez - 29 de Diciembre de 2014
        /// </summary>
        private string codProyecto
        {
            get { return HttpContext.Current.Session["codProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["codProyecto"].ToString()) ? HttpContext.Current.Session["codProyecto"].ToString() : ""; }
        }

        /// <summary>
        /// Diego Quiñonez - 29 de Diciembre de 2014
        /// </summary>
        private string codConvocatoria
        {
            get { return HttpContext.Current.Session["codConvocatoria"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["codConvocatoria"].ToString()) ? HttpContext.Current.Session["codConvocatoria"].ToString() : ""; }
        }

        /// <summary>
        /// Diego Quiñonez
        /// 29 de Diciembre de 2014
        /// </summary>
        public Boolean esMiembro
        {
            get { return fnMiembroProyecto(usuario.IdContacto, codProyecto); }
        }

        /// <summary>
        /// Diego Quiñonez - 29 de Diciembre de 2014
        /// </summary>
        public Boolean bRealizado
        {
            get {
                return esRealizado(txtTab.ToString(), codProyecto, codConvocatoria); 
            }
        }

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
            if (esMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor && !bRealizado)
            {
                PanelGuardar.Visible = true;
                PanelGuardar.Enabled = true;
                btn_guardar.Visible = true;
                btn_guardar.Enabled = true;
                cke_ImpactoEconomico.Visible = true;
                cke_ImpactoEconomico.ReadOnly = false;
                ImageButton1.Visible = true;
                ImageButton2.Visible = true;
            }
            else
            {
                tr_div_data.Visible = true;
                div_data.Visible = true;                
                tr_data_impacto.Controls.Clear();
                ImageButton1.Visible = false;
                ImageButton2.Visible = false;
            }

            if (!IsPostBack)
            {
                ObtenerDatosUltimaActualizacion();
                llenarcampos(Int32.Parse(codProyecto));

                if (usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador)
                { this.tr_numEmpleados_Evaluacion.Visible = false; }
            }

            if (!chk_realizado.Checked && esMiembro)
            {
                td_postIt.Visible = true;
                Post_It1.Visible = true;
                Post_It1._mostrarPost = true;
            }
        }

        private void llenarcampos(int codigo)
        {
            var sqlQuery = (from ProImpacto in consultas.Db.ProyectoImpactos
                            where ProImpacto.CodProyecto == codigo
                            select new
                            {
                                impacto = ProImpacto.Impacto,
                            }).FirstOrDefault();
            if (sqlQuery != null)
            {                
                if (esMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor && !bRealizado)
                {                    
                    cke_ImpactoEconomico.Text = sqlQuery.impacto;
                }
                else
                    div_data.InnerHtml = sqlQuery.impacto;
            }
        }
        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Session["TabInvoca"] = "Impacto";
            HttpContext.Current.Session["CodProyecto"] = codProyecto;
            HttpContext.Current.Session["txtTab"] = txtTab.ToString();
            HttpContext.Current.Session["Accion"] = "Nuevo";
            Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            Session["TabInvoca"] = "Impacto";
            HttpContext.Current.Session["CodProyecto"] = codProyecto;
            HttpContext.Current.Session["txtTab"] = txtTab.ToString();
            HttpContext.Current.Session["Accion"] = "Vista";
            Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }
        protected void btn_guardar_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                SqlCommand cmd = new SqlCommand("MD_InsertUpdateImpactoProyecto", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codigoProyecto", codProyecto);                
                cmd.Parameters.AddWithValue("@textoImpacto", cke_ImpactoEconomico.Text);
                SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
                con.Open();
                cmd2.ExecuteNonQuery();
                cmd.ExecuteNonQuery();
                con.Close();
                con.Dispose();
                cmd2.Dispose();
                cmd.Dispose();
                //Actualizar fecha modificación del tab.
                prActualizarTab(Constantes.CONST_Impacto.ToString(), codProyecto);
                ObtenerDatosUltimaActualizacion();
                Response.Redirect(Request.RawUrl);                
            }
            finally {
            }
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
            bool EsNuevo = true;
            Int32 numPostIt = 0;
            Int32 CodigoEstado = 0;

            try
            {
                //Consultar si es miembro.
                EsMiembro = fnMiembroProyecto(usuario.IdContacto, codProyecto);

                //Obtener número "numPostIt".
                numPostIt = Obtener_numPostIt();

                //Consultar el "Estado" del proyecto.
                CodigoEstado = CodEstado_Proyecto(Constantes.CONST_Impacto.ToString(), codProyecto, ""); //codConvocatoria);

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
                        && tu.CodProyecto == Int32.Parse(codProyecto)
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
            int flag = 0;
            flag = Marcar(Constantes.CONST_Impacto.ToString(), codProyecto, "", chk_realizado.Checked); 
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

        #endregion
    }
}