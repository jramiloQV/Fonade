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
using Fonade.Error;

namespace Fonade.FONADE.evaluacion
{
    public partial class EvaluacionNomina : Negocio.Base_Page
    {
        private string codProyecto;
        private string codConvocatoria;
        public bool esMiembro;
        /// <summary>
        /// Indica si está o no "realizado".
        /// </summary>
        public Boolean bRealizado;

        protected void Page_Load(object sender, EventArgs e)
        {
            codProyecto = HttpContext.Current.Session["CodProyecto"].ToString();
            codConvocatoria = HttpContext.Current.Session["CodConvocatoria"].ToString();

            if (!IsPostBack)
            {

                //Consultar si es miembro.
                esMiembro = fnMiembroProyecto(usuario.IdContacto, codProyecto.ToString());

                //Consultar si está "realizado".
                bRealizado = esRealizado(Constantes.CONST_subNomina,Int32.Parse(codProyecto), Int32.Parse(codConvocatoria));

                if (esMiembro && !bRealizado) { this.div_Post_It1.Visible = true; Post_It1._mostrarPost = true; }

                CargarGridPersonalCalificado();
                CargarGridManodeObraDirecta();
                ObtenerDatosUltimaActualizacion();
            }
            prActualizarTabEval(Constantes.CONST_subNomina.ToString(), codProyecto, codConvocatoria);
            
        }

        protected void CargarGridPersonalCalificado()
        {

            var query = (from p in consultas.Db.ProyectoGastosPersonals
                         where p.CodProyecto == Convert.ToInt32(codProyecto)
                         orderby p.Id_Cargo ascending
                         select new { p.Id_Cargo, p.Cargo });
            if (query.Count() != 0)
            {

                var query2 = (from p1 in consultas.Db.MD_MostrarEvalNominaPersonal(Convert.ToInt32(codProyecto))
                              select new
                              {
                                  p1,
                              });

                DataTable datos = new DataTable();
                DataTable detalle = new DataTable();
                datos.Columns.Add("CodProyecto");
                datos.Columns.Add("Id_Cargo");
                datos.Columns.Add("Cargo");
                for (int i = 0; i <= 12; i++)
                {
                    detalle.Columns.Add("Sueldo" + i);
                    detalle.Columns.Add("Prestaciones" + i);
                }
                detalle.Columns.Add("SueldoTotal");
                detalle.Columns.Add("PrestacionesTotal");

                foreach (var cargo in query)
                {
                    DataRow dr = datos.NewRow();

                    dr["CodProyecto"] = codProyecto;
                    dr["Id_Cargo"] = cargo.Id_Cargo;
                    dr["Cargo"] = cargo.Cargo;
                    datos.Rows.Add(dr);
                }

                foreach (var consulta in query2)
                {
                    DataRow drDet = detalle.NewRow();

                    //Si algo, quitarle el "+1" ya que ssalía error de NullReferenceException "porque NO HAY la columna (sueldo0)".
                    for (int i = Convert.ToInt32(consulta.p1.GeneradoPrimerAno); i <= 12; i++)
                    //for (int i = 1; i <= 12; i++)
                    {
                        drDet["Sueldo" + i] = "$" + consulta.p1.valormensual.ToString("0,0.00", CultureInfo.InvariantCulture);
                        drDet["Prestaciones" + i] = "$" + ((decimal)consulta.p1.prestaciones).ToString("0,0.00", CultureInfo.InvariantCulture);
                    }
                    drDet["SueldoTotal"] = "$" + (consulta.p1.valormensual * (12 - (Convert.ToInt32(consulta.p1.GeneradoPrimerAno) - 1))).ToString("0,0.00", CultureInfo.InvariantCulture);
                    drDet["PrestacionesTotal"] = "$" + (((decimal)consulta.p1.prestaciones) * (12 - (Convert.ToInt32(consulta.p1.GeneradoPrimerAno) - 1))).ToString("0,0.00", CultureInfo.InvariantCulture);
                    detalle.Rows.Add(drDet);
                }

                gw_Anexos.DataSource = datos;
                gw_Anexos.DataBind();

                gw_AnexosActividad.DataSource = detalle;
                gw_AnexosActividad.DataBind();

                PanelPersonalCalificado.Visible = true;
            }

        }

        protected void CargarGridManodeObraDirecta()
        {
            try
            {
                var query = (from p in consultas.Db.ProyectoInsumos
                             where p.CodProyecto == Convert.ToInt32(codProyecto)
                             && p.codTipoInsumo == 2
                             orderby p.Id_Insumo ascending
                             select new { p.Id_Insumo, p.nomInsumo });
                if (query.Count() != 0)
                {

                    var query2 = (from p1 in consultas.Db.MD_MostrarEvalNominaManoObra(Convert.ToInt32(codProyecto))
                                  select new
                                  {
                                      p1,
                                  });

                    DataTable datos = new DataTable();
                    DataTable detalle = new DataTable();
                    datos.Columns.Add("CodProyecto");
                    datos.Columns.Add("Id_Insumo");
                    datos.Columns.Add("nomInsumo");

                    for (int i = 1; i <= 12; i++)
                    {
                        detalle.Columns.Add("Sueldo" + i);
                        detalle.Columns.Add("Prestaciones" + i);
                    }
                    detalle.Columns.Add("SueldoTotal");
                    detalle.Columns.Add("PrestacionesTotal");

                    foreach (var cargo in query)
                    {
                        DataRow dr = datos.NewRow();

                        dr["CodProyecto"] = codProyecto;
                        dr["Id_Insumo"] = cargo.Id_Insumo;
                        dr["nomInsumo"] = cargo.nomInsumo;
                        datos.Rows.Add(dr);
                    }

                    foreach (var consulta in query2)
                    {
                        DataRow drDet = detalle.NewRow();

                        for (int i = Convert.ToInt32(consulta.p1.GeneradoPrimerAno); i <= 12; i++)
                        {
                            drDet["Sueldo" + i] = "$" + consulta.p1.sueldomes.ToString("0,0.00", CultureInfo.InvariantCulture);
                        }
                        drDet["SueldoTotal"] = "$" + (consulta.p1.sueldomes * (12 - (Convert.ToInt32(consulta.p1.GeneradoPrimerAno) - 1))).ToString("0,0.00", CultureInfo.InvariantCulture);
                        drDet["PrestacionesTotal"] = "$" + 0.ToString("0,0.00", CultureInfo.InvariantCulture);
                        detalle.Rows.Add(drDet);
                    }

                    gw_Anexos2.DataSource = datos;
                    gw_Anexos2.DataBind();

                    gw_AnexosActividad2.DataSource = detalle;
                    gw_AnexosActividad2.DataBind();
                    PanelManodeObraDirecta.Visible = true;
                }
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
            bool bRealizado = false;
            bool EsMiembro = false;
            bool bEnActa = false;
            Int32 numPostIt = 0;
            Int32 CodigoEstado = 0;

            try
            {
                //Consultar si está en acta...
                bEnActa = es_EnActa(codProyecto.ToString(), codConvocatoria.ToString());

                //Consultar si es miembro.
                EsMiembro = fnMiembroProyecto(usuario.IdContacto, codProyecto);

                //Obtener número "numPostIt".
                numPostIt = Obtener_numPostIt();

                //Consultar el "Estado" del proyecto.
                CodigoEstado = CodEstado_Proyecto(Constantes.CONST_subNomina.ToString(), codProyecto, codConvocatoria);

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
                var usuActualizo = consultas.RetornarInformacionActualizaPagina(int.Parse(codProyecto), int.Parse(codConvocatoria), Constantes.CONST_subNomina);

                var objList = usuActualizo.ToList();
                if(objList.Count > 0)
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

                string url = Request.Url.ToString();

                string mensaje = ex.Message.ToString();
                string data = ex.Data.ToString();
                string stackTrace = ex.StackTrace.ToString();
                string innerException = ex.InnerException == null ? "" : ex.InnerException.Message.ToString();

                // Log the error
                ErrHandler.WriteError(mensaje, url, data, stackTrace, innerException, usuario.Email, usuario.IdContacto.ToString());

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
            flag = Marcar(Constantes.CONST_subNomina.ToString(), codProyecto, codConvocatoria, chk_realizado.Checked); 
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