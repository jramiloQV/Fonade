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
    public partial class Conclusion : Negocio.Base_Page
    {
        DataTable datatable;

        public int CodigoProyecto { get { return Convert.ToInt32(Request.QueryString["codproyecto"]); } set { } }
        public int CodigoConvocatoria
        {
            get
            {
                return Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(CodigoProyecto, HttpContext.Current.Session["HistorialEvaluacion"] != null ? Convert.ToInt32(HttpContext.Current.Session["HistorialEvaluacion"]) : 0).GetValueOrDefault();
            }
            set { }
        }
        public int CodigoTab { get { return Constantes.Const_ConclusionDeViabilidadV2; } set { } }
        private ProyectoMercadoProyeccionVenta pm;
        Int32 bolValorRecomendado;
        public bool esMiembro;
        public Boolean bRealizado;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                EncabezadoEval.IdProyecto = CodigoProyecto;
                EncabezadoEval.IdConvocatoria = CodigoConvocatoria;
                EncabezadoEval.IdTabEvaluacion = CodigoTab;

                inicioEncabezado(CodigoProyecto.ToString(), CodigoConvocatoria.ToString(), CodigoTab);

                if (!Page.IsPostBack)
                {
                    string txtSQl = "SELECT empleosevaluacion FROM EvaluacionObservacion WHERE CodProyecto=" + CodigoProyecto + " AND CodConvocatoria= " + CodigoConvocatoria;

                    var empleosevaluacion = consultas.ObtenerDataTable(txtSQl, "text");

                    if (empleosevaluacion.Rows.Count > 0)
                    {
                        txtEmpleosDetectados.Text = empleosevaluacion.Rows[0][0].ToString();
                    }
                    llenarPlantilla();
                }

                esMiembro = fnMiembroProyecto(usuario.IdContacto, CodigoProyecto.ToString());
                bRealizado = esRealizado(CodigoTab, CodigoProyecto, CodigoConvocatoria);

                if (esMiembro && usuario.CodGrupo == Constantes.CONST_Evaluador && !bRealizado) { B_Guardar.Visible = true; }
                if (esMiembro && bRealizado && usuario.CodGrupo == Constantes.CONST_Evaluador)
                {
                    B_Guardar.Enabled = false;
                    __TB_Justificacion.Enabled = false;
                }

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
                    B_Guardar.Visible = true;
                    __TB_Justificacion.Enabled = true;
                }
                else
                {
                    #region Ocultar otros campos...
                    if (usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador)
                    {
                    }
                    else
                    {
                        DropDownList1.Enabled = false;
                        DDL_Conceptos.Enabled = false;
                        __TB_Justificacion.Enabled = false;
                    }


                    #endregion

                    DDL_Conceptos.Enabled = false;
                    DropDownList1.Enabled = false;
                    __TB_Justificacion.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error al cargar los datos, intentalo de nuevo.');", true);
            }
        }

        private void llenarPlantilla()
        {
            vonvocatoria();
            String sql;
            sql = "SELECT [id_evaluacionconceptos], [nomevaluacionconceptos] FROM [evaluacionconceptos] ORDER BY [id_evaluacionconceptos]";

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd = new SqlCommand(sql, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                DDL_Conceptos.Items.Clear();
                while (reader.Read())
                {
                    ListItem item = new ListItem();
                    item.Text = reader["nomevaluacionconceptos"].ToString();
                    item.Value = reader["id_evaluacionconceptos"].ToString();

                    if (!(datatable.Rows.Count < 1))
                        if (datatable.Rows[0]["codevaluacionconceptos"].ToString().Equals(reader["id_evaluacionconceptos"].ToString()))
                            item.Selected = true;

                    DDL_Conceptos.Items.Add(item);
                }
                DDL_Conceptos.Items.Insert(0, new ListItem("Seleccione el concepto que corresponda", "0"));
                reader.Close();

                if (!(datatable.Rows.Count < 0))
                {
                    __TB_Justificacion.Text = datatable.Rows[0]["Justificacion"].ToString();

                    if (datatable.Rows[0]["Viable"] == null)
                    {
                        DropDownList1.Items[0].Selected = false;
                    }
                    else if (datatable.Rows[0]["Viable"].ToString().ToLower().Equals("true"))
                    {
                        DropDownList1.Items[1].Selected = true;
                    }
                    else
                    {
                        DropDownList1.Items[0].Selected = false;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        private void vonvocatoria()
        {
            datatable = new DataTable();

            datatable.Columns.Add("CodConvocatoria");
            datatable.Columns.Add("CodProyecto");
            datatable.Columns.Add("Fecha");
            datatable.Columns.Add("Justificacion");
            datatable.Columns.Add("Viable");
            datatable.Columns.Add("codevaluacionconceptos");

            String sql;
            sql = "SELECT * FROM [ConvocatoriaProyecto] WHERE [CodProyecto] = " + CodigoProyecto + " AND [CodConvocatoria] = " + CodigoConvocatoria;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(sql, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    DataRow fila = datatable.NewRow();
                    fila["CodConvocatoria"] = reader["CodConvocatoria"].ToString();
                    fila["CodProyecto"] = reader["CodProyecto"].ToString();
                    fila["Fecha"] = reader["Fecha"].ToString();
                    fila["Justificacion"] = reader["Justificacion"].ToString();
                    fila["Viable"] = reader["Viable"].ToString();
                    fila["codevaluacionconceptos"] = reader["codevaluacionconceptos"].ToString();
                    datatable.Rows.Add(fila);
                }
                reader.Close();
            }
            catch (SqlException se) { }
            finally
            { conn.Close(); conn.Dispose(); }
        }

        protected void B_Guardar_Click(object sender, EventArgs e)
        {
            string validar = "";
            ClientScriptManager cm = this.ClientScript;
            validar = fnValidar_Campos();

            if (validar == "")
            {
                string conexionStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                using (var con = new SqlConnection(conexionStr))
                {
                    using (var com = con.CreateCommand())
                    {
                        com.CommandText = "MD_Actualizar_ConvocatoriaProyecto";
                        com.CommandType = System.Data.CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@_CodConvocatoria", CodigoConvocatoria);
                        com.Parameters.AddWithValue("@_CodProyecto", CodigoProyecto);
                        com.Parameters.AddWithValue("@_Justificacion", __TB_Justificacion.Text);
                        com.Parameters.AddWithValue("@_Viable", DropDownList1.SelectedIndex);
                        com.Parameters.AddWithValue("@_codevaluacionconceptos", DDL_Conceptos.SelectedValue);

                        try
                        {
                            con.Open();
                            com.ExecuteReader();
                        }
                        catch (Exception ex)
                        { }
                        finally
                        {
                            com.Dispose();
                            con.Close();
                            con.Dispose();
                        }
                    }
                }

                if (!string.IsNullOrEmpty(txtEmpleosDetectados.Text))
                {
                    var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
                    var sql = string.Empty;
                    string txtSQl = "SELECT empleosevaluacion FROM EvaluacionObservacion WHERE CodProyecto=" + CodigoProyecto + " AND CodConvocatoria= " + CodigoConvocatoria;
                    var empleosevaluacion = consultas.ObtenerDataTable(txtSQl, "text");
                    if (empleosevaluacion.Rows.Count > 0)
                    {
                        sql = "Update EvaluacionObservacion set empleosevaluacion = " + txtEmpleosDetectados.Text.Trim() + " Where CodProyecto = " + CodigoProyecto + " AND CodConvocatoria = " + CodigoConvocatoria;
                    }
                    else
                    {
                        sql = "Insert EvaluacionObservacion(CodProyecto, CodConvocatoria, empleosevaluacion) Values(" + CodigoProyecto + ", " + CodigoConvocatoria + ", " + txtEmpleosDetectados.Text.Trim() + ")";
                    }

                    var cmd = new SqlCommand(sql, conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    conn.Dispose();


                }

                UpdateTab();

                return;
            }
            else
            {
                cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('" + validar + "');</script>");
                return;
            }
        }

        private bool fnValidoPuntaje(String CodProyecto, String CodConvocatoria)
        {
            //Inicializar variables.
            String txtSQL;
            bool bValido = true;
            DataTable RsAspecto = new DataTable();
            DataTable RS = new DataTable();

            try
            {
                //Obtener el puntaje mininmo aprobatorio para cada aspecto
                txtSQL = " select id_campo, puntaje " +
                         " from convocatoriacampo cc, campo c " +
                         " where id_campo=cc.codcampo and c.codcampo is null " +
                         " and codconvocatoria = " + CodConvocatoria;

                RsAspecto = consultas.ObtenerDataTable(txtSQL, "text");

                foreach (DataRow r_RsAspecto in RsAspecto.Rows)
                {
                    //Puntaje obtenido en el aspecto
                    txtSQL = " select sum(ec.puntaje) puntaje " +
                             " from evaluacioncampo ec " +
                             " inner join campo c on c.id_campo = ec.codcampo " +
                             " inner join campo v on v.id_campo = c.codcampo " +
                             " inner join campo a on a.id_campo = v.codcampo " +
                             " where codproyecto=" + CodProyecto + " and codconvocatoria = " + CodConvocatoria +
                             " and a.id_campo = " + r_RsAspecto["id_campo"].ToString();

                    RS = consultas.ObtenerDataTable(txtSQL, "text");

                    if (Int32.Parse(RS.Rows[0]["puntaje"].ToString()) < Int32.Parse(r_RsAspecto["puntaje"].ToString()))
                    { bValido = false; break; }
                }

                RsAspecto = null;
                RS = null;
                return bValido;
            }
            catch { return false; }
        }

        private Boolean validar()
        {
            Boolean resul = true;
            DataTable comparacion = new DataTable();

            comparacion.Columns.Add("id_Campo");
            comparacion.Columns.Add("Puntaje");

            String sql;
            sql = @"SELECT [id_Campo], [Puntaje]
                    FROM [ConvocatoriaCampo] AS CC, [Campo] AS C
                    WHERE [id_Campo] = CC.[codCampo] AND C.[codCampo] IS NULL
                    AND [codConvocatoria] = " + CodigoConvocatoria;

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd = new SqlCommand(sql, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    DataRow fila = comparacion.NewRow();

                    fila["id_Campo"] = reader["id_Campo"].ToString();
                    fila["Puntaje"] = reader["Puntaje"].ToString();

                    comparacion.Rows.Add(fila);
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


            for (int i = 0; i < comparacion.Rows.Count; i++)
            {
                sql = @"SELECT SUM(EC.[Puntaje]) AS PUNTAJE
                    FROM [EvaluacionCampo] AS EC
                    INNER JOIN [Campo] AS C ON C.[id_Campo] = EC.[codCampo]
                    INNER JOIN [Campo] AS V ON V.[id_Campo] = C.[codCampo]
                    INNER JOIN [Campo] AS A ON A.[id_Campo] = V.[codCampo]
                    WHERE [codProyecto] = " + CodigoProyecto + @" AND [codConvocatoria] = " + CodigoConvocatoria + @"
                    AND A.[id_Campo] = " + comparacion.Rows[i]["id_Campo"].ToString();

                conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
                cmd = new SqlCommand(sql, conn);

                cmd = new SqlCommand(sql, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        if (Int32.Parse(reader["PUNTAJE"].ToString()) < Int32.Parse(comparacion.Rows[i]["Puntaje"].ToString()))
                        {
                            resul = false;
                            return resul;
                        }
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

            return resul;
        }

        protected void I_AyudaProVentas_Click(object sender, ImageClickEventArgs e)
        {
            HttpContext.Current.Session["mensaje"] = 7;
            ClientScriptManager cm = this.ClientScript;
            cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>open('../Ayuda/Mensaje.aspx', 'Conceptos de Justificacion:', 'width=500,height=400');</script>");
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            HttpContext.Current.Session["mensaje"] = 8;
            ClientScriptManager cm = this.ClientScript;
            cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>open('../Ayuda/Mensaje.aspx', 'Justificacion', 'width=500,height=400');</script>");
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
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        private string fnValidar_Campos()
        {
            var empleosGenerados = 1;

            if (!string.IsNullOrEmpty(txtEmpleosDetectados.Text))
            {
                empleosGenerados = int.Parse(txtEmpleosDetectados.Text);
            }
            else
            {
                empleosGenerados = 1;
            }

            //Inicializar variables.
            String txtSQL = "";
            DataTable rs = new DataTable();
            DataTable rsPuntaje = new DataTable();

            string valor = "";

            bolValorRecomendado = 0;
            txtSQL = " SELECT ISNULL(SUM(ValorRecomendado),0) AS Recomendado FROM EvaluacionObservacion WHERE CodProyecto = " + CodigoProyecto +
                     " AND CodConvocatoria = " + CodigoConvocatoria;
            rs = consultas.ObtenerDataTable(txtSQL, "text");

            if (rs.Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(rs.Rows[0]["Recomendado"].ToString()))
                {
                    if (Int32.Parse(rs.Rows[0]["Recomendado"].ToString()) > 0)
                        bolValorRecomendado = 1;
                    else
                        bolValorRecomendado = 0;
                }
            }

            var sqlPuntaje = "SELECT SUM(ec.Puntaje) PuntajeObtenido, " +
                "MAX(a.TipoCampo) AS TipoAspecto, MAX(a.Campo) As Aspecto, " +
                "MAX(cc.Puntaje) AS PuntajeAprobatorio " +
                "FROM dbo.EvaluacionCampo AS ec " +
                "INNER JOIN dbo.Campo AS c ON c.id_Campo = ec.codCampo " +
                "INNER JOIN dbo.Campo AS v ON v.id_Campo = c.codCampo " +
                "INNER JOIN dbo.Campo AS a ON a.id_Campo = v.codCampo " +
                "INNER JOIN dbo.Proyecto AS p ON ec.codProyecto = p.Id_Proyecto " +
                "inner join ConvocatoriaCampo cc " +
                "on cc.codCampo = v.codCampo " +
                "and cc.codConvocatoria = ec.codConvocatoria " +
                "where p.idVersionProyecto = 2 and " +
                "p.id_proyecto = " + CodigoProyecto + " and ec.codconvocatoria = " + CodigoConvocatoria + " " +
                "group by a.id_Campo having SUM(ec.Puntaje) < MAX(cc.Puntaje)";

            rsPuntaje = consultas.ObtenerDataTable(sqlPuntaje, "text");

            //Validacion de cantidad de elementos evaluados
            DataTable CantrsPuntaje = new DataTable();

            var CantsqlPuntaje = "SELECT SUM(ec.Puntaje) PuntajeObtenido, " +
              "MAX(a.TipoCampo) AS TipoAspecto, MAX(a.Campo) As Aspecto, " +
              "MAX(cc.Puntaje) AS PuntajeAprobatorio " +
              "FROM dbo.EvaluacionCampo AS ec " +
              "INNER JOIN dbo.Campo AS c ON c.id_Campo = ec.codCampo " +
              "INNER JOIN dbo.Campo AS v ON v.id_Campo = c.codCampo " +
              "INNER JOIN dbo.Campo AS a ON a.id_Campo = v.codCampo " +
              "INNER JOIN dbo.Proyecto AS p ON ec.codProyecto = p.Id_Proyecto " +
              "inner join ConvocatoriaCampo cc " +
              "on cc.codCampo = v.codCampo " +
              "and cc.codConvocatoria = ec.codConvocatoria " +
              "where p.idVersionProyecto = 2 and " +
              "p.id_proyecto = " + CodigoProyecto + " and ec.codconvocatoria = " + CodigoConvocatoria + " " +
              "group by a.id_Campo";

            CantrsPuntaje = consultas.ObtenerDataTable(CantsqlPuntaje, "text");

            int cantEvaludadosItemPadres = CantrsPuntaje.Rows.Count;
            int cantConvocatoriaCampo = 0;
            int cantEvaluacionCampo = 0;

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                cantConvocatoriaCampo = (from c in db.ConvocatoriaCampos
                                         where c.codConvocatoria == CodigoConvocatoria
                                         select c).Count();

                cantEvaluacionCampo = (from e in db.EvaluacionCampos
                                       where e.codConvocatoria == CodigoConvocatoria &&
                                            e.codProyecto == CodigoProyecto
                                       select e).Count();
            }


            try
            {
                if (DDL_Conceptos.SelectedValue == "")
                    valor = "Debe Seleccionar un concepto";
            }
            catch
            {
                valor = "Debe Seleccionar un concepto";
            }

            if (__TB_Justificacion.Text.Trim() == "")
                valor = "La justificación es requerida";

            if (empleosGenerados.Equals(0))
                valor = "Debe ingresar los empleos generados";

            if (DropDownList1.SelectedValue == "1")
            {
                if (bolValorRecomendado == 0)
                    valor = "El Plan de Negocio no puede ser marcado como viable ya que el Valor Recomendado (smlv) es igual a cero (0).";

                if (rsPuntaje.Rows.Count > 0)
                    valor = "No puede marcar como viable el proyecto porque no cumple los puntajes mínimos aprobatorios.";

                if (cantConvocatoriaCampo != (cantEvaluacionCampo + cantEvaludadosItemPadres))
                    valor = "No puede marcar como viable el proyecto porque no han sido evaluados la totalidad de los aspectos.";

                if (DDL_Conceptos.SelectedValue != "5")
                    valor = "No puede marcar como viable el proyecto, el concepto es incorrecto";
            }
            else
            {
                if (rsPuntaje.Rows.Count == 0)
                    valor = "El Plan de Negocio no puede ser marcado como NO viable, ya que los aspectos de evaluación cumplen con los mínimos aprobatorios.";
                if (DDL_Conceptos.SelectedValue == "5")
                    valor = "No puede marcar como no viable el proyecto, el concepto es incorrecto";
            }

            return valor;
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

        protected void lnkObservaciones_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "window.open('VerObservacionesEvaluacion.aspx?codproyecto=" + CodigoProyecto + "','_blank','width=580,height=300,toolbar=yes, scrollbars=yes, resizable=yes');", true);
        }
    }
}