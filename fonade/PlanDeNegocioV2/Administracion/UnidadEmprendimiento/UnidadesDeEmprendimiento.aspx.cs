using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Administracion.UnidadEmprendimiento
{
    public partial class UnidadesDeEmprendimiento : Negocio.Base_Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                if (Request["__EVENTTARGET"].ToString().Equals("jefeUnidad"))
                {
                    Response.Redirect(Request.RawUrl);
                }
            }               
        }

        protected void lnkOrdenOpcion_Click(object sender, EventArgs e)
        {
            LinkButton orderOption = (LinkButton)sender;
            var option = orderOption.Text;
        }

        public List<UnidadesEmprendimiento> getUnidadesEmprendimiento()
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from institucionEmprendimiento in db.Institucions
                                join tipo in db.TipoInstitucions on institucionEmprendimiento.CodTipoInstitucion equals tipo.Id_TipoInstitucion
                                orderby institucionEmprendimiento.NomUnidad
                                select new UnidadesEmprendimiento
                                {
                                    Id = institucionEmprendimiento.Id_Institucion,
                                    NombreUnidad = institucionEmprendimiento.NomUnidad,
                                    NombreInstitucion = institucionEmprendimiento.NomInstitucion,
                                    Estado = institucionEmprendimiento.Inactivo,
                                    TipoInstitucion = tipo.NomTipoInstitucion
                                }).ToList();

                return entities;
            }
        }

        protected void gvUnidadesEmprendimiento_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("inactivar"))
            {
                if (e.CommandArgument != null)
                {
                    int idInstitucion = Convert.ToInt32(e.CommandArgument);
                    Session["idUnidad"] = idInstitucion;

                    Redirect(null, "~/FONADE/Administracion/DesactivarUnidadEmprende.aspx", "_blank", "menubar=0,scrollbars=1,width=360,height=200,top=50");
                }
            }
            if (e.CommandName.Equals("activar"))
            {
                if (e.CommandArgument != null)
                {
                    int idInstitucion = Convert.ToInt32(e.CommandArgument);
                    //Session["codigoPlanDeNegocio"] = codigoPlanDeNegocio;
                    Response.Redirect("CrearPlanDeNegocio.aspx");
                }
            }
            if (e.CommandName.Equals("updateUnidad"))
            {
                if (e.CommandArgument != null)
                {
                    int idInstitucion = Convert.ToInt32(e.CommandArgument);
                    Session["idUnidad"] = idInstitucion;                    
                    Redirect(null, "~/PlanDeNegocioV2/Administracion/UnidadEmprendimiento/UnidadEmprendimiento.aspx", "_blank", "menubar=0,scrollbars=1,width=1024,height=600,top=50");
                }
            }
        }

        protected void lnkNuevaUnidadEmprendimiento_Click(object sender, EventArgs e)
        {
            Session["idUnidad"] = null;

            Redirect(null, "~/PlanDeNegocioV2/Administracion/UnidadEmprendimiento/UnidadEmprendimiento.aspx", "_blank", "menubar=0,scrollbars=1,width=1024,height=600,top=50");
        }
    }

    public class UnidadesEmprendimiento
    {
        public int Id { get; set; }
        public string NombreUnidad { get; set; }
        public string NombreInstitucion { get; set; }
        public bool Estado { get; set; }
        public string TipoInstitucion { get; set; }

        public string EstadoFormated
        {
            get
            {
                return !Estado ? "Activa" : "Inactiva";
            }
            set
            {
            }
        }
    }
}