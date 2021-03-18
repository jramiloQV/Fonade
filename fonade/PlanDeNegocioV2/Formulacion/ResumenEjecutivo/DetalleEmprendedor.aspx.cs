using Datos.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Formulacion.ResumenEjecutivo
{
    public partial class DetalleEmprendedor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Emprendedor entEmprendedor = (Session["ListEmprendedor"] as List<Emprendedor>).Single(x => x.IdContacto == int.Parse(Request.QueryString["IdContacto"]));

            LabelEmail.Text = entEmprendedor.Email;
            LabelFechaNac.Text = entEmprendedor.FechaNac != null ? ((DateTime)entEmprendedor.FechaNac).ToString("MMMM, dd") + " de " + ((DateTime)entEmprendedor.FechaNac).ToString("yyyy") : "N/A";
            LabelLugarNac.Text = entEmprendedor.LugarNac;
            LabelNombre.Text = entEmprendedor.Nombre;
        }
    }
}