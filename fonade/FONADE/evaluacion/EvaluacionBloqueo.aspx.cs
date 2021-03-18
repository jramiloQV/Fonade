using Datos;
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
    public partial class EvaluacionBloqueo : Negocio.Base_Page //: System.Web.UI.Page---Se comento esta parte para poder hacer la herencia de Negocio.Base_Page y acceder a la variable usuario
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //L_Fecha.Text = "" + DateTime.Now.Day + " Del Mes " + DateTime.Now.Month + " De " + DateTime.Now.Year;
        }

        protected void LB_Editar_Nombre_Click(object sender, EventArgs e)
        {
            var indicefila = ((GridViewRow)((Control)sender).NamingContainer).RowIndex;
            GridViewRow filaDta = GV_Reporte.Rows[indicefila];

            Int64 idNomParametro = Int64.Parse(GV_Reporte.DataKeys[filaDta.RowIndex].Value.ToString());

            HttpContext.Current.Session["EvalCodProyecto"] = idNomParametro;

            Response.Redirect("EvaluacionBloqueoEditar.aspx");
        }

        public DataTable contacto()
        {
            DataTable datatable = new DataTable();

            datatable.Columns.Add("id_Parametro");
            datatable.Columns.Add("nomParametro");

            String sql = "SELECT id_Proyecto, nomProyecto FROM Proyecto WHERE codOperador=" + usuario.CodOperador + " AND CodEstado = " + Constantes.CONST_Evaluacion;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(sql, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    DataRow fila = datatable.NewRow();
                    fila["id_Parametro"] = reader["id_Proyecto"].ToString();
                    fila["nomParametro"] = reader["nomProyecto"].ToString();
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
    }
}