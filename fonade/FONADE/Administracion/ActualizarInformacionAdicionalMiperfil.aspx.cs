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

namespace Fonade.FONADE.Administracion
{
    /// <summary>
    /// ActualizarInformacionAdicionalMiperfil
    /// </summary>    
    public partial class ActualizarInformacionAdicionalMiperfil : Base_Page
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
            try
            {
                //Establecer el título de la página actual.
                this.Page.Title = "FONDO EMPRENDER - ACTUALIZAR INFORMACIÓN ADICIONAL";

                if (!IsPostBack)
                {
                    //MostrarInterventor(); //Mauricio Arias Olave "26/04/2014": Ya no se muestra los valores del nombre y fecha.
                    //SANTIAGO SANCHEZ 13/12/14
                    //marca el check segun el estado actual
                    chk_actualizarInfo.Checked = EstablecerCheck();
                }
            }
            catch (Exception)
            {
                Response.Redirect("~/Account/Login.aspx");
            }
        }
        //SANTIAGO SANCHEZ 13/12/14 
        // funcion que mira el estado actual del check y asi mismo lo marca por default
        private bool EstablecerCheck()
        {
            //Inicializar variables.
            String sqlConsulta = "";

            try
            {
                //Consulta.
                sqlConsulta = "SELECT valor FROM parametro WHERE nomparametro = 'IngresarInformacionAdicionalPerfil' ORDER BY id_parametro DESC";

                //Asignar resultado de la consulta a variable de tipo DataTable.
                var dtEmpresas = consultas.ObtenerDataTable(sqlConsulta, "text");

                //Si hay datos, establecer chequeo dependiendo del resultado de la consulta.
                if (dtEmpresas.Rows.Count > 0)
                {
                    if (dtEmpresas.Rows[0]["Valor"].ToString() == "1")
                        return true;
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
        /// Actualizar información.
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

            try
            {
                //Establecer el valor de la variable "valor_chequeado" con relación al estado "Checked" del 
                //CheckBox "chk_actualizarInfo".
                if (chk_actualizarInfo.Checked == true) { valor_chequeado = "1"; } else { valor_chequeado = "0"; }

                //Consulta.
                sqlConsulta = "SELECT valor FROM parametro WHERE nomparametro = 'IngresarInformacionAdicionalPerfil'";

                //Asignar resultado de la consulta a variable de tipo DataTable.
                var dtEmpresas = consultas.ObtenerDataTable(sqlConsulta, "text");

                if (dtEmpresas.Rows.Count > 0)
                {
                    #region Si está vacío, inserte.

                    sqlConsulta = " INSERT INTO parametro (nomparametro, valor) " +
                                              " VALUES('IngresarInformacionAdicionalPerfil','" + valor_chequeado + "')";

                    //Asignar SqlCommand para su ejecución.
                    cmd = new SqlCommand(sqlConsulta, conn);

                    //Ejecutar SQL.
                    correcto = EjecutarSQL(conn, cmd);

                    if (correcto == false)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error en inserción (1).')", true);
                        return;
                    }

                    #endregion
                }
                else
                {
                    #region Si tiene datos, actualize.

                    sqlConsulta = " UPDATE parametro SET valor = '" + valor_chequeado + "' " +
                                              " WHERE nomparametro = 'IngresarInformacionAdicionalPerfil' ";

                    //Asignar SqlCommand para su ejecución.
                    cmd = new SqlCommand(sqlConsulta, conn);

                    //Ejecutar SQL.
                    correcto = EjecutarSQL(conn, cmd);

                    if (correcto == false)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error en actualización (2).')", true);
                        return;
                    }

                    #endregion
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error en consulta principal.')", true);
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
                p_connection.Close();
                return true;
            }
            catch (Exception) { return false; }
            finally { p_connection.Close(); p_connection.Dispose(); }
        }

        #endregion
    }
}