using Fonade.Account;
using Fonade.Clases;
using Fonade.Negocio.PlanDeNegocioV2.Administracion.Interventoria.Reintegros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.TareaEspecial
{
    public partial class SeleccionarPago : System.Web.UI.Page
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
                    FieldValidate.ValidateNumeric("Código de pago", txtCodigo.Text, true);
                }

                gvMain.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Sucedio un error detalle :" + ex.Message;
            }
        }

        public List<ReintegroDTO> Get(string codigo, string tipo)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                try
                {
                    if (tipo.Equals("proyecto"))
                    {
                        FieldValidate.ValidateNumeric("Código de proyecto", codigo, true);
                    }
                    else
                    {
                        FieldValidate.ValidateNumeric("Código de pago", codigo, true);
                    }

                    if (tipo.Equals("proyecto"))
                    {
                        return Reintegro.GetPagosByProyecto(Usuario.CodOperador, Convert.ToInt32(codigo));
                    }
                    else
                    {
                        return Reintegro.GetPagosByPagoId(Usuario.CodOperador, Convert.ToInt32(codigo));
                    }
                }
                catch (ApplicationException ex)
                {
                    return new List<ReintegroDTO>();
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
                if (e.CommandName.Equals("Ver"))
                {
                    if (e.CommandArgument != null)
                    {
                        var codigoPago = Convert.ToInt32(e.CommandArgument.ToString());
                        Response.Redirect("~/PlanDeNegocioV2/Administracion/Interventoria/TareaEspecial/CrearTarea.aspx?codigo=" + codigoPago);
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