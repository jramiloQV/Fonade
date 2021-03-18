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
    /// FiltroAsesorInactivo
    /// </summary>    
    public partial class FiltroAsesorInactivo : Base_Page
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

        #region Métodos generales.

        #endregion

        /// <summary>
        /// Mauricio Arias Olave.
        /// 28/04/2014.
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
                #region Primer paso de la construcción de la consulta.

                sqlConsulta = " SELECT DISTINCT Id_Contacto, Nombres, Apellidos, Email, Identificacion, NomInstitucion " +
                              " FROM Contacto C, ProyectoContacto PC, Institucion I " +
                              " WHERE CodInstitucion = Id_Institucion AND PC.inactivo = 1 AND C.inactivo = 1 " +
                              " AND CodContacto = Id_Contacto " +
                              " AND codrol IN (1,2) ";

                #endregion

                #region Evaluación del nombre del usuario digitado.

                if (txt_Nombres.Text.Trim() != "")
                {
                    sqlWhere = " AND Nombres LIKE '%'+'" + txt_Nombres.Text.Trim() + "'+'%' ";
                }

                #endregion

                #region Evaluación de los apellidos.

                if (txt_Apellidos.Text.Trim() != "")
                {
                    if (sqlWhere == "")
                    {
                        sqlWhere = " AND Apellidos LIKE '%'+'" + txt_Apellidos.Text.Trim() + "'+'%' ";
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
                        sqlWhere = " AND Email LIKE '%'+'" + txt_Email.Text.Trim() + "'+'%' ";
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
                        sqlWhere = " AND Identificacion = " + txt_Identificacion.Text.Trim() + " ";
                    }
                    else
                    {
                        sqlWhere = sqlWhere + " AND Identificacion = " + txt_Identificacion.Text.Trim() + " ";
                    }
                }

                #endregion

                if (sqlWhere.Trim() == "")
                {
                    //sqlWhere = "WHERE Nombres LIKE '%'+'Ninguno... .+''%'"; //Creo debe ser modificarlo y dejarse así: "LIKE '%'+' '+'%'"
                }

                //Consulta
                sqlConsultaFinal = sqlConsulta + " " + sqlWhere;

                var dtEmpresas = consultas.ObtenerDataTable(sqlConsultaFinal, "text");

                //La consulta estuvo bien armada y continúa el flujo; es decir, generar la grilla.
                //Actualización: No se pone la condicional IF porque si no hay datos, NO se debe mostrar la grilla
                //ni resultados "según el comportamiento del FONADE clásico".
                HttpContext.Current.Session["dtEmpresas"] = dtEmpresas;
                gv_ResultadosBusqueda.DataSource = dtEmpresas;
                gv_ResultadosBusqueda.DataBind();

                //Según el comportamiento del FONADE clásico, los campos son vaciados al terminar de consultar.
                txt_Nombres.Text = "";
                txt_Apellidos.Text = "";
                txt_Email.Text = "";
                txt_Identificacion.Text = "";
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error en la consulta y/o procedimiento.')", true);
                return;
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 28/04/2014.
        /// Seleccionar valores de búsqueda, enviarlos por sesión y redirigir al usuario
        /// a la página "modificarusuario.aspx".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_ResultadosBusqueda_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "mostrar")
            {
                #region Enviar el código del usuario seleccionado para agregar el valor a una variable de sesión.

                try
                {
                    HttpContext.Current.Session["CodContactoSeleccionado"] = e.CommandArgument.ToString();
                    Response.Redirect("ActivarAsesor.aspx");
                }
                catch { throw; }

                #endregion
            }
        }
    }
}