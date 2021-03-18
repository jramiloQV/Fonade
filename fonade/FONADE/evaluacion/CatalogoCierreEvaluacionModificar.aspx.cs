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
    /// CatalogoCierreEvaluacionModificar
    /// </summary>
    
    public partial class CatalogoCierreEvaluacionModificar : Negocio.Base_Page
    {
        // Variables globales.

        /// <summary>
        /// Variable que almacena las consultas SQL.
        /// </summary>
        String txtSQL;

        /// <summary>
        /// Variable que contiene la fecha actual (o se configura de acuerdo al valor a cargar).
        /// </summary>
        DateTime fecha_data = DateTime.Today;



        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    txtSQL = " SELECT * FROM Parametro WHERE Id_Parametro= " + HttpContext.Current.Session["idNomParametro"].ToString();
                    var dt = consultas.ObtenerDataTable(txtSQL, "text");

                    try
                    {
                        lbl_ID.Text = "Id: " + dt.Rows[0]["NomParametro"].ToString();
                        fecha_data = Convert.ToDateTime(dt.Rows[0]["Valor"].ToString());

                        DDL_Dia.SelectedValue = fecha_data.Day.ToString();
                        DDL_Mes.SelectedValue = fecha_data.Month.ToString();
                        DD_Anio.SelectedValue = fecha_data.Year.ToString();
                    }
                    catch
                    {
                        DDL_Dia.SelectedValue = fecha_data.Day.ToString();
                        DDL_Mes.SelectedValue = fecha_data.Month.ToString();
                        DD_Anio.SelectedValue = fecha_data.Year.ToString();
                    }

                    DDL_Dia.SelectedValue = fecha_data.Day.ToString();
                    DDL_Mes.SelectedValue = fecha_data.Month.ToString();
                    DD_Anio.SelectedValue = fecha_data.Year.ToString();

                    txtSQL = null;
                    dt = null;
                }
                catch
                {
                    DDL_Dia.SelectedValue = fecha_data.Day.ToString();
                    DDL_Mes.SelectedValue = fecha_data.Month.ToString();
                    DD_Anio.SelectedValue = fecha_data.Year.ToString();
                }
            }
        }

        /// <summary>
        /// Actualizar parámetro "Fecha".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void B_Actualizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(HttpContext.Current.Session["idNomParametro"].ToString()))
                {
                    Response.Redirect("CatalogoCierreEvaluacion.aspx");
                    return;
                }
            }
            catch
            {
                Response.Redirect("CatalogoCierreEvaluacion.aspx");
                return;
            }

            try
            {
                DateTime date = Convert.ToDateTime(DDL_Dia.SelectedValue + "/" + DDL_Mes.SelectedValue + "/" + DD_Anio.SelectedValue);

                if (!date.Equals(null))
                {
                    Int32 anio = date.Year;
                    Int32 mes = date.Month;
                    Int32 dia = date.Day;

                    String fecha = "" + anio + "/" + mes + "/" + dia;

                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
                    SqlCommand cmd = new SqlCommand("UPDATE Parametro set Valor ='" + fecha + "' WHERE Id_Parametro=" + HttpContext.Current.Session["idNomParametro"].ToString(), conn);
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
                        ErrHandler.WriteError(mensaje, url, data, stackTrace, innerException, usuario.Email, usuario.IdContacto.ToString());
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                        Response.Redirect("CatalogoCierreEvaluacion.aspx");
                    }
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La fecha no es correcta.')", true);
                return;
            }
        }
    }
}