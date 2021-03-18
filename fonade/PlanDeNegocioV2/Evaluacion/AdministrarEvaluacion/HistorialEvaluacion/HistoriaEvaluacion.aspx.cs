using Fonade.Account;
using Fonade.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Evaluacion.AdministrarEvaluacion.HistorialEvaluacion
{
    public partial class HistoriaEvaluacion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                FieldValidate.ValidateNumeric("código", txtCodigoProyecto.Text, true);

                gvMain.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Sucedio un error detalle :" + ex.Message;
            }
        }

        protected FonadeUser Usuario
        {
            get
            {
                return HttpContext.Current.Session["usuarioLogged"] != null ? 
                    (FonadeUser)HttpContext.Current.Session["usuarioLogged"] 
                    : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true);
            }
            set
            {
            }
        }

        public List<HistorialDeEvaluacion> Get(int codigoProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                if (!codigoProyecto.Equals(0))
                {
                    var entities = (from historiaEvaluacion in db.EvaluacionObservacions
                                    join convocatorias in db.Convocatoria on historiaEvaluacion.CodConvocatoria equals convocatorias.Id_Convocatoria
                                    join proyectos in db.Proyecto on historiaEvaluacion.CodProyecto equals proyectos.Id_Proyecto
                                    where historiaEvaluacion.CodProyecto.Equals(codigoProyecto) 
                                    && proyectos.codOperador == Usuario.CodOperador
                                    select new HistorialDeEvaluacion()
                                    {
                                        CodigoProyecto = proyectos.Id_Proyecto,
                                        NombreProyecto = proyectos.NomProyecto,
                                        CodigoConvocatoria = convocatorias.Id_Convocatoria,
                                        NombreConvocatoria = convocatorias.NomConvocatoria
                                    }).ToList();

                    return entities;
                }
                else
                {
                    return new List<HistorialDeEvaluacion>();
                }
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
                        string[] data = e.CommandArgument.ToString().Split(';');
                        var codigoProyecto = Convert.ToInt32(data[0]);
                        var codigoConvocatoria = Convert.ToInt32(data[1]);

                        HttpContext.Current.Session["CodProyecto"] = codigoProyecto;
                        HttpContext.Current.Session["CodConvocatoria"] = codigoConvocatoria;
                        HttpContext.Current.Session["HistorialEvaluacion"] = codigoConvocatoria;

                        FieldValidate.Redirect(null, "~/FONADE/evaluacion/EvaluacionFrameSet.aspx", "_blank", "menubar=0,scrollbars=1,width=1000,height=600,top=50");
                    }                    
                }
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Sucedio un error detalle :" + ex.Message;
            }
        }
    }
    public class HistorialDeEvaluacion
    {
        public int CodigoProyecto { get; set; }
        public string NombreProyecto { get; set; }

        public int CodigoConvocatoria { get; set; }
        public string NombreConvocatoria { get; set; }
    }
}