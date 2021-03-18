using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Negocio;
using System.Linq;
using System.Data;
using System.Web;
using Datos;

namespace Fonade.FONADE.evaluacion
{
    public partial class EvaluacionAspectos : Base_Page
    {

        public int CodigoAspecto { get; set; }
        public int CodigoConvocatoria { get; set; }
        public int CodigoProyecto { get; set; }
        public int Puntaje { get; set; }
        public int Total { get; set; }
        public int CodigoTab { get; set; }
        public string TabEvalucion { get; set; }
        public Boolean EsMiembro { get; set; }
        public Boolean EsRealizado { get; set; }
        public Boolean PostitVisible { 
            get {
                return EsMiembro && !EsRealizado; 
            }
            set { }
        }
        public Boolean AllowUpdate { 
            get {
                return EsMiembro && !EsRealizado && usuario.CodGrupo.Equals(Constantes.CONST_Evaluador);
            } 
            set { } 
        }

        String Accion2 = "CAMBIAR!";

        protected void Page_Load(object sender, EventArgs e)
        {
            try 
	        {
                if (Request["codAspecto"] == null)
                    throw new ApplicationException("No se pudo obtener el codigo del aspecto, intentenlo de nuevo.");
                if (Session["CodProyecto"] == null)
                    throw new ApplicationException("No se pudo obtener el codigo del proyecto, intentenlo de nuevo.");
                if (Session["CodConvocatoria"] == null)
                    throw new ApplicationException("No se pudo obtener el codigo de la convocatoria, intentenlo de nuevo.");
                if(usuario == null)
                    throw new ApplicationException("No se pudo obtener la información del usuario, intentenlo de nuevo.");

                CodigoAspecto = Convert.ToInt32(Request["codAspecto"]);
                CodigoProyecto = Convert.ToInt32(HttpContext.Current.Session["CodProyecto"]);
                CodigoConvocatoria = Convert.ToInt32(Session["CodConvocatoria"]);
                GetTipoAspectoandTabEvaluacion(CodigoAspecto);
                EsMiembro = VerificarSiEsMienbroProyecto(CodigoProyecto, usuario.IdContacto);
                EsRealizado = VerificarSiEsRealizado(CodigoTab, CodigoProyecto, CodigoConvocatoria);

                if (!IsPostBack)
                {                                        
                    Post_It._txtCampo = TabEvalucion;
                    inicioEncabezado(CodigoProyecto.ToString(), CodigoConvocatoria.ToString(), 1);
                    LoadDatosEvaluacionAspectos(CodigoProyecto, CodigoConvocatoria, CodigoAspecto);
                    ObtenerDatosUltimaActualizacion();               
                }

                div_Post_It.Visible = PostitVisible;
                Post_It._mostrarPost = PostitVisible;
                update.Visible = AllowUpdate;
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
  
        protected void GetTipoAspectoandTabEvaluacion( Int32 codigoAspecto)
        {
            switch (codigoAspecto)
            {
                case 1:
                    CodigoTab = Constantes.ConstSubGenerales;
                    TabEvalucion = "tablaGenerales";
                    break;
                case 2:
                    CodigoTab = Constantes.ConstSubComerciales;
                    TabEvalucion = "tablaComerciales";
                    break;
                case 3:
                    CodigoTab = Constantes.ConstSubTecnicos;
                    TabEvalucion = "tablaTecnicos";
                    break;
                case 4:
                    CodigoTab = Constantes.ConstSubOrganizacionales;
                    TabEvalucion = "tablaOrganizacionales";
                    break;
                case 5:
                    CodigoTab = Constantes.ConstSubFinancieros;
                    TabEvalucion = "tablaFinancieros";
                    break;
                case 6:
                    CodigoTab = Constantes.ConstSubMedioAmbiente;
                    TabEvalucion = "tablaMedioAmbiente";
                    break;
                default:
                    CodigoTab = Constantes.ConstSubAspectosEvaluados;
                    TabEvalucion = "AspectosEvaluados";
                    break;
            }
        }
                         
        protected Boolean VerificarSiEsMienbroProyecto(Int32 codigoProyecto, Int32 codigoContacto) {
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

                if (entity != null)
                    HttpContext.Current.Session["CodRol"] = entity;

                return entity != null ? true : false;
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

        public void LoadDatosEvaluacionAspectos(Int32 codigoProyecto, Int32 codigoConvocatoria, Int32 codigoAspecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = db.MD_ObtenerCamposEvaluacionObservaciones(codigoProyecto, codigoConvocatoria, codigoAspecto).ToList();

                DltEvaluacion.DataSource = entity;
                DltEvaluacion.DataBind();                
            }            
        }

        public List<MD_ObtenerCamposEvaluacionObservacionesHijasResult> LoadDatosEvaluacionAspectos(string orden)
        {
            string mensajeDeError = string.Empty;
            List<MD_ObtenerCamposEvaluacionObservacionesHijasResult> result = null;

            if (CodigoAspecto != 0)
            {
                result = consultas.ObtenerCamposEvaluacionObservacionesHijas(orden, CodigoProyecto, CodigoConvocatoria, CodigoAspecto, ref mensajeDeError);

            }
            return result;
        }


        public void DisabledControls()
        {
            if (CodigoAspecto != 6)
            {
                imagen1.Style.Add("display", "block");
                imagen2.Style.Add("display", "none");
            }
            else
            {
                imagen1.Style.Add("display", "none");
                imagen2.Style.Add("display", "block");
            }
        }

        public string GetObservation(int idcampo)
        {
            string observation = string.Empty;

            var query = consultas.Db.EvaluacionCampoJustificacions.FirstOrDefault(
                e => e.CodProyecto == CodigoProyecto
                     && e.CodConvocatoria == CodigoConvocatoria
                     && e.CodCampo == idcampo);
            if (query != null && !string.IsNullOrEmpty(query.Justificacion))
            {
                observation = query.Justificacion;
            }

            return observation;
        }
        
        protected void DltEvaluacion_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var campoid = e.Item.FindControl("campoid") as Label;

                var dtlHijos = e.Item.FindControl("DtlHijos") as DataList;

                if (CodigoAspecto != 6)
                {
                    if (dtlHijos != null)
                    {
                        if (campoid != null)
                            dtlHijos.DataSource = LoadDatosEvaluacionAspectos(campoid.Text);
                        dtlHijos.DataBind();
                    }
                }
                else
                {
                    DltEvaluacion.ShowHeader = false;
                    DltEvaluacion.ShowFooter = false;

                    if (dtlHijos != null)
                    {
                        if (campoid != null)
                            dtlHijos.DataSource = LoadDatosEvaluacionAspectos(campoid.Text);
                        dtlHijos.DataBind();
                    }
                }
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                var lpuntaje = e.Item.FindControl("lpuntajeObtenido") as Label;

                if (CodigoAspecto != 6)
                {
                    if (lpuntaje != null) lpuntaje.Text = Convert.ToString(Puntaje);
                }
                else
                {
                    if (lpuntaje != null) lpuntaje.Visible = false;
                }
            }
        }

        protected void DtlHijos_ItemDataBound(object sender, DataListItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var idcampo = e.Item.Parent.Parent.FindControl("campoid") as Label;
                var observaciones = e.Item.Parent.Parent.FindControl("txtobservaciones") as TextBox;
                var asignado = e.Item.FindControl("lAsignado") as Label;
                var maximo = e.Item.FindControl("lblmaximo") as Label;
                var ddl = e.Item.FindControl("Ddlpuntaje") as DropDownList;
                var ddl_1 = e.Item.FindControl("lbl_Ddlpuntaje") as Label;
                var ddlMedio = e.Item.FindControl("DdlpuntajeMedio") as DropDownList;
                var ddlMedio_1 = e.Item.FindControl("lbl_DdlpuntajeMedio") as Label;


                if (CodigoAspecto == 6)
                {
                    maximo.Visible = false;
                    ddl.Visible = false;
                    ddlMedio.Visible = true;

                    ddlMedio.Items.Clear();
                    ddlMedio.AutoPostBack = false;
                    ddlMedio.Items.Insert(0, new ListItem("SI", "1"));
                    ddlMedio.Items.Insert(1, new ListItem("NO", "0"));

                    if (usuario.CodGrupo == Constantes.CONST_Evaluador && EsRealizado)
                    {
                        ddlMedio.Enabled = false;
                    }

                    if (asignado.Text != null )
                    {
                        ddlMedio.SelectedValue = asignado.Text;
                        ddlMedio_1.Text = ddlMedio.SelectedItem.Text;

                        if (usuario.CodGrupo == Constantes.CONST_GerenteEvaluador || usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador)
                        {
                            ddlMedio.Visible = false;
                            ddlMedio_1.Visible = true;
                        }
                    }
                    if (asignado.Text != null && !asignado.Text.Equals("0"))
                    {               
                        ddlMedio_1.Text = ddlMedio.SelectedItem.Text;

                        if (usuario.CodGrupo == Constantes.CONST_GerenteEvaluador || usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador)
                        {
                            ddlMedio.Visible = false;
                            ddlMedio_1.Visible = true;
                        }
                    }
                    if (idcampo != null && !string.IsNullOrEmpty(idcampo.Text))
                    {
                        if (observaciones != null)
                        {
                            observaciones.Text = GetObservation(Convert.ToInt32(idcampo.Text)); observaciones.MaxLength = 1000;
                            if (!(Accion2 != "Todos" && EsMiembro && !EsRealizado && usuario.CodGrupo == Constantes.CONST_Evaluador))
                            { observaciones.Enabled = false; }
                        }
                    }
                }
                else
                {
                    if (idcampo != null && !string.IsNullOrEmpty(idcampo.Text))
                    {
                        if (observaciones != null)
                        {
                            observaciones.Text = GetObservation(Convert.ToInt32(idcampo.Text)); observaciones.MaxLength = 1000;
                            if (!(Accion2 != "Todos" && EsMiembro && !EsRealizado && usuario.CodGrupo == Constantes.CONST_Evaluador))
                            { observaciones.Enabled = false; }
                        }
                    }

                    ddl.Items.Insert(1, new ListItem(maximo.Text, maximo.Text));

                    if (usuario.CodGrupo == Constantes.CONST_Evaluador && EsRealizado)
                    {
                        ddl.Enabled = false;
                    }

                    if (asignado.Text != null && asignado.Text.Equals("0"))
                    {
                        ddl.SelectedValue = asignado.Text;
                        ddl_1.Text = asignado.Text;

                        if (usuario.CodGrupo == Constantes.CONST_GerenteEvaluador || usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador)
                        {
                            ddl.Visible = false;
                            ddl_1.Visible = true;
                        }

                    }
                    if (asignado.Text != null && !asignado.Text.Equals("0"))
                    {
                        ddl.SelectedValue = maximo.Text;
                        ddl_1.Text = asignado.Text;

                        if (usuario.CodGrupo == Constantes.CONST_GerenteEvaluador || usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador)
                        {
                            ddl.Visible = false;
                            ddl_1.Visible = true;
                        }
                    }
                }
                if (asignado.Text != null)
                {
                    Puntaje += !string.IsNullOrEmpty(asignado.Text) ? Convert.ToInt32(asignado.Text) : 0;
                }
            }
        }

        protected void update_Click(object sender, EventArgs e)
        {
            try
            {            
                string resultado = "";

                resultado = Validar();

                if (resultado == "")
                {
                    ActualizarEvaluacion();
                    LoadDatosEvaluacionAspectos(CodigoProyecto, CodigoConvocatoria, CodigoAspecto);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + resultado + "');", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }
        }

        public void ActualizarEvaluacion()
        {
            foreach (DataListItem item in DltEvaluacion.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Footer)
                {
                    var observaciones = item.FindControl("txtobservaciones") as TextBox;
                    var campoid = item.FindControl("campoid") as Label;
                    var dtlHijos = item.FindControl("DtlHijos") as DataList;

                    if (dtlHijos != null)
                    {
                        foreach (DataListItem itemhijos in dtlHijos.Items)
                        {
                            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                            {
                                var evaluacionCampos = new EvaluacionCampo();

                                var idcampo = itemhijos.FindControl("idcampo") as Label;

                                var ddl = itemhijos.FindControl("Ddlpuntaje") as DropDownList;

                                if (ddl != null && ddl.Visible)
                                {
                                    if (idcampo != null)
                                    {
                                        evaluacionCampos.Puntaje = Convert.ToInt16(ddl.SelectedValue);
                                        evaluacionCampos.codCampo = Convert.ToInt16(idcampo.Text);
                                        consultarItems(evaluacionCampos);
                                    }
                                }

                                var ddlMedio = itemhijos.FindControl("DdlpuntajeMedio") as DropDownList;
                                if (ddlMedio != null && ddlMedio.Visible)
                                {
                                    evaluacionCampos.Puntaje = Convert.ToInt16(ddlMedio.SelectedValue);
                                    evaluacionCampos.codCampo = Convert.ToInt16(idcampo.Text);
                                    consultarItems(evaluacionCampos);
                                }
                            }
                        }
                    }
                    var evaluacion = new EvaluacionCampoJustificacion();
                    if (observaciones != null)
                    {
                        evaluacion.Justificacion = observaciones.Text;
                    }

                    if (campoid != null && !string.IsNullOrEmpty(campoid.Text))
                    {
                        evaluacion.CodCampo = Convert.ToInt16(campoid.Text);
                    }

                    crearcampoJustificacion(evaluacion);
                }
            }
        }
        
        public void consultarItems(EvaluacionCampo entidad)
        {
            try
            {
                //Obtener los valores de las variables de sesión.
                CodigoAspecto = Convert.ToInt16(Request["codAspecto"]);
                CodigoProyecto = !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) && HttpContext.Current.Session["CodProyecto"] != null ? Convert.ToInt32(HttpContext.Current.Session["CodProyecto"].ToString()) : 0;
                CodigoConvocatoria = !string.IsNullOrEmpty(HttpContext.Current.Session["CodConvocatoria"].ToString()) && HttpContext.Current.Session["CodConvocatoria"] != null ? Convert.ToInt32(HttpContext.Current.Session["CodConvocatoria"].ToString()) : 0;

                if (entidad.codCampo != 0)
                {
                    var query = consultas.Db.EvaluacionCampos.FirstOrDefault(e => e.codProyecto == CodigoProyecto &&
                                                                                  e.codConvocatoria == CodigoConvocatoria &&
                                                                                  e.codCampo == entidad.codCampo);
                    if (query != null && query.codCampo != 0)
                    {
                        query.Puntaje = entidad.Puntaje;
                        consultas.Db.SubmitChanges();
                        //Actualizar fecha de modificación
                        prActualizarTabEval(CodigoTab.ToString(), CodigoProyecto.ToString(), CodigoConvocatoria.ToString());
                        ObtenerDatosUltimaActualizacion();
                    }
                    else
                    {
                        entidad.codProyecto = CodigoProyecto;
                        entidad.codConvocatoria = CodigoConvocatoria;
                        consultas.Db.EvaluacionCampos.InsertOnSubmit(entidad);
                        consultas.Db.SubmitChanges();
                        //Actualizar fecha de modificación
                        prActualizarTabEval(CodigoTab.ToString(), CodigoProyecto.ToString(), CodigoConvocatoria.ToString());
                        ObtenerDatosUltimaActualizacion();
                    }
                }
            }
            catch (Exception) { throw new Exception("Error"); }
        }
        
        public void crearcampoJustificacion(EvaluacionCampoJustificacion entitdad)
        {            
            CodigoAspecto = Convert.ToInt16(Request["codAspecto"]);
            CodigoProyecto = !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) && HttpContext.Current.Session["CodProyecto"] != null ? Convert.ToInt32(HttpContext.Current.Session["CodProyecto"].ToString()) : 0;
            CodigoConvocatoria = !string.IsNullOrEmpty(HttpContext.Current.Session["CodConvocatoria"].ToString()) && HttpContext.Current.Session["CodConvocatoria"] != null ? Convert.ToInt32(HttpContext.Current.Session["CodConvocatoria"].ToString()) : 0;

            try
            {
                var query = consultas.Db.EvaluacionCampoJustificacions.FirstOrDefault(c => c.CodProyecto == CodigoProyecto &&
                                                                                   c.CodConvocatoria == CodigoConvocatoria &&
                                                                                   c.CodCampo == entitdad.CodCampo);

                if (query != null && query.CodCampo != 0)
                {
                    query.Justificacion = entitdad.Justificacion;
                    consultas.Db.SubmitChanges();                    
                    prActualizarTabEval(CodigoTab.ToString(), CodigoProyecto.ToString(), CodigoConvocatoria.ToString());
                    ObtenerDatosUltimaActualizacion();
                }
                else
                {
                    entitdad.CodProyecto = CodigoProyecto;
                    entitdad.CodConvocatoria = CodigoConvocatoria;
                    consultas.Db.EvaluacionCampoJustificacions.InsertOnSubmit(entitdad);
                    consultas.Db.SubmitChanges();                   
                    prActualizarTabEval(CodigoTab.ToString(), CodigoProyecto.ToString(), CodigoConvocatoria.ToString());
                    ObtenerDatosUltimaActualizacion();
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
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
            EsRealizado = false;
            bool bEnActa = false;
            bool EsMiembro = false;
            Int32 CodigoEstado = 0;

            try
            {                
                bNuevo = es_bNuevo(CodigoProyecto.ToString());
                
                bEnActa = es_EnActa(CodigoProyecto.ToString(), CodigoConvocatoria.ToString());
                
                EsMiembro = fnMiembroProyecto(usuario.IdContacto, CodigoProyecto.ToString());
                
                CodigoEstado = CodEstado_Proyecto(CodigoTab.ToString(), CodigoProyecto.ToString(), CodigoConvocatoria.ToString());
               
                txtSQL = " SELECT CodContacto, CodRol From ProyectoContacto " +
                         " Where CodProyecto = " + CodigoProyecto + " And CodContacto = " + usuario.IdContacto +
                         " and inactivo=0 and FechaInicio<=getdate() and FechaFin is null ";
                
                var rs = consultas.ObtenerDataTable(txtSQL, "text");

                if (rs.Rows.Count > 0)
                {                    
                    HttpContext.Current.Session["CodRol"] = rs.Rows[0]["CodRol"].ToString();
                }
                
                rs = null;                
                
                txtSQL = " select nombres+' '+apellidos as nombre, fechamodificacion, realizado  " +
                         " from tabEvaluacionproyecto, contacto " +
                         " where id_contacto = codcontacto and codtabEvaluacion = " + CodigoTab +
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
                   
                    EsRealizado = Convert.ToBoolean(tabla.Rows[0]["Realizado"].ToString());
                }

                chk_realizado.Checked = EsRealizado;
             
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
                tabla = null;
                txtSQL = null;
                return;
            }
        }        
        
        private int NumeroPostits()
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
            flag = Marcar(CodigoTab.ToString(), CodigoProyecto.ToString(), CodigoConvocatoria.ToString(), chk_realizado.Checked); 
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
        
        private string Validar()
        {
            string resultado = "";

            foreach (DataListItem item in DltEvaluacion.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Footer)
                {
                    var observaciones = item.FindControl("txtobservaciones") as TextBox;
                    var label_obsv = item.FindControl("Label2") as Label; //Label que contiene el "Orden" de los campos de "Observaciones".

                    if (observaciones.Text.Trim().Length == 0)
                    { if (label_obsv != null) { resultado = "La observación en la sección " + label_obsv.Text + " es requerida."; break; } else { resultado = Texto("TXT_OBSERVACION_REQ"); break; } }
                    if (observaciones.Text.Trim().Length > 1000)
                    { if (label_obsv != null) { resultado = "El texto de la observación en la sección " + label_obsv.Text + " es demasiado largo"; break; } else { resultado = "El texto es demasiado largo"; break; } }
                }
            }
            return resultado;
        }
        
    }
}