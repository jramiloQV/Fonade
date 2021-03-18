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

namespace Fonade.FONADE.interventoria
{
    public partial class CatalogoCumpliInformeInter : Negocio.Base_Page
    {
        #region Variables globales (Vienen de "AgregarInformeFinalInterventoria").

        String s_hdf_CodInforme;
        String s_hdf_CodItem;
        String s_hdf_CodEmpresa;

        #endregion

        /// <summary>
        /// Usada para armar consultas SQL.
        /// </summary>
        String txtSQL;

        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                s_hdf_CodInforme = HttpContext.Current.Session["s_hdf_CodInforme"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["s_hdf_CodInforme"].ToString()) ? HttpContext.Current.Session["s_hdf_CodInforme"].ToString() : "0";
                s_hdf_CodItem = HttpContext.Current.Session["s_hdf_CodItem"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["s_hdf_CodItem"].ToString()) ? HttpContext.Current.Session["s_hdf_CodItem"].ToString() : "0";
                s_hdf_CodEmpresa = HttpContext.Current.Session["s_hdf_CodEmpresa"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["s_hdf_CodEmpresa"].ToString()) ? HttpContext.Current.Session["s_hdf_CodEmpresa"].ToString() : "0";

                //Asignar datos.
                hdf_CodInforme.Value = s_hdf_CodInforme;
                hdf_CodItem.Value = s_hdf_CodItem;
                hdf_CodEmpresa.Value = s_hdf_CodEmpresa;

                if (s_hdf_CodInforme == "0" || s_hdf_CodItem == "0" || s_hdf_CodEmpresa == "0")
                { System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.reload();window.close();", true); }
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 21/05/2014.
        /// Cerrar ventana modal.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Cerrar_Click(object sender, EventArgs e)
        {
            s_hdf_CodInforme = null;
            s_hdf_CodItem = null;
            s_hdf_CodEmpresa = null;
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.reload();window.close();", true);
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 21/05/2014.
        /// Establecer acción.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Accion_Click(object sender, EventArgs e)
        {
            //Inicializar variables.
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            bool procesado = false;

            try
            {
                //Consultar.
                txtSQL = " SELECT * FROM InterventorInformeFinalItem " +
                         " WHERE NomInterventorInformeFinalItem = '" + NomItem.Text.Trim() + "'" +
                         " AND CodInformeFinalCriterio = " + hdf_CodItem.Value +
                         " AND CodEmpresa IN(0," + hdf_CodEmpresa.Value + ") ";

                //Asignar valores a variable temporal.
                var dt = consultas.ObtenerDataTable(txtSQL, "text");

                if (dt.Rows.Count == 0)
                {
                    #region Crear.

                    txtSQL = " INSERT INTO InterventorInformeFinalItem (NomInterventorInformeFinalItem, CodInformeFinalCriterio, CodEmpresa) " +
                             " VALUES('" + NomItem.Text.Trim() + "'," + hdf_CodItem.Value + "," + hdf_CodEmpresa.Value + ") ";

                    cmd = new SqlCommand(txtSQL, connection);
                    procesado = EjecutarSQL(connection, cmd);

                    if (procesado == false)
                    {
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo crear.')", true);
                        return;
                    }
                    else
                    {
                        dt = null;
                        txtSQL = null;
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.reload();window.close();", true);
                    }
                    #endregion
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Ya existe un item con ese nombre.')", true);
                    return;
                }
            }
            catch
            {
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo generar el nuevo cumplimiento.')", true);
                return;
            }
        }
    }
}