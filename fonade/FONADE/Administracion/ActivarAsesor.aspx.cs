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
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;
using Fonade.Clases;
using System.Web;

namespace Fonade.FONADE.Administracion
{
    /// <summary>
    /// ActivarAsesor
    /// </summary>    
    public partial class ActivarAsesor : Base_Page
    {
        const string Emailpattern = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"; //Mauricio Arias Olave.
        //const string Emailpattern = @"/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/"; //FONADE clásico.

        #region Variables globales.

        /// <summary>
        /// Id del contacto seleccionado como resultado de la búsqueda en "FiltrosUsuario.aspx".
        /// </summary>
        public Int32 CodContactoSeleccionado;

        /// <summary>
        /// Nombre del usuario seleccionado en "FiltrosUsuario.aspx".
        /// </summary>
        public string NombreUsuarioSeleccionado;

        /// <summary>
        /// Valor inactivo del usuario seleccionado en "FiltrosUsuario.aspx".
        /// </summary>
        public bool ValorInactivo;

        /// <summary>
        /// Valor inactivo del usuario seleccionado en "FiltrosUsuario.aspx".
        /// </summary>
        public bool ValorBloqueado;

        /// <summary>
        /// Valor "Id_Clave" del usuario seleccionado y consultado en el método "CargarInformacionContactoSeleccionado".
        /// </summary>
        public int ValorIdClave;

        /// <summary>
        /// Valor "flagAcreditador" del usuario seleccionado y consultado en el método "CargarInformacionContactoSeleccionado".
        /// </summary>
        public bool b_flagAcreditador;

        /// <summary>
        /// Valor "flagActaParcial" del usuario seleccionado y consultado en el método "CargarInformacionContactoSeleccionado".
        /// </summary>
        public bool b_flagActaParcial;

        /// <summary>
        /// Valor "flagGeneraReporte" del usuario seleccionado y consultado en el método "CargarInformacionContactoSeleccionado".
        /// </summary>
        public bool b_flagGeneraReporte;

        #endregion

        /// <summary>
        /// Mauricio Arias Olave.
        /// 28/04/2014.
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Establecer el título de la página actual.
                this.Page.Title = "FONDO EMPRENDER - ";

                if (!IsPostBack)
                {
                    CodContactoSeleccionado = HttpContext.Current.Session["CodContactoSeleccionado"] != null ? CodContactoSeleccionado = Convert.ToInt32(HttpContext.Current.Session["CodContactoSeleccionado"].ToString()) : 0;
                    CargarInformacionContactoSeleccionado();

                    //MostrarInterventor(); //Mauricio Arias Olave "26/04/2014": Ya no se muestra los valores del nombre y fecha.
                }
            }
            catch (Exception)
            {
                Response.Redirect("~/Account/Login.aspx");
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 28/04/2014.
        /// El código del usuario seleccionado es usado en una consulta para
        /// cargar la información completa desde otras tablas y continuar con el flujo.
        /// </summary>
        private void CargarInformacionContactoSeleccionado()
        {
            //Inicializar variables.
            String sqlConsulta;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();

            try
            {
                if (CodContactoSeleccionado > 0)
                {
                    //Consulta.
                    sqlConsulta = " SELECT Nombres, Apellidos, Email, Identificacion, ISNULL(Direccion,'') AS Direccion, ISNULL(Telefono,'') AS Telefono " +
                                  " FROM Contacto WHERE Id_Contacto = " + CodContactoSeleccionado;

                    //Obtener resultado de la consulta en variable DataTable.
                    var dtEmpresas = consultas.ObtenerDataTable(sqlConsulta, "text");

                    if (dtEmpresas.Rows.Count > 0)
                    {
                        //Establecer valores en campos del formulario y variables internas.
                        txt_Nombres.Text = dtEmpresas.Rows[0]["Nombres"].ToString();
                        txt_Apellidos.Text = dtEmpresas.Rows[0]["Apellidos"].ToString();
                        txt_Email.Text = dtEmpresas.Rows[0]["Email"].ToString();
                        txt_Identificacion.Text = dtEmpresas.Rows[0]["Identificacion"].ToString();
                        txt_Direccion.Text = dtEmpresas.Rows[0]["Direccion"].ToString();
                        txt_Telefono.Text = dtEmpresas.Rows[0]["Telefono"].ToString();
                    }
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error en consulta (1).')", true);
                return;
            }
        }

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
            int Exito=0;
            if (p_Exitoso)
                Exito = 1;


            try
            {
                sqlConsulta = " INSERT INTO LogEnvios (Fecha, Asunto, EnviadoPor, EnviadoA, Programa, CodProyecto, Exitoso) " +
                              " VALUES (GETDATE(),'" + p_Asunto + "','" + p_EnviadoPor + "','" + p_EnviadoA + "','" + p_Programa + "'," + codProyectoActual + "," + Exito + ") ";

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
            finally { p_connection.Close(); p_connection.Dispose();}
        }

        #endregion

        /// <summary>
        /// Mauricio Arias Olave.
        /// 28/04/2014.
        /// Modificar la información del usuario seleccionado
        /// previamente en "FiltroAsesorInactivo.aspx". (re-activarlo).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            //Inicializar variables.
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            String sqlConsulta = "";
            String claveObtenida = ""; //Variable que almacena el valor de la consulta de la clave.
            String Texto_Obtenido = ""; //Variable que almacena el valor de la consulta de la tabla "Texto".
            bool Enviado = false; //Variable que determina si el mensaje fue enviado o no "como resultado de la re-activación".
            bool correcto = false;

            try
            {
                //Obtener el código del usuario seleccionado desde la variable de sesión.
                CodContactoSeleccionado = Convert.ToInt32(HttpContext.Current.Session["CodContactoSeleccionado"].ToString());

                if (CodContactoSeleccionado == 0)
                {
                    //Redirigir al usuario al login "acceso denegado".
                    Response.Redirect("~/Account/Login.aspx");
                }
                else
                {
                    //Continuar con el flujo.

                    #region Validaciones de los campos del formulario.

                    if (txt_Nombres.Text.Trim() == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El Nombre del Usuario es Obligatorio')", true);
                        return;
                    }
                    if (txt_Apellidos.Text.Trim() == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El Apellido del Usuario es Obligatorio')", true);
                        return;
                    }
                    if (txt_Identificacion.Text.Trim() == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La Identificación del Usuario es Obligatorio')", true);
                        return;
                    }
                    if (txt_Email.Text.Trim() != "")
                    {
                        if (!Regex.IsMatch(txt_Email.Text.Trim(), Emailpattern))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La dirección de Email es incorrecta.')", true);
                            return;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El Email del Usuario es Obligatorio')", true);
                        return;
                    }

                    #endregion

                    #region Verificar si el Email es único.

                    var email_check = (from s in consultas.Db.Contacto
                                       where s.Email == txt_Email.Text.Trim() && s.Id_Contacto != CodContactoSeleccionado
                                       select new
                                       {
                                           s.Id_Contacto
                                       }).ToList();
                    #endregion

                    if (email_check.Count() == 0)
                    {
                        #region Actualización 1.

                        sqlConsulta = " UPDATE Contacto SET Nombres = '" + txt_Nombres.Text.Trim() + "', " +
                                      " Apellidos = '" + txt_Apellidos.Text.Trim() + "', " +
                                      " Email = '" + txt_Email.Text.Trim() + "', " +
                                      " Identificacion = " + txt_Identificacion.Text.Trim() + ", " +
                                      " Direccion = '" + txt_Direccion.Text.Trim() + "', " +
                                      " Telefono = '" + txt_Telefono.Text.Trim() + "'," +
                                       " Inactivo = 0  " + //" Inactivo = " + ValorInactivo + ", " +
                                      " WHERE Id_Contacto = " + CodContactoSeleccionado;

                        //Asignar SqlCommand para su ejecución.
                        cmd = new SqlCommand(sqlConsulta, conn);

                        //Ejecutar SQL.
                        correcto = EjecutarSQL(conn, cmd);

                        if (correcto == false)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error en actualización (1).')", true);
                            return;
                        }

                        #endregion

                        #region Actualización 2.

                        SqlConnection connectDB = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());

                        sqlConsulta = " UPDATE ProyectoContacto SET Inactivo = 1, " +
                                      " FechaFin = GETDATE() " +
                                      " WHERE CodRol IN(1,2) AND CodContacto = " + CodContactoSeleccionado;

                        //Asignar SqlCommand para su ejecución.
                        cmd = new SqlCommand(sqlConsulta, connectDB);

                        //Ejecutar SQL.
                        correcto = EjecutarSQL(connectDB, cmd);

                        if (correcto == false)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error en actualización (2).')", true);
                            return;
                        }

                        #endregion

                        #region Consultar la "Clave" y asignar valor en variable local.

                        //Consultar la clave.
                        sqlConsulta = "SELECT Clave from Contacto WHERE Id_Contacto = " + CodContactoSeleccionado;

                        //Asignar el resultado de la consulta en variable DataTable.
                        var dt_clave = consultas.ObtenerDataTable(sqlConsulta, "text");

                        //Si el DataTable tiene mas de una fila, se establece el primer (y único valor) en la variable local.
                        if (dt_clave.Rows.Count > 0) { claveObtenida = dt_clave.Rows[0]["Clave"].ToString(); }

                        #endregion

                        #region Se registra la activación en la tabla de activaciones "ContactoReActivacion".

                        sqlConsulta = "INSERT INTO ContactoReActivacion (CodContacto, FechaReActivacion, CodContactoQReActiva) " +
                                     " VALUES(" + CodContactoSeleccionado + ", GETDATE(), " + usuario.IdContacto + ")";

                        #region COMENTARIOS NO BORRAR!.
                        ////Asignar SqlCommand para su ejecución.
                        //cmd = new SqlCommand(sqlConsulta, conn);

                        ////Ejecutar SQL.
                        //correcto = EjecutarSQL(conn, cmd);

                        //if (correcto == false)
                        //{
                        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error en inserción de contacto re-activación (1).')", true);
                        //    return;
                        //} 
                        #endregion

                        //Ejecutar setencia.
                        ejecutaReader(sqlConsulta, 2);

                        #endregion

                        #region Se agrega campo para verificar actualización de información después de reactivación  "En FONADE Clásico fue hecho por: sandraem 30/07/2010".

                        sqlConsulta = "INSERT INTO ContactoActualizoReactivacion (CodContacto, ActualizoDatos, CambioClave, FechaReActivacion) " +
                                     " VALUES(" + CodContactoSeleccionado + ", 0, 0, GETDATE())";

                        #region COMENTARIOS NO BORRAR.
                        ////Asignar SqlCommand para su ejecución.
                        //cmd = new SqlCommand(sqlConsulta, conn);

                        ////Ejecutar SQL.
                        //correcto = EjecutarSQL(conn, cmd); 
                        #endregion

                        //Ejecutar setencia.
                        ejecutaReader(sqlConsulta, 2);

                        if (correcto == false)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error en inserción de contacto actualizó re-activación (2).')", true);
                            return;
                        }

                        #endregion

                        #region Enviar el Email al usuario.

                        //Consultar el "TEXTO".
                        Texto_Obtenido = Texto("TXT_EMAILENVIOCLAVE");

                        //Sólo por si acaso, si el resultado de "Texto_Obtenido" NO devuelve los datos según el texto esperado,
                        //se debe asignar el texto tal cual se vió en BD el "28/04/2014".
                        if (Texto_Obtenido.Contains("Señor Usuario") || Texto_Obtenido.Trim() == null)
                        {
                            Texto_Obtenido = "Señor Usuario Con el usuario {{Email}} y contraseña {{Clave}},  podrá acceder al sistema de información por medio de la pagina www.fondoemprender.com,  allí encontrara en la parte superior del sistema específicamente en el botón con el signo de interrogación  (?) el manual de su perfil ''{{Rol}}''";
                        }

                        //Reemplazar determinados caracteres por caracteres definidos específicamente para esta acción.
                        Texto_Obtenido = Texto_Obtenido.Replace("{{Rol}}", "Asesor");
                        Texto_Obtenido = Texto_Obtenido.Replace("{{Email}}", txt_Email.Text.Trim());
                        Texto_Obtenido = Texto_Obtenido.Replace("{{Clave}}", claveObtenida);

                        try
                        {
                            //Generar y enviar mensaje.
                            Correo correo = new Correo(usuario.Email,
                                                       "Fondo Emprender",
                                                       txt_Email.Text.Trim(),
                                                       txt_Nombres.Text.Trim() + " " + txt_Apellidos.Text.Trim(),
                                                       "Re-Activación a Fondo Emprender",
                                                       Texto_Obtenido);
                            correo.Enviar();

                            //El mensaje fue enviado.
                            Enviado = true;

                            //Inserción en tabla "LogEnvios".
                            prLogEnvios("Re-Activación a Fondo Emprender", usuario.Email, txt_Email.Text.Trim(), "Reactivación Asesor", 0, Enviado);

                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Se reactivó el asesor correctamente, y se envió el correo correctamente ')", true);

                            //Finalmente se redirige al usuario a "FiltroAsesorInactivo.aspx".
                            Response.Redirect("FiltroAsesorInactivo.aspx");
                        }
                        catch
                        {
                            //El mensaje no pudo ser enviado.
                            Enviado = false;

                            //Inserción en tabla "LogEnvios".
                            prLogEnvios("Re-Activación a Fondo Emprender", usuario.Email, txt_Email.Text.Trim(), "Reactivación Asesor", 0, Enviado);

                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Se reactivó el asesor correctamente, pero no se pudo enviar el correo ')", true);

                            //Finalmente se redirige al usuario a "FiltroAsesorInactivo.aspx".
                            Response.Redirect("FiltroAsesorInactivo.aspx");
                        }

                        #endregion
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El Email Ingresado ya existe Por favor Verificar')", true);
                        return;
                    }
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo re-activar el usuario seleccionado.')", true);
                return;
            }
        }
    }
}