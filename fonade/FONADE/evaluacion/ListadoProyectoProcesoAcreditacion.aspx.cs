#region Diego Quiñonez

// <Author>Diego Quiñonez</Author>
// <Fecha>17 - 03 - 2014</Fecha>
// <Archivo>ListadoProyectoProcesoAcreditacion.aspx.cs</Archivo>

#endregion

#region

using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using Fonade.Negocio;
using TSHAK.Components;
using Fonade.Account;

#endregion

namespace Fonade.FONADE.evaluacion
{
    public partial class ListadoProyectoProcesoAcreditacion : Base_Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ValidacionCuenta validacionCuenta = new ValidacionCuenta();

                //Recuperar la url
                string pathRuta = HttpContext.Current.Request.Url.AbsolutePath;

                if (!validacionCuenta.validarPermiso(usuario.IdContacto, pathRuta))
                {
                    Response.Redirect(validacionCuenta.rutaHome(), true);
                }
                else
                {
                    CargarCombos();
                }
            }
        }

        private void CargarCombos()
        {
            try
            {

                consultas.Parameters = new[]
                                               {
                                                   new SqlParameter
                                                       {
                                                           ParameterName = "@CodOperador",
                                                           Value = usuario.CodOperador

                                                       }
                                               };

                DataSet dtCombos = consultas.ObtenerDataSet("MD_CargarConvocatorias_Estados");

                if (dtCombos.Tables.Count != 0)
                {
                    if (dtCombos.Tables[0].Rows.Count != 0)
                    {
                        ddlConvocatoria.DataSource = dtCombos.Tables[0];
                        ddlConvocatoria.DataTextField = "NomConvocatoria";
                        ddlConvocatoria.DataValueField = "Id_Convocatoria";
                        ddlConvocatoria.DataBind();
                        ddlConvocatoria.Items.Insert(0, new ListItem("Seleccione", "0"));
                    }

                    if (dtCombos.Tables[1].Rows.Count != 0)
                    {
                        ddlestados.DataSource = dtCombos.Tables[1];
                        ddlestados.DataTextField = "NomEstado";
                        ddlestados.DataValueField = "Id_Estado";
                        ddlestados.DataBind();
                        ddlestados.Items.Insert(0, new ListItem("Seleccione", "0"));
                    }
                }
                consultas.Parameters = null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            CargarGridconvocatorias();
        }

        private void CargarGridconvocatorias()
        {
            try
            {
                consultas.Parameters = new[]
                                           {
                                               new SqlParameter
                                                   {
                                                       ParameterName = "@CodConvocatoria"
                                                       ,
                                                       Value =
                                                           Convert.ToInt32(
                                                               !string.IsNullOrEmpty(ddlConvocatoria.SelectedValue)
                                                                   ? Convert.ToInt32(ddlConvocatoria.SelectedValue)
                                                                   : 0)
                                                   },
                                               new SqlParameter
                                                   {
                                                       ParameterName = "@CodProyecto"
                                                       ,
                                                       Value =
                                                           Convert.ToInt32(!string.IsNullOrEmpty(txtcodigoproyecto.Text)
                                                                               ? Convert.ToInt32(txtcodigoproyecto.Text)
                                                                               : 0)
                                                   },
                                               new SqlParameter
                                                   {
                                                       ParameterName = "@NomProyecto"
                                                       ,
                                                       Value = txtnombreproyecto.Text
                                                   }
                                               ,
                                               new SqlParameter
                                                   {
                                                       ParameterName = "@Estado"
                                                       ,
                                                       Value =
                                                           Convert.ToInt32(
                                                               !string.IsNullOrEmpty(ddlestados.SelectedValue)
                                                                   ? Convert.ToInt32(ddlestados.SelectedValue)
                                                                   : 0)
                                               }, new SqlParameter
                                                      {
                                                          ParameterName = "@NombreEmprendedor"
                                                          ,
                                                          Value = txtnombreemprendedor.Text
                                                      }
                                               , new SqlParameter
                                                     {
                                                         ParameterName = "@ApellidosEmprendedor"
                                                         ,
                                                         Value = txtapellidosemprendedor.Text
                                               }, new SqlParameter
                                                      {
                                                          ParameterName = "@DocumentoEmprendedor"
                                                          ,
                                                          Value =
                                                              Convert.ToInt32(
                                                                  !string.IsNullOrEmpty(txtdocemprendedor.Text)
                                                                      ? Convert.ToInt32(txtdocemprendedor.Text)
                                                                      : 0)
                                                      }
                                               , new SqlParameter
                                                      {
                                                          ParameterName = "@CodOperador",
                                                          Value = usuario.CodOperador
                                                      }
                                           };
                var lstConvocatorias = consultas.ObtenerDataTable("MD_ObtenerListadoProyectoProcesoAcreditacion");

                if (lstConvocatorias.Rows.Count != 0)
                {
                    HttpContext.Current.Session["dtconvocatoria"] = lstConvocatorias;
                    GrvConvocatorias.DataSource = lstConvocatorias;
                    GrvConvocatorias.DataBind();
                }
            }
            catch (Exception EX)
            {
                throw new Exception(EX.Message);
            }
        }

        protected void GrvConvocatoriasPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrvConvocatorias.PageIndex = e.NewPageIndex;
            GrvConvocatorias.DataSource = HttpContext.Current.Session["dtconvocatoria"];
            GrvConvocatorias.DataBind();
        }

        protected void GrvConvocatoriaRowcommad(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "proyecto")
            {
                string variables = e.CommandArgument.ToString();
                var querystringSeguro = CreateQueryStringUrl();
                string[] parametros = variables.Split(';');
                querystringSeguro["CodProyecto"] = parametros[0];
                querystringSeguro["CodConvocatoria"] = parametros[1];
                Response.Redirect("ProyectoAcreditacion.aspx?info=" + HttpUtility.UrlEncode(querystringSeguro.ToString()));

            }
        }
    }
}