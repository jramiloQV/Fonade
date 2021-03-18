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
using System.Windows.Forms;

namespace Fonade.FONADE.evaluacion
{
    public partial class CatalogoRiesgoMitigacio : Negocio.Base_Page
    {
        private string conexionStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        public String codProyecto;
        //public int txtTab = Constantes.CONST_ProyeccionesVentas;
        public int txtTab = Constantes.ConstSubRiesgosIdentificados;
        public String codConvocatoria;
        /// <summary>
        /// Indica si se vá a crear o actualizar un indicador seleccionado.
        /// </summary>
        public String Accion;
        /// <summary>
        /// Código del riesgo seleccionado para editar.
        /// </summary>
        public String CodRiesgo;
        private string errormessagedetail = "";

        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                codProyecto = HttpContext.Current.Session["codProyecto"].ToString();
                HttpContext.Current.Session["codProyectoval"] = codProyecto;
                codConvocatoria = HttpContext.Current.Session["codConvocatoria"].ToString();

                HttpContext.Current.Session["codProyectoval"] = codProyecto;

                Accion = HttpContext.Current.Session["Accion"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["Accion"].ToString()) ? HttpContext.Current.Session["Accion"].ToString() : "Crear";
                CodRiesgo = HttpContext.Current.Session["CodRiesgo"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodRiesgo"].ToString()) ? HttpContext.Current.Session["CodRiesgo"].ToString() : "0";

                //Establecer el texto del botón "para activar su acción.
                B_Crear.Text = Accion;
            }
            catch { LimpiarCampos(); }

            if (!IsPostBack)
            { CargarRiesgoSeleccionado(CodRiesgo); }
        }

        /// <summary>
        /// Crear o actualizar el riesgo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void B_Crear_Click(object sender, EventArgs e)
        {
            String riesgo = TB_Riesgo.Text;
            String mitigacion = TB_Mitigacion.Text;

            if (B_Crear.Text == "Crear")
            {
                // Crear Riesgo.
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                try
                {
                    //NEW RESULTS:

                    SqlCommand cmd = new SqlCommand("MD_InsertarNuevoRiesgo", con);

                    if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@_CodProyecto", codProyecto);
                    cmd.Parameters.AddWithValue("@_CodConvocatoria", codConvocatoria);
                    cmd.Parameters.AddWithValue("@_Riesgo", riesgo);
                    cmd.Parameters.AddWithValue("@_Mitigacion", mitigacion);
                    cmd.ExecuteNonQuery();

                    cmd.Dispose();
                    //Actualizar fecha modificación del tab.
                    prActualizarTabEval(txtTab.ToString(), codProyecto.ToString(), codConvocatoria.ToString());
                }
                catch (Exception ex)
                {
                    errormessagedetail = ex.Message;
                }

                finally
                {
                    con.Close();
                    con.Dispose();
                }

            }
            else if (B_Crear.Text == "Actualizar")
            {
                // Actualizar Riesgo.

                //Inicializar variables.
                DataTable Rs = new DataTable();
                String txtSQL = "";

                txtSQL = " SELECT Id_Riesgo FROM EvaluacionRiesgo " +
                         " WHERE riesgo = '" + TB_Riesgo.Text + "' and CodProyecto=" + codProyecto + " and " +
                         " CodConvocatoria = " + codConvocatoria + " AND Id_Riesgo<>" + CodRiesgo;

                Rs = consultas.ObtenerDataTable(txtSQL, "text");

                if (Rs.Rows.Count == 0)
                {
                    txtSQL = " UPDATE EvaluacionRiesgo " +
                             " SET Riesgo = '" + TB_Riesgo.Text + "', " +
                                 " Mitigacion = '" + TB_Mitigacion.Text + "' " +
                             " WHERE Id_Riesgo=" + CodRiesgo;

                    //Ejecutar consulta SQL.
                    ejecutaReader(txtSQL, 2);

                    //Actualizar fecha modificación del tab.
                    prActualizarTabEval(txtTab.ToString(), codProyecto.ToString(), codConvocatoria.ToString());
                }

            }
            LimpiarCampos();
            Response.Redirect("EvaluacionRiesgos.aspx");
        }

        /// <summary>
        /// Limpiar los campos.
        /// </summary>
        private void LimpiarCampos()
        {
            TB_Riesgo.Text = "";
            TB_Mitigacion.Text = "";
            B_Crear.Text = "Crear";
        }

        // Métodos de actualización.

        /// <summary>
        /// Cargar el riesgo seleccionado.
        /// </summary>
        /// <param name="CodRiesgo">Riesgo seleccionado.</param>
        private void CargarRiesgoSeleccionado(String CodRiesgo)
        {
            //Inicializar variables.
            DataTable rs = new DataTable();

            try
            {
                rs = consultas.ObtenerDataTable("SELECT Riesgo, Mitigacion FROM EvaluacionRiesgo WHERE Id_Riesgo = " + CodRiesgo, "text");

                if (rs.Rows.Count > 0)
                {
                    TB_Riesgo.Text = rs.Rows[0]["Riesgo"].ToString();
                    TB_Mitigacion.Text = rs.Rows[0]["Mitigacion"].ToString();
                }
            }
            catch { LimpiarCampos(); }
        }


    }
}