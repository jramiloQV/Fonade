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
using System.IO;
using System.Web.Configuration;
using Fonade.Clases;
using Fonade.Error;

namespace Fonade.FONADE.evaluacion
{
    public partial class EvaluacionModeloFinanciero : Negocio.Base_Page
    {
        #region variables globales
        private string codProyecto;
        private string codConvocatoria;
        public Boolean esMiembro;
        
        /// <summary>
        /// Determina si "está" o "no" realizado...
        /// </summary>
        public Boolean bRealizado;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //codProyecto = FieldValidate.GetSessionString("CodProyecto");
                codProyecto = HttpContext.Current.Session["CodProyecto"].ToString();
                //codConvocatoria = FieldValidate.GetSessionString("CodConvocatoria");
                codConvocatoria = HttpContext.Current.Session["CodConvocatoria"].ToString();

                if (codProyecto.Equals(String.Empty))
                    throw new ApplicationException(" No se pudo obtener la información del usuario y el proyecto.");

                //Consultar si es miembro.
                esMiembro = fnMiembroProyecto(usuario.IdContacto, codProyecto);

                //Consultar si está "realizado".
                bRealizado = esRealizado(Constantes.ConstSubModeloFinanciero, Int32.Parse(codProyecto), Int32.Parse(codConvocatoria));

                if (!IsPostBack)
                {
                    #region En el page_load.

                    inicioEncabezado(codProyecto, codConvocatoria, Constantes.ConstSubModeloFinanciero);
                    if (miembro == true && usuario.CodGrupo == Constantes.CONST_Evaluador || usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador || usuario.CodGrupo == Constantes.CONST_GerenteEvaluador )
                    {
                        VerificarLinKModeloFinanciero();
                        
                        if (usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador)
                        {
                            ImageButton2.Visible = false;
                            ImageButton2.Enabled = false;
                            HyperLink1.Visible = false;
                        }
                        else if (usuario.CodGrupo == Constantes.CONST_Evaluador && realizado)
                        {
                            ImageButton2.Visible = false;
                            ImageButton2.Enabled = false;
                        }

                        if (usuario.CodGrupo == Constantes.CONST_GerenteEvaluador)
                        {
                            HyperLink1.Visible = false;
                            ImageButton2.Visible = false;
                        }
                        
                        PanelModelo.Visible = true;
  
                    }
               
                    #endregion

                    ObtenerDatosUltimaActualizacion();
                }

            
                if (esMiembro && !bRealizado)
                { this.div_Post_It1.Visible = true; Post_It1._mostrarPost = true; }
                //CtrlCheckedProyecto1.tabCod = Constantes.ConstSubModeloFinanciero;
                //CtrlCheckedProyecto1.CodProyecto = codProyecto;

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

        protected void Button1_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "window.open('SubirModeloFinanciero.aspx','_blank','width=580,height=300,toolbar=no, scrollbars=no, resizable=no');", true);

                string nombrearchivo = "modelofinanciero" + codProyecto + ".xls";
                string rutamodeloFin = ConfigurationManager.AppSettings.Get("RutaDocumentosEvaluacion") + Math.Abs(Convert.ToInt32(codProyecto) / 2000) + @"\EvaluacionProyecto_" + codProyecto + @"\" + nombrearchivo;
                string rutaAl = rutamodeloFin.Replace("\\", "/");

                if (File.Exists(rutamodeloFin))
                {
                    DescargarArchivo(rutamodeloFin);
                }
                else
                {
                    Response.Write("<script>alert('No existe modelo');</script>");
                }
        }


        protected void lnk_modeloFinanciero_Click(object sender, EventArgs e)
        {
            Redirect(null, "../../FONADEDOCUMENTOS/MODELOFINANCIERO.xls", "", "menubar=0,scrollbars=1,width=710,height=400,top=100");
        }

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
        /// Mauricio Arias Olave.
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
            bRealizado = false;
            bool EsMiembro = false;
            Int32 numPostIt = 0;
            bool bEnActa = false; //Determinar si el proyecto esta incluido en un acta de comite evaluador.
            Int32 CodigoEstado = 0;

            try
            {
                //Consultar si es miembro.
                EsMiembro = fnMiembroProyecto(usuario.IdContacto, codProyecto);

                //Obtener número "numPostIt".
                numPostIt = Obtener_numPostIt();

                //Determinar si "está en acta".
                bEnActa = es_EnActa(codProyecto.ToString(), codConvocatoria.ToString());

                //Consultar el "Estado" del proyecto.
                CodigoEstado = CodEstado_Proyecto(Constantes.ConstModeloFinanciero.ToString(), codProyecto, codConvocatoria);

                #region Obtener el rol.

                //Consulta.
                txtSQL = " SELECT CodContacto, CodRol From ProyectoContacto " +
                         " Where CodProyecto = " + codProyecto + " And CodContacto = " + usuario.IdContacto +
                         " and inactivo=0 and FechaInicio<=getdate() and FechaFin is null ";

                //Asignar variables a DataTable.
                var rs = consultas.ObtenerDataTable(txtSQL, "text");

                if (rs.Rows.Count > 0)
                {
                    //Crear la variable de sesión.
                    HttpContext.Current.Session["CodRol"] = rs.Rows[0]["CodRol"].ToString();
                }

                //Destruir la variable.
                rs = null;

                #endregion

                //Consultar los datos a mostrar en los campos correspondientes a la actualización.
                var usuActualizo = consultas.RetornarInformacionActualizaPagina(int.Parse(codProyecto), int.Parse(codConvocatoria), Constantes.ConstSubModeloFinanciero).FirstOrDefault();

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

                ////Evaluar "habilitación" del CheckBox.
                //if (!(EsMiembro && HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolCoordinadorEvaluador.ToString()) || lbl_nombre_user_ult_act.Text.Trim() == "" || CodigoEstado != Constantes.CONST_Evaluacion || bEnActa)
                //{ chk_realizado.Enabled = false; }

                ////if (EsMiembro && HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolCoordinadorEvaluador.ToString() && lbl_nombre_user_ult_act.Text.Trim() != "" && CodigoEstado == Constantes.CONST_Evaluacion && (!bEnActa))
                //if (EsMiembro && HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolCoordinadorEvaluador.ToString() && lbl_nombre_user_ult_act.Text.Trim() != "" && CodigoEstado == Constantes.CONST_Evaluacion && (!bEnActa))
                //{
                //    btn_guardar_ultima_actualizacion.Enabled = true;
                //    btn_guardar_ultima_actualizacion.Visible = true;
                //}

                //Nuevos controles para los check
                //Si es coordinador de evaluacion debe tener habilitado los checks
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador)
                {
                    btn_guardar_ultima_actualizacion.Visible = true;
                    chk_realizado.Enabled = true;
                }
                else
                {
                    btn_guardar_ultima_actualizacion.Visible = false;
                    chk_realizado.Enabled = false;
                }

                //Destruir variables.
                tabla = null;
                txtSQL = null;

            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error: " + ex.Message + ".')", true);
                //Destruir variables.
                tabla = null;
                txtSQL = null;

                string url = Request.Url.ToString();

                string mensaje = ex.Message.ToString();
                string data = ex.Data.ToString();
                string stackTrace = ex.StackTrace.ToString();
                string innerException = ex.InnerException == null ? "" : ex.InnerException.Message.ToString();

                // Log the error
                ErrHandler.WriteError(mensaje, url, data, stackTrace, innerException, usuario.Email, usuario.IdContacto.ToString());

                return;
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
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
        /// Mauricio Arias Olave.
        /// 24/06/2014.
        /// Guardar la información "Ultima Actualización".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_guardar_ultima_actualizacion_Click(object sender, EventArgs e)
        {
            int flag = 0;
            flag = Marcar(Constantes.ConstSubModeloFinanciero.ToString(), codProyecto, codConvocatoria, chk_realizado.Checked); 
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

        //Verifica si existe el archivo cargado del modelo financiero y muestra el enlace para verlo.
        protected void VerificarLinKModeloFinanciero() 
        {
                Int64 hashDirectorioUsuario = Convert.ToInt64(codProyecto) / 2000;
                string directorioDestino = "EvaluacionProyecto\\" + hashDirectorioUsuario.ToString() + "\\EvaluacionProyecto_" + codProyecto.ToString() + "\\"; //Directorio destino archivo                
                string nombreArchivo = "modelofinanciero" + codProyecto + ".xls";
                string directorioBase = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual");

                if (File.Exists(directorioBase + directorioDestino + nombreArchivo))
                    btnLinkVerModeloFinanciero.Visible = true;
        }

        //Muestra el modelo financiero cargado.
        protected void btnLinkVerModeloFinanciero_Click(object sender, EventArgs e)
        {
            try
            {
                Int64 hashDirectorioUsuario = Convert.ToInt64(codProyecto) / 2000;
                string directorioDestino = "EvaluacionProyecto\\" + hashDirectorioUsuario.ToString() + "\\EvaluacionProyecto_" + codProyecto.ToString() + "\\"; //Directorio destino archivo                
                string nombreArchivo = "modelofinanciero" + codProyecto + ".xls";
                string directorioBase = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual");

                if (!File.Exists(directorioBase + directorioDestino + nombreArchivo))
                    throw new ApplicationException("No se encontro el archivo de Modelo financiero.");

                string directorioModelo = "EvaluacionProyecto/" + hashDirectorioUsuario.ToString() + "/EvaluacionProyecto_" + codProyecto.ToString() + "/"; //Directorio destino archivo                

                string rutaWebModeloFinanciero = ConfigurationManager.AppSettings.Get("RutaWebSite") + ConfigurationManager.AppSettings.Get("DirVirtual2") + directorioModelo + nombreArchivo;

                Redirect(null, rutaWebModeloFinanciero, "_blank", null);
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

        protected void ImageButton2_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "window.open('UploadModeloFinancieroEvaluador.aspx','_blank','width=580,height=300,toolbar=no, scrollbars=no, resizable=no');", true);
        }

       
    
    }
}