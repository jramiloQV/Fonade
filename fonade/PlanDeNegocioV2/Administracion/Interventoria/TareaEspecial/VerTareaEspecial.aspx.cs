using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Negocio.PlanDeNegocioV2.Administracion.Interventoria.Reintegros;
using Fonade.Negocio.PlanDeNegocioV2.Interventoria;
using Fonade.Clases;
using System.IO;
using System.Configuration;
using Fonade.Account;
using System.Web.Security;
using Fonade.PlanDeNegocioV2.Formulacion.Utilidad;
using Fonade.Negocio.Mensajes;
using Fonade.Error;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.TareaEspecial
{
    public partial class VerTareaEspecial : System.Web.UI.Page
    {
        public int CodigoTarea
        {
            get
            {
                if (Request.QueryString["tareaEspecial"] != null)
                    return Convert.ToInt32(Request.QueryString["tareaEspecial"]);
                else
                    return 0;
            }
            set { }
        }
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                GetInfoPago();

                ValidateUsers();
            }
        }   
        public void GetInfoPago()
        {
            try
            {
                var tareaEspecial = Negocio.PlanDeNegocioV2.Administracion.Interventoria.TareasEspeciales.TareaEspecial.GetTareaEspecialeByTareaId(CodigoTarea);
                
                if (tareaEspecial != null)
                {
                    var pago = Pagos.GetInfoPago(tareaEspecial.CodigoPago);
                    var interventorAsignado = Pagos.GetInterventorByPagoId(tareaEspecial.CodigoPago);

                    if (interventorAsignado == null)
                        throw new ApplicationException("Este proyecto no tiene interventor asignado, debe asignar un interventor primero para continuar.");

                    lblMainTitle.Text = "Tarea de interventoria N°" + tareaEspecial .Id + " - pago " + pago.Id_PagoActividad + " del proyecto " + pago.CodProyecto.GetValueOrDefault();
                    lblRemitente.Text = Usuario.Nombres + " " + Usuario.Apellidos;

                    if (Usuario.CodGrupo == Datos.Constantes.CONST_Interventor)
                        lblDestinatario.Text = tareaEspecial.NombreRemitente;
                    else
                        lblDestinatario.Text = interventorAsignado.Nombres;

                    lblFechaCreacion.Text = tareaEspecial.FechaInicio.getFechaAbreviadaConFormato(true);
                    lblEstado.Text = tareaEspecial.NombreEstado;
                    lblDescripcionTarea.Text = tareaEspecial.Descripcion;
                    lnkArchivoDescripcionTarea.Visible = tareaEspecial.HasFile;

                    if (Usuario.CodGrupo == Datos.Constantes.CONST_GerenteInterventor)
                        btnCerrarTarea.Visible = true;

                    if (tareaEspecial.Estado == Datos.Constantes.const_estado_tareaEspecial_cerrada)
                    {
                        btnAdicionar.Visible = false;
                        btnCerrarTarea.Visible = false;
                    }   
                }
                else
                {
                    Response.Redirect("~/PlanDeNegocioV2/Administracion/Interventoria/TareaEspecial/TareasEspecialesGerencia.aspx");
                }
            }
            catch (ApplicationException ex)
            {
                lblError.Visible = true;
                btnAdicionar.Visible = false;
                lblError.Text = "Advertencia:" + ex.Message;
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Error inesperado :" + ex.Message;
            }
        }
        public void ValidateUsers()
        {
            try
            {                
                if (!(Usuario.CodGrupo == Datos.Constantes.CONST_GerenteInterventor || Usuario.CodGrupo == Datos.Constantes.CONST_Interventor))
                {
                    Response.Redirect("~/FONADE/evaluacion/AccesoDenegado.aspx");
                }
            }
            catch (Exception ex)
            {
                string url = Request.Url.ToString();

                string mensaje = ex.Message.ToString();
                string data = ex.Data.ToString();
                string stackTrace = ex.StackTrace.ToString();
                string innerException = ex.InnerException == null ? "" : ex.InnerException.Message.ToString();

                // Log the error
                ErrHandler.WriteError(mensaje, url, data, stackTrace, innerException, Usuario.Email, Usuario.IdContacto.ToString());

                Response.Redirect("~/FONADE/evaluacion/AccesoDenegado.aspx");
            }
        }
        
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                FieldValidate.ValidateString("Descripción", txtDescripcion.Text, true, 250);

                var tareaEspecial = Negocio.PlanDeNegocioV2.Administracion.Interventoria.TareasEspeciales.TareaEspecial.GetTareaEspecialeByTareaId(CodigoTarea);

                var pago = Pagos.GetInfoPago(tareaEspecial.CodigoPago);
                var interventorAsignado = Pagos.GetInterventorByPagoId(tareaEspecial.CodigoPago);
                var rutaArchivo = string.Empty;

                if (fuArchivo.Visible)
                {
                    if (!fuArchivo.HasFile)
                    {
                        throw new ApplicationException("Si no desea adjuntar archivo, por favor haga click en 'No adjuntar archivo'.");
                    }
                    else
                    {
                        rutaArchivo = UploadFile(pago.Id_PagoActividad, pago.CodProyecto.GetValueOrDefault(), fuArchivo);
                    }
                }


                if (Usuario.CodGrupo == Datos.Constantes.CONST_Interventor) {
                    var newHistoriaTarea = new Datos.HistoriaTareaEspecial
                    {
                        Observacion = txtDescripcion.Text,
                        Archivo = fuArchivo.Visible && fuArchivo.HasFile ? rutaArchivo : null,
                        Remitente = Usuario.IdContacto,
                        Destinatario = tareaEspecial.CodigoRemitente,
                        FechaCreacion = DateTime.Now,
                        IdTareaEspecialInterventoria = tareaEspecial.Id
                    };

                    Negocio.PlanDeNegocioV2.Administracion.Interventoria.TareasEspeciales.TareaEspecial.InsertHistoria(newHistoriaTarea);
                }
                else if(Usuario.CodGrupo == Datos.Constantes.CONST_GerenteInterventor)
                {
                    var newHistoriaTarea = new Datos.HistoriaTareaEspecial
                    {
                        Observacion = txtDescripcion.Text,
                        Archivo = fuArchivo.Visible && fuArchivo.HasFile ? rutaArchivo : null,
                        Remitente = Usuario.IdContacto,
                        Destinatario = interventorAsignado.Id_Contacto,
                        FechaCreacion = DateTime.Now,
                        IdTareaEspecialInterventoria = tareaEspecial.Id
                    };

                    Negocio.PlanDeNegocioV2.Administracion.Interventoria.TareasEspeciales.TareaEspecial.InsertHistoria(newHistoriaTarea);
                }
                
                gvHistoriaTarea.DataBind();

                if (Negocio.PlanDeNegocioV2.Administracion.Interventoria.TareasEspeciales.TareaEspecial.HasUpdatesTareaEspecialByTareaIdAndUser(tareaEspecial.Id, Usuario.IdContacto))
                    Negocio.PlanDeNegocioV2.Administracion.Interventoria.TareasEspeciales.TareaEspecial.MarkAsReadByTarea(tareaEspecial.Id, Usuario.IdContacto);

                cleanFields();
                if (Usuario.CodGrupo == Datos.Constantes.CONST_Interventor)
                    EnviarTarea(tareaEspecial.CodigoProyecto, Usuario.IdContacto, tareaEspecial.CodigoRemitente, "Actualización", "Tarea N° " +  tareaEspecial.Id + " " + tareaEspecial.Descripcion, tareaEspecial.Id);
                if (Usuario.CodGrupo == Datos.Constantes.CONST_GerenteInterventor)
                    EnviarTarea(tareaEspecial.CodigoProyecto, Usuario.IdContacto, interventorAsignado.Id_Contacto, "Actualización", "Tarea N° " + tareaEspecial.Id + " " + tareaEspecial.Descripcion, tareaEspecial.Id);
            }
            catch (ApplicationException ex)
            {
                lblError.Visible = true;
                lblError.Text = "Advertencia: " + ex.Message;
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Error inesperado: " + ex.Message;
            }
        }
        protected void cleanFields()
        {
            txtDescripcion.Text = "";
            fuArchivo.Attributes.Clear();
            lblError.Visible = false;
        }
        protected string UploadFile(int codigoPago, int codigoProyecto, FileUpload _archivo)
        {
            string directorioDestino = "TareasEspeciales\\" + codigoProyecto + "\\pago_" + codigoPago.ToString() + "\\"; //Directorio destino archivo           
            string directorioBase = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual");
            string nombreArchivo = "ArchivoTareaEspecial_" + codigoPago + "_" + codigoProyecto + "_" + DateTime.Now.GetShortDateOnlyNumbersWithUnderscore() + ".pdf";

            // ¿ Es valido el archivo ?
            if (!_archivo.HasFile)
                throw new ApplicationException("Archivo invalido");
            // ¿ Es image valida ? 
            if (_archivo.PostedFile.ContentType != "application/pdf")
                throw new ApplicationException("Adjunte un pdf, los demas tipos son invalidos.");
            // ¿ Pesa mas de diez megas ?
            if (!(_archivo.PostedFile.ContentLength < 10485760))
                throw new ApplicationException("El archivo es muy pesado, maximo 10 megas.");

            UploadFileToServer(_archivo, directorioBase, directorioDestino, nombreArchivo);

            return directorioDestino + nombreArchivo;
        }
        protected void UploadFileToServer(FileUpload _archivo, string _directorioBase, string _directorioDestino, string fileName)
        {
            // ¿ Carpeta de destino existe ?
            if (!Directory.Exists(_directorioBase + _directorioDestino))
                Directory.CreateDirectory(_directorioBase + _directorioDestino);
            if (File.Exists(_directorioBase + _directorioDestino + _archivo.FileName))
                File.Delete(_directorioBase + _directorioDestino + _archivo.FileName);

            _archivo.SaveAs(_directorioBase + _directorioDestino + fileName); //Guardamos el archivo
        }
        protected void lnkSeleccionarPago_Click(object sender, EventArgs e)
        {
            fuArchivo.Visible = !fuArchivo.Visible;
            lnkSeleccionarPago.Text = !fuArchivo.Visible ? "+ Adjuntar Archivo (Opcional)" : "- No adjuntar Archivo";
        }
        protected void EnviarTarea(int codigoProyecto,int remitente,int destinatario,string accion,string tarea,int idTarea)
        {
            AgendarTarea agenda = new AgendarTarea(
                                    destinatario,
                                    accion + " de tarea especial de interventoria " + tarea.Trim() + " " + " - Proyecto " + codigoProyecto,
                                    "Revisar tarea especial de interventoria ---> " + tarea.Trim() + " Ver tarea en Tarea especiales de interventoria mediante este enlace.",
                                    codigoProyecto.ToString(),
                                    33,
                                    "0",
                                    false,
                                    1,
                                    true,
                                    false,
                                    remitente,
                                    "tareaEspecial=" + idTarea,
                                    "",
                                    "Tarea especial de interventoria");

                agenda.Agendar();            
        }
        protected void lnkArchivoDescripcionTarea_Click(object sender, EventArgs e)
        {
            try
            {
                var tareaEspecial = Negocio.PlanDeNegocioV2.Administracion.Interventoria.TareasEspeciales.TareaEspecial.GetTareaEspecialeByTareaId(CodigoTarea);
                AccionGrid("VerDocumento", tareaEspecial.Archivo);
            }
            catch (ApplicationException ex)
            {
                lblError.Visible = true;
                btnAdicionar.Visible = false;
                lblError.Text = "Advertencia:" + ex.Message;
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Error inesperado :" + ex.Message;
            }
        }
        public List<Negocio.PlanDeNegocioV2.Administracion.Interventoria.TareasEspeciales.HistoriaTareaDTO> GetHistoria(string tareaEspecial)
        {           
                return Negocio.PlanDeNegocioV2.Administracion.Interventoria.TareasEspeciales.TareaEspecial.GetHistoriaTareaEspecialeByTareaId(Convert.ToInt32(tareaEspecial),Usuario.IdContacto);                        
        }
        protected void btnCerrarTarea_Click(object sender, EventArgs e)
        {
            try
            {                
                var tareaEspecial = Negocio.PlanDeNegocioV2.Administracion.Interventoria.TareasEspeciales.TareaEspecial.GetTareaEspecialeByTareaId(CodigoTarea);

                var pago = Pagos.GetInfoPago(tareaEspecial.CodigoPago);
                var interventorAsignado = Pagos.GetInterventorByPagoId(tareaEspecial.CodigoPago);
                var rutaArchivo = string.Empty;                

                var newHistoriaTarea = new Datos.HistoriaTareaEspecial
                {
                    Observacion = "Tarea cerrada por el gerente interventor",
                    Archivo = fuArchivo.Visible && fuArchivo.HasFile ? rutaArchivo : null,
                    Remitente = Usuario.IdContacto,
                    Destinatario = interventorAsignado.Id_Contacto,
                    FechaCreacion = DateTime.Now,
                    IdTareaEspecialInterventoria = tareaEspecial.Id,
                    FechaLecturaDestinatario = DateTime.Now
                };

                Negocio.PlanDeNegocioV2.Administracion.Interventoria.TareasEspeciales.TareaEspecial.InsertHistoria(newHistoriaTarea);

                Negocio.PlanDeNegocioV2.Administracion.Interventoria.TareasEspeciales.TareaEspecial.cerrarTarea(tareaEspecial.Id);

                if (Negocio.PlanDeNegocioV2.Administracion.Interventoria.TareasEspeciales.TareaEspecial.HasUpdatesTareaEspecialByTareaIdAndUser(tareaEspecial.Id, Usuario.IdContacto))
                    Negocio.PlanDeNegocioV2.Administracion.Interventoria.TareasEspeciales.TareaEspecial.MarkAsReadByTarea(tareaEspecial.Id, Usuario.IdContacto);

                EnviarTarea(tareaEspecial.CodigoProyecto, Usuario.IdContacto, interventorAsignado.Id_Contacto, "Cierre", tareaEspecial.Id + " " + tareaEspecial.Descripcion, tareaEspecial.Id);


                Response.Redirect("~/PlanDeNegocioV2/Administracion/Interventoria/TareaEspecial/TareasEspecialesGerencia.aspx");                
            }
            catch (ApplicationException ex)
            {
                lblError.Visible = true;
                lblError.Text = "Advertencia: " + ex.Message;
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Error inesperado: " + ex.Message;
            }
        }
        protected void gvHistoriaTarea_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            AccionGrid(e.CommandName.ToString(), e.CommandArgument.ToString());
        }        
        protected void AccionGrid(string accion, string argumento)
        {
            try
            {
                switch (accion)
                {
                    case "VerDocumento":
                        string url = ConfigurationManager.AppSettings.Get("RutaIP") + argumento;
                        Utilidades.DescargarArchivo(url);
                        break;
                }
            }
            catch (Exception ex)
            {
                Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
        }
    }
}