using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Fonade.Account;
using LinqKit;
using AjaxControlToolkit;
using System.ComponentModel;
using System.Text;

namespace Fonade.FONADE.evaluacion
{
    public partial class EvaluacionIndicadoresFinancieros : Negocio.Base_Page
    {
        //Cadena de conexión

        public String codProyecto;
        public int txtTab = Constantes.ConstSubIndicadoresFinancieros;
        public String codConvocatoria;
        private ProyectoMercadoProyeccionVenta pm;
        private string conexionStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public Boolean esMiembro;
        /// <summary>
        /// Determina si "está" o "no" realizado...
        /// </summary>
        public Boolean bRealizado;
        public bool rel = false;

        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                codProyecto = HttpContext.Current.Session["codProyecto"].ToString();
                codConvocatoria = HttpContext.Current.Session["codConvocatoria"].ToString();

                //codProyecto = Request.QueryString["codProyecto"].ToString();
                HttpContext.Current.Session["codProyectoval"] = codProyecto;
            }
            catch (Exception) { }

            //Consultar si es miembro.
            esMiembro = fnMiembroProyecto(usuario.IdContacto, codProyecto);

            //Consultar si está "realizado".
            bRealizado = esRealizado(txtTab, Int32.Parse(codProyecto), Int32.Parse(codConvocatoria));

            if (!IsPostBack)
            {
                gv_evaluacionindicadores.DataSource = sp_EvaluacionProyectoIndicador_SelectAll(Int32.Parse(codProyecto), Int32.Parse(codConvocatoria));
                gv_evaluacionindicadores.DataBind();

                //CtrlCheckedProyecto1.tabCod = Constantes.ConstSubIndicadoresFinancieros;
                //CtrlCheckedProyecto1.CodProyecto = codProyecto;
                ObtenerDatosUltimaActualizacion();

                //Limpiar variables de sesión.
                HttpContext.Current.Session["CodAporte"] = null;
                HttpContext.Current.Session["AccionAporteEvaluacion"] = null;
            }
            if (esMiembro && !bRealizado)
            { this.div_Post_It1.Visible = true; Post_It1._mostrarPost = true; }

            if (usuario.CodGrupo == Constantes.CONST_Evaluador && !bRealizado)//!chk_realizado.Checked)
            {
                ImageB.Visible = true;
                LB_InsertarIndicadores.Visible = true;
            }
            if (bRealizado == true)
            {
                rel = true;
            }            
        }

        /// <summary>
        /// RowCommand.
        /// FALTA CREAR EL MÉTODO PARA ELIMINAR EL INDICADOR SELECCIONADO.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_evaluacionindicadores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Modificar")
            {
                string[] idCapital = e.CommandArgument.ToString().Split(';');

                HttpContext.Current.Session["CodAporte"] = idCapital[0];
                HttpContext.Current.Session["AccionAporteEvaluacion"] = "Modificar";
                HttpContext.Current.Session["Descripcion_Loaded"] = idCapital[1];
                Response.Redirect("CatalogoIndicador.aspx");
                //CargarGridCapitalTrabajo();
            }
            if (e.CommandName == "eliminar")
            {
                string[] idCapital = e.CommandArgument.ToString().Split(';');

                //Eliminar el indicador financiero seleccionado.
                EliminarIndicadorFinanciero(idCapital[0]);
            }
        }

        /// <summary>
        /// Consultar  Indicador x id
        /// </summary>
        /// <param name="codProyecto">Código del proyecto.</param>
        /// <param name="codConvocatoria">Código de la convocatoria.</param>
        /// <returns>DataTable</returns>
        public DataTable sp_EvaluacionProyectoIndicador_SelectAll(int codProyecto, int codConvocatoria)
        {
            DataTable dt = new DataTable();
            //String txtSQL = "";

            #region Comentarios (se tuvo que cambiar la consulta, desde el inicio no se habían colocado los campos).
            //consultas.Parameters = new[] { new SqlParameter
            //                                       { 
            //                                            ParameterName = "@codProyecto",
            //                                            Value = codProyecto
            //                                       },
            //                                new SqlParameter
            //                                        {
            //                                            ParameterName = "@codConvocatoria",
            //                                            Value = codConvocatoria
            //                                        }
            //};

            //dt = consultas.ObtenerDataTable("sp_EvaluacionProyectoIndicador_SelectAll"); 
            #endregion

            StringBuilder querySqlIndicadoresFinancieros = new StringBuilder();

            querySqlIndicadoresFinancieros.AppendFormat("SELECT ind.id_indicador,ind.Descripcion,ind.Tipo,ind.Valor,ind.Protegido FROM EvaluacionProyectoIndicador ind inner join ( select distinct Descripcion, min(id_Indicador) as id from EvaluacionProyectoIndicador WHERE codProyecto = {0} and codConvocatoria = {1}  group by Descripcion ) as ind2 on ind.Descripcion = ind2.Descripcion and ind.id_Indicador = ind2.id WHERE ind.codProyecto = {0} and codConvocatoria = {1}", codProyecto, codConvocatoria);

            dt = consultas.ObtenerDataTable(querySqlIndicadoresFinancieros.ToString(), "text");

            dt.Columns.Add("Valor1");

            #region Versión anterior funcional, pero SIN formato según FONADE clásico.
            //foreach (DataRow fila in dt.Rows)
            //{
            //    switch (fila["Tipo"].ToString())
            //    {
            //        case "$":
            //            fila["Valor1"] = "$ " + fila["Valor"].ToString();
            //            break;
            //        case "%":
            //            fila["Valor1"] = fila["Valor"].ToString() + " %";
            //            break;
            //        case "#":
            //            fila["Valor1"] = fila["Valor"].ToString();
            //            break;
            //    }
            //} 
            #endregion

            #region Versión de Mauricio Arias Olave CON formato según FONADE clásico.

            Double valor = 0;
            String convertido = "";

            foreach (DataRow fila in dt.Rows)
            {
                switch (fila["Tipo"].ToString())
                {
                    case "$":
                        #region FUNCIONAL.
                        valor = Convert.ToDouble(fila["Valor"].ToString());
                        convertido = valor.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                        if (convertido.Contains("(") && convertido.Contains(")"))
                        {
                            convertido = convertido.Replace("(", "");
                            convertido = convertido.Replace(")", "");

                            if (convertido.Contains("$"))
                            {
                                if (valor.ToString().Contains("-"))
                                {
                                    convertido = convertido.Replace("$", "<b>$ </b>-");
                                }
                                else
                                {
                                    convertido = convertido.Replace("$", "<b>$ </b>");
                                }
                            }
                        }
                        fila["Valor1"] = convertido;
                        #endregion

                        #region PRUEBA (1.1).
                        //valor = Convert.ToDouble(fila["Valor"].ToString());
                        //System.Globalization.CultureInfo ni = System.Globalization.CultureInfo.CreateSpecificCulture("es-CO");
                        //ni.NumberFormat.CurrencyPositivePattern = 2;
                        //convertido = valor.ToString("C2", ni);
                        //if (convertido.Contains("(") && convertido.Contains(")"))
                        //{
                        //    convertido = convertido.Replace("(", "");
                        //    convertido = convertido.Replace(")", "");

                        //    if (convertido.Contains("$"))
                        //    {
                        //        if (valor.ToString().Contains("-"))
                        //        {
                        //            convertido = convertido.Replace("$", "<b>$ </b>-");
                        //        }
                        //        else
                        //        {
                        //            convertido = convertido.Replace("$", "<b>$ </b>");
                        //        }
                        //    }
                        //} 
                        #endregion

                        #region PRUEBA (2).
                        //valor = Convert.ToDouble(fila["Valor"].ToString());
                        //System.Globalization.CultureInfo modCulture = new System.Globalization.CultureInfo("es-CO");
                        //modCulture = System.Globalization.CultureInfo.CreateSpecificCulture("es-CO");
                        //System.Globalization.NumberFormatInfo ni = modCulture.NumberFormat;
                        //ni.CurrencyPositivePattern = 1;
                        //ni.CurrencySymbol = "$";
                        //convertido = valor.ToString(String.Format("C2", modCulture)); 
                        #endregion
                        break;
                    case "%":
                        valor = Convert.ToDouble(fila["Valor"].ToString());
                        convertido = String.Format("{0:0.00}", valor);
                        fila["Valor1"] = convertido + " <b>%</b>";
                        break;
                    case "#":
                        valor = Convert.ToDouble(fila["Valor"].ToString());
                        convertido = String.Format("{0:0.00}", valor);
                        fila["Valor1"] = convertido;
                        break;

                    #region Comentarios anteriores "no estaban formateados los datos".
                    //case "$":
                    //    fila["Valor1"] = "$ " + fila["Valor"].ToString();
                    //    break;
                    //case "%":
                    //    fila["Valor1"] = fila["Valor"].ToString() + " %";
                    //    break;
                    //case "#":
                    //    Double valor = Convert.ToDouble(fila["Valor"].ToString());
                    //    //fila["Valor1"] = valor.ToString("C2", System.Globalization.CultureInfo.InvariantCulture);
                    //    //fila["Valor1"] = valor.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture);
                    //    //string valor_pasado = valor.ToString("0,0.00", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                    //    //valor_pasado = valor_pasado.Replace("$","");
                    //    //fila["Valor1"] = valor_pasado;
                    //    break; 
                    #endregion
                }
            }

            #endregion

            return dt;

            #region Comentarios anteriores a las modificaciones del código fuente.
            //using (var con = new SqlConnection(conexionStr))
            //{
            //    using (var com = con.CreateCommand())
            //    {
            //        com.CommandText = "sp_EvaluacionProyectoIndicador_SelectAll";
            //        com.CommandType = System.Data.CommandType.StoredProcedure;
            //        com.Parameters.AddWithValue("@codProyecto", codProyecto);
            //        com.Parameters.AddWithValue("@codConvocatoria", codConvocatoria);
            //        // Validar que no guarde espacios en blanco
            //        try
            //        {


            //        }
            //        catch (Exception ex)
            //        {
            //            throw new Exception(ex.Message);
            //        }
            //    } // using comando
            //} // using conexion 
            #endregion
        }

        /// <summary>
        /// Redirecciona a "CatalogoAporteEvaluacion.aspx" para crear el indicador.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_agregar_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["CodAporte"] = 0;
            HttpContext.Current.Session["AccionAporteEvaluacion"] = "Crear";
            Response.Redirect("CatalogoAporteEvaluacion.aspx");
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
                CodigoEstado = CodEstado_Proyecto(Constantes.ConstSubIndicadoresFinancieros.ToString(), codProyecto, codConvocatoria);

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
                         " where id_contacto = codcontacto and codtabEvaluacion = " + Constantes.ConstSubIndicadoresFinancieros +
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

                ////Evaluar "habilitación" del CheckBox.
                //if (!(EsMiembro && HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolCoordinadorEvaluador.ToString()) || lbl_nombre_user_ult_act.Text.Trim() == "" || CodigoEstado != Constantes.CONST_Evaluacion || bEnActa)
                //{ chk_realizado.Enabled = false; }

                ////if (EsMiembro && HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolCoordinadorEvaluador.ToString() && lbl_nombre_user_ult_act.Text.Trim() != "" && CodigoEstado == Constantes.CONST_Evaluacion && (!bEnActa))
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
            flag = Marcar(Constantes.ConstSubIndicadoresFinancieros.ToString(), codProyecto, codConvocatoria, chk_realizado.Checked); 
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

        /// <summary>
        /// RowDataBound.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_evaluacionindicadores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Controles de modificar indicador seleccionado.
                var lnk = e.Row.FindControl("lnk_btn_descripcion") as LinkButton;
                var hdf = e.Row.FindControl("oculto") as HiddenField;

                //Controles de eliminaciòn del indicador.
                var lnk_2 = e.Row.FindControl("lnkeliminar") as LinkButton;
                var img = e.Row.FindControl("imgeditar") as Image;

                bool estaProtegido = false;

                if (lnk_2 != null && img != null)
                {
                    estaProtegido = Boolean.Parse(hdf.Value);

                    if (!estaProtegido && usuario.CodGrupo == Constantes.CONST_Evaluador && !rel)
                    {
                        //Mostrar botones de eliminación.
                        lnk_2.Visible = true;
                        img.Visible = true;
                    }
                }

                if (lnk != null && hdf != null)
                {
                    //Habilitar el LinkButton para editar.
                    if (usuario.CodGrupo == Constantes.CONST_Evaluador && !rel)
                    { lnk.Enabled = true; }
                }

                if (bRealizado && usuario.CodGrupo == Constantes.CONST_Evaluador)
                {
                    img.Visible = false;
                    lnk_2.Visible = false;
                    lnk.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Adicionar Indicador financiero.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ImageB_Click(object sender, ImageClickEventArgs e)
        {
            string a = "";
            HttpContext.Current.Session["CodAporte"] = 0;
            HttpContext.Current.Session["AccionAporteEvaluacion"] = "Crear";
            Response.Redirect("CatalogoIndicador.aspx");
        }

        /// <summary>
        /// Adicionar Indicador financiero.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LB_InsertarIndicadores_Click(object sender, EventArgs e)
        {
            string a = "";
            HttpContext.Current.Session["CodAporte"] = 0;
            HttpContext.Current.Session["AccionAporteEvaluacion"] = "Crear";
            Response.Redirect("CatalogoIndicador.aspx");
        }

        /// <summary>
        /// Eliminar el indicador seleccionado.
        /// </summary>
        /// <param name="CodIndicador"></param>
        private void EliminarIndicadorFinanciero(String CodIndicador)
        {
            //Inicializar variables.
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            String txtSQL;
            String bRepetido = "No se pudo eliminar el indicador financiero seleccionado.";

            try
            {
                //Borrar la inversión.
                txtSQL = " Delete from EvaluacionProyectoIndicador where protegido = 0 and Id_Indicador = " + CodIndicador;
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                try
                {
                    //NEW RESULTS:
                    
                    cmd = new SqlCommand(txtSQL, con);

                    if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    
                    cmd.Dispose();

                    //Actualizar fecha modificación del tab.
                    prActualizarTabEval(Constantes.ConstSubIndicadoresFinancieros.ToString(), codProyecto, codConvocatoria); ObtenerDatosUltimaActualizacion();

                    //Recargar la grilla.
                    gv_evaluacionindicadores.DataSource = sp_EvaluacionProyectoIndicador_SelectAll(Int32.Parse(codProyecto), Int32.Parse(codConvocatoria));
                    gv_evaluacionindicadores.DataBind();
                }
                catch
                {

                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + bRepetido + "')", true);
                    return;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

            }
            catch
            {
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + bRepetido + "')", true);
                return;
            }
        }
    }
}