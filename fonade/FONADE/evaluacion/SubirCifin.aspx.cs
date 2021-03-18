#region Diego Quiñonez

// <Author>Diego Quiñonez</Author>
// <Fecha>16 - 03 - 2014</Fecha>
// <Archivo>SubirCifin.aspx.cs</Archivo>

#endregion

#region

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Negocio;

#endregion

namespace Fonade.FONADE.evaluacion
{
    public partial class SubirCifin : Base_Page
    {
        #region Propiedades


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        public bool validar()
        {
            return false;
        }

        protected void btnsubir_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SubirArchivo();
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 28/05/2014.
        /// Limitado por tamaño la carga de archivos.
        /// </summary>
        void SubirArchivo()
        {
            if (FileUploadArchive.HasFile)
            {
                if (FileUploadArchive.PostedFile.ContentLength > 10485760) // = 10MB
                {
                    CustomValidator2.ErrorMessage = "El tamaño del archivo debe ser menor a 10 Mb.";
                    CustomValidator2.ForeColor = System.Drawing.Color.Red;
                    CustomValidator2.IsValid = false;
                    return;
                }
                else
                {
                    #region Procesar el archivo seleccionado.
                    string ruta = ConfigurationManager.AppSettings.Get("RutaDocumentos");
                    string nombrearchivo = "cifin";

                    if (CargarArchivoServidor(FileUploadArchive, ruta, nombrearchivo, "xls", ConfigurationManager.AppSettings.Get("RutaDocumentosTEMP")))
                    {
                        CustomValidator2.ErrorMessage = "Archivo cargado exitosamente!";
                        CustomValidator2.ValidationGroup = "Grupo1";
                        CustomValidator2.ForeColor = System.Drawing.Color.Green;
                        CustomValidator2.IsValid = false;

                    }
                    else
                    {
                        CustomValidator2.ForeColor = System.Drawing.Color.Red;
                        CustomValidator2.ErrorMessage = "El archivo no se puedo cargar Verifiquelo y vuelva a intentar.!";
                        CustomValidator2.IsValid = false;
                    }
                    #endregion
                }
            }
            else
            {
                CustomValidator2.ErrorMessage = "Seleccione un archivo";
                CustomValidator2.ForeColor = System.Drawing.Color.Red;
                CustomValidator2.IsValid = false;
            }
        }

        protected void ValidateNombre(object source, ServerValidateEventArgs args)
        {
            string file = FileUploadArchive.FileName.ToLower();

            if (file != "cifin.xls")
            {
                args.IsValid = false;
                CustomValidator2.ErrorMessage = "El nombre no es valido, cambielo por cifin";
            }
            else
            {
                CustomValidator2.ErrorMessage = "";
            }
        }
    }
}