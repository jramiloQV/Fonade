using System;
using System.Linq;
using System.Web.Services;
using Datos;
using Fonade.Negocio;
using Newtonsoft.Json;
using System.Data;
using System.Web.UI;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using Fonade.Clases;

namespace Fonade.FONADE.evaluacion
{
    public partial class EvaluacionObservaciones : Base_Page
    {        
        public int CodigoConvocatoria { get; set; }
        public int CodigoProyecto { get; set; }        
        public Boolean EsMiembro { get; set; }
        public Boolean EsRealizado { get; set; }
        public int CodigoTab { get; set; }
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
        public string txtSQL;
                
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["CodProyecto"] == null)
                    throw new ApplicationException("No se pudo obtener el codigo del proyecto, intentenlo de nuevo.");
                if (Session["CodConvocatoria"] == null)
                    throw new ApplicationException("No se pudo obtener el codigo de la convocatoria, intentenlo de nuevo.");
                if (usuario == null)
                    throw new ApplicationException("No se pudo obtener la información del usuario, intentenlo de nuevo.");
                
                CodigoTab = Constantes.ConstSubObservaciones;
                CodigoProyecto = Convert.ToInt32(HttpContext.Current.Session["CodProyecto"]);
                CodigoConvocatoria = Convert.ToInt32(Session["CodConvocatoria"]);               
                EsMiembro = VerificarSiEsMienbroProyecto(CodigoProyecto, usuario.IdContacto);
                EsRealizado = VerificarSiEsRealizado(CodigoTab, CodigoProyecto, CodigoConvocatoria);

                if (!IsPostBack)
                {                    
                    inicioEncabezado(CodigoProyecto.ToString(), CodigoConvocatoria.ToString(), Constantes.ConstSubObservaciones);
                    cargarobservacion(CodigoProyecto, CodigoConvocatoria);
                    ObtenerDatosUltimaActualizacion();
                }

                div_Post_It.Visible = PostitVisible;
                Post_It._mostrarPost = PostitVisible;

                div_Post_It0.Visible = PostitVisible;
                Post_It0._mostrarPost = PostitVisible;

                div_Post_It1.Visible = PostitVisible;
                Post_It1._mostrarPost = PostitVisible;

                div_Post_It2.Visible = PostitVisible;
                Post_It2._mostrarPost = PostitVisible;

                div_Post_It3.Visible = PostitVisible;
                Post_It3._mostrarPost = PostitVisible;

                div_Post_It4.Visible = PostitVisible;
                Post_It4._mostrarPost = PostitVisible;

                div_Post_It5.Visible = PostitVisible;
                Post_It5._mostrarPost = PostitVisible;

                Actividades.Enabled = AllowUpdate;
                ProductosServicios.Enabled = AllowUpdate;
                EstrategiaMercado.Enabled = AllowUpdate;
                ProcesoProduccion.Enabled = AllowUpdate;
                EstructuraOrganizacionalEval.Enabled = AllowUpdate;
                TamanioLocalizacion.Enabled = AllowUpdate;
                Generales.Enabled = AllowUpdate;

                btnGuardar.Visible = AllowUpdate;
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

                if (entity != 0)
                    HttpContext.Current.Session["CodRol"] = entity;

                return entity != 0 ? true : false;
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

        public void cargarobservacion(Int32 codigoProyecto, Int32 codigoConvocatoria)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from observaciones in db.EvaluacionObservacions
                              where observaciones.CodConvocatoria == codigoConvocatoria
                                    && observaciones.CodProyecto == codigoProyecto
                              select observaciones).ToList().SingleOrDefault();

                if (entity != null)
                {
                    Actividades.Text = entity.Actividades;
                    ProductosServicios.Text = entity.ProductosServicios;
                    EstrategiaMercado.Text = entity.EstrategiaMercado;
                    ProcesoProduccion.Text = entity.ProcesoProduccion;
                    EstructuraOrganizacionalEval.Text = entity.EstructuraOrganizacional;
                    TamanioLocalizacion.Text = entity.TamanioLocalizacion;
                    Generales.Text = entity.Generales;
                }
            }
        }
    
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                FieldValidate.ValidateString("Actividades a las que se dedicará la empresa ", Actividades.Text, true, 10000);
                FieldValidate.ValidateString("Productos y servicios que ofrecerá ", ProductosServicios.Text, true, 10000);
                FieldValidate.ValidateString("Canales de distribución, estrategias de mercado ", EstrategiaMercado.Text, true, 10000);
                FieldValidate.ValidateString("Proceso de Producción ", ProcesoProduccion.Text, true, 10000);
                FieldValidate.ValidateString("Análisis estructura organizacional ", EstructuraOrganizacionalEval.Text, true, 10000);
                FieldValidate.ValidateString("Análisis Tamaño propuesto y localización ", TamanioLocalizacion.Text, true, 10000);
                FieldValidate.ValidateString("Resumen concepto General - Compromisos y Condiciones ", Generales.Text, true, 10000);

                var actividad = Actividades.Text;
                var productosYServicios = ProductosServicios.Text;
                var estrategiaMercado = EstrategiaMercado.Text;
                var procesoProduccion = ProcesoProduccion.Text;
                var estructuraOrganizacional = EstructuraOrganizacionalEval.Text;
                var tamanoLocalizacion = TamanioLocalizacion.Text;
                var generales = Generales.Text;

                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    var evaluacionObservacion = db.EvaluacionObservacions.SingleOrDefault(
                                                                                      filter =>
                                                                                                filter.CodConvocatoria == CodigoConvocatoria
                                                                                                && filter.CodProyecto == CodigoProyecto);
                    if (evaluacionObservacion == null)
                    {
                        evaluacionObservacion = new EvaluacionObservacion();

                        evaluacionObservacion.CodProyecto = CodigoProyecto;
                        evaluacionObservacion.CodConvocatoria = CodigoConvocatoria;
                        evaluacionObservacion.Actividades = actividad;
                        evaluacionObservacion.ProductosServicios = productosYServicios;
                        evaluacionObservacion.EstrategiaMercado = estrategiaMercado;
                        evaluacionObservacion.ProcesoProduccion = procesoProduccion;
                        evaluacionObservacion.EstructuraOrganizacional = estructuraOrganizacional;
                        evaluacionObservacion.TamanioLocalizacion = tamanoLocalizacion;
                        evaluacionObservacion.Generales = generales;
                        db.EvaluacionObservacions.InsertOnSubmit(evaluacionObservacion);                        
                    }
                    else
                    {
                        evaluacionObservacion.Actividades = actividad;
                        evaluacionObservacion.ProductosServicios = productosYServicios;
                        evaluacionObservacion.EstrategiaMercado = estrategiaMercado;
                        evaluacionObservacion.ProcesoProduccion = procesoProduccion;
                        evaluacionObservacion.EstructuraOrganizacional = estructuraOrganizacional;
                        evaluacionObservacion.TamanioLocalizacion = tamanoLocalizacion;
                        evaluacionObservacion.Generales = generales;                        
                    }
                    db.SubmitChanges();
                    
                    prActualizarTabEval(Constantes.ConstSubObservaciones.ToString(), CodigoProyecto.ToString(), CodigoConvocatoria.ToString());                                       
                    ObtenerDatosUltimaActualizacion();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Información guardada exitosamente.');", true);
                }
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
        
        private void ObtenerDatosUltimaActualizacion()
        {            
            String txtSQL;
            DateTime fecha = new DateTime();
            DataTable tabla = new DataTable();
            bool bNuevo = true; 
            var bRealizado = false;
            bool bEnActa = false;
            bool EsMiembro = false;
            Int32 CodigoEstado = 0;

            try
            {                
                bNuevo = es_bNuevo(CodigoProyecto.ToString());                
                bEnActa = es_EnActa(CodigoProyecto.ToString(), CodigoConvocatoria.ToString());
                EsMiembro = fnMiembroProyecto(usuario.IdContacto, CodigoProyecto.ToString());                
                CodigoEstado = CodEstado_Proyecto(Constantes.ConstSubObservaciones.ToString(), CodigoProyecto.ToString(), CodigoConvocatoria.ToString());
                               
                txtSQL = " SELECT CodContacto, CodRol From ProyectoContacto " +
                         " Where CodProyecto = " + CodigoProyecto.ToString() + " And CodContacto = " + usuario.IdContacto +
                         " and inactivo=0 and FechaInicio<=getdate() and FechaFin is null ";
                
                var rs = consultas.ObtenerDataTable(txtSQL, "text");

                if (rs.Rows.Count > 0)
                {                    
                    HttpContext.Current.Session["CodRol"] = rs.Rows[0]["CodRol"].ToString();
                }
                
                rs = null;
                txtSQL = " select nombres+' '+apellidos as nombre, fechamodificacion, realizado  " +
                         " from tabEvaluacionproyecto, contacto " +
                         " where id_contacto = codcontacto and codtabEvaluacion = " + Constantes.ConstSubObservaciones +
                         " and codproyecto = " + CodigoProyecto +
                         " and codconvocatoria = " + CodigoConvocatoria;
                
                tabla = consultas.ObtenerDataTable(txtSQL, "text");
                
                if (tabla.Rows.Count > 0)
                {                    
                    lbl_nombre_user_ult_act.Text = tabla.Rows[0]["nombre"].ToString().ToUpperInvariant();
                                        
                    try { fecha = Convert.ToDateTime(tabla.Rows[0]["FechaModificacion"].ToString()); }
                    catch { fecha = DateTime.Today; }                    
                    string sMes = fecha.ToString("MMM", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));                    
                    string hora = fecha.ToString("hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture).ToLowerInvariant();                    
                    if (hora.Contains("am")) { hora = hora.Replace("am", "a.m"); } if (hora.Contains("pm")) { hora = hora.Replace("pm", "p.m"); }                    
                    lbl_fecha_formateada.Text = UppercaseFirst(sMes) + " " + fecha.Day + " de " + fecha.Year + " " + hora + ".";                    
                    
                    bRealizado = Convert.ToBoolean(tabla.Rows[0]["Realizado"].ToString());                
                }

                chk_realizado.Checked = bRealizado;
                
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
                
                tabla = null;
                txtSQL = null;
            }
            catch (Exception ex)
            {                
            }
        }
        
        private int Obtener_numPostIt()
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from tareaUsuario in db.TareaUsuarioRepeticions
                              from tarea in db.TareaUsuarios
                              from programa in db.TareaProgramas
                              where programa.Id_TareaPrograma == tarea.CodTareaPrograma
                                    && tarea.Id_TareaUsuario == tareaUsuario.CodTareaUsuario
                                    && tarea.CodProyecto == CodigoProyecto
                                    && programa.Id_TareaPrograma == Constantes.CONST_PostIt
                                    && tareaUsuario.FechaCierre == null
                              select tareaUsuario).Count();

                return entity;
            }
        }
        
        protected void btn_guardar_ultima_actualizacion_Click(object sender, EventArgs e)
        {
            int flag = 0;
            flag = Marcar(Constantes.ConstSubObservaciones.ToString(), CodigoProyecto.ToString(), CodigoConvocatoria.ToString(), chk_realizado.Checked); 
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
    }
}