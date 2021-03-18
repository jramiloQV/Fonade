using Datos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Negocio;
using System.Globalization;
using System.Data;
using Fonade.Clases;
using Fonade.Negocio.PlanDeNegocioV2.Interventoria;

namespace Fonade.FONADE.evaluacion
{
    /// <summary>
    /// CatalogoProduccionPOInterventoria
    /// </summary>

    public partial class CatalogoProduccionPOInterventoria : Base_Page
    {
        public int codProyecto;
        String nombreProduccion; //Valor que debe enviarse cuando se crea una nuevo registro, se usa para la notificación en tareas.
        String CodUsuario;
        String CodGrupo;
        int CodProduccion;
        int Mes;
        String nombreMes; //Este valor se le debe pasar por RowCommand del GridView donde se esté invocando.
        int v_CodTipoFinanciacion = 0;
        bool txtDeshabilitar;
        String txtSQL;
        Int32 txtTab = Constantes.CONST_SubPlanOperativo;

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["proyecto"] == null)
                    throw new ApplicationException("No se pudo obtener el codigo del proyecto, intentenlo de nuevo.");
                if (Session["CodProduccion"] == null)
                    throw new ApplicationException("No se pudo obtener numero del mes, intentenlo de nuevo.");
                if (Session["Accion"] == null)
                    throw new ApplicationException("No se pudo obtener el tipo de accion requerida, intentenlo de nuevo.");
                if (Session["MesDelProductoSeleccionado"] == null)
                    throw new ApplicationException("No se pudo obtener el mes del producto requerido, intentenlo de nuevo.");
                if (Session["NombreDelProductoSeleccionado"] == null)
                    throw new ApplicationException("No se pudo obtener el nombre del producto requerido, intentenlo de nuevo.");

                codProyecto = int.Parse(Session["proyecto"].ToString());
                var accion = Session["Accion"].ToString();
                Mes = int.Parse(Session["MesDelProductoSeleccionado"].ToString());
                nombreProduccion = Session["NombreDelProductoSeleccionado"].ToString();
                CodProduccion = int.Parse(Session["CodProduccion"].ToString());
                if (!Page.IsPostBack)
                {
                    CargarCombo();
                    MostrarControles(accion);

                    //cargarHistorial
                    cargarHistorial(CodProduccion, Mes);

                    validarPerfil(usuario.CodGrupo);
                }

                if (Session["datosProd"] != null)
                {
                    var datosform = (string[])Session["datosProd"];

                    txt_observaciones.Text = datosform[0];
                    txt_sueldo_obtenido.Text = datosform[1];
                    txt_prestaciones_obtenidas.Text = datosform[2];
                    txt_observ_interventor.Text = datosform[3];
                    Session["datosProd"] = null;
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

        private void validarPerfil(int _codGrupo)
        {
            if (_codGrupo != Constantes.CONST_Interventor)
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
            var historial = AvancesInterventoriaReg.GetHistoricoAvancesProduccion(_codActividad, _codMes);

            gvHistoricoAvances.DataSource = historial;
            gvHistoricoAvances.DataBind();
        }

        protected void B_Acion_Click(object sender, EventArgs e)
        {
            Metodos();
        }

        protected void img_btn_NuevoDocumento_Click(object sender, ImageClickEventArgs e)
        {
            string[] datosform = new string[4];
            datosform[0] = txt_observaciones.Text;
            datosform[1] = txt_sueldo_obtenido.Text;
            datosform[2] = txt_prestaciones_obtenidas.Text;
            datosform[3] = txt_observ_interventor.Text;

            Session["datosProd"] = datosform;

            Session["Accion_Docs"] = img_btn_NuevoDocumento.CommandName;
            Session["CodProyecto"] = codProyecto;
            Redirect(null, "../interventoria/CatalogoProduccionInter.aspx", "_self", "menubar=0,scrollbars=1,width=710,height=400,top=100");
        }

        protected void img_btn_enlazar_grilla_PDF_Click(object sender, ImageClickEventArgs e)
        {
            string[] datosform = new string[4];
            datosform[0] = txt_observaciones.Text;
            datosform[1] = txt_sueldo_obtenido.Text;
            datosform[2] = txt_prestaciones_obtenidas.Text;
            datosform[3] = txt_observ_interventor.Text;

            Session["datosProd"] = datosform;

            Session["Accion_Docs"] = "Lista";
            Redirect(null, "../interventoria/CatalogoProduccionInter.aspx", "_self", "menubar=0,scrollbars=1,width=710,height=400,top=100");
        }

        protected void B_Cancelar_Click(object sender, EventArgs e)
        {
            Session["Accion"] = null;
            Session["MesDelProductoSeleccionado"] = null;
            Session["CodProduccion"] = null;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Ventana", "window.close();", true);
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
            //var nomActividad = nombreProduccion;
            txt_cargo.Text = nombreProduccion;
            switch (accion)
            {
                case "Nuevo":
                    lbl_enunciado.Text = "Nuevo avance";
                    B_Acion.Text = "Crear";
                    Label1.Visible = false;
                    txt_mes.Visible = false;
                    Label2.Visible = false;
                    txt_cargo.Visible = false;
                    Label4.Visible = false;
                    txt_observ_interventor.Visible = false;
                    Label5.Visible = false;
                    img_btn_NuevoDocumento.Visible = true;
                    dd_aprobado.Visible = false;
                    txtFechaAvance.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtFechaAprobacion.Visible = false;
                    break;
                case "Modificar":
                    lbl_enunciado.Text = "Editar avance";
                    B_Acion.Text = "Actualizar";
                    Label4.Visible = true;
                    txt_observ_interventor.Visible = true;
                    txt_observ_interventor.Enabled = false;
                    if (usuario.CodGrupo == Constantes.CONST_Emprendedor) img_btn_NuevoDocumento.Visible = true;
                    TraerDatos(CodProduccion);
                    break;
                case "actualizar":
                    lbl_enunciado.Text = "Editar avance";
                    txt_observaciones.Enabled = false;
                    txt_sueldo_obtenido.Enabled = false;
                    txt_prestaciones_obtenidas.Enabled = false;
                    B_Acion.Text = "Actualizar";
                    TraerDatos(CodProduccion);
                    break;
                default:
                    lbl_enunciado.Text = "Editar avance";
                    B_Acion.Text = "Actualizar";
                    Label4.Visible = true;
                    txt_observ_interventor.Visible = true;
                    txt_observ_interventor.Enabled = false;
                    if (usuario.CodGrupo == Constantes.CONST_Emprendedor) img_btn_NuevoDocumento.Visible = true;
                    TraerDatos(CodProduccion);
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

        private void TraerDatos(int codActividad)
        {
            var avanceProducto = (from ap in consultas.Db.AvanceProduccionPOMes
                                  where ap.CodProducto == codActividad && ap.Mes == Mes
                                  select ap).ToList();

            if (avanceProducto.Count() > 0)
            {
                foreach (var actividad in avanceProducto)
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

                if (Convert.ToBoolean(avanceProducto[0].Aprobada))
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

        private void Metodos()
        {
            //var parametros = Session["CodActividad2"].ToString().Split(';');
            switch (B_Acion.Text)
            {
                case "Crear":
                    var mensaje = Validar(B_Acion.Text);
                    if (string.IsNullOrEmpty(mensaje))
                    {
                        var consultar = (from ap in consultas.Db.AvanceProduccionPOMes
                                         where ap.CodProducto == CodProduccion && ap.Mes == Mes
                                         select ap).ToList();

                        if (consultar.Count == 0)
                        {
                            var avance = new AvanceProduccionPOMes
                            {
                                CodProducto = CodProduccion,
                                Mes = Convert.ToByte(Mes),
                                CodTipoFinanciacion = 1,
                                Valor = decimal.Parse((!string.IsNullOrEmpty(txt_sueldo_obtenido.Text)) ? txt_sueldo_obtenido.Text : "0"),
                                Observaciones = txt_observaciones.Text,
                                CodContacto = usuario.IdContacto,
                                ObservacionesInterventor = "",
                                Aprobada = false,
                                FechaAvance = DateTime.Now
                            };

                            consultas.Db.AvanceProduccionPOMes.InsertOnSubmit(avance);
                            consultas.Db.SubmitChanges();

                            var avance2 = new AvanceProduccionPOMes
                            {
                                CodProducto = CodProduccion,
                                Mes = Convert.ToByte(Mes),
                                CodTipoFinanciacion = 2,
                                Valor = decimal.Parse((!string.IsNullOrEmpty(txt_prestaciones_obtenidas.Text)) ? txt_prestaciones_obtenidas.Text : "0"),
                                Observaciones = txt_observaciones.Text,
                                CodContacto = usuario.IdContacto,
                                ObservacionesInterventor = "",
                                Aprobada = false,
                                FechaAvance = DateTime.Now
                            };

                            consultas.Db.AvanceProduccionPOMes.InsertOnSubmit(avance2);
                            consultas.Db.SubmitChanges();

                            //Consultar Id Interventor para agendar tarea
                            var datos = (from ei in consultas.Db.EmpresaInterventors
                                         join ee in consultas.Db.Empresas on ei.CodEmpresa equals ee.id_empresa
                                         join p in consultas.Db.Proyecto on ee.codproyecto equals p.Id_Proyecto
                                         where ee.codproyecto == codProyecto && ei.Inactivo == false
                                         select new datosAgendar
                                         {
                                             idContacto = (int)ei.CodContacto,
                                             idProyecto = (int)p.Id_Proyecto,
                                             nombre = p.NomProyecto
                                         }).ToList();

                            //Insertar Historico
                            insertarHistorico(avance, avance2.Valor, codProyecto);

                            var asunto = "Revisar Actividad de Producción. Se ha creado una actividad.";
                            TareaAgendar(CodProduccion, codProyecto, usuario.IdContacto, datos[0].idContacto, datos[0].nombre, nombreProduccion,
                                asunto, txt_observaciones.Text.Trim());
                        }
                        Session["Accion"] = null;
                        Session["MesDelProductoSeleccionado"] = null;
                        Session["CodProduccion"] = null;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Información de producción creada correctamente.'); window.close();", true);
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
                            var avancesActividadTipo1 = (from ap in consultas.Db.AvanceProduccionPOMes
                                                         where
                                                             ap.CodProducto == CodProduccion
                                                             && ap.Mes == Mes
                                                             && ap.CodTipoFinanciacion == 1
                                                         select ap).ToList();

                            if (avancesActividadTipo1.Any())
                            {
                                foreach (var avance in avancesActividadTipo1)
                                {
                                    avance.FechaAvance = DateTime.Now;
                                    avance.Observaciones = txt_observaciones.Text.Trim();
                                    avance.Valor = (avance.CodTipoFinanciacion == 1) ? decimal.Parse((!string.IsNullOrEmpty(txt_sueldo_obtenido.Text)) ? txt_sueldo_obtenido.Text : "0") : decimal.Parse((!string.IsNullOrEmpty(txt_prestaciones_obtenidas.Text)) ? txt_prestaciones_obtenidas.Text : "0");
                                }
                            }
                            else
                            {
                                var avanceMes = new AvanceProduccionPOMes()
                                {
                                    Mes = Convert.ToByte(Mes),
                                    CodProducto = CodProduccion,
                                    CodTipoFinanciacion = 1,
                                    Aprobada = false,
                                    CodContacto = usuario.IdContacto,
                                    Observaciones = txt_observaciones.Text.Trim(),
                                    ObservacionesInterventor = string.Empty,
                                    Valor = decimal.Parse((!string.IsNullOrEmpty(txt_sueldo_obtenido.Text)) ? txt_sueldo_obtenido.Text : "0")
                                };

                                consultas.Db.AvanceProduccionPOMes.InsertOnSubmit(avanceMes);
                            }

                            var avancesActividadTipo2 = (from ap in consultas.Db.AvanceProduccionPOMes
                                                         where
                                                             ap.CodProducto == CodProduccion
                                                             && ap.Mes == Mes
                                                             && ap.CodTipoFinanciacion == 2
                                                         select ap).ToList();

                            if (avancesActividadTipo2.Any())
                            {
                                foreach (var avance in avancesActividadTipo2)
                                {
                                    avance.FechaAvance = DateTime.Now;
                                    avance.Observaciones = txt_observaciones.Text.Trim();
                                    avance.Valor = (avance.CodTipoFinanciacion == 1) ? decimal.Parse((!string.IsNullOrEmpty(txt_sueldo_obtenido.Text)) ? txt_sueldo_obtenido.Text : "0") : decimal.Parse((!string.IsNullOrEmpty(txt_prestaciones_obtenidas.Text)) ? txt_prestaciones_obtenidas.Text : "0");
                                }
                            }
                            else
                            {
                                var avanceMes = new AvanceProduccionPOMes()
                                {
                                    Mes = Convert.ToByte(Mes),
                                    CodProducto = CodProduccion,
                                    CodTipoFinanciacion = 2,
                                    Aprobada = false,
                                    CodContacto = usuario.IdContacto,
                                    Observaciones = txt_observaciones.Text.Trim(),
                                    ObservacionesInterventor = string.Empty,
                                    Valor = decimal.Parse((!string.IsNullOrEmpty(txt_sueldo_obtenido.Text)) ? txt_sueldo_obtenido.Text : "0")
                                };

                                consultas.Db.AvanceProduccionPOMes.InsertOnSubmit(avanceMes);
                            }


                            consultas.Db.SubmitChanges();

                            //Consultar Id Interventor para agendar tarea
                            var datos = (from ei in consultas.Db.EmpresaInterventors
                                         join ee in consultas.Db.Empresas on ei.CodEmpresa equals ee.id_empresa
                                         join p in consultas.Db.Proyecto on ee.codproyecto equals p.Id_Proyecto
                                         where ee.codproyecto == codProyecto && ei.Inactivo == false
                                         select new datosAgendar
                                         {
                                             idContacto = (int)ei.CodContacto,
                                             idProyecto = (int)p.Id_Proyecto,
                                             nombre = p.NomProyecto
                                         }).ToList();

                            decimal valorcosto = decimal.Parse((!string.IsNullOrEmpty(txt_prestaciones_obtenidas.Text)) ? txt_prestaciones_obtenidas.Text.Replace(".", ",") : "0");

                            var hisAvance = avancesActividadTipo1.Where(x => x.CodTipoFinanciacion == 1).FirstOrDefault();

                            //Insertar Historico
                            insertarHistorico(hisAvance, valorcosto, codProyecto);


                            var asunto = "Revisar Actividad de Prtoducción. Se ha modificado una actividad.";
                            TareaAgendar(CodProduccion, codProyecto, usuario.IdContacto, datos[0].idContacto, datos[0].nombre, nombreProduccion,
                                asunto, txt_observaciones.Text.Trim());

                            Session["Accion"] = null;
                            Session["MesDelProductoSeleccionado"] = null;
                            Session["CodProduccion"] = null;
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Información de Producción procesada correctamente.'); window.close();", true);
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
                            var avancesActividad = (from ap in consultas.Db.AvanceProduccionPOMes
                                                    where ap.CodProducto == CodProduccion && ap.Mes == Mes
                                                    select ap).ToList();

                            foreach (var avance in avancesActividad)
                            {
                                avance.FechaAprobacion = DateTime.Now;
                                avance.ObservacionesInterventor = txt_observ_interventor.Text.Trim();
                                avance.Aprobada = (dd_aprobado.SelectedValue == "0") ? false : true;
                            }
                            consultas.Db.SubmitChanges();

                            //consulta Id del emprendedor para agendar tarea
                            var datos = (from pc in consultas.Db.ProyectoContactos
                                         join p in consultas.Db.Proyecto on pc.CodProyecto equals p.Id_Proyecto
                                         where pc.CodProyecto == codProyecto && pc.CodRol == 3
                                         select new datosAgendar
                                         {
                                             idContacto = pc.CodContacto,
                                             idProyecto = p.Id_Proyecto,
                                             nombre = p.NomProyecto
                                         }).ToList();


                            decimal valorcosto = decimal.Parse((!string.IsNullOrEmpty(txt_prestaciones_obtenidas.Text)) ? txt_prestaciones_obtenidas.Text.Replace(".", ",") : "0");

                            var hisAvance = avancesActividad.Where(x => x.CodTipoFinanciacion == 1).FirstOrDefault();

                            //Insertar Historico
                            insertarHistorico(hisAvance, valorcosto, codProyecto);

                            var asunto = "Revisar Actividad de Producción. Se ha modificado una actividad.";
                            TareaAgendar(CodProduccion, codProyecto, usuario.IdContacto, datos[0].idContacto, datos[0].nombre, nombreProduccion,
                                asunto, txt_observaciones.Text.Trim());

                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Información de Producción procesada correctamente.'); window.close();", true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + mensaje + "');", true);
                        }
                    }
                    break;
            }


        }

        private void insertarHistorico(AvanceProduccionPOMes avance, decimal _costo, int _CodProyecto)
        {
            HistoricoAvanceModel avanceHis = new HistoricoAvanceModel
            {
                codActividad = avance.CodProducto,
                codContacto = usuario.IdContacto,
                FechaAvanceEmprendedor = avance.FechaAvance,
                Mes = avance.Mes,
                Cantidad = avance.Valor,
                Costo = _costo,
                fechaRegistro = DateTime.Now,
                ObservacionEmprendedor = avance.Observaciones,
                ObservacionInterventor = avance.ObservacionesInterventor,
                FechaAvanceInterventor = avance.FechaAprobacion,
                Aprobada = avance.Aprobada,
                codProyecto = _CodProyecto
            };

            AvancesInterventoriaReg.insertarHistoricoProduccion(avanceHis);
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

        private void TareaAgendar(int codActivdad, int codProyecto, int agendo, int paraQuien, string nomProyecto, string nomActividad, string asunto, string detalle)
        {
            AgendarTarea agenda = new AgendarTarea
                            (paraQuien,
                            asunto,
                            "Revisar la actividad de Producción " + nomProyecto + " - Actividad --> " + nomActividad + " Observaciones: " + detalle.Trim(),
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
                            "Catálogo Actividad Producción");

            agenda.Agendar();
        }
        #endregion


    }
}