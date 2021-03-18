using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using Ionic.Zip;
using Datos;
using System.Drawing;
using Fonade.Account;
using System.Web.Security;
using System.Web;
using Fonade.Negocio.PlanDeNegocioV2.Interventoria;
using Fonade.PlanDeNegocioV2.Administracion.Interventoria.InformeConsolidado.Report;
using static Fonade.PlanDeNegocioV2.Administracion.Interventoria.InformeConsolidado.Report.dsConfirmacionEnvioCorreo;
using Microsoft.Reporting.WebForms;
using Fonade.Negocio.Interventoria;
using static Fonade.Negocio.Interventoria.CorreosNotificacionBLL;

namespace Fonade.SoporteHelper.CargueMasivoContratos
{
    public partial class CargueContratos : System.Web.UI.Page
    {

        protected FonadeUser Usuario
        {
            get
            {
                return HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true);
            }
            set
            {
            }
        }
        private string BaseFile { get { return "contratos.zip"; } set { } }

        private string BaseDirectory { get { return ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual") + "CargueMasivoContratos\\";} set { }}        
        private string FullBaseFilDirectory { get { return BaseDirectory  + BaseFile; } set { } }                                                                 
        private string OutPutDirectory { get { return ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual") + "Proyecto\\"; } set { } }

        ValidacionCuenta validacionCuenta = new ValidacionCuenta();



        private string BaseFileInforme
        {
            get
            {
                return "Consolidado_" + Usuario.IdContacto + "_"
                                     + string.Format("{0:00}", DateTime.Now.Day)
                                     + string.Format("{0:00}", DateTime.Now.Month)
                                     + DateTime.Now.Year + "_"
                                     + string.Format("{0:00}", DateTime.Now.Hour)
                                     + string.Format("{0:00}", DateTime.Now.Minute) + ".zip";
            }
            set { }
        }

        private string BaseDirectoryInforme { get { return ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual") + "CargueMasivoInformeConsolidado\\"; } set { } }
        private string FullBaseFilDirectoryInforme { get { return BaseDirectoryInforme + BaseFileInforme; } set { } }
        private string OutPutDirectoryInforme { get { return ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual") + "Proyecto\\"; } set { } }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string pathRuta = HttpContext.Current.Request.Url.AbsolutePath;

                if (!validacionCuenta.validarPermiso(Usuario.IdContacto, pathRuta))
                {
                    Response.Redirect(validacionCuenta.rutaHome(), true);
                }
                else
                {
                    //cargar Datos
                    cargarRecomendaciones();
                }
            }
        }

        protected void SubirArchivo_Click(object sender, EventArgs e)
        {
            lblError.Visible = false;
            try
            {
                if (!Archivo.HasFile)
                    throw new ApplicationException("Archivo invalido");
                if (!Archivo.FileName.Contains(".zip"))
                    throw new ApplicationException("Debe ser un archivo con extensión .zip");
                if (!(Archivo.PostedFile.ContentLength < 1048576000))
                    throw new ApplicationException("El archivo es muy pesado, maximo 100 megas.");

                UploadFile(Archivo);
                var messages = UnZipFile(FullBaseFilDirectory);
                gvResult.DataSource = messages;
                gvResult.DataBind();
                               
            }
            catch (ApplicationException ex)
            {
                lblError.Visible = true;
                lblError.Text = "Advertencia, detalle : " + ex.Message;
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Error, detalle : " + ex.Message;
            }
        }

        protected void UploadFile(FileUpload archivo)
        {                     
            if (!Directory.Exists(BaseDirectory))
                Directory.CreateDirectory(BaseDirectory);            
            if (File.Exists(FullBaseFilDirectory))
                File.Delete(FullBaseFilDirectory);
            //if (Directory.Exists(OutPutDirectory))
                //Directory.Delete(OutPutDirectory);

            Archivo.SaveAs(FullBaseFilDirectory);
        }

        protected List<CargueMasivoMessage> UnZipFile(string fullBaseFileDirectory) {
            using (ZipFile zip = ZipFile.Read(fullBaseFileDirectory)) {        
                    
                List<CargueMasivoMessage> messages = new List<CargueMasivoMessage>();       
             
                foreach (ZipEntry zipFile in zip)
                {
                    try
                    {
                        zipFile.FileName.IsValidFile();
                        var codigoProyecto = zipFile.FileName.GetCodigoProyecto();
                        codigoProyecto.ProyectoExist();



                        var customOutPutDirectory = OutPutDirectory + "Proyecto_"+ codigoProyecto+"\\";

                        if (!Directory.Exists(customOutPutDirectory))
                            Directory.CreateDirectory(customOutPutDirectory);
                        if (File.Exists(customOutPutDirectory + zipFile.FileName))
                            throw new ApplicationException("Existe un archivo con el mismo nombre, renombrelo para poder subirlo.");


                        zipFile.Extract(customOutPutDirectory, ExtractExistingFileAction.OverwriteSilently);
                        Insert(codigoProyecto, zipFile.FileName);

                        messages.Add(new CargueMasivoMessage {
                            Archivo = zipFile.FileName,
                            CodigoProyecto = codigoProyecto,
                            Message = "Cargado exitosamente.",
                            Url = ConfigurationManager.AppSettings.Get("RutaWebSite") + "Documentos/Proyecto/Proyecto_" + codigoProyecto + "/" + zipFile.FileName,
                            Error = ErrorType.success                                                                                                         
                        });                        
                    }
                    catch (ApplicationException ex)
                    {
                        messages.Add(new CargueMasivoMessage
                        {
                            Archivo = zipFile.FileName,
                            CodigoProyecto = null,
                            Message = "Advertencia : " + ex.Message,                            
                            Error = ErrorType.Warning
                        });                        
                    }
                    catch (Exception ex)
                    {
                        messages.Add(new CargueMasivoMessage
                        {
                            Archivo = zipFile.FileName,
                            CodigoProyecto = null,
                            Message = "Advertencia : " + ex.Message,
                            Error = ErrorType.Error
                        });
                    }
                }
                return messages;
            }           
        }

        protected void Insert(int codigoProyecto, string fileName ) {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var archivoContrato = new ContratosArchivosAnexo {                   
                    CodProyecto = codigoProyecto,
                    NombreArchivo = fileName,
                    ruta = "Documentos/Proyecto/Proyecto_" + codigoProyecto + "/" + fileName,
                    CodContacto = Usuario.IdContacto,
                    FechaIngreso = DateTime.Now
                };
                db.ContratosArchivosAnexos.InsertOnSubmit(archivoContrato);
                db.SubmitChanges();
            }
        }

        protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var color = gvResult.DataKeys[e.Row.RowIndex].Values[0].ToString();
                e.Row.BackColor = Color.FromName(color);
            }
        }

        private void cargarRecomendaciones()
        {
            List<RecomendacionInterventoriaModel> opciones = new List<RecomendacionInterventoriaModel>();

            opciones = RecomendacionInterventoriaBLL.getRecomendaciones();

            ddlRecomendacion.DataSource = opciones;
            ddlRecomendacion.DataTextField = "Recomendacion"; // FieldName of Table in DataBase
            ddlRecomendacion.DataValueField = "idRecomendacion";
            ddlRecomendacion.DataBind();
        }

        protected void btnSubirInformes_Click(object sender, EventArgs e)
        {
            string rutaArchivo = FullBaseFilDirectoryInforme;
            lblErrorInforme.Visible = false;
            try
            {
                if (!FUInformes.HasFile)
                    throw new ApplicationException("Archivo invalido");
                if (!FUInformes.FileName.Contains(".zip"))
                    throw new ApplicationException("Debe ser un archivo con extensión .zip");
                if (!(FUInformes.PostedFile.ContentLength < 1048576000))
                    throw new ApplicationException("El archivo es muy pesado, maximo 100 megas.");

                UploadFileInforme(FUInformes, rutaArchivo);
                var messages = UnZipFileInforme(rutaArchivo);
                gvResult.DataSource = messages;
                gvResult.DataBind();

            }
            catch (ApplicationException ex)
            {
                lblErrorInforme.Visible = true;
                lblErrorInforme.Text = "Advertencia, detalle : " + ex.Message;
            }
            catch (Exception ex)
            {
                lblErrorInforme.Visible = true;
                lblErrorInforme.Text = "Error, detalle : " + ex.Message;
            }
        }

        protected void UploadFileInforme(FileUpload archivo, string _rutaArchivo)
        {
            if (!Directory.Exists(BaseDirectoryInforme))
                Directory.CreateDirectory(BaseDirectoryInforme);
            if (File.Exists(_rutaArchivo))
                File.Delete(_rutaArchivo);
            //if (Directory.Exists(OutPutDirectory))
            //Directory.Delete(OutPutDirectory);

            FUInformes.SaveAs(_rutaArchivo);
        }

        protected List<CargueMasivoMessage> UnZipFileInforme(string fullBaseFileDirectory)
        {
            using (ZipFile zip = ZipFile.Read(fullBaseFileDirectory))
            {

                List<CargueMasivoMessage> messages = new List<CargueMasivoMessage>();

                foreach (ZipEntry zipFile in zip)
                {
                    try
                    {
                        zipFile.FileName.IsValidFile();
                        var codigoProyecto = zipFile.FileName.GetCodigoProyecto();
                        codigoProyecto.ProyectoExist();

                        var customOutPutDirectory = OutPutDirectoryInforme + "Proyecto_" + codigoProyecto + "\\";

                        if (!Directory.Exists(customOutPutDirectory))
                            Directory.CreateDirectory(customOutPutDirectory);
                        if (File.Exists(customOutPutDirectory + zipFile.FileName))
                            throw new ApplicationException("Existe un archivo con el mismo nombre, renombrelo para poder subirlo.");

                        //Se coloca el archivo en la carpeta del proyecto
                        zipFile.Extract(customOutPutDirectory, ExtractExistingFileAction.OverwriteSilently);
                        InsertInforme(codigoProyecto, zipFile.FileName);


                        //Ingresar en la tabla los datos de correos a enviar.
                        int codRecomendacion = Convert.ToInt32(ddlRecomendacion.SelectedItem.Value);
                        long codCorreoInforme = 0;
                        string mensajeError = "";

                        if (IngresarInformacionCorreoInforme(codigoProyecto, Usuario.IdContacto
                                                , codRecomendacion, ref codCorreoInforme
                                                , ref mensajeError))
                        {
                            string archivoNotificacion = "";
                            //generar pdf de notificacion
                            if (generarPDFEnvioCorreoInforme(customOutPutDirectory, codCorreoInforme, codigoProyecto
                                                        , ref archivoNotificacion, ref mensajeError))
                            {
                                //guardar en contratoARchivos el pdf de notificacion
                                InsertInforme(codigoProyecto, archivoNotificacion);
                            }
                            else
                            {
                                throw new ApplicationException(mensajeError);
                            }
                        }
                        else
                        {
                            throw new ApplicationException(mensajeError);
                        }

                        //Si se realizó todo correctamente
                        messages.Add(new CargueMasivoMessage
                        {
                            Archivo = zipFile.FileName,
                            CodigoProyecto = codigoProyecto,
                            Message = "Cargado exitosamente.",
                            Url = ConfigurationManager.AppSettings.Get("RutaWebSite") + "Documentos/Proyecto/Proyecto_" + codigoProyecto + "/" + zipFile.FileName,
                            Error = ErrorType.success
                        });
                    }
                    catch (ApplicationException ex)
                    {
                        messages.Add(new CargueMasivoMessage
                        {
                            Archivo = zipFile.FileName,
                            CodigoProyecto = null,
                            Message = "Advertencia : " + ex.Message,
                            Error = ErrorType.Warning
                        });
                    }
                    catch (Exception ex)
                    {
                        messages.Add(new CargueMasivoMessage
                        {
                            Archivo = zipFile.FileName,
                            CodigoProyecto = null,
                            Message = "Advertencia : " + ex.Message,
                            Error = ErrorType.Error
                        });
                    }
                }
                return messages;
            }
        }

        private bool generarPDFEnvioCorreoInforme(string ruta, long idCorreo, int codProyecto
            , ref string archivoNotificacion, ref string mensajeError)
        {
            bool generado = false;

            try
            {
                dsConfirmacionEnvioCorreo dsConfirmacionEnvio = new dsConfirmacionEnvioCorreo();

                var datosCorreo = informesBLL.getCorreoInforme(idCorreo);

                dtInfoEnvioRow dtInfoRow = dsConfirmacionEnvio.dtInfoEnvio.NewdtInfoEnvioRow();

                dtInfoRow.EmailRecibe = datosCorreo.emailRecibe;
                dtInfoRow.FechaHoraEnvio = datosCorreo.fechaHoraEnvio;
                dtInfoRow.Mensaje = datosCorreo.mensaje.Replace("<br />", System.Environment.NewLine);
                dtInfoRow.NombreEnvia = datosCorreo.nombreEnvia;
                dtInfoRow.NombreRecibe = datosCorreo.nombreRecibe;

                dsConfirmacionEnvio.dtInfoEnvio.AdddtInfoEnvioRow(dtInfoRow);

                ReportDataSource reportDataInfoEnvio = new ReportDataSource();
                reportDataInfoEnvio.Value = dsConfirmacionEnvio.dtInfoEnvio;
                reportDataInfoEnvio.Name = "dsInfoEnvio";

                LocalReport report = new LocalReport();

                report.DataSources.Add(reportDataInfoEnvio);

                report.ReportPath = @"PlanDeNegocioV2\Administracion\Interventoria\InformeConsolidado\Report\confirmacionEnvioCorreo.rdlc";

                byte[] fileBytes = report.Render("PDF");

                if (fileBytes != null)
                {
                    //Response.ContentType = "application/pdf";
                    //Response.AddHeader("content-disposition", fileBytes.Length.ToString());
                    //Response.BinaryWrite(fileBytes);
                    string strFilePath = ruta;
                    string strFileName = codProyecto + "-CorreoNotInformeConsolidado" + idCorreo + ".pdf";
                    string filename = Path.Combine(strFilePath, strFileName);
                    using (FileStream fs = new FileStream(filename, FileMode.Create))
                    {
                        fs.Write(fileBytes, 0, fileBytes.Length);
                    }

                    archivoNotificacion = strFileName;
                }

                generado = true;

                //MemoryStream ms = new MemoryStream(fileBytes, 0, 0, true, true);
                //Response.AddHeader("content-disposition", "attachment;filename= CorreoNotificacionInforme.pdf");
                //Response.Buffer = true;
                //Response.Clear();
                //Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                //Response.OutputStream.Flush();
                //Response.End();
            }
            catch (Exception ex)
            {
                generado = false;
                mensajeError = "No se logró crear el pdf de notificacion: " + ex.Message;
            }
            return generado;
        }

        protected void InsertInforme(int codigoProyecto, string fileName)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var archivoContrato = new ContratosArchivosAnexo
                {
                    CodProyecto = codigoProyecto,
                    NombreArchivo = fileName,
                    ruta = "Documentos/Proyecto/Proyecto_" + codigoProyecto + "/" + fileName,
                    CodContacto = Usuario.IdContacto,
                    FechaIngreso = DateTime.Now
                };
                db.ContratosArchivosAnexos.InsertOnSubmit(archivoContrato);
                db.SubmitChanges();
            }
        }
        CargueMasivoInformesBLL informesBLL = new CargueMasivoInformesBLL();
        private bool IngresarInformacionCorreoInforme(int _codProyecto, int _codUsuario
                                                , int _codRecomendacion, ref long idCorreoInforme
                                                , ref string MensajeError)
        {
            bool ingresado = informesBLL.GuardarInformacionCorreo(_codProyecto, _codUsuario
                                                            , _codRecomendacion, ref idCorreoInforme
                                                            , ref MensajeError);

            return ingresado;
        }

        protected void btnSubirContratos_Click(object sender, EventArgs e)
        {
            CargueMasivoZIPBLL cargueMasivo = new CargueMasivoZIPBLL();
            string mensajeError = "";
            //Nuevo metodo para cargar archivos
            var resultados = cargueMasivo.cargarArchivo(tipoDeCargaArchivo.ContratosDeCooperacion
                                                    , Usuario.IdContacto, FUContratosCooperacion
                                                    , Usuario.Email, ref mensajeError);

            if ((!mensajeError.Equals("")) && (!mensajeError.Equals("OK")))
            {
                lblErrorSubirContratosCooperacion.Text = mensajeError;
                lblErrorSubirContratosCooperacion.Visible = true;
            }

            gvResult.DataSource = resultados;
            gvResult.DataBind();
        }

        protected void btnSubirActasLiquidacion_Click(object sender, EventArgs e)
        {
            CargueMasivoZIPBLL cargueMasivo = new CargueMasivoZIPBLL();
            string mensajeError = "";
            //Nuevo metodo para cargar archivos
            var resultados = cargueMasivo.cargarArchivo(tipoDeCargaArchivo.ActasDeLiquidacion
                                                    , Usuario.IdContacto, FUSubirActasLiquidacion
                                                    , Usuario.Email, ref mensajeError);

            if ((!mensajeError.Equals("")) && (!mensajeError.Equals("OK")))
            {
                lblErrorSubirActasLiquidacion.Text = mensajeError;
                lblErrorSubirActasLiquidacion.Visible = true;
            }

            gvResult.DataSource = resultados;
            gvResult.DataBind();
        }

        protected void btnProrrogaContrato_Click(object sender, EventArgs e)
        {
            CargueMasivoZIPBLL cargueMasivo = new CargueMasivoZIPBLL();
            string mensajeError = "";
            //Nuevo metodo para cargar archivos
            var resultados = cargueMasivo.cargarArchivo(tipoDeCargaArchivo.ProrrogasDeContratos
                                                    , Usuario.IdContacto, FUProrrogaContratos
                                                    , Usuario.Email, ref mensajeError);

            if ((!mensajeError.Equals("")) && (!mensajeError.Equals("OK")))
            {
                lblErrorProrrogaContrato.Text = mensajeError;
                lblErrorProrrogaContrato.Visible = true;
            }

            gvResult.DataSource = resultados;
            gvResult.DataBind();
        }

        protected void btnOtrosDocumentos_Click(object sender, EventArgs e)
        {
            CargueMasivoZIPBLL cargueMasivo = new CargueMasivoZIPBLL();
            string mensajeError = "";
            //Nuevo metodo para cargar archivos
            var resultados = cargueMasivo.cargarArchivo(tipoDeCargaArchivo.OtrosDocumentos
                                                    , Usuario.IdContacto, FUOtrosDocumentos
                                                    , Usuario.Email, ref mensajeError);

            if ((!mensajeError.Equals("")) && (!mensajeError.Equals("OK")))
            {
                lblErrorOtrosDocumentos.Text = mensajeError;
                lblErrorOtrosDocumentos.Visible = true;
            }

            gvResult.DataSource = resultados;
            gvResult.DataBind();
        }

        protected void btnActasTerminacion_Click(object sender, EventArgs e)
        {
            CargueMasivoZIPBLL cargueMasivo = new CargueMasivoZIPBLL();
            string mensajeError = "";
            //Nuevo metodo para cargar archivos
            var resultados = cargueMasivo.cargarArchivo(tipoDeCargaArchivo.ActasDeTerminacion
                                                    , Usuario.IdContacto, FUActasTerminacion
                                                    , Usuario.Email, ref mensajeError);

            if ((!mensajeError.Equals("")) && (!mensajeError.Equals("OK")))
            {
                lblErrorActasTerminacion.Text = mensajeError;
                lblErrorActasTerminacion.Visible = true;
            }

            gvResult.DataSource = resultados;
            gvResult.DataBind();
        }

        protected void btnContratosCooperacionFirmados_Click(object sender, EventArgs e)
        {
            CargueMasivoZIPBLL cargueMasivo = new CargueMasivoZIPBLL();
            string mensajeError = "";
            //Nuevo metodo para cargar archivos
            var resultados = cargueMasivo.cargarArchivo(tipoDeCargaArchivo.ContratosDeCooperacionFirmados
                                                    , Usuario.IdContacto, FUContratosCooperacionFirmados
                                                    , Usuario.Email, ref mensajeError);

            if ((!mensajeError.Equals("")) && (!mensajeError.Equals("OK")))
            {
                lblErrorActasTerminacion.Text = mensajeError;
                lblErrorActasTerminacion.Visible = true;
            }

            gvResult.DataSource = resultados;
            gvResult.DataBind();
        }
    }

    public static class CargueContratoHelper {
        private static FonadeUser User
        {
            get
            {
                return HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true);
            }
            set
            {
            }
        }

        public static void IsValidFile(this string fileName)
        {
            fileName.FilePathHasInvalidExtension();
            fileName.FileNameHasInvalidChars();
            fileName.FilePathHasInvalidCodigoProyecto();
        }

        public static void FileNameHasInvalidChars(this string fileName)
        {
            foreach (var invalidChar in Path.GetInvalidFileNameChars()) {
                if (fileName.Contains(invalidChar))
                    throw new ApplicationException("Contiene caracteres especiales.");            
            }                                                                                                     
        }

        public static void FilePathHasInvalidExtension(this string fileName)
        {
            if (!fileName.Contains(".pdf"))
                throw new ApplicationException("Debe ser un archivo con extensión .pdf");
        }

        public static void FilePathHasInvalidCodigoProyecto(this string fileName)
        {
            try
            {
                var indiceCodigoProyecto = fileName.IndexOf("-");
                var codigoProyecto = fileName.Substring(0, indiceCodigoProyecto);                
                int value;
                if (!int.TryParse(codigoProyecto, out value))
                    throw new Exception();
                
            }
            catch (Exception)
            {
                throw new Exception("Formato invalido. ex : CodigoProyecto-xxxxxxxx.pdf");
            }           
        }

        public static int GetCodigoProyecto(this string fileName) {
            var indiceCodigoProyecto = fileName.IndexOf("-");
            var codigoProyecto = fileName.Substring(0, indiceCodigoProyecto);

            return Convert.ToInt32(codigoProyecto);
        }

        public static void ProyectoExist(this int codigoProyecto) {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                if (!db.Proyecto.Any(exist => exist.Id_Proyecto.Equals(codigoProyecto)))
                    throw new ApplicationException("No existe el proyecto.");
            }
        }

        public static void ProyectoOperador(this int codigoProyecto)
        {
            if (User.CodGrupo != Constantes.CONST_AdministradorSistema) {
                int? codOperador;
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    codOperador = (from p in db.Proyecto
                                   where p.Id_Proyecto == codigoProyecto
                                   select p.codOperador).FirstOrDefault();

                    if (codOperador != User.CodOperador)
                        throw new ApplicationException("El proyecto no esta asociado a su operador.");
                }
            }
            
        }
    }

    public enum ErrorType
    {
        success,
        Error,
        Warning
    }

    public class CargueMasivoMessage {
        public int? CodigoProyecto { get; set; }  
        public ErrorType Error { get; set; }      
        public string MessageColor { get {
                if (Error.Equals(ErrorType.Warning))
                    return "#FFFF66";
                if (Error.Equals(ErrorType.Error))
                    return "#FFCCCC";
                if (Error.Equals(ErrorType.success))
                    return "#29de61";
                return "#DCDCDC";
            } set { } }
        public string Message { get; set; }
        public string Archivo { get; set; }
        public string Url { get; set; }
        public Boolean ShowUrl { get { return Error.Equals(ErrorType.success);} set { } }
    }

}