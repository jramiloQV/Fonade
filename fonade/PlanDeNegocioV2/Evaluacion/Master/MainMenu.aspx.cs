using System;
using Fonade.Negocio.PlanDeNegocioV2.Utilidad;
using Datos;
using System.Linq;
using System.Web.UI;
using Fonade.Account;
using System.Web.Security;
using System.Web;

namespace Fonade.PlanDeNegocioV2.Evaluacion.Master
{
    public partial class MainMenu : System.Web.UI.Page
    {
        public int CodigoProyecto
        {
            get
            {
                return Convert.ToInt32(Request.QueryString["codproyecto"]);
            }
            set { }
        }

        public int? CodigoConvocatoria {
            get
            {
                return Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(CodigoProyecto, HttpContext.Current.Session["HistorialEvaluacion"] != null ? Convert.ToInt32(HttpContext.Current.Session["HistorialEvaluacion"]) : 0);
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

        public bool AllowAvanceEvaluacion {
            get {
                return Usuario.CodGrupo.Equals(Constantes.CONST_CoordinadorEvaluador);
            } set {
            }
        } 

        protected void Page_Load(object sender, EventArgs e)
        {           
        }

        protected string GetTabStatus(int codigoTab)
        {
            return Negocio.PlanDeNegocioV2.Utilidad.TabEvaluacion.VerificarTabSiEsRealizado(codigoTab, CodigoProyecto, CodigoConvocatoria.GetValueOrDefault()) ? "tab_aprobado" : string.Empty;
        }
    }
}