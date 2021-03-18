using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Negocio;
using System.Linq;
using System.Data;
using System.Web;
using Datos;
using System.Text;
using Datos.Modelos;

namespace Fonade.PlanDeNegocioV2.Evaluacion.TablaDeEvaluacion
{
    public partial class EvaluacionAspectos : Base_Page
    {
        public int CodigoAspecto { get {
				int codigo = 0;
				if (Convert.ToInt32(Request["codAspecto"]) == 156)
				{
					using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
					{
						codigo = (int)(from C in db.Campo
												 join CC in db.ConvocatoriaCampos
												 on C.id_Campo equals CC.codCampo
												 where CC.codConvocatoria == CodigoConvocatoria
												 && C.Campo1.Contains("Resumen")
												 orderby C.id_Campo descending
												 select C.id_Campo).FirstOrDefault();
					}
				}
				else
				{
					codigo = Convert.ToInt32(Request["codAspecto"]);
				}
                return codigo;
            }
            set {
            }
        }
        public int CodigoConvocatoria
        {
            get
            {
                return Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(CodigoProyecto, HttpContext.Current.Session["HistorialEvaluacion"] != null ? Convert.ToInt32(HttpContext.Current.Session["HistorialEvaluacion"]) : 0).GetValueOrDefault();
            }
            set { }
        }
        public int CodigoProyecto { get { return Convert.ToInt32(Request.QueryString["codproyecto"]); } set { } }
        public int Puntaje { get; set; }
        public int Total { get; set; }
        public int CodigoTab { get; set; }
        public string TabEvalucion { get; set; }
        public Boolean EsMiembro { get; set; }
        public Boolean EsRealizado { get; set; }
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

        String Accion2 = "CAMBIAR!";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request["codAspecto"] == null)
                    throw new ApplicationException("No se pudo obtener el codigo del aspecto, intentenlo de nuevo.");
                if (Request["codproyecto"] == null)
                    throw new ApplicationException("No se pudo obtener el codigo del proyecto, intentenlo de nuevo.");
				
				GetTipoAspectoandTabEvaluacion(CodigoAspecto);
                EsMiembro = VerificarSiEsMienbroProyecto(CodigoProyecto, usuario.IdContacto);
                EsRealizado = VerificarSiEsRealizado(CodigoTab, CodigoProyecto, CodigoConvocatoria);

                EncabezadoEval.IdProyecto = CodigoProyecto;
                EncabezadoEval.IdConvocatoria = CodigoConvocatoria;
                EncabezadoEval.IdTabEvaluacion = CodigoTab;

                if (!IsPostBack)
                {
                    Post_It._txtCampo = TabEvalucion;
                    inicioEncabezado(CodigoProyecto.ToString(), CodigoConvocatoria.ToString(), 1);
                    LoadDatosEvaluacionAspectos(CodigoProyecto, CodigoConvocatoria, CodigoAspecto);                    
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

        protected void GetTipoAspectoandTabEvaluacion(Int32 codigoAspecto)
        {
            switch (codigoAspecto)
            {
                case Constantes.Const_AspectoProtagonistaV2:
                    CodigoTab = Constantes.Const_QuienEsElProtagonistaV2;
                    TabEvalucion = "tablaProtagonista";
                    break;
                case Constantes.Const_AspectoOportunidadMercadoV2:
                    CodigoTab = Constantes.Const_ExisteOportunidadEnElMercadoV2;
                    TabEvalucion = "tablaOportunidadMercado";
                    break;
                case Constantes.Const_AspectoCualEsMiSolucionV2:
                    CodigoTab = Constantes.Const_CualEsMiSolucionV2;
                    TabEvalucion = "tablaCualEsMiSolucion";
                    break;
                case Constantes.Const_AspectoDesarrolloSolucionV2:
                    CodigoTab = Constantes.Const_ComoDesarrolloMiSolucionV2;
                    TabEvalucion = "tablaDesarrolloSolucion";
                    break;
                case Constantes.Const_AspectoFuturoNegocioV2:
                    CodigoTab = Constantes.Const_CualEsElFuturoDeMiNegocioV2;
                    TabEvalucion = "tablaFuturoNegocio";
                    break;
                case Constantes.Const_AspectoRiesgosV2:
                    CodigoTab = Constantes.Const_QueRiesgosEnfrentoV2;
                    TabEvalucion = "tablaRiesgos";
                    break;
                case Constantes.Const_AspectoResumenEjecutivoV2:
                    CodigoTab = Constantes.Const_ResumenEjecutivoV2;
                    TabEvalucion = "tablaResumenEjecutivo";
                    break;
                case Constantes.Const_AspectoResumenEjecutivoV2NewProd:
                    CodigoTab = Constantes.Const_ResumenEjecutivoV2;
                    TabEvalucion = "tablaResumenEjecutivo";
                    break;

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
                          ).FirstOrDefault();

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
            if (CodigoAspecto != Constantes.Const_AspectoResumenEjecutivoV2)
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

                if (CodigoAspecto != Constantes.Const_AspectoResumenEjecutivoV2)
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

                if (CodigoAspecto != Constantes.Const_AspectoResumenEjecutivoV2)
                {
                    if (lpuntaje != null) {                  
                            lpuntaje.Text = Convert.ToString(Puntaje);
                    }
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

				MD_ObtenerCamposEvaluacionObservacionesHijasResult 
					row = (MD_ObtenerCamposEvaluacionObservacionesHijasResult)e.Item.DataItem;
				 

				var idcampo = e.Item.Parent.Parent.FindControl("campoid") as Label;
                var observaciones = e.Item.Parent.Parent.FindControl("txtobservaciones") as TextBox;
                var asignado = e.Item.FindControl("lAsignado") as Label;
                var maximo = e.Item.FindControl("lblmaximo") as Label;
                var ddl = e.Item.FindControl("Ddlpuntaje") as DropDownList;
                var ddl_1 = e.Item.FindControl("lbl_Ddlpuntaje") as Label;
                var ddlMedio = e.Item.FindControl("DdlpuntajeMedio") as DropDownList;
                var ddlMedio_1 = e.Item.FindControl("lbl_DdlpuntajeMedio") as Label;


                if (CodigoAspecto == Constantes.Const_AspectoResumenEjecutivoV2)
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

                    if (asignado.Text != null)
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

					//Carga de seleccion de valores de puntajes William P.L.
					Negocio.PlanDeNegocioV2.Evaluacion.TablaDeEvaluacion.ConsultasTablaDeEvaluacion 
						ConsultasTablaEval = new Negocio.PlanDeNegocioV2.Evaluacion.TablaDeEvaluacion.ConsultasTablaDeEvaluacion();

					int idTipoCampo = ConsultasTablaEval.getIdTipoCampo(row.id_campo);

					if (idTipoCampo == 2) //se consulta en BD las opciones para agregar
					{
						List<TipoCampoValoresModel> list = ConsultasTablaEval.getPuntajes(row.id_campo);
						int count = 1;
						foreach (var r in list)
						{
							ddl.Items.Insert(count, new ListItem(r.puntaje.ToString(), r.puntaje.ToString()));
							count++;
						}

						//ddl.Items.Insert(1, new ListItem("1", "1"));
						//ddl.Items.Insert(2, new ListItem("2", "2"));
						//ddl.Items.Insert(3, new ListItem("3", "3"));
					}
					else
					{
						ddl.Items.Insert(1, new ListItem(maximo.Text, maximo.Text));
					}

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
						//Se cambia de maximo.Text a asignado.Text para que tome el valor de bd
						// y no de lo que contenga el label - William P.L. 18/06/2018 
						ddl.SelectedValue = asignado.Text;

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
                ValidarCampos();                
                ActualizarEvaluacion();
                LoadDatosEvaluacionAspectos(CodigoProyecto, CodigoConvocatoria, CodigoAspecto);

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
                EncabezadoEval.GetUltimaActualizacion();

                Formulacion.Utilidad.Utilidades.PresentarMsj(messageResult, this, "Alert");                
            }
            catch (ApplicationException ex)
            {
                Formulacion.Utilidad.Utilidades.PresentarMsj(ex.Message, this, "Alert");
            }
            catch (Exception ex)
            {
                Formulacion.Utilidad.Utilidades.PresentarMsj("Sucedio un error al guardar los aspectos.", this, "Alert");
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
                    }
                    else
                    {
                        entidad.codProyecto = CodigoProyecto;
                        entidad.codConvocatoria = CodigoConvocatoria;
                        consultas.Db.EvaluacionCampos.InsertOnSubmit(entidad);
                        consultas.Db.SubmitChanges();
                        //Actualizar fecha de modificación
                        prActualizarTabEval(CodigoTab.ToString(), CodigoProyecto.ToString(), CodigoConvocatoria.ToString());                        
                    }
                }
            }
            catch (Exception) { throw new Exception("Error"); }
        }

        public void crearcampoJustificacion(EvaluacionCampoJustificacion entitdad)
        {          
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
                }
                else
                {
                    entitdad.CodProyecto = CodigoProyecto;
                    entitdad.CodConvocatoria = CodigoConvocatoria;
                    consultas.Db.EvaluacionCampoJustificacions.InsertOnSubmit(entitdad);
                    consultas.Db.SubmitChanges();
                    prActualizarTabEval(CodigoTab.ToString(), CodigoProyecto.ToString(), CodigoConvocatoria.ToString());                    
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
                        
        private void ValidarCampos()
        {            
            foreach (DataListItem item in DltEvaluacion.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Footer)
                {
                    var txtObservacion = item.FindControl("txtobservaciones") as TextBox;
                    var lblObservacion = item.FindControl("Label2") as Label;

                    ValidateString(lblObservacion.Text, txtObservacion.Text, 1000);
                    
                }
            }            
        }
                
        public static void ValidateString(string fieldName,string fieldData, int maxLength = 0)
        {
            StringBuilder messageError = new StringBuilder();

            if (String.IsNullOrEmpty(fieldData))
                throw new ApplicationException(messageError.AppendFormat(" La observación en la sección {0} es obligatoria y no puede estar vacía.", fieldName).ToString());

            if (fieldData.Length > maxLength && maxLength != 0)
                throw new ApplicationException(messageError.AppendFormat("La observación en la sección {0} debe ser máximo de {1} caracteres.", fieldName, maxLength.ToString()).ToString());
            
        }
    }
}