using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;

namespace Fonade.Controles
{
    /// <summary>
    /// Clase para cargar archivos a servidor de archivos
    /// </summary>
    public partial class CargarArchivos : System.Web.UI.UserControl
    {
        RespuestaCargue respuesta = new RespuestaCargue();

        /// <summary>
        /// Array de strings con listado de extensiones permitidas para el archivo. 
        /// </summary>
        private string[] ExtensionesPermitidas;
        private string PathDestino;
        private string PathDestinoTEMP = ConfigurationManager.AppSettings.Get("RutaDocumentosTEMP");
        private string NombreDocumento;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HttpContext.Current.Session["ExtensionesPermitidas"] = HttpContext.Current.Session["PathDestino"] = HttpContext.Current.Session["PathDestinoTEMP"] = HttpContext.Current.Session["NombreDocumento"] = String.Empty;
            }
            lblErrorDocumento.Text = "";
            
            if (!string.IsNullOrEmpty(HttpContext.Current.Session["ExtensionesPermitidas"].ToString()))
                LlenarVariables();
        }

        /// <summary>
        /// Handles the Click event of the btnCancelar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Mostrar panel de cargue para cargar archivos
        /// </summary>
        /// <param name="extensionesPermitidas">Extensiones permitidas</param>
        /// <param name="pathDestino"> Ruta de destino </param>
        /// <param name="nombreDocumento"> Nombre del documento a cargar </param>
        public void show(string[] extensionesPermitidas, string pathDestino, string nombreDocumento)
        {
            HttpContext.Current.Session["ExtensionesPermitidas"] = extensionesPermitidas;
            HttpContext.Current.Session["PathDestino"] = pathDestino;
            HttpContext.Current.Session["PathDestinoTEMP"] = PathDestinoTEMP + pathDestino.Substring(2);
            HttpContext.Current.Session["NombreDocumento"] = nombreDocumento;
            LlenarVariables();
            pnlCargue.Visible = true;
        }

        /// <summary>
        /// Obtener el valor de las variables de sesión
        /// </summary>
        protected void LlenarVariables()
        {
            ExtensionesPermitidas = (string[])Session["ExtensionesPermitidas"];
            PathDestino = HttpContext.Current.Session["PathDestino"].ToString();
            PathDestinoTEMP = HttpContext.Current.Session["PathDestinoTEMP"].ToString();
            NombreDocumento = HttpContext.Current.Session["NombreDocumento"].ToString();
        }

        /// <summary>
        /// Subir documento al sistema de archivos
        /// </summary>
        protected void btnSubirDocumento_Click(object sender, EventArgs e)
        {
            string[] extencion;
            if (ValidarFormatoDocumento())
            {
                if (fuArchivo.PostedFile.ContentLength > 10485760) // = 10MB
                {
                    respuesta.Mensaje = "El tamaño del archivo debe ser menor a 10 Mb.";
                    lblErrorDocumento.Text = respuesta.Mensaje;
                    return;
                }
                else
                {
                    extencion = fuArchivo.PostedFile.FileName.ToString().Trim().Split('.');
                    respuesta.Extencion = extencion[extencion.Length - 1];

                    if (CargarTemporal())
                    {
                        CargarDocumentoFinal();
                    }
                    else
                    {                       
                        lblErrorDocumento.Text = respuesta.Mensaje;
                        return;
                    }
                }
            }
            else
            {
                respuesta.Mensaje = "El archivo no pudo ser validado.";
                lblErrorDocumento.Text = respuesta.Mensaje;
                return;
            }
        }

        /// <summary>
        /// Validación de formato de archivo
        /// </summary>
        /// <returns> Booleano de confirmación de formato </returns>
        protected bool ValidarFormatoDocumento()
        {
            if (fuArchivo.PostedFile.FileName.ToString().Trim() != "")
            {
                if (ExtensionesPermitidas.Any(ext => fuArchivo.PostedFile.FileName.EndsWith(ext)) == false)
                {
                    respuesta.Mensaje = "El archivo seleccionado no es valido";
                    lblErrorDocumento.Text = respuesta.Mensaje;
                    return false;
                }
                if (fuArchivo.PostedFile.ContentLength > 10485760)
                {
                    respuesta.Mensaje = "El tamaño del archivo debe ser menor a 10 Mb.";
                    lblErrorDocumento.Text = respuesta.Mensaje;
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                respuesta.Mensaje = "Debe Seleccionar un archivo";
                lblErrorDocumento.Text = respuesta.Mensaje;
                return false;
            }
        }

        /// <summary>
        /// Carga de archivo a carpeta temporal
        /// </summary>
        /// <returns> Si realizo la carga al destino temporal </returns>
        protected bool CargarTemporal()
        {
            if (File.Exists(PathDestinoTEMP) == false)
            {
                try
                {
                    System.IO.Directory.CreateDirectory(PathDestinoTEMP);
                    try
                    {
                        fuArchivo.PostedFile.SaveAs(PathDestinoTEMP + NombreDocumento + "." + respuesta.Extencion);
                        respuesta.PathTemporal = PathDestinoTEMP + NombreDocumento + "." + respuesta.Extencion;
                        return true;
                    }
                    catch
                    {
                        respuesta.Mensaje = "Error No se pudo subir el documento a la carpeta TMP: ";
                        lblErrorDocumento.Text = respuesta.Mensaje;
                        return false;
                    }
                }
                catch
                {
                    respuesta.Mensaje = "Error No se pudo crear la carpeta TMP: " + PathDestinoTEMP;
                    lblErrorDocumento.Text = respuesta.Mensaje;
                    return false;
                }
            }
            
            return true;
        }

        /// <summary>
        /// Carga del archivo al destino final
        /// </summary>
        /// <returns> Si se realizo el cargue al destino final </returns>
        protected bool CargarDocumentoFinal()
        {
            if (File.Exists(PathDestino) == false)
            {
                try
                {
                    System.IO.Directory.CreateDirectory(PathDestino);
                }
                catch
                {
                    respuesta.Mensaje = "Error No se pudo crear la carpeta: " + PathDestino;
                    lblErrorDocumento.Text = respuesta.Mensaje;
                    return false;
                }
            }

            try
            {
                byte[] archivoPlano = File.ReadAllBytes(PathDestinoTEMP + NombreDocumento + "." + respuesta.Extencion);
                try
                {
                    File.WriteAllBytes(PathDestino + NombreDocumento + "." + respuesta.Extencion, archivoPlano);
                    respuesta.Mensaje = "Modelo financiero cargado correctamente.";
                    lblErrorDocumento.Text = respuesta.Mensaje;
                    respuesta.PathFisico = PathDestino + NombreDocumento + "." + respuesta.Extencion;
                    File.Delete(PathDestinoTEMP + NombreDocumento + "." + respuesta.Extencion);

                    return true;
                }
                catch
                {
                    respuesta.Mensaje = "Error al mover el archivo temporal a la ruta final: " + PathDestino;
                    lblErrorDocumento.Text = respuesta.Mensaje;
                    respuesta.PathFisico = PathDestino + NombreDocumento + "." + respuesta.Extencion;
                    return false;
                }
            }
            catch
            {
                respuesta.Mensaje = "Error No se pudo crear la carpeta: " + PathDestino;
                lblErrorDocumento.Text = respuesta.Mensaje;
                return false;
            }

        }

        /// <summary>
        /// Resultadoes this instance.
        /// </summary>
        /// <returns></returns>
        public RespuestaCargue Resultado()
        {
            return respuesta;
        }
    }

    /// <summary>
    /// Estructura de datos de respuesta a un cargue de archivos
    /// </summary>
    public class RespuestaCargue
    {
        /// <summary>
        /// Gets or sets the mensaje.
        /// </summary>
        /// <value>
        /// The mensaje.
        /// </value>
        public string Mensaje { get; set; }

        /// <summary>
        /// Gets or sets the path temporal.
        /// </summary>
        /// <value>
        /// The path temporal.
        /// </value>
        public string PathTemporal { get; set; }

        /// <summary>
        /// Gets or sets the path fisico.
        /// </summary>
        /// <value>
        /// The path fisico.
        /// </value>
        public string PathFisico { get; set; }

        /// <summary>
        /// Gets or sets the extencion.
        /// </summary>
        /// <value>
        /// The extencion.
        /// </value>
        public string Extencion { get; set; }
    }
}