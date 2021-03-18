using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Fonade.Account;
using LinqKit;
using AjaxControlToolkit;
using System.ComponentModel;

namespace Fonade.Controles.evaluacion
{
    /// <summary>
    /// CatalogoIndicador
    /// </summary>
    /// <seealso cref="System.Web.UI.UserControl" />
    public partial class CatalogoIndicador : System.Web.UI.UserControl
    {
        int codProyecto;
        int codConvocatoria;
        string Descripcion;
        string Tipo;
        float Valor;
        //Boolean Protegido;


        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (HttpContext.Current.Session["CodConvocatoria"].ToString() == null || HttpContext.Current.Session["CodConvocatoria"].ToString() == "" && HttpContext.Current.Session["codProyecto"].ToString() == null || HttpContext.Current.Session["codProyecto"].ToString() == "" && HttpContext.Current.Session["codIndicador"].ToString() == null)
                {
                }
                else
                {
                    DataTable dt_indicador = new DataTable();
                    dt_indicador = sp_EvaluacionProyectoIndicador_SelectRow(int.Parse(HttpContext.Current.Session["codIndicador"].ToString()));
                    foreach (DataRow dr_indicador in dt_indicador.Rows)
                    {
                        txtDescripcion.Value = dr_indicador["descripcion"].ToString();
                        dpl_tipo.SelectedValue = dr_indicador["tipo"].ToString();
                        txtValor.Value = dr_indicador["Valor"].ToString();
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the btn_crear control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btn_crear_Click(object sender, EventArgs e)
        {
            codConvocatoria = int.Parse(HttpContext.Current.Session["CodConvocatoria"].ToString());
            codProyecto = int.Parse(HttpContext.Current.Session["codProyecto"].ToString());
            Descripcion = txtDescripcion.Value;
            Tipo = dpl_tipo.SelectedValue;
            Valor = float.Parse(txtValor.Value);

            if (int.Parse(HttpContext.Current.Session["codIndicador"].ToString()) > 0)
            {
                int codindicador = int.Parse(HttpContext.Current.Session["codIndicador"].ToString());
                sp_EvaluacionProyectoIndicador_Update(codindicador, codProyecto, codConvocatoria, Descripcion, Tipo, Valor, false);
            }
            else
            {
                sp_EvaluacionProyectoIndicador_Insert(codProyecto, codConvocatoria, Descripcion, Tipo, Valor, true);

            }
        }

        /// <summary>
        /// Crear indicador por ID
        /// </summary>
        /// <param name="cod_proyecto"></param>
        /// <param name="cod_convocatoria"></param>
        /// <param name="descripcion"></param>
        /// <param name="tipo"></param>
        /// <param name="valor"></param>
        /// <param name="protegido"></param>
        protected void sp_EvaluacionProyectoIndicador_Insert(int cod_proyecto, int cod_convocatoria, string descripcion, string tipo, float valor, Boolean protegido)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_EvaluacionProyectoIndicador_Insert", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@codProyecto", cod_proyecto);
            cmd.Parameters.AddWithValue("@codConvocatoria", cod_convocatoria);
            cmd.Parameters.AddWithValue("@descripcion", descripcion);
            cmd.Parameters.AddWithValue("@Tipo", tipo);
            cmd.Parameters.AddWithValue("@Valor", valor);
            cmd.Parameters.AddWithValue("@Protegido", protegido);
            try
            {
                con.Open();
                int id = (int)cmd.ExecuteScalar();
                cmd.Dispose();
            }
            catch (Exception ex)
            {

                Response.Write(ex.Message);
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

        /// <summary>
        /// Actualizar Indicador x id
        /// </summary>
        /// <param name="id_Indicador"> identifier indicador.</param>
        /// <param name="cod_proyecto"> cod proyecto.</param>
        /// <param name="cod_convocatoria"> cod convocatoria.</param>
        /// <param name="descripcion">The descripcion.</param>
        /// <param name="tipo">The tipo.</param>
        /// <param name="valor">The valor.</param>
        /// <param name="protegido">if set to <c>true</c> [protegido].</param>
        protected void sp_EvaluacionProyectoIndicador_Update(int id_Indicador, int cod_proyecto, int cod_convocatoria, string descripcion, string tipo, float valor, Boolean protegido)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_EvaluacionProyectoIndicador_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@codProyecto", id_Indicador);
            cmd.Parameters.AddWithValue("@codProyecto", cod_proyecto);
            cmd.Parameters.AddWithValue("@codConvocatoria", cod_convocatoria);
            cmd.Parameters.AddWithValue("@descripcion", descripcion);
            cmd.Parameters.AddWithValue("@Tipo", tipo);
            cmd.Parameters.AddWithValue("@Valor", valor);
            cmd.Parameters.AddWithValue("@Protegido", protegido);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ex)
            {

                Response.Write(ex.Message);
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

        /// <summary>
        /// Consultar  Indicador x id
        /// </summary>
        /// <param name="id_Indicador">identifier indicador.</param>
        /// <returns></returns>
        public DataTable sp_EvaluacionProyectoIndicador_SelectRow(int id_Indicador)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_EvaluacionProyectoIndicador_SelectRow", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id_Indicador", id_Indicador);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    return new DataTable();

                }
                return null;
            }
        }


    }
}