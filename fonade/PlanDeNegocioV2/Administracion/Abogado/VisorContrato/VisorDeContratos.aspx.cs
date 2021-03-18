using Fonade.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Account;
using System.Web.Security;

namespace Fonade.PlanDeNegocioV2.Administracion.Abogado.VisorContrato
{
    public partial class VisorDeContratos : System.Web.UI.Page
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                ValidateUsers();
        }
        public void ValidateUsers()
        {
            try
            {
                //Para validar si el gerente interventor tiene o no permisos para usar esta funcionalidad
                //se utiliza el campo AceptoTerminosYCondiciones para la validación.
                if (!(Usuario.CodGrupo == Datos.Constantes.CONST_PerfilAbogado || Usuario.CodGrupo == Datos.Constantes.CONST_AdministradorSistema))
                {
                    Response.Redirect("~/FONADE/evaluacion/AccesoDenegado.aspx");
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("~/FONADE/evaluacion/AccesoDenegado.aspx");
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbTipo.SelectedValue.Equals("proyecto"))
                {
                    FieldValidate.ValidateNumeric("Código de proyecto", txtCodigo.Text, true);
                }
                else
                {
                    FieldValidate.ValidateNumeric("Número de contrato", txtCodigo.Text, true);
                }

                gvMain.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Sucedio un error detalle :" + ex.Message;
            }
        }

        public List<Negocio.PlanDeNegocioV2.Administracion.Interventoria.ContratoEmpresaDTO> Get(string codigo, string tipo)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                try
                {
                    if (tipo.Equals("proyecto"))
                    {
                        
                        FieldValidate.ValidateNumeric("Código de proyecto", codigo, false);
                    }
                    else
                    {
                        FieldValidate.ValidateNumeric("Código de pago", codigo, false);
                    }

                    if (codigo != null) {
                        if (tipo.Equals("proyecto"))
                        {
                            return Negocio.PlanDeNegocioV2.Administracion.Interventoria.Abogado.GetContratoByProyecto(Convert.ToInt32(codigo), Usuario.CodOperador);
                        }
                        else
                        {
                            return Negocio.PlanDeNegocioV2.Administracion.Interventoria.Abogado.GetContratoByContratoNumber(codigo, Usuario.CodOperador);
                        }
                    }                    
                    else
                    {
                        return Negocio.PlanDeNegocioV2.Administracion.Interventoria.Abogado.GetContratoLast50(Usuario.CodOperador);
                    }
                }
                catch (ApplicationException ex)
                {
                    return new List<Negocio.PlanDeNegocioV2.Administracion.Interventoria.ContratoEmpresaDTO>();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        protected void gvMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Editar"))
                {
                    if (e.CommandArgument != null)
                    {
                        var idCodigo = e.CommandArgument.ToString();
                        
                        Response.Redirect("~/PlanDeNegocioV2/Administracion/Abogado/VisorContrato/UpdateContrato.aspx?codigo=" + idCodigo);
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Sucedio un error detalle :" + ex.Message;
            }
        }
    }
}