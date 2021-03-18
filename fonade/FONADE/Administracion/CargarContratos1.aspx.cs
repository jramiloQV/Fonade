#region Diego Quiñonez

// <Author>Diego Quiñonez</Author>
// <Archivo>CargarContratos1.cs</Archivo>

#endregion

using Fonade.Error;
using Ionic.Zip;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.Administracion
{
    /// <summary>
    /// CargarContratos1
    /// </summary>    
    public partial class CargarContratos1 : Negocio.Base_Page
    {
        string txtSQL;
        //string error;

        /// <summary>
        /// Gets or sets the label information.
        /// </summary>
        /// <value>
        /// The label information.
        /// </value>
        public string lblInfo_ { get; set; }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) lblInfo_ = "Cargar Archivo zip con archivos de contratos ";
        }

        /// <summary>
        /// Handles the Click event of the btncargar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btncargar_Click(object sender, EventArgs e)
        {
            ClientScriptManager cm = this.ClientScript;

            if (!fld_cargar.HasFile)
            {
                cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('No ha subido ningun archivo');</script>");
                return;
            }
            else
            {
                if (fld_cargar.PostedFile.ContentLength > 10485760)
                {
                    cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('El tamaño del archivo debe ser menor a 10 Mb.');</script>");
                    return;
                }
                else
                {
                    #region Iniciar con el procesamiento del archivo.

                    string nombreArchivo;
                    string extencionArchivo;

                    nombreArchivo = System.IO.Path.GetFileName(fld_cargar.PostedFile.FileName);
                    extencionArchivo = System.IO.Path.GetExtension(fld_cargar.PostedFile.FileName);

                    if (string.IsNullOrEmpty(nombreArchivo))
                    {
                        cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('Debe seleccionar un archivo!!!');</script>");
                        return;
                    }
                    if (!(extencionArchivo.Equals(".zip") || extencionArchivo.Equals(".ZIP")))
                    {
                        cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('El archivo seleccionado no es valido');</script>");
                        return;
                    }

                    string saveLocation = string.Format(@"{0}{1}",ConfigurationManager.AppSettings["RutaDocumentosZIPContratos"],nombreArchivo);
                    if (System.IO.DriveInfo.GetDrives().Where(d => Equals(d.RootDirectory, ConfigurationManager.AppSettings["RutaDocumentosZIPContratos"])).Count() < 1) {
                        cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('El directorio de destino no existe.');</script>");
                        return;
                    }

                    if ((System.IO.File.Exists(saveLocation)))
                    {
                        System.IO.File.Delete(saveLocation);
                    }

                    if (!(File.Exists(saveLocation)))
                    {
                        fld_cargar.PostedFile.SaveAs(saveLocation);
                    }
                    else
                    {
                        cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('Ya se encuantra almacenado un archivo con este nombre');</script>");
                        return;
                    }

                    string destino = string.Format(@"{0}ZIP{1}", ConfigurationManager.AppSettings["RutaDocumentosContratoUnZip"], usuario.IdContacto);

                    try
                    {
                        if (!(Directory.Exists(destino)))
                        {
                            Directory.CreateDirectory(destino);
                        }
                        else {
                            System.IO.DirectoryInfo dy = new DirectoryInfo(destino);
                            foreach (var rgn in dy.GetFiles()){
                                File.Delete(rgn.FullName);
                            }
                            dy.Delete();
                            //File.Delete(saveLocation);               
                        }
                    }
                    catch (IOException)
                    {
                        cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('Error No se pudo crear la carpeta');</script>");
                        return;
                    }

                    try
                    {
                        DescomprimirFicheros(saveLocation, destino);
                    }
                    catch (ZipException)
                    {
                        cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('Error No se pudo descomprimir archivo');</script>");
                        return;
                    }

                    string[] files;

                    files = Directory.GetFiles(destino);

                    foreach (string file in files)
                    {
                        FileInfo arch = new FileInfo(file);
                        StreamReader objReader = new StreamReader(destino + "\\" + arch.Name);
                        string sLine = "";
                        ArrayList arrText = new ArrayList();

                        while (sLine != null)
                        {
                            sLine = objReader.ReadLine();
                            if (sLine != null)
                            {
                                arrText.Add(sLine);
                                string[] atrib = sLine.Split('_');
                                try
                                {
                                    string CodProyecto = atrib[0];
                                    string CodContrato = atrib[1];
                                    string NombreArchivoContrato = atrib[2];

                                    txtSQL = "SELECT Id_Empresa FROM Empresa WHERE CodProyecto = " + CodProyecto;

                                    //SqlDataReader reader = base.ejecutaReader(txtSQL, 1);
                                    var dt = consultas.ObtenerDataTable(txtSQL, "text");

                                    if (dt.Rows.Count > 0)
                                    {
                                        string CodEmpresa = dt.Rows[0].ItemArray[0].ToString(); // reader["Id_Empresa"].ToString();

                                        txtSQL = "UPDATE ContratoEmpresa SET NumeroContrato='" + CodContrato + "' WHERE CodEmpresa = " + CodEmpresa;
                                        instruccion(txtSQL, 2);
                                        txtSQL = "insert into ContratosArchivosAnexos (CodProyecto,ruta,NombreArchivo) values (" + CodProyecto + ",'" + destino + arch.Name + "','" + arch.Name + "')";
                                        instruccion(txtSQL, 2);
                                    }
                                }
                                catch (IndexOutOfRangeException) { }
                            }
                        }
                        objReader.Close();
                    }

                    //if ((File.Exists(saveLocation)))
                    //{
                    //    File.Delete(saveLocation);
                    //}

                    //if ((Directory.Exists(destino)))
                    //{
                    //    Directory.Delete(destino);
                    //}
                    lblInfo_ = "Carga de archivos finalizada.";
                    #endregion
                }
            }
        }

        /// <summary>
        /// Descomprimirs the ficheros.
        /// </summary>
        /// <param name="Zip">The zip.</param>
        /// <param name="RutaDestino">The ruta destino.</param>
        public static void DescomprimirFicheros(string Zip, string RutaDestino)
        {
            using (ZipFile FicheroComprimido = ZipFile.Read(Zip))
            {
                FicheroComprimido.ExtractAll(RutaDestino);
            }
        }

        /// <summary>
        /// Instruccions the specified SQL.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public SqlDataReader instruccion(String sql, int obj)
        {
            SqlDataReader reader = null;

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(sql, conn);

            try
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                        reader.Close();
                }

                if (conn != null)
                    conn.Close();

                conn.Open();

                if (obj == 1)
                    reader = cmd.ExecuteReader();
                else
                    cmd.ExecuteReader();
            }
            catch (SqlException ex) {
                string url = Request.Url.ToString();

                string mensaje = ex.Message.ToString();
                string data = ex.Data.ToString();
                string stackTrace = ex.StackTrace.ToString();
                string innerException = ex.InnerException == null ? "" : ex.InnerException.Message.ToString();

                // Log the error
                ErrHandler.WriteError(mensaje, url, data, stackTrace, innerException, usuario.Email, usuario.IdContacto.ToString());
            }
            finally
            {
                if (conn != null)
                    conn.Close();
                    conn.Dispose();
            }

            return reader;
        }
    }
}