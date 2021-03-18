using Datos;
using Fonade.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Evaluacion.HojaAvance
{
    public partial class EvaluacionHojaAvance : System.Web.UI.Page
    {
        protected FonadeUser usuario { get { return HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true); } set { } }
        int IdProyecto;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.AllKeys.Contains("codproyecto"))
                IdProyecto = int.Parse(Request.QueryString["codproyecto"].ToString());

            if (!IsPostBack)
            {


            }
        }

        public bool Enabled(object value)
        {
            return !bool.Parse(value.ToString());
        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            //foreach (RepeaterItem item in RepeaterAvance.Items)
            //{
            //    CheckBox chkName = (CheckBox)item.FindControl("CheckBoxAspecto");
            //    if (chkName != null && chkName.Enabled)
            //    {
            //        chkName.Enabled = !chkName.Checked;
            //        AvanceEvaluacion entAvance = new AvanceEvaluacion()
            //        {
            //            Avance = chkName.Checked,
            //            FechaActualizacion = DateTime.Now,
            //            IdCampo = int.Parse(chkName.Attributes["IdCampo"]),
            //            IdContacto = usuario.IdContacto,
            //            IdConvocatoria = (int)Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(IdProyecto),
            //            IdProyecto = IdProyecto
            //        };

            //        string msg;
            //        bool resul = Negocio.PlanDeNegocioV2.Evaluacion.TablaDeEvaluacion.HojaAvance.InsertarAvance(entAvance, out msg);
            //        if (!resul)
            //            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + msg + "');", true);


            //    }
            //}
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }
    }
}