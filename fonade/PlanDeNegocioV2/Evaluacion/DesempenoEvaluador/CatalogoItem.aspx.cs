using Datos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Evaluacion.DesempenoEvaluador
{
    public partial class CatalogoItem : System.Web.UI.Page
    {
        public int CodigoProyecto
        {
            get
            {
                return Convert.ToInt32(Request.QueryString["codproyecto"]);
            }
            set { }
        }
        public int CodigoConvocatoria
        {
            get
            {
                return Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(CodigoProyecto, HttpContext.Current.Session["HistorialEvaluacion"] != null ? Convert.ToInt32(HttpContext.Current.Session["HistorialEvaluacion"]) : 0).GetValueOrDefault();
            }
            set { }
        }
        public int txtTab = Constantes.CONST_RolEvaluador;
        public String Aspecto
        {
            get
            {
                return Request.QueryString["codaspecto"];
            }
            set { }
        }
        private String MessageErrorDetail = "";

        protected void Page_Load(object sender, EventArgs e)
        {           
            L_Fecha.Text = "" + DateTime.Now.Day + " Del Mes " + DateTime.Now.Month + " De " + DateTime.Now.Year;
        }
                
        protected void B_Crear_Click(object sender, EventArgs e)
        {
            String NomItem = TB_NomItem.Text;
            Int32 ID_Item;

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());

            try
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO [Item] ( [NomItem], [CodTabEvaluacion] ) VALUES ('" + NomItem + "','" + Aspecto + "')", conn);
                conn.Open();
                cmd.ExecuteReader();
                conn.Close();

                cmd = new SqlCommand("SELECT MAX([Id_Item]) AS MAXIMO FROM [Item]", conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                    ID_Item = Int32.Parse(reader["MAXIMO"].ToString());
                else
                    ID_Item = 1;
                conn.Close();

                cmd = new SqlCommand("INSERT INTO [EvaluacionEvaluador] ( [CodProyecto], [CodConvocatoria], [CodItem] ) VALUES('" + CodigoProyecto + "','" + CodigoConvocatoria + "','" + ID_Item + "')", conn);
                conn.Open();
                cmd.ExecuteReader();
                conn.Close();

                for (int i = 1; i <= 5; i++)
                {
                    String objetoteesto = "Texto" + i;
                    String objetopuntaje = "Puntaje" + i;

                    TextBox textboxTexto = (TextBox)this.Master.FindControl("bodyHolder").FindControl(objetoteesto);
                    TextBox textboxPuntaje = (TextBox)this.Master.FindControl("bodyHolder").FindControl(objetopuntaje);
                    
                    if (!String.IsNullOrEmpty(textboxTexto.Text))
                    {
                        if (String.IsNullOrEmpty(textboxPuntaje.Text))
                            textboxPuntaje.Text = "0";

                        cmd = new SqlCommand("INSERT INTO [ItemEscala] ( [CodItem], [Texto], [Puntaje] ) VALUES ('" + ID_Item + "','" + textboxTexto.Text + "','" + textboxPuntaje.Text + "')", conn);
                        try
                        {
                            conn.Close();
                            conn.Open();
                            cmd.ExecuteReader();
                            conn.Close();
                        }
                        catch (SqlException) { }
                    }
                }
            }
            catch (SqlException se)
            {
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            redirigir();
        }

        private void redirigir()
        {
            switch (Int32.Parse(Aspecto))
            {
                case 1:
                    Response.Redirect("EvaluacionFinanciera.aspx?codaspecto=1&codproyecto=" + CodigoProyecto);
                    break;
                case 4:
                    Response.Redirect("EvaluacionFinanciera.aspx?codaspecto=4&codproyecto=" + CodigoProyecto);
                    break;
                case 15:
                    Response.Redirect("EvaluacionFinanciera.aspx?codaspecto=15&codproyecto=" + CodigoProyecto);
                    break;
            }
        }

        protected void B_Cancelar_Click(object sender, EventArgs e)
        {
            redirigir();
        }
    }
}