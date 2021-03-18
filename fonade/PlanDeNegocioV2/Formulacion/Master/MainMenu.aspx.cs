using System;
using Fonade.Negocio.PlanDeNegocioV2.Utilidad;
using Datos;
using System.Linq;
using System.Web.UI;
using Fonade.Account;
using System.Web.UI.WebControls;
using Fonade.Negocio.Proyecto;

namespace Fonade.PlanDeNegocioV2.Formulacion.Master
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
        public Boolean ShowEjecucionTabs { get; set; }
        public Boolean ShowContratoTabs { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {            
            try
            {
                if (Request.QueryString.AllKeys.Contains("codproyecto"))
                {
                    int value;
                    if (int.TryParse(Request.QueryString["codproyecto"], out value))
                    {
                        GetProyectoEstado();
                    }
                }
            }           
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Advertencia : " + ex.Message + "');", true);
                Response.Redirect("~/FONADE/MiPerfil/Home.aspx");
            }
        }

        private void GetProyectoEstado()
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var estado = ProyectoGeneral.getEstadoProyecto(CodigoProyecto);

                ShowEjecucionTabs = estado.Equals(Constantes.CONST_Ejecucion)
                                    || estado.Equals(Constantes.CONST_EvaluacionIndicadores)
                                    || estado.Equals(Constantes.CONST_Condonacion);

                ShowContratoTabs = estado.Equals(Constantes.CONST_Ejecucion)
                                    || estado.Equals(Constantes.CONST_EvaluacionIndicadores)
                                    || estado.Equals(Constantes.CONST_Condonacion)
                                    || estado.Equals(Constantes.CONST_LegalizacionContrato);
            }
        }

        protected string GetTabStatus(int codigoTab)
        {
            return ProyectoGeneral.VerificarTabSiEsRealizado(codigoTab, CodigoProyecto) ? "tab_aprobado" : string.Empty;
        }
    }
}