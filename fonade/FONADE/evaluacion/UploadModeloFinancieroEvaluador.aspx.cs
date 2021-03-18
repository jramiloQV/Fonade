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

namespace Fonade.FONADE.evaluacion
{
    public partial class UploadModeloFinancieroEvaluador : Negocio.Base_Page
    {
        //Pestaña actual
        Int32 pestanaActual = Constantes.ConstSubModeloFinanciero;
        protected string codigoProyecto;
        protected string codConvocatoria;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                codigoProyecto = FieldValidate.GetSessionString("CodProyecto");
                codConvocatoria = FieldValidate.GetSessionString("CodConvocatoria");

                if (codigoProyecto.Equals(String.Empty) || codConvocatoria.Equals(String.Empty))
                    throw new ApplicationException(" No se pudo obtener la información de la convocatoria y el proyecto.");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }

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
                Int64 hashDirectorioUsuario = Convert.ToInt64(codigoProyecto) / 2000;
                string directorioDestino = "EvaluacionProyecto\\" + hashDirectorioUsuario.ToString() + "\\EvaluacionProyecto_" + codigoProyecto.ToString() + "\\"; //Directorio destino archivo                
                string nombreArchivo = "modelofinanciero" + codigoProyecto + ".xls";
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
                if (string.IsNullOrEmpty(Archivo.FileName) || !(Archivo.FileName.ToLower() == "modelofinanciero.xls".ToLower() || Archivo.FileName.ToLower() == nombreArchivo.ToLower()) )
                    throw new ApplicationException("Nombre de archivo invalido, debe ser el mismo descargado 'modelofinanciero.xls' ");

                uploadFileToServer(Archivo, directorioBase, directorioDestino, nombreArchivo);

                actualizarFechaActualizacionTab();

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

        /// <summary>
        /// Actualizar la fecha de actualización de la tab actual.
        /// </summary>
        private void actualizarFechaActualizacionTab()
        {
            prActualizarTabEval(pestanaActual.ToString(), codigoProyecto, codConvocatoria);
        }

    }
}