using Datos;
using Fonade.Error;
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
    /// <summary>
    /// clase que implementa la actualizacion del catalogo item
    /// </summary>
    public partial class CatalogoItem : System.Web.UI.Page
    {
        public String codProyecto;
        public String codConvocatoria;
        public int txtTab = Constantes.CONST_RolEvaluador;
        public String Aspecto;
        private String MessageErrorDetail = "";

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            datos();
            L_Fecha.Text = "" + DateTime.Now.Day + " Del Mes " + DateTime.Now.Month + " De " + DateTime.Now.Year;
        }

        private void datos()
        {
            try
            {
                Aspecto = HttpContext.Current.Session["txtTabAspecto"].ToString();

                codProyecto = HttpContext.Current.Session["codProyecto"].ToString();
                HttpContext.Current.Session["codProyectoval"] = codProyecto;
                codConvocatoria = HttpContext.Current.Session["codConvocatoria"].ToString();

                codProyecto = Request.QueryString["codProyecto"].ToString();
                HttpContext.Current.Session["codProyectoval"] = codProyecto;
            }
            catch (Exception ex ) {
                MessageErrorDetail = ex.Message;
            }
        }
        /// <summary>
        /// actualizacion tablas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void B_Crear_Click(object sender, EventArgs e)
        {
            String NomItem = TB_NomItem.Text;
            Int32 ID_Item;

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());

            try
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO [Item] ( [NomItem], [CodTabEvaluacion] ) VALUES ('" + NomItem + "','" + HttpContext.Current.Session["txtTabAspecto"].ToString() + "')", conn);
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

                cmd = new SqlCommand("INSERT INTO [EvaluacionEvaluador] ( [CodProyecto], [CodConvocatoria], [CodItem] ) VALUES('" + codProyecto + "','" + codConvocatoria + "','" + ID_Item + "')", conn);
                conn.Open();
                cmd.ExecuteReader();
                conn.Close();

                for (int i = 1; i <= 5; i++)
                {
                    String objetoteesto = "Texto" + i;
                    String objetopuntaje = "Puntaje" + i;

                    TextBox textboxTexto = (TextBox)this.FindControl(objetoteesto);
                    TextBox textboxPuntaje = (TextBox)this.FindControl(objetopuntaje);

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
                        }catch(SqlException){}
                    }
                }
            }
            catch (SqlException ex)
            {
                string url = Request.Url.ToString();

                string mensaje = ex.Message.ToString();
                string data = ex.Data.ToString();
                string stackTrace = ex.StackTrace.ToString();
                string innerException = ex.InnerException == null ? "" : ex.InnerException.Message.ToString();

                // Log the error
                ErrHandler.WriteError(mensaje, url, data, stackTrace, innerException,"catalogoItem", "catalogoItem");
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
            switch (Int32.Parse(HttpContext.Current.Session["txtTabAspecto"].ToString()))
            {
                case 1:
                    Response.Redirect("EvaluacionFinanciera.aspx?txtTab=1");
                    break;
                case 4:
                    Response.Redirect("EvaluacionFinanciera.aspx?txtTab=4");
                    break;
                case 15:
                    Response.Redirect("EvaluacionFinanciera.aspx?txtTab=15");
                    break;
            }
        }

        /// <summary>
        /// Handles the Click event of the B_Cancelar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void B_Cancelar_Click(object sender, EventArgs e)
        {
            redirigir();
        }
    }
}