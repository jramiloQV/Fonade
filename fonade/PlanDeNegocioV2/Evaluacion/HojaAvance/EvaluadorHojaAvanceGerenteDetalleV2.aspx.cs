using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Evaluacion.HojaAvance
{
    public partial class EvaluadorHojaAvanceGerenteDetalleV2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarEvaluaciones();
        }


        void CargarEvaluaciones()
        {
            try
            {
                string msg;
                var query = Negocio.PlanDeNegocioV2.Evaluacion.TablaDeEvaluacion.HojaAvance.GetEvaluacionSeguimiento(0, int.Parse(Session["EvalContactoDetalle"].ToString()), out msg);

                if (query.Count() > 0)
                {
                    DtSeguimiento.DataSource = query;
                    DtSeguimiento.DataBind();
                    lblmensaje.Visible = false;
                }
                else
                {
                    if (!msg.Equals("true"))
                        throw new Exception(msg);

                    lblmensaje.Visible = true;

                }


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


        }

    }
}