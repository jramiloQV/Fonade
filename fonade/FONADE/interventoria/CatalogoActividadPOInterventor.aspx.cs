using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Globalization;
using Fonade.Clases;
using Fonade.Negocio.PlanDeNegocioV2.Interventoria;

namespace Fonade.FONADE.interventoria
{
    public partial class CatalogoActividadPOInterventor : Negocio.Base_Page
    {
        //variables 
        private string conexionstr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        int CodProyecto;
        int CodActividad;
        int Mes;
        String NombreInterventor;
        string pagina, accion;
        /// <summary>
        /// Codigo de contacto
        /// </summary>
        protected int CodContacto;
        String txtSQL;
        int txtTab = Constantes.CONST_PlanOperativoInter2;

        static string _Mes { get; set; }


        #region Eventos

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <exception cref="ApplicationException">
        /// No se pudo obtener el codigo del proyecto, intentenlo de nuevo.
        /// or
        /// No se pudo obtener numero del mes, intentenlo de nuevo.
        /// or
        /// No se pudo obtener el tipo de accion requerida, intentenlo de nuevo.
        /// or
        /// No se pudo obtener el tipo de accion requerida, intentenlo de nuevo.
        /// </exception>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {                
                if (Session["CodProyecto"] == null)
                    throw new ApplicationException("No se pudo obtener el codigo del proyecto, intentenlo de nuevo.");
                if (Session["linkid"] == null)
                    throw new ApplicationException("No se pudo obtener numero del mes, intentenlo de nuevo.");
                if (Session["Accion"] == null)
                    throw new ApplicationException("No se pudo obtener el tipo de accion requerida, intentenlo de nuevo.");
                if (Session["CodActividad"] == null)
                    throw new ApplicationException("No se pudo obtener el tipo de accion requerida, intentenlo de nuevo.");

                CodProyecto = Convert.ToInt32(Request.QueryString["prj"]); //Convert.ToInt32(Session["CodProyecto"]);
                CodActividad = Convert.ToInt32(Request.QueryString["act"]);
                Mes = Convert.ToInt32(Request.QueryString["mes"]); //Convert.ToInt32(Session["linkid"]);
                accion = Request.QueryString["do"]; // Session["Accion"].ToString();

                if (!Page.IsPostBack)
                {                    
                    if (!string.IsNullOrEmpty(accion))
                    {
                        CargarCombo();
                        MostrarControles(accion);

                        //cargarHistorial
                        cargarHistorial(CodActividad, Mes);
                    }
                }

                if (Session["datosForm"] != null)
                {
                    var datosform = (string[])Session["datosForm"];

                    txt_observaciones.Text = datosform[0];
                    txt_sueldo_obtenido.Text = datosform[1];
                    txt_prestaciones_obtenidas.Text = datosform[2];
                    txt_observ_interventor.Text =datosform[3];
                    Session["datosForm"] = null;
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

        private void cargarHistorial(int _codActividad, int _codMes)
        {
            var historial = AvancesInterventoriaReg.GetHistoricoAvancesPlanOperativo(_codActividad, _codMes);

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
            Session["Accion"] = null;
            Session["CodActividad"] = null;
            Session["pagina"] = null;
        }

        /// <summary>
        /// Handles the Click event of the img_btn_NuevoDocumento control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ImageClickEventArgs"/> instance containing the event data.</param>
        protected void img_btn_NuevoDocumento_Click(object sender, ImageClickEventArgs e)
        {
            string[] datosform = new string[4];
            datosform[0] = txt_observaciones.Text;
            datosform[1] = txt_sueldo_obtenido.Text;
            datosform[2] = txt_prestaciones_obtenidas.Text;
            datosform[3] = txt_observ_interventor.Text;

			Session["datosForm"] = datosform;
            Session["Accion_Docs"] = img_btn_NuevoDocumento.CommandName;
            Redirect(null, "CatalogoDocumentoInterventoria.aspx?do=" + accion + "&prj=" + CodProyecto + "&act=" + CodActividad + "&mes=" + Mes, "_self", "menubar=0,scrollbars=1,width=710,height=400,top=100");
        }

        /// <summary>
        /// Handles the Click event of the img_btn_enlazar_grilla_PDF control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ImageClickEventArgs"/> instance containing the event data.</param>
        protected void img_btn_enlazar_grilla_PDF_Click(object sender, ImageClickEventArgs e)
        {
            string[] datosform = new string[4];
            datosform[0] = txt_observaciones.Text;
            datosform[1] = txt_sueldo_obtenido.Text;
            datosform[2] = txt_prestaciones_obtenidas.Text;
            datosform[3] = txt_observ_interventor.Text;

            Session["datosForm"] = datosform;
            Session["Accion_Docs"] = img_btn_enlazar_grilla_PDF.CommandName;
            Redirect(null, "CatalogoDocumentoInterventoria.aspx?do=" + accion + "&prj=" + CodProyecto + "&act=" + CodActividad + "&mes=" + Mes+"&lock="+ (dd_aprobado.SelectedValue.Equals("1") ? "true" : "false") , "_self", "menubar=0,scrollbars=1,width=710,height=400,top=100");
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
            var nomActividad = Session["CodActividad2"].ToString().Split(';');
            txt_cargo.Text = nomActividad[1];
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
                    TraerDatos(int.Parse(nomActividad[0]));
                    break;
                case "actualizar":
                    txt_observaciones.Enabled = false;
                    txt_sueldo_obtenido.Enabled = false;
                    txt_prestaciones_obtenidas.Enabled = false;
                    TraerDatos(int.Parse(nomActividad[0]));
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
            var parametros = Session["CodActividad2"].ToString().Split(';');
            switch(B_Acion.Text)
            {
                case "Crear":
                    var mensaje = Validar(B_Acion.Text);
                    if (string.IsNullOrEmpty(mensaje))
                    {
                        var consultar = (from aa in consultas.Db.AvanceActividadPOMes
                                         where aa.CodActividad == CodActividad && aa.Mes == Mes //int.Parse(parametros[0])
                                         select aa).ToList();

                        if(consultar.Count == 0)
                        {
							var avance = new AvanceActividadPOMes
							{
								CodActividad = CodActividad,
								Mes = Convert.ToByte(Mes),
								CodTipoFinanciacion = 1,
								Valor = decimal.Parse((!string.IsNullOrEmpty(txt_sueldo_obtenido.Text.Replace(".", ","))) ? txt_sueldo_obtenido.Text : "0"),
								Observaciones = txt_observaciones.Text,
								CodContacto = usuario.IdContacto,
								ObservacionesInterventor = "",
								Aprobada = false,
								FechaAvance = DateTime.Now
                            };

                            consultas.Db.AvanceActividadPOMes.InsertOnSubmit(avance);
                            consultas.Db.SubmitChanges();

                            var avance2 = new AvanceActividadPOMes
                            {
                                CodActividad = CodActividad, // int.Parse(parametros[0]),
                                Mes = Convert.ToByte(Mes),
                                CodTipoFinanciacion = 2,
                                Valor = decimal.Parse((!string.IsNullOrEmpty(txt_prestaciones_obtenidas.Text)) ? txt_prestaciones_obtenidas.Text.Replace(".", ",") : "0"),
                                Observaciones = txt_observaciones.Text,
                                CodContacto = usuario.IdContacto,
                                ObservacionesInterventor = "",
                                Aprobada = false,
								FechaAvance = DateTime.Now
                            };

                            consultas.Db.AvanceActividadPOMes.InsertOnSubmit(avance2);
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

                            var asunto = "Revisar Actividad del Plan Operativo. Se ha creado una actividad.";
                            TareaAgendar(int.Parse(parametros[0]), CodProyecto, usuario.IdContacto, datos[0].idContacto, datos[0].nombre, parametros[1],
                                asunto, txt_observaciones.Text.Trim());
                        }
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Avance registrado.'); window.close();", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('"+ mensaje  +"');", true);
                    }
                    break;
                case "Actualizar":
                    if(usuario.CodGrupo == Constantes.CONST_Emprendedor)
                    {
                        mensaje = Validar(B_Acion.Text);
                        if (string.IsNullOrEmpty(mensaje))
                        {
                            var avancesActividad = (from aa in consultas.Db.AvanceActividadPOMes
                                                    where aa.CodActividad == CodActividad && aa.Mes == Mes //int.Parse(parametros[0])
                                                    select aa).ToList();

                            foreach(var avance in avancesActividad)
                            {
								avance.FechaAvance = DateTime.Now;
                                avance.Observaciones = txt_observaciones.Text.Trim();
                                avance.Valor = (avance.CodTipoFinanciacion == 1) ? decimal.Parse((!string.IsNullOrEmpty(txt_sueldo_obtenido.Text.Replace(",00", "").Replace(".00", "").Replace(".", ","))) ? txt_sueldo_obtenido.Text.Replace(",00", "").Replace(".00", "").Replace(".", ",") : "0") : decimal.Parse((!string.IsNullOrEmpty(txt_prestaciones_obtenidas.Text.Replace(",00", "").Replace(".00", "").Replace(".", ","))) ? txt_prestaciones_obtenidas.Text.Replace(",00", "").Replace(".00", "").Replace(".", ",") : "0");
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

                            decimal valorEmprendedor = decimal.Parse((!string.IsNullOrEmpty(txt_prestaciones_obtenidas.Text)) ? txt_prestaciones_obtenidas.Text.Replace(".", ",") : "0");

                            var avanceHis = avancesActividad.Where(x => x.CodTipoFinanciacion == 1).FirstOrDefault();

                            //Insertar Historico
                            insertarHistorico(avanceHis, valorEmprendedor, CodProyecto);


                            var asunto = "Revisar Actividad del Plan Operativo. Se ha modificado una actividad.";
                            TareaAgendar(int.Parse(parametros[0]), CodProyecto, usuario.IdContacto, datos[0].idContacto, datos[0].nombre, parametros[1],
                                asunto, txt_observaciones.Text.Trim());

                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Información de Plan Operativo procesada correctamente.'); window.close();", true);
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
                            var avancesActividad = (from aa in consultas.Db.AvanceActividadPOMes
                                                    where aa.CodActividad == CodActividad && aa.Mes == Mes //int.Parse(parametros[0])
                                                    select aa).ToList();

                            foreach(var avance in avancesActividad)
                            {
								avance.FechaAprobacion = DateTime.Now;
                                avance.ObservacionesInterventor = txt_observ_interventor.Text.Trim();
                                avance.Aprobada = (dd_aprobado.SelectedValue == "0") ? false : true;
                            }
                            consultas.Db.SubmitChanges();

                            //consulta Id del emprendedor para agendar tarea
                            var datos = (from pc in consultas.Db.ProyectoContactos
                                         join p in consultas.Db.Proyecto on pc.CodProyecto equals p.Id_Proyecto
                                         where pc.CodProyecto == CodProyecto && pc.CodRol == 3 && pc.FechaFin == null
                                         select new datosAgendar
                                         {
                                             idContacto = pc.CodContacto,
                                             idProyecto = p.Id_Proyecto,
                                             nombre = p.NomProyecto
                                         }).ToList();

                            decimal valorEmprendedor = decimal.Parse((!string.IsNullOrEmpty(txt_prestaciones_obtenidas.Text)) ? txt_prestaciones_obtenidas.Text.Replace(".", ",") : "0");

                            var avanceHis = avancesActividad.Where(x => x.CodTipoFinanciacion == 1).FirstOrDefault();

                            //Insertar Historico
                            insertarHistorico(avanceHis, valorEmprendedor, CodProyecto);

                            var asunto = "Revisar Actividad del Plan Operativo. Se ha modificado una actividad.";
                            TareaAgendar(int.Parse(parametros[0]), CodProyecto, usuario.IdContacto, datos[0].idContacto, datos[0].nombre, parametros[1],
                                asunto, txt_observaciones.Text.Trim());

                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Información de Plan Operativo procesada correctamente.'); window.close();", true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + mensaje + "');", true);
                        }
                    }
                    break;
            }


        }

        private void insertarHistorico(AvanceActividadPOMes avance, decimal aporteEmprendedor, int _CodProyecto)
        {
            HistoricoAvanceModel avanceHis = new HistoricoAvanceModel
            {
                codActividad = avance.CodActividad,
                codContacto = usuario.IdContacto,
                FechaAvanceEmprendedor = avance.FechaAvance,
                Mes = avance.Mes,
                ValorFondoEmprender = avance.Valor,
                ValorAporteEmprendedor = aporteEmprendedor,
                fechaRegistro = DateTime.Now,
                ObservacionEmprendedor = avance.Observaciones,
                ObservacionInterventor = avance.ObservacionesInterventor,
                FechaAvanceInterventor = avance.FechaAprobacion,
                Aprobada = avance.Aprobada,
                codProyecto = _CodProyecto
            };

            AvancesInterventoriaReg.insertarHistoricoPlanOperativo(avanceHis);
        }

        private void TareaAgendar(int codActivdad, int codProyecto, int agendo, int paraQuien, string nomProyecto, string nomActividad, string asunto,  string detalle)
        {
            AgendarTarea agenda = new AgendarTarea
                            (paraQuien,
                            asunto,
                            "Revisar actividad del plan operativo " + nomProyecto + " - Actividad --> " + nomActividad + " Observaciones: " + detalle.Trim(),
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
                            "Catálogo Actividad Plan Operativo");

            agenda.Agendar();
        }

        private string Validar(string accion)
        {
            var mensaje = string.Empty;
            switch(accion)
            {
                case "Crear":
                    if (string.IsNullOrEmpty(txt_observaciones.Text.Trim()))
                    {
                        mensaje = "Debe ingresar una observación";
                    }
                    else if (txt_observaciones.Text.Length > 5120) {
                        mensaje = "La observación debe ser máximo de 5120 caracteres.";
                    }                    
                    break;
                case "actualizar":
                    if (string.IsNullOrEmpty(txt_observaciones.Text.Trim()))
                    {
                        mensaje = "Debe ingresar una observación";
                    } else if (txt_observaciones.Text.Length > 5120)
                    {
                        mensaje = "La observación debe ser máximo de 5120 caracteres.";
                    }
                    break;
                case "Actualizar":
                    if(usuario.CodGrupo == Constantes.CONST_Interventor)
                    {
                        if (string.IsNullOrEmpty(txt_observ_interventor.Text.Trim()))
                        {
                            mensaje = "Debe ingresar una observación";
                        }else if (txt_observ_interventor.Text.Length > 5120)
                        {
                            mensaje = "La observación debe ser máximo de 5120 caracteres.";
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(txt_observaciones.Text.Trim()))
                        {
                            mensaje = "Debe ingresar una observación";
                        }else if (txt_observaciones.Text.Length > 5120)
                        {
                            mensaje = "La observación debe ser máximo de 5120 caracteres.";
                        }
                    }
                    break;
            }

            return mensaje;
        }

        private void TraerDatos(int codActividad)
        {
            var avanceActividad = (from aa in consultas.Db.AvanceActividadPOMes
                                   where aa.CodActividad == codActividad && aa.Mes == Mes
                                   select aa).ToList();

            if(avanceActividad.Count() > 0)
            {
                foreach (var actividad in avanceActividad)
                {
					
                    txt_observaciones.Text = actividad.Observaciones;
                    txt_observ_interventor.Text = actividad.ObservacionesInterventor;
					txtFechaAvance.Text = actividad.FechaAvance.HasValue?
										actividad.FechaAvance.Value.ToShortDateString() : "";

					txtFechaAprobacion.Text = actividad.FechaAprobacion.HasValue ?
										actividad.FechaAprobacion.Value.ToShortDateString() : "";

					dd_aprobado.SelectedValue = (Convert.ToBoolean(actividad.Aprobada).ToString() == "true") ? "1" : "0";
                    if (Convert.ToInt32(actividad.CodTipoFinanciacion) == 1)
                    {
                        txt_sueldo_obtenido.Text = Convert.ToDecimal(actividad.Valor).ToString("0.00", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        txt_prestaciones_obtenidas.Text = Convert.ToDecimal(actividad.Valor).ToString("0.00", CultureInfo.InvariantCulture);
                    }
                }

                if (Convert.ToBoolean(avanceActividad[0].Aprobada))
                {
                    txt_observaciones.Enabled = false;
                    txt_observ_interventor.Enabled = false;
                    dd_aprobado.Enabled = false;
                    txt_sueldo_obtenido.Enabled = false;
                    txt_prestaciones_obtenidas.Enabled = false;
                    B_Acion.Visible = false;
                    img_btn_NuevoDocumento.Visible = false;
                    dd_aprobado.SelectedValue = "1";
                    if(usuario.CodGrupo == Constantes.CONST_Interventor)
                    {
                        dd_aprobado.Enabled = true;
                        txt_observ_interventor.Enabled = true;
                        B_Acion.Visible = true;
                    }
                }
                else
                {
                    if(usuario.CodGrupo == Constantes.CONST_Emprendedor)
                    {
                        txt_observaciones.Enabled = true;
                        txt_observ_interventor.Enabled = false;
                        txt_sueldo_obtenido.Enabled = true;
                        txt_prestaciones_obtenidas.Enabled = true;
                        dd_aprobado.Enabled = false;
                    }
                    else if(usuario.CodGrupo == Constantes.CONST_Asesor 
                        || usuario.CodGrupo == Constantes.CONST_JefeUnidad 
                        || usuario.CodGrupo == Constantes.CONST_CallCenter
                        || usuario.CodGrupo == Constantes.CONST_CallCenterOperador
                        || usuario.CodGrupo == Constantes.CONST_LiderRegional 
                        || usuario.CodGrupo == Constantes.CONST_GerenteInterventor
                        || usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor
                        || usuario.CodGrupo == Constantes.CONST_AdministradorSistema)
                    {
                        txt_observaciones.Enabled = false;
                        txt_observ_interventor.Enabled = false;
                        txt_sueldo_obtenido.Enabled = false;
                        txt_prestaciones_obtenidas.Enabled = false;
                        dd_aprobado.Enabled = false;
                        B_Acion.Visible = false;
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
            Session["Accion"] = null;
            Session["CodActividad"] = null;
            Session["pagina"] = null;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Ventana", "window.close();", true);
        }

    }
}

/// <summary>
/// Estructura de quien agenda la tarea
/// </summary>
public class datosAgendar
{
    /// <summary>
    /// Obtiene o establece el id del contacto.
    /// </summary>
    /// <value>
    /// The identifier contacto.
    /// </value>
    public int idContacto{get; set;}

    /// <summary>
    /// Obtiene o establece el id del proyecto
    /// </summary>
    /// <value>
    /// Id del Proyecto
    /// </value>
    public int idProyecto { get; set; }

    /// <summary>
    /// Obtiene o establece el nombre del contacto
    /// </summary>
    /// <value>
    /// Nombre del contacto
    /// </value>
    public string nombre {get; set;}
}