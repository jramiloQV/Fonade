using Fonade.Clases;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Account;
using System.Web.Security;
using Fonade.Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades;
using Datos;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.Entidad
{
    public partial class Entidades : System.Web.UI.Page
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
                if (Usuario.CodGrupo == Datos.Constantes.CONST_GerenteInterventor || Usuario.CodGrupo == Datos.Constantes.CONST_AdministradorSistema)
                    lnkSeleccionarPago.Visible = true;

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
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                FieldValidate.ValidateString("Nombre de entidad", txtCodigo.Text, false);

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
                        var id = Convert.ToInt32(e.CommandArgument.ToString());
                        Response.Redirect("~/PlanDeNegocioV2/Administracion/Interventoria/Entidad/UpdateEntidad.aspx?codigo=" + id);
                    }
                }

                if (e.CommandName.Equals("Contrato"))
                {
                    if (e.CommandArgument != null)
                    {
                        var id = Convert.ToInt32(e.CommandArgument.ToString());
                        Response.Redirect("~/PlanDeNegocioV2/Administracion/Interventoria/Entidad/Contrato/Contratos.aspx?codigo=" + id);
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Sucedio un error detalle :" + ex.Message;
            }
        }

        string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        public List<EntidadDTO> Get(string codigo, int startIndex, int maxRows)
        {
            try
            {
                List<EntidadDTO> list = new List<EntidadDTO>();

                FieldValidate.ValidateString("Nombre de entidad", codigo, false);

                var entidades = Negocio.PlanDeNegocioV2.Administracion.Interventoria
                    .Entidades.Entidad.Get(codigo, startIndex, maxRows, Usuario.CodOperador);

                var Operadores = Negocio.PlanDeNegocioV2.Administracion.Interventoria
                    .Entidades.Entidad.GetOperadores(Usuario.CodOperador);

                foreach(var e in entidades)
                {
                    EntidadDTO dto = new EntidadDTO();

                    dto.Id = e.Id;
                    dto.codOperador = e.codOperador;
                    dto.Dependencia = e.Dependencia;
                    dto.Direccion = e.Direccion;
                    dto.Email = e.Email;
                    dto.FechaActualizacion = e.FechaActualizacion;
                    dto.FechaCreacion = e.FechaCreacion;
                    dto.FechaPoliza = e.FechaPoliza;
                    dto.ImagenLogo = e.ImagenLogo;
                    dto.Nombre = e.Nombre;
                    dto.NombreCorto = e.NombreCorto;
                    dto.NumerpPoliza = e.NumeroPoliza;
                    dto.PersonaACargo = e.PersonaACargo;
                    dto.TelefonoCelular = e.TelefonoCelular;
                    dto.TelefonoOficina = e.TelefonoOficina;
                    dto.UsuarioCreacion = e.UsuarioCreacion;

                    foreach (var o in Operadores)
                    {
                        if (e.codOperador == o.IdOperador)
                        {
                            dto.Operador = o.NombreOperador;
                            break;
                        }
                    }

                    list.Add(dto);
                }

                return list;
            }
            catch (ApplicationException ex)
            {
                return new List<EntidadDTO>();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        //public List<Datos.EntidadInterventoria> Get(string codigo, int startIndex, int maxRows)
        //{
        //    try
        //    {                                        
        //        FieldValidate.ValidateString("Nombre de entidad", codigo, false);                
        //        return Negocio.PlanDeNegocioV2.Administracion.Interventoria
        //            .Entidades.Entidad.Get(codigo, startIndex, maxRows, Usuario.CodOperador);                
        //    }
        //    catch (ApplicationException ex)
        //    {
        //        return new List<Datos.EntidadInterventoria>();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }

        //}

        public int Count(string codigo)
        {
            try
            {
                FieldValidate.ValidateString("Nombre de entidad", codigo, false);
                return Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Entidad.Count(codigo);
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