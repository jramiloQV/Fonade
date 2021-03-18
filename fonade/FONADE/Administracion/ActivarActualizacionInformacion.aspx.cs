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
using System.Threading;

namespace Fonade.FONADE.Administracion
{
    /// <summary>
    /// ActivarActualizacionInformacion
    /// </summary>    
    public partial class ActivarActualizacionInformacion : Base_Page
    {
        /// <summary>
        /// Mauricio Arias Olave.
        /// 28/04/2014.
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            String sqltime = "";
            try
            {
                //Establecer el título de la página actual.
                this.Page.Title = "FONDO EMPRENDER - ACTIVAR ACTUALIZACIÓN DE INFORMACIÓN";

                if (!IsPostBack)
                {
                    alert_time.Visible = false;
                    sqltime = "SELECT valor FROM parametro WHERE nomparametro = 'TiempoValidacionActualizacionInformacionUsuarios'";
                    var time_record = consultas.ObtenerDataTable(sqltime, "text");
                    if (time_record.Rows.Count > 0)
                    {
                        txt_diasActualizacion.Text = time_record.Rows[0]["Valor"].ToString();
                    }
                    else 
                    {
                        txt_diasActualizacion.Text = "";
                    }
                    
                    //MostrarInterventor(); //Mauricio Arias Olave "26/04/2014": Ya no se muestra los valores del nombre y fecha.
                    chk_actualizarInfo.Checked = EstablecerCheck();
                }
            }
            catch { Response.Redirect("~/Account/Login.aspx");  }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 28/04/2014.
        /// Según FONADE Clásico, establecer el valor "Checked" del CheckBox dependiendo
        /// del resultado de una consulta.
        /// </summary>
        /// <returns></returns>
        private bool EstablecerCheck()
        {
            //Inicializar variables.
            String sqlConsulta = "";
            

            try
            {
                //Consulta.
                sqlConsulta = "SELECT valor FROM parametro WHERE nomparametro = 'ObligatoriedadCagarAnexosActualizacionDatos'";
                
                //Asignar resultado de la consulta a variable de tipo DataTable.
                var dtEmpresas = consultas.ObtenerDataTable(sqlConsulta, "text");

                //Si hay datos, establecer chequeo dependiendo del resultado de la consulta.
                if (dtEmpresas.Rows.Count > 0)
                {
                    if (dtEmpresas.Rows[0]["Valor"].ToString() == "1")
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else { return false; }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error en consulta principal (1).')", true);
                return false;
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
        /// Actualizar información.
        /// PENDIENTE!!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Actualizar_Click(object sender, EventArgs e)
        {
            //Inicializar variables.
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            String sqlConsulta = "";
            bool correcto = false;
            //Según FONADE clásico, se ingresa en el campo "valor" un varchar, basado en la consulta en la BD de FONADE, se le enviará un Int.
            String valor_chequeado = "0";
            Int32 tiempo_agregado = 0; //Tiempo digitado o establecido según directrices de FONADE Clásico.
            bool Mensaje_Enviado = false;
            String textoConsultado = ""; //Variable que almacena el valor consultado del método "Texto".
            

            try
            {
                #region Obtener el valor de la caja de texto "Tiempo en dias".

                try
                {
                    if (txt_diasActualizacion.Text.Trim() != null)
                        tiempo_agregado = Convert.ToInt32(txt_diasActualizacion.Text.Trim());
                    else
                        tiempo_agregado = 30; //Si está vacío, se establece el valor por defecto de "30" dias.
                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El valor ingresado es incorrecto, solo se admiten números.')", true);
                    return;
                }

                #endregion

                #region Realizar consulta para obtener información "TEXTO".

                textoConsultado = Texto("strTextoEmailActualizacionDatos");

                #endregion

                #region Actualizar parámetro de actualización.

                //Establecer el valor de la variable "valor_chequeado" con relación al estado "Checked" del 
                //CheckBox "chk_actualizarInfo".
                if (chk_actualizarInfo.Checked == true) { valor_chequeado = "1"; } else { valor_chequeado = "0"; }

                //Consulta.
                sqlConsulta = "SELECT valor FROM parametro WHERE nomparametro = 'ActivarActualizacionInformacionUsuarios'";

                //Asignar resultado de la consulta a variable de tipo DataTable.
                var dtEmpresas = consultas.ObtenerDataTable(sqlConsulta, "text");

                if (dtEmpresas.Rows.Count == 0)
                {
                    #region Si está vacío, inserte.

                    sqlConsulta = " INSERT INTO parametro (nomparametro, valor) " +
                                              " VALUES('ActivarActualizacionInformacionUsuarios','" + valor_chequeado + "')";

                    //Asignar SqlCommand para su ejecución.
                    cmd = new SqlCommand(sqlConsulta, conn);

                    //Ejecutar SQL.
                    correcto = EjecutarSQL(conn, cmd);

                    if (correcto == false)
                    { }

                    #endregion
                }
                else
                {
                    #region Si tiene datos, actualize.

                    sqlConsulta = " UPDATE parametro SET valor = '" + valor_chequeado + "' " +
                                              " WHERE nomparametro = 'ActivarActualizacionInformacionUsuarios' ";

                    //Asignar SqlCommand para su ejecución.
                    cmd = new SqlCommand(sqlConsulta, conn);

                    //Ejecutar SQL.
                    correcto = EjecutarSQL(conn, cmd);

                    if (correcto == false)
                    { }

                    #endregion
                }

                #endregion

                #region Actualizar parámetro de tiempo de actualización.

                //Consulta.
                sqlConsulta = "SELECT valor FROM parametro WHERE nomparametro = 'TiempoValidacionActualizacionInformacionUsuarios'";

                //Asignar resultado de la consulta a variable de tipo DataTable.
                var dtEmpresas1 = consultas.ObtenerDataTable(sqlConsulta, "text");

                if (dtEmpresas1.Rows.Count == 0)
                {
                    #region Si está vacío, inserte.

                    sqlConsulta = " INSERT INTO parametro (nomparametro, valor) " +
                                  " VALUES('TiempoValidacionActualizacionInformacionUsuarios','" + tiempo_agregado + "')";

                    //Asignar SqlCommand para su ejecución.
                    cmd = new SqlCommand(sqlConsulta, conn);

                    //Ejecutar SQL.
                    correcto = EjecutarSQL(conn, cmd);

                    if (correcto == false)
                    { }

                    #endregion
                }
                else
                {
                    #region Si tiene datos, actualize.

                    sqlConsulta = " UPDATE parametro SET valor = '" + tiempo_agregado + "' " +
                                  " WHERE nomparametro = 'TiempoValidacionActualizacionInformacionUsuarios' ";

                    //Asignar SqlCommand para su ejecución.
                    cmd = new SqlCommand(sqlConsulta, conn);

                    //Ejecutar SQL.
                    correcto = EjecutarSQL(conn, cmd);

                    if (correcto == false)
                    { }

                    #endregion
                }


                #endregion

                if (chk_actualizarInfo.Checked == true)
                {
                    #region Generar consulta.

                    #region Consulta que genera "46839" registros aprox.

                    sqlConsulta = " SELECT * FROM  ((SELECT DISTINCT c.Id_Contacto, c.Nombres, c.Apellidos, c.CodTipoIdentificacion, c.Identificacion, c.Cargo, c.Email, " +
                                              " 							     gc.CodGrupo, g.NomGrupo " +
                                              " 				 FROM  Contacto AS c INNER JOIN GrupoContacto AS gc " +
                                              " 				 ON c.Id_Contacto = gc.CodContacto INNER JOIN Grupo AS g " +
                                              " 				 ON G.Id_Grupo = gc.CodGrupo INNER JOIN ProyectoContacto AS pc " +
                                              " 				 ON pc.CodContacto = c.Id_Contacto " +
                                              " 				 WHERE c.Inactivo = 0 and pc.Inactivo = 0 AND  gc.CodGrupo in (5,6,11)) " +
                                              " UNION		    (SELECT DISTINCT c.Id_Contacto, c.Nombres, c.Apellidos, c.CodTipoIdentificacion, c.Identificacion, c.Cargo, c.Email, " +
                                              " 						gc.CodGrupo, g.NomGrupo " +
                                              " 				 FROM Contacto AS c INNER JOIN GrupoContacto AS gc " +
                                              " 				 ON c.Id_Contacto = gc.CodContacto INNER JOIN Grupo AS g  " +
                                              " 				 ON G.Id_Grupo = gc.CodGrupo INNER JOIN EmpresaInterventor ei " +
                                              " 				 ON ei.CodContacto = c.Id_Contacto " +
                                              " 				 WHERE c.Inactivo = 0 AND ei.inactivo=0  and gc.CodGrupo in (14)))tbl " +
                                              " ORDER BY NomGrupo,Id_contacto";

                    #endregion

                    //Generar resultados en variable DataTable.
                    var tabla_contactos = consultas.ObtenerDataTable(sqlConsulta, "text");

                    //Si la tabla tiene datos, entra en el flujo.
                    if (tabla_contactos.Rows.Count > 0)
                    {
                        #region Recorrer la tabla con los valores para enviar mensajes y generar registros en la tabla "LogEnvios".
                        alert_time.Visible = true;
                        for (int i = 0; i < tabla_contactos.Rows.Count; i++)
                        {
                            try
                            {
                                
                                //Enviar mensaje.
                                Correo correo = new Correo(usuario.Email,
                                                           usuario.Nombres,
                                                           tabla_contactos.Rows[i]["Email"].ToString(),
                                                           tabla_contactos.Rows[i]["Nombres"].ToString() + " " + tabla_contactos.Rows[i]["Apellidos"].ToString(),
                                                           "Fondo Emprender Actualizacion Masiva",
                                                           txt_diasActualizacion.Text);
                                correo.Enviar();
                               
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "document.title='"+i+"'", true);
                        
                                //Si lo envió, se hace inserción en log con el valor "TRUE".
                                Mensaje_Enviado = true;

                            }
                            catch (Exception)
                            {
                                //Como no lo envió "por x o y motivo", se hace inserción en log con el valor "FALSE".
                                Mensaje_Enviado = false;
                            }
                            // SANTIAGO SANCHEZ 13/12/14
                            // insert en logenvios
                            prLogEnvios("Fondo Emprender Actualizacion Masiva", usuario.Email,tabla_contactos.Rows[i]["Email"].ToString(),"Activa Actualización de Información",0,Mensaje_Enviado);
                         
                        }
                        alert_time.Visible = false;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Se terminó El envío De notificaciones a los " + tabla_contactos.Rows.Count + " usuarios')", true);
                        //div_tabla.Visible = true;
                        #endregion
                    }

                    #endregion
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error')", true);
                return;
            }
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
            finally { p_connection.Close(); p_connection.Dispose(); }
        }
        #endregion
    }
}