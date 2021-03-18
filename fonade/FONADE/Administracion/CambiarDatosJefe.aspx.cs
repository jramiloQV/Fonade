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
    /// CambiarDatosJefe
    /// </summary>    
    public partial class CambiarDatosJefe : Base_Page
    {
        #region Variables de sesión.

        /// <summary>
        /// Código del Jefe Unidad seleccionado "este dato viajará por la variable de sesión (CodJefeUnidadSeleccionado)".
        /// </summary>
        private Int32 CodJefeUnidadSeleccionado = 0;

        #endregion

        const string Emailpattern = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"; //Mauricio Arias Olave.
        //const string Emailpattern = @"/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/"; //FONADE clásico.

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
                //Establecer el título de la página actual.
                this.Page.Title = "FONDO EMPRENDER - Administrar Unidades de Emprendimiento";
                l_fechaActual.Text = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");

                if (!IsPostBack)
                {
                    CodJefeUnidadSeleccionado = HttpContext.Current.Session["codJefeUnidad"] != null ? CodJefeUnidadSeleccionado = Convert.ToInt32(HttpContext.Current.Session["codJefeUnidad"].ToString()) : 0;
                    CargarTiposDeIdentificacion();
                    CargarInformacionJefeUnidad(CodJefeUnidadSeleccionado);
                }
            }
            catch (Exception)
            { Response.Redirect("~/Account/Login.aspx"); }
        }

        #region Métodos generales.

        /// <summary>
        /// Mauricio Arias Olave.
        /// 02/05/2014.
        /// Cargar la información del Jefe Unidad seleccionado previamente en "CatalogoUnidadEmprende.aspx".
        /// La información será cargada en los campos del formulario.
        /// </summary>
        /// <param name="CodJefe">Código del Jefe Unidad seleccionado.</param>
        private void CargarInformacionJefeUnidad(Int32 CodJefe)
        {
            //Inicializar variables.
            String sqlConsulta = "";

            try
            {
                if (CodJefe == 0) //{ return; }
                { ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.close();", true); }
                else
                {
                    #region Consultar la información.

                    sqlConsulta = " SELECT Nombres, Apellidos, CodTipoIdentificacion, Identificacion, Cargo, Email, Telefono, Fax, " +
                                  " CodGrupo " +
                                  " FROM Contacto, GrupoContacto  " +
                                  " WHERE Id_Contacto = CodContacto AND Id_Contacto = " + CodJefe;

                    var sql_1 = consultas.ObtenerDataTable(sqlConsulta, "text");

                    if (sql_1.Rows.Count > 0)
                    {
                        //Asignar valores a los campos de texto.
                        txt_Nombres.Text = sql_1.Rows[0]["Nombres"].ToString();
                        txt_Apellidos.Text = sql_1.Rows[0]["Apellidos"].ToString();
                        dd_listado_TipoIdentificacion.SelectedValue = sql_1.Rows[0]["CodTipoIdentificacion"].ToString();
                        txt_num_identificacion.Text = sql_1.Rows[0]["Identificacion"].ToString();
                        txt_Email.Text = sql_1.Rows[0]["Email"].ToString();
                        hdf_tel.Value = sql_1.Rows[0]["Telefono"].ToString();
                        hdf_fax.Value = sql_1.Rows[0]["Fax"].ToString();
                        hdf_CodGrupo.Value = sql_1.Rows[0]["CodGrupo"].ToString();
                        hdf_EmailOld.Value = sql_1.Rows[0]["Email"].ToString();
                    }

                    #endregion
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo cargar la información del jefe de unidad seleccionado.')", true);
                return;
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
        /// 28/04/2014.
        /// Generar registros en tabla "LogEnvios".
        /// </summary>
        /// <param name="p_Asunto">Asunto.</param>
        /// <param name="p_EnviadoPor">Enviado Por.</param>
        /// <param name="p_EnviadoA">Enviado A:</param>
        /// <param name="p_Programa">Programa:</param>
        /// <param name="codProyectoActual">Código del proyecto</param>
        /// <param name="p_Exitoso">Exitoso "1/0".</param>
        private void prLogEnvios(String p_Asunto, String p_EnviadoPor, String p_EnviadoA, String p_Programa, Int32 codProyectoActual, Boolean p_Exitoso)
        {
            //Inicializar variables.
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            String sqlConsulta = "";
            bool correcto = false;

            try
            {
                sqlConsulta = " INSERT INTO LogEnvios (Fecha, Asunto, EnviadoPor, EnviadoA, Programa, CodProyecto, Exitoso) " +
                              " VALUES (GETDATE(),'" + p_Asunto + "','" + p_EnviadoPor + "','" + p_EnviadoA + "','" + p_Programa + "'," + codProyectoActual + "," + p_Exitoso + ") ";

                //Asignar SqlCommand para su ejecución.
                cmd = new SqlCommand(sqlConsulta, conn);

                //Ejecutar SQL.
                correcto = EjecutarSQL(conn, cmd);

                if (correcto == false)
                {
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error en inserción de log.')", true);
                    //return;
                }
            }
            catch { }
        }

        #endregion

        #region Actualización.

        /// <summary>
        /// Mauricio Arias Olave.
        /// 30/04/2014.
        /// Método que hace consultas a la tabla "Texto" para validar los campos para la actualización 
        /// del jefe unidad seleccionado.
        /// </summary>
        /// <returns>Cadena de texto vacía = Todos los campos pasaron las validaciones. // variable llena = Error y/o no pasaron las validaciones.</returns>
        private string ValidarCamposActualizacion()
        {
            //Inicializar variables.
            string validado = "";

            try
            {
                if (txt_Nombres.Text.Trim() == "")
                { validado = Texto("TXT_NOMBRE_REQ"); return validado; }
                if (txt_Apellidos.Text.Trim() == "")
                { validado = Texto("TXT_APELLIDO_REQ"); return validado; }

                //Validación del número de identificación.
                if (txt_num_identificacion.Text.Trim() == "")
                { validado = Texto("TXT_IDENT_REQ"); return validado; }
                else
                {
                    try { int valor_dato = Convert.ToInt32(txt_num_identificacion.Text.Trim()); }
                    catch { validado = Texto("TXT_IDENT_NUM"); return validado; }
                }

                //Validación del email.
                try
                {
                    if (txt_Email.Text.Trim() != "")
                    {
                        if (!Regex.IsMatch(txt_Email.Text.Trim(), Emailpattern))
                        { validado = Texto("TXT_EMAIL_INV"); return validado; }
                    }
                    else
                    { validado = Texto("TXT_EMAIL_REQ"); return validado; }
                }
                catch { validado = Texto("TXT_EMAIL_INV"); return validado; }

                //Retornar datos.
                return validado;

            }
            catch { return validado; }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 02/05/2014.
        /// Actualizar información del Jefe Unidad seleccionado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Actualizar_Click(object sender, EventArgs e)
        {
            //Inicializar variables.
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            bool ejecucion_correcta = false; //Comprueba si la inserción y/o consultas SQL si se ejecutaron.
            String sqlConsulta = "";
            Int32 CodTipoIdentificacion_Seleccionado = 0; //Almacena la variable "CodTipoIdentificacion" seleccionado.
            Int32 Identificacion = 0; //Almacena el valor del texto "txt_num_identificacion" seleccionado.

            try
            {
                string validado = "";
                validado = ValidarCamposActualizacion();

                if (validado.Trim() == "")
                {
                    #region Flujo de la actualización.

                    #region Obtener los valores seleccionados para ser almacenados en variables internas.

                    //Convertir el valor seleccionado en "dd_listado_TipoIdentificacion" en un dato utilizable en el código.
                    CodTipoIdentificacion_Seleccionado = Convert.ToInt32(dd_listado_TipoIdentificacion.SelectedValue);

                    //Convertir el valor digitado en "txt_num_identificacion" en un dato utilizable en el código.
                    Identificacion = Convert.ToInt32(txt_num_identificacion.Text.Trim());

                    try { CodJefeUnidadSeleccionado = Convert.ToInt32(HttpContext.Current.Session["codJefeUnidad"].ToString()); }
                    catch
                    {
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Por favor cierre y abra esta ventana nuevamente.')", true);
                        //return;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.reload();window.close();", true);
                    }

                    #endregion

                    #region Consulta para el bloque #1. Update = 0;

                    sqlConsulta = " SELECT Id_Contacto FROM Contacto " +
                                  " WHERE (Email = '" + txt_Email.Text.Trim() + "'" +
                                  " OR (CodTipoIdentificacion = " + CodTipoIdentificacion_Seleccionado +
                                  " AND Identificacion = " + Identificacion + "))" +
                                  " AND Id_Contacto <> " + CodJefeUnidadSeleccionado;

                    //Asignar resultados de la consulta anterior en variable DataTable.
                    var sql_c_1 = consultas.ObtenerDataTable(sqlConsulta, "text");

                    #endregion

                    //Si la consulta anteriorm "bloque #1" NO tiene datos...
                    if (sql_c_1.Rows.Count == 0)
                    {
                        #region Procesar sobre el Email.

                        //Si el email digitado es diferente al Email cargado...
                        if (txt_Email.Text.Trim() != hdf_EmailOld.Value)
                        {
                            #region Consultar Email.

                            //Se realiza la siguiente consulta.
                            sqlConsulta = " SELECT Email FROM Contacto WHERE Email = '" + txt_Email.Text.Trim() + "' " +
                                          " AND Id_Contacto != " + CodJefeUnidadSeleccionado;

                            //Asginar resultados de la consulta anterior a variable DataTable.
                            var sql_c_2 = consultas.ObtenerDataTable(sqlConsulta, "text");

                            //Si la consulta anterior NO trae datos...
                            if (sql_c_2.Rows.Count == 0)
                            {
                                #region Como la consulta anterior no trajo datos, hace esta actualización.

                                sqlConsulta = " UPDATE Contacto SET Nombres = '" + txt_Nombres.Text.Trim() + "'," +
                                                                  " Apellidos = '" + txt_Apellidos.Text.Trim() + "', " +
                                                                  " CodTipoIdentificacion = " + CodTipoIdentificacion_Seleccionado + ", " +
                                                                  " Identificacion = " + Identificacion + ", " +
                                                                  " Email = '" + txt_Email.Text.Trim() + "'" +
                                                                  " WHERE Id_Contacto = " + CodJefeUnidadSeleccionado;

                                //Asignar SqlCommand para su ejecución.
                                cmd = new SqlCommand(sqlConsulta, conn);

                                //Ejecutar inserción.
                                ejecucion_correcta = EjecutarSQL(conn, cmd);

                                if (ejecucion_correcta == false)
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error en actualización (1).')", true);
                                    return;
                                }
                                else { HttpContext.Current.Session["bRepetido"] = "false"; }

                                #endregion

                                #region Trae los datos del usuario para el envío del email y envía Email al usuario.

                                sqlConsulta = " SELECT Nombres, Apellidos, Email, Clave " +
                                              " FROM Contacto WHERE Id_Contacto = " + CodJefeUnidadSeleccionado;

                                var tabla_final = consultas.ObtenerDataTable(sqlConsulta, "text");

                                if (tabla_final.Rows.Count > 0)
                                {
                                    for (int j = 0; j < tabla_final.Rows.Count; j++)
                                    {
                                        #region Obtener valores de la consulta anterior para generar mensajes masivos.

                                        bool Enviado = false; //Establece si el mensaje fue o no enviado.
                                        String claveObtenida = ""; //Variable que almacena la clave obtenida de la consulta anterior.
                                        String Email_obtenido = ""; //Variable que almacena el email obtenido de la consulta anterior.
                                        String Nombre_obtenido = ""; //Variable que almacena el nombre obtenido de la consulta anterior.
                                        String Apellidos_obtenidos = ""; //Variable que almacena el nombre obtenido de la consulta anterior.

                                        //Obtener el valor "Clave" de "tabla_final".
                                        claveObtenida = tabla_final.Rows[j]["Clave"].ToString();

                                        //Obtener el valor "Email" de "tabla_final".
                                        Email_obtenido = tabla_final.Rows[j]["Email"].ToString();

                                        //Obtener el valor "Nombres" de "tabla_final".
                                        Nombre_obtenido = tabla_final.Rows[j]["Nombres"].ToString();

                                        //Obtener el valor "Nombres" de "tabla_final".
                                        Apellidos_obtenidos = tabla_final.Rows[j]["Apellidos"].ToString();

                                        #endregion

                                        #region Enviar el Email al usuario.

                                        //Variable que almacena el texto obtenido de la consulta a la tabla "Texto".
                                        String Texto_Obtenido = "";

                                        //Consultar el "TEXTO".
                                        Texto_Obtenido = Texto("TXT_EMAILENVIOCLAVE");

                                        //Sólo por si acaso, si el resultado de "Texto_Obtenido" NO devuelve los datos según el texto esperado,
                                        //se debe asignar el texto tal cual se vió en BD el "28/04/2014".
                                        if (Texto_Obtenido.Contains("Señor Usuario") || Texto_Obtenido.Trim() == null)
                                        {
                                            Texto_Obtenido = "Señor Usuario Con el usuario {{Email}} y contraseña {{Clave}},  podrá acceder al sistema de información por medio de la pagina www.fondoemprender.com,  allí encontrara en la parte superior del sistema específicamente en el botón con el signo de interrogación  (?) el manual de su perfil ''{{Rol}}''";
                                        }

                                        //Reemplazar determinados caracteres por caracteres definidos específicamente para esta acción.
                                        Texto_Obtenido = Texto_Obtenido.Replace("{{Rol}}", "Jefe Unidad");
                                        Texto_Obtenido = Texto_Obtenido.Replace("{{Email}}", Email_obtenido);
                                        Texto_Obtenido = Texto_Obtenido.Replace("{{Clave}}", claveObtenida);

                                        try
                                        {
                                            //Generar y enviar mensaje.
                                            Correo correo = new Correo(usuario.Email,
                                                                       "Fondo Emprender",
                                                                       Email_obtenido,
                                                                       Nombre_obtenido + " " + Apellidos_obtenidos,
                                                                       "Registro a Fondo Emprender",
                                                                       Texto_Obtenido);
                                            correo.Enviar();

                                            //El mensaje fue enviado.
                                            Enviado = true;

                                            //Inserción en tabla "LogEnvios".
                                            prLogEnvios("Registro a Fondo Emprender", usuario.Email, Email_obtenido, "Cambio Datos Jefe", 0, Enviado);
                                        }
                                        catch
                                        {
                                            //El mensaje no pudo ser enviado.
                                            Enviado = false;

                                            //Inserción en tabla "LogEnvios".
                                            prLogEnvios("Registro a Fondo Emprender", usuario.Email, Email_obtenido, "Cambio Datos Jefe", 0, Enviado);
                                        }

                                        #endregion
                                    }
                                }

                                #endregion
                            }
                            else
                            {
                                #region Realizar esta actualización.

                                sqlConsulta = " UPDATE Contacto SET Nombres = '" + txt_Nombres.Text.Trim() + "'," +
                                              " Apellidos = '" + txt_Apellidos.Text.Trim() + "', " +
                                              " CodTipoIdentificacion = " + CodTipoIdentificacion_Seleccionado + ", " +
                                              " Identificacion = " + Identificacion +
                                              " WHERE Id_Contacto = " + CodJefeUnidadSeleccionado;

                                //Asignar SqlCommand para su ejecución.
                                cmd = new SqlCommand(sqlConsulta, conn);

                                //Ejecutar inserción.
                                ejecucion_correcta = EjecutarSQL(conn, cmd);

                                if (ejecucion_correcta == false)
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error en actualización (2).')", true);
                                    return;
                                }
                                else { HttpContext.Current.Session["bRepetido"] = "false"; }

                                #endregion
                                //Update = 1;
                            }

                            #endregion
                        }
                        else
                        {
                            #region Como el Email digitado es el mismo que tenía el Jefe Unidad = "no se editó el Email".

                            sqlConsulta = " UPDATE Contacto SET Nombres ='" + txt_Nombres.Text.Trim() + "'," +
                                                              " Apellidos = '" + txt_Apellidos.Text.Trim() + "', " +
                                                              " CodTipoIdentificacion = " + CodTipoIdentificacion_Seleccionado + ", " +
                                                              " Identificacion = " + Identificacion + ", " +
                                                              " Email = '" + txt_Email.Text.Trim() + "'" +
                                                              " WHERE Id_Contacto = " + CodJefeUnidadSeleccionado;

                            //Asignar SqlCommand para su ejecución.
                            cmd = new SqlCommand(sqlConsulta, conn);

                            //Ejecutar inserción.
                            ejecucion_correcta = EjecutarSQL(conn, cmd);

                            if (ejecucion_correcta == false)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error en actualización (1).')", true);
                                return;
                            }
                            else { HttpContext.Current.Session["bRepetido"] = "false"; }

                            #endregion
                        }

                        #endregion
                    }
                    else
                    { HttpContext.Current.Session["bRepetido"] = "true"; /*Si sí trajo, es porque había un jefe unidad "segun FONADE clásico".*/ }


                    //Mostrar mensaje.
                    if (HttpContext.Current.Session["bRepetido"].ToString() == "true")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Ya existe un Usuario con ese email o Identificación.')", true);
                        return;
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Datos Actualizados con éxito');window.opener.location.reload();window.close();", true);
                        return;
                        //Response.Write("<script>window.close();</script>");
                    }

                    #endregion
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + validado + "')", true);
                    return;
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo actualizar el jefe unidad seleccionado.')", true);
                return;
            }
        }

        #endregion

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
                return true;
            }
            catch (Exception) { return false; }
            finally
            { p_connection.Close(); p_connection.Dispose(); }
        }

        #endregion
    }
}