using Fonade.Negocio.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimientos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento
{
    public partial class AdministrarActasSeguimiento : Negocio.Base_Page //System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ValidateUsers();
                cargarGrilla();
            }
        }

        public void ValidateUsers()
        {
            try
            {
                if (!(usuario.CodGrupo == Datos.Constantes.CONST_CoordinadorInterventor))
                {
                    Response.Redirect("~/FONADE/evaluacion/AccesoDenegado.aspx");
                }
            }
            catch (Exception)
            {
                Response.Redirect("~/FONADE/evaluacion/AccesoDenegado.aspx");
            }
        }

        private void cargarGrilla()
        {
            List<InterventoresProyecto> list = ActaSeguimiento.interventoresPorProyecto(usuario.IdContacto);

            gvMain.DataSource = list;
            gvMain.DataBind();
        }

        protected void gvMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Administrar"))
            {
                if (e.CommandArgument != null)
                {
                    string codProyecto = e.CommandArgument.ToString();
                    
                    Session["idProyectoActasSeg"] = codProyecto;                    
                    Response.Redirect("~/PlanDeNegocioV2/Administracion/Interventoria/ActasDeSeguimiento/AccionesActasSeg.aspx");

                }
            }
        }
    }
}