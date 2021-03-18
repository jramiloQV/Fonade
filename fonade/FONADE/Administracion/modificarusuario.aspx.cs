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
using System.Web;

namespace Fonade.FONADE.Administracion
{
    /// <summary>
    /// modificarusuario
    /// </summary>    
    public partial class modificarusuario : Base_Page
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
        public string ValorInactivo;

        /// <summary>
        /// Valor inactivo del usuario seleccionado en "FiltrosUsuario.aspx".
        /// </summary>
        public string ValorBloqueado;

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
        /// 25/04/2014.
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

        #region Métodos para mostrar los títulos.

        /// <summary>
        /// Mauricio Arias Olave.
        /// 24/04/2014.
        /// Mostrar el nombre completo del interventor (usuario logueado), así como la fecha actual formateada según FONADE Clásico.
        /// </summary>
        private void MostrarInterventor()
        {
            String sql;

            sql = "SELECT Nombres + ' ' + Apellidos AS Nombre from Contacto where id_Contacto = " + usuario.IdContacto;

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(sql, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    //lbl_Interventor.Text = reader["Nombre"].ToString();
                    //DateTime fecha = DateTime.Now;
                    //string sMes = fecha.ToString("MMM", CultureInfo.CreateSpecificCulture("es-CO"));
                    //lbl_tiempo.Text = UppercaseFirst(sMes) + " " + fecha.Day + " de " + fecha.Year;
                }
                reader.Close();
                //conn.Close();
            }
            catch (SqlException) { }
            finally { conn.Close(); conn.Dispose(); }
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

        #endregion

        /// <summary>
        /// Mauricio Arias Olave.
        /// 26/04/2014.
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
                    /*Consulta.
                    sqlConsulta = " SELECT TOP 1 Nombres, Apellidos, Email, Identificacion, ISNULL(Direccion,'') AS Direccion, " +
                                  " ISNULL(Telefono,'') AS Telefono, Inactivo, ISNULL(flagAcreditador, '0') AS flagAcreditador," +
                                  " ISNULL(flagActaParcial,'0') AS flagActaParcial, " +
                                  " ISNULL(flagGeneraReporte, '0') AS flagGeneraReporte, Clave.Estado, Clave.Id_Clave  " +
                                  " From Contacto, clave  " +
                                  " where Id_Contacto = " + CodContactoSeleccionado + " and id_contacto = " + CodContactoSeleccionado +
                                  " Order by Id_Clave DESC";

                     * CAMBIO EN LOS PARAMETROS
                    Obtener resultado de la consulta en variable DataTable.*/
                    sqlConsulta = string.Format("SELECT Contacto.Nombres, Contacto.Apellidos, Contacto.Email, Contacto.Identificacion, " +
                                                "Contacto.Direccion, Contacto.Telefono, Contacto.Inactivo, Contacto.flagAcreditador, Contacto.flagActaParcial, " + 
                                                "Contacto.flagGeneraReporte, Clave.Estado, Clave.Id_Clave FROM Clave INNER JOIN " +
                                                "Contacto ON Clave.codContacto = Contacto.Id_Contacto WHERE (Contacto.Id_Contacto = {0}) " +
                                                "AND (Clave.codContacto = {0})", CodContactoSeleccionado);

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

                        //Valores "Inactivo" y "Estado (sinónimo de bloqueado)".
                        ValorInactivo = dtEmpresas.Rows[0]["Inactivo"].ToString();
                        int valorEstado = Convert.ToInt32(dtEmpresas.Rows[0]["Estado"].ToString());
                        ValorBloqueado = valorEstado==0?"0":"1";

                        //Inicializar variables e instancias de controles.
                        int valor1 = 0;
                        int valor2 = 0;

                        //Establecer selección del ítem del DropDownList "dd_EstaInactivo".
                        valor1 = Equals(ValorInactivo,"True")?1:0;
                        dd_EstaInactivo.SelectedValue = valor1.ToString();                        

                        //Establecer selección del ítem del DropDownList "dd_EstaBloqueado".
                        if (valorEstado == 0) { valor2 = 1; ValorBloqueado = "0"; } else { valor2 = 0; ValorBloqueado = "1"; }
                        dd_EstaBloqueado.SelectedValue = ValorBloqueado;

                        //Establecer si el CheckBox "de acuerdo a la consulta los carga "
                        /*b_flagAcreditador  */
                        chk_1.Checked = Convert.IsDBNull(dtEmpresas.Rows[0]["flagAcreditador"]) ? false : Convert.ToBoolean(dtEmpresas.Rows[0]["flagAcreditador"]);
                        /*b_flagActaParcial  */                        
                        chk_2.Checked = Convert.IsDBNull(dtEmpresas.Rows[0]["flagActaParcial"])?false:Convert.ToBoolean(dtEmpresas.Rows[0]["flagActaParcial"]);
                        /*b_flagGeneraReporte*/
                        chk_3.Checked = Convert.IsDBNull(dtEmpresas.Rows[0]["flagGeneraReporte"])?false:Convert.ToBoolean(dtEmpresas.Rows[0]["flagGeneraReporte"]);
                        //Obtener Id_Clave.
                        ValorIdClave = Convert.IsDBNull(dtEmpresas.Rows[0]["Id_Clave"]) ? 0 : Convert.ToInt32(dtEmpresas.Rows[0]["Id_Clave"]);
                    }

                    //Si el usuario tiene planes de negocio pendeintes por acreditar se deshabilita el check Acreditador de planes de negocio
                        string txtSQL = @"SELECT * FROM PROYECTOCONTACTO WHERE CODCONTACTO=" + CodContactoSeleccionado + " AND inactivo=0 AND acreditador=1";

                        var result = consultas.ObtenerDataTable(txtSQL, "text");

                        if (result.Rows.Count > 0)
                        {
                            chk_1.Enabled = false;
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
        /// Código obtenido de http://stackoverflow.com/questions/6803073/get-local-ip-address-c-sharp
        /// Consultar la dirección IP para ser usada en las inserciones de registros en la tabla "Bitácora".
        /// </summary>
        /// <returns>Ip local.</returns>
        public string LocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 25/04/2014.
        /// Modificar la información del usuario seleccionado
        /// previamente en "FiltrosUsuario.aspx".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Modificar_Click(object sender, EventArgs e)
        {
            //Inicializar variables.
            //Int32 mEsAcreditadorEnProyectoContacto = 0; //Contendrá el resultado del conteo obtenido de la consulta.
            ///Según el valor que tenga, se ajustará la selección del CheckBox "Acreditador de planes de negocio?".
            //Boolean mAcreditadorPlanesNegocio;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            String sqlConsulta = "";
            //Estas dos variables enteras tienen el valor según el checked de los CheckBox.
            //Boolean mGeneraActasParciales;
            //Boolean mGenerarReporteFinal;
            bool correcto = false;
            String ipAddress_result; //Dirección IP obtenida del método "LocalIPAdress".
            //bool bloqueado_modificado; //valores equivalentes de la selección de los DropDownList.
            //bool inactivo_modificado;  //valores equivalentes de la selección de los DropDownList.

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

                    #region Conocer si el usuario seleccionado es "Acreditador en Proyecto" y establecer checks...

                    #region Versión SQL Comentada.
                    //sqlConsulta = " SELECT COUNT(*) AS CANTIDAD " +
                    //              " FROM PROYECTOCONTACTO " +
                    //              " WHERE CODCONTACTO = " + CodContactoSeleccionado + " AND inactivo = 0 AND acreditador = 1 "; 
                    #endregion

                    //Versión LINQ.
                    //var conteo_LINQ = (from t in consultas.Db.ProyectoContactos
                    //                   where t.CodContacto == CodContactoSeleccionado && t.Inactivo == false
                    //                   && t.Acreditador == true
                    //                   select new { Cantidad = t.Id_ProyectoContacto }).Count();

                    //Obtener en variable el resultado del conteo.
                    //mEsAcreditadorEnProyectoContacto = conteo_LINQ;

                    //Si el valor del conteo es 0, se establece la variable destinada con el valor 0, de lo contrario, tendrá 1.
                    //if (mEsAcreditadorEnProyectoContacto > 0)
                    //{ mAcreditadorPlanesNegocio = false; chk_1.Checked = mAcreditadorPlanesNegocio; }
                    //else { mAcreditadorPlanesNegocio = true; chk_1.Checked = mAcreditadorPlanesNegocio; }

                    //Establecer valores de variables enteras según estado de chequeo de los CheckBox.
                    //mGeneraActasParciales = chk_2.Checked ? mGeneraActasParciales = false : mGeneraActasParciales = true;
                    //mGenerarReporteFinal = chk_3.Checked ? mGenerarReporteFinal = false : mGenerarReporteFinal = true;

                    //Valores asignados a variables asignables para el UPDATE.
                    int v_1 = 0;
                    int v_2 = 0;
                    int v_3 = 0;

                    if (chk_1.Checked == true){
                        v_1 = 1; 
                    } 
                    else { 
                        v_1 = 0; 
                    }

                    if (chk_2.Checked == true){ 
                        v_2 = 1; 
                    } 
                    else {
                        v_2 = 0; 
                    }

                    if (chk_3.Checked == true){ 
                        v_3 = 1; 
                    } else {
                        v_3 = 0; 
                    }

                    #endregion

                    #region Verificar si el Email es único COMENTADO.

                    //var email_check = (from s in consultas.Db.Contactos
                    //                   where s.Email == txt_Email.Text.Trim() && s.Id_Contacto == CodContactoSeleccionado
                    //                   select new
                    //                   {
                    //                       s.Email
                    //                   }).ToList();

                    //if (email_check.Count() == 0)
                    //{
                    //    //Aquí se ubicaría según el FONADE clásico el código para editar el usuario.
                    //}
                    //else
                    //{
                    //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El Email Ingresado ya existe Por favor Verificar')", true);
                    //    return;
                    //}

                    #endregion

                    #region Actualización!!

                    //Establecer el valor "Inactivo" basado en FONADE Clásico, se carga según la selección del mismo formulario.
                    if (dd_EstaInactivo.SelectedValue == "1") { ValorInactivo = "1"; } else { ValorInactivo = "0"; }

                    //Actualizar la tabla LogIngreso para que el usuario pueda iniciar sesion
                    if (ValorInactivo == "0")
                    {
                        sqlConsulta = " update LogIngreso " +
                                      " set FechaUltimoIngreso = GETDATE() " +
                                      " where CodContacto = " + CodContactoSeleccionado;

                        ejecutaReader(sqlConsulta, 2);
                    }

                    #region Actualización 1.

                    sqlConsulta = " UPDATE Contacto SET Nombres = '" + txt_Nombres.Text.Trim() + "', " +
                                  " Apellidos = '" + txt_Apellidos.Text.Trim() + "', " +
                                  " Email = '" + txt_Email.Text.Trim() + "', " +
                                  " Identificacion = " + txt_Identificacion.Text.Trim() + ", " +
                                  " Direccion = '" + txt_Direccion.Text.Trim() + "', " +
                                  " Telefono = '" + txt_Telefono.Text.Trim() + "', " +
                                  //Se corrige el update de activo
                                  //" Inactivo = " + v_1 + ", " + //" Inactivo = " + ValorInactivo + ", " +
                                  " Inactivo = " + ValorInactivo + ", " +
                                  //Se corrige la insercion de flagAcreditador
                                  //" flagAcreditador = " + mEsAcreditadorEnProyectoContacto + ", " +
                                  " flagAcreditador = " + v_1 + ", " +
                                  " flagActaParcial = " + v_2 + ", " + //" flagActaParcial = " + mGeneraActasParciales + ", " +
                                  " flagGeneraReporte = " + v_3 + //" flagGeneraReporte = " + mGenerarReporteFinal +
                                  " WHERE Id_Contacto = " + CodContactoSeleccionado;

                    //Asignar SqlCommand para su ejecución.
                    //cmd = new SqlCommand(sqlConsulta, conn);
                    ejecutaReader(sqlConsulta, 2);

                    //Ejecutar SQL.
                    //correcto = EjecutarSQL(conn, cmd);

                    //if (correcto == false)
                    //{
                    //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error en actualización (1).')", true);
                    //    return;
                    //}

                    #endregion

                    ////Establecer valores de booleanos.
                    //if (dd_EstaBloqueado.Items[0].Selected) { bloqueado_modificado = true; } else { bloqueado_modificado = false; }
                    //if (dd_EstaInactivo.Items[0].Selected) { inactivo_modificado = true; } else { inactivo_modificado = false; }

                    #region Actualización 2.

                    sqlConsulta = " UPDATE Clave SET Estado = " + dd_EstaBloqueado.SelectedValue + " " +
                                  " WHERE codContacto = " + CodContactoSeleccionado;

                    ejecutaReader(sqlConsulta, 2);

                    //Asignar SqlCommand para su ejecución.
                    cmd = new SqlCommand(sqlConsulta, conn);

                    //Ejecutar SQL.
                    //correcto = EjecutarSQL(conn, cmd);

                    //if (correcto == false)
                    //{
                    //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error en actualización (2).')", true);
                    //    return;
                    //}

                    #endregion

                    //Consultar si el valor "Bloqueado" que "proviene" por sesión es DIFERENTE del valor cargado
                    //de la consulta encontrada en el método "CargarInformacionContactoSeleccionado" para así
                    //ejecutar la siguiente inserción:
                    #region Inserción en Bitácora con CodEventoBitacora = 6 "Bloqueado".

                    if (ValorBloqueado != dd_EstaBloqueado.SelectedValue)
                    {
                        //Obtener Dirección IP.
                        ipAddress_result = LocalIPAddress();

                        sqlConsulta = "INSERT INTO Bitacora (FechaBitacora, CodEventoBitacora, Accion, CodContacto, IP) " +
                                     " VALUES(GETDATE(), 6, 'Usuario " + NombreUsuarioSeleccionado + " desbloqueo = " + ValorInactivo + " a usuario CodContacto = " + CodContactoSeleccionado + "', " + CodContactoSeleccionado + ", '" + ipAddress_result + "')";

                        //Asignar SqlCommand para su ejecución.
                        ejecutaReader(sqlConsulta, 2);
                        //cmd = new SqlCommand(sqlConsulta, conn);

                        ////Ejecutar SQL.
                        //correcto = EjecutarSQL(conn, cmd);

                        //if (correcto == false)
                        //{
                        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error en inserción de bitácora (1).')", true);
                        //    return;
                        //}
                    }

                    #endregion

                    //Consultar si el valor "Inactivo" que "proviene" por sesión es DIFERENTE del valor cargado
                    //de la consulta encontrada en el método "CargarInformacionContactoSeleccionado" para así
                    //ejecutar la siguiente inserción:
                    #region Inserción en Bitácora con CodEventoBitacora = 30 "Inactivo".

                    if (ValorInactivo != dd_EstaInactivo.SelectedValue)
                    {
                        //Obtener Dirección IP.
                        ipAddress_result = LocalIPAddress();

                        sqlConsulta = "INSERT INTO Bitacora (FechaBitacora, CodEventoBitacora, Accion, CodContacto, IP) " +
                                     " VALUES(GETDATE(), 30, 'Usuario " + NombreUsuarioSeleccionado + " inactivo = " + ValorInactivo + " a usuario CodContacto = " + CodContactoSeleccionado + "', " + CodContactoSeleccionado + ", '" + ipAddress_result + "')";

                        //Asignar SqlCommand para su ejecución.
                        cmd = new SqlCommand(sqlConsulta, conn);

                        //Ejecutar SQL.
                        correcto = EjecutarSQL(conn, cmd);

                        if (correcto == false)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error en inserción de bitácora (2).')", true);
                            return;
                        }
                    }

                    #endregion

                    #endregion
                }
                Response.Redirect("FiltrosUsuario2.aspx");
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo modificar el usuario seleccionado.')", true);
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
                //p_connection.Close();
                return true;
            }
            catch (Exception) { return false; }
            finally { p_connection.Close(); p_connection.Dispose(); }
        }

        #endregion
    }
}