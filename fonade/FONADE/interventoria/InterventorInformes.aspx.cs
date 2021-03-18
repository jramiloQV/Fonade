using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.interventoria
{
    public partial class InterventorInformes : Negocio.Base_Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (usuario.CodGrupo)
            {
                case Constantes.CONST_Interventor:
                case Constantes.CONST_CoordinadorInterventor:
                case Constantes.CONST_GerenteInterventor:
                    L_titulo.Text = "INFORMES DE INTERVENTORÍA";
                    break;
                default:
                    L_titulo.Text = "VER PLANES DE NEGOCIO";
                    break;
            }

            if (!IsPostBack)
            {
                if (usuario.CodGrupo==Constantes.CONST_CoordinadorInterventor
                    || usuario.CodGrupo == Constantes.CONST_GerenteInterventor
                    || usuario.CodGrupo == Constantes.CONST_AdministradorSistema)
                {
                    LB_EstadisticasPagos.Visible = true;
                    LB_EstadisticasAvances.Visible = true;
                }

                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                {
                    lb_AdministrarActas.Visible = true;
                }

                ocultarLink(usuario.CodGrupo);

            }
            
        }

        private void ocultarLink(int _codGrupo)
        {
            if(_codGrupo == Constantes.CONST_AdministradorSistema
                || _codGrupo == Constantes.CONST_CoordinadorInterventor
                || _codGrupo == Constantes.CONST_GerenteInterventor
                || _codGrupo == Constantes.CONST_Interventor)
            {
                LB_InformeVisitaInter.Visible = false;
                LB_InformeBimensualInter.Visible = false;
                LB_InformeEjecucionInter.Visible = false;
                LB_InformeConsolidadoInter.Visible = false;
            }

            if (_codGrupo == Constantes.CONST_AdministradorSistema
                || _codGrupo == Constantes.CONST_CoordinadorInterventor
                || _codGrupo == Constantes.CONST_GerenteInterventor)
            {
                LB_ActasSeguimiento.Visible = false;
            }
        }
    }
}