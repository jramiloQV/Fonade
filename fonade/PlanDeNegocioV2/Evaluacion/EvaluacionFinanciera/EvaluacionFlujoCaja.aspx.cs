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
using Fonade.Clases;

namespace Fonade.PlanDeNegocioV2.Evaluacion.EvaluacionFinanciera
{
    public partial class EvaluacionFlujoCaja : Negocio.Base_Page
    {
        private Int32 numPeriodos;
        private String txtConclusionesFinancieras;
        Table tabla;
        String Accion = "CAMBIAR!";
        public int CodigoProyecto { get { return Convert.ToInt32(Request.QueryString["codproyecto"]); } set { } }
        public int CodigoConvocatoria
        {
            get
            {
                return Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(CodigoProyecto, HttpContext.Current.Session["HistorialEvaluacion"] != null ? Convert.ToInt32(HttpContext.Current.Session["HistorialEvaluacion"]) : 0).GetValueOrDefault();
            }
            set { }
        }
        public int CodigoTab { get { return Constantes.Const_FlujoDeCajaV2; } set { } }
        public Boolean EsMiembro { get; set; }
        public Boolean EsRealizado { get; set; }
        public Boolean PostitVisible
        {
            get
            {
                return EsMiembro && !EsRealizado;
            }
            set { }
        }
        public Boolean AllowUpdate
        {
            get
            {
                return EsMiembro && !EsRealizado && usuario.CodGrupo.Equals(Constantes.CONST_Evaluador);
            }
            set { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                EncabezadoEval.IdProyecto = CodigoProyecto;
                EncabezadoEval.IdConvocatoria = CodigoConvocatoria;
                EncabezadoEval.IdTabEvaluacion = CodigoTab;

                EsMiembro = VerificarSiEsMienbroProyecto(CodigoProyecto, usuario.IdContacto);
                EsRealizado = VerificarSiEsRealizado(CodigoTab, CodigoProyecto, CodigoConvocatoria);

                DatosPron();
                if (!IsPostBack)
                {
                    llenarPanelFlujo();
                    inicioEncabezado(CodigoProyecto.ToString(), CodigoConvocatoria.ToString(), 1);
                    TB_Conclusiones.Text = txtConclusionesFinancieras.htmlDecode();                    
                    this.div_conclusiones.InnerText = txtConclusionesFinancieras.htmlDecode();
                    HttpContext.Current.Session["P_TablaValE"] = tabla;
                }

                div_Post_It1.Visible = PostitVisible;
                Post_It1._mostrarPost = PostitVisible;

                TB_Conclusiones.Visible = AllowUpdate;
                B_Registar.Visible = AllowUpdate;
                this.div_conclusiones.Visible = !AllowUpdate;

                tabla = new Table();
                tabla = (Table)Session["P_TablaValE"];
                P_FlujoCaja.Controls.Add(tabla);
            }
            catch (ApplicationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Advertencia, detalle : " + ex.Message + "');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }
        }
        protected Boolean VerificarSiEsMienbroProyecto(Int32 codigoProyecto, Int32 codigoContacto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from proyectoContacto in db.ProyectoContactos
                              where
                                    proyectoContacto.CodProyecto == codigoProyecto
                                   && proyectoContacto.CodContacto == codigoContacto
                                   && proyectoContacto.Inactivo == false
                                   && proyectoContacto.FechaInicio.Date <= DateTime.Now.Date
                                   && proyectoContacto.FechaFin == null
                              select
                                   proyectoContacto.CodRol
                          ).ToList().FirstOrDefault();

                if (entity != null)
                    HttpContext.Current.Session["CodRol"] = entity;

                return entity != null ? true : false;
            }
        }

        protected Boolean VerificarSiEsRealizado(Int32 codigoTab, Int32 codigoProyecto, Int32 codigoConvocatoria)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from tabEvaluacion in db.TabEvaluacionProyectos
                              where
                                   tabEvaluacion.CodProyecto.Equals(codigoProyecto)
                                   && tabEvaluacion.CodConvocatoria.Equals(codigoConvocatoria)
                                   && tabEvaluacion.CodTabEvaluacion.Equals(codigoTab)
                                   && tabEvaluacion.Realizado.Equals(true)
                              select
                                   tabEvaluacion.Realizado
                             ).Any();

                return entity;
            }
        }

        private void DatosPron()
        {
            try
            {
                DataTable Anexos = new DataTable();

                Consultas consulta = new Consultas();

                String sql = "SELECT TiempoProyeccion FROM EvaluacionObservacion WHERE CodProyecto = " + CodigoProyecto + " and codconvocatoria=" + CodigoConvocatoria;

                Anexos = consulta.ObtenerDataTable(sql, "text");

                try
                {
                    if (Anexos.Rows.Count > 0)
                    { if (!String.IsNullOrEmpty(Anexos.Rows[0]["TiempoProyeccion"].ToString())) { numPeriodos = Int32.Parse(Anexos.Rows[0]["TiempoProyeccion"].ToString()); } else { numPeriodos = 0; } }
                    else { numPeriodos = 0; }
                }
                catch { numPeriodos = 0; }

                sql = "SELECT ConclusionesFinancieras FROM evaluacionobservacion WHERE CodProyecto = " + CodigoProyecto + " AND CodConvocatoria = " + CodigoConvocatoria;

                Anexos = consulta.ObtenerDataTable(sql, "text");

                txtConclusionesFinancieras = Anexos.Rows[0]["ConclusionesFinancieras"].ToString();
            }
            catch { }
        }

        private void llenarPanelFlujo()
        {
            DataTable Anexos = new DataTable();
            Consultas consulta = new Consultas();
            String sql = "SELECT Id_EvaluacionRubroProyecto, Descripcion FROM EvaluacionRubroProyecto WHERE codProyecto = " + CodigoProyecto + " AND codConvocatoria = " + CodigoConvocatoria;
            Anexos = consulta.ObtenerDataTable(sql, "text");

            tabla = new Table();
            tabla.CssClass = "Grilla";

            TableHeaderRow headerrow = new TableHeaderRow();

            headerrow.Cells.Add(crearceladtitulo("Rubro / Periodo", 1, 1, ""));

            for (int i = 0; i < numPeriodos; i++)
            {
                headerrow.Cells.Add(crearceladtitulo("" + (i + 1), 1, 1, ""));
                headerrow.Width = 78;
                headerrow.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
            }
            tabla.Rows.Add(headerrow);

            for (int i = 0; i < Anexos.Rows.Count; i++)
            {
                TableRow filaTabla = new TableRow();

                filaTabla.Cells.Add(celdaNormal(Anexos.Rows[i]["Descripcion"].ToString(), 1, 1, "1", "Des" + Anexos.Rows[i]["Id_EvaluacionRubroProyecto"].ToString()));

                for (int j = 0; j < numPeriodos; j++)
                {
                    sql = "SELECT Valor FROM EvaluacionRubroValor WHERE CodEvaluacionRubroProyecto = " + Anexos.Rows[i]["Id_EvaluacionRubroProyecto"].ToString() + " AND Periodo = " + (j + 1);
                    DataTable ValorData = new DataTable();
                    ValorData = consulta.ObtenerDataTable(sql, "text");

                    try
                    {
                        filaTabla.Cells.Add(celdaNormal(ValorData.Rows[0]["Valor"].ToString(), 1, 1, "", "val;" + (i + 1) + ";" + (j + 1)));
                    }
                    catch (IndexOutOfRangeException)
                    {
                        filaTabla.Cells.Add(celdaNormal("0", 1, 1, "", "val;" + (i + 1) + ";" + (j + 1)));
                    }
                }
                tabla.Rows.Add(filaTabla);
            }
        }

        private TableHeaderCell crearceladtitulo(String mensaje, Int32 colspan, Int32 rowspan, String cssestilo)
        {
            TableHeaderCell celda1 = new TableHeaderCell();
            celda1.ColumnSpan = colspan;
            celda1.RowSpan = rowspan;
            celda1.CssClass = cssestilo;

            Label titulo1 = new Label();
            titulo1.Text = mensaje;
            celda1.Controls.Add(titulo1);

            return celda1;
        }

        private TableCell celdaNormal(String mensaje, Int32 colspan, Int32 rowspan, String cssestilo, String id)
        {
            TableCell celda1 = new TableCell();
            celda1.ColumnSpan = colspan;
            celda1.RowSpan = rowspan;
            celda1.CssClass = cssestilo;

            if (cssestilo.Equals("1"))
            {
                Label titulo1 = new Label();
                titulo1.Text = mensaje;
                titulo1.Width = 50; //70 Tamaño del celda
                titulo1.ID = id;
                celda1.Controls.Add(titulo1);
            }
            else
            {
                TextBox titulo1 = new TextBox();
                titulo1.Text = mensaje;
                titulo1.Width = 50; //70 Tamaño de la celda
                titulo1.ID = id;
                titulo1.TextChanged += TB_Conclusiones_TextChanged;

                titulo1.Enabled = AllowUpdate;

                celda1.Controls.Add(titulo1);
            }

            return celda1;
        }

        protected void B_Registar_Click(object sender, EventArgs e)
        {
            #region Código de inserción/actualización.

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd;

            tabla = new Table();
            tabla = (Table)Session["P_TablaValE"];
            String sql = "SELECT Id_EvaluacionRubroProyecto, Descripcion FROM EvaluacionRubroProyecto WHERE codProyecto = " + CodigoProyecto + " AND codConvocatoria = " + CodigoConvocatoria;

            DataTable Anexos = new DataTable();

            Consultas consulta = new Consultas();

            Anexos = consulta.ObtenerDataTable(sql, "text");

            for (int i = 0; i < Anexos.Rows.Count; i++)
            {
                for (int j = 0; j < numPeriodos; j++)
                {
                    sql = "SELECT Valor FROM EvaluacionRubroValor WHERE CodEvaluacionRubroProyecto = " + Anexos.Rows[i]["Id_EvaluacionRubroProyecto"].ToString() + " AND Periodo = " + (j + 1);
                    DataTable ValorData = new DataTable();
                    ValorData = consulta.ObtenerDataTable(sql, "text");

                    TextBox valor = new TextBox();

                    foreach (Control celda in tabla.Rows[i + 1].Cells[j + 1].Controls)
                    {
                        if (celda is TextBox)
                        {
                            valor = (TextBox)celda;
                        }
                    }

                    String txtSQL;

                    if (ValorData.Rows.Count > 0)
                    {
                        txtSQL = "UPDATE EvaluacionRubroValor SET Valor = " + valor.Text.Replace(',', '.') + " WHERE CodEvaluacionRubroProyecto = " + Anexos.Rows[i]["Id_EvaluacionRubroProyecto"].ToString() + " AND Periodo = " + (j + 1);
                    }
                    else
                    {
                        txtSQL = "INSERT INTO EvaluacionRubroValor (CodEvaluacionRubroProyecto, Periodo, Valor) VALUES(" + Anexos.Rows[i]["Id_EvaluacionRubroProyecto"].ToString() + "," + (j + 1) + "," + valor.Text.Replace(',', '.') + ")";
                    }

                    ejecutaReader(txtSQL, 2);
                }
            }

            sql = "SELECT ConclusionesFinancieras FROM evaluacionobservacion WHERE CodProyecto = " + CodigoProyecto + " AND CodConvocatoria = " + CodigoConvocatoria;
            Anexos = consulta.ObtenerDataTable(sql, "text");

            if (Anexos.Rows.Count > 0)
            {
                sql = "UPDATE evaluacionobservacion SET ConclusionesFinancieras = '" + TB_Conclusiones.Text.htmlEncode() + "' WHERE CodProyecto = " + CodigoProyecto + " AND CodConvocatoria = " + CodigoConvocatoria;
            }
            else
            {
                sql = "INSERT INTO evaluacionobservacion (CodProyecto,CodConvocatoria,Actividades,ProductosServicios,EstrategiaMercado,ProcesoProduccion,EstructuraOrganizacional,TamanioLocalizacion,Generales,ConclusionesFinancieras) Values(" + CodigoProyecto + "," + CodigoConvocatoria + ",'','','','','','','','" + TB_Conclusiones.Text.htmlEncode() + "')";
            }

            ejecutaReader(sql, 2);
            #endregion

            UpdateTab();
        }

        protected void TB_Conclusiones_TextChanged(object sender, EventArgs e)
        {
            try
            {
                tabla = new Table();
                tabla = (Table)Session["P_TablaValE"];                
            }
            catch { }
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