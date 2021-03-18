using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
namespace Fonade.FONADE.Convocatoria
{
    /// <summary>
    /// EvaluacionConvocatoria
    /// </summary>    
    public partial class EvaluacionConvocatoria : Negocio.Base_Page
    {
        /// <summary>
        /// Gets the identifier version proyecto.
        /// </summary>
        /// <value>
        /// The identifier version proyecto.
        /// </value>
        public int IdVersionProyecto
        {
            get { return Request.QueryString.AllKeys.Contains("IdVersionProyecto") ? int.Parse(Request.QueryString["IdVersionProyecto"]) : 0; }
        }

        /// <summary>
        /// The cod convocatoria
        /// </summary>
        public String codConvocatoria;

        private DataTable campo;
        private DataTable orden;
        //private DataTable justificacion;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                codConvocatoria = Request.QueryString["IdConvoct"];
                datos();
                llenadoDinamico();
            }

            if (IdVersionProyecto == Datos.Constantes.CONST_PlanV2) btnActualizat.Visible = false;

        }

        private void datos()
        {
            try
            {
                //codConvocatoria = HttpContext.Current.Session["Id_EvalConvocatoria"].ToString();
                lblLink.Text = "<a href=\"javascript:OpenPage('../Convocatoria/FrameAspectos.aspx?Id_EvalConvocatoria=" + codConvocatoria + "&IdVersionProyecto=" + IdVersionProyecto + "')\">Agregar Aspectos</a>";
            }
            catch (Exception) { }
        }

        private void llenadoDinamico()
        {
            datos();
            llenarCampo();

            llenarOrden();

            //llenarjustificacion();

            imprimir();
        }

        private void llenarCampo()
        {
            campo = new DataTable();

            campo.Columns.Add("id_Campo");
            campo.Columns.Add("Campo");
            campo.Columns.Add("Puntaje");
            campo.Columns.Add("indice");

            String sql;
            sql = @"SELECT [id_Campo], [Campo], [Puntaje], [ValorPorDefecto], [TipoCampo]
                    FROM [Campo] AS C, [ConvocatoriaCampo] AS CC
                    WHERE C.[id_Campo] = CC.[codCampo] AND C.[codCampo] IS NULL AND [codConvocatoria] = " + codConvocatoria + " order by id_Campo";

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd = new SqlCommand(sql, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                int i = 0;
                while (reader.Read())
                {
                    DataRow item = campo.NewRow();
                    item["id_Campo"] = reader["id_Campo"].ToString();
                    item["Campo"] = reader["Campo"].ToString();
                    item["Puntaje"] = IdVersionProyecto.Equals(Datos.Constantes.CONST_PlanV2) ? 
										(reader["TipoCampo"].Equals(1) ? "SI" : reader["Puntaje"]) : reader["Puntaje"].ToString();
                    item["indice"] = i;
                    campo.Rows.Add(item);
                    i += 1;
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
        }

        private void llenarOrden()
        {
            orden = new DataTable();

            orden.Columns.Add("id_campo");
            orden.Columns.Add("campo");
            orden.Columns.Add("orden");
            orden.Columns.Add("Maximo");
            orden.Columns.Add("indice");
            orden.Columns.Add("indice2");

            for (int i = 0; i < campo.Rows.Count; i++)
            {
                String sql;
				//Se agrega a la consulta ORDER BY C.codCampo, para ordenar por su padre William P.L.
				//Se quita condicion C.Inactivo = 0  and
				sql = "SELECT C.id_campo, c.campo, case when cc.Puntaje is null then c.campo else p.campo end orden, cc.Puntaje Maximo, C.ValorPorDefecto, C.TipoCampo " +
                        "FROM CAMPO C LEFT JOIN CAMPO P ON c.codcampo=p.id_campo  " +
                        "INNER JOIN ConvocatoriaCampo CC ON C.id_campo = CC.codCampo AND  " +
                        "codconvocatoria=" + codConvocatoria + " AND C.codCampo is NOT null and P.codcampo = " + campo.Rows[i]["id_Campo"].ToString() + " " +
						"ORDER BY C.codCampo, C.id_campo, orden, maximo";

				SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd = new SqlCommand(sql, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        DataRow item = orden.NewRow();
                        item["id_campo"] = reader["id_campo"].ToString();
                        item["campo"] = reader["campo"].ToString();
                        item["orden"] = reader["orden"].ToString();
                        item["Maximo"] = IdVersionProyecto.Equals(Datos.Constantes.CONST_PlanV2) ? 
										(reader["TipoCampo"].Equals(1) ? 
											"SI" : reader["Maximo"]) : reader["Maximo"].ToString();


						//Se cambió "SI" : reader["ValorPorDefecto"]) por "SI" : reader["Maximo"])

						item["indice"] = campo.Rows[i]["indice"].ToString();
                        item["indice2"] = i;
                        orden.Rows.Add(item);
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
            }
        }

        //private void llenarjustificacion()
        //{
        //    justificacion = new DataTable();

        //    justificacion.Columns.Add("puntaje");
        //    justificacion.Columns.Add("indice");
        //    justificacion.Columns.Add("indice2");

        //    for (int i = 0; i < orden.Rows.Count; i++)
        //    {
        //        String sql;

        //        sql = "select puntaje from evaluacioncampo e where  e.codcampo=" + orden.Rows[i]["id_campo"].ToString() + " and e.codconvocatoria=" + codConvocatoria;

        //        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
        //        SqlCommand cmd = new SqlCommand(sql, conn);

        //        cmd = new SqlCommand(sql, conn);

        //        try
        //        {
        //            conn.Open();
        //            SqlDataReader reader = cmd.ExecuteReader();

        //            while (reader.Read())
        //            {
        //                DataRow item = justificacion.NewRow();
        //                item["puntaje"] = reader["puntaje"].ToString();
        //                item["indice"] = orden.Rows[i]["indice"].ToString();
        //                item["indice2"] = orden.Rows[i]["indice2"].ToString();
        //                justificacion.Rows.Add(item);
        //            }
        //            reader.Close();
        //        }
        //        catch (SqlException)
        //        {
        //        }
        //        finally
        //        {
        //            conn.Close();
        //        }
        //    }
        //}

        private void panelEncabezado()
        {

            Label lblDes = new Label();
            lblDes.ID = "lblDes";
            lblDes.Text = "Descripción<br/>";
            lblDes.Width = (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2) - 150;
            lblDes.CssClass = "fondo";
            lblDes.Font.Bold = true;
            lblDes.Style.Add(HtmlTextWriterStyle.PaddingTop, "14px");
            lblDes.Style.Add(HtmlTextWriterStyle.TextAlign, "center");

            Label lblMinApro = new Label();
            lblMinApro.ID = "lblDes";
            lblMinApro.Text = "Mínimo Aprobatorio<br/>";
            lblMinApro.Width = (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2) - 150;
            lblMinApro.CssClass = "fondo";
            lblMinApro.Font.Bold = true;
            lblMinApro.Style.Add(HtmlTextWriterStyle.Width, "100px");
            lblMinApro.Style.Add(HtmlTextWriterStyle.TextAlign, "center");

            Panel pnlEncabezado = new Panel();
            pnlEncabezado.ID = "pnlEncabezado";

            pnlEncabezado.Controls.Add(lblDes);
            pnlEncabezado.Controls.Add(lblMinApro);

            TableRow filaEncabezado = new TableRow();
            T_Observaciones.Rows.Add(filaEncabezado);
            TableCell celdaEncabezado = new TableCell();
            celdaEncabezado.Controls.Add(pnlEncabezado);
            filaEncabezado.Cells.Add(celdaEncabezado);

        }

        private void imprimir()
        {
            String txtJustificacion = "";

            panelEncabezado();

            for (int i = 0; i < campo.Rows.Count; i++)
            {
                Panel panelPrincipal = new Panel();
                panelPrincipal.ID = "P_" + campo.Rows[i]["Campo"].ToString();

                Label labelCampo = new Label();
                labelCampo.ID = "L_" + campo.Rows[i]["Campo"].ToString();
                labelCampo.Text = "" + campo.Rows[i]["Campo"].ToString();

                labelCampo.Width = (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2) - 150;

                labelCampo.CssClass = "fondo";
                labelCampo.Font.Bold = true;


                panelPrincipal.Controls.Add(labelCampo);



                //TextBox textbospuntajeMax = new TextBox();
                //textbospuntajeMax.ID = "txtMax" + campo.Rows[i]["Campo"].ToString().Replace(" ", "");
                //textbospuntajeMax.Text = campo.Rows[i]["Puntaje"].ToString();
                //textbospuntajeMax.CssClass = campo.Rows[i]["id_campo"].ToString();
                //textbospuntajeMax.CssClass = "fondo";
                //textbospuntajeMax.Font.Bold = true;
                //textbospuntajeMax.Attributes.Add("onkeypress", "javascript: return ValidNum(event);");
                //textbospuntajeMax.Width = 100;
                //panelPrincipal.Controls.Add(textbospuntajeMax);

                var txtAtribMax = new HtmlInputText();
                txtAtribMax.Name = "txtAtribMax_" + i;
                txtAtribMax.ID = "txtAtribMax_" + campo.Rows[i]["id_campo"].ToString();
                txtAtribMax.Value = campo.Rows[i]["Puntaje"].ToString();
                txtAtribMax.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
                txtAtribMax.Style.Add(HtmlTextWriterStyle.Width, "100px");
                txtAtribMax.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#00468f");
                txtAtribMax.Style.Add(HtmlTextWriterStyle.Color, "white");
                txtAtribMax.Attributes.Add("onkeypress", "javascript: return ValidNum(event);");
                txtAtribMax.ClientIDMode = ClientIDMode.Static;
                panelPrincipal.Controls.Add(txtAtribMax);
                if (IdVersionProyecto.Equals(Datos.Constantes.CONST_PlanV2)) txtAtribMax.Disabled = true;
                CamposMax.Value += "txtAtribMax_" + campo.Rows[i]["id_campo"].ToString() + "|";

                String txtVariable = "";
                TotalRows.Value = orden.Rows.Count.ToString();
                for (int j = 0; j < orden.Rows.Count; j++)
                {
                    if (campo.Rows[i]["indice"].ToString().Equals(orden.Rows[j]["indice"].ToString()))
                    {
                        if (!txtVariable.Equals(orden.Rows[j]["orden"].ToString()))
                        {
                            txtVariable = orden.Rows[j]["orden"].ToString();

                            Label labelOrden = new Label();

                            labelOrden.ID = "L_" + orden.Rows[j]["orden"].ToString() + j;
                            labelOrden.Text = orden.Rows[j]["orden"].ToString();

                            labelOrden.Width = (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2);
                            labelOrden.CssClass = "clasemas";
                            labelOrden.Font.Bold = true;

                            panelPrincipal.Controls.Add(labelOrden);
                        }
                        txtJustificacion = orden.Rows[j]["Campo"].ToString();
                        TextBox textboxJustificacion = new TextBox();
                        textboxJustificacion.ID = "L_" + orden.Rows[j]["Campo"].ToString() + "" + j;
                        textboxJustificacion.Text = orden.Rows[j]["Campo"].ToString();
                        textboxJustificacion.BackColor = System.Drawing.Color.White;
                        textboxJustificacion.Width = (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2) - 150;
                        textboxJustificacion.Height = 50;
                        textboxJustificacion.ForeColor = System.Drawing.Color.Black;
                        textboxJustificacion.TextMode = TextBoxMode.MultiLine;
                        textboxJustificacion.Enabled = false;

                        panelPrincipal.Controls.Add(textboxJustificacion);
                        HtmlInputText textbospuntaje = new HtmlInputText();

                        textbospuntaje.Name = "txtPuntajeCampo_" + j;
                        textbospuntaje.ID = "txtPuntajeCampo_" + orden.Rows[j].ItemArray[0].ToString();

                        string txtSQL = "select puntaje from evaluacioncampo e where  e.codcampo=" + orden.Rows[j]["id_campo"].ToString() + " and e.codconvocatoria=" + codConvocatoria;
                        //SqlDataReader reader = ejecutaReader(txtSQL, 1);
                        var reader = consultas.ObtenerDataTable(txtSQL, "text");
                        if (reader.Rows.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(reader.Rows[0].ItemArray[0].ToString()))
                            {
                                try
                                {
                                    if (Convert.ToInt32(reader.Rows[0].ItemArray[0].ToString()) > 0)
                                    {
                                        textbospuntaje.Disabled = true;
                                    }
                                }
                                catch (FormatException) { }
                            }
                        }
                        textbospuntaje.Value = orden.Rows[j]["Maximo"].ToString();
                        textbospuntaje.Attributes.Add("onkeypress", "javascript: return ValidNum(event);");
                        CampoName.Value += "txtPuntajeCampo_" + orden.Rows[j]["id_campo"].ToString() + "|";
                        textbospuntaje.Attributes.Add("onkeypress", "javascript: return ValidNum(event);");
                        textbospuntaje.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                        if (IdVersionProyecto.Equals(Datos.Constantes.CONST_PlanV2)) textbospuntaje.Disabled = true;
                        textbospuntaje.Style.Add(HtmlTextWriterStyle.Width, "100px");
                        textbospuntaje.Style.Add(HtmlTextWriterStyle.Height, "10px");
                        panelPrincipal.Controls.Add(textbospuntaje);
                    }
                }
                TableRow fila = new TableRow();
                T_Observaciones.Rows.Add(fila);
                TableCell celda = new TableCell();
                celda.Controls.Add(panelPrincipal);
                fila.Cells.Add(celda);
            }
            if (!string.IsNullOrEmpty(CampoName.Value))
            {
                CampoName.Value = CampoName.Value.Substring(0, CampoName.Value.Length - 1);
            }


        }

        /// <summary>
        /// Handles the Click event of the btnActualizat control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnActualizat_Click(object sender, EventArgs e)
        {
            //int CountRows=Convert.ToInt32(TotalRows.Value);
            string namecontrol = "ctl00$bodyContentPlace$";

            string[] arrName = CampoName.Value.Split('|');
            foreach (var item in arrName)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    string nomControl = namecontrol + item;
                    //var val = Request.Form[nomControl].ToString();
                    if (Request.Form[nomControl] != null)
                    {
                        string textbospuntajeMax = Request.Form[nomControl].ToString();
                        ejecutaActualiza(textbospuntajeMax, item.Split('_')[1]);
                    }
                }
            }

            string[] arrNameMax = CamposMax.Value.Split('|');
            foreach (var itemMax in arrNameMax)
            {
                if (!string.IsNullOrEmpty(itemMax))
                {
                    var nomControl = namecontrol + itemMax;
                    var valTextBox = Request.Form[nomControl].ToString();
                    ejecutaActualiza(valTextBox, itemMax.Split('_')[1]);
                }
            }

            //ejecutaActualiza(textbospuntajeMax, textbospuntajeMax.CssClass);
            //foreach (TableRow tr in T_Observaciones.Rows)
            //{
            //foreach (TableCell tc in tr.Cells)
            //{
            //    foreach (Control control in tc.Controls)
            //    {
            //        if (control is Panel)
            //        {
            //            foreach (Control ctl in ((Panel)control).Controls)
            //            {
            //                if (ctl is TextBox)
            //                {
            //                    try
            //                    {
            //                        TextBox textbospuntajeMax = (TextBox)ctl;
            //                        ejecutaActualiza(textbospuntajeMax.Text, textbospuntajeMax.CssClass);
            //                    }
            //                    catch (NullReferenceException) { }
            //                }
            //            }
            //        }
            //    }
            //}
            //}
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Evaluación actualizada') ; location.href = 'CatalogoConvocatoria.aspx'", true);

            //var controles = HttpContext.Current.Request.Form.AllKeys;
            //for (var i = 0; i < controles.Length; i++)
            //{
            //    if (controles[i].StartsWith("ctl00$bodyContentPlace$txtPuntajeCampo_"))
            //    {
            //        var idControl = controles[i].Split('_');
            //        var valor = HttpContext.Current.Request.Form[i];
            //        ejecutaActualiza(valor, idControl[1]);
            //    }
            //}

            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Evaluación actualizada'); document.location=('EvaluacionConvocatoria.aspx?IdConvoct='" + Request.QueryString["IdConvoct"] + ");", true);
        }

        private void ejecutaActualiza(string objeto, string id)
        {
            codConvocatoria = HttpContext.Current.Session["Id_EvalConvocatoria"].ToString();

            string txtSQL;
            txtSQL = "UPDATE ConvocatoriaCampo " +
               "SET Puntaje = " + objeto +
               " WHERE codCampo = " + id + " AND  codConvocatoria= " + codConvocatoria;
            ejecutaReader(txtSQL, 2);
        }

        /// <summary>
        /// Ejecutas the reader.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public SqlDataReader ejecutaReader(String sql, int obj)
        {
            SqlDataReader reader = null;

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(sql, conn);

            try
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                        reader.Close();
                }

                if (conn != null)
                    conn.Close();

                conn.Open();

                if (obj == 1)
                    reader = cmd.ExecuteReader();
                else
                    cmd.ExecuteReader();
            }
            catch (SqlException)
            {
                if (conn != null)
                    conn.Close();
                return null;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return reader;
        }

        /// <summary>
        /// Handles the Click event of the lnkadicionar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnkadicionar_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["Id_EvalConvocatoriaAS"] = codConvocatoria;

            Redirect(null, "FrameAspectos.aspx", "_Blank", "width=990,height=700");
        }
    }
}