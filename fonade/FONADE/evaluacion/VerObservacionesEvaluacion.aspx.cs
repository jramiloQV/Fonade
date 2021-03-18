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

namespace Fonade.FONADE.evaluacion
{
    public partial class VerObservacionesEvaluacion : System.Web.UI.Page
    {
        public String codProyecto;
        public int txtTab = Constantes.CONST_ProyeccionesVentas;
        public String codConvocatoria;
        private ProyectoMercadoProyeccionVenta pm;

        private DataTable campo;
        private DataTable orden;
        private DataTable justificacion;

        protected void Page_Load(object sender, EventArgs e)
        {
            datos();

            llenadoDinamico();
        }

        private void datos()
        {
            try
            {
                codProyecto = HttpContext.Current.Session["codProyecto"].ToString();
                HttpContext.Current.Session["codProyectoval"] = codProyecto;
                codConvocatoria = HttpContext.Current.Session["codConvocatoria"].ToString();

                codProyecto = Request.QueryString["codProyecto"].ToString();
                HttpContext.Current.Session["codProyectoval"] = codProyecto;
            }
            catch (Exception) { }
        }

        private void llenadoDinamico()
        {
            datos();
            llenarCampo();

            llenarOrden();

            llenarjustificacion();

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
            sql = @"SELECT [id_Campo], [Campo], [Puntaje]
                    FROM [Campo] AS C, [ConvocatoriaCampo] AS CC
                    WHERE C.[id_Campo] = CC.[codCampo] AND C.[codCampo] IS NULL AND [codConvocatoria] = " + codConvocatoria;

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
                    item["Puntaje"] = reader["Puntaje"].ToString();
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

            orden.Columns.Add("id_Campo");
            orden.Columns.Add("Campo");
            orden.Columns.Add("IDvARIABLE");
            orden.Columns.Add("ORDEN");
            orden.Columns.Add("MAXIMO");
            orden.Columns.Add("ASIGNADO");
            orden.Columns.Add("indice");
            orden.Columns.Add("indice2");

            for(int i = 0; i < campo.Rows.Count; i++)
            {
                String sql;
                sql = @"SELECT C.[id_Campo], C.[Campo], P.[id_Campo] AS IDvARIABLE,
	                    CASE
	                     WHEN CC.[Puntaje] IS NULL
		                    THEN C.[Campo]
		                    ELSE P.[Campo]
	                    END ORDEN
	                    , CC.[Puntaje] AS MAXIMO, ISNULL(EC.[Puntaje], 0) AS ASIGNADO
                    FROM [Campo] AS C
                    LEFT JOIN [Campo] AS P ON C.[codCampo] = P.[id_Campo]
                    LEFT JOIN [ConvocatoriaCampo] AS CC ON C.[id_Campo] = CC.[codCampo] AND
	                    C.[Inactivo] = 0 AND (P.[codCampo] = " + campo.Rows[i]["id_Campo"].ToString() + @" OR C.[codCampo] = " + campo.Rows[i]["id_Campo"].ToString() + @") AND
	                    CC.[codConvocatoria] = " + codConvocatoria + @"
                    LEFT JOIN [EvaluacionCampo] EC ON C.[id_Campo] = EC.[codCampo] AND
	                    CC.[codConvocatoria] = EC.[codConvocatoria] AND
	                    EC.[codProyecto] = " + codProyecto + @"
                    ORDER BY ORDEN, MAXIMO";
                
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
                        item["id_Campo"] = reader["id_Campo"].ToString();
                        item["Campo"] = reader["Campo"].ToString();
                        item["IDvARIABLE"] = reader["IDvARIABLE"].ToString();
                        item["ORDEN"] = reader["ORDEN"].ToString();
                        item["MAXIMO"] = reader["MAXIMO"].ToString();
                        item["ASIGNADO"] = reader["ASIGNADO"].ToString();
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

        private void llenarjustificacion()
        {
            justificacion = new DataTable();

            justificacion.Columns.Add("CodCampo");
            justificacion.Columns.Add("Justificacion");
            justificacion.Columns.Add("indice");
            justificacion.Columns.Add("indice2");

            for (int i = 0; i < orden.Rows.Count; i++)
            {
                String sql;
                sql = @"SELECT [CodCampo],[Justificacion]
                        FROM [EvaluacionCampoJustificacion]
                        WHERE [CodProyecto] = " + codProyecto + @"
                        AND [CodConvocatoria] = " + codConvocatoria + @"
                        AND [CodCampo] = " + orden.Rows[i]["IDvARIABLE"].ToString();

                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd = new SqlCommand(sql, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        DataRow item = justificacion.NewRow();
                        item["CodCampo"] = reader["CodCampo"].ToString();
                        item["Justificacion"] = reader["Justificacion"].ToString();
                        item["indice"] = orden.Rows[i]["indice"].ToString();
                        item["indice2"] = orden.Rows[i]["indice2"].ToString();
                        justificacion.Rows.Add(item);
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

        private void imprimir()
        {
            String txtJustificacion = "";

            for (int i = 0; i < campo.Rows.Count; i++)
            {
                Panel panelPrincipal = new Panel();
                panelPrincipal.ID = "P_" + campo.Rows[i]["Campo"].ToString();

                Label labelCampo = new Label();
                labelCampo.ID = "L_" + campo.Rows[i]["Campo"].ToString();
                labelCampo.Text = campo.Rows[i]["Campo"].ToString();
                
                labelCampo.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2;
                
                labelCampo.CssClass = "fondo";
                labelCampo.Font.Bold = true;

                panelPrincipal.Controls.Add(labelCampo);

                String txtVariable = "";
                for (int j = 0; j < orden.Rows.Count; j++)
                {
                    if (campo.Rows[i]["indice"].ToString().Equals(orden.Rows[j]["indice"].ToString()))
                    {
                        if (!txtVariable.Equals(orden.Rows[j]["ORDEN"].ToString()))
                        {
                            if (orden.Rows[j]["IDvARIABLE"].ToString().Equals(campo.Rows[i]["id_Campo"].ToString()))
                            {
                                txtVariable = orden.Rows[j]["ORDEN"].ToString();

                                Label labelOrden = new Label();

                                labelOrden.ID = "L_" + orden.Rows[j]["ORDEN"].ToString();
                                labelOrden.Text = orden.Rows[j]["ORDEN"].ToString();

                                labelOrden.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2;
                                labelOrden.CssClass = "clasemas";
                                labelOrden.Font.Bold = true;

                                panelPrincipal.Controls.Add(labelOrden);

                                for (int k = 0; k < justificacion.Rows.Count; k++)
                                {
                                    if (justificacion.Rows[k]["CodCampo"].ToString().Equals(orden.Rows[j]["id_Campo"].ToString()))
                                    {
                                        txtJustificacion = justificacion.Rows[k]["Justificacion"].ToString();

                                        TextBox textboxJustificacion = new TextBox();

                                        textboxJustificacion.ID = "L_" + justificacion.Rows[k]["justificacion"].ToString() + "" + j;
                                        textboxJustificacion.Text = justificacion.Rows[k]["Justificacion"].ToString();
                                        textboxJustificacion.BackColor = System.Drawing.Color.White;
                                        textboxJustificacion.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2;
                                        textboxJustificacion.ForeColor = System.Drawing.Color.Black;
                                        textboxJustificacion.TextMode = TextBoxMode.MultiLine;
                                        textboxJustificacion.Rows = 7;
                                        textboxJustificacion.Enabled = false;

                                        panelPrincipal.Controls.Add(textboxJustificacion);

                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                TableRow fila = new TableRow();
                T_Observaciones.Rows.Add(fila);

                TableCell celda = new TableCell();
                celda.Controls.Add(panelPrincipal);

                fila.Cells.Add(celda);
            }
        }
    }
}