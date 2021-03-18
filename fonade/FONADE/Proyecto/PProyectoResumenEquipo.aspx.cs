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

namespace Fonade.FONADE.Proyecto
{
    public partial class PProyectoResumenEquipo : Negocio.Base_Page
    {
        public string codigo;
        public string codConvocatoria;
        public int estadoProy;
        public int txtTab = Constantes.CONST_EquipoTrabajo;

        public bool vldt { get { if (usuario.CodGrupo == Constantes.CONST_Evaluador) { return false; } else { return new Clases.genericQueries().ValidateUserCode(usuario.IdContacto, HttpContext.Current.Session["CodProyecto"]); } } }

        public bool ejecucion
        {
            get
            {
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador) { return (usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador && !vldt); }
                int codigoProyecto = HttpContext.Current.Session["CodProyecto"] != null ? Convert.ToInt32(HttpContext.Current.Session["CodProyecto"].ToString()) : 0;
                return
                new Clases.genericQueries().getItemsProyectoMercadoProyeccionVentas(codigoProyecto).Count > 0
                && usuario.CodGrupo != Constantes.CONST_Emprendedor && usuario.CodGrupo != Constantes.CONST_Evaluador;
            }
        }

        public bool visibleGuardar { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["CodProyecto"] == null)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "refreshParent", "window.top.location.reload();", true);
            }

            codigo = HttpContext.Current.Session["CodProyecto"] != null ? HttpContext.Current.Session["CodProyecto"].ToString() : "0";
            codConvocatoria = HttpContext.Current.Session["CodConvocatoria"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodConvocatoria"].ToString()) ? HttpContext.Current.Session["codConvocatoria"].ToString() : "0";
            estadoProy = estadoProyecto(Int32.Parse(codigo));

            if (!IsPostBack)
            {
                ObtenerDatosUltimaActualizacion();
                llenarInfo();

                if(usuario.CodGrupo == Constantes.CONST_Emprendedor)
                {
                    if(chk_realizado.Checked)
                    {
                        chk_realizado.Enabled = false;
                    }
                    else
                    {
                        tabla_docs.Visible = true;
                    }
                }
            }
        }

        protected void llenarInfo()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("MD_Mostrar_resumen_equipo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RolEmprendedor", Constantes.CONST_RolEmprendedor);
                cmd.Parameters.AddWithValue("@RolAsesor", Constantes.CONST_RolAsesor);
                cmd.Parameters.AddWithValue("@RolAsesorLider", Constantes.CONST_RolAsesorLider);
                cmd.Parameters.AddWithValue("@CodigoProyecto", codigo);
                cmd.Parameters.AddWithValue("@caso", "Info");
                con.Open();
                SqlDataReader r = cmd.ExecuteReader();
                r.Read();
                l_info1.Text = Convert.ToString(r["nomproyecto"]) + " - " + Convert.ToString(r["nomunidad"]) + " (" + Convert.ToString(r["nominstitucion"]) + ")";
                l_info2.Text = Convert.ToString(r["nomsubsector"]);
                l_info3.Text = Convert.ToString(r["nomciudad"]) + " (" + Convert.ToString(r["nomdepartamento"]) + ")";
                lbl_sumario.Text = r["sumario"].ToString();

                if (r["recursos"] != null)
                {
                    Panel_recursos.Visible = true;
                    l_recursos.Text = Convert.ToString(r["recursos"]) + " SMLV";
                }

                //con.Close();
                //con.Dispose();
                cmd.Dispose();
            }
            finally {
                con.Close();
                con.Dispose();
            }
        }

        protected void lds_equipo_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            try
            {
                var query = from P in consultas.Db.MD_Mostrar_resumen_equipo(Constantes.CONST_RolEmprendedor, Constantes.CONST_RolAsesor, Constantes.CONST_RolAsesorLider, Int32.Parse(codigo), "Equipo")
                            select P;
                e.Result = query;
            }
            catch (Exception)
            { }
        }

        protected void gv_equipotrabajo_DataBound(object sender, EventArgs e)
        {

            //var indexRow = [e.Row.RowIndex][].ToString();
            //var indexRow = DataBinder.Eval(e.Row.DataItem, "lrol");
            foreach (GridViewRow grd_Row in this.gv_equipotrabajo.Rows)
            {
                string ruta = "javascript:void(window.open('../../FONADE/MiPerfil/VerPerfilContacto.aspx?LoadCode=" + ((HiddenField)grd_Row.FindControl("hiddenID")).Value + "','_blank','width=600,height=580,toolbar=no, scrollbars=1, resizable=no'));";
                ((HyperLink)grd_Row.FindControl("lnombres")).NavigateUrl = ruta;
                if (((HiddenField)grd_Row.FindControl("hiddenID")).Value == usuario.IdContacto.ToString()
                    && usuario.CodGrupo == Constantes.CONST_Emprendedor
                    && estadoProy == Constantes.CONST_Inscripcion)
                {
                    ((Button)grd_Row.FindControl("btn_Guardar")).Visible = true;
                    ((Button)grd_Row.FindControl("btn_Guardar")).Enabled = true;
                    ((TextBox)grd_Row.FindControl("txt_horasDedicadas")).Enabled = true;
                }

                Label rol = ((Label)grd_Row.FindControl("lrol"));
                if (rol.Text.ToString().ToLower().Trim() == "Asesor Lider".ToLower().Trim())
                {
                    ((TextBox)grd_Row.FindControl("txt_horasDedicadas")).Visible = false;
                }
            }
        }

        protected int estadoProyecto(int codigoProyecto)
        {
            int estado = 0;

            var query = (from proyect in consultas.Db.Proyecto1s
                         where proyect.Id_Proyecto == codigoProyecto
                         select new
                         {
                             proyect.CodEstado,
                         }).FirstOrDefault();
            estado = query.CodEstado;

            return estado;
        }

        protected void Guardar_Horas(object sender, EventArgs e)
        {
            foreach (GridViewRow grd_Row in this.gv_equipotrabajo.Rows)
            {
                if (((HiddenField)grd_Row.FindControl("hiddenID")).Value == usuario.IdContacto.ToString())
                {
                    if (((TextBox)grd_Row.FindControl("txt_horasDedicadas")).Text != "")
                    {
                        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                        try
                        {
                            SqlCommand cmd = new SqlCommand("MD_Update_Horas_Al_Proyecto", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@HorasProyecto", Convert.ToInt32(((TextBox)grd_Row.FindControl("txt_horasDedicadas")).Text));
                            cmd.Parameters.AddWithValue("@CodProyecto", codigo);
                            cmd.Parameters.AddWithValue("@CodContacto", usuario.IdContacto);
                            cmd.Parameters.AddWithValue("@Codrol", Convert.ToInt16(Constantes.CONST_RolEmprendedor));
                            SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
                            con.Open();
                            cmd2.ExecuteNonQuery();
                            cmd.ExecuteNonQuery();
                            //con.Close();
                            //con.Dispose();
                            cmd2.Dispose();
                            cmd.Dispose();
                            //Actualizar fecha modificación del tab.
                            prActualizarTab(txtTab.ToString(), codigo);
                            ObtenerDatosUltimaActualizacion();
                            Response.Redirect(Request.RawUrl);
                            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Guardado exitosamente!')", true);
                            Response.Redirect(Request.RawUrl);
                        }
                        finally {
                            con.Close();
                            con.Dispose();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe ingresar el número de horas que le dedicará diariamente al proyecto!')", true);
                    }
                }

            }
        }

        #region Métodos de .

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
            bool bRealizado = false;
            bool EsMiembro = false;
            Int32 numPostIt = 0;
            Int32 CodigoEstado = 0;

            try
            {
                //Consultar si es miembro.
                EsMiembro = fnMiembroProyecto(usuario.IdContacto, codigo.ToString());

                //Obtener número "numPostIt".
                numPostIt = Obtener_numPostIt();

                //Consultar el "Estado" del proyecto.
                CodigoEstado = CodEstado_Proyecto(txtTab.ToString(), codigo, codConvocatoria);

                #region Obtener el rol.

                //Consulta.
                txtSQL = " SELECT CodContacto, CodRol From ProyectoContacto " +
                         " Where CodProyecto = " + codigo + " And CodContacto = " + usuario.IdContacto +
                         " and inactivo=0 and FechaInicio<=getdate() and FechaFin is null ";

                //Asignar variables a DataTable.
                var rs = consultas.ObtenerDataTable(txtSQL, "text");

                //Crear la variable de sesión.
                if (rs.Rows.Count > 0) { HttpContext.Current.Session["CodRol"] = rs.Rows[0]["CodRol"].ToString(); }
                else { HttpContext.Current.Session["CodRol"] = ""; }

                //Destruir la variable.
                rs = null;

                #endregion

                //Consultar los datos a mostrar en los campos correspondientes a la actualización.
                var usuActualizo = consultas.RetornarInformacionActualizaPPagina(int.Parse(codigo), txtTab);

                if (usuActualizo != null)
                {
                    lbl_nombre_user_ult_act.Text = usuActualizo.SingleOrDefault().nombres.ToUpper();

                    //Convertir fecha.
                    try { fecha = Convert.ToDateTime(usuActualizo.SingleOrDefault().fecha); }
                    catch { fecha = DateTime.Today; }
                    //Obtener el nombre del mes (las primeras tres letras).
                    string sMes = fecha.ToString("MMM", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                    //Obtener la hora en minúscula.
                    string hora = fecha.ToString("hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture).ToLowerInvariant();
                    //Reemplazar el valor "am" o "pm" por "a.m" o "p.m" respectivamente.
                    if (hora.Contains("am")) { hora = hora.Replace("am", "a.m"); } if (hora.Contains("pm")) { hora = hora.Replace("pm", "p.m"); }
                    //Formatear la fecha según manejo de FONADE clásico. "Ej: Nov 19 de 2013 07:36:26 p.m.".
                    lbl_fecha_formateada.Text = UppercaseFirst(sMes) + " " + fecha.Day + " de " + fecha.Year + " " + hora + ".";

                    //Realizado
                    bRealizado = usuActualizo.SingleOrDefault().realizado;
                }
                

                //Asignar check de acuerdo al valor obtenido en "bRealizado".
                chk_realizado.Checked = bRealizado;

                //Determinar si el usuario actual puede o no "chequear" la actualización.
                //if (!(EsMiembro && numPostIt == 0 && ((usuario.CodGrupo == Constantes.CONST_RolAsesorLider && CodigoEstado == Constantes.CONST_Inscripcion) || (CodigoEstado == Constantes.CONST_Evaluacion && usuario.CodGrupo == Constantes.CONST_RolEvaluador && es_bNuevo(codProyecto)))) || lbl_nombre_user_ult_act.Text.Trim() == "")
                if (!(EsMiembro && numPostIt == 0 && ((HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolAsesorLider.ToString() && CodigoEstado == Constantes.CONST_Inscripcion) || (CodigoEstado == Constantes.CONST_Evaluacion && HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolEvaluador.ToString() && es_bNuevo(codigo)))) || lbl_nombre_user_ult_act.Text.Trim() == "")
                { chk_realizado.Enabled = false; }

                //Mostrar el botón de guardar.
                //if (EsMiembro && numPostIt == 0 && lbl_nombre_user_ult_act.Text != "" && (usuario.CodGrupo == Constantes.CONST_RolAsesorLider && CodigoEstado == Constantes.CONST_Inscripcion) || (usuario.CodGrupo == Constantes.CONST_RolEvaluador && CodigoEstado == Constantes.CONST_Evaluacion && es_bNuevo(codProyecto)))
                if (EsMiembro && numPostIt == 0 && lbl_nombre_user_ult_act.Text != "" && (HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolAsesorLider.ToString() && CodigoEstado == Constantes.CONST_Inscripcion) || (HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolEvaluador.ToString() && CodigoEstado == Constantes.CONST_Evaluacion && es_bNuevo(codigo)))
                {
                    if (usuario.CodGrupo == Constantes.CONST_Evaluador)
                    {
                        visibleGuardar = false;
                    }
                    else
                    {
                        visibleGuardar = true;
                    }
                }

                //Mostrar los enlaces para adjuntar documentos.
                if (EsMiembro && HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolEmprendedor.ToString() && !bRealizado)
                {
                    tabla_docs.Visible = true;
                }
                visibleGuardar = visibleGuardar || Constantes.CONST_CoordinadorEvaluador == usuario.CodGrupo;
                //Destruir variables.
                tabla = null;
                txtSQL = null;
            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error: " + ex.Message + ".')", true);
                //Destruir variables.
                //tabla = null;
                //txtSQL = null;
                //return;
            }
        }

        /// <summary>
        
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
                        && tu.CodProyecto == Int32.Parse(codigo)
                        && tp.Id_TareaPrograma == Constantes.CONST_PostIt
                        && tur.FechaCierre == null
                        select tur;

            numPosIt = query.Count();

            return numPosIt;
        }

        /// <summary>
        
        /// 24/06/2014.
        /// Guardar la información "Ultima Actualización".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_guardar_ultima_actualizacion_Click(object sender, EventArgs e)
        {
            prActualizarTab(txtTab.ToString(), codigo.ToString()); 
            Marcar(txtTab.ToString(), codigo, "", chk_realizado.Checked); 
            ObtenerDatosUltimaActualizacion();
            Response.Redirect(Request.RawUrl);
            #region NO BORRAR

            /*
                {
                    //NO BORRAR!
                    //prActualizarTabEval(txtTab.ToString(), codigo.ToString(), codConvocatoria.ToString()); ObtenerDatosUltimaActualizacion();

                    //Inicializar variables.
                    String txtSQL;
                    SqlCommand cmd = new SqlCommand();
                    DataTable rs = new DataTable();
                    Int32 numTabsEval = 0;

                    if (codConvocatoria == "")
                    { txtSQL = " update tabproyecto "; }
                    else { txtSQL = " update tabEvaluacionproyecto "; }

                    if (chk_realizado.Checked) { txtSQL = txtSQL + "set realizado =1 "; }
                    else { txtSQL = txtSQL + "set realizado = 0 "; }

                    if (codConvocatoria == "") { txtSQL = txtSQL + " where codtab = " + txtTab + " and codproyecto = " + codigo; }
                    else { txtSQL = txtSQL + " where codtabEvaluacion = " + txtTab + " and codConvocatoria = " + codConvocatoria + " and codproyecto = " + codigo; }

                    try
                    {
                        //NEW RESULTS:
                        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                        cmd = new SqlCommand(txtSQL, con);

                        if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        con.Close();
                        con.Dispose();
                        cmd.Dispose();
                    }
                    catch { }

                    //Actualizar el tab padre
                    prActualizarTabPadre(txtTab.ToString(), codigo, ""); //NO tiene mucho sentido, el método usa el CodProyecto.

                    //Si el coordinador aprueba una evaluación el emprendedor no debe poder realizar ningún cambio
                    if (codConvocatoria != "")
                    {
                        //Calcular número de tabs de evaluación
                        rs = consultas.ObtenerDataTable("SELECT isnull(COUNT(0),0) as Total FROM TabEvaluacion WHERE codTabEvaluacion is NULL", "text");

                        if (rs.Rows.Count > 0)
                        {
                            try { numTabsEval = Int32.Parse(rs.Rows[0]["Total"].ToString()); }
                            catch { numTabsEval = 0; }

                            numTabsEval = numTabsEval - 2; //No tomar en cuenta los tab informes y desempeño de evaluador-

                            //Calcular cuantos tabs estan aprobados
                            txtSQL = " select count(tep.codtabevaluacion) AS ConteoDatos from tabevaluacionproyecto tep,tabevaluacion te " +
                                     " where id_tabevaluacion=tep.codtabevaluacion  and realizado=1 and te.codtabevaluacion is null " +
                                     " and codproyecto = " + codigo + " and codconvocatoria = " + codConvocatoria;

                            rs = consultas.ObtenerDataTable(txtSQL, "text");

                            if (rs.Rows.Count > 0)
                            {
                                if (Int32.Parse(rs.Rows[0]["ConteoDatos"].ToString()) == numTabsEval)
                                {
                                    //Si todos los tabs se encuentran aprobados el emprendedor no debe poder realizar cambios.
                                    txtSQL = "Update tabproyecto Set realizado=1 where codproyecto = " + codigo;

                                    try
                                    {
                                        //NEW RESULTS:
                                        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                                        cmd = new SqlCommand(txtSQL, con);

                                        if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                                        cmd.CommandType = CommandType.Text;
                                        cmd.ExecuteNonQuery();
                                        con.Close();
                                        con.Dispose();
                                        cmd.Dispose();
                                    }
                                    catch { }

                                }
                            }
                        }

                        rs = null;
                    }
                }*/

            #endregion
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Session["TabInvoca"] = "ResumenEquipo";
            HttpContext.Current.Session["CodProyecto"] = codigo;
            HttpContext.Current.Session["txtTab"] = txtTab.ToString();
            HttpContext.Current.Session["Accion"] = "Nuevo";
            Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            Session["TabInvoca"] = "ResumenEquipo";
            HttpContext.Current.Session["CodProyecto"] = codigo;
            HttpContext.Current.Session["txtTab"] = txtTab.ToString();
            HttpContext.Current.Session["Accion"] = "Vista";
            Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        #endregion
    }
}