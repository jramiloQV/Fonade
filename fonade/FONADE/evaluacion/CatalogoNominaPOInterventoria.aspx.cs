using Datos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Fonade.Negocio;
using System.Globalization;
using Fonade.Clases;
using Fonade.Negocio.PlanDeNegocioV2.Interventoria;

namespace Fonade.FONADE.evaluacion
{
    /// <summary>
    /// CatalogoNominaPOInterventoria
    /// </summary>
    
    public partial class CatalogoNominaPOInterventoria : Base_Page
    {
        int CodProyecto;
        //String codActividad;
        //String NombreDeLaActividad; //Valor que debe enviarse cuando se crea una nuevo registro, se usa para la notificación en tareas.
        //String CodUsuario;
        //String CodGrupo;
        //int CodCargo;
        String CodNomina;
        int Mes;
        String NombreDelCargo; //Este valor se le debe pasar por RowCommand del GridView donde se esté invocando.
        
        //int v_CodTipoFinanciacion = 0;
        //Int32 txtTab = Constantes.CONST_SubPlanOperativo;
        //String txtSQL;

        static string _codCargo { get; set; } static string _mes { get; set; } static string _codcontacto { get; set; } static string _url { get; set; }
        static string _codigoformato { get; set; } static string _fecha { get; set; } static string _borrado { get; set; } static string _comentario { get; set; }
        static string _codtipointv { get; set; } static string _nombre { get; set; } static bool _trsct { get; set; } string accion { get; set; }

        #region Eventos

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <exception cref="ApplicationException">
        /// No se pudo obtener el codigo del proyecto, intentenlo de nuevo.
        /// or
        /// No se pudo obtener el tipo de accion requerida, intentenlo de nuevo.
        /// or
        /// No se pudo obtener el mes de la nomina requerido, intentenlo de nuevo.
        /// or
        /// No se pudo obtener el Id de la nomina requerido, intentenlo de nuevo.
        /// or
        /// No se pudo obtener el nombre del cargo requerido, intentenlo de nuevo.
        /// </exception>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["CodProyecto"] == null)
                    throw new ApplicationException("No se pudo obtener el codigo del proyecto, intentenlo de nuevo.");
                if (Session["Accion"] == null)
                    throw new ApplicationException("No se pudo obtener el tipo de accion requerida, intentenlo de nuevo.");
                if (Session["MesDeLaNomina"] == null)
                    throw new ApplicationException("No se pudo obtener el mes de la nomina requerido, intentenlo de nuevo.");
                if (Session["idAnexoNomina"] == null)
                    throw new ApplicationException("No se pudo obtener el Id de la nomina requerido, intentenlo de nuevo.");
                if (Session["NombreDelCargo"] == null)
                    throw new ApplicationException("No se pudo obtener el nombre del cargo requerido, intentenlo de nuevo.");

                CodProyecto = Convert.ToInt32(Session["CodProyecto"]);
                Mes = Convert.ToInt32(Session["MesDeLaNomina"]);
                CodNomina = Session["idAnexoNomina"].ToString();
                NombreDelCargo = Session["NombreDelCargo"].ToString();
                var accion = Session["Accion"].ToString();

                if (!Page.IsPostBack)
                {
                    if (!string.IsNullOrEmpty(accion))
                    {
                        CargarCombo();
                        MostrarControles(accion);

                        //cargarHistorial
                        cargarHistorial((int.Parse(CodNomina)), Mes);

                        permisosAprobar(usuario.CodGrupo);
                    }
                }

                if (Session["datosNomina"] != null)
                {
                    var datosform = (string[])Session["datosNomina"];

                    txt_observaciones.Text = datosform[0];
                    txt_sueldo_obtenido.Text = datosform[1];
                    txt_prestaciones_obtenidas.Text = datosform[2];
                    txt_observ_interventor.Text = datosform[3];
                    Session["datosNomina"] = null;
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

        private void permisosAprobar(int _codGrupo)
        {
            if(_codGrupo != Constantes.CONST_Interventor)
            {
                txt_observ_interventor.Enabled = false;
                dd_aprobado.Enabled = false;

                if (_codGrupo == Constantes.CONST_Asesor)
                {
                    B_Acion.Visible = false;
                    B_Cancelar.Visible = false;
                }
            }
        }

        private void cargarHistorial(int _codActividad, int _codMes)
        {
            var historial = AvancesInterventoriaReg.GetHistoricoAvancesNomina(_codActividad, _codMes);

            gvHistoricoAvances.DataSource = historial;
            gvHistoricoAvances.DataBind();
        }

        /// <summary>
        /// Handles the Click event of the B_Acion control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void B_Acion_Click(object sender, EventArgs e)
        {
            Metodos();
            Session["idAnexoNomina"] = null;
            Session["MesDeLaNomina"] = null;
            Session["Archivos"] = null;
        }

        /// <summary>
        /// Handles the Click event of the img_btn_NuevoDocumento control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ImageClickEventArgs"/> instance containing the event data.</param>
        protected void img_btn_NuevoDocumento_Click(object sender, ImageClickEventArgs e)
        {
            HttpContext.Current.Session["Accion_Docs"] = img_btn_NuevoDocumento.CommandName;

            string[] datosform = new string[4];
            datosform[0] = txt_observaciones.Text;
            datosform[1] = txt_sueldo_obtenido.Text;
            datosform[2] = txt_prestaciones_obtenidas.Text;
            datosform[3] = txt_observ_interventor.Text;
            Session["datosNomina"] = datosform;

            Redirect(null, "../interventoria/CatalogoNominaInter.aspx", "_self", "menubar=0,scrollbars=1,width=710,height=400,top=100");
        }

        /// <summary>
        /// Handles the Click event of the img_btn_enlazar_grilla_PDF control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ImageClickEventArgs"/> instance containing the event data.</param>
        protected void img_btn_enlazar_grilla_PDF_Click(object sender, ImageClickEventArgs e)
        {
            HttpContext.Current.Session["Accion_Docs"] = "Lista";

            string[] datosform = new string[4];
            datosform[0] = txt_observaciones.Text;
            datosform[1] = txt_sueldo_obtenido.Text;
            datosform[2] = txt_prestaciones_obtenidas.Text;
            datosform[3] = txt_observ_interventor.Text;
            Session["datosNomina"] = datosform;

            Redirect(null, "../interventoria/CatalogoNominaInter.aspx", "_self", "menubar=0,scrollbars=1,width=710,height=400,top=100");
        }
        #endregion

        #region Metodos
        private void CargarCombo()
        {
            dd_aprobado.Items.Insert(0, new ListItem("No", "0"));
            dd_aprobado.Items.Insert(1, new ListItem("Si", "1"));
            dd_aprobado.SelectedValue = "0";
        }

        private void MostrarControles(string accion)
        {
            lbl_Interventor.Text = usuario.Nombres + " " + usuario.Apellidos;
            var fecha = DateTime.Now;
            var sMes = fecha.ToString("MMM", CultureInfo.CreateSpecificCulture("es-CO"));
            lbl_tiempo.Text = UppercaseFirst(sMes) + " " + fecha.Day + " de " + fecha.Year;
            lbl_tipoReq_Enunciado.Text = "REQUERIMIENTOS DE RECURSOS POR MES: " + Mes;
            txt_mes.Text = Mes.ToString();
            txt_cargo.Text = NombreDelCargo;
            switch (accion)
            {
                case "Reportar":
                    lbl_enunciado.Text = "Nuevo avance";
                    B_Acion.Text = "Crear";
                    Label4.Visible = false;
                    txt_observ_interventor.Visible = false;
                    Label5.Visible = false;
                    img_btn_NuevoDocumento.Visible = true;
                    dd_aprobado.Visible = false;
                    Session["Archivos"] = "Viene";
					txtFechaAvance.Text = DateTime.Now.ToString("dd/MM/yyyy");
					txtFechaAprobacion.Visible = false;
					break;
                case "editar":
                    lbl_enunciado.Text = "Editar avance";
                    B_Acion.Text = "Actualizar";
                    Label4.Visible = true;
                    txt_observ_interventor.Visible = true;
                    txt_observ_interventor.Enabled = false;
                    if (usuario.CodGrupo == Constantes.CONST_Emprendedor) img_btn_NuevoDocumento.Visible = true;
                    TraerDatos(int.Parse(CodNomina));

                    break;
                case "actualizar":
                    if (string.IsNullOrEmpty(B_Acion.Text) && Session["Archivos"] != null)
                    {
                        lbl_enunciado.Text = "Nuevo avance";
                        B_Acion.Text = "Crear";
                        Label4.Visible = false;
                        txt_observ_interventor.Visible = false;
                        Label5.Visible = false;
                        img_btn_NuevoDocumento.Visible = true;
                        dd_aprobado.Visible = false;
                        TraerDatos(int.Parse(CodNomina));
                    }
                    else
                    {
                        txt_observaciones.Enabled = false;
                        txt_sueldo_obtenido.Enabled = false;
                        txt_prestaciones_obtenidas.Enabled = false;
                        img_btn_NuevoDocumento.Visible = false;
                        B_Acion.Text = "Actualizar";
                        TraerDatos(int.Parse(CodNomina));
                    }
                    break;
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

        private void Metodos()
        {
            //var parametros = Session["CodActividad2"].ToString().Split(';');
            switch (B_Acion.Text)
            {
                case "Crear":
                    var mensaje = Validar(B_Acion.Text);
                    if (string.IsNullOrEmpty(mensaje))
                    {
                        var consultar = (from an in consultas.Db.AvanceCargoPOMes
                                         where an.CodCargo == int.Parse(CodNomina) && an.Mes == Mes
                                         select an).ToList();

                        if (consultar.Count == 0)
                        {
                            var avance = new AvanceCargoPOMes
                            {
                                CodCargo = int.Parse(CodNomina),
                                Mes = Convert.ToByte(Mes),
                                CodTipoFinanciacion = 1,
                                Valor = decimal.Parse((!string.IsNullOrEmpty(txt_sueldo_obtenido.Text)) ? txt_sueldo_obtenido.Text : "0"),
                                Observaciones = txt_observaciones.Text,
                                CodContacto = usuario.IdContacto,
                                ObservacionesInterventor = "",
                                Aprobada = false,
								FechaAvance = DateTime.Now
							};

                            consultas.Db.AvanceCargoPOMes.InsertOnSubmit(avance);
                            consultas.Db.SubmitChanges();

                            var avance2 = new AvanceCargoPOMes
                            {
                                CodCargo = int.Parse(CodNomina),
                                Mes = Convert.ToByte(Mes),
                                CodTipoFinanciacion = 2,
                                Valor = decimal.Parse((!string.IsNullOrEmpty(txt_prestaciones_obtenidas.Text)) ? txt_prestaciones_obtenidas.Text : "0"),
                                Observaciones = txt_observaciones.Text,
                                CodContacto = usuario.IdContacto,
                                ObservacionesInterventor = "",
                                Aprobada = false,
								FechaAvance = DateTime.Now
							};

                            consultas.Db.AvanceCargoPOMes.InsertOnSubmit(avance2);
                            consultas.Db.SubmitChanges();

                            //Consultar Id Interventor para agendar tarea
                            var datos = (from ei in consultas.Db.EmpresaInterventors
                                         join ee in consultas.Db.Empresas on ei.CodEmpresa equals ee.id_empresa
                                         join p in consultas.Db.Proyecto on ee.codproyecto equals p.Id_Proyecto
                                         where ee.codproyecto == CodProyecto && ei.Inactivo == false
                                         select new datosAgendar
                                         {
                                             idContacto = (int)ei.CodContacto,
                                             idProyecto = (int)p.Id_Proyecto,
                                             nombre = p.NomProyecto
                                         }).ToList();

                            //Insertar Historico
                            insertarHistorico(avance, avance2.Valor, CodProyecto);

                            var asunto = "Revisar Actividad de Nómina. Se ha creado una actividad.";
                            TareaAgendar(int.Parse(CodNomina), CodProyecto, usuario.IdContacto, datos[0].idContacto, datos[0].nombre, NombreDelCargo,
                                asunto, txt_observaciones.Text.Trim());
                        }
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Avance registrado.'); window.close();", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + mensaje + "');", true);
                    }
                    break;
                case "Actualizar":
                    if (usuario.CodGrupo == Constantes.CONST_Emprendedor)
                    {
                        mensaje = Validar(B_Acion.Text);
                        if (string.IsNullOrEmpty(mensaje))
                        {
                            var avancesNomina = (from an in consultas.Db.AvanceCargoPOMes
                                                    where an.CodCargo == int.Parse(CodNomina) && an.Mes == Mes
                                                    select an).ToList();

                            foreach (var avance in avancesNomina)
                            {
								avance.FechaAvance = DateTime.Now;
								avance.Observaciones = txt_observaciones.Text.Trim();
                                avance.Valor = (avance.CodTipoFinanciacion == 1) ? decimal.Parse((!string.IsNullOrEmpty(txt_sueldo_obtenido.Text)) ? txt_sueldo_obtenido.Text : "0") : decimal.Parse((!string.IsNullOrEmpty(txt_prestaciones_obtenidas.Text)) ? txt_prestaciones_obtenidas.Text : "0");
                            }

                            consultas.Db.SubmitChanges();

                            //Consultar Id Interventor para agendar tarea
                            var datos = (from ei in consultas.Db.EmpresaInterventors
                                         join ee in consultas.Db.Empresas on ei.CodEmpresa equals ee.id_empresa
                                         join p in consultas.Db.Proyecto on ee.codproyecto equals p.Id_Proyecto
                                         where ee.codproyecto == CodProyecto && ei.Inactivo == false
                                         select new datosAgendar
                                         {
                                             idContacto = (int)ei.CodContacto,
                                             idProyecto = (int)p.Id_Proyecto,
                                             nombre = p.NomProyecto
                                         }).ToList();

                            decimal valorPrestaciones = decimal.Parse((!string.IsNullOrEmpty(txt_prestaciones_obtenidas.Text)) ? txt_prestaciones_obtenidas.Text.Replace(".", ",") : "0");

                            var avanceHis = avancesNomina.Where(x => x.CodTipoFinanciacion == 1).FirstOrDefault();

                            //Insertar Historico
                            insertarHistorico(avanceHis, valorPrestaciones, CodProyecto);

                            var asunto = "Revisar Actividad de nomina. Se ha modificado una actividad.";
                            TareaAgendar(int.Parse(CodNomina), CodProyecto, usuario.IdContacto, datos[0].idContacto, datos[0].nombre, NombreDelCargo,
                                asunto, txt_observaciones.Text.Trim());

                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Información de nomina procesada correctamente.'); window.close();", true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + mensaje + "');", true);
                        }
                    }
                    else
                    {
                        mensaje = Validar(B_Acion.Text);
                        if (string.IsNullOrEmpty(mensaje))
                        {
                            var avancesNomina = (from an in consultas.Db.AvanceCargoPOMes
                                                    where an.CodCargo == int.Parse(CodNomina) && an.Mes == Mes
                                                    select an).ToList();

                            foreach (var avance in avancesNomina)
                            {
								avance.FechaAprobacion = DateTime.Now;
								avance.ObservacionesInterventor = txt_observ_interventor.Text.Trim();
                                avance.Aprobada = (dd_aprobado.SelectedValue == "0") ? false : true;
                            }
                            consultas.Db.SubmitChanges();

                            //consulta Id del emprendedor para agendar tarea
                            var datos = (from pc in consultas.Db.ProyectoContactos
                                         join p in consultas.Db.Proyecto on pc.CodProyecto equals p.Id_Proyecto
                                         where pc.CodProyecto == CodProyecto && pc.CodRol == 3
                                         select new datosAgendar
                                         {
                                             idContacto = pc.CodContacto,
                                             idProyecto = p.Id_Proyecto,
                                             nombre = p.NomProyecto
                                         }).ToList();


                            decimal valorPrestaciones = decimal.Parse((!string.IsNullOrEmpty(txt_prestaciones_obtenidas.Text)) ? txt_prestaciones_obtenidas.Text.Replace(".", ",") : "0");

                            var avanceHis = avancesNomina.Where(x => x.CodTipoFinanciacion == 1).FirstOrDefault();
                            //Insertar Historico
                            insertarHistorico(avanceHis, valorPrestaciones, CodProyecto);

                            var asunto = "Revisar Actividad de nomina. Se ha modificado una actividad.";
                            TareaAgendar(int.Parse(CodNomina), CodProyecto, usuario.IdContacto, datos[0].idContacto, datos[0].nombre, NombreDelCargo,
                                asunto, txt_observaciones.Text.Trim());

                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Información de nomina procesada correctamente.'); window.close();", true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + mensaje + "');", true);
                        }
                    }
                    break;
            }


        }

        private void insertarHistorico(AvanceCargoPOMes avance, decimal _prestaciones, int _CodProyecto)
        {
            HistoricoAvanceModel avanceHis = new HistoricoAvanceModel
            {
                codActividad = avance.CodCargo,
                codContacto = usuario.IdContacto,
                FechaAvanceEmprendedor = avance.FechaAvance,
                Mes = avance.Mes,
                ValorSueldo = avance.Valor,
                ValorPrestaciones = _prestaciones,
                fechaRegistro = DateTime.Now,
                ObservacionEmprendedor = avance.Observaciones,
                ObservacionInterventor = avance.ObservacionesInterventor,
                FechaAvanceInterventor = avance.FechaAprobacion,
                Aprobada = avance.Aprobada,
                codProyecto = _CodProyecto
            };

            AvancesInterventoriaReg.insertarHistoricoNomina(avanceHis);
        }

        private void TareaAgendar(int codActivdad, int codProyecto, int agendo, int paraQuien, string nomProyecto, string nomActividad, string asunto, string detalle)
        {
            AgendarTarea agenda = new AgendarTarea
                            (paraQuien,
                            asunto,
                            "Revisar la actividad de Nómina " + nomProyecto + " - Actividad --> " + nomActividad + " Observaciones: " + detalle.Trim(),
                            codProyecto.ToString(),
                            2,
                            "0",
                            true,
                            1,
                            true,
                            false,
                            agendo,
                            "CodProyecto=" + codProyecto.ToString(),
                            "",
                            "Catálogo Actividad Nomina");

            agenda.Agendar();
        }

        private string Validar(string accion)
        {
            var mensaje = string.Empty;
            switch (accion)
            {
                case "Crear":
                    if (string.IsNullOrEmpty(txt_observaciones.Text.Trim()))
                    {
                        mensaje = "Debe ingresar una observación";
                    }
                    break;
                case "actualizar":
                    if (string.IsNullOrEmpty(txt_observaciones.Text.Trim()))
                    {
                        mensaje = "Debe ingresar una observación";
                    }
                    break;
                case "Actualizar":
                    if (usuario.CodGrupo == Constantes.CONST_Interventor)
                    {
                        if (string.IsNullOrEmpty(txt_observ_interventor.Text.Trim()))
                        {
                            mensaje = "Debe ingresar una observación";
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(txt_observaciones.Text.Trim()))
                        {
                            mensaje = "Debe ingresar una observación";
                        }
                    }
                    break;
            }

            return mensaje;
        }

        private void TraerDatos(int codActividad)
        {
            var avanceNomina = (from an in consultas.Db.AvanceCargoPOMes
                                   where an.CodCargo == codActividad && an.Mes == Mes
                                   select an).ToList();

            if (avanceNomina.Count() > 0)
            {
                foreach (var actividad in avanceNomina)
                {
                    txt_observaciones.Text = actividad.Observaciones;
                    txt_observ_interventor.Text = actividad.ObservacionesInterventor;
					txtFechaAvance.Text = actividad.FechaAvance.HasValue ?
										actividad.FechaAvance.Value.ToShortDateString() : "";

					txtFechaAprobacion.Text = actividad.FechaAprobacion.HasValue ?
										actividad.FechaAprobacion.Value.ToShortDateString() : "";

					dd_aprobado.SelectedValue = (Convert.ToBoolean(actividad.Aprobada).ToString() == "true") ? "1" : "0";
                    if (Convert.ToInt32(actividad.CodTipoFinanciacion) == 1)
                    {
                        txt_sueldo_obtenido.Text = Convert.ToInt32(actividad.Valor).ToString();
                    }
                    else
                    {
                        txt_prestaciones_obtenidas.Text = Convert.ToInt32(actividad.Valor).ToString();
                    }
                }

                if (Convert.ToBoolean(avanceNomina[0].Aprobada))
                {
                    txt_observaciones.Enabled = false;
                    txt_observ_interventor.Enabled = false;
                    dd_aprobado.Enabled = false;
                    txt_sueldo_obtenido.Enabled = false;
                    txt_prestaciones_obtenidas.Enabled = false;
                    B_Acion.Visible = false;
                    img_btn_NuevoDocumento.Visible = false;
                    dd_aprobado.SelectedValue = "1";
                    if (usuario.CodGrupo == Constantes.CONST_Interventor)
                    {
                        dd_aprobado.Enabled = true;
                        txt_observ_interventor.Enabled = true;
                        B_Acion.Visible = true;
                    }
                }
                else
                {
                    if (usuario.CodGrupo == Constantes.CONST_Emprendedor)
                    {
                        txt_observaciones.Enabled = true;
                        txt_observ_interventor.Enabled = false;
                        txt_sueldo_obtenido.Enabled = true;
                        txt_prestaciones_obtenidas.Enabled = true;
                        dd_aprobado.Enabled = false;
                    }
                    else
                    {
                        txt_observaciones.Enabled = false;
                        txt_observ_interventor.Enabled = true;
                        txt_sueldo_obtenido.Enabled = false;
                        txt_prestaciones_obtenidas.Enabled = false;
                        dd_aprobado.Enabled = true;
                    }
                }
            }
            else
            {
                lbl_enunciado.Text = "Nuevo avance";
                B_Acion.Text = "Crear";
                Label4.Visible = false;
                txt_observ_interventor.Visible = false;
                Label5.Visible = false;
                dd_aprobado.Visible = false;
                img_btn_NuevoDocumento.Visible = true;
            }

        }

        #endregion

        /// <summary>
        /// Handles the Click event of the B_Cancelar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void B_Cancelar_Click(object sender, EventArgs e)
        {
            Session["idAnexoNomina"] = null;
            Session["MesDeLaNomina"] = null;
            Session["Archivos"] = null;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Ventana", "window.close();", true);
        }


    }
}