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
    public partial class EvaluacionProductos : Negocio.Base_Page
    {
        public String codProyecto;
        public int txtTab = Constantes.ConstSubProductosIndicadores; //Constantes.CONST_subProduccion; //Constantes.CONST_ProyeccionesVentas;
        public String codConvocatoria;
        public Boolean esMiembro;
        /// <summary>
        /// Determina si "está" o "no" realizado...
        /// </summary>
        public Boolean bRealizado;

        protected void Page_Load(object sender, EventArgs e)
        {
            inicioEncabezado(codProyecto, codConvocatoria, Constantes.ConstSubFlujoCaja);
            if (miembro == false && realizado == true && usuario.CodGrupo == Constantes.CONST_Evaluador)
            {
                B_AgregarIndicador.Enabled = false;
                IB_AgregarIndicador.Enabled = false;
            }
            try
            {
                codProyecto = HttpContext.Current.Session["codProyecto"].ToString();
                HttpContext.Current.Session["codProyectoval"] = codProyecto;
                codConvocatoria = HttpContext.Current.Session["codConvocatoria"].ToString();

                codProyecto = Request.QueryString["codProyecto"].ToString();
                HttpContext.Current.Session["codProyectoval"] = codProyecto;
            }
            catch (Exception) { }

            //Consultar si es miembro.
            esMiembro = fnMiembroProyecto(usuario.IdContacto, codProyecto.ToString());

            //Consultar si está "realizado".
            bRealizado = esRealizado(txtTab, Int32.Parse(codProyecto), Int32.Parse(codConvocatoria));

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
            }else
            {
                IB_AgregarIndicador.Visible = false;
                B_AgregarIndicador.Visible = false;

                GV_Indicador.Columns[3].Visible = false;

                foreach (GridViewRow gvr in GV_Indicador.Rows)
                {
                    ((LinkButton)gvr.FindControl("LB_Aspecto")).Enabled = false;
                }
            }

            if (!IsPostBack)
            {
                ObtenerDatosUltimaActualizacion();
            }
            
        }

        private void inicioPagina()
        {
            try
            {
                codProyecto = HttpContext.Current.Session["codProyecto"].ToString();
                HttpContext.Current.Session["codProyectoval"] = codProyecto;
                codConvocatoria = HttpContext.Current.Session["codConvocatoria"].ToString();

                if (!string.IsNullOrEmpty(Request.QueryString["codProyecto"].ToString()))
                {
                    codProyecto = Request.QueryString["codProyecto"].ToString();
                }
                HttpContext.Current.Session["codProyectoval"] = codProyecto;
            }
            catch (Exception) { }

            //codProyecto = "49472";
            //codConvocatoria = "151";
        }

        public DataTable llenarGriView()
        {
            inicioPagina();
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

            cmd = new SqlCommand("SELECT * FROM EvaluacionIndicadorGestion WHERE CodProyecto = " + codProyecto + " AND CodConvocatoria = " + codConvocatoria, conn);

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
            Response.Redirect("~/FONADE/evaluacion/CatalogoIndicadorGestion.aspx?Accion=Crear&IdIndicador=0" + "&codProyecto=" + codProyecto + "&codConvocatoria=" + codConvocatoria);
        }

        protected void B_AgregarIndicador_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/FONADE/evaluacion/CatalogoIndicadorGestion.aspx?Accion=Crear&IdIndicador=0" + "&codProyecto=" + codProyecto + "&codConvocatoria=" + codConvocatoria);
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
                //Actualizar fecha modificación del tab.
                prActualizarTabEval(txtTab.ToString(), codProyecto.ToString(), codConvocatoria.ToString());
                ObtenerDatosUltimaActualizacion();
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
            verCampos();
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
                        codProyecto = HttpContext.Current.Session["codProyectoval"].ToString();
                        codConvocatoria = HttpContext.Current.Session["codConvocatoria"].ToString();
                        prActualizarTabEval(txtTab.ToString(), codProyecto, codConvocatoria);
                        ObtenerDatosUltimaActualizacion();
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

        private void verCampos()
        {

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

        /// <summary>
        /// Mauricio Arias Olave.
        /// 14/05/2014.
        /// Establecer vista y funcionalidad del LinkButton "LBA_Aspecto", lo cual depende del código Grupo
        /// del usuario en sesión.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        #region Métodos de Mauricio Arias Olave.

        /// <summary>
        /// Establecer el primer valor en mayúscula, retornando un string con la primera en maýsucula.
        /// </summary>
        /// <param name="s">String a procesar</param>
        /// <returns>String procesado.</returns>
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

        /// <summary>
        /// Mauricio Arias Olave.
        /// 06/06/2014.
        /// Obtener la información acerca de la última actualización realizada, ási como la habilitación del 
        /// CheckBox para el usuario dependiendo de su grupo / rol.
        /// </summary>
        private void ObtenerDatosUltimaActualizacion()
        {
            //Inicializar variables.
            String txtSQL;
            DateTime fecha = new DateTime();
            DataTable tabla = new DataTable();
            bool bNuevo = true; //Indica si las aprobaciones de las pestañas pueden ser levantadas por el evaluador.
            bRealizado = false;
            bool bEnActa = false; //Determinar si el proyecto esta incluido en un acta de comite evaluador.
            bool EsMiembro = false;
            Int32 CodigoEstado = 0;

            try
            {
                //Consultar si es "Nuevo".
                bNuevo = es_bNuevo(codProyecto.ToString());

                //Determinar si "está en acta".
                bEnActa = es_EnActa(codProyecto.ToString(), codConvocatoria.ToString());

                //Consultar si es "Miembro".
                EsMiembro = fnMiembroProyecto(usuario.IdContacto, codProyecto.ToString());

                //Consultar el "Estado" del proyecto.
                CodigoEstado = CodEstado_Proyecto(txtTab.ToString(), codProyecto, codConvocatoria);

                #region Obtener el rol.

                //Consulta.
                txtSQL = " SELECT CodContacto, CodRol From ProyectoContacto " +
                         " Where CodProyecto = " + codProyecto + " And CodContacto = " + usuario.IdContacto +
                         " and inactivo=0 and FechaInicio<=getdate() and FechaFin is null ";

                //Asignar variables a DataTable.
                var rs = consultas.ObtenerDataTable(txtSQL, "text");

                if (rs.Rows.Count > 0)
                {
                    //Crear la variable de sesión.
                    HttpContext.Current.Session["CodRol"] = rs.Rows[0]["CodRol"].ToString();
                }

                //Destruir la variable.
                rs = null;

                #endregion

                //Consultar los datos a mostrar en los campos correspondientes a la actualización.
                txtSQL = " select nombres+' '+apellidos as nombre, fechamodificacion, realizado  " +
                         " from tabEvaluacionproyecto, contacto " +
                         " where id_contacto = codcontacto and codtabEvaluacion = " + txtTab +
                         " and codproyecto = " + codProyecto +
                         " and codconvocatoria = " + codConvocatoria;

                //Asignar resultados de la consulta a variable DataTable.
                tabla = consultas.ObtenerDataTable(txtSQL, "text");

                //Si tiene datos "y debe tenerlos" ejecuta el siguiente código.
                if (tabla.Rows.Count > 0)
                {
                    //Nombre del usuario quien hizo la actualización.
                    lbl_nombre_user_ult_act.Text = tabla.Rows[0]["nombre"].ToString().ToUpperInvariant();

                    #region Formatear la fecha.

                    //Convertir fecha.
                    try { fecha = Convert.ToDateTime(tabla.Rows[0]["FechaModificacion"].ToString()); }
                    catch { fecha = DateTime.Today; }

                    //Obtener el nombre del mes (las primeras tres letras).
                    string sMes = fecha.ToString("MMM", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));

                    //Obtener la hora en minúscula.
                    string hora = fecha.ToString("hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture).ToLowerInvariant();

                    //Reemplazar el valor "am" o "pm" por "a.m" o "p.m" respectivamente.
                    if (hora.Contains("am")) { hora = hora.Replace("am", "a.m"); } if (hora.Contains("pm")) { hora = hora.Replace("pm", "p.m"); }

                    //Formatear la fecha según manejo de FONADE clásico. "Ej: Nov 19 de 2013 07:36:26 p.m.".
                    lbl_fecha_formateada.Text = UppercaseFirst(sMes) + " " + fecha.Day + " de " + fecha.Year + " " + hora + ".";

                    #endregion

                    //Valor "bRealizado".
                    bRealizado = Convert.ToBoolean(tabla.Rows[0]["Realizado"].ToString());
                }

                //Asignar check de acuerdo al valor obtenido en "bRealizado".
                chk_realizado.Checked = bRealizado;

                //Evaluar "habilitación" del CheckBox.
                //if (!(EsMiembro && HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolCoordinadorEvaluador.ToString()) || lbl_nombre_user_ult_act.Text.Trim() == "" || CodigoEstado != Constantes.CONST_Evaluacion || bEnActa)
                //{ chk_realizado.Enabled = false; }

                //if (EsMiembro && HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolCoordinadorEvaluador.ToString() && lbl_nombre_user_ult_act.Text.Trim() != "" && CodigoEstado == Constantes.CONST_Evaluacion && (!bEnActa))
                //{
                //    btn_guardar_ultima_actualizacion.Enabled = true;
                //    btn_guardar_ultima_actualizacion.Visible = true;
                //}

                //Nuevos controles para los check
                //Si es coordinador de evaluacion debe tener habilitado los checks
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador)
                {
                    btn_guardar_ultima_actualizacion.Visible = true;
                    chk_realizado.Enabled = true;
                }
                else
                {
                    btn_guardar_ultima_actualizacion.Visible = false;
                    chk_realizado.Enabled = false;
                }

                //Destruir variables.
                tabla = null;
                txtSQL = null;
            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error: " + ex.Message + ".')", true);
                //Destruir variables.
                tabla = null;
                txtSQL = null;
                return;
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 06/06/2014.
        /// Obtener el número "numPostIt" usado en la condicional de "obtener última actualización".
        /// El código se encuentra en "Base_Page" línea "116", método "inicioEncabezado".
        /// Ya se le están enviado por parámetro en el método el código del proyecto y la constante "CONST_PostIt".
        /// </summary>
        /// <returns>numPostIt.</returns>
        private int Obtener_numPostIt()
        {
            Int32 numPosIt = 0;

            //Hallar numero de post it por tab
            var query = from tur in consultas.Db.TareaUsuarioRepeticions
                        from tu in consultas.Db.TareaUsuarios
                        from tp in consultas.Db.TareaProgramas
                        where tp.Id_TareaPrograma == tu.CodTareaPrograma
                        && tu.Id_TareaUsuario == tur.CodTareaUsuario
                        && tu.CodProyecto == Convert.ToInt32(codProyecto)
                        && tp.Id_TareaPrograma == Constantes.CONST_PostIt
                        && tur.FechaCierre == null
                        select tur;

            numPosIt = query.Count();

            return numPosIt;
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 24/06/2014.
        /// Guardar la información "Ultima Actualización".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_guardar_ultima_actualizacion_Click(object sender, EventArgs e)
        {
            int flag = 0;
            flag = Marcar(txtTab.ToString(), codProyecto, codConvocatoria, chk_realizado.Checked); 
            ObtenerDatosUltimaActualizacion();
             
            if (flag == 1)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "refreshParent", "window.top.location.reload();", true);
            }
            else
            {
                Response.Redirect(Request.RawUrl);
            }  
        }

        #endregion

        protected void GV_Indicador_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            LinkButton lnkBtn = e.CommandSource as LinkButton;

            switch (e.CommandName)
            {
                case "editar":
                    HttpContext.Current.Session["Accion"] = "Actualizar";
                    HttpContext.Current.Session["CodIndicador"] = e.CommandArgument.ToString();
                    HttpContext.Current.Session["codProyecto"] = codProyecto;
                    HttpContext.Current.Session["codConvocatoria"] = codConvocatoria;

                    //if (e.CommandArgument.ToString() == "") { return; }
                    Response.Redirect("~/FONADE/evaluacion/CatalogoIndicadorGestion.aspx?Accion=Editar&IdIndicador=" + e.CommandArgument.ToString() + "&codProyecto=" + codProyecto + "&codConvocatoria=" + codConvocatoria);
                    break;
                default:
                    break;
            }
        }
    }
}