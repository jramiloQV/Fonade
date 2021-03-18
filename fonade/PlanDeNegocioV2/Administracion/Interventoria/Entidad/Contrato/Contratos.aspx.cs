using Fonade.Clases;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Account;
using System.Web.Security;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.Entidad.Contrato
{
    public partial class Contratos : System.Web.UI.Page
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) {
                ValidateUsers();
                GetInfoEntidad();
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

        public void GetInfoEntidad() {
            var entidad = Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Entidad.FirstOrDefault(CodigoEntidad);

            if (entidad != null)
                lblMainTitle.Text = "Contratos de entidad : " + entidad.Nombre;
        }
        
        protected void gvMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Ver"))
                {
                    if (e.CommandArgument != null)
                    {
                        var data = e.CommandArgument.ToString().Split(';');

                        var idContrato = Convert.ToInt32(data[0]);
                        var idEntidad = Convert.ToInt32(data[1]);
                        
                        Response.Redirect("~/PlanDeNegocioV2/Administracion/Interventoria/Entidad/Contrato/UpdateContrato.aspx?contrato=" + idContrato+"&"+ "codigo=" + idEntidad);
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Sucedio un error detalle :" + ex.Message;
            }
        }

        public List<Datos.ContratoEntidad> Get(string codigo, string numeroContrato, int startIndex, int maxRows)
        {
            try
            {
                FieldValidate.ValidateString("Número de contrato", numeroContrato, false, 9);
                FieldValidate.ValidateNumeric("Número de contrato", numeroContrato, false);
                return Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Contratos.Contrato.Get(Convert.ToInt32(codigo), numeroContrato, startIndex, maxRows);
            }
            catch (ApplicationException ex)
            {
                return new List<Datos.ContratoEntidad>();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public int Count(string codigo, string numeroContrato)
        {
            try
            {
                FieldValidate.ValidateString("Número de contrato", numeroContrato, false, 9);
                FieldValidate.ValidateNumeric("Número de contrato", numeroContrato, false);
                return Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Contratos.Contrato.Count(Convert.ToInt32(codigo), numeroContrato);
            }
            catch (ApplicationException ex)
            {
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void lnkSeleccionarPago_Click(object sender, EventArgs e)
        {
            try
            {
                        Response.Redirect("~/PlanDeNegocioV2/Administracion/Interventoria/Entidad/Contrato/NewContrato.aspx?codigo=" + CodigoEntidad);
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Sucedio un error detalle :" + ex.Message;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                FieldValidate.ValidateString("Número de contrato", txtCodigo.Text, false,9);
                FieldValidate.ValidateNumeric("Número de contrato", txtCodigo.Text, false);

                gvMain.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Sucedio un error detalle :" + ex.Message;
            }
        }

    }
}