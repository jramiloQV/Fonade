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
using System.Windows.Forms;

namespace Fonade.PlanDeNegocioV2.Evaluacion.ConceptoFinal
{
    public partial class Riesgos : Negocio.Base_Page
    {
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
        public bool esMiembro;     
        public Boolean bRealizado;

        protected void Page_Load(object sender, EventArgs e)
        {
            EncabezadoEval.IdProyecto = CodigoProyecto;
            EncabezadoEval.IdConvocatoria = CodigoConvocatoria;
            EncabezadoEval.IdTabEvaluacion = CodigoTab;

            inicioEncabezado(CodigoProyecto.ToString(), CodigoConvocatoria.ToString(), CodigoTab);
            if (miembro == false && realizado == true && usuario.CodGrupo == Constantes.CONST_Evaluador)
            {
                btn_agregar.Enabled = false;
                IB_AgregarIndicador.Enabled = false;
            }            
            RestringirLetras(0);
            
            esMiembro = fnMiembroProyecto(usuario.IdContacto, CodigoProyecto.ToString());
            bRealizado = esRealizado(CodigoTab, CodigoProyecto, CodigoConvocatoria);

            if (esMiembro && !bRealizado)
            { this.div_Post_It1.Visible = true; Post_It1._mostrarPost = true; }

            if (usuario.CodGrupo == Constantes.CONST_GerenteEvaluador)
            {
                IB_AgregarIndicador.Visible = false;
                btn_agregar.Visible = false;                
            }

            if (esMiembro && !bRealizado && usuario.CodGrupo == Constantes.CONST_Evaluador)
            {
                IB_AgregarIndicador.Visible = true;
                btn_agregar.Visible = true;

                try { this.GridView1.Columns[0].Visible = true; }
                catch { this.GridView1.Columns[1].Visible = true; }
            }
            else
            {
                IB_AgregarIndicador.Visible = false;
                btn_agregar.Visible = false;                

                GridView1.Columns[0].Visible = false;

                foreach (GridViewRow gvr in GridView1.Rows)
                {
                    ((LinkButton)gvr.Cells[1].FindControl("lnkeditarRiesgo")).Enabled = false;
                }
            }           
        }
                
        protected void btn_agregar_Click(object sender, EventArgs e)
        {
            var accion = "Crear";
            Response.Redirect("CatalogoRiesgoMitigacion.aspx?codproyecto=" + CodigoProyecto + "&accion=" + accion+"&codigoriesgo=0");
        }

        protected void IB_AgregarIndicador_Click(object sender, ImageClickEventArgs e)
        {
            var accion = "Crear";
            Response.Redirect("CatalogoRiesgoMitigacion.aspx?codproyecto=" + CodigoProyecto + "&accion=" + accion + "&codigoriesgo=0");
        }

        private void RestringirLetras(int serie)
        {
            try
            {
                GridViewRow filaGrillaInventario = GridView1.Rows[serie];
                System.Web.UI.WebControls.TextBox cantidad = (System.Web.UI.WebControls.TextBox)filaGrillaInventario.FindControl("TextBox1");
                cantidad.Attributes.Add("onkeypress", "javascript: return ValidNum(event);");
                RestringirLetras(serie + 1);
            }
            catch (Exception)
            {

            }
        }

        public DataTable resultado()
        {            
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand("SELECT [Id_Riesgo],[CodProyecto],[CodConvocatoria],[Riesgo],[Mitigacion] FROM [dbo].[EvaluacionRiesgo] WHERE [CodProyecto] = " + CodigoProyecto + " AND [CodConvocatoria] = " + CodigoConvocatoria, conn);
            DataTable datatable = new DataTable();

            datatable.Columns.Add("Id_Riesgo");
            datatable.Columns.Add("CodProyecto");
            datatable.Columns.Add("CodConvocatoria");
            datatable.Columns.Add("Riesgo");
            datatable.Columns.Add("Mitigacion");

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DataRow fila = datatable.NewRow();

                    fila["Id_Riesgo"] = reader["Id_Riesgo"].ToString();
                    fila["CodProyecto"] = reader["CodProyecto"].ToString();
                    fila["CodConvocatoria"] = reader["CodConvocatoria"].ToString();
                    fila["Riesgo"] = reader["Riesgo"].ToString();
                    fila["Mitigacion"] = reader["Mitigacion"].ToString();

                    datatable.Rows.Add(fila);
                }
                reader.Close();
            }
            catch (SqlException se)
            {
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return datatable;
        }

        public void eliminar(int Id_Riesgo)
        {            
            String txtSQL = "";            
            txtSQL = @"DELETE FROM EvaluacionRiesgo WHERE Id_Riesgo = " + Id_Riesgo;            
            ejecutaReader(txtSQL, 2);

            UpdateTab();            
            ScriptManager.RegisterStartupScript(this, typeof(string), "script", "<script type=text/javascript>parent.location.href = parent.location.href;</script>", false);            
        }

        public void modificar(int Id_Riesgo, String Riesgo, String Mitigacion)
        {
            string conexionStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

            using (var con = new SqlConnection(conexionStr))
            {
                using (var com = con.CreateCommand())
                {
                    com.CommandText = "MD_ModificarNuevoRiesgo";
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@_ID_Riesgo", Id_Riesgo);
                    com.Parameters.AddWithValue("@_Riesgo", Riesgo);
                    com.Parameters.AddWithValue("@_Mitigacion", Mitigacion);                    
                    try
                    {
                        con.Open();
                        com.ExecuteReader();                                                
                        UpdateTab();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
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

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {                
                var lnk2 = e.Row.FindControl("lnkeditarRiesgo") as LinkButton;
                                
                if (lnk2 != null)
                {
                    if (!esMiembro || bRealizado || usuario.CodGrupo != Constantes.CONST_Evaluador)
                    {
                        lnk2.ForeColor = System.Drawing.Color.Black;
                        lnk2.Style.Add(HtmlTextWriterStyle.TextDecoration, "none");
                        lnk2.Enabled = false;
                    }
                }
            }
            RestringirLetras(0);
        }

        protected void btn_agregar_Click1(object sender, EventArgs e)
        {
            var accion = "Crear";
            Response.Redirect("CatalogoRiesgoMitigacion.aspx?codproyecto="+CodigoProyecto+"&accion="+ accion + "&codigoriesgo=0");
        }

        static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "editar":                   
                    var codigoRiesgo = e.CommandArgument.ToString();
                    var accion = "Actualizar";
                    Response.Redirect("CatalogoRiesgoMitigacion.aspx?codproyecto=" + CodigoProyecto+"&codigoriesgo="+codigoRiesgo+"&accion="+ accion);
                    break;
                default:
                    break;
            }
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