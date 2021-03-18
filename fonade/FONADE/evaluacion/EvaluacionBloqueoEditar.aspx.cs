using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.evaluacion
{
    public partial class EvaluacionBloqueoEditar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //L_Fecha.Text = "" + DateTime.Now.Day + " Del Mes " + DateTime.Now.Month + " De " + DateTime.Now.Year;


            try
            {
                if (String.IsNullOrEmpty(HttpContext.Current.Session["EvalCodProyecto"].ToString()))
                {
                    Response.Redirect("EvaluacionBloqueo.aspx");
                }
            }
            catch (Exception)
            {
                Response.Redirect("EvaluacionBloqueo.aspx");
            }

            TB_Nuevo.Text = "Proyecto No " + HttpContext.Current.Session["EvalCodProyecto"].ToString();

            if (!IsPostBack)
            {
                String sql = "SELECT Bloqueado FROM EvaluacionProyectoBloqueado WHERE CodProyecto=" + HttpContext.Current.Session["EvalCodProyecto"].ToString();
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
                SqlCommand cmd = new SqlCommand(sql, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        if (Boolean.Parse(reader["Bloqueado"].ToString()) == true)
                        {
                            CB_Bloqueado.Checked = true;
                        }
                        else
                        {
                            CB_Bloqueado.Checked = false;
                        }
                    }
                    else
                    {
                        CB_Bloqueado.Checked = false;
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
            }
        }

        protected void B_Nuevo_Click(object sender, EventArgs e)
        {
            Int32 check = 0;

            if (CB_Bloqueado.Checked)
            {
                check = 1;
            }
            else
            {
                check = 0;
            }

            String sql = "SELECT Bloqueado FROM EvaluacionProyectoBloqueado WHERE CodProyecto=" + HttpContext.Current.Session["EvalCodProyecto"].ToString();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(sql, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    sql = "UPDATE EvaluacionProyectoBloqueado SET Bloqueado = " + check + " WHERE CodProyecto = " + HttpContext.Current.Session["EvalCodProyecto"].ToString();
                }
                else
                {
                    sql = "INSERT INTO EvaluacionProyectoBloqueado (CodProyecto,Bloqueado)  VALUES(" + HttpContext.Current.Session["EvalCodProyecto"].ToString() + "," + check + ")";
                }
                conn.Close();
                reader.Close();

                cmd = new SqlCommand(sql, conn);
                try
                {

                    conn.Open();
                    cmd.ExecuteReader();
                    conn.Close();
                }
                catch (SqlException se)
                {
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }

                reader.Close();
            }
            catch (SqlException)
            {
            }
            finally
            {
                conn.Close();
            }

            Response.Redirect("EvaluacionBloqueo.aspx");
        }
    }
}