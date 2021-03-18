using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.RegistroUnico
{
    public partial class Ejemplo : Negocio.Base_Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (IsPostBack)
            {
                if (Request["__EVENTTARGET"].ToString().Equals("GuardarDatos"))
                {
                    Button1_Click1(sender, e);


                }
            }


        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            lblprueba.Text = "Prueba Finalizada";

        }
    }

}


       