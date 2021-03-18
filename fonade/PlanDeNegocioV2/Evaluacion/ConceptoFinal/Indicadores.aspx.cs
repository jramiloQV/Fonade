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

namespace Fonade.PlanDeNegocioV2.Evaluacion.ConceptoFinal
{
    public partial class Indicadores : Negocio.Base_Page
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
        public int CodigoTab { get { return Constantes.Const_IndicadoresDeGestionYCumplimientoV2; } set { } }
        public Boolean esMiembro;
        /// <summary>
        /// Determina si "está" o "no" realizado...
        /// </summary>
        public Boolean bRealizado;

        protected void Page_Load(object sender, EventArgs e)
        {
            EncabezadoEval.IdProyecto = CodigoProyecto;
            EncabezadoEval.IdConvocatoria = CodigoConvocatoria;
            EncabezadoEval.IdTabEvaluacion = CodigoTab;

            inicioEncabezado(CodigoProyecto.ToString(), CodigoConvocatoria.ToString(), CodigoProyecto);
            if (miembro == false && realizado == true && usuario.CodGrupo == Constantes.CONST_Evaluador)
            {
                B_AgregarIndicador.Enabled = false;
                IB_AgregarIndicador.Enabled = false;
            }            

            //Consultar si es miembro.
            esMiembro = fnMiembroProyecto(usuario.IdContacto, CodigoProyecto.ToString());

            //Consultar si está "realizado".
            bRealizado = esRealizado(CodigoTab, CodigoProyecto, CodigoConvocatoria);

            if (esMiembro && !bRealizado)
            {
                this.div_Post_It1.Visible = true;
                Post_It1._mostrarPost = true;
            }
            else
            {
                this.div_Post_It1.Visible = false;
                Post_It1._mostrarPost = false;
            }

            if (esMiembro && !bRealizado && usuario.CodGrupo == Constantes.CONST_Evaluador)
            {
                IB_AgregarIndicador.Visible = true;
                B_AgregarIndicador.Visible = true;

                try { this.GV_Indicador.Columns[0].Visible = true; }
                catch { this.GV_Indicador.Columns[1].Visible = true; }
            }
            else
            {
                IB_AgregarIndicador.Visible = false;
                B_AgregarIndicador.Visible = false;

                GV_Indicador.Columns[3].Visible = false;

                foreach (GridViewRow gvr in GV_Indicador.Rows)
                {
                    ((LinkButton)gvr.FindControl("LB_Aspecto")).Enabled = false;
                }
            }           
        }
        
        public DataTable llenarGriView()
        {            
            DataTable datatable = new DataTable();

            datatable.Columns.Add("Id_IndicadorGestion");
            datatable.Columns.Add("CodProyecto");
            datatable.Columns.Add("CodConvocatoria");
            datatable.Columns.Add("Aspecto");
            datatable.Columns.Add("FechaSeguimiento");
            datatable.Columns.Add("TipoDeIndicador");
            datatable.Columns.Add("Numerador");
            datatable.Columns.Add("Denominador");
            datatable.Columns.Add("Descripcion");
            datatable.Columns.Add("RangoAceptable");

            SqlCommand cmd;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());

            cmd = new SqlCommand("SELECT * FROM EvaluacionIndicadorGestion WHERE CodProyecto = " + CodigoProyecto + " AND CodConvocatoria = " + CodigoConvocatoria, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DataRow fila = datatable.NewRow();

                    fila["Id_IndicadorGestion"] = reader["Id_IndicadorGestion"].ToString();
                    fila["CodProyecto"] = reader["CodProyecto"].ToString();
                    fila["CodConvocatoria"] = reader["CodConvocatoria"].ToString();
                    fila["Aspecto"] = reader["Aspecto"].ToString();
                    fila["FechaSeguimiento"] = reader["FechaSeguimiento"].ToString();
                    if (reader["Denominador"].ToString().Equals("") || String.IsNullOrEmpty(reader["Denominador"].ToString()))
                        fila["TipoDeIndicador"] = "Indicadores Cualitativos y de Cumplimiento";
                    else
                        fila["TipoDeIndicador"] = "Indicadores de Gestión";

                    fila["Numerador"] = reader["Numerador"].ToString();
                    fila["Denominador"] = reader["Denominador"].ToString();
                    fila["Descripcion"] = reader["Descripcion"].ToString();
                    fila["RangoAceptable"] = reader["RangoAceptable"].ToString();

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
            //datatable.DefaultView.Sort = "[TipoDeIndicador] Desc";
            var dView = new DataView(datatable);
            dView.Sort = "TipoDeIndicador Desc";
            //return datatable;
            return dView.ToTable();
        }

        protected void I_AyudaProVentas_Click(object sender, ImageClickEventArgs e)
        {
            HttpContext.Current.Session["mensaje"] = "4";
            ClientScriptManager cm = this.ClientScript;
            cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>open('../Ayuda/Mensaje.aspx', 'Proyección de ventas', 'width=500,height=400');</script>");
        }

        protected void IB_AgregarIndicador_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("CatalogoIndicadorGestion.aspx?Accion=Crear&IdIndicador=0" + "&codProyecto=" + CodigoProyecto + "&codConvocatoria=" + CodigoConvocatoria);
        }

        protected void B_AgregarIndicador_Click(object sender, EventArgs e)
        {
            Response.Redirect("CatalogoIndicadorGestion.aspx?Accion=Crear&IdIndicador=0" + "&codProyecto=" + CodigoProyecto + "&codConvocatoria=" + CodigoConvocatoria);
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            var indicefila = ((GridViewRow)((Control)sender).NamingContainer).RowIndex;
            GridViewRow GVInventario = GV_Indicador.Rows[indicefila];

            String ID = GV_Indicador.DataKeys[GVInventario.RowIndex].Value.ToString();

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand("DELETE FROM [EvaluacionIndicadorGestion] WHERE [Id_IndicadorGestion] = " + ID, conn);
            try
            {
                conn.Open();
                cmd.ExecuteReader();
                conn.Close();
                UpdateTab();
            }
            catch (SqlException se)
            {
                throw se;
            }
            catch (Exception) { }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            GV_Indicador.DataBind();
        }

        public void actualizar(String Id_IndicadorGestion, String CodProyecto, String CodConvocatoria, String Aspecto, String FechaSeguimiento, String TipoDeIndicador, String Numerador, String Denominador, String Descripcion, String RangoAceptable)
        {            
            ClientScriptManager cm = this.ClientScript;

            if (!TipoDeIndicador.Equals("Indicadores Cualitativos y de Cumplimiento"))
            {
                if (String.IsNullOrEmpty(Denominador))
                {
                    System.Windows.Forms.MessageBox.Show("El Campo Denominador es requerido", "Error de usuario", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                Denominador = "";
            }

            string conexionStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            using (var con = new SqlConnection(conexionStr))
            {
                using (var com = con.CreateCommand())
                {
                    com.CommandText = "MD_Insertar_Actualizar_EvaluacionIndicadorGestion";
                    com.CommandType = System.Data.CommandType.StoredProcedure;

                    com.Parameters.AddWithValue("@_Id_IndicadorGestion", Id_IndicadorGestion);
                    com.Parameters.AddWithValue("@_CodProyecto", 0);
                    com.Parameters.AddWithValue("@_CodConvocatoria", 0);
                    com.Parameters.AddWithValue("@_Aspecto", Aspecto);
                    com.Parameters.AddWithValue("@_FechaSeguimiento", FechaSeguimiento);
                    com.Parameters.AddWithValue("@_Numerador", Numerador);
                    com.Parameters.AddWithValue("@_Denominador", Denominador);
                    com.Parameters.AddWithValue("@_Descripcion", Descripcion);
                    com.Parameters.AddWithValue("@_RangoAceptable", RangoAceptable);

                    com.Parameters.AddWithValue("@_caso", "UPDATE");
                    try
                    {
                        con.Open();
                        com.ExecuteReader();
                        UpdateTab();

                        string mensaje = "Indicador actualizado correctamente";
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Mensaje", "alert('" + mensaje + "');", true);
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "", "window.opener.location.reload();", true);
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

        protected void DD_TipoIndicador_SelectedIndexChanged(object sender, EventArgs e)
        {
            var indicefila = ((GridViewRow)((Control)sender).NamingContainer).RowIndex;
            GridViewRow GVInventario = GV_Indicador.Rows[indicefila];
            DropDownList TBCantidades = (DropDownList)GVInventario.FindControl("DD_TipoIndicador");
            TextBox textbox = (TextBox)GVInventario.FindControl("TB_Denominador");
            String cantidad = TBCantidades.SelectedValue;

            if (cantidad.Equals("Indicadores Cualitativos y de Cumplimiento"))
            {
                textbox.Visible = false;
            }
            else
            {
                textbox.Visible = true;
            }
        }

        protected void GV_Indicador_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var lnk = e.Row.FindControl("LBA_Aspecto") as LinkButton;
                var lbl_1 = e.Row.FindControl("lbl_hr_1") as Label;
                //var lbl_2 = e.Row.FindControl("lbl_hr_2") as Label;
                var lbl = (Label)e.Row.FindControl("Label5");

                #region Diego Quiñonez - 15 de Enero de 2015

                Label lblIndicador = ((Label)e.Row.FindControl("Label1"));

                if (lblIndicador != null)
                {
                    if (lblIndicador.Text.Equals("Indicadores de Gestión"))
                        lbl_1.Visible = true;
                    else
                        lbl_1.Visible = false;
                }

                #endregion

                if (lnk != null)
                {
                    if (usuario.CodGrupo == Constantes.CONST_GerenteEvaluador || usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador)
                    {
                        lnk.Enabled = false;
                        lnk.ForeColor = System.Drawing.Color.Black;
                        lnk.Style.Add(HtmlTextWriterStyle.TextDecoration, "none");
                    }
                }

                if (lblIndicador.Text == "Indicadores Cualitativos y de Cumplimiento")
                {
                    lbl.Visible = false;
                }

            }
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

        protected void GV_Indicador_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            LinkButton lnkBtn = e.CommandSource as LinkButton;

            switch (e.CommandName)
            {
                case "editar":                    
                    var codigoIndicador = e.CommandArgument.ToString();
                    
                    Response.Redirect("CatalogoIndicadorGestion.aspx?Accion=Editar&IdIndicador=" + codigoIndicador + "&codProyecto=" + CodigoProyecto + "&codConvocatoria=" + CodigoConvocatoria);
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
            EncabezadoEval.GetUltimaActualizacion();
        }
    }
}