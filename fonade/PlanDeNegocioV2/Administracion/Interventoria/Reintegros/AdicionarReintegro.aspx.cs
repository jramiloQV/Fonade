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

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.Reintegros
{
    public partial class AdicionarReintegro : System.Web.UI.Page
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
            set {
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) {
                GetInfoPago();

                ValidateUsers();
            }
        }

        public void GetInfoPago() {
            try
            {
                var pago = Pagos.GetInfoPago(CodigoPago);

                if (pago != null) {
                    lblMainTitle.Text = "Reintegros - pago " + pago.Id_PagoActividad + " del proyecto " + pago.CodProyecto.GetValueOrDefault();

                    lblValorPagoPostReintegro.Text = FieldValidate.moneyFormat(pago.CantidadDinero.GetValueOrDefault(0), true);
                    var presupuesto = Negocio.PlanDeNegocioV2.Interventoria.Interventoria.PresupuestoAprobadoInterventoria(pago.CodProyecto.GetValueOrDefault(0), null);

                    lblPresupuestoVigente.Text = FieldValidate.moneyFormat(presupuesto, true);
                    lblPresupuestoConReintegro.Text = FieldValidate.moneyFormat(presupuesto, true);
                }
                else
                {
                    Response.Redirect("~/PlanDeNegocioV2/Administracion/Interventoria/Reintegros/Reintegros.aspx");
                }
            }
            catch (ApplicationException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void ValidateUsers() {
            try
            {
                //Para validar si el gerente interventor tiene o no permisos para usar esta funcionalidad
                //se utiliza el campo AceptoTerminosYCondiciones para la validación.
                if(!(Usuario.CodGrupo == Datos.Constantes.CONST_GerenteInterventor && Usuario.AceptoTerminosYCondiciones))
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
                FieldValidate.ValidateNumeric("Consecutivo de reintegro", txtCodigoReintegro.Text, true);
                FieldValidate.ValidateString("Fecha de ingreso", txtFechaReintegro.Text, true);
                FieldValidate.ValidateNumeric("Valor del reintegro", txtValorReintegro.Text, true);
                FieldValidate.ValidateString("Observación", txtDescripcion.Text, true,250);
                if (!fuArchivo.HasFile) {
                    throw new ApplicationException("Es obligatorio subir el informe de interventoria.");
                }

                DateTime fechaReintegro = DateTime.Parse(txtFechaReintegro.Text);
                
                var pago = Pagos.GetInfoPago(CodigoPago);
                var valorReintegro = decimal.Parse(txtValorReintegro.Text.Trim().Replace(",", "").Replace(".", ","));

                if (valorReintegro > pago.CantidadDinero)
                    throw new ApplicationException("El valor del reintegro no puede ser mayor al valor del pago.");

                var presupuesto = Negocio.PlanDeNegocioV2.Interventoria.Interventoria.PresupuestoAprobadoInterventoria(pago.CodProyecto.GetValueOrDefault(0), null);

                var valorPagoConReintegro = pago.CantidadDinero.GetValueOrDefault(0) - valorReintegro;                
                var presupuestoConReintegro = Convert.ToDecimal(presupuesto) + valorReintegro;
                var rutaArchivo = UploadFile(pago.Id_PagoActividad, pago.CodProyecto.GetValueOrDefault(), fuArchivo);

                var entity = new Datos.Reintegro
                {
                    Consecutivo = Convert.ToInt32(txtCodigoReintegro.Text),
                    FechaIngreso = DateTime.Now,
                    ValorReintegro = valorReintegro,
                    Observacion = txtDescripcion.Text,
                    archivoInforme = rutaArchivo,
                    ValorPagoConReintegro = valorPagoConReintegro,
                    PresupuestoPreReintegro = Convert.ToDecimal(presupuesto),
                    PresupuestoPostReintegro = presupuestoConReintegro,
                    FechaReintegro = fechaReintegro,
                    codigoContacto = Usuario.IdContacto,
                    CodigoPago = pago.Id_PagoActividad
                };

                Reintegro.Insert(entity);
                Pagos.Reintegrar(pago.Id_PagoActividad, valorPagoConReintegro);

                gvReintegros.DataBind();

                cleanFields();
            }
            catch (ApplicationException ex)
            {
                lblError.Visible = true;
                lblError.Text = "Advertencia:" + ex.Message;
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Error inesperado :" + ex.Message;
            }
        }

        protected void cleanFields() {
            txtCodigoReintegro.Text = "";
            txtFechaReintegro.Text = "";
            txtValorReintegro.Text = "";
            txtDescripcion.Text = "";
            lblError.Visible = false;
        }
        protected string UploadFile(int codigoPago, int codigoProyecto, FileUpload _archivo) {            
            string directorioDestino = "Reintegros\\" + codigoProyecto + "\\pago_" + codigoPago.ToString() + "\\"; //Directorio destino archivo           
            string directorioBase = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual");
            string nombreArchivo = "InformeReintegro_" + codigoPago + "_" + codigoProyecto +"_"+ DateTime.Now.GetShortDateOnlyNumbersWithUnderscore()+ ".pdf";

            // ¿ Es valido el archivo ?
            if (!_archivo.HasFile)
                throw new ApplicationException("Archivo invalido");
            // ¿ Es image valida ? 
            if (_archivo.PostedFile.ContentType != "application/pdf")
                throw new ApplicationException("Adjunte un pdf, los demas tipos son invalidos.");
            // ¿ Pesa mas de diez megas ?
            if (!(_archivo.PostedFile.ContentLength < 10485760))
                throw new ApplicationException("El archivo es muy pesado, maximo 10 megas.");

            UploadFileToServer(_archivo, directorioBase, directorioDestino,nombreArchivo);

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

        protected void txtCodigoReintegro_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var pago = Pagos.GetInfoPago(CodigoPago);
                var valorReintegro = decimal.Parse(txtValorReintegro.Text.Trim().Replace(",","").Replace(".",","));

                if (valorReintegro > pago.CantidadDinero)
                    throw new ApplicationException("El valor del reintegro no puede ser mayor al valor del pago.");

                var presupuesto = Negocio.PlanDeNegocioV2.Interventoria.Interventoria.PresupuestoAprobadoInterventoria(pago.CodProyecto.GetValueOrDefault(0), null);

                var valorPago = pago.CantidadDinero.GetValueOrDefault(0) - valorReintegro;
                lblValorPagoPostReintegro.Text = FieldValidate.moneyFormat(valorPago);
                var presupuestoConReintegro = Convert.ToDecimal(presupuesto) + valorReintegro;

                lblPresupuestoConReintegro.Text = FieldValidate.moneyFormat(presupuestoConReintegro, true);
            }
            catch (ApplicationException ex)
            {
                lblError.Visible = true;
                lblError.Text = "Advertencia:" + ex.Message;
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Error inesperado :" + ex.Message;
            }
        }

        protected void gvReintegros_RowCommand(object sender, GridViewCommandEventArgs e)
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