using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Runtime.Caching;
using Datos;
using Fonade.Clases;
using Fonade.Account;
using System.Web.Security;
using System.Web.UI.WebControls;
using Fonade.Negocio.PlanDeNegocioV2.Administracion.Operador;
using Fonade.Negocio.Proyecto;

namespace Fonade.PlanNegocioV2.Formulacion.Master
{
    public partial class Proyecto : System.Web.UI.MasterPage
    {
        
        public int CodigoProyecto { get; set; }    
        public String FechaInicioSesion {
            get {
                return DateTime.Now.getFechaConFormato();
            } set { }
        }
        public Boolean AllowCambiarProyecto {
            get {
                return Usuario.CodGrupo.Equals(Constantes.CONST_GerenteAdministrador)
                       || Usuario.CodGrupo.Equals(Constantes.CONST_AdministradorSistema)
                       || Usuario.CodGrupo.Equals(Constantes.CONST_CallCenter)
                       || Usuario.CodGrupo.Equals(Constantes.CONST_CallCenterOperador)
                       || Usuario.CodGrupo.Equals(Constantes.CONST_GerenteInterventor)
                       || Usuario.CodGrupo.Equals(Constantes.CONST_CoordinadorInterventor)
                       || Usuario.CodGrupo.Equals(Constantes.CONST_Interventor)
                       || Usuario.CodGrupo.Equals(Constantes.CONST_GerenteEvaluador)
                       || Usuario.CodGrupo.Equals(Constantes.CONST_CoordinadorEvaluador)
                       || Usuario.CodGrupo.Equals(Constantes.CONST_PerfilAbogado);
            }
            set { }
        }

        public Boolean AllowShowAllProjects
        {
            get
            {
                return  Usuario.CodGrupo.Equals(Constantes.CONST_LiderRegional)
                        || Usuario.CodGrupo.Equals(Constantes.CONST_JefeUnidad)
                        || Usuario.CodGrupo.Equals(Constantes.CONST_PerfilAbogado)
                        || Usuario.CodGrupo.Equals(Constantes.CONST_PerfilAcreditador);
            }
            set { }
        }

        public FonadeUser Usuario {
            get {
                return HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true); }
            set { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                int value;
                if (!int.TryParse(Request.QueryString["codproyecto"], out value))
                    throw new ApplicationException("No se encontro la información del proyecto, sera redireccionado al inicio de la aplicación para que lo intente de nuevo.");

                CodigoProyecto = Convert.ToInt32(Request.QueryString["codproyecto"]);

                if (!Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.ProyectoExist(CodigoProyecto))
                    throw new Exception("No se logro obtener la información necesaria para continuar.");

                if (!Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.VerificarVersionProyecto(CodigoProyecto, Constantes.CONST_PlanV2))
                {
                    HttpContext.Current.Session["CodProyecto"] = CodigoProyecto;
                    Response.Redirect("~/FONADE/Proyecto/ProyectoFrameSet.aspx", false);
                    return;                    
                }                    

                GetProyectDetails();

                var esMienbro = Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.EsMienbroDelProyecto(CodigoProyecto, Usuario.IdContacto);

                if (!(AllowCambiarProyecto || esMienbro || AllowShowAllProjects))
                    throw new ApplicationException("No tiene permiso para ver este proyecto");

                validarDocumentosCompletos();

                validarTerminosSCD();
            }
            catch (ApplicationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + ex.Message + ".');", true);

                if (ex.Message == "Debe aceptar los términos y condiciones de Servicio de Certificación Digital")
                {
                    
                        String url = "~/PlanDeNegocioV2/Ejecucion/TerminosSCD/TerminosSCD.aspx";
                        Response.Redirect(url);
                    
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Advertencia : " + ex.Message + "');", true);
                    Response.Redirect("~/FONADE/MiPerfil/Home.aspx");
                }
                
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error inesperado, sera redireccionado al inicio de la aplicación para que lo intente de nuevo. detalle :" + ex.Message + " ');", true);
                Response.Redirect("~/FONADE/MiPerfil/Home.aspx");
            }                                                                                 
        }

        private bool validarTerminosSCDXProyecto(int _codUsuario)
        {
            ProyectoController proyectoController = new ProyectoController();
            return proyectoController.validarTerminosSCDProyecto(_codUsuario);
        }

        private void validarTerminosSCD()
        {
            
            if (Usuario.CodGrupo == Constantes.CONST_Emprendedor)
            {
                
                    if(validarTerminosSCDXProyecto(Usuario.IdContacto))
                        throw new ApplicationException("Debe aceptar los términos y condiciones de Servicio de Certificación Digital");
                             
            }
        }

        protected void LoginStatus_LoggedOut(Object sender, System.EventArgs e)
        {
            MemoryCache.Default.Dispose();
            Session.Abandon();
        }

        protected void GetProyectDetails() {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from p in db.Proyecto
                              from i in db.Institucions
                              from s in db.SubSector
                              from c in db.Ciudad
                              from d in db.departamento
                              where
                                 p.Id_Proyecto.Equals(CodigoProyecto)
                                 && s.Id_SubSector == p.CodSubSector
                                 && i.Id_Institucion == p.CodInstitucion
                                 && p.CodCiudad == c.Id_Ciudad
                                 && c.CodDepartamento == d.Id_Departamento
                              select new
                              {
                                  p.Id_Proyecto,
                                  p.NomProyecto,
                                  s.NomSubSector,
                                  i.NomUnidad,
                                  c.NomCiudad,
                                  d.NomDepartamento,
                                  d.Id_Departamento,
                                  i.NomInstitucion,
                                  p.CodEstado
                              }).FirstOrDefault();
                if (entity == null)
                    throw new ApplicationException("No se encontro la información del proyecto, sera redireccionado al inicio de la aplicación para que lo intente de nuevo.");

                lbl_title.Text = entity.Id_Proyecto
                                 + " - " + entity.NomProyecto
                                 + " - " + entity.NomUnidad
                                 + " (" + entity.NomInstitucion + ")";
                img_lt.Src = "~/Images/ImgLT" + entity.CodEstado + ".jpg";
                img_map.Src = "~/Images/Mapas/" + entity.NomDepartamento.remplazarTilde() + "Pq.gif";
                img_map.Alt = entity.NomCiudad + "(" + entity.NomDepartamento + ")";
                link_map.HRef = "~/Mapas/Mapas.aspx?ver=1&pc=" + entity.Id_Departamento + "&pid=" + entity.Id_Proyecto;
                lbl_convocatoria.Text = lbl_convocatoria.Text + entity.NomSubSector;

                Session["TituloProyectoMaster"] = lbl_title.Text;
            }                     
        }
        
        protected void validarDocumentosCompletos()
        {            
            ContentPlaceHolder contenidoMasterPage = (ContentPlaceHolder)FindControl("bodyHolder");
            string paginaActual = contenidoMasterPage.Page.ToString();

            if (Usuario.CodGrupo == Constantes.CONST_Emprendedor)
            {               
                try
                {
                    using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                    {
                        var documentos = db.ContactoArchivosAnexos.Where(filter => filter.CodContacto.Equals(Usuario.IdContacto)).ToList();

                        if (!documentos.Any(filter => filter.TipoArchivo.Equals("FotocopiaDocumento")))
                            throw new ApplicationException("Debe adjuntar fotocopia de cedula");

                        if (!documentos.Any(filter => filter.TipoArchivo.Equals("CertificacionEstudios")))
                            throw new ApplicationException("Debe adjuntar Certificado de estudios");
                    }
                }
                catch (ApplicationException ex)
                {
                    String url = "~/Fonade/MiPerfil/MiPerfil.aspx";
                    Response.Redirect(url);
                }
            }
        }
                              
        protected void btnBuscarProyecto_Click(object sender, EventArgs e)
        {
            try
            {
                FieldValidate.ValidateString("Codigo de proyecto", txtCodigoProyecto.Text, true);

                int value;
                if (!int.TryParse(txtCodigoProyecto.Text, out value))
                    throw new ApplicationException("El codigo del proyecto debe ser numerico.");

                if (!operadorController.validarOperadorXProyecto(Usuario.CodOperador, value))
                    throw new ApplicationException("No tiene permisos para ver este proyecto.");

                Response.Redirect("~/PlanDeNegocioV2/Formulacion/Master/MainMenu.aspx?codproyecto=" + txtCodigoProyecto.Text);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Advertencia : " + ex.Message + "');", true);
            }
        }

        OperadorController operadorController = new OperadorController();
    }
}