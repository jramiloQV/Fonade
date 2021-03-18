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
    public partial class InsumoProductoInterventor : Negocio.Base_Page
    {
        #region Variables globales "Vienen de FrameProduccionInter.aspx".

        String s_CodProducto;
        String s_NombreProducto;
        String s_CodProyecto;

        #endregion

        /// <summary>
        /// Variable que almacena sentencias SQL.
        /// </summary>
        String txtSQL;

        /// <summary>
        /// Tabla que almacena los resultados de la consulta.
        /// </summary>
        DataTable tabla;

        /// <summary>
        /// Tipo del insumo agregado, se compara luego con el valor almacenado para establecer el encabezado.
        /// </summary>
        String txtTipoInsumo;

        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Establecer valores de sesión en variables internas.
                s_CodProducto = HttpContext.Current.Session["s_CodProduccion"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["s_CodProduccion"].ToString()) ? HttpContext.Current.Session["s_CodProduccion"].ToString() : "0";
                s_NombreProducto = HttpContext.Current.Session["s_NombreProducto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["s_NombreProducto"].ToString()) ? HttpContext.Current.Session["s_NombreProducto"].ToString() : "0";
                s_CodProyecto = HttpContext.Current.Session["s_CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["s_CodProyecto"].ToString()) ? HttpContext.Current.Session["s_CodProyecto"].ToString() : "0";

                if (s_CodProducto == "0" || s_NombreProducto == "0" || s_CodProyecto == "0")
                { System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.reload();window.close();", true); }
                else
                {
                    #region Procesar información.
                    try
                    {
                        //txtTipoInsumo del producto.
                        lbl_enunciado.Text = s_NombreProducto;

                        //Consultar.
                        txtSQL = " SELECT ProyectoProducto.NomProducto, TipoInsumo.NomTipoInsumo, ProyectoInsumo.nomInsumo " +
                                 " FROM ProyectoProductoInsumo INNER JOIN ProyectoInsumo ON ProyectoProductoInsumo.CodInsumo = ProyectoInsumo.Id_Insumo " +
                                 " INNER JOIN ProyectoProducto ON ProyectoProductoInsumo.CodProducto = ProyectoProducto.Id_Producto  " +
                                 " INNER JOIN InterventorProduccion ON ProyectoProducto.NomProducto = InterventorProduccion.NomProducto  " +
                                 " INNER JOIN TipoInsumo ON ProyectoInsumo.codTipoInsumo = TipoInsumo.Id_TipoInsumo " +
                                 " WHERE (InterventorProduccion.CodProyecto = " + s_CodProyecto + ")  " +
                                 " AND (InterventorProduccion.NomProducto = '" + s_NombreProducto + "') ";

                        tabla = consultas.ObtenerDataTable(txtSQL, "text");

                        if (tabla.Rows.Count > 0)
                        {
                            //Generar tabla.
                            lbl_tabla.Text = "    <table width='250px' border='0' cellpadding='0' cellspacing='0' bordercolor='#4E77AF'>" +
                                              "        <tr>" +
                                              "          <td align='center' valign='top'>" +
                                              "            <table border='0' cellspacing='0' cellpadding='0'>";

                            //Asignar valor.
                            txtTipoInsumo = "";

                            //Recorrer la lista para obtener datos.
                            for (int i = 0; i < tabla.Rows.Count; i++)
                            {
                                if (txtTipoInsumo != tabla.Rows[i]["NomTipoInsumo"].ToString())
                                {
                                    //Asignar valor.
                                    txtTipoInsumo = tabla.Rows[i]["NomTipoInsumo"].ToString();

                                    //Continuar generando la tabla.
                                    lbl_tabla.Text = lbl_tabla.Text + "  <tr>" +
                                                                      "      <td><br/></td>" +
                                                                      "  </tr>" +
                                                                      "	<tr> " +
                                                                      "		<td style='text-align:left;'><strong style=\"color:#174680;\">" + txtTipoInsumo + "</strong></td>" +
                                                                      "	</tr>";
                                }

                                //Continuar generando la tabla.
                                lbl_tabla.Text = lbl_tabla.Text + "<tr align='left' valign='top'>" +
                                                                  "   <td align='left'>" + tabla.Rows[i]["NomInsumo"].ToString() + "</td>" +
                                                                  "</tr>";
                            }

                            //Finalizar la tabla.
                            lbl_tabla.Text = lbl_tabla.Text + "            </table>" +
                                                              "          </td>" +
                                                              "        </tr>" +
                                                              "      </table>";
                        }
                    }
                    catch { }
                    #endregion
                }
            }
        }
    }
}