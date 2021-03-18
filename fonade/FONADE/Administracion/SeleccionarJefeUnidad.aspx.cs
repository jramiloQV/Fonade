using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI.WebControls;
using Datos;
using Fonade.Negocio;
using System.Configuration;
using System.Web.UI;
using Fonade.Clases;
using System.Text.RegularExpressions;
using System.Web;

namespace Fonade.FONADE.Administracion
{
    /// <summary>
    /// SeleccionarJefeUnidad
    /// </summary>    
    public partial class SeleccionarJefeUnidad : Base_Page
    {
        const string Emailpattern = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"; //Mauricio Arias Olave.
        //const string Emailpattern = @"/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/"; //FONADE clásico.

        #region Variables globales.

        /// <summary>
        /// Variable de sesión que contiene el CodInstitucion que recibe de la página "CatalogoUnidadEmprende.aspx",
        /// es decir, el Id_Institucion de la unidad de emprendimiento seleccionada.
        /// </summary>
        private Int32 CodInstitucionNueva;

        /// <summary>
        /// Variable de sesión que contiene el CodCiudad que recibe de la página "CatalogoUnidadEmprende.aspx".
        /// </summary>
        private Int32 CodCiudad_JefeUnidad;

        /// <summary>
        /// Variable de sesión que contiene el Id_Departamento que recibe de la página "CatalogoUnidadEmprende.aspx".
        /// </summary>
        private Int32 CodDepartamento_JefeUnidad;

        /// <summary>
        /// Variable de sesión que contiene el Id_Contacto "JEFE UNIDAD" que recibe de la página "CatalogoUnidadEmprende.aspx"
        /// y puede ser reemplazado con el valor del id del nuevo usuario creado.
        /// </summary>
        private Int32 CodJefeUnidadSeleccionado;

        #endregion

        /// <summary>
        /// Mauricio Arias Olave.
        /// 02/05/2014.
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    lbl_enunciado.Text = "JEFE UNIDAD DE EMPRENDIMIENTO";
                    CodInstitucionNueva = HttpContext.Current.Session["CodInstitucionNueva"] != null ? CodInstitucionNueva = Convert.ToInt32(HttpContext.Current.Session["CodInstitucionNueva"].ToString()) : 0;
                    CodCiudad_JefeUnidad = HttpContext.Current.Session["CodCiudad_JefeUnidad"] != null ? CodInstitucionNueva = Convert.ToInt32(HttpContext.Current.Session["CodCiudad_JefeUnidad"].ToString()) : 0;
                    CodDepartamento_JefeUnidad = HttpContext.Current.Session["CodDepartamento_JefeUnidad"] != null ? CodInstitucionNueva = Convert.ToInt32(HttpContext.Current.Session["CodDepartamento_JefeUnidad"].ToString()) : 0;
                    CodJefeUnidadSeleccionado = HttpContext.Current.Session["CodNuevoJefe_Seleccionado"] != null ? CodJefeUnidadSeleccionado = Convert.ToInt32(HttpContext.Current.Session["CodNuevoJefe_Seleccionado"].ToString()) : 0;
                    CargarTiposDeIdentificacion();
                    CargarDepartamentos();
                    CargarCiudades();
                    CargarDatosIniciales();

                    // Esto para configurar el mensaje de confirmacion del boton
                    string[] a = lnk_jefeSeleccionable.CommandArgument.ToString().Split(';');
                    string b = lnk_jefeSeleccionable.CommandName;

                    if (a[0].Equals("Asesor"))
                    {
                        btn_enviar_cambio_jefe_unidad_ConfirmButtonExtender.ConfirmText = Texto("TXT_ADVERTENCIA_PROYECTOSHUERFANOS");
                    }
                    else
                    {
                        btn_enviar_cambio_jefe_unidad_ConfirmButtonExtender.ConfirmText = Texto("TXT_ADVERTENCIA_CAMBIOROLUSUARIO");
                    }

                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        #region Métodos generales.

        /// <summary>
        /// Mauricio Arias Olave.
        /// 24/04/2014.
        /// Dependiendo de qué panel está visible, se establecen ciertos valores.
        /// </summary>
        private void EvaluarEnunciado()
        {
            if (pnlPrincipal.Visible == true)
            {
                pnlPrincipal.Visible = true;
                pnl_new_user.Visible = false;
                pnl_Resultado.Visible = false;
            }
            if (pnl_new_user.Visible == true)
            {
                pnl_new_user.Visible = true;
                pnlPrincipal.Visible = false;
                pnl_Resultado.Visible = false;
            }
            if (pnl_Resultado.Visible == true)
            {
                pnl_Resultado.Visible = true;
                pnlPrincipal.Visible = false;
                pnl_new_user.Visible = false;
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 30/04/2014.
        /// Cargar el listado de tipos de identificación al DropDownList "dd_listado_TipoIdentificacion".
        /// </summary>
        private void CargarTiposDeIdentificacion()
        {
            //Inicializar variables.
            String sqlConsulta;

            try
            {
                //Consulta 
                sqlConsulta = " SELECT Id_TipoIdentificacion, NomTipoIdentificacion " +
                              " FROM TipoIdentificacion " +
                              " ORDER BY NomTipoIdentificacion";

                var datos = consultas.ObtenerDataTable(sqlConsulta, "text");

                if (datos.Rows.Count > 0)
                {
                    for (int i = 0; i < datos.Rows.Count; i++)
                    {
                        ListItem item = new ListItem();
                        item.Value = datos.Rows[i]["Id_TipoIdentificacion"].ToString();
                        item.Text = datos.Rows[i]["NomTipoIdentificacion"].ToString();
                        dd_listado_TipoIdentificacion.Items.Add(item);
                    }
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo cargar la lista de tipos de identificación.')", true);
                return;
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 30/04/2014.
        /// Cargar el listado de departamentos al DropDownList "dd_SelDpto2".
        /// </summary>
        private void CargarDepartamentos()
        {
            //Inicializar variables.
            String sqlConsulta;

            try
            {
                //Consulta 
                sqlConsulta = "SELECT id_Departamento, NomDepartamento From Departamento ORDER BY NomDepartamento";

                var datos = consultas.ObtenerDataTable(sqlConsulta, "text");

                if (datos.Rows.Count > 0)
                {
                    //for (int i = 0; i < datos.Rows.Count; i++)
                    //{
                    //    ListItem item = new ListItem();
                    //    item.Value = datos.Rows[i]["id_Departamento"].ToString();
                    //    item.Text = datos.Rows[i]["NomDepartamento"].ToString();
                    //    dd_SelDpto2_Unidad.Items.Add(item);
                    //}

                    dd_SelDpto2_Unidad.DataSource = datos;
                    dd_SelDpto2_Unidad.DataTextField = "NomDepartamento";
                    dd_SelDpto2_Unidad.DataValueField ="id_Departamento";
                    dd_SelDpto2_Unidad.DataBind();
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo cargar la lista de departamentos.')", true);
                return;
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 30/04/2014.
        /// Cargar el listado de ciudades al DropDownList "dd_Ciudades".
        /// <param name="codDepto">Código del departamento seleccionado.</param>
        /// </summary>
        private void CargarCiudades(Int32 codDepto = 0)
        {
            //Inicializar variables.
            String sqlConsulta;

            try
            {
                if (codDepto == 0)
                { return; }
                else
                {
                    //Vaciar los elementos que podría tener.
                    dd_Ciudades_Unidad.Items.Clear();

                    //Consulta 
                    sqlConsulta = "SELECT Id_Ciudad, NomCiudad FROM Ciudad WHERE CodDepartamento = " + codDepto;

                    var datos = consultas.ObtenerDataTable(sqlConsulta, "text");

                    if (datos.Rows.Count > 0)
                    {
                        for (int i = 0; i < datos.Rows.Count; i++)
                        {
                            ListItem item = new ListItem();
                            item.Value = datos.Rows[i]["Id_Ciudad"].ToString();
                            item.Text = datos.Rows[i]["NomCiudad"].ToString();
                            dd_Ciudades_Unidad.Items.Add(item);
                        }
                    }
                    else
                    {
                        //Se coloca valor por defecto porque no hay elementos.
                        ListItem item_default = new ListItem();
                        item_default.Value = "0";
                        item_default.Text = "No se hallaron ciudades";
                        item_default.Enabled = false;
                        dd_Ciudades_Unidad.Items.Add(item_default);
                    }
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo cargar la lista de departamentos.')", true);
                return;
            }
        }

        #endregion

        #region Métodos especiales.

        /// <summary>
        /// Mauricio Arias Olave.
        /// 28/04/2014.
        /// Método usado en "DeclaraVariables.inc" de FONADE Clásico.
        /// Usado para obtener el valor "Texto" de la tabla "Texto", este valor será usado en la creación
        /// de mensajes cuando el CheckBox "chk_actualizarInfo" esté chequeado; Si el resultado de la consulta
        /// NO trae datos, según FONADE Clásico, crea un registro con la información dada.
        /// </summary>
        /// <param name="NomTexto">Nombre del texto a consultar.</param>
        /// <returns>NomTexto consultado.</returns>
        private string Texto(String NomTexto)
        {
            //Inicializar variables.
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            //String RSTexto;
            String txtSQL;
            bool correcto = false;

            //Consulta
            txtSQL = "SELECT Texto FROM Texto WHERE NomTexto='" + NomTexto + "'";

            var resultado = consultas.ObtenerDataTable(txtSQL, "text");

            if (resultado.Rows.Count > 0)
                return resultado.Rows[0]["Texto"].ToString();
            else
            {
                #region Si no existe la palabra "consultada", la crea.

                txtSQL = "INSERT INTO Texto (NomTexto, Texto) VALUES ('" + NomTexto + "','" + NomTexto + "')";

                //Asignar SqlCommand para su ejecución.
                cmd = new SqlCommand(txtSQL, conn);

                //Ejecutar SQL.
                correcto = EjecutarSQL(conn, cmd);

                if (correcto == false)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error en inserción de TEXTO.')", true);
                    return NomTexto; //""; //Debería retornar vacío y validar en el método donde se llame si esté validado.
                }
                else
                { return NomTexto; }

                #endregion
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 02/05/2014.
        /// Revisar si ya existe funcionalidad que genere números al azar como indica la línea
        /// 590 de "DeclaraVariables.inc" de FONADE clásico.
        /// </summary>
        /// <returns></returns>
        private Int32 GeneraClave()
        { return 0; }

        #endregion

        #region Eventos de los DropDownLists.

        /// <summary>
        /// Mauricio Arias Olave.
        /// 30/04/2014.
        /// Al seleccionar el departamento, se generará el listado de ciudades correspondientes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dd_SelDpto2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            { int valor_depto = Convert.ToInt32(dd_SelDpto2_Unidad.SelectedValue); CargarCiudades(valor_depto); }
            catch { CargarCiudades(0); }
        }

        #endregion

        /// <summary>
        /// Mauricio Arias Olave.
        /// 02/05/2014.
        /// Validar la información registrada en los campos del fomrulario antes de realizar la búsqueda.
        /// </summary>
        /// <returns>Cadena vacía = Pasó las validaciones. // Cadena llena = No pasó las validaciones.</returns>
        private string ValidarBusqueda()
        {
            //Inicializar variables.
            string validado = "";

            try
            {
                if (txt_numIdentificacion.Text.Trim() != "")
                {
                    try { int dato = Convert.ToInt32(txt_numIdentificacion.Text.Trim()); }
                    catch { validado = Texto("TXT_IDENT_NUM"); txt_numIdentificacion.Text = ""; return validado; }
                }
                else
                {
                    validado = Texto("TXT_IDENT_NUM_REQ");
                    return validado;
                }

                return validado;

            }
            catch { return "ERROR"; }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 02/05/2014.
        /// Trascripción de la función "validarDatosJefe" usado en FONADE clásico.
        /// </summary>
        /// <returns>Cadena vacía = Pasó las validaciones. // Cadena llena = No pasó las validaciones.</returns>
        private string ValidarDatosJefe()
        {
            //Inicializar variables.
            string validado = "";

            try
            {
                //Nombres.
                if (txt_Nombres.Text.Trim() == "")
                { validado = Texto("TXT_NOMBRE_REQ"); return validado; }

                //Apellidos.
                if (txt_Apellidos.Text.Trim() == "")
                { validado = Texto("TXT_APELLIDO_REQ"); return validado; }

                //Validar Email.
                if (txt_Email.Text.Trim() != "")
                {
                    if (!Regex.IsMatch(txt_Email.Text.Trim(), Emailpattern))
                    { validado = Texto("TXT_EMAIL_INV"); return validado; }
                }
                else
                { validado = Texto("TXT_EMAIL_REQ"); return validado; }

                //Departamento.
                try
                {
                    if (dd_SelDpto2_Unidad.SelectedValue == "")
                    { validado = Texto("TXT_DEPARTAMENTO_REQ"); return validado; }
                }
                catch { validado = Texto("TXT_DEPARTAMENTO_REQ"); return validado; }

                //Ciudad.
                try
                {
                    if (dd_Ciudades_Unidad.SelectedValue == "")
                    { validado = Texto("TXT_CIUDAD_REQ"); return validado; }
                }
                catch { validado = Texto("TXT_CIUDAD_REQ"); return validado; }

                //Teléfono
                if (txt_Telefono_Unidad.Text.Trim() == "")
                { validado = Texto("TXT_TELEFONOUNIDAD_REQ"); return validado; }

                //Fax
                if (txt_fax_Unidad.Text.Trim() == "")
                { validado = Texto("TXT_FAXUNIDAD_REQ"); return validado; }

                //Sitio web
                if (txt_website_Unidad.Text.Trim() == "")
                { validado = Texto("TXT_WEBSITE_REQ"); return validado; }

                //Cargo
                if (txt_NombreCargo_NEWUSER.Text.Trim() == "")
                { validado = Texto("TXT_CARGO_REQ"); return validado; }

                //Teléfono contacto.
                if (txt_Telefono_NEWUSER.Text.Trim() == "")
                { validado = Texto("TXT_TELEFONOCONTACTO_REQ"); return validado; }

                //Fax contacto "COMENTADO EN FONADE CLÁSICO, SE DEJA COMENTADO EN CÓDIGO C# PARA FUTURAS REFERENCIAS.
                if (txt_Fax_NEWUSER.Text.Trim() == "")
                { validado = Texto("TXT_FAXCONTACTO_REQ"); return validado; }

                return validado;
            }
            catch { return validado; }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// Establecer la selección de los DropDownLists según las variables obtenidas
        /// de sesión, depuradas en el Page_Load y usadas en este método.
        /// </summary>
        private void CargarDatosIniciales()
        {
            try
            {
                if (dd_SelDpto2_Unidad.Items.Count > 0)
                {
                    if (CodDepartamento_JefeUnidad != 0)
                    { dd_SelDpto2_Unidad.SelectedValue = CodDepartamento_JefeUnidad.ToString(); }
                }

                if (dd_Ciudades_Unidad.Items.Count > 0)
                {
                    if (CodCiudad_JefeUnidad != 0)
                    { dd_Ciudades_Unidad.SelectedValue = CodCiudad_JefeUnidad.ToString(); }
                }
            }
            catch { Session.Abandon(); Response.Redirect("~/Account/Login.aspx"); /*Error no controlado.*/ }
        }

        // ------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Mauricio Arias Olave.
        /// 02/05/2014.
        /// Enviar el jefe unidad seleccionado en la variable "codJefeUnidadAnterior".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_enviar_cambio_jefe_unidad_Click(object sender, EventArgs e)
        {
            //Inicializar variables.
            String sqlConsulta = "";
            DataTable RS = new DataTable();
            DataTable rstRol = new DataTable();

            try
            {
                string validado = "";
                validado = ValidarBusqueda();

                if (validado.Trim() == "")
                {
                    #region PARA QUÉ?
                    ////Obtener el código del jefe unidad seleccionado previamente.
                    //try { CodJefeUnidadSeleccionado = Convert.ToInt32(HttpContext.Current.Session["CodNuevoJefe_Seleccionado"].ToString()); }
                    //catch { return; } 
                    #endregion

                    //Mostrar el panel detalles que es el resultado de la búsqueda.
                    //NOTA: Para comportarse como FONADE clásico, se coloca TOP 1 para que cargue un solo valor.
                    sqlConsulta = " SELECT TOP 1 * FROM Contacto WHERE CodTipoIdentificacion = " + dd_listado_TipoIdentificacion.SelectedValue +
                                  " AND Identificacion = " + txt_numIdentificacion.Text.Trim();

                    //Asignar los resultados de la consulta anterior a la variable DataTable.
                    RS = consultas.ObtenerDataTable(sqlConsulta, "text");

                    //Si la consulta generó datos...
                    if (RS.Rows.Count > 0)
                    {
                        //Por cada resultado encontrado se hace un ciclo para consultas mas información
                        //y generar los enlaces para la selección del jefe unidad.
                        for (int i = 0; i < RS.Rows.Count; i++)
                        {
                            //Realizar la siguiente consulta:
                            sqlConsulta = " SELECT G.NomGrupo FROM Grupo G, GrupoContacto GC " +
                                          " WHERE GC.CodContacto = " + RS.Rows[i]["Id_Contacto"].ToString() + //+ CodJefeUnidadSeleccionado +
                                          " AND G.Id_Grupo = GC.CodGrupo";

                            //Asignar resultados de la consulta a la variable DataTable.
                            rstRol = consultas.ObtenerDataTable(sqlConsulta, "text");

                            //Si contiene datos...
                            if (rstRol.Rows.Count > 0)
                            {
                                //Realizar ciclo para generar LinkButton dinámicos según la cantidad de resultados.
                                for (int j = 0; j < rstRol.Rows.Count; j++)
                                {
                                    #region Si el Nombre de grupo de la consulta anterior es "Asesor" genera los LinkButton.
                                    if (rstRol.Rows[j]["NomGrupo"].ToString() == "Asesor")
                                    {
                                        #region Si el Id_Institucion es igual al valor "CodInstitucion" encontrado en la primera consulta.
                                        if (CodInstitucionNueva.ToString() == RS.Rows[i]["CodInstitucion"].ToString())
                                        {
                                            #region Generar LinkButton con los datos y su reenvío.

                                            lnk_jefeSeleccionable.Text = "";
                                            lnk_jefeSeleccionable.Text = RS.Rows[i]["Id_Contacto"].ToString() + " - " + RS.Rows[i]["Nombres"].ToString() + " " + RS.Rows[i]["Apellidos"].ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;" + rstRol.Rows[j]["NomGrupo"].ToString();
                                            lnk_jefeSeleccionable.ForeColor = System.Drawing.Color.Black;
                                            lnk_jefeSeleccionable.CommandArgument = RS.Rows[i]["Id_Contacto"].ToString() + ";" + rstRol.Rows[j]["NomGrupo"].ToString();
                                            lnk_jefeSeleccionable.CommandName = "SELECCIONABLE"; //RS.Rows[i]["Nombres"].ToString() + " " + RS.Rows[i]["Apellidos"].ToString();
                                            //Se muestra los datos.
                                            pnl_Resultado.Visible = true;
                                            TB_JefeSeleccionable.Visible = true;
                                            TB_Jefe_NO_seleccionable.Visible = false;
                                            pnl_new_user.Visible = false;
                                            pnlPrincipal.Visible = false;
                                            EvaluarEnunciado();

                                            #endregion
                                        }
                                        else
                                        {
                                            #region Mostrar que el usuario pertenece a otra institución y no puede ser seleccionado.

                                            lnk_jefeSeleccionable.Text = RS.Rows[i]["Id_Contacto"].ToString() + " - " + RS.Rows[i]["Nombres"].ToString() + " " + RS.Rows[i]["Apellidos"].ToString();
                                            lnk_jefeSeleccionable.Text = rstRol.Rows[j]["NomGrupo"].ToString();
                                            lbl_usuarioEsRol.Text = "El usuario pertenece a otra institución y no puede ser cambiado.";
                                            //Se muestra los datos.
                                            pnl_Resultado.Visible = true;
                                            TB_JefeSeleccionable.Visible = false;
                                            TB_Jefe_NO_seleccionable.Visible = true;
                                            pnl_new_user.Visible = false;
                                            pnlPrincipal.Visible = false;
                                            EvaluarEnunciado();

                                            #endregion
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region Se valida la misma condición dos veces...
                                        //Si el Id_Institucion es igual al valor "CodInstitucion" encontrado en la primera consulta...
                                        if (CodInstitucionNueva.ToString() == RS.Rows[i]["CodInstitucion"].ToString())
                                        {
                                            #region Mostrar que el usuario es del rol "..." y no puede ser seleccionado.

                                            lnk_jefeSeleccionable.Text = RS.Rows[i]["Id_Contacto"].ToString() + " - " + RS.Rows[i]["Nombres"].ToString() + " " + RS.Rows[i]["Apellidos"].ToString();
                                            lnk_jefeSeleccionable.Text = rstRol.Rows[j]["NomGrupo"].ToString();
                                            lbl_usuarioEsRol.Text = "El usuario ya es " + rstRol.Rows[j]["NomGrupo"].ToString() + " y no puede ser cambiado.";

                                            //Se muestra los datos.
                                            pnl_Resultado.Visible = true;
                                            TB_JefeSeleccionable.Visible = false;
                                            TB_Jefe_NO_seleccionable.Visible = true;
                                            pnl_new_user.Visible = false;
                                            pnlPrincipal.Visible = false;
                                            EvaluarEnunciado();

                                            #endregion
                                        }
                                        else
                                        {
                                            #region Mostrar que el usuario ya es "..." en otra institución y no puede ser cambiado.

                                            lnk_jefeSeleccionable.Text = RS.Rows[i]["Id_Contacto"].ToString() + " - " + RS.Rows[i]["Nombres"].ToString() + " " + RS.Rows[i]["Apellidos"].ToString();
                                            lnk_jefeSeleccionable.Text = rstRol.Rows[j]["NomGrupo"].ToString();
                                            lbl_usuarioEsRol.Text = "El usuario ya es " + rstRol.Rows[j]["NomGrupo"].ToString() + " en otra institución y no puede ser cambiado.";

                                            //Se muestra los datos.
                                            pnl_Resultado.Visible = true;
                                            TB_JefeSeleccionable.Visible = false;
                                            TB_Jefe_NO_seleccionable.Visible = true;
                                            pnl_new_user.Visible = false;
                                            pnlPrincipal.Visible = false;
                                            EvaluarEnunciado();

                                            #endregion
                                        }
                                        #endregion
                                    }
                                    #endregion
                                }
                            }
                            else
                            {
                                #region Mostrar el resultado de la consulta en el método "Texto("TXT_USUARIO_SIN_ROL_ASIGNADO")".

                                lnk_jefeSeleccionable.Text = "";
                                lnk_jefeSeleccionable.Text = RS.Rows[i]["Identificacion"].ToString() + " - " + RS.Rows[i]["Nombres"].ToString() + " " + RS.Rows[i]["Apellidos"].ToString();
                                lbl_rol_jefe_seleccionable.Text = Texto("TXT_USUARIO_SIN_ROL_ASIGNADO");
                                lnk_jefeSeleccionable.ForeColor = System.Drawing.Color.Black;
                                lnk_jefeSeleccionable.CommandArgument = RS.Rows[i]["Id_Contacto"].ToString() + ";" + "SINROL";
                                lnk_jefeSeleccionable.CommandName = "SINROL";

                                //Se muestra los datos.
                                pnl_Resultado.Visible = true;
                                TB_JefeSeleccionable.Visible = true;
                                TB_Jefe_NO_seleccionable.Visible = false;
                                pnl_new_user.Visible = false;
                                pnlPrincipal.Visible = false;
                                EvaluarEnunciado();

                                #endregion
                            }
                        }
                    }
                    else
                    {
                        //Se muestra el panel para agregar un nuevo jefe de unidad.
                        pnl_new_user.Visible = true;
                        pnlPrincipal.Visible = false;
                        pnl_Resultado.Visible = false;
                        TB_JefeSeleccionable.Visible = false;
                        TB_Jefe_NO_seleccionable.Visible = false;
                        EvaluarEnunciado();
                    }

                    //TXT_IDENT_NUM_REQ
                    //Session["codJefeUnidadAnterior"] = 000;//aaa
                    //CodJefeUnidadSeleccionado
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + validado + "')", true);
                    return;
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Hubo un inconveniente al realizar la búsqueda')", true);
                return;
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 02/05/2014.
        /// Evento OnClick dinámico, asignado dinámicamente a los resultados de búsqueda de jefes de unidad.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnk_enviar_Click(object sender, EventArgs e)
        {
            //Separar el nombre del linkbutton que fue accionado para obtener los valores a
            //ser usados en la página "CatalogoUnidaEmprende.aspx".

            LinkButton lnk_sender = (LinkButton)sender;

            string[] palabras = lnk_sender.CommandArgument.Split(';');
            String partes = lnk_sender.ID;
            String ID = palabras[0];
            String Rol = palabras[1];
            ClientScriptManager cm = this.ClientScript;

            //Evaluar el CommandName del LinkButton "para verificar si el usuario tiene o no rol, y si es seleccionable".
            switch (lnk_sender.CommandName)
            {
            case "SINROL":
                //Mostrar mensaje de que el usuario NO tiene rol y se continúa el proceso.
                HttpContext.Current.Session["CodNuevoJefe_Seleccionado"] = ID;
                cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>window.opener.location=window.opener.location;window.close();</script>");
                break;
            case "SELECCIONABLE":
                //Seleccionar el ID del Jefe de Unidad, cerrar la ventana, recargar, etc...
                HttpContext.Current.Session["CodNuevoJefe_Seleccionado"] = ID;
                cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>window.opener.location=window.opener.location;window.close();</script>");
                break;
            default:
                break;
            }

            //return;
        }

        #region Métodos SQL.

        /// <summary>
        /// Mauricio Arias Olave.
        /// Ejecutar SQL.
        /// Método que recibe la conexión y la consulta SQL y la ejecuta.
        /// </summary>
        /// <param name="p_connection">Conexión</param>
        /// <param name="p_cmd">Consulta SQL.</param>
        /// <returns>TRUE = Sentencia SQL ejecutada correctamente. // FALSE = Error.</returns>
        private bool EjecutarSQL(SqlConnection p_connection, SqlCommand p_cmd)
        {
            //Ejecutar controladamente la consulta SQL.
            try
            {
                p_connection.Open();
                p_cmd.ExecuteReader();
                //p_connection.Close();
                return true;
            }
            catch (Exception) { return false; }
            finally
            { p_connection.Close(); p_connection.Dispose(); }
        }

        #endregion

        /// <summary>
        /// Mauricio Arias Olave.
        /// 02/05/2014.
        /// Crear nuevo jefe.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_crearJefeUnidad_Click(object sender, EventArgs e)
        {
            //Inicializar variables.
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            String sqlConsulta = "";
            //bool ejecucion_correcta = false;

            try
            {
                //Consultar que no exista un jefe unidad con el mismo correo.
                sqlConsulta = "SELECT Email FROM Contacto WHERE Email LIKE '%" + txt_Email.Text.Trim() + "%'";

                //Asignar resultados de la consulta anterior a variable DataTable.
                var sql_1 = consultas.ObtenerDataTable(sqlConsulta, "text");

                //Si está vacío, inserta.
                if (sql_1.Rows.Count == 0)
                {
                    #region Comentarios NO BORRAR!
                    ////Inserción.
                    //sqlConsulta = "INSERT INTO Contacto (Nombres, Apellidos, CodTipoIdentificacion, Identificacion, Email, " +
                    //              " Clave, CodCiudad, Telefono, Fax, Cargo, fechaActualizacion)" +
                    //              " VALUES('" + txt_Nombres + "','" + txt_Apellidos.Text.Trim() + "'," + Convert.ToInt32(dd_listado_TipoIdentificacion.SelectedValue) + "," +
                    //              Convert.ToInt32(txt_numIdentificacion.Text.Trim()) + ",'" + txt_Email.Text.Trim() + "','" + GeneraClave() + "'"
                    //                  + " , '" + Convert.ToInt32(dd_Ciudades_Unidad.SelectedValue) + "', '" + txt_Telefono_NEWUSER.Text.Trim() + "','" +
                    //                  txt_Fax_NEWUSER.Text.Trim() + "','" + txt_NombreCargo_NEWUSER.Text.Trim() + "', GETDATE()" + ")";

                    ////Asignar SqlCommand para su ejecución.
                    //cmd = new SqlCommand(sqlConsulta, conn);

                    ////Ejecutar inserción.
                    //ejecucion_correcta = EjecutarSQL(conn, cmd);

                    //if (ejecucion_correcta == false)
                    //{
                    //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error en inserción de nuevo jefe (1).')", true);
                    //    return;
                    //}
                    //else
                    //{ 
                    #endregion

                    #region Inserción.
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                    try
                    {
                        //NEW RESULTS:
                        
                        cmd = new SqlCommand("MD_Create_JefeUnidad_Admin", con);

                        if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Nombres", txt_Nombres.Text.Trim());
                        cmd.Parameters.AddWithValue("@Apellidos", txt_Apellidos.Text.Trim());
                        cmd.Parameters.AddWithValue("@CodTipoIdentificacion", Convert.ToInt32(dd_listado_TipoIdentificacion.SelectedValue));
                        cmd.Parameters.AddWithValue("@Identificacion", Convert.ToInt32(txt_numIdentificacion.Text.Trim()));
                        cmd.Parameters.AddWithValue("@Email", txt_Email.Text.Trim());
                        cmd.Parameters.AddWithValue("@Clave", GeneraClave());
                        cmd.Parameters.AddWithValue("@CodCiudad", Convert.ToInt32(dd_Ciudades_Unidad.SelectedValue));
                        cmd.Parameters.AddWithValue("@Telefono", txt_Telefono_NEWUSER.Text.Trim());
                        cmd.Parameters.AddWithValue("@Fax", txt_Fax_NEWUSER.Text.Trim());
                        cmd.Parameters.AddWithValue("@Cargo", txt_NombreCargo_NEWUSER.Text.Trim());


                        cmd.ExecuteNonQuery();
                        //con.Close();
                        //con.Dispose();
                        cmd.Dispose();
                    }
                    catch { }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                    }

                    #endregion

                    //Obtener el Id del nuevo jefe ingresado.
                    sqlConsulta = "SELECT Id_Contacto FROM Contacto WHERE CodTipoIdentificacion = " + Convert.ToInt32(dd_listado_TipoIdentificacion.SelectedValue) + " AND Identificacion = " + Convert.ToInt32(txt_numIdentificacion.Text.Trim());

                    var sql_2 = consultas.ObtenerDataTable(sqlConsulta, "text");

                    HttpContext.Current.Session["CodNuevoJefe_Seleccionado"] = sql_2.Rows[0]["Id_Contacto"].ToString();

                    //1. Recargar la página principal. 
                    //2. Cargar el valor "CodNuevoJefe_Seleccionado" en la página padre como "Jefe Unidad".
                    //3. Cerrar la ventana actual "SeleccionarJefeUnidad.aspx".
                    //4. Continuar con la edición.

                    //Preguntar a Mónica cuáles usuarios "estarían" disponibles en la búsqueda para
                    //asignarlos como nuevos jefes de unidad.

                    ClientScriptManager cm = this.ClientScript;
                    cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>window.opener.location=window.opener.location;window.close();</script>");

                }
                else
                {
                    string validado = "";
                    validado = Texto("TXT_EMAIL_REPETIDO");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + validado + ".')", true);
                    return;
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo crear el nuevo jefe.')", true);
                return;
            }
        }

        private void Retornar(String Rol)
        {
            if (Rol == "Asesor")
            {
                //Mostrar mensaje de confirmación:
                //TXT_ADVERTENCIA_PROYECTOSHUERFANOS

                //Si le dá "ACEPTAR" al mensaje anterior, debe mostrar este otro mensaje.
                //TXT_ADVERTENCIA_CAMBIOROLUSUARIO

                //Si acepta finalmente el mensaje, cierra la ventana con los valores cargados.
            }
            else
            {
                //Mostrar mensaje de confirmación:
                //TXT_ADVERTENCIA_CAMBIOROLUSUARIO

                //Si le dá "ACEPTAR" al mensaje, cierra la ventana con los valores cargados.
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 27/06/2014.
        /// Seleccionar jefe de unidad "mostrando/pasando" por mensajes de confirmación.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnk_jefeSeleccionable_Click(object sender, EventArgs e)
        {
            string[] a = lnk_jefeSeleccionable.CommandArgument.ToString().Split(';');
            string b = lnk_jefeSeleccionable.CommandName;

            if (a[1].Equals("Asesor"))
            {
                btn_enviar_cambio_jefe_unidad_ConfirmButtonExtender.ConfirmText = Texto("TXT_ADVERTENCIA_PROYECTOSHUERFANOS");
            }
            else
            {
                btn_enviar_cambio_jefe_unidad_ConfirmButtonExtender.ConfirmText = Texto("TXT_ADVERTENCIA_CAMBIOROLUSUARIO");
            }


            if (a[1].Equals("Asesor"))
            {
                if (System.Windows.Forms.MessageBox.Show(Texto("TXT_ADVERTENCIA_PROYECTOSHUERFANOS"), "Confirmación", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (System.Windows.Forms.MessageBox.Show(Texto("TXT_ADVERTENCIA_CAMBIOROLUSUARIO"), "Confirmación", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                    {
                        HttpContext.Current.Session["CodNuevoJefe_Seleccionado"] = a;
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El Jefe de unidad es válido.')", true);
                        HttpContext.Current.Session["OPEN"] = "true";
                        ClientScriptManager cm = this.ClientScript;
                        cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>window.opener.location=window.opener.location;window.close();</script>");
                        return;
                    }
                }
            }
            else
            {
                if (System.Windows.Forms.MessageBox.Show(Texto("TXT_ADVERTENCIA_CAMBIOROLUSUARIO"), "Confirmación", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                {
                    HttpContext.Current.Session["CodNuevoJefe_Seleccionado"] = a;
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El Jefe de unidad es válido.')", true);
                    HttpContext.Current.Session["OPEN"] = "true";
                    ClientScriptManager cm = this.ClientScript;
                    cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>window.opener.location=window.opener.location;window.close();</script>");
                    return;
                }
            }

            //string a = lnk_jefeSeleccionable.CommandArgument;
            //string b = lnk_jefeSeleccionable.CommandName;
            //Session["CodNuevoJefe_Seleccionado"] = a;
            ////ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El Jefe de unidad es válido.')", true);
            //Session["OPEN"] = "true";
            //ClientScriptManager cm = this.ClientScript;
            //cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>window.opener.location=window.opener.location;window.close();</script>");
            //return;
        }
    }
}