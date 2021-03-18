using Fonade.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Evaluacion.HojaAvance
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarEvaluaciones();
        }

        void CargarEvaluaciones()
        {
            FonadeUser usuario = Session["usuarioLogged"] as FonadeUser;
            string msg;
            var evaluaciones = Negocio.PlanDeNegocioV2.Evaluacion.TablaDeEvaluacion.HojaAvance.GetEvaluacionSeguimiento(1,usuario.IdContacto, out msg);

            if (evaluaciones.Count() > 0)
            {
                lblmensaje.Visible = false;
                DtSeguimiento.DataSource = evaluaciones;
                DtSeguimiento.DataBind();
            }
            else
                lblmensaje.Visible = true;



        }
    }
}