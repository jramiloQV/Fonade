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
using System.Globalization;

namespace Fonade.FONADE.interventoria
{
    public partial class VerProyectosInterventor : Negocio.Base_Page
    {
        /// <summary>
        /// Còdigo del interventor seleccionado.
        /// Obtenido por sesión.
        /// </summary>
        private Int32 CodInterventor_Seleccionado;

        #region Métodos generales.

        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    //Obtener la variable de sesión.
                    CodInterventor_Seleccionado = HttpContext.Current.Session["ContactoInterventor"] != null ? CodInterventor_Seleccionado = Convert.ToInt32(HttpContext.Current.Session["ContactoInterventor"].ToString()) : 0;
                    //Llamar al método que cargará la información en la grilla.
                    CargarEmpresas(CodInterventor_Seleccionado);
                }
                catch { Response.Redirect("~/Account/Login.aspx"); }
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 07/05/2014.
        /// Cargar las empresas del interventor seleccionado previamente en "CatalogoInterventor.aspx".
        /// </summary>
        /// <param name="codigoInterventorSeleccionado">Código del Interventor seleccionado.</param>
        private void CargarEmpresas(Int32 codigoInterventorSeleccionado)
        {
            //Inicializar variables.
            String sqlConsulta = "";

            try
            {
                //Generar consulta.
                sqlConsulta = " SELECT Empresa.razonsocial, Rol.Nombre AS Rol, EmpresaInterventor.FechaInicio " +
                              " FROM EmpresaInterventor " +
                              " INNER JOIN Rol ON EmpresaInterventor.Rol = dbo.Rol.Id_Rol " +
                              " INNER JOIN Empresa ON EmpresaInterventor.CodEmpresa = dbo.Empresa.id_empresa " +
                              " WHERE (dbo.EmpresaInterventor.Inactivo = 0) " +
                              " AND (EmpresaInterventor.Rol IN(6,8)) " +
                              " AND (dbo.EmpresaInterventor.CodContacto = " + codigoInterventorSeleccionado + ")";

                //Asignar resultados de la consulta a la variable DataTable.
                var tabla_sql = consultas.ObtenerDataTable(sqlConsulta, "text");

                gv_Empresas.DataSource = tabla_sql;
                gv_Empresas.DataBind();
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

        #endregion

        /// <summary>
        /// Cerrar ventana.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnk_btn_Cerrar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "window.close();", true);
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 07/05/2014.
        /// Establecer valor formateado de la fecha según FONADE clásico.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_Empresas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Inicializar variables internas.
            var lbl = e.Row.FindControl("lbl_fecha_formateada") as Label;

            //Si la variable NO es NULL, es decir, fue instanciada correctamente.
            if (lbl != null)
            {
                try
                {
                    DateTime fecha = new DateTime();
                    fecha = Convert.ToDateTime(lbl.Text);
                    string sMes = fecha.ToString("MMM", CultureInfo.CreateSpecificCulture("es-CO"));
                    lbl.Text = UppercaseFirst(sMes) + " " + fecha.Day + " de " + fecha.Year;
                }
                catch (Exception ex) { string err = ex.Message; }
            }
        }
    }
}