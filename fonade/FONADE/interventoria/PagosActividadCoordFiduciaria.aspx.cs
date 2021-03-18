using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Negocio;

namespace Fonade.FONADE.interventoria
{
    public partial class PagosActividadCoordFiduciaria : Base_Page
    {
        public string codContactoFiduciariaDDL
        {
            get
            {
                return ddlcontacto.SelectedValue;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public DataTable lstcontacto()
        {
            string sql = "SELECT Id_Contacto, Email FROM Contacto, GrupoContacto " +
                "WHERE id_Contacto=CodContacto " +
                "and inactivo=0 and CodGrupo=15 and codOperador = " + usuario.CodOperador;
            return consultas.ObtenerDataTable(sql, "text");
        }

        protected void btnenviar_Click(object sender, EventArgs e)
        {
            //Inicializar variables.
            String sTexto = "";

            if (ddlcontacto.SelectedValue != "")
            {
                //Asignar valor seleccionado del DropDownList a variable de sesión.
                HttpContext.Current.Session["CodContatoFiduciaria"] = ddlcontacto.SelectedValue;
                //Redirigir al usuario a "PagosActividadCoord.aspx".
                Response.Redirect("PagosActividadCoord.aspx");
            }
            else
            {
                sTexto = Texto("TXT_CONTACTOFIDUCIARIA_REQ");
                return;
            }
        }
    }
}