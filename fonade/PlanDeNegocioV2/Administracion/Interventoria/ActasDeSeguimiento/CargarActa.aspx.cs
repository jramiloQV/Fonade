using Fonade.Account;
using Datos;
using Fonade.Clases;
using Fonade.Negocio.FonDBLight;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento
{
    public partial class CargarActa : System.Web.UI.Page
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
        public int CodigoActa
        {
            get
            {
                if (Request.QueryString["acta"] != null)
                    return Convert.ToInt32(Request.QueryString["acta"]);
                else
                    return 0;
            }
            set { }
        }

        public int CodigoProyecto
        {
            get
            {
                if (Session["idProyecto"] != null)
                    return Convert.ToInt32(Session["idProyecto"].ToString());
                else
                    return 0;
            }
            set { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {            
        }
        ActaSeguimientoInterventoriaController interventoriaController = new ActaSeguimientoInterventoriaController();
        protected void btnAdicionar_Click(object sender, EventArgs e)
        {
            try
            {
                var acta = new ActaSeguimientoInterventoria();
                if (CodigoActa == 0)
                {
                    acta = Negocio.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimientos.ActaSeguimiento.GetActaById(CodigoActa, CodigoProyecto);
                }
                else
                {
                    acta = interventoriaController.GetActaInterventoria(CodigoProyecto, CodigoActa);
                }
                

                if (acta == null)
                    throw new ApplicationException("No se logro encontrar información de esta acta");
                if (!string.IsNullOrEmpty(acta.ArchivoActa))
                    throw new ApplicationException("Ya fue cargado un archivo para esta acta");

                string tipoActa = "Acta de Seguimiento";

                if(CodigoActa == 0)
                    tipoActa = "Acta de Inicio";
                else
                    tipoActa = "Acta de Seguimiento";

                var rutaArchivo = UploadFile(CodigoActa, acta.IdProyecto, fuArchivo, tipoActa);

                acta.ArchivoActa = rutaArchivo;
                acta.FechaActualizacion = DateTime.Now;

                Negocio.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimientos.ActaSeguimiento.InsertOrUpdateActa(acta);
                Negocio.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimientos.ActaSeguimiento.InsertActaContrato(acta.IdProyecto, "Documentos//" + rutaArchivo, Usuario.IdContacto);

                Alert("Archivo cargado correctamente");

                CloseAndRefreshParent();
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Sucedio un error detalle :" + ex.Message;
            }           
        }

        private void Alert(string mensaje)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
        }

        protected string UploadFile(int codigoActa, int codigoProyecto, FileUpload _archivo,string tipoActa)
        {
            int hashDirectorioUsuario = codigoProyecto / 2000;            
            string directorioDestino = "ActasSeguimiento//" + hashDirectorioUsuario + "//Actas_" + codigoProyecto + "//"; //Directorio destino archivo           
            string directorioBase = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual");

            //string nombreArchivo = tipoActa.Trim().Replace(' ', '_') + "_NO_" + codigoActa + "_" + codigoProyecto + "_" + DateTime.Now.GetShortDateOnlyNumbersWithUnderscore() + ".pdf";
            string nombreArchivo = "";
            string numContrato = GetNumContrato(codigoProyecto);
            if (codigoActa == 0)
            {
                nombreArchivo = codigoProyecto+"-"+numContrato.Trim()+" Acta de Inicio" + ".pdf";
            }
            else
            {
                nombreArchivo = codigoProyecto + "-" + numContrato.Trim() + " Acta de Seguimiento No " +codigoActa + ".pdf";
            }
            

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

        public string GetNumContrato(int _codProyecto)
        {
            try
            {
                var contrato = Negocio.PlanDeNegocioV2.Administracion.Interventoria.Abogado.GetContratoByProyecto(_codProyecto, Usuario.CodOperador);

                if (contrato.Any())
                {
                    if (!contrato.First().HasInfoCompleted)
                    {
                        throw new ApplicationException("Este proyecto no tiene información de contratos completa");                        
                    }
                    else
                    {
                        var infoContrato = contrato.First().Contrato;                     

                        return infoContrato.NumeroContrato;
                    }
                }
                else
                {
                    return "";
                }
            }
            catch (ApplicationException ex)
            {
                btnAdicionar.Visible = false;
                lblError.Visible = true;
                lblError.Text = "Advertencia: " + ex.Message;
                return "";
            }
            catch (Exception ex)
            {
                btnAdicionar.Visible = false;
                lblError.Visible = true;
                lblError.Text = "Error inesperado: " + ex.Message;
                return "";
            }
        }

        protected void UploadFileToServer(FileUpload _archivo, string _directorioBase, string _directorioDestino, string fileName)
        {
            // ¿ Carpeta de destino existe ?
            if (!Directory.Exists(_directorioBase + _directorioDestino))
                Directory.CreateDirectory(_directorioBase + _directorioDestino);
            if (File.Exists(_directorioBase + _directorioDestino + _archivo.FileName))
                File.Delete(_directorioBase + _directorioDestino + _archivo.FileName);

            _archivo.SaveAs(_directorioBase + _directorioDestino + fileName);
        }

        protected void CloseAndRefreshParent()
        {            
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Salida por solicitud del usuario", "window.opener.location=window.opener.location;window.close();", true);
        }
    }
}