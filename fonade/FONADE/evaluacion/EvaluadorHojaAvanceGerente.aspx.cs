using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.evaluacion
{
    public partial class EvaluadorHojaAvanceGerente :  Negocio.Base_Page //: System.Web.UI.Page---Se comento esta parte para poder hacer la herencia de Negocio.Base_Page y acceder a la variable usuario
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //L_Fecha.Text = "" + DateTime.Now.Day + " Del Mes " + DateTime.Now.Month + " De " + DateTime.Now.Year;
        }

        public DataTable contacto()
        {
            DataTable datatable = new DataTable();

            datatable.Columns.Add("Id");
            datatable.Columns.Add("Nombre");

            String sql = "SELECT DISTINCT C.Id_Contacto,C.Nombres+' '+C.Apellidos AS Nombre FROM Contacto AS C INNER JOIN ProyectoContacto AS PC ON C.Id_Contacto=PC.CodContacto WHERE PC.CodRol=5 AND PC.Inactivo=0 and C.codOperador=" + usuario.CodOperador + " ORDER BY Nombre";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(sql, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    DataRow fila = datatable.NewRow();
                    fila["Id"] = reader["id_contacto"].ToString();
                    fila["Nombre"] = reader["nombre"].ToString();
                    datatable.Rows.Add(fila);
                }
                reader.Close();
            }
            catch (SqlException)
            {
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return datatable;
        }

        protected void LB_Contacto_Click(object sender, EventArgs e)
        {
            var indicefila = ((GridViewRow)((Control)sender).NamingContainer).RowIndex;
            GridViewRow GV_fila = GV_Reporte.Rows[indicefila];

            Int64 idContacto = Int64.Parse(GV_Reporte.DataKeys[GV_fila.RowIndex].Value.ToString());

            HttpContext.Current.Session["EvalContactoDetalle"] = idContacto;

            Response.Redirect("EvaluadorHojaAvanceGerenteDetalle.aspx");
        }

        protected void btnPlanesActuales_Click(object sender, EventArgs e)
        {
            var indicefila = ((GridViewRow)((Control)sender).NamingContainer).RowIndex;
            GridViewRow GV_fila = GV_Reporte.Rows[indicefila];

            Int64 idContacto = Int64.Parse(GV_Reporte.DataKeys[GV_fila.RowIndex].Value.ToString());

            HttpContext.Current.Session["EvalContactoDetalle"] = idContacto;

            Response.Redirect("EvaluadorHojaAvanceGerenteDetalle.aspx");
        }

        protected void btnPlanNuevaEstructura_Click(object sender, EventArgs e)
        {
            var indicefila = ((GridViewRow)((Control)sender).NamingContainer).RowIndex;
            GridViewRow GV_fila = GV_Reporte.Rows[indicefila];

            Int64 idContacto = Int64.Parse(GV_Reporte.DataKeys[GV_fila.RowIndex].Value.ToString());

            HttpContext.Current.Session["EvalContactoDetalle"] = idContacto;

            Response.Redirect("../../PlanDeNegocioV2/Evaluacion/HojaAvance/EvaluadorHojaAvanceGerenteDetalleV2.aspx");
        }
    }
}