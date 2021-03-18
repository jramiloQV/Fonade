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

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.Entidad.Contrato
{
    public partial class UpdateContrato : System.Web.UI.Page
    {
        public int CodigoEntidad
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

        public int CodigoContrato
        {
            get
            {
                if (Request.QueryString["contrato"] != null)
                    return Convert.ToInt32(Request.QueryString["contrato"]);
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
            var entidad = Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Entidad.FirstOrDefault(CodigoEntidad);
            var contrato = Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Contratos.Contrato.FirstOrDefault(CodigoContrato);

            if (entidad != null && contrato != null) {
                lblMainTitle.Text = "Actualizar contrato a entidad " + entidad.Nombre;
                txtNumeroContrato.Text = contrato.NumeroContrato.Trim();
                txtFechaInicio.Text = contrato.FechaInicio.ToString("dd/MM/yyyy");
                txtFechaFin.Text = contrato.FechaTerminacion.ToString("dd/MM/yyyy");
            }                
        }
        
        public void ValidateUsers()
        {
            try
            {
                if (!(Usuario.CodGrupo == Datos.Constantes.CONST_GerenteInterventor || Usuario.CodGrupo == Datos.Constantes.CONST_AdministradorSistema))
                {
                    Response.Redirect("~/FONADE/evaluacion/AccesoDenegado.aspx");
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("~/FONADE/evaluacion/AccesoDenegado.aspx");
            }
        }

        protected void ValidarDatos()
        {
            FieldValidate.ValidateNumeric("Número de contrato", txtNumeroContrato.Text, true);
            FieldValidate.ValidateString("Fecha de inicio", txtFechaInicio.Text, true, 50);
            FieldValidate.ValidateString("Fecha de fin", txtFechaFin.Text, true, 50);
            FieldValidate.ValidateIsDateMayor("Fecha de inicio", DateTime.Parse(txtFechaInicio.Text), "Fecha fin", DateTime.Parse(txtFechaFin.Text));
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarDatos();

                var numeroContrato = txtNumeroContrato.Text;

                if (Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Contratos.Contrato.ExistContrato(numeroContrato, CodigoEntidad, CodigoContrato))
                    throw new ApplicationException("Existe un contrato de esta entidad con ese mismo nombre.");

                var currenEntity = new Datos.ContratoEntidad
                {
                    Id = CodigoContrato,
                    NumeroContrato = txtNumeroContrato.Text.Trim(),
                    FechaInicio = DateTime.Parse(txtFechaInicio.Text),
                    FechaTerminacion = DateTime.Parse(txtFechaFin.Text),
                    IdEntidad = CodigoEntidad,
                    UsuarioCreacion = Usuario.IdContacto                    
                };

                Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Contratos.Contrato.Update(currenEntity);

                Response.Redirect("~/PlanDeNegocioV2/Administracion/Interventoria/Entidad/Contrato/Contratos.aspx?codigo=" + CodigoEntidad, true);
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
            Response.Redirect("~/PlanDeNegocioV2/Administracion/Interventoria/Entidad/Contrato/Contratos.aspx?codigo=" + CodigoEntidad, true);
        }
    }
}