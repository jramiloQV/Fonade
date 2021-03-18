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

namespace Fonade.FONADE.evaluacion
{
    public partial class SubirModeloFinanciero : Negocio.Base_Page
    {

        private string codProyecto;
        private string codConvocatoria;

        protected void Page_Load(object sender, EventArgs e)
        {
            l_fechaActual.Text = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");
            lbl_Titulo.Text = void_establecerTitulo("CARGAR MODELO FINANCIERO");
            codProyecto = HttpContext.Current.Session["CodProyecto"].ToString();
            codConvocatoria = HttpContext.Current.Session["CodConvocatoria"].ToString();
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 28/05/2014.
        /// Limitado por tamaño la carga de archivos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Cargar_Click(object sender, EventArgs e)
        {
            if (fu_archivo.HasFile)
            {
                if (fu_archivo.PostedFile.ContentLength > 10485760) // = 10MB
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El tamaño del archivo debe ser menor a 10 Mb.');", true);
                    return;
                }
                else
                {
                    #region Procesar el archivo seleccionado.
                    if (fu_archivo.FileName == "modelofinanciero.xls")
                    {
                    //string ruta = ConfigurationManager.AppSettings.Get("RutaDocumentosEvaluacion") + Math.Abs(Convert.ToInt32(codProyecto) / 2000) + @"/EvaluacionProyecto_" + codProyecto + @"/";                    
                        string ruta = Math.Abs(Convert.ToInt32(codProyecto) / 2000) + @"\EvaluacionProyecto_" + codProyecto + @"\";
                        string nombrearchivo = "modelofinanciero" + codProyecto;

                        if (CargarArchivoServidor(fu_archivo, ruta, nombrearchivo, "xls", "RutaDocumentosEvaluacion"))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", string.Format("alert('Archivo cargado exitosamente! {0} ');", base.respuesta.PathTemporal), true);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "window.opener.location.reload(); window.close();", true);
                        }
                        else
                        {
                            Alert1.Ver(respuesta.Mensaje, true);
                            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + capturaevento + "!');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El archivo seleccionado no es correcto!');", true);
                        return;
                    }
                    #endregion
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Aún no ha seleccionado un archivo!');", true);
                return;
            }
        }

        protected void btn_cerrar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "window.close();", true);
        }
    }
}