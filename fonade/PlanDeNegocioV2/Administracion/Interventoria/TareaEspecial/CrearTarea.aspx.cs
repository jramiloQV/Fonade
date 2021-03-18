using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.TareaEspecial
{
    public partial class CrearTarea : System.Web.UI.Page
    {
        public int CodigoPago
        {
            get
            {
                if (Request.QueryString["codigo"] != null)
                    return Convert.ToInt32(Request.QueryString["codigo"]);
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
                var pago = Pagos.GetInfoPago(CodigoPago);
                var interventorAsignado = Pagos.GetInterventorByPagoId(CodigoPago);

                if (pago != null)
                {
                    if (interventorAsignado == null)
                        throw new ApplicationException("Este proyecto no tiene interventor asignado, debe asignar un interventor primero para continuar.");

                    lblMainTitle.Text = "Crear tarea de interventoria - pago " + pago.Id_PagoActividad + " del proyecto " + pago.CodProyecto.GetValueOrDefault();
                    lblRemitente.Text = Usuario.Nombres + " " + Usuario.Apellidos;
                    lblDestinatario.Text = interventorAsignado.Nombres + " " + interventorAsignado.Apellidos;
                }
                else 
                {
                    Response.Redirect("~/PlanDeNegocioV2/Administracion/Interventoria/TareaEspecial/SeleccionarPago.aspx");
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
                if (!(Usuario.CodGrupo == Datos.Constantes.CONST_GerenteInterventor))
                {
                    Response.Redirect("~/FONADE/evaluacion/AccesoDenegado.aspx");
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("~/FONADE/evaluacion/AccesoDenegado.aspx");
            }
        }
        public List<HistorialReintegroDTO> GetReintegros(string codigo)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                try
                {
                    return Reintegro.GetReintegros(Convert.ToInt32(codigo));

                }
                catch (ApplicationException ex)
                {
                    return new List<HistorialReintegroDTO>();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                FieldValidate.ValidateString("Descripción", txtDescripcion.Text, true, 250);
                
                var pago = Pagos.GetInfoPago(CodigoPago);
                var interventorAsignado = Pagos.GetInterventorByPagoId(CodigoPago);
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

                var newTareaEspecial = new Datos.TareaEspecialInterventoria {
                    CodigoPago = pago.Id_PagoActividad,
                    Remitente = Usuario.IdContacto,
                    Destinatario = interventorAsignado.Id_Contacto,
                    Estado = Datos.Constantes.const_estado_tareaEspecial_pendiente,
                    FechaInicio = DateTime.Now,
                    FechaUltimaActualizacion = DateTime.Now
                };

                Negocio.PlanDeNegocioV2.Administracion.Interventoria.TareasEspeciales.TareaEspecial.Insert(newTareaEspecial);

                var newHistoriaTarea = new Datos.HistoriaTareaEspecial
                {
                    Observacion = txtDescripcion.Text,
                    Archivo = fuArchivo.Visible && fuArchivo.HasFile ? rutaArchivo : null,
                    Remitente = Usuario.IdContacto,
                    Destinatario = interventorAsignado.Id_Contacto,
                    FechaCreacion = DateTime.Now,
                    IdTareaEspecialInterventoria = newTareaEspecial.Id_tareaEspecial
                };

                Negocio.PlanDeNegocioV2.Administracion.Interventoria.TareasEspeciales.TareaEspecial.InsertHistoria(newHistoriaTarea);
                
                if (Usuario.CodGrupo == Datos.Constantes.CONST_GerenteInterventor)
                    EnviarTarea(pago.CodProyecto.GetValueOrDefault(0), Usuario.IdContacto, interventorAsignado.Id_Contacto, "Asignación", "Tarea N° " +  newTareaEspecial.Id_tareaEspecial + " " + txtDescripcion.Text, newTareaEspecial.Id_tareaEspecial);

                Response.Redirect("~/PlanDeNegocioV2/Administracion/Interventoria/TareaEspecial/VerTareaEspecial.aspx?tareaEspecial=" + newTareaEspecial.Id_tareaEspecial);
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
        protected void EnviarTarea(int codigoProyecto, int remitente, int destinatario, string accion, string tarea, int idTarea)
        {
            AgendarTarea agenda = new AgendarTarea(
                                                  destinatario,
                                                  accion + " de tarea especial de interventoria " + tarea + " " + " - Proyecto " + codigoProyecto,
                                                  "Revisar tarea especial de interventoria ---> " + tarea + " Ver tarea en Tarea especiales de interventoria mediante este enlace.",
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
        protected void lnkSeleccionarPago_Click(object sender, EventArgs e)
        {
            fuArchivo.Visible = !fuArchivo.Visible;

            lnkSeleccionarPago.Text = !fuArchivo.Visible ? "+ Adjuntar Archivo (Opcional)" : "- No adjuntar Archivo";           
        }
        protected void EnviarTarea(int codigoProyecto, string tipoActividad, string accion, string nombreActividad)
        {            
            //if (codigoInterventor != null)
            //{
            //    AgendarTarea agenda = new AgendarTarea(
            //                                          codigoInterventor.GetValueOrDefault(),
            //                                          "Actividad de " + tipoActividad + " " + accion + " - Proyecto " + codigoProyecto,
            //                                          "Revisar actividad de " + tipoActividad + " - Actividad --> " + nombreActividad + "<br>Observaciones:</br> Aprobado por gerente interventor de manera masiva.",
            //    codigoProyecto.ToString(),
            //    2,
            //    "0",
            //    false,
            //    1,
            //    true,
            //    false,
            //    usuario.IdContacto,
            //    "CodProyecto=" + codigoProyecto,
            //    "",
            //    "Catálogo Actividad" + tipoActividad);

            //    agenda.Agendar();

            //}
        }

    }
}