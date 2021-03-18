using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using Fonade.Account;

namespace Fonade.FONADE.Tareas
{
    public partial class MisTareas : Negocio.Base_Page
    {
        int TotalFilasActuales = 0;
        int TotalFilas = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            int idContacto = usuario.IdContacto;
            Datos.Consultas cs = new Datos.Consultas();

            GridView1.DataBind();
            TotalFilasActuales = GridView1.Rows.Count;
            Lbl_Resultados.Text = TotalFilasActuales + "de" + TotalFilas + "Actividades";
            lbl_Titulo.Text = void_establecerTitulo("AGENDAR TAREAS");
        }
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataBind();
            TotalFilasActuales = GridView1.Rows.Count;
            Lbl_Resultados.Text = TotalFilasActuales + "de" + TotalFilas + "Actividades";
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}