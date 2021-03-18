using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Formulacion.Controles
{
    public partial class Help : System.Web.UI.UserControl
    {


        public string Titulo { get; set; }

        public string Mensaje { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            divTextoAyuda.Attributes["onclick"] = "textoAyuda({titulo: '" + Titulo + "', texto: '" + Mensaje + "'});";
            LiteralTitulo.Text = Titulo;
        }

    }
}