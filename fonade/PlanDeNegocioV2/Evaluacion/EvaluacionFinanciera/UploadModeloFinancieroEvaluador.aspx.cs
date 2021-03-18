using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Clases;
using Datos;

namespace Fonade.PlanDeNegocioV2.Evaluacion.EvaluacionFinanciera
{
    public partial class UploadModeloFinancieroEvaluador : Negocio.Base_Page
    {
        Int32 pestanaActual = Constantes.Const_CargueModeloFinancieroEvaluacionV2;

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

        protected void Page_Load(object sender, EventArgs e)
        {                        
        }

        protected void SubirArchivo_Click(object sender, EventArgs e)
        {
            UploadFile();
        }

        /// <summary>
        /// Metodo para subir una documento de identidad o certificado de estudios a la base de datos y al sistema de archivos
        /// Modificado por Marcel Solera @marztres 
        /// </summary>
        private void UploadFile()
        {
            Error.Visible = false;
            try
            {
                Int64 hashDirectorioUsuario = Convert.ToInt64(CodigoProyecto) / 2000;
                string directorioDestino = "EvaluacionProyecto\\" + hashDirectorioUsuario.ToString() + "\\EvaluacionProyecto_" + CodigoProyecto.ToString() + "\\"; //Directorio destino archivo                
                string nombreArchivo = "modelofinanciero" + CodigoProyecto + ".xls";
                string directorioBase = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual");
                StringBuilder querySql = new StringBuilder();

                // ¿ Es valido el archivo ?
                if (!Archivo.HasFile)
                    throw new ApplicationException("Archivo invalido");
                // ¿ Es Excel Valido ? 
                if (Archivo.PostedFile.ContentType != "application/vnd.ms-excel")
                    throw new ApplicationException("Adjunte un archivo Excel con extensión .xls, los demas tipos son invalidos.");
                // ¿ Pesa mas de diez megas ?
                if (!(Archivo.PostedFile.ContentLength < 10485760))
                    throw new ApplicationException("El archivo es muy pesado, maximo 10 megas.");
                //¿ Nombre archivo empty?
                if (string.IsNullOrEmpty(Archivo.FileName) || !(Archivo.FileName.ToLower() == "modelofinanciero.xls".ToLower() || Archivo.FileName.ToLower() == nombreArchivo.ToLower()))
                    throw new ApplicationException("Nombre de archivo invalido, debe ser el mismo descargado 'modelofinanciero.xls' ");

                uploadFileToServer(Archivo, directorioBase, directorioDestino, nombreArchivo);

                UpdateTab();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.href = window.opener.location.href;", true);
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "window.close()", true);
            }
            catch (ApplicationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + ex.Message + "');", true);
                Error.Visible = true;
                Error.Text = "Sucedio un error , Detalle error : " + ex.Message;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
                Error.Visible = true;
                Error.Text = "Sucedio un error, intentelo de nuevo. detalle : " + ex.Message;
            }
        }

        private void UpdateTab()
        {
            TabEvaluacionProyecto tabEvaluacion = new TabEvaluacionProyecto()
            {
                CodProyecto = CodigoProyecto,
                CodConvocatoria = CodigoConvocatoria,
                CodTabEvaluacion = (Int16)pestanaActual,
                CodContacto = usuario.IdContacto,
                FechaModificacion = DateTime.Now,
                Realizado = false
            };

            string messageResult;
            Negocio.PlanDeNegocioV2.Utilidad.TabEvaluacion.SetUltimaActualizacion(tabEvaluacion, out messageResult);
            Formulacion.Utilidad.Utilidades.PresentarMsj(messageResult, this, "Alert");
        }

        /// <summary>
        /// Metodo para subir el archivo a un directorio indicado
        /// </summary>
        /// <param name="_archivo"></param>
        /// <param name="_directorioBase"></param>
        /// <param name="_directorioDestino"></param>
        protected void uploadFileToServer(FileUpload _archivo, string _directorioBase, string _directorioDestino, string _nombreArchivo)
        {
            // ¿ Carpeta de destino existe ?
            if (!Directory.Exists(_directorioBase + _directorioDestino))
                Directory.CreateDirectory(_directorioBase + _directorioDestino);
            //¿ Existe el archivo ? si delete
            if (File.Exists(_directorioBase + _directorioDestino + _nombreArchivo))
                File.Delete(_directorioBase + _directorioDestino + _nombreArchivo);

            Archivo.SaveAs(_directorioBase + _directorioDestino + _nombreArchivo); //Guardamos el archivo
        }

        /// <summary>
        /// Cierra el formulario.
        /// </summary>
        protected void CancelarEvent(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Salida por solicitud del usuario", "window.close();", true);
        }        
    }
}