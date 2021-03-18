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

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento
{
    public partial class CrearActaInicio : System.Web.UI.Page
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
            try
            {
                var contrato = Negocio.PlanDeNegocioV2.Administracion.Interventoria.Abogado.GetContratoByProyecto(CodigoProyecto, Usuario.CodOperador);

                if (contrato.Any())
                {
                    if (!contrato.First().HasInfoCompleted)
                    {
                        throw new ApplicationException("Este proyecto no tiene información de contratos completa");
                    }
                    else
                    {
                        var infoContrato = contrato.First().Contrato;

                        var emprendedores = Negocio.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimientos.ActaSeguimiento.GetEmprendedoresYEquipoTrabajo(CodigoProyecto);

                        if (!emprendedores.Any())
                            throw new ApplicationException("Este proyecto no tiene emprendedores activos.");

                        string contratistas = string.Empty;
                        foreach (var emprendedor in emprendedores)
                        {                            
                            if (string.IsNullOrEmpty(contratistas))
                            {
                                contratistas += emprendedor.Nombres + "-" + emprendedor.Identificacion;
                            }
                            else
                            {
                                contratistas += "," + emprendedor.Nombres + "-" + emprendedor.Identificacion;
                            }
                        }

                        lblNumeroContrato.Text = infoContrato.NumeroContrato;
                        lblTipoDeContrato.Text = infoContrato.TipoContrato;
                        lblObjeto.Text = infoContrato.ObjetoContrato;
                        lblValor.Text = FieldValidate.moneyFormat((double)infoContrato.ValorInicialEnPesos.GetValueOrDefault(0), true);
                        lblPlazo.Text = infoContrato.PlazoContratoMeses + " Meses";
                        lblContratistas.Text = contratistas;
                    }
                }
            }
            catch (ApplicationException ex)
            {
                btnAdicionar.Visible = false;
                lblError.Visible = true;
                lblError.Text = "Advertencia: " + ex.Message;
            }
            catch (Exception ex)
            {
                btnAdicionar.Visible = false;
                lblError.Visible = true;
                lblError.Text = "Error inesperado: " + ex.Message;
            }           
        }

        public void ValidateUsers()
        {
            try
            {
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
       
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var newEntity = new Datos.ActaSeguimientoInterventoria
                {
                    Nombre = "Acta de inicio",
                    IdTipoActa = Datos.Constantes.const_actaSeguimientoInicio,
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now,
                    IdUsuarioCreacion = Usuario.IdContacto,
                    Publicado = false,
                    IdProyecto = CodigoProyecto,
                    NumeroActa = 0
                };

                Negocio.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimientos.ActaSeguimiento.InsertOrUpdateActa(newEntity);

                Response.Redirect("~/PlanDeNegocioV2/Administracion/Interventoria/ActasDeSeguimiento/ActaInicio.aspx?acta=" + newEntity.NumeroActa+"&codigo="+newEntity.IdProyecto, true);
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
            Response.Redirect("~/PlanDeNegocioV2/Administracion/Interventoria/ActasDeSeguimiento/GestionarActas.aspx?codigo=" + CodigoProyecto, true);
        }
    }
}