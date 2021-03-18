using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using System.Globalization;
using System.Configuration;
using System.IO;
using System.Web.UI.HtmlControls;
using Fonade.Clases;
using System.Data.SqlClient;

namespace Fonade.FONADE.evaluacion
{
    public partial class EvaluacionCentrales : Negocio.Base_Page
    {
        private string codProyecto;
        private string codConvocatoria;
        public Boolean esMiembro;
        /// <summary>
        /// Determina si "está" o "no" realizado...
        /// </summary>
        public Boolean bRealizado;

        protected void Page_Load(object sender, EventArgs e)
        {
            codProyecto = HttpContext.Current.Session["CodProyecto"].ToString();
            codConvocatoria = HttpContext.Current.Session["CodConvocatoria"].ToString();

            //Consultar si es miembro.
            esMiembro = fnMiembroProyecto(usuario.IdContacto, codProyecto);
            //obtener el codigo de convocatoria del proyecto
            //corresponde a la última convocatoria a la que se postuló el proyecto
            var txtSQL = " select codconvocatoria from convocatoriaproyecto where codproyecto = " + codProyecto + " order by codconvocatoria desc ";
            var rs = consultas.ObtenerDataTable(txtSQL, "text");
            var cc = "0";
            if (rs.Rows.Count > 0)
            {
                cc = rs.Rows[0]["codconvocatoria"].ToString();
            }
            //Consultar si está "realizado".
            bRealizado = esRealizado(Constantes.ConstSubCentralesRiesgo, Int32.Parse(codProyecto), Int32.Parse(codConvocatoria));
            
            
            if (!IsPostBack)
            {
                llenarDemasCampos();

                if (habilitarGuardadoEval(codProyecto, codConvocatoria, Constantes.ConstSubCentralesRiesgo, Constantes.CONST_Evaluador) == false)
                {
                    //btnimgcalend4.Visible = false;
                    //txt_observaciones.Enabled = false;
                    //btn_actualizar.Visible = false;
                    //btn_actualizar.Enabled = false;
                }

                ObtenerDatosUltimaActualizacion();
                
            }

            //realizado = bRealizado;

            ////if (esMiembro && !bRealizado)
            //if (!(esMiembro && !bRealizado))
            //{ this.div_Post_It1.Visible = true; }

            //Se agrega control para ocultar post it al gerente evaluador
            //if (usuario.CodGrupo == Constantes.CONST_GerenteEvaluador)
            //{
            //    this.div_Post_It1.Visible = false;
            //}

            if (esMiembro && !bRealizado)
            { this.div_Post_It1.Visible = true; Post_It1._mostrarPost = true; }

            if (esMiembro && !bRealizado && usuario.CodGrupo == Constantes.CONST_Evaluador)
            {
                //CalendarExtender4.Enabled = false;
                txt_observaciones.Visible = true;
                btn_actualizar.Visible = true;
            }
            else
            {
                btn_actualizar.Visible = false;
                CalendarExtender4.Enabled = false;
                txt_observaciones.Enabled = false;
            }

        }

        protected void lds_Integrantes_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            try
            {
                var query = from P in consultas.MostrarIntegrantesCentralesRiesgos(Convert.ToInt32(codProyecto), Constantes.CONST_RolEmprendedor, Convert.ToInt32(codConvocatoria))
                            select P;
                e.Result = query;
            }
            catch (Exception)
            { }
        }

        protected void llenarDemasCampos()
        {
            try
            {
                var query = (from x in consultas.Db.EvaluacionObservacions
                             where x.CodProyecto == Convert.ToInt32(codProyecto)
                             && x.CodConvocatoria == Convert.ToInt32(codConvocatoria)
                             select new
                             {
                                 x.FechaCentralesRiesgo,
                                 x.CentralesRiesgo,
                             }).FirstOrDefault();

                if (query!=null)
                txt_fechareporte.Text = ((DateTime)query.FechaCentralesRiesgo).ToString("dd/MM/yyyy");

                else
                    txt_fechareporte.Text = "" + DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;

                txt_observaciones.Text = query.CentralesRiesgo;
                this.div_observaciones.InnerText = query.CentralesRiesgo;
            }
            catch (Exception)
            { }
        }

        protected void btn_actualizar_Click(object sender, EventArgs e)
        {
            insertarDatos(txt_fechareporte.Text, txt_observaciones.Text);
        }

        protected void insertarDatos(string fecha, string observaciones)
        {
            try
            {
                DateTime fCentralSql = DateTime.ParseExact(fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var fechaCentralSql = fCentralSql.Date.ToString("yyyy-MM-dd HH:mm:ss");

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                SqlCommand cmd = new SqlCommand("MD_InsertUpdateEvaluacionCentrales", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodProyecto", Convert.ToInt32(codProyecto));
                cmd.Parameters.AddWithValue("@CodConvocatoria", Convert.ToInt32(codConvocatoria));
                cmd.Parameters.AddWithValue("@CentralRiesgo", observaciones);
                cmd.Parameters.AddWithValue("@FechaCentral", fechaCentralSql);
                SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
                con.Open();
                cmd2.ExecuteNonQuery();
                cmd.ExecuteNonQuery();
                con.Close();
                con.Dispose();
                cmd.Dispose();
                cmd2.Dispose();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Actualizado satisfactoriamente!');", true);
                prActualizarTabEval(Constantes.ConstSubCentralesRiesgo.ToString(), codProyecto, codConvocatoria);
                ObtenerDatosUltimaActualizacion();
            }
            catch (Exception)
            { }
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
                CodigoEstado = CodEstado_Proyecto(Constantes.ConstSubCentralesRiesgo.ToString(), codProyecto, codConvocatoria);

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
                         " where id_contacto = codcontacto and codtabEvaluacion = " + Constantes.ConstSubCentralesRiesgo +
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
            flag = Marcar(Constantes.ConstSubCentralesRiesgo.ToString(), codProyecto, codConvocatoria, chk_realizado.Checked);
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
    }
}