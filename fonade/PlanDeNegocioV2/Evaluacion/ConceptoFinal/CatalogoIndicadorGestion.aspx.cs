using Datos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


namespace Fonade.PlanDeNegocioV2.Evaluacion.ConceptoFinal
{
    public partial class CatalogoIndicadorGestion : Negocio.Base_Page
    {
        string Accion {
            get
            {
                return Request.QueryString["Accion"];
            }
            set { }
        }
        string IdIndicador
        {
            get
            {
                return Request.QueryString["IdIndicador"];
            }
            set { }
        }
        public int CodigoProyecto { get { return Convert.ToInt32(Request.QueryString["codproyecto"]); } set { } }
        public int CodigoConvocatoria
        {
            get
            {
                return Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(CodigoProyecto, HttpContext.Current.Session["HistorialEvaluacion"] != null ? Convert.ToInt32(HttpContext.Current.Session["HistorialEvaluacion"]) : 0).GetValueOrDefault();
            }
            set { }
        }
        public int CodigoTab { get { return Constantes.Const_IndicadoresDeGestionYCumplimientoV2; } set { } }
        string conexionStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        // Eventos
        protected void Page_Load(object sender, EventArgs e)
        {                                               
            if (!Page.IsPostBack)
            {
                CargarCombo();
                ClearObject();
                if (IdIndicador != "0")
                {
                    CargarIndicador(IdIndicador);
                }
            }
        }

        protected void DD_TipoIndicador_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DD_TipoIndicador.SelectedValue != "0")
            {
                L_Indicador.Visible = true;
                if (DD_TipoIndicador.SelectedValue == "1")
                {
                    txtDenominador.Visible = true;
                    txtNumerador.Visible = true;
                }
                if (DD_TipoIndicador.SelectedValue == "2")
                {
                    txtNumerador.Visible = true;
                    txtDenominador.Visible = false;
                }
            }
            else
            {
                L_Indicador.Visible = false;
                txtDenominador.Visible = false;
                txtNumerador.Visible = false;
            }
        }

        protected void B_Crear_Click(object sender, EventArgs e)
        {
            if (B_Crear.Text == "Crear")
            {
                CrearIndicador();
                Response.Redirect("Indicadores.aspx?codproyecto=" + CodigoProyecto);
            }
            else
            {
                ActualizarIndicador();
                Response.Redirect("Indicadores.aspx?codproyecto=" + CodigoProyecto);
            }
        }


        // Metodos
        private void CargarCombo()
        {
            DD_TipoIndicador.Items.Insert(0, new ListItem("Seleccione", "0"));
            DD_TipoIndicador.Items.Insert(1, new ListItem("Indicadores de Gestión", "1"));
            DD_TipoIndicador.Items.Insert(2, new ListItem("Indicadores Cualitativos y de Cumplimiento", "2"));
        }
        /// <summary>
        /// carga indicador
        /// </summary>
        /// <param name="idIndicador"></param>
        private void CargarIndicador(string idIndicador)
        {
            B_Crear.Text = "Actualizar";
            var rs = new DataTable();
            rs = consultas.ObtenerDataTable(" SELECT * FROM EvaluacionIndicadorGestion WHERE id_indicadorGestion = " + idIndicador, "text");
            if (rs.Rows.Count > 0)
            {
                L_NUEVOINDICADOR.Text = "MODIFICAR INDICADOR";
                TB_Aspecto.Text = rs.Rows[0]["Aspecto"].ToString();
                TB_fechaSeguimiento.Text = rs.Rows[0]["FechaSeguimiento"].ToString();

                if (String.IsNullOrEmpty(rs.Rows[0]["Denominador"].ToString()))
                {
                    DD_TipoIndicador.SelectedValue = "2";
                    txtNumerador.Value = rs.Rows[0]["Numerador"].ToString();
                    L_Indicador.Visible = true;
                    txtNumerador.Visible = true;
                }
                else
                {
                    DD_TipoIndicador.SelectedValue = "1";
                    txtNumerador.Value = rs.Rows[0]["Numerador"].ToString();
                    txtDenominador.Value = rs.Rows[0]["Denominador"].ToString();
                    L_Indicador.Visible = true;
                    txtNumerador.Visible = true;
                    txtDenominador.Visible = true;
                }
                TB_Descripcion.Text = rs.Rows[0]["Descripcion"].ToString();
                TB_rango.Text = rs.Rows[0]["RangoAceptable"].ToString();
            }
        }
        /// <summary>
        /// crea indicador
        /// </summary>
        private void CrearIndicador()
        {
            using (var con = new SqlConnection(conexionStr))
            {
                using (var com = con.CreateCommand())
                {
                    com.CommandText = "MD_Insertar_Actualizar_EvaluacionIndicadorGestion";
                    com.CommandType = System.Data.CommandType.StoredProcedure;

                    com.Parameters.AddWithValue("@_Id_IndicadorGestion", 0);
                    com.Parameters.AddWithValue("@_CodProyecto", CodigoProyecto);
                    com.Parameters.AddWithValue("@_CodConvocatoria", CodigoConvocatoria);
                    com.Parameters.AddWithValue("@_Aspecto", TB_Aspecto.Text);
                    com.Parameters.AddWithValue("@_FechaSeguimiento", TB_fechaSeguimiento.Text);
                    com.Parameters.AddWithValue("@_Numerador", txtNumerador.Value);
                    if (!string.IsNullOrEmpty(txtDenominador.Value))
                    {
                        com.Parameters.AddWithValue("@_Denominador", txtDenominador.Value);
                    }
                    else
                    {
                        com.Parameters.AddWithValue("@_Denominador", "");
                    }
                    com.Parameters.AddWithValue("@_Descripcion", TB_Descripcion.Text);
                    com.Parameters.AddWithValue("@_RangoAceptable", int.Parse(TB_rango.Text));
                    com.Parameters.AddWithValue("@_caso", "CREATE");

                    try
                    {
                        con.Open();
                        com.ExecuteReader();
                        ClearObject();                        
                        UpdateTab();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        com.Dispose();
                        con.Close();
                        con.Dispose();
                    }
                }
            }
        }
        /// <summary>
        /// actualiza evaluacionindicadorgestion
        /// </summary>
        private void ActualizarIndicador()
        {
            var Denominador = string.Empty;
            if (!string.IsNullOrEmpty(txtDenominador.Value))
            {
                Denominador = txtDenominador.Value;
            }
            else
            {
                Denominador = "";
            }
            var txtSQL = " UPDATE EvaluacionIndicadorGestion " +
                         " SET Aspecto = '" + TB_Aspecto.Text + "', " +
                             "FechaSeguimiento = '" + TB_fechaSeguimiento.Text + "', " +
                             "Numerador = '" + txtNumerador.Value + "', " +
                             "Denominador = '" + Denominador + "', " +
                             "Descripcion = '" + TB_Descripcion.Text + "', " +
                             "RangoAceptable = " + TB_rango.Text + " " +
                         " WHERE Id_IndicadorGestion = " + IdIndicador;

            //Ejecutar consulta.
            ejecutaReader(txtSQL, 2);
            ClearObject();
            UpdateTab();
        }

        private void ClearObject()
        {
            TB_Aspecto.Text = "";
            TB_fechaSeguimiento.Text = "";
            DD_TipoIndicador.SelectedValue = "0";
            txtNumerador.Value = "";
            txtDenominador.Value = "";
            TB_Descripcion.Text = "";
            TB_rango.Text = "";
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

        protected void B_Cancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Indicadores.aspx?codproyecto="+CodigoProyecto);
        }

    }
}