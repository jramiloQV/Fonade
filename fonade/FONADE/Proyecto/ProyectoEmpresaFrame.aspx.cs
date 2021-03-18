using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.Proyecto
{
    public partial class ProyectoEmpresaFrame : Negocio.Base_Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Constantes.CONST_CallCenter == usuario.CodGrupo
                    || usuario.CodGrupo == Constantes.CONST_CallCenterOperador
                    || usuario.CodGrupo == Constantes.CONST_Interventor)
                {
                    tbInformesInterventoria.Enabled = false;
                    tbInformesInterventoria.Visible = false;
                }

                if (usuario.CodGrupo == Constantes.CONST_Asesor || usuario.CodGrupo == Constantes.CONST_Emprendedor)
                {
                    tbInformesInterventoria.Enabled = true;
                    tbInformesInterventoria.Visible = true;
                }
            }
        }
    }
}