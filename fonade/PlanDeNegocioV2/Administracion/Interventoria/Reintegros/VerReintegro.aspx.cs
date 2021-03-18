using Fonade.Negocio.Mensajes;
using Fonade.Negocio.PlanDeNegocioV2.Administracion.Interventoria.Reintegros;
using Fonade.PlanDeNegocioV2.Formulacion.Utilidad;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.Reintegros
{
    public partial class VerReintegro : System.Web.UI.Page
    {       
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public List<HistorialReintegroDTO> GetReintegros(string codigo)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                try
                {
                    return Reintegro.GetReintegros(Convert.ToInt32(codigo));

                }
                catch (ApplicationException ex)
                {
                    return new List<HistorialReintegroDTO>();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        protected void gvReintegros_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            AccionGrid(e.CommandName.ToString(), e.CommandArgument.ToString());
        }
        protected void AccionGrid(string accion, string argumento)
        {
            try
            {
                switch (accion)
                {
                    case "VerDocumento":
                        string url = ConfigurationManager.AppSettings.Get("RutaIP") + argumento;
                        Utilidades.DescargarArchivo(url);
                        break;
                }
            }
            catch (Exception ex)
            {
                Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
        }
    }
}