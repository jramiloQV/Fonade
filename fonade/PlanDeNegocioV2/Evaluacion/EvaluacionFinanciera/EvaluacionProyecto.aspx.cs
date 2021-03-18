using Datos;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Evaluacion.EvaluacionFinanciera
{
    public partial class EvaluacionProyecto : Negocio.Base_Page
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
        public int CodigoTab { get { return Constantes.Const_EvaluacionDelProyectoEvaluacionV2; } set { } }
        private ProyectoMercadoProyeccionVenta pm;
        String selectIndex;
        DataTable TipoSupuesto;
        DataTable EvaluacionProyectoSupuesto;
        DataTable EvaluacionIndicadorFinancieroProyecto;
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

                if (!IsPostBack)
                {
                    inicioEncabezado(CodigoProyecto.ToString(), CodigoConvocatoria.ToString(), 1);
                    cargarDatos();                    
                }

                div_Post_It1.Visible = PostitVisible;
                Post_It1._mostrarPost = PostitVisible;
                DD_TiempoProyeccion.Enabled = AllowUpdate;
                B_Guardar.Visible = AllowUpdate;
                B_ActualizarSupuesto.Visible = AllowUpdate;
                B_ActualizarIndicador.Visible = AllowUpdate;

                cargarSupuestos();
                cargarIndicador();
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

        protected void B_Guardar_Click(object sender, EventArgs e)
        {            
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            String sql;

            try
            {
                sql = "DELETE FROM [EvaluacionProyectoSupuestoValor] WHERE [CodSupuesto] IN (SELECT [Id_EvaluacionProyectoSupuesto] FROM [EvaluacionProyectoSupuesto] WHERE [CodProyecto] = " + CodigoProyecto + " AND[CodConvocatoria] = " + CodigoConvocatoria + ")";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                cmd.ExecuteReader();
                conn.Close();

                sql = "DELETE FROM [EvaluacionIndicadorFinancieroValor] WHERE [CodEvaluacionIndicadorFinancieroProyecto] IN (SELECT [Id_EvaluacionIndicadorFinancieroProyecto] FROM [EvaluacionIndicadorFinancieroProyecto] WHERE [CodProyecto] = " + CodigoProyecto + " AND[CodConvocatoria] = " + CodigoConvocatoria + ")";
                cmd = new SqlCommand(sql, conn);
                conn.Open();
                cmd.ExecuteReader();
                conn.Close();

                sql = "DELETE FROM [EvaluacionRubroValor] WHERE [CodEvaluacionRubroProyecto] IN (SELECT [Id_EvaluacionRubroProyecto] FROM [EvaluacionRubroProyecto] WHERE [CodProyecto] = " + CodigoProyecto + " AND[CodConvocatoria] = " + CodigoConvocatoria + ")";
                cmd = new SqlCommand(sql, conn);
                conn.Open();
                cmd.ExecuteReader();
                conn.Close();

                sql = "SELECT COUNT(*) AS TOTAL FROM [EvaluacionObservacion] WHERE [CodProyecto] = " + CodigoProyecto + " AND[CodConvocatoria] = " + CodigoConvocatoria;
                cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();

                if (Int32.Parse(reader["TOTAL"].ToString()) > 0)
                {
                    sql = "UPDATE [EvaluacionObservacion] SET [TiempoProyeccion] = " + DD_TiempoProyeccion.SelectedValue + " WHERE [CodProyecto] = " + CodigoProyecto + " AND [CodConvocatoria] = " + CodigoConvocatoria;
                }
                else
                {
                    sql = "INSERT INTO [EvaluacionObservacion] ([CodProyecto], [CodConvocatoria], [TiempoProyeccion]) VALUES (" + CodigoProyecto + "," + CodigoConvocatoria + "," + DD_TiempoProyeccion.SelectedValue + ")";
                }

                conn.Close();
                cmd = new SqlCommand(sql, conn);
                conn.Open();
                cmd.ExecuteReader();

                UpdateTab();

            }
            catch (SqlException se)
            {
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            
            Response.Redirect("EvaluacionProyecto.aspx?codproyecto="+CodigoProyecto);
        }

        private void cargarDatos()
        {
            String sql;
            sql = "SELECT [TiempoProyeccion] FROM [EvaluacionObservacion] WHERE [CodProyecto] = " + CodigoProyecto + " AND [CodConvocatoria] = " + CodigoConvocatoria;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(sql, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                    if (!String.IsNullOrEmpty(reader["TiempoProyeccion"].ToString()))
                    {
                        if (!IsPostBack)
                            DD_TiempoProyeccion.SelectedIndex = (Int32.Parse(reader["TiempoProyeccion"].ToString()) - 3);
                        selectIndex = reader["TiempoProyeccion"].ToString();
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
        }

        private void cargarSupuestos()
        {
            selectIndex = DD_TiempoProyeccion.SelectedValue;
            llenarTipoSupuesto();
            llenarEvaluacionProyectoSupuesto();

            if (String.IsNullOrEmpty(selectIndex)) selectIndex = "3";

            for (int i = 0; i < TipoSupuesto.Rows.Count; i++)
            {
                Panel panelPrincipal = new Panel();
                panelPrincipal.ID = "P_" + TipoSupuesto.Rows[i]["NomTipoSupuesto"].ToString();

                Label labelCampo = new Label();
                labelCampo.ID = "L_Supuesto" + TipoSupuesto.Rows[i]["Id_TipoSupuesto"].ToString();
                labelCampo.Text = "Supuestos " + TipoSupuesto.Rows[i]["NomTipoSupuesto"].ToString();
                labelCampo.Width = 31 * Int32.Parse(selectIndex) + 251;
                labelCampo.Font.Bold = true;

                panelPrincipal.Controls.Add(labelCampo);

                Table tablatitullo = new Table();
                tablatitullo.Width = 31 * Int32.Parse(selectIndex) + 251;
                tablatitullo.CssClass = "Grilla";
                TableHeaderRow cabezafila = new TableHeaderRow();

                for (int j = 0; j <= Int32.Parse(selectIndex); j++)
                {
                    TableHeaderCell celda = new TableHeaderCell();
                    Label titulo = new Label();
                    titulo.ID = "L_tituloSupuestos" + TipoSupuesto.Rows[i]["Id_TipoSupuesto"].ToString() + j + 1;

                    if (j == 0)
                    {
                        titulo.Text = "Variable / Periodo";
                        titulo.Width = 250;
                    }
                    else
                    {
                        titulo.Text = "" + j;
                        titulo.Width = 78;
                        titulo.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                    }
                    celda.Controls.Add(titulo);
                    cabezafila.Cells.Add(celda);
                }
                tablatitullo.Rows.Add(cabezafila);

                TableRow fila = new TableRow();
                for (int j = 0; j < EvaluacionProyectoSupuesto.Rows.Count; j++)
                {
                    if (TipoSupuesto.Rows[i]["Id_TipoSupuesto"].ToString().Equals(EvaluacionProyectoSupuesto.Rows[j]["CodTipoSupuesto"].ToString()))
                    {
                        fila = new TableRow();
                        for (int k = 0; k <= Int32.Parse(selectIndex); k++)
                        {
                            TableCell celda = new TableCell();
                            if (k == 0)
                            {
                                Label titulo = new Label();
                                titulo.ID = "L_EvaluacionProyectoSupuesto" + EvaluacionProyectoSupuesto.Rows[j]["Id_EvaluacionProyectoSupuesto"].ToString() + k;
                                titulo.Text = EvaluacionProyectoSupuesto.Rows[j]["NomEvaluacionProyectoSupuesto"].ToString();
                                titulo.Width = 250;
                                celda.Controls.Add(titulo);
                            }
                            else
                            {
                                TextBox textbox = new TextBox();
                                if (EsMiembro && !EsRealizado && usuario.CodGrupo == Constantes.CONST_Evaluador)
                                { textbox.Enabled = true; }
                                else { textbox.Enabled = false; }
                                textbox.ID = "TB_EvaluacionProyectoSupuesto" + EvaluacionProyectoSupuesto.Rows[j]["Id_EvaluacionProyectoSupuesto"].ToString() + k;                                

                                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
                                SqlCommand cmd = new SqlCommand("SELECT [Valor] FROM [EvaluacionProyectoSupuestoValor] WHERE CodSupuesto = " + EvaluacionProyectoSupuesto.Rows[j]["Id_EvaluacionProyectoSupuesto"].ToString() + " AND Periodo = " + k, conn);

                                try
                                {
                                    conn.Open();
                                    SqlDataReader reader = cmd.ExecuteReader();

                                    textbox.Text = "";
                                    if (reader.Read())
                                        textbox.Text = reader["Valor"].ToString();
                                    else
                                        textbox.Text = "0";

                                    reader.Close();
                                }
                                catch (SqlException)
                                {
                                    textbox.Text = "0";
                                }
                                finally
                                {
                                    conn.Close();
                                    conn.Dispose();
                                }

                                textbox.Width = 30;
                                celda.Controls.Add(textbox);
                            }
                            fila.Cells.Add(celda);
                        }
                        tablatitullo.Rows.Add(fila);
                    }
                }
                panelPrincipal.Controls.Add(tablatitullo);

                TableRow filatablaprincipal = new TableRow();
                T_Supuestos.Rows.Add(filatablaprincipal);

                TableCell celdatablaprincipal = new TableCell();
                celdatablaprincipal.Controls.Add(panelPrincipal);

                filatablaprincipal.Cells.Add(celdatablaprincipal);
            }
        }

        private void cargarIndicador()
        {
            selectIndex = DD_TiempoProyeccion.SelectedValue;

            llenarEvaluacionIndicadorFinancieroProyecto();

            if (String.IsNullOrEmpty(selectIndex)) selectIndex = "3";

            Panel panelPrincipal = new Panel();
            panelPrincipal.ID = "P_NomIndicadorFinanciero";

            Table tablatitullo = new Table();            

            tablatitullo.Width = 31 * Int32.Parse(selectIndex) + 251 + 126;
            tablatitullo.CssClass = "Grilla";
            TableHeaderRow cabezafila = new TableHeaderRow();

            for (int j = 0; j <= Int32.Parse(selectIndex) + 1; j++)
            {
                TableHeaderCell celda = new TableHeaderCell();
                Label titulo = new Label();
                titulo.ID = "L_tituloIndicador" + j;

                if (j == 0)
                {
                    titulo.Text = "Variable / Periodo";
                    titulo.Width = 250;
                }
                else
                {
                    if (j == (Int32.Parse(selectIndex) + 1))
                    {
                        titulo.Text = "Promedio Sector";
                        titulo.Width = 125;
                    }
                    else
                    {
                        titulo.Text = "" + j;
                        titulo.Width = 78;
                        titulo.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                    }
                }
                celda.Controls.Add(titulo);
                cabezafila.Cells.Add(celda);
            }
            tablatitullo.Rows.Add(cabezafila);

            TableRow fila = new TableRow();
            for (int i = 0; i < EvaluacionIndicadorFinancieroProyecto.Rows.Count; i++)
            {
                fila = new TableRow();
                for (int k = 0; k <= Int32.Parse(selectIndex) + 1; k++)
                {
                    TableCell celda = new TableCell();
                    if (k == 0)
                    {
                        Label titulo = new Label();
                        titulo.ID = "L_EvaluacionIndicadorFinancieroProyecto" + EvaluacionIndicadorFinancieroProyecto.Rows[i]["Id_EvaluacionIndicadorFinancieroProyecto"].ToString() + k;
                        titulo.Text = EvaluacionIndicadorFinancieroProyecto.Rows[i]["Descripcion"].ToString();
                        titulo.Width = 250;
                        celda.Controls.Add(titulo);
                    }
                    else
                    {
                        TextBox textbox = new TextBox();
                        if (EsMiembro && !EsRealizado && usuario.CodGrupo == Constantes.CONST_Evaluador)
                        { textbox.Enabled = true; }
                        else { textbox.Enabled = false; }
                        textbox.ID = "TB_EvaluacionIndicadorFinancieroProyecto" + EvaluacionIndicadorFinancieroProyecto.Rows[i]["Id_EvaluacionIndicadorFinancieroProyecto"].ToString() + k;

                        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
                        SqlCommand cmd;

                        if (k == (Int32.Parse(selectIndex) + 1))
                        {
                            cmd = new SqlCommand("SELECT [Valor] FROM [EvaluacionIndicadorFinancieroValor]  WHERE [CodEvaluacionIndicadorFinancieroProyecto] = " + EvaluacionIndicadorFinancieroProyecto.Rows[i]["Id_EvaluacionIndicadorFinancieroProyecto"].ToString() + " AND Periodo = 0", conn);
                        }
                        else
                        {
                            cmd = new SqlCommand("SELECT [Valor] FROM [EvaluacionIndicadorFinancieroValor]  WHERE [CodEvaluacionIndicadorFinancieroProyecto] = " + EvaluacionIndicadorFinancieroProyecto.Rows[i]["Id_EvaluacionIndicadorFinancieroProyecto"].ToString() + " AND Periodo = " + k, conn);
                        }

                        try
                        {
                            conn.Open();
                            SqlDataReader reader = cmd.ExecuteReader();

                            textbox.Text = "";
                            if (reader.Read())
                                textbox.Text = reader["Valor"].ToString();
                            else
                                textbox.Text = "0";

                            reader.Close();
                        }
                        catch (SqlException)
                        {
                            textbox.Text = "0";
                        }
                        finally
                        {
                            conn.Close();
                            conn.Close();
                        }

                        textbox.Width = 30;
                        celda.Controls.Add(textbox);
                    }
                    fila.Cells.Add(celda);
                }
                tablatitullo.Rows.Add(fila);
            }

            panelPrincipal.Controls.Add(tablatitullo);

            TableRow filatablaprincipal = new TableRow();
            T_Indicadores.Rows.Add(filatablaprincipal);

            TableCell celdatablaprincipal = new TableCell();
            celdatablaprincipal.Controls.Add(panelPrincipal);

            filatablaprincipal.Cells.Add(celdatablaprincipal);
        }

        private void llenarTipoSupuesto()
        {
            TipoSupuesto = new DataTable();

            TipoSupuesto.Columns.Add("Id_TipoSupuesto");
            TipoSupuesto.Columns.Add("NomTipoSupuesto");

            String sql;
            sql = "SELECT * FROM [TipoSupuesto]";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(sql, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    DataRow fila = TipoSupuesto.NewRow();
                    fila["Id_TipoSupuesto"] = reader["Id_TipoSupuesto"].ToString();
                    fila["NomTipoSupuesto"] = reader["NomTipoSupuesto"].ToString();
                    TipoSupuesto.Rows.Add(fila);
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
        }

        private void llenarEvaluacionProyectoSupuesto()
        {
            EvaluacionProyectoSupuesto = new DataTable();

            EvaluacionProyectoSupuesto.Columns.Add("Id_EvaluacionProyectoSupuesto");
            EvaluacionProyectoSupuesto.Columns.Add("NomEvaluacionProyectoSupuesto");
            EvaluacionProyectoSupuesto.Columns.Add("CodTipoSupuesto");
            EvaluacionProyectoSupuesto.Columns.Add("CodProyecto");
            EvaluacionProyectoSupuesto.Columns.Add("CodConvocatoria");

            String sql;
            sql = "SELECT * FROM [EvaluacionProyectoSupuesto] WHERE [CodProyecto] = " + CodigoProyecto + " AND [CodConvocatoria] = " + CodigoConvocatoria + " ORDER BY [CodTipoSupuesto]";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(sql, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    DataRow fila = EvaluacionProyectoSupuesto.NewRow();
                    fila["Id_EvaluacionProyectoSupuesto"] = reader["Id_EvaluacionProyectoSupuesto"].ToString();
                    fila["NomEvaluacionProyectoSupuesto"] = reader["NomEvaluacionProyectoSupuesto"].ToString();
                    fila["CodTipoSupuesto"] = reader["CodTipoSupuesto"].ToString();
                    fila["CodProyecto"] = reader["CodProyecto"].ToString();
                    fila["CodConvocatoria"] = reader["CodConvocatoria"].ToString();
                    EvaluacionProyectoSupuesto.Rows.Add(fila);
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
        }

        private void llenarEvaluacionIndicadorFinancieroProyecto()
        {
            EvaluacionIndicadorFinancieroProyecto = new DataTable();

            EvaluacionIndicadorFinancieroProyecto.Columns.Add("Id_EvaluacionIndicadorFinancieroProyecto");
            EvaluacionIndicadorFinancieroProyecto.Columns.Add("Descripcion");
            EvaluacionIndicadorFinancieroProyecto.Columns.Add("CodProyecto");
            EvaluacionIndicadorFinancieroProyecto.Columns.Add("CodConvocatoria");

            String sql;
            sql = "SELECT * FROM [EvaluacionIndicadorFinancieroProyecto] WHERE codProyecto = " + CodigoProyecto + " AND codConvocatoria = " + CodigoConvocatoria;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(sql, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    DataRow fila = EvaluacionIndicadorFinancieroProyecto.NewRow();
                    fila["Id_EvaluacionIndicadorFinancieroProyecto"] = reader["Id_EvaluacionIndicadorFinancieroProyecto"].ToString();
                    fila["Descripcion"] = reader["Descripcion"].ToString();
                    fila["CodProyecto"] = reader["CodProyecto"].ToString();
                    fila["CodConvocatoria"] = reader["CodConvocatoria"].ToString();
                    EvaluacionIndicadorFinancieroProyecto.Rows.Add(fila);
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
        }

        protected void B_ActualizarSupuesto_Click(object sender, EventArgs e)
        {
            this.ClientIDMode = ClientIDMode.Static;
            selectIndex = DD_TiempoProyeccion.SelectedValue;
            llenarTipoSupuesto();
            llenarEvaluacionProyectoSupuesto();

            for (int i = 0; i < TipoSupuesto.Rows.Count; i++)
            {
                for (int j = 0; j < EvaluacionProyectoSupuesto.Rows.Count; j++)
                {
                    if (TipoSupuesto.Rows[i]["Id_TipoSupuesto"].ToString().Equals(EvaluacionProyectoSupuesto.Rows[j]["CodTipoSupuesto"].ToString()))
                    {
                        for (int k = 1; k <= Int32.Parse(selectIndex); k++)
                        {
                            String objetoTextBox;
                            objetoTextBox = "TB_EvaluacionProyectoSupuesto" + EvaluacionProyectoSupuesto.Rows[j]["Id_EvaluacionProyectoSupuesto"].ToString() + k;

                            TextBox controlSupuesto = (TextBox)this.Master.FindControl("bodyHolder").FindControl(objetoTextBox);
                                                        
                            try
                            {
                                decimal numero = decimal.Parse(controlSupuesto.Text);
                            }
                            catch (Exception ex)
                            {
                                if (ex is FormatException)
                                {
                                    ClientScriptManager cm = this.ClientScript;
                                    cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('Formato numérico no válido ( " + controlSupuesto.Text + ")');</script>");
                                    return;
                                }
                                else
                                {
                                    ClientScriptManager cm = this.ClientScript;
                                    cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('Error desconocido.');</script>");
                                    return;
                                }
                            }

                            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
                            SqlCommand cmd = new SqlCommand("SELECT [Valor] FROM [EvaluacionProyectoSupuestoValor] WHERE CodSupuesto = " + EvaluacionProyectoSupuesto.Rows[j]["Id_EvaluacionProyectoSupuesto"].ToString() + " AND Periodo = " + k, conn);

                            try
                            {
                                String sql;
                                conn.Open();
                                SqlDataReader reader = cmd.ExecuteReader();

                                if (reader.Read())
                                    sql = "UPDATE [EvaluacionProyectoSupuestoValor] SET [Valor] = " + controlSupuesto.Text + " WHERE [CodSupuesto] = " + EvaluacionProyectoSupuesto.Rows[j]["Id_EvaluacionProyectoSupuesto"].ToString() + " AND [Periodo] = " + k;
                                else
                                    sql = "INSERT INTO [EvaluacionProyectoSupuestoValor] ([CodSupuesto], [Periodo], [Valor]) VALUES (" + EvaluacionProyectoSupuesto.Rows[j]["Id_EvaluacionProyectoSupuesto"].ToString() + ", " + k + ", " + controlSupuesto.Text + ")";

                                reader.Close();
                                conn.Close();

                                ejecutaReader(sql, 2);
                            }
                            catch (SqlException) { }
                            finally
                            {
                                conn.Close();
                                conn.Dispose();
                            }
                        }
                    }
                }
            }

            UpdateTab();
        }

        protected void B_ActualizarIndicador_Click(object sender, EventArgs e)
        {
            selectIndex = DD_TiempoProyeccion.SelectedValue;
            llenarEvaluacionIndicadorFinancieroProyecto();

            for (int i = 0; i < EvaluacionIndicadorFinancieroProyecto.Rows.Count; i++)
            {
                for (int k = 1; k <= Int32.Parse(selectIndex) + 1; k++)
                {
                    String objetoTextBox;
                    objetoTextBox = "TB_EvaluacionIndicadorFinancieroProyecto" + EvaluacionIndicadorFinancieroProyecto.Rows[i]["Id_EvaluacionIndicadorFinancieroProyecto"].ToString() + k;

                    TextBox controlSupuesto = (TextBox)this.Master.FindControl("bodyHolder").FindControl(objetoTextBox);

                    try
                    {
                        decimal numero = decimal.Parse(controlSupuesto.Text);
                    }
                    catch (Exception ex)
                    {
                        if (ex is FormatException)
                        {
                            ClientScriptManager cm = this.ClientScript;
                            cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('Formato numérico no válido ( " + controlSupuesto.Text + ")');</script>");
                            return;
                        }
                        else
                        {
                            ClientScriptManager cm = this.ClientScript;
                            cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('Error desconocido.');</script>");
                            return;
                        }
                    }

                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
                    SqlCommand cmd;

                    int preiodoValor;
                    if (k == (Int32.Parse(selectIndex) + 1))
                    {
                        cmd = new SqlCommand("SELECT [Valor] FROM [EvaluacionIndicadorFinancieroValor]  WHERE [CodEvaluacionIndicadorFinancieroProyecto] = " + EvaluacionIndicadorFinancieroProyecto.Rows[i]["Id_EvaluacionIndicadorFinancieroProyecto"].ToString() + " AND Periodo = 0", conn);
                        preiodoValor = 0;
                    }
                    else
                    {
                        cmd = new SqlCommand("SELECT [Valor] FROM [EvaluacionIndicadorFinancieroValor]  WHERE [CodEvaluacionIndicadorFinancieroProyecto] = " + EvaluacionIndicadorFinancieroProyecto.Rows[i]["Id_EvaluacionIndicadorFinancieroProyecto"].ToString() + " AND Periodo = " + k, conn);
                        preiodoValor = k;
                    }

                    try
                    {
                        String sql;
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                            sql = "UPDATE [EvaluacionIndicadorFinancieroValor] SET [Valor] = " + controlSupuesto.Text + " WHERE [CodEvaluacionIndicadorFinancieroProyecto] = " + EvaluacionIndicadorFinancieroProyecto.Rows[i]["Id_EvaluacionIndicadorFinancieroProyecto"].ToString() + " AND [Periodo] = " + preiodoValor;
                        else
                            sql = "INSERT INTO [EvaluacionIndicadorFinancieroValor] ([CodEvaluacionIndicadorFinancieroProyecto], [Periodo], [Valor]) VALUES (" + EvaluacionIndicadorFinancieroProyecto.Rows[i]["Id_EvaluacionIndicadorFinancieroProyecto"].ToString() + ", " + preiodoValor + ", " + controlSupuesto.Text + ")";

                        reader.Close();

                        ejecutaReader(sql, 2);
                    }
                    catch (SqlException) { }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }

            UpdateTab();
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

       
    }
}