using Fonade.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Evaluacion.Controles
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        //protected FonadeUser usuario = HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true);

        protected void Page_Load(object sender, EventArgs e)
        {
            EncabezadoEval.IdProyecto = 63743;
            EncabezadoEval.IdConvocatoria = 358;
            EncabezadoEval.IdTabEvaluacion = 51;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Datos.TabEvaluacionProyecto entity = new Datos.TabEvaluacionProyecto()
            {
                CodContacto = ((FonadeUser)HttpContext.Current.Session["usuarioLogged"]).IdContacto,
                FechaModificacion = DateTime.Now,
                CodConvocatoria = 341,
                CodProyecto = 63870,
                CodTabEvaluacion = 51,
            };

            string msg;
            Negocio.PlanDeNegocioV2.Utilidad.TabEvaluacion.SetUltimaActualizacion(entity, out msg);

            EncabezadoEval.GetUltimaActualizacion();
        }
    }
}