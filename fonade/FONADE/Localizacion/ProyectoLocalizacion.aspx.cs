using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.Localizacion
{
    public partial class ProyectoLocalizacion : Negocio.Base_Page
    {
        string pid;
        string pc;

        string txtSQL;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pid = HttpContext.Current.Session["pid"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["pid"].ToString()) ? HttpContext.Current.Session["pid"].ToString() : "0";
                pc = HttpContext.Current.Session["pc"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["pc"].ToString()) ? HttpContext.Current.Session["pc"].ToString() : "0";
            }
            catch (NullReferenceException)
            {
                ClientScriptManager cm = this.ClientScript;
                cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>window.opener.location.reload();window.close();</script>");
                return;
            }

            if (string.IsNullOrEmpty(pid))
                txtSQL = "Select NomDepartamento, Id_Departamento FROM Departamento WHERE Id_Departamento = " + pc;
            else
                txtSQL = "select nomdepartamento, Id_Departamento from proyecto p, institucion, subsector, ciudad, departamento WHERE id_subsector=codsubsector and id_institucion = codinstitucion and id_ciudad=p.codciudad and id_departamento=codDepartamento and id_proyecto=" + pid;

            var reader = consultas.ObtenerDataTable(txtSQL, "text"); // ejecutaReader(txtSQL, 1);

            if (reader.Rows.Count > 0)
            {
                if (reader.Rows.Count > 0)
                {
                    imgbtn_mapa.ImageUrl = "~/Images/mapas/" + reader.Rows[0].ItemArray[0].ToString() + ".gif";
                    HttpContext.Current.Session["Id_Departamento_Localizacion"] = reader.Rows[0].ItemArray[1].ToString();
                }
                reader = null;
            }
        }

        protected void imgbtn_mapa_Click(object sender, ImageClickEventArgs e)
        {
            ClientScriptManager cm = this.ClientScript;


            int coorX = e.X;
            int coorY = e.Y;

            HttpContext.Current.Session["coorXX"] = coorX;
            HttpContext.Current.Session["coorYY"] = coorY;

            cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>window.opener.location.reload();window.close();</script>");
        }
    }
}