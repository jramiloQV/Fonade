using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Fonade.Account;
using LinqKit;
using AjaxControlToolkit;
using System.ComponentModel;

namespace Fonade.FONADE.interventoria
{
    public partial class CatalogoContratoInterventor : Negocio.Base_Page
    {
        #region Código hecho por Mauricio Arias Olave.

        #region Variables internas.

        /// <summary>
        /// Valor que se declara desde "CatalogoInterventor.aspx" y se obtiene para saber si se debe
        /// "Crear" un nuevo registro.
        /// </summary>
        private String Accion;

        /// <summary>
        /// Código del contacto seleccionado en "CatalogoInterventor.aspx" y se obtiene para realizar determinadas acciones.
        /// </summary>
        private String CodContacto_Seleccionado;

        /// <summary>
        /// Nombre del contacto seleccionado previamente en "CatalogoInterventor.aspx".
        /// </summary>
        private String NombreContactoSeleccionado;

        /// <summary>
        /// Código del contrato.
        /// </summary>
        private String CodContrato_Seleccionado;

        #endregion

        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Accion = HttpContext.Current.Session["Accion_Interventor"] != null ? Accion = HttpContext.Current.Session["Accion_Interventor"].ToString() : "";
            CodContacto_Seleccionado = HttpContext.Current.Session["CodContacto_Seleccionado"] != null ? CodContacto_Seleccionado = HttpContext.Current.Session["CodContacto_Seleccionado"].ToString() : "";
            NombreContactoSeleccionado = HttpContext.Current.Session["NombreContactoSeleccionado"] != null ? NombreContactoSeleccionado = HttpContext.Current.Session["NombreContactoSeleccionado"].ToString() : "";

            if (!IsPostBack)
            {
                //Establecer el título.
                if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                { this.Page.Title = "Administrar Coordinadores de Interventoria"; }
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                { this.Page.Title = "Administrar Interventores"; }

                try
                {
                    CargarDaysMonthsAndYears();
                    //Establecer fecha de selección en "Fecha de Inicio"
                    DateTime hoy = DateTime.Today;
                    FechaInicioDD.SelectedValue = hoy.Day.ToString();
                    FechaInicioMM.SelectedValue = hoy.Month.ToString();
                    FechaInicioYYYY.SelectedValue = hoy.Year.ToString();

                    FechaInicioDD.SelectedValue = hoy.Day.ToString();
                    FechaInicioMM.SelectedValue = hoy.AddMonths(1).ToString();
                    FechaInicioYYYY.SelectedValue = hoy.Year.ToString();

                    lbl_nmb_coord_interv.Text = NombreContactoSeleccionado;
                    lbl_nmb_coord_interv_mod.Text = NombreContactoSeleccionado;
                    if (Accion == "Nuevo") //Actualizar
                    {
                        lbl_fecha_expiracion.Visible = true;
                        dd_days_expiracion.Visible = true;
                        lbl_exp_1.Visible = true;
                        dd_months_expiracion.Visible = true;
                        lbl_exp_2.Visible = true;
                        dd_years_expiracion.Visible = true;
                        lbl_motivo_modificacion.Visible = true;
                        txt_Motivo.Visible = true;
                        tb_ver.Visible = true;

                        if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                        {
                            panel_expriracion.Visible = false;
                            panel_motivo.Visible = false;
                        }
                    }
                    else //Otro valor...
                    {
                        //tb_crear.Visible = false;
                        //tb_ver.Visible = true;
                        //lbl_fecha_expiracion.Visible = true;
                        //dd_days_expiracion.Visible = true;
                        //lbl_motivo_modificacion.Visible = true;
                        //dd_months_expiracion.Visible = true;
                        //dd_years_expiracion.Visible = true;
                        //txt_Motivo.Visible = true;

                        tb_crear.Visible = true;
                        tb_ver.Visible = false;
                        CargarInformacion_UsuarioHabilitado();
                        HttpContext.Current.Session["EDITMODE"] = "yes";
                    }
                }
                catch { Session.Clear(); Response.Redirect("~/Account/Login.aspx"); }
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 07/05/2014.
        /// Cargar los DropDownLists de Dias, Meses y Años.
        /// ACTUALIZACIÓN: Los meses vienen por defecto.
        /// </summary>
        private void CargarDaysMonthsAndYears()
        {
            try
            {
                //Cargar los días.
                for (int i = 1; i < 32; i++)
                {
                    ListItem item_days = new ListItem();
                    item_days.Value = i.ToString();
                    item_days.Text = i.ToString();
                    FechaInicioDD.Items.Add(item_days);
                    dd_days_expiracion.Items.Add(item_days);
                }

                //Los meses son por defecto.

                //Cargar los años. = Mayor información, buscar el método "private void GenerarFecha_Year()".
                int currentYear = DateTime.Today.AddYears(-11).Year;
                int futureYear = DateTime.Today.AddYears(5).Year;

                for (int i = currentYear; i < futureYear; i++)
                {
                    ListItem item_year = new ListItem();
                    item_year.Text = i.ToString();
                    item_year.Value = i.ToString();
                    FechaInicioYYYY.Items.Add(item_year);
                    dd_years_expiracion.Items.Add(item_year);
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error al cargar las listas de fecha.')", true);
                return;
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 08/05/2014.
        /// Flujo de código que establece valores en variables de acuerdo a las condiciones internas.
        /// </summary>
        /// <param name="opcional">Debe ser diferente de vacío para indicar que se vá a registrar un nuevo contrato al contacto.</param>
        private void Crear(string opcional = null)
        {
            //Inicializar variables.
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            bool ejecucion_correcta = false;
            String sqlConsulta = "";
            DateTime fechaInicio = new DateTime();

            DateTime fechaFin = new DateTime();

            Int32 MesesFinalizacion = 0; //Variable que contiene el valor digitado en el campo "Meses".

            #region Validaciones iniciales.
            //Verificar si la variable "CodContacto_Seleccionado" tiene datos.
            try { CodContacto_Seleccionado = Convert.ToString(usuario.IdContacto); }
            catch { return; }

            //Si el valor "opcional" es diferente de vacío, se le asigna el valor a la variable local "CodContacto_Seleccionado".
            try
            { if (opcional != null) { CodContacto_Seleccionado = opcional; } }
            catch { return; }

            //Obtener la fecha de inicio.
            try { fechaInicio = Convert.ToDateTime(FechaInicioYYYY.SelectedValue + "/" + FechaInicioMM.SelectedValue + "/" + FechaInicioDD.SelectedValue); }
            //fechaInicio = Convert.ToDateTime(FechaInicioYYYY.SelectedValue + FechaInicioMM.SelectedValue  + FechaInicioDD.SelectedValue);}
            catch { ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La fecha de inicio obtenida " + FechaInicioYYYY.SelectedValue + FechaInicioMM.SelectedValue + FechaInicioDD.SelectedValue + " no es válida.')", true); return; }

            //Obtener el valor correspondiente de meses.
            try
            {
                if (txt_Meses.Text.Trim() != "")
                { MesesFinalizacion = Convert.ToInt32(txt_Meses.Text); }
                else { ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe ingresar el(los) mes(es).')", true); return; }
            }
            catch { ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El mes ingresado " + txt_Meses.Text + " no es válido.')", true); return; }

            //Sumarle la fecha finalización los meses digitados.
            fechaFin = fechaInicio;

            fechaFin = fechaFin.AddMonths(MesesFinalizacion);
            #endregion

            try
            {

                try
                { Int32 a = Int32.Parse(txt_NumContrato.Text); }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El número del contrato debe ser un valor numérico.')", true);
                    return;
                }

                //Consulta #1:
                sqlConsulta = " SELECT * FROM InterventorContrato " +
                              " WHERE codContacto = " + CodContacto_Seleccionado + " AND NumContrato = " + txt_NumContrato.Text;

                //Asignar los resultados de la consulta #1:
                var tabla_1 = consultas.ObtenerDataTable(sqlConsulta, "text");

                //Si NO hay datos, inserta.
                if (tabla_1.Rows.Count == 0)
                {
                    //#region Inserción "Nueva Versión".
                    //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                    //try
                    //{
                    //    //NEW RESULTS:

                    //    cmd = new SqlCommand("MD_Create_InterventorContrato", con);

                    //    if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    cmd.Parameters.AddWithValue("@CodContacto", CodContacto_Seleccionado);
                    //    cmd.Parameters.AddWithValue("@NumContacto", txt_NumContrato.Text.Trim());
                    //    cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    //    cmd.Parameters.AddWithValue("@FechaExpiracion", fechaFin);
                    //    cmd.ExecuteNonQuery();
                    //    //con.Close();
                    //    //con.Dispose();
                    //    cmd.Dispose();
                    //}
                    //catch
                    //{
                    //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error en inserción.');", true);
                    //    return;
                    //}
                    //finally {
                    //    con.Close();
                    //    con.Dispose();
                    //}
                    //#endregion

                    var interveContrato = new Datos.InterventorContrato
                    {
                        CodContacto = int.Parse(CodContacto_Seleccionado),
                        numContrato = int.Parse(txt_NumContrato.Text.Trim()),
                        FechaInicio = fechaInicio,
                        FechaExpiracion = fechaFin
                    };

                    consultas.Db.InterventorContratos.InsertOnSubmit(interveContrato);
                    consultas.Db.SubmitChanges();
                }
                //Si hay datos, obtiene los valores de la consulta para asignarlos en las variables internas.
                if (tabla_1.Rows.Count > 0)
                {
                    #region Ya existe un contrato con ese número para este mismo Coordinador de Interventoría.
                    fechaInicio = Convert.ToDateTime(tabla_1.Rows[0]["FechaInicio"].ToString());
                    hdf_FechaInicio.Value = fechaInicio.ToString();
                    fechaFin = Convert.ToDateTime(tabla_1.Rows[0]["FechaExpiracion"].ToString());
                    hdf_FechaExpiracion.Value = fechaFin.ToString();
                    CodContacto_Seleccionado = tabla_1.Rows[0]["Id_InterventorContrato"].ToString();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Ya existe un contrato con ese número para este mismo Coordinador de Interventoría.')window.opener.location.reload();window.close();", true);
                    return;
                    #endregion
                }

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.reload();window.close();", true);
                return;
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error no controlado en la inserción.')", true);
                return;
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 08/05/2014.
        /// Actualizar el contrato y contacto seleccionado en la tabla "InterventorContrato".
        /// </summary>
        private void Actualizar()
        {
            //Inicializar variables.
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            bool ejecucion_correcta = false;
            String sqlConsulta = "";
            DateTime fechaInicio = new DateTime();
            DateTime fechaFin = new DateTime();

            #region Validaciones iniciales.

            //Verificar si la variable "CodContacto_Seleccionado" tiene datos.
            try { CodContacto_Seleccionado = HttpContext.Current.Session["CodContacto_Seleccionado"].ToString(); }
            catch { return; }

            //Obtener la fecha de inicio.
            try { fechaInicio = Convert.ToDateTime(FechaInicioDD.SelectedValue + "/" + FechaInicioMM.SelectedValue + "/" + FechaInicioYYYY.SelectedValue); }
            catch { ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La fecha de inicio obtenida " + FechaInicioDD.SelectedValue + FechaInicioMM.SelectedValue + FechaInicioYYYY.SelectedValue + " no es válida.')", true); return; }

            //Obtener la fecha de expiración.
            try { fechaFin = Convert.ToDateTime(dd_days_expiracion.SelectedValue + "/" + dd_months_expiracion.SelectedValue + "/" + dd_years_expiracion.SelectedValue); }
            catch { ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La fecha de expiración obtenida " + dd_days_expiracion.SelectedValue + dd_months_expiracion.SelectedValue + dd_years_expiracion.SelectedValue + " no es válida.')", true); return; }

            #endregion

            try
            {
                //#region Actualización "Nueva versión".
                //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                //try
                //{
                //    //NEW RESULTS:

                //    cmd = new SqlCommand("MD_Update_InterventorContrato", con);

                //    if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.AddWithValue("@Id_InterventorContrato", CodContacto_Seleccionado);
                //    cmd.Parameters.AddWithValue("@Motivo", txt_Motivo.Text.Trim());
                //    cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                //    cmd.Parameters.AddWithValue("@FechaExpiracion", fechaFin);
                //    cmd.ExecuteNonQuery();
                //    //con.Close();
                //    //con.Dispose();
                //    cmd.Dispose();
                //}
                //catch
                //{
                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error en actualización.');", true);
                //    return;
                //}
                //finally {
                //    con.Close();
                //    con.Dispose();
                
                //}
                //#endregion

                var intervenContrato = (from ic in consultas.Db.InterventorContratos
                                        where ic.Id_InterventorContrato == int.Parse(Session["IdIntervenContrato"].ToString())
                                        select ic).FirstOrDefault();

                intervenContrato.numContrato = int.Parse(txt_NumContrato.Text.Trim());
                intervenContrato.FechaInicio = fechaInicio;
                intervenContrato.FechaExpiracion = fechaFin;
                intervenContrato.Motivo = txt_Motivo.Text.Trim();

                //consultas.Db.InterventorContratos.InsertOnSubmit(intervenContrato);
                consultas.Db.SubmitChanges();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Contrato actualizado satisfactoriamente!'); window.opener.location.reload();window.close();", true);
                return;
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error no controlado en actualización.')", true);
                return;
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 08/05/2014.
        /// Editar la información "realmente hace una consulta y si tiene datos, los carga a variables internas y
        /// campos del fomurlario".
        /// </summary>
        private void Editar()
        {
            //Inicializar variables.
            String sqlConsulta = "";
            DateTime fechaInicio = new DateTime();
            DateTime fechaFin = new DateTime();

            try
            {
                //Consultar el Código del contacto seleccionado.
                CodContacto_Seleccionado = HttpContext.Current.Session["CodContacto_Seleccionado"].ToString();

                //Consulta:
                sqlConsulta = "SELECT * FROM InterventorContrato WHERE id_InterventorContrato = " + CodContacto_Seleccionado;

                //Asignar resultados de la consulta anterior a variable DataTable.
                var tabla_1 = consultas.ObtenerDataTable(sqlConsulta, "text");

                //Si hay datos, obtiene los valores de la consulta para asignarlos en las variables y campos del formulario.
                if (tabla_1.Rows.Count > 0)
                {
                    fechaInicio = Convert.ToDateTime(tabla_1.Rows[0]["FechaInicio"].ToString());
                    hdf_FechaInicio.Value = fechaInicio.ToString();
                    fechaFin = Convert.ToDateTime(tabla_1.Rows[0]["FechaExpiracion"].ToString());
                    hdf_FechaExpiracion.Value = fechaFin.ToString();
                    txt_NumContrato.Text = tabla_1.Rows[0]["numContrato"].ToString();
                    hdf_numContrato.Value = tabla_1.Rows[0]["numContrato"].ToString();
                    txt_Motivo.Text = tabla_1.Rows[0]["Motivo"].ToString();
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error no controlado en edición.')", true);
                return;
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 08/05/2014.
        /// Dependiendo del valor en sesión "Accion" se evalúa la acción a ejecutar.
        /// </summary>
        /// <param name="palabra">Acción a ejecutar.</param>
        private void EvaluarAccion(String palabra)
        {
            try
            {
                switch (palabra)
                {
                    case "Nuevo":
                        Crear();
                        break;

                    case "Actualizar":
                        lbl_fecha_expiracion.Visible = true;
                        dd_days_expiracion.Visible = true;
                        lbl_exp_1.Visible = true;
                        dd_months_expiracion.Visible = true;
                        lbl_exp_2.Visible = true;
                        dd_years_expiracion.Visible = true;
                        lbl_motivo_modificacion.Visible = true;
                        txt_Motivo.Visible = true;
                        Actualizar();
                        break;

                    case "Editar":
                        Editar();
                        break;

                    default:
                        hdf_FechaInicio.Value = DateTime.Today.ToString();
                        hdf_numContrato.Value = "";
                        break;
                }
            }
            catch (Exception ex)
            { string err = ex.Message; }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 08/05/2014.
        /// Crear el interventor para el contrato.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Crear_Click(object sender, EventArgs e)
        {
            if (btn_Crear.Text == "Crear")
            {
                //Crear datos.
                Crear(CodContacto_Seleccionado);
            }
            if (btn_Crear.Text == "Actualizar")
            {
                //Editar datos.
                Actualizar();
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 05/06/2014.
        /// Cargar la grilla del contacto "que fue seleccionado en (CatalogoInterventor.aspx)" y que estaba (Habilitado).
        /// </summary>
        private void CargarInformacion_UsuarioHabilitado()
        {
            //Inicializar variables.
            String txtSQL;
            DataTable tabla = new DataTable();

            try
            {
                //Consulta.
                //txtSQL = " SELECT numContrato, FechaInicio, FechaExpiracion " +
                txtSQL = " SELECT * " +
                         " FROM InterventorContrato where codcontacto = " + CodContacto_Seleccionado;

                //Asignar resultados de la consulta a variable DataTable.
                tabla = consultas.ObtenerDataTable(txtSQL, "text");

                //Añadir columnas que tendrán los DateTime formateados según FONADE clásico.
                tabla.Columns.Add("P_FechaInicio", typeof(System.String));
                tabla.Columns.Add("P_FechaExpiracion", typeof(System.String));

                string[] arr_palabras;
                string mes = "";

                //Formatear las fechas.
                foreach (DataRow row in tabla.Rows)
                {
                    //Fecha de inicio.
                    row["P_FechaInicio"] = Convert.ToDateTime(row["FechaInicio"].ToString()).ToString("d ' - ' MMM ' - ' yyyy");
                    arr_palabras = row["P_FechaInicio"].ToString().Trim().Split('-');
                    mes = UppercaseFirst(arr_palabras[1].Trim());
                    row["P_FechaInicio"] = arr_palabras[0] + " - " + mes + " - " + arr_palabras[2];

                    //Fecha de expiración.
                    row["P_FechaExpiracion"] = Convert.ToDateTime(row["FechaExpiracion"].ToString()).ToString("d ' - ' MMM ' - ' yyyy");
                    arr_palabras = row["P_FechaExpiracion"].ToString().Trim().Split('-');
                    mes = UppercaseFirst(arr_palabras[1].Trim());
                    row["P_FechaExpiracion"] = arr_palabras[0] + " - " + mes + " - " + arr_palabras[2];
                }

                //Asignar resultados de la consulta a DataTable que será a su vez asignado al GridView.
                HttpContext.Current.Session["dtEmpresas"] = tabla;
                gv_intv.DataSource = tabla;
                gv_intv.DataBind();
            }
            catch { }
        }

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
        /// RowCommand.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_intv_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "VerNumero":

                    //Instancia el LinkButton seleccionado.
                    LinkButton lnkbtn = e.CommandSource as LinkButton;

                    //Si es diferente de NULL, es porque hizo correctamente la instancia.
                    if (lnkbtn != null)
                    {
                        //Crear sub-variables de sesión para ejecutarlas en el método "EditarContrato".
                        string[] palabras = e.CommandArgument.ToString().Split(';');
                        HttpContext.Current.Session["p_cod_contacto"] = palabras[0];
                        HttpContext.Current.Session["p_num_contrato"] = palabras[1];
                        HttpContext.Current.Session["p_FechaInicio"] = palabras[2];

                        var interContrato = (from ic in consultas.Db.InterventorContratos
                                             where ic.Id_InterventorContrato == int.Parse(palabras[0])
                                             select ic).FirstOrDefault();

                        Session["IdIntervenContrato"] = interContrato.Id_InterventorContrato;

                        //Establecer valores.
                        btn_Crear.Text = "Actualizar";
                        tb_crear.Visible = false;
                        tb_ver.Visible = true;

                        //Establecer fecha de selección en "Fecha de Inicio".
                        DateTime fecha_inicio = Convert.ToDateTime(palabras[2]);
                        DateTime fechaExpira = (DateTime) interContrato.FechaExpiracion;
                        FechaInicioDD.SelectedValue = fecha_inicio.Day.ToString();
                        FechaInicioDD.Enabled = false;
                        FechaInicioMM.SelectedValue = fecha_inicio.Month.ToString();
                        FechaInicioMM.Enabled = false;
                        FechaInicioYYYY.SelectedValue = fecha_inicio.Year.ToString();
                        FechaInicioYYYY.Enabled = false;
                        lblMeses.Visible = false;
                        txt_Meses.Visible = false;
                        if(fechaExpira != null)
                        {
                            dd_days_expiracion.SelectedValue = fechaExpira.Day.ToString();
                            dd_months_expiracion.SelectedValue = fechaExpira.Month.ToString();
                            dd_years_expiracion.SelectedValue = fechaExpira.Year.ToString();
                        }

                        txt_Motivo.Text = interContrato.Motivo;

                        //Cargar datos.
                        txt_NumContrato.Text = interContrato.numContrato.ToString(); // palabras[1];
                        txt_NumContrato.Enabled = false;

                        //Habilitar campos.
                        lbl_fecha_expiracion.Visible = true;
                        dd_days_expiracion.Visible = true;
                        dd_days_expiracion.Enabled = true;
                        lbl_exp_1.Visible = true;
                        dd_months_expiracion.Visible = true;
                        dd_months_expiracion.Enabled = true;
                        lbl_exp_2.Visible = true;
                        dd_years_expiracion.Visible = true;
                        dd_years_expiracion.Enabled = true;
                        lbl_motivo_modificacion.Visible = true;
                        txt_Motivo.Visible = true;
                        txt_Motivo.Enabled = true;
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 05/06/2014.
        /// Ingresar nuevo contrato...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void img_btn_Add_Click(object sender, ImageClickEventArgs e)
        {
            LimpiarCampos();
            tb_crear.Visible = false;
            tb_ver.Visible = true; /*EvaluarAccion("Crear");*/
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 05/06/2014.
        /// Ingresar nuevo contrato...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnk_btn_Add_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            txt_NumContrato.Enabled = true;
            tb_crear.Visible = false;
            tb_ver.Visible = true; /*EvaluarAccion("Crear");*/
        }

        protected void txt_Motivo_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 14/07/2014.
        /// Lista de los contratos creados...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Lista_Click(object sender, EventArgs e)
        {
            try
            {
                tb_crear.Visible = true;
                tb_ver.Visible = false;
                #region Comentarios.
                //txtSQL = " SELECT * FROM InterventorContrato where codcontacto = " + usuario.IdContacto;

                //tabla = consultas.ObtenerDataTable(txtSQL,"text");

                //gv_intv.DataSource = tabla;
                //gv_intv.DataBind(); 
                #endregion
                CargarInformacion_UsuarioHabilitado();
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error no controlado al cargar la lista.')", true);
                return;
            }
        }

        /// <summary>
        /// Limpiar campos.
        /// </summary>
        private void LimpiarCampos()
        {
            DateTime hoy = DateTime.Today;
            FechaInicioDD.SelectedValue = hoy.Day.ToString();
            FechaInicioMM.SelectedValue = hoy.Month.ToString();
            FechaInicioYYYY.SelectedValue = hoy.Year.ToString();
            lbl_nmb_coord_interv.Text = NombreContactoSeleccionado;
            lbl_nmb_coord_interv_mod.Text = NombreContactoSeleccionado;
            txt_NumContrato.Text = "";
            txt_Meses.Text = "";
            txt_Motivo.Text = "";
        }

        #endregion
    }
}