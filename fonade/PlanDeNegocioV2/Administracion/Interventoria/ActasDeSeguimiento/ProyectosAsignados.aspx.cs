using Fonade.Clases;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Account;
using System.Web.Security;
using System.Linq;
using Datos;
using Fonade.Negocio.FonDBLight;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento
{
    public partial class ProyectosAsignados : System.Web.UI.Page
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
                if (!(Usuario.CodGrupo == Datos.Constantes.CONST_Interventor || Usuario.CodGrupo == Datos.Constantes.CONST_CoordinadorInterventor || Usuario.CodGrupo == Datos.Constantes.CONST_GerenteInterventor))
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
                if (cmbTipo.SelectedValue.Equals("id"))
                {
                    FieldValidate.ValidateNumeric("Código de proyecto", txtCodigo.Text, true);
                }
                else
                {
                    FieldValidate.ValidateString("Nombre de proyecto", txtCodigo.Text, true);
                }
                lblError.Visible = false;
                gvMain.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Sucedio un error detalle :" + ex.Message;
            }
        }

        public List<Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Empresas.ProyectoInterventoriaDTO> Get(string codigo, string tipo)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                try
                {
                    List<Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Empresas.ProyectoInterventoriaDTO>
                           list = new List<Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Empresas.ProyectoInterventoriaDTO>();

                    if (tipo.Equals("id"))
                    {
                        FieldValidate.ValidateNumeric("Código de proyecto", codigo, true);
                    }
                    else
                    {
                        FieldValidate.ValidateString("Nombre de proyecto", codigo, true);
                    }

                    if (tipo.Equals("id"))
                    {
                        list = Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Empresas.Empresa.GetProyectoAsignadoByInterventor(Convert.ToInt32(codigo), Usuario.IdContacto);

                        var query = list.Where(x => x.IsOwner == true).ToList();

                        return query;
                    }
                    else
                    {
                        list = Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Empresas.Empresa.GetProyectoAsignadoByInterventorAndNombre(codigo, Usuario.IdContacto);
                        var query = list.Where(x => x.IsOwner == true).ToList();
                        return query;
                    }
                }
                catch (ApplicationException ex)
                {
                    return new List<Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Empresas.ProyectoInterventoriaDTO>();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
        DatosActaDTOController datosActaDTOController = new DatosActaDTOController();
        protected void gvMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Ver"))
                {
                    if (e.CommandArgument != null)
                    {
                        var data = e.CommandArgument.ToString();
                        var codigoConvocatoria = Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(Convert.ToInt32(data), 0).GetValueOrDefault(0);
                        IndicadorGestionEvaluacion entidadIndicador = Negocio.PlanDeNegocioV2.Utilidad.IndicadorEvaluacion.GetIndicadores(Convert.ToInt32(data), codigoConvocatoria);

                        if (entidadIndicador == null)
                            throw new ApplicationException("El Proyecto ID: " + data + " no tiene información de indicadores, por favor actualicela.");

                        var datosInterventor = datosActaDTOController.GetContactosInterventor(Convert.ToInt32(data));
                        if (datosInterventor == null)
                            throw new ApplicationException("El Proyecto ID: " + data + " no tiene interventor asociado.");

                        var entidadInterv = datosActaDTOController.getInfoEntidadInteventora(Convert.ToInt32(data), Usuario.IdContacto);
                        if (entidadInterv == null)
                            throw new ApplicationException("El Proyecto ID: " + data + " no tiene contrato de interventoria asignado a este usuario.");

                        Response.Redirect("~/PlanDeNegocioV2/Administracion/Interventoria/ActasDeSeguimiento/GestionarActas.aspx?codigo=" + data);
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Sucedio un error detalle: " + ex.Message;
            }
        }
    }
}