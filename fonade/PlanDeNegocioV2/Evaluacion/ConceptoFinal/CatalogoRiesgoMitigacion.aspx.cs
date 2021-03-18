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

namespace Fonade.PlanDeNegocioV2.Evaluacion.ConceptoFinal
{
    public partial class CatalogoRiesgoMitigacion : Negocio.Base_Page
    {
        private string conexionStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        public int CodigoProyecto { get { return Convert.ToInt32(Request.QueryString["codproyecto"]); } set { } }
        public int CodigoConvocatoria
        {
            get
            {
                return Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(CodigoProyecto, HttpContext.Current.Session["HistorialEvaluacion"] != null ? Convert.ToInt32(HttpContext.Current.Session["HistorialEvaluacion"]) : 0).GetValueOrDefault();
            }
            set { }
        }
        public int CodigoTab { get { return Constantes.Const_RiesgosIdentificadosYMitigadosV2; } set { } }
        
        public String Accion {
            get {
                return Request.QueryString["accion"];
            }
            set {
            }
        }
        
        public String CodRiesgo
        {
            get
            {
                return Request.QueryString["codigoriesgo"];
            }
            set
            {
            }
        }
        private string errormessagedetail = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {                                                                                                
                B_Crear.Text = Accion;
            }
            catch { LimpiarCampos(); }

            if (!IsPostBack)
            { CargarRiesgoSeleccionado(CodRiesgo); }
        }

        protected void B_Crear_Click(object sender, EventArgs e)
        {
            String riesgo = TB_Riesgo.Text;
            String mitigacion = TB_Mitigacion.Text;

            if (B_Crear.Text == "Crear")
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                try
                {
                    SqlCommand cmd = new SqlCommand("MD_InsertarNuevoRiesgo", con);

                    if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@_CodProyecto", CodigoProyecto);
                    cmd.Parameters.AddWithValue("@_CodConvocatoria", CodigoConvocatoria);
                    cmd.Parameters.AddWithValue("@_Riesgo", riesgo);
                    cmd.Parameters.AddWithValue("@_Mitigacion", mitigacion);
                    cmd.ExecuteNonQuery();

                    cmd.Dispose();

                    UpdateTab();
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
                         " WHERE riesgo = '" + TB_Riesgo.Text + "' and CodProyecto=" + CodigoProyecto + " and " +
                         " CodConvocatoria = " + CodigoConvocatoria + " AND Id_Riesgo<>" + CodRiesgo;

                Rs = consultas.ObtenerDataTable(txtSQL, "text");

                if (Rs.Rows.Count == 0)
                {
                    txtSQL = " UPDATE EvaluacionRiesgo " +
                             " SET Riesgo = '" + TB_Riesgo.Text + "', " +
                                 " Mitigacion = '" + TB_Mitigacion.Text + "' " +
                             " WHERE Id_Riesgo=" + CodRiesgo;

                    //Ejecutar consulta SQL.
                    ejecutaReader(txtSQL, 2);
                    UpdateTab();
                }

            }
            LimpiarCampos();
            Response.Redirect("Riesgos.aspx?codproyecto="+CodigoProyecto);
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

        private void UpdateTab()
        {
            TabEvaluacionProyecto tabEvaluacion = new TabEvaluacionProyecto()
            {
                CodProyecto = CodigoProyecto,
                CodConvocatoria = CodigoConvocatoria,
                CodTabEvaluacion = (Int16)CodigoTab,
                CodContacto = usuario.IdContacto,
                FechaModificacion = DateTime.Now,
                Realizado = false
            };

            string messageResult;
            Negocio.PlanDeNegocioV2.Utilidad.TabEvaluacion.SetUltimaActualizacion(tabEvaluacion, out messageResult);
            Formulacion.Utilidad.Utilidades.PresentarMsj(messageResult, this, "Alert");
        }

    }
}