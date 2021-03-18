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

namespace Fonade.FONADE.evaluacion
{
    public partial class EvaluacionRiesgos : Negocio.Base_Page
    {
        public String codProyecto;
        public int txtTab = Constantes.ConstSubRiesgosIdentificados;//Constantes.CONST_ProyeccionesVentas;
        public String codConvocatoria;
        public bool esMiembro;
        /// <summary>
        /// PAra saber si está o no "realizado".
        /// </summary>
        public Boolean bRealizado;

        protected void Page_Load(object sender, EventArgs e)
        {
            inicioEncabezado(codProyecto, codConvocatoria, Constantes.ConstSubFlujoCaja);
            if (miembro == false && realizado == true && usuario.CodGrupo == Constantes.CONST_Evaluador)
            {
                btn_agregar.Enabled = false;
                IB_AgregarIndicador.Enabled = false;
            }
            recogerDatos();
            RestringirLetras(0);

            //Consultar si es miembro.
            esMiembro = fnMiembroProyecto(usuario.IdContacto, codProyecto.ToString());

            //Consultar si está "realizado".
            bRealizado = esRealizado(txtTab, Int32.Parse(codProyecto), Int32.Parse(codConvocatoria)); //Estaba vacío...

            if (esMiembro && !bRealizado)
            { this.div_Post_It1.Visible = true; Post_It1._mostrarPost = true; }

            if (usuario.CodGrupo == Constantes.CONST_GerenteEvaluador)
            {
                IB_AgregarIndicador.Visible = false;
                btn_agregar.Visible = false;
                //this.div_Post_It1.Visible = false;
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
                //this.div_Post_It1.Visible = false;

                GridView1.Columns[0].Visible = false;

                foreach (GridViewRow gvr in GridView1.Rows)
                {
                    ((LinkButton)gvr.Cells[1].FindControl("lnkeditarRiesgo")).Enabled = false;
                }
            }

            if (!IsPostBack) 
            {
                ObtenerDatosUltimaActualizacion();
            }
        }

        private void recogerDatos()
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

            //CtrlCheckedProyecto1.tabCod = Constantes.ConstSubRiesgosIdentificados;
            //CtrlCheckedProyecto1.CodProyecto = codProyecto;
        }

        protected void btn_agregar_Click(object sender, EventArgs e)
        {

            Response.Redirect("CatalogoRiesgoMitigacio.aspx");
        }

        protected void IB_AgregarIndicador_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("CatalogoRiesgoMitigacio.aspx");
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
            recogerDatos();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand("SELECT [Id_Riesgo],[CodProyecto],[CodConvocatoria],[Riesgo],[Mitigacion] FROM [dbo].[EvaluacionRiesgo] WHERE [CodProyecto] = " + codProyecto + " AND [CodConvocatoria] = " + codConvocatoria, conn);
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
            #region Versión 1.0 de Mauricio Arias Olave COMENTADO.
            //SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            //SqlCommand cmd = new SqlCommand("DELETE FROM [dbo].[EvaluacionRiesgo] WHERE [Id_Riesgo] = " + Id_Riesgo, conn);
            //try
            //{

            //    conn.Open();
            //    cmd.ExecuteReader();
            //    conn.Close();
            //    codProyecto = HttpContext.Current.Session["codProyecto"].ToString();
            //    codConvocatoria = HttpContext.Current.Session["codConvocatoria"].ToString();
            //    //Actualizar fecha modificación del tab.
            //    prActualizarTabEval(txtTab.ToString(), codProyecto.ToString(), codConvocatoria.ToString());
            //    ObtenerDatosUltimaActualizacion();
            //}
            //catch (SqlException se)
            //{ }
            //finally
            //{
            //    conn.Close();
            //}
            //ObtenerDatosUltimaActualizacion();
            ////ClientScriptManager cm = this.ClientScript;
            ////cm.RegisterClientScriptBlock(this.GetType(), "", "document.getElementById('frmproduccion').src = document.getElementById('frmproduccion').src");
            ////return;
            //Session["test_data"] = "Riesgos";
            //ScriptManager.RegisterStartupScript(this, typeof(string), "script", "<script type=text/javascript>parent.location.href = parent.location.href;</script>", false);
            ////Response.Redirect("EvaluacionIndicadoresFrame.aspx"); 
            #endregion

            #region Versión 2.0 de Mauricio Arias Olave.

            //Inicializar variables.
            String txtSQL = "";

            //Sentencia de eliminación.
            txtSQL = @"DELETE FROM EvaluacionRiesgo WHERE Id_Riesgo = " + Id_Riesgo;

            //Ejecutar setencia.
            ejecutaReader(txtSQL, 2);

            codProyecto = HttpContext.Current.Session["codProyecto"].ToString();
            codConvocatoria = HttpContext.Current.Session["codConvocatoria"].ToString();
            //Actualizar fecha modificación del tab.
            prActualizarTabEval(txtTab.ToString(), codProyecto.ToString(), codConvocatoria.ToString());
            ObtenerDatosUltimaActualizacion();
            HttpContext.Current.Session["test_data"] = "Riesgos";
            ScriptManager.RegisterStartupScript(this, typeof(string), "script", "<script type=text/javascript>parent.location.href = parent.location.href;</script>", false);

            #endregion
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
                    // Validar que no guarde espacios en blanco
                    try
                    {
                        con.Open();
                        com.ExecuteReader();
                        codProyecto = HttpContext.Current.Session["codProyecto"].ToString();
                        codConvocatoria = HttpContext.Current.Session["codConvocatoria"].ToString();
                        //Actualizar fecha modificación del tab.
                        prActualizarTabEval(txtTab.ToString(), codProyecto.ToString(), codConvocatoria.ToString());
                        ObtenerDatosUltimaActualizacion();
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
            //ObtenerDatosUltimaActualizacion();
            //ClientScriptManager cm = this.ClientScript;
            //cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>window.opener.location=window.opener.location;</script>");
            //return;
            ////Nada, toca ver cómo recargar el iframe...
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //var lnk = e.Row.FindControl("LinkButton1") as LinkButton;
                var lnk2 = e.Row.FindControl("lnkeditarRiesgo") as LinkButton;

                //if (txt != null && lnk != null)
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
            Response.Redirect("CatalogoRiesgoMitigacio.aspx");
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
                         " where id_contacto = codcontacto and codtabEvaluacion = " + txtTab.ToString() +
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

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "editar":
                    HttpContext.Current.Session["codProyecto"] = codProyecto;
                    HttpContext.Current.Session["codConvocatoria"] = codConvocatoria;
                    HttpContext.Current.Session["CodRiesgo"] = e.CommandArgument.ToString();
                    HttpContext.Current.Session["Accion"] = "Actualizar";
                    Response.Redirect("CatalogoRiesgoMitigacio.aspx");
                    break;
                default:
                    break;
            }
        }
    }
}