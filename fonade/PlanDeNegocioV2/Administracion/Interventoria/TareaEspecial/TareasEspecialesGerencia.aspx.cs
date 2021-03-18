using Fonade.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Account;
using System.Web.Security;
using Fonade.Negocio.PlanDeNegocioV2.Administracion.Interventoria.TareasEspeciales;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.TareaEspecial
{
    public partial class TareasEspecialesGerencia : System.Web.UI.Page
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
                if (Usuario.CodGrupo == Datos.Constantes.CONST_GerenteInterventor)
                    lnkSeleccionarPago.Visible = true;
                
                if (!(Usuario.CodGrupo == Datos.Constantes.CONST_GerenteInterventor || Usuario.CodGrupo == Datos.Constantes.CONST_Interventor))
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
                    FieldValidate.ValidateNumeric("Código de proyecto", txtCodigo.Text, false);
                }
                else
                {
                    FieldValidate.ValidateNumeric("Código de pago", txtCodigo.Text, false);
                }

                gvMain.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Sucedio un error detalle :" + ex.Message;
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
                        var codigoTarea = Convert.ToInt32(e.CommandArgument.ToString());
                        Response.Redirect("~/PlanDeNegocioV2/Administracion/Interventoria/TareaEspecial/VerTareaEspecial.aspx?tareaEspecial=" + codigoTarea);
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Sucedio un error detalle :" + ex.Message;
            }
        }

        public List<TareaEspecialDTO> Get(string codigo, string tipo, string estado, int startIndex, int maxRows)
        {
            try
            {
                var estadoTarea = String.Equals(estado, "pending");
                if (tipo.Equals("proyecto"))
                {
                    FieldValidate.ValidateNumeric("Código de proyecto", codigo, false);
                }
                else
                {
                    FieldValidate.ValidateNumeric("Código de pago", codigo, false);
                }

                if (tipo.Equals("proyecto"))
                {
                    return Negocio.PlanDeNegocioV2.Administracion.Interventoria.TareasEspeciales.TareaEspecial.GetTareasEspecialesByProyectoId(Convert.ToInt32(codigo), Usuario.IdContacto, estadoTarea, startIndex, maxRows, Usuario.CodOperador);
                }
                else
                {
                    return Negocio.PlanDeNegocioV2.Administracion.Interventoria.TareasEspeciales.TareaEspecial.GetTareasEspecialesByPagoId(Convert.ToInt32(codigo), Usuario.IdContacto, estadoTarea, startIndex, maxRows, Usuario.CodOperador);
                }
            }
            catch (ApplicationException ex)
            {
                return new List<TareaEspecialDTO>();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public int Count(string codigo, string tipo, string estado)
        {
            try
            {
                var estadoTarea = String.Equals(estado, "pending");

                if (tipo.Equals("proyecto"))
                {
                    FieldValidate.ValidateNumeric("Código de proyecto", codigo, false);
                }
                else
                {
                    FieldValidate.ValidateNumeric("Código de pago", codigo, false);
                }

                if (tipo.Equals("proyecto"))
                {
                    return Negocio.PlanDeNegocioV2.Administracion.Interventoria.TareasEspeciales.TareaEspecial.CountTareasEspecialesByProyectoId(Convert.ToInt32(codigo), Usuario.IdContacto, estadoTarea, Usuario.CodOperador);
                }
                else
                {
                    return Negocio.PlanDeNegocioV2.Administracion.Interventoria.TareasEspeciales.TareaEspecial.CountTareasEspecialesByPagoId(Convert.ToInt32(codigo), Usuario.IdContacto, estadoTarea, Usuario.CodOperador);
                }
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
    }
}