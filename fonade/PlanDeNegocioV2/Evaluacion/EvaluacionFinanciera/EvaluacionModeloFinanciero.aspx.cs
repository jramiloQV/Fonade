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

namespace Fonade.PlanDeNegocioV2.Evaluacion.EvaluacionFinanciera
{
    public partial class EvaluacionModeloFinanciero : Negocio.Base_Page
    {

        #region variables globales
        public int CodigoProyecto
        {
            get
            {
                return Convert.ToInt32(Request.QueryString["codproyecto"]);
            }
            set { }
        }
        public int CodigoConvocatoria
        {
            get
            {
                return Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(CodigoProyecto, HttpContext.Current.Session["HistorialEvaluacion"] != null ? Convert.ToInt32(HttpContext.Current.Session["HistorialEvaluacion"]) : 0).GetValueOrDefault();
            }
            set { }
        }
        public int txtTab = Constantes.Const_CargueModeloFinancieroEvaluacionV2;

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
                EncabezadoEval.IdProyecto = CodigoProyecto;
                EncabezadoEval.IdConvocatoria = CodigoConvocatoria;
                EncabezadoEval.IdTabEvaluacion = txtTab;

                esMiembro = fnMiembroProyecto(usuario.IdContacto, CodigoProyecto.ToString());
                                
                bRealizado = esRealizado(txtTab, CodigoProyecto, CodigoConvocatoria);

                if (!IsPostBack)
                {
                    #region En el page_load.

                    inicioEncabezado(CodigoProyecto.ToString(), CodigoConvocatoria.ToString(), txtTab);
                    if (miembro == true && usuario.CodGrupo == Constantes.CONST_Evaluador || usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador || usuario.CodGrupo == Constantes.CONST_GerenteEvaluador)
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
                    
                }


                if (esMiembro && !bRealizado)
                { this.div_Post_It1.Visible = true; Post_It1._mostrarPost = true; }                

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
            string nombrearchivo = "modelofinanciero" + CodigoProyecto + ".xls";
            string rutamodeloFin = ConfigurationManager.AppSettings.Get("RutaDocumentosEvaluacion") + Math.Abs(Convert.ToInt32(CodigoProyecto) / 2000) + @"\EvaluacionProyecto_" + CodigoProyecto + @"\" + nombrearchivo;
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
                            
        //Verifica si existe el archivo cargado del modelo financiero y muestra el enlace para verlo.
        protected void VerificarLinKModeloFinanciero()
        {
            Int64 hashDirectorioUsuario = Convert.ToInt64(CodigoProyecto) / 2000;
            string directorioDestino = "EvaluacionProyecto\\" + hashDirectorioUsuario.ToString() + "\\EvaluacionProyecto_" + CodigoProyecto.ToString() + "\\"; //Directorio destino archivo                
            string nombreArchivo = "modelofinanciero" + CodigoProyecto + ".xls";
            string directorioBase = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual");

            if (File.Exists(directorioBase + directorioDestino + nombreArchivo))
                btnLinkVerModeloFinanciero.Visible = true;
        }

        //Muestra el modelo financiero cargado.
        protected void btnLinkVerModeloFinanciero_Click(object sender, EventArgs e)
        {
            try
            {
                Int64 hashDirectorioUsuario = Convert.ToInt64(CodigoProyecto) / 2000;
                string directorioDestino = "EvaluacionProyecto\\" + hashDirectorioUsuario.ToString() + "\\EvaluacionProyecto_" + CodigoProyecto.ToString() + "\\"; //Directorio destino archivo                
                string nombreArchivo = "modelofinanciero" + CodigoProyecto + ".xls";
                string directorioBase = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual");

                if (!File.Exists(directorioBase + directorioDestino + nombreArchivo))
                    throw new ApplicationException("No se encontro el archivo de Modelo financiero.");

                string directorioModelo = "EvaluacionProyecto/" + hashDirectorioUsuario.ToString() + "/EvaluacionProyecto_" + CodigoProyecto.ToString() + "/"; //Directorio destino archivo                

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
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "window.open('UploadModeloFinancieroEvaluador.aspx?codproyecto="+CodigoProyecto +"','_blank','width=580,height=300,toolbar=no, scrollbars=no, resizable=no');", true);
        }

    }
}