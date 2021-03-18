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

namespace Fonade.PlanDeNegocioV2.Evaluacion.Master
{
    public partial class Evaluacion : System.Web.UI.MasterPage
    {
        public int CodigoProyecto { get {
                return Convert.ToInt32(Request.QueryString["codproyecto"]);
            } set { } }
        public int? CodigoConvocatoria { get {
                return Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(CodigoProyecto, HttpContext.Current.Session["HistorialEvaluacion"] != null ? Convert.ToInt32(HttpContext.Current.Session["HistorialEvaluacion"]) : 0);
            } set { } }
        public String FechaInicioSesion
        {
            get
            {
                return DateTime.Now.getFechaConFormato();
            }
            set { }
        }
        public Boolean AllowCambiarProyecto
        {
            get
            {
                return Usuario.CodGrupo.Equals(Constantes.CONST_GerenteAdministrador)
                       || Usuario.CodGrupo.Equals(Constantes.CONST_AdministradorSistema)
                       || Usuario.CodGrupo.Equals(Constantes.CONST_CallCenter)
                       || Usuario.CodGrupo.Equals(Constantes.CONST_CallCenterOperador)
                       || Usuario.CodGrupo.Equals(Constantes.CONST_Interventor)
                       || Usuario.CodGrupo.Equals(Constantes.CONST_GerenteEvaluador);
            }
            set { }
        }

        public FonadeUser Usuario
        {
            get
            {
                return HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true);
            }
            set { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                int value;
                if (!int.TryParse(Request.QueryString["codproyecto"], out value))
                    throw new ApplicationException("No se encontro la información del proyecto, sera redireccionado al inicio de la aplicación para que lo intente de nuevo.");
                
                if (!Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.ProyectoExist(CodigoProyecto))
                    throw new Exception("No se logro obtener la información necesaria para continuar.");

                if (!Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.VerificarVersionProyecto(CodigoProyecto, Constantes.CONST_PlanV2))
                {
                    Response.Redirect("~/FONADE/evaluacion/EvaluacionFrameSet.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }

                if(CodigoConvocatoria == null)
                    throw new Exception("No se logro obtener la información necesaria para continuar.");

                if (!Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.ConvocatoriaExist(CodigoConvocatoria.GetValueOrDefault()))
                    throw new Exception("No se logro obtener la información necesaria para continuar.");

                GetProyectDetails();

                var esMienbro = Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.EsMienbroDelProyecto(CodigoProyecto, Usuario.IdContacto);

                if (!(AllowCambiarProyecto || esMienbro))
                    throw new ApplicationException("No tiene permiso para ver este proyecto");
                
            }
            catch (ApplicationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Advertencia : " + ex.Message + "');", true);
                Response.Redirect("~/FONADE/MiPerfil/Home.aspx");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error inesperado, sera redireccionado al inicio de la aplicación para que lo intente de nuevo. detalle :" + ex.Message + " ');", true);
                Response.Redirect("~/FONADE/MiPerfil/Home.aspx");
            }
        }

        protected void LoginStatus_LoggedOut(Object sender, System.EventArgs e)
        {
            MemoryCache.Default.Dispose();
            Session.Abandon();
        }

        protected void GetProyectDetails()
        {
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

                var Convocatoria = Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaDetails(CodigoConvocatoria.GetValueOrDefault());

                lbl_title.Text = entity.Id_Proyecto
                                 + " - " + entity.NomProyecto
                                 + " - " + entity.NomUnidad
                                 + " (" + entity.NomInstitucion + ")";
                img_lt.Src = "~/Images/ImgLT" + entity.CodEstado + ".jpg";
                img_map.Src = "~/Images/Mapas/" + entity.NomDepartamento.remplazarTilde() + "Pq.gif";
                img_map.Alt = entity.NomCiudad + "(" + entity.NomDepartamento + ")";
                link_map.HRef = "~/Mapas/Mapas.aspx?ver=1&pc=" + entity.Id_Departamento + "&pid=" + entity.Id_Proyecto;                
                lbl_convocatoria.Text = Convocatoria.NomConvocatoria +" - " + entity.NomSubSector;                
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

                Response.Redirect("~/PlanDeNegocioV2/Evaluacion/Master/MainMenu.aspx?codproyecto=" + txtCodigoProyecto.Text);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Advertencia : " + ex.Message + "');", true);
            }
        }
    }
}