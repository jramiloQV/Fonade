using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using Fonade.Negocio;
using Newtonsoft.Json;
using System.Globalization;

namespace Fonade.FONADE.evaluacion
{
    public partial class CrearActa : Base_Page
    {
        string[] _arrQuery;
        string[] _arrCriterio;
        string[] _arrIncidencia;
        private double Total;
        String actaPublicado;
        Int32 editar;
        Int32 id;
  
        /// <summary>
        /// Page_Load.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            actaPublicado = Convert.ToString(HttpContext.Current.Session["publicado"] ?? "false");
            id = Convert.ToInt32(HttpContext.Current.Session["idacta"] ?? 0);

            //Validacion de permisos, si pertenece a alguno de estos grupos, no se permite editar solo consulta.
            if (usuario.CodGrupo != Constantes.CONST_GerenteInterventor && usuario.CodGrupo != Constantes.CONST_CoordinadorInterventor && usuario.CodGrupo != Constantes.CONST_Interventor)
                editar = 1;
            else
                editar = 0;            
            if (!IsPostBack)
                Titulos();

            if (usuario.CodGrupo != Constantes.CONST_GerenteEvaluador) {
                btnCrearActa.Visible = false;
                btnupdate.Visible = false;
            }            
        }

        /// <summary>
        /// Establecer acciones sobre los controles de esta pantalla, así como su 
        /// visualización
        /// </summary>
        void Titulos()
        {
            if (!string.IsNullOrEmpty(Request["a"]))
            {
                lbltitulo.Text = "Crear Acta";
                imgadicionarplan.Visible = false;
                lnkadcionarplan.Visible = false;
                panelNegocioGrid.Visible = false;
            }
            else
            {
                if (HttpContext.Current.Session["idacta"] != null)
                {
                    lbltitulo.Text = "Ver Acta";
                    if (id == 0 || String.IsNullOrEmpty(HttpContext.Current.Session["idacta"].ToString())) btnCrearActa.Visible = true; else btnCrearActa.Visible = false;
                    BuscarActaId(id);
                }
                if (Boolean.Parse(actaPublicado))
                {
                    txtNroActa.Enabled = false;
                    txtnomActa.Enabled = false;
                    txtfecha.Enabled = false;
                    btnDate2.Visible = false;
                    txtObservaciones.Enabled = false;
                    DdlCodConvocatoria.Enabled = false;
                    pnlNegocioPublico.Visible = false;
                    txtSearch.Enabled = false;

                    imgadicionarplan.Visible = false;
                    lnkadcionarplan.Visible = false;
                    btnimprimir.Visible = true;
                }
                else
                {
                    if (editar == 0)
                    {
                        txtSearch.Enabled = false;
                        txtNroActa.Enabled = false;
                        txtnomActa.Enabled = false;
                        txtfecha.Enabled = false;
                        btnDate2.Visible = false;
                        txtObservaciones.Enabled = false;
                        DdlCodConvocatoria.Enabled = false;
                        pnlNegocioPublico.Visible = false;                        
                        btnCrearActa.Visible = true;
                        btnupdate.Visible = false;
                    }
                    else
                    {
                        txtSearch.Enabled = true;
                        txtNroActa.Enabled = true;
                        txtnomActa.Enabled = true;
                        txtfecha.Enabled = true;
                        txtObservaciones.Enabled = true;
                        DdlCodConvocatoria.Enabled = false;
                        pnlNegocioPublico.Visible = true;
                        imgadicionarplan.Visible = true;
                        lnkadcionarplan.Visible = true;
                        btnupdate.Visible = true;
                        btnCrearActa.Visible = false;
                    }
                }
            }
        }

        /// <summary>
        /// Crear un acta de validación final de evaluación
        /// </summary>
        void Crear()
        {
            var actaFinalEvaluacion = new Datos.EvaluacionActa();
            try
            {
                var idactas = consultas.Db.EvaluacionActas.FirstOrDefault(a => Equals(a.NumActa, txtNroActa.Text));
                if (idactas == null)
                {
                    var validaActa = consultas.Db.EvaluacionActas.FirstOrDefault(a => a.NomActa.Contains(txtnomActa.Text));

                    if (validaActa == null)
                    {
                        if (string.IsNullOrEmpty(DdlCodConvocatoria.SelectedValue))
                        {
                            RedirectPage(false, "seleccione una convocatoria");
                        }
                        else
                        {
                            HttpContext.Current.Session["publicado"] = false;
                            actaFinalEvaluacion.NumActa = txtNroActa.Text.Trim();
                            actaFinalEvaluacion.NomActa = txtnomActa.Text;
                            actaFinalEvaluacion.FechaActa = DateTime.ParseExact(txtfecha.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            actaFinalEvaluacion.Observaciones = txtObservaciones.Text.Trim();
                            actaFinalEvaluacion.CodConvocatoria = Convert.ToInt32(DdlCodConvocatoria.SelectedValue);
                            actaFinalEvaluacion.publicado = false;

                            HttpContext.Current.Session["oEvaluacionActa"] = actaFinalEvaluacion;

                            consultas.Db.EvaluacionActas.InsertOnSubmit(actaFinalEvaluacion);
                            consultas.Db.SubmitChanges();
                            var actaid = consultas.Db.EvaluacionActas.FirstOrDefault(a => a.NomActa.Contains(txtnomActa.Text));

                            if (actaid != null && actaid.Id_Acta != 0)
                            {
                                lidacta.Text = actaid.Id_Acta.ToString();
                                HttpContext.Current.Session["idacta"] = actaid.Id_Acta;
                            }
                            DeshabilitarPanelCrear(false, true);

                            Response.Redirect(Request.Url.ToString().Split('?')[0]);                            

                            lbltitulo.Text = "Ver Acta";
                            int id = Convert.ToInt32(HttpContext.Current.Session["idacta"].ToString());

                            actaPublicado = "false";
                            pnlNegocioPublico.Visible = true;
                            DdlCodConvocatoria.Enabled = false;
                            btnCrearActa.Visible = false;
                            btnupdate.Visible = true;

                            txtNroActa.Enabled = true;
                            txtnomActa.Enabled = true;
                            txtfecha.Enabled = true;
                            txtObservaciones.Enabled = true;
                            DdlCodConvocatoria.Enabled = false;
                            pnlNegocioPublico.Visible = true;

                            imgadicionarplan.Visible = true;
                            lnkadcionarplan.Visible = true;
                        }
                    }
                    else
                    {
                        RedirectPage(false, "Ya existe un Acta con ese nombre");
                        btnupdate.Visible = false;
                        btnCrearActa.Visible = true;
                    }
                }
                else
                {
                    RedirectPage(false, "Ya existe un Acta con ese numero");
                    btnupdate.Visible = false;
                    btnCrearActa.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error al intentar crear el acta, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }
        }

        /// <summary>
        /// Buscar un acta de validación final de evaluación por Id
        /// </summary>
        /// <param name="idacta"> Codigo de acta </param>
        [WebMethod]
        void BuscarActaId(int idacta)
        {
            if (idacta != 0)
            {
                var actas = consultas.Db.EvaluacionActas.FirstOrDefault(a => a.Id_Acta == idacta);

                if (actas != null && actas.Id_Acta != 0)
                {
                    HttpContext.Current.Session["oEvaluacionActa"] = actas;
                }

                if (HttpContext.Current.Session["oEvaluacionActa"] != null)
                {
                    actas = (Datos.EvaluacionActa)Session["oEvaluacionActa"];
                }

                if (actas != null && actas.Id_Acta != 0)
                {
                    txtNroActa.Text = actas.NumActa;
                    txtnomActa.Text = actas.NomActa;
                    txtfecha.Text = actas.FechaActa.ToShortDateString();
                    txtObservaciones.Text = actas.Observaciones;
                    DdlCodConvocatoria.SelectedValue = actas.CodConvocatoria.ToString();
                    lidacta.Text = actas.Id_Acta.ToString();
                    if (actas.publicado != null && (bool)actas.publicado)
                    {
                        pnlNegocioPublico.Visible = false;
                        actaPublicado = "true";
                        DeshabilitarEdit(false);
                        btnCrearActa.Visible = false;
                    }
                    else
                    {
                        actaPublicado = "false";
                        pnlNegocioPublico.Visible = true;
                        DdlCodConvocatoria.Enabled = false;
                        btnCrearActa.Visible = false;
                        btnupdate.Visible = false;
                    }
                }
            }
        }

        /// <summary>
        /// Actualizar un acta de validación final
        /// </summary>
        void Actualizar()
        {
            Boolean isPublicada = chkpublico.Checked;

            try
            {
                var actaEvaluacion = consultas.Db.EvaluacionActas.FirstOrDefault(e => e.NomActa.Contains(txtnomActa.Text) && e.Id_Acta == Convert.ToInt32(lidacta.Text));
                if (actaEvaluacion == null)
                    throw new ApplicationException("No se pudo obtener el acta final de validación");

                //Actualizar datos del acta
                actaEvaluacion.NomActa = txtnomActa.Text;
                actaEvaluacion.NumActa = txtNroActa.Text;
                actaEvaluacion.FechaActa = DateTime.ParseExact(txtfecha.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                actaEvaluacion.Observaciones = txtObservaciones.Text;
                actaEvaluacion.publicado = isPublicada;

                consultas.Db.SubmitChanges();

                if (GrvPlanesNegocio.Rows.Count != 0)
                {
                    //Recorremos los planes de negoción del acta
                    //Para actualizarlos
                    foreach (GridViewRow row in GrvPlanesNegocio.Rows)
                    {
                        Int32 idActa = Convert.ToInt32(lidacta.Text);
                        Int32 idProyecto = Convert.ToInt32((row.FindControl("lblidproyecto") as Label).Text);
                        var radioViable = row.FindControl("rdbViable") as RadioButtonList;

                        var proyectoEvaluacion = consultas.Db.EvaluacionActaProyectos.FirstOrDefault(a => a.CodActa == idActa && a.CodProyecto == idProyecto);

                        if (proyectoEvaluacion != null)
                        {
                            if (radioViable != null) proyectoEvaluacion.Viable = radioViable.SelectedValue == "1" ? true : false;
                            consultas.Db.SubmitChanges();
                        }

                        if (isPublicada)
                        {
                            var proyecto = consultas.Db.Proyecto.FirstOrDefault(p => p.Id_Proyecto == idProyecto);
                            if (proyecto == null)
                                throw new ApplicationException("No se pudo obtener la información del proyecto");

                            if (radioViable != null && radioViable.SelectedValue == "1")
                            {
                                proyecto.CodEstado = Constantes.CONST_AsignacionRecursos;
                                consultas.Db.SubmitChanges();
                                
                                var criterios = (from criteriosConvocatoria in consultas.Db.ConvocatoriaCriterioPriorizacions
                                                 join criteriosEvaluacion in consultas.Db.CriterioPriorizacions on criteriosConvocatoria.CodCriterioPriorizacion equals criteriosEvaluacion.Id_Criterio
                                                 where criteriosConvocatoria.CodConvocatoria == actaEvaluacion.CodConvocatoria.Value                                               
                                                 select new CriteriosParaEvaluacion {
                                                    Id = criteriosEvaluacion.Id_Criterio,
                                                    Nombre = criteriosEvaluacion.NomCriterio,
                                                    Sigla = criteriosEvaluacion.Sigla,
                                                    Query = criteriosEvaluacion.Query,
                                                    ModeloParametros = criteriosEvaluacion.Parametros,
                                                    Parametros = criteriosConvocatoria.Parametros,
                                                    Convocatoria = criteriosConvocatoria.CodConvocatoria,
                                                    Incidencia = criteriosConvocatoria.Incidencia
                                                 }).ToList();
                                
                                foreach (var criterio in criterios)
                                {
                                    
                                }

                                var puntajeTotal = consultas.Db
                                                   .PuntajeTotalPriorizacions
                                                   .SingleOrDefault(filter => filter.CodConvocatoria.Equals(actaEvaluacion.CodConvocatoria.Value)
                                                                               && filter.CodProyecto.Equals(proyecto.Id_Proyecto)
                                                                   );

                                if (puntajeTotal == null)
                                {
                                    PuntajeTotalPriorizacion nuevoPuntaje = new PuntajeTotalPriorizacion()
                                    {
                                        CodConvocatoria = actaEvaluacion.CodConvocatoria.Value,
                                        Total = 1,
                                        CodProyecto = proyecto.Id_Proyecto
                                    };

                                    consultas.Db.PuntajeTotalPriorizacions.InsertOnSubmit(nuevoPuntaje);
                                    consultas.Db.SubmitChanges();
                                }
                                else
                                {
                                    puntajeTotal.Total = 0;
                                    consultas.Db.SubmitChanges();
                                }                                
                            }
                            else
                            {
                                proyecto.CodEstado = Constantes.CONST_Inscripcion;
                                consultas.Db.TabProyectos.Where(p => p.CodProyecto == idProyecto).ToList().ForEach(filter =>
                                {
                                    filter.Realizado = false;
                                });
                                
                                var proyectoContacto = consultas.Db.ProyectoContactos.Where(p => p.CodProyecto == idProyecto
                                                                                            && p.Inactivo == true
                                                                                            && (p.CodRol == Constantes.CONST_RolEvaluador || p.CodRol == Constantes.CONST_RolCoordinadorEvaluador)).ToList();
                                proyectoContacto.ForEach(pc =>
                                {
                                    pc.FechaFin = DateTime.Now;
                                    pc.Inactivo = false;
                                });

                                consultas.Db.SubmitChanges();
                            }
                        }
                    }

                }

                // Si el acta esta publicada 
                // Ocultamos controles
                if (isPublicada)
                {
                    pnlNegocioPublico.Visible = false;
                    panelNegocioGrid.Visible = true;
                    DeshabilitarEdit(false);
                }
                else
                {
                    DdlCodConvocatoria.Enabled = false;
                    return;
                }
            }
            catch (ApplicationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, detalle : " + ex.Message.Replace("'"," ") + " ');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo. detalle : " + ex.Message.Replace("'", " ") + " ');", true);
            }

            btnupdate.Visible = false;
            btnimprimir.Visible = true;
            btnCrearActa.Visible = false;
        }

        /// <summary>
        /// Eliminar un proyecto de un acta de validación final de evaluación
        /// </summary>
        /// <param name="idacta"> Codigo de acta </param>
        /// <param name="codproyecto"> Codigo de proyecto </param>
        /// <returns> Mensaje de error </returns>
        [WebMethod]
        public static string EliminarProyecto(int idacta, int codproyecto)
        {
            try
            {
                string mensajeDeError;
                var consulta = new Datos.Consultas();

                var actaEvaluacion = consulta.Db.EvaluacionActas.FirstOrDefault(e => e.Id_Acta == idacta);

                if (actaEvaluacion == null)
                    throw new ApplicationException("No se pudo obtener el acta final de validación");

                if (actaEvaluacion.publicado.Value)
                    throw new ApplicationException("No se puede eliminar el proyecto porque el acta se encuentra publicada.");

                var proyectoEvaluacion = consulta.Db.EvaluacionActaProyectos.FirstOrDefault(p => p.CodActa == idacta && p.CodProyecto == codproyecto);

                if (proyectoEvaluacion != null)
                    throw new ApplicationException("No se pudo obtener la información del proyecto del acta final de validación");

                consulta.Db.EvaluacionActaProyectos.DeleteOnSubmit(proyectoEvaluacion);
                consulta.Db.SubmitChanges();
                mensajeDeError = "ok";

                return JsonConvert.SerializeObject(new
                {
                    mensaje = mensajeDeError
                });
            }
            catch (ApplicationException ex)
            {
                return JsonConvert.SerializeObject(new
                {
                    mensaje = "Sucedio un error, detalle : " + ex.Message
                });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new
                {
                    mensaje = "No se pudo eliminar el proyecto del acta"
                });
            }
        }

        /// <summary>
        /// Desabilitar controles de creación de acta final de evaluación
        /// </summary>       
        void DeshabilitarPanelCrear(bool bandera, bool panel)
        {
            if (panel)
            {
                pnlNegocioPublico.Visible = true;
                DdlCodConvocatoria.Enabled = bandera;
                panelNegocioGrid.Visible = true;
                btnCrearActa.Visible = false;
                btnimprimir.Visible = false;
                btnupdate.Visible = true;
            }
        }

        /// <summary>
        /// Desabilitar edición
        /// </summary>
        /// <param name="bandera"></param>
        void DeshabilitarEdit(bool bandera)
        {
            txtNroActa.Enabled = bandera;
            txtnomActa.Enabled = bandera;
            txtfecha.Enabled = bandera;
            txtObservaciones.Enabled = bandera;
            DdlCodConvocatoria.Enabled = bandera;
            btnupdate.Visible = false;
            btnDate2.Enabled = false;
            btnimprimir.Visible = true;
            btnCrearActa.Visible = false;
            panelNegocioGrid.Visible = true;
            CalendarExtender1.Enabled = false;

        }

        /// <summary>
        /// Crear acta final de evaluación
        /// </summary>        
        protected void btnCrearActa_Click(object sender, EventArgs e)
        {
            Crear();
        }
        /// <summary>
        /// Actualizar acta final de evaluación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnupdate_Click(object sender, EventArgs e)
        {
            Actualizar();
        }

        /// <summary>
        /// Event RowDataBpund gridview de planes de negocio
        /// </summary>
        protected void GrvPlanesNegocio_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var imgBorrar = e.Row.FindControl("imgborrar") as Image;
                var lnkBorrar = e.Row.FindControl("lnk_del") as LinkButton;
                LinkButton lnkProyecto = (LinkButton)e.Row.FindControl("lnkProyecto");
                var lblViable = e.Row.FindControl("lblviable") as Label;
                var radioViable = e.Row.FindControl("rdbViable") as RadioButtonList;
                var lblViableEvaluador = e.Row.FindControl("lblviableevaluador") as Label;

                //se crea un control para dar acceso denegado al los usuarios que no puedan ver proyectos
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_Interventor)
                {
                    lnkProyecto.PostBackUrl = "AccesoDenegado.aspx";
                }

                //if ((bool)ViewState["publico"])
                if (Boolean.Parse(actaPublicado))
                {
                    lnkBorrar.Visible = false;
                    imgBorrar.Visible = false;
                    radioViable.Enabled = false;
                }
                else
                {
                    if (usuario.CodGrupo != Constantes.CONST_GerenteInterventor && usuario.CodGrupo != Constantes.CONST_CoordinadorInterventor && usuario.CodGrupo != Constantes.CONST_Interventor)
                    {
                        radioViable.Text = string.Empty;
                    }
                    else
                    {
                        radioViable.Enabled = false;
                    }
                    if (editar == 1)
                    {
                        lnkBorrar.Visible = true;
                        imgBorrar.Visible = true;
                    }
                    else
                    {
                        lnkBorrar.Visible = false;
                        imgBorrar.Visible = false;
                    }
                }

                if (lblViable != null)
                {
                    if (lblViable.Text == "True")
                    {
                        radioViable.SelectedValue = "1";
                    }
                    else if (lblViableEvaluador.Text.Trim() == "SI" && lblViable.Text == "True")
                    {
                        radioViable.SelectedValue = "1";
                    }
                    else
                    {
                        radioViable.SelectedValue = "0";
                    }
                }
            }
        }

        /// <summary>
        /// Evento de RowCommand
        /// </summary>        
        protected void GrvPlanesNegocio_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "proyecto":
                        if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                        {
                            Response.Redirect("~/Fonade/evaluacion/AccesoDenegado.aspx");
                        }
                        else
                        {
                            HttpContext.Current.Session["CodProyecto"] = e.CommandArgument.ToString();
                            HttpContext.Current.Session["CodConvocatoria"] = DdlCodConvocatoria.SelectedValue;
                            Response.Redirect("EvaluacionFrameSet.aspx");
                        }                        
                        break;
                    case "evaluador":
                        HttpContext.Current.Session["codcontacto"] = e.CommandArgument;
                        Redirect(null, "VerPerfilContacto.aspx", "_blank", "menubar=0,scrollbars=1,width=710,height=400,top=100");
                        break;
                    case "eliminar":
                        var idacta = Convert.ToInt32(HttpContext.Current.Session["idacta"] ?? 0);
                        var codproyecto = Convert.ToInt32(e.CommandArgument);

                        var actaEvaluacion = consultas.Db.EvaluacionActas.FirstOrDefault(p => p.Id_Acta == idacta);

                        if (actaEvaluacion == null)
                            throw new ApplicationException("No se pudo obtener el acta final de validación");

                        if (actaEvaluacion.publicado.Value)
                            throw new ApplicationException("No se puede eliminar el proyecto porque el acta se encuentra publicada.");

                        var proyectoEvaluacion = consultas.Db.EvaluacionActaProyectos.FirstOrDefault(p => p.CodActa == idacta && p.CodProyecto == codproyecto);

                        if (proyectoEvaluacion == null)
                            throw new ApplicationException("No se pudo obtener la información del proyecto del acta final de validación");

                        consultas.Db.EvaluacionActaProyectos.DeleteOnSubmit(proyectoEvaluacion);
                        consultas.Db.SubmitChanges();

                        GrvPlanesNegocio.DataBind();

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert(' El proyecto fue eliminado satisfactoriamente. ');", true);
                        break;
                    default:
                        break;
                }
            }
            catch (ApplicationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error. detalle : " + ex.Message + " ');", true);
            }
        }

        /// <summary>
        /// Cambio de indice en grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrvPlanesNegocio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrvPlanesNegocio.PageIndex = e.NewPageIndex;
        }

        /// <summary>
        /// Agregar nuevo plan de negocio al acta
        /// </summary>
        protected void lnkadcionarplan_Click(object sender, EventArgs e)
        {
            try
            {
                HttpContext.Current.Session["CodConvocatoria"] = DdlCodConvocatoria.SelectedValue;
                HttpContext.Current.Session["CodActa"] = txtNroActa.Text;
                HttpContext.Current.Session["idacta"].ToString();
                Redirect(null, "AdicionarProyectoActa.aspx", "_Blank", "width=730,height=585");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error al intentar agregar un nuevo proyecto al acta, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }
        }

        /// <summary>
        /// Agregar nuevo plan de negocio al acta
        /// </summary>        
        protected void imgadicionarplan_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                HttpContext.Current.Session["CodConvocatoria"] = DdlCodConvocatoria.SelectedValue;
                HttpContext.Current.Session["CodActa"] = txtNroActa.Text;
                Redirect(null, "AdicionarProyectoActa.aspx", "_Blank", "width=730,height=585");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error al intentar agregar un nuevo proyecto al acta, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }

        }

        /// <summary>
        /// Obtener planes de negocio de evaluación del acta 
        /// </summary>
        protected void lds_planesnegocio_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            try
            {
                int idActaEval = -1;

                idActaEval = Convert.ToInt32(HttpContext.Current.Session["idacta"] ?? 0);

                var proyectoDeEvaluacion = (from p in consultas.Db.pr_ProyectosEvaluados(idActaEval, 0) select p).ToList();

                e.Result = proyectoDeEvaluacion;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error al cargar los planes de negocio del acta, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }
        }

        /// <summary>
        /// Obtener todas las convocatorias
        /// </summary>
        protected void lds_convocatoria_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var convocatoria = from c in consultas.Db.Convocatoria
                               where c.codOperador == usuario.CodOperador
                               orderby c.NomConvocatoria
                               select new
                               {
                                   Id = c.Id_Convocatoria,
                                   Nombre = c.NomConvocatoria
                               };
            e.Result = convocatoria.ToList();
        }
    }

    public class CriteriosParaEvaluacion {
        public short Id { get; set; }
        public string Nombre { get; set; }
        public string Sigla { get; set; }
        public string Query { get; set; }
        public string ModeloParametros { get; set; }
        public string Parametros { get; set; }
        public int Convocatoria { get; set; }
        public double Incidencia { get; set; }
    }


}