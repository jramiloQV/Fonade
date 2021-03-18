using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.Master
{
    public partial class MainMenuActasSeg : System.Web.UI.Page
    {
        public int CodigoProyecto
        {
            get
            {
                int id = Convert.ToInt32(Session["idProyecto"]);
                return id;
            }
            set { }
        }

        public int CodigoActa
        {
            get
            {
                int id = Convert.ToInt32(Session["idActa"]);
                return id;
            }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

       

        //protected string GetTabStatus(int codigoTab)
        //{
        //    return ProyectoGeneral.VerificarTabSiEsRealizado(codigoTab, CodigoProyecto) ? "tab_aprobado" : string.Empty;
        //}
    }
}