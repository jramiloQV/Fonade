using Fonade.Error;
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
    /// <summary>
    /// CatalogoEvaluacionConceptos
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class CatalogoEvaluacionConceptos : System.Web.UI.Page
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
        }


        /// <summary>
        /// evaluacion proyectos
        /// </summary>
        /// <returns></returns>
        
        public DataTable contacto()
        {
            DataTable datatable = new DataTable();

            datatable.Columns.Add("Id");
            datatable.Columns.Add("Nombre");

            String sql = "select id_EvaluacionConceptos, nomEvaluacionConceptos from EvaluacionConceptos";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(sql, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    DataRow fila = datatable.NewRow();
                    fila["Id"] = reader["id_EvaluacionConceptos"].ToString();
                    fila["Nombre"] = reader["nomEvaluacionConceptos"].ToString();
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

        /// <summary>
        /// Eliminar EvaluacionConceptos.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        public void eliminar(int Id)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand("DELETE FROM EvaluacionConceptos WHERE Id_EvaluacionConceptos=" + Id, conn);
            try
            {

                conn.Open();
                cmd.ExecuteReader();
            }
            catch (SqlException se)
            {
                string url = Request.Url.ToString();

                string mensaje = se.Message.ToString();
                string data = se.Data.ToString();
                string stackTrace = se.StackTrace.ToString();
                string innerException = se.InnerException == null ? "" : se.InnerException.Message.ToString();

                // Log the error
                ErrHandler.WriteError(mensaje, url, data, stackTrace, innerException, "catalogoEvaluacionConceptos", "catalogoEvaluacionConceptos");
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        /// <summary>
        /// actualiza tabla evaluacion conceptos
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Nombre"></param>
        public void modificar(Int32 Id, String Nombre)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand("Update EvaluacionConceptos set NomEvaluacionConceptos ='" + Nombre + "' WHERE Id_EvaluacionConceptos=" + Id, conn);
            try
            {

                conn.Open();
                cmd.ExecuteReader();
            }
            catch (SqlException ex)
            {
                string url = Request.Url.ToString();

                string mensaje = ex.Message.ToString();
                string data = ex.Data.ToString();
                string stackTrace = ex.StackTrace.ToString();
                string innerException = ex.InnerException == null ? "" : ex.InnerException.Message.ToString();

                // Log the error
                ErrHandler.WriteError(mensaje, url, data, stackTrace, innerException, "catalogoEvaluacionConceptos", "CatalogoEvaluacionConceptos");
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        /// <summary>
        /// Handles the Click event of the IB_AgregarIndicador control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ImageClickEventArgs"/> instance containing the event data.</param>
        protected void IB_AgregarIndicador_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("CatalogoEvaluacionConceptosAdicionar.aspx");
        }

        /// <summary>
        /// Handles the Click event of the btn_agregar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btn_agregar_Click(object sender, EventArgs e)
        {
            Response.Redirect("CatalogoEvaluacionConceptosAdicionar.aspx");
        }
    }
}