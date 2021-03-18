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
using System.Web;

namespace Fonade.FONADE.Administracion
{
    /// <summary>
    /// FiltrosUsuario
    /// </summary>    
    public partial class FiltrosUsuario : Base_Page
    {
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
                    //MostrarInterventor(); //Mauricio Arias Olave "26/04/2014": Ya no se muestra los valores del nombre y fecha.
                }
            }
            catch (Exception)
            {
                Response.Redirect("~/Account/Login.aspx");
            }
        }
        /*MODIFICACION REV ADFON 12 18-02-015*/


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
                //SqlDataReader reader = cmd.ExecuteReader();
                var dt = consultas.ObtenerDataTable(sql, "text");

                if (dt.Rows.Count > 0)
                {
                    //lbl_Interventor.Text = reader["Nombre"].ToString();
                    //DateTime fecha = DateTime.Now;
                    //string sMes = fecha.ToString("MMM", CultureInfo.CreateSpecificCulture("es-CO"));
                    //lbl_tiempo.Text = UppercaseFirst(sMes) + " " + fecha.Day + " de " + fecha.Year;
                }
                //reader.Close();
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

        #region Métodos generales.

        #endregion

        /// <summary>
        /// Mauricio Arias Olave.
        /// 25/04/2014.
        /// Mostrar la grilla con los resultados de la búsqueda.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Buscar_Click(object sender, EventArgs e)
        {
            //Inicializar variables.
            String sqlConsulta = "";
            String sqlWhere = "";
            String sqlConsultaFinal = "";

            try
            {
                #region Construcción de la consulta dependiendo de el nombre y número del plan/proyecto.

                if (txt_NombrePlan.Text.Trim() == "" && txt_NumeroPlan.Text.Trim() == "")
                {
                    sqlConsulta = " SELECT DISTINCT Id_Contacto, Nombres, Apellidos, Email, Identificacion, o.NombreOperador" +
                        ", NomGrupo, " +
                        /*Se debe colocar las columnas para evitar que se "dañe el flujo" del código.*/
                                  " '' AS Id_Proyecto, '' AS Id_Grupo, '' AS NomGrupo, '' AS NomProyecto " +
                                  " FROM Contacto c LEFT JOIN GrupoContacto gc  " +
                                  " ON c.Id_Contacto = gc.CodContacto LEFT JOIN Grupo g on gc.CodGrupo = g.Id_Grupo" +
                                  "  left join Operador o on c.codOperador = o.IdOperador";
                }
                else
                {
                    sqlConsulta = " SELECT DISTINCT Id_Contacto, Nombres, Apellidos, Email, Identificacion, o.NombreOperador" +
                        ", NomProyecto, " +
                                  " Id_Proyecto, Id_Grupo, NomGrupo " +
                                  " FROM contacto c LEFT JOIN proyectocontacto pc " +
                                  " ON c.Id_Contacto = pc.CodContacto LEFT JOIN proyecto p " +
                                  " ON pc.CodProyecto = p.id_proyecto LEFT JOIN GrupoContacto gc " +
                                  " ON c.Id_Contacto = gc.CodContacto LEFT JOIN Grupo g " +
                                  " ON gc.CodGrupo = g.id_Grupo" +
                                  "  left join Operador o on p.codOperador = o.IdOperador";
                }

                #endregion

                /*
                                  
                #region Evaluación del nombre del usuario digitado.

                if (txt_Nombres.Text.Trim() != "")
                {
                    sqlWhere = " WHERE Nombres LIKE '%'+'" + txt_Nombres.Text.Trim() + "'+'%' ";
                }

                #endregion

                #region Evaluación de los apellidos.

                if (txt_Apellidos.Text.Trim() != "")
                {
                    if (sqlWhere == "")
                    {
                        sqlWhere = "WHERE Apellidos LIKE '%'+'" + txt_Apellidos.Text.Trim() + "'+'%' ";
                    }
                    else
                    {
                        sqlWhere = sqlWhere + " AND Apellidos LIKE '%'+'" + txt_Apellidos.Text.Trim() + "'+'%' ";
                    }
                }

                #endregion

                #region Evaluación del correo electrónico.

                if (txt_Email.Text.Trim() != "")
                {
                    if (sqlWhere == "")
                    {
                        sqlWhere = "WHERE Email LIKE '%'+'" + txt_Email.Text.Trim() + "'+'%' ";
                    }
                    else
                    {
                        sqlWhere = sqlWhere + " AND Email LIKE '%'+'" + txt_Email.Text.Trim() + "'+'%' ";
                    }
                }

                #endregion

                #region Evaluación del número de identificación.

                if (txt_Identificacion.Text.Trim() != "")
                {
                    if (sqlWhere == "")
                    {
                        sqlWhere = "WHERE Identificacion = " + txt_Identificacion.Text.Trim() + " ";
                    }
                    else
                    {
                        sqlWhere = sqlWhere + " AND Identificacion = " + txt_Identificacion.Text.Trim() + " ";
                    }
                }

                #endregion

                #region Evaluación del nombre del proyecto.

                if (txt_NombrePlan.Text.Trim() != "")
                {
                    if (sqlWhere == "")
                    {
                        sqlWhere = "WHERE NomProyecto LIKE '%'+'" + txt_NombrePlan.Text.Trim() + "'+'%' ";
                    }
                    else
                    {
                        sqlWhere = sqlWhere + " AND NomProyecto LIKE '%'+'" + txt_NombrePlan.Text.Trim() + "'+'%' ";
                    }
                }

                #endregion

                #region Evaluación del número del proyecto.

                if (txt_NumeroPlan.Text.Trim() != "")
                {
                    if (sqlWhere == "")
                    {
                        sqlWhere = "WHERE Id_Proyecto = " + txt_NumeroPlan.Text.Trim() + " ";
                    }
                    else
                    {
                        sqlWhere = sqlWhere + " AND Id_Proyecto = " + txt_NumeroPlan.Text.Trim() + " ";
                    }
                }

                #endregion
                 
                 */

                if (sqlWhere.Trim() == "")
                {
                    //sqlWhere = "WHERE Nombres LIKE '%'+'Ninguno... .+''%'"; //Creo debe ser modificarlo y dejarse así: "LIKE '%'+' '+'%'"
                }

                //Consulta
                sqlConsultaFinal = sqlConsulta; // +" " + sqlWhere;

                //var dtEmpresas = consultas.ObtenerDataTable(sqlConsultaFinal, "text");

                //La consulta estuvo bien armada y continúa el flujo; es decir, generar la grilla.
                //Actualización: No se pone la condicional IF porque si no hay datos, NO se debe mostrar la grilla
                //ni resultados "según el comportamiento del FONADE clásico".
                //Session["dtEmpresas"] = dtEmpresas;

                if (!String.IsNullOrEmpty(txt_Nombres.Text) || !String.IsNullOrEmpty(txt_Apellidos.Text) ||
                   !String.IsNullOrEmpty(txt_Email.Text) || !String.IsNullOrEmpty(txt_Identificacion.Text) ||
                   !String.IsNullOrEmpty(txt_NombrePlan.Text) || !String.IsNullOrEmpty(txt_NumeroPlan.Text))
                {
                    sqlConsultaFinal = sqlConsultaFinal.Insert(sqlConsultaFinal.Length, " where ");
                    sqlConsultaFinal = sqlConsultaFinal.Insert(sqlConsultaFinal.Length, !String.IsNullOrEmpty(txt_Nombres.Text) ?
                        string.Format(" + Nombres LIKE'%{0}%' ", txt_Nombres.Text) : string.Empty);
                    sqlConsultaFinal = sqlConsultaFinal.Insert(sqlConsultaFinal.Length, !String.IsNullOrEmpty(txt_Apellidos.Text) ?
                        string.Format(" + Apellidos LIKE'%{0}%' ", txt_Apellidos.Text) : string.Empty);
                    sqlConsultaFinal = sqlConsultaFinal.Insert(sqlConsultaFinal.Length, !String.IsNullOrEmpty(txt_Email.Text) ?
                        string.Format(" + Email LIKE'%{0}%' ", txt_Email.Text) : string.Empty);
                    sqlConsultaFinal = sqlConsultaFinal.Insert(sqlConsultaFinal.Length, !String.IsNullOrEmpty(txt_Identificacion.Text) ?
                        string.Format(" + Identificacion IN ({0})", txt_Identificacion.Text) : string.Empty);
                    if (!String.IsNullOrEmpty(txt_NombrePlan.Text) || !String.IsNullOrEmpty(txt_NumeroPlan.Text))
                    {
                        sqlConsultaFinal = sqlConsultaFinal.Insert(sqlConsultaFinal.Length, !String.IsNullOrEmpty(txt_NombrePlan.Text) ?
                        string.Format(" + NomProyecto LIKE'%{0}%'", txt_NombrePlan.Text) : string.Empty);
                        sqlConsultaFinal = sqlConsultaFinal.Insert(sqlConsultaFinal.Length, !String.IsNullOrEmpty(txt_NumeroPlan.Text) ?
                        string.Format(" + Id_Proyecto IN({0})", txt_NumeroPlan.Text) : string.Empty);
                    }
                    sqlConsultaFinal = sqlConsultaFinal.IndexOf("+")>-1?sqlConsultaFinal.Remove(sqlConsultaFinal.IndexOf("+"),1):sqlConsultaFinal;
                    var ijn = sqlConsultaFinal.Split('+');
                    sqlConsultaFinal = ijn.Length >= 2 ? sqlConsultaFinal.Replace("+", " AND ") : sqlConsultaFinal.Replace("+", string.Empty);
                }
                else {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Vldt", "alert('Especifique los parámetros de consulta.')", true);
                    return;
                }

                SqlDataSource sqlds = new SqlDataSource(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString+";Connect Timeout=120", sqlConsultaFinal) { DataSourceMode = SqlDataSourceMode.DataReader, ConflictDetection = ConflictOptions.OverwriteChanges, CancelSelectOnNullParameter = true,
                SelectCommandType = SqlDataSourceCommandType.Text, ID="sqlDs" };
                sqlds.DataBind();
                
                //gv_ResultadosBusqueda.DataSource = dtEmpresas;
                //gv_ResultadosBusqueda.DataBind();

                //gv_ResultadosBusqueda.DataSource = sqlds;
                //gv_ResultadosBusqueda.DataBind();
                
                var dtEmpresas = new DataTable();

                GridView1.DataSource = sqlds.Select(new DataSourceSelectArguments());
                sqlds.DataBind();
                GridView1.DataBind();

                //Según el comportamiento del FONADE clásico, los campos son vaciados al terminar de consultar.
                txt_Nombres.Text = "";
                txt_Apellidos.Text = "";
                txt_Email.Text = "";
                txt_Identificacion.Text = "";
                txt_NombrePlan.Text = "";
                txt_NumeroPlan.Text = "";
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error en la consulta y/o procedimiento.')", true);
                return;
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 26/04/2014.
        /// Seleccionar valores de búsqueda, enviarlos por sesión y redirigir al usuario
        /// a la página "modificarusuario.aspx".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_ResultadosBusqueda_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            /*
            if (e.CommandName == "mostrar")
            {
                #region Enviar el código del usuario seleccionado para agregar el valor a una variable de sesión.

                try
                {
                    HttpContext.Current.Session["CodContactoSeleccionado"] = e.CommandArgument.ToString();
                    Response.Redirect("modificarusuario.aspx");
                }
                catch { return; }

                #endregion

            }             
             */

        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the GridView1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e){
            if (((GridView)sender).SelectedDataKey==null) return;
            HttpContext.Current.Session["CodContactoSeleccionado"] = ((GridView)sender).SelectedDataKey.Value;
            Response.Redirect("modificarusuario.aspx");
        }
    }
}