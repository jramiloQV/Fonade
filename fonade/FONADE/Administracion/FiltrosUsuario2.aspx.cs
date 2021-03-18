using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Datos;
using Fonade.Negocio;
using System.Configuration;
using System.Data;

namespace Fonade.FONADE.Administracion
{
    /// <summary>
    /// FiltrosUsuario2
    /// </summary>    
    public partial class FiltrosUsuario2 :  Base_Page
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = "FONDO EMPRENDER - ";
        }

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
        /// Handles the Click event of the btn_Buscar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
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
                    sqlConsulta = " SELECT DISTINCT Id_Contacto, Nombres, Apellidos, Email, Identificacion, NomGrupo, " +
                        /*Se debe colocar las columnas para evitar que se "dañe el flujo" del código.*/
                                  " '' AS Id_Proyecto, '' AS Id_Grupo, '' AS NomGrupo, '' AS NomProyecto " +
                                  " FROM Contacto c LEFT JOIN GrupoContacto gc  " +
                                  " ON c.Id_Contacto = gc.CodContacto LEFT JOIN Grupo g on gc.CodGrupo = g.Id_Grupo";
                }
                else
                {
                    sqlConsulta = " SELECT DISTINCT Id_Contacto, Nombres, Apellidos, Email, Identificacion, NomProyecto, " +
                                  " Id_Proyecto, Id_Grupo, NomGrupo " +
                                  " FROM contacto c LEFT JOIN proyectocontacto pc " +
                                  " ON c.Id_Contacto = pc.CodContacto LEFT JOIN proyecto p " +
                                  " ON pc.CodProyecto = p.id_proyecto LEFT JOIN GrupoContacto gc " +
                                  " ON c.Id_Contacto = gc.CodContacto LEFT JOIN Grupo g " +
                                  " ON gc.CodGrupo = g.id_Grupo";
                }

                #endregion

                

                if (sqlWhere.Trim() == "")
                {
                    
                }

                //Consulta
                sqlConsultaFinal = sqlConsulta; // +" " + sqlWhere;

                
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
                    sqlConsultaFinal = sqlConsultaFinal.IndexOf("+") > -1 ? sqlConsultaFinal.Remove(sqlConsultaFinal.IndexOf("+"), 1) : sqlConsultaFinal;
                    var ijn = sqlConsultaFinal.Split('+');
                    sqlConsultaFinal = ijn.Length >= 2 ? sqlConsultaFinal.Replace("+", " AND ") : sqlConsultaFinal.Replace("+", string.Empty);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Vldt", "alert('Especifique los parámetros de consulta.')", true);
                    return;
                }

                SqlDataSource sqlds = new SqlDataSource(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString + ";Connect Timeout=120", sqlConsultaFinal)
                {
                    DataSourceMode = SqlDataSourceMode.DataReader,
                    ConflictDetection = ConflictOptions.OverwriteChanges,
                    CancelSelectOnNullParameter = true,
                    SelectCommandType = SqlDataSourceCommandType.Text,
                    ID = "sqlDs"
                };
                sqlds.DataBind();


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
        /// Handles the SelectedIndexChanged event of the GridView1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((GridView)sender).SelectedDataKey == null) return;
            HttpContext.Current.Session["CodContactoSeleccionado"] = ((GridView)sender).SelectedDataKey.Value;
            Response.Redirect("modificarusuario.aspx");
        }

    }
}