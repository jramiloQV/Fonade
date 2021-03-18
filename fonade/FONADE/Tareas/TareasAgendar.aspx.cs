using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using Fonade.Account;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using Fonade.Clases;

namespace Fonade.FONADE.Tareas
{
    public partial class TareasAgendar : Negocio.Base_Page/*System.Web.UI.Page*/
    {
        /// <summary>
        /// Tarea seleccionada.
        /// </summary>
        Int32 Id_TareaUsuarioRepeticion;
        /// <summary>
        /// Variable que contiene las consultas SQL.
        /// </summary>
        String txtSQL;
        string errorMessageDetail;

        protected void lds_tareas_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {

        }


        ValidacionCuenta validacionCuenta = new ValidacionCuenta();
        protected void Page_Load(object sender, EventArgs e)
        {
            Id_TareaUsuarioRepeticion = HttpContext.Current.Session["Id_tareaRepeticion"] != null ? Id_TareaUsuarioRepeticion = Convert.ToInt32(HttpContext.Current.Session["Id_tareaRepeticion"].ToString()) : 0;

            if (!IsPostBack)
            {
                //Recuperar la url
                string pathRuta = HttpContext.Current.Request.Url.AbsolutePath;

                if (!validacionCuenta.validarPermiso(usuario.IdContacto, pathRuta))
                {
                    Response.Redirect(validacionCuenta.rutaHome(), true);
                }
                else
                {
                    DateTime? fechaActual = DateTime.Today;
                    DateTime fechamostar = DateTime.Today;
                    txtDate2.Text = fechamostar.ToString("dd/MM/yyyy");



                    if (Id_TareaUsuarioRepeticion != 0)
                    {
                        menuMostar();
                        tbl1.Visible = false;
                        tbl1.Enabled = false;
                        Panel2.Visible = true;
                        Panel2.Enabled = true;
                        lbl_Titulo.Text = "REVISAR TAREA";
                    }
                    else
                    {
                        CargarPlanesDeNegocio();
                        tbl1.Visible = true;
                        tbl1.Enabled = true;
                        Panel2.Visible = false;
                        Panel2.Enabled = false;
                        lbl_Titulo.Text = "AGENDAR TAREA";
                    }

                    cargarActividades();


                    if (usuario.CodGrupo == Constantes.CONST_AdministradorSistema || usuario.CodGrupo == Constantes.CONST_GerenteAdministrador)
                    {
                        labelPlan.Visible = false;
                    }
                }
            }

        }

        protected void Button1_click(object sender, EventArgs e)
        {
            if (ListBox1.Items.Count != 0)
            {
                string listausuarios = " ";

                foreach (ListItem li in ListBox1.Items)
                {
                    if (li.Selected)
                    {
                        listausuarios = listausuarios + " " + li.Text + " ";

                        bool wrequiererespuesta = false;

                        if (ddl_respuesta.SelectedValue.Trim() == "Sí")
                        {
                            wrequiererespuesta = true;
                        }

                        AgendarTarea agenda = new AgendarTarea(Int32.Parse(li.Value), tb_tarea.Text, tb_descripcion.Text, plan_seleccionado.Value, int.Parse(ddl_usuarios.SelectedValue), "0", Convert.ToBoolean(ddl_avisar.SelectedValue), Int32.Parse(ddl_urgencia.SelectedValue.ToString()), false, wrequiererespuesta, usuario.IdContacto, null, null, null);
                        agenda.Agendar();
                    }
                }

                if (ddl_avisar.SelectedValue == "Sí" || ddl_avisar.SelectedValue == "True")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Se ha enviado correo a los siguiente usuarios: " + listausuarios + "');window.location='TareasAgendar.aspx'", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Tarea " + tb_tarea.Text + " Agendada');window.location='TareasAgendar.aspx'", true);
                }
            }
        }

        protected void ListBox1_OnSelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btn_Grabar_Click(object sender, EventArgs e)
        {
            Consultas consulta = new Consultas();
            String txtSQL1 = " SELECT codtareaUsuario FROM TareaUsuarioRepeticion  ";
            txtSQL1 += " WHERE Id_TareaUsuarioRepeticion = " + Id_TareaUsuarioRepeticion;
            var resul = consulta.ObtenerDataTable(txtSQL1, "text");
            String wdato = resul.Rows[0].ItemArray[0].ToString();

            txtSQL1 = " SELECT RequiereRespuesta FROM TareaUsuario where id_tareaUsuario= '" + wdato + "'";

            var resul1 = consulta.ObtenerDataTable(txtSQL1, "text");
            if (resul1.Rows[0].ItemArray[0].ToString() == "True")
                if (TextBox9.Text.ToString().Length == 0)
                {
                    lbl_Titulo0.Text = "Se Requiere Una Observación , Por favor Registre su Observación para que el registro sea Guardado";
                    return;
                }
                else { lbl_Titulo0.Text = ""; }


            String txtSQL = "UPDATE TareaUsuarioRepeticion SET ";

            if (CheckBox1.Checked)
            {
                txtSQL += " FechaCierre = CONVERT(DATETIME,'" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "',102)";
            }
            else
            {
                txtSQL += " FechaCierre = null";
            }

            if (String.IsNullOrEmpty(TextBox9.Text))
            {
                txtSQL += ", Respuesta = ''";
            }
            else
            {
                txtSQL += " , Respuesta = '" + TextBox9.Text + "'";
            }

            txtSQL += " WHERE Id_TareaUsuarioRepeticion = " + Id_TareaUsuarioRepeticion;

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(txtSQL, conn);
            try
            {

                conn.Open();
                cmd.ExecuteReader();
            }
            catch (SqlException se) { errorMessageDetail = se.Message; }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            menuMostar();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "update", "alert('Registro de tarea actualizado.')", true);
        }

        private void menuMostar()
        {
            consultas.Parameters = new[] { new SqlParameter
                                                   {
                                                        ParameterName = "@Id_TareaUsuarioRepeticion",
                                                        Value = Id_TareaUsuarioRepeticion
                                                   }
                };
            DataTable dtActas = consultas.ObtenerDataTable("MP_ReporteTareasUsuario");

            try
            {
                TextBox1.Text = dtActas.Rows[0]["NomUsuarioAgendo"].ToString();
                TextBox2.Text = dtActas.Rows[0]["NomUsuario"].ToString();
                TextBox3.Text = dtActas.Rows[0]["NomTareaPrograma"].ToString();
                TextBox4.Text = dtActas.Rows[0]["NomProyecto"].ToString();

                if (String.IsNullOrEmpty(TextBox4.Text))
                {
                    tr_planNegocio.Visible = false;
                    tr_planNegocio.Attributes.Add("display", "none");
                }

                try
                {
                    if (!String.IsNullOrEmpty(dtActas.Rows[0]["Ejecutable"].ToString()))
                    {
                        //Adicionar evento dinámico.
                        lnk_SolicitudPago.Visible = false;
                        TextBox5.Text = dtActas.Rows[0]["NomTareaUsuario"].ToString();

                        if (lnk_SolicitudPago.CommandArgument.ToString().Trim() == "")
                        {
                            lnk_SolicitudPago.CommandArgument = dtActas.Rows[0]["Ejecutable"].ToString() + ";" + dtActas.Rows[0]["Parametros"].ToString();
                        }

                        lnk_SolicitudPago.Text = dtActas.Rows[0]["NomTareaUsuario"].ToString();

                        if (TextBox5.Visible)
                        {
                            TextBox5.Text = dtActas.Rows[0]["NomTareaUsuario"].ToString();
                            if (TextBox5.Text.Contains("+#39;")) { TextBox5.Text = TextBox5.Text.Replace("+#39;", "'"); }
                            TextBox5.Text = dtActas.Rows[0]["NomTareaUsuario"].ToString();
                        }
                    }
                    else
                    {
                        TextBox5.Text = dtActas.Rows[0]["NomTareaUsuario"].ToString();
                        TextBox5.Text = dtActas.Rows[0]["NomTareaUsuario"].ToString();
                    }
                }
                catch { TextBox5.Text = ""; lnk_SolicitudPago.Visible = false; }

                TextBox6.Text = dtActas.Rows[0]["Descripcion"].ToString().Replace("<b>", "").Replace("</b>", "").Replace("<B>", "").Replace("</B>", "");
                if (TextBox6.Text.Contains("+#39;")) { TextBox6.Text = TextBox6.Text.Replace("+#39;", "'"); }
                TextBox7.Text = dtActas.Rows[0]["SoloFecha"].ToString() + " " + dtActas.Rows[0]["SoloHora"].ToString();

                int Urgencia = Int32.Parse(dtActas.Rows[0]["NivelUrgencia"].ToString());

                switch (Int32.Parse(dtActas.Rows[0]["NivelUrgencia"].ToString()))
                {
                    case 0:
                        img_urgencia.ImageUrl = "../../Images/Tareas/Urgencia1.gif";
                        img_urgencia.ToolTip = "Muy Alta";
                        lblUrgencia_Text.Text = img_urgencia.ToolTip;
                        img_urgencia.AlternateText = img_urgencia.ToolTip;
                        break;
                    case 1:
                        img_urgencia.ImageUrl = "../../Images/Tareas/Urgencia1.gif";
                        img_urgencia.ToolTip = "Muy Alta";
                        lblUrgencia_Text.Text = img_urgencia.ToolTip;
                        img_urgencia.AlternateText = img_urgencia.ToolTip;
                        break;
                    case 2:
                        img_urgencia.ImageUrl = "../../Images/Tareas/Urgencia2.gif";
                        img_urgencia.ToolTip = "Alta";
                        lblUrgencia_Text.Text = img_urgencia.ToolTip;
                        img_urgencia.AlternateText = img_urgencia.ToolTip;
                        break;
                    case 3:
                        img_urgencia.ImageUrl = "../../Images/Tareas/Urgencia3.gif";
                        img_urgencia.ToolTip = "Normal";
                        lblUrgencia_Text.Text = img_urgencia.ToolTip;
                        img_urgencia.AlternateText = img_urgencia.ToolTip;
                        break;
                    case 4:
                        img_urgencia.ImageUrl = "../../Images/Tareas/Urgencia4.gif";
                        img_urgencia.ToolTip = "Baja";
                        lblUrgencia_Text.Text = img_urgencia.ToolTip;
                        img_urgencia.AlternateText = img_urgencia.ToolTip;
                        break;
                    default:
                        img_urgencia.ImageUrl = "../../Images/Tareas/Urgencia5.gif";
                        img_urgencia.ToolTip = "Muy Baja";
                        lblUrgencia_Text.Text = img_urgencia.ToolTip;
                        img_urgencia.AlternateText = img_urgencia.ToolTip;
                        break;
                }

                TextBox9.Text = dtActas.Rows[0]["Respuesta"].ToString();

                if (String.IsNullOrEmpty(dtActas.Rows[0]["FechaCierre"].ToString()))
                {
                    CheckBox1.Enabled = true;
                    CheckBox1.Checked = false;
                }
                else
                {
                    CheckBox1.Checked = true;
                    CheckBox1.Enabled = false;
                    TextBox9.Enabled = false;
                }
            }
            catch (System.IndexOutOfRangeException)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "back",
                    " alert('No se registró programación en agenda para esta tarea.'); history.go(-1)", true);
            }
        }

        private void CargarPlanesDeNegocio()
        {
            //Inicializar variables.
            DataTable tabla = new DataTable();

            try
            {
                txtSQL = "SELECT Id_Proyecto, CONVERT(varchar(max), Id_Proyecto ) + ' - ' + NomProyecto NomProyecto FROM Proyecto WHERE Inactivo = 0 ";

                switch (usuario.CodGrupo)
                {
                    case Constantes.CONST_Asesor:
                    case Constantes.CONST_Emprendedor:
                        txtSQL = txtSQL + " and CodInstitucion = " + usuario.CodInstitucion +
                                          " and  exists (select codproyecto from proyectocontacto pc " +
                                          " where id_proyecto = codproyecto and pc.codcontacto = " + usuario.IdContacto + " and pc.inactivo = 0) ";
                        break;

                    case Constantes.CONST_JefeUnidad:
                        txtSQL = txtSQL + " and CodInstitucion = " + usuario.CodInstitucion;
                        break;

                    case Constantes.CONST_GerenteEvaluador:
                        txtSQL = txtSQL + " and CodEstado = " + Constantes.CONST_Evaluacion;
                        txtSQL = txtSQL + " and codOperador = " + usuario.CodOperador;
                        break;

                    case Constantes.CONST_CoordinadorEvaluador:
                    case Constantes.CONST_Evaluador:
                        txtSQL = txtSQL + " and  exists (select codproyecto from proyectocontacto pc " +
                                          " where id_proyecto = codproyecto and pc.codcontacto = " + usuario.IdContacto + " and pc.inactivo = 0) ";
                        break;

                    //Interventoria.
                    case Constantes.CONST_GerenteInterventor:
                        txtSQL = txtSQL + " and CodEstado IN (" + Constantes.CONST_LegalizacionContrato + "," + Constantes.CONST_Ejecucion + ")"
                                        + " and codOperador = " + usuario.CodOperador;
                        break;

                    case Constantes.CONST_CoordinadorInterventor:
                        txtSQL = txtSQL + " and  exists (select codproyecto from proyectocontacto pc " +
                                          " where id_proyecto = codproyecto and pc.codcontacto = " + usuario.IdContacto + " and pc.inactivo = 0) ";
                        break;

                    case Constantes.CONST_Interventor:
                        txtSQL = txtSQL + " and  Id_Proyecto IN (SELECT Empresa.codproyecto FROM Empresa INNER JOIN EmpresaInterventor ON Empresa.id_empresa = EmpresaInterventor.CodEmpresa " +
                                          " WHERE (EmpresaInterventor.Inactivo = 0) AND (EmpresaInterventor.CodContacto = " + usuario.IdContacto + ")) ";
                        break;
                    case Constantes.CONST_LiderRegional:
                        var lider = (from c in consultas.Db.Contacto
                                     where c.Id_Contacto == usuario.IdContacto
                                     select c).FirstOrDefault();

                        txtSQL = "Select distinct p.Id_Proyecto, CONVERT(varchar(max), Id_Proyecto ) + ' - ' + p.NomProyecto + ' - '+ c.NomCiudad NomProyecto from Proyecto p ";
                        txtSQL += "Inner Join ProyectoContacto pc on pc.codProyecto = p.Id_Proyecto ";
                        txtSQL += "Inner Join ciudad c on c.Id_Ciudad = p.CodCiudad ";
                        txtSQL += "Inner Join departamento d on d.id_Departamento = c.codDepartamento ";
                        txtSQL += "Where p.inactivo = 0 And d.id_Departamento = " + lider.CodTipoAprendiz + " order by 2";
                        break;

                    case Constantes.CONST_PerfilAcreditador:
                        txtSQL = txtSQL + " and CodEstado in ("
                                                    + Constantes.CONST_Asignado_para_acreditacion + ","
                                                    + Constantes.CONST_Aprobacion_Acreditacion + ","
                                                    + Constantes.CONST_Aprobacion_No_Acreditacion + ","
                                                    + Constantes.CONST_Acreditado + ","
                                                    + Constantes.CONST_No_acreditado + ","
                                                    + Constantes.CONST_concejo_directivo
                                                    + ")";
                        txtSQL = txtSQL + " and codOperador = " + usuario.CodOperador;
                        break;

                    default:
                        break;
                }

                if (usuario.CodGrupo != Constantes.CONST_LiderRegional)
                {
                    txtSQL = txtSQL + " ORDER BY NomProyecto";
                }

                //Limpiar los elementos que puedan contener el DropDownList.
                DropDownList1.Items.Clear();

                //Generar el ítem vacío por defecto.
                ListItem item_default = new ListItem();
                item_default.Text = "";
                item_default.Value = "";
                DropDownList1.Items.Add(item_default);

                if (txtSQL.Trim() != "")
                {
                    tabla = consultas.ObtenerDataTable(txtSQL, "text");
                    foreach (DataRow row in tabla.Rows)
                    {
                        ListItem items = new ListItem();
                        items.Text = row["NomProyecto"].ToString().Trim();
                        items.Value = row["Id_Proyecto"].ToString();
                        DropDownList1.Items.Add(items);
                    }
                }
            }
            catch (Exception ex) { errorMessageDetail = ex.Message; }
        }

        protected void ldslistbox_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (usuario.CodGrupo == Constantes.CONST_GerenteEvaluador)
            {
                var result = (from atr in consultas.Db.MD_AgendarTareas_Prueba(usuario.IdContacto, usuario.CodGrupo, usuario.CodInstitucion, "TraerUsuarios", null, null, null, null, null, null, null, null, null, null, null, null)

                              select new
                              {
                                  atr.Id_Contacto,
                                  Nombre = atr.Nombre.Trim()
                              }).ToList();
                var query = consultas.Db.Contacto.Where(x => result.Select(y => y.Id_Contacto).Contains(x.Id_Contacto)).ToList();
                query = query.Where(x => x.codOperador == usuario.CodOperador).ToList();
                //e.Result = result.ToList();
                e.Result = (from q in query
                            select new
                            {
                                q.Id_Contacto,
                                Nombre = q.Nombres + " " + q.Apellidos
                            }
                            ).ToList();
            }
            else if (usuario.CodGrupo == Constantes.CONST_LiderRegional)
            {
                var lider = (from c in consultas.Db.Contacto
                             where c.Id_Contacto == usuario.IdContacto
                             select c).FirstOrDefault();
                //Solo Asesores Lider
                var result = (from c in consultas.Db.Contacto
                              join gc in consultas.Db.GrupoContactos on c.Id_Contacto equals gc.CodContacto
                              join g in consultas.Db.Grupos on gc.CodGrupo equals g.Id_Grupo
                              join ci in consultas.Db.Ciudad on c.CodCiudad equals ci.Id_Ciudad
                              join d in consultas.Db.departamento on ci.CodDepartamento equals d.Id_Departamento
                              where gc.CodGrupo == 5 && d.Id_Departamento == lider.CodTipoAprendiz && c.Inactivo == false
                              orderby ci.NomCiudad
                              select new
                              {
                                  c.Id_Contacto,
                                  Nombre = c.Nombres.ToUpper() + " " + c.Apellidos.ToUpper() + " (" + ci.NomCiudad + " - " + g.NomGrupo + ")"
                              });
                e.Result = result.ToList();
            }
            else
            {

                var result = (from atr in consultas.Db.MD_AgendarTareas_Prueba(usuario.IdContacto, usuario.CodGrupo, usuario.CodInstitucion, "TraerUsuarios", null, null, null, null, null, null, null, null, null, null, null, null)
                              select new
                              {
                                  atr.Id_Contacto,
                                  Nombre = atr.Nombre
                              });

                e.Result = result.ToList();
            }

        }

        protected void ldscontacto_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var result = (from tp in consultas.Db.TareaProgramas
                          where tp.delSistema == null | tp.delSistema != 1
                          select new
                          {
                              Id_Tarea = tp.Id_TareaPrograma,
                              Nombre_Tarea = tp.NomTareaPrograma,
                          });

            e.Result = result.ToList();
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.plan_seleccionado.Value) && usuario.CodGrupo.Equals(Constantes.CONST_Evaluador))
                SeleccionarEquipoTrabajo(Convert.ToInt32(this.plan_seleccionado.Value), false);

            if (!String.IsNullOrEmpty(DropDownList1.SelectedValue) && usuario.CodGrupo.Equals(Constantes.CONST_Evaluador))
            {
                SeleccionarEquipoTrabajo(Convert.ToInt32(DropDownList1.SelectedValue));
            }

            if (!String.IsNullOrEmpty(DropDownList1.SelectedValue) && usuario.CodGrupo.Equals(Constantes.CONST_PerfilAcreditador))
            {
                SeleccionarEquipoTrabajoAcreditador(Convert.ToInt32(DropDownList1.SelectedValue));
            }

            this.plan_seleccionado.Value = DropDownList1.SelectedValue;
        }

        protected void SeleccionarEquipoTrabajoAcreditador(int codigoProyecto, bool selectTeam = true)
        {
            bool seleccionoAlguien = false;
            var equipoTrabajo = Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.GetEquipoTrabajo(codigoProyecto);
            ListBox1.ClearSelection();

            foreach (Int32 integrante in equipoTrabajo)
            {
                var contacto = ListBox1.Items.FindByValue(integrante.ToString());

                if (contacto != null)
                {
                    contacto.Selected = selectTeam;
                    seleccionoAlguien = true;
                }
            }

            if (selectTeam && seleccionoAlguien)
                PlanDeNegocioV2.Formulacion.Utilidad.Utilidades.PresentarMsj("Se seleccionó el emprendedor del proyecto.", this, "Alert");
            else
            {
                PlanDeNegocioV2.Formulacion.Utilidad.Utilidades.PresentarMsj("El proyecto no cuenta con emprendedores activos.", this, "Alert");
            }
        }

        protected void SeleccionarEquipoTrabajo(int codigoProyecto, bool selectTeam = true)
        {

            var equipoTrabajo = Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.GetEquipoTrabajo(codigoProyecto);

            foreach (Int32 integrante in equipoTrabajo)
            {
                var contacto = ListBox1.Items.FindByValue(integrante.ToString());

                if (contacto != null)
                {
                    contacto.Selected = selectTeam;
                }
            }

            if (selectTeam)
                PlanDeNegocioV2.Formulacion.Utilidad.Utilidades.PresentarMsj("Se seleccionó el equipo de trabajo asociado al proyecto: Emprendedores,asesores, evaluador y coordinador de evaluadores.", this, "Alert");
        }

        /// <summary>
        /// Evento OnClick para enviar por sesión los parámetros y mostrar así la ventana emergente.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnk_SolicitudPago_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)(sender);

                //Obtiene los valores del CommandArgument a procesar.
                var valores_command = btn.CommandArgument.ToString().Split(';');

                //Crea la variable "Ejecutable" (que contiene el nombre de la página a redireccionar).
                string Ejecutable = valores_command[0];
                //Evalúo, si la página contiene la parte ".asp", se cambiará a ".aspx".
                if (Ejecutable.Contains(".asp")) { Ejecutable = Ejecutable.Replace(".asp", ".aspx"); }

                //Crea la variable "inputString" (que contiene los parámetros de la página).
                string inputString = valores_command[1];

                //Listas que contendrán la división de los valores procesados.
                var valores_ampersand = new string[] { };
                var valores_equals = new string[] { };

                //Divido en strings los valores que tengan como delimitador el ampersand "&".
                valores_ampersand = inputString.Split('&');

                //Variable que determina si puede abrir la ventana emergente "o no".
                bool AbrirVentana = false;

                //Recorrer cada valor obtenidos al separarlos por los ampersands.
                foreach (string item in valores_ampersand)
                {
                    //Inicializo las variables "esta vez, para separarlos por el signo (=)"
                    valores_equals = new string[] { };
                    valores_equals = item.Split('=');

                    //Crea las variables de sesión.
                    if (valores_equals.Where(t => !string.IsNullOrEmpty(t)).Count() > 0)
                        Session[string.IsNullOrEmpty(valores_equals[0]) ? "Accion" : valores_equals[0]] =
                            string.IsNullOrEmpty(valores_equals[1]) ? "Editar" : valores_equals[1];
                    else HttpContext.Current.Session["Accion"] = "Editar";
                    //Permitir "cuando salga del ciclo" generar la ventana emergente.
                    AbrirVentana = true;
                }

                if (AbrirVentana)
                {
                    //Verificar la ubicación de la página antes de redireccionarlo.
                    switch (Ejecutable)
                    {
                        case "AdicionarInformeBimensual.aspx":
                            Ejecutable = "../interventoria/" + Ejecutable;
                            break;
                        case "AdicionarInformePresupuestal.aspx":
                            Ejecutable = "../interventoria/" + Ejecutable;
                            break;
                        case "AgregarInformeFinalInterventoria.aspx":
                            Ejecutable = "../interventoria/" + Ejecutable;
                            break;
                        case "CatalogoActividadPO.aspx":
                            Ejecutable = "../evaluacion/" + Ejecutable;
                            break;
                        case "CatalogoIndicadorInter.aspx":
                            Ejecutable = "../interventoria/" + Ejecutable;
                            break;
                        case "CatalogoInterventorTMP.aspx":
                            Ejecutable = "../interventoria/" + Ejecutable;
                            break;
                        case "CatalogoProduccionTMP.aspx":
                            Ejecutable = "../evaluacion/" + Ejecutable;
                            break;
                        case "CatalogoRiesgoInter.aspx":
                            Ejecutable = "../interventoria/" + Ejecutable;
                            break;
                        case "CatalogoUnidadEmprende.aspx":
                            Ejecutable = "../Administracion/" + Ejecutable;
                            break;
                        case "CatalogoVentasTMP.aspx":
                            Ejecutable = "../evaluacion/" + Ejecutable;
                            break;
                        case "FrameAsesorProyecto.aspx":
                            Ejecutable = "../AdministrarPerfiles/" + Ejecutable;
                            break;
                        case "FrameCoordinadorEvaluador.aspx":
                            break;
                        case "FrameEvaluadorProyecto.aspx":
                            break;
                        case "FrameInterventorProyecto.aspx":
                            break;
                        case "PagosActividad.aspx":
                            Ejecutable = "../interventoria/" + Ejecutable;
                            break;
                        case "PagosActividadCoord.aspx":
                            Ejecutable = "../interventoria/" + Ejecutable;
                            break;
                        case "PagosActividadInter.aspx":
                            Ejecutable = "../interventoria/" + Ejecutable;
                            break;
                        default:
                            break;
                    }

                    Redirect(null, Ejecutable, "_blank", "menubar=0,scrollbars=1,width=710,height=500,top=100");
                }
            }
            catch (Exception ex) { errorMessageDetail = ex.Message; }
        }

        private void cargarActividades()
        {
            var result = (from tp in consultas.Db.TareaProgramas
                          where tp.delSistema == null || tp.delSistema != 1
                          select tp).ToList();

            ddl_usuarios.DataSource = result;
            ddl_usuarios.DataTextField = "NomTareaPrograma";
            ddl_usuarios.DataValueField = "Id_TareaPrograma";
            ddl_usuarios.DataBind();
        }
    }
}