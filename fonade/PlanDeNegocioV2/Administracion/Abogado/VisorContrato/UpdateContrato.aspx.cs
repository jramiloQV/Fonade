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

namespace Fonade.PlanDeNegocioV2.Administracion.Abogado.VisorContrato
{
    public partial class UpdateContrato : System.Web.UI.Page
    {
        public int CodigoProyecto
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
                ValidateUsers();
                GetInfo();

            }
        }
        public void GetInfo()
        {           
            var entity = Negocio.PlanDeNegocioV2.Administracion.Interventoria.Abogado.GetContratoByProyecto(CodigoProyecto, Usuario.CodOperador).FirstOrDefault();

            if (entity != null)
            {
                lblMainTitle.Text = "Actualizar contrato a  proyecto " + CodigoProyecto;

                txtNumeroContrato.Text = entity.Contrato.NumeroContrato != null ? entity.Contrato.NumeroContrato.Trim() : "";
                txtFechaFirmaContrato.Text = entity.Contrato.FechaFirmaDelContrato.GetValueOrDefault(DateTime.Now).ToString("dd/MM/yyyy");
                lblFechaInicio.Text = entity.Contrato.FechaDeInicioContrato != null ? entity.Contrato.FechaDeInicioContrato.GetValueOrDefault().ToString("dd/MM/yyyy") : "Sin fecha de inicio";
                lblObjeto.Text = entity.Contrato.ObjetoContrato != null ? entity.Contrato.ObjetoContrato.Trim() : "";
                txtCertificadoDisponibilidad.Text = entity.Contrato.CertificadoDisponibilidad.GetValueOrDefault(0).ToString();
                txtFechaCertificadoDisponibilidad.Text = entity.Contrato.FechaCertificadoDisponibilidad.GetValueOrDefault(DateTime.Now).ToString("dd/MM/yyyy");
                lblPlazoInicialContrato.Text = entity.Contrato.PlazoContratoMeses.GetValueOrDefault(0).ToString();
                txtNumeroActaConcejoDirectivo.Text = entity.Contrato.NumeroActaConcejoDirectivo.ToString().Trim();
                txtFechaActaConcejoDirectivo.Text = entity.Contrato.FechaActaConcejoDirectivo != null ? entity.Contrato.FechaActaConcejoDirectivo.GetValueOrDefault().ToString("dd/MM/yyyy") : "";
                txtValorEnte.Text = entity.Contrato.ValorEnte.GetValueOrDefault(0).ToString();
                txtValorSena.Text = entity.Contrato.Valorsena.GetValueOrDefault(0).ToString();
                txtNumeroPoliza.Text = entity.Contrato.NumeroPoliza != null ? entity.Contrato.NumeroPoliza.Trim() : "";                
                txtValorInicial.Text = entity.Contrato.ValorInicialEnPesos.GetValueOrDefault(0).ToString();
                txtTipoContrato.Text = entity.Contrato.TipoContrato != null ? entity.Contrato.TipoContrato.Trim() : "";
                txtEstado.Text = entity.Contrato.Estado != null ? entity.Contrato.Estado.Trim() : "";


                var emprendedores = Negocio.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimientos.ActaSeguimiento.GetEmprendedoresYEquipoTrabajo(CodigoProyecto);
                string contratistas = string.Empty;
                string contratistasTelefono = string.Empty;
                string contratistasEmail = string.Empty;
                if (!emprendedores.Any())
                    contratistas = contratistasTelefono = contratistasEmail = "Sin emprendedores";

                foreach (var emprendedor in emprendedores)
                {
                    if (string.IsNullOrEmpty(contratistas))
                    {
                        contratistas += emprendedor.Nombres + "-" + emprendedor.Identificacion;
                        contratistasTelefono += emprendedor.Telefono;
                        contratistasEmail += emprendedor.Email;
                    }
                    else
                    {
                        contratistas += "," + emprendedor.Nombres + "-" + emprendedor.Identificacion;
                        contratistasTelefono += "," + emprendedor.Telefono;
                        contratistasEmail += "," + emprendedor.Email;
                    }
                }

                lblEmprendedores.Text = contratistas;
                lblTelefono.Text = contratistasTelefono;
                lblEmail.Text = contratistasEmail;

                var prorroga = Negocio.PlanDeNegocioV2.Administracion.Interventoria.Abogado.GetProrroga(CodigoProyecto);
                lblProrroga.Text = prorroga.ToString();

                lblFechaFinalContrato.Text = entity.Contrato.FechaDeInicioContrato != null ? entity.Contrato.FechaDeInicioContrato.GetValueOrDefault().AddMonths(prorroga).ToString("dd/MM/yyyy") : "Ninguna"; 
            }
            else
            {
                btnAdicionar.Visible = false;
            }
        }

        public void ValidateUsers()
        {
            //try
            //{
            //    if (!(Usuario.CodGrupo == Datos.Constantes.CONST_GerenteInterventor))
            //    {
            //        Response.Redirect("~/FONADE/evaluacion/AccesoDenegado.aspx");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Response.Redirect("~/FONADE/evaluacion/AccesoDenegado.aspx");
            //}
        }

        protected void ValidarDatos()
        {
            FieldValidate.ValidateString("Número de contrato", txtNumeroContrato.Text, true,10);
            FieldValidate.ValidateNumeric("Número de contrato", txtNumeroContrato.Text, true);
            FieldValidate.ValidateString("Fecha firma contrato", txtFechaFirmaContrato.Text, true);            
            FieldValidate.ValidateIsDateMayor("Fecha firma contrato", DateTime.Parse(txtFechaFirmaContrato.Text), "Fecha del día de hoy", DateTime.Today);
            FieldValidate.ValidateString("Certificado de disponibilidad", txtCertificadoDisponibilidad.Text, true, 5);
            FieldValidate.ValidateNumeric("Certificado de disponibilidad", txtCertificadoDisponibilidad.Text, true);            
            FieldValidate.ValidateString("Fecha de certificado de disponibilidad", txtFechaCertificadoDisponibilidad.Text, true);
            FieldValidate.ValidateString("Número de acta de concejo directivo", txtNumeroActaConcejoDirectivo.Text, true, 10);
            FieldValidate.ValidateNumeric("Número de acta de concejo directivo", txtNumeroActaConcejoDirectivo.Text, true);
            FieldValidate.ValidateString("Fecha de acta de concejo directivo", txtFechaActaConcejoDirectivo.Text, true);
            FieldValidate.ValidateString("Valor Ente", txtValorEnte.Text, true,13);
            FieldValidate.ValidateNumeric("Valor Ente", txtValorEnte.Text, true);
            FieldValidate.ValidateString("Valor Sena", txtValorSena.Text, true,13);
            FieldValidate.ValidateNumeric("Valor Sena", txtValorSena.Text, true);
            FieldValidate.ValidateString("Número poliza seguro de vida", txtNumeroPoliza.Text, true,15);                        
            FieldValidate.ValidateNumeric("Valor inicial en pesos", txtValorInicial.Text, true);
            FieldValidate.ValidateString("Tipo de contrato", txtTipoContrato.Text, true);
            FieldValidate.ValidateString("Estado", txtEstado.Text, true);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarDatos();
                var entity = Negocio.PlanDeNegocioV2.Administracion.Interventoria.Abogado.GetContratoByProyecto(CodigoProyecto, Usuario.CodOperador).FirstOrDefault();

                string[] estadosContrato = new string[] { "Condonado", "Terminado", "No condonado", "En evaluación de indicadores", "Liquidados", "Con ejecución de recursos", "Sin ejecución de Recursos", "Legalización" };

                if (!estadosContrato.Contains(txtEstado.Text.Trim().Trim(), StringComparer.InvariantCultureIgnoreCase))
                    throw new ApplicationException("Estado invalido, solo es permitido: Condonado,Terminado,No condonado,En evaluación de indicadores,Liquidados,Con ejecución de recursos,Sin ejecución de Recursos,Legalización");

                if (Negocio.PlanDeNegocioV2.Administracion.Interventoria.Abogado.ExistContrato(txtNumeroContrato.Text, CodigoProyecto))
                    throw new ApplicationException("El número contrato ya existe para otro proyecto.");
                
                entity.Contrato.NumeroContrato = txtNumeroContrato.Text.Trim();
                entity.Contrato.FechaFirmaDelContrato = DateTime.Parse(txtFechaFirmaContrato.Text);                                
                entity.Contrato.CertificadoDisponibilidad = Convert.ToInt32(txtCertificadoDisponibilidad.Text);
                entity.Contrato.FechaCertificadoDisponibilidad = DateTime.Parse(txtFechaCertificadoDisponibilidad.Text);                
                entity.Contrato.NumeroActaConcejoDirectivo = Convert.ToInt32(txtNumeroActaConcejoDirectivo.Text);                
                entity.Contrato.ValorEnte = Convert.ToDecimal(txtValorEnte.Text);
                entity.Contrato.Valorsena = Convert.ToDecimal(txtValorSena.Text);
                entity.Contrato.NumeroPoliza = txtNumeroPoliza.Text.Trim();                
                entity.Contrato.ValorInicialEnPesos = Convert.ToDecimal(txtValorInicial.Text);
                entity.Contrato.TipoContrato = txtTipoContrato.Text.Trim();
                entity.Contrato.Estado = txtEstado.Text.Trim();
                entity.Contrato.FechaActaConcejoDirectivo = DateTime.Parse(txtFechaActaConcejoDirectivo.Text);

                Negocio.PlanDeNegocioV2.Administracion.Interventoria.Abogado.UpdateExtension(entity.Contrato);

                Response.Redirect("~/PlanDeNegocioV2/Administracion/Abogado/VisorContrato/VisorDeContratos.aspx", true);               
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

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PlanDeNegocioV2/Administracion/Abogado/VisorContrato/VisorDeContratos.aspx", true);
        }
    }
}